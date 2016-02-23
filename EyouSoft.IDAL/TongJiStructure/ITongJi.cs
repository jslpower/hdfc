using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EyouSoft.IDAL.TongJiStructure
{
    /// <summary>
    /// 统计数据访问接口
    /// </summary>
    public interface ITongJi
    {
        /// <summary>
        /// 获取团散统计列表
        /// </summary>
        /// <param name="companyId">公司编号</param>
        /// <param name="pageSize">每页记录数</param>
        /// <param name="pageIndex">当前页数</param>
        /// <param name="recordCount">总记录数</param>
        /// <param name="search">团散统计查询实体</param>
        /// <param name="heJi">团散统计合计实体(应用传null或者实例化一个就可以了，不用具体赋值)</param>
        /// <returns></returns>
        IList<Model.TongJiStructure.MTourAndSan> GetTourAndSan(
            int companyId,
            int pageSize,
            int pageIndex,
            ref int recordCount,
            Model.TongJiStructure.MSearchTourAndSan search,
            Model.TongJiStructure.MTourAndSanHeJi heJi);

        /// <summary>
        /// 获取组团社统计列表
        /// </summary>
        /// <param name="companyId">公司编号</param>
        /// <param name="pageSize">每页记录数</param>
        /// <param name="pageIndex">当前页数</param>
        /// <param name="recordCount">总记录数</param>
        /// <param name="search">组团社统计查询实体</param>
        /// <param name="heJi">组团社统计合计实体(应用传null或者实例化一个就可以了，不用具体赋值)</param>
        /// <returns></returns>
        IList<Model.TongJiStructure.MCustomerTongJi> GetCustomerTongJi(
            int companyId,
            int pageSize,
            int pageIndex,
            ref int recordCount,
            Model.TongJiStructure.MSearchCustomerTongJi search,
            Model.TongJiStructure.MCustomerTongJiHeJi heJi);

        /// <summary>
        /// 获取销售地区统计列表
        /// </summary>
        /// <param name="companyId">公司编号</param>
        /// <param name="pageSize">每页记录数</param>
        /// <param name="pageIndex">当前页数</param>
        /// <param name="recordCount">总记录数</param>
        /// <param name="search">销售地区统计查询实体</param>
        /// <param name="heJi">销售地区统计合计实体(应用传null或者实例化一个就可以了，不用具体赋值)</param>
        /// <returns></returns>
        IList<Model.TongJiStructure.MSaleAreaTongJi> GetSaleAreaTongJi(
            int companyId,
            int pageSize,
            int pageIndex,
            ref int recordCount,
            Model.TongJiStructure.MSearchSaleAreaTongJi search,
            Model.TongJiStructure.MSaleAreaTongJiHeJi heJi);
    }
}
