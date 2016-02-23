//2011-09-30 汪奇志
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using EyouSoft.Common;

namespace Web.Webmaster
{
    /// <summary>
    /// 查看子系统信息
    /// </summary>
    public partial class _system : WebmasterPageBase
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            var info = new EyouSoft.BLL.SysStructure.BSys().GetSysInfo(Utils.GetInt(Utils.GetQueryStringValue("SysId")));

            if (info != null)
            {
                this.ltrSysId.Text = info.SysId.ToString();
                this.ltrCompanyId.Text = info.CompanyId.ToString();
                hidCompanyId.Value = info.CompanyId.ToString();
                this.ltrIssueTime.Text = info.IssueTime.ToString();
                this.ltrSysName1.Text = this.ltrSysName2.Text = info.SysName;
                this.ltrFullname.Text = info.FullName;
                this.ltrTelephone.Text = info.Telephone;
                this.ltrMobile.Text = info.Mobile;
                this.ltrUserId.Text = info.UserId.ToString();
                this.ltrUsername.Text = info.Username;
                this.ltrPassword.Text = info.Password.NoEncryptPassword;

                InitCompanySetting(info);
            }
        }

        private void InitCompanySetting(EyouSoft.Model.SysStructure.MSysInfo info)
        {
            var model = new EyouSoft.BLL.CompanyStructure.CompanySetting().GetSetting(info.CompanyId);
            if (model == null) return;
            if (model.MaxSonUserNum > 0) txtSonNum.Value = model.MaxSonUserNum.ToString();
            if (model.BirthdayReminderDays > 0) txtDay.Value = model.BirthdayReminderDays.ToString();
            if (!string.IsNullOrEmpty(model.CompanyLogo))
            {
                ltrCompanyLog.Text = string.Format(
                    "<a target=\"_blank\" title=\"点击查看\" href=\"{0}\">点击查看</a>", model.CompanyLogo);
                hidOldFile.Value = model.CompanyLogo;
            }
        }

        protected void btnCreate_Click(object sender, EventArgs e)
        {
            int max = Utils.GetInt(Utils.GetFormValue(txtSonNum.UniqueID));
            int day = Utils.GetInt(Utils.GetFormValue(txtDay.UniqueID));
            string log = string.Empty;

            string strMsg;
            bool isCheck = EyouSoft.Common.Function.UploadFile.CheckFileType(
                this.Request.Files, "fileUpLoad", new[] { ".gif", ".bmp", ".png", ".jpg", ".jpeg" }, 2, out strMsg);
            if (isCheck)
            {
                //文件路径
                string filePath = string.Empty;
                //文件名
                string fileName = string.Empty;
                //文件上传
                if (EyouSoft.Common.Function.UploadFile.FileUpLoad(Request.Files["fileUpLoad"], "CompanyLog", out filePath, out fileName))
                {
                    if (!string.IsNullOrEmpty(filePath))
                    {
                        log = filePath;
                    }
                }
            }

            if (max > 0 || day > 0 || !string.IsNullOrEmpty(log))
            {
                var model = new EyouSoft.Model.CompanyStructure.CompanyFieldSetting
                    { CompanyId = Utils.GetInt(Utils.GetFormValue(this.hidCompanyId.UniqueID)) };
                if (max > 0)
                {
                    model.MaxSonUserNum = max;
                }
                if (day > 0)
                {
                    model.BirthdayReminderDays = day;
                }
                if (!string.IsNullOrEmpty(log))
                {
                    model.CompanyLogo = log;
                }
                else
                {
                    model.CompanyLogo = Utils.GetFormValue(hidOldFile.UniqueID);
                }

                new EyouSoft.BLL.CompanyStructure.CompanySetting().SetCompanySetting(model);
            }

            this.RegisterAlertAndRedirectScript("修改成功", "systems.aspx");
        }
    }
}
