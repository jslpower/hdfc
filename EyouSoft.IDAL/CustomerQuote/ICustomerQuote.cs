using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EyouSoft.IDAL.CustomerQuote
{
    /// <summary>
    /// 客户日常询价数据接口 
    /// </summary>
    public interface ICustomerQuote
    {
        /// <summary>
        /// 添加客户日常询价
        /// </summary>
        /// <param name="model">客户日常询价实体</param>
        /// <returns>
        /// 1：成功；
        /// 0：参数错误；
        /// -1：添加失败；
        /// </returns>
        int AddCustomerQuote(Model.CustomerQuote.MCustomerQuote model);

        /// <summary>
        /// 修改客户日常询价
        /// </summary>
        /// <param name="model">客户日常询价实体</param>
        /// <returns>
        /// 1：成功；
        /// 0：参数错误；
        /// -1：修改失败；
        /// </returns>
        int UpdateCustomerQuote(Model.CustomerQuote.MCustomerQuote model);

        /// <summary>
        /// 删除客户日常询价
        /// </summary>
        /// <param name="id">客户日常询价编号</param>
        /// <returns>
        /// 1：成功；
        /// 0：参数错误；
        /// -1：删除失败；
        /// </returns>
        int DeleteCustomerQuote(params int[] id);

        /// <summary>
        /// 获取客户日常询价
        /// </summary>
        /// <param name="id">客户日常询价编号</param>
        /// <returns></returns>
        Model.CustomerQuote.MCustomerQuote GetCustomerQuote(int id);

        /// <summary>
        /// 获取客户日常询价
        /// </summary>
        /// <param name="companyId">公司编号</param>
        /// <param name="pageSize">每页记录数</param>
        /// <param name="pageIndex">当前页数</param>
        /// <param name="recordCount">总记录数</param>
        /// <param name="seach">客户日常询价查询实体</param>
        /// <returns></returns>
        IList<Model.CustomerQuote.MCustomerQuote> GetCustomerQuote(
            int companyId,
            int pageSize,
            int pageIndex,
            ref int recordCount,
            Model.CustomerQuote.MSearchCustomerQuote seach);
    }
}
