using System.Collections.Generic;

namespace EyouSoft.IDAL.CompanyStructure
{
    /// <summary>
    /// 专线商角色管理IDAL
    /// </summary>
    public interface ISysRoleManage
    {
        /// <summary>
        /// 获取公司职务信息集合
        /// </summary>
        /// <param name="pageSize"></param>
        /// <param name="pageIndex"></param>
        /// <param name="recordCount"></param>
        /// <param name="companyId">公司编号</param>
        /// <returns></returns>
        IList<Model.CompanyStructure.SysRoleManage> GetList(int pageSize, int pageIndex, ref int recordCount, int companyId);
        /// <summary>
        /// 获取角色信息实体
        /// </summary>
        /// <param name="companyId">公司编号</param>
        /// <param name="id">角色编号</param>
        /// <returns></returns>
        Model.CompanyStructure.SysRoleManage GetModel(int companyId, int id);
        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="model">角色信息实体</param>
        /// <returns></returns>
        bool Add(Model.CompanyStructure.SysRoleManage model);
        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="model">角色信息实体</param>
        /// <returns></returns>
        bool Update(Model.CompanyStructure.SysRoleManage model);

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="companyId"></param>
        /// <param name="roleId">角色ID</param>
        /// <returns></returns>
        bool Delete(int companyId, params int[] roleId);
    }
}
