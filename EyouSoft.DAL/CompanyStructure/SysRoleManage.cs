using System.Data;
using System.Data.Common;
using System.Collections.Generic;
using System.Text;
using Microsoft.Practices.EnterpriseLibrary.Data;
using EyouSoft.Toolkit.DAL;

namespace EyouSoft.DAL.CompanyStructure
{
    /// <summary>
    /// 专线商角色管理DAL
    /// </summary>
    public class SysRoleManage : DALBase, IDAL.CompanyStructure.ISysRoleManage
    {
        private readonly Database _db;

        #region 构造函数
        /// <summary>
        /// 构造函数
        /// </summary>
        public SysRoleManage()
        {
            _db = this.SystemStore;
        }
        #endregion 构造函数

        /// <summary>
        /// 获取公司职务信息集合
        /// </summary>
        /// <param name="pageSize"></param>
        /// <param name="pageIndex"></param>
        /// <param name="recordCount"></param>
        /// <param name="companyId">公司编号</param>
        /// <returns></returns>
        public IList<Model.CompanyStructure.SysRoleManage> GetList(int pageSize, int pageIndex, ref int recordCount, int companyId)
        {
            if (pageSize <= 0 || pageIndex <= 0 || companyId <= 0) return null;

            IList<Model.CompanyStructure.SysRoleManage> resultList;
            string tableName = "tbl_SysRoleManage";
            string fields = "[Id],[RoleChilds],[RoleName]";
            string query = string.Format("[CompanyId]={0} AND IsDelete='0' ", companyId);
            string orderByString = " [id] ASC";
            using (IDataReader dr = DbHelper.ExecuteReader1(_db, pageSize, pageIndex, ref recordCount, tableName, fields, query
                , orderByString, string.Empty))
            {
                resultList = new List<Model.CompanyStructure.SysRoleManage>();
                while (dr.Read())
                {
                    var model = new Model.CompanyStructure.SysRoleManage
                        {
                            Id = dr.GetInt32(dr.GetOrdinal("Id")),
                            RoleChilds = dr.GetString(dr.GetOrdinal("RoleChilds")),
                            RoleName = dr.GetString(dr.GetOrdinal("RoleName"))
                        };
                    resultList.Add(model);
                }
            };
            return resultList;
        }

        /// <summary>
        /// 获取角色信息实体
        /// </summary>
        /// <param name="companyId">公司编号</param>
        /// <param name="id">角色编号</param>
        /// <returns></returns>
        public Model.CompanyStructure.SysRoleManage GetModel(int companyId, int id)
        {
            if (id <= 0 || companyId <= 0) return null;

            var strSql = new StringBuilder();
            strSql.Append(" select * from tbl_SysRoleManage where CompanyId = @CompanyId and Id = @Id ");

            DbCommand dc = _db.GetSqlStringCommand(strSql.ToString());

            _db.AddInParameter(dc, "CompanyId", DbType.Int32, companyId);
            _db.AddInParameter(dc, "Id", DbType.Int32, id);

            Model.CompanyStructure.SysRoleManage model = null;
            using (IDataReader dr = DbHelper.ExecuteReader(dc, _db))
            {
                if (dr.Read())
                {
                    model = new Model.CompanyStructure.SysRoleManage
                        {
                            CompanyId = companyId,
                            Id = id,
                            RoleChilds =
                                dr.IsDBNull(dr.GetOrdinal("RoleChilds"))
                                    ? string.Empty
                                    : dr.GetString(dr.GetOrdinal("RoleChilds")),
                            RoleName =
                                dr.IsDBNull(dr.GetOrdinal("RoleName"))
                                    ? string.Empty
                                    : dr.GetString(dr.GetOrdinal("RoleName"))
                        };
                }
            }

            return model;
        }

        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="model">角色信息实体</param>
        /// <returns></returns>
        public bool Add(Model.CompanyStructure.SysRoleManage model)
        {
            if (model == null || model.CompanyId <= 0 || string.IsNullOrEmpty(model.RoleName)) return false;

            var strSql = new StringBuilder();
            strSql.Append(@" INSERT INTO [tbl_SysRoleManage]
                                ([RoleName],[RoleChilds],[CompanyId],[IsDelete])
                             VALUES
                                (@RoleName,@RoleChilds,@CompanyId,@IsDelete);
                            select @@identity ;");
            DbCommand dc = _db.GetSqlStringCommand(strSql.ToString());
            _db.AddInParameter(dc, "RoleName", DbType.String, model.RoleName);
            _db.AddInParameter(dc, "RoleChilds", DbType.String, model.RoleChilds);
            _db.AddInParameter(dc, "CompanyId", DbType.Int32, model.CompanyId);
            _db.AddInParameter(dc, "IsDelete", DbType.AnsiStringFixedLength, "0");

            object obj = DbHelper.GetSingle(dc, _db);
            if (obj == null || Toolkit.Utils.GetInt(obj.ToString()) <= 0)
            {
                model.Id = 0;
                return false;
            }

            model.Id = Toolkit.Utils.GetInt(obj.ToString());
            return true;
        }

        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="model">角色信息实体</param>
        /// <returns></returns>
        public bool Update(Model.CompanyStructure.SysRoleManage model)
        {
            if (model == null || model.CompanyId <= 0 || model.Id <= 0 || string.IsNullOrEmpty(model.RoleName)) return false;

            var strSql = new StringBuilder();
            strSql.Append(" declare @oldRoleChilds nvarchar(max); ");
            strSql.Append(" SELECT @oldRoleChilds = RoleChilds FROM tbl_SysRoleManage WHERE [Id] = @Id AND CompanyId = @CompanyId; ");
            strSql.Append(
                " UPDATE tbl_SysRoleManage SET RoleChilds = @RoleChilds,RoleName = @RoleName WHERE [Id] = @Id AND CompanyId = @CompanyId; ");
            strSql.Append(" if @oldRoleChilds <> @RoleChilds ");
            strSql.Append(" begin ");
            strSql.Append(" UPDATE tbl_CompanyUser SET PermissionList = @RoleChilds WHERE [RoleID] = @Id AND CompanyId = @CompanyId; ");
            strSql.Append(" end ");

            DbCommand dc = _db.GetSqlStringCommand(strSql.ToString());
            _db.AddInParameter(dc, "RoleName", DbType.String, model.RoleName);
            _db.AddInParameter(dc, "RoleChilds", DbType.String, model.RoleChilds);
            _db.AddInParameter(dc, "CompanyId", DbType.Int32, model.CompanyId);
            _db.AddInParameter(dc, "Id", DbType.Int32, model.Id);

            return DbHelper.ExecuteSqlTrans(dc, _db) > 0 ? true : false;
        }

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="companyId"></param>
        /// <param name="roleId">角色ID</param>
        /// <returns></returns>
        public bool Delete(int companyId, params int[] roleId)
        {
            if (companyId <= 0 || roleId == null || roleId.Length <= 0) return false;

            var strSql = new StringBuilder();
            strSql.Append(" UPDATE tbl_SysRoleManage SET IsDelete = '1' WHERE CompanyId = @CompanyId ");
            if (roleId.Length == 1)
            {
                strSql.AppendFormat(" and [Id] = {0} ", roleId[0]);
            }
            else
            {
                strSql.AppendFormat(" and [Id] in ({0}) ", GetIdsByArr(roleId));
            }

            DbCommand dc = _db.GetSqlStringCommand(strSql.ToString());
            _db.AddInParameter(dc, "CompanyId", DbType.Int32, companyId);

            return DbHelper.ExecuteSql(dc, _db) > 0 ? true : false;
        }
    }
}
