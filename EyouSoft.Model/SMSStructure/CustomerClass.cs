using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EyouSoft.Model.SMSStructure
{
    /// <summary>
    /// 短信中心客户类型表
    /// </summary>
    /// Author:汪奇志 2010-03-25
    /// 修改：xuqh 2011-01-20
    public class CustomerClass
    {
        #region Model
        /// <summary>
        /// 客户类型编号
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// 添加公司ID
        /// </summary>
        public int CompanyID { get; set; }

        /// <summary>
        /// 添加用户ID
        /// </summary>
        public int UserID { get; set; }

        /// <summary>
        /// 客户类型名称
        /// </summary>
        public string ClassName { get; set; }

        /// <summary>
        /// 添加时间
        /// </summary>
        public DateTime IssueTime { get; set; }
        #endregion

        #region 构造函数
        public CustomerClass() { }

        public CustomerClass(int id, int companyId, int userId, string className, DateTime issueTime)
        {
            this.Id = id;
            this.CompanyID = companyId;
            this.UserID = userId;
            this.ClassName = className;
            this.IssueTime = issueTime;
        }
        #endregion
    }

}
