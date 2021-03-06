﻿using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Practices.EnterpriseLibrary.Data;
using System.Data.Common;
using System.Data;
using EyouSoft.Toolkit.DAL;

namespace EyouSoft.DAL.CompanyStructure
{
    /// <summary>
    /// 公司线路区域DAL
    /// </summary>
    public class Area : DALBase, IDAL.CompanyStructure.IArea
    {
        #region static constants
        /// <summary>
        /// 验证线路区域名称是否重复
        /// </summary>
        private const string SqlSelectIsexists = "select count(*) from tbl_Area where AreaName = @AreaName and CompanyId = @CompanyId and Id != @Id and IsDelete = '0' ;";
        /// <summary>
        /// 插入线路区域
        /// </summary>
        private const string SqlInsertArea = "insert into tbl_Area (AreaName,CompanyId,OperatorId,IsDelete,SortId) values(@AreaName,@CompanyId,@OperatorId,@IsDelete,@SortId); select @@identity;";
        private const string SqlGetAreaByCompanyId = "select Id,AreaName from tbl_Area where CompanyId = @CompanyId and IsDelete = '0' ORDER BY [SortId] ASC";
        const string SqlSelectGetAreaSortId = "SELECT MIN(SortId) AS MinSortId,MAX(SortId) AS MaxSortId FROM tbl_Area WHERE CompanyId=@CompanyId AND IsDelete='0'";

        /// <summary>
        /// 修改线路区域Sql
        /// </summary>
        private const string SqlAreaUpdate = @" update tbl_Area set AreaName = @AreaName,SortId = @SortId where Id = @Id ";
        /// <summary>
        /// 线路区域查询
        /// </summary>
        private const string SqlAreaSelect = @" select * from tbl_Area ";

        private readonly Database _db;
        #endregion

        #region 构造函数
        public Area()
        {
            this._db = SystemStore;
        }
        #endregion

        #region IArea 成员

        /// <summary>
        /// 验证是否已经存在同名的线路区域
        /// </summary>
        /// <param name="areaName">线路区域名称</param>
        /// <param name="companyId">公司编号</param>
        /// <param name="id">当前线路区域Id</param>
        /// <returns>true:存在 false:不存在</returns>
        public bool IsExists(string areaName, int companyId, int id)
        {
            bool isExists = false;
            DbCommand cmd = this._db.GetSqlStringCommand(SqlSelectIsexists);

            this._db.AddInParameter(cmd, "AreaName", DbType.String, areaName);
            this._db.AddInParameter(cmd, "CompanyId", DbType.Int32, companyId);
            this._db.AddInParameter(cmd, "Id", DbType.Int32, id);

            using (IDataReader rdr = DbHelper.ExecuteReader(cmd, this._db))
            {
                if (rdr.Read())
                {
                    isExists = rdr.GetInt32(0) > 0 ? true : false;
                }
            }

            return isExists;
        }

        /// <summary>
        /// 添加
        /// </summary>
        /// <param name="model">线路区域实体</param>
        /// <returns>true:成功 false:失败</returns>
        public bool Add(Model.CompanyStructure.Area model)
        {
            DbCommand cmd = this._db.GetSqlStringCommand(SqlInsertArea);
            this._db.AddInParameter(cmd, "AreaName", DbType.String, model.AreaName);
            this._db.AddInParameter(cmd, "CompanyId", DbType.Int32, model.CompanyId);
            this._db.AddInParameter(cmd, "OperatorId", DbType.Int32, model.OperatorId);
            this._db.AddInParameter(cmd, "IsDelete", DbType.AnsiStringFixedLength, "0");
            _db.AddInParameter(cmd, "SortId", DbType.Int32, model.SortId);

            object obj = DbHelper.GetSingle(cmd, this._db);
            if(obj == null)
            {
                model.Id = 0;
                return false;
            }

            model.Id = Toolkit.Utils.GetInt(obj.ToString());
            return true;
        }

        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="model">线路区域实体</param>
        /// <returns>true:成功 false:失败</returns>
        public bool Update(Model.CompanyStructure.Area model)
        {
            DbCommand cmd = this._db.GetSqlStringCommand(SqlAreaUpdate);
            this._db.AddInParameter(cmd, "Id", DbType.Int32, model.Id);
            this._db.AddInParameter(cmd, "AreaName", DbType.String, model.AreaName);
            _db.AddInParameter(cmd, "SortId", DbType.Int32, model.SortId);

            return DbHelper.ExecuteSql(cmd, _db) > 0 ? true : false;
        }

        /// <summary>
        /// 获取线路区域实体
        /// </summary>
        /// <param name="id">主键编号</param>
        /// <returns></returns>
        public Model.CompanyStructure.Area GetModel(int id)
        {
            Model.CompanyStructure.Area areaModel = null;
            DbCommand cmd = this._db.GetSqlStringCommand(SqlAreaSelect + " where Id = @Id ");
            this._db.AddInParameter(cmd, "Id", DbType.Int32, id);

            using (IDataReader rdr = DbHelper.ExecuteReader(cmd, this._db))
            {
                if (rdr.Read())
                {
                    #region 线路区域信息
                    areaModel = new Model.CompanyStructure.Area
                        {
                            Id = rdr.GetInt32(rdr.GetOrdinal("Id")),
                            AreaName = rdr.GetString(rdr.GetOrdinal("AreaName")),
                            CompanyId = rdr.GetInt32(rdr.GetOrdinal("CompanyId")),
                            OperatorId = rdr.GetInt32(rdr.GetOrdinal("OperatorId")),
                            IssueTime = rdr.GetDateTime(rdr.GetOrdinal("IssueTime")),
                            IsDelete = Convert.ToBoolean(rdr.GetOrdinal("IsDelete")),
                            SortId = rdr.GetInt32(rdr.GetOrdinal("SortId"))
                        };

                    #endregion
                }
            }

            return areaModel;
        }

        /// <summary>
        /// 删除线路区域集合
        /// </summary>
        /// <param name="ids">主键编号</param>
        /// <returns>true:成功 false:失败</returns>
        public bool Delete(params int[] ids)
        {
            if (ids == null || ids.Length <= 0)
                return false;

            var strSql = new StringBuilder();
            strSql.Append(" update tbl_Area set IsDelete = '1' ");
            strSql.Append(" where  ");
            if (ids.Length == 1)
            {
                strSql.AppendFormat(" Id = {0} ", ids[0]);
            }
            else
            {
                strSql.AppendFormat(" Id in ({0}) ", GetIdsByArr(ids));
            }

            DbCommand dc = _db.GetSqlStringCommand(strSql.ToString());
            return DbHelper.ExecuteSql(dc, _db) > 0 ? true : false;
        }

        /*/// <summary>
        /// 线路区域是否发布过
        /// </summary>
        /// <param name="areaId">线路ID</param>
        /// <param name="companyId">公司ID</param>
        /// <returns>true发布过 false没发布过 </returns>
        public bool IsAreaPublish(int areaId, int companyId)
        {
            var strSql = new StringBuilder();
            strSql.Append(
                " select count(*) from tbl_Route where IsDelete = '0' and CompanyId = @CompanyId and AreaId = @AreaId; ");
            strSql.Append(
                " select count(*) from tbl_KongWei where IsDelete = '0' and CompanyId = @CompanyId and AreaId = @AreaId; ");
            DbCommand cmd = this._db.GetSqlStringCommand(strSql.ToString());
            this._db.AddInParameter(cmd, "AreaId", DbType.Int32, areaId);
            this._db.AddInParameter(cmd, "CompanyId", DbType.Int32, companyId);

            bool r = false;
            using (IDataReader dr = DbHelper.ExecuteReader(cmd, _db))
            {
                if (dr.Read())
                {
                    if (!dr.IsDBNull(0) && dr.GetInt32(0) > 0)
                    {
                        r = true;
                    }
                }

                dr.NextResult();
                if (!r && dr.Read())
                {
                    if (!dr.IsDBNull(0) && dr.GetInt32(0) > 0)
                    {
                        r = true;
                    }
                }
            }

            return r;
        }*/

        /// <summary>
        /// 分页获取公司线路区域集合
        /// </summary>
        /// <param name="pageSize">每页显示条数</param>
        /// <param name="pageIndex">当前页码</param>
        /// <param name="recordCount">总记录数</param>
        /// <param name="companyId">公司编号</param>
        /// <returns>公司线路区域集合</returns>
        public IList<Model.CompanyStructure.Area> GetList(int pageSize, int pageIndex, ref int recordCount, int companyId)
        {
            IList<Model.CompanyStructure.Area> totals = new List<Model.CompanyStructure.Area>();

            string tableName = "tbl_Area";
            string orderByString = "SortId ASC,IssueTime desc";
            var fields = new StringBuilder();
            fields.Append(" Id, AreaName, CompanyId, OperatorId, IssueTime,IsDelete,");
            fields.Append(" SortId ");

            var cmdQuery = new StringBuilder(" IsDelete = '0' ");
            cmdQuery.AppendFormat(" and CompanyId = {0} ", companyId);

            using (IDataReader rdr = DbHelper.ExecuteReader1(this._db, pageSize, pageIndex, ref recordCount, tableName, fields.ToString()
                , cmdQuery.ToString(), orderByString, string.Empty))
            {
                while (rdr.Read())
                {
                    var areaInfo = new Model.CompanyStructure.Area
                        {
                            Id = rdr.GetInt32(rdr.GetOrdinal("Id")),
                            AreaName = rdr.GetString(rdr.GetOrdinal("AreaName")),
                            CompanyId = rdr.GetInt32(rdr.GetOrdinal("CompanyId")),
                            OperatorId = rdr.GetInt32(rdr.GetOrdinal("OperatorId")),
                            IssueTime = rdr.GetDateTime(rdr.GetOrdinal("IssueTime")),
                            IsDelete = Convert.ToBoolean(rdr.GetOrdinal("IsDelete")),
                            SortId = rdr.GetInt32(rdr.GetOrdinal("SortId"))
                        };

                    totals.Add(areaInfo);
                }
            }

            return totals;
        }

        /// <summary>
        /// 获取当前公司的所有线路区域信息
        /// </summary>
        /// <param name="companyId">公司ID</param>
        /// <returns></returns>
        public IList<Model.CompanyStructure.Area> GetAreaByCompanyId(int companyId)
        {
            IList<Model.CompanyStructure.Area> lsArea = new List<Model.CompanyStructure.Area>();
            DbCommand cmd = this._db.GetSqlStringCommand(SqlGetAreaByCompanyId);
            this._db.AddInParameter(cmd, "CompanyId", DbType.Int32, companyId);

            using (IDataReader rdr = DbHelper.ExecuteReader(cmd, this._db))
            {
                while (rdr.Read())
                {
                    var areaModel = new Model.CompanyStructure.Area
                        {
                            Id = rdr.GetInt32(rdr.GetOrdinal("Id")),
                            AreaName = rdr.GetString(rdr.GetOrdinal("AreaName"))
                        };
                    lsArea.Add(areaModel);
                }
            }

            return lsArea;
        }

        /// <summary>
        /// 获取指定公司线路区域排序信息(最小及最大排序号)
        /// </summary>
        /// <param name="companyId">公司编号</param>
        /// <param name="min">最小排序号</param>
        /// <param name="max">最大排序号</param>
        public void GetAreaSortId(int companyId, out int min, out int max)
        {
            min = 0; max = 0;
            DbCommand cmd = _db.GetSqlStringCommand(SqlSelectGetAreaSortId);
            _db.AddInParameter(cmd, "CompanyId", DbType.Int32, companyId);

            using (IDataReader rdr = DbHelper.ExecuteReader(cmd, _db))
            {
                if (rdr.Read())
                {
                    if (!rdr.IsDBNull(rdr.GetOrdinal("MinSortId")))
                        min = rdr.GetInt32(rdr.GetOrdinal("MinSortId"));
                    if (!rdr.IsDBNull(rdr.GetOrdinal("MaxSortId")))
                        max = rdr.GetInt32(rdr.GetOrdinal("MaxSortId"));
                }
            }

        }

        /// <summary>
        /// 线路区域是否被使用
        /// </summary>
        /// <param name="areaId">线路区域编号</param>
        /// <returns></returns>
        public bool IsShiYong(int areaId)
        {
            DbCommand cmd = _db.GetSqlStringCommand("IF EXISTS(SELECT 1 FROM [tbl_TourData] WHERE [AreaId]=@AreaId) SELECT 1 ELSE SELECT 0");
            _db.AddInParameter(cmd, "AreaId", DbType.Int32,areaId);

            using (IDataReader rdr = DbHelper.ExecuteReader(cmd, _db))
            {
                if (rdr.Read())
                {
                    return rdr.GetInt32(0) == 1;
                }
            }

            return false;
        }
        #endregion
    }
}
