using System.Collections.Generic;
using EyouSoft.Toolkit.DAL;
using Microsoft.Practices.EnterpriseLibrary.Data;
using System.Data.Common;
using System.Data;
using System.Text;

namespace EyouSoft.DAL.CompanyStructure
{
    /// <summary>
    /// 城市管理DAL
    /// </summary>
    public class City : DALBase, IDAL.CompanyStructure.ICity
    {
        #region static constants

        private const string SqlSelectIsexists = " select count(*) from tbl_CompanyCity where CityName = @cityName and CompanyId = @companyId and Id != @Id ";
        private const string SqlInsertCity = " insert into tbl_CompanyCity (ProvinceId,CityName,CompanyId,IsFav,OperatorId) values(@ProvinceId,@CityName,@CompanyId,@IsFav,@OperatorId);select @@identity; ";
        private const string SqlUpdateCity = " update tbl_CompanyCity set ProvinceId = @ProvinceId,CityName = @CityName where Id = @Id";
        private const string SqlSelectCity = " select Id,ProvinceId,CityName,CompanyId,IsFav,OperatorId,IssueTime from tbl_CompanyCity where Id = @Id ";
        private const string SqlDeleteCity = " delete from tbl_CompanyCity ";
        private const string SqlSetFav = "update tbl_CompanyCity set IsFav = @IsFav where Id = @Id ";
        private const string SqlGetList = " select Id,ProvinceId,CityName,CompanyId,IsFav,OperatorId,IssueTime from tbl_CompanyCity where CompanyId = @CompanyId and ProvinceId = @ProvinceId ";

        private readonly Database _db;

        #endregion

        #region 构造函数
        public City()
        {
            this._db = SystemStore;
        }
        #endregion

        #region ICity 成员

        /// <summary>
        /// 验证城市名是否已经存在
        /// </summary>
        /// <param name="cityName">城市名称</param>
        /// <param name="companyId">公司编号</param>
        /// <param name="cityId">城市编号</param>
        /// <returns>true:已存在 false:不存在</returns>
        public bool IsExists(string cityName, int companyId, int cityId)
        {
            if (string.IsNullOrEmpty(cityName) || companyId <= 0) return true;

            bool isExists = false;
            DbCommand cmd = this._db.GetSqlStringCommand(SqlSelectIsexists);

            this._db.AddInParameter(cmd, "cityName", DbType.String, cityName);
            this._db.AddInParameter(cmd, "companyId", DbType.Int32, companyId);
            this._db.AddInParameter(cmd, "Id", DbType.Int32, cityId);

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
        /// 添加城市
        /// </summary>
        /// <param name="model">城市实体</param>
        /// <returns>true:成功 false:失败</returns>
        public bool Add(Model.CompanyStructure.City model)
        {
            if (model == null || string.IsNullOrEmpty(model.CityName)) return false;

            DbCommand cmd = this._db.GetSqlStringCommand(SqlInsertCity);

            this._db.AddInParameter(cmd, "ProvinceId", DbType.String, model.ProvinceId);
            this._db.AddInParameter(cmd, "CityName", DbType.String, model.CityName);
            this._db.AddInParameter(cmd, "CompanyId", DbType.Int32, model.CompanyId);
            this._db.AddInParameter(cmd, "IsFav", DbType.AnsiStringFixedLength, this.GetBooleanToStr(model.IsFav));
            this._db.AddInParameter(cmd, "OperatorId", DbType.Int32, model.OperatorId);

            object obj = DbHelper.GetSingle(cmd, this._db);
            if (obj == null) return false;

            model.Id = Toolkit.Utils.GetInt(obj.ToString());
            return true;
        }

        /// <summary>
        /// 修改城市
        /// </summary>
        /// <param name="model">城市实体</param>
        /// <returns>true:成功 false:失败</returns>
        public bool Update(Model.CompanyStructure.City model)
        {
            if (model == null || model.Id <= 0) return false;

            DbCommand cmd = this._db.GetSqlStringCommand(SqlUpdateCity);
            this._db.AddInParameter(cmd, "ProvinceId", DbType.Int32, model.ProvinceId);
            this._db.AddInParameter(cmd, "CityName", DbType.String, model.CityName);
            this._db.AddInParameter(cmd, "Id", DbType.Int32, model.Id);

            return DbHelper.ExecuteSql(cmd, this._db) > 0 ? true : false;
        }

        /// <summary>
        /// 获取城市实体
        /// </summary>
        /// <param name="id">主键编号</param>
        /// <returns></returns>
        public Model.CompanyStructure.City GetModel(int id)
        {
            if (id <= 0) return null;

            Model.CompanyStructure.City cityModel = null;
            DbCommand cmd = this._db.GetSqlStringCommand(SqlSelectCity);
            this._db.AddInParameter(cmd, "Id", DbType.Int32, id);

            using (IDataReader rdr = DbHelper.ExecuteReader(cmd, this._db))
            {
                if (rdr.Read())
                {
                    cityModel = new Model.CompanyStructure.City
                        {
                            Id = rdr.GetInt32(rdr.GetOrdinal("Id")),
                            ProvinceId = rdr.GetInt32(rdr.GetOrdinal("ProvinceId")),
                            CityName = rdr.GetString(rdr.GetOrdinal("CityName")),
                            CompanyId = rdr.GetInt32(rdr.GetOrdinal("CompanyId")),
                            IsFav = this.GetBoolean(rdr.GetString(rdr.GetOrdinal("IsFav"))),
                            OperatorId = rdr.GetInt32(rdr.GetOrdinal("OperatorId")),
                            IssueTime = rdr.GetDateTime(rdr.GetOrdinal("IssueTime"))
                        };
                }
            }

            return cityModel;
        }

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="ids">主键集合</param>
        /// <returns>true:成功 false:失败</returns>
        public bool Delete(params int[] ids)
        {
            if (ids == null || ids.Length <= 0)
                return false;

            var strSql = new StringBuilder();
            strSql.Append(SqlDeleteCity);
            strSql.Append(" where ");
            if (ids.Length == 1)
            {
                strSql.AppendFormat(" Id = {0} ", ids[0]);
            }
            else
            {
                strSql.AppendFormat(" Id in ({0}) ", this.GetIdsByArr(ids));
            }

            DbCommand dc = _db.GetSqlStringCommand(strSql.ToString());
            return DbHelper.ExecuteSql(dc, _db) > 0 ? true : false;
        }

        /// <summary>
        /// 设置是否常用
        /// </summary>
        /// <param name="id">主键编号</param>
        /// <param name="isFav">是否常用</param>
        /// <returns>true:成功 false:失败</returns>
        public bool SetFav(int id, bool isFav)
        {
            DbCommand cmd = this._db.GetSqlStringCommand(SqlSetFav);
            this._db.AddInParameter(cmd, "IsFav", DbType.AnsiStringFixedLength, isFav ? "1" : "0");
            this._db.AddInParameter(cmd, "Id", DbType.String, id);

            return DbHelper.ExecuteSql(cmd, this._db) > 0 ? true : false;
        }

        /// <summary>
        /// 获取城市集合
        /// </summary>
        /// <param name="companyId">公司编号</param>
        /// <param name="provinceId">省份编号</param>
        /// <param name="isFav">是否常用城市 =null返回全部</param>
        /// <returns>城市集合</returns>
        public IList<Model.CompanyStructure.City> GetList(int companyId, int provinceId, bool? isFav)
        {
            IList<Model.CompanyStructure.City> lsCity = new List<Model.CompanyStructure.City>();
            DbCommand cmd;

            if (isFav.HasValue)
            {
                cmd = this._db.GetSqlStringCommand(SqlGetList + " and IsFav=@IsFav ");
                this._db.AddInParameter(cmd, "IsFav", DbType.AnsiStringFixedLength, isFav == true ? "1" : "0");

            }
            else
            {
                cmd = this._db.GetSqlStringCommand(SqlGetList);
            }

            this._db.AddInParameter(cmd, "CompanyId", DbType.Int32, companyId);
            this._db.AddInParameter(cmd, "ProvinceId", DbType.Int32, provinceId);

            using (IDataReader rdr = DbHelper.ExecuteReader(cmd, this._db))
            {
                while (rdr.Read())
                {
                    var cityModel = new Model.CompanyStructure.City
                        {
                            Id = rdr.GetInt32(rdr.GetOrdinal("Id")),
                            ProvinceId = rdr.GetInt32(rdr.GetOrdinal("ProvinceId")),
                            CityName = rdr.GetString(rdr.GetOrdinal("CityName")),
                            CompanyId = rdr.GetInt32(rdr.GetOrdinal("CompanyId")),
                            IsFav = this.GetBoolean(rdr.GetString(rdr.GetOrdinal("IsFav"))),
                            OperatorId = rdr.GetInt32(rdr.GetOrdinal("OperatorId")),
                            IssueTime = rdr.GetDateTime(rdr.GetOrdinal("IssueTime"))
                        };
                    lsCity.Add(cityModel);
                }
            }

            return lsCity;
        }

        /// <summary>
        /// 获取城市集合
        /// </summary>
        /// <param name="companyId">公司编号</param>
        /// <param name="provinceId">省份编号</param>
        /// <param name="isFav">是否常用城市 =null返回全部</param>
        /// <returns>城市集合</returns>
        public IList<Model.CompanyStructure.City> GetList(int companyId, int? provinceId, bool? isFav)
        {
            if (companyId <= 0)
                return null;

            IList<Model.CompanyStructure.City> lsCity = new List<Model.CompanyStructure.City>();
            var strSql = new StringBuilder(" select Id,ProvinceId,CityName,CompanyId,IsFav,OperatorId,IssueTime from tbl_CompanyCity where CompanyId = @CompanyId ");
            if (provinceId.HasValue && provinceId.Value > 0)
                strSql.AppendFormat(" and ProvinceId = {0} ", provinceId);
            if (isFav.HasValue)
                strSql.AppendFormat("  and IsFav = '{0}' ", isFav.Value ? "1" : "0");

            DbCommand dc = this._db.GetSqlStringCommand(strSql.ToString());
            this._db.AddInParameter(dc, "CompanyId", DbType.Int32, companyId);

            using (IDataReader rdr = DbHelper.ExecuteReader(dc, this._db))
            {
                while (rdr.Read())
                {
                    var cityModel = new Model.CompanyStructure.City
                        {
                            Id = rdr.GetInt32(rdr.GetOrdinal("Id")),
                            ProvinceId = rdr.GetInt32(rdr.GetOrdinal("ProvinceId")),
                            CityName = rdr.GetString(rdr.GetOrdinal("CityName")),
                            CompanyId = rdr.GetInt32(rdr.GetOrdinal("CompanyId")),
                            IsFav = this.GetBoolean(rdr.GetString(rdr.GetOrdinal("IsFav"))),
                            OperatorId = rdr.GetInt32(rdr.GetOrdinal("OperatorId")),
                            IssueTime = rdr.GetDateTime(rdr.GetOrdinal("IssueTime"))
                        };

                    lsCity.Add(cityModel);
                }
            }

            return lsCity;
        }

        #endregion
    }
}
