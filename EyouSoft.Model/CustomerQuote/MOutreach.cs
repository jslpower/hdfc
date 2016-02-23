using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EyouSoft.Model.CustomerQuote
{
    #region 外联每日足迹实体

    /// <summary>
    /// 外联每日足迹实体
    /// </summary>
    public class MOutreach
    {
        #region Model

        private int _id;
        private int _companyid = 0;
        private DateTime _saledate = DateTime.Now;
        private string _saleunit;
        private string _salename;
        private string _tel;
        private string _address;
        private string _remark;
        /// <summary>
        /// 编号
        /// </summary>
        public int Id
        {
            set { _id = value; }
            get { return _id; }
        }
        /// <summary>
        /// 公司编号
        /// </summary>
        public int CompanyId
        {
            set { _companyid = value; }
            get { return _companyid; }
        }
        /// <summary>
        /// 销售日期
        /// </summary>
        public DateTime SaleDate
        {
            set { _saledate = value; }
            get { return _saledate; }
        }

        /// <summary>
        /// 销售单位编号
        /// </summary>
        public string SaleUnitId { get; set; }

        /// <summary>
        /// 销售单位
        /// </summary>
        public string SaleUnit
        {
            set { _saleunit = value; }
            get { return _saleunit; }
        }
        /// <summary>
        /// 销售人
        /// </summary>
        public string SaleName
        {
            set { _salename = value; }
            get { return _salename; }
        }
        /// <summary>
        /// 联系电话
        /// </summary>
        public string Tel
        {
            set { _tel = value; }
            get { return _tel; }
        }
        /// <summary>
        /// 单位地址
        /// </summary>
        public string Address
        {
            set { _address = value; }
            get { return _address; }
        }
        /// <summary>
        /// 备注
        /// </summary>
        public string Remark
        {
            set { _remark = value; }
            get { return _remark; }
        }

        /// <summary>
        /// 操作人编号
        /// </summary>
        public int OperatorId { get; set; }

        /// <summary>
        /// 添加时间
        /// </summary>
        public DateTime IssueTime { get; set; }

        #endregion Model
    }

    #endregion

    #region 外联每日足迹查询实体

    /// <summary>
    /// 外联每日足迹查询实体
    /// </summary>
    public class MSearchOutreach
    {
        /// <summary>
        /// 销售单位编号
        /// </summary>
        public string SaleUnitId { get; set; }

        /// <summary>
        /// 销售单位
        /// </summary>
        public string SaleUnitName { get; set; }

        /// <summary>
        /// 销售时间开始
        /// </summary>
        public DateTime? StartSaleTime { get; set; }

        /// <summary>
        /// 销售时间结束
        /// </summary>
        public DateTime? EndSaleTime { get; set; }
    }

    #endregion
}
