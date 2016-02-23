using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Web.Webmaster
{
    /// <summary>
    /// 我的信息管理
    /// </summary>
    /// Author:汪奇志 2011-09-23
    public partial class self : WebmasterPageBase
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!this.Page.IsPostBack)
            {
                this.InitSelfInfo();
            }
        }

        #region private members       
        /// <summary>
        /// init self info
        /// </summary>
        private void InitSelfInfo()
        {
            EyouSoft.Model.SSOStructure.MWebmasterInfo webmasterInfo = EyouSoft.Security.Membership.WebmasterProvider.GetWebmasterInfo();
            if (webmasterInfo != null)
            {
                this.ltrUserId.Text = webmasterInfo.UserId.ToString();
                this.ltrUsername.Text = webmasterInfo.Username;
            }
        }
        #endregion

        #region btn click event
        /// <summary>
        /// btnUpdate_Click
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnUpdate_Click(object sender, EventArgs e)
        {
            EyouSoft.Model.SSOStructure.MWebmasterInfo webmasterInfo = EyouSoft.Security.Membership.WebmasterProvider.GetWebmasterInfo();

            if (webmasterInfo != null)
            {
                EyouSoft.Model.CompanyStructure.PassWord pwd = new EyouSoft.Model.CompanyStructure.PassWord();
                pwd.NoEncryptPassword = EyouSoft.Common.Utils.GetFormValue("t_p");


                if (string.IsNullOrEmpty(pwd.NoEncryptPassword))
                {
                    this.RegisterAlertAndRedirectScript("登录密码更新成功", "");
                    return;
                }

                EyouSoft.BLL.SysStructure.BSys bll = new EyouSoft.BLL.SysStructure.BSys();
                if (bll.SetWebmasterPwd(webmasterInfo.UserId, webmasterInfo.Username, pwd))
                {
                    this.RegisterAlertAndRedirectScript("登录密码更新成功", "");
                }
                else
                {
                    this.RegisterAlertAndRedirectScript("登录密码更新失败", "");
                }
            }
        }
        
        /// <summary>
        /// btnMD5Encrypt_Click
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnMD5Encrypt_Click(object sender, EventArgs e)
        {
            string s = EyouSoft.Common.Utils.GetFormValue("t_s");

            if (string.IsNullOrEmpty(s))
            {
                this.RegisterAlertAndRedirectScript("输入不能为空！", "");
                return;
            }

            EyouSoft.Model.CompanyStructure.PassWord p = new EyouSoft.Model.CompanyStructure.PassWord();
            p.NoEncryptPassword = s;

            this.RegisterAlertAndRedirectScript(p.MD5Password, "");
        }

        /// <summary>
        /// btnRemoveCache_Click
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnRemoveCache_Click(object sender, EventArgs e)
        {
            string s = EyouSoft.Common.Utils.GetFormValue("t_cache_name");

            if (string.IsNullOrEmpty(s))
            {
                this.RegisterAlertAndRedirectScript("缓存键不能为空", "");
                return;
            }

            new EyouSoft.BLL.SysStructure.BSys().RemoveCache(s);

            this.RegisterAlertAndRedirectScript("缓存移除成功", null);
        }
        #endregion
    }
}
