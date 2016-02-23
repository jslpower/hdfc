using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EyouSoft.Model.CompanyStructure
{
    /// <summary>
    /// 客户信用等级实体
    /// </summary>
    [Serializable]
    public class Rating
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        public Rating() { }

        /// <summary>
        /// 编号
        /// </summary>
        public int Id { get; set; }
        /// <summary>
        /// 客户信用等级名称
        /// </summary>
        public string RatingName{ get; set; }
        /// <summary>
        /// 公司编号
        /// </summary>
        public int CompanyId { get; set; }
        /// <summary>
        /// 操作人
        /// </summary>
        public int OperatorId { get; set; }
        /// <summary>
        /// 添加时间
        /// </summary>
        public DateTime IssueTime { get; set; }
        /// <summary>
        /// 是否删除
        /// </summary>
        public bool IsDelete { get; set; }
        /// <summary>
        /// 排序编号
        /// </summary>
        public int SortId { get; set; }
    }
}
