using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Web
{
    /// <summary>
    /// 用户登录跳转处理
    /// </summary>
    public partial class _Default : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            EyouSoft.Model.SysStructure.SystemDomain domain = EyouSoft.Security.Membership.UserProvider.GetDomain();

            if (domain == null || domain.CompanyId < 1 || domain.SysId < 1)
            {
                Response.Clear();
                Response.Write("请求异常：错误的域名配置。");
                Response.End();
            }

            EyouSoft.Model.SSOStructure.MUserInfo uinfo = null;
            string url = "/CompanyFiles/MsgManageList.aspx";

            bool isLogin = EyouSoft.Security.Membership.UserProvider.IsLogin(out uinfo);

            if (!isLogin)
            {
                if (!string.IsNullOrEmpty(domain.Url))
                {
                    url = domain.Url;
                }
                else
                {
                    url = "/login.aspx";
                }

                Response.Redirect(url, true);
            }

            if (uinfo.UserType == EyouSoft.Model.EnumType.CompanyStructure.UserType.地接用户)
            {
                url = "/jidiaoCenter/DijieList.aspx";
            }
            else if (uinfo.UserType == EyouSoft.Model.EnumType.CompanyStructure.UserType.票务用户)
            {
                url = "/jidiaoCenter/TicketList.aspx";
            }
            else
            {
                if (EyouSoft.Security.Membership.UserProvider.IsPrivs3(uinfo.Privs, (int)EyouSoft.Model.EnumType.PrivsStructure.Privs3.计调中心_确认件登记_栏目))
                {
                    url = "/jidiaoCenter/TeamList.aspx";
                }
            }
            
            Response.Redirect(url, true);
        }
    }
}
