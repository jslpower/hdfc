using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.UI.HtmlControls;
using System.Web;
using EyouSoft.Model.SSOStructure;

namespace EyouSoft.Common.Page
{
    /// <summary>
    /// back page base
    /// </summary>
    public class BackPage : System.Web.UI.Page
    {
        #region attributes

        /// <summary>
        /// 页面请求类型，是浏览器正常请求还是Ajax请求
        /// </summary>
        private bool isAjaxConnect = false;

        private bool _IsLogin = false;
        /// <summary>
        /// 是否登录
        /// </summary>
        public bool IsLogin
        {
            get
            {
                return _IsLogin;
            }
        }
        private EyouSoft.Model.SSOStructure.MUserInfo _userInfo = null;
        /// <summary>
        /// 当前用户信息
        /// </summary>
        public EyouSoft.Model.SSOStructure.MUserInfo SiteUserInfo
        {
            get
            {
                return _userInfo;
            }
        }

        /// <summary>
        /// 获取当前用户所在的公司ID
        /// </summary>
        public int CurrentUserCompanyID
        {
            get
            {
                return _userInfo.CompanyId;
            }
        }

        /// <summary>
        /// 快速登录页面URL
        /// </summary>
        public static string Url_MinLogin
        {
            get
            {
                return "/slogin.aspx";
            }
        }

        /// <summary>
        /// 统一金额显示格式
        /// </summary>
        public string ProviderToMoney
        {
            get
            {
                return "zh-cn";
            }
        }

        /// <summary>
        /// 获得金额前缀，如￥,$;
        /// </summary>
        public string ProviderMoneyStr
        {
            get
            {
                return UtilsCommons.GetMoneyString(0, this.ProviderToMoney).Substring(0, 1);
            }
        }

        /// <summary>
        /// 统一日期显示格式(短)
        /// </summary>
        public string ProviderToDate
        {
            get
            {
                return "yyyy-MM-dd";
            }
        }
        /// <summary>
        /// 统一日期显示格式(长)
        /// </summary>
        public string ProviderToDateLong
        {
            get
            {
                return "yyyy-MM-dd HH:mm:ss";
            }
        }
        #endregion

        #region private members

        #endregion

        #region protected override members
        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);

            EyouSoft.Model.SysStructure.SystemDomain domain = EyouSoft.Security.Membership.UserProvider.GetDomain();

            if (domain == null || domain.CompanyId < 1 || domain.SysId < 1)
            {
                RCWE("请求异常：错误的域名配置。");
            }

            //获取页面请求类型
            string urlType = EyouSoft.Common.Utils.GetQueryStringValue("urltype");
            if (urlType == "pageajax") isAjaxConnect = true;

            //初始化用户信息
            MUserInfo userInfo = null;
            _IsLogin = EyouSoft.Security.Membership.UserProvider.IsLogin(out userInfo);
            _userInfo = userInfo;

            if (!_IsLogin)//没有登录
            {
                //判断页面请求类型
                if (isAjaxConnect)//是Ajax请求
                {
                    RCWE("{\"Islogin\":\"false\"}");
                }
                else//普通浏览器请求
                {
                    string s = string.Empty;
                    s += "<script type='text/javascript'>";
                    s += string.Format(" if (\"{0}\" != \"\") {{", Utils.GetQueryStringValue("iframeId"));
                    s += string.Format(" window.parent.location.href = \"{0}\"; ", Utils.GetLoginUrl());
                    s += "} else {";
                    s += string.Format(
                        " window.location.href = \"{0}?returnurl={1}\"; ",
                        Utils.GetLoginUrl(),
                        Server.UrlEncode(Request.Url.ToString()));
                    s += "}";
                    s += "</script>";
                    RCWE(s);
                }
            }
        }

        protected override void OnPreRender(EventArgs e)
        {
            base.OnPreRender(e);
        }
        #endregion

        #region protected members
        /// <summary>
        /// response to xls
        /// </summary>
        /// <param name="s">要写入 HTTP 输出流的字符串。</param>
        protected void ResponseToXls(string s)
        {
            ResponseToXls(s, System.Text.Encoding.Default);
        }

        /// <summary>
        /// response to xls
        /// </summary>
        /// <param name="s">要写入 HTTP 输出流的字符串。</param>
        /// <param name="encoding">encoding</param>
        protected void ResponseToXls(string s, Encoding encoding)
        {
            string filename = DateTime.Now.ToString("yyyyMMddHHmmss") + ".xls";
            ResponseToXls(s, encoding, filename);
        }

        /// <summary>
        /// response to xls
        /// </summary>
        /// <param name="s">要写入 HTTP 输出流的字符串。</param>
        /// <param name="encoding">encoding</param>
        /// <param name="filename">filename</param>
        protected void ResponseToXls(string s, Encoding encoding, string filename)
        {
            if (string.IsNullOrEmpty(filename)) filename = DateTime.Now.ToString("yyyyMMddHHmmss") + ".xls";
            if (System.IO.Path.GetExtension(filename).ToLower() != ".xls") filename += ".xls";

            Response.Clear();
            Response.ContentEncoding = encoding;
            Response.AppendHeader("Content-Disposition", "attachment;filename=" + filename);
            Response.ContentType = "application/ms-excel";
            Response.Write(s.ToString());
            Response.End();
        }


        /// <summary>
        /// Response.Clear();Response.Write(s);Response.End();
        /// </summary>
        /// <param name="s">输出字符串</param>
        protected void RCWE(string s)
        {
            Response.Clear();
            Response.Write(s);
            Response.End();
        }

        /// <summary>
        /// 判断当前用户是否有权限
        /// </summary>
        /// <param name="permissionId">权限ID</param>
        /// <returns></returns>
        protected bool CheckGrant(EyouSoft.Model.EnumType.PrivsStructure.Privs3 permission)
        {
            if (_userInfo == null) return false;
            return _userInfo.Privs.Contains((int)permission);
        }

        /// <summary>
        /// 跳转到登录页面，并指定登录成功后要跳转的页面地址
        /// </summary>
        /// <param name="tourl">指定登录成功后要跳转的页面地址</param>
        protected static void RedirectLogin(string tourl)
        {
            if (string.IsNullOrEmpty(tourl)) tourl = string.Empty;

            string loginurl = Utils.GetLoginUrl();
            string s = string.Empty;

            if (!string.IsNullOrEmpty(tourl))
            {
                if (loginurl.IndexOf("?") == -1)
                {
                    s = loginurl + "?returnurl=" + HttpContext.Current.Server.UrlEncode(tourl);
                }
                else
                {
                    s = loginurl + "&returnurl=" + HttpContext.Current.Server.UrlEncode(tourl);
                }
            }

            HttpContext.Current.Response.Redirect(s);
        }



        /// <summary>
        /// register alert script
        /// </summary>
        /// <param name="s">msg</param>
        protected void RegisterAlertScript(string s)
        {
            this.RegisterScript(string.Format("alert('{0}');", s));
        }

        /// <summary>
        /// register alert and redirect script
        /// </summary>
        /// <param name="s"></param>
        /// <param name="url">IsNullOrEmpty(url)=true page reload</param>
        protected void RegisterAlertAndRedirectScript(string s, string url)
        {
            if (!string.IsNullOrEmpty(url))
            {
                this.RegisterScript(string.Format("alert('{0}');window.location.href='{1}';", s, url));
            }
            else
            {
                this.RegisterScript(string.Format("alert('{0}');window.location.href=window.location.href;", s));
            }
        }

        /// <summary>
        /// register alert and reload script
        /// </summary>
        /// <param name="s"></param>
        protected void RegisterAlertAndReloadScript(string s)
        {
            RegisterAlertAndRedirectScript(s, null);
        }

        /// <summary>
        /// register scripts
        /// </summary>
        /// <param name="script"></param>
        protected void RegisterScript(string script)
        {
            this.Page.ClientScript.RegisterClientScriptBlock(this.GetType(), Guid.NewGuid().ToString(), script, true);
        }

        /// <summary>
        /// 转换成货币字符串
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        protected string ToMoneyString(object obj)
        {
            return UtilsCommons.GetMoneyString(obj, "zh-cn");
        }

        /// <summary>
        /// 转换成日期字符串
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        protected string ToDateTimeString(object obj)
        {
            return UtilsCommons.GetDateString(obj, "yyyy-MM-dd");
        }
        #endregion
    }
}

