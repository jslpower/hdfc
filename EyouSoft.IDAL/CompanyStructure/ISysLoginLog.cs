using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EyouSoft.IDAL.CompanyStructure
{
    /// <summary>
    /// 系统登录日志
    /// </summary>
    /// 鲁功源 2010-01-21
    public interface ISysLoginLog
    {
        /// <summary>
        /// 添加
        /// </summary>
        /// <param name="model">系统登陆日志实体</param>
        /// <returns></returns>
        bool Add(Model.CompanyStructure.SysLoginLog model);
        /// <summary>
        /// 获取登录日志实体
        /// </summary>
        /// <param name="id">主键编号</param>
        /// <returns>操作日志实体</returns>
        Model.CompanyStructure.SysLoginLog GetModel(string id);

        /// <summary>
        /// 分页获取登录日志列表
        /// </summary>
        /// <param name="pageSize">每页现实条数</param>
        /// <param name="pageIndex">当前页码</param>
        /// <param name="recordCount">总记录数</param>
        /// <param name="model">系统登录日志查询实体</param>
        /// <returns></returns>
        IList<Model.CompanyStructure.SysLoginLog> GetList(int pageSize, int pageIndex, ref int recordCount
            , Model.CompanyStructure.QuerySysLoginLog model);
    }
}
