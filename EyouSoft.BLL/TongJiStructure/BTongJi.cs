using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EyouSoft.BLL.TongJiStructure
{
    /// <summary>
    /// 统计中心业务逻辑
    /// </summary>
    public class BTongJi : BLLBase
    {
        private readonly IDAL.TongJiStructure.ITongJi _dal =
            Component.Factory.ComponentFactory.CreateDAL<IDAL.TongJiStructure.ITongJi>();

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
        public IList<Model.TongJiStructure.MTourAndSan> GetTourAndSan(
            int companyId,
            int pageSize,
            int pageIndex,
            ref int recordCount,
            Model.TongJiStructure.MSearchTourAndSan search,
            Model.TongJiStructure.MTourAndSanHeJi heJi)
        {
            if (companyId <= 0 || pageSize <= 0 || pageIndex <= 0) return null;

            return _dal.GetTourAndSan(companyId, pageSize, pageIndex, ref recordCount, search, heJi);
        }

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
        public IList<Model.TongJiStructure.MCustomerTongJi> GetCustomerTongJi(
            int companyId,
            int pageSize,
            int pageIndex,
            ref int recordCount,
            Model.TongJiStructure.MSearchCustomerTongJi search,
            Model.TongJiStructure.MCustomerTongJiHeJi heJi)
        {
            if (companyId <= 0 || pageSize <= 0 || pageIndex <= 0) return null;

            return _dal.GetCustomerTongJi(companyId, pageSize, pageIndex, ref recordCount, search, heJi);
        }

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
        public IList<Model.TongJiStructure.MSaleAreaTongJi> GetSaleAreaTongJi(
            int companyId,
            int pageSize,
            int pageIndex,
            ref int recordCount,
            Model.TongJiStructure.MSearchSaleAreaTongJi search,
            Model.TongJiStructure.MSaleAreaTongJiHeJi heJi)
        {
            if (companyId <= 0 || pageSize <= 0 || pageIndex <= 0) return null;

            return _dal.GetSaleAreaTongJi(companyId, pageSize, pageIndex, ref recordCount, search, heJi);
        }
    }
}
