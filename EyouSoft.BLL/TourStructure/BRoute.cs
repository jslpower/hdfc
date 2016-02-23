using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using EyouSoft.BLL.CompanyStructure;

namespace EyouSoft.BLL.TourStructure
{
    public class BRoute : BLLBase
    {

        private readonly EyouSoft.IDAL.TourStructure.IRoute dal = EyouSoft.Component.Factory.ComponentFactory.CreateDAL<EyouSoft.IDAL.TourStructure.IRoute>();

        /// <summary>
        /// 获取线路的集合
        /// </summary>
        /// <param name="companyId"></param>
        /// <param name="routeName"></param>
        /// <returns></returns>
        public IList<EyouSoft.Model.TourStructure.MRoute> GetRouteList(int companyId, string routeName)
        {
            if (companyId == 0) return null;
            return dal.GetRouteList(companyId, routeName);
        }

        /// <summary>
        /// 获取线路信息
        /// </summary>
        /// <param name="routeId">线路编号</param>
        /// <returns></returns>
        public Model.TourStructure.MRoute GetRoute(string routeId)
        {
            if (string.IsNullOrEmpty(routeId)) return null;

            return dal.GetRoute(routeId);
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
            if (companyId == 0) return null;
            return dal.GetList(companyId, pageSize, pageSize, ref recordCount, routeName);
        }


        /// <summary>
        /// 删除线路
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        public bool Delete(string Id)
        {
            if (string.IsNullOrEmpty(Id)) return false;

            if (dal.Delete(Id))
            {
                var log = new Model.CompanyStructure.SysHandleLogs
                {
                    EventTitle = "删除线路",
                    ModuleId = Model.EnumType.PrivsStructure.Privs2.计调中心_线路管理,
                    EventMessage = "删除线路，编号：" + Id + "。"
                };

                new SysHandleLogs().Add(log);

                return true;
            }
            return false;
        }


        /// <summary>
        /// 修改线路
        /// </summary>
        /// <param name="RouteName">线路名称</param>
        /// <param name="RouteId">线路编号</param>
        /// <param name="CompanyId">公司编号</param>
        /// <returns>1:成功 0：失败 -1:线路名称已存在</returns>
        public int Update(string RouteName, string RouteId, int CompanyId)
        {
            if (string.IsNullOrEmpty(RouteId)) return 0;

            int flg=dal.Update(RouteName, RouteId,CompanyId);

            if (flg==1) 
            {
                var log = new Model.CompanyStructure.SysHandleLogs
                {
                    EventTitle = "修改线路",
                    ModuleId = Model.EnumType.PrivsStructure.Privs2.计调中心_线路管理,
                    EventMessage = "修改线路，编号：" + RouteId + "。"
                };

                new SysHandleLogs().Add(log);

               
            }
            return flg;
        }
    }
}
