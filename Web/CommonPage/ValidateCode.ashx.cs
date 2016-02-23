using System;
using System.Collections;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Web.Services.Protocols;
using System.Xml.Linq;
using EyouSoft.Common.ValidateNumberAndChar;

namespace Web.ashx
{
    /// <summary>
    /// 页面：处理登录页面的验证码
    /// </summary>
    /// 创建人：戴银柱
    /// 创建时间：2011-9-20
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]    
    public class ValidateCode : IHttpHandler
    {
        private string CookieName = "SYS_VC";
        public void ProcessRequest(HttpContext context)
        {
            if (!string.IsNullOrEmpty(context.Request.QueryString["ValidateCodeName"]))
            {
                CookieName = context.Request.QueryString["ValidateCodeName"];
            }
            getNumbers(4,context);
        }

        private void getNumbers(int len,HttpContext context)
        {
            ValidateNumberAndChar.CurrentLength = len;
            ValidateNumberAndChar.CurrentNumber = ValidateNumberAndChar.CreateValidateNumber(ValidateNumberAndChar.CurrentLength);
            string number = ValidateNumberAndChar.CurrentNumber;
            ValidateNumberAndChar.CreateValidateGraphic(context, number);
            context.Response.Cookies[CookieName].Value = number;
        }

        public bool IsReusable
        {
            get
            {
                return false;
            }
        }
    }
}
