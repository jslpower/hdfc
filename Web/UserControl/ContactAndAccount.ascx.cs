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
using EyouSoft.Common;
using System.Collections.Generic;
using EyouSoft.Model.SourceStructure;
using EyouSoft.Model.CompanyStructure;

namespace Web.UserControl
{
    /// <summary>
    /// 创建人：刘飞
    /// 时间：2013-1-4
    /// 描述：供应商联系人信息
    /// </summary>
    public partial class ContactAndAccount : System.Web.UI.UserControl
    {

        private bool _isAccount = false;
        /// <summary>
        /// 是否显示分配帐号
        /// </summary>
        public bool IsAccount
        {
            get { return _isAccount; }
            set { _isAccount = value; }
        }
        private EyouSoft.Model.EnumType.CompanyStructure.UserType _usertype;
        /// <summary>
        /// 公司用户类型
        /// </summary>
        public EyouSoft.Model.EnumType.CompanyStructure.UserType Usertype
        {
            get { return _usertype; }
            set { _usertype = value; }
        }
        /// <summary>
        /// 设置控件的数据源
        /// </summary>
        private IList<MSupplierContact> _setTravelList;

        public IList<MSupplierContact> SetTravelList
        {
            get { return _setTravelList; }
            set { _setTravelList = value; }
        }


        protected void Page_Load(object sender, EventArgs e)
        {
            this.phaccount.Visible = IsAccount;
            if (!IsPostBack)
            {
                SetDataList();
            }
        }

        /// <summary>
        /// 页面初始化时绑定数据
        /// </summary>
        private void SetDataList()
        {
            if (this.SetTravelList != null && this.SetTravelList.Count > 0)
            {
                this.rptList.DataSource = this.SetTravelList;
                this.rptList.DataBind();
                this.PhDefault.Visible = false;
            }
            else
            {
                this.PhDefault.Visible = true;
            }
        }
        /// <summary>
        /// 获取联系人信息集合
        /// </summary>
        /// <returns></returns>
        public IList<MSupplierContact> GetDateList()
        {
            string[] contactname = Utils.GetFormValues("contact_Name");
            string[] duty = Utils.GetFormValues("contact_duty");
            string[] tel = Utils.GetFormValues("contact_tel");
            string[] mobile = Utils.GetFormValues("contact_mobile");
            string[] qq = Utils.GetFormValues("contact_qq");
            string[] email = Utils.GetFormValues("contact_email");
            string[] Id = Utils.GetFormValues("contact_Id");
            string[] fax = Utils.GetFormValues("contact_fax");
            string[] birthday = Utils.GetFormValues("contact_birthday");

            string[] account = Utils.GetFormValues("account");
            string[] pwd = Utils.GetFormValues("pwd");
            string[] repwd = Utils.GetFormValues("repwd");
            string[] userid = Utils.GetFormValues("userid");
            string[] isdel = Utils.GetFormValues("hidIsDel");
            if (contactname == null || duty == null || tel == null ||
                mobile == null || qq == null || email == null || Id == null || fax == null || birthday == null || contactname.Length != duty.Length || duty.Length != tel.Length || tel.Length != mobile.Length ||
                mobile.Length != qq.Length || qq.Length != email.Length ||
                email.Length != Id.Length || Id.Length != fax.Length || fax.Length != birthday.Length)
            {
                return null;
            }
            IList<MSupplierContact> list = new List<MSupplierContact>();
            for (int i = 0; i < contactname.Length; i++)
            {
                if (contactname[i] == "") break;

                MSupplierContact model = new MSupplierContact();

                model.ContactMobile = mobile[i].ToString();
                model.ContactName = contactname[i].ToString();
                model.ContactTel = tel[i].ToString();
                model.Email = email[i].ToString();
                model.JobTitle = duty[i].ToString();
                model.QQ = qq[i].ToString();
                model.Id = Id[i].ToString();
                model.ContactFax = fax[i].ToString();
                model.Birthday = Utils.GetDateTimeNullable(birthday[i].ToString());
                if (!string.IsNullOrEmpty(account[i]) && !string.IsNullOrEmpty(pwd[i]) && !string.IsNullOrEmpty(repwd[i]))
                {
                    model.LoginInfo = new MSupplierContactLoginInfo();
                    model.LoginInfo.PassWord = new PassWord();
                    if (isdel[i] == "1")
                    {
                        model.LoginInfo.UserId = userid[i];
                        model.LoginInfo.UserName = account[i];
                        model.LoginInfo.PassWord.NoEncryptPassword = pwd[i];
                        model.LoginInfo.UserType = Usertype;
                    }
                    else
                    {
                        model.LoginInfo.UserId = userid[i];
                        model.LoginInfo.UserName = "";
                        model.LoginInfo.UserType = Usertype;
                    }

                }
                list.Add(model);
            }
            return list;
        }
    }
}