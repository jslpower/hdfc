using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EyouSoft.Model.SourceStructure
{
    /// <summary>
    /// 供应商景点
    /// </summary>
    public class MSpotSupplier : MSupplier
    {

        /// <summary>
        /// 星级
        /// </summary>
        public EyouSoft.Model.EnumType.SourceStructure.SpotStar? SpotStar { get; set; }

        /// <summary>
        /// 导游词
        /// </summary>
        public string TourGuide { get; set; }

        /// <summary>
        /// 门市价
        /// </summary>
        public decimal StorePrice { get; set; }

        /// <summary>
        /// 旺季价
        /// </summary>
        public decimal WJPrice { get; set; }

        /// <summary>
        /// 淡季价
        /// </summary>
        public decimal DJPrice { get; set; }

        /// <summary>
        /// 折扣价
        /// </summary>
        public decimal ZKPrice { get; set; }

    }


    /// <summary>
    /// 分页显示供应商信息
    /// </summary>
    public class MPageSpot : MPageSupplier
    {
        /// <summary>
        /// 星级
        /// </summary>
        public EyouSoft.Model.EnumType.SourceStructure.SpotStar SpotStar { get; set; }

        /// <summary>
        /// 门市价
        /// </summary>
        public decimal StorePrice { get; set; }

        /// <summary>
        /// 旺季价
        /// </summary>
        public decimal WJPrice { get; set; }

        /// <summary>
        /// 淡季价
        /// </summary>
        public decimal DJPrice { get; set; }

        /// <summary>
        /// 折扣价
        /// </summary>
        public decimal ZKPrice { get; set; }

        /// <summary>
        /// 是否是最新的
        /// </summary>
        public bool IsNew { get; set; }
    }

    /// <summary>
    /// 查询实体
    /// </summary>
    public class MSearchSpot : MSupplierSearch
    {
        /// <summary>
        /// 星级
        /// </summary>
        public EyouSoft.Model.EnumType.SourceStructure.SpotStar? SpotStar { get; set; }
    }
}
