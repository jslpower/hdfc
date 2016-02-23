using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using EyouSoft.BLL.CompanyStructure;

namespace EyouSoft.BLL.CRM
{
    /// <summary>
    /// 客户关怀业务逻辑
    /// </summary>
    public class BCustomerCare :BLLBase
    {
        private readonly IDAL.CRM.ICustomerCare _dal =
            Component.Factory.ComponentFactory.CreateDAL<IDAL.CRM.ICustomerCare>();

        /// <summary>
        /// 添加客户关怀
        /// </summary>
        /// <param name="model">客户关怀实体</param>
        /// <returns>
        /// 1：成功；
        /// 0：参数错误；
        /// -1：客户资料不存在；
        /// -2：添加失败；
        /// </returns>
        public int AddCustomerCare(Model.CRM.MCustomerCare model)
        {
            if (model == null || model.CompanyId <= 0 || string.IsNullOrEmpty(model.CustomerId)) return 0;

            int dalRetCode = _dal.AddCustomerCare(model);
            if (dalRetCode == 1)
            {
                var log = new Model.CompanyStructure.SysHandleLogs
                {
                    EventTitle = "新增客户关怀",
                    ModuleId = Model.EnumType.PrivsStructure.Privs2.客户管理_客户关怀,
                    EventMessage = "新增客户关怀，客户关怀编号：" + model.CareId + "。"
                };

                new SysHandleLogs().Add(log);
            }

            return dalRetCode;
        }

        /// <summary>
        /// 修改客户关怀
        /// </summary>
        /// <param name="model">客户关怀实体</param>
        /// <returns>
        /// 1：成功；
        /// 0：参数错误；
        /// -1：客户资料不存在；
        /// -2：修改失败；
        /// </returns>
        public int UpdateCustomerCare(Model.CRM.MCustomerCare model)
        {
            if (model == null || model.CompanyId <= 0 || string.IsNullOrEmpty(model.CustomerId) || model.CareId <= 0) return 0;

            int dalRetCode = _dal.UpdateCustomerCare(model);
            if (dalRetCode == 1)
            {
                var log = new Model.CompanyStructure.SysHandleLogs
                {
                    EventTitle = "修改客户关怀",
                    ModuleId = Model.EnumType.PrivsStructure.Privs2.客户管理_客户关怀,
                    EventMessage = "修改客户关怀，客户关怀编号：" + model.CareId + "。"
                };

                new SysHandleLogs().Add(log);
            }

            return dalRetCode;
        }

        /// <summary>
        /// 删除客户关怀
        /// </summary>
        /// <param name="id">客户关怀编号</param>
        /// <returns>
        /// 1：成功；
        /// 0：参数错误；
        /// -1：删除失败；
        /// </returns>
        public int DeleteCustomerCare(params int[] id)
        {
            if (id == null || id.Length <= 0) return 0;

            int dalRetCode = _dal.DeleteCustomerCare(id);
            if (dalRetCode == 1)
            {
                var log = new Model.CompanyStructure.SysHandleLogs
                {
                    EventTitle = "删除客户关怀",
                    ModuleId = Model.EnumType.PrivsStructure.Privs2.客户管理_客户关怀,
                    EventMessage = "删除客户关怀，客户关怀编号：" + this.GetIdsByArr(id) + "。"
                };

                new SysHandleLogs().Add(log);
            }

            return dalRetCode;
        }

        /// <summary>
        /// 获取客户关怀
        /// </summary>
        /// <param name="id">客户关怀编号</param>
        /// <returns></returns>
        public Model.CRM.MCustomerCare GetCustomerCare(int id)
        {
            if (id <= 0) return null;

            return _dal.GetCustomerCare(id);
        }

        /// <summary>
        /// 获取客户关怀
        /// </summary>
        /// <param name="companyId">公司编号</param>
        /// <param name="pageSize">每页记录数</param>
        /// <param name="pageIndex">当前页数</param>
        /// <param name="recordCount">总记录数</param>
        /// <param name="seach">客户关怀查询实体</param>
        /// <returns></returns>
        public IList<Model.CRM.MCustomerCare> GetCustomerCare(
            int companyId,
            int pageSize,
            int pageIndex,
            ref int recordCount,
            Model.CRM.MSearchCustomerCare seach)
        {
            if (companyId <= 0 || pageSize <= 0 || pageIndex <= 0) return null;

            return _dal.GetCustomerCare(companyId, pageSize, pageIndex, ref recordCount, seach);
        }
    }
}
