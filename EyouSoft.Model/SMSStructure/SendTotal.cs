using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EyouSoft.Model.SMSStructure
{
    /// <summary>
    /// 短信发送统计表
    /// </summary>
    /// Author:汪奇志 2010-03-25
    /// 修改：xuqh 2011-01-20
    public class SendTotal
    {
        /// <summary>
        /// 统计编号
        /// </summary>
        public string ID { get; set; }

        /// <summary>
        /// 发送短信的公司编号
        /// </summary>
        public int CompanyID { get; set; }

        /// <summary>
        /// 发送短信的公司名称
        /// </summary>
        public string CompanyName { get; set; }

        /// <summary>
        /// 发送短信的用户编号
        /// </summary>
        public int UserID { get; set; }

        /// <summary>
        /// 发送短信的用户姓名
        /// </summary>
        public string ContactName { get; set; }

        /// <summary>
        /// 短信类型
        /// </summary>
        public byte SMSType { get; set; }

        /// <summary>
        /// 发送内容
        /// </summary>
        public string SMSContent { get; set; }

        /// <summary>
        /// 提交时间
        /// </summary>
        public DateTime IssueTime { get; set; }

        /// <summary>
        /// 消费金额
        /// </summary>
        public decimal UseMoeny { get; set; }

        /// <summary>
        /// 发送成功短信数量
        /// </summary>
        public int SuccessCount { get; set; }

        /// <summary>
        /// 发送失败短信数量
        /// </summary>
        public int ErrorCount { get; set; }

        /// <summary>
        /// 发送成功实际短信数量
        /// </summary>
        public int SuccessSplitCount { get; set; }

        /// <summary>
        /// 发送失败实际短信数量
        /// </summary>
        public int ErrorSplitCount { get; set; }

        /// <summary>
        /// 每条实际短信单价
        /// </summary>
        public decimal SMSUnitPrice { get; set; }

        /// <summary>
        /// 短信发送通道
        /// </summary>
        public SMSChannel SendChannel { get; set; }
    }
}
