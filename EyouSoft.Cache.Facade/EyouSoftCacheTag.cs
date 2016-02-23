using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EyouSoft.Cache.Tag
{
    /// <summary>
    /// 缓存标签
    /// </summary>
    public static class TagName
    {
        /// <summary>
        /// 系统域名 WZYB/SYS/DOMAIN/{0}
        /// </summary>
        public const string SysDomain = "WZYB/SYS/DOMAIN/{0}";
        /// <summary>
        /// 系统域名集合 WZYB/SYS/DOMAINS
        /// </summary>
        public const string SysDomains = "WZYB/SYS/DOMAINS";
        /// <summary>
        /// 登录用户 WZYB/COM/{0}/USER/{1}
        /// </summary>
        public const string ComUser = "WZYB/COM/{0}/USER/{1}";
        /// <summary>
        /// 公司部门 WZYB/COM/{0}/DEPT
        /// </summary>
        public const string ComDept = "WZYB/COM/{0}/DEPT";
        /// <summary>
        /// 公司配置 WZYB/COM/{0}/SETTING
        /// </summary>
        public const string ComSetting = "WZYB/COM/{0}/SETTING";
        /// <summary>
        /// 公司银行账户 WZYB/COM/{0}/YINHANGZHANGHU
        /// </summary>
        public const string ComYinHangZhangHu = "WZYB/COM/{0}/YINHANGZHANGHU";
    }
}
