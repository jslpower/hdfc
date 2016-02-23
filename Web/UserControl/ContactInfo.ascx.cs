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
using EyouSoft.Model.CRM;
using EyouSoft.Common;

namespace Web.UserControl
{
    /// <summary>
    /// 创建人：刘飞
    /// 时间：2013-1-4
    /// 描述:联系人信息控件
    /// </summary>
    public partial class ContactInfo : System.Web.UI.UserControl
    {
        /// <summary>
        /// 设置控件的数据源
        /// </summary>
        private IList<MCustomerContact> _setTravelList;

        public IList<MCustomerContact> SetTravelList
        {
            get { return _setTravelList; }
            set { _setTravelList = value; }
        }


        protected void Page_Load(object sender, EventArgs e)
        {
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
                this.rptlist.DataSource = this.SetTravelList;
                this.rptlist.DataBind();
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
        public IList<MCustomerContact> GetDateList()
        {
            string[] contactname = Utils.GetFormValues("contact_Name");
            string[] birthday = Utils.GetFormValues("contact_birthday");
            string[] department = Utils.GetFormValues("contact_depart");
            string[] duty = Utils.GetFormValues("contact_duty");
            string[] tel = Utils.GetFormValues("contact_tel");
            string[] mobile = Utils.GetFormValues("contact_mobile");
            string[] qq = Utils.GetFormValues("contact_qq");
            string[] email = Utils.GetFormValues("contact_email");
            string[] remark = Utils.GetFormValues("contact_remark");
            string[] Id = Utils.GetFormValues("contact_Id");
            string[] sex = Utils.GetFormValues("sltsex");
            if (contactname == null || birthday == null || department == null || duty == null || tel == null ||
                mobile == null || qq == null || email == null || remark == null || Id == null || sex == null ||
                contactname.Length != birthday.Length || birthday.Length != department.Length || department.Length != duty.Length ||
                duty.Length != tel.Length || tel.Length != mobile.Length || mobile.Length != qq.Length || qq.Length != email.Length ||
                email.Length != remark.Length || remark.Length != Id.Length || Id.Length != sex.Length)
            {
                return null;
            }
            IList<MCustomerContact> list = new List<MCustomerContact>();
            EyouSoft.Model.CRM.MCustomerContact model = new MCustomerContact();
            for (int i = 0; i < contactname.Length; i++)
            {
                if (string.IsNullOrEmpty(contactname[i]) || string.IsNullOrEmpty(mobile[i]) || string.IsNullOrEmpty(qq[i])) continue;
                list.Add(new MCustomerContact
                {
                    BirthDay = Utils.GetDateTimeNullable(birthday[i]),
                    DepartmentName = department[i],
                    Email = email[i],
                    Job = duty[i],
                    Mobile = mobile[i],
                    Name = contactname[i],
                    Qq = qq[i],
                    Remark = remark[i],
                    Tel = tel[i],
                    Sex = (EyouSoft.Model.EnumType.CompanyStructure.Sex)(Utils.GetInt(sex[i]))
                });
            }
            return list;
        }
    }
}