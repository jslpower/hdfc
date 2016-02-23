using System.Collections.Generic;

namespace EyouSoft.IDAL.CompanyStructure
{
    /// <summary>
    /// 公司部门信息数据层接口
    /// </summary>
    public interface IDepartment
    {
        /// <summary>
        /// 添加部门信息
        /// </summary>
        /// <param name="model">部门实体</param>
        /// <returns>true:成功 false:失败</returns>
        bool Add(Model.CompanyStructure.Department model);
        /// <summary>
        /// 修改部门信息
        /// </summary>
        /// <param name="model">部门实体</param>
        /// <returns>true:成功 false:失败</returns>
        bool Update(Model.CompanyStructure.Department model);
        /// <summary>
        /// 获取公司部门实体
        /// </summary>
        /// <param name="id">主键编号</param>
        /// <returns></returns>
        Model.CompanyStructure.Department GetModel(int id);
        /// <summary>
        /// 删除部门信息
        /// </summary>
        /// <param name="id">主键集合</param>
        /// <returns>true:成功 false:失败</returns>
        bool Delete(int id);

        /// <summary>
        /// 获取公司的所有部门信息
        /// </summary>
        /// <param name="companyId">公司编号</param>
        /// <param name="parentDepartId">父级部门编号</param>
        /// <returns>部门信息集合</returns>
        IList<Model.CompanyStructure.Department> GetList(int companyId, int parentDepartId);

        /// <summary>
        /// 获取所有部门信息
        /// </summary>
        /// <param name="companyId">公司编号</param>
        /// <returns></returns>
        IList<Model.CompanyStructure.Department> GetAllDept(int companyId);

        /// <summary>
        /// 判断是否有下级部门
        /// </summary>
        /// <param name="id">部门ID</param>
        /// <returns></returns>
        bool HasChildDept(int id);

        /// <summary>
        /// 判断该部门下是否有员工
        /// </summary>
        /// <param name="id">部门ID</param>
        /// <param name="companyId">公司编号</param>
        /// <returns></returns>
        bool HasDeptUser(int id, int companyId);

    }
}
