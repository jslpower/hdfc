using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using EyouSoft.BLL.CompanyStructure;

namespace EyouSoft.BLL.TourStructure
{
    public class BTourData : BLLBase
    {
        private readonly EyouSoft.IDAL.TourStructure.ITourData dal = EyouSoft.Component.Factory.ComponentFactory.CreateDAL<EyouSoft.IDAL.TourStructure.ITourData>();

        #region IRouteData 成员

        /// <summary>
        /// 团队报价资料库
        /// </summary>
        /// <param name="model"></param>
        /// <returns>0:添加失败 1:添加成功</returns>
        public int Add(EyouSoft.Model.TourStructure.MTourData model)
        {
            if (model.CompanyId == 0 || model.AreaId == 0 || model.OperatorId == 0) return 0;
            int flg = dal.Add(model);

            if (flg!= 0)
            {

                var log = new Model.CompanyStructure.SysHandleLogs
                {
                    EventTitle = "团队报价资料库添加",
                    ModuleId = Model.EnumType.PrivsStructure.Privs2.计调中心_团队报价资料库,
                    EventMessage = "团队报价资料库添加，编号：" + flg + "。"
                };

                new SysHandleLogs().Add(log);
                flg = 1;
            }

            return flg;
        }

        /// <summary>
        /// 修改团队报价资料库
        /// </summary>
        /// <param name="model"></param>
        /// <returns>0:失败 1:成功</returns>
        public int Update(EyouSoft.Model.TourStructure.MTourData model)
        {
            if (model.TourDataId == 0||model.AreaId==0) return 0;
            int flg = dal.Update(model);
            if (flg == 1)
            {
                var log = new Model.CompanyStructure.SysHandleLogs
                {
                    EventTitle = "团队报价资料库修改",
                    ModuleId = Model.EnumType.PrivsStructure.Privs2.计调中心_团队报价资料库,
                    EventMessage = "团队报价资料库修改，编号：" + model.TourDataId + "。"
                };

                new SysHandleLogs().Add(log);
            }

            return flg;
        }

        /// <summary>
        /// 删除报价资料库
        /// </summary>
        /// <param name="Id"></param>
        /// <returns>0:失败 1:成功</returns>
        public int Delete(int Id)
        {
            if (Id == 0) return 0;
            int flg = dal.Delete(Id);
            if (flg == 1)
            {
                var log = new Model.CompanyStructure.SysHandleLogs
                {
                    EventTitle = "团队报价资料库删除",
                    ModuleId = Model.EnumType.PrivsStructure.Privs2.计调中心_团队报价资料库,
                    EventMessage = "团队报价资料库删除，编号：" + Id + "。"
                };

                new SysHandleLogs().Add(log);
            }

            return flg;

        }


        /// <summary>
        /// 分页获取资料库列表
        /// </summary>
        /// <param name="companyId"></param>
        /// <param name="pageSize"></param>
        /// <param name="pageIndex"></param>
        /// <param name="recordCount"></param>
        /// <param name="search"></param>
        /// <returns></returns>
        public IList<EyouSoft.Model.TourStructure.MTourData> GetList(int companyId, int pageSize, int pageIndex, ref int recordCount, EyouSoft.Model.TourStructure.MSearchTourData search)
        {
            if (companyId == 0) return null;
            return dal.GetList(companyId, pageSize, pageIndex, ref recordCount, search);

        }

        /// <summary>
        /// 获取model
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public EyouSoft.Model.TourStructure.MTourData GetModel(int id)
        {
            if (id == 0) return null;

            return dal.GetModel(id);
        }


        /// <summary>
        /// 审核
        /// </summary>
        /// <param name="Id"></param>
        /// <param name="OperatorId"></param>
        /// <returns>0:失败 1:成功</returns>
        public int Check(int Id, int OperatorId)
        {
            if (Id == 0 || OperatorId == 0) return 0;
            int flg = dal.Check(Id, OperatorId);
            if (flg == 1)
            {

                var log = new Model.CompanyStructure.SysHandleLogs
                {
                    EventTitle = "团队报价资料库审核",
                    ModuleId = Model.EnumType.PrivsStructure.Privs2.财务管理_应收管理,
                    EventMessage = "团队报价资料库审核，编号：" + Id + "，审核人编号：" + OperatorId + "。"
                };

                new SysHandleLogs().Add(log);
            }

            return flg;
        }

        #endregion
    }
}
