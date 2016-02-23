using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;

namespace Web.SOAP
{
    /// <summary>
    /// SMSAPI 的摘要说明
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    public class SMSAPI : System.Web.Services.WebService
    {
        public Web.SOAP.APISoapHeader apisoapHeader = new Web.SOAP.APISoapHeader();

        /// <summary>
        /// 发送短信服务
        /// </summary>
        /// <param name="sendMessageInfo">发送短信提交的业务实体</param>
        /// <returns>返回发送结果</returns>
        [System.Web.Services.Protocols.SoapHeader("apisoapHeader")]
        [WebMethod]
        public EyouSoft.Model.SMSStructure.SendResultInfo Send(EyouSoft.Model.SMSStructure.SendMessageInfo sendMessageInfo)
        {

            if (!apisoapHeader.IsSafeCall)
            {
                throw new Exception("对不起，您没有权限调用此服务！");
            }

            return new EyouSoft.BLL.SMSStructure.SendMessage().Send(sendMessageInfo);
        }
    }
}
