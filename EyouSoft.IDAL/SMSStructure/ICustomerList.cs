using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EyouSoft.IDAL.SMSStructure
{
    /// <summary>
    /// 短信中心-客户列表及客户类型数据访问接口
    /// Author：xuqh 2011-01-21
    /// </summary>
    public interface ICustomerList
    {
        /// <summary>
        /// 添加客户类型信息
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        int AddCustomClass(EyouSoft.Model.SMSStructure.CustomerClass model);

        /// <summary>
        /// 删除客户类型信息
        /// </summary>
        /// <param name="Id">客户类型编号</param>
        /// <returns></returns>
        bool DeleteCustomClass(int Id);

        /// <summary>
        /// 跟据公司ID获取该公司所有客户类型信息
        /// </summary>
        /// <param name="CompanyID">公司编号</param>
        /// <returns></returns>
        IList<EyouSoft.Model.SMSStructure.CustomerClass> GetCustomerClass(int CompanyID);

        /// <summary>
        /// 添加客户信息
        /// </summary>
        /// <param name="?"></param>
        /// <returns></returns>
        bool AddCustomerList(EyouSoft.Model.SMSStructure.CustomerList model);


        /// <summary>
        /// 修改客户
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        bool UpdateCustomerList(EyouSoft.Model.SMSStructure.CustomerList model);

        /// <summary>
        /// 删除客户信息
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        bool DeleteCustomerList(string[] Ids);

        /// <summary>
        /// 获取一个客户实体
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        EyouSoft.Model.SMSStructure.CustomerList GetCustomer(string Id);

        /// <summary>
        /// 获取客户总数
        /// </summary>
        /// <returns></returns>
        //public int GetCustomersCount();

        /// <summary>
        /// 分页获取客户列表
        /// </summary>
        /// <param name="pageSize">每页记录数</param>
        /// <param name="pageIndex">当前页数</param>
        /// <param name="RecordCount">总记录数</param>
        /// <param name="CustomerCompanyName">客户单位名称（为空不做查询条件）</param>
        /// <param name="CustomerContactName">客户姓名（为空不做查询条件）</param>
        /// <param name="MobileNumber">手机号码（为空不做查询条件）</param>
        /// <param name="CustomClassId">客户分类ID</param>
        /// <returns></returns>
        IList<EyouSoft.Model.SMSStructure.CustomerList> GetList(int pageSize, int pageIndex, ref int recordCount, string companyId, string customerCompanyName, string customerUserFullName, string mobile, int categoryId);

        /// <summary>
        /// 判断客户手机是否存在
        /// </summary>
        /// <param name="CompanyID">公司编号</param>
        /// <param name="Id">客户编号</param>
        /// <param name="MobileNumber">手机号码</param>
        /// <returns></returns>
        bool IsExistCustomerMobile(int CompanyID, string Id, string MobileNumber);
    }
}
