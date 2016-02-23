using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EyouSoft.Model.SMSStructure
{
    /// <summary>
    /// 短信中心客户列表
    /// </summary>
    /// Author:汪奇志 2010-03-25
    /// 修改：xuqh-2011-01-20
    public class CustomerList
    {
        #region Model
        /// <summary>
        /// 客户列表编号
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
        /// 客户单位名称
        /// </summary>
        public string CustomerCompanyName { get; set; }

        /// <summary>
        /// 客户姓名
        /// </summary>
        public string CustomerContactName { get; set; }

        /// <summary>
        /// 客户类型
        /// </summary>
        public int ClassID { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        public string ReMark { get; set; }

        /// <summary>
        /// 手机号码
        /// </summary>
        public string MobileNumber { get; set; }

        /// <summary>
        /// 添加时间
        /// </summary>
        public DateTime IssueTime { get; set; }

        /// <summary>
        /// 省份ID
        /// </summary>
        public int ProvinceId { get; set; }

        /// <summary>
        /// 城市ID
        /// </summary>
        public int CityId { get; set; }
        #endregion

        /// <summary>
        /// 客户类型
        /// </summary>
        public string ClassName { get; set; }

        /// <summary>
        /// 客户来源
        /// </summary>
        public enum CustomerSource
        {
            /// <summary>
            /// 所有客户
            /// </summary>
            所有客户 = 0,

            /// <summary>
            /// 我的客户
            /// </summary>
            我的客户 = 1,

            /// <summary>
            /// 平台组团社客户
            /// </summary>
            平台组团社客户 = 2
        }

        #region 构造函数
        public CustomerList() { }

        public CustomerList(string id, int companyId, int userId, string customerCompanyName, 
                            string customerContactName, int classId, string reMark, string mobileNumber, 
                            DateTime issueTime, int provinceId,int cityId)
        {
            this.ID = id;
            this.CompanyID = companyId;
            this.UserID = userId;
            this.CustomerCompanyName = customerCompanyName;
            this.CustomerContactName = customerContactName;
            this.ClassID = classId;
            this.ReMark = reMark;
            this.MobileNumber = mobileNumber;
            this.IssueTime = issueTime;
            this.ProvinceId = provinceId;
            this.CityId = cityId;
        }
        #endregion
    }
}
