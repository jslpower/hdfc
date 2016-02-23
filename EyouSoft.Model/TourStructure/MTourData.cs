using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EyouSoft.Model.TourStructure
{
    public class MTourData
    {
        /// <summary>
        /// 编号
        /// </summary>
        public int TourDataId { get; set; }

        /// <summary>
        /// 公司编号
        /// </summary>
        public int CompanyId { get; set; }

        /// <summary>
        /// 添加时间
        /// </summary>
        public DateTime IssueTime { get; set; }

        /// <summary>
        /// 线路编号
        /// </summary>
        public int AreaId { get; set; }

        /// <summary>
        /// 线路名称
        /// </summary>
        public string RouteName { get; set; }

        /// <summary>
        /// 团型
        /// </summary>
        public EyouSoft.Model.EnumType.TourStructure.TourDataType TourDataType { get; set; }

        /// <summary>
        /// 进出港口
        /// </summary>
        public string TourPort { get; set; }

        /// <summary>
        /// 操作人编号
        /// </summary>
        public int OperatorId { get; set; }

        /// <summary>
        /// 操作人
        /// </summary>
        public string OperatorName { get; set; }


        /// <summary>
        /// 文件附件
        /// </summary>
        public IList<MFile> FileList { get; set; }


        public bool IsCheck { get; set; }
    }

    public class MSearchTourData
    {
        public int? AreaId { get; set; }

        public string RouteName { get; set; }

        public bool? IsCheck { get; set; }
    }
}
