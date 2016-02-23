using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EyouSoft.Model.CompanyStructure
{
    /// <summary>
    /// 销售地区实体
    /// </summary>
    public class MSaleArea
    {
        /// <summary>
        /// 销售地区编号
        /// </summary>
        public int SaleAreaId { get; set; }

        /// <summary>
        /// 销售地区名称
        /// </summary>
        public string SaleAreaName { get; set; }

        /// <summary>
        /// 公司编号
        /// </summary>
        public int CompanyId { get; set; }
    }

    /// <summary>
    /// 销售地区查询实体
    /// </summary>
    public class MSearchSaleArea
    {

    }
}
