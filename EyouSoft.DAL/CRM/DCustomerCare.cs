using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using System.Text;
using System.Data.Common;
using EyouSoft.Toolkit.DAL;
using EyouSoft.Toolkit;
using Microsoft.Practices.EnterpriseLibrary.Data;
using EyouSoft.Model.CRM;
using System.Data;

namespace EyouSoft.DAL.CRM
{
    /// <summary>
    /// 客户关怀数据访问 
    /// </summary>
    public class DCustomerCare : DALBase, IDAL.CRM.ICustomerCare
    {
        private readonly Database _db;

        public DCustomerCare()
        {
            _db = this.SystemStore;
        }

        /// <summary>
        /// 添加客户关怀
        /// </summary>
        /// <param name="model">客户关怀实体</param>
        /// <returns>
        /// 1：成功；
        /// 0：参数错误；
        /// -1：客户资料不存在；
        /// -2：添加失败；
        /// </returns>
        public int AddCustomerCare(MCustomerCare model)
        {
            if (model == null || model.CompanyId <= 0 || string.IsNullOrEmpty(model.CustomerId)) return 0;

            model.IssueTime = DateTime.Now;
            var strSql = new StringBuilder();
            strSql.Append(" declare @tmpId int; ");
            strSql.Append(" set @tmpId = -1; ");
            strSql.Append(" if exists (select 1 from tbl_Customer where Id = @CustomerId and IsDelete = '0') ");
            strSql.Append(" begin ");
            strSql.Append(" INSERT INTO [tbl_CustomerCarefor] ([CompanyId],[CustomerId],[VisitName],[VisitTime],[PayMoney],[PayReason],[CustomerHobby],[OperatorId],[IssueTime]) VALUES  (@CompanyId,@CustomerId,@VisitName,@VisitTime,@PayMoney,@PayReason,@CustomerHobby,@OperatorId,@IssueTime); ");
            strSql.Append(" set @tmpId = @@identity; ");
            strSql.Append(" if @tmpId <= 0 begin  set @tmpId = -2;  end ");
            strSql.Append(" end ");
            strSql.Append(" select @tmpId; ");

            DbCommand dc = _db.GetSqlStringCommand(strSql.ToString());
            _db.AddInParameter(dc, "CompanyId", DbType.Int32, model.CompanyId);
            _db.AddInParameter(dc, "CustomerId", DbType.AnsiStringFixedLength, model.CustomerId);
            _db.AddInParameter(dc, "VisitName", DbType.String, model.VisitName);
            _db.AddInParameter(dc, "VisitTime", DbType.DateTime, model.VisitTime);
            _db.AddInParameter(dc, "PayMoney", DbType.Decimal, model.PayMoney);
            _db.AddInParameter(dc, "PayReason", DbType.String, model.PayReason);
            _db.AddInParameter(dc, "CustomerHobby", DbType.String, model.CustomerHobby);
            _db.AddInParameter(dc, "OperatorId", DbType.Int32, model.OperatorId);
            _db.AddInParameter(dc, "IssueTime", DbType.DateTime, model.IssueTime);

            object obj = DbHelper.GetSingleBySqlTrans(dc, _db);

            if (obj == null) return -2;

            int tmp = Utils.GetInt(obj.ToString());

            if (tmp > 0)
            {
                model.CareId = tmp;
                return 1;
            }

            return tmp;
        }

        /// <summary>
        /// 修改客户关怀
        /// </summary>
        /// <param name="model">客户关怀实体</param>
        /// <returns>
        /// 1：成功；
        /// 0：参数错误；
        /// -1：客户资料不存在；
        /// -2：修改失败；
        /// </returns>
        public int UpdateCustomerCare(MCustomerCare model)
        {
            if (model == null || model.CompanyId <= 0 || string.IsNullOrEmpty(model.CustomerId) || model.CareId <= 0) return 0;
            var strSql = new StringBuilder();

            strSql.Append(" declare @tmpId int; ");
            strSql.Append(" set @tmpId = -1; ");
            strSql.Append(" if exists (select 1 from tbl_Customer where Id = @CustomerId and IsDelete = '0') ");
            strSql.Append(" begin ");
            strSql.Append(" UPDATE [tbl_CustomerCarefor] SET [CustomerId]=@CustomerId,[VisitName]=@VisitName,[VisitTime]=@VisitTime,[PayMoney]=@PayMoney,[PayReason]=@PayReason,[CustomerHobby]=@CustomerHobby,[OperatorId]=@OperatorId WHERE [Id] = @Id; ");
            strSql.Append(" set @tmpId = 1; ");
            strSql.Append(" end ");
            strSql.Append(" select @tmpId; ");

            DbCommand dc = _db.GetSqlStringCommand(strSql.ToString());
            _db.AddInParameter(dc, "Id", DbType.Int32, model.CareId);
            _db.AddInParameter(dc, "CustomerId", DbType.AnsiStringFixedLength, model.CustomerId);
            _db.AddInParameter(dc, "VisitName", DbType.String, model.VisitName);
            _db.AddInParameter(dc, "VisitTime", DbType.DateTime, model.VisitTime);
            _db.AddInParameter(dc, "PayMoney", DbType.Decimal, model.PayMoney);
            _db.AddInParameter(dc, "PayReason", DbType.String, model.PayReason);
            _db.AddInParameter(dc, "CustomerHobby", DbType.String, model.CustomerHobby);
            _db.AddInParameter(dc, "OperatorId", DbType.Int32, model.OperatorId);

            object obj = DbHelper.GetSingleBySqlTrans(dc, _db);

            if (obj == null) return -2;

            return Utils.GetInt(obj.ToString());
        }

        /// <summary>
        /// 删除客户关怀
        /// </summary>
        /// <param name="id">客户关怀编号</param>
        /// <returns>
        /// 1：成功；
        /// 0：参数错误；
        /// -1：删除失败；
        /// </returns>
        public int DeleteCustomerCare(params int[] id)
        {
            if (id == null || id.Length <= 0) return 0;

            var strSql = new StringBuilder();
            strSql.Append(" delete from tbl_CustomerCarefor where ");
            if (id.Length == 1)
            {
                strSql.AppendFormat(" Id = {0} ", id[0]);
            }
            else
            {
                strSql.AppendFormat(" Id in ({0}) ", this.GetIdsByArr(id));
            }

            DbCommand dc = _db.GetSqlStringCommand(strSql.ToString());

            return DbHelper.ExecuteSql(dc, _db) > 0 ? 1 : -1;
        }

        /// <summary>
        /// 获取客户关怀
        /// </summary>
        /// <param name="id">客户关怀编号</param>
        /// <returns></returns>
        public MCustomerCare GetCustomerCare(int id)
        {
            if (id <= 0) return null;

            var strSql = new StringBuilder();
            strSql.Append(" SELECT [Id],[CompanyId],[CustomerId],[VisitName],[VisitTime],[PayMoney],[PayReason],[CustomerHobby],[OperatorId],[IssueTime] ");
            strSql.Append(" ,(select CustomerName from tbl_Customer as c where c.Id = CustomerId) as CustomerName ");
            strSql.Append(" FROM [tbl_CustomerCarefor] where Id = @Id ");

            DbCommand dc = _db.GetSqlStringCommand(strSql.ToString());
            _db.AddInParameter(dc, "Id", DbType.Int32, id);

            MCustomerCare model = null;
            using (IDataReader dr = DbHelper.ExecuteReader(dc, _db))
            {
                if (dr.Read())
                {
                    model = new MCustomerCare
                        {
                            CareId = dr.IsDBNull(dr.GetOrdinal("Id")) ? 0 : dr.GetInt32(dr.GetOrdinal("Id")),
                            CompanyId =
                                dr.IsDBNull(dr.GetOrdinal("CompanyId")) ? 0 : dr.GetInt32(dr.GetOrdinal("CompanyId")),
                            CustomerId =
                                dr.IsDBNull(dr.GetOrdinal("CustomerId"))
                                    ? string.Empty
                                    : dr.GetString(dr.GetOrdinal("CustomerId")),
                            VisitName =
                                dr.IsDBNull(dr.GetOrdinal("VisitName"))
                                    ? string.Empty
                                    : dr.GetString(dr.GetOrdinal("VisitName")),
                            PayMoney =
                                dr.IsDBNull(dr.GetOrdinal("PayMoney")) ? 0M : dr.GetDecimal(dr.GetOrdinal("PayMoney")),
                            PayReason =
                                dr.IsDBNull(dr.GetOrdinal("PayReason"))
                                    ? string.Empty
                                    : dr.GetString(dr.GetOrdinal("PayReason")),
                            CustomerHobby =
                                dr.IsDBNull(dr.GetOrdinal("CustomerHobby"))
                                    ? string.Empty
                                    : dr.GetString(dr.GetOrdinal("CustomerHobby")),
                            OperatorId =
                                dr.IsDBNull(dr.GetOrdinal("OperatorId")) ? 0 : dr.GetInt32(dr.GetOrdinal("OperatorId")),
                            CustomerName =
                                dr.IsDBNull(dr.GetOrdinal("CustomerName"))
                                    ? string.Empty
                                    : dr.GetString(dr.GetOrdinal("CustomerName"))
                        };
                    if (!dr.IsDBNull(dr.GetOrdinal("VisitTime"))) model.VisitTime = dr.GetDateTime(dr.GetOrdinal("VisitTime"));
                    if (!dr.IsDBNull(dr.GetOrdinal("IssueTime"))) model.IssueTime = dr.GetDateTime(dr.GetOrdinal("IssueTime"));
                }
            }

            return model;
        }

        /// <summary>
        /// 获取客户关怀
        /// </summary>
        /// <param name="companyId">公司编号</param>
        /// <param name="pageSize">每页记录数</param>
        /// <param name="pageIndex">当前页数</param>
        /// <param name="recordCount">总记录数</param>
        /// <param name="seach">客户关怀查询实体</param>
        /// <returns></returns>
        public IList<MCustomerCare> GetCustomerCare(int companyId, int pageSize, int pageIndex, ref int recordCount
            , MSearchCustomerCare seach)
        {
            if (companyId <= 0 || pageSize <= 0 || pageIndex <= 0) return null;

            IList<MCustomerCare> list;
            string tableName = "tbl_CustomerCarefor";
            var fileds = new StringBuilder("[Id],[CompanyId],[CustomerId],[VisitName],[VisitTime],[PayMoney],[PayReason],[CustomerHobby],[OperatorId],[IssueTime]");
            fileds.Append(" ,(select CustomerName from tbl_Customer as c where c.Id = CustomerId) as CustomerName ");
            fileds.Append(" ,(select * from tbl_CustomerContactInfo as b where b.CustomerId =tbl_CustomerCarefor.CustomerId for xml raw,root('Root')) as CustomerContact ");
            string orderbyStr = " IssueTime desc ";
            var strWhere = new StringBuilder();
            strWhere.AppendFormat(" CompanyId = {0} ", companyId);

            if (seach != null)
            {
                if (!string.IsNullOrEmpty(seach.VisitName))
                {
                    strWhere.AppendFormat(" and VisitName like '%{0}%' ", Utils.ToSqlLike(seach.VisitName));
                }
                if (!string.IsNullOrEmpty(seach.CustomerName))
                {
                    strWhere.AppendFormat(
                        " and exists (select 1 from tbl_Customer as d where d.Id = CustomerId and d.CustomerName like '%{0}%') ",
                        Utils.ToSqlLike(seach.CustomerName));
                }
                if (seach.StartVisitTime.HasValue)
                {
                    strWhere.AppendFormat(
                        " and datediff(dd,'{0}',VisitTime) >= 0 ", seach.StartVisitTime.Value.ToShortDateString());
                }
                if (seach.EndVisitTime.HasValue)
                {
                    strWhere.AppendFormat(
                        " and datediff(dd,'{0}',VisitTime) <= 0 ", seach.EndVisitTime.Value.ToShortDateString());
                }
            }

            using (IDataReader dr = DbHelper.ExecuteReader1(_db, pageSize, pageIndex, ref recordCount, tableName, fileds.ToString()
                , strWhere.ToString(), orderbyStr, string.Empty))
            {
                list = new List<MCustomerCare>();
                while (dr.Read())
                {
                    var model = new MCustomerCare
                        {
                            CareId = dr.IsDBNull(dr.GetOrdinal("Id")) ? 0 : dr.GetInt32(dr.GetOrdinal("Id")),
                            CompanyId =
                                dr.IsDBNull(dr.GetOrdinal("CompanyId")) ? 0 : dr.GetInt32(dr.GetOrdinal("CompanyId")),
                            CustomerId =
                                dr.IsDBNull(dr.GetOrdinal("CustomerId"))
                                    ? string.Empty
                                    : dr.GetString(dr.GetOrdinal("CustomerId")),
                            VisitName =
                                dr.IsDBNull(dr.GetOrdinal("VisitName"))
                                    ? string.Empty
                                    : dr.GetString(dr.GetOrdinal("VisitName")),
                            PayMoney =
                                dr.IsDBNull(dr.GetOrdinal("PayMoney")) ? 0M : dr.GetDecimal(dr.GetOrdinal("PayMoney")),
                            PayReason =
                                dr.IsDBNull(dr.GetOrdinal("PayReason"))
                                    ? string.Empty
                                    : dr.GetString(dr.GetOrdinal("PayReason")),
                            CustomerHobby =
                                dr.IsDBNull(dr.GetOrdinal("CustomerHobby"))
                                    ? string.Empty
                                    : dr.GetString(dr.GetOrdinal("CustomerHobby")),
                            OperatorId =
                                dr.IsDBNull(dr.GetOrdinal("OperatorId")) ? 0 : dr.GetInt32(dr.GetOrdinal("OperatorId")),
                            CustomerName =
                                dr.IsDBNull(dr.GetOrdinal("CustomerName"))
                                    ? string.Empty
                                    : dr.GetString(dr.GetOrdinal("CustomerName"))
                        };
                    if (!dr.IsDBNull(dr.GetOrdinal("VisitTime"))) model.VisitTime = dr.GetDateTime(dr.GetOrdinal("VisitTime"));
                    if (!dr.IsDBNull(dr.GetOrdinal("IssueTime"))) model.IssueTime = dr.GetDateTime(dr.GetOrdinal("IssueTime"));
                    if (!dr.IsDBNull(dr.GetOrdinal("CustomerContact")))
                        model.Contact = this.GetCustomerContact(dr.GetString(dr.GetOrdinal("CustomerContact")));

                    list.Add(model);
                }
            }

            return list;
        }

        /// <summary>
        /// 根据客户联系人sqlxml生成联系人信息集合
        /// </summary>
        /// <param name="xml">客户联系人sqlxml</param>
        /// <returns>联系人信息集合</returns>
        private IList<MCustomerContact> GetCustomerContact(string xml)
        {
            if (string.IsNullOrEmpty(xml)) return null;

            IList<MCustomerContact> list;

            var xRoot = XElement.Parse(xml);
            var xRows = Utils.GetXElements(xRoot, "row");
            if (xRows == null || !xRows.Any()) return null;

            list = new List<MCustomerContact>();
            foreach (var t in xRows)
            {
                if (t == null) continue;

                list.Add(
                    new MCustomerContact
                        {
                            BirthDay = Utils.GetDateTimeNullable(Utils.GetXAttributeValue(t, "BirthDay")),
                            CompanyId = Utils.GetInt(Utils.GetXAttributeValue(t, "CompanyId")),
                            CustomerId = Utils.GetXAttributeValue(t, "CustomerId"),
                            DepartmentName = Utils.GetXAttributeValue(t, "DepartmentId"),
                            Email = Utils.GetXAttributeValue(t, "Email"),
                            Fax = Utils.GetXAttributeValue(t, "Fax"),
                            Id = Utils.GetInt(Utils.GetXAttributeValue(t, "Id")),
                            Job = Utils.GetXAttributeValue(t, "JobId"),
                            Mobile = Utils.GetXAttributeValue(t, "Mobile"),
                            Name = Utils.GetXAttributeValue(t, "Name"),
                            Qq = Utils.GetXAttributeValue(t, "qq"),
                            Remark = Utils.GetXAttributeValue(t, "Remark"),
                            Sex = (Model.EnumType.CompanyStructure.Sex)Utils.GetInt(Utils.GetXAttributeValue(t, "Sex")),
                            Tel = Utils.GetXAttributeValue(t, "Tel")
                        });
            }

            return list;
        }
    }
}
