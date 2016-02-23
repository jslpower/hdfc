using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EyouSoft.IDAL.SysStructure
{
    /// <summary>
    /// 系统管理(WEBMASTER)数据访问类接口
    /// </summary>
    public interface ISys
    {
        /// <summary>
        /// 创建子系统
        /// </summary>
        /// <param name="info">系统信息业务实体</param>
        /// <returns></returns>
        int CreateSys(EyouSoft.Model.SysStructure.MSysInfo info);
        /// <summary>
        /// 设置系统域名
        /// </summary>
        /// <param name="sysId">系统编号</param>
        /// <param name="domains">域名信息集合</param>
        /// <returns></returns>
        int SetSysDomains(int sysId, IList<EyouSoft.Model.SysStructure.SystemDomain> domains);
        /// <summary>
        /// 设置子系统一级栏目、二级栏目、明细权限
        /// </summary>
        /// <param name="sysId">子系统编号</param>
        /// <param name="privs">权限信息</param>
        /// <returns></returns>
        int SetComPrivs(int sysId, EyouSoft.Model.SysStructure.MComPrivsInfo privs);
        /// <summary>
        /// 设置角色权限为子系统开通的所有权限
        /// </summary>
        /// <param name="roleId">角色编号</param>
        /// <param name="sysId">系统编号</param>
        /// <returns></returns>
        int SetRoleBySysPrivs(int roleId, int sysId);
        /// <summary>
        /// 设置用户权限为子系统开通的所有权限
        /// </summary>
        /// <param name="userId">用户编号</param>
        /// <param name="sysId">系统编号</param>
        /// <returns></returns>
        int SetUserBySysPrivs(int userId, int sysId);
        /// <summary>
        /// 设置用户角色
        /// </summary>
        /// <param name="userId">用户编号</param>
        /// <param name="roleId">角色编号</param>
        /// <returns></returns>
        bool SetUserRole(int userId, int roleId);
        /// <summary>
        /// 获取子系统信息集合
        /// </summary>
        /// <param name="pageSize">每页显示记录数</param>
        /// <param name="pageIndex">当前页索引</param>        
        /// <param name="recordCount">总记录数</param>
        /// <param name="searchInfo">查询信息</param>
        /// <returns></returns>
        IList<EyouSoft.Model.SysStructure.MSysInfo> GetSyss(int pageSize, int pageIndex, ref int recordCount, EyouSoft.Model.SysStructure.MSysSearchInfo searchInfo);
        /// <summary>
        /// 获取子系统信息
        /// </summary>
        /// <param name="sysId">子系统编号</param>
        /// <returns></returns>
        EyouSoft.Model.SysStructure.MSysInfo GetSysInfo(int sysId);
        /// <summary>
        /// 获取一级栏目信息集合
        /// </summary>
        /// <returns></returns>
        IList<EyouSoft.Model.SysStructure.MSysMenu1Info> GetPrivs1();
        /// <summary>
        /// 获取明细权限信息集合
        /// </summary>
        /// <param name="privs2Id">二级栏目编号</param>
        /// <returns></returns>
        IList<EyouSoft.Model.SysStructure.MSysPrivsInfo> GetPrivs3(int privs2Id);
        /// <summary>
        /// set webmaster pwd
        /// </summary>
        /// <param name="webmasterId">webmaster id</param>
        /// <param name="username">webmaster username</param>
        /// <param name="pwd">webmaster pwd info</param>
        /// <returns></returns>
        bool SetWebmasterPwd(int webmasterId, string username, EyouSoft.Model.CompanyStructure.PassWord pwd);
        /// <summary>
        /// 获取系统域名信息集合
        /// </summary>
        /// <param name="sysId">系统编号</param>
        /// <returns></returns>
        IList<Model.SysStructure.SystemDomain> GetDomains(int sysId);
        /// <summary>
        /// 判断域名是否重复，返回重复的域名信息集合
        /// </summary>
        /// <param name="sysId">系统编号</param>
        /// <param name="domains">域名集合</param>
        /// <returns></returns>
        IList<string> IsExistsDomains(int sysId, IList<string> domains);
        /// <summary>
        /// 获取子系统角色(管理员)编号
        /// </summary>
        /// <param name="companyId">公司编号</param>
        /// <returns></returns>
        int GetSysRoleId(int companyId);
        /// <summary>
        /// 基础权限管理-写入一级栏目
        /// </summary>
        /// <param name="info">一级栏目信息业务实体</param>
        /// <returns></returns>
        int InsertPrivs1(EyouSoft.Model.SysStructure.MSysMenu1Info info);
        /// <summary>
        /// 基础权限管理-写入二级栏目
        /// </summary>
        /// <param name="info">二级栏目信息业务实体</param>
        /// <returns></returns>
        int InsertPrivs2(EyouSoft.Model.SysStructure.MSysMenu2Info info);
        /// <summary>
        /// 基础权限管理-写入明细权限
        /// </summary>
        /// <param name="info">权限信息业务实体</param>
        /// <returns></returns>
        int InsertPrivs3(EyouSoft.Model.SysStructure.MSysPrivsInfo info);
        /// <summary>
        /// 相同二级栏目下是否存在相同的权限类别
        /// </summary>
        /// <param name="privs2Id">二级栏目编号</param>
        /// <param name="privsType">权限类别</param>
        /// <returns></returns>
        bool IsExistsPrivs3Type(int privs2Id, EyouSoft.Model.EnumType.SysStructure.PrivsType privsType);
        /// <summary>
        ///  相同二级栏目下是否存在相同的权限名称
        /// </summary>
        /// <param name="privs2Id">二级栏目编号</param>
        /// <param name="privsName">权限名称</param>
        /// <returns></returns>
        bool IsExistsPrivs3Name(int privs2Id, string privsName);
        /// <summary>
        /// 相同一级栏目下是否存在相同的二级栏目名称
        /// </summary>
        /// <param name="privs1Id">一级栏目编号</param>
        /// <param name="menu2Name">二级栏目名称</param>
        /// <returns></returns>
        bool IsExistsPrivs2Name(int privs1Id, string menu2Name);
        /// <summary>
        /// 是否存在相同的一级栏目名称
        /// </summary>
        /// <param name="privs1Name">一级栏目名称</param>
        /// <returns></returns>
        bool IsExistsPrivs1Name(string privs1Name);
        /// <summary>
        /// 获取公司一级栏目、二级栏目、明细权限信息业务实体
        /// </summary>
        /// <param name="companyId">公司编号</param>
        /// <returns></returns>
        EyouSoft.Model.SysStructure.MComPrivsInfo GetComPrivsInfo(int companyId);
    }
}
