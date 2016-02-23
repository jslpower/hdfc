using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using EyouSoft.BLL.CompanyStructure;

namespace EyouSoft.BLL.TourStructure
{
    #region 确认件登记短信提醒组团社业务逻辑

    /// <summary>
    /// 确认件登记短信提醒组团社业务逻辑
    /// </summary>
    public class BSMSTourCustomer : BLLBase
    {
        private readonly IDAL.CompanyStructure.ICustomerCareFor _dalCustomerCareFor = EyouSoft.Component.Factory.ComponentFactory.CreateDAL<IDAL.CompanyStructure.ICustomerCareFor>();

        private readonly IDAL.TourStructure.ISMSTourCustomer _dalSmsTourCustomer = EyouSoft.Component.Factory.ComponentFactory.CreateDAL<IDAL.TourStructure.ISMSTourCustomer>();

        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="model">确认件登记短信提醒组团社实体</param>
        /// <returns>1成功；其他失败</returns>
        public int Add(Model.TourStructure.MSMSTourCustomer model)
        {
            if (model == null || string.IsNullOrEmpty(model.TourId)) return 0;

            return _dalSmsTourCustomer.Add(model);
        }

        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="model">短信中心 短信关怀实体</param>
        /// <returns>1成功；其他失败</returns>
        public int Update(Model.TourStructure.MSMSTourCustomer model)
        {
            if (model == null || model.Id <= 0) return 0;

            return _dalSmsTourCustomer.Update(model);
        }

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="tourId">团队编号</param>
        /// <param name="smsId">短信提醒编号</param>
        /// <returns>1成功；其他失败</returns>
        public int Delete(string tourId, params int[] smsId)
        {
            if (string.IsNullOrEmpty(tourId) || smsId == null || smsId.Length < 1) return 0;

            return _dalSmsTourCustomer.Delete(tourId, smsId);
        }

        /// <summary>
        /// 停发
        /// </summary>
        /// <param name="smsId"></param>
        /// <returns></returns>
        public bool StopIt(int smsId)
        {
            if (smsId <= 0) return false;

            return _dalCustomerCareFor.StopIt(smsId);
        }

        /// <summary>
        /// 开启
        /// </summary>
        /// <param name="smsId">编号</param>
        /// <returns></returns>
        public bool StartIt(int smsId)
        {
            if (smsId <= 0) return false;

            return _dalCustomerCareFor.StartIt(smsId);
        }

        /// <summary>
        /// 获取确认件登记短信提醒组团社实体
        /// </summary>
        /// <param name="smsId">短信提醒编号</param>
        /// <returns></returns>
        public Model.TourStructure.MSMSTourCustomer GetModel(int smsId)
        {
            if (smsId <= 0) return null;

            return _dalSmsTourCustomer.GetModel(smsId);
        }

        /// <summary>
        /// 获取确认件登记短信提醒组团社列表
        /// </summary>
        /// <param name="companyId">公司编号</param>
        /// <param name="pageSize">每页记录数</param>
        /// <param name="pageIndex">当前页数</param>
        /// <param name="recordCount">总记录数</param>
        /// <param name="search">查询实体</param>
        /// <returns></returns>
        public IList<Model.TourStructure.MSMSTourCustomer> GetList(int companyId,
         int pageSize,
         int pageIndex,
         ref int recordCount,
         Model.TourStructure.MSearchSMSTourCustomer search)
        {
            if (companyId <= 0 || pageIndex <= 0 || pageSize <= 1) return null;

            return _dalSmsTourCustomer.GetList(companyId, pageSize, pageIndex, ref recordCount, search);
        }
    }

    #endregion

    public class BTour : BLLBase
    {
        private readonly EyouSoft.IDAL.TourStructure.ITour dal = EyouSoft.Component.Factory.ComponentFactory.CreateDAL<EyouSoft.IDAL.TourStructure.ITour>();

        /// <summary>
        /// 添加确认件
        /// </summary>
        /// <param name="model"></param>
        /// <returns> 1:成功 0：失败</returns>
        public int Add(EyouSoft.Model.TourStructure.MTourInfo model)
        {
            if (model.CompanyId == 0
                || !model.TourType.HasValue
                || !model.TourStatus.HasValue
                || !model.LDate.HasValue
                || !model.RDate.HasValue
                || string.IsNullOrEmpty(model.RouteName)
                || model.Adults + model.Childs + model.Accompanys == 0
                || model.SaleId == 0
                || string.IsNullOrEmpty(model.BuyCompanyId)
                || model.SumPrice == 0)
            {
                return 0;
            }

            if (model.IsMonth)
            {
                if (!model.MonthTime.HasValue)
                {
                    return 0;
                }
            }

            model.TourId = Guid.NewGuid().ToString();

            int flg = dal.Add(model);

            if (flg == 1)
            {
                var log = new Model.CompanyStructure.SysHandleLogs
                {

                    EventTitle = "新增确认件登记",
                    ModuleId = Model.EnumType.PrivsStructure.Privs2.计调中心_确认件登记,
                    EventMessage = "新增确认件登记，确认件编号：" + model.TourId + "。"
                };

                new SysHandleLogs().Add(log);
            }
            return flg;
        }

        /// <summary>
        /// 确认件修改
        /// </summary>
        /// <param name="model"></param>
        /// <returns>
        /// -1:确认登记件财务已操作结束 无法修改
        ///	 1:成功 0：失败
        /// </returns>
        public int Update(EyouSoft.Model.TourStructure.MTourInfo model)
        {
            if (string.IsNullOrEmpty(model.TourId)
                || model.CompanyId == 0
                || !model.TourType.HasValue
                || !model.LDate.HasValue
                || !model.RDate.HasValue
                || string.IsNullOrEmpty(model.RouteName)
                || model.Adults + model.Childs + model.Accompanys == 0
                || model.SaleId == 0
                || string.IsNullOrEmpty(model.BuyCompanyId)
                || model.SumPrice == 0)
            {
                return 0;
            }

            if (model.IsMonth)
            {
                if (!model.MonthTime.HasValue)
                {
                    return 0;
                }
            }
            int flg = dal.Update(model);

            if (flg == 1)
            {
                var log = new Model.CompanyStructure.SysHandleLogs
                {
                    EventTitle = "修改确认件登记",
                    ModuleId = Model.EnumType.PrivsStructure.Privs2.计调中心_确认件登记,
                    EventMessage = "修改确认件登记，确认件编号：" + model.TourId + "。"
                };

                new SysHandleLogs().Add(log);
            }
            return flg;
        }



        /// <summary>
        /// 财务管理 登记件——操作结束
        /// </summary>
        /// <param name="model"></param>
        /// <returns>0:失败 1:成功
        /// -1: 确认登记件已操作结束
        /// </returns>
        public int Update_End(string TourId)
        {
            if (string.IsNullOrEmpty(TourId)) return 0;

            int flg = dal.Update(TourId, true);
            if (flg == 1)
            {
                var log = new Model.CompanyStructure.SysHandleLogs
                {
                    EventTitle = "财务确认 登记件 操作结束",
                    ModuleId = Model.EnumType.PrivsStructure.Privs2.财务管理_应收管理,
                    EventMessage = "财务确认 登记件 操作结束，确认件编号：" + TourId + "。"
                };

                new SysHandleLogs().Add(log);
            }
            return flg;
        }

        /// <summary>
        /// 财务管理 登记件——退回计调
        /// </summary>
        /// <param name="TourId"></param>
        /// <returns>0:失败 1:成功 -2:确认登记件已退回计调</returns>
        public int Update_Back(string TourId)
        {
            if (string.IsNullOrEmpty(TourId)) return 0;

            int flg = dal.Update(TourId, false);
            if (flg == 1)
            {
                var log = new Model.CompanyStructure.SysHandleLogs
                {
                    EventTitle = "财务确认 登记件 退回计调",
                    ModuleId = Model.EnumType.PrivsStructure.Privs2.财务管理_应收管理,
                    EventMessage = "财务确认 登记件 退回计调，确认件编号：" + TourId + "。"
                };

                new SysHandleLogs().Add(log);
            }
            return flg;
        }


        /// <summary>
        /// 修改团队状态
        /// </summary>
        /// <param name="TourId"></param>
        /// <param name="TourStatus"></param>
        /// <returns>-1:团队操作已结束 
        /// 1:成功 0：失败
        /// </returns>
        public int Update(string TourId, EyouSoft.Model.EnumType.TourStructure.TourStatus TourStatus)
        {
            if (string.IsNullOrEmpty(TourId)) return 0;

            int flg = dal.Update(TourId, TourStatus);
            if (flg == 1)
            {

                var log = new Model.CompanyStructure.SysHandleLogs();

                if (TourStatus == EyouSoft.Model.EnumType.TourStructure.TourStatus.未出发)
                {
                    log.EventTitle = "财务确认 登记件 审核";
                }

                if (TourStatus == EyouSoft.Model.EnumType.TourStructure.TourStatus.未处理)
                {
                    log.EventTitle = "财务确认 登记件 退回";
                }
                log.ModuleId = Model.EnumType.PrivsStructure.Privs2.财务管理_应收管理;

                log.EventMessage = "财务确认 登记件 退回计调，确认件编号：" + TourId + "。";

                new SysHandleLogs().Add(log);
            }
            return flg;
        }

        /// <summary>
        /// 财务管理 登记件——财务保存操作
        /// </summary>
        /// <param name="model"></param>
        /// <returns>
        /// -1:只有未处理的订单才能审核
        /// 1:成功 0：失败
        /// </returns>
        public int Update_Fin(EyouSoft.Model.TourStructure.MTourInfo model)
        {
            if (string.IsNullOrEmpty(model.TourId)
                || model.CompanyId == 0
                || !model.LDate.HasValue
                || !model.RDate.HasValue
                || string.IsNullOrEmpty(model.RouteName)
                || model.Adults + model.Childs + model.Accompanys == 0
                || model.SaleId == 0
                || string.IsNullOrEmpty(model.BuyCompanyId)
                || model.SumPrice == 0
                || model.ConfirmOperatorId == 0)
            {
                return 0;
            }

            int flg = dal.Update_Fin(model);

            if (flg == 1)
            {
                var log = new Model.CompanyStructure.SysHandleLogs
                {
                    EventTitle = "财务确认登记件",
                    ModuleId = Model.EnumType.PrivsStructure.Privs2.财务管理_应收管理,
                    EventMessage = "财务确认登记件，确认件编号：" + model.TourId + "。"
                };

                new SysHandleLogs().Add(log);
            }

            return flg;

        }

        /// <summary>
        /// 删除确认件
        /// </summary>
        /// <param name="TourId"></param>
        /// <returns>
        /// -1:做过地接安排不允许删除
        ///-2:做过出票或退票安排不允许删除
        ///-3:财务审核后不能删除
        ///-4:操作结束不允许删除
        ///1:成功 0：失败		
        /// </returns>
        public int Delete(string TourId)
        {
            int flg = dal.Delete(TourId);

            if (flg == 1)
            {
                var log = new Model.CompanyStructure.SysHandleLogs
                {
                    EventTitle = "删除确认件登记",
                    ModuleId = Model.EnumType.PrivsStructure.Privs2.计调中心_确认件登记,
                    EventMessage = "删除确认件登记，确认件编号：" + TourId + "。"
                };

                new SysHandleLogs().Add(log);
            }

            return flg;
        }


        /// <summary>
        /// 强制删除确认件
        /// </summary>
        /// <param name="TourId">团号</param>
        /// <returns>1:成功 0：失败	</returns>
        public int Delete_(string TourId)
        {
            int flg = dal.Delete_(TourId);
            if (flg == 1)
            {
                var log = new Model.CompanyStructure.SysHandleLogs
                {
                    EventTitle = "强制删除确认件",
                    ModuleId = Model.EnumType.PrivsStructure.Privs2.计调中心_确认件登记,
                    EventMessage = "强制删除确认件，确认件编号：" + TourId + "。"
                };

                new SysHandleLogs().Add(log);
            }

            return flg;
        }

        /// <summary>
        /// 根据团队编号获取实体
        /// </summary>
        /// <param name="TourId"></param>
        /// <returns></returns>
        public EyouSoft.Model.TourStructure.MTourInfo GetModel(string TourId)
        {
            if (string.IsNullOrEmpty(TourId)) return null;
            return dal.GetModel(TourId);
        }


        /// <summary>
        /// 分页获取确认件登记的列表
        /// </summary>
        /// <param name="companyId"></param>
        /// <param name="pageSize"></param>
        /// <param name="pageIndex"></param>
        /// <param name="recordCount"></param>
        /// <param name="search"></param>
        /// <returns></returns>
        public IList<EyouSoft.Model.TourStructure.MPageTour> GetList(
         int companyId,
         int pageSize,
         int pageIndex,
         ref int recordCount,
         EyouSoft.Model.TourStructure.MSearchTour search)
        {
            if (companyId == 0) return null;
            return dal.GetList(companyId, pageSize, pageIndex, ref recordCount, search);
        }


        /// <summary>
        /// 获取当日登记团，散个数
        /// </summary>
        /// <param name="companyId"></param>
        /// <param name="SanTour"></param>
        /// <param name="Tour"></param>
        public void GetTodayTour(int companyId, ref int SanTour, ref int Tour)
        {
            dal.GetTodayTour(companyId, ref SanTour, ref Tour);
        }

        /// <summary>
        /// 根据公司编号，团号获取团队列表
        /// </summary>
        /// <param name="companyId"></param>
        /// <param name="TourCode"></param>
        /// <param name="TourStatus">团队状态的数组</param>
        /// <returns></returns>
        public IList<EyouSoft.Model.TourStructure.MTour> GetList(int companyId, string TourCode, params int[] TourStatus)
        {
            if (companyId == 0) return null;
            return dal.GetList(companyId, TourCode, TourStatus);
        }
    }
}
