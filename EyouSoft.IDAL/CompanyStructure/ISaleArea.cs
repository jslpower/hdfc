using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EyouSoft.IDAL.CompanyStructure
{
    /// <summary>
    ///  销售地区数据接口
    /// </summary>
    public interface ISaleArea
    {
        /// <summary>
        /// 验证是否存在某名称的销售区域
        /// </summary>
        /// <param name="name">名称</param>
        /// <param name="id">要排除的销售区域编号</param>
        /// <param name="companyId">公司编号</param>
        /// <returns>返回true 存在；false 不存在</returns>
        bool ExistsSaleAreaName(string name, int companyId, int id);

        /// <summary>
        /// 验证某销售地区是否可以删除
        /// </summary>
        /// <param name="id">要验证的销售区域编号</param>
        /// <returns>返回可以删除的Id集合</returns>
        int[] ExistsDeleteSaleArea(params int[] id);

        /// <summary>
        /// 添加销售地区
        /// </summary>
        /// <param name="model">销售地区实体</param>
        /// <returns>
        /// 1：成功；
        /// 0：参数错误；
        /// -1：名称已经存在；
        /// -2：新增失败（sql错误）
        /// </returns>
        int AddSaleArea(Model.CompanyStructure.MSaleArea model);

        /// <summary>
        /// 修改销售地区
        /// </summary>
        /// <param name="model">销售地区实体</param>
        /// <returns>
        /// 1：成功；
        /// 0：参数错误；
        /// -1：名称已经存在；
        /// -2：修改失败（sql错误）
        ///</returns>
        int UpdateSaleArea(Model.CompanyStructure.MSaleArea model);

        /// <summary>
        /// 删除销售地区
        /// </summary>
        /// <param name="id">销售地区编号</param>
        /// <returns>
        /// 1：成功；
        /// 0：参数错误；
        /// -3：删除失败（sql错误）
        /// </returns>
        int DeleteSaleArea(params int[] id);

        /// <summary>
        /// 获取销售地区信息
        /// </summary>
        /// <param name="id">销售地区编号</param>
        /// <returns></returns>
        Model.CompanyStructure.MSaleArea GetSaleArea(int id);

        /// <summary>
        /// 获取销售地区信息
        /// </summary>
        /// <param name="seach">销售地区查询实体</param>
        /// <param name="companyId">公司编号</param>
        /// <returns></returns>
        IList<Model.CompanyStructure.MSaleArea> GetSaleArea(int companyId, Model.CompanyStructure.MSearchSaleArea seach);

        /// <summary>
        /// 获取销售地区信息
        /// </summary>
        /// <param name="companyId">公司编号</param>
        /// <param name="pageSize">每页记录数</param>
        /// <param name="pageIndex">当前页数</param>
        /// <param name="recordCount">总记录数</param>
        /// <param name="seach">销售地区查询实体</param>
        /// <returns></returns>
        IList<Model.CompanyStructure.MSaleArea> GetSaleArea(
            int companyId,
            int pageSize,
            int pageIndex,
            ref int recordCount,
            Model.CompanyStructure.MSearchSaleArea seach);
    }
}
