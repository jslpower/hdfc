using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EyouSoft.Model.CompanyStructure
{
    /// <summary>
    /// 角色管理信息实体
    /// </summary>
    public class SysRoleManage
    {
        /// <summary>
        /// 角色编号
        /// </summary>
        public int Id { get; set; }
        /// <summary>
        /// 角色名称
        /// </summary>
        public string RoleName { get; set; }
        /// <summary>
        /// 角色权限集合(权限值以逗号隔开)
        /// </summary>
        public string RoleChilds { get; set; }
        /// <summary>
        /// 公司编号
        /// </summary>
        public int CompanyId { get; set; }
    }
}
