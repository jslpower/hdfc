using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EyouSoft.Model.CompanyStructure
{
    /// <summary>
    /// 系统登录日志实体
    /// </summary>
    [Serializable]
    public class SysLoginLog
    {
        #region Model
        /// <summary>
        /// 编号
        /// </summary>
        public string ID
        {
            get;
            set;
        }
        /// <summary>
        /// 登录人
        /// </summary>
        public int UserId
        {
            get;
            set;
        }

        /// <summary>
        /// 登录用户账户
        /// </summary>
        public string UserName { get; set; }

        /// <summary>
        /// 登录用户姓名
        /// </summary>
        public string ContactName { get; set; }

        /// <summary>
        /// 公司id
        /// </summary>
        public int CompanyId
        {
            get;
            set;
        }
        /// <summary>
        /// 登录时间
        /// </summary>
        public DateTime LoginTime
        {
            get;
            set;
        }
        /// <summary>
        /// 登录IP
        /// </summary>
        public string LoginIp
        {
            get;
            set;
        }

        /// <summary>
        /// 登录类型
        /// </summary>
        public EnumType.CompanyStructure.UserLoginType LoginType { get; set; }

        /// <summary>
        /// web请求头内容
        /// </summary>
        public string BrowserType { get; set; }

        #endregion Model
    }

    /// <summary>
    /// 系统登录日志查询实体
    /// </summary>
    [Serializable]
    public class QuerySysLoginLog
    {
        /// <summary>
        /// 公司编号
        /// </summary>
        public int CompanyId
        {
            get;
            set;
        }

        /// <summary>
        /// 操作员编号
        /// </summary>
        public int UserId
        {
            get;
            set;
        }

        /// <summary>
        /// 用户姓名
        /// </summary>
        public string ContactName { get; set; }

        /// <summary>
        /// 操作开始时间
        /// </summary>
        public DateTime? StartTime
        {
            get;
            set;
        }
        /// <summary>
        /// 操作结束时间
        /// </summary>
        public DateTime? EndTime
        {
            get;
            set;
        }
    }
}
