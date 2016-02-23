using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EyouSoft.Toolkit.SoapHeader
{
    #region aoapheader
    /// <summary>
    /// soapheader
    /// </summary>
    public abstract class ApiSoapHeader : System.Web.Services.Protocols.SoapHeader
    {
        protected string _localhostkey = string.Empty;
        private string _secretkey = string.Empty;

        /// <summary>
        /// 获取或设置密钥
        /// </summary>
        public string SecretKey
        {
            get { return _secretkey; }
            set { _secretkey = value; }
        }

        /// <summary>
        /// 是否为安全的API调用
        /// </summary>
        public bool IsSafeCall
        {
            get
            {
                if (_localhostkey == _secretkey)
                {
                    return true;
                }

                return false;
            }
        }

        /// <summary>
        /// set localhost key
        /// </summary>
        protected abstract void SetLocalHostKey();
    }
    #endregion

    #region sms center soapheader
    /// <summary>
    /// sms center soapheader
    /// </summary>
    public class SmsCenterApiSoapHeader : ApiSoapHeader
    {
        /// <summary>
        /// default constructor
        /// </summary>
        public SmsCenterApiSoapHeader() { SetLocalHostKey(); }

        /// <summary>
        ///  set localhost key
        /// </summary>
        protected override void SetLocalHostKey()
        {
            _localhostkey = System.Configuration.ConfigurationManager.AppSettings.Get("SmsCenter_ApiKey");
        }
    }
    #endregion

    #region sms center webmaster soapheader
    /// <summary>
    /// sms center webmaster soapheader
    /// </summary>
    public class SmsCenterWebmasterApiSoapHeader : ApiSoapHeader
    {
        /// <summary>
        /// default constructor
        /// </summary>
        public SmsCenterWebmasterApiSoapHeader() { SetLocalHostKey(); }

        /// <summary>
        ///  set localhost key
        /// </summary>
        protected override void SetLocalHostKey()
        {
            _localhostkey = System.Configuration.ConfigurationManager.AppSettings.Get("SmsCenter_Webmaster_ApiKey");
        }
    }
    #endregion

    #region sms soapheader
    /// <summary>
    /// sms soapheader
    /// </summary>
    public class SmsApiSoapHeader : ApiSoapHeader
    {
        /// <summary>
        /// default constructor
        /// </summary>
        public SmsApiSoapHeader() { SetLocalHostKey(); }

        /// <summary>
        ///  set localhost key
        /// </summary>
        protected override void SetLocalHostKey()
        {
            _localhostkey = System.Configuration.ConfigurationManager.AppSettings.Get("SmsApi_ApiKey");
        }
    }
    #endregion
}
