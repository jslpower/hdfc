using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using EyouSoft.BLL.CompanyStructure;

namespace EyouSoft.BLL.CustomerQuote
{
    /// <summary>
    /// 客户日常询价业务逻辑
    /// </summary>
    public class BCustomerQuote : BLLBase
    {
        private readonly IDAL.CustomerQuote.ICustomerQuote _dal =
            Component.Factory.ComponentFactory.CreateDAL<IDAL.CustomerQuote.ICustomerQuote>();

        /// <summary>
        /// 添加客户日常询价
        /// </summary>
        /// <param name="model">客户日常询价实体</param>
        /// <returns>
        /// 1：成功；
        /// 0：参数错误；
        /// -1：添加失败；
        /// </returns>
        public int AddCustomerQuote(Model.CustomerQuote.MCustomerQuote model)
        {
            if (model == null || model.CompanyId <= 0 || string.IsNullOrEmpty(model.CostomerId) || model.PeopleNum <= 0) return 0;

            int dalRetCode = _dal.AddCustomerQuote(model);
            if (dalRetCode == 1)
            {
                var log = new Model.CompanyStructure.SysHandleLogs
                {
                    EventTitle = "新增客户日常询价",
                    ModuleId = Model.EnumType.PrivsStructure.Privs2.客户询价_客户日常询价,
                    EventMessage = "新增客户日常询价，客户日常询价编号：" + model.QuoteId + "。"
                };

                new SysHandleLogs().Add(log);
            }

            return dalRetCode;
        }

        /// <summary>
        /// 修改客户日常询价
        /// </summary>
        /// <param name="model">客户日常询价实体</param>
        /// <returns>
        /// 1：成功；
        /// 0：参数错误；
        /// -1：修改失败；
        /// </returns>
        public int UpdateCustomerQuote(Model.CustomerQuote.MCustomerQuote model)
        {
            if (model == null || string.IsNullOrEmpty(model.CostomerId) || model.PeopleNum <= 0 || model.QuoteId <= 0) return 0;

            int dalRetCode = _dal.UpdateCustomerQuote(model);
            if (dalRetCode == 1)
            {
                var log = new Model.CompanyStructure.SysHandleLogs
                {
                    EventTitle = "修改客户日常询价",
                    ModuleId = Model.EnumType.PrivsStructure.Privs2.客户询价_客户日常询价,
                    EventMessage = "修改客户日常询价，客户日常询价编号：" + model.QuoteId + "。"
                };

                new SysHandleLogs().Add(log);
            }

            return dalRetCode;
        }

        /// <summary>
        /// 删除客户日常询价
        /// </summary>
        /// <param name="id">客户日常询价编号</param>
        /// <returns>
        /// 1：成功；
        /// 0：参数错误；
        /// -1：删除失败；
        /// </returns>
        public int DeleteCustomerQuote(params int[] id)
        {
            if (id == null || id.Length <= 0) return 0;

            int dalRetCode = _dal.DeleteCustomerQuote(id);
            if (dalRetCode == 1)
            {
                var log = new Model.CompanyStructure.SysHandleLogs
                {
                    EventTitle = "删除客户日常询价",
                    ModuleId = Model.EnumType.PrivsStructure.Privs2.客户询价_客户日常询价,
                    EventMessage = "删除客户日常询价，客户日常询价编号：" + this.GetIdsByArr(id) + "。"
                };

                new SysHandleLogs().Add(log);
            }

            return dalRetCode;
        }

        /// <summary>
        /// 获取客户日常询价
        /// </summary>
        /// <param name="id">客户日常询价编号</param>
        /// <returns></returns>
        public Model.CustomerQuote.MCustomerQuote GetCustomerQuote(int id)
        {
            if (id <= 0) return null;

            return _dal.GetCustomerQuote(id);
        }

        /// <summary>
        /// 获取客户日常询价
        /// </summary>
        /// <param name="companyId">公司编号</param>
        /// <param name="pageSize">每页记录数</param>
        /// <param name="pageIndex">当前页数</param>
        /// <param name="recordCount">总记录数</param>
        /// <param name="seach">客户日常询价查询实体</param>
        /// <returns></returns>
        public IList<Model.CustomerQuote.MCustomerQuote> GetCustomerQuote(
            int companyId,
            int pageSize,
            int pageIndex,
            ref int recordCount,
            Model.CustomerQuote.MSearchCustomerQuote seach)
        {
            if (companyId <= 0 || pageSize <= 0 || pageIndex <= 0) return null;

            return _dal.GetCustomerQuote(companyId, pageSize, pageIndex, ref recordCount, seach);
        }
    }
}
