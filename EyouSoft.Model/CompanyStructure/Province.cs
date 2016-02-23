using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EyouSoft.Model.CompanyStructure
{
    /// <summary>
    /// 公司省份表
    /// </summary>
    public class Province
    {
        #region 构造函数
        /// <summary>
        /// 构造函数
        /// </summary>
        public Province() { }
        #endregion

        #region 属性
        /// <summary>
        /// 编号
        /// </summary>
        public int Id
        {
            get;
            set;
        }
        /// <summary>
        /// 省份名称
        /// </summary>
        public string ProvinceName
        {
            get;
            set;
        }
        /// <summary>
        /// 公司编号
        /// </summary>
        public int CompanyId
        {
            get;
            set;
        }
        /// <summary>
        /// 操作人
        /// </summary>
        public int OperatorId
        {
            get;
            set;
        }
        /// <summary>
        /// 添加时间
        /// </summary>
        public DateTime IssueTime
        {
            get;
            set;
        }
        #endregion

        #region 城市集合
        /// <summary>
        /// 下属城市集合
        /// </summary>
        public IList<EyouSoft.Model.CompanyStructure.City> CityList
        {
            get;
            set;
        }
        #endregion
    }
}
