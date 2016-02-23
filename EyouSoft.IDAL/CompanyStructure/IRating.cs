using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EyouSoft.IDAL.CompanyStructure
{
    /// <summary>
    /// 客户信用等级接口
    /// </summary>
    /// 邓保朝 2013-09-29
    public interface IRating
    {
        /// <summary>
        /// 验证是否已经存在同名的客户信用等级
        /// </summary>
        /// <param name="RatingName">客户信用等级名称</param>
        /// <param name="companyId">公司编号</param>
        /// <param name="id">当前客户信用等级Id</param>
        /// <returns>true:存在 false:不存在</returns>
        bool IsExists(string RatingName, int companyId, int id);
        /// <summary>
        /// 添加
        /// </summary>
        /// <param name="model">客户信用等级实体</param>
        /// <returns>true:成功 false:失败</returns>
        bool Add(Model.CompanyStructure.Rating model);
        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="model">客户信用等级实体</param>
        /// <returns>true:成功 false:失败</returns>
        bool Update(Model.CompanyStructure.Rating model);
        /// <summary>
        /// 获取客户信用等级实体
        /// </summary>
        /// <param name="id">主键编号</param>
        /// <returns></returns>
        Model.CompanyStructure.Rating GetModel(int id);
        /// <summary>
        /// 删除客户信用等级集合
        /// </summary>
        /// <param name="ids">主键编号</param>
        /// <returns>true:成功 false:失败</returns>
        bool Delete(params int[] ids);
        /// <summary>
        /// 分页获取公司客户信用等级集合
        /// </summary>
        /// <param name="pageSize">每页显示条数</param>
        /// <param name="pageIndex">当前页码</param>
        /// <param name="recordCount">总记录数</param>
        /// <param name="companyId">公司编号</param>
        /// <returns>公司客户信用等级集合</returns>
        IList<Model.CompanyStructure.Rating> GetList(int pageSize, int pageIndex, ref int recordCount, int companyId);

        /// <summary>
        /// 获取当前公司的所有客户信用等级信息
        /// </summary>
        /// <param name="companyId">公司ID</param>
        /// <returns></returns>
        IList<Model.CompanyStructure.Rating> GetRatingByCompanyId(int companyId);

        /// <summary>
        /// 获取指定公司客户信用等级排序信息(最小及最大排序号)
        /// </summary>
        /// <param name="companyId">公司编号</param>
        /// <param name="min">最小排序号</param>
        /// <param name="max">最大排序号</param>
        void GetRatingSortId(int companyId, out int min, out int max);
        /// <summary>
        /// 客户信用等级是否被使用
        /// </summary>
        /// <param name="RatingId">客户信用等级编号</param>
        /// <returns></returns>
        bool IsShiYong(int RatingId);
    }
}
