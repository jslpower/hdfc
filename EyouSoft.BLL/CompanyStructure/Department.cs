using System;
using System.Collections.Generic;

namespace EyouSoft.BLL.CompanyStructure
{
    /// <summary>
    /// 公司部门信息BLL
    /// </summary>
    public class Department : BLLBase
    {
        private readonly IDAL.CompanyStructure.IDepartment _dal = Component.Factory.ComponentFactory.CreateDAL<IDAL.CompanyStructure.IDepartment>();
        private readonly SysHandleLogs _handleLogsBll = new SysHandleLogs();

        #region private members
        /// <summary>
        /// 移除公司部门缓存
        /// </summary>
        /// <param name="companyId">公司编号</param>
        internal void RemoveDepartmentCache(int companyId)
        {
            string cacheKey = string.Format(Cache.Tag.TagName.ComDept, companyId);
            if (Cache.Facade.EyouSoftCache.GetCache(cacheKey) != null)
            {
                Cache.Facade.EyouSoftCache.Remove(cacheKey);
            }
        }

        /// <summary>
        /// 添加日志记录
        /// </summary>
        /// <param name="actionName">操作名称</param>
        /// <param name="id">操作项编号（可以多个）</param>
        /// <param name="departName">部门名称</param>
        /// <returns></returns>
        private Model.CompanyStructure.SysHandleLogs AddLogs(string actionName, string id, string departName)
        {
            var model = new Model.CompanyStructure.SysHandleLogs
                {
                    ModuleId = Model.EnumType.PrivsStructure.Privs2.系统设置_组织机构,
                    EventCode = Model.CompanyStructure.SysHandleLogsNO.EventCode,
                    EventMessage =
                        DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "{0}在"
                        + Model.EnumType.PrivsStructure.Privs2.系统设置_组织机构 + actionName + "了部门，编号为" + id + "，名称为"
                        + departName,
                    EventTitle = actionName + Model.EnumType.PrivsStructure.Privs2.系统设置_组织机构 + "部门"
                };

            return model;
        }
        #endregion

        #region public members

        /// <summary>
        /// 添加部门信息
        /// </summary>
        /// <param name="model">部门实体</param>
        /// <returns>true:成功 false:失败</returns>
        public bool Add(Model.CompanyStructure.Department model)
        {
            if (model == null || string.IsNullOrEmpty(model.DepartName)) return false;

            bool dalResult = this._dal.Add(model);

            if (dalResult)
            {
                this.RemoveDepartmentCache(model.CompanyId);
                this._handleLogsBll.Add(AddLogs("添加", model.Id.ToString(), model.DepartName));
            }

            return dalResult;
        }

        /// <summary>
        /// 修改部门信息
        /// </summary>
        /// <param name="model">部门实体</param>
        /// <returns>true:成功 false:失败</returns>
        public bool Update(Model.CompanyStructure.Department model)
        {
            if (model == null || string.IsNullOrEmpty(model.DepartName) || model.Id <= 0) return false;

            bool dalResult = this._dal.Update(model);

            if (dalResult)
            {
                this.RemoveDepartmentCache(model.CompanyId);
                this._handleLogsBll.Add(AddLogs("修改", model.Id.ToString(), model.DepartName));
            }

            return this._dal.Update(model);
        }

        /// <summary>
        /// 获取公司部门实体
        /// </summary>
        /// <param name="id">主键编号</param>
        /// <returns></returns>
        public Model.CompanyStructure.Department GetModel(int id)
        {
            if (id <= 0) return null;

            return this._dal.GetModel(id);
        }

        /// <summary>
        /// 删除部门信息
        /// </summary>
        /// <param name="companyId">公司编号</param>
        /// <param name="id">部门编号</param>
        /// <returns>true:成功 false:失败</returns>
        public bool Delete(int companyId, int id)
        {
            if (companyId <= 0 || id <= 0) return false;

            bool dalResult = this._dal.Delete(id);

            if (dalResult)
            {
                this.RemoveDepartmentCache(companyId);
            }

            this._handleLogsBll.Add(AddLogs("删除", id.ToString(), string.Empty));
            return dalResult;
        }

        /// <summary>
        /// 获取上级部门信息
        /// </summary>
        /// <param name="companyId">公司编号</param>
        /// <param name="parentDepartId">父级部门编号</param>
        /// <returns>部门信息集合</returns>
        public IList<Model.CompanyStructure.Department> GetList(int companyId, int parentDepartId)
        {
            if (companyId <= 0) return null;

            return this._dal.GetList(companyId, parentDepartId);
        }

        /// <summary>
        /// 获取所有部门信息
        /// </summary>
        /// <param name="companyId"></param>
        /// <returns></returns>
        public IList<Model.CompanyStructure.Department> GetAllDept(int companyId)
        {
            if (companyId <= 0) return null;

            return this._dal.GetAllDept(companyId);
        }

        /// <summary>
        /// 判断是否有下级部门
        /// </summary>
        /// <param name="id">部门ID</param>
        /// <returns></returns>
        public bool HasChildDept(int id)
        {
            if (id <= 0) return false;

            return this._dal.HasChildDept(id);
        }

        /// <summary>
        /// 判断该部门下是否有员工
        /// </summary>
        /// <param name="id">部门ID</param>
        /// <param name="companyId">公司ID</param>
        /// <returns></returns>
        public bool HasDeptUser(int id, int companyId)
        {
            if (id <= 0 || companyId <= 0) return false;

            return this._dal.HasDeptUser(id, companyId);
        }

        #endregion
    }
}
