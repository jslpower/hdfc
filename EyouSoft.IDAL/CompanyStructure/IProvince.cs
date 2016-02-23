using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EyouSoft.IDAL.CompanyStructure
{
    /// <summary>
    /// 公司省份数据层接口
    /// </summary>
    public interface IProvince
    {
        /// <summary>
        /// 验证省份名是否已经存在
        /// </summary>
        /// <param name="provinceName">省份名称</param>
        /// <param name="companyId">公司编号</param>
        /// <param name="provinceId">身份编号</param>
        /// <returns>true:已存在 false:不存在</returns>
        bool IsExists(string provinceName, int companyId, int provinceId);
        /// <summary>
        /// 添加省份
        /// </summary>
        /// <param name="model">省份实体</param>
        /// <returns>true:成功 false:失败</returns>
        bool Add(Model.CompanyStructure.Province model);
        /// <summary>
        /// 修改省份
        /// </summary>
        /// <param name="model">省份实体</param>
        /// <returns>true:成功 false:失败</returns>
        bool Update(Model.CompanyStructure.Province model);
        /// <summary>
        /// 获取省份实体
        /// </summary>
        /// <param name="id">主键编号</param>
        /// <returns></returns>
        Model.CompanyStructure.Province GetModel(int id);
        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="ids">主键集合</param>
        /// <returns>true:成功 false:失败</returns>
        bool Delete(params int[] ids);
        /// <summary>
        /// 获取指定公司的省份集合
        /// </summary>
        /// <param name="companyId">公司编号</param>
        /// <returns>省份集合</returns>
        IList<Model.CompanyStructure.Province> GetList(int companyId);

        /// <summary>
        /// 获取某个公司所有省份的信息包括城市
        /// </summary>
        /// <param name="companyId">公司编号</param>
        /// <returns></returns>
        IList<Model.CompanyStructure.Province> GetProvinceInfo(int companyId);

        /// <summary>
        /// 获取有常用城市的省份列表
        /// </summary>
        /// <param name="companyId">公司编号</param>
        /// <returns></returns>
        IList<Model.CompanyStructure.Province> GetHasFavCityProvince(int companyId);
    }
}
