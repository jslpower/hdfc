using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Web.Webmaster
{
    #region webmaster page base
    /// <summary>
    /// webmaster page base
    /// </summary>
    /// Author:汪奇志 2011-09-23
    public class WebmasterPageBase : System.Web.UI.Page
    {
        #region attibutes
        /// <summary>
        /// login page file path
        /// </summary>
        public const string LoginFilePath = "/webmaster/login.aspx";
        #endregion

        protected override void OnInit(EventArgs e)
        {
            bool isLogin = EyouSoft.Security.Membership.WebmasterProvider.IsLogin();

            if (!isLogin)
            {
                HttpContext.Current.Response.Redirect(LoginFilePath);
            }

            base.OnInit(e);
        }

        #region protected members
        /// <summary>
        /// register alert script
        /// </summary>
        /// <param name="s">msg</param>
        protected void RegisterAlertScript(string s)
        {
            this.RegisterScript(string.Format("alert('{0}');", s));
        }

        /// <summary>
        /// register alert and redirect script
        /// </summary>
        /// <param name="s"></param>
        /// <param name="url">IsNullOrEmpty(url)=true page reload</param>
        protected void RegisterAlertAndRedirectScript(string s, string url)
        {
            if (!string.IsNullOrEmpty(url))
            {
                this.RegisterScript(string.Format("alert('{0}');window.location.href='{1}';", s, url));
            }
            else
            {
                this.RegisterScript(string.Format("alert('{0}');window.location.href=window.location.href;", s));
            }
        }

        /// <summary>
        /// register alert and reload script
        /// </summary>
        /// <param name="s"></param>
        protected void RegisterAlertAndReloadScript(string s)
        {
            RegisterAlertAndRedirectScript(s, null);
        }

        /// <summary>
        /// register scripts
        /// </summary>
        /// <param name="script"></param>
        protected void RegisterScript(string script)
        {
            this.Page.ClientScript.RegisterClientScriptBlock(this.GetType(), Guid.NewGuid().ToString(), script, true);
        }
        #endregion
    }
    #endregion
}
