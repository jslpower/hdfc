using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EyouSoft.Model.CRM
{
    #region 客户资料实体

    /// <summary>
    /// 客户资料实体
    /// </summary>
    public class MCustomer
    {
        #region 表属性

        /// <summary>
        /// 客户编号
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        /// 公司编号
        /// </summary>
        public int CompanyId { get; set; }

        /// <summary>
        /// 省份编号
        /// </summary>
        public int ProviceId { get; set; }

        /// <summary>
        /// 城市编号
        /// </summary>
        public int CityId { get; set; }
        /// <summary>
        /// 销售区域编号
        /// </summary>
        public int SaleAreadId { get; set; }
        /// <summary>
        /// 客户类型
        /// </summary>
        public EnumType.CustomerStructure.CustomerType CustomerType { get; set; }
        /// <summary>
        /// 客户单位名称
        /// </summary>
        public string CustomerName { get; set; }
        /// <summary>
        /// 许可证
        /// </summary>
        public string Licence { get; set; }
        /// <summary>
        /// 地址
        /// </summary>
        public string Address { get; set; }
        /// <summary>
        /// 邮编
        /// </summary>
        public string PostalCode { get; set; }
        /// <summary>
        /// 银行账号
        /// </summary>
        public string BankCode { get; set; }
        /// <summary>
        /// 主要联系人姓名
        /// </summary>
        public string ContactName { get; set; }
        /// <summary>
        /// 电话
        /// </summary>
        public string Phone { get; set; }
        /// <summary>
        /// 手机
        /// </summary>
        public string Mobile { get; set; }
        /// <summary>
        /// 传真
        /// </summary>
        public string Fax { get; set; }
        /// <summary>
        /// 备注
        /// </summary>
        public string Remark { get; set; }
        /// <summary>
        /// 操作人
        /// </summary>
        public int OperatorId { get; set; }
        /// <summary>
        /// 添加时间
        /// </summary>
        public DateTime IssueTime { get; set; }
        /// <summary>
        /// 信用等级
        /// </summary>
        public int RatingId { get; set; }

        /// <summary>
        /// 客户评级
        /// </summary>
        public EyouSoft.Model.EnumType.CustomerStructure.CustomerRating CustomerRating { get; set; }

        #endregion

        #region 扩展属性

        /// <summary>
        /// 省份名称
        /// </summary>
        public string ProvinceName { get; set; }

        /// <summary>
        /// 城市名称
        /// </summary>
        public string CityName { get; set; }
        /// <summary>
        /// 信用等级名称
        /// </summary>
        public string RatingName{ get; set; }

        /// <summary>
        /// 联系人信息
        /// </summary>
        public IList<MCustomerContact> Contact { get; set; }

        #endregion
    }

    #endregion

    #region 客户联系人实体

    /// <summary>
    /// 客户联系人实体
    /// </summary>
    public class MCustomerContact
    {
        /// <summary>
        /// 编号
        /// </summary>
        public int Id { get; set; }
        /// <summary>
        /// 客户编号
        /// </summary>
        public string CustomerId { get; set; }
        /// <summary>
        /// 公司编号
        /// </summary>
        public int CompanyId { get; set; }
        /// <summary>
        /// 联系人姓名
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// 性别
        /// </summary>
        public EnumType.CompanyStructure.Sex Sex { get; set; }
        /// <summary>
        /// 部门
        /// </summary>
        public string DepartmentName { get; set; }
        /// <summary>
        /// 职务
        /// </summary>
        public string Job { get; set; }
        /// <summary>
        /// 传真
        /// </summary>
        public string Fax { get; set; }
        /// <summary>
        /// 电话
        /// </summary>
        public string Tel { get; set; }
        /// <summary>
        /// 手机
        /// </summary>
        public string Mobile { get; set; }
        /// <summary>
        /// QQ
        /// </summary>
        public string Qq { get; set; }
        /// <summary>
        /// 生日
        /// </summary>
        public DateTime? BirthDay { get; set; }
        /// <summary>
        /// Email
        /// </summary>
        public string Email { get; set; }
        /// <summary>
        /// 备注
        /// </summary>
        public string Remark { get; set; }
    }

    #endregion

    #region 客户资料查询实体

    /// <summary>
    /// 客户资料查询实体
    /// </summary>
    public class MSearchCustomer
    {
        /// <summary>
        /// 省份编号
        /// </summary>
        public int[] ProvinceId { get; set; }

        /// <summary>
        /// 城市编号
        /// </summary>
        public int[] CityId { get; set; }

        /// <summary>
        /// 客户单位名称
        /// </summary>
        public string CustomerName { get; set; }

        /// <summary>
        /// 联系人姓名（查询主要联系人和其他所有联系人）
        /// </summary>
        public string ContactName { get; set; }

        /// <summary>
        /// 联系人电话（查询主要联系人和其他所有联系人）
        /// </summary>
        public string Phone { get; set; }

        /// <summary>
        /// 地址
        /// </summary>
        public string Address { get; set; }

        /// <summary>
        /// 手机号码
        /// </summary>
        public string Mobile { get; set; }
        /// <summary>
        /// 信用等级
        /// </summary>
        public int RatingId { get; set; }
        /// <summary>
        /// 客户评级
        /// </summary>
        public EyouSoft.Model.EnumType.CustomerStructure.CustomerRating? CustomerRating { get; set; }
    }

    #endregion

    #region 客户关怀实体

    /// <summary>
    /// 客户关怀实体
    /// </summary>
    public class MCustomerCare
    {
        #region 表属性

        /// <summary>
        /// 编号
        /// </summary>
        public int CareId { get; set; }
        /// <summary>
        /// 公司编号
        /// </summary>
        public int CompanyId { get; set; }
        /// <summary>
        /// 被访客户编号
        /// </summary>
        public string CustomerId { get; set; }
        /// <summary>
        /// 拜访人
        /// </summary>
        public string VisitName { get; set; }
        /// <summary>
        /// 拜访日期
        /// </summary>
        public DateTime VisitTime { get; set; }
        /// <summary>
        /// 支出费用
        /// </summary>
        public decimal PayMoney { get; set; }
        /// <summary>
        /// 支出理由
        /// </summary>
        public string PayReason { get; set; }
        /// <summary>
        /// 客户喜好
        /// </summary>
        public string CustomerHobby { get; set; }
        /// <summary>
        /// 操作人
        /// </summary>
        public int OperatorId { get; set; }
        /// <summary>
        /// 添加时间
        /// </summary>
        public DateTime IssueTime { get; set; }

        #endregion

        #region 扩展属性

        /// <summary>
        /// 被访客户名称
        /// </summary>
        public string CustomerName { get; set; }

        /// <summary>
        /// 被访客户联系人信息(不含主要联系人)
        /// </summary>
        public IList<MCustomerContact> Contact { get; set; }

        #endregion
    }

    #endregion

    #region 客户关怀实体查询实体

    /// <summary>
    /// 客户关怀实体查询实体
    /// </summary>
    public class MSearchCustomerCare
    {
        /// <summary>
        /// 拜访人
        /// </summary>
        public string VisitName { get; set; }

        /// <summary>
        /// 被访客户名称
        /// </summary>
        public string CustomerName { get; set; }

        /// <summary>
        /// 拜访日期开始
        /// </summary>
        public DateTime? StartVisitTime { get; set; }

        /// <summary>
        /// 拜访日期结束
        /// </summary>
        public DateTime? EndVisitTime { get; set; }
    }

    #endregion

    #region 生日提醒实体

    /// <summary>
    /// 生日提醒基类
    /// </summary>
    public class MBirthdayBase
    {
        /// <summary>
        /// 姓名
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 生日
        /// </summary>
        public DateTime Birthday { get; set; }

        /// <summary>
        /// 手机号码
        /// </summary>
        public string Mobile { get; set; }
    }

    #region 生日礼物实体

    /// <summary>
    /// 生日礼物实体基类
    /// </summary>
    public class MBirthdayGiftBase
    {
        /// <summary>
        /// 生日礼物明细编号
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        /// 公司编号
        /// </summary>
        public int CompanyId { get; set; }

        /// <summary>
        /// 赠送生日礼物时间
        /// </summary>
        public DateTime SendGiftTime { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        public string Remark { get; set; }

        /// <summary>
        /// 操作人
        /// </summary>
        public int OperatorId { get; set; }

        /// <summary>
        /// 记录添加时间
        /// </summary>
        public DateTime IssueTime { get; set; }
    }

    /// <summary>
    /// 用户生日礼物实体
    /// </summary>
    public class MUserBirthdayGift : MBirthdayGiftBase
    {
        /// <summary>
        /// 用户编号
        /// </summary>
        public int UserId { get; set; }
    }

    /// <summary>
    /// 导游生日礼物实体
    /// </summary>
    public class MGuideBirthdayGift : MBirthdayGiftBase
    {
        /// <summary>
        /// 导游编号
        /// </summary>
        public string GuideId { get; set; }
    }

    /// <summary>
    /// 组团社联系人生日礼物实体
    /// </summary>
    public class MContactBirthdayGift : MBirthdayGiftBase
    {
        /// <summary>
        /// 组团社联系人编号
        /// </summary>
        public int ContactId { get; set; }
    }

    /// <summary>
    /// 游客生日礼物实体
    /// </summary>
    public class MTravellerBirthdayGift : MBirthdayGiftBase
    {
        /// <summary>
        /// 游客编号
        /// </summary>
        public string TravellerId { get; set; }
    }

    /// <summary>
    /// 地接社联系人生日礼物实体
    /// </summary>
    public class MGroundBirthdayGift : MBirthdayGiftBase
    {
        /// <summary>
        /// 地接社联系人编号
        /// </summary>
        public int GroundId { get; set; }
    }

    /// <summary>
    /// 景点联系人生日礼物实体
    /// </summary>
    public class MSpotBirthdayGift : MBirthdayGiftBase
    {
        /// <summary>
        /// 景点联系人编号
        /// </summary>
        public int SpotId { get; set; }
    }

    #endregion

    #region 员工生日提醒实体

    /// <summary>
    /// 员工生日提醒实体
    /// </summary>
    public class MUserBirthday : MBirthdayBase
    {
        /// <summary>
        /// 用户编号
        /// </summary>
        public int UserId { get; set; }

        /// <summary>
        /// 地址
        /// </summary>
        public string Address { get; set; }

        /// <summary>
        /// 电话
        /// </summary>
        public string Tel { get; set; }

        /// <summary>
        /// QQ
        /// </summary>
        public string Qq { get; set; }
    }

    #endregion

    #region 导游生日提醒实体

    /// <summary>
    /// 导游生日提醒实体
    /// </summary>
    public class MGuideBirthday : MBirthdayBase
    {
        /// <summary>
        /// 导游编号
        /// </summary>
        public string GuideId { get; set; }
    }

    #endregion

    #region 组团社联系人生日提醒实体

    /// <summary>
    /// 组团社联系人生日提醒实体
    /// </summary>
    public class MContactBirthday : MBirthdayBase
    {
        /// <summary>
        /// 组团社联系人编号
        /// </summary>
        public int ContactId { get; set; }

        /// <summary>
        /// 单位名称
        /// </summary>
        public string UnitName { get; set; }

        /// <summary>
        /// 地址
        /// </summary>
        public string Address { get; set; }

        /// <summary>
        /// 电话
        /// </summary>
        public string Tel { get; set; }

        /// <summary>
        /// QQ
        /// </summary>
        public string Qq { get; set; }
    }

    #endregion

    #region 游客生日提醒实体

    /// <summary>
    /// 游客生日提醒实体
    /// </summary>
    public class MTravellerBirthday : MBirthdayBase
    {
        /// <summary>
        /// 游客编号
        /// </summary>
        public string TravellerId { get; set; }

        /// <summary>
        /// 团号
        /// </summary>
        public string TourNo { get; set; }

        /// <summary>
        /// 性别
        /// </summary>
        public EnumType.CompanyStructure.Sex Sex { get; set; }

        /// <summary>
        /// 身份证
        /// </summary>
        public string CardNumber { get; set; }

        /// <summary>
        /// 年龄
        /// </summary>
        public int Age
        {
            get
            {
                return this.GetAgeByBirthday(this.Birthday);
            }
        }

        /// <summary>
        /// 根据出生年月计算年龄
        /// </summary>
        /// <param name="birthday">出生年月</param>
        /// <returns></returns>
        private int GetAgeByBirthday(DateTime birthday)
        {
            if (birthday == DateTime.MinValue || birthday == DateTime.MaxValue || birthday >= DateTime.Now) return 0;
            DateTime dnow = DateTime.Now;

            int year = dnow.Year - birthday.Year;
            if (dnow.Month < birthday.Month) year--;

            return year;
        }
    }

    #endregion

    #region 地接社联系人生日提醒实体

    /// <summary>
    /// 地接社联系人生日提醒实体
    /// </summary>
    public class MGroundBirthday : MBirthdayBase
    {
        /// <summary>
        /// 地接社联系人编号
        /// </summary>
        public int GroundId { get; set; }

        /// <summary>
        /// 地接社名称
        /// </summary>
        public string GroundName { get; set; }
    }

    #endregion

    #region 景点联系人生日提醒实体

    /// <summary>
    /// 景点联系人生日提醒实体
    /// </summary>
    public class MSpotBirthday : MBirthdayBase
    {
        /// <summary>
        /// 景点联系人编号
        /// </summary>
        public int SpotId { get; set; }

        /// <summary>
        /// 景点名称
        /// </summary>
        public string SpotName { get; set; }
    }

    #endregion

    #region 生日提醒查询实体

    /// <summary>
    /// 生日提醒查询实体
    /// </summary>
    public class MSearchBirthday
    {
        /// <summary>
        /// 姓名
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 生日开始时间
        /// </summary>
        public DateTime? StartBirthday { get; set; }

        /// <summary>
        /// 生日结束时间
        /// </summary>
        public DateTime? EndBirthday { get; set; }
    }

    #endregion

    #region 生日弹窗提醒实体

    /// <summary>
    /// 生日弹窗提醒实体
    /// </summary>
    public class MBirthdayWindow : MBirthdayBase
    {
        /// <summary>
        /// 类型(员工，游客……)
        /// </summary>
        public EnumType.CustomerStructure.BirthdayGiftType PeopleType { get; set; }
    }

    #endregion

    #endregion
}
