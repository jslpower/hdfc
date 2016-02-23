using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EyouSoft.IDAL.TourStructure
{
    public interface IRoute
    {
        /// <summary>
        /// 获取线路的集合
        /// </summary>
        /// <param name="companyId"></param>
        /// <param name="routeName"></param>
        /// <returns></returns>
        IList<EyouSoft.Model.TourStructure.MRoute> GetRouteList(int companyId, string routeName);

        IList<EyouSoft.Model.TourStructure.MRoute> GetList(int companyId,
            int pageSize,
            int pageIndex,
            ref int recordCount, string routeName);


        bool Delete(string Id);

        /// <summary>
        /// 修改线路
        /// </summary>
        /// <param name="RouteName">线路名称</param>
        /// <param name="RouteId">线路编号</param>
        /// <param name="CompanyId">公司编号</param>
        /// <returns></returns>
        int Update(string RouteName, string RouteId, int CompanyId);

        /// <summary>
        /// 获取线路信息
        /// </summary>
        /// <param name="routeId">线路编号</param>
        /// <returns></returns>
        Model.TourStructure.MRoute GetRoute(string routeId);
    }
}
