using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EyouSoft.Model.CustomerQuote
{
    #region 客户日常询价实体

    /// <summary>
    /// 客户日常询价实体
    /// </summary>
    public class MCustomerQuote
    {
        #region Model

        private int _quoteid;
        private int _companyid = 0;
        private DateTime _leavedate;
        private int _peoplenum = 0;
        private string _costomer;
        private DateTime? _quotedate;
        private string _contactname;
        private string _contacttel;
        private string _contactmobile;
        private string _contactqq;
        private string _content;
        private int _operatorid = 0;
        private DateTime _issuetime = DateTime.Now;
        /// <summary>
        /// 询价编号
        /// </summary>
        public int QuoteId
        {
            set { _quoteid = value; }
            get { return _quoteid; }
        }
        /// <summary>
        /// 公司编号
        /// </summary>
        public int CompanyId
        {
            set { _companyid = value; }
            get { return _companyid; }
        }
        /// <summary>
        /// 出团日期
        /// </summary>
        public DateTime LeaveDate
        {
            set { _leavedate = value; }
            get { return _leavedate; }
        }
        /// <summary>
        /// 人数
        /// </summary>
        public int PeopleNum
        {
            set { _peoplenum = value; }
            get { return _peoplenum; }
        }

        /// <summary>
        /// 询价单位编号
        /// </summary>
        public string CostomerId { get; set; }

        /// <summary>
        /// 询价单位
        /// </summary>
        public string Costomer
        {
            set { _costomer = value; }
            get { return _costomer; }
        }
        /// <summary>
        /// 询价时间
        /// </summary>
        public DateTime? QuoteDate
        {
            set { _quotedate = value; }
            get { return _quotedate; }
        }
        /// <summary>
        /// 询价人
        /// </summary>
        public string ContactName
        {
            set { _contactname = value; }
            get { return _contactname; }
        }
        /// <summary>
        /// 联系电话
        /// </summary>
        public string ContactTel
        {
            set { _contacttel = value; }
            get { return _contacttel; }
        }
        /// <summary>
        /// 手机
        /// </summary>
        public string ContactMobile
        {
            set { _contactmobile = value; }
            get { return _contactmobile; }
        }
        /// <summary>
        /// QQ
        /// </summary>
        public string ContactQQ
        {
            set { _contactqq = value; }
            get { return _contactqq; }
        }
        /// <summary>
        /// 询价内容
        /// </summary>
        public string Content
        {
            set { _content = value; }
            get { return _content; }
        }
        /// <summary>
        /// 操作人编号
        /// </summary>
        public int OperatorId
        {
            set { _operatorid = value; }
            get { return _operatorid; }
        }

        /// <summary>
        /// 操作人
        /// </summary>
        public string Operator { get; set; }

        /// <summary>
        /// 添加时间
        /// </summary>
        public DateTime IssueTime
        {
            set { _issuetime = value; }
            get { return _issuetime; }
        }

        #endregion Model

        /// <summary>
        /// 今年询价次数
        /// </summary>
        public int YearQuoteCount { get; set; }
    }

    #endregion

    #region 客户日常询价查询实体

    /// <summary>
    /// 客户日常询价查询实体
    /// </summary>
    public class MSearchCustomerQuote
    {
        /// <summary>
        /// 询价单位编号
        /// </summary>
        public string CostomerId { get; set; }

        /// <summary>
        /// 询价单位
        /// </summary>
        public string QuoteUnitName { get; set; }

        /// <summary>
        /// 询价时间开始
        /// </summary>
        public DateTime? StartQuoteTime { get; set; }

        /// <summary>
        /// 询价时间结束
        /// </summary>
        public DateTime? EndQuoteTime { get; set; }
    }

    #endregion
}
