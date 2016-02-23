using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EyouSoft.Model.CompanyStructure
{
    /// <summary>
    /// 公司城市实体
    /// </summary>
    [Serializable]
    public class City
    {
        #region 构造函数
        /// <summary>
        /// 构造函数
        /// </summary>
        public City() { }
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
        /// 公司编号
        /// </summary>
        public int CompanyId
        {
            get;
            set;
        }
        /// <summary>
        /// 省份编号
        /// </summary>
        public int ProvinceId
        {
            get;
            set;
        }
        /// <summary>
        /// 城市名称
        /// </summary>
        public string CityName
        {
            get;
            set;
        }
        /// <summary>
        /// 是否常用
        /// </summary>
        public bool IsFav
        {
            get;
            set;
        }
        /// <summary>
        /// 操作人编号
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
    }
}
