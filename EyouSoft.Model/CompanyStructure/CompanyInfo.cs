using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EyouSoft.Model.CompanyStructure
{
    /// <summary>
    /// 专线公司信息实体
    /// </summary>
    public class CompanyInfo
    {
        /// <summary>
        /// 公司编号
        /// </summary>
        public int Id { get; set; }
        /// <summary>
        /// 公司名称
        /// </summary>
        public string CompanyName { get; set; }
        /// <summary>
        /// 旅行社类别
        /// </summary>
        public string CompanyType { get; set; }
        /// <summary>
        /// 公司英文名称
        /// </summary>
        public string CompanyEnglishName { get; set; }
        /// <summary>
        /// 许可证号
        /// </summary>
        public string License { get; set; }
        /// <summary>
        /// 公司负责人
        /// </summary>
        public string ContactName { get; set; }
        /// <summary>
        /// 电话号码
        /// </summary>
        public string ContactTel { get; set; }
        /// <summary>
        /// 手机号码
        /// </summary>
        public string ContactMobile { get; set; }
        /// <summary>
        /// 传真号码
        /// </summary>
        public string ContactFax { get; set; }
        /// <summary>
        /// 地址
        /// </summary>
        public string CompanyAddress { get; set; }
        /// <summary>
        /// 邮编
        /// </summary>
        public string CompanyZip { get; set; }
        /// <summary>
        /// 公司网站Url
        /// </summary>
        public string CompanySiteUrl { get; set; }
        /// <summary>
        /// 系统Id
        /// </summary>
        public int SystemId { get; set; }
        /// <summary>
        /// 添加时间
        /// </summary>
        public DateTime IssueTime { get; set; }

        /// <summary>
        /// 公司附件集合
        /// </summary>
        public IList<CompanyFile> FilePath { get; set; }
    }

    /// <summary>
    /// 公司附件实体
    /// </summary>
    public class CompanyFile
    {
        /// <summary>
        /// 附件编号
        /// </summary>
        public string FileId { get; set; }

        /// <summary>
        /// 附件名称
        /// </summary>
        public string FilePath { get; set; }
    }
}
