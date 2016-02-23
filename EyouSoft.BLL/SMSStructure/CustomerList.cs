using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EyouSoft.BLL.SMSStructure
{
    /// <summary>
    /// 短信中心-客户列表及客户类型数据访问接口
    /// Author：xuqh 2011-01-22
    /// </summary>
    public class CustomerList
    {
        private readonly EyouSoft.IDAL.SMSStructure.ICustomerList Dal = EyouSoft.Component.Factory.ComponentFactory.CreateDAL<EyouSoft.IDAL.SMSStructure.ICustomerList>();
        private readonly BLL.CompanyStructure.SysHandleLogs handleLogsBll = new BLL.CompanyStructure.SysHandleLogs();

        /// <summary>
        /// 添加客户类型信息,返回0插入失败,>0时为插入的类型编号
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public int AddCustomClass(EyouSoft.Model.SMSStructure.CustomerClass model)
        {
            int result = 0;
            result = Dal.AddCustomClass(model);
            handleLogsBll.Add(AddLogs("新增客户类型", result == 0 ? false : true));
            return result;
        }

        /// <summary>
        /// 删除客户类型信息
        /// </summary>
        /// <param name="Id">客户类型编号</param>
        /// <returns></returns>
        public bool DeleteCustomClass(int Id)
        {
            bool result = false;
            result = Dal.DeleteCustomClass(Id);
            handleLogsBll.Add(AddLogs("删除了客户类型", result));
            return result;
        }

        /// <summary>
        /// 跟据公司ID获取该公司所有客户类型信息
        /// </summary>
        /// <param name="CompanyID">公司编号</param>
        /// <returns></returns>
        public IList<EyouSoft.Model.SMSStructure.CustomerClass> GetCustomerClass(int CompanyID)
        {
            return Dal.GetCustomerClass(CompanyID);
        }

        /// <summary>
        /// 添加客户信息
        /// </summary>
        /// <param name="?"></param>
        /// <returns></returns>
        public bool AddCustomerList(EyouSoft.Model.SMSStructure.CustomerList model)
        {
            if (model == null)
            {
                return false;
            }

            bool result = false;
            result = Dal.AddCustomerList(model);
            handleLogsBll.Add(AddLogs("新增了客户信息", result));
            return result;
        }


        /// <summary>
        /// 修改客户
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public bool UpdateCustomerList(EyouSoft.Model.SMSStructure.CustomerList model)
        {
            bool result = false;
            result = Dal.UpdateCustomerList(model);
            handleLogsBll.Add(AddLogs("修改了客户信息", result));
            return result;
        }

        /// <summary>
        /// 删除客户信息
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        public bool DeleteCustomerList(string[] Ids)
        {
            bool result = false;
            result = Dal.DeleteCustomerList(Ids);
            handleLogsBll.Add(AddLogs("删除了客户信息", result));
            return result;
        }

        /// <summary>
        /// 获取一个客户实体
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        public EyouSoft.Model.SMSStructure.CustomerList GetCustomer(string Id)
        {
            return Dal.GetCustomer(Id);
        }

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
        public IList<EyouSoft.Model.SMSStructure.CustomerList> GetList(int pageSize, int pageIndex, ref int recordCount, string companyId, string customerCompanyName, string customerUserFullName, string mobile, int categoryId)
        {
            return Dal.GetList(pageSize, pageIndex, ref recordCount,companyId, customerCompanyName, customerUserFullName, mobile, categoryId);
        }

        /// <summary>
        /// 判断客户手机是否存在
        /// </summary>
        /// <param name="CompanyID">公司编号</param>
        /// <param name="Id">客户编号</param>
        /// <param name="MobileNumber">手机号码</param>
        /// <returns></returns>
        public bool IsExistCustomerMobile(int CompanyID, string Id, string MobileNumber)
        {
            return Dal.IsExistCustomerMobile(CompanyID, Id, MobileNumber);
        }

        /// <summary>
        /// 添加日志记录
        /// </summary>
        /// <param name="areaModel">日志操作实体</param>
        /// <param name="actionName">操作名称</param>
        /// <param name="flag">操作状态</param>
        /// <returns></returns>
        private EyouSoft.Model.CompanyStructure.SysHandleLogs AddLogs(string actionName, bool flag)
        {
            EyouSoft.Model.CompanyStructure.SysHandleLogs model = new EyouSoft.Model.CompanyStructure.SysHandleLogs();
            model.ModuleId = Model.EnumType.PrivsStructure.Privs2.短信中心_短信中心;
            model.EventCode = Model.CompanyStructure.SysHandleLogsNO.EventCode;
            model.EventMessage = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "{0}在" + Model.EnumType.PrivsStructure.Privs2.短信中心_短信中心 + (flag ? actionName : actionName + "失败") + "!";
            model.EventTitle = (flag ? actionName + "在" : actionName + "失败在") + Model.EnumType.PrivsStructure.Privs2.短信中心_短信中心;

            return model;
        }
    }
}
