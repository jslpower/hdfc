//2011-09-23 汪奇志
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;

namespace EyouSoft.Security.Membership
{
    /// <summary>
    /// webmaster登录处理类
    /// </summary>
    public sealed class WebmasterProvider
    {
        #region static constants
        //static constants
        /// <summary>
        /// webmaster login session name
        /// </summary>
        public const string LoginSessionName = "SYS_WEBMASTER";
        #endregion

        #region private members
        /// <summary>
        /// set login session
        /// </summary>
        /// <param name="info">webmaster info</param>
        private static void SetSession(EyouSoft.Model.SSOStructure.MWebmasterInfo info)
        {
            HttpContext.Current.Session[LoginSessionName] = info;
        }

        /// <summary>
        /// get login session
        /// </summary>
        /// <returns></returns>
        private static EyouSoft.Model.SSOStructure.MWebmasterInfo GetSession()
        {
            return (EyouSoft.Model.SSOStructure.MWebmasterInfo)HttpContext.Current.Session[LoginSessionName];
        }

        /// <summary>
        /// remove login session
        /// </summary>
        private static void RemoveSession()
        {
            SetSession(null);
        }
        #endregion

        #region public members
        /// <summary>
        /// webmaster login
        /// </summary>
        /// <param name="username">username</param>
        /// <param name="pwdInfo">pwd</param>
        /// <param name="uInfo">webmaster info</param>
        /// <returns></returns>
        public static int Login(string username, Model.CompanyStructure.PassWord pwdInfo, out Model.SSOStructure.MWebmasterInfo uInfo)
        {
            IWebmasterLogin dal = new DWebmasterLogin();
            uInfo = null;

            if (string.IsNullOrEmpty(username)) return 0;
            if (pwdInfo == null || string.IsNullOrEmpty(pwdInfo.NoEncryptPassword)) return -1;

            uInfo = dal.Login(username, pwdInfo);

            if (uInfo == null) return -3;

            SetSession(uInfo);

            return 1;
        }

        /// <summary>
        /// get logion webmaster info
        /// </summary>
        /// <returns></returns>
        public static EyouSoft.Model.SSOStructure.MWebmasterInfo GetWebmasterInfo()
        {
            return GetSession();
        }

        /// <summary>
        /// webmaster is login
        /// </summary>
        /// <param name="info">out webmaster info</param>
        /// <returns></returns>
        public static bool IsLogin(out EyouSoft.Model.SSOStructure.MWebmasterInfo info)
        {
            info = GetWebmasterInfo();

            if (info == null) return false;

            return true;
        }

        /// <summary>
        /// webmaster is login
        /// </summary>
        /// <returns></returns>
        public static bool IsLogin()
        {
            EyouSoft.Model.SSOStructure.MWebmasterInfo info = null;

            return IsLogin(out info);
        }

        /// <summary>
        /// webmaster logout
        /// </summary>
        public static void Logout()
        {
            RemoveSession();
        }
        #endregion
    }
}
