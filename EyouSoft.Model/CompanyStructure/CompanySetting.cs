using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using System.Collections;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

namespace EyouSoft.Model.CompanyStructure
{
    #region 公司Hash配置实体[键：值形式]
    /// <summary>
    /// 公司Hash配置实体[键：值形式]
    /// </summary>
    /// 鲁功源 2011-01-18
    public class CompanySetting
    {
        #region 构造函数
        /// <summary>
        /// 构造函数
        /// </summary>
        public CompanySetting() { }
        #endregion

        #region 属性
        /// <summary>
        /// 公司编号
        /// </summary>
        public int CompanyId
        {
            get;
            set;
        }
        /// <summary>
        /// 字段名称[Key]
        /// </summary>
        public string FieldName
        {
            get;
            set;
        }
        /// <summary>
        /// 字段数值[Value]
        /// </summary>
        public string FieldValue
        {
            get;
            set;
        }
        #endregion

    }
    #endregion

    #region 公司配置实体
    /// <summary>
    /// 公司配置实体
    /// </summary>
    [Serializable]
    public class CompanyFieldSetting
    {
        /// <summary>
        /// 公司编号
        /// </summary>
        public int CompanyId { get; set; }
        
        /// <summary>
        /// 公司 LOGO
        /// </summary>
        public string CompanyLogo { get; set; }

        /// <summary>
        /// 登录限制类型
        /// </summary>
        public EnumType.CompanyStructure.UserLoginLimitType UserLoginLimitType { get; set; }

        /// <summary>
        /// 最大子账号数量
        /// </summary>
        public int MaxSonUserNum { get; set; }

        /// <summary>
        /// 生日提醒提前天数
        /// </summary>
        public int BirthdayReminderDays { get; set; }

    }
    #endregion

}
