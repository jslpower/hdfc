using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EyouSoft.Model.EnumType.CustomerStructure
{
    #region 客户类型

    /// <summary>
    /// 客户类型
    /// </summary>
    public enum CustomerType
    {
        /// <summary>
        /// 组团社
        /// </summary>
        组团社 = 0,
        /// <summary>
        /// 单位直客
        /// </summary>
        单位直客 = 1
    }

    #endregion

    #region 生日中心-生日礼物明细-收到礼物对象类型

    /// <summary>
    /// 生日中心-生日礼物明细-收到礼物对象类型
    /// </summary>
    public enum BirthdayGiftType
    {
        /// <summary>
        /// 员工
        /// </summary>
        员工 = 0,
        /// <summary>
        /// 导游
        /// </summary>
        导游 = 1,
        /// <summary>
        /// 组团联系人
        /// </summary>
        组团联系人 = 2,
        /// <summary>
        /// 游客
        /// </summary>
        游客 = 3,
        /// <summary>
        /// 地接联系人
        /// </summary>
        地接联系人 = 4,
        /// <summary>
        /// 景点联系人
        /// </summary>
        景点联系人 = 5
    }

    #endregion

    #region 客户评级
    /// <summary>
    /// 客户评级
    /// </summary>
    public enum CustomerRating
    {
        /// <summary>
        /// 请选择
        /// </summary>
        请选择 = 3,
        /// <summary>
        /// A级
        /// </summary>
        A级 = 0,

        /// <summary>
        /// B级
        /// </summary>
        B级 = 1,

        /// <summary>
        /// C级
        /// </summary>
        C级 = 2
    }

    #endregion

}
