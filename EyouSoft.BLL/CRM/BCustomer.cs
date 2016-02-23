using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using EyouSoft.BLL.CompanyStructure;

namespace EyouSoft.BLL.CRM
{
    /// <summary>
    /// 客户资料业务逻辑
    /// </summary>
    public class BCustomer : BLLBase
    {
        private readonly IDAL.CRM.ICustomer _dal = Component.Factory.ComponentFactory.CreateDAL<IDAL.CRM.ICustomer>();

        /// <summary>
        /// 添加客户资料
        /// </summary>
        /// <param name="model">客户资料实体</param>
        /// <returns>
        /// 1：成功；
        /// 0：参数错误；
        /// -1：添加失败；
        /// -2: 客户名存在
        /// </returns>
        public int AddCustomer(Model.CRM.MCustomer model)
        {
            if (model == null || string.IsNullOrEmpty(model.CustomerName) || model.SaleAreadId <= 0) return 0;

            if (this.IsExistsCustomerName(model.CustomerName))
            {
                return -2;
            }

            int dalRetCode = _dal.AddCustomer(model);
            if (dalRetCode == 1)
            {
                var log = new Model.CompanyStructure.SysHandleLogs
                {
                    EventTitle = "新增客户资料",
                    ModuleId = Model.EnumType.PrivsStructure.Privs2.客户管理_客户资料,
                    EventMessage = "新增客户资料，客户资料编号：" + model.Id + "。"
                };

                new SysHandleLogs().Add(log);
            }

            return dalRetCode;
        }

        /// <summary>
        /// 修改客户资料
        /// </summary>
        /// <param name="model">客户资料实体</param>
        /// <returns>
        /// 1：成功；
        /// 0：参数错误；
        /// -1：修改失败；
        /// -2:客户名存在
        /// </returns>
        public int UpdateCustomer(Model.CRM.MCustomer model)
        {
            if (model == null || string.IsNullOrEmpty(model.CustomerName) || model.SaleAreadId <= 0 || string.IsNullOrEmpty(model.Id))
                return 0;
            if (_dal.IsExistsCustomerName(model.CustomerName, model.Id))
            {
                return -2;
            }
            int dalRetCode = _dal.UpdateCustomer(model);
            if (dalRetCode == 1)
            {
                var log = new Model.CompanyStructure.SysHandleLogs
                {
                    EventTitle = "修改客户资料",
                    ModuleId = Model.EnumType.PrivsStructure.Privs2.客户管理_客户资料,
                    EventMessage = "修改客户资料，客户资料编号：" + model.Id + "。"
                };

                new SysHandleLogs().Add(log);
            }

            return dalRetCode;
        }

        /// <summary>
        /// 删除客户资料
        /// </summary>
        /// <param name="id">客户编号</param>
        /// <returns>
        /// 1：成功；
        /// 0：参数错误；
        /// -1：客户已被使用，不能删除；
        /// -2：删除失败；
        /// </returns>
        public int DeleteCustomer(params string[] id)
        {
            if (id == null || id.Length <= 0) return 0;

            int dalRetCode = _dal.DeleteCustomer(id);
            if (dalRetCode == 1)
            {
                var log = new Model.CompanyStructure.SysHandleLogs
                {
                    EventTitle = "删除客户资料",
                    ModuleId = Model.EnumType.PrivsStructure.Privs2.客户管理_客户资料,
                    EventMessage = "删除客户资料，客户资料编号：" + this.GetIdsByArr(id) + "。"
                };

                new SysHandleLogs().Add(log);
            }

            return dalRetCode;
        }

        /// <summary>
        /// 获取客户资料
        /// </summary>
        /// <param name="id">客户编号</param>
        /// <returns></returns>
        public Model.CRM.MCustomer GetCustomer(string id)
        {
            if (string.IsNullOrEmpty(id)) return null;

            return _dal.GetCustomer(id);
        }

        /// <summary>
        /// 获取客户资料
        /// </summary>
        /// <param name="companyId">公司编号</param>
        /// <param name="pageSize">每页记录数</param>
        /// <param name="pageIndex">当前页数</param>
        /// <param name="recordCount">总记录数</param>
        /// <param name="seach">客户资料查询实体</param>
        /// <returns></returns>
        public IList<Model.CRM.MCustomer> GetCustomer(
            int companyId,
            int pageSize,
            int pageIndex,
            ref int recordCount,
            Model.CRM.MSearchCustomer seach)
        {
            if (companyId <= 0 || pageSize <= 0 || pageIndex <= 0) return null;

            return _dal.GetCustomer(companyId, pageSize, pageIndex, ref recordCount, seach);
        }


        /// <summary>
        /// 获取客户联系人
        /// </summary>
        /// <param name="companyId">公司编号</param>
        /// <param name="pageSize">每页记录数</param>
        /// <param name="pageIndex">当前页数</param>
        /// <param name="recordCount">总记录数</param>
        /// <param name="seach">客户资料查询实体</param>
        /// <returns></returns>
        public IList<Model.CRM.MCustomer> GetCustomerContactInfo(
            int companyId,
            int pageSize,
            int pageIndex,
            ref int recordCount,
            Model.CRM.MSearchCustomer seach)
        {
            if (companyId <= 0 || pageSize <= 0 || pageIndex <= 0) return null;

            return _dal.GetCustomerContactInfo(companyId, pageSize, pageIndex, ref recordCount, seach);
        }

        /// <summary>
        /// 获取客户资料
        /// </summary>
        /// <param name="companyId">公司编号</param>
        /// <param name="seach">客户资料查询实体</param>
        /// <returns></returns>
        public IList<Model.CRM.MCustomer> GetCustomer(
            int companyId,
            Model.CRM.MSearchCustomer seach)
        {
            if (companyId <= 0) return null;

            return _dal.GetCustomer(companyId, seach);
        }

        /// <summary>
        /// 根据客户编号获取联系人信息（不含主要联系人）
        /// </summary>
        /// <param name="customerId">客户编号</param>
        /// <returns></returns>
        public IList<Model.CRM.MCustomerContact> GetCustomerContact(string customerId)
        {
            if (string.IsNullOrEmpty(customerId)) return null;

            return _dal.GetCustomerContact(customerId);
        }


        /// <summary>
        /// 设置客户信用等级
        /// </summary>
        /// <param name="cid"></param>
        /// <returns></returns>
        public bool SetRating(string cid, int rid, int OperatorId)
        {
            if (string.IsNullOrEmpty(cid) || string.IsNullOrEmpty(rid.ToString()) || OperatorId == 0) return false;
            if (_dal.SetRating(cid, rid, OperatorId))
            {
                var log = new Model.CompanyStructure.SysHandleLogs
                {
                    EventTitle = "设置客户信用等级",
                    ModuleId = Model.EnumType.PrivsStructure.Privs2.客户管理_客户资料,
                    EventMessage = "设置客户信用等级，编号：" + cid + "。"
                };

                new SysHandleLogs().Add(log);

                return true;
            }
            return false;
        }

        /// <summary>
        /// 判断客户是否存在
        /// </summary>
        /// <param name="customer"></param>
        /// <returns></returns>
        public bool IsExistsCustomerName(string customerName)
        {
            if (string.IsNullOrEmpty(customerName)) return true;
            return _dal.IsExistsCustomerName(customerName.Trim(), null);

        }
    }
}
