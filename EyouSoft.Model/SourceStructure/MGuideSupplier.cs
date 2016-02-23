using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EyouSoft.Model.SourceStructure
{
    /// <summary>
    /// 供应商导游
    /// </summary>
    public class MGuideSupplier
    {
        /// <summary>
        /// 编号
        /// </summary>
        public string Id { get; set; }


        /// <summary>
        /// 公司编号
        /// </summary>
        public int CompanyId { get; set; }

        /// <summary>
        /// 旅行社编号
        /// </summary>
        public string GysId { get; set; }

        /// <summary>
        /// 旅行社名称
        /// </summary>
        public string GysName { get; set; }

        /// <summary>
        /// 省份编号
        /// </summary>
        public int ProvinceId { get; set; }

        /// <summary>
        /// 城市编号
        /// </summary>
        public int CityId { get; set; }

        /// <summary>
        /// 导游名称
        /// </summary>
        public string GuideName { get; set; }

        /// <summary>
        /// 手机
        /// </summary>
        public string Phone { get; set; }

        /// <summary>
        /// 生日
        /// </summary>
        public DateTime? Birthday { get; set; }

        /// <summary>
        /// 带团时间
        /// </summary>
        public string TourTime { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        public string Remark { get; set; }


        /// <summary>
        /// 导游等级
        /// </summary>
        public EyouSoft.Model.EnumType.SourceStructure.GuideStar GuideStar { get; set; }


        /// <summary>
        /// 所属社别
        /// </summary>
        public string Belongs { get; set; }

        /// <summary>
        /// 操作员
        /// </summary>
        public int OperatorId { get; set; }

        /// <summary>
        /// 附件
        /// </summary>
        public IList<MFileInfo> FileList { get; set; }
    }



    /// <summary>
    /// 导游反馈
    /// </summary>
    public class MGuideFanKui
    {

        /// <summary>
        /// 编号
        /// </summary>
        public int Id { get; set; }


        /// <summary>
        /// 导游编号
        /// </summary>
        public string GuideId { get; set; }

        /// <summary>
        /// 反馈类型
        /// </summary>
        public EyouSoft.Model.EnumType.SourceStructure.FanKuiType FanKuiType { get; set; }

        /// <summary>
        /// 反馈时间
        /// </summary>
        public DateTime? FanKuiTime { get; set; }

        /// <summary>
        /// 反馈内容
        /// </summary>
        public string FanKuiRemark { get; set; }
    }


    /// <summary>
    /// 导游分页显示信息
    /// </summary>
    public class MPageGuide : MGuideSupplier
    {

        /// <summary>
        /// 省份编号
        /// </summary>
        public string ProvinceName { get; set; }

        /// <summary>
        /// 城市编号
        /// </summary>
        public string CityName { get; set; }


        /// <summary>
        /// 反馈数量
        /// </summary>
        public int FanKuiNum { get; set; }

    }
}
