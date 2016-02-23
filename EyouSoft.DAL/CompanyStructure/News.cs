using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Practices.EnterpriseLibrary.Data;
using System.Data.Common;
using System.Data;
using EyouSoft.Toolkit.DAL;

namespace EyouSoft.DAL.CompanyStructure
{
    /// <summary>
    /// 系统设置-信息管理
    /// </summary>
    public class News : DALBase, IDAL.CompanyStructure.INews
    {
        private readonly Database _db;

        private const string SqlSetclicks = " update tbl_News set Views = isnull(Views,0) + 1 where ID = @ID";

        public News()
        {
            this._db = SystemStore;
        }

        /// <summary>
        /// 添加
        /// </summary>
        /// <param name="model">公告信息实体</param>
        /// <returns>true:成功 false:失败</returns>
        public int Add(Model.CompanyStructure.News model)
        {
            if (model == null || model.CompanyId <= 0 || string.IsNullOrEmpty(model.Title)) return 0;

            var strSql = new StringBuilder();
            strSql.Append(" declare @newsId int; ");
            strSql.Append(
                " INSERT INTO [tbl_News] ([CompanyId],[Title],[OperatorId],[IssueTime],[Content],[Views],[Files],[IsDelete])  VALUES  (@CompanyId,@Title,@OperatorId,@IssueTime,@Content,@Views,@Files,@IsDelete); ");
            strSql.Append(" select @newsId = @@identity; ");
            if (model.AcceptList != null && model.AcceptList.Any())
            {
                foreach (var t in model.AcceptList)
                {
                    if (t == null) continue;

                    strSql.AppendFormat(
                        " INSERT INTO [tbl_NewsAccept]([NewId],[AcceptType],[AcceptId]) VALUES (@newsId,{0},{1}); ",
                        (int)t.AcceptType,
                        t.AcceptId);
                }
            }

            strSql.Append(" select @newsId; ");

            DbCommand dc = _db.GetSqlStringCommand(strSql.ToString());
            _db.AddInParameter(dc, "CompanyId", DbType.Int32, model.CompanyId);
            _db.AddInParameter(dc, "Title", DbType.String, model.Title);
            _db.AddInParameter(dc, "OperatorId", DbType.Int32, model.OperatorId);
            _db.AddInParameter(dc, "IssueTime", DbType.DateTime, DateTime.Now);
            _db.AddInParameter(dc, "Content", DbType.String, model.Content);
            _db.AddInParameter(dc, "Views", DbType.Int32, model.Clicks <= 0 ? 0 : model.Clicks);
            _db.AddInParameter(dc, "Files", DbType.String, model.UploadFiles);
            _db.AddInParameter(dc, "IsDelete", DbType.AnsiStringFixedLength, "0");

            object obj = DbHelper.GetSingle(dc, _db);
            if (obj == null || Toolkit.Utils.GetInt(obj.ToString()) <= 0)
            {
                model.ID = 0;
                return -1;
            }

            model.ID = Toolkit.Utils.GetInt(obj.ToString());
            return 1;
        }

        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="model">公告信息实体</param>
        /// <returns>true:成功 false:失败</returns>
        public int Update(Model.CompanyStructure.News model)
        {
            if (model == null || model.ID <= 0 || model.CompanyId <= 0 || string.IsNullOrEmpty(model.Title)) return 0;

            var strSql = new StringBuilder();
            strSql.Append(" declare @oldFiles nvarchar(255); ");
            strSql.Append(" select @oldFiles = [Files] from [tbl_News] where Id = @Id; ");
            strSql.Append(
                " UPDATE [tbl_News] SET [Title] = @Title,[OperatorId] = @OperatorId,[Content] = @Content,[Views] = @Views,[Files] = @Files WHERE Id = @Id; ");
            if (model.AcceptList != null && model.AcceptList.Any())
            {
                strSql.Append(" delete from [tbl_NewsAccept] where [NewId] = @Id; ");
                foreach (var t in model.AcceptList)
                {
                    if (t == null) continue;

                    strSql.AppendFormat(
                        " INSERT INTO [tbl_NewsAccept]([NewId],[AcceptType],[AcceptId]) VALUES (@Id,{0},{1}); ",
                        (int)t.AcceptType,
                        t.AcceptId);
                }
            }

            strSql.Append(" if @oldFiles is not null and @oldFiles <> @Files ");
            strSql.Append(" begin ");
            strSql.Append(" insert into tbl_SysDeletedFileQue (FilePath) values (@oldFiles); ");
            strSql.Append(" end ");

            DbCommand dc = _db.GetSqlStringCommand(strSql.ToString());
            _db.AddInParameter(dc, "Title", DbType.String, model.Title);
            _db.AddInParameter(dc, "OperatorId", DbType.Int32, model.OperatorId);
            _db.AddInParameter(dc, "Content", DbType.String, model.Content);
            _db.AddInParameter(dc, "Views", DbType.Int32, model.Clicks <= 0 ? 0 : model.Clicks);
            _db.AddInParameter(dc, "Files", DbType.String, model.UploadFiles);
            _db.AddInParameter(dc, "Id", DbType.Int32, model.ID);

            return DbHelper.ExecuteSqlTrans(dc, _db) > 0 ? 1 : -1;
        }

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="ids">主键编号集合</param>
        /// <returns>true:成功 false:失败</returns>
        public int Delete(params int[] ids)
        {
            if (ids == null || ids.Length <= 0)
                return 0;

            var strSql = new StringBuilder(" update tbl_News set IsDelete = '1' where ");
            if (ids.Length == 1)
            {
                strSql.AppendFormat(" ID = {0} ", ids[0]);
            }
            else
            {
                strSql.AppendFormat(" ID in ({0}) ", GetIdsByArr(ids));
            }

            DbCommand dc = _db.GetSqlStringCommand(strSql.ToString());

            return DbHelper.ExecuteSql(dc, this._db) > 0 ? 1 : -1;
        }

        /// <summary>
        /// 获取公告信息实体
        /// </summary>
        /// <param name="id">主键编号</param>
        /// <returns></returns>
        public Model.CompanyStructure.News GetModel(int id)
        {
            if (id <= 0) return null;

            Model.CompanyStructure.News model = null;
            var strSql = new StringBuilder(" select *,(select ContactName from tbl_CompanyUser where tbl_CompanyUser.Id = tbl_News.OperatorId) as OperatorName from tbl_News ");
            strSql.Append(" where Id = @Id; ");
            strSql.Append(" select AcceptType,AcceptId from tbl_NewsAccept where NewId = @Id; ");

            DbCommand dc = _db.GetSqlStringCommand(strSql.ToString());
            _db.AddInParameter(dc, "Id", DbType.Int32, id);

            using (IDataReader dr = DbHelper.ExecuteReader(dc, _db))
            {

                if (dr.Read())
                {
                    model = new Model.CompanyStructure.News
                        {
                            ID = id,
                            CompanyId =
                                dr.IsDBNull(dr.GetOrdinal("CompanyId")) ? 0 : dr.GetInt32(dr.GetOrdinal("CompanyId")),
                            Title =
                                dr.IsDBNull(dr.GetOrdinal("Title")) ? string.Empty : dr.GetString(dr.GetOrdinal("Title")),
                            OperatorId =
                                dr.IsDBNull(dr.GetOrdinal("OperatorId")) ? 0 : dr.GetInt32(dr.GetOrdinal("OperatorId")),
                            OperatorName =
                                dr.IsDBNull(dr.GetOrdinal("OperatorName"))
                                    ? string.Empty
                                    : dr.GetString(dr.GetOrdinal("OperatorName")),
                            IssueTime =
                                dr.IsDBNull(dr.GetOrdinal("IssueTime"))
                                    ? DateTime.MinValue
                                    : dr.GetDateTime(dr.GetOrdinal("IssueTime")),
                            Content =
                                dr.IsDBNull(dr.GetOrdinal("Content"))
                                    ? string.Empty
                                    : dr.GetString(dr.GetOrdinal("Content")),
                            Clicks = dr.IsDBNull(dr.GetOrdinal("Views")) ? 0 : dr.GetInt32(dr.GetOrdinal("Views")),
                            UploadFiles =
                                dr.IsDBNull(dr.GetOrdinal("Files")) ? string.Empty : dr.GetString(dr.GetOrdinal("Files")),
                            IsDelete =
                                dr.IsDBNull(dr.GetOrdinal("IsDelete"))
                                    ? false
                                    : this.GetBoolean(dr.GetString(dr.GetOrdinal("IsDelete")))
                        };
                }

                if (model != null)
                {
                    dr.NextResult();

                    model.AcceptList = new List<Model.CompanyStructure.NewsAccept>();
                    while (dr.Read())
                    {
                        model.AcceptList.Add(
                            new Model.CompanyStructure.NewsAccept
                                {
                                    NewId = id,
                                    AcceptId =
                                        dr.IsDBNull(dr.GetOrdinal("AcceptId"))
                                            ? 0
                                            : dr.GetInt32(dr.GetOrdinal("AcceptId")),
                                    AcceptType =
                                        dr.IsDBNull(dr.GetOrdinal("AcceptType"))
                                            ? Model.EnumType.CompanyStructure.AcceptType.所有
                                            : (Model.EnumType.CompanyStructure.AcceptType)
                                              dr.GetByte(dr.GetOrdinal("AcceptType"))
                                });
                    }
                }
            }

            return model;
        }

        /// <summary>
        /// 设置点击次数
        /// </summary>
        /// <param name="id">主键编号</param>
        /// <returns>true:成功 false:失败</returns>
        public int SetClicks(int id)
        {
            if (id <= 0) return 0;

            DbCommand cmd = this._db.GetSqlStringCommand(SqlSetclicks);

            this._db.AddInParameter(cmd, "ID", DbType.Int32, id);

            return DbHelper.ExecuteSql(cmd, this._db) > 0 ? 1 : -1;
        }

        /// <summary>
        /// 分页获取公告信息列表
        /// </summary>
        /// <param name="pageSize"></param>
        /// <param name="pageIndex"></param>
        /// <param name="recordCount"></param>
        /// <param name="companyId"></param>
        /// <returns></returns>
        public IList<Model.CompanyStructure.News> GetList(
            int pageSize,
            int pageIndex,
            ref int recordCount,
            int companyId)
        {
            if (pageSize <= 0 || pageIndex <= 0 || companyId <= 0) return null;

            IList<Model.CompanyStructure.News> totals = new List<Model.CompanyStructure.News>();

            string tableName = "tbl_News";
            string orderByString = "IssueTime DESC";
            string fields = " Id, CompanyId, Title, OperatorId,IssueTime,[Content],[Views],Files,IsDelete,(select ContactName from tbl_CompanyUser where tbl_CompanyUser.Id = tbl_News.OperatorId) as OperatorName ";

            var cmdQuery = new StringBuilder(" IsDelete = '0' ");
            cmdQuery.AppendFormat(" and CompanyId = {0} ", companyId);

            using (IDataReader rdr = DbHelper.ExecuteReader1(this._db, pageSize, pageIndex, ref recordCount, tableName, fields
                , cmdQuery.ToString(), orderByString, string.Empty))
            {
                while (rdr.Read())
                {
                    var newInfo = new Model.CompanyStructure.News
                        {
                            ID = rdr.GetInt32(rdr.GetOrdinal("ID")),
                            CompanyId = rdr.GetInt32(rdr.GetOrdinal("CompanyId")),
                            Title = rdr.IsDBNull(rdr.GetOrdinal("Title")) ? " " : rdr.GetString(rdr.GetOrdinal("Title")),
                            OperatorId = rdr.GetInt32(rdr.GetOrdinal("OperatorId")),
                            OperatorName =
                                rdr.IsDBNull(rdr.GetOrdinal("OperatorName"))
                                    ? string.Empty
                                    : rdr.GetString(rdr.GetOrdinal("OperatorName")),
                            IssueTime = rdr.GetDateTime(rdr.GetOrdinal("IssueTime")),
                            Content =
                                rdr.IsDBNull(rdr.GetOrdinal("Content")) ? " " : rdr.GetString(rdr.GetOrdinal("Content")),
                            Clicks = rdr.IsDBNull(rdr.GetOrdinal("Views")) ? 0 : rdr.GetInt32(rdr.GetOrdinal("Views")),
                            UploadFiles =
                                rdr.IsDBNull(rdr.GetOrdinal("Files")) ? " " : rdr.GetString(rdr.GetOrdinal("Files")),
                            IsDelete = this.GetBoolean(rdr.GetString(rdr.GetOrdinal("IsDelete")))
                        };

                    totals.Add(newInfo);
                }
            }

            return totals;
        }

        /// <summary>
        /// 获取某个用户接收到的消息列表
        /// </summary>
        /// <param name="recordCount"></param>
        /// <param name="userId">用户编号</param>
        /// <param name="companyId">公司编号</param>
        /// <param name="pageSize"></param>
        /// <param name="pageIndex"></param>
        /// <returns></returns>
        public IList<Model.CompanyStructure.NoticeNews> GetAcceptNews(
            int pageSize,
            int pageIndex,
            ref int recordCount,
            int userId, int companyId)
        {
            if (pageSize <= 0 || pageIndex <= 0 || userId <= 0 || companyId <= 0) return null;

            IList<Model.CompanyStructure.NoticeNews> lsNews = new List<Model.CompanyStructure.NoticeNews>();

            string tableName = " SELECT a.ID, a.CompanyId, a.Title, a.Views, a.OperatorId, a.IssueTime, a.IsDelete,b.ContactName as OperatorName FROM tbl_News AS a LEFT JOIN tbl_CompanyUser AS b on a.OperatorId = b.Id ";
            string orderByString = "IssueTime DESC";
            string fields = " ID, Title,Views,OperatorId,IssueTime,OperatorName";

            var cmdQuery = new StringBuilder(" IsDelete = '0' ");
            cmdQuery.AppendFormat(" and CompanyId = {0} ", companyId);
            cmdQuery.Append(" and ( ");
            cmdQuery.AppendFormat(" OperatorId = {0} ", userId);
            cmdQuery.AppendFormat(
                " or exists (select 1 from tbl_NewsAccept as na where na.NewId = Id and (na.AcceptType = {0} or na.AcceptId = {1})) ",
                (int)Model.EnumType.CompanyStructure.AcceptType.所有,
                string.Format(" (select top 1 cu.DepartId from tbl_CompanyUser as cu where cu.Id = {0} ) ", userId));
            cmdQuery.Append(" ) ");

            using (IDataReader rdr = DbHelper.ExecuteReader2(this._db, pageSize, pageIndex, ref recordCount, tableName, fields
                , cmdQuery.ToString(), orderByString, string.Empty))
            {
                while (rdr.Read())
                {
                    var newInfo = new Model.CompanyStructure.NoticeNews
                        {
                            Id = rdr.GetInt32(rdr.GetOrdinal("ID")),
                            Title = rdr.IsDBNull(rdr.GetOrdinal("Title")) ? "" : rdr.GetString(rdr.GetOrdinal("Title")),
                            ClickNum = rdr.IsDBNull(rdr.GetOrdinal("Views")) ? 0 : rdr.GetInt32(rdr.GetOrdinal("Views")),
                            OperateName =
                                rdr.IsDBNull(rdr.GetOrdinal("OperatorName")) ? "" : rdr.GetString(rdr.GetOrdinal("OperatorName")),
                            IssueTime = rdr.GetDateTime(rdr.GetOrdinal("IssueTime"))
                        };

                    lsNews.Add(newInfo);
                }
            }

            return lsNews;
        }


        /// <summary>
        /// 阅读新闻
        /// </summary>
        /// <param name="newsId"></param>
        /// <param name="userId"></param>
        public void ReadNews(int newsId, int userId)
        {
            StringBuilder query = new StringBuilder();
            query.AppendFormat("if not exists(select 1 from tbl_NewsRead where ID=@Id and ReadId=@ReadId)");
            query.Append(" begin ");
            query.Append("  insert into tbl_NewsRead(ID,ReadId)values(@Id,@ReadId)");
            query.Append(" end ");
            DbCommand cmd = this._db.GetSqlStringCommand(query.ToString());
            this._db.AddInParameter(cmd, "Id", DbType.Int32, newsId);
            this._db.AddInParameter(cmd, "ReadId", DbType.Int32, userId);
            DbHelper.ExecuteSql(cmd, this._db);
        }

        /// <summary>
        /// 查询是否有未读的公告
        /// </summary>
        /// <param name="companyId"></param>
        /// <param name="departId"></param>
        /// <param name="UserId"></param>
        /// <returns></returns>
        public bool IsNews(int companyId, int departId, int UserId)
        {
            StringBuilder query = new StringBuilder();
            query.AppendFormat(
                "select 1 from tbl_News where companyId={0} and IsDelete='0' and OperatorId <> {1} ", companyId, UserId);
            query.AppendFormat(
                " and exists (select 1 from tbl_NewsAccept as na where na.NewId = tbl_News.Id and (na.AcceptType = {0} or na.AcceptId = {1})) ",
                (int)Model.EnumType.CompanyStructure.AcceptType.所有,
                string.Format(" (select top 1 cu.DepartId from tbl_CompanyUser as cu where cu.Id = {0} ) ", UserId));
            query.AppendFormat(" and not exists(select Id from  tbl_NewsRead where readId={0} and tbl_News.Id=tbl_NewsRead.Id)", UserId);

            DbCommand cmd = this._db.GetSqlStringCommand(query.ToString());
            using (IDataReader dr = DbHelper.ExecuteReader(cmd, this._db))
            {
                if (dr.Read())
                {
                    return true;
                }
            }
            return false;
        }
    }
}
