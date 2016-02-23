using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EyouSoft.IDAL.FinStructure
{
    /// <summary>
    /// 出纳登账数据接口
    /// </summary>
    public interface IChuNaDengZhang
    {
        /// <summary>
        /// 添加出纳登账信息（只做财务管理-出纳登账-新增使用）
        /// </summary>
        /// <param name="model">出纳登账实体</param>
        /// <returns>
        /// 1：成功；
        /// 0：参数错误；
        /// -1：添加失败；
        /// </returns>
        int AddChuNaDengZhang(Model.FinStructure.MChuNaDengZhang model);

        /// <summary>
        /// 修改出纳登账信息
        /// </summary>
        /// <param name="model">出纳登账实体</param>
        /// <returns>
        /// 1：成功；
        /// 0：参数错误；
        /// -1：不是用户登记的数据，不能修改；
        /// -2：修改失败；
        /// </returns>
        int UpdateChuNaDengZhang(Model.FinStructure.MChuNaDengZhang model);

        /// <summary>
        /// 删除出纳登账信息
        /// </summary>
        /// <param name="id">出纳登账编号</param>
        /// <returns>
        /// 1：成功；
        /// 0：参数错误；
        /// -1：单条删除时，不是用户登记的数据，不能删除；
        /// -2：多条删除时，删除能删除的，不能删除的没有删除；
        /// -3：删除失败；
        /// </returns>
        int DeleteChuNaDengZhang(params string[] id);

        /// <summary>
        /// 获取出纳登账信息
        /// </summary>
        /// <param name="id">出纳登账编号</param>
        /// <returns></returns>
        Model.FinStructure.MChuNaDengZhang GetChuNaDengZhang(string id);

        /// <summary>
        /// 获取出纳登账信息
        /// </summary>
        /// <param name="companyId"> 公司编号</param>
        /// <param name="pageSize">每页记录数</param>
        /// <param name="pageIndex">当前页数</param>
        /// <param name="recordCount">总记录数</param>
        /// <param name="search">查询实体</param>
        /// <param name="heJi">合计实体（赋值null或者新实例化一个对象）</param>
        /// <returns></returns>
        IList<Model.FinStructure.MChuNaDengZhang> GetChuNaDengZhang(
            int companyId,
            int pageSize,
            int pageIndex,
            ref int recordCount,
            Model.FinStructure.MSearchChuNaDengZhang search,
            ref Model.FinStructure.MChuNaDengZhangHeJi heJi); 
    }
}
