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
using System.Collections.Generic;
using System.Text;

namespace Web.CustomerManage
{
    /// <summary>
    /// 创建人：刘飞
    /// 时间：2012-12-31
    /// 描述：客户新增/修改
    /// </summary>
    public partial class CustomerEdit : EyouSoft.Common.Page.BackPage
    {
        protected int Areaid = -1;
        protected string Province = "0";
        protected string City = "0";
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
            //绑定客户等级
            Array values = Enum.GetValues(typeof(EyouSoft.Model.EnumType.CustomerStructure.CustomerRating));
            foreach (var item in values)
            {
                int value = (int)Enum.Parse(typeof(EyouSoft.Model.EnumType.CustomerStructure.CustomerRating), item.ToString());
                string text = Enum.GetName(typeof(EyouSoft.Model.EnumType.CustomerStructure.CustomerRating), item);

                this.ddlCustomerRating.Items.Add(new ListItem() { Text = text, Value = value.ToString() });
            }
            this.ddlCustomerRating.Items[3].Selected = true;

            if (id != "" && dotype == "update")
            {
                EyouSoft.BLL.CRM.BCustomer bll = new EyouSoft.BLL.CRM.BCustomer();
                EyouSoft.Model.CRM.MCustomer model = bll.GetCustomer(id);
                if (model != null)
                {
                    txtAddress.Value = model.Address;
                    txtBankAccount.Value = model.BankCode;
                    txtContactName.Value = model.ContactName;
                    txtFax.Value = model.Fax;
                    txtLicense.Value = model.Licence;
                    txtMobile.Value = model.Mobile;
                    txtName.Value = model.CustomerName;
                    txtPhone.Value = model.Phone;
                    txtPostalCode.Value = model.PostalCode;
                    txtRemark.Text = model.Remark;
                    if (model.Contact != null && model.Contact.Count > 0)
                        this.ContactInfo1.SetTravelList = model.Contact;
                    this.Province = model.ProviceId.ToString();
                    this.City = model.CityId.ToString();
                    this.hidareaid.Value = model.SaleAreadId.ToString();
                    Areaid = model.SaleAreadId;
                    this.ddlCustomerRating.SelectedValue = ((int)model.CustomerRating).ToString();
                }

            }
        }

        protected string BindArea(int areaid)
        {
            System.Text.StringBuilder str = new System.Text.StringBuilder();

            EyouSoft.BLL.CompanyStructure.BSaleArea bll = new EyouSoft.BLL.CompanyStructure.BSaleArea();
            IList<EyouSoft.Model.CompanyStructure.MSaleArea> list = bll.GetSaleArea(SiteUserInfo.CompanyId, null);
            if (areaid >= 0)
                str.Append("<option value='-1'>请选择</option>");
            else
                str.Append("<option value='-1' selected='selected'>请选择</option>");
            if (list != null && list.Count > 0)
            {
                for (int i = 0; i < list.Count; i++)
                {
                    if (list[i].SaleAreaId != areaid)
                        str.AppendFormat("<option value='{0}'>{1}</option>", list[i].SaleAreaId.ToString(), list[i].SaleAreaName);
                    else
                        str.AppendFormat("<option value='{0}' selected='selected'>{1}</option>", list[i].SaleAreaId.ToString(), list[i].SaleAreaName);
                }
            }
            return str.ToString();
        }


        /// <summary>
        /// 绑定客户评级
        /// </summary>
        protected string BindCustomerRating(string selectItem)
        {
            System.Text.StringBuilder query = new System.Text.StringBuilder();
            query.Append("<option value=''>-客户评级-</option>");
            //EyouSoft.Model.EnumType.PersonalCenterStructure.PlanCheckState
            Array values = Enum.GetValues(typeof(EyouSoft.Model.EnumType.CustomerStructure.CustomerRating));
            foreach (var item in values)
            {
                int value = (int)Enum.Parse(typeof(EyouSoft.Model.EnumType.CustomerStructure.CustomerRating), item.ToString());
                string text = Enum.GetName(typeof(EyouSoft.Model.EnumType.CustomerStructure.CustomerRating), item);
                if (value.ToString().Equals(selectItem))
                {
                    query.AppendFormat("<option value='{0}' selected='selected'>{1}</option>", value, text);
                }
                else
                {
                    query.AppendFormat("<option value='{0}' >{1}</option>", value, text);

                }
            }
            return query.ToString();
        }

        /// <summary>
        /// 保存
        /// </summary>
        /// <param name="id"></param>
        /// <param name="dotype"></param>
        /// <returns></returns>
        private string PageSave(string id, string dotype)
        {
            //t为true 新增，false 修改
            bool t = string.IsNullOrEmpty(id) && dotype == "add";
            string msg = UtilsCommons.AjaxReturnJson("0", "操作失败!");
            EyouSoft.BLL.CRM.BCustomer bll = new EyouSoft.BLL.CRM.BCustomer();
            EyouSoft.Model.CRM.MCustomer model = new EyouSoft.Model.CRM.MCustomer();
            model.Address = Utils.GetFormValue(txtAddress.UniqueID);
            model.BankCode = Utils.GetFormValue(txtBankAccount.UniqueID);
            model.CityId = Utils.GetInt(Utils.GetFormValue(ddlCity.UniqueID));
            model.CompanyId = this.SiteUserInfo.CompanyId;
            model.Contact = ContactInfo1.GetDateList();
            model.ContactName = Utils.GetFormValue(txtContactName.UniqueID);
            model.CustomerName = Utils.GetFormValue(txtName.UniqueID);
            model.CustomerType = EyouSoft.Model.EnumType.CustomerStructure.CustomerType.组团社;
            model.Fax = Utils.GetFormValue(txtFax.UniqueID);
            model.IssueTime = DateTime.Now;
            model.Licence = Utils.GetFormValue(txtLicense.UniqueID);
            model.Mobile = Utils.GetFormValue(txtMobile.UniqueID);
            model.OperatorId = SiteUserInfo.UserId;
            model.Phone = Utils.GetFormValue(txtPhone.UniqueID);
            model.PostalCode = Utils.GetFormValue(txtPostalCode.UniqueID);
            model.ProviceId = Utils.GetInt(Utils.GetFormValue(ddlProvice.UniqueID));
            model.Remark = Utils.GetFormValue(txtRemark.UniqueID);
            model.SaleAreadId = Utils.GetInt(Utils.GetFormValue(this.hidareaid.UniqueID));
            model.CustomerRating = (EyouSoft.Model.EnumType.CustomerStructure.CustomerRating)Utils.GetInt(Utils.GetFormValue(this.ddlCustomerRating.UniqueID));
            int result = 0;
            if (t)
            {
                result = bll.AddCustomer(model);
            }
            else
            {
                model.Id = id;
                result = bll.UpdateCustomer(model);
            }
            switch (result)
            {
                case 0:
                    msg = UtilsCommons.AjaxReturnJson("0", "参数错误!");
                    break;
                case -1:
                    msg = UtilsCommons.AjaxReturnJson("0", (t ? "新增" : "修改") + "失败!");
                    break;
                case 1:
                    msg = UtilsCommons.AjaxReturnJson("1", (t ? "新增" : "修改") + "成功!");
                    break;
                case -2:
                    msg = UtilsCommons.AjaxReturnJson("0", "客户名称已存在,请填写其他名称!");
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
            if (!this.CheckGrant(EyouSoft.Model.EnumType.PrivsStructure.Privs3.客户管理_客户资料_新增))
            {
                this.btn.Visible = false;
            }
            if (!this.CheckGrant(EyouSoft.Model.EnumType.PrivsStructure.Privs3.客户管理_客户资料_修改))
            {
                this.btn.Visible = false;
            }
        }
    }
}
