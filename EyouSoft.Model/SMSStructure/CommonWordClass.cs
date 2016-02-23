using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EyouSoft.Model.SMSStructure
{
    /// <summary>
    /// 短信常用短语类型表
    /// </summary>
    /// Author:汪奇志 2010-03-25
    /// 修改：xuqh 2011-01-20
    public class CommonWordClass
    {
        /// <summary>
        /// 常用语类型编号
        /// </summary>
        public int ID { get; set; }

        /// <summary>
        /// 公司ID
        /// </summary>
        public int CompanyID { get; set; }

        /// <summary>
        /// 用户ID
        /// </summary>
        public int UserID { get; set; }

        /// <summary>
        /// 类型名称
        /// </summary>
        public string ClassName { get; set; }

        /// <summary>
        /// 添加时间
        /// </summary>
        public DateTime IssueTime { get; set; }

        #region 构造函数
        public CommonWordClass() { }

        public CommonWordClass(int id,int companyId,int userId,string className,DateTime issueTime)
        {
            this.ID = id;
            this.CompanyID = companyId;
            this.UserID = userId;
            this.ClassName = className;
            this.IssueTime = issueTime;
        }
        #endregion
    }
}
