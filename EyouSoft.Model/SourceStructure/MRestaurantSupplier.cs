using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EyouSoft.Model.SourceStructure
{
    /// <summary>
    /// 供应商餐馆
    /// </summary>
    public class MRestaurantSupplier : MSupplier
    {

        /// <summary>
        /// 简介
        /// </summary>
        public string Introduce { get; set; }

        /// <summary>
        /// 菜系
        /// </summary>
        public EyouSoft.Model.EnumType.SourceStructure.Cuisine Cuisine { get; set; }


    }


    /// <summary>
    /// 分页显示供应商信息
    /// </summary>
    public class MPageRestaurant : MPageSupplier
    {
        /// <summary>
        /// 菜系
        /// </summary>
        public EyouSoft.Model.EnumType.SourceStructure.Cuisine Cuisine { get; set; }
    }

    /// <summary>
    /// 查询实体
    /// </summary>
    public class MSearchRestaurant : MSupplierSearch
    {
        /// <summary>
        /// 菜系
        /// </summary>
        public EyouSoft.Model.EnumType.SourceStructure.Cuisine? Cuisine { get; set; }
    }


}
