//2011-09-27 汪奇志
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
    /// 添加子系统信息
    /// </summary>
    public partial class _systemadd : WebmasterPageBase
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        #region btnCreate click
        /// <summary>
        /// btnCreate click
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnCreate_Click(object sender, EventArgs e)
        {
            EyouSoft.Model.SysStructure.MSysInfo info = new EyouSoft.Model.SysStructure.MSysInfo();
            info.Password = new EyouSoft.Model.CompanyStructure.PassWord();

            #region get form values
            info.SysName = Utils.GetFormValue("txtSysName");
            info.FullName = Utils.GetFormValue("txtFullname");
            info.Telephone = Utils.GetFormValue("txtTelephone");
            info.Mobile = Utils.GetFormValue("txtMobile");
            info.Username = Utils.GetFormValue("txtUsername");
            info.Password.NoEncryptPassword = Utils.GetFormValue("txtPassword");

            int max = Utils.GetInt(Utils.GetFormValue("txtSonNum"));
            int day = Utils.GetInt(Utils.GetFormValue("txtDay"));
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

            #endregion

            #region validate form
            if (string.IsNullOrEmpty(info.SysName))
            {
                RegisterAlertScript("请输入系统名称！");
                return;
            }

            if (string.IsNullOrEmpty(info.Username))
            {
                RegisterAlertScript("请输入登录账号！");
                return;
            }

            if (string.IsNullOrEmpty(info.Password.NoEncryptPassword))
            {
                RegisterAlertScript("请输入登录密码！");
                return;
            }
            if (this.Request.Files.Count > 0 && !isCheck)
            {
                RegisterAlertScript(strMsg);
                return;
            }
            #endregion

            if (max > 0 || day > 0 || !string.IsNullOrEmpty(log))
            {
                info.CompanySetting = new EyouSoft.Model.CompanyStructure.CompanyFieldSetting();
                if (max > 0)
                {
                    info.CompanySetting.MaxSonUserNum = max;
                }
                if (day > 0)
                {
                    info.CompanySetting.BirthdayReminderDays = day;
                }
                if (!string.IsNullOrEmpty(log))
                {
                    info.CompanySetting.CompanyLogo = log;
                }
            }

            int createResult = new EyouSoft.BLL.SysStructure.BSys().CreateSys(info);

            if (createResult == 1)
            {
                this.RegisterAlertAndRedirectScript("子系统添加成功", "systems.aspx");
            }
            else
            {
                this.RegisterAlertAndRedirectScript("子系统添加失败", "systemadd.aspx");
            }
        }
        #endregion
    }
}
