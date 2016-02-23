using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EyouSoft.Model.SMSStructure
{
    /// <summary>
    /// 短信充值支付记录
    /// </summary>
    /// Author:汪奇志 2010-03-25
    /// 修改： xuqh 2011-01-21
    public class PayMoneyInfo
    {
        /// <summary>
        /// default constructor
        /// </summary>
        public PayMoneyInfo() { }

        /// <summary>
        /// 充值编号
        /// </summary>
        public string ID { get; set; }

        /// <summary>
        /// 充值公司编号
        /// </summary>
        public int CompanyID { get; set; }

        /// <summary>
        /// 充值公司名称
        /// </summary>
        public string CompanyName { get; set; }

        /// <summary>
        /// 充值人ID
        /// </summary>
        public int OperatorID { get; set; }

        /// <summary>
        /// 充值人姓名
        /// </summary>
        public string OperatorName { get; set; }

        /// <summary>
        /// 充值人电话
        /// </summary>
        public string OperatorTel { get; set; }

        /// <summary>
        /// 充值人手机
        /// </summary>
        public string OperatorMobile { get; set; }


        /// <summary>
        /// 可用金额
        /// </summary>
        public decimal UseMoney { get; set; }

        /// <summary>
        /// 充值金额
        /// </summary>
        public decimal PayMoney { get; set; }

        /// <summary>
        /// 充值短信条数
        /// </summary>
        public int PaySMSNumber { get; set; }

        /// <summary>
        /// 充值时间
        /// </summary>
        public DateTime PayTime { get; set; }

        /// <summary>
        /// 充值操作时间
        /// </summary>
        public DateTime OperatorTime { get; set; }

        /// <summary>
        /// 审核状态 0:未审核 1:审核通过  2:审核未通过
        /// </summary>
        public int IsChecked { get; set; }

        /// <summary>
        /// 审核时间
        /// </summary>
        public DateTime CheckTime { get; set; }

        /// <summary>
        /// 审核人用户名
        /// </summary>
        public string CheckUserName { get; set; }

        /// <summary>
        /// 审核人姓名
        /// </summary>
        public string CheckOperatorName { get; set; }

        /// <summary>
        /// 充值公司省份编号
        /// </summary>
        public int ProvinceId { get; set; }

        /// <summary>
        /// 充值公司城市编号
        /// </summary>
        public int CityId { get; set; }
    }
}
