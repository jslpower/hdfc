using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EyouSoft.IDAL.CRM
{
    /// <summary>
    /// 客户关怀数据接口
    /// </summary>
    public interface ICustomerCare
    {
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
        int AddCustomerCare(Model.CRM.MCustomerCare model);

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
        int UpdateCustomerCare(Model.CRM.MCustomerCare model);

        /// <summary>
        /// 删除客户关怀
        /// </summary>
        /// <param name="id">客户关怀编号</param>
        /// <returns>
        /// 1：成功；
        /// 0：参数错误；
        /// -1：删除失败；
        /// </returns>
        int DeleteCustomerCare(params int[] id);

        /// <summary>
        /// 获取客户关怀
        /// </summary>
        /// <param name="id">客户关怀编号</param>
        /// <returns></returns>
        Model.CRM.MCustomerCare GetCustomerCare(int id);

        /// <summary>
        /// 获取客户关怀
        /// </summary>
        /// <param name="companyId">公司编号</param>
        /// <param name="pageSize">每页记录数</param>
        /// <param name="pageIndex">当前页数</param>
        /// <param name="recordCount">总记录数</param>
        /// <param name="seach">客户关怀查询实体</param>
        /// <returns></returns>
        IList<Model.CRM.MCustomerCare> GetCustomerCare(
            int companyId,
            int pageSize,
            int pageIndex,
            ref int recordCount,
            Model.CRM.MSearchCustomerCare seach);
    }
}
