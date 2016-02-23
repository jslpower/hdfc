using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EyouSoft.Model.PlanStructure
{
    public class MPlanTicket
    {
        /// <summary>
        /// 安排编号
        /// </summary>
        public string PlanId { get; set; }

        /// <summary>
        /// 公司编号
        /// </summary>
        public int CompanyId { get; set; }

        /// <summary>
        /// 团队编号
        /// </summary>
        public string TourId { get; set; }

        /// <summary>
        /// 团号
        /// </summary>
        public string TourCode { get; set; }

        /// <summary>
        /// 出票类型
        /// </summary>
        public EyouSoft.Model.EnumType.PlanStructure.TicketType? TicketType { get; set; }

        /// <summary>
        /// 出票或退票
        /// </summary>
        public EyouSoft.Model.EnumType.PlanStructure.TicketMode? TicketMode { get; set; }

        /// <summary>
        /// 团队状态
        /// </summary>
        public EyouSoft.Model.EnumType.TourStructure.TourStatus? TourStatus { get; set; }

        /// <summary>
        /// 供应商
        /// </summary>
        public string GysId { get; set; }

        /// <summary>
        /// 供应商名称
        /// </summary>
        public string GysName { get; set; }

        /// <summary>
        /// 出票人(编号)
        /// </summary>
        public int TicketerId { get; set; }

        /// <summary>
        /// 出票人
        /// </summary>
        public string Ticketer { get; set; } 

        /// <summary>
        /// 车次/航班
        /// </summary>
        public string TrafficNumber { get; set; }

        /// <summary>
        /// 开车/起飞时间
        /// </summary>
        public DateTime? TrafficTime { get; set; }

        /// <summary>
        /// 区间
        /// </summary>
        public string Interval { get; set; }

        /// <summary>
        /// 成人数
        /// </summary>
        public int Adults { get; set; }

        /// <summary>
        /// 儿童数
        /// </summary>
        public int Childs { get; set; }

        /// <summary>
        /// 成人价
        /// </summary>
        public decimal AdultPrice { get; set; }

        /// <summary>
        /// 儿童价
        /// </summary>
        public decimal ChildPrice { get; set; }

        /// <summary>
        /// 手续费/机燃
        /// </summary>
        public decimal OtherPrice { get; set; }

        /// <summary>
        /// 佣金
        /// </summary>
        public decimal Brokerage { get; set; }

        /// <summary>
        /// 舱位或席别
        /// </summary>
        public string TrafficSeat { get; set; }


        /// <summary>
        /// 舱位或席别
        /// </summary>
        public EyouSoft.Model.EnumType.PlanStructure.TrafficSeat? _TrafficSeat { get; set; }

        /// <summary>
        /// 收款方式
        /// </summary>
        public EyouSoft.Model.EnumType.FinStructure.ShouFuKuanFangShi? PayType { get; set; }


        /// <summary>
        /// 合计金额
        /// </summary>
        public decimal SumPrice { get; set; }

        /// <summary>
        /// 操作人
        /// </summary>
        public int OperatorId { get; set; }

        /// <summary>
        /// 添加时间
        /// </summary>
        public DateTime? IssueTime { get; set; }

        /// <summary>
        /// 票务状态
        /// </summary>
        public EyouSoft.Model.EnumType.PlanStructure.TicketStatus? TicketStatus { get; set; }

        /// <summary>
        /// 是否月结
        /// </summary>
        public bool IsMonth { get; set; }

        /// <summary>
        /// 月结时间
        /// </summary>
        public DateTime? MonthTime { get; set; }

    }


    /// <summary>
    /// 游客表
    /// </summary>
    public class MTraveller
    {
        /// <summary>
        /// 游客编号
        /// </summary>
        public string TravellerId { get; set; }


        /// <summary>
        /// 安排编号
        /// </summary>
        public string PlanId { get; set; }

        /// <summary>
        /// 控位编号(计划编号)
        /// </summary>
        public string TourId { get; set; }

        /// <summary>
        /// 游客姓名
        /// </summary>
        public string TravellerName { get; set; }

        /// <summary>
        /// 游客类型
        /// </summary>
        public EyouSoft.Model.EnumType.TourStructure.TravellerType TravellerType { get; set; }


        /// <summary>
        /// 证件类型
        /// </summary>
        public EyouSoft.Model.EnumType.TourStructure.CardType CardType { get; set; }


        /// <summary>
        /// 证件号码
        /// </summary>
        public string CardNumber { get; set; }

        /// <summary>
        /// 生日
        /// </summary>
        public DateTime? Brithday { get; set; }


        /// <summary>
        /// 性别
        /// </summary>
        public EyouSoft.Model.EnumType.CompanyStructure.Sex Sex { get; set; }


        /// <summary>
        /// 联系方式
        /// </summary>
        public string Contact { get; set; }

    }

    /// <summary>
    /// 票务安排
    /// </summary>
    public class MPlanTicketInfo : MPlanTicket
    {
        /// <summary>
        /// 游客
        /// </summary>
        public IList<MTraveller> TravellerList { get; set; }

        /// <summary>
        /// 附件
        /// </summary>
        public IList<EyouSoft.Model.TourStructure.MFile> FileList { get; set; }

    }

    /// <summary>
    /// 票务的查询实体
    /// </summary>
    public class MSearchTicket
    {
        /// <summary>
        /// 供应商编号
        /// </summary>
        public string GysId { get; set; }

        /// <summary>
        /// 团号
        /// </summary>
        public string TourCode { get; set; }

        /// <summary>
        /// 出团开始时间
        /// </summary>
        public DateTime? LBeginDate { get; set; }

        /// <summary>
        /// 出团结束时间
        /// </summary>
        public DateTime? LEndDate { get; set; }


        /// <summary>
        /// 出票类型
        /// </summary>
        public EyouSoft.Model.EnumType.PlanStructure.TicketType? TicketType { get; set; }

        /// <summary>
        /// 票务状态
        /// </summary>
        public EyouSoft.Model.EnumType.PlanStructure.TicketStatus? TicketStatus { get; set; }

        /// <summary>
        /// 是否月结
        /// </summary>
        public bool? IsMonth { get; set; }

        /// <summary>
        /// 出票or退票
        /// </summary>
        public EyouSoft.Model.EnumType.PlanStructure.TicketMode? TicketMode { get; set; }


        /// <summary>
        /// 团队类型 团/散
        /// </summary>
        public EyouSoft.Model.EnumType.TourStructure.TourType? TourType { get; set; }

        /// <summary>
        /// 出票点
        /// </summary>
        public string GysName { get; set; }

    }

    #region MPlanTicketFloat

    /// <summary>
    /// 机票安排浮动显示实体
    /// </summary>
    public class MPlanTicketFloat
    {
        /// <summary>
        /// 出票类型
        /// </summary>
        public EnumType.PlanStructure.TicketType TicketType { get; set; }

        /// <summary>
        /// 车次/航班
        /// </summary>
        public string TrafficNumber { get; set; }

        /// <summary>
        /// 开车/起飞时间
        /// </summary>
        public DateTime? TrafficTime { get; set; }

        /// <summary>
        /// 区间
        /// </summary>
        public string Interval { get; set; }
    }

    #endregion

}
