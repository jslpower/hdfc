using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Web.Webmaster
{
    /// <summary>
    /// webmaster inner page masterpage
    /// </summary>
    /// Author:汪奇志 2011-09-23
    public partial class mpage : System.Web.UI.MasterPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected override void OnInit(EventArgs e)
        {
            bool isLogin = EyouSoft.Security.Membership.WebmasterProvider.IsLogin();

            if (!isLogin)
            {
                HttpContext.Current.Response.Redirect(WebmasterPageBase.LoginFilePath);
            }
        }
    }
}
