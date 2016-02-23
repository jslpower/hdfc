using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EyouSoft.Model.SSOStructure
{
    #region 登录用户信息业务实体
    /// <summary>
    /// 登录用户信息业务实体
    /// </summary>
    [Serializable]
    public class MUserInfo
    {
        /// <summary>
        /// 系统编号
        /// </summary>
        public int SysId { get; set; }
        /// <summary>
        /// 系统公司编号
        /// </summary>
        public int CompanyId { get; set; }
        /// <summary>
        /// 系统公司名称
        /// </summary>
        public string CompanyName { get; set; }
        /// <summary>
        /// 用户编号
        /// </summary>
        public int UserId { get; set; }      
        /// <summary>
        /// 用户名
        /// </summary>
        public string Username { get; set; }
        /// <summary>
        /// 用户权限集合
        /// </summary>
        public int[] Privs { get; set; }

        /// <summary>
        /// 是否管理员
        /// </summary>
        public bool IsAdmin { get; set; }
        /// <summary>
        /// 部门编号
        /// </summary>
        public int DeptId { get; set; }
        /// <summary>
        /// 部门名称
        /// </summary>
        public string DeptName { get; set; }
        /// <summary>
        /// 兼管部门编号
        /// </summary>
        public int JGDeptId { get; set; }

        /// <summary>
        /// 最后登录Ip
        /// </summary>
        public string LastLoginIp { get; set; }

        /// <summary>
        /// 最后登录时间
        /// </summary>
        public DateTime? LastLoginTime { get; set; }
        /// <summary>
        /// 用户姓名
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// 联系电话
        /// </summary>
        public string Telephone { get; set; }
        /// <summary>
        /// 联系手机
        /// </summary>
        public string Mobile { get; set; }
        
        /// <summary>
        /// 是否启用
        /// </summary>
        public EnumType.CompanyStructure.UserStatus Status { get; set; }
        /// <summary>
        /// 登录时间
        /// </summary>
        public DateTime LoginTime { get; set; }
        /// <summary>
        /// 所属角色编号
        /// </summary>
        public int RoleId { get; set; }

        /// <summary>
        /// 联系传真
        /// </summary>
        public string Fax { get; set; }

        /// <summary>
        /// 用户在线状态
        /// </summary>
        public EnumType.CompanyStructure.UserOnlineStatus OnlineStatus { get; set; }

        /// <summary>
        /// 用户会话状态标识
        /// </summary>
        public string OnlineSessionId { get; set; }

        /// <summary>
        /// 用户类型
        /// </summary>
        public EnumType.CompanyStructure.UserType UserType { get; set; }

        /// <summary>
        /// 供应商编号（用户类型为地接、票务时才有用）
        /// </summary>
        public string SupplierCompanyId { get; set; }
    }
    #endregion
}
