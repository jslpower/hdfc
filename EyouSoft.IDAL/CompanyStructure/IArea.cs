using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EyouSoft.IDAL.CompanyStructure
{
    /// <summary>
    /// 公司产品数据层接口
    /// </summary>
    /// 鲁功源 2011-01-21
    public interface IArea
    {
        /// <summary>
        /// 验证是否已经存在同名的线路区域
        /// </summary>
        /// <param name="areaName">线路区域名称</param>
        /// <param name="companyId">公司编号</param>
        /// <param name="id">当前线路区域Id</param>
        /// <returns>true:存在 false:不存在</returns>
        bool IsExists(string areaName, int companyId, int id);
        /// <summary>
        /// 添加
        /// </summary>
        /// <param name="model">线路区域实体</param>
        /// <returns>true:成功 false:失败</returns>
        bool Add(Model.CompanyStructure.Area model);
        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="model">线路区域实体</param>
        /// <returns>true:成功 false:失败</returns>
        bool Update(Model.CompanyStructure.Area model);
        /// <summary>
        /// 获取线路区域实体
        /// </summary>
        /// <param name="id">主键编号</param>
        /// <returns></returns>
        Model.CompanyStructure.Area GetModel(int id);
        /// <summary>
        /// 删除线路区域集合
        /// </summary>
        /// <param name="ids">主键编号</param>
        /// <returns>true:成功 false:失败</returns>
        bool Delete(params int[] ids);

        /*/// <summary>
        /// 线路区域是否发布过
        /// </summary>
        /// <param name="areaId">线路ID</param>
        /// <param name="companyId">公司ID</param>
        /// <returns>true发布过 false没发布过 </returns>
        bool IsAreaPublish(int areaId, int companyId);*/
        /// <summary>
        /// 分页获取公司线路区域集合
        /// </summary>
        /// <param name="pageSize">每页显示条数</param>
        /// <param name="pageIndex">当前页码</param>
        /// <param name="recordCount">总记录数</param>
        /// <param name="companyId">公司编号</param>
        /// <returns>公司线路区域集合</returns>
        IList<Model.CompanyStructure.Area> GetList(int pageSize, int pageIndex, ref int recordCount, int companyId);

        /// <summary>
        /// 获取当前公司的所有线路区域信息
        /// </summary>
        /// <param name="companyId">公司ID</param>
        /// <returns></returns>
        IList<Model.CompanyStructure.Area> GetAreaByCompanyId(int companyId);

        /// <summary>
        /// 获取指定公司线路区域排序信息(最小及最大排序号)
        /// </summary>
        /// <param name="companyId">公司编号</param>
        /// <param name="min">最小排序号</param>
        /// <param name="max">最大排序号</param>
        void GetAreaSortId(int companyId, out int min, out int max);
        /// <summary>
        /// 线路区域是否被使用
        /// </summary>
        /// <param name="areaId">线路区域编号</param>
        /// <returns></returns>
        bool IsShiYong(int areaId);
    }
}
