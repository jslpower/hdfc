using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EyouSoft.BLL.CompanyStructure
{
    /// <summary>
    /// 系统操作日志BLL
    /// </summary>
    public class SysHandleLogs : BLLBase
    {
        /// <summary>
        /// 操作日志数据层
        /// </summary>
        private readonly IDAL.CompanyStructure.ISysHandleLogs _dal =
            Component.Factory.ComponentFactory.CreateDAL<IDAL.CompanyStructure.ISysHandleLogs>();

        /// <summary>
        /// 当前登录用户信息
        /// </summary>
        private readonly Model.SSOStructure.MUserInfo _currUserInfo = Security.Membership.UserProvider.GetUserInfo();

        /// <summary>
        /// 添加操作日志
        /// </summary>
        /// <param name="model">系统操作日志实体</param>
        /// <returns>true:成功 false:失败</returns>
        public bool Add(Model.CompanyStructure.SysHandleLogs model)
        {
            if (model == null)
                return false;

            model.EventIp = Toolkit.Utils.GetRemoteIP();
            model.EventTime = DateTime.Now;
            if (this._currUserInfo != null)
            {
                model.OperatorId = this._currUserInfo.UserId;
                model.CompanyId = this._currUserInfo.CompanyId;
                model.DepatId = this._currUserInfo.DeptId;
                if (!string.IsNullOrEmpty(model.EventMessage) && model.EventMessage.Contains("{0}") && this._currUserInfo.Name != null)
                    model.EventMessage = string.Format(model.EventMessage, this._currUserInfo.Name);
            }

            return this._dal.Add(model);
        }

        /// <summary>
        /// 获取操作日志实体
        /// </summary>
        /// <param name="id">主键编号</param>
        /// <returns>操作日志实体</returns>
        public Model.CompanyStructure.SysHandleLogs GetModel(string id)
        {
            return this._dal.GetModel(id);
        }

        /// <summary>
        /// 分页获取操作日志列表
        /// </summary>
        /// <param name="pageSize">每页现实条数</param>
        /// <param name="pageIndex">当前页码</param>
        /// <param name="recordCount">总记录数</param>
        /// <param name="model">系统操作日志查询实体</param>
        /// <returns></returns>
        public IList<Model.CompanyStructure.SysHandleLogs> GetList(int pageSize, int pageIndex, ref int recordCount
            , Model.CompanyStructure.QueryHandleLog model)
        {
            return this._dal.GetList(pageSize, pageIndex, ref recordCount, model);
        }
    }
}
