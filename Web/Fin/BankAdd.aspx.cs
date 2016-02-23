using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using EyouSoft.Common;

namespace Web.Fin
{
    public partial class BankAdd : EyouSoft.Common.Page.BackPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            PowerControl();
            string id = Utils.GetQueryStringValue("tid");
            string dotype = Utils.GetQueryStringValue("dotype").Trim();
            string type = Utils.GetQueryStringValue("type").Trim();

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
        /// 绑定要修改数据
        /// </summary>
        /// <param name="tid"></param>
        private void PageInit(string id, string dotype)
        {
            //绑定账号
            EyouSoft.BLL.CompanyStructure.BYinHangZhangHu bll = new EyouSoft.BLL.CompanyStructure.BYinHangZhangHu();
            ddlBank.Items.Clear();
            IList<EyouSoft.Model.CompanyStructure.CompanyAccount> list = bll.GetZhangHus(CurrentUserCompanyID);
            if (list != null && list.Count != 0)
            {
                foreach (var t in list)
                {
                    if (t == null) continue;
                    ddlBank.Items.Add(
                        new ListItem(string.Format("{0}-{1}-{2}", t.BankName, t.AccountName, t.BankNo), t.Id));
                }
            }
            ddlBank.Items.Insert(0, new ListItem("请选择", ""));


        }


        /// <summary>
        /// 保存或修改信息
        /// </summary>
        private string PageSave(string id, string dotype)
        {
            //t为true 新增，false 修改
            bool t = string.IsNullOrEmpty(id) && dotype == "add";
            string msg = string.Empty;
            EyouSoft.BLL.FinStructure.BBankBalance bll = new EyouSoft.BLL.FinStructure.BBankBalance();

            EyouSoft.Model.FinStructure.MBankBalance model = new EyouSoft.Model.FinStructure.MBankBalance();
            model.BankId = Utils.GetFormValue(this.ddlBank.UniqueID);
            model.Balance = Utils.GetDecimal(Utils.GetFormValue(this.txtBankBalance.UniqueID));
            model.Date = Utils.GetDateTime(Utils.GetFormValue(this.txtDate.UniqueID));
            model.CompanyId = CurrentUserCompanyID;
            model.OperatorId = SiteUserInfo.UserId;
            model.IssueTime = DateTime.Now;



            int result = 0;
            if (t)
            {
                result = bll.AddBankBalance(model);
            }
            switch (result)
            {
                case 0:
                    msg = UtilsCommons.AjaxReturnJson("0", (t ? "新增" : "修改") + "失败");
                    break;
                case 1:
                    msg = UtilsCommons.AjaxReturnJson("1", (t ? "新增" : "修改") + "成功");
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
            if (!this.CheckGrant(EyouSoft.Model.EnumType.PrivsStructure.Privs3.财务管理_银行余额_新增))
            {
                this.btn.Visible = false;
            }


        }
    }
}
