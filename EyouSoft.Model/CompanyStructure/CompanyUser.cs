using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EyouSoft.Model.CompanyStructure
{
    #region 用户帐号基本信息实体
    /// <summary>
    /// 用户帐号基本信息实体
    /// </summary>
    [Serializable]
    public class UserAccount
    {
        /// <summary>
        /// 用户编号
        /// </summary>
        public int ID { get; set; }
        /// <summary>
        /// 用户名
        /// </summary>
        public string UserName { get; set; }
        /// <summary>
        /// 用户密码(在应用层设置时,只需设置其NoEncryptPassword属性)
        /// </summary>
        public PassWord PassWordInfo { get; set; }
        /// <summary>
        /// 专线公司编号
        /// </summary>
        public int CompanyId { get; set; }
    }
    #endregion

    #region 用户信息实体
    /// <summary>
    /// 专线公司用户信息表
    /// </summary>
    public class CompanyUser : UserAccount
    {
        #region Model
        /// <summary>
        /// 部门名称
        /// </summary>
        public string DepartName
        {
            get;
            set;
        }
        /// <summary>
        /// 部门编号
        /// </summary>
        public int DepartId
        {
            get;
            set;
        }
        /// <summary>
        /// 监管部门编号
        /// </summary>
        public int SuperviseDepartId
        {
            get;
            set;
        }
        /// <summary>
        /// 监管部门名称
        /// </summary>
        public string SuperviseDepartName
        {
            get;
            set;
        }
        /// <summary>
        /// 联系人信息
        /// </summary>
        public ContactPersonInfo PersonInfo
        {
            get;
            set;
        }
        /// <summary>
        /// 上次登录IP
        /// </summary>
        public string LastLoginIP
        {
            get;
            set;
        }
        /// <summary>
        /// 上次登录时间
        /// </summary>
        public DateTime? LastLoginTime
        {
            get;
            set;
        }
        /// <summary>
        /// 权限组(角色)编号
        /// </summary>
        public int RoleID
        {
            get;
            set;
        }
        /// <summary>
        /// 权限集合(权限值以逗号隔开)
        /// </summary>
        public string PermissionList
        {
            get;
            set;
        }
        /// <summary>
        /// 是否删除
        /// </summary>
        public bool IsDeleted
        {
            get;
            set;
        }
        /// <summary>
        /// 用户状态
        /// </summary>
        public EnumType.CompanyStructure.UserStatus UserStatus
        {
            get;
            set;
        }
        /// <summary>
        /// 是否管理员
        /// </summary>
        public bool IsAdmin
        {
            get;
            set;
        }
        /// <summary>
        /// 操作时间 
        /// </summary>
        public DateTime IssueTime
        {
            get;
            set;
        }

        /// <summary>
        /// 用户类型
        /// </summary>
        public EnumType.CompanyStructure.UserType UserType { get; set; }

        /// <summary>
        /// 供应商编号（用户类型为地接、票务时才有用）
        /// </summary>
        public string SupplierCompanyId { get; set; }

        #endregion Model

    }
    #endregion

    #region 用户密码实体
    /// <summary>
    /// 密码实体
    /// </summary>
    [Serializable]
    public class PassWord
    {
        private readonly Toolkit.DataProtection.HashCrypto hashcrypto = new Toolkit.DataProtection.HashCrypto();
        /// <summary>
        /// MD5加密密码
        /// </summary>
        private string _md5Password = string.Empty;

        /// <summary>
        /// 明文密码
        /// </summary>
        private string _noEncryptPassword = string.Empty;

        /// <summary>
        /// 获取或设置未加密密码(只需要设置未加密密码即可)
        /// </summary>
        public string NoEncryptPassword
        {
            get
            {
                return _noEncryptPassword;
            }
            set
            {
                this._noEncryptPassword = value;
                this._md5Password = hashcrypto.MD5Encrypt(this._noEncryptPassword);
            }
        }
        /// <summary>
        /// 获取MD5加密密码(只需要设置未加密密码即可)
        /// </summary>
        public string MD5Password { get { return this._md5Password; } }

        /// <summary>
        /// 构造方法
        /// </summary>
        public PassWord() { }

        /// <summary>
        /// 构造方法
        /// </summary>
        /// <param name="noencryptpassword">未加密密码</param>
        public PassWord(string noencryptpassword)
        {
            this.NoEncryptPassword = noencryptpassword;
        }

        /// <summary>
        /// 设置所有密码(该方法只需在业务逻辑层使用)
        /// </summary>
        /// <param name="noencryptpassword">未加密密码</param>
        /// <param name="md5password">MD5加密密码</param>
        public void SetEncryptPassWord(string noencryptpassword, string md5password)
        {
            this._noEncryptPassword = noencryptpassword;
            this._md5Password = md5password;
        }
    }
    #endregion

    #region 联系人信息实体类
    /// <summary>
    /// 联系人信息实体类
    /// </summary>
    [Serializable]
    public class ContactPersonInfo
    {
        /// <summary>
        /// 姓名
        /// </summary>
        public string ContactName
        {
            get;
            set;
        }
        /// <summary>
        /// 性别
        /// </summary>
        public EnumType.CompanyStructure.Sex ContactSex
        {
            get;
            set;
        }
        /// <summary>
        /// 电话
        /// </summary>
        public string ContactTel
        {
            get;
            set;
        }
        /// <summary>
        /// 传真
        /// </summary>
        public string ContactFax
        {
            get;
            set;
        }
        /// <summary>
        /// 手机
        /// </summary>
        public string ContactMobile
        {
            get;
            set;
        }
        /// <summary>
        /// 邮箱
        /// </summary>
        public string ContactEmail
        {
            get;
            set;
        }
        /// <summary>
        /// QQ
        /// </summary>
        public string QQ
        {
            get;
            set;
        }
        /// <summary>
        /// MSN
        /// </summary>
        public string MSN
        {
            get;
            set;
        }
        /// <summary>
        /// 职务
        /// </summary>
        public string JobName
        {
            get;
            set;
        }
        /// <summary>
        /// 个人简介
        /// </summary>
        public string PeopProfile
        {
            get;
            set;
        }
        /// <summary>
        /// 个人备注
        /// </summary>
        public string Remark
        {
            get;
            set;
        }

        /// <summary>
        /// 生日
        /// </summary>
        public DateTime? Birthday { get; set; }

        /// <summary>
        /// 住址
        /// </summary>
        public string Address { get; set; }
    }
    #endregion

    #region 用户查询实体

    /// <summary>
    /// 用户查询实体
    /// </summary>
    public class QueryCompanyUser
    {
        /// <summary>
        /// 用户编号
        /// </summary>
        public int[] UserId { get; set; }

        /// <summary>
        /// 部门编号
        /// </summary>
        public int[] DepartId { get; set; }

        /// <summary>
        /// 用户名
        /// </summary>
        public string UserName { get; set; }

        /// <summary>
        /// 用户姓名
        /// </summary>
        public string ContactName { get; set; }

        /// <summary>
        /// 用户状态
        /// </summary>
        public EnumType.CompanyStructure.UserStatus[] UserStatus { get; set; }

        /// <summary>
        /// 是否包含删除的用户（传值 null 或者 false 都为false，传值为true 则结果集包含已经删除的用户）
        /// </summary>
        public bool? IsDelete { get; set; }

        /// <summary>
        /// 是否取管理员的用户
        /// </summary>
        public bool? IsAdmin { get; set; }

        /// <summary>
        /// 用户类型
        /// </summary>
        public EnumType.CompanyStructure.UserType? UserType { get; set; }
    }

    #endregion
}
