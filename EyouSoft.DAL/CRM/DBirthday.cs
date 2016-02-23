using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.Common;
using EyouSoft.Toolkit.DAL;
using EyouSoft.Toolkit;
using Microsoft.Practices.EnterpriseLibrary.Data;
using EyouSoft.Model.CRM;
using EyouSoft.Model.EnumType.CustomerStructure;

namespace EyouSoft.DAL.CRM
{
    /// <summary>
    /// 生日提醒数据访问
    /// </summary>
    public class DBirthday : DALBase, IDAL.CRM.IBirthday
    {
        private readonly Database _db;

        public DBirthday()
        {
            _db = this.SystemStore;
        }

        /// <summary>
        /// 添加礼物明细
        /// </summary>
        /// <param name="type">收到礼物对象类型</param>
        /// <param name="id">收到礼物对象编号</param>
        /// <param name="model">礼物基类</param>
        /// <returns>
        /// 1：成功；
        /// 0：参数错误；
        /// -1：新增失败；
        /// </returns>
        public int AddBirthdayGift(BirthdayGiftType type, string id, MBirthdayGiftBase model)
        {
            if (string.IsNullOrEmpty(id) || model == null) return 0;

            model.Id = Guid.NewGuid().ToString();
            model.IssueTime = DateTime.Now;
            var strSql = new StringBuilder();
            strSql.Append(" INSERT INTO [tbl_BirthdayGift] ([Id],[CompanyId],[ItemId],[ItemType],[SendGiftTime],[Remark],[OperatorId],[IssueTime]) VALUES (@Id,@CompanyId,@ItemId,@ItemType,@SendGiftTime,@Remark,@OperatorId,@IssueTime); ");

            DbCommand dc = _db.GetSqlStringCommand(strSql.ToString());
            _db.AddInParameter(dc, "Id", DbType.AnsiStringFixedLength, model.Id);
            _db.AddInParameter(dc, "CompanyId", DbType.Int32, model.CompanyId);
            _db.AddInParameter(dc, "ItemId", DbType.AnsiStringFixedLength, id);
            _db.AddInParameter(dc, "ItemType", DbType.Byte, (int)type);
            _db.AddInParameter(dc, "SendGiftTime", DbType.DateTime, model.SendGiftTime);
            _db.AddInParameter(dc, "Remark", DbType.String, model.Remark);
            _db.AddInParameter(dc, "OperatorId", DbType.Int32, model.OperatorId);
            _db.AddInParameter(dc, "IssueTime", DbType.DateTime, model.IssueTime);

            return DbHelper.ExecuteSql(dc, _db) > 0 ? 1 : -1;
        }

        /// <summary>
        /// 修改礼物明细
        /// </summary>
        /// <param name="model">礼物基类</param>
        /// <returns>
        /// 1：成功；
        /// 0：参数错误；
        /// -1：新增失败；
        /// </returns>
        public int UpdateBirthdayGift(MBirthdayGiftBase model)
        {
            if (model == null || string.IsNullOrEmpty(model.Id)) return 0;

            var strSql = new StringBuilder();
            strSql.Append(
                " UPDATE [tbl_BirthdayGift] SET [SendGiftTime] = @SendGiftTime,[Remark] = @Remark,[OperatorId] = @OperatorId WHERE [Id] = @Id; ");

            DbCommand dc = _db.GetSqlStringCommand(strSql.ToString());
            _db.AddInParameter(dc, "Id", DbType.AnsiStringFixedLength, model.Id);
            _db.AddInParameter(dc, "SendGiftTime", DbType.DateTime, model.SendGiftTime);
            _db.AddInParameter(dc, "Remark", DbType.String, model.Remark);
            _db.AddInParameter(dc, "OperatorId", DbType.Int32, model.OperatorId);

            return DbHelper.ExecuteSql(dc, _db) > 0 ? 1 : -1;
        }

        /// <summary>
        /// 删除礼物明细
        /// </summary>
        /// <param name="id">礼物明细编号</param>
        /// <returns>
        /// 1：成功；
        /// 0：参数错误；
        /// -1：删除失败；
        /// </returns>
        public int DeleteBirthdayGift(params string[] id)
        {
            if (id == null || id.Length <= 0) return 0;

            var strSql = new StringBuilder();
            strSql.Append(" delete from tbl_BirthdayGift where ");
            if (id.Length == 1)
            {
                strSql.AppendFormat(" Id = '{0}' ", Utils.ToSqlLike(id[0]));
            }
            else
            {
                strSql.AppendFormat(" Id in ({0}) ", this.GetIdsByArr(id));
            }

            DbCommand dc = _db.GetSqlStringCommand(strSql.ToString());

            return DbHelper.ExecuteSql(dc, _db) > 0 ? 1 : -1;
        }

        /// <summary>
        /// 根据编号获取礼物明细
        /// </summary>
        /// <param name="id">礼物明细编号</param>
        /// <returns>
        /// 返回类型对应的子类
        /// </returns>
        public object GetBirthdayGift(string id)
        {
            if (string.IsNullOrEmpty(id)) return null;

            var strSql = new StringBuilder();
            BirthdayGiftType type = BirthdayGiftType.员工;
            strSql.Append(
                " SELECT [Id],[CompanyId],[ItemId],[ItemType],[SendGiftTime],[Remark],[OperatorId],[IssueTime] ");
            strSql.Append(" FROM [tbl_BirthdayGift] where [Id] = @Id; ");

            DbCommand dc = _db.GetSqlStringCommand(strSql.ToString());
            _db.AddInParameter(dc, "Id", DbType.AnsiStringFixedLength, id);

            MContactBirthdayGift contact = null;
            MGroundBirthdayGift ground = null;
            MGuideBirthdayGift guide = null;
            MSpotBirthdayGift spot = null;
            MTravellerBirthdayGift traveller = null;
            MUserBirthdayGift user = null;
            using (IDataReader dr = DbHelper.ExecuteReader(dc, _db))
            {
                #region 实体赋值

                if (dr.Read())
                {
                    type = (BirthdayGiftType)dr.GetByte(dr.GetOrdinal("ItemType"));
                    switch (type)
                    {
                        case BirthdayGiftType.员工:
                            user = new MUserBirthdayGift
                                {
                                    Id =
                                        dr.IsDBNull(dr.GetOrdinal("Id"))
                                            ? string.Empty
                                            : dr.GetString(dr.GetOrdinal("Id")),
                                    CompanyId =
                                        dr.IsDBNull(dr.GetOrdinal("CompanyId"))
                                            ? 0
                                            : dr.GetInt32(dr.GetOrdinal("CompanyId")),
                                    UserId =
                                        dr.IsDBNull(dr.GetOrdinal("ItemId"))
                                            ? 0
                                            : Utils.GetInt(dr.GetString(dr.GetOrdinal("ItemId"))),
                                    Remark =
                                        dr.IsDBNull(dr.GetOrdinal("Remark"))
                                            ? string.Empty
                                            : dr.GetString(dr.GetOrdinal("Remark")),
                                    OperatorId =
                                        dr.IsDBNull(dr.GetOrdinal("OperatorId"))
                                            ? 0
                                            : dr.GetInt32(dr.GetOrdinal("OperatorId"))
                                };
                            if (!dr.IsDBNull(dr.GetOrdinal("SendGiftTime"))) user.SendGiftTime = dr.GetDateTime(dr.GetOrdinal("SendGiftTime"));

                            if (!dr.IsDBNull(dr.GetOrdinal("IssueTime"))) user.IssueTime = dr.GetDateTime(dr.GetOrdinal("IssueTime"));
                            break;
                        case BirthdayGiftType.导游:
                            guide = new MGuideBirthdayGift
                                {
                                    Id =
                                        dr.IsDBNull(dr.GetOrdinal("Id"))
                                            ? string.Empty
                                            : dr.GetString(dr.GetOrdinal("Id")),
                                    CompanyId =
                                        dr.IsDBNull(dr.GetOrdinal("CompanyId"))
                                            ? 0
                                            : dr.GetInt32(dr.GetOrdinal("CompanyId")),
                                    GuideId =
                                        dr.IsDBNull(dr.GetOrdinal("ItemId"))
                                            ? string.Empty
                                            : dr.GetString(dr.GetOrdinal("ItemId")),
                                    Remark =
                                        dr.IsDBNull(dr.GetOrdinal("Remark"))
                                            ? string.Empty
                                            : dr.GetString(dr.GetOrdinal("Remark")),
                                    OperatorId =
                                        dr.IsDBNull(dr.GetOrdinal("OperatorId"))
                                            ? 0
                                            : dr.GetInt32(dr.GetOrdinal("OperatorId"))
                                };
                            if (!dr.IsDBNull(dr.GetOrdinal("SendGiftTime"))) guide.SendGiftTime = dr.GetDateTime(dr.GetOrdinal("SendGiftTime"));

                            if (!dr.IsDBNull(dr.GetOrdinal("IssueTime"))) guide.IssueTime = dr.GetDateTime(dr.GetOrdinal("IssueTime"));
                            break;
                        case BirthdayGiftType.组团联系人:
                            contact = new MContactBirthdayGift
                                {
                                    Id =
                                        dr.IsDBNull(dr.GetOrdinal("Id"))
                                            ? string.Empty
                                            : dr.GetString(dr.GetOrdinal("Id")),
                                    CompanyId =
                                        dr.IsDBNull(dr.GetOrdinal("CompanyId"))
                                            ? 0
                                            : dr.GetInt32(dr.GetOrdinal("CompanyId")),
                                    ContactId =
                                        dr.IsDBNull(dr.GetOrdinal("ItemId"))
                                            ? 0
                                            : Utils.GetInt(dr.GetString(dr.GetOrdinal("ItemId"))),
                                    Remark =
                                        dr.IsDBNull(dr.GetOrdinal("Remark"))
                                            ? string.Empty
                                            : dr.GetString(dr.GetOrdinal("Remark")),
                                    OperatorId =
                                        dr.IsDBNull(dr.GetOrdinal("OperatorId"))
                                            ? 0
                                            : dr.GetInt32(dr.GetOrdinal("OperatorId"))
                                };
                            if (!dr.IsDBNull(dr.GetOrdinal("SendGiftTime"))) contact.SendGiftTime = dr.GetDateTime(dr.GetOrdinal("SendGiftTime"));

                            if (!dr.IsDBNull(dr.GetOrdinal("IssueTime"))) contact.IssueTime = dr.GetDateTime(dr.GetOrdinal("IssueTime"));
                            break;
                        case BirthdayGiftType.游客:
                            traveller = new MTravellerBirthdayGift
                                {
                                    Id =
                                        dr.IsDBNull(dr.GetOrdinal("Id"))
                                            ? string.Empty
                                            : dr.GetString(dr.GetOrdinal("Id")),
                                    CompanyId =
                                        dr.IsDBNull(dr.GetOrdinal("CompanyId"))
                                            ? 0
                                            : dr.GetInt32(dr.GetOrdinal("CompanyId")),
                                    TravellerId =
                                        dr.IsDBNull(dr.GetOrdinal("ItemId"))
                                            ? string.Empty
                                            : dr.GetString(dr.GetOrdinal("ItemId")),
                                    Remark =
                                        dr.IsDBNull(dr.GetOrdinal("Remark"))
                                            ? string.Empty
                                            : dr.GetString(dr.GetOrdinal("Remark")),
                                    OperatorId =
                                        dr.IsDBNull(dr.GetOrdinal("OperatorId"))
                                            ? 0
                                            : dr.GetInt32(dr.GetOrdinal("OperatorId"))
                                };
                            if (!dr.IsDBNull(dr.GetOrdinal("SendGiftTime"))) traveller.SendGiftTime = dr.GetDateTime(dr.GetOrdinal("SendGiftTime"));

                            if (!dr.IsDBNull(dr.GetOrdinal("IssueTime"))) traveller.IssueTime = dr.GetDateTime(dr.GetOrdinal("IssueTime"));
                            break;
                        case BirthdayGiftType.地接联系人:
                            ground = new MGroundBirthdayGift
                            {
                                Id =
                                    dr.IsDBNull(dr.GetOrdinal("Id"))
                                        ? string.Empty
                                        : dr.GetString(dr.GetOrdinal("Id")),
                                CompanyId =
                                    dr.IsDBNull(dr.GetOrdinal("CompanyId"))
                                        ? 0
                                        : dr.GetInt32(dr.GetOrdinal("CompanyId")),
                                GroundId =
                                    dr.IsDBNull(dr.GetOrdinal("ItemId"))
                                        ? 0
                                        : Utils.GetInt(dr.GetString(dr.GetOrdinal("ItemId"))),
                                Remark =
                                    dr.IsDBNull(dr.GetOrdinal("Remark"))
                                        ? string.Empty
                                        : dr.GetString(dr.GetOrdinal("Remark")),
                                OperatorId =
                                    dr.IsDBNull(dr.GetOrdinal("OperatorId"))
                                        ? 0
                                        : dr.GetInt32(dr.GetOrdinal("OperatorId"))
                            };
                            if (!dr.IsDBNull(dr.GetOrdinal("SendGiftTime"))) ground.SendGiftTime = dr.GetDateTime(dr.GetOrdinal("SendGiftTime"));

                            if (!dr.IsDBNull(dr.GetOrdinal("IssueTime"))) ground.IssueTime = dr.GetDateTime(dr.GetOrdinal("IssueTime"));
                            break;
                        case BirthdayGiftType.景点联系人:
                            spot = new MSpotBirthdayGift
                            {
                                Id =
                                    dr.IsDBNull(dr.GetOrdinal("Id"))
                                        ? string.Empty
                                        : dr.GetString(dr.GetOrdinal("Id")),
                                CompanyId =
                                    dr.IsDBNull(dr.GetOrdinal("CompanyId"))
                                        ? 0
                                        : dr.GetInt32(dr.GetOrdinal("CompanyId")),
                                SpotId =
                                    dr.IsDBNull(dr.GetOrdinal("ItemId"))
                                        ? 0
                                        : Utils.GetInt(dr.GetString(dr.GetOrdinal("ItemId"))),
                                Remark =
                                    dr.IsDBNull(dr.GetOrdinal("Remark"))
                                        ? string.Empty
                                        : dr.GetString(dr.GetOrdinal("Remark")),
                                OperatorId =
                                    dr.IsDBNull(dr.GetOrdinal("OperatorId"))
                                        ? 0
                                        : dr.GetInt32(dr.GetOrdinal("OperatorId"))
                            };
                            if (!dr.IsDBNull(dr.GetOrdinal("SendGiftTime"))) spot.SendGiftTime = dr.GetDateTime(dr.GetOrdinal("SendGiftTime"));

                            if (!dr.IsDBNull(dr.GetOrdinal("IssueTime"))) spot.IssueTime = dr.GetDateTime(dr.GetOrdinal("IssueTime"));
                            break;
                    }
                }

                #endregion
            }

            switch (type)
            {
                case BirthdayGiftType.员工:
                    return user;
                case BirthdayGiftType.组团联系人:
                    return contact;
                case BirthdayGiftType.地接联系人:
                    return ground;
                case BirthdayGiftType.导游:
                    return guide;
                case BirthdayGiftType.景点联系人:
                    return spot;
                case BirthdayGiftType.游客:
                    return traveller;
                default:
                    return null;
            }
        }

        /// <summary>
        /// 获取礼物明细
        /// </summary>
        /// <param name="type">收到礼物对象类型</param>
        /// <param name="id">收到礼物对象编号</param>
        /// <returns>返回类型对应的子类</returns>
        public object GetBirthdayGift(BirthdayGiftType type, string id)
        {
            if (string.IsNullOrEmpty(id)) return null;
            var strSql = new StringBuilder();

            strSql.Append(
                " select [Id],[CompanyId],[ItemId],[ItemType],[SendGiftTime],[Remark],[OperatorId],[IssueTime] ");
            strSql.Append(" FROM [tbl_BirthdayGift] where ItemType = @ItemType and ItemId = @ItemId ");

            DbCommand dc = _db.GetSqlStringCommand(strSql.ToString());
            _db.AddInParameter(dc, "ItemType", DbType.Byte, (int)type);
            _db.AddInParameter(dc, "ItemId", DbType.AnsiStringFixedLength, id);


            IList<MContactBirthdayGift> contact = null;
            IList<MGroundBirthdayGift> ground = null;
            IList<MGuideBirthdayGift> guide = null;
            IList<MSpotBirthdayGift> spot = null;
            IList<MTravellerBirthdayGift> traveller = null;
            IList<MUserBirthdayGift> user = null;
            switch (type)
            {
                case BirthdayGiftType.导游:
                    guide = new List<MGuideBirthdayGift>();
                    break;
                case BirthdayGiftType.地接联系人:
                    ground = new List<MGroundBirthdayGift>();
                    break;
                case BirthdayGiftType.景点联系人:
                    spot = new List<MSpotBirthdayGift>();
                    break;
                case BirthdayGiftType.游客:
                    traveller = new List<MTravellerBirthdayGift>();
                    break;
                case BirthdayGiftType.员工:
                    user = new List<MUserBirthdayGift>();
                    break;
                case BirthdayGiftType.组团联系人:
                    contact = new List<MContactBirthdayGift>();
                    break;
            }
            using (IDataReader dr = DbHelper.ExecuteReader(dc, _db))
            {
                #region 实体赋值

                while (dr.Read())
                {
                    switch (type)
                    {
                        case BirthdayGiftType.员工:
                            var yg = new MUserBirthdayGift
                            {
                                Id =
                                    dr.IsDBNull(dr.GetOrdinal("Id"))
                                        ? string.Empty
                                        : dr.GetString(dr.GetOrdinal("Id")),
                                CompanyId =
                                    dr.IsDBNull(dr.GetOrdinal("CompanyId"))
                                        ? 0
                                        : dr.GetInt32(dr.GetOrdinal("CompanyId")),
                                UserId =
                                    dr.IsDBNull(dr.GetOrdinal("ItemId"))
                                        ? 0
                                        : Utils.GetInt(dr.GetString(dr.GetOrdinal("ItemId"))),
                                Remark =
                                    dr.IsDBNull(dr.GetOrdinal("Remark"))
                                        ? string.Empty
                                        : dr.GetString(dr.GetOrdinal("Remark")),
                                OperatorId =
                                    dr.IsDBNull(dr.GetOrdinal("OperatorId"))
                                        ? 0
                                        : dr.GetInt32(dr.GetOrdinal("OperatorId"))
                            };
                            if (!dr.IsDBNull(dr.GetOrdinal("SendGiftTime"))) yg.SendGiftTime = dr.GetDateTime(dr.GetOrdinal("SendGiftTime"));

                            if (!dr.IsDBNull(dr.GetOrdinal("IssueTime"))) yg.IssueTime = dr.GetDateTime(dr.GetOrdinal("IssueTime"));
                            if (user != null) user.Add(yg);
                            break;
                        case BirthdayGiftType.导游:
                            var dy = new MGuideBirthdayGift
                            {
                                Id =
                                    dr.IsDBNull(dr.GetOrdinal("Id"))
                                        ? string.Empty
                                        : dr.GetString(dr.GetOrdinal("Id")),
                                CompanyId =
                                    dr.IsDBNull(dr.GetOrdinal("CompanyId"))
                                        ? 0
                                        : dr.GetInt32(dr.GetOrdinal("CompanyId")),
                                GuideId =
                                    dr.IsDBNull(dr.GetOrdinal("ItemId"))
                                        ? string.Empty
                                        : dr.GetString(dr.GetOrdinal("ItemId")),
                                Remark =
                                    dr.IsDBNull(dr.GetOrdinal("Remark"))
                                        ? string.Empty
                                        : dr.GetString(dr.GetOrdinal("Remark")),
                                OperatorId =
                                    dr.IsDBNull(dr.GetOrdinal("OperatorId"))
                                        ? 0
                                        : dr.GetInt32(dr.GetOrdinal("OperatorId"))
                            };
                            if (!dr.IsDBNull(dr.GetOrdinal("SendGiftTime"))) dy.SendGiftTime = dr.GetDateTime(dr.GetOrdinal("SendGiftTime"));

                            if (!dr.IsDBNull(dr.GetOrdinal("IssueTime"))) dy.IssueTime = dr.GetDateTime(dr.GetOrdinal("IssueTime"));
                            if (guide != null) guide.Add(dy);
                            break;
                        case BirthdayGiftType.组团联系人:
                            var ztslxr = new MContactBirthdayGift
                            {
                                Id =
                                    dr.IsDBNull(dr.GetOrdinal("Id"))
                                        ? string.Empty
                                        : dr.GetString(dr.GetOrdinal("Id")),
                                CompanyId =
                                    dr.IsDBNull(dr.GetOrdinal("CompanyId"))
                                        ? 0
                                        : dr.GetInt32(dr.GetOrdinal("CompanyId")),
                                ContactId =
                                    dr.IsDBNull(dr.GetOrdinal("ItemId"))
                                        ? 0
                                        : Utils.GetInt(dr.GetString(dr.GetOrdinal("ItemId"))),
                                Remark =
                                    dr.IsDBNull(dr.GetOrdinal("Remark"))
                                        ? string.Empty
                                        : dr.GetString(dr.GetOrdinal("Remark")),
                                OperatorId =
                                    dr.IsDBNull(dr.GetOrdinal("OperatorId"))
                                        ? 0
                                        : dr.GetInt32(dr.GetOrdinal("OperatorId"))
                            };
                            if (!dr.IsDBNull(dr.GetOrdinal("SendGiftTime"))) ztslxr.SendGiftTime = dr.GetDateTime(dr.GetOrdinal("SendGiftTime"));

                            if (!dr.IsDBNull(dr.GetOrdinal("IssueTime"))) ztslxr.IssueTime = dr.GetDateTime(dr.GetOrdinal("IssueTime"));
                            if (contact != null) contact.Add(ztslxr);
                            break;
                        case BirthdayGiftType.游客:
                            var yk = new MTravellerBirthdayGift
                            {
                                Id =
                                    dr.IsDBNull(dr.GetOrdinal("Id"))
                                        ? string.Empty
                                        : dr.GetString(dr.GetOrdinal("Id")),
                                CompanyId =
                                    dr.IsDBNull(dr.GetOrdinal("CompanyId"))
                                        ? 0
                                        : dr.GetInt32(dr.GetOrdinal("CompanyId")),
                                TravellerId =
                                    dr.IsDBNull(dr.GetOrdinal("ItemId"))
                                        ? string.Empty
                                        : dr.GetString(dr.GetOrdinal("ItemId")),
                                Remark =
                                    dr.IsDBNull(dr.GetOrdinal("Remark"))
                                        ? string.Empty
                                        : dr.GetString(dr.GetOrdinal("Remark")),
                                OperatorId =
                                    dr.IsDBNull(dr.GetOrdinal("OperatorId"))
                                        ? 0
                                        : dr.GetInt32(dr.GetOrdinal("OperatorId"))
                            };
                            if (!dr.IsDBNull(dr.GetOrdinal("SendGiftTime"))) yk.SendGiftTime = dr.GetDateTime(dr.GetOrdinal("SendGiftTime"));

                            if (!dr.IsDBNull(dr.GetOrdinal("IssueTime"))) yk.IssueTime = dr.GetDateTime(dr.GetOrdinal("IssueTime"));
                            if (traveller != null) traveller.Add(yk);
                            break;
                        case BirthdayGiftType.地接联系人:
                            var djlxr = new MGroundBirthdayGift
                            {
                                Id =
                                    dr.IsDBNull(dr.GetOrdinal("Id"))
                                        ? string.Empty
                                        : dr.GetString(dr.GetOrdinal("Id")),
                                CompanyId =
                                    dr.IsDBNull(dr.GetOrdinal("CompanyId"))
                                        ? 0
                                        : dr.GetInt32(dr.GetOrdinal("CompanyId")),
                                GroundId =
                                    dr.IsDBNull(dr.GetOrdinal("ItemId"))
                                        ? 0
                                        : Utils.GetInt(dr.GetString(dr.GetOrdinal("ItemId"))),
                                Remark =
                                    dr.IsDBNull(dr.GetOrdinal("Remark"))
                                        ? string.Empty
                                        : dr.GetString(dr.GetOrdinal("Remark")),
                                OperatorId =
                                    dr.IsDBNull(dr.GetOrdinal("OperatorId"))
                                        ? 0
                                        : dr.GetInt32(dr.GetOrdinal("OperatorId"))
                            };
                            if (!dr.IsDBNull(dr.GetOrdinal("SendGiftTime"))) djlxr.SendGiftTime = dr.GetDateTime(dr.GetOrdinal("SendGiftTime"));

                            if (!dr.IsDBNull(dr.GetOrdinal("IssueTime"))) djlxr.IssueTime = dr.GetDateTime(dr.GetOrdinal("IssueTime"));
                            if (ground != null) ground.Add(djlxr);
                            break;
                        case BirthdayGiftType.景点联系人:
                            var jdlxr = new MSpotBirthdayGift
                            {
                                Id =
                                    dr.IsDBNull(dr.GetOrdinal("Id"))
                                        ? string.Empty
                                        : dr.GetString(dr.GetOrdinal("Id")),
                                CompanyId =
                                    dr.IsDBNull(dr.GetOrdinal("CompanyId"))
                                        ? 0
                                        : dr.GetInt32(dr.GetOrdinal("CompanyId")),
                                SpotId =
                                    dr.IsDBNull(dr.GetOrdinal("ItemId"))
                                        ? 0
                                        : Utils.GetInt(dr.GetString(dr.GetOrdinal("ItemId"))),
                                Remark =
                                    dr.IsDBNull(dr.GetOrdinal("Remark"))
                                        ? string.Empty
                                        : dr.GetString(dr.GetOrdinal("Remark")),
                                OperatorId =
                                    dr.IsDBNull(dr.GetOrdinal("OperatorId"))
                                        ? 0
                                        : dr.GetInt32(dr.GetOrdinal("OperatorId"))
                            };
                            if (!dr.IsDBNull(dr.GetOrdinal("SendGiftTime"))) jdlxr.SendGiftTime = dr.GetDateTime(dr.GetOrdinal("SendGiftTime"));

                            if (!dr.IsDBNull(dr.GetOrdinal("IssueTime"))) jdlxr.IssueTime = dr.GetDateTime(dr.GetOrdinal("IssueTime"));
                            if (spot != null) spot.Add(jdlxr);
                            break;
                    }
                }

                #endregion
            }

            switch (type)
            {
                case BirthdayGiftType.员工:
                    return user;
                case BirthdayGiftType.组团联系人:
                    return contact;
                case BirthdayGiftType.地接联系人:
                    return ground;
                case BirthdayGiftType.导游:
                    return guide;
                case BirthdayGiftType.景点联系人:
                    return spot;
                case BirthdayGiftType.游客:
                    return traveller;
                default:
                    return null;
            }
        }

        /// <summary>
        /// 获取生日提醒列表（seach 有值以seach为准，为null则以days为准）
        /// </summary>
        /// <param name="type">生日提醒类型</param>
        /// <param name="companyId">公司编号</param>
        /// <param name="days">生日提醒提前天数</param>
        /// <param name="pageSize">每页记录数</param>
        /// <param name="pageIndex">当前页数</param>
        /// <param name="recordCount">总记录数</param>
        /// <param name="seach">生日查询实体</param>
        /// <returns>返回类型对应的子类</returns>
        public object GetBirthdayList(BirthdayGiftType type, int companyId, int days, int pageSize,
            int pageIndex, ref int recordCount, MSearchBirthday seach)
        {
            if (companyId <= 0 || pageSize <= 0 || pageIndex <= 0) return null;

            IList<MUserBirthday> user = null;
            IList<MGuideBirthday> guide = null;
            IList<MContactBirthday> contact = null;
            IList<MTravellerBirthday> traveller = null;
            IList<MGroundBirthday> ground = null;
            IList<MSpotBirthday> spot = null;
            string tableName = string.Empty;
            string strWhere = string.Empty;
            string fields = string.Empty;
            string orderByStr = " Birthday desc ";

            switch (type)
            {
                case BirthdayGiftType.员工:
                    user = new List<MUserBirthday>();
                    this.GetYgSql(days, companyId, seach, ref tableName, ref strWhere, ref fields);
                    break;
                case BirthdayGiftType.导游:
                    guide = new List<MGuideBirthday>();
                    this.GetDySql(days, companyId, seach, ref tableName, ref strWhere, ref fields);
                    break;
                case BirthdayGiftType.组团联系人:
                    contact = new List<MContactBirthday>();
                    this.GetZtsLxr(days, companyId, seach, ref tableName, ref strWhere, ref fields);
                    break;
                case BirthdayGiftType.游客:
                    traveller = new List<MTravellerBirthday>();
                    this.GetYk(days, companyId, seach, ref tableName, ref strWhere, ref fields);
                    break;
                case BirthdayGiftType.地接联系人:
                    ground = new List<MGroundBirthday>();
                    this.GetDjsLxr(type, days, companyId, seach, ref tableName, ref strWhere, ref fields);
                    break;
                case BirthdayGiftType.景点联系人:
                    spot = new List<MSpotBirthday>();
                    this.GetDjsLxr(type, days, companyId, seach, ref tableName, ref strWhere, ref fields);
                    break;
            }

            if (string.IsNullOrEmpty(tableName) || string.IsNullOrEmpty(strWhere) || string.IsNullOrEmpty(fields)) return null;

            using (IDataReader dr = DbHelper.ExecuteReader1(_db, pageSize, pageIndex, ref recordCount, tableName, fields, strWhere
                , orderByStr, string.Empty))
            {
                #region 实体赋值

                while (dr.Read())
                {
                    switch (type)
                    {
                        case BirthdayGiftType.员工:
                            var yg = new MUserBirthday
                                {
                                    Address =
                                        dr.IsDBNull(dr.GetOrdinal("Address"))
                                            ? string.Empty
                                            : dr.GetString(dr.GetOrdinal("Address")),
                                    Mobile =
                                        dr.IsDBNull(dr.GetOrdinal("Mobile"))
                                            ? string.Empty
                                            : dr.GetString(dr.GetOrdinal("Mobile")),
                                    Name =
                                        dr.IsDBNull(dr.GetOrdinal("Name"))
                                            ? string.Empty
                                            : dr.GetString(dr.GetOrdinal("Name")),
                                    Qq =
                                        dr.IsDBNull(dr.GetOrdinal("QQ"))
                                            ? string.Empty
                                            : dr.GetString(dr.GetOrdinal("QQ")),
                                    Tel =
                                        dr.IsDBNull(dr.GetOrdinal("Tel"))
                                            ? string.Empty
                                            : dr.GetString(dr.GetOrdinal("Tel")),
                                    UserId = dr.IsDBNull(dr.GetOrdinal("Id")) ? 0 : dr.GetInt32(dr.GetOrdinal("Id"))
                                };
                            if (!dr.IsDBNull(dr.GetOrdinal("Birthday"))) yg.Birthday = dr.GetDateTime(dr.GetOrdinal("Birthday"));
                            if (user != null) user.Add(yg);
                            break;
                        case BirthdayGiftType.导游:
                            var dy = new MGuideBirthday
                                {
                                    Mobile =
                                        dr.IsDBNull(dr.GetOrdinal("Phone"))
                                            ? string.Empty
                                            : dr.GetString(dr.GetOrdinal("Phone")),
                                    Name =
                                        dr.IsDBNull(dr.GetOrdinal("Name"))
                                            ? string.Empty
                                            : dr.GetString(dr.GetOrdinal("Name")),
                                    GuideId =
                                        dr.IsDBNull(dr.GetOrdinal("Id"))
                                            ? string.Empty
                                            : dr.GetString(dr.GetOrdinal("Id"))
                                };
                            if (!dr.IsDBNull(dr.GetOrdinal("Birthday"))) dy.Birthday = dr.GetDateTime(dr.GetOrdinal("Birthday"));
                            if (guide != null) guide.Add(dy);
                            break;
                        case BirthdayGiftType.组团联系人:
                            var ztslxr = new MContactBirthday
                                {
                                    Address =
                                        dr.IsDBNull(dr.GetOrdinal("Address"))
                                            ? string.Empty
                                            : dr.GetString(dr.GetOrdinal("Address")),
                                    Mobile =
                                        dr.IsDBNull(dr.GetOrdinal("Mobile"))
                                            ? string.Empty
                                            : dr.GetString(dr.GetOrdinal("Mobile")),
                                    Name =
                                        dr.IsDBNull(dr.GetOrdinal("Name"))
                                            ? string.Empty
                                            : dr.GetString(dr.GetOrdinal("Name")),
                                    Qq =
                                        dr.IsDBNull(dr.GetOrdinal("QQ"))
                                            ? string.Empty
                                            : dr.GetString(dr.GetOrdinal("QQ")),
                                    Tel =
                                        dr.IsDBNull(dr.GetOrdinal("Tel"))
                                            ? string.Empty
                                            : dr.GetString(dr.GetOrdinal("Tel")),
                                    ContactId = dr.IsDBNull(dr.GetOrdinal("Id")) ? 0 : dr.GetInt32(dr.GetOrdinal("Id")),
                                    UnitName =
                                        dr.IsDBNull(dr.GetOrdinal("UnitName"))
                                            ? string.Empty
                                            : dr.GetString(dr.GetOrdinal("UnitName"))
                                };
                            if (!dr.IsDBNull(dr.GetOrdinal("Birthday"))) ztslxr.Birthday = dr.GetDateTime(dr.GetOrdinal("Birthday"));
                            if (contact != null) contact.Add(ztslxr);
                            break;
                        case BirthdayGiftType.游客:
                            var yk = new MTravellerBirthday
                                {
                                    Mobile =
                                        dr.IsDBNull(dr.GetOrdinal("Mobile"))
                                            ? string.Empty
                                            : dr.GetString(dr.GetOrdinal("Mobile")),
                                    Name =
                                        dr.IsDBNull(dr.GetOrdinal("Name"))
                                            ? string.Empty
                                            : dr.GetString(dr.GetOrdinal("Name")),
                                    TravellerId =
                                        dr.IsDBNull(dr.GetOrdinal("Id"))
                                            ? string.Empty
                                            : dr.GetString(dr.GetOrdinal("Id")),
                                    CardNumber =
                                        dr.IsDBNull(dr.GetOrdinal("CardNumber"))
                                            ? string.Empty
                                            : dr.GetString(dr.GetOrdinal("CardNumber")),
                                    TourNo =
                                        dr.IsDBNull(dr.GetOrdinal("TourNo"))
                                            ? string.Empty
                                            : dr.GetString(dr.GetOrdinal("TourNo"))
                                };
                            if (!dr.IsDBNull(dr.GetOrdinal("Birthday"))) yk.Birthday = dr.GetDateTime(dr.GetOrdinal("Birthday"));
                            if (!dr.IsDBNull(dr.GetOrdinal("Sex"))) yk.Sex = (Model.EnumType.CompanyStructure.Sex)dr.GetByte(dr.GetOrdinal("Sex"));
                            if (traveller != null) traveller.Add(yk);
                            break;
                        case BirthdayGiftType.地接联系人:
                            var djslxr = new MGroundBirthday
                                {
                                    Mobile =
                                        dr.IsDBNull(dr.GetOrdinal("Mobile"))
                                            ? string.Empty
                                            : dr.GetString(dr.GetOrdinal("Mobile")),
                                    Name =
                                        dr.IsDBNull(dr.GetOrdinal("Name"))
                                            ? string.Empty
                                            : dr.GetString(dr.GetOrdinal("Name")),
                                    GroundId = dr.IsDBNull(dr.GetOrdinal("Id")) ? 0 : dr.GetInt32(dr.GetOrdinal("Id")),
                                    GroundName =
                                        dr.IsDBNull(dr.GetOrdinal("UnitName"))
                                            ? string.Empty
                                            : dr.GetString(dr.GetOrdinal("UnitName"))
                                };
                            if (!dr.IsDBNull(dr.GetOrdinal("Birthday"))) djslxr.Birthday = dr.GetDateTime(dr.GetOrdinal("Birthday"));
                            if (ground != null) ground.Add(djslxr);
                            break;
                        case BirthdayGiftType.景点联系人:
                            var jdlxr = new MSpotBirthday
                            {
                                Mobile =
                                    dr.IsDBNull(dr.GetOrdinal("Mobile"))
                                        ? string.Empty
                                        : dr.GetString(dr.GetOrdinal("Mobile")),
                                Name =
                                    dr.IsDBNull(dr.GetOrdinal("Name"))
                                        ? string.Empty
                                        : dr.GetString(dr.GetOrdinal("Name")),
                                SpotId = dr.IsDBNull(dr.GetOrdinal("Id")) ? 0 : dr.GetInt32(dr.GetOrdinal("Id")),
                                SpotName =
                                    dr.IsDBNull(dr.GetOrdinal("UnitName"))
                                        ? string.Empty
                                        : dr.GetString(dr.GetOrdinal("UnitName"))
                            };
                            if (!dr.IsDBNull(dr.GetOrdinal("Birthday"))) jdlxr.Birthday = dr.GetDateTime(dr.GetOrdinal("Birthday"));
                            if (spot != null) spot.Add(jdlxr);
                            break;
                    }
                }

                #endregion
            }

            switch (type)
            {
                case BirthdayGiftType.员工:
                    return user;
                case BirthdayGiftType.组团联系人:
                    return contact;
                case BirthdayGiftType.地接联系人:
                    return ground;
                case BirthdayGiftType.导游:
                    return guide;
                case BirthdayGiftType.景点联系人:
                    return spot;
                case BirthdayGiftType.游客:
                    return traveller;
                default:
                    return null;
            }
        }

        /// <summary>
        /// 获取生日弹窗提醒
        /// </summary>
        /// <param name="companyId">公司编号</param>
        /// <param name="seach">查询实体</param>
        /// <returns></returns>
        public IList<MBirthdayWindow> GetBirthdayWindow(int companyId, MSearchBirthday seach)
        {
            if (companyId <= 0) return null;

            string tableName = string.Empty;
            string strWhere = string.Empty;
            string fields = string.Empty;
            string orderByStr = " Birthday desc ";
            var strSql = new StringBuilder();

            //员工
            this.GetYgSql(0, companyId, seach, ref tableName, ref strWhere, ref fields);
            strSql.AppendFormat(" select {0} from {1} where {2} order by {3}; ", fields, tableName, strWhere, orderByStr);
            strSql.AppendLine();
            //导游
            this.GetDySql(0, companyId, seach, ref tableName, ref strWhere, ref fields);
            strSql.AppendFormat(" select {0} from {1} where {2} order by {3}; ", fields, tableName, strWhere, orderByStr);
            strSql.AppendLine();
            //组团联系人
            this.GetZtsLxr(0, companyId, seach, ref tableName, ref strWhere, ref fields);
            strSql.AppendFormat(" select {0} from {1} where {2} order by {3}; ", fields, tableName, strWhere, orderByStr);
            strSql.AppendLine();
            //游客 
            this.GetYk(0, companyId, seach, ref tableName, ref strWhere, ref fields);
            strSql.AppendFormat(" select {0} from {1} where {2} order by {3}; ", fields, tableName, strWhere, orderByStr);
            strSql.AppendLine();
            //地接联系人
            this.GetDjsLxr(BirthdayGiftType.地接联系人, 0, companyId, seach, ref tableName, ref strWhere, ref fields);
            strSql.AppendFormat(" select {0} from {1} where {2} order by {3}; ", fields, tableName, strWhere, orderByStr);
            strSql.AppendLine();
            //景点联系人
            this.GetDjsLxr(BirthdayGiftType.景点联系人, 0, companyId, seach, ref tableName, ref strWhere, ref fields);
            strSql.AppendFormat(" select {0} from {1} where {2} order by {3}; ", fields, tableName, strWhere, orderByStr);
            strSql.AppendLine();

            var list = new List<MBirthdayWindow>();
            DbCommand dc = _db.GetSqlStringCommand(strSql.ToString());

            using (IDataReader dr = DbHelper.ExecuteReader(dc, _db))
            {
                while (dr.Read())
                {
                    var item = new MBirthdayWindow
                    {
                        Mobile =
                            dr.IsDBNull(dr.GetOrdinal("Mobile"))
                                ? string.Empty
                                : dr.GetString(dr.GetOrdinal("Mobile")),
                        Name =
                            dr.IsDBNull(dr.GetOrdinal("Name"))
                                ? string.Empty
                                : dr.GetString(dr.GetOrdinal("Name"))
                    };
                    if (!dr.IsDBNull(dr.GetOrdinal("Birthday"))) item.Birthday = dr.GetDateTime(dr.GetOrdinal("Birthday"));
                    item.PeopleType = BirthdayGiftType.员工;

                    list.Add(item);
                }

                dr.NextResult();
                while (dr.Read())
                {
                    var item = new MBirthdayWindow
                    {
                        Mobile =
                            dr.IsDBNull(dr.GetOrdinal("Mobile"))
                                ? string.Empty
                                : dr.GetString(dr.GetOrdinal("Mobile")),
                        Name =
                            dr.IsDBNull(dr.GetOrdinal("Name"))
                                ? string.Empty
                                : dr.GetString(dr.GetOrdinal("Name"))
                    };
                    if (!dr.IsDBNull(dr.GetOrdinal("Birthday"))) item.Birthday = dr.GetDateTime(dr.GetOrdinal("Birthday"));
                    item.PeopleType = BirthdayGiftType.导游;

                    list.Add(item);
                }

                dr.NextResult();
                while (dr.Read())
                {
                    var item = new MBirthdayWindow
                    {
                        Mobile =
                            dr.IsDBNull(dr.GetOrdinal("Mobile"))
                                ? string.Empty
                                : dr.GetString(dr.GetOrdinal("Mobile")),
                        Name =
                            dr.IsDBNull(dr.GetOrdinal("Name"))
                                ? string.Empty
                                : dr.GetString(dr.GetOrdinal("Name"))
                    };
                    if (!dr.IsDBNull(dr.GetOrdinal("Birthday"))) item.Birthday = dr.GetDateTime(dr.GetOrdinal("Birthday"));
                    item.PeopleType = BirthdayGiftType.组团联系人;

                    list.Add(item);
                }

                dr.NextResult();
                while (dr.Read())
                {
                    var item = new MBirthdayWindow
                    {
                        Mobile =
                            dr.IsDBNull(dr.GetOrdinal("Mobile"))
                                ? string.Empty
                                : dr.GetString(dr.GetOrdinal("Mobile")),
                        Name =
                            dr.IsDBNull(dr.GetOrdinal("Name"))
                                ? string.Empty
                                : dr.GetString(dr.GetOrdinal("Name"))
                    };
                    if (!dr.IsDBNull(dr.GetOrdinal("Birthday"))) item.Birthday = dr.GetDateTime(dr.GetOrdinal("Birthday"));
                    item.PeopleType = BirthdayGiftType.游客;

                    list.Add(item);
                }

                dr.NextResult();
                while (dr.Read())
                {
                    var item = new MBirthdayWindow
                    {
                        Mobile =
                            dr.IsDBNull(dr.GetOrdinal("Mobile"))
                                ? string.Empty
                                : dr.GetString(dr.GetOrdinal("Mobile")),
                        Name =
                            dr.IsDBNull(dr.GetOrdinal("Name"))
                                ? string.Empty
                                : dr.GetString(dr.GetOrdinal("Name"))
                    };
                    if (!dr.IsDBNull(dr.GetOrdinal("Birthday"))) item.Birthday = dr.GetDateTime(dr.GetOrdinal("Birthday"));
                    item.PeopleType = BirthdayGiftType.地接联系人;

                    list.Add(item);
                }

                dr.NextResult();
                while (dr.Read())
                {
                    var item = new MBirthdayWindow
                    {
                        Mobile =
                            dr.IsDBNull(dr.GetOrdinal("Mobile"))
                                ? string.Empty
                                : dr.GetString(dr.GetOrdinal("Mobile")),
                        Name =
                            dr.IsDBNull(dr.GetOrdinal("Name"))
                                ? string.Empty
                                : dr.GetString(dr.GetOrdinal("Name"))
                    };
                    if (!dr.IsDBNull(dr.GetOrdinal("Birthday"))) item.Birthday = dr.GetDateTime(dr.GetOrdinal("Birthday"));
                    item.PeopleType = BirthdayGiftType.景点联系人;

                    list.Add(item);
                }
            }

            return list;
        }

        #region private member

        /// <summary>
        /// 获取地接社、景点联系人生日提醒sql
        /// </summary>
        /// <param name="type">类型，只能是地接和景点</param>
        /// <param name="days">生日提醒提前天数</param>
        /// <param name="companyId">公司编号</param>
        /// <param name="seach">生日查询实体</param>
        /// <param name="fields">字段</param>
        /// <param name="where">条件</param>
        /// <param name="tableName">表名字</param>
        /// <returns></returns>
        private void GetDjsLxr(BirthdayGiftType type, int days, int companyId, MSearchBirthday seach, ref string tableName
            , ref string where, ref string fields)
        {
            var strFields = new StringBuilder();
            var strWhere = new StringBuilder();
            var t = Model.EnumType.CompanyStructure.SupplierType.地接;
            switch (type)
            {
                case BirthdayGiftType.地接联系人:
                    t = Model.EnumType.CompanyStructure.SupplierType.地接;
                    break;
                case BirthdayGiftType.景点联系人:
                    t = Model.EnumType.CompanyStructure.SupplierType.景点;
                    break;
            }
            tableName = "tbl_SupplierContact";
            strFields.Append(" Id,ContactName as Name,Birthday,ContactMobile as Mobile ");
            strFields.Append(" ,(select UnitName from tbl_CompanySupplier as cu where cu.Id = SupplierId) as UnitName ");
            strWhere.Append(" Birthday is not null ");
            strWhere.AppendFormat(" and CompanyId = {0} ", companyId);
            strWhere.AppendFormat(
                " and exists (select 1 from tbl_CompanySupplier as cs where tbl_SupplierContact.SupplierId = cs.Id and cs.SupplierType = {0}) ",
                (int)t);
            strWhere.Append(
                " and exists (select 1 from tbl_CompanySupplier as a where a.IsDelete = '0' and a.Id = SupplierId) ");
            if (seach == null || (string.IsNullOrEmpty(seach.Name) && !seach.StartBirthday.HasValue && !seach.EndBirthday.HasValue))
            {
                if (days > 0)
                {
                    //月份相同，提前days天提醒，不含生日当天；
                    //要含生日当天 将 and day('{0}') - day(Birthday) > 0 改成 and day('{0}') - day(Birthday) >= 0即可
                    strWhere.AppendFormat(
                        " and month(Birthday) = month('{0}') and day('{0}') - day(Birthday) > 0 and day('{0}') - day(Birthday) <= {1}  ",
                        DateTime.Now.AddDays(days).ToShortDateString(),
                        days);
                }
                else
                {
                    strWhere.AppendFormat(
                        " and month(Birthday) = month('{0}') and day(Birthday) = day('{0}') ",
                        DateTime.Now.ToShortDateString());
                }
            }
            else
            {
                if (!string.IsNullOrEmpty(seach.Name))
                {
                    strWhere.AppendFormat(" and ContactName like '%{0}%' ", Utils.ToSqlLike(seach.Name));
                }
                if (seach.StartBirthday.HasValue)
                {
                    strWhere.AppendFormat(
                        " and datediff(dd,cast('{0}-{1}-{2}' as DateTime),cast('{0}-' + cast(month(Birthday) as varchar(2)) + '-' + cast(day(Birthday) as varchar(2)) as DateTime)) >= 0 ",
                        DateTime.Now.Year,
                        seach.StartBirthday.Value.Month,
                        seach.StartBirthday.Value.Day);
                }
                if (seach.EndBirthday.HasValue)
                {
                    strWhere.AppendFormat(
                        " and datediff(dd,cast('{0}-{1}-{2}' as DateTime),cast('{0}-' + cast(month(Birthday) as varchar(2)) + '-' + cast(day(Birthday) as varchar(2)) as DateTime)) <= 0 ",
                        DateTime.Now.Year,
                        seach.EndBirthday.Value.Month,
                        seach.EndBirthday.Value.Day);
                }
            }

            where = strWhere.ToString();
            fields = strFields.ToString();
        }

        /// <summary>
        /// 获取游客生日提醒sql
        /// </summary>
        /// <param name="days">生日提醒提前天数</param>
        /// <param name="companyId">公司编号</param>
        /// <param name="seach">生日查询实体</param>
        /// <param name="fields">字段</param>
        /// <param name="where">条件</param>
        /// <param name="tableName">表名字</param>
        /// <returns></returns>
        private void GetYk(int days, int companyId, MSearchBirthday seach, ref string tableName, ref string where, ref string fields)
        {
            var strFields = new StringBuilder();
            var strWhere = new StringBuilder();
            tableName = "tbl_PlanPiaoTraveller";
            strFields.Append(" TravellerId as Id,TravellerName as Name,Gender as Sex,CardNumber,Birthday,Contact as Mobile ");
            strFields.Append(" ,(select TourCode from tbl_Tour as a where a.TourId = (select top 1 b.TourId from tbl_PlanPiao as b where b.PlanId = tbl_PlanPiaoTraveller.PlanId)) as TourNo ");
            strWhere.Append(" Birthday is not null ");
            strWhere.AppendFormat(
                " and CompanyId = {1} and CardType = {0} ", (int)Model.EnumType.TourStructure.CardType.身份证, companyId);
            strWhere.Append(
                " and exists (select 1 from tbl_Tour as d where d.IsDelete = '0' and d.TourId = (select top 1 e.TourId from tbl_PlanPiao as e where e.PlanId = tbl_PlanPiaoTraveller.PlanId)) ");
            if (seach == null || (string.IsNullOrEmpty(seach.Name) && !seach.StartBirthday.HasValue && !seach.EndBirthday.HasValue))
            {
                if (days > 0)
                {
                    //月份相同，提前days天提醒，不含生日当天；
                    //要含生日当天 将 and day('{0}') - day(Birthday) > 0 改成 and day('{0}') - day(Birthday) >= 0即可
                    strWhere.AppendFormat(
                        " and month(Birthday) = month('{0}') and day('{0}') - day(Birthday) > 0 and day('{0}') - day(Birthday) <= {1}  ",
                        DateTime.Now.AddDays(days).ToShortDateString(),
                        days);
                }
                else
                {
                    strWhere.AppendFormat(
                        " and month(Birthday) = month('{0}') and day(Birthday) = day('{0}') ",
                        DateTime.Now.ToShortDateString());
                }
            }
            else
            {
                if (!string.IsNullOrEmpty(seach.Name))
                {
                    strWhere.AppendFormat(" and TravellerName like '%{0}%' ", Utils.ToSqlLike(seach.Name));
                }
                if (seach.StartBirthday.HasValue)
                {
                    strWhere.AppendFormat(
                       " and datediff(dd,cast('{0}-{1}-{2}' as DateTime),cast('{0}-' + cast(month(Birthday) as varchar(2)) + '-' + cast(day(Birthday) as varchar(2)) as DateTime)) >= 0 ",
                       DateTime.Now.Year,
                       seach.StartBirthday.Value.Month,
                       seach.StartBirthday.Value.Day);
                }
                if (seach.EndBirthday.HasValue)
                {
                    strWhere.AppendFormat(
                        " and datediff(dd,cast('{0}-{1}-{2}' as DateTime),cast('{0}-' + cast(month(Birthday) as varchar(2)) + '-' + cast(day(Birthday) as varchar(2)) as DateTime)) <= 0 ",
                        DateTime.Now.Year,
                        seach.EndBirthday.Value.Month,
                        seach.EndBirthday.Value.Day);
                }
            }

            where = strWhere.ToString();
            fields = strFields.ToString();
        }

        /// <summary>
        /// 获取组团社联系人生日提醒sql
        /// </summary>
        /// <param name="days">生日提醒提前天数</param>
        /// <param name="companyId">公司编号</param>
        /// <param name="seach">生日查询实体</param>
        /// <param name="fields">字段</param>
        /// <param name="where">条件</param>
        /// <param name="tableName">表名字</param>
        /// <returns></returns>
        private void GetZtsLxr(int days, int companyId, MSearchBirthday seach, ref string tableName, ref string where, ref string fields)
        {
            var strFields = new StringBuilder();
            var strWhere = new StringBuilder();
            tableName = "tbl_CustomerContactInfo";

            strFields.Append(" Id,[Name],Birthday,Tel,Mobile,QQ ");
            strFields.Append(" ,(select CustomerName from tbl_Customer as a where a.Id = CustomerId) as UnitName ");
            strFields.Append(" ,(select Address from tbl_Customer as b where b.Id = CustomerId) as Address ");
            strWhere.AppendFormat(" Birthday is not null and CompanyId = {0} ", companyId);
            strWhere.Append(" and exists (select 1 from tbl_Customer as c where c.Id = CustomerId and c.IsDelete = '0') ");
            if (seach == null || (string.IsNullOrEmpty(seach.Name) && !seach.StartBirthday.HasValue && !seach.EndBirthday.HasValue))
            {
                if (days > 0)
                {
                    //月份相同，提前days天提醒，不含生日当天；
                    //要含生日当天 将 and day('{0}') - day(Birthday) > 0 改成 and day('{0}') - day(Birthday) >= 0即可
                    strWhere.AppendFormat(
                        " and month(Birthday) = month('{0}') and day('{0}') - day(Birthday) > 0 and day('{0}') - day(Birthday) <= {1}  ",
                        DateTime.Now.AddDays(days).ToShortDateString(),
                        days);
                }
                else
                {
                    strWhere.AppendFormat(
                        " and month(Birthday) = month('{0}') and day(Birthday) = day('{0}') ",
                        DateTime.Now.ToShortDateString());
                }
            }
            else
            {
                if (!string.IsNullOrEmpty(seach.Name))
                {
                    strWhere.AppendFormat(" and [Name] like '%{0}%' ", Utils.ToSqlLike(seach.Name));
                }
                if (seach.StartBirthday.HasValue)
                {
                    strWhere.AppendFormat(
                       " and datediff(dd,cast('{0}-{1}-{2}' as DateTime),cast('{0}-' + cast(month(Birthday) as varchar(2)) + '-' + cast(day(Birthday) as varchar(2)) as DateTime)) >= 0 ",
                       DateTime.Now.Year,
                       seach.StartBirthday.Value.Month,
                       seach.StartBirthday.Value.Day);
                }
                if (seach.EndBirthday.HasValue)
                {
                    strWhere.AppendFormat(
                        " and datediff(dd,cast('{0}-{1}-{2}' as DateTime),cast('{0}-' + cast(month(Birthday) as varchar(2)) + '-' + cast(day(Birthday) as varchar(2)) as DateTime)) <= 0 ",
                        DateTime.Now.Year,
                        seach.EndBirthday.Value.Month,
                        seach.EndBirthday.Value.Day);
                }
            }

            where = strWhere.ToString();
            fields = strFields.ToString();
        }

        /// <summary>
        /// 获取导游生日提醒sql
        /// </summary>
        /// <param name="days">生日提醒提前天数</param>
        /// <param name="companyId">公司编号</param>
        /// <param name="seach">生日查询实体</param>
        /// <param name="fields">字段</param>
        /// <param name="where">条件</param>
        /// <param name="tableName">表名字</param>
        /// <returns></returns>
        private void GetDySql(int days, int companyId, MSearchBirthday seach, ref string tableName, ref string where, ref string fields)
        {
            var strFields = new StringBuilder();
            var strWhere = new StringBuilder();
            tableName = "tbl_SupplierGuide";

            strFields.Append(" Id,GuideName as Name,Birthday,Phone,Phone as Tel,Phone as Mobile ");
            strWhere.AppendFormat(" Birthday is not null and CompanyId = {0} ", companyId);
            if (seach == null || (string.IsNullOrEmpty(seach.Name) && !seach.StartBirthday.HasValue && !seach.EndBirthday.HasValue))
            {
                if (days > 0)
                {
                    //月份相同，提前days天提醒，不含生日当天；
                    //要含生日当天 将 and day('{0}') - day(Birthday) > 0 改成 and day('{0}') - day(Birthday) >= 0即可
                    strWhere.AppendFormat(
                        " and month(Birthday) = month('{0}') and day('{0}') - day(Birthday) > 0 and day('{0}') - day(Birthday) <= {1}  ",
                        DateTime.Now.AddDays(days).ToShortDateString(),
                        days);
                }
                else
                {
                    strWhere.AppendFormat(
                        " and month(Birthday) = month('{0}') and day(Birthday) = day('{0}') ",
                        DateTime.Now.ToShortDateString());
                }
            }
            else
            {
                if (!string.IsNullOrEmpty(seach.Name))
                {
                    strWhere.AppendFormat(" and GuideName like '%{0}%' ", Utils.ToSqlLike(seach.Name));
                }
                if (seach.StartBirthday.HasValue)
                {
                    strWhere.AppendFormat(
                      " and datediff(dd,cast('{0}-{1}-{2}' as DateTime),cast('{0}-' + cast(month(Birthday) as varchar(2)) + '-' + cast(day(Birthday) as varchar(2)) as DateTime)) >= 0 ",
                      DateTime.Now.Year,
                      seach.StartBirthday.Value.Month,
                      seach.StartBirthday.Value.Day);
                }
                if (seach.EndBirthday.HasValue)
                {
                    strWhere.AppendFormat(
                        " and datediff(dd,cast('{0}-{1}-{2}' as DateTime),cast('{0}-' + cast(month(Birthday) as varchar(2)) + '-' + cast(day(Birthday) as varchar(2)) as DateTime)) <= 0 ",
                        DateTime.Now.Year,
                        seach.EndBirthday.Value.Month,
                        seach.EndBirthday.Value.Day);
                }
            }

            where = strWhere.ToString();
            fields = strFields.ToString();
        }

        /// <summary>
        /// 获取员工生日提醒sql
        /// </summary>
        /// <param name="days">生日提醒提前天数</param>
        /// <param name="companyId">公司编号</param>
        /// <param name="seach">生日查询实体</param>
        /// <param name="fields">字段</param>
        /// <param name="where">条件</param>
        /// <param name="tableName">表名字</param>
        /// <returns></returns>
        private void GetYgSql(int days, int companyId, MSearchBirthday seach, ref string tableName, ref string where, ref string fields)
        {
            var strFields = new StringBuilder();
            var strWhere = new StringBuilder();
            tableName = "tbl_CompanyUser";

            strFields.Append(" Id,ContactName as Name,Birthday,Address,ContactTel as Tel,ContactMobile as Mobile,QQ ");
            strWhere.Append(" IsDelete = '0' and IsSystem = '0' and Birthday is not null ");
            strWhere.AppendFormat(
                " and CompanyId = {0} and UserType = {1} ",
                companyId,
                (int)Model.EnumType.CompanyStructure.UserType.专线用户);
            if (seach == null || (string.IsNullOrEmpty(seach.Name) && !seach.StartBirthday.HasValue && !seach.EndBirthday.HasValue))
            {
                if (days > 0)
                {
                    //月份相同，提前days天提醒，不含生日当天；
                    //要含生日当天 将 and day('{0}') - day(Birthday) > 0 改成 and day('{0}') - day(Birthday) >= 0即可
                    strWhere.AppendFormat(
                        " and month(Birthday) = month('{0}') and day('{0}') - day(Birthday) > 0 and day('{0}') - day(Birthday) <= {1}  ",
                        DateTime.Now.AddDays(days).ToShortDateString(),
                        days);
                }
                else
                {
                    strWhere.AppendFormat(
                        " and month(Birthday) = month('{0}') and day(Birthday) = day('{0}') ",
                        DateTime.Now.ToShortDateString());
                }
            }
            else
            {
                if (!string.IsNullOrEmpty(seach.Name))
                {
                    strWhere.AppendFormat(" and ContactName like '%{0}%' ", Utils.ToSqlLike(seach.Name));
                }
                if (seach.StartBirthday.HasValue)
                {
                    strWhere.AppendFormat(
                      " and datediff(dd,cast('{0}-{1}-{2}' as DateTime),cast('{0}-' + cast(month(Birthday) as varchar(2)) + '-' + cast(day(Birthday) as varchar(2)) as DateTime)) >= 0 ",
                      DateTime.Now.Year,
                      seach.StartBirthday.Value.Month,
                      seach.StartBirthday.Value.Day);
                }
                if (seach.EndBirthday.HasValue)
                {
                    strWhere.AppendFormat(
                         " and datediff(dd,cast('{0}-{1}-{2}' as DateTime),cast('{0}-' + cast(month(Birthday) as varchar(2)) + '-' + cast(day(Birthday) as varchar(2)) as DateTime)) <= 0 ",
                         DateTime.Now.Year,
                         seach.EndBirthday.Value.Month,
                         seach.EndBirthday.Value.Day);
                }
            }

            where = strWhere.ToString();
            fields = strFields.ToString();
        }

        #endregion
    }
}
