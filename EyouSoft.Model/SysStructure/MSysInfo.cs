using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EyouSoft.Model.SysStructure
{
    #region 系统信息业务实体
    /// <summary>
    /// 系统信息业务实体
    /// </summary>
    public class MSysInfo
    {
        public MSysInfo() { }

        /// <summary>
        /// 系统编号
        /// </summary>
        public int SysId { get; set; }
        /// <summary>
        /// 系统名称
        /// </summary>
        public string SysName { get; set; }
        /// <summary>
        /// 系统公司编号
        /// </summary>
        public int CompanyId { get; set; }
        /// <summary>
        /// 管理员账号编号
        /// </summary>
        public int UserId { get; set; }
        /// <summary>
        /// 管理员登录账号
        /// </summary>
        public string Username { get; set; }
        /// <summary>
        /// 管理员登录密码
        /// </summary>
        public EyouSoft.Model.CompanyStructure.PassWord Password { get; set; }
        /// <summary>
        /// 联系姓名
        /// </summary>
        public string FullName { get; set; }
        /// <summary>
        /// 联系电话
        /// </summary>
        public string Telephone { get; set; }
        /// <summary>
        /// 联系手机
        /// </summary>
        public string Mobile { get; set; }
        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime IssueTime { get; set; }
        /// <summary>
        /// 管理员在线状态
        /// </summary>
        public EyouSoft.Model.EnumType.CompanyStructure.UserOnlineStatus OnlineStatus { get; set; }

        /// <summary>
        /// 系统公司配置
        /// </summary>
        public CompanyStructure.CompanyFieldSetting CompanySetting { get; set; }
    }
    #endregion

    #region 系统查询信息业务实体
    /// <summary>
    /// 系统查询信息业务实体
    /// </summary>
    public class MSysSearchInfo
    {

    }
    #endregion

    #region 系统一级栏目信息业务实体
    /// <summary>
    /// 系统一级栏目信息业务实体
    /// </summary>
    public class MSysMenu1Info
    {
        /// <summary>
        /// 栏目编号
        /// </summary>
        public int MenuId { get; set; }
        /// <summary>
        /// 栏目名称
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// 二级栏目信息集合
        /// </summary>
        public IList<MSysMenu2Info> Menu2s { get; set; }
        /// <summary>
        /// 样式名称
        /// </summary>
        public string ClassName { get; set; }
    }
    #endregion

    #region 系统二级栏目信息业务实体
    /// <summary>
    /// 系统二级栏目信息业务实体
    /// </summary>
    [Serializable]
    public class MSysMenu2Info
    {
        /// <summary>
        /// 一级栏目编号
        /// </summary>
        public int FirstId { get; set; }
        /// <summary>
        /// 栏目编号
        /// </summary>
        public int MenuId { get; set; }
        /// <summary>
        /// 栏目名称
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// 链接URL
        /// </summary>
        public string Url { get; set; }
        /// <summary>
        /// 权限信息集合
        /// </summary>
        public IList<MSysPrivsInfo> Privs { get; set; }
    }
    #endregion

    #region 系统明细权限信息业务实体
    /// <summary>
    /// 系统明细权限信息业务实体
    /// </summary>
    [Serializable]
    public class MSysPrivsInfo
    {
        /// <summary>
        /// 二级栏目编号
        /// </summary>
        public int SecondId { get; set; }
        /// <summary>
        /// 权限编号
        /// </summary>
        public int PrivsId { get; set; }
        /// <summary>
        /// 权限名称
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// 权限类别
        /// </summary>
        public EyouSoft.Model.EnumType.SysStructure.PrivsType PrivsType { get; set; }
        /// <summary>
        /// 权限说明
        /// </summary>
        public string Remark { get; set; }
    }
    #endregion

    #region 子系统一级栏目、二级栏目、明细权限信息业务实体
    /// <summary>
    /// 子系统一级栏目、二级栏目、明细权限信息业务实体
    /// </summary>
    public class MComPrivsInfo
    {
        /// <summary>
        /// 一级栏目
        /// </summary>
        public int[] Privs1 { get; set; }
        /// <summary>
        /// 二级栏目
        /// </summary>
        public int[] Privs2 { get; set; }
        /// <summary>
        /// 明细权限
        /// </summary>
        public int[] Privs3 { get; set; }
    }
    #endregion
}
