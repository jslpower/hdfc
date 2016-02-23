using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EyouSoft.Model.EnumType.TourStructure
{
    /// <summary>
    /// 团队类型
    /// </summary>
    public enum TourType
    {
        /// <summary>
        /// 团队
        /// </summary>
        团 = 0,
        /// <summary>
        /// 散拼
        /// </summary>
        散,
    }

    /// <summary>
    /// 团队状态
    /// </summary>
    public enum TourStatus
    {
        /// <summary>
        /// 未处理
        /// </summary>
        未处理 = 0,

        /// <summary>
        /// 未出发
        /// </summary>
        未出发,

        /// <summary>
        /// 行程中
        /// </summary>
        行程中,

        /// <summary>
        /// 已回团
        /// </summary>
        已回团,
        /// <summary>
        /// 已取消
        /// </summary>
        已取消 = 4

    }


    /// <summary>
    /// 游客类型
    /// </summary>
    public enum TravellerType
    {
        /// <summary>
        /// 儿童
        /// </summary>
        儿童 = 0,
        /// <summary>
        /// 成人
        /// </summary>
        成人 = 1,
        /// <summary>
        /// 军残
        /// </summary>
        军残 = 2
    }



    #region 游客证件类型枚举
    /// <summary>
    /// 游客证件类型枚举
    /// </summary>
    public enum CardType
    {
        /// <summary>
        /// 未知
        /// </summary>
        未知 = 0,
        /// <summary>
        /// 身份证
        /// </summary>
        身份证 = 1,
        /// <summary>
        /// 军官证
        /// </summary>
        军官证 = 2,
        /// <summary>
        /// 台胞证
        /// </summary>
        台胞证 = 3,
        /// <summary>
        /// 港澳通行证
        /// </summary>
        港澳通行证 = 4,
        /// <summary>
        /// 户口本
        /// </summary>
        户口本 = 5,
    }
    #endregion


    /// <summary>
    /// 回访星级
    /// </summary>
    public enum Score
    {
        /// <summary>
        /// 投诉
        /// </summary>
        投诉 = 1,
        /// <summary>
        /// 严重投诉
        /// </summary>
        严重投诉 = 2,
        /// <summary>
        /// 正常
        /// </summary>
        正常 = 3,
        /// <summary>
        /// 很好
        /// </summary>
        很好 = 4,
        /// <summary>
        /// 非常好
        /// </summary>
        非常好 = 5,
    }

    /// <summary>
    /// 团队报价 团型
    /// </summary>
    public enum TourDataType
    {
        /// <summary>
        /// 购物
        /// </summary>
        购物 = 0,

        /// <summary>
        /// 纯玩
        /// </summary>
        纯玩,
    }










}
