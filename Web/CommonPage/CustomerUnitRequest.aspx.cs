using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;
using System.Collections.Generic;
using EyouSoft.Common;

namespace Web.CommonPage
{
    public partial class CustomerUnitRequest : EyouSoft.Common.Page.BackPage
    {
        protected int IsSelectMore;
        protected int recordcount = 0;
        protected void Page_Load(object sender, EventArgs e)
        {
            string cname = Utils.GetQueryStringValue("cname");
            string dotype = Utils.GetQueryStringValue("dotype");
            switch (dotype)
            {
                case "save":
                    Response.Clear();
                    Response.Write(PageSave());
                    Response.End();
                    break;
                default:
                    break;
            }
            if (!IsPostBack)
            {
                if (cname != "")
                    InitPage(cname);
                else
                    this.formUnit.Visible = false;
            }
        }
        private void InitPage(string cname)
        {
            IsSelectMore = Utils.GetInt(Utils.GetQueryStringValue("isMore"));
            var search = new EyouSoft.Model.CRM.MSearchCustomer
            {
                CustomerName = cname
            };
            EyouSoft.BLL.CRM.BCustomer bll = new EyouSoft.BLL.CRM.BCustomer();
            IList<EyouSoft.Model.CRM.MCustomer> list = bll.GetCustomer(SiteUserInfo.CompanyId, search);
            if (list != null && list.Count > 0)
            {
                this.recordcount = list.Count;
                rptCustomer.DataSource = list;
                rptCustomer.DataBind();
                this.Tabform.Visible = false;
            }
            else
            {
                BindArea();
                lbemptymsg.Text = "<tr><td align='center' height='30px'><span id='empty'>暂无数据!</span></td></tr>";
                this.Tabform.Attributes.Add("style", "display:black");
            }
        }

        protected string BindArea()
        {
            System.Text.StringBuilder str = new System.Text.StringBuilder();

            EyouSoft.BLL.CompanyStructure.BSaleArea bll = new EyouSoft.BLL.CompanyStructure.BSaleArea();
            IList<EyouSoft.Model.CompanyStructure.MSaleArea> list = bll.GetSaleArea(SiteUserInfo.CompanyId, null);

            str.Append("<option value='-1'>请选择</option>");
            if (list != null && list.Count > 0)
            {
                for (int i = 0; i < list.Count; i++)
                {
                    str.AppendFormat("<option value='{0}'>{1}</option>", list[i].SaleAreaId.ToString(), list[i].SaleAreaName);
                }
            }
            return str.ToString();
        }
        /// <summary>
        /// 获取input选择框
        /// </summary>
        /// <param name="id"></param>
        /// <param name="name"></param>
        /// <param name="contactName"></param>
        /// <param name="phone"></param>
        /// <param name="mobile"></param>
        /// <returns></returns>
        protected string GetInputHtml(object id, object name, object contactName, object phone, object mobile)
        {
            bool isCheck = false;

            string initId = Utils.GetQueryStringValue("initId");
            if (!string.IsNullOrEmpty(initId))
            {
                string[] tmpId = initId.Split(',');
                if (tmpId.Length > 0)
                {
                    isCheck = tmpId.Contains(id.ToString());
                }
            }

            if (IsSelectMore == 1)
            {
                return
                    string.Format(
                        "<input type=\"checkbox\" class=\"c1\" data-value=\"{0}\" value=\"{0}\" name=\"ckbCId\" data-cname=\"{1}\" data-ccname=\"{2}\" data-tel=\"{3}\" data-mobile=\"{4}\" {5} />",
                        id.ToString(),
                        name.ToString(),
                        contactName.ToString(),
                        phone.ToString(),
                        mobile.ToString(),
                        isCheck ? "checked" : string.Empty);
            }

            return string.Format(
                        "<input type=\"radio\" class=\"c1\" data-value=\"{0}\" value=\"{0}\" name=\"radCId\" data-cname=\"{1}\" data-ccname=\"{2}\" data-tel=\"{3}>\" data-mobile=\"{4}\" {5} />",
                        id.ToString(),
                        name.ToString(),
                        contactName.ToString(),
                        phone.ToString(),
                        mobile.ToString(),
                        isCheck ? "checked" : string.Empty);
        }

        /// <summary>
        /// 新增组团社
        /// </summary>
        /// <returns></returns>
        private string PageSave()
        {
            string msg = UtilsCommons.AjaxReturnJson("0", "添加失败!");
            EyouSoft.BLL.CRM.BCustomer bll = new EyouSoft.BLL.CRM.BCustomer();
            EyouSoft.Model.CRM.MCustomer model = new EyouSoft.Model.CRM.MCustomer();
            string unitname = Utils.GetFormValue(this.txtunitname.UniqueID);
            if (string.IsNullOrEmpty(unitname))
            {
                return UtilsCommons.AjaxReturnJson("0", "组团社名称不能为空!");
            }
            model.ProviceId = Utils.GetInt(Utils.GetFormValue(this.ddlProvice.UniqueID));
            model.CityId = Utils.GetInt(Utils.GetFormValue(this.ddlCity.UniqueID));
            model.CustomerName = unitname;
            model.ContactName = Utils.GetFormValue(this.txtcontactname.UniqueID);
            model.Phone = Utils.GetFormValue(this.txtcontacttel.UniqueID);
            model.CompanyId = SiteUserInfo.CompanyId;
            model.Mobile = Utils.GetFormValue(this.txtcontactmobile.UniqueID);
            model.SaleAreadId = Utils.GetInt(Utils.GetFormValue(this.hidareaid.UniqueID));
            model.Contact = ContactInfo1.GetDateList();
            int result = 0;
            result = bll.AddCustomer(model);
            string newgid = model.Id;
            string newname = model.CustomerName;

            switch (result)
            {
                case 0:
                    msg = UtilsCommons.AjaxReturnJson("0", "参数错误!");
                    break;
                case -1:
                    msg = UtilsCommons.AjaxReturnJson("0", "添加失败!");
                    break;
                case 1:
                    msg = UtilsCommons.AjaxReturnJson(newgid + "|" + newname, "添加成功!");
                    break;
                case -2:
                    msg = UtilsCommons.AjaxReturnJson("0", "组团社已存在!");
                    break;
                default:
                    break;
            }
            return msg;
        }
    }
}
