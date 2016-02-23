using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EyouSoft.Model.SourceStructure
{
    /// <summary>
    /// 供应商酒店
    /// </summary>
    public class MHotelSupplier : MSupplier
    {

        /// <summary>
        /// 星级
        /// </summary>
        public EyouSoft.Model.EnumType.SourceStructure.HotelStar? HotelStar { get; set; }

        /// <summary>
        /// 介绍
        /// </summary>
        public string Introduce { get; set; }

        /// <summary>
        /// 酒店房型价格
        /// </summary>
        public IList<MHotelRomePrice> RomePriceList { get; set; }


    }




    /// <summary>
    /// 房型信息
    /// </summary>
    public class MHotelRomePrice
    {
        /// <summary>
        /// 房型名称
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 前台价
        /// </summary>
        public decimal SellingPrice { get; set; }

        /// <summary>
        /// 结算价
        /// </summary>
        public decimal AccountingPrice { get; set; }

        /// <summary>
        /// 是否含早
        /// </summary>
        public bool IsBreakfast { get; set; }
    }


    /// <summary>
    /// 分页显示供应商信息
    /// </summary>
    public class MPageHotel : MPageSupplier
    {
        /// <summary>
        /// 星级
        /// </summary>
        public EyouSoft.Model.EnumType.SourceStructure.HotelStar HotelStar { get; set; }
    }

    /// <summary>
    /// 查询实体
    /// </summary>
    public class MSearchHotel : MSupplierSearch
    {
        /// <summary>
        /// 星级
        /// </summary>
        public EyouSoft.Model.EnumType.SourceStructure.HotelStar? HotelStar { get; set; }
    }








}
