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
using System.Text;
using EyouSoft.Common;
using EyouSoft.Model.EnumType.CustomerStructure;

namespace Web.Common
{
    

    public partial class AwakePage : EyouSoft.Common.Page.BackPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            GetAjaxHtml();
        }

        private void GetAjaxHtml()
        {
            Response.Clear();
            var sb = new StringBuilder();

            var list = new EyouSoft.BLL.CRM.BUserBirthday().GetBirthdayWindow(SiteUserInfo.CompanyId, null);
            int peopleNum = 0;
            if (list != null && list.Any())
            {
                peopleNum = list.Count(t => (t.PeopleType == BirthdayGiftType.员工));
                if (peopleNum > 0)
                {
                    sb.AppendFormat(
                        "<p><a href=\"javascript:window.opener.location.href='http/CustomerManage/YuangongList.aspx?birthdaystart={0}&birthdayend={1}';window.opener.focus();\";  >今天有{2}名{3}过生日！</a></p>",
                        DateTime.Now.ToString("MM-dd"),
                        DateTime.Now.ToString("MM-dd"),
                        peopleNum,
                        BirthdayGiftType.员工);
                }
                peopleNum = list.Count(t => (t.PeopleType == BirthdayGiftType.组团联系人));
                if (peopleNum > 0)
                {
                    sb.AppendFormat(
                        "<p><a href=\"javascript:window.opener.location.href='http/CustomerManage/ZutuansheList.aspx?birthdaystart={0}&birthdayend={1}';window.opener.focus();\";  >今天有{2}名{3}过生日！</a></p>",
                        DateTime.Now.ToString("MM-dd"),
                        DateTime.Now.ToString("MM-dd"),
                        peopleNum,
                        BirthdayGiftType.组团联系人);
                }
                peopleNum = list.Count(t => (t.PeopleType == BirthdayGiftType.地接联系人));
                if (peopleNum > 0)
                {
                    sb.AppendFormat(
                        "<p><a href=\"javascript:window.opener.location.href='http/CustomerManage/DijiesheList.aspx?birthdaystart={0}&birthdayend={1}';window.opener.focus();\";  >今天有{2}名{3}过生日！</a></p>",
                        DateTime.Now.ToString("MM-dd"),
                        DateTime.Now.ToString("MM-dd"),
                        peopleNum,
                        BirthdayGiftType.地接联系人);
                }
                peopleNum = list.Count(t => (t.PeopleType == BirthdayGiftType.导游));
                if (peopleNum > 0)
                {
                    sb.AppendFormat(
                        "<p><a href=\"javascript:window.opener.location.href='http/CustomerManage/DaoYouList.aspx?birthdaystart={0}&birthdayend={1}';window.opener.focus();\";  >今天有{2}名{3}过生日！</a></p>",
                        DateTime.Now.ToString("MM-dd"),
                        DateTime.Now.ToString("MM-dd"),
                        peopleNum,
                        BirthdayGiftType.导游);
                }
                peopleNum = list.Count(t => (t.PeopleType == BirthdayGiftType.景点联系人));
                if (peopleNum > 0)
                {
                    sb.AppendFormat(
                        "<p><a href=\"javascript:window.opener.location.href='http/CustomerManage/JingdianList.aspx?birthdaystart={0}&birthdayend={1}';window.opener.focus();\";  >今天有{2}名{3}过生日！</a></p>",
                        DateTime.Now.ToString("MM-dd"),
                        DateTime.Now.ToString("MM-dd"),
                        peopleNum,
                        BirthdayGiftType.景点联系人);
                }
                peopleNum = list.Count(t => (t.PeopleType == BirthdayGiftType.游客));
                if (peopleNum > 0)
                {
                    sb.AppendFormat(
                        "<p><a href=\"javascript:window.opener.location.href='http/CustomerManage/YouKeList.aspx?birthdaystart={0}&birthdayend={1}';window.opener.focus();\";  >今天有{2}名{3}过生日！</a></p>",
                        DateTime.Now.ToString("MM-dd"),
                        DateTime.Now.ToString("MM-dd"),
                        peopleNum,
                        BirthdayGiftType.游客);
                }
            }

            Utils.SetCookie(
                EyouSoft.Security.Membership.UserProvider.LoginCookieLastLogTime,
                DateTime.Now.ToString("yyyy-M-d-H-m-s"));
            Response.Write(sb.ToString());
            Response.End();
        }
    }
}
