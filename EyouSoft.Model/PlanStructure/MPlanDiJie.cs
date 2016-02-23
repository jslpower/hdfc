using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EyouSoft.Model.PlanStructure
{
    public class MPlanDiJie
    {
        /// <summary>
        /// 编号
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
        /// 供应商编号
        /// </summary>
        public string GysId { get; set; }

        /// <summary>
        /// 供应商名称
        /// </summary>
        public string GysName { get; set; }

        /// <summary>
        /// 酒店(房)
        /// </summary>
        public decimal Hotel { get; set; }

        /// <summary>
        /// 用餐
        /// </summary>
        public decimal Dining { get; set; }

        /// <summary>
        /// 用车
        /// </summary>
        public decimal Car { get; set; }

        /// <summary>
        /// 门票
        /// </summary>
        public decimal Ticket { get; set; }

        /// <summary>
        /// 导游
        /// </summary>
        public decimal Guide { get; set; }

        /// <summary>
        /// 大交通
        /// </summary>
        public decimal Traffic { get; set; }

        /// <summary> 
        /// 购物人头
        /// </summary>
        public decimal Head { get; set; }

        /// <summary>
        /// 加点费用
        /// </summary>
        public decimal AddPrice { get; set; }

        /// <summary>
        /// 导游现收
        /// </summary>
        public decimal GuideIncome { get; set; }

        /// <summary>
        /// 导游现付
        /// </summary>
        public decimal GuidePay { get; set; }

        /// <summary>
        /// 其它
        /// </summary>
        public decimal Other { get; set; }

        /// <summary>
        /// 支付方式
        /// </summary>
        public EyouSoft.Model.EnumType.FinStructure.ShouFuKuanFangShi? PayType { get; set; }

        /// <summary>
        /// 合计金额
        /// </summary>
        public decimal SumPrice { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        public string Remark { get; set; }

        /// <summary>
        /// 操作人编号
        /// </summary>
        public int OperatorId { get; set; }

        /// <summary>
        /// 操作时间
        /// </summary>
        public DateTime? IssueTime { get; set; }

        /// <summary>
        /// 状态
        /// </summary>
        public EyouSoft.Model.EnumType.PlanStructure.DiJieStatus? DiJieStatus { get; set; }

        /// <summary>
        /// 团队类型
        /// </summary>
        public EyouSoft.Model.EnumType.TourStructure.TourType? TourType { get; set; }


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
    /// 地接详细信息
    /// </summary>
    public class MPlanDiJieInfo : MPlanDiJie
    {
        /// <summary>
        /// 文件附件
        /// </summary>
        public IList<EyouSoft.Model.TourStructure.MFile> FileList { get; set; }
    }

    /// <summary>
    /// 地接查询实体
    /// </summary>
    public class MSeachDiJie
    {

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
        /// 地接社编号
        /// </summary>
        public string DiJieId { get; set; }

        /// <summary>
        /// 地接社名称
        /// </summary>
        public string DiJieName { get; set; }

        /// <summary>
        /// 是否月结
        /// </summary>
        public bool? IsMonth { get; set; }

        /// <summary>
        /// 团队类型
        /// </summary>
        public EyouSoft.Model.EnumType.TourStructure.TourType? TourType { get; set; }
    }

    /// <summary>
    /// 地接社分页列表
    /// </summary>
    public class MPageDiJie : MPlanDiJie
    {
        /// <summary>
        /// 线路名称
        /// </summary>
        public string RouteName { get; set; }

        /// <summary>
        /// 出团日期
        /// </summary>
        public DateTime? LDate { get; set; }

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
        /// 团队状态
        /// </summary>
        public EyouSoft.Model.EnumType.TourStructure.TourStatus TourStatus { get; set; }


        /// <summary>
        /// 大交通出票信息
        /// </summary>
        public IList<EyouSoft.Model.PlanStructure.MPlanTicket> PlanTicketList { get; set; }

        /// <summary>
        /// 地接社
        /// </summary>
        public IList<EyouSoft.Model.SourceStructure.MSupplierContact> ContactList { get; set; }

    }
}
