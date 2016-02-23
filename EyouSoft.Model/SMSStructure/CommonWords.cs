using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EyouSoft.Model.SMSStructure
{
    /// <summary>
    /// 短信常用短语列表
    /// </summary>
    /// Author:汪奇志 2010-03-25
    /// 修改：xuqh 2011-01-20
    public class CommonWords
    {
        /// <summary>
        /// 常用短语编号
        /// </summary>
        public string ID { get; set; }

        /// <summary>
        /// 添加公司ID
        /// </summary>
        public int CompanyID { get; set; }

        /// <summary>
        /// 添加用户ID
        /// </summary>
        public int UserID { get; set; }

        /// <summary>
        /// 常用语类型ID
        /// </summary>
        public int ClassID { get; set; }

        /// <summary>
        /// 常用语内容
        /// </summary>
        public string WordContent { get; set; }

        /// <summary>
        /// 添加时间
        /// </summary>
        public DateTime IssueTime { get; set; }

        /// <summary>
        /// 常用短语类型名
        /// </summary>
        public string ClassName { get; set; }
        #region 构造函数
        public CommonWords() { }

        public CommonWords(string id,int companyId,int userId,int classId,string wordContent,DateTime issueTime,string className)
        {
            this.ID = id;
            this.CompanyID = companyId;
            this.UserID = userId;
            this.ClassID = classId;
            this.WordContent = wordContent;
            this.IssueTime = issueTime;
            this.ClassName = className;
        }
        #endregion
    }
}
