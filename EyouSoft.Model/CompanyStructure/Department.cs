using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EyouSoft.Model.CompanyStructure
{
   /// <summary>
	/// 系统设置-部门信息实体
	/// </summary>
    [Serializable]
    public class Department
    {
        /// <summary>
        /// 编号
        /// </summary>
        public int Id { get; set; }
        /// <summary>
        /// 部门名称
        /// </summary>
        public string DepartName { get; set; }
        /// <summary>
        /// 上级部门
        /// </summary>
        public int PrevDepartId { get; set; }
        /// <summary>
        /// 部门主管
        /// </summary>
        public int DepartManger { get; set; }
        /// <summary>
        /// 联系电话
        /// </summary>
        public string ContactTel { get; set; }
        /// <summary>
        /// 传真
        /// </summary>
        public string ContactFax { get; set; }
        /// <summary>
        /// 备注
        /// </summary>
        public string Remark { get; set; }
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
        /// 是否存在子级部门
        /// </summary>
        public bool HasNextLev { get; set; }
    }

}
