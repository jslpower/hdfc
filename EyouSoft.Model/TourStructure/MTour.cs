using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EyouSoft.Model.TourStructure
{

    #region 确认件登记短信提醒组团社实体

    /// <summary>
    /// 确认件登记短信提醒组团社实体
    /// </summary>
    public class MSMSTourCustomer : CompanyStructure.CustomerCareforInfo
    {
        /// <summary>
        /// 团队编号
        /// </summary>
        public string TourId { get; set; }

        /// <summary>
        /// 团号
        /// </summary>
        public string TourNo { get; set; }
    }

    /// <summary>
    /// 确认件登记短信提醒组团社 查询实体
    /// </summary>
    public class MSearchSMSTourCustomer
    {
        /// <summary>
        /// 团队编号
        /// </summary>
        public string TourId { get; set; } 
    }

    #endregion

    /// <summary>
    /// 团队
    /// </summary>
    public class MTour
    {
        /// <summary>
        /// 团队编号    
        /// </summary>
        public string TourId { get; set; }

        /// <summary>
        /// 公司编号
        /// </summary>
        public int CompanyId { get; set; }

        /// <summary>
        /// 团号
        /// </summary>
        public string TourCode { get; set; }

        /// <summary>
        /// 团队类型 团/散
        /// </summary>
        public EyouSoft.Model.EnumType.TourStructure.TourType? TourType { get; set; }

        /// <summary>
        /// 线路编号
        /// </summary>
        public string RouteId { get; set; }

        /// <summary>
        /// 线路名称
        /// </summary>
        public string RouteName { get; set; }

        /// <summary>
        /// 出团时间
        /// </summary>
        public DateTime? LDate { get; set; }

        /// <summary>
        /// 回团时间
        /// </summary>
        public DateTime? RDate { get; set; }

        /// <summary>
        /// 销售员编号
        /// </summary>
        public int SaleId { get; set; }

        /// <summary>
        /// 销售员
        /// </summary>
        public string SaleName { get; set; }

        /// <summary>
        /// 是否月结
        /// </summary>
        public bool IsMonth { get; set; }

        /// <summary>
        /// 月结时间
        /// </summary>
        public DateTime? MonthTime { get; set; }

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
        /// 组团社编号
        /// </summary>
        public string BuyCompanyId { get; set; }

        /// <summary>
        /// 组团社名称
        /// </summary>
        public string BuyCompnayName { get; set; }

        /// <summary>
        /// 合计金额
        /// </summary>
        public decimal SumPrice { get; set; }

        /// <summary>
        /// 操作人编号
        /// </summary>
        public int OperatorId { get; set; }

        /// <summary>
        /// 团队状态
        /// </summary>
        public EyouSoft.Model.EnumType.TourStructure.TourStatus? TourStatus { get; set; }

        /// <summary>
        /// 是否出票
        /// </summary>
        public bool IsChuPiao { get; set; }

        /// <summary>
        /// 添加时间
        /// </summary>
        public DateTime? IssueTime { get; set; }

        /// <summary>
        /// 已收已审核金额
        /// </summary>
        public decimal CheckMoney { get; set; }

        /// <summary>
        /// 已退已审核金额
        /// </summary>
        public decimal ReturnMoney { get; set; }

        /// <summary>
        /// 已收（不管审核状态）
        /// </summary>
        public decimal ReceivedMoney { get; set; }

        /// <summary>
        /// 已退（不管审核状态）
        /// </summary>
        public decimal RefundMoney { get; set; }

        /// <summary>
        /// 是否结清
        /// </summary>
        public bool IsClean
        {
            get
            {
                return this.SumPrice - this.CheckMoney + this.ReturnMoney == 0;
            }
        }

        /// <summary>
        /// 质量评估等级
        /// </summary>
        public EyouSoft.Model.EnumType.TourStructure.Score? Score { get; set; }

        /// <summary>
        /// 评估人
        /// </summary>
        public int SOperatorId { get; set; }

        /// <summary>
        /// 返佣人数
        /// </summary>
        public int RebatePeople { get; set; }

        /// <summary>
        /// 返佣价格
        /// </summary>
        public decimal RebatePrice { get; set; }

        /// <summary>
        /// 毛利
        /// </summary>
        public decimal Profit { get; set; }

        /// <summary>
        /// 财务确认人
        /// </summary>
        public int ConfirmOperatorId { get; set; }

        /// <summary>
        /// 财务确认时间
        /// </summary>
        public DateTime? ConfirmTime { get; set; }

        /// <summary>
        /// 是否结束
        /// </summary>
        public bool IsEnd { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        public string Remark { get; set; }

    }

    /// <summary>
    /// 附件
    /// </summary>
    public class MFile
    {
        /// <summary>
        /// 编号
        /// </summary>
        public string Id { get; set; }
        /// <summary>
        /// 文件名称
        /// </summary>
        public string FileName { get; set; }

        /// <summary>
        /// 文件路径
        /// </summary>
        public string FilePath { get; set; }
    }


    /// <summary>
    /// 地接社信息
    /// </summary>
    public class MTourDiJie
    {
        /// <summary>
        /// 编号
        /// </summary>
        public string Id { get; set; }
        /// <summary>
        /// 地接社编号
        /// </summary>
        public string DiJieId { get; set; }

        /// <summary>
        /// 地接社名称
        /// </summary>
        public string DiJieName { get; set; }

        /// <summary>
        /// 计调
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 电话
        /// </summary>
        public string Phone { get; set; }

        /// <summary>
        /// QQ
        /// </summary>
        public string QQ { get; set; }


    }

    /// <summary>
    /// 团队导游
    /// </summary>
    public class MTourGuide
    {
        /// <summary>
        /// 编号
        /// </summary>
        public string Id { get; set; }
        /// <summary>
        /// 导游编号
        /// </summary>
        public string GuideId { get; set; }

        /// <summary>
        /// 导游名称
        /// </summary>
        public string GuideName { get; set; }

        /// <summary>
        /// 导游电话
        /// </summary>
        public string Phone { get; set; }
    }


    /// <summary>
    /// 团队信息
    /// </summary>
    public class MTourInfo : MTour
    {
        /// <summary>
        /// 线路是否回填
        /// </summary>
        public bool IsRouteHuiTian { get; set; }

        /// <summary>
        /// 文件附件
        /// </summary>
        public IList<MFile> FileList { get; set; }

        /// <summary>
        /// 导游
        /// </summary>
        public IList<MTourGuide> GuideList { get; set; }

        /// <summary>
        /// 地接信息
        /// </summary>
        public IList<MTourDiJie> DiJieList { get; set; }
    }

    /// <summary>
    /// 查询实体
    /// </summary>
    public class MSearchTour
    {
        /// <summary>
        /// 团号
        /// </summary>
        public string TourCode { get; set; }

        /// <summary>
        /// 线路名称
        /// </summary>
        public string RouteName { get; set; }

        /// <summary>
        /// 销售员
        /// </summary>
        public string SaleName { get; set; }

        /// <summary>
        /// 出团开始时间
        /// </summary>
        public DateTime? LBeginDate { get; set; }

        /// <summary>
        /// 出团结束时间
        /// </summary>
        public DateTime? LEndDate { get; set; }

        /// <summary>
        /// 下单开始时间
        /// </summary>
        public DateTime? IssueBeginDate { get; set; }

        /// <summary>
        /// 下单结束时间
        /// </summary>
        public DateTime? IssueEndDate { get; set; }

        /// <summary>
        /// 计调员
        /// </summary>
        public string Planer { get; set; }


        /// <summary>
        /// 组团社
        /// </summary>
        public string ZuTuanShe { get; set; }

        /// <summary>
        /// 地接社
        /// </summary>
        public string DiJieShe { get; set; }

        /// <summary>
        /// 是否月结
        /// </summary>
        public bool? IsMonth { get; set; }

        /// <summary>
        /// 是否出票
        /// </summary>
        public bool? IsChuPiao { get; set; }

        /// <summary>
        /// 是否结清
        /// </summary>
        public bool? IsClean { get; set; }


        /// <summary>
        /// 是否结束
        /// </summary>
        public bool? IsEnd { get; set; }


        /// <summary>
        /// 团队类型 团/散
        /// </summary>
        public EyouSoft.Model.EnumType.TourStructure.TourType? TourType { get; set; }

        /// <summary>
        /// 团队状态
        /// </summary>
        public EyouSoft.Model.EnumType.TourStructure.TourStatus? TourStatus { get; set; }

        /// <summary>
        /// 团队状态排序索引:   1/2 团队状态升/降序；
        /// </summary>
        public int OrderByTourState { get; set; }

    }

    /// <summary>
    /// 确认件登记分页列表
    /// </summary>
    public class MPageTour : MTour
    {
        /// <summary>
        /// 组团社今年交易次数
        /// </summary>
        public int BuyCompanyCount { get; set; }

        /// <summary>
        /// 计调员
        /// </summary>
        public string Planer { get; set; }

        /// <summary>
        /// 组团社联系人
        /// </summary>
        public IList<EyouSoft.Model.CRM.MCustomerContact> CustomerContactList { get; set; }

        /// <summary>
        /// 出票人信息
        /// </summary>
        public IList<EyouSoft.Model.PlanStructure.MPlanTicket> PlanTicketList { get; set; }

        /// <summary>
        /// 出票人
        /// </summary>
        public string Ticketer { get { return this.PlanTicketList != null ? this.PlanTicketList.FirstOrDefault().Ticketer : null; } }

        /// <summary>
        /// 出票点名称
        /// </summary>
        public string TicketCompany { get { return this.PlanTicketList != null ? this.PlanTicketList.FirstOrDefault().GysName : null; } }


        /// <summary>
        /// 导游信息
        /// </summary>
        public IList<MTourGuide> GuideList { get; set; }

        /// <summary>
        /// 导游名称
        /// </summary>
        public string GuideName { get { return this.GuideList != null ? this.GuideList.FirstOrDefault().GuideName : null; } }

        /// <summary>
        /// 地接信息
        /// </summary>
        public IList<MTourDiJie> DiJieList { get; set; }

        /// <summary>
        /// 地接社名称
        /// </summary>
        public string DiJieName { get { return this.DiJieList != null ? this.DiJieList.FirstOrDefault().DiJieName : null; } }


        /// <summary>
        /// 收款信息
        /// </summary>
        public IList<EyouSoft.Model.FinStructure.MShouKuan> ShouKuanList { get; set; }
    }


}
