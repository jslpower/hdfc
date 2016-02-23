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

namespace Web
{
    /// <summary>
    /// 处理登录请求
    /// </summary>
    /// 戴银柱 2011-09-19
    public partial class logindomain : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            string u = Utils.InputText(Request.QueryString["u"]);
            string p = Utils.InputText(Request.QueryString["p"]);
            string pmd = Utils.InputText(Request.QueryString["pmd"]);
            string vc = Utils.InputText(Request.QueryString["vc"]);
            string callback = Utils.InputText(Request.QueryString["callback"]);

            var domain = EyouSoft.Security.Membership.UserProvider.GetDomain();

            if (domain == null || domain.CompanyId<1)
            {
                Response.Clear();
                Response.Write(";" + callback + "({m:'系统域名配置错误'});");
                Response.End();
            }

            int companyId = domain.CompanyId;

            int isUserValid = 0;
            EyouSoft.Model.SSOStructure.MUserInfo userInfo = null;

            EyouSoft.Model.CompanyStructure.PassWord pwdInfo = new EyouSoft.Model.CompanyStructure.PassWord();
            //pwdInfo.NoEncryptPassword = p;
            pwdInfo.SetEncryptPassWord(p, pmd);
            isUserValid = EyouSoft.Security.Membership.UserProvider.Login(companyId, u, pwdInfo, out userInfo);

            if (isUserValid == 1)
            {
                Response.Clear();
                Response.Write(";" + callback + "({h:1});");
                Response.End();
            }
            else if (isUserValid == -4)
            {
                Response.Clear();
                Response.Write(";" + callback + "({m:'用户名或密码不正确'});");
                Response.End();
            }
            else if (isUserValid == -7)
            {
                Response.Clear();
                Response.Write(";" + callback + "({m:'您的账户已停用或已过期，请联系管理员'});");
                Response.End();
            }
            else if (isUserValid == -8)
            {
                Response.Clear();
                Response.Write(";" + callback + "({m:'您的账户已登录，不能重复登录'});");
                Response.End();
            }
            else
            {
                Response.Clear();
                Response.Write(";" + callback + "({m:'登录异常，请联系管理员'});");
                Response.End();
            }
        }
    }
}
