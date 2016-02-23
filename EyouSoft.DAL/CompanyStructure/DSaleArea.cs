using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using EyouSoft.Model.CompanyStructure;
using Microsoft.Practices.EnterpriseLibrary.Data;
using System.Data.Common;
using EyouSoft.Toolkit.DAL;

namespace EyouSoft.DAL.CompanyStructure
{
    using System.Data;

    /// <summary>
    /// 销售地区数据层
    /// </summary>
    public class DSaleArea : DALBase, IDAL.CompanyStructure.ISaleArea
    {
        private readonly Database _db;

        #region 构造函数
        /// <summary>
        /// 构造函数
        /// </summary>
        public DSaleArea()
        {
            _db = this.SystemStore;
        }
        #endregion 构造函数

        /// <summary>
        /// 验证是否存在某名称的销售区域
        /// </summary>
        /// <param name="name">名称</param>
        /// <param name="id">要排除的销售区域编号</param>
        /// <param name="companyId">公司编号</param>
        /// <returns>返回true 存在；false 不存在</returns>
        public bool ExistsSaleAreaName(string name, int companyId, int id)
        {
            if (string.IsNullOrEmpty(name) || companyId <= 0) return false;

            var strSql = new StringBuilder();
            strSql.Append(
                " select count(*) from tbl_CompanySaleArea as a where a.SaleAreaName = @SaleAreaName and a.CompanyId = @CompanyId ");
            if (id > 0)
            {
                strSql.AppendFormat(" and a.SaleAreaId = {0} ", id);
            }

            DbCommand dc = _db.GetSqlStringCommand(strSql.ToString());
            _db.AddInParameter(dc, "SaleAreaName", DbType.String, name);
            _db.AddInParameter(dc, "CompanyId", DbType.Int32, companyId);

            object obj = DbHelper.GetSingle(dc, _db);
            if (obj == null) return false;

            if (Toolkit.Utils.GetInt(obj.ToString()) > 0) return true;

            return false;
        }

        /// <summary>
        /// 验证某销售地区是否可以删除
        /// </summary>
        /// <param name="id">要验证的销售区域编号</param>
        /// <returns>返回可以删除的Id集合</returns>
        public int[] ExistsDeleteSaleArea(params int[] id)
        {
            if (id == null || id.Length <= 0) return null;
            var strSql = new StringBuilder();
            strSql.Append(" select SaleAreadId from tbl_Customer where IsDelete = '0'  ");
            if (id.Length == 1)
            {
                strSql.AppendFormat(" and SaleAreadId = {0} ", id[0]);
            }
            else
            {
                strSql.AppendFormat(" and SaleAreadId in ({0}) ", GetIdsByArr(id));
            }

            IList<int> list = new List<int>();
            DbCommand dc = _db.GetSqlStringCommand(strSql.ToString());
            using (IDataReader dr = DbHelper.ExecuteReader(dc, _db))
            {
                while (dr.Read())
                {
                    if (!dr.IsDBNull(0)) list.Add(dr.GetInt32(0));
                }
            }

            if (!list.Any()) return id;

            return (from c in id where !list.Contains(c) select c).ToArray();
        }

        /// <summary>
        /// 添加销售地区
        /// </summary>
        /// <param name="model">销售地区实体</param>
        /// <returns>
        /// 1：成功；
        /// 0：参数错误；
        /// -1：名称已经存在；
        /// -2：新增失败（sql错误）
        /// </returns>
        public int AddSaleArea(MSaleArea model)
        {
            if (model == null || string.IsNullOrEmpty(model.SaleAreaName) || model.CompanyId <= 0) return 0;

            var strSql = new StringBuilder();
            strSql.Append(" declare @saleId int; ");
            strSql.Append(" set @saleId = -1; ");
            strSql.Append(" if not exists (select 1 from tbl_CompanySaleArea where tbl_CompanySaleArea.SaleAreaName = @SaleAreaName and tbl_CompanySaleArea.CompanyId = @CompanyId) ");
            strSql.Append(" begin ");
            strSql.Append(
                " insert into tbl_CompanySaleArea (SaleAreaName,CompanyId) values (@SaleAreaName,@CompanyId); ");
            strSql.Append(" set @saleId = @@identity; ");
            strSql.Append(" end ");
            strSql.Append(" select @saleId; ");

            DbCommand dc = _db.GetSqlStringCommand(strSql.ToString());
            _db.AddInParameter(dc, "SaleAreaName", DbType.String, model.SaleAreaName);
            _db.AddInParameter(dc, "CompanyId", DbType.Int32, model.CompanyId);

            object obj = DbHelper.GetSingleBySqlTrans(dc, _db);
            if (obj == null) return -2;

            int saleId = Toolkit.Utils.GetInt(obj.ToString(), -2);

            if (saleId > 0)
            {
                model.SaleAreaId = saleId;
                return 1;
            }

            return saleId;
        }

        /// <summary>
        /// 修改销售地区
        /// </summary>
        /// <param name="model">销售地区实体</param>
        /// <returns>
        /// 1：成功；
        /// 0：参数错误；
        /// -1：名称已经存在；
        /// -2：新增失败（sql错误）
        ///</returns>
        public int UpdateSaleArea(MSaleArea model)
        {
            if (model == null || string.IsNullOrEmpty(model.SaleAreaName) || model.CompanyId <= 0 || model.SaleAreaId <= 0) return 0;

            var strSql = new StringBuilder();
            strSql.Append(" declare @saleId int; ");
            strSql.Append(" set @saleId = -1; ");
            strSql.Append(" if not exists (select 1 from tbl_CompanySaleArea where tbl_CompanySaleArea.SaleAreaName = @SaleAreaName and tbl_CompanySaleArea.CompanyId = @CompanyId and tbl_CompanySaleArea.SaleAreaId <> @SaleAreaId) ");
            strSql.Append(" begin ");
            strSql.Append(
                " update tbl_CompanySaleArea set SaleAreaName = @SaleAreaName where SaleAreaId = @SaleAreaId ");
            strSql.Append(" set  @saleId = 1; ");
            strSql.Append(" end ");
            strSql.Append(" select @saleId; ");

            DbCommand dc = _db.GetSqlStringCommand(strSql.ToString());
            _db.AddInParameter(dc, "SaleAreaName", DbType.String, model.SaleAreaName);
            _db.AddInParameter(dc, "CompanyId", DbType.Int32, model.CompanyId);
            _db.AddInParameter(dc, "SaleAreaId", DbType.Int32, model.SaleAreaId);

            object obj = DbHelper.GetSingleBySqlTrans(dc, _db);
            if (obj == null) return -2;

            return Toolkit.Utils.GetInt(obj.ToString(), -2);
        }

        /// <summary>
        /// 删除销售地区
        /// </summary>
        /// <param name="id">销售地区编号</param>
        /// <returns>
        /// 1：成功；
        /// 0：参数错误；
        /// -3：删除失败（sql错误）
        /// </returns>
        public int DeleteSaleArea(params int[] id)
        {
            if (id == null || id.Length <= 0) return 0;

            var strSql = new StringBuilder();

            strSql.Append(" delete from tbl_CompanySaleArea where ");
            if (id.Length == 1)
            {
                strSql.AppendFormat(" SaleAreaId = {0} ", id[0]);
            }
            else
            {
                strSql.AppendFormat(" SaleAreaId in ({0}) ", GetIdsByArr(id));
            }

            DbCommand dc = _db.GetSqlStringCommand(strSql.ToString());

            return DbHelper.ExecuteSql(dc, _db) > 0 ? 1 : -3;
        }

        /// <summary>
        /// 获取销售地区信息
        /// </summary>
        /// <param name="id">销售地区编号</param>
        /// <returns></returns>
        public MSaleArea GetSaleArea(int id)
        {
            if (id <= 0) return null;

            DbCommand dc =
                _db.GetSqlStringCommand(
                    " select SaleAreaId,SaleAreaName,CompanyId from tbl_CompanySaleArea where SaleAreaId = @SaleAreaId ");

            _db.AddInParameter(dc, "SaleAreaId", DbType.Int32, id);

            MSaleArea model = null;
            using (IDataReader dr = DbHelper.ExecuteReader(dc, _db))
            {
                if (dr.Read())
                {
                    model = new MSaleArea
                        {
                            CompanyId =
                                dr.IsDBNull(dr.GetOrdinal("CompanyId")) ? 0 : dr.GetInt32(dr.GetOrdinal("CompanyId")),
                            SaleAreaId =
                                dr.IsDBNull(dr.GetOrdinal("SaleAreaId")) ? 0 : dr.GetInt32(dr.GetOrdinal("SaleAreaId")),
                            SaleAreaName =
                                dr.IsDBNull(dr.GetOrdinal("SaleAreaName"))
                                    ? string.Empty
                                    : dr.GetString(dr.GetOrdinal("SaleAreaName"))
                        };
                }
            }

            return model;
        }

        /// <summary>
        /// 获取销售地区信息
        /// </summary>
        /// <param name="seach">销售地区查询实体</param>
        /// <param name="companyId"></param>
        /// <returns></returns>
        public IList<MSaleArea> GetSaleArea(int companyId, MSearchSaleArea seach)
        {
            var strSql = new StringBuilder(" select SaleAreaId,SaleAreaName,CompanyId from tbl_CompanySaleArea ");
            strSql.AppendFormat(" where CompanyId = {0} ", companyId);
            if (seach != null)
            {

            }

            DbCommand dc = _db.GetSqlStringCommand(strSql.ToString());

            IList<MSaleArea> list = new List<MSaleArea>();
            using (IDataReader dr = DbHelper.ExecuteReader(dc, _db))
            {

                while (dr.Read())
                {
                    list.Add(
                        new MSaleArea
                            {
                                CompanyId =
                                    dr.IsDBNull(dr.GetOrdinal("CompanyId"))
                                        ? 0
                                        : dr.GetInt32(dr.GetOrdinal("CompanyId")),
                                SaleAreaId =
                                    dr.IsDBNull(dr.GetOrdinal("SaleAreaId"))
                                        ? 0
                                        : dr.GetInt32(dr.GetOrdinal("SaleAreaId")),
                                SaleAreaName =
                                    dr.IsDBNull(dr.GetOrdinal("SaleAreaName"))
                                        ? string.Empty
                                        : dr.GetString(dr.GetOrdinal("SaleAreaName"))
                            });
                }
            }

            return list;
        }

        /// <summary>
        /// 获取销售地区信息
        /// </summary>
        /// <param name="companyId">公司编号</param>
        /// <param name="pageSize">每页记录数</param>
        /// <param name="pageIndex">当前页数</param>
        /// <param name="recordCount">总记录数</param>
        /// <param name="seach">销售地区查询实体</param>
        /// <returns></returns>
        public IList<MSaleArea> GetSaleArea(int companyId, int pageSize, int pageIndex, ref int recordCount, MSearchSaleArea seach)
        {
            if (companyId <= 0 || pageIndex <= 0 || pageSize <= 0) return null;

            string tableName = "tbl_CompanySaleArea";
            string fileName = " SaleAreaId,SaleAreaName,CompanyId ";
            var strWhere = new StringBuilder();
            strWhere.AppendFormat(" CompanyId = {0} ", companyId);
            if (seach != null)
            {

            }
            string orderByString = " SaleAreaId desc ";

            IList<MSaleArea> list = new List<MSaleArea>();

            using (IDataReader dr = DbHelper.ExecuteReader1(_db, pageSize, pageIndex, ref recordCount, tableName, fileName
                , strWhere.ToString(), orderByString, string.Empty))
            {
                while (dr.Read())
                {
                    list.Add(
                        new MSaleArea
                        {
                            CompanyId =
                                dr.IsDBNull(dr.GetOrdinal("CompanyId"))
                                    ? 0
                                    : dr.GetInt32(dr.GetOrdinal("CompanyId")),
                            SaleAreaId =
                                dr.IsDBNull(dr.GetOrdinal("SaleAreaId"))
                                    ? 0
                                    : dr.GetInt32(dr.GetOrdinal("SaleAreaId")),
                            SaleAreaName =
                                dr.IsDBNull(dr.GetOrdinal("SaleAreaName"))
                                    ? string.Empty
                                    : dr.GetString(dr.GetOrdinal("SaleAreaName"))
                        });
                }
            }

            return list;
        }
    }
}
