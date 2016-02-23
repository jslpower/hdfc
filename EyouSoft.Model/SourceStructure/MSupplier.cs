using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EyouSoft.Model.SourceStructure
{
    public class MSupplier
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
        /// 省份编号
        /// </summary>
        public int ProvinceId { get; set; }

        /// <summary>
        /// 城市编号
        /// </summary>
        public int CityId { get; set; }

        /// <summary>
        /// 供应商名称
        /// </summary>
        public string UnitName { get; set; }

        /// <summary>
        /// 供应商类型
        /// </summary>
        public EyouSoft.Model.EnumType.CompanyStructure.SupplierType SupplierType { get; set; }

        /// <summary>
        /// 许可证号
        /// </summary>
        public string LicenseKey { get; set; }

        /// <summary>
        /// 合作协议
        /// </summary>
        public string AgreementFile { get; set; }

        /// <summary>
        /// 地址
        /// </summary>
        public string UnitAddress { get; set; }

        /// <summary>
        /// 政策
        /// </summary>
        public string UnitPolicy { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        public string Remark { get; set; }

        /// <summary>
        /// 操作人
        /// </summary>
        public int OperatorId { get; set; }

        /// <summary>
        /// 操作时间
        /// </summary>
        public DateTime IssueTime { get; set; }


        /// <summary>
        /// 联系人信息
        /// </summary>
        public IList<MSupplierContact> ContactList { get; set; }

        /// <summary>
        /// 附件
        /// </summary>
        public IList<MFileInfo> FileList { get; set; }
    }


    /// <summary>
    /// 供应商联系人
    /// </summary>
    public class MSupplierContact
    {

        /// <summary>
        /// 联系人编号
        /// </summary>
        public string Id { get; set; }
        /// <summary>
        /// 联系人名称
        /// </summary>
        public string ContactName { get; set; }

        /// <summary>
        /// 职务
        /// </summary>
        public string JobTitle { get; set; }

        /// <summary>
        /// 传真
        /// </summary>
        public string ContactFax { get; set; }

        /// <summary>
        /// 联系电话
        /// </summary>
        public string ContactTel { get; set; }

        /// <summary>
        /// 联系人手机
        /// </summary>
        public string ContactMobile { get; set; }

        /// <summary>
        /// 联系QQ
        /// </summary>
        public string QQ { get; set; }

        /// <summary>
        /// 电子邮箱
        /// </summary>
        public string Email { get; set; }

        /// <summary>
        /// 生日
        /// </summary>
        public DateTime? Birthday { get; set; }


        /// <summary>
        /// 登陆信息
        /// </summary>
        public MSupplierContactLoginInfo LoginInfo { get; set; }

    }

    /// <summary>
    /// 供应商联系人登陆信息
    /// </summary>
    public class MSupplierContactLoginInfo
    {

        /// <summary>
        /// 用户编号
        /// </summary>
        public string UserId { get; set; }

        /// <summary>
        /// 登陆名
        /// </summary>
        public string UserName { get; set; }

        /// <summary>
        /// 登陆密码
        /// </summary>
        public EyouSoft.Model.CompanyStructure.PassWord PassWord { get; set; }

        /// <summary>
        /// 用户类型
        /// </summary>
        public EyouSoft.Model.EnumType.CompanyStructure.UserType UserType { get; set; }

    }



    /// <summary>
    /// 附件
    /// </summary>
    public class MFileInfo
    {
        /// <summary>
        /// 附件名称
        /// </summary>
        public string FileName { get; set; }

        /// <summary>
        /// 附件路径
        /// </summary>
        public string FilePath { get; set; }

        /// <summary>
        /// 附件类型
        /// </summary>
        public EyouSoft.Model.EnumType.SourceStructure.FileMode FileMode { get; set; }

    }

    /// <summary>
    /// 查询类
    /// </summary>
    public class MSupplierSearch
    {
        /// <summary>
        /// 省份编号
        /// </summary>
        public int? ProvinceId { get; set; }

        /// <summary>
        /// 城市编号
        /// </summary>
        public int? CityId { get; set; }

        /// <summary>
        /// 供应商名称
        /// </summary>
        public string UnitName { get; set; }

    }

    /// <summary>
    /// 分页实体
    /// </summary>
    public class MPageSupplier
    {

        /// <summary>
        /// 编号
        /// </summary>
        public string Id { get; set; }


        /// <summary>
        /// 省份
        /// </summary>
        public string ProvinceName { get; set; }

        /// <summary>
        /// 城市
        /// </summary>
        public string CityName { get; set; }

        /// <summary>
        /// 供应商名称
        /// </summary>
        public string UnitName { get; set; }

        /// <summary>
        /// 联系人信息
        /// </summary>
        public MSupplierContact ContactInfo { get; set; }


        /// <summary>
        /// 附件
        /// </summary>
        public IList<MFileInfo> FileList { get; set; }

    }




}
