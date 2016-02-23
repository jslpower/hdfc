using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EyouSoft.BLL.CompanyStructure
{
    /// <summary>
    /// 系统登录日志BLL
    /// </summary>
    public class SysLoginLog : BLLBase
    {
        private readonly IDAL.CompanyStructure.ISysLoginLog _dal =
            Component.Factory.ComponentFactory.CreateDAL<IDAL.CompanyStructure.ISysLoginLog>();

        /// <summary>
        /// 添加
        /// </summary>
        /// <param name="model">系统登陆日志实体</param>
        /// <returns></returns>
        public bool Add(Model.CompanyStructure.SysLoginLog model)
        {
            if (model == null) return false;

            return this._dal.Add(model);
        }
        /// <summary>
        /// 获取登录日志实体
        /// </summary>
        /// <param name="id">主键编号</param>
        /// <returns>操作日志实体</returns>
        public Model.CompanyStructure.SysLoginLog GetModel(string id)
        {
            if (string.IsNullOrEmpty(id)) return null;

            return this._dal.GetModel(id);
        }

        /// <summary>
        /// 分页获取登录日志列表
        /// </summary>
        /// <param name="pageSize">每页现实条数</param>
        /// <param name="pageIndex">当前页码</param>
        /// <param name="recordCount">总记录数</param>
        /// <param name="model">系统登录日志查询实体</param>
        /// <returns></returns>
        public IList<Model.CompanyStructure.SysLoginLog> GetList(int pageSize, int pageIndex, ref int recordCount
            , Model.CompanyStructure.QuerySysLoginLog model)
        {
            if (model == null || model.CompanyId <= 0 || pageSize < 1 || pageIndex < 1) return null;

            return _dal.GetList(pageSize, pageIndex, ref recordCount, model);
        }
    }
}
