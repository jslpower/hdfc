using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Web.Webmaster
{
    /// <summary>
    /// 短信充值审核
    /// </summary>
    /// Author:汪奇志 2011-04-26
    public partial class smscashier : WebmasterPageBase
    {
        /// <summary>
        /// webmaster realname
        /// </summary>
        protected string WebmasterRealname = string.Empty;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (EyouSoft.Common.Utils.GetInt(Request.QueryString["cashier"], 0) == 1)//审核操作
            {
                this.Cashier();
            }
            else
            {

                EyouSoft.Model.SSOStructure.MWebmasterInfo loginUserInfo =
                    EyouSoft.Security.Membership.WebmasterProvider.GetWebmasterInfo();
                if (loginUserInfo != null)
                {
                    WebmasterRealname = loginUserInfo.Username;
                }

                this.InitRecharge();
            }
        }

        #region private members
        /// <summary>
        /// 初始化短信充值列表
        /// </summary>
        private void InitRecharge()
        {
            int pageSize = 20;
            int pageIndex = EyouSoft.Common.Utils.GetInt(Request.QueryString["Page"], 1);
            int recordCount = 0;

            string companyName = EyouSoft.Common.Utils.InputText(Request.QueryString["cn"]);
            int status = EyouSoft.Common.Utils.GetInt(Request.QueryString["status"], -1);

            EyouSoft.BLL.SMSStructure.Account bll = new EyouSoft.BLL.SMSStructure.Account();
            IList<EyouSoft.Model.SMSStructure.PayMoneyInfo> items = bll.GetPayMoneys(pageSize, pageIndex, ref recordCount, 0, companyName, null, null, null, null, null, null, status, null);
            bll = null;

            if (items != null && items.Count > 0)
            {
                this.rptRecharges.DataSource = items;
                this.rptRecharges.DataBind();
                this.phNotFound.Visible = false;

                this.Page.ClientScript.RegisterClientScriptBlock(this.GetType(), Guid.NewGuid().ToString(), string.Format("pConfig.pageSize={0};pConfig.pageIndex={1};pConfig.recordCount={2};", pageSize, pageIndex, recordCount), true);
            }
            else
            {
                this.phNotFound.Visible = true;

            }
        }

        /// <summary>
        /// 审核
        /// </summary>
        private void Cashier()
        {
            string pid = EyouSoft.Common.Utils.InputText(Request.QueryString["pid"]);
            string status = EyouSoft.Common.Utils.InputText(Request.QueryString["status"]);
            decimal amount = EyouSoft.Common.Utils.GetDecimal(Request.QueryString["amount"], 0);
            string webmasterRealname = EyouSoft.Common.Utils.InputText(Request.QueryString["wn"]);

            if (string.IsNullOrEmpty(pid))
            {
                ResponseClearAndWrite("0");
            }

            if (string.IsNullOrEmpty(webmasterRealname))
            {
                ResponseClearAndWrite("-1");
            }

            if (amount < 0)
            {
                ResponseClearAndWrite("-2");
            }

            EyouSoft.BLL.SMSStructure.Account bll = new EyouSoft.BLL.SMSStructure.Account();
            EyouSoft.Model.SMSStructure.PayMoneyInfo info = new EyouSoft.Model.SMSStructure.PayMoneyInfo();
            EyouSoft.Model.SSOStructure.MWebmasterInfo loginUserInfo = EyouSoft.Security.Membership.WebmasterProvider.GetWebmasterInfo();

            info.CheckOperatorName = webmasterRealname;
            info.CheckTime = DateTime.Now;
            info.CheckUserName = loginUserInfo.Username;
            info.ID = pid;
            info.IsChecked = status == "true" ? 1 : 2;
            info.UseMoney = amount;

            if (info.IsChecked != 1)
            {
                info.UseMoney = 0;
            }

            if (bll.CheckPayMoney(info))
            {
                ResponseClearAndWrite("1");
            }
            else
            {
                ResponseClearAndWrite("-3");
            }
        }

        /// <summary>
        /// Response.Clear();Response.Write(s);Response.End();
        /// </summary>
        /// <param name="s"></param>
        private void ResponseClearAndWrite(string s)
        {
            Response.Clear();
            Response.Write(s);
            Response.End();
        }
        #endregion

        #region protected members
        /// <summary>
        /// rptRecharges_ItemDataBound
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void rptRecharges_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemIndex < 0) return;

            Literal ltr = (Literal)e.Item.FindControl("ltrStatus");
            PlaceHolder ph = (PlaceHolder)e.Item.FindControl("phCashierBox");
            EyouSoft.Model.SMSStructure.PayMoneyInfo info = (EyouSoft.Model.SMSStructure.PayMoneyInfo)e.Item.DataItem;

            string s = string.Empty;
            if (info.IsChecked==0)
            {
                s = "<a href='javascript:void(0);' onclick=\"openCashierBox('" + info.ID + "',this); return false;\">审核</a>";
                ph.Visible = true;
            }
            else if (info.IsChecked == 1)
            {
                s = "<span style=\"color:green\">已通过</span>";
                ph.Visible = false;
            }
            else
            {
                s = "<span style=\"color:red\">未通过</a>";
                ph.Visible = false;
            }

            ltr.Text = s;
        }
        #endregion
    }
}
