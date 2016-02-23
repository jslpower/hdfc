using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using EyouSoft.Toolkit.DAL;
using EyouSoft.Toolkit;
using EyouSoft.Model.CustomerQuote;
using Microsoft.Practices.EnterpriseLibrary.Data;
using System.Data.Common;
using System.Data;


namespace EyouSoft.DAL.CustomerQuote
{
    /// <summary>
    /// 客户日常询价数据访问
    /// </summary>
    public class DCustomerQuote : DALBase, IDAL.CustomerQuote.ICustomerQuote
    {
        private readonly Database _db;

        public DCustomerQuote()
        {
            _db = this.SystemStore;
        }

        /// <summary>
        /// 添加客户日常询价
        /// </summary>
        /// <param name="model">客户日常询价实体</param>
        /// <returns>
        /// 1：成功；
        /// 0：参数错误；
        /// -1：添加失败；
        /// </returns>
        public int AddCustomerQuote(MCustomerQuote model)
        {
            if (model == null || model.CompanyId <= 0 || string.IsNullOrEmpty(model.CostomerId) || model.PeopleNum <= 0) return 0;

            model.IssueTime = DateTime.Now;
            var strSql = new StringBuilder();

            strSql.Append(
                @" INSERT INTO [tbl_CustomerQuote] ([CompanyId] ,[LeaveDate] ,[PeopleNum] ,[Costomer] ,[QuoteDate] ,[ContactName] 
                        ,[ContactTel] ,[ContactMobile] ,[ContactQQ] ,[Content] ,[OperatorId] ,[IssueTime],[CostomerId]) 
                    VALUES (@CompanyId ,@LeaveDate ,@PeopleNum ,@Costomer ,@QuoteDate ,@ContactName ,@ContactTel ,@ContactMobile 
                        ,@ContactQQ ,@Content ,@OperatorId ,@IssueTime,@CostomerId); ");
            strSql.Append(" select @@Identity; ");

            DbCommand dc = _db.GetSqlStringCommand(strSql.ToString());
            _db.AddInParameter(dc, "CompanyId", DbType.Int32, model.CompanyId);
            _db.AddInParameter(dc, "LeaveDate", DbType.DateTime, model.LeaveDate);
            _db.AddInParameter(dc, "PeopleNum", DbType.Int32, model.PeopleNum);
            _db.AddInParameter(dc, "Costomer", DbType.String, model.Costomer);
            if (model.QuoteDate.HasValue)
            {
                _db.AddInParameter(dc, "QuoteDate", DbType.DateTime, model.QuoteDate.Value);
            }
            else
            {
                _db.AddInParameter(dc, "QuoteDate", DbType.DateTime, DBNull.Value);
            }
            _db.AddInParameter(dc, "ContactName", DbType.String, model.ContactName);
            _db.AddInParameter(dc, "ContactTel", DbType.String, model.ContactTel);
            _db.AddInParameter(dc, "ContactMobile", DbType.String, model.ContactMobile);
            _db.AddInParameter(dc, "ContactQQ", DbType.String, model.ContactQQ);
            _db.AddInParameter(dc, "Content", DbType.String, model.Content);
            _db.AddInParameter(dc, "OperatorId", DbType.Int32, model.OperatorId);
            _db.AddInParameter(dc, "IssueTime", DbType.DateTime, model.IssueTime);
            _db.AddInParameter(dc, "CostomerId", DbType.AnsiStringFixedLength, model.CostomerId);

            object obj = DbHelper.GetSingleBySqlTrans(dc, _db);

            if (obj == null) return -1;

            int id = Utils.GetInt(obj.ToString());

            if (id <= 0) return -1;

            model.QuoteId = id;
            return 1;
        }

        /// <summary>
        /// 修改客户日常询价
        /// </summary>
        /// <param name="model">客户日常询价实体</param>
        /// <returns>
        /// 1：成功；
        /// 0：参数错误；
        /// -1：修改失败；
        /// </returns>
        public int UpdateCustomerQuote(MCustomerQuote model)
        {
            if (model == null || string.IsNullOrEmpty(model.CostomerId) || model.PeopleNum <= 0 || model.QuoteId <= 0) return 0;

            var strSql = new StringBuilder();
            strSql.Append(
                @" UPDATE [tbl_CustomerQuote] SET [LeaveDate] = @LeaveDate,[PeopleNum] = @PeopleNum,[Costomer] = @Costomer
                    ,[QuoteDate] = @QuoteDate,[ContactName] = @ContactName,[ContactTel] = @ContactTel,[ContactMobile] = @ContactMobile
                    ,[ContactQQ] = @ContactQQ,[Content] = @Content,[OperatorId] = @OperatorId,[CostomerId] = @CostomerId 
                    WHERE QuoteId = @QuoteId; ");

            DbCommand dc = _db.GetSqlStringCommand(strSql.ToString());
            _db.AddInParameter(dc, "QuoteId", DbType.Int32, model.QuoteId);
            _db.AddInParameter(dc, "LeaveDate", DbType.DateTime, model.LeaveDate);
            _db.AddInParameter(dc, "PeopleNum", DbType.Int32, model.PeopleNum);
            _db.AddInParameter(dc, "Costomer", DbType.String, model.Costomer);
            if (model.QuoteDate.HasValue)
            {
                _db.AddInParameter(dc, "QuoteDate", DbType.DateTime, model.QuoteDate.Value);
            }
            else
            {
                _db.AddInParameter(dc, "QuoteDate", DbType.DateTime, DBNull.Value);
            }
            _db.AddInParameter(dc, "ContactName", DbType.String, model.ContactName);
            _db.AddInParameter(dc, "ContactTel", DbType.String, model.ContactTel);
            _db.AddInParameter(dc, "ContactMobile", DbType.String, model.ContactMobile);
            _db.AddInParameter(dc, "ContactQQ", DbType.String, model.ContactQQ);
            _db.AddInParameter(dc, "Content", DbType.String, model.Content);
            _db.AddInParameter(dc, "OperatorId", DbType.Int32, model.OperatorId);
            _db.AddInParameter(dc, "CostomerId", DbType.AnsiStringFixedLength, model.CostomerId);

            return DbHelper.ExecuteSql(dc, _db) > 0 ? 1 : -1;
        }

        /// <summary>
        /// 删除客户日常询价
        /// </summary>
        /// <param name="id">客户日常询价编号</param>
        /// <returns>
        /// 1：成功；
        /// 0：参数错误；
        /// -1：删除失败；
        /// </returns>
        public int DeleteCustomerQuote(params int[] id)
        {
            if (id == null || id.Length <= 0) return 0;

            var strSql = new StringBuilder();

            strSql.Append(" delete from tbl_CustomerQuote where ");
            if (id.Length == 1)
            {
                strSql.AppendFormat(" QuoteId = {0} ", id[0]);
            }
            else
            {
                strSql.AppendFormat(" QuoteId in ({0}) ", this.GetIdsByArr(id));
            }

            DbCommand dc = _db.GetSqlStringCommand(strSql.ToString());

            return DbHelper.ExecuteSql(dc, _db) > 0 ? 1 : -1;
        }

        /// <summary>
        /// 获取客户日常询价
        /// </summary>
        /// <param name="id">客户日常询价编号</param>
        /// <returns></returns>
        public MCustomerQuote GetCustomerQuote(int id)
        {
            if (id <= 0) return null;

            var strSql = new StringBuilder();

            strSql.Append(" SELECT [QuoteId],[CompanyId],[LeaveDate],[PeopleNum],[Costomer],[QuoteDate],[ContactName],[ContactTel],[ContactMobile],[ContactQQ],[Content],[OperatorId],[IssueTime],[CostomerId] FROM [tbl_CustomerQuote] where QuoteId = @QuoteId; ");

            DbCommand dc = _db.GetSqlStringCommand(strSql.ToString());
            _db.AddInParameter(dc, "QuoteId", DbType.Int32, id);

            MCustomerQuote model = null;
            using (IDataReader dr = DbHelper.ExecuteReader(dc, _db))
            {
                if (dr.Read())
                {
                    model = new MCustomerQuote
                        {
                            CompanyId =
                                dr.IsDBNull(dr.GetOrdinal("CompanyId")) ? 0 : dr.GetInt32(dr.GetOrdinal("CompanyId")),
                            ContactMobile =
                                dr.IsDBNull(dr.GetOrdinal("ContactMobile"))
                                    ? string.Empty
                                    : dr.GetString(dr.GetOrdinal("ContactMobile")),
                            ContactName =
                                dr.IsDBNull(dr.GetOrdinal("ContactName"))
                                    ? string.Empty
                                    : dr.GetString(dr.GetOrdinal("ContactName")),
                            ContactQQ =
                                dr.IsDBNull(dr.GetOrdinal("ContactQQ"))
                                    ? string.Empty
                                    : dr.GetString(dr.GetOrdinal("ContactQQ")),
                            ContactTel =
                                dr.IsDBNull(dr.GetOrdinal("ContactTel"))
                                    ? string.Empty
                                    : dr.GetString(dr.GetOrdinal("ContactTel")),
                            Content =
                                dr.IsDBNull(dr.GetOrdinal("Content"))
                                    ? string.Empty
                                    : dr.GetString(dr.GetOrdinal("Content")),
                            Costomer =
                                dr.IsDBNull(dr.GetOrdinal("Costomer"))
                                    ? string.Empty
                                    : dr.GetString(dr.GetOrdinal("Costomer")),
                            OperatorId =
                                dr.IsDBNull(dr.GetOrdinal("OperatorId")) ? 0 : dr.GetInt32(dr.GetOrdinal("OperatorId")),
                            PeopleNum =
                                dr.IsDBNull(dr.GetOrdinal("PeopleNum")) ? 0 : dr.GetInt32(dr.GetOrdinal("PeopleNum")),
                            QuoteId = dr.IsDBNull(dr.GetOrdinal("QuoteId")) ? 0 : dr.GetInt32(dr.GetOrdinal("QuoteId")),
                            CostomerId =
                                dr.IsDBNull(dr.GetOrdinal("CostomerId"))
                                    ? string.Empty
                                    : dr.GetString(dr.GetOrdinal("CostomerId"))
                        };
                    if (!dr.IsDBNull(dr.GetOrdinal("IssueTime"))) model.IssueTime = dr.GetDateTime(dr.GetOrdinal("IssueTime"));
                    if (!dr.IsDBNull(dr.GetOrdinal("LeaveDate"))) model.LeaveDate = dr.GetDateTime(dr.GetOrdinal("LeaveDate"));
                    if (!dr.IsDBNull(dr.GetOrdinal("QuoteDate"))) model.QuoteDate = dr.GetDateTime(dr.GetOrdinal("QuoteDate"));
                }
            }

            return model;
        }

        /// <summary>
        /// 获取客户日常询价
        /// </summary>
        /// <param name="companyId">公司编号</param>
        /// <param name="pageSize">每页记录数</param>
        /// <param name="pageIndex">当前页数</param>
        /// <param name="recordCount">总记录数</param>
        /// <param name="seach">客户日常询价查询实体</param>
        /// <returns></returns>
        public IList<MCustomerQuote> GetCustomerQuote(int companyId, int pageSize, int pageIndex, ref int recordCount
            , MSearchCustomerQuote seach)
        {
            if (companyId <= 0 || pageSize <= 0 || pageIndex <= 0) return null;

            IList<MCustomerQuote> list;
            string tableName = "tbl_CustomerQuote";
            string fileds = "[QuoteId],[CompanyId],[LeaveDate],[PeopleNum],[Costomer],[QuoteDate],[ContactName],[ContactTel],[ContactMobile],[ContactQQ],[Content],[OperatorId],[IssueTime],[CostomerId],(select count(cq.QuoteId) from tbl_CustomerQuote as cq where cq.CompanyId = tbl_CustomerQuote.CompanyId and cq.CostomerId = tbl_CustomerQuote.CostomerId and year(cq.IssueTime) = year(getdate())) as YearQuoteCount";
            fileds += ",(select ContactName from tbl_CompanyUser as tcu where tcu.Id = tbl_CustomerQuote.OperatorId ) as OperatorName ";
            string orderbyStr = " IssueTime desc,QuoteId desc ";
            var strWhere = new StringBuilder();
            strWhere.AppendFormat(" CompanyId = {0} ", companyId);
            if (seach != null)
            {
                if (!string.IsNullOrEmpty(seach.QuoteUnitName))
                {
                    strWhere.AppendFormat(" and Costomer like '%{0}%' ", Utils.ToSqlLike(seach.QuoteUnitName));
                }
                if (seach.StartQuoteTime.HasValue)
                {
                    strWhere.AppendFormat(
                        " and datediff(dd,'{0}',QuoteDate) >= 0 ", seach.StartQuoteTime.Value.ToShortDateString());
                }
                if (seach.EndQuoteTime.HasValue)
                {
                    strWhere.AppendFormat(
                        " and datediff(dd,'{0}',QuoteDate) <= 0 ", seach.EndQuoteTime.Value.ToShortDateString());
                }
                if (!string.IsNullOrEmpty(seach.CostomerId))
                {
                    strWhere.AppendFormat(" and CostomerId = '{0}' ", Utils.ToSqlLike(seach.CostomerId));
                }
            }

            using (IDataReader dr = DbHelper.ExecuteReader1(_db, pageSize, pageIndex, ref recordCount, tableName, fileds
                , strWhere.ToString(), orderbyStr, string.Empty))
            {
                list = new List<MCustomerQuote>();
                while (dr.Read())
                {
                    var model = new MCustomerQuote
                        {
                            CompanyId =
                                dr.IsDBNull(dr.GetOrdinal("CompanyId")) ? 0 : dr.GetInt32(dr.GetOrdinal("CompanyId")),
                            ContactMobile =
                                dr.IsDBNull(dr.GetOrdinal("ContactMobile"))
                                    ? string.Empty
                                    : dr.GetString(dr.GetOrdinal("ContactMobile")),
                            ContactName =
                                dr.IsDBNull(dr.GetOrdinal("ContactName"))
                                    ? string.Empty
                                    : dr.GetString(dr.GetOrdinal("ContactName")),
                            ContactQQ =
                                dr.IsDBNull(dr.GetOrdinal("ContactQQ"))
                                    ? string.Empty
                                    : dr.GetString(dr.GetOrdinal("ContactQQ")),
                            ContactTel =
                                dr.IsDBNull(dr.GetOrdinal("ContactTel"))
                                    ? string.Empty
                                    : dr.GetString(dr.GetOrdinal("ContactTel")),
                            Content =
                                dr.IsDBNull(dr.GetOrdinal("Content"))
                                    ? string.Empty
                                    : dr.GetString(dr.GetOrdinal("Content")),
                            Costomer =
                                dr.IsDBNull(dr.GetOrdinal("Costomer"))
                                    ? string.Empty
                                    : dr.GetString(dr.GetOrdinal("Costomer")),
                            OperatorId =
                                dr.IsDBNull(dr.GetOrdinal("OperatorId")) ? 0 : dr.GetInt32(dr.GetOrdinal("OperatorId")),
                            PeopleNum =
                                dr.IsDBNull(dr.GetOrdinal("PeopleNum")) ? 0 : dr.GetInt32(dr.GetOrdinal("PeopleNum")),
                            QuoteId = dr.IsDBNull(dr.GetOrdinal("QuoteId")) ? 0 : dr.GetInt32(dr.GetOrdinal("QuoteId")),
                            YearQuoteCount =
                                dr.IsDBNull(dr.GetOrdinal("YearQuoteCount"))
                                    ? 0
                                    : dr.GetInt32(dr.GetOrdinal("YearQuoteCount")),
                            CostomerId =
                                dr.IsDBNull(dr.GetOrdinal("CostomerId"))
                                    ? string.Empty
                                    : dr.GetString(dr.GetOrdinal("CostomerId")),
                            Operator =
                        dr.IsDBNull(dr.GetOrdinal("OperatorName"))
                            ? string.Empty
                            : dr.GetString(dr.GetOrdinal("OperatorName"))
                        };
                    if (!dr.IsDBNull(dr.GetOrdinal("IssueTime"))) model.IssueTime = dr.GetDateTime(dr.GetOrdinal("IssueTime"));
                    if (!dr.IsDBNull(dr.GetOrdinal("LeaveDate"))) model.LeaveDate = dr.GetDateTime(dr.GetOrdinal("LeaveDate"));
                    if (!dr.IsDBNull(dr.GetOrdinal("QuoteDate"))) model.QuoteDate = dr.GetDateTime(dr.GetOrdinal("QuoteDate"));

                    list.Add(model);
                }
            }

            return list;
        }
    }
}
