using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using EyouSoft.Common;

namespace Web
{
    public partial class Login : System.Web.UI.Page
    {
        #region attributes
        /// <summary>
        /// 公司名称
        /// </summary>
        protected string CompanyName = string.Empty;
        /// <summary>
        /// logo文件路径
        /// </summary>
        protected string LogoFilePath = "/images/pngclear.gif";

        /// <summary>
        /// 联系电话
        /// </summary>
        protected string ContactTel = "010-58032362";

        #endregion

        /// <summary>
        /// 用户登录
        /// </summary>
        /// 戴银柱 2011-9-7
        protected void Page_Load(object sender, EventArgs e)
        {
            EyouSoft.Model.SysStructure.SystemDomain sysDomain = EyouSoft.Security.Membership.UserProvider.GetDomain();

            if (sysDomain == null || sysDomain.CompanyId < 1 || sysDomain.SysId < 1)
            {
                Response.Clear();
                Response.Write("请求异常：错误的域名配置。");
                Response.End();
            }

            CompanyName = sysDomain.CompanyName;

            var setting=EyouSoft.Security.Membership.UserProvider.GetComSetting(sysDomain.CompanyId);
            var info = new EyouSoft.BLL.CompanyStructure.CompanyInfo().GetModel(sysDomain.CompanyId);

            if (setting != null && !string.IsNullOrEmpty(setting.CompanyLogo)) LogoFilePath = setting.CompanyLogo;

            if (info != null) ContactTel = info.ContactTel;

        }
    }
}
