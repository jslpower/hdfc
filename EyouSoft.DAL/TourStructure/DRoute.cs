using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Practices.EnterpriseLibrary.Data;
using System.Data.Common;
using System.Data;
using EyouSoft.Toolkit.DAL;

namespace EyouSoft.DAL.TourStructure
{
    public class DRoute : EyouSoft.Toolkit.DAL.DALBase, EyouSoft.IDAL.TourStructure.IRoute
    {

        #region 初始化db
        private Database _db = null;

        /// <summary>
        /// 初始化_db
        /// </summary>
        public DRoute()
        {
            _db = base.SystemStore;
        }
        #endregion

        /// <summary>
        /// 获取线路的集合
        /// </summary>
        /// <param name="companyId"></param>
        /// <param name="routeName"></param>
        /// <returns></returns>
        public IList<EyouSoft.Model.TourStructure.MRoute> GetRouteList(int companyId, string routeName)
        {

            IList<EyouSoft.Model.TourStructure.MRoute> list = new List<EyouSoft.Model.TourStructure.MRoute>();

            StringBuilder query = new StringBuilder();
            query.AppendFormat("select * from tbl_Route where CompanyId=@CompanyId and RouteName like '%{0}%' ", routeName);

            DbCommand cmd = this._db.GetSqlStringCommand(query.ToString());
            this._db.AddInParameter(cmd, "CompanyId", DbType.Int32, companyId);

            using (IDataReader dr = DbHelper.ExecuteReader(cmd, this._db))
            {
                while (dr.Read())
                {
                    EyouSoft.Model.TourStructure.MRoute model = new EyouSoft.Model.TourStructure.MRoute();
                    model.RouteId = dr.GetString(dr.GetOrdinal("Id"));
                    model.RouteName = dr.GetString(dr.GetOrdinal("RouteName"));
                    list.Add(model);
                }
            }
            return list;
        }


        /// <summary>
        /// 分页获取线路信息
        /// </summary>
        /// <param name="companyId"></param>
        /// <param name="pageSize"></param>
        /// <param name="pageIndex"></param>
        /// <param name="recordCount"></param>
        /// <param name="routeName"></param>
        /// <returns></returns>
        public IList<EyouSoft.Model.TourStructure.MRoute> GetList(int companyId,
             int pageSize,
             int pageIndex,
             ref int recordCount, string routeName)
        {

            IList<EyouSoft.Model.TourStructure.MRoute> list = new List<EyouSoft.Model.TourStructure.MRoute>();

            string fileds = "Id,RouteName";

            string orderByString = " IssueTime desc";

            string tableName = "tbl_Route";

            StringBuilder query = new StringBuilder();
            query.AppendFormat(" CompanyId={0}", companyId);

            if (!string.IsNullOrEmpty(routeName))
            {
                query.AppendFormat(" and  RouteName like  '%{0}%' ", routeName);
            }

            using (IDataReader dr = DbHelper.ExecuteReader1(this._db, pageSize, pageIndex, ref recordCount, tableName, fileds, query.ToString(), orderByString, null))
            {
                while (dr.Read())
                {
                    EyouSoft.Model.TourStructure.MRoute model = new EyouSoft.Model.TourStructure.MRoute();
                    model.RouteId = dr.GetString(dr.GetOrdinal("Id"));
                    model.RouteName = dr.GetString(dr.GetOrdinal("RouteName"));
                    list.Add(model);
                }
            }

            return list;
        }

        /// <summary>
        /// 删除线路
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        public bool Delete(string Id)
        {
            string sql = "Delete from  tbl_Route where Id=@Id";
            DbCommand cmd = this._db.GetSqlStringCommand(sql);
            this._db.AddInParameter(cmd, "Id", DbType.AnsiStringFixedLength, Id);
            return DbHelper.ExecuteSql(cmd, this._db) == 1;
        }


        /// <summary>
        /// 修改线路
        /// </summary>
        /// <param name="RouteName">线路名称</param>
        /// <param name="RouteId">线路编号</param>
        /// <param name="CompanyId">公司编号</param>
        /// <returns></returns>
        public int Update(string RouteName, string RouteId, int CompanyId)
        {
            DbCommand cmd = this._db.GetStoredProcCommand("proc_Route_Update");

            this._db.AddInParameter(cmd, "Id", DbType.AnsiStringFixedLength, RouteId);
            this._db.AddInParameter(cmd, "CompanyId", DbType.Int32, CompanyId);
            this._db.AddInParameter(cmd, "RouteName", DbType.String, RouteName);
            this._db.AddOutParameter(cmd, "Result", DbType.Int32, 4);

            DbHelper.RunProcedureWithResult(cmd, this._db);

            return Convert.ToInt32( this._db.GetParameterValue(cmd, "Result"));

        }

        /// <summary>
        /// 获取线路信息
        /// </summary>
        /// <param name="routeId">线路编号</param>
        /// <returns></returns>
        public Model.TourStructure.MRoute GetRoute(string routeId)
        {
            if (string.IsNullOrEmpty(routeId)) return null;

            DbCommand cmd = this._db.GetSqlStringCommand(" select Id,CompanyId,RouteName,IssueTime from tbl_Route where Id = @Id; ");
            this._db.AddInParameter(cmd, "Id", DbType.AnsiStringFixedLength, routeId);

            Model.TourStructure.MRoute model = null;
            using (IDataReader dr = DbHelper.ExecuteReader(cmd, _db))
            {
                if (dr.Read())
                {
                    model = new Model.TourStructure.MRoute
                        {
                            RouteId = routeId,
                            CompanyId = dr.IsDBNull(dr.GetOrdinal("CompanyId")) ? 0 : dr.GetInt32(dr.GetOrdinal("CompanyId")),
                            RouteName = dr.IsDBNull(dr.GetOrdinal("RouteName")) ? string.Empty : dr.GetString(dr.GetOrdinal("RouteName"))
                        };
                    if (!dr.IsDBNull(dr.GetOrdinal("IssueTime"))) model.IssueTime = dr.GetDateTime(dr.GetOrdinal("IssueTime"));
                }
            }

            return model;
        }
    }
}
