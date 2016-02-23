using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EyouSoft.Model.FinStructure
{
    #region 收付款基类

    /// <summary>
    /// 收付款登记基类
    /// </summary>
    public class MKuanBase
    {
        /// <summary>
        /// 收付款编号
        /// </summary>
        public string DengJiId { get; set; }
        /// <summary>
        /// 公司编号
        /// </summary>
        public int CompanyId { get; set; }
        /// <summary>
        /// 收款日期
        /// </summary>
        public DateTime ShouKuanRiQi { get; set; }
        /// <summary>
        /// 收款人编号 
        /// </summary>
        public int ShouKuanRenId { get; set; }
        /// <summary>
        /// 收款人姓名
        /// </summary>
        public string ShouKuanRenName { get; set; }
        /// <summary>
        /// 收款金额
        /// </summary>
        public decimal JinE { get; set; }

        /// <summary>
        /// 收付款项目名称
        /// </summary>
        public string ItemName { get; set; }

        /// <summary>
        /// 收款方式
        /// </summary>
        public EnumType.FinStructure.ShouFuKuanFangShi FangShi { get; set; }
        /// <summary>
        /// 收款账户编号
        /// </summary>
        public string ZhangHuId { get; set; }
        /// <summary>
        /// 收款账户
        /// </summary>
        public string ZhangHuCode { get; set; }
        /// <summary>
        /// 收款备注
        /// </summary>
        public string ShouKuanBeiZhu { get; set; }
        /// <summary>
        /// 是否开票
        /// </summary>
        public bool IsKaiPiao { get; set; }
        /// <summary>
        /// 手续费
        /// </summary>
        public decimal OtherPrice { get; set; }
        /// <summary>
        /// 收款状态
        /// </summary>
        public EnumType.FinStructure.KuanXiangStatus Status { get; set; }
        /// <summary>
        /// 操作人编号
        /// </summary>
        public int OperatorId { get; set; }
        /// <summary>
        /// 操作时间
        /// </summary>
        public DateTime IssueTime { get; set; }

        /// <summary>
        /// 附件信息
        /// </summary>
        public IList<MKuanFile> File { get; set; }

    }

    #endregion

    #region 收付款登记附件实体

    /// <summary>
    /// 收付款登记附件实体
    /// </summary>
    public class MKuanFile
    {
        /// <summary>
        /// 附件编号
        /// </summary>
        public int FileId { get; set; }
        /// <summary>
        /// 收付款登记编号
        /// </summary>
        public string DengJiId { get; set; }
        /// <summary>
        /// 附件名称
        /// </summary>
        public string FileName { get; set; }
        /// <summary>
        /// 附件路径
        /// </summary>
        public string FilePath { get; set; }
    }

    #endregion

    #region 计划应收款登记实体

    /// <summary>
    /// 计划应收款登记实体
    /// </summary>
    public class MShouKuan : MKuanBase
    {
        /// <summary>
        /// 团队编号
        /// </summary>
        public string TourId { get; set; }
    }

    #endregion

    #region 地接付款登记实体

    /// <summary>
    /// 地接付款登记实体
    /// </summary>
    public class MDiJieFuKuan : MKuanBase
    {
        /// <summary>
        /// 地接安排编号
        /// </summary>
        public string DiJiePlanId { get; set; }
    }

    #endregion

    #region 票务付款登记实体

    /// <summary>
    /// 票务付款登记实体
    /// </summary>
    public class MPiaoFuKuan : MKuanBase
    {
        /// <summary>
        /// 票务安排编号
        /// </summary>
        public string PiaoPlanId { get; set; }

        /// <summary>
        /// 票务类型(定、退)
        /// </summary>
        public EnumType.PlanStructure.TicketMode TicketType { get; set; }
    }

    #endregion

    #region 其他收入实体


    /// <summary>
    /// 其他收入实体
    /// </summary>
    public class MQiTaShouKuan : MKuanBase
    {

    }

    #endregion

    #region 其他支付实体

    /// <summary>
    /// 其他支付实体
    /// </summary>
    public class MQiTaFuKuan : MKuanBase
    {

    }

    #endregion

    #region 其他收入支出查询实体

    /// <summary>
    /// 其他收入支出查询实体
    /// </summary>
    public class MSearchOther
    {
        /// <summary>
        /// 开始收支日期
        /// </summary>
        public DateTime? StartTime { get; set; }
        /// <summary>
        /// 结束收支日期
        /// </summary>
        public DateTime? EndTime { get; set; }
        /// <summary>
        /// 收支项目名称
        /// </summary>
        public string ShouZhiXingMu { get; set; }
    }

    #endregion

    #region 财务管理-应付管理-地接列表实体

    /// <summary>
    /// 财务管理-应付管理-地接列表实体
    /// </summary>
    public class MDiJieList
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
        /// 线路名称
        /// </summary>
        public string RouteName { get; set; }

        /// <summary>
        /// 出团日期
        /// </summary>
        public DateTime LDate { get; set; }

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
        /// 供应商编号
        /// </summary>
        public string GysId { get; set; }

        /// <summary>
        /// 供应商名称
        /// </summary>
        public string GysName { get; set; }

        /// <summary>
        /// 计调员
        /// </summary>
        public string PlanPeopleName { get; set; }

        /// <summary>
        /// 应付金额
        /// </summary>
        public decimal YingFuKuan { get; set; }

        /// <summary>
        /// 已付金额
        /// </summary>
        public decimal YiFuKuan { get; set; }

        /// <summary>
        /// 未付金额
        /// </summary>
        public decimal WeiFuKuan
        {
            get
            {
                return this.YingFuKuan - this.YiFuKuan;
            }
        }

        /// <summary>
        /// 地接社联系人信息
        /// </summary>
        public IList<SourceStructure.MSupplierContact> Contact { get; set; }

        /// <summary>
        /// 状态(地接安排)
        /// </summary>
        public EnumType.PlanStructure.DiJieStatus State { get; set; }

        /// <summary>
        /// 团队状态
        /// </summary>
        public EnumType.TourStructure.TourStatus TourState { get; set; }
    }

    #endregion

    #region 财务管理-应付管理-地接列表查询实体

    /// <summary>
    /// 财务管理-应付管理-地接列表查询实体
    /// </summary>
    public class MSearchDiJieList
    {
        /// <summary>
        /// 团号
        /// </summary>
        public string TourNo { get; set; }
        /// <summary>
        /// 出团时间开始
        /// </summary>
        public DateTime? StartLeaveDate { get; set; }
        /// <summary>
        /// 出团时间结束
        /// </summary>
        public DateTime? EndLeaveDate { get; set; }
        /// <summary>
        /// 是否结清
        /// </summary>
        public bool? IsJieQing { get; set; }
        /// <summary>
        /// 是否审核
        /// </summary>
        public bool? IsCheck { get; set; }

        /// <summary>
        /// 是否月结
        /// </summary>
        public bool? IsYueJie { get; set; }

        /// <summary>
        /// 团队类型
        /// </summary>
        public EnumType.TourStructure.TourType? TourType { get; set; }
    }

    #endregion

    #region 财务管理-应付管理-票务列表实体

    /// <summary>
    /// 财务管理-应付管理-票务列表实体
    /// </summary>
    public class MPiaoList
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
        /// 出团日期
        /// </summary>
        public DateTime LDate { get; set; }

        /// <summary>
        /// 成人数
        /// </summary>
        public int Adults { get; set; }

        /// <summary>
        /// 儿童数
        /// </summary>
        public int Childs { get; set; }

        /// <summary>
        /// 车次/航班号
        /// </summary>
        public string TrafficNumber { get; set; }

        /// <summary>
        /// 区间
        /// </summary>
        public string Interval { get; set; }
        /// <summary>
        /// 出票人编号
        /// </summary>
        public int ChuPiaoRenId { get; set; }
        /// <summary>
        /// 出票人名称
        /// </summary>
        public string ChuPiaoRenName { get; set; }

        /// <summary>
        /// 供应商编号
        /// </summary>
        public string GysId { get; set; }

        /// <summary>
        /// 供应商名称
        /// </summary>
        public string GysName { get; set; }

        /// <summary>
        /// 支付方式
        /// </summary>
        public EnumType.FinStructure.ShouFuKuanFangShi PayType { get; set; }

        /// <summary>
        /// 出票或者退票
        /// </summary>
        public EnumType.PlanStructure.TicketMode TicketMode { get; set; }

        /// <summary>
        /// 状态(机票安排)
        /// </summary>
        public EnumType.PlanStructure.TicketStatus State { get; set; }

        /// <summary>
        /// 应付金额
        /// </summary>
        public decimal YingFuKuan { get; set; }

        /// <summary>
        /// 已付金额
        /// </summary>
        public decimal YiFuKuan { get; set; }

        /// <summary>
        /// 未付金额
        /// </summary>
        public decimal WeiFuKuan
        {
            get
            {
                return this.YingFuKuan - this.YiFuKuan;
            }
        }


        /// <summary>
        /// 团队状态
        /// </summary>
        public EnumType.TourStructure.TourStatus TourState { get; set; }
    }

    #endregion

    #region 财务管理-应付管理-票务列表查询实体

    /// <summary>
    /// 财务管理-应付管理-票务列表查询实体
    /// </summary>
    public class MSearchPiaoList
    {
        /// <summary>
        /// 团号
        /// </summary>
        public string TourNo { get; set; }
        /// <summary>
        /// 出团时间开始
        /// </summary>
        public DateTime? StartLeaveDate { get; set; }
        /// <summary>
        /// 出团时间结束
        /// </summary>
        public DateTime? EndLeaveDate { get; set; }
        /// <summary>
        /// 是否结清
        /// </summary>
        public bool? IsJieQing { get; set; }
        /// <summary>
        /// 是否审核
        /// </summary>
        public bool? IsCheck { get; set; }

        /// <summary>
        /// 是否月结
        /// </summary>
        public bool? IsYueJie { get; set; }

        /// <summary>
        /// 供应商名称（出票点）
        /// </summary>
        public string GysName { get; set; }

        /// <summary>
        /// 团队类型
        /// </summary>
        public EnumType.TourStructure.TourType? TourType { get; set; }
    }

    #endregion

    #region 财务管理-应付管理-合计实体

    /// <summary>
    /// 财务管理-应付管理-合计实体
    /// </summary>
    public class MPlanHeJi
    {
        /// <summary>
        /// 应付金额
        /// </summary>
        public decimal YingFuKuan { get; set; }

        /// <summary>
        /// 已付金额
        /// </summary>
        public decimal YiFuKuan { get; set; }

        /// <summary>
        /// 未付金额
        /// </summary>
        public decimal WeiFuKuan
        {
            get
            {
                return this.YingFuKuan - this.YiFuKuan;
            }
        }
    }

    #endregion
}
