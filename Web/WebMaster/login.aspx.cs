using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Text;

namespace Web.Webmaster
{
    /// <summary>
    /// webmaster login page
    /// </summary>
    /// Author:汪奇志 2011-09-23
    public partial class login : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            //throw new SystemException("system exception test.");
            //text:000000 MD5:670b14728ad9902aecba32e22fa4f6bd
        }

        /// <summary>
        /// btnLogin click event
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnLogin_Click(object sender, EventArgs e)
        {
            EyouSoft.Model.CompanyStructure.PassWord pwd = new EyouSoft.Model.CompanyStructure.PassWord();
            string username = EyouSoft.Common.Utils.GetFormValue("t_u");
            pwd.NoEncryptPassword = EyouSoft.Common.Utils.GetFormValue("t_p");

            if (string.IsNullOrEmpty(username))
            {
                this.RegisterAlertScript("Please enter your login information.");
            }

            if (string.IsNullOrEmpty(pwd.NoEncryptPassword))
            {
                this.RegisterAlertScript("Please enter a password.");
            }

            EyouSoft.Model.SSOStructure.MWebmasterInfo webmasterInfo = null;
            EyouSoft.Security.Membership.WebmasterProvider.Login(username, pwd, out webmasterInfo);

            if (webmasterInfo != null)
            {
                Response.Redirect("default.aspx");
            }

            this.RegisterAlertScript("Please enter the correct password.");
        }

        #region private members
        /// <summary>
        /// register alert script
        /// </summary>
        /// <param name="s">msg</param>
        private void RegisterAlertScript(string s)
        {
            this.Page.ClientScript.RegisterClientScriptBlock(this.GetType(), Guid.NewGuid().ToString(), string.Format("alert('{0}');", s), true);
        }
        #endregion
    }
}
