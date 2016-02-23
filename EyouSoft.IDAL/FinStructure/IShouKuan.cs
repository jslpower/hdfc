using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EyouSoft.IDAL.FinStructure
{
    /// <summary>
    /// 收付款登记数据接口
    /// </summary>
    public interface IShouKuan
    {
        /// <summary>
        /// 添加收付款登记
        /// </summary>
        /// <param name="type">收付款登记类型</param>
        /// <param name="id">收付款登记项目编号</param>
        /// <param name="model">收付款基类</param>
        /// <returns>
        /// 1：成功；
        /// 0：参数错误；
        /// -1：收付款总登记和超过应收（应付）；
        /// -2：添加失败
        /// </returns>
        int AddFinCope(Model.EnumType.FinStructure.KuanXiangType type, string id, Model.FinStructure.MKuanBase model);

        /// <summary>
        /// 修改收付款登记
        /// </summary>
        /// <param name="model">收付款基类</param>
        /// <returns>
        /// 1：成功；
        /// 0：参数错误；
        /// -1：收付款总登记和超过应收（应付）；
        /// -2：修改失败；
        /// </returns>
        int UpdateFinCope(Model.FinStructure.MKuanBase model);

        /// <summary>
        /// 删除收付款登记
        /// </summary>
        /// <param name="id">收付款登记编号</param>
        /// <returns>
        /// 1：成功；
        /// 0：参数错误；
        /// -1：删除失败；
        /// </returns>
        int DeleteFinCope(string id);

        /// <summary>
        /// 获取收付款登记信息
        /// </summary>
        /// <param name="id">收付款登记编号</param>
        /// <returns></returns>
        object GetFinCope(string id);

        /// <summary>
        /// 获取收付款登记信息
        /// </summary>
        /// <param name="type">收付款登记类型</param>
        /// <param name="id">收付款登记项目编号</param>
        /// <returns></returns>
        object GetFinCopeList(Model.EnumType.FinStructure.KuanXiangType type, string id);

        /// <summary>
        /// 获取收付款登记信息
        /// </summary>
        /// <param name="companyId">公司编号</param>
        /// <param name="type">收付款登记类型</param>
        /// <param name="search">其他收入支出查询实体</param>
        /// <param name="pageSize">每页记录数</param>
        /// <param name="pageIndex">当前页数</param>
        /// <param name="recordCount">总记录数</param>
        /// <returns></returns>
        object GetFinCopeList(
            int companyId,
            Model.EnumType.FinStructure.KuanXiangType type,
            int pageSize,
            int pageIndex,
            ref int recordCount,
            Model.FinStructure.MSearchOther search);

        /// <summary>
        /// 获取财务管理应付管理地接列表
        /// </summary>
        /// <param name="companyId">公司编号</param>
        /// <param name="pageSize">每页记录数</param>
        /// <param name="pageIndex">当前页数</param>
        /// <param name="recordCount">总记录数</param>
        /// <param name="search">查询实体</param>
        /// <param name="heJi">合计实体</param>
        /// <returns></returns>
        IList<Model.FinStructure.MDiJieList> GetDiJieList(
            int companyId,
            int pageSize,
            int pageIndex,
            ref int recordCount,
            Model.FinStructure.MSearchDiJieList search,
            Model.FinStructure.MPlanHeJi heJi);

        /// <summary>
        /// 获取财务管理应付管理票务列表
        /// </summary>
        /// <param name="companyId">公司编号</param>
        /// <param name="pageSize">每页记录数</param>
        /// <param name="pageIndex">当前页数</param>
        /// <param name="recordCount">总记录数</param>
        /// <param name="search">查询实体</param>
        /// <param name="heJi">合计实体</param>
        /// <returns></returns>
        IList<Model.FinStructure.MPiaoList> GetPiaoList(
            int companyId,
            int pageSize,
            int pageIndex,
            ref int recordCount,
            Model.FinStructure.MSearchPiaoList search,
            Model.FinStructure.MPlanHeJi heJi);
    }
}
