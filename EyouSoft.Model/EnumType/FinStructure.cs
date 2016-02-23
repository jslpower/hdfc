using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EyouSoft.Model.EnumType.FinStructure
{

    /// <summary>
    /// 收付款方式
    /// </summary>
    public enum ShouFuKuanFangShi
    {
        /// <summary>
        /// 银行电汇
        /// </summary>
        银行电汇 = 0,
        /// <summary>
        /// 转账支票
        /// </summary>
        转账支票,
        /// <summary>
        /// 财务现收
        /// </summary>
        财务现收,
        /// <summary>
        /// 财务现付
        /// </summary>
        财务现付,
        /// <summary>
        /// 导游现收
        /// </summary>
        导游现收,
        /// <summary>
        /// 导游现付
        /// </summary>
        导游现付
    }


    /// <summary>
    /// 收付款登记明细类型
    /// </summary>
    public enum KuanXiangType
    {
        /// <summary>
        /// 收入-计划收款
        /// </summary>
        计划收款 = 0,

        /// <summary>
        /// 收入-其它收入
        /// </summary>
        其它收入 = 1,

        /// <summary>
        /// 支出-地接支出付款
        /// </summary>
        地接支出付款 = 101,

        /// <summary>
        /// 支出-出票支出
        /// </summary>
        出票支出 = 102,

        /// <summary>
        /// 支出-退票支出
        /// </summary>
        退票支出 = 103,

        /// <summary>
        /// 支出-其它支出
        /// </summary>
        其它支出 = 104,


    }

    /// <summary>
    /// 出纳登账的类型
    /// </summary>
    public enum DengJiMode
    {
        /// <summary>
        /// 收款
        /// </summary>
        收款 = 0,

        /// <summary>
        /// 退款
        /// </summary>
        退款,
    }


    /// <summary>
    /// 收付款登记状态
    /// </summary>
    public enum KuanXiangStatus
    {
        /// <summary>
        /// 未审批
        /// </summary>
        未审批 = 0,
        /// <summary>
        /// 未支付
        /// </summary>
        未支付 = 1,
        /// <summary>
        /// 已支付
        /// </summary>
        已支付 = 2
    }




}
