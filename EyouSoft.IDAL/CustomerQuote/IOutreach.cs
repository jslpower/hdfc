using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EyouSoft.IDAL.CustomerQuote
{
    /// <summary>
    /// 外联每日足迹数据接口 
    /// </summary>
    public interface IOutreach
    {
        /// <summary>
        /// 添加外联每日足迹
        /// </summary>
        /// <param name="model">外联每日足迹实体</param>
        /// <returns>
        /// 1：成功；
        /// 0：参数错误；
        /// -1：添加失败；
        /// </returns>
        int AddOutreach(Model.CustomerQuote.MOutreach model);

        /// <summary>
        /// 修改外联每日足迹
        /// </summary>
        /// <param name="model">外联每日足迹实体</param>
        /// <returns>
        /// 1：成功；
        /// 0：参数错误；
        /// -1：修改失败；
        /// </returns>
        int UpdateOutreach(Model.CustomerQuote.MOutreach model);

        /// <summary>
        /// 删除外联每日足迹
        /// </summary>
        /// <param name="id">外联每日足迹编号</param>
        /// <returns>
        /// 1：成功；
        /// 0：参数错误；
        /// -1：删除失败；
        /// </returns>
        int DeleteOutreach(params int[] id);

        /// <summary>
        /// 获取外联每日足迹
        /// </summary>
        /// <param name="id">外联每日足迹编号</param>
        /// <returns></returns>
        Model.CustomerQuote.MOutreach GetOutreach(int id);

        /// <summary>
        /// 获取外联每日足迹
        /// </summary>
        /// <param name="companyId">公司编号</param>
        /// <param name="pageSize">每页记录数</param>
        /// <param name="pageIndex">当前页数</param>
        /// <param name="recordCount">总记录数</param>
        /// <param name="seach">外联每日足迹查询实体</param>
        /// <returns></returns>
        IList<Model.CustomerQuote.MOutreach> GetOutreach(
            int companyId,
            int pageSize,
            int pageIndex,
            ref int recordCount,
            Model.CustomerQuote.MSearchOutreach seach);
    }
}
