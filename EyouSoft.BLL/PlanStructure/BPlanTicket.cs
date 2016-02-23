using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using EyouSoft.BLL.CompanyStructure;

namespace EyouSoft.BLL.PlanStructure
{
    public class BPlanTicket : BLLBase
    {
        private readonly EyouSoft.IDAL.PlanStructure.IPlanTicket dal = EyouSoft.Component.Factory.ComponentFactory.CreateDAL<EyouSoft.IDAL.PlanStructure.IPlanTicket>();



        #region IPlanTicket 成员

        /// <summary>
        /// 添加票务安排
        /// </summary>
        /// <param name="model"></param>
        /// <returns>1:成功 0:失败
        /// -1:供应商不存在
        /// </returns>
        public int Add(EyouSoft.Model.PlanStructure.MPlanTicketInfo model)
        {
            if (model.CompanyId == 0
                || string.IsNullOrEmpty(model.TourId)
                || model.OperatorId == 0
                || !model.PayType.HasValue) return 0;

            if (model.IsMonth)
            {
                if (!model.MonthTime.HasValue) return 0;
            }

            model.PlanId = Guid.NewGuid().ToString();
            int flg = dal.Add(model);

            if (flg == 1)
            {
                var log = new Model.CompanyStructure.SysHandleLogs
                {
                    EventTitle = "添加票务安排",
                    ModuleId = Model.EnumType.PrivsStructure.Privs2.计调中心_票务安排,
                    EventMessage = "添加票务安排，票务编号：" + model.PlanId + "。"
                };

                new SysHandleLogs().Add(log);
            }
            return flg;
        }

        /// <summary>
        /// 修改票务安排
        /// </summary>
        /// <param name="model"></param>
        /// <returns>
        /// -1:只有出票、退票状态在申请中才能修改	
        /// -2:供应商不存在
        ///	1:成功 0：失败
        /// </returns>
        public int Update(EyouSoft.Model.PlanStructure.MPlanTicketInfo model)
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
                    EventTitle = "修改票务安排",
                    ModuleId = Model.EnumType.PrivsStructure.Privs2.计调中心_票务安排,
                    EventMessage = "修改票务安排，票务编号：" + model.PlanId + "。"
                };

                new SysHandleLogs().Add(log);
            }

            return flg;
        }


        /// <summary>
        /// 修改票务安排
        /// </summary>
        /// <param name="model"></param>
        /// <returns>
        /// 1:成功 0：失败
        /// -1:票务安排已确认
        /// -2:财务已审核出票无法再确认
        /// -3:票务安排未确认无法审核出票
        /// -4:财务已审核无法再审核出票
        /// </returns>
        public int Update(string PlanId, EyouSoft.Model.EnumType.PlanStructure.TicketStatus TicketStatus)
        {
            if (string.IsNullOrEmpty(PlanId)) return 0;

            int flg = dal.Update(PlanId, TicketStatus);
            if (flg == 1)
            {
                var log = new Model.CompanyStructure.SysHandleLogs
                {
                    EventTitle = "修改票务安排状态",
                    ModuleId = Model.EnumType.PrivsStructure.Privs2.计调中心_票务安排,
                    EventMessage = "修改票务安排状态为：" + TicketStatus + "，票务编号：" + PlanId + "。"
                };

                new SysHandleLogs().Add(log);
            }

            return flg;

        }

        /// <summary>
        /// 删除票务安排
        /// </summary>
        /// <param name="Id"></param>
        /// <returns>
        /// -1:只有处在申请中的出票、退票安排才能删除
        ///	-2:票务存在支出不能删除	
        ///	1:成功 0：失败
        /// </returns>
        public int Delete(string Id)
        {
            int flg = dal.Delete(Id);
            if (flg == 1)
            {
                var log = new Model.CompanyStructure.SysHandleLogs
                {
                    EventTitle = "删除票务安排",
                    ModuleId = Model.EnumType.PrivsStructure.Privs2.计调中心_票务安排,
                    EventMessage = "删除票务安排，票务编号：" + Id + "。"
                };

                new SysHandleLogs().Add(log);
            }
            return flg;
        }

        /// <summary>
        /// 获取model
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        public EyouSoft.Model.PlanStructure.MPlanTicketInfo GetModel(string Id)
        {
            if (string.IsNullOrEmpty(Id)) return null;
            return dal.GetModel(Id);

        }

        /// <summary>
        /// 分页获取列表
        /// </summary>
        /// <param name="companyId"></param>
        /// <param name="pageSize"></param>
        /// <param name="pageIndex"></param>
        /// <param name="recordCount"></param>
        /// <param name="search"></param>
        /// <returns>Sum[0]:Adults,Sum[1]:Childs,Sum[2]:SumPrice</returns>
        public IList<EyouSoft.Model.PlanStructure.MPlanTicket> GetList(
            int companyId,
            int pageSize,
            int pageIndex,
            ref int recordCount,
            EyouSoft.Model.PlanStructure.MSearchTicket search, ref decimal[] Sum)
        {
            if (companyId == 0) return null;
            return dal.GetList(companyId, pageSize, pageIndex, ref recordCount, search, ref  Sum);
        }

        #endregion
    }
}
