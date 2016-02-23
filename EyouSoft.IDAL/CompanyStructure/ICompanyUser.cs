using System.Collections.Generic;

namespace EyouSoft.IDAL.CompanyStructure
{
    /// <summary>
    /// 用户信息数据层接口
    /// </summary>
    public interface ICompanyUser
    {
        /// <summary>
        /// 判断E-MAIL是否已存在
        /// </summary>
        /// <param name="email">email地址</param>
        /// <param name="userId">当前修改Email的用户ID</param>
        /// <returns></returns>
        bool IsExistsEmail(string email, int userId);
        /// <summary>
        /// 判断用户名是否已存在
        /// </summary>
        /// <param name="userName">用户名</param>
        /// <param name="companyId">当前公司编号</param>
        /// <param name="id">编号</param>
        /// <returns></returns>
        bool IsExists(int id, string userName, int companyId);

        /// <summary>
        /// 验证公司是否可以添加子账户
        /// </summary>
        /// <param name="companyId">公司编号</param>
        /// <returns>
        /// true：可以添加
        /// false：不能添加，子账号已满
        /// </returns>
        bool IsAddUser(int companyId);

        /// <summary>
        /// 真实删除用户信息
        /// </summary>
        /// <param name="userIdList">用户ID列表</param>
        /// <returns></returns>
        bool Delete(params int[] userIdList);
        /// <summary>
        /// 移除用户(即虚拟删除用户)
        /// </summary>
        /// <param name="userIdList">用户ID列表</param>
        /// <returns></returns>
        bool Remove(params int[] userIdList);
        /// <summary>
        /// 添加用户信息
        /// </summary>
        /// <param name="model">用户信息实体</param>
        /// <returns>true:成功 false:失败</returns>
        bool Add(Model.CompanyStructure.CompanyUser model);
        /// <summary>
        /// 修改用户信息
        /// </summary>
        /// <param name="model">用户信息实体</param>
        /// <returns>true:成功 false:失败</returns>
        bool Update(Model.CompanyStructure.CompanyUser model);
        /// <summary>
        /// 根据用户编号获取用户信息
        /// </summary>
        /// <param name="userId">用户编号</param>
        /// <returns>用户实体</returns>
        Model.CompanyStructure.CompanyUser GetUserInfo(int userId);
        /// <summary>
        /// 根据用户名密码获取用户信息
        /// </summary>
        /// <param name="userName"></param>
        /// <param name="pwd"></param>
        /// <returns></returns>
        Model.CompanyStructure.CompanyUser GetUserInfo(string userName, string pwd);
        /// <summary>
        /// 修改密码
        /// </summary>
        /// <param name="id">用户ID</param>
        /// <param name="password">密码实体类</param>
        /// <returns></returns>
        bool UpdatePassWord(int id, Model.CompanyStructure.PassWord password);
        /// <summary>
        /// 获得管理员实体信息
        /// </summary>
        /// <param name="companyId">公司ID</param>
        /// <returns></returns>
        Model.CompanyStructure.CompanyUser GetAdminModel(int companyId);
        /// <summary>
        /// 获取指定公司下的所有帐号用户详细信息列表[注;不包括总帐号]
        /// </summary>
        /// <param name="companyId">公司ID</param>
        /// <param name="pageSize">分页大小</param>
        /// <param name="pageIndex">当前页</param>
        /// <param name="recordCount">总的记录数</param>
        /// <param name="model">查询实体</param>
        /// <returns></returns>
        IList<Model.CompanyStructure.CompanyUser> GetList(int companyId, int pageSize, int pageIndex, ref int recordCount
            , Model.CompanyStructure.QueryCompanyUser model);
        /// <summary>
        /// 根据当前用户组织架构信息分页获取用户列表
        /// </summary>
        /// <param name="companyId">公司编号</param>
        /// <param name="model">查询实体</param>
        /// <returns></returns>
        IList<Model.CompanyStructure.CompanyUser> GetList(int companyId, Model.CompanyStructure.QueryCompanyUser model);
        
        /// <summary>
        /// 状态设置
        /// </summary>
        /// <param name="id">用户ID</param>
        /// <param name="status">用户状态</param>
        /// <returns></returns>
        bool SetEnable(int id, Model.EnumType.CompanyStructure.UserStatus status);

        /// <summary>
        /// 设置用户权限
        /// </summary>
        /// <param name="userId">用户编号</param>
        /// <param name="roleId">角色编号</param>
        /// <param name="permissionList">权限集合</param>
        /// <returns>是否成功</returns>
        bool SetPermission(int userId, int roleId, params string[] permissionList);

        /// <summary>
        /// 简单修改用户基本信息
        /// </summary>
        /// <param name="model">用户信息实体</param>
        /// <returns></returns>
        bool SimpleUpdate(Model.CompanyStructure.CompanyUser model);
    }
}
