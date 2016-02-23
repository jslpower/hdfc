using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EyouSoft.Model.EnumType.SourceStructure
{
    /// <summary>
    /// 酒店星级
    /// </summary>
    public enum HotelStar
    {
        /// <summary>
        /// 3星以下
        /// </summary>
        _3星以下 = 1,
        /// <summary>
        /// 挂3
        /// </summary>
        挂3,
        /// <summary>
        /// 准3
        /// </summary>
        准3,
        /// <summary>
        /// 挂4
        /// </summary>
        挂4,
        /// <summary>
        /// 准4
        /// </summary>
        准4,
        /// <summary>
        /// 挂5
        /// </summary>
        挂5,
        /// <summary>
        /// 准5
        /// </summary>
        准5
    }

    /// <summary>
    /// 景点星级
    /// </summary>
    public enum SpotStar
    {

        /// <summary>
        /// 5A
        /// </summary>
        五A = 1,

        /// <summary>
        /// 4A
        /// </summary>
        四A,

        /// <summary>
        /// 3A
        /// </summary>
        三A
    }

    /// <summary>
    /// 附件类型
    /// </summary>
    public enum FileMode
    {
        /// <summary>
        /// 文件
        /// </summary>
        文件 = 0,

        /// <summary>
        /// 图片
        /// </summary>
        图片
    }

    /// <summary>
    /// 导游等级
    /// </summary>
    public enum GuideStar
    {
        /// <summary>
        /// 初级
        /// </summary>
        初级 = 1,

        /// <summary>
        /// 中级
        /// </summary>
        中级,

        /// <summary>
        /// 高级
        /// </summary>
        高级,

        /// <summary>
        /// 特级
        /// </summary>
        特级
    }


    /// <summary>
    /// 菜系
    /// </summary>
    public enum Cuisine
    {
        /// <summary>
        /// 闽菜
        /// </summary>
        闽菜 = 0,

        /// <summary>
        /// 苏菜
        /// </summary>
        苏菜,

        /// <summary>
        /// 粤菜
        /// </summary>
        粤菜,

        /// <summary>
        /// 鲁菜
        /// </summary>
        鲁菜,

        /// <summary>
        /// 湘菜
        /// </summary>
        湘菜,

        /// <summary>
        /// 京菜
        /// </summary>
        京菜,

        /// <summary>
        /// 徽菜
        /// </summary>
        徽菜,

        /// <summary>
        /// 鄂菜
        /// </summary>
        鄂菜,

        /// <summary>
        /// 川菜
        /// </summary>
        川菜,

        /// <summary>
        /// 其它
        /// </summary>
        其它
    }


    /// <summary>
    /// 导游反馈类型
    /// </summary>
    public enum FanKuiType
    {
        投诉 = 0,
        反馈
    }
}
