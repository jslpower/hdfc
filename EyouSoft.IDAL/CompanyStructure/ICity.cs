using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EyouSoft.IDAL.CompanyStructure
{
    /// <summary>
    /// 公司常用城市数据层接口
    /// </summary>
    public interface ICity
    {
        /// <summary>
        /// 验证城市名是否已经存在
        /// </summary>
        /// <param name="cityName">城市名称</param>
        /// <param name="companyId">公司编号</param>
        /// <param name="cityId">城市编号</param>
        /// <returns>true:已存在 false:不存在</returns>
        bool IsExists(string cityName, int companyId, int cityId);
        /// <summary>
        /// 添加城市
        /// </summary>
        /// <param name="model">城市实体</param>
        /// <returns>true:成功 false:失败</returns>
        bool Add(Model.CompanyStructure.City model);
        /// <summary>
        /// 修改城市
        /// </summary>
        /// <param name="model">城市实体</param>
        /// <returns>true:成功 false:失败</returns>
        bool Update(Model.CompanyStructure.City model);
        /// <summary>
        /// 获取城市实体
        /// </summary>
        /// <param name="id">主键编号</param>
        /// <returns></returns>
        Model.CompanyStructure.City GetModel(int id);
        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="ids">主键集合</param>
        /// <returns>true:成功 false:失败</returns>
        bool Delete(params int[] ids);
        /// <summary>
        /// 设置是否常用
        /// </summary>
        /// <param name="id">主键编号</param>
        /// <param name="isFav">是否常用</param>
        /// <returns>true:成功 false:失败</returns>
        bool SetFav(int id, bool isFav);
        /// <summary>
        /// 获取城市集合
        /// </summary>
        /// <param name="companyId">公司编号</param>
        /// <param name="provinceId">省份编号</param>
        /// <param name="isFav">是否常用城市 =null返回全部</param>
        /// <returns>城市集合</returns>
        IList<Model.CompanyStructure.City> GetList(int companyId, int provinceId, bool? isFav);

        /// <summary>
        /// 获取城市集合
        /// </summary>
        /// <param name="companyId">公司编号</param>
        /// <param name="provinceId">省份编号</param>
        /// <param name="isFav">是否常用城市 =null返回全部</param>
        /// <returns>城市集合</returns>
        IList<Model.CompanyStructure.City> GetList(int companyId, int? provinceId, bool? isFav);
    }
}
