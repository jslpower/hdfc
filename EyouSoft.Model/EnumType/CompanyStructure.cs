//系统设置-相关枚举
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EyouSoft.Model.EnumType.CompanyStructure
{
    #region 公司银行账号性质

    /// <summary>
    /// 公司银行账号性质
    /// </summary>
    public enum AccountType
    {
        /// <summary>
        /// 对公
        /// </summary>
        对公 = 0,
        /// <summary>
        /// 对私
        /// </summary>
        对私 = 1
    }

    #endregion

    #region 公司银行帐号状态

    /// <summary>
    /// 公司银行帐号状态
    /// </summary>
    public enum AccountState
    {
        /// <summary>
        /// 未审批
        /// </summary>
        未审批 = 0,
        /// <summary>
        /// 可用
        /// </summary>
        可用 = 1,
        /// <summary>
        /// 不可用
        /// </summary>
        不可用 = 2
    }

    #endregion

    #region 性别
    /// <summary>
    /// 性别
    /// </summary>
    public enum Sex
    {
        /// <summary>
        /// 未知
        /// </summary>
        未知 = 0,
        /// <summary>
        /// 女
        /// </summary>
        女 = 1,
        /// <summary>
        /// 男
        /// </summary>
        男 = 2
    }

    #endregion

    #region 公司用户类型
    /// <summary>
    /// 公司用户类型
    /// </summary>
    public enum UserType
    {
        /// <summary>
        /// 专线用户
        /// </summary>
        专线用户 = 0,
        /// <summary>
        /// 地接用户
        /// </summary>
        地接用户 = 1,
        /// <summary>
        /// 组团用户
        /// </summary>
        票务用户 = 2
    }
    #endregion

    #region 用户状态
    /// <summary>
    /// 用户状态
    /// </summary>
    public enum UserStatus
    {
        /// <summary>
        /// 未启用=0
        /// </summary>
        未启用 = 0,
        /// <summary>
        /// 正常=1
        /// </summary>
        正常 = 1,
        /// <summary>
        /// 黑名单=2
        /// </summary>
        黑名单 = 2,
        /// <summary>
        /// 已停用=3
        /// </summary>
        已停用 = 3
    }
    #endregion

    #region 用户在线状态
    /// <summary>
    /// 用户在线状态
    /// </summary>
    public enum UserOnlineStatus
    {
        /// <summary>
        /// 离线
        /// </summary>
        Offline = 0,
        /// <summary>
        /// 在线
        /// </summary>
        Online
    }
    #endregion

    #region 用户登录类型
    /// <summary>
    /// 用户登录类型
    /// </summary>
    public enum UserLoginType
    {
        /// <summary>
        /// 用户登录
        /// </summary>
        用户登录 = 0,
        /// <summary>
        /// 客服登录
        /// </summary>
        客服登录,
        /// <summary>
        /// 自动登录
        /// </summary>
        自动登录
    }
    #endregion

    #region 用户登录限制类型
    /// <summary>
    /// 用户登录限制类型
    /// </summary>
    public enum UserLoginLimitType
    {
        /// <summary>
        /// 所有登录有效
        /// </summary>
        None,
        /// <summary>
        /// 最早登录有效
        /// </summary>
        Earliest,
        /// <summary>
        /// 最近登录有效
        /// </summary>
        Latest
    }
    #endregion

    #region 供应商类型

    /// <summary>
    /// 供应商类型
    /// </summary>
    public enum SupplierType
    {
        /// <summary>
        /// 地接
        /// </summary>
        地接 = 1,
        /// <summary>
        /// 票务
        /// </summary>
        票务 = 2,
        /// <summary>
        /// 酒店
        /// </summary>
        酒店 = 3,
        /// <summary>
        /// 景点
        /// </summary>
        景点 = 4,
        /// <summary>
        /// 景点
        /// </summary>
        其他 = 5,
        /// <summary>
        /// 导游
        /// </summary>
        导游 = 6,
        /// <summary>
        /// 餐饮
        /// </summary>
        餐饮=7
    }

    #endregion

    #region 发布对象类型

    /// <summary>
    /// 发布对象类型
    /// </summary>
    public enum AcceptType
    {
        /// <summary>
        /// 所有
        /// </summary>
        所有 = 0,
        /// <summary>
        /// 指定部门
        /// </summary>
        指定部门 = 1,
        /// <summary>
        /// 指定组团
        /// </summary>
        指定组团 = 2,
        /// <summary>
        /// 指定人
        /// </summary>
        指定人 = 3
    }

    #endregion

    #region 客户关怀固定特殊节日发送

    /// <summary>
    /// 客户关怀固定特殊节日发送
    /// </summary>
    public enum CustomerCareForSendSpecialTime
    {
        无 = 0,
        生日 = 1,
        元旦 = 2,
        春节 = 3,
        元宵 = 4,
        五一 = 5,
        国庆 = 6,
        中秋 = 7,
        圣诞 = 8

    }
    #endregion

}

