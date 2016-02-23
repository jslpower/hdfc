using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EyouSoft.Model.EnumType.SysStructure
{
    #region 域名类型
    /// <summary>
    /// 域名类型
    /// </summary>
    public enum DomainType
    {
        /// <summary>
        /// 专线入口
        /// </summary>
        专线入口=0,
        /// <summary>
        /// 同行入口
        /// </summary>
        同行入口
    }
    #endregion

    #region 权限类别
    /// <summary>
    /// 权限类别
    /// </summary>
    public enum PrivsType
    {
        /// <summary>
        /// 其它
        /// </summary>
        其它 = 0,
        /// <summary>
        /// 栏目
        /// </summary>
        栏目
    }
    #endregion
}
