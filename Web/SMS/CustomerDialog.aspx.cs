using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using EyouSoft.Common;
using EyouSoft.Common.Function;

namespace Web.SMS
{
    /// <summary>
    /// 短信中心选择客户号码弹窗
    /// xuty 2011/1/21
    /// </summary>
    public partial class CustomerDialog : EyouSoft.Common.Page.BackPage
    {
        protected int pageIndex;
        protected int recordCount;
        protected int pageSize = 20;
        protected int itemIndex = 1;
        protected void Page_Load(object sender, EventArgs e)
        {
            //判断权限
            if (!CheckGrant(EyouSoft.Model.EnumType.PrivsStructure.Privs3.短信中心_短信中心_栏目))
            {
                Utils.ResponseNoPermit(EyouSoft.Model.EnumType.PrivsStructure.Privs3.短信中心_短信中心_栏目, true);
                return;
            }
            #region 绑定客户列表
            pageIndex = Utils.GetInt(Utils.GetQueryStringValue("Page"), 1);
            itemIndex = (pageIndex - 1) * pageSize + 1;
            //获取查询条件
            string userName = Utils.GetQueryStringValue("username");//姓名
            string mobile = Utils.GetQueryStringValue("mobile");//手机号
            string companyname = Utils.GetQueryStringValue("companyname");//单位名称

            #endregion


            #region 导出客户Excel
            //导出Excel
            var customerBll = new EyouSoft.BLL.CRM.BCustomer();//客户bll
            //查询条件实体
            var searchModel = new EyouSoft.Model.CRM.MSearchCustomer
                { ContactName = userName, Mobile = mobile, CustomerName = companyname };
            IList<EyouSoft.Model.CRM.MCustomer> list = customerBll.GetCustomerContactInfo(
                CurrentUserCompanyID, pageSize, pageIndex, ref recordCount, searchModel);

            if (list != null && list.Count > 0)
            {
                rptCustomer.DataSource = list;
                rptCustomer.DataBind();
                BindExportPage();
            }
            else
            {
                rptCustomer.EmptyText = "<tr><td colspan='7' align='center'>对不起，暂无客户信息！</td></tr>";
                ExportPageInfo1.Visible = false;
            }
            #endregion

            //恢复查询条件
            txtCompanyName.Value = companyname;//单位名称
            txtMobile.Value = mobile;//手机号
            txtUserName.Value = userName;//姓名
        }
        #region 绑定分页控件
        /// <summary>
        /// 绑定分页控件
        /// </summary>
        protected void BindExportPage()
        {
            this.ExportPageInfo1.intPageSize = pageSize;
            this.ExportPageInfo1.CurrencyPage = pageIndex;
            this.ExportPageInfo1.intRecordCount = recordCount;
        }
        #endregion


    }
}
