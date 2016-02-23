using System;
using System.Collections.Generic;
using System.Web;
using System.Web.Services;
using System.Text.RegularExpressions;

namespace Web.SOAP
{
    /// <summary>
    /// 发送短信测试WEB服务
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    public class TESTSMS : System.Web.Services.WebService
    {
        [WebMethod]
        public int SendSms(string enterpriseid, string mobile, string content, string account, string pwd)
        {
            int result=0;

            if (!this.IsMobile(mobile))
            {
                return -1000;
            }

            int second = 0;

            try
            {
                second = DateTime.Now.Second;

                int mod = Convert.ToInt32(second % 7);

                switch (mod)
                {
                    case 0: 
                        result = 0;
                        break;
                    case 1:
                        result = -1000;
                        break;
                    case 2:
                        result = -1001;
                        break;
                    case 3:
                        result = -1002;
                        break;
                    case 4:
                        result = -1003;
                        break;
                    case 5:
                        result = -1004;
                        break;
                    default:
                        result = -1000;
                        break;
                }
            }
            catch
            {
                result=0;
            }

            return result;
        }

       
        /// <summary>
        /// 判断是否是手机号码或小灵通号码
        /// </summary>
        /// <param name="inputData"></param>
        /// <returns></returns>
        private bool IsMobile(string inputData)
        {
            Regex RegPHSNo = new Regex(@"^0(([1-9]\d)|([3-9]\d{2}))\d{8}$");
            Regex RegMobileNo = new Regex(@"^(13[0-9]|15[0|1|2|3|5|6|7|8|9]|18[6|8|9])\d{8}$"); 

            if (inputData != null)
            {
                Match match1 = RegMobileNo.Match(inputData);
                Match match2 = RegPHSNo.Match(inputData);
                return match1.Success || match2.Success;
            }
            else
            {
                return false;
            }
        }
    }
}
