using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EyouSoft.BLL.CompanyStructure
{
    /// <summary>
    /// 用户信息
    /// </summary>
    public class CompanyUser : BLLBase
    {
        private readonly IDAL.CompanyStructure.ICompanyUser _dal =
            Component.Factory.ComponentFactory.CreateDAL<IDAL.CompanyStructure.ICompanyUser>();
        private readonly SysHandleLogs _handleLogsBll = new SysHandleLogs();

        #region private members
        /// <summary>
        /// 移除公司部门缓存
        /// </summary>
        /// <param name="companyId">公司编号</param>
        private void RemoveDepartmentCache(int companyId)
        {
            var departmentbll = new Department();
            departmentbll.RemoveDepartmentCache(companyId);
        }

        /// <summary>
        /// 添加日志记录
        /// </summary>
        /// <param name="actionName">操作名称</param>
        /// <param name="id">用户编号</param>
        /// <param name="userName">用户名称</param>
        /// <returns></returns>
        private Model.CompanyStructure.SysHandleLogs AddLogs(string actionName, string id, string userName)
        {
            var model = new Model.CompanyStructure.SysHandleLogs
                {
                    ModuleId = Model.EnumType.PrivsStructure.Privs2.系统设置_组织机构,
                    EventCode = Model.CompanyStructure.SysHandleLogsNO.EventCode,
                    EventMessage =
                        DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "{0}在"
                        + Model.EnumType.PrivsStructure.Privs2.系统设置_组织机构 + actionName + "了部门人员数据，编号为" + id + "，用户名为"
                        + userName,
                    EventTitle = actionName + Model.EnumType.PrivsStructure.Privs2.系统设置_组织机构 + "部门人员数据"
                };

            return model;
        }
        #endregion

        #region public members
        /// <summary>
        /// 判断E-MAIL是否已存在
        /// </summary>
        /// <param name="email">email地址</param>
        /// <param name="userId">当前修改Email的用户ID</param>
        /// <returns></returns>
        public bool IsExistsEmail(string email, int userId)
        {
            if (string.IsNullOrEmpty(email)) return false;

            return this._dal.IsExistsEmail(email, userId);
        }

        /// <summary>
        /// 判断用户名是否已存在
        /// </summary>
        /// <param name="id">要排除的用户编号</param>
        /// <param name="userName">用户名</param>
        /// <param name="companyId">当前公司编号</param>
        /// <returns></returns>
        public bool IsExists(int id, string userName, int companyId)
        {
            if (string.IsNullOrEmpty(userName) || companyId <= 0) return false;
            return this._dal.IsExists(id, userName, companyId);
        }

        /// <summary>
        /// 移除用户(即虚拟删除用户)
        /// </summary>
        /// <param name="companyId">公司编号</param>
        /// <param name="userIdList">用户ID列表</param>
        /// <returns></returns>
        public bool Remove(int companyId, params int[] userIdList)
        {
            if (userIdList == null || userIdList.Length <= 0) return false;

            bool dalResult = this._dal.Remove(userIdList);

            if (dalResult)
            {
                this.RemoveDepartmentCache(companyId);
                this._handleLogsBll.Add(AddLogs("删除", this.GetIdsByArr(userIdList), string.Empty));
            }

            return dalResult;
        }

        /// <summary>
        /// 添加用户信息
        /// </summary>
        /// <param name="model">用户信息实体</param>
        /// <returns>
        /// 1：成功；
        /// 0：参数错误；
        /// -1：子账号数量已满，请联系易诺客服；
        /// -2：添加失败；
        /// </returns>
        public int Add(Model.CompanyStructure.CompanyUser model)
        {
            if (model == null || model.PassWordInfo == null || model.PersonInfo == null) return 0;

            if (!_dal.IsAddUser(model.CompanyId))
            {
                return -1;
            }

            bool dalResult = this._dal.Add(model);

            if (dalResult)
            {
                this.RemoveDepartmentCache(model.CompanyId);
                this._handleLogsBll.Add(AddLogs("添加", model.ID.ToString(), model.UserName));
            }

            return dalResult ? 1 : -2;
        }

        /// <summary>
        /// 修改用户信息
        /// </summary>
        /// <param name="model">用户信息实体</param>
        /// <returns>true:成功 false:失败</returns>
        public bool Update(Model.CompanyStructure.CompanyUser model)
        {
            if (model == null || model.PassWordInfo == null || model.PersonInfo == null || model.ID <= 0) return false;

            bool dalResult = this._dal.Update(model);

            if (dalResult)
            {
                this.RemoveDepartmentCache(model.CompanyId);

                //修改密码
                UpdatePassWord(model.ID, model.PassWordInfo);

                this._handleLogsBll.Add(AddLogs("修改", model.ID.ToString(), model.UserName));
            }

            return dalResult;
        }

        /// <summary>
        /// 根据用户编号获取用户信息
        /// </summary>
        /// <param name="userId">用户编号</param>
        /// <returns>用户实体</returns>
        public Model.CompanyStructure.CompanyUser GetUserInfo(int userId)
        {
            if (userId <= 0) return null;

            return this._dal.GetUserInfo(userId);
        }

        /// <summary>
        /// 根据用户名及密码获取用户信息实体
        /// </summary>
        /// <param name="userName">用户名</param>
        /// <param name="pwd">MD5密码</param>
        /// <returns>用户信息实体</returns>
        public Model.CompanyStructure.CompanyUser GetUserInfo(string userName, string pwd)
        {
            if (string.IsNullOrEmpty(userName) || string.IsNullOrEmpty(pwd)) return null;

            return this._dal.GetUserInfo(userName, pwd);
        }

        /// <summary>
        /// 修改密码
        /// </summary>
        /// <param name="id">用户ID</param>
        /// <param name="password">密码实体类</param>
        /// <returns></returns>
        public bool UpdatePassWord(int id, Model.CompanyStructure.PassWord password)
        {
            if (id <= 0 || password == null || string.IsNullOrEmpty(password.NoEncryptPassword)
                || string.IsNullOrEmpty(password.MD5Password)) return false;

            return this._dal.UpdatePassWord(id, password);
        }

        /// <summary>
        /// 获得管理员实体信息
        /// </summary>
        /// <param name="companyId">公司ID</param>
        /// <returns></returns>
        public Model.CompanyStructure.CompanyUser GetAdminModel(int companyId)
        {
            if (companyId <= 0) return null;

            return this._dal.GetAdminModel(companyId);
        }

        /// <summary>
        /// 获取指定公司下的所有帐号用户详细信息列表
        /// </summary>
        /// <param name="companyId">公司ID</param>
        /// <param name="pageSize">分页大小</param>
        /// <param name="pageIndex">当前页</param>
        /// <param name="recordCount">总的记录数</param>
        /// <param name="model">查询实体</param>
        /// <returns></returns>
        public IList<Model.CompanyStructure.CompanyUser> GetList(int companyId, int pageSize, int pageIndex, ref int recordCount
            , Model.CompanyStructure.QueryCompanyUser model)
        {
            if (companyId <= 0 || pageSize <= 0 || pageIndex <= 0) return null;

            return _dal.GetList(companyId, pageSize, pageIndex, ref recordCount, model);
        }

        /// <summary>
        /// 获取指定公司的所有用户信息
        /// </summary>
        /// <param name="companyId">公司编号</param>
        /// <param name="model">查询实体</param>
        /// <returns></returns>
        public IList<Model.CompanyStructure.CompanyUser> GetList(int companyId, Model.CompanyStructure.QueryCompanyUser model)
        {
            if (companyId <= 0) return null;

            return _dal.GetList(companyId, model);
        }

        /// <summary>
        /// 设置用户启用状态
        /// </summary>
        /// <param name="id">用户编号</param>
        /// <param name="status">用户状态</param>
        /// <returns>true:成功 false:失败</returns>
        public bool SetEnable(int id, Model.EnumType.CompanyStructure.UserStatus status)
        {
            if (id <= 0) return false;

            return this._dal.SetEnable(id, status);
        }

        /// <summary>
        /// 设置用户权限
        /// </summary>
        /// <param name="userId">用户编号</param>
        /// <param name="roleId">角色编号</param>
        /// <param name="permissionList">权限集合</param>
        /// <returns>是否成功</returns>
        public bool SetPermission(int userId, int roleId, params string[] permissionList)
        {
            if (userId <= 0 || permissionList == null) return false;
            return this._dal.SetPermission(userId, roleId, permissionList);
        }

        /// <summary>
        /// 简单修改用户基本信息
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public bool SimpleUpdate(Model.CompanyStructure.CompanyUser model)
        {
            if (model == null || model.PassWordInfo == null || model.PersonInfo == null || model.ID <= 0) return false;

            bool dalResult = this._dal.SimpleUpdate(model);

            if (dalResult)
            {
                this.RemoveDepartmentCache(model.CompanyId);

                this._handleLogsBll.Add(AddLogs("修改", model.ID.ToString(), model.UserName));
            }

            return dalResult;
        }

        #endregion
    }
}
