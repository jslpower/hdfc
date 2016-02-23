using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using EyouSoft.Common;

namespace Web.CustomerManage
{
    /// <summary>
    /// 编辑客户询价信息
    /// </summary>
    public partial class CustomerQuoteEdit : EyouSoft.Common.Page.BackPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            string save = Utils.GetQueryStringValue("save");
            string action = Utils.GetQueryStringValue("action").ToLower();
            int qId = Utils.GetInt(Utils.GetQueryStringValue("qId"));
            if (!string.IsNullOrEmpty(save))
            {
                Save(action, qId);
                return;
            }

            if (!IsPostBack)
            {
                CheckPrive(action, qId);
            }
        }

        /// <summary>
        /// 权限验证和初始化页面
        /// </summary>
        /// <param name="qId">询价编号</param>
        /// <param name="action">操作行为</param>
        private void CheckPrive(string action, int qId)
        {
            switch (action)
            {
                case "edit":
                    if (!CheckGrant(EyouSoft.Model.EnumType.PrivsStructure.Privs3.客户询价_客户日常询价_修改))
                    {
                        Utils.ShowMsgAndCloseBoxy(
                            string.Format(
                                "您没有{0}的权限，请联系管理员！", EyouSoft.Model.EnumType.PrivsStructure.Privs3.客户询价_客户日常询价_修改),
                            Utils.GetQueryStringValue("iframeId"),
                            false);
                        return;
                    }
                    if (qId <= 0)
                    {
                        Utils.ShowMsgAndCloseBoxy("url错误，请重新打开此窗口！", Utils.GetQueryStringValue("iframeId"), false);
                        return;
                    }
                    InitPage(action, qId);
                    break;
                case "see":
                    if (qId <= 0)
                    {
                        Utils.ShowMsgAndCloseBoxy("url错误，请重新打开此窗口！", Utils.GetQueryStringValue("iframeId"), false);
                        return;
                    }
                    InitPage(action, qId);
                    break;
                default:
                    if (!CheckGrant(EyouSoft.Model.EnumType.PrivsStructure.Privs3.客户询价_客户日常询价_新增))
                    {
                        Utils.ShowMsgAndCloseBoxy(
                            string.Format(
                                "您没有{0}的权限，请联系管理员！", EyouSoft.Model.EnumType.PrivsStructure.Privs3.客户询价_客户日常询价_新增),
                            Utils.GetQueryStringValue("iframeId"),
                            false);
                        return;
                    }
                    plnSave.Visible = true;
                    break;
            }
        }

        /// <summary>
        /// 初始化页面信息
        /// </summary>
        /// <param name="qId">询价编号</param>
        /// <param name="action">操作行为</param>
        private void InitPage(string action, int qId)
        {
            if (action == "edit" || action == "add")
            {
                plnSave.Visible = true;
            }
            else
            {
                plnSave.Visible = false;
            }

            if (qId <= 0) return;

            var model = new EyouSoft.BLL.CustomerQuote.BCustomerQuote().GetCustomerQuote(qId);
            if (model == null) return;

            txtLeaveDate.Value = this.ToDateTimeString(model.LeaveDate);
            txtPeopleNum.Value = model.PeopleNum.ToString();
            txtXunJiaDate.Value = this.ToDateTimeString(model.QuoteDate);
            this.CustomerUnit1.InitCustomerId = model.CostomerId;
            this.CustomerUnit1.InitCustomerName = model.Costomer;
            txtXunJiaPeople.Value = model.ContactName;
            txtTel.Value = model.ContactTel;
            txtMobile.Value = model.ContactMobile;
            txtQQ.Value = model.ContactQQ;
            txtNeiRong.Value = model.Content;
        }

        /// <summary>
        ///  保存
        /// </summary>
        /// <param name="qId">询价编号</param>
        /// <param name="action">操作行为</param>
        private void Save(string action, int qId)
        {
            if (string.IsNullOrEmpty(action) || (action != "add" && action != "edit"))
            {
                this.RCWE(UtilsCommons.AjaxReturnJson("0", "url错误，请刷新页面后重新打开此窗口！"));
                return;
            }
            if (action == "edit" && qId <= 0)
            {
                this.RCWE(UtilsCommons.AjaxReturnJson("0", "url错误，请刷新页面后重新打开此窗口！"));
                return;
            }
            if (action == "add")
            {
                if (!CheckGrant(EyouSoft.Model.EnumType.PrivsStructure.Privs3.客户询价_客户日常询价_新增))
                {
                    this.RCWE(UtilsCommons.AjaxReturnJson("0", string.Format(
                            "您没有{0}的权限，请联系管理员！", EyouSoft.Model.EnumType.PrivsStructure.Privs3.客户询价_客户日常询价_新增)));
                    return;
                }
            }
            if (action == "edit")
            {
                if (!CheckGrant(EyouSoft.Model.EnumType.PrivsStructure.Privs3.客户询价_客户日常询价_修改))
                {
                    this.RCWE(UtilsCommons.AjaxReturnJson("0", string.Format(
                            "您没有{0}的权限，请联系管理员！", EyouSoft.Model.EnumType.PrivsStructure.Privs3.客户询价_客户日常询价_修改)));
                    return;
                }
            }

            int r = 0;
            var bll = new EyouSoft.BLL.CustomerQuote.BCustomerQuote();
            var model = this.GetFormValue();

            if (action == "add")
            {
                r = bll.AddCustomerQuote(model);
            }
            else if (action == "edit")
            {
                model.QuoteId = qId;
                r = bll.UpdateCustomerQuote(model);
            }

            switch (r)
            {
                case 0:
                    this.RCWE(UtilsCommons.AjaxReturnJson("0", "操作失败！"));
                    break;
                case 1:
                    this.RCWE(UtilsCommons.AjaxReturnJson("1", "操作成功！"));
                    break;
                default:
                    this.RCWE(UtilsCommons.AjaxReturnJson("0", "操作失败！"));
                    break;
            }
        }

        /// <summary>
        /// 获取表单值 生成实体
        /// </summary>
        /// <returns></returns>
        private EyouSoft.Model.CustomerQuote.MCustomerQuote GetFormValue()
        {
            return new EyouSoft.Model.CustomerQuote.MCustomerQuote
                {
                    CompanyId = CurrentUserCompanyID,
                    ContactMobile = Utils.GetFormValue(txtMobile.UniqueID),
                    ContactName = Utils.GetFormValue(txtXunJiaPeople.UniqueID),
                    ContactQQ = Utils.GetFormValue(txtQQ.UniqueID),
                    ContactTel = Utils.GetFormValue(txtTel.UniqueID),
                    Content = Utils.GetFormValue(txtNeiRong.UniqueID),
                    Costomer = Utils.GetFormValue("txtCustomerName"),
                    CostomerId = Utils.GetFormValue(this.CustomerUnit1.HidClientName),
                    IssueTime = DateTime.Now,
                    LeaveDate = Utils.GetDateTime(Utils.GetFormValue(txtLeaveDate.UniqueID)),
                    OperatorId = this.SiteUserInfo.UserId,
                    PeopleNum = Utils.GetInt(Utils.GetFormValue(txtPeopleNum.UniqueID)),
                    QuoteDate = Utils.GetDateTime(Utils.GetFormValue(txtXunJiaDate.UniqueID))
                };
        }
    }
}
