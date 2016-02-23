using System;
using System.Collections.Generic;

namespace EyouSoft.BLL.CompanyStructure
{
    /// <summary>
    /// 系统设置信息管理
    /// </summary>
    public class News : BLLBase
    {
        private readonly IDAL.CompanyStructure.INews _dal = Component.Factory.ComponentFactory.CreateDAL<IDAL.CompanyStructure.INews>();
        private readonly SysHandleLogs _handleLogsBll = new SysHandleLogs();

        #region 成员方法
        /// <summary>
        /// 添加
        /// </summary>
        /// <param name="model">公告信息实体</param>
        /// <returns>1:成功 其他:失败</returns>
        public int Add(Model.CompanyStructure.News model)
        {
            if (model == null || model.CompanyId <= 0 || string.IsNullOrEmpty(model.Title)) return 0;

            int result = this._dal.Add(model);
            if (result == 1) this._handleLogsBll.Add(AddLogs("添加", model.ID.ToString(), model.Title));

            return result;
        }

        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="model">公告信息实体</param>
        /// <returns>1:成功 其他:失败</returns>
        public int Update(Model.CompanyStructure.News model)
        {
            if (model == null || model.ID <= 0 || model.CompanyId <= 0 || string.IsNullOrEmpty(model.Title)) return 0;

            int result = this._dal.Update(model);
            if (result == 1) this._handleLogsBll.Add(AddLogs("修改", model.ID.ToString(), model.Title));

            return result;
        }

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="ids">主键编号集合</param>
        /// <returns>1:成功 其他:失败</returns>
        public int Delete(params int[] ids)
        {
            if (ids == null || ids.Length <= 0)
                return 0;

            int result = this._dal.Delete(ids);
            if (result == 1) this._handleLogsBll.Add(AddLogs("删除", this.GetIdsByArr(ids), string.Empty));

            return result;
        }

        /// <summary>
        /// 获取公告信息实体
        /// </summary>
        /// <param name="id">主键编号</param>
        /// <returns></returns>
        public Model.CompanyStructure.News GetModel(int id)
        {
            if (id <= 0) return null;

            return this._dal.GetModel(id);
        }

        /// <summary>
        /// 设置点击次数
        /// </summary>
        /// <param name="id">主键编号</param>
        /// <returns>1:成功 其他:失败</returns>
        public int SetClicks(int id)
        {
            if (id <= 0) return 0;

            return this._dal.SetClicks(id);
        }

        /// <summary>
        /// 分页获取公告信息列表
        /// </summary>
        /// <param name="pageSize"></param>
        /// <param name="pageIndex"></param>
        /// <param name="recordCount"></param>
        /// <param name="companyId"></param>
        /// <returns></returns>
        public IList<Model.CompanyStructure.News> GetList(int pageSize, int pageIndex, ref int recordCount, int companyId)
        {
            if (pageSize <= 0 || pageIndex <= 0 || companyId <= 0) return null;
            return this._dal.GetList(pageSize, pageIndex, ref recordCount, companyId);
        }

        /// <summary>
        /// 获取某个用户接收到的消息列表
        /// </summary>
        /// <param name="pageSize">每页显示数</param>
        /// <param name="pageIndex">开始页码</param>
        /// <param name="recordCount">总数</param>
        /// <param name="userId">用户编号</param>
        /// <param name="companyId">公司编号</param>
        /// <returns></returns>
        public IList<Model.CompanyStructure.NoticeNews> GetAcceptNews(int pageSize, int pageIndex, ref int recordCount, int userId
            , int companyId)
        {
            if (pageSize <= 0 || pageIndex <= 0 || userId <= 0 || companyId <= 0) return null;
            return this._dal.GetAcceptNews(pageSize, pageIndex, ref recordCount, userId, companyId);
        }


        #endregion


        /// <summary>
        /// 添加日志记录
        /// </summary>
        /// <param name="actionName">操作名称</param>
        /// <param name="id">编号</param>
        /// <param name="name">标题</param>
        /// <returns></returns>
        private Model.CompanyStructure.SysHandleLogs AddLogs(string actionName, string id, string name)
        {
            var model = new Model.CompanyStructure.SysHandleLogs
                {
                    ModuleId = Model.EnumType.PrivsStructure.Privs2.公司文件_公告通知,
                    EventCode = Model.CompanyStructure.SysHandleLogsNO.EventCode,
                    EventMessage =
                        DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "{0}在"
                        + Model.EnumType.PrivsStructure.Privs2.公司文件_公告通知 + actionName + "了公告通知数据，编号为" + id
                        + (string.IsNullOrEmpty(name) ? string.Empty : "，标题为" + name),
                    EventTitle = actionName + Model.EnumType.PrivsStructure.Privs2.公司文件_公告通知 + "数据"
                };

            return model;
        }


        /// <summary>
        /// 阅读新闻
        /// </summary>
        /// <param name="newsId"></param>
        /// <param name="userId"></param>
        public void ReadNews(int newsId, int userId)
        {
            this._dal.ReadNews(newsId, userId);
        }



        /// <summary>
        /// 查询是否有未读的公告
        /// </summary>
        /// <param name="companyId"></param>
        /// <param name="departId"></param>
        /// <param name="UserId"></param>
        /// <returns></returns>
        public bool IsNews(int companyId, int departId, int UserId)
        {
            return this._dal.IsNews(companyId, departId, UserId);
        }
    }
}
