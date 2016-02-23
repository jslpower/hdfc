using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EyouSoft.Model.TongJiStructure
{
    #region 团散统计实体

    /// <summary>
    /// 团散统计实体
    /// </summary>
    public class MTourAndSan
    {
        /// <summary>
        /// 团队编号
        /// </summary>
        public string TourId { get; set; }

        /// <summary>
        /// 组团社名称
        /// </summary>
        public string CustomerName { get; set; }

        /// <summary>
        /// 计调员名称
        /// </summary>
        public string PlanName { get; set; }
        /// <summary>
        /// 销售员名称
        /// </summary>
        public string SaleName { get; set; }
        /// <summary>
        /// 出发时间
        /// </summary>
        public DateTime LeaveDate { get; set; }
        /// <summary>
        /// 团号
        /// </summary>
        public string TourNo { get; set; }
        /// <summary>
        /// 成人数
        /// </summary>
        public int Adults { get; set; }
        /// <summary>
        /// 儿童数
        /// </summary>
        public int Childs { get; set; }
        /// <summary>
        /// 全陪数
        /// </summary>
        public int Accompanys { get; set; }
        /// <summary>
        /// 线路名称
        /// </summary>
        public string RouteName { get; set; }
        /// <summary>
        /// 大交通
        /// </summary>
        public string HeavyTraffic
        {
            get
            {
                if (this.TicketFloat == null || !this.TicketFloat.Any()) return string.Empty;

                return this.TicketFloat.FirstOrDefault().TicketType.ToString();
            }
        }
        /// <summary>
        /// 大交通浮动
        /// </summary>
        public IList<PlanStructure.MPlanTicketFloat> TicketFloat { get; set; }
        /// <summary>
        /// 总收入
        /// </summary>
        public decimal ShouRuSumPrice { get; set; }

        /// <summary>
        /// 已收
        /// </summary>
        public decimal YiShou { get; set; }

        /// <summary>
        /// 未收
        /// </summary>
        public decimal WeiShou
        {
            get
            {
                return this.ShouRuSumPrice - YiShou;
            }
        }

        /// <summary>
        /// 地接支出
        /// </summary>
        public decimal DiJieZhiChu { get; set; }

        /// <summary>
        /// 机票支出
        /// </summary>
        public decimal JiPiaoZhiChu { get; set; }

        /// <summary>
        /// 总支出
        /// </summary>
        public decimal ZhiChuSumPrice
        {
            get
            {
                return this.DiJieZhiChu + this.JiPiaoZhiChu;
            }
        }
        /// <summary>
        /// 佣金
        /// </summary>
        public decimal YongJin { get; set; }
        /// <summary>
        /// 毛利
        /// </summary>
        public decimal MaoLi { get; set; }

        /// <summary>
        /// 收入-支出 = 毛利
        /// </summary>
        public decimal MaoLi2
        {
            get
            {
                return this.ShouRuSumPrice - this.ZhiChuSumPrice;
            }
        }
    }

    /// <summary>
    /// 团散统计 查询实体（Id和名称同时存在的属性，Id有值的情况下名称不作为查询条件，Id为空或者小于等于0，则名称作为模糊查询条件）
    /// </summary>
    public class MSearchTourAndSan
    {
        /// <summary>
        /// 组团社所在省份
        /// </summary>
        public int[] ProvinceId { get; set; }
        /// <summary>
        /// 组团社所在城市
        /// </summary>
        public int[] CityId { get; set; }
        /// <summary>
        /// 出团开始时间
        /// </summary>
        public DateTime? StartLeaveDate { get; set; }
        /// <summary>
        /// 出团结束时间
        /// </summary>
        public DateTime? EndLeaveDate { get; set; }
        /// <summary>
        /// 下单开始时间
        /// </summary>
        public DateTime? StartOrderDate { get; set; }
        /// <summary>
        /// 下单结束时间
        /// </summary>
        public DateTime? EndOrderDate { get; set; }
        /// <summary>
        /// 组团社编号
        /// </summary>
        public string CustomerId { get; set; }
        /// <summary>
        /// 组团社名称
        /// </summary>
        public string CustomerName { get; set; }
        /// <summary>
        /// 计调员编号
        /// </summary>
        public int OperatorId { get; set; }
        /// <summary>
        /// 计调员名称
        /// </summary>
        public string OperatorName { get; set; }
        /// <summary>
        /// 销售员编号
        /// </summary>
        public int SaleId { get; set; }
        /// <summary>
        /// 销售员名称
        /// </summary>
        public string SaleName { get; set; }

        /// <summary>
        /// 团队类型
        /// </summary>
        public EnumType.TourStructure.TourType? TourType { get; set; }
    }

    /// <summary>
    /// 团散统计合计实体
    /// </summary>
    public class MTourAndSanHeJi
    {
        /// <summary>
        /// 成人数
        /// </summary>
        public int Adults { get; set; }
        /// <summary>
        /// 儿童数
        /// </summary>
        public int Childs { get; set; }
        /// <summary>
        /// 全陪数
        /// </summary>
        public int Accompanys { get; set; }
        /// <summary>
        /// 收入
        /// </summary>
        public decimal ShouRu { get; set; }

        /// <summary>
        /// 已收
        /// </summary>
        public decimal YiShou { get; set; }

        /// <summary>
        /// 未收
        /// </summary>
        public decimal WeiShou
        {
            get
            {
                return this.ShouRu - this.YiShou;
            }
        }

        /// <summary>
        /// 地接支出
        /// </summary>
        public decimal DiJieZhiChu { get; set; }

        /// <summary>
        /// 机票支出
        /// </summary>
        public decimal JiPiaoZhiChu { get; set; }

        /// <summary>
        /// 总支出
        /// </summary>
        public decimal ZhiChu
        {
            get
            {
                return this.DiJieZhiChu + this.JiPiaoZhiChu;
            }
        }

        /// <summary>
        /// 佣金
        /// </summary>
        public decimal YongJin { get; set; }
        /// <summary>
        /// 毛利
        /// </summary>
        public decimal MaoLi { get; set; }

        /// <summary>
        /// 收入-支出 = 毛利
        /// </summary>
        public decimal MaoLi2
        {
            get
            {
                return this.ShouRu - this.ZhiChu;
            }
        }
    }

    #endregion

    #region 组团社统计实体

    /// <summary>
    /// 组团社统计实体
    /// </summary>
    public class MCustomerTongJi
    {
        /// <summary>
        /// 客户编号 
        /// </summary>
        public string CustomerId { get; set; }

        /// <summary>
        /// 所在省份名称
        /// </summary>
        public string ProvinceName { get; set; }
        /// <summary>
        /// 所在城市名称
        /// </summary>
        public string CityName { get; set; }
        /// <summary>
        /// 公司名称
        /// </summary>
        public string CustomerName { get; set; }
        /// <summary>
        /// 联系人
        /// </summary>
        public string ContactName { get; set; }
        /// <summary>
        /// 电话
        /// </summary>
        public string ContactTel { get; set; }
        /// <summary>
        /// 手机
        /// </summary>
        public string ContactMobile { get; set; }
        /// <summary>
        /// 交易人数（成人 + 儿童，不累计全陪）
        /// </summary>
        public int JiaoYiRenShu { get; set; }
        /// <summary>
        /// 交易次数
        /// </summary>
        public int JiaoYiCiShu { get; set; }
        /// <summary>
        /// 交易金额
        /// </summary>
        public decimal JiaoYiJinE { get; set; }
        /// <summary>
        /// 拜访次数
        /// </summary>
        public int BaiFangCiShu { get; set; }
        /// <summary>
        /// 拜访总支出
        /// </summary>
        public decimal BaiFangJinE { get; set; }
    }

    /// <summary>
    /// 组团社统计查询实体
    /// </summary>
    public class MSearchCustomerTongJi
    {
        /// <summary>
        /// 组团社所在省份
        /// </summary>
        public int[] ProvinceId { get; set; }
        /// <summary>
        /// 组团社所在城市
        /// </summary>
        public int[] CityId { get; set; }
        /// <summary>
        /// 开始时间（只对交易信息和拜访信息做筛选，不对组团社信息做筛选）
        /// </summary>
        public DateTime? StartTime { get; set; }
        /// <summary>
        /// 结束始时间（只对交易信息和拜访信息做筛选，不对组团社信息做筛选）
        /// </summary>
        public DateTime? EndTime { get; set; }
        /// <summary>
        /// 排序索引；0/1：组团社编号升/降序；2/3交易人数升/降序；4/5交易次数升/降序；6/7交易金额升/降序；8/9拜访次数升/降序；10/11拜访支出升/降序；
        /// </summary>
        public int OrderByIndex { get; set; }
    }

    /// <summary>
    /// 组团社统计合计实体
    /// </summary>
    public class MCustomerTongJiHeJi
    {
        /// <summary>
        /// 交易人数（成人 + 儿童，不累计全陪）
        /// </summary>
        public int JiaoYiRenShu { get; set; }
        /// <summary>
        /// 交易次数
        /// </summary>
        public int JiaoYiCiShu { get; set; }
        /// <summary>
        /// 交易金额
        /// </summary>
        public decimal JiaoYiJinE { get; set; }
        /// <summary>
        /// 拜访次数
        /// </summary>
        public int BaiFangCiShu { get; set; }
        /// <summary>
        /// 拜访支出
        /// </summary>
        public decimal BaiFangJinE { get; set; }
    }

    #endregion


    #region 销售地区统计实体

    /// <summary>
    /// 销售地区统计实体
    /// </summary>
    public class MSaleAreaTongJi
    {
        /// <summary>
        /// 销售区域编号
        /// </summary>
        public int SaleAreaId { get; set; }

        /// <summary>
        /// 销售地区名称
        /// </summary>
        public string SaleAreaName { get; set; }
        /// <summary>
        /// 总收入
        /// </summary>
        public decimal ZongShouRu { get; set; }

        /// <summary>
        /// 地接支出
        /// </summary>
        public decimal DiJieZhiChu { get; set; }

        /// <summary>
        /// 机票支出
        /// </summary>
        public decimal JiPiaoZhiChu { get; set; }

        /// <summary>
        /// 总支出
        /// </summary>
        public decimal ZongZhiChu
        {
            get
            {
                return this.DiJieZhiChu + this.JiPiaoZhiChu;
            }
        }

        /// <summary>
        /// 佣金
        /// </summary>
        public decimal YongJin { get; set; }
        /// <summary>
        /// 毛利
        /// </summary>
        public decimal MaoLi { get; set; }

        public decimal MaoLi2
        {
            get
            {
                return this.ZongShouRu - this.ZongZhiChu;
            }
        }
    }

    /// <summary>
    /// 销售地区统计查询实体
    /// </summary>
    public class MSearchSaleAreaTongJi
    {
        /// <summary>
        /// 下单开始时间（只对金额信息做筛选，不对销售地区信息做筛选）
        /// </summary>
        public DateTime? StartTime { get; set; }
        /// <summary>
        /// 下单结束始时间（只对金额信息做筛选，不对销售地区信息做筛选）
        /// </summary>
        public DateTime? EndTime { get; set; }
    }

    /// <summary>
    /// 销售地区统计合计实体
    /// </summary>
    public class MSaleAreaTongJiHeJi
    {
        /// <summary>
        /// 收入
        /// </summary>
        public decimal ShouRu { get; set; }

        /// <summary>
        /// 地接支出
        /// </summary>
        public decimal DiJieZhiChu { get; set; }

        /// <summary>
        /// 机票支出
        /// </summary>
        public decimal JiPiaoZhiChu { get; set; }

        /// <summary>
        /// 总支出
        /// </summary>
        public decimal ZhiChu
        {
            get
            {
                return this.DiJieZhiChu + this.JiPiaoZhiChu;
            }
        }

        /// <summary>
        /// 佣金
        /// </summary>
        public decimal YongJin { get; set; }
        /// <summary>
        /// 毛利
        /// </summary>
        public decimal MaoLi { get; set; }

        /// <summary>
        /// 收入 - 支出 = 毛利
        /// </summary>
        public decimal MaoLi2
        {
            get
            {
                return this.ShouRu - this.ZhiChu;
            }
        }
    }

    #endregion

}
