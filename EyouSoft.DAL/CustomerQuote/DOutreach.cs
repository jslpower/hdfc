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
    /// 外联每日足迹数据访问
    /// </summary>
    public class DOutreach : DALBase, IDAL.CustomerQuote.IOutreach
    {
        private readonly Database _db;

        public DOutreach()
        {
            _db = this.SystemStore;
        }

        /// <summary>
        /// 添加外联每日足迹
        /// </summary>
        /// <param name="model">外联每日足迹实体</param>
        /// <returns>
        /// 1：成功；
        /// 0：参数错误；
        /// -1：添加失败；
        /// </returns>
        public int AddOutreach(MOutreach model)
        {
            if (model == null || model.CompanyId <= 0 || string.IsNullOrEmpty(model.SaleUnitId)) return 0;

            var strSql = new StringBuilder();

            strSql.Append(
                @" INSERT INTO [tbl_Outreach] ([CompanyId] ,[SaleDate] ,[SaleUnit] ,[SaleName] ,[Tel] ,[Address] ,[Remark]
                        ,[OperatorId],[IssueTime],[SaleUnitId]) 
                    VALUES (@CompanyId ,@SaleDate ,@SaleUnit ,@SaleName ,@Tel ,@Address ,@Remark,@OperatorId,@IssueTime,@SaleUnitId); ");
            strSql.Append(" select @@Identity; ");

            DbCommand dc = _db.GetSqlStringCommand(strSql.ToString());
            _db.AddInParameter(dc, "CompanyId", DbType.Int32, model.CompanyId);
            _db.AddInParameter(dc, "SaleDate", DbType.DateTime, model.SaleDate);
            _db.AddInParameter(dc, "SaleUnit", DbType.String, model.SaleUnit);
            _db.AddInParameter(dc, "SaleName", DbType.String, model.SaleName);
            _db.AddInParameter(dc, "Tel", DbType.String, model.Tel);
            _db.AddInParameter(dc, "Address", DbType.String, model.Address);
            _db.AddInParameter(dc, "Remark", DbType.String, model.Remark);
            _db.AddInParameter(dc, "OperatorId", DbType.Int32, model.OperatorId);
            _db.AddInParameter(dc, "IssueTime", DbType.DateTime, model.IssueTime);
            _db.AddInParameter(dc, "SaleUnitId", DbType.AnsiStringFixedLength, model.SaleUnitId);

            object obj = DbHelper.GetSingleBySqlTrans(dc, _db);

            if (obj == null) return -1;

            int id = Utils.GetInt(obj.ToString());

            if (id <= 0) return -1;

            model.Id = id;
            return 1;
        }

        /// <summary>
        /// 修改外联每日足迹
        /// </summary>
        /// <param name="model">外联每日足迹实体</param>
        /// <returns>
        /// 1：成功；
        /// 0：参数错误；
        /// -1：修改失败；
        /// </returns>
        public int UpdateOutreach(MOutreach model)
        {
            if (model == null || model.Id <= 0 || string.IsNullOrEmpty(model.SaleUnitId)) return 0;

            var strSql = new StringBuilder();

            strSql.Append(
                @" UPDATE [tbl_Outreach] SET [SaleDate] = @SaleDate,[SaleUnit] = @SaleUnit,[SaleName] = @SaleName,[Tel] = @Tel,[Address] = @Address,[Remark] = @Remark,[OperatorId] = @OperatorId,[SaleUnitId] = @SaleUnitId WHERE Id = @Id; ");

            DbCommand dc = _db.GetSqlStringCommand(strSql.ToString());
            _db.AddInParameter(dc, "Id", DbType.Int32, model.Id);
            _db.AddInParameter(dc, "SaleDate", DbType.DateTime, model.SaleDate);
            _db.AddInParameter(dc, "SaleUnit", DbType.String, model.SaleUnit);
            _db.AddInParameter(dc, "SaleName", DbType.String, model.SaleName);
            _db.AddInParameter(dc, "Tel", DbType.String, model.Tel);
            _db.AddInParameter(dc, "Address", DbType.String, model.Address);
            _db.AddInParameter(dc, "Remark", DbType.String, model.Remark);
            _db.AddInParameter(dc, "OperatorId", DbType.Int32, model.OperatorId);
            _db.AddInParameter(dc, "SaleUnitId", DbType.AnsiStringFixedLength, model.SaleUnitId);

            return DbHelper.ExecuteSql(dc, _db) > 0 ? 1 : -1;
        }

        /// <summary>
        /// 删除外联每日足迹
        /// </summary>
        /// <param name="id">外联每日足迹编号</param>
        /// <returns>
        /// 1：成功；
        /// 0：参数错误；
        /// -1：删除失败；
        /// </returns>
        public int DeleteOutreach(params int[] id)
        {
            if (id == null || id.Length <= 0) return 0;

            var strSql = new StringBuilder();

            strSql.Append(" delete from tbl_Outreach where ");
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
        /// 获取外联每日足迹
        /// </summary>
        /// <param name="id">外联每日足迹编号</param>
        /// <returns></returns>
        public MOutreach GetOutreach(int id)
        {
            if (id <= 0) return null;

            var strSql = new StringBuilder();

            strSql.Append(
                " SELECT [Id],[CompanyId],[SaleDate],[SaleUnit],[SaleName],[Tel],[Address],[Remark],[OperatorId],[IssueTime],[SaleUnitId] FROM [tbl_Outreach] where Id = @Id; ");

            DbCommand dc = _db.GetSqlStringCommand(strSql.ToString());
            _db.AddInParameter(dc, "Id", DbType.Int32, id);

            MOutreach model = null;
            using (IDataReader dr = DbHelper.ExecuteReader(dc, _db))
            {
                if (dr.Read())
                {
                    model = new MOutreach
                        {
                            Address =
                                dr.IsDBNull(dr.GetOrdinal("Address"))
                                    ? string.Empty
                                    : dr.GetString(dr.GetOrdinal("Address")),
                            CompanyId =
                                dr.IsDBNull(dr.GetOrdinal("CompanyId")) ? 0 : dr.GetInt32(dr.GetOrdinal("CompanyId")),
                            Id = dr.IsDBNull(dr.GetOrdinal("Id")) ? 0 : dr.GetInt32(dr.GetOrdinal("Id")),
                            Remark =
                                dr.IsDBNull(dr.GetOrdinal("Remark"))
                                    ? string.Empty
                                    : dr.GetString(dr.GetOrdinal("Remark")),
                            SaleName =
                                dr.IsDBNull(dr.GetOrdinal("SaleName"))
                                    ? string.Empty
                                    : dr.GetString(dr.GetOrdinal("SaleName")),
                            SaleUnit =
                                dr.IsDBNull(dr.GetOrdinal("SaleUnit"))
                                    ? string.Empty
                                    : dr.GetString(dr.GetOrdinal("SaleUnit")),
                            Tel = dr.IsDBNull(dr.GetOrdinal("Tel")) ? string.Empty : dr.GetString(dr.GetOrdinal("Tel")),
                            OperatorId =
                                dr.IsDBNull(dr.GetOrdinal("OperatorId")) ? 0 : dr.GetInt32(dr.GetOrdinal("OperatorId")),
                            SaleUnitId =
                                dr.IsDBNull(dr.GetOrdinal("SaleUnitId"))
                                    ? string.Empty
                                    : dr.GetString(dr.GetOrdinal("SaleUnitId"))
                        };
                    if (!dr.IsDBNull(dr.GetOrdinal("SaleDate"))) model.SaleDate = dr.GetDateTime(dr.GetOrdinal("SaleDate"));
                    if (!dr.IsDBNull(dr.GetOrdinal("IssueTime"))) model.IssueTime = dr.GetDateTime(dr.GetOrdinal("IssueTime"));
                }
            }

            return model;
        }

        /// <summary>
        /// 获取外联每日足迹
        /// </summary>
        /// <param name="companyId">公司编号</param>
        /// <param name="pageSize">每页记录数</param>
        /// <param name="pageIndex">当前页数</param>
        /// <param name="recordCount">总记录数</param>
        /// <param name="seach">外联每日足迹查询实体</param>
        /// <returns></returns>
        public IList<MOutreach> GetOutreach(int companyId, int pageSize, int pageIndex, ref int recordCount, MSearchOutreach seach)
        {
            if (companyId <= 0 || pageSize <= 0 || pageIndex <= 0) return null;

            IList<MOutreach> list;
            string tableName = "tbl_Outreach";
            string fileds = "[Id],[CompanyId],[SaleDate],[SaleUnit],[SaleName],[Tel],[Address],[Remark],[OperatorId],[IssueTime],[SaleUnitId]";
            string orderbyStr = " Id desc ";
            var strWhere = new StringBuilder();
            strWhere.AppendFormat(" CompanyId = {0} ", companyId);
            if (seach != null)
            {
                if (!string.IsNullOrEmpty(seach.SaleUnitName))
                {
                    strWhere.AppendFormat(" and SaleUnit like '%{0}%' ", Utils.ToSqlLike(seach.SaleUnitName));
                }
                if (seach.StartSaleTime.HasValue)
                {
                    strWhere.AppendFormat(
                        " and datediff(dd,'{0}',SaleDate) >= 0 ", seach.StartSaleTime.Value.ToShortDateString());
                }
                if (seach.EndSaleTime.HasValue)
                {
                    strWhere.AppendFormat(
                        " and datediff(dd,'{0}',SaleDate) <= 0 ", seach.EndSaleTime.Value.ToShortDateString());
                }
                if (!string.IsNullOrEmpty(seach.SaleUnitId))
                {
                    strWhere.AppendFormat(" and SaleUnitId = '{0}' ", Utils.ToSqlLike(seach.SaleUnitId));
                }
            }

            using (IDataReader dr = DbHelper.ExecuteReader1(_db, pageSize, pageIndex, ref recordCount, tableName, fileds
                , strWhere.ToString(), orderbyStr, string.Empty))
            {
                list = new List<MOutreach>();
                while (dr.Read())
                {
                    var model = new MOutreach
                    {
                        Address =
                            dr.IsDBNull(dr.GetOrdinal("Address"))
                                ? string.Empty
                                : dr.GetString(dr.GetOrdinal("Address")),
                        CompanyId =
                            dr.IsDBNull(dr.GetOrdinal("CompanyId")) ? 0 : dr.GetInt32(dr.GetOrdinal("CompanyId")),
                        Id = dr.IsDBNull(dr.GetOrdinal("Id")) ? 0 : dr.GetInt32(dr.GetOrdinal("Id")),
                        Remark =
                            dr.IsDBNull(dr.GetOrdinal("Remark"))
                                ? string.Empty
                                : dr.GetString(dr.GetOrdinal("Remark")),
                        SaleName =
                            dr.IsDBNull(dr.GetOrdinal("SaleName"))
                                ? string.Empty
                                : dr.GetString(dr.GetOrdinal("SaleName")),
                        SaleUnit =
                            dr.IsDBNull(dr.GetOrdinal("SaleUnit"))
                                ? string.Empty
                                : dr.GetString(dr.GetOrdinal("SaleUnit")),
                        Tel = dr.IsDBNull(dr.GetOrdinal("Tel")) ? string.Empty : dr.GetString(dr.GetOrdinal("Tel")),
                        OperatorId = dr.IsDBNull(dr.GetOrdinal("OperatorId")) ? 0 : dr.GetInt32(dr.GetOrdinal("OperatorId")),
                        SaleUnitId =
                                dr.IsDBNull(dr.GetOrdinal("SaleUnitId"))
                                    ? string.Empty
                                    : dr.GetString(dr.GetOrdinal("SaleUnitId"))
                    };
                    if (!dr.IsDBNull(dr.GetOrdinal("SaleDate"))) model.SaleDate = dr.GetDateTime(dr.GetOrdinal("SaleDate"));
                    if (!dr.IsDBNull(dr.GetOrdinal("IssueTime"))) model.IssueTime = dr.GetDateTime(dr.GetOrdinal("IssueTime"));

                    list.Add(model);
                }
            }

            return list;
        }
    }
}
