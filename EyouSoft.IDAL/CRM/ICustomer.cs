using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EyouSoft.IDAL.CRM
{
    /// <summary>
    /// 客户资料数据接口
    /// </summary>
    public interface ICustomer
    {
        /// <summary>
        /// 添加客户资料
        /// </summary>
        /// <param name="model">客户资料实体</param>
        /// <returns>
        /// 1：成功；
        /// 0：参数错误；
        /// -1：添加失败；
        /// </returns>
        int AddCustomer(Model.CRM.MCustomer model);

        /// <summary>
        /// 修改客户资料
        /// </summary>
        /// <param name="model">客户资料实体</param>
        /// <returns>
        /// 1：成功；
        /// 0：参数错误；
        /// -1：修改失败；
        /// </returns>
        int UpdateCustomer(Model.CRM.MCustomer model);

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
        int DeleteCustomer(params string[] id);

        /// <summary>
        /// 获取客户资料
        /// </summary>
        /// <param name="id">客户编号</param>
        /// <returns></returns>
        Model.CRM.MCustomer GetCustomer(string id);

        /// <summary>
        /// 获取客户资料
        /// </summary>
        /// <param name="companyId">公司编号</param>
        /// <param name="pageSize">每页记录数</param>
        /// <param name="pageIndex">当前页数</param>
        /// <param name="recordCount">总记录数</param>
        /// <param name="seach">客户资料查询实体</param>
        /// <returns></returns>
        IList<Model.CRM.MCustomer> GetCustomer(
            int companyId,
            int pageSize,
            int pageIndex,
            ref int recordCount,
            Model.CRM.MSearchCustomer seach);




        /// <summary>
        /// 获取客户联系人
        /// </summary>
        /// <param name="companyId">公司编号</param>
        /// <param name="pageSize">每页记录数</param>
        /// <param name="pageIndex">当前页数</param>
        /// <param name="recordCount">总记录数</param>
        /// <param name="seach">客户资料查询实体</param>
        /// <returns></returns>
        IList<Model.CRM.MCustomer> GetCustomerContactInfo(
            int companyId,
            int pageSize,
            int pageIndex,
            ref int recordCount,
            Model.CRM.MSearchCustomer seach);

        /// <summary>
        /// 获取客户资料
        /// </summary>
        /// <param name="companyId">公司编号</param>
        /// <param name="seach">客户资料查询实体</param>
        /// <returns></returns>
        IList<Model.CRM.MCustomer> GetCustomer(
            int companyId,
            Model.CRM.MSearchCustomer seach);

        /// <summary>
        /// 根据客户编号获取联系人信息（不含主要联系人）
        /// </summary>
        /// <param name="customerId">客户编号</param>
        /// <returns></returns>
        IList<Model.CRM.MCustomerContact> GetCustomerContact(string customerId);

        /// <summary>
        /// 判断客户是否存在
        /// </summary>
        /// <param name="customer"></param>
        /// <returns></returns>
         bool IsExistsCustomerName(string customerName, string customerId);

        /// <summary>
        /// 设置客户的信用等级
        /// </summary>
        /// <param name="cid">客户Id</param>
        /// <param name="rid">信用等级Id</param>
        /// <param name="OperatorId">操作人</param>
        /// <returns></returns>
         bool SetRating(string cid, int rid, int OperatorId);
    }
}
