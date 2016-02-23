using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.Xml.Linq;
using EyouSoft.Common;

namespace Web.CustomerManage
{
    public partial class GuanhuaiEdit : EyouSoft.Common.Page.BackPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            string dotype = Utils.GetQueryStringValue("dotype");
            string id = Utils.GetQueryStringValue("id");
            string type = Utils.GetQueryStringValue("type").Trim();
            PowerControl();
            switch (type)
            {
                case "save":
                    Response.Clear();
                    Response.Write(PageSave(id, dotype));
                    Response.End();
                    break;
                default:
                    break;
            }
            if (!IsPostBack)
            {
                PageInit(id, dotype);
            }
        }
        /// <summary>
        /// 初始化
        /// </summary>
        /// <param name="id"></param>
        /// <param name="dotype"></param>
        private void PageInit(string id, string dotype)
        {
            if (id != "" && dotype == "update")
            {
                EyouSoft.BLL.CRM.BCustomerCare bll = new EyouSoft.BLL.CRM.BCustomerCare();
                EyouSoft.Model.CRM.MCustomerCare model = bll.GetCustomerCare(Utils.GetInt(id));
                if (model != null)
                {
                    this.txtoutliYou.Text = model.PayReason;
                    this.txtoutMoney.Value = model.PayMoney.ToString("f2");
                    this.txtvistor.Value = model.VisitName;
                    this.txtvistTime.Value = model.VisitTime.ToString("yyyy-MM-dd");
                    this.txtXiHao.Text = model.CustomerHobby;
                    this.CustomerUnit1.InitCustomerName = model.CustomerName;
                    this.CustomerUnit1.InitCustomerId = model.CustomerId;
                }
            }
        }
        private string PageSave(string id, string dotype)
        {
            //t为true 新增，false 修改
            bool t = string.IsNullOrEmpty(id) && dotype == "add";
            string msg = string.Empty;
            EyouSoft.BLL.CRM.BCustomerCare bll = new EyouSoft.BLL.CRM.BCustomerCare();
            EyouSoft.Model.CRM.MCustomerCare model = new EyouSoft.Model.CRM.MCustomerCare();
            model.CompanyId = SiteUserInfo.CompanyId;
            model.CustomerHobby = Utils.GetFormValue(this.txtXiHao.UniqueID);
            model.CustomerId = Utils.GetFormValue(this.CustomerUnit1.HidClientName);
            model.CustomerName = Utils.GetFormValue("txtCustomerName");
            model.IssueTime = DateTime.Now;
            model.OperatorId = SiteUserInfo.UserId;
            model.PayMoney = Utils.GetDecimal(Utils.GetFormValue(this.txtoutMoney.UniqueID));
            model.PayReason = Utils.GetFormValue(this.txtoutliYou.UniqueID);
            model.VisitName = Utils.GetFormValue(this.txtvistor.UniqueID);
            model.VisitTime = Utils.GetDateTime(Utils.GetFormValue(this.txtvistTime.UniqueID));
            int result = 0;
            if (t)
            {
                result = bll.AddCustomerCare(model);
            }
            else
            {
                model.CareId = Utils.GetInt(id);
                result = bll.UpdateCustomerCare(model);
            }
            switch (result)
            {
                case 0:
                    msg = UtilsCommons.AjaxReturnJson("0", "参数错误");
                    break;
                case 1:
                    msg = UtilsCommons.AjaxReturnJson("1", (t ? "添加" : "修改") + "成功");
                    break;
                case -1:
                    msg = UtilsCommons.AjaxReturnJson("0", "客户资料不存在");
                    break;
                case -2:
                    msg = UtilsCommons.AjaxReturnJson("0", (t ? "添加" : "修改") + "失败");
                    break;
                default:
                    break;
            }
            return msg;
        }

        /// <summary>
        /// 权限判断
        /// </summary>
        protected void PowerControl()
        {
            if (!this.CheckGrant(EyouSoft.Model.EnumType.PrivsStructure.Privs3.客户管理_客户关怀_新增))
            {
                this.btn.Visible = false;
            }
            if (!this.CheckGrant(EyouSoft.Model.EnumType.PrivsStructure.Privs3.客户管理_客户关怀_修改))
            {
                this.btn.Visible = false;
            }
        }
    }
}
