using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EyouSoft.Model.CompanyStructure
{
    /// <summary>
    /// 公司票务
    /// </summary>
    public class MCompanyTicket
    {
        /// <summary>
        /// 编号
        /// </summary>
        public int Id { get; set; }
        /// <summary>
        /// 公司编号
        /// </summary>
        public int CompanyId { get; set; }
        /// <summary>
        /// 票务类型
        /// </summary>
        public EyouSoft.Model.EnumType.PlanStructure.TicketType? TicketType { get; set; }
        /// <summary>
        /// 车次或航班号
        /// </summary>
        public string TrafficNumber { get; set; }
        /// <summary>
        /// 开车或起飞时间
        /// </summary>
        public DateTime? TrafficTime { get; set; }
        /// <summary>
        /// 区间
        /// </summary>
        public string Interval { get; set; }
        /// <summary>
        /// 手续费或机燃费
        /// </summary>
        public decimal OtherPrice { get; set; }
        /// <summary>
        ///  佣金
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
        /// 操作人编号
        /// </summary>
        public int OperatorId { get; set; }
        /// <summary>
        /// 操作人
        /// </summary>
        public string OperatorName { get; set; }
        /// <summary>
        /// 操作时间
        /// </summary>
        public DateTime IssueTime { get; set; }
    }


    /// <summary>
    /// 公司票务的查询
    /// </summary>
    public class MCompanyTicketSearch
    {
        /// <summary>
        /// 票务类型
        /// </summary>
        public EyouSoft.Model.EnumType.PlanStructure.TicketType? TicketType { get; set; }

        /// <summary>
        /// 车次或航班号
        /// </summary>
        public string TrafficNumber { get; set; }

        /// <summary>
        /// 操作人
        /// </summary>
        public string OperatorName { get; set; }
    }
}
