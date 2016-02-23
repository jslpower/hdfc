using System;
using System.Collections.Generic;

namespace EyouSoft.BLL.CompanyStructure
{
    /// <summary>
    /// 城市管理BLL
    /// </summary>
    public class City : BLLBase
    {
        private readonly IDAL.CompanyStructure.ICity _dal = Component.Factory.ComponentFactory.CreateDAL<IDAL.CompanyStructure.ICity>();
        private readonly SysHandleLogs _handleLogsBll = new SysHandleLogs();

        #region 成员方法

        /// <summary>
        /// 验证城市名是否已经存在
        /// </summary>
        /// <param name="cityName">城市名称</param>
        /// <param name="companyId">公司编号</param>
        /// <param name="cityId">城市编号</param>
        /// <returns>true:已存在 false:不存在</returns>
        public bool IsExists(string cityName, int companyId, int cityId)
        {
            if (string.IsNullOrEmpty(cityName) || companyId <= 0) return true;

            return this._dal.IsExists(cityName, companyId, cityId);
        }

        /// <summary>
        /// 添加城市
        /// </summary>
        /// <param name="model">城市实体</param>
        /// <returns>true:成功 false:失败</returns>
        public bool Add(Model.CompanyStructure.City model)
        {
            if (model == null || string.IsNullOrEmpty(model.CityName)) return false;

            bool result = this._dal.Add(model);
            if (result) this._handleLogsBll.Add(AddLogs("新增", model.Id.ToString(), model.CityName));

            return result;
        }

        /// <summary>
        /// 修改城市
        /// </summary>
        /// <param name="model">城市实体</param>
        /// <returns>true:成功 false:失败</returns>
        public bool Update(Model.CompanyStructure.City model)
        {
            if (model == null || model.Id <= 0) return false;

            bool result = this._dal.Update(model);
            if (result) this._handleLogsBll.Add(AddLogs("修改", model.Id.ToString(), model.CityName));

            return result;
        }

        /// <summary>
        /// 获取城市实体
        /// </summary>
        /// <param name="id">主键编号</param>
        /// <returns></returns>
        public Model.CompanyStructure.City GetModel(int id)
        {
            if (id <= 0) return null;

            return this._dal.GetModel(id);
        }

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="ids">主键集合</param>
        /// <returns>true:成功 false:失败</returns>
        public bool Delete(params int[] ids)
        {
            if (ids == null || ids.Length <= 0) return false;

            bool result = this._dal.Delete(ids);
            if (result) this._handleLogsBll.Add(AddLogs("删除", this.GetIdsByArr(ids), string.Empty));

            return result;
        }

        /// <summary>
        /// 设置是否常用
        /// </summary>
        /// <param name="id">主键编号</param>
        /// <param name="isFav">是否常用</param>
        /// <returns>true:成功 false:失败</returns>
        public bool SetFav(int id, bool isFav)
        {
            if (id <= 0) return false;
            return this._dal.SetFav(id, isFav);
        }

        /// <summary>
        /// 获取城市集合
        /// </summary>
        /// <param name="companyId">公司编号</param>
        /// <param name="provinceId">省份编号</param>
        /// <param name="isFav">是否常用城市 =null返回全部</param>
        /// <returns>城市集合</returns>
        public IList<Model.CompanyStructure.City> GetList(int companyId, int provinceId, bool? isFav)
        {
            if (companyId <= 0 || provinceId <= 0) return null;
            return this._dal.GetList(companyId, provinceId, isFav);
        }

        /// <summary>
        /// 获取城市集合
        /// </summary>
        /// <param name="companyId">公司编号</param>
        /// <param name="provinceId">省份编号</param>
        /// <param name="isFav">是否常用城市 =null返回全部</param>
        /// <returns>城市集合</returns>
        public IList<Model.CompanyStructure.City> GetList(int companyId, int? provinceId, bool? isFav)
        {
            if (companyId <= 0)
                return null;

            return this._dal.GetList(companyId, provinceId, isFav);
        }

        #endregion

        /// <summary>
        /// 添加日志记录
        /// </summary>
        /// <param name="actionName">操作名称</param>
        /// <param name="cityId">操作的城市编号（可以是多个）</param>
        /// <param name="cityName">城市名称</param>
        /// <returns></returns>
        private Model.CompanyStructure.SysHandleLogs AddLogs(string actionName, string cityId, string cityName)
        {
            var model = new Model.CompanyStructure.SysHandleLogs
                {
                    ModuleId = Model.EnumType.PrivsStructure.Privs2.系统设置_基础设置,
                    EventCode = Model.CompanyStructure.SysHandleLogsNO.EventCode,
                    EventMessage =
                        DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "{0}在"
                        + Model.EnumType.PrivsStructure.Privs2.系统设置_基础设置 + actionName + "了城市，编号为" + cityId + "，名称为"
                        + cityName,
                    EventTitle = actionName + Model.EnumType.PrivsStructure.Privs2.系统设置_基础设置 + " 城市"
                };

            return model;
        }
    }
}
