using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using EyouSoft.Common;

namespace Web.SystemSet
{
    /// <summary>
    /// 登录用户修改个人信息
    /// </summary>
    public partial class EditUserInfo : EyouSoft.Common.Page.BackPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            string save = Utils.GetQueryStringValue("save");
            if (!string.IsNullOrEmpty(save))
            {
                SaveData();
                return;
            }

            if (!IsPostBack)
            {
                InitPage();
            }
        }

        /// <summary>
        /// 初始化用户信息
        /// </summary>
        private void InitPage()
        {
            var model = new EyouSoft.BLL.CompanyStructure.CompanyUser().GetUserInfo(SiteUserInfo.UserId);
            if (model == null || model.PersonInfo == null) return;

            ltrUserName.Text = model.UserName;

            txtPassword.Attributes.Add("value", model.PassWordInfo.NoEncryptPassword);
            txtPassWordCheck.Attributes.Add("value", model.PassWordInfo.NoEncryptPassword);
            txtName.Value = model.PersonInfo.ContactName;
            txtEmail.Value = model.PersonInfo.ContactEmail;
            txtFax.Value = model.PersonInfo.ContactFax;
            txtMoible.Value = model.PersonInfo.ContactMobile;
            txtMSN.Value = model.PersonInfo.MSN;
            txtQQ.Value = model.PersonInfo.QQ;
            txtTel.Value = model.PersonInfo.ContactTel;
            rdiSex.SelectedValue = ((int)model.PersonInfo.ContactSex).ToString();
            if (model.PersonInfo.Birthday.HasValue)
                txtBirthday.Value = model.PersonInfo.Birthday.Value.ToShortDateString();
        }

        /// <summary>
        /// 保存用户信息
        /// </summary>
        private void SaveData()
        {
            var model = this.GetFormValue();

            bool r = new EyouSoft.BLL.CompanyStructure.CompanyUser().SimpleUpdate(model);
            if (r)
            {
                //供应商用户同步修改联系人信息
                if (SiteUserInfo.UserType != EyouSoft.Model.EnumType.CompanyStructure.UserType.专线用户)
                {
                    new EyouSoft.BLL.SourceStructure.BSupplier().UpdateSupplierContact(
                        SiteUserInfo.CompanyId,
                        SiteUserInfo.SupplierCompanyId,
                        SiteUserInfo.UserId,
                        new EyouSoft.Model.SourceStructure.MSupplierContact
                            {
                                ContactName = Utils.GetFormValue(txtName.UniqueID),
                                ContactFax = Utils.GetFormValue(txtFax.UniqueID),
                                ContactTel = Utils.GetFormValue(txtTel.UniqueID),
                                ContactMobile = Utils.GetFormValue(txtMoible.UniqueID),
                                QQ = Utils.GetFormValue(txtQQ.UniqueID),
                                Email = Utils.GetFormValue(txtEmail.UniqueID),
                                Birthday = Utils.GetDateTimeNullable(Utils.GetFormValue(txtBirthday.UniqueID))
                            });
                }

                this.RCWE(UtilsCommons.AjaxReturnJson("1", "修改用户信息成功！"));
                return;
            }

            this.RCWE(UtilsCommons.AjaxReturnJson("0", "修改用户信息失败！"));
        }

        /// <summary>
        /// 获取表单值
        /// </summary>
        /// <returns></returns>
        private EyouSoft.Model.CompanyStructure.CompanyUser GetFormValue()
        {
            var model = new EyouSoft.Model.CompanyStructure.CompanyUser
                {
                    PersonInfo =
                        new EyouSoft.Model.CompanyStructure.ContactPersonInfo
                            {
                                ContactEmail = Utils.GetFormValue(txtEmail.UniqueID),
                                ContactFax = Utils.GetFormValue(txtFax.UniqueID),
                                ContactMobile = Utils.GetFormValue(txtMoible.UniqueID),
                                MSN = Utils.GetFormValue(txtMSN.UniqueID),
                                ContactName = Utils.GetFormValue(txtName.UniqueID),
                                QQ = Utils.GetFormValue(txtQQ.UniqueID),
                                ContactTel = Utils.GetFormValue(txtTel.UniqueID),
                                ContactSex =
                                    (EyouSoft.Model.EnumType.CompanyStructure.Sex)
                                    Utils.GetInt(Utils.GetFormValue(rdiSex.UniqueID)),
                                Birthday = Utils.GetDateTimeNullable(Utils.GetFormValue(txtBirthday.UniqueID)),
                            },
                    PassWordInfo =
                        new EyouSoft.Model.CompanyStructure.PassWord(Utils.GetFormValue(txtPassword.UniqueID)),
                    ID = SiteUserInfo.UserId
                };

            return model;
        }
    }
}
