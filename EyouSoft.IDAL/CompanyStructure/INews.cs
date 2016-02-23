using System.Collections.Generic;

namespace EyouSoft.IDAL.CompanyStructure
{
    /// <summary>
    /// 系统设置信息管理接口
    /// </summary>
    public interface INews
    {
        /// <summary>
        /// 添加
        /// </summary>
        /// <param name="model">公告信息实体</param>
        /// <returns>true:成功 false:失败</returns>
        int Add(Model.CompanyStructure.News model);

        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="model">公告信息实体</param>
        /// <returns>true:成功 false:失败</returns>
        int Update(Model.CompanyStructure.News model);

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="ids">主键编号集合</param>
        /// <returns>true:成功 false:失败</returns>
        int Delete(params int[] ids);

        /// <summary>
        /// 获取公告信息实体
        /// </summary>
        /// <param name="id">主键编号</param>
        /// <returns></returns>
        Model.CompanyStructure.News GetModel(int id);

        /// <summary>
        /// 设置点击次数
        /// </summary>
        /// <param name="id">主键编号</param>
        /// <returns>true:成功 false:失败</returns>
        int SetClicks(int id);

        /// <summary>
        /// 分页获取公告信息列表
        /// </summary>
        /// <param name="pageSize"></param>
        /// <param name="pageIndex"></param>
        /// <param name="recordCount"></param>
        /// <param name="companyId"></param>
        /// <returns></returns>
        IList<Model.CompanyStructure.News> GetList(int pageSize, int pageIndex, ref int recordCount, int companyId);

        /// <summary>
        /// 获取某个用户接收到的消息列表
        /// </summary>
        /// <param name="recordCount"></param>
        /// <param name="userId">用户编号</param>
        /// <param name="companyId">公司编号</param>
        /// <param name="pageSize"></param>
        /// <param name="pageIndex"></param>
        /// <returns></returns>
        IList<Model.CompanyStructure.NoticeNews> GetAcceptNews(int pageSize, int pageIndex, ref int recordCount
            , int userId, int companyId);

        /// <summary>
        /// 阅读新闻
        /// </summary>
        /// <param name="newsId"></param>
        /// <param name="userId"></param>
        void ReadNews(int newsId, int userId);



        /// <summary>
        /// 查询是否有未读的公告
        /// </summary>
        /// <param name="companyId"></param>
        /// <param name="departId"></param>
        /// <param name="UserId"></param>
        /// <returns></returns>
        bool IsNews(int companyId, int departId, int UserId);
    }
}
