using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EyouSoft.Model.EnumType.PlanStructure
{
    /// <summary>
    /// 地接状态
    /// </summary>
    public enum DiJieStatus
    {
        /// <summary>
        /// 申请中
        /// </summary>
        申请中 = 0,

        /// <summary>

        /// 已确认
        /// </summary>
        已确认,

        /// <summary>
        /// 已审核
        /// </summary>
        已审核,

    }

    /// <summary>
    /// 出票或退票
    /// </summary>
    public enum TicketMode
    {
        /// <summary>
        /// 出票
        /// </summary>
        出票 = 0,

        /// <summary>
        /// 退票
        /// </summary>
        退票,

    }

    /// <summary>
    /// 票务类型
    /// </summary>
    public enum TicketType
    {
        /// <summary>
        /// 机票
        /// </summary>
        机票 = 0,

        /// <summary>
        /// 火车票
        /// </summary>
        火车票,
    }

    /// <summary>
    /// 票务状态
    /// </summary>
    public enum TicketStatus
    {
        /// <summary>
        /// 申请中
        /// </summary>
        已申请 = 0,

        /// <summary>
        /// 已确认
        /// </summary>
        已确认,

        /// <summary>
        /// 已审核
        /// </summary>
        已出票,

    }


    /// <summary>
    ///  舱位/席别
    /// </summary>
    public enum TrafficSeat
    {
        /// <summary>
        /// 硬座
        /// </summary>
        硬座 = 1,

        /// <summary>
        /// 软座
        /// </summary>
        软座 = 2,

        /// <summary>
        /// 硬卧
        /// </summary>
        硬卧 = 3,

        /// <summary>
        /// 软卧
        /// </summary>
        软卧 = 4,

        /// <summary>
        /// 高铁一等
        /// </summary>
        高铁一等 = 5,

        /// <summary>
        /// 高铁二等
        /// </summary>
        高铁二等 = 6,

        /// <summary>
        /// 商务舱
        /// </summary>
        商务舱 = 7,

        /// <summary>
        /// 经济舱
        /// </summary>
        经济舱 = 8,

        /// <summary>
        /// 头等舱
        /// </summary>
        头等舱 = 9

    }



}
