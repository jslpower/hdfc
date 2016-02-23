using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EyouSoft.Model.CompanyStructure
{
    #region 银行账户基类
    /// <summary>
    /// 银行账户基类
    /// </summary>
    [Serializable]
    public class CompanyAccountBase
    {
        /// <summary>
        /// 账户姓名
        /// </summary>
        public string AccountName { get; set; }
        /// <summary>
        /// 开户银行行名称
        /// </summary>
        public string BankName { get; set; }
        /// <summary>
        /// 帐号
        /// </summary>
        public string BankNo { get; set; }
    }
    #endregion

    #region  专线公司账户信息实体
    /// <summary>
    /// 专线公司账户信息实体
    /// </summary>
    [Serializable]
    public class CompanyAccount : CompanyAccountBase
    {
        /// <summary>
        /// 编号
        /// </summary>
        public string  Id { get; set; }
        /// <summary>
        /// 账号性质
        /// </summary>
        public EnumType.CompanyStructure.AccountType AccountType { get; set; }
        /// <summary>
        /// 公司编号
        /// </summary>
        public int CompanyId { get; set; }
        /// <summary>
        /// 公司银行帐号状态
        /// </summary>
        public EnumType.CompanyStructure.AccountState AccountState { get; set; }
        /// <summary>
        /// 原始金额(不给修改)
        /// </summary>
        public decimal AccountMoney { get; set; }
        /// <summary>
        /// 附件路径
        /// </summary>
        public string FilePath { get; set; }
        /// <summary>
        /// 添加时间
        /// </summary>
        public DateTime IssueTime { get; set; }
        /// <summary>
        /// 操作人编号
        /// </summary>
        public int OperatorId { get; set; }
    }
    #endregion


    #region 操作人相关信息业务实体

    /// <summary>
    /// 操作人相关信息业务实体
    /// </summary>
    public class MOperatorInfo
    {
        /// <summary>
        /// default constructor
        /// </summary>
        public MOperatorInfo() { }

        /// <summary>
        /// 操作人编号
        /// </summary>
        public int OperatorId { get; set; }
        /// <summary>
        /// 操作时间
        /// </summary>
        public DateTime OperatorTime
        {
            get { return DateTime.Now; }
        }
        /// <summary>
        /// 操作备注
        /// </summary>
        public string BeiZhu { get; set; }
    }

    #endregion
}
