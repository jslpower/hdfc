using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using EyouSoft.Common;
using EyouSoft.Common.Function;
namespace Web.SMS
{   
    /// <summary>
    /// 发送详情
    /// xuty 2011/1/28
    /// </summary>
    public partial class SendDetailDialog : EyouSoft.Common.Page.BackPage
    {
        protected string detailMess = string.Empty;//发送详情html
        protected void Page_Load(object sender, EventArgs e)
        {
            //判断权限
            if (!CheckGrant(EyouSoft.Model.EnumType.PrivsStructure.Privs3.短信中心_短信中心_栏目))
            {
                Utils.ResponseNoPermit(EyouSoft.Model.EnumType.PrivsStructure.Privs3.短信中心_短信中心_栏目, true);
                return;
            }
            string method = Utils.GetQueryStringValue("method");
            string tid=Utils.GetQueryStringValue("tid");
            EyouSoft.BLL.SMSStructure.SendMessage sendBll = new EyouSoft.BLL.SMSStructure.SendMessage();
            StringBuilder strBuilder = new StringBuilder();
            strBuilder.Append("<table width='90%' cellspacing='1' cellpadding='0' border='0'>");
            //获取发送详情列表1成功2失败
            if (method == "1"||method=="2")
            {
                //获取发送详情
               IList<EyouSoft.Model.SMSStructure.SendDetail> list= sendBll.GetSendDetails(tid, CurrentUserCompanyID.ToString(), Utils.GetInt(method));
               strBuilder.Append("<tr><td class='odd'>手机号码</td><td class='odd'>发送状态</td></tr>");
                if (list != null && list.Count > 0)
               {
                   foreach (EyouSoft.Model.SMSStructure.SendDetail detail in list)
                   {
                       strBuilder.AppendFormat("<tr><td class='even'>{0}</td><td class='even'>{1}</td></tr>", detail.MobileNumber, detail.ReturnResult == 0 ? "发送成功" : "发送失败");
                   }
               }
               else
               {
                   strBuilder.Append("<tr><td colspan='2'>无号码</tr>");
               }
            }
            else if (method == "mess")
            {  
                //获取发送内容
                strBuilder.Append("<tr><td class='odd'>发送内容</td></tr>");
                string content=sendBll.GetSendContent(tid, CurrentUserCompanyID.ToString());
                strBuilder.AppendFormat("<tr><td class='even'>{0}</td></tr>", content != null ? content : "无信息");
            }
            strBuilder.Append("</table>");
            detailMess = strBuilder.ToString();
        }
    }
}
