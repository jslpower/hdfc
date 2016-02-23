using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EyouSoft.Model.FinStructure
{
    /// <summary>
    /// 银行余额实体
    /// </summary>
    public class MBankBalance
    {
        /// <summary>
        /// 余额编号
        /// </summary>
        public int Id { get; set; }
        /// <summary>
        /// 日期
        /// </summary>
        public DateTime Date { get; set; }
        /// <summary>
        /// 银行编号
        /// </summary>
        public string BankId { get; set; }
        /// <summary>
        /// 余额
        /// </summary>
        public decimal Balance { get; set; }

        /// <summary>
        /// 公司编号
        /// </summary>
        public int CompanyId { get; set; }

        /// <summary>
        /// 操作人编号
        /// </summary>
        public int OperatorId { get; set; }

        /// <summary>
        /// 操作时间
        /// </summary>
        public DateTime IssueTime { get; set; }

        /// <summary>
        /// 银行名称
        /// </summary>
        public string BankName { get; set; }
    }

    /// <summary>
    /// 银行余额查询实体
    /// </summary>
    public class MSearchBankBalance
    {
        /// <summary>
        /// 开始日期
        /// </summary>
        public DateTime? StartDate { get; set; }
        /// <summary>
        /// 结束日期
        /// </summary>
        public DateTime? EndDate { get; set; }
    }
}
