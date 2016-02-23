//2011-09-23 汪奇志
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using EyouSoft.Model.CompanyStructure;
using EyouSoft.Model.SSOStructure;
using EyouSoft.Model.SysStructure;
using EyouSoft.Cache.Facade;

namespace EyouSoft.Security.Membership
{
    /// <summary>
    /// 系统用户登录处理类
    /// </summary>
    public sealed class UserProvider
    {
        #region static constants
        //static constants
        /// <summary>
        /// 登录Cookie，用户编号
        /// </summary>
        public const string LoginCookieUserId = "SYS_YIBAI_UID";
        /// <summary>
        /// 登录Cookie，用户账号
        /// </summary>
        public const string LoginCookieUsername = "SYS_YIBAI_UN";
        /// <summary>
        /// 登录Cookie，公司编号
        /// </summary>
        public const string LoginCookieCompanyId = "SYS_YIBAI_CID";
        /// <summary>
        /// 登录Cookie，系统编号
        /// </summary>
        public const string LoginCookieSysId = "SYS_YIBAI_SID";
        /// <summary>
        /// 登录Cookie，客服登录
        /// </summary>
        public const string LoginCookieKeFu = "SYS_YIBAI_KF";
        /// <summary>
        /// 登录Cookie，会话标识
        /// </summary>
        public const string LoginCookieSessionId = "SYS_YIBAI_SESSIONID";

        /// <summary>
        /// 登录Cookie
        /// 作为存储用户最后登录时间的KEY.
        /// 存储的时间格式为：year-month-day-hour-minutes-seconds.
        /// </summary>
        public const string LoginCookieLastLogTime = "lastlogintime";

        #endregion

        #region private members
        /// <summary>
        /// 设置登录用户cache
        /// </summary>
        /// <param name="info">登录用户信息</param>
        private static void SetUserCache(MUserInfo info)
        {
            string cacheKey = string.Format(Cache.Tag.TagName.ComUser, info.CompanyId, info.UserId);
            EyouSoftCache.Remove(cacheKey);
            EyouSoftCache.Add(cacheKey, info, DateTime.Now.AddHours(12));
        }

        /// <summary>
        /// 移除登录用户cache
        /// </summary>
        /// <param name="companyId">公司编号</param>
        /// <param name="userId">用户编号</param>
        private static void RemoveUserCache(int companyId, int userId)
        {
            string cacheKey = string.Format(Cache.Tag.TagName.ComUser, companyId, userId);

            EyouSoftCache.Remove(cacheKey);
        }

        /// <summary>
        /// 获取登录用户Cookie信息
        /// </summary>
        /// <param name="name">登录Cookie名称</param>
        /// <returns></returns>
        private static string GetCookie(string name)
        {
            HttpRequest request = HttpContext.Current.Request;

            if (request.Cookies[name] == null)
            {
                return string.Empty;
            }

            return HttpContext.Current.Server.UrlDecode(request.Cookies[name].Value);
        }

        /// <summary>
        /// 移除登录用户Cookie
        /// </summary>
        private static void RemoveCookies()
        {
            HttpResponse response = HttpContext.Current.Response;

            response.Cookies.Remove(LoginCookieCompanyId);
            response.Cookies.Remove(LoginCookieSysId);
            response.Cookies.Remove(LoginCookieUserId);
            response.Cookies.Remove(LoginCookieUsername);
            response.Cookies.Remove(LoginCookieSessionId);
            response.Cookies.Remove(LoginCookieKeFu);

            DateTime cookiesExpiresDateTime = DateTime.Now.AddDays(-1);

            response.Cookies[LoginCookieCompanyId].Expires = cookiesExpiresDateTime;
            response.Cookies[LoginCookieSysId].Expires = cookiesExpiresDateTime;
            response.Cookies[LoginCookieUserId].Expires = cookiesExpiresDateTime;
            response.Cookies[LoginCookieUsername].Expires = cookiesExpiresDateTime;
            response.Cookies[LoginCookieSessionId].Expires = cookiesExpiresDateTime;
            response.Cookies[LoginCookieKeFu].Expires = cookiesExpiresDateTime;
        }

        /// <summary>
        /// 设置登录Cookies
        /// </summary>
        /// <param name="info">登录用户信息</param>
        private static void SetCookies(MUserInfo info)
        {
            //Cookies生存周期为浏览器进程
            HttpResponse response = HttpContext.Current.Response;

            RemoveCookies();

            var cookie = new HttpCookie(LoginCookieCompanyId);
            cookie.Value = info.CompanyId.ToString();
            cookie.HttpOnly = true;
            response.AppendCookie(cookie);

            cookie = new HttpCookie(LoginCookieSysId);
            cookie.Value = info.SysId.ToString();
            cookie.HttpOnly = true;
            response.AppendCookie(cookie);

            cookie = new HttpCookie(LoginCookieUserId);
            cookie.Value = info.UserId.ToString();
            cookie.HttpOnly = true;
            response.AppendCookie(cookie);

            cookie = new HttpCookie(LoginCookieUsername);
            cookie.Value = HttpContext.Current.Server.UrlEncode(info.Username);
            cookie.HttpOnly = true;
            response.AppendCookie(cookie);

            cookie = new HttpCookie(LoginCookieSessionId);
            cookie.Value = info.OnlineSessionId;
            cookie.HttpOnly = true;
            response.AppendCookie(cookie);

            cookie = new HttpCookie(LoginCookieLastLogTime);
            cookie.Value = DateTime.Now.ToString("yyyy-M-d-H-m-s");
            //cookie.HttpOnly = true;
            response.AppendCookie(cookie);
        }

        /// <summary>
        /// 设置客服登录Cookies
        /// </summary>
        private static void SetKeFuLoginCookies()
        {
            HttpResponse response = HttpContext.Current.Response;

            var cookie = new HttpCookie(LoginCookieKeFu);
            cookie.Value = "Y";
            cookie.HttpOnly = true;
            response.AppendCookie(cookie);

        }

        /// <summary>
        /// 自动登录处理
        /// </summary>
        /// <param name="sysId">系统编号</param>
        /// <param name="companyId">公司编号</param>
        /// <param name="userId">用户编号</param>
        /// <param name="username">用户账号</param>  
        /// <param name="uInfo">登录用户信息</param>
        private static void AutoLogin(int sysId, int companyId, int userId, string username, out MUserInfo uInfo)
        {
            uInfo = null;
            IUserLogin dal = new DUserLogin();
            SystemDomain domainInfo = GetDomain();
            if (domainInfo == null || domainInfo.SysId != sysId || domainInfo.CompanyId != companyId) { uInfo = null; return; }

            uInfo = dal.Login(userId);

            if (uInfo == null) return;
            if (uInfo.Username != username) { uInfo = null; return; }
            if (uInfo.CompanyId != companyId) { uInfo = null; return; }

            if (uInfo.Status != Model.EnumType.CompanyStructure.UserStatus.正常) { uInfo = null; return; }

            uInfo.SysId = sysId;
            uInfo.LoginTime = uInfo.LastLoginTime.HasValue ? uInfo.LastLoginTime.Value : DateTime.Now;

            dal.LoginLogwr(uInfo, Model.EnumType.CompanyStructure.UserLoginType.自动登录);

            SetUserCache(uInfo);
        }

        /// <summary>
        /// 是否客服登录
        /// </summary>
        /// <returns></returns>
        private static bool IsKeFuLogin()
        {
            return GetCookie(LoginCookieKeFu) == "Y";
        }

        /// <summary>
        /// 根据用户类型设置用户权限(主要处理供应商用户，专线用户不做处理)
        /// </summary>
        /// <param name="userType">用户类型</param>
        /// <param name="uInfo">用户实体</param>
        private static void SetPrivsByUserType(Model.EnumType.CompanyStructure.UserType userType, MUserInfo uInfo)
        {
            if (userType == Model.EnumType.CompanyStructure.UserType.专线用户 || uInfo == null) return;

            if (userType == Model.EnumType.CompanyStructure.UserType.地接用户)
            {
                var privs = new List<int>
                    {
                        (int)Model.EnumType.PrivsStructure.Privs3.计调中心_地接安排_栏目,
                        //(int)Model.EnumType.PrivsStructure.Privs3.计调中心_地接安排_新增,
                        //(int)Model.EnumType.PrivsStructure.Privs3.计调中心_地接安排_修改,
                        //(int)Model.EnumType.PrivsStructure.Privs3.计调中心_地接安排_确认
                    };
                uInfo.Privs = privs.ToArray();
            }
            if (userType == Model.EnumType.CompanyStructure.UserType.票务用户)
            {
                var privs = new List<int>
                    {
                        (int)Model.EnumType.PrivsStructure.Privs3.计调中心_票务安排_栏目,
                        //(int)Model.EnumType.PrivsStructure.Privs3.计调中心_票务安排_出票申请,
                        //(int)Model.EnumType.PrivsStructure.Privs3.计调中心_票务安排_退票申请,
                        //(int)Model.EnumType.PrivsStructure.Privs3.计调中心_票务安排_出票修改,
                        //(int)Model.EnumType.PrivsStructure.Privs3.计调中心_票务安排_退票修改,
                        //(int)Model.EnumType.PrivsStructure.Privs3.计调中心_票务安排_出票确认,
                        //(int)Model.EnumType.PrivsStructure.Privs3.计调中心_票务安排_退票确认
                    };
                uInfo.Privs = privs.ToArray();
            }
        }

        #endregion

        #region public members
        /// <summary>
        /// 用户登录，返回1登录成功
        /// </summary>
        /// <param name="companyId">系统公司编号</param>
        /// <param name="username">用户名</param>
        /// <param name="pwdInfo">登录密码</param>
        /// <param name="uInfo">登录用户信息</param>
        /// <returns></returns>
        public static int Login(int companyId, string username, PassWord pwdInfo, out MUserInfo uInfo)
        {
            IUserLogin dal = new DUserLogin();
            uInfo = null;

            if (companyId <= 0) return 0;
            if (string.IsNullOrEmpty(username)) return -1;
            if (pwdInfo == null || string.IsNullOrEmpty(pwdInfo.NoEncryptPassword)) return -2;
            SystemDomain domainInfo = GetDomain();
            if (domainInfo == null) return -3;

            uInfo = dal.Login(companyId, username, pwdInfo);

            //通过用户名及密码验证失败，判断登录密码是否为客服服务密码，如果是将绕过密码验证
            //使用客服密码登录时登录日志做客服登录标识
            Model.EnumType.CompanyStructure.UserLoginType loginType = Model.EnumType.CompanyStructure.UserLoginType.用户登录;
            if (uInfo == null)
            {
                if (System.Configuration.ConfigurationManager.AppSettings["KeFuPwd"] == pwdInfo.MD5Password)
                {
                    uInfo = dal.Login(companyId, username);
                    loginType = Model.EnumType.CompanyStructure.UserLoginType.客服登录;
                }

                if (uInfo == null) return -4;
            }

            if (uInfo.Status != Model.EnumType.CompanyStructure.UserStatus.正常)
            {
                uInfo = null;
                return -7;
            }

            var setting = GetComSetting(companyId);


            switch (setting.UserLoginLimitType)
            {
                case Model.EnumType.CompanyStructure.UserLoginLimitType.None: break;
                case Model.EnumType.CompanyStructure.UserLoginLimitType.Earliest:
                    if (loginType == Model.EnumType.CompanyStructure.UserLoginType.用户登录
                        && uInfo.OnlineStatus == Model.EnumType.CompanyStructure.UserOnlineStatus.Online)
                    {
                        uInfo = null;
                        return -8;
                    }
                    break;
                case Model.EnumType.CompanyStructure.UserLoginLimitType.Latest: break;
                default: break;
            }

            uInfo.SysId = domainInfo.SysId;
            uInfo.LoginTime = DateTime.Now;

            if (loginType == Model.EnumType.CompanyStructure.UserLoginType.用户登录)
            {
                uInfo.OnlineStatus = Model.EnumType.CompanyStructure.UserOnlineStatus.Online;
                uInfo.OnlineSessionId = Guid.NewGuid().ToString();
            }

            dal.LoginLogwr(uInfo, loginType);

            SetUserCache(uInfo);
            SetCookies(uInfo);
            SetPrivsByUserType(uInfo.UserType, uInfo);
            if (loginType == Model.EnumType.CompanyStructure.UserLoginType.客服登录)
            {
                SetKeFuLoginCookies();
            }

            return 1;
        }

        /// <summary>
        /// 获取当前域名信息
        /// </summary>
        /// <returns></returns>
        public static SystemDomain GetDomain()
        {
            string s = HttpContext.Current.Request.Url.Host.ToLower();

            var domains = (IDictionary<string, SystemDomain>)EyouSoftCache.GetCache(EyouSoft.Cache.Tag.TagName.SysDomains);
            SystemDomain info = null;
            domains = domains ?? new Dictionary<string, SystemDomain>();
            if (domains.ContainsKey(s))
            {
                info = domains[s];
            }
            else
            {
                IUserLogin dal = new DUserLogin();
                info = dal.GetDomain(s);
                if (info != null)
                {
                    domains.Add(s, info);
                    EyouSoftCache.Add(Cache.Tag.TagName.SysDomains, domains);
                }
            }

            return info;
        }

        /// <summary>
        /// 用户退出
        /// </summary>
        public static void Logout()
        {
            int companyId = Toolkit.Utils.GetInt(GetCookie(LoginCookieCompanyId));
            int userId = Toolkit.Utils.GetInt(GetCookie(LoginCookieUserId));

            if (!IsKeFuLogin() && userId > 0 && companyId > 0)
            {
                RemoveUserCache(companyId, userId);
            }

            RemoveCookies();

            if (!IsKeFuLogin())
            {
                IUserLogin dal = new DUserLogin();
                dal.SetOnlineStatus(userId, Model.EnumType.CompanyStructure.UserOnlineStatus.Offline, "00000000-0000-0000-0000-000000000000");
            }
        }

        /// <summary>
        /// 获取登录用户信息
        /// </summary>
        /// <returns></returns>
        public static MUserInfo GetUserInfo()
        {
            MUserInfo info = null;
            int companyId = Toolkit.Utils.GetInt(GetCookie(LoginCookieCompanyId));
            int userId = Toolkit.Utils.GetInt(GetCookie(LoginCookieUserId));
            string username = GetCookie(LoginCookieUsername);
            int sysId = Toolkit.Utils.GetInt(GetCookie(LoginCookieSysId));


            if (companyId <= 0
                || userId <= 0
                || string.IsNullOrEmpty(username)
                || sysId <= 0)
            {
                return null;
            }

            //从缓存查询登录用户信息
            string cacheKey = string.Format(Cache.Tag.TagName.ComUser, companyId, userId);
            //从缓存查询登录用户信息计数器
            int getCacheCount = 2;

            do
            {
                info = (MUserInfo)EyouSoftCache.GetCache(cacheKey);
                getCacheCount--;
            } while (info == null && getCacheCount > 0);

            //缓存中未找到登录用户信息，自动登录处理
            if (info == null)
            {
                AutoLogin(sysId, companyId, userId, username, out info);
            }

            if (info == null) return null;

            var setting = GetComSetting(companyId);

            if (!IsKeFuLogin())
            {
                switch (setting.UserLoginLimitType)
                {
                    case Model.EnumType.CompanyStructure.UserLoginLimitType.Earliest:
                        if (info.OnlineStatus == Model.EnumType.CompanyStructure.UserOnlineStatus.Offline) return null;
                        break;
                    case Model.EnumType.CompanyStructure.UserLoginLimitType.Latest:
                        if (info.OnlineSessionId != GetCookie(LoginCookieSessionId)) return null;
                        break;
                    case Model.EnumType.CompanyStructure.UserLoginLimitType.None:
                        break;
                    default: break;
                }
            }

            SetPrivsByUserType(info.UserType, info);

            return info;
        }

        /// <summary>
        /// 用户是否登录
        /// </summary>
        /// <param name="info">登录用户信息</param>
        /// <returns></returns>
        public static bool IsLogin(out MUserInfo info)
        {
            info = GetUserInfo();

            if (info == null) return false;

            return true;
        }

        /// <summary>
        /// 是否有指定权限（权限集范围内）
        /// </summary>
        /// <param name="userPrivs">权限集</param>
        /// <param name="privsId">要验证的权限</param>
        /// <returns></returns>
        public static bool IsPrivs3(int[] userPrivs, int privsId)
        {
            if (userPrivs != null && userPrivs.Length > 0 && userPrivs.Contains(privsId)) return true;

            return false;
        }

        /// <summary>
        /// 是否有指定权限（当前登录用户权限集）
        /// </summary>
        /// <param name="privsId">要验证的权限</param>
        /// <returns></returns>
        public static bool IsPrivs3(int privsId)
        {
            var info = GetUserInfo();

            if (info != null && info.Privs != null && info.Privs.Length > 0 && info.Privs.Contains(privsId)) return true;

            return false;
        }

        /// <summary>
        /// 获取公司配置信息
        /// </summary>
        /// <param name="companyId">公司编号</param>
        /// <returns></returns>
        public static CompanyFieldSetting GetComSetting(int companyId)
        {
            if (companyId <= 0) return null;

            string cacheName = string.Format(Cache.Tag.TagName.ComSetting, companyId);
            var setting = (CompanyFieldSetting)EyouSoftCache.GetCache(cacheName);

            if (setting == null)
            {
                IUserLogin dal = new DUserLogin();
                setting = dal.GetComSetting(companyId);
                EyouSoftCache.Add(cacheName, setting);
            }

            return setting;
        }

        /// <summary>
        /// 用户退出
        /// </summary>
        public static void Logout(int companyId, int userId)
        {
            if (userId > 0 && companyId > 0)
            {
                string cacheKey = string.Format(Cache.Tag.TagName.ComUser, companyId, userId);
                var info = (MUserInfo)EyouSoftCache.GetCache(cacheKey);

                if (info != null)
                {
                    info.OnlineStatus = Model.EnumType.CompanyStructure.UserOnlineStatus.Offline;
                    info.OnlineSessionId = string.Empty;
                }

                IUserLogin dal = new DUserLogin();
                dal.SetOnlineStatus(userId, EyouSoft.Model.EnumType.CompanyStructure.UserOnlineStatus.Offline, "00000000-0000-0000-0000-000000000000");
            }
        }
        #endregion


    }
}
