using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EyouSoft.Model.FinStructure
{
    /// <summary>
    /// 出纳登账实体
    /// </summary>
    public class MChuNaDengZhang
    {
        /// <summary>
        /// 登账编号
        /// </summary>
        public string DengZhangId { get; set; }
        /// <summary>
        /// 公司编号
        /// </summary>
        public int CompanyId { get; set; }
        /// <summary>
        /// 收款/付款
        /// </summary>
        public EnumType.FinStructure.DengJiMode DengZhangType { get; set; }
        /// <summary>
        /// 收支登记编号
        /// </summary>
        public string DengJiId { get; set; }
        /// <summary>
        /// 团号
        /// </summary>
        public string TourNo { get; set; }
        /// <summary>
        /// 登账人编号
        /// </summary>
        public int DengZhangPeopleId { get; set; }

        /// <summary>
        /// 登账人姓名
        /// </summary>
        public string DengZhangPeople { get; set; }

        /// <summary>
        /// 收款/付款日期
        /// </summary>
        public DateTime DengZhangDate { get; set; }
        /// <summary>
        /// 金额
        /// </summary>
        public decimal Price { get; set; }
        /// <summary>
        /// 银行账号编号
        /// </summary>
        public string BankId { get; set; }
        /// <summary>
        /// 支付方式
        /// </summary>
        public EnumType.FinStructure.ShouFuKuanFangShi PayMode { get; set; }
        /// <summary>
        /// 收支款原因
        /// </summary>
        public string Reason { get; set; }
        /// <summary>
        /// 手续费
        /// </summary>
        public decimal OtherPrice { get; set; }
        /// <summary>
        /// 是否开票
        /// </summary>
        public bool IsKaiPiao { get; set; }
        /// <summary>
        /// 操作人编号
        /// </summary>
        public int OperatorId { get; set; }
        /// <summary>
        /// 记录添加时间
        /// </summary>
        public DateTime IssueTime { get; set; }

        /// <summary>
        /// 银行名称
        /// </summary>
        public string BankName { get; set; }

        /// <summary>
        /// 出纳登账附件信息
        /// </summary>
        public IList<MChuNaDengZhangFile> File { get; set; }

        /// <summary>
        /// 是否最新出纳登账
        /// </summary>
        public bool IsNew
        {
            get
            {
                var dt = DateTime.Now;
                if (this.DengZhangDate.Year == dt.Year && this.DengZhangDate.Month == dt.Month && this.DengZhangDate.Day == dt.Day)
                    return true;

                return false;
            }
        }

        /// <summary>
        /// 是否可以删除和修改
        /// </summary>
        public bool IsEdit { get; set; }
    }

    /// <summary>
    /// 出纳登账查询实体
    /// </summary>
    public class MSearchChuNaDengZhang
    {
        /// <summary>
        /// 收款/付款日期开始
        /// </summary>
        public DateTime? StartTime { get; set; }

        /// <summary>
        /// 收款/付款日期结束
        /// </summary>
        public DateTime? EndTime { get; set; }

        /// <summary>
        /// 收款/付款
        /// </summary>
        public EnumType.FinStructure.DengJiMode? DengZhangType { get; set; }
    }

    /// <summary>
    /// 出纳登账合计实体
    /// </summary>
    public class MChuNaDengZhangHeJi
    {
        /// <summary>
        /// 金额
        /// </summary>
        public decimal SumPrice { get; set; }

        /// <summary>
        /// 手续费
        /// </summary>
        public decimal SumOtherPrice { get; set; }
    }

    /// <summary>
    /// 出纳登账附件实体
    /// </summary>
    public class MChuNaDengZhangFile
    {
        /// <summary>
        /// 附件编号
        /// </summary>
        public int FileId { get; set; }
        /// <summary>
        /// 出纳登账编号
        /// </summary>
        public string DengZhangId { get; set; }
        /// <summary>
        /// 附件名称
        /// </summary>
        public string FileName { get; set; }
        /// <summary>
        /// 附件路径
        /// </summary>
        public string FilePath { get; set; }
    }
}
