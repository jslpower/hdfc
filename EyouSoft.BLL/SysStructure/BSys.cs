using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EyouSoft.BLL.SysStructure
{
    /// <summary>
    /// 系统管理(WEBMASTER)业务逻辑类
    /// </summary>
    public class BSys
    {
        private readonly EyouSoft.IDAL.SysStructure.ISys dal = EyouSoft.Component.Factory.ComponentFactory.CreateDAL<EyouSoft.IDAL.SysStructure.ISys>();

        #region constructure
        /// <summary>
        /// default constructor
        /// </summary>
        public BSys() { }
        #endregion

        #region public members
        /// <summary>
        /// 创建子系统
        /// </summary>
        /// <param name="info">系统信息业务实体</param>
        /// <returns></returns>
        public int CreateSys(EyouSoft.Model.SysStructure.MSysInfo info)
        {
            if (info == null) return 0;
            if (info.Password == null || string.IsNullOrEmpty(info.Password.NoEncryptPassword)) return -1;

            info.SysId = 0;
            info.CompanyId = 0;
            info.UserId = 0;
            info.IssueTime = DateTime.Now;

            int dalRetCode = dal.CreateSys(info);

            if (dalRetCode == 1)
            {
                
            }

            return dalRetCode;
        }

        /// <summary>
        /// 设置系统域名
        /// </summary>
        /// <param name="sysId">系统编号</param>
        /// <param name="domains">域名信息集合</param>
        /// <returns></returns>
        public int SetSysDomains(int sysId, IList<EyouSoft.Model.SysStructure.SystemDomain> domains)
        {
            if (sysId < 1) return 0;
            if (domains == null || domains.Count < 1) return -1;

            IList<string> notVerifyDomains = new List<string>();
            IList<string> existsDomains = new List<string>();

            foreach (var item in domains)
            {
                if (item != null && !string.IsNullOrEmpty(item.Domain))
                {
                    item.Domain = item.Domain.Trim().ToLower();
                    notVerifyDomains.Add(item.Domain);
                }
                else
                {
                    existsDomains.Add("null or empty!");
                }
            }

            if (existsDomains != null && existsDomains.Count > 0) return -2;

            existsDomains = IsExistsDomains(sysId, notVerifyDomains);

            if (existsDomains != null && existsDomains.Count > 0) return -2;

            if (dal.SetSysDomains(sysId, domains) == 1)
            {
                EyouSoft.Cache.Facade.EyouSoftCache.Remove(EyouSoft.Cache.Tag.TagName.SysDomains);
                return 1;
            }

            return -2;
        }

        /// <summary>
        /// 判断域名是否重复，返回重复的域名信息集合
        /// </summary>
        /// <param name="sysId">系统编号</param>
        /// <param name="domains">域名集合</param>
        /// <returns></returns>
        public IList<string> IsExistsDomains(int sysId, IList<string> domains)
        {
            if (sysId < 1 || domains == null || domains.Count < 1) return null;

            IList<string> notExistsDomains = new List<string>();
            IList<string> existsDomains = new List<string>();

            foreach (var s in domains)
            {
                string s1 = s;
                if (s1 != null) s1 = s1.Trim().ToLower();

                if (string.IsNullOrEmpty(s1))
                {
                    existsDomains.Add(null);
                    continue;
                }

                if (s1.IndexOf("http://") > -1)
                {
                    existsDomains.Add(s1);
                    continue;
                }

                if (notExistsDomains.IndexOf(s1) > -1)
                {
                    existsDomains.Add(s1);
                }
                else
                {
                    notExistsDomains.Add(s1);
                }
            }

            if (existsDomains != null && existsDomains.Count > 0) return existsDomains;

            return dal.IsExistsDomains(sysId, domains);
        }

        /// <summary>
        /// 设置子系统一级栏目、二级栏目、明细权限
        /// </summary>
        /// <param name="sysId">子系统编号</param>
        /// <param name="privs">权限信息</param>
        /// <returns></returns>
        public int SetComPrivs(int sysId, EyouSoft.Model.SysStructure.MComPrivsInfo privs)
        {
            if (sysId<1) return -1;

            return dal.SetComPrivs(sysId, privs);
        }

        /// <summary>
        /// 设置角色权限为子系统开通的所有权限
        /// </summary>
        /// <param name="roleId">角色编号</param>
        /// <param name="sysId">系统编号</param>
        /// <returns></returns>
        public int SetRoleBySysPrivs(int roleId, int sysId)
        {
            return dal.SetRoleBySysPrivs(roleId, sysId);
        }

        /// <summary>
        /// 设置用户权限为子系统开通的所有权限
        /// </summary>
        /// <param name="userId">用户编号</param>
        /// <param name="sysId">系统编号</param>
        /// <returns></returns>
        public int SetUserBySysPrivs(int userId, int sysId)
        {
            return dal.SetUserBySysPrivs(userId, sysId);
        }

        /// <summary>
        /// 设置用户角色
        /// </summary>
        /// <param name="userId">用户编号</param>
        /// <param name="roleId">角色编号</param>
        /// <returns></returns>
        public bool SetUserRole(int userId, int roleId)
        {
            if (userId < 1 || roleId < 1) return false;
            return dal.SetUserRole(userId, roleId);
        }

        /// <summary>
        /// 获取子系统信息集合
        /// </summary>
        /// <param name="pageSize">每页显示记录数</param>
        /// <param name="pageIndex">当前页索引</param>        
        /// <param name="recordCount">总记录数</param>
        /// <param name="searchInfo">查询信息</param>
        /// <returns></returns>
        public IList<EyouSoft.Model.SysStructure.MSysInfo> GetSyss(int pageSize, int pageIndex, ref int recordCount, EyouSoft.Model.SysStructure.MSysSearchInfo searchInfo)
        {
            var items = dal.GetSyss(pageSize, pageIndex, ref recordCount, searchInfo);

            return items;
        }

        /// <summary>
        /// 获取子系统信息
        /// </summary>
        /// <param name="sysId">子系统编号</param>
        /// <returns></returns>
        public EyouSoft.Model.SysStructure.MSysInfo GetSysInfo(int sysId)
        {
            return dal.GetSysInfo(sysId);
        }

        /// <summary>
        /// 获取一级栏目信息集合
        /// </summary>
        /// <returns></returns>
        public IList<EyouSoft.Model.SysStructure.MSysMenu1Info> GetPrivs1()
        {
            return dal.GetPrivs1();
        }

        /// <summary>
        /// 获取明细权限信息集合
        /// </summary>
        /// <param name="privs2Id">二级栏目编号</param>
        /// <returns></returns>
        public IList<EyouSoft.Model.SysStructure.MSysPrivsInfo> GetPrivs3(int privs2Id)
        {
            return dal.GetPrivs3(privs2Id);
        }

        /// <summary>
        /// set webmaster pwd
        /// </summary>
        /// <param name="webmasterId">webmaster id</param>
        /// <param name="username">webmaster username</param>
        /// <param name="pwd">webmaster pwd info</param>
        /// <returns></returns>
        public bool SetWebmasterPwd(int webmasterId, string username, EyouSoft.Model.CompanyStructure.PassWord pwd)
        {
            if (webmasterId < 1 || string.IsNullOrEmpty(username) || pwd == null || string.IsNullOrEmpty(pwd.NoEncryptPassword)) return false;

            return dal.SetWebmasterPwd(webmasterId, username, pwd);
        }

        /// <summary>
        /// 获取系统域名信息集合
        /// </summary>
        /// <param name="sysId">系统编号</param>
        /// <returns></returns>
        public IList<EyouSoft.Model.SysStructure.SystemDomain> GetDomains(int sysId)
        {
            if (sysId < 1) return null;

            return dal.GetDomains(sysId);
        }

        /// <summary>
        /// 获取子系统角色(管理员)编号
        /// </summary>
        /// <param name="companyId">公司编号</param>
        /// <returns></returns>
        public int GetSysRoleId(int companyId)
        {
            if (companyId<1) return -1;

            return dal.GetSysRoleId(companyId);
        }

        /// <summary>
        /// 基础权限管理-写入一级栏目
        /// </summary>
        /// <param name="info">一级栏目信息业务实体</param>
        /// <returns></returns>
        public int InsertPrivs1(EyouSoft.Model.SysStructure.MSysMenu1Info info)
        {
            if (info == null || string.IsNullOrEmpty(info.Name)) return 0;
            info.Name = info.Name.Trim();
            if (string.IsNullOrEmpty(info.Name)) return 0;
            if (dal.IsExistsPrivs1Name(info.Name)) return 0;

            return dal.InsertPrivs1(info);
        }

        /// <summary>
        /// 基础权限管理-写入二级栏目
        /// </summary>
        /// <param name="info">二级栏目信息业务实体</param>
        /// <returns></returns>
        public int InsertPrivs2(EyouSoft.Model.SysStructure.MSysMenu2Info info)
        {
            if (info == null || string.IsNullOrEmpty(info.Name) || info.FirstId < 1 || string.IsNullOrEmpty(info.Url)) return 0;
            info.Name = info.Name.Trim();
            if (string.IsNullOrEmpty(info.Name)) return 0;
            if (dal.IsExistsPrivs2Name(info.FirstId, info.Name)) return 0;

            int menu2Id = dal.InsertPrivs2(info);

            return 0;
        }

        /// <summary>
        /// 基础权限管理-写入明细权限
        /// </summary>
        /// <param name="info">权限信息业务实体</param>
        /// <returns></returns>
        public int InsertPrivs3(EyouSoft.Model.SysStructure.MSysPrivsInfo info)
        {
            if (info == null || string.IsNullOrEmpty(info.Name) || info.SecondId < 1) return 0;
            info.Name = info.Name.Trim();
            if (string.IsNullOrEmpty(info.Name)) return 0;
            if (dal.IsExistsPrivs3Name(info.SecondId, info.Name)) return 0;
            if (info.PrivsType != EyouSoft.Model.EnumType.SysStructure.PrivsType.其它 && dal.IsExistsPrivs3Type(info.SecondId, info.PrivsType)) return 0;

            int privsId = dal.InsertPrivs3(info);

            return privsId;
        }

        /// <summary>
        /// 获取公司一级栏目、二级栏目、明细权限信息业务实体
        /// </summary>
        /// <param name="companyId">公司编号</param>
        /// <returns></returns>
        public EyouSoft.Model.SysStructure.MComPrivsInfo GetComPrivsInfo(int companyId)
        {
            return dal.GetComPrivsInfo(companyId);
        }

        /// <summary>
        /// 清除缓存
        /// </summary>
        /// <param name="cacheName">cache name</param>
        /// <returns></returns>
        public bool RemoveCache(string cacheName)
        {
            if (string.IsNullOrEmpty(cacheName)) return true;

            EyouSoft.Cache.Facade.EyouSoftCache.Remove(cacheName);

            return true;
        }
        #endregion
    }
}
