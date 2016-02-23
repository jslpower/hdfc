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
    /// 短信客户列表
    /// xuty 2011/1/24
    /// </summary>
    public partial class SmsCustomerList : EyouSoft.Common.Page.BackPage
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
            EyouSoft.BLL.SMSStructure.CustomerList custBll = new EyouSoft.BLL.SMSStructure.CustomerList();
            //绑定客户类别
             IList<EyouSoft.Model.SMSStructure.CustomerClass> classList = custBll.GetCustomerClass(CurrentUserCompanyID);
            if (classList != null && classList.Count > 0)
            {
                selCustType.DataTextField = "ClassName";
                selCustType.DataValueField = "ID";
                selCustType.DataSource = classList;
                selCustType.DataBind();
            }
            selCustType.Items.Insert(0, new ListItem("请选择", ""));

            string method = Utils.GetQueryStringValue("method");
            pageIndex = Utils.GetInt(Utils.GetQueryStringValue("Page"), 1);
            itemIndex = (pageIndex - 1) * pageSize + 1;
            //删除客户
            if (method == "del")
            {   
                string custid = Utils.GetQueryStringValue("custid");
                bool result=custBll.DeleteCustomerList(custid.Split(','));
                Utils.ResponseMeg(result, string.Empty);
                return;
            }
            //查询条件
            string userName = Request.QueryString["username"];//客户名
            userName = !string.IsNullOrEmpty(userName) ? Utils.InputText(Server.UrlDecode(userName)) : "";
            string mobile = Request.QueryString["mobile"];//手机号
            mobile = !string.IsNullOrEmpty(mobile) ? Utils.InputText(Server.UrlDecode(mobile)) : "";
            string companyname = Request.QueryString["companyname"];//单位名称
            companyname = !string.IsNullOrEmpty(companyname) ? Utils.InputText(Server.UrlDecode(companyname)) : "";
            string custtype = Utils.GetQueryStringValue("custtype");//客户类别
            //绑定客户列表
            IList<EyouSoft.Model.SMSStructure.CustomerList> list = custBll.GetList(pageSize, pageIndex, ref recordCount, CurrentUserCompanyID.ToString(), companyname, userName, mobile, Utils.GetInt(custtype));
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
            //恢复查询条件
            txtCompanyName.Value = companyname;//单位名称
            txtMobile.Value = mobile;//手机号
            selCustType.Value = custtype;//客户类别
            txtUserName.Value = userName;//客户名
            
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
