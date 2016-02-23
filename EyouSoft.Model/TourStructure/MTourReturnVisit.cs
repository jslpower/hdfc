using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EyouSoft.Model.TourStructure
{
    public class MTourReturnVisit
    {
        /// <summary>
        /// 回访编号
        /// </summary>
        public string VisitId { get; set; }

        /// <summary>
        /// 团队编号
        /// </summary>
        public string TourId { get; set; }

        /// <summary>
        /// 全陪姓名
        /// </summary>
        public string QuanPeiName { get; set; }

        /// <summary>
        /// 全陪电话
        /// </summary>
        public string QuanPeiPhone { get; set; }

        /// <summary>
        /// 回访时间
        /// </summary>
        public DateTime? VisitTime { get; set; }

        /// <summary>
        /// 回访人
        /// </summary>
        public string Vistor { get; set; }

        /// <summary>
        /// 回访星级
        /// </summary>
        public EyouSoft.Model.EnumType.TourStructure.Score Score { get; set; }

        /// <summary>
        /// 客人意见
        /// </summary>
        public string CustomerOpinion { get; set; }

        /// <summary>
        /// 操作人编号
        /// </summary>
        public int OperatorId { get; set; }

        /// <summary>
        /// 添加时间
        /// </summary>
        public DateTime IssueTime { get; set; }
    }


    /// <summary>
    /// 团队回访列表
    /// </summary>
    public class MPageTourReturnVisit : MTour
    {
        /// <summary>
        /// 地接名称
        /// </summary>
        public string DiJieName { get { return this.DiJieList != null ? this.DiJieList.FirstOrDefault().DiJieName : null; } }

        /// <summary>
        /// 地接信息
        /// </summary>
        public IList<EyouSoft.Model.TourStructure.MTourDiJie> DiJieList { get; set; }


        /// <summary>
        /// 导游信息
        /// </summary>
        public IList<MTourGuide> GuideList { get; set; }

        /// <summary>
        /// 导游名称
        /// </summary>
        public string GuideName { get { return this.GuideList != null ? this.GuideList.FirstOrDefault().GuideName : null; } }

        /// <summary>
        /// 回访集合
        /// </summary>
        public IList<MTourReturnVisit> VisitList { get; set; }

        /// <summary>
        /// 全陪姓名
        /// </summary>
        public string QuanPeiName { get { return VisitList != null ? VisitList.FirstOrDefault().QuanPeiName : null; } }

        /// <summary>
        /// 回访次数
        /// </summary>
        public int VisitNum { get { return VisitList != null ? VisitList.Count : 0; } }


        /// <summary>
        /// 第一次评分
        /// </summary>
        public EyouSoft.Model.EnumType.TourStructure.Score? FristScore { get { return VisitList != null ? (EyouSoft.Model.EnumType.TourStructure.Score?)VisitList[0].Score : null; } }

        /// <summary>
        /// 第二次评分
        /// </summary>
        public EyouSoft.Model.EnumType.TourStructure.Score? SecondScore { get { return VisitList != null && VisitList.Count >=2 ? (EyouSoft.Model.EnumType.TourStructure.Score?)VisitList[1].Score : null; } }

    }

    /// <summary>
    /// 查询实体
    /// </summary>
    public class MSeachVist
    {
        /// <summary>
        /// 出团开始时间
        /// </summary>
        public DateTime? LBeginDate { get; set; }

        /// <summary>
        /// 出团结束时间
        /// </summary>
        public DateTime? LEndDate { get; set; }

        /// <summary>
        /// 团号
        /// </summary>
        public string TourCode { get; set; }

        /// <summary>
        /// 是否回访
        /// </summary>
        public bool? IsVist { get; set; }

        /// <summary>
        /// 团队类型
        /// </summary>
        public EyouSoft.Model.EnumType.TourStructure.TourType? TourType { get; set; }
    }
}
