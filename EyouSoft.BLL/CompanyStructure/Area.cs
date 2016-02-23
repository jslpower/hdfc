using System;
using System.Collections.Generic;

namespace EyouSoft.BLL.CompanyStructure
{
    /// <summary>
    /// 线路区域BLL
    /// Author:xuqh 2011-01-21 
    /// </summary>
    public class Area : BLLBase
    {
        #region constructure

        /// <summary>
        /// 线路区域数据层
        /// </summary>
        private readonly IDAL.CompanyStructure.IArea _dal = Component.Factory.ComponentFactory.CreateDAL<IDAL.CompanyStructure.IArea>();
        /// <summary>
        ///  操作日志业务逻辑
        /// </summary>
        private readonly SysHandleLogs _handleLogsBll = new SysHandleLogs();

        #endregion

        #region 成员方法

        /// <summary>
        /// 验证是否已经存在同名的线路区域
        /// </summary>
        /// <param name="areaName">线路区域名称</param>
        /// <param name="companyId">公司编号</param>
        /// <param name="id">要排除的线路区域编号</param>
        /// <returns>true:存在 false:不存在</returns>
        public bool IsExists(string areaName, int companyId, int id)
        {
            if (string.IsNullOrEmpty(areaName) || companyId <= 0) return true;

            return this._dal.IsExists(areaName, companyId, id);
        }

        /// <summary>
        /// 添加
        /// </summary>
        /// <param name="model">线路区域实体</param>
        /// <returns>true:成功 false:失败</returns>
        public bool Add(Model.CompanyStructure.Area model)
        {
            if (model == null || string.IsNullOrEmpty(model.AreaName)) return false;

            bool result = this._dal.Add(model);

            if (result) this._handleLogsBll.Add(AddLogs("新增", model.Id.ToString(), model.AreaName));

            return result;
        }

        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="model">线路区域实体</param>
        /// <returns>true:成功 false:失败</returns>
        public bool Update(Model.CompanyStructure.Area model)
        {
            if (model == null || string.IsNullOrEmpty(model.AreaName) || model.Id <= 0) return false;

            bool result = this._dal.Update(model);

            if (result) this._handleLogsBll.Add(AddLogs("修改", model.Id.ToString(), model.AreaName));

            return result;
        }

        /// <summary>
        /// 获取线路区域实体
        /// </summary>
        /// <param name="id">主键编号</param>
        /// <returns></returns>
        public Model.CompanyStructure.Area GetModel(int id)
        {
            if (id <= 0) return null;

            return this._dal.GetModel(id);
        }

        /// <summary>
        /// 删除线路区域
        /// </summary>
        /// <param name="areaId">线路区域编号</param>
        /// <returns>true:成功 false:失败</returns>
        public int Delete(int areaId)
        {
            if (areaId < 1) return 0;
            if (_dal.IsShiYong(areaId)) return -1;

            bool result = this._dal.Delete(areaId);

            if (result)
            {
                this._handleLogsBll.Add(AddLogs("删除", areaId.ToString(), string.Empty));
                return 1;
            }

            return -100;
        }

        /*/// <summary>
        /// 线路区域是否发布过
        /// </summary>
        /// <param name="areaId">线路ID</param>
        /// <param name="companyId">公司ID</param>
        /// <returns>true发布过 false没发布过 </returns>
        public bool IsAreaPublish(int areaId, int companyId)
        {
            if (areaId <= 0 || companyId <= 0) return true;

            return this._dal.IsAreaPublish(areaId, companyId);
        }*/

        /// <summary>
        /// 分页获取公司线路区域集合
        /// </summary>
        /// <param name="pageSize">每页显示条数</param>
        /// <param name="pageIndex">当前页码</param>
        /// <param name="recordCount">总记录数</param>
        /// <param name="companyId">公司编号</param>
        /// <returns>公司线路区域集合</returns>
        public IList<Model.CompanyStructure.Area> GetList(int pageSize, int pageIndex, ref int recordCount, int companyId)
        {
            if (companyId <= 0 || pageIndex <= 0 || pageSize <= 0) return null;

            return this._dal.GetList(pageSize, pageIndex, ref recordCount, companyId);
        }

        /// <summary>
        /// 获取当前公司的所有线路区域信息
        /// </summary>
        /// <param name="companyId">公司ID</param>
        /// <returns></returns>
        public IList<Model.CompanyStructure.Area> GetAreaByCompanyId(int companyId)
        {
            if (companyId <= 0) return null;

            return this._dal.GetAreaByCompanyId(companyId);
        }

        /// <summary>
        /// 获取指定公司线路区域排序信息(最小及最大排序号)
        /// </summary>
        /// <param name="companyId">公司编号</param>
        /// <param name="min">最小排序号</param>
        /// <param name="max">最大排序号</param>
        public void GetAreaSortId(int companyId, out int min, out int max)
        {
            min = 0; max = 0;
            if (companyId < 1) return;
            this._dal.GetAreaSortId(companyId, out min, out max);
        }
        #endregion

        /// <summary>
        /// 添加日志记录
        /// </summary>
        /// <param name="actionName">操作名称</param>
        /// <param name="areaId">操作的线路区域编号（可以是多个）</param>
        /// <param name="areaName">线路区域名称</param>
        /// <returns></returns>
        private Model.CompanyStructure.SysHandleLogs AddLogs(string actionName, string areaId, string areaName)
        {
            var model = new Model.CompanyStructure.SysHandleLogs
                {
                    ModuleId = Model.EnumType.PrivsStructure.Privs2.系统设置_基础设置,
                    EventCode = Model.CompanyStructure.SysHandleLogsNO.EventCode,
                    EventMessage =
                        DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "{0}在"
                        + Model.EnumType.PrivsStructure.Privs2.系统设置_基础设置 + actionName + "了线路区域，编号为" + areaId + "，名称为"
                        + areaName,
                    EventTitle = actionName + Model.EnumType.PrivsStructure.Privs2.系统设置_基础设置 + " 线路区域"
                };

            return model;
        }
    }
}
