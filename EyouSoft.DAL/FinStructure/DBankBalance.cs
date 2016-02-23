using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.Common;
using EyouSoft.Toolkit;
using EyouSoft.Toolkit.DAL;
using EyouSoft.Model.FinStructure;
using Microsoft.Practices.EnterpriseLibrary.Data;

namespace EyouSoft.DAL.FinStructure
{
    /// <summary>
    /// 银行余额数据访问
    /// </summary>
    public class DBankBalance : DALBase, IDAL.FinStructure.IBankBalance
    {
        private readonly Database _db;

        public DBankBalance()
        {
            _db = this.SystemStore;
        }

        /// <summary>
        /// 添加银行明细
        /// </summary>
        /// <param name="model">银行明细实体</param>
        /// <returns>
        /// 1：成功；
        /// 0：参数错误；
        /// -1：添加失败；
        /// </returns>
        public int AddBankBalance(MBankBalance model)
        {
            if (model == null || string.IsNullOrEmpty(model.BankId) 
                || model.CompanyId <= 0 || model.OperatorId <= 0) return 0;

            model.IssueTime = DateTime.Now;
            var strSql = new StringBuilder();
            strSql.Append(" INSERT INTO [tbl_BankBalance] ([Date],[BankId],[Balance],[CompanyId],[OperatorId],[IssueTime]) VALUES (@Date,@BankId,@Balance,@CompanyId,@OperatorId,@IssueTime); ");
            strSql.Append(" select @@Identity; ");

            DbCommand dc = _db.GetSqlStringCommand(strSql.ToString());
            _db.AddInParameter(dc, "Date", DbType.DateTime, model.Date);
            _db.AddInParameter(dc, "BankId", DbType.AnsiStringFixedLength, model.BankId);
            _db.AddInParameter(dc, "Balance", DbType.Decimal, model.Balance);
            _db.AddInParameter(dc, "CompanyId", DbType.Int32, model.CompanyId);
            _db.AddInParameter(dc, "OperatorId", DbType.Int32, model.OperatorId);
            _db.AddInParameter(dc, "IssueTime", DbType.DateTime, model.IssueTime);

            object obj = DbHelper.GetSingleBySqlTrans(dc, _db);
            if (obj == null) return -1;
            int tmp = Utils.GetInt(obj.ToString());
            if (tmp > 0)
            {
                model.Id = tmp;
                return 1;
            }

            return -1;
        }


        /// <summary>
        /// 删除银行余额明细
        /// </summary>
        /// <param name="id"></param>
        /// <returns>1成功 0失败</returns>
        public int DeleteBankBalance(int id)
        {
            string sql = "delete from tbl_BankBalance where Id=@Id";
            DbCommand cmd = this._db.GetSqlStringCommand(sql);
            this._db.AddInParameter(cmd, "Id", DbType.Int32, id);
            return DbHelper.ExecuteSql(cmd, this._db) != 0 ? 1 : 0;
        }

        /// <summary>
        /// 获取银行明细列表
        /// </summary>
        /// <param name="companyId">公司编号</param>
        /// <param name="pageSize">每页记录数</param>
        /// <param name="pageIndex">当前页数</param>
        /// <param name="recordCount">总记录数</param>
        /// <param name="search">银行明细查询实体</param>
        /// <returns></returns>
        public IList<MBankBalance> GetBankBalance(int companyId, int pageSize, int pageIndex, ref int recordCount
            , MSearchBankBalance search, out decimal totalBalance)
        {
            totalBalance = 0;
            if (companyId <= 0 || pageSize <= 0 || pageIndex <= 0) return null;
            string tableName = "tbl_BankBalance";
            string fields = " [Id],[Date],[BankId],[Balance],[CompanyId],[OperatorId],[IssueTime] ";
            var strWhere = new StringBuilder();
            string orderByStr = " IssueTime desc ";
            fields += " ,(select BankName + '  ' + AccountName + '  ' + BankNo from tbl_CompanyAccount as ca where ca.Id = BankId) as BankName ";
            strWhere.AppendFormat(" CompanyId = {0} ", companyId);
            if (search != null)
            {
                if (search.StartDate.HasValue)
                {
                    strWhere.AppendFormat(
                        " and datediff(dd,'{0}',Date) >= 0 ", search.StartDate.Value.ToShortDateString());
                }
                if (search.EndDate.HasValue)
                {
                    strWhere.AppendFormat(
                        " and datediff(dd,'{0}',Date) <= 0 ", search.EndDate.Value.ToShortDateString());
                }
            }

            string SumString = "sum(Balance) as SumBalance";

            var list = new List<MBankBalance>();
            using (IDataReader dr = DbHelper.ExecuteReader1(_db, pageSize, pageIndex, ref recordCount, tableName, fields
                , strWhere.ToString(), orderByStr, SumString))
            {
                while (dr.Read())
                {
                    var tmp = new MBankBalance
                        {
                            Id = dr.IsDBNull(dr.GetOrdinal("Id")) ? 0 : dr.GetInt32(dr.GetOrdinal("Id")),
                            BankId =
                                dr.IsDBNull(dr.GetOrdinal("BankId"))
                                    ? string.Empty
                                    : dr.GetString(dr.GetOrdinal("BankId")),
                            Balance = dr.IsDBNull(dr.GetOrdinal("Balance")) ? 0M : dr.GetDecimal(dr.GetOrdinal("Balance")),
                            CompanyId =
                                dr.IsDBNull(dr.GetOrdinal("CompanyId")) ? 0 : dr.GetInt32(dr.GetOrdinal("CompanyId")),
                            OperatorId =
                                dr.IsDBNull(dr.GetOrdinal("OperatorId")) ? 0 : dr.GetInt32(dr.GetOrdinal("OperatorId")),
                            BankName =
                                dr.IsDBNull(dr.GetOrdinal("BankName"))
                                    ? string.Empty
                                    : dr.GetString(dr.GetOrdinal("BankName")),
                        };
                    if (!dr.IsDBNull(dr.GetOrdinal("Date"))) tmp.Date = dr.GetDateTime(dr.GetOrdinal("Date"));
                    if (!dr.IsDBNull(dr.GetOrdinal("IssueTime"))) tmp.IssueTime = dr.GetDateTime(dr.GetOrdinal("IssueTime"));

                    list.Add(tmp);
                }

                dr.NextResult();

                if (dr.Read())
                {
                    if (!dr.IsDBNull(0)) totalBalance = dr.GetDecimal(0);
                }
            }

            return list;
        }
    }
}
