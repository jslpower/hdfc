using System;
using System.Collections.Generic;

namespace EyouSoft.BLL.CompanyStructure
{
    /// <summary>
    /// 专线商公司角色信息BLL
    /// </summary>
    /// 创建人：luofx 2011-01-17
    public class SysRoleManage : BLLBase
    {
        private readonly IDAL.CompanyStructure.ISysRoleManage _dal =
            Component.Factory.ComponentFactory.CreateDAL<IDAL.CompanyStructure.ISysRoleManage>();

        #region 公共方法

        /// <summary>
        /// 获取公司角色信息集合
        /// </summary>
        /// <param name="pageSize"></param>
        /// <param name="pageIndex"></param>
        /// <param name="recordCount"></param>
        /// <param name="companyId">公司编号</param>
        /// <returns></returns>
        public IList<Model.CompanyStructure.SysRoleManage> GetList(int pageSize, int pageIndex, ref int recordCount, int companyId)
        {
            if (pageSize <= 0 || pageIndex <= 0 || companyId <= 0) return null;

            return this._dal.GetList(pageSize, pageIndex, ref recordCount, companyId);
        }
        /// <summary>
        /// 获取角色信息实体
        /// </summary>
        /// <param name="companyId">公司编号</param>
        /// <param name="id">角色编号</param>
        /// <returns></returns>
        public Model.CompanyStructure.SysRoleManage GetModel(int companyId, int id)
        {
            if (id <= 0 || companyId <= 0) return null;

            return this._dal.GetModel(companyId, id);
        }
        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="model">角色信息实体</param>
        /// <returns></returns>
        public bool Add(Model.CompanyStructure.SysRoleManage model)
        {
            if (model == null || model.CompanyId <= 0 || string.IsNullOrEmpty(model.RoleName) || model.RoleName == "管理员") return false;

            bool isTrue = this._dal.Add(model);

            #region LGWR

            if (isTrue)
            {
                var logInfo = new Model.CompanyStructure.SysHandleLogs
                    {
                        CompanyId = 0,
                        DepatId = 0,
                        EventCode = Model.CompanyStructure.SysHandleLogsNO.EventCode,
                        EventIp = string.Empty,
                        EventMessage =
                            DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "{0}在"
                            + Model.EnumType.PrivsStructure.Privs2.系统设置_角色管理 + "新增了角色信息，编号为" + model.Id + "，名称为"
                            + model.RoleName,
                        EventTime = DateTime.Now,
                        EventTitle = "新增角色管理信息",
                        ModuleId = Model.EnumType.PrivsStructure.Privs2.系统设置_角色管理,
                        OperatorId = 0
                    };
                this.Logwr(logInfo);
            }

            #endregion

            return isTrue;
        }
        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="model">角色信息实体</param>
        /// <returns></returns>
        public bool Update(Model.CompanyStructure.SysRoleManage model)
        {
            if (model == null || model.CompanyId <= 0 || model.Id <= 0 || string.IsNullOrEmpty(model.RoleName)) return false;

            bool isTrue = this._dal.Update(model);

            #region LGWR

            if (isTrue)
            {
                var logInfo = new Model.CompanyStructure.SysHandleLogs
                    {
                        CompanyId = 0,
                        DepatId = 0,
                        EventCode = Model.CompanyStructure.SysHandleLogsNO.EventCode,
                        EventIp = string.Empty,
                        EventMessage =
                            DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "{0}在"
                            + Model.EnumType.PrivsStructure.Privs2.系统设置_角色管理 + "修改了角色信息,编号为" + model.Id + "，名称为"
                            + model.RoleName,
                        EventTime = DateTime.Now,
                        EventTitle = "修改角色管理信息",
                        ModuleId = Model.EnumType.PrivsStructure.Privs2.系统设置_角色管理,
                        OperatorId = 0
                    };
                this.Logwr(logInfo);
            }

            #endregion

            return isTrue;
        }

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="companyId"></param>
        /// <param name="roleId">角色ID</param>
        /// <returns></returns>
        public bool Delete(int companyId, params int[] roleId)
        {
            if (companyId <= 0 || roleId == null || roleId.Length <= 0) return false;

            bool isTrue = this._dal.Delete(companyId, roleId);

            #region LGWR

            if (isTrue)
            {
                var logInfo = new Model.CompanyStructure.SysHandleLogs();
                logInfo.CompanyId = 0;
                logInfo.DepatId = 0;
                logInfo.EventCode = Model.CompanyStructure.SysHandleLogsNO.EventCode;
                logInfo.EventIp = string.Empty;
                logInfo.EventMessage = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "{0}在" + Model.EnumType.PrivsStructure.Privs2.系统设置_角色管理 + "删除了角色信息，编号为" + this.GetIdsByArr(roleId);
                logInfo.EventTime = DateTime.Now;
                logInfo.EventTitle = "删除角色管理信息";
                logInfo.ModuleId = Model.EnumType.PrivsStructure.Privs2.系统设置_角色管理;
                logInfo.OperatorId = 0;
                this.Logwr(logInfo);
            }

            #endregion

            return isTrue;
        }
        #endregion 公共方法

        #region 私有方法
        /// <summary>
        /// 写日志
        /// </summary>
        /// <param name="logInfo">日志信息</param>
        private void Logwr(Model.CompanyStructure.SysHandleLogs logInfo)
        {
            new SysHandleLogs().Add(logInfo);
        }
        #endregion
    }
}
