using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using EyouSoft.BLL.CompanyStructure;

namespace EyouSoft.BLL.PlanStructure
{
    public class BPlanDiJie : BLLBase
    {
        private readonly EyouSoft.IDAL.PlanStructure.IPlanDiJie dal = EyouSoft.Component.Factory.ComponentFactory.CreateDAL<EyouSoft.IDAL.PlanStructure.IPlanDiJie>();

        /// <summary>
        /// 添加地接
        /// </summary>
        /// <param name="model"></param>
        /// <returns>1:成功 0：失败
        /// -1:供应商不存在
        /// </returns>
        public int Add(EyouSoft.Model.PlanStructure.MPlanDiJieInfo model)
        {
            if (model.CompanyId == 0
                || string.IsNullOrEmpty(model.TourId)
                || model.OperatorId == 0
                || !model.PayType.HasValue) return 0;


            if (model.IsMonth) {
                if (!model.MonthTime.HasValue) return 0;
            }

            model.PlanId = Guid.NewGuid().ToString();
            int flg = dal.Add(model);

            if (flg == 1)
            {
                var log = new Model.CompanyStructure.SysHandleLogs
                {
                    EventTitle = "添加地接安排",
                    ModuleId = Model.EnumType.PrivsStructure.Privs2.计调中心_地接安排,
                    EventMessage = "添加地接安排，地接编号：" + model.PlanId + "。"
                };

                new SysHandleLogs().Add(log);
            }
            return flg;

        }

        /// <summary>
        /// 修改地接安排
        /// </summary>
        /// <param name="model"></param>
        /// <returns>-1:只有地接状态在申请中才能修改	
        /// -2:供应商不存在
        /// 1:成功 0：失败</returns>
        public int Update(EyouSoft.Model.PlanStructure.MPlanDiJieInfo model)
        {

            if (string.IsNullOrEmpty(model.PlanId) || model.CompanyId == 0 || string.IsNullOrEmpty(model.TourId) || string.IsNullOrEmpty(model.GysId)) return 0;

            if (model.IsMonth)
            {
                if (!model.MonthTime.HasValue) return 0;
            }

            int flg = dal.Update(model);
            if (flg == 1)
            {
                var log = new Model.CompanyStructure.SysHandleLogs
                {
                    EventTitle = "修改地接安排",
                    ModuleId = Model.EnumType.PrivsStructure.Privs2.计调中心_地接安排,
                    EventMessage = "修改地接安排，地接编号：" + model.PlanId + "。"
                };

                new SysHandleLogs().Add(log);
            }

            return flg;

        }

        /// <summary>
        /// 修改地接状态
        /// </summary>
        /// <param name="PlanId"></param>
        /// <param name="DiJieStatus"></param>
        /// <returns>
        /// 1:成功 0：失败
        /// -1:地接安排已确认过
        /// -2:财务已审核无法再确认
        /// -3:地接安排未确认无法审核
        /// -4:财务已审核无法再审核
        /// </returns>
        public int Update(string PlanId, EyouSoft.Model.EnumType.PlanStructure.DiJieStatus DiJieStatus)
        {
            if (string.IsNullOrEmpty(PlanId)) return 0;
            int flg = dal.Update(PlanId, DiJieStatus);
            if (flg == 1)
            {
                var log = new Model.CompanyStructure.SysHandleLogs
                {
                    EventTitle = "修改地接状态",
                    ModuleId = Model.EnumType.PrivsStructure.Privs2.计调中心_地接安排,
                    EventMessage = "修改地接状态为：" + DiJieStatus + "，地接编号：" + PlanId + "。"
                };
                new SysHandleLogs().Add(log);
            }

            return flg;
        }

        /// <summary>
        /// 删除地接安排
        /// </summary>
        /// <param name="Id"></param>
        /// <returns>
        /// -1:只有处在申请中的地接安排才能删除
        ///	-2:地接存在支出不能删除
        ///	1:成功 0：失败
        /// </returns>
        public int Delete(string Id)
        {
            int flg = dal.Delete(Id);
            if (flg == 1)
            {
                var log = new Model.CompanyStructure.SysHandleLogs
                {
                    EventTitle = "删除地接安排",
                    ModuleId = Model.EnumType.PrivsStructure.Privs2.计调中心_地接安排,
                    EventMessage = "删除地接安排，地接编号：" + Id + "。"
                };

                new SysHandleLogs().Add(log);
            }
            return flg;
        }

        /// <summary>
        /// 获取实体
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        public EyouSoft.Model.PlanStructure.MPlanDiJieInfo GetModel(string Id)
        {
            if (string.IsNullOrEmpty(Id)) return null;
            return dal.GetModel(Id);
        }

        /// <summary>
        /// 获取地接列表
        /// </summary>
        /// <param name="companyId"></param>
        /// <param name="pageSize"></param>
        /// <param name="pageIndex"></param>
        /// <param name="recordCount"></param>
        /// <param name="search"></param>
        /// <returns>Sum[]:0:SumPrice</returns>
        public IList<EyouSoft.Model.PlanStructure.MPageDiJie> GetList(
        int companyId,
        int pageSize,
        int pageIndex,
        ref int recordCount,
        EyouSoft.Model.PlanStructure.MSeachDiJie search, ref decimal[] Sum)
        {
            if (companyId == 0) return null;
            return dal.GetList(companyId, pageSize, pageIndex, ref recordCount, search, ref Sum);
        }
    }
}
