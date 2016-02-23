using System;
using System.Collections.Generic;
using Microsoft.Practices.EnterpriseLibrary.Data;
using System.Data.Common;
using System.Data;
using EyouSoft.Toolkit.DAL;

namespace EyouSoft.DAL.CompanyStructure
{
    /// <summary>
    /// 公司部门信息
    /// </summary>
    public class Department : DALBase, IDAL.CompanyStructure.IDepartment
    {
        #region static constants
        private const string SqlInsertDepartment = " insert into tbl_CompanyDepartment (DepartName,PrevDepartId,DepartManger,ContactTel,ContactFax,Remark,CompanyId,OperatorId,IssueTime) values(@DepartName,@PrevDepartId,@DepartManger,@ContactTel,@ContactFax,@Remark,@CompanyId,@OperatorId,@IssueTime); select @@identity; ";
        private const string SqlUpdateDepartment = " update tbl_CompanyDepartment set DepartName = @DepartName,DepartManger=@DepartManger,ContactTel=@ContactTel,ContactFax=@ContactFax,Remark=@Remark,OperatorId=@OperatorId where Id = @Id ";
        private const string SqlSelectDepartment = " select Id,DepartName,PrevDepartId,DepartManger,ContactTel,ContactFax,Remark,CompanyId,OperatorId,IssueTime from tbl_CompanyDepartment where Id = @Id";
        private const string SqlDeleteDepartment = " delete from tbl_CompanyDepartment where Id = @Id";
        private const string SqlSelectGetList = "select Id,DepartName,PrevDepartId,DepartManger,ContactTel,ContactFax,Remark,CompanyId,OperatorId,IssueTime from tbl_CompanyDepartment where CompanyId = @CompanyId and ";
        private const string SqlSelectGetAllDept = "select Id,DepartName,PrevDepartId,DepartManger,ContactTel,ContactFax,Remark,CompanyId,OperatorId,IssueTime from tbl_CompanyDepartment where CompanyId = @CompanyId ";
        private const string SqlGetIdByPid = " select Id from tbl_CompanyDepartment where PrevDepartId = @PrevDepartId and CompanyId = @CompanyId";
        private const string SqlHasNextLev = "select count(*) from tbl_CompanyDepartment where PrevDepartId = @Id ";
        private const string SqlHasDeptUser = "select count(*) from tbl_CompanyUser where DepartID = @DepartID and CompanyId = @CompanyId and IsDelete = '0' ";

        private readonly Database _db;
        #endregion

        #region 构造函数
        public Department()
        {
            this._db = SystemStore;
        }
        #endregion

        #region IDepartment 成员

        /// <summary>
        /// 添加部门信息
        /// </summary>
        /// <param name="model">部门实体</param>
        /// <returns>true:成功 false:失败</returns>
        public bool Add(Model.CompanyStructure.Department model)
        {
            DbCommand cmd = this._db.GetSqlStringCommand(SqlInsertDepartment);

            this._db.AddInParameter(cmd, "DepartName", DbType.String, model.DepartName);
            this._db.AddInParameter(cmd, "PrevDepartId", DbType.Int32, model.PrevDepartId);
            this._db.AddInParameter(cmd, "DepartManger", DbType.Int32, model.DepartManger);
            this._db.AddInParameter(cmd, "ContactTel", DbType.String, model.ContactTel);
            this._db.AddInParameter(cmd, "ContactFax", DbType.String, model.ContactFax);
            this._db.AddInParameter(cmd, "Remark", DbType.String, model.Remark);
            this._db.AddInParameter(cmd, "CompanyId", DbType.Int32, model.CompanyId);
            this._db.AddInParameter(cmd, "OperatorId", DbType.Int32, model.OperatorId);
            this._db.AddInParameter(cmd, "IssueTime", DbType.DateTime, DateTime.Now);

            object obj = DbHelper.GetSingle(cmd, this._db);

            if (obj == null)
            {
                model.Id = 0;
                return false;
            }

            model.Id = Toolkit.Utils.GetInt(obj.ToString());
            return true;
        }

        /// <summary>
        /// 修改部门信息
        /// </summary>
        /// <param name="model">部门实体</param>
        /// <returns>true:成功 false:失败</returns>
        public bool Update(Model.CompanyStructure.Department model)
        {
            DbCommand cmd = this._db.GetSqlStringCommand(SqlUpdateDepartment);
            this._db.AddInParameter(cmd, "Id", DbType.Int32, model.Id);
            this._db.AddInParameter(cmd, "DepartName", DbType.String, model.DepartName);
            this._db.AddInParameter(cmd, "DepartManger", DbType.Int32, model.DepartManger);
            this._db.AddInParameter(cmd, "ContactTel", DbType.String, model.ContactTel);
            this._db.AddInParameter(cmd, "ContactFax", DbType.String, model.ContactFax);
            this._db.AddInParameter(cmd, "Remark", DbType.String, model.Remark);
            this._db.AddInParameter(cmd, "OperatorId", DbType.Int32, model.OperatorId);

            return DbHelper.ExecuteSql(cmd, this._db) > 0 ? true : false;
        }

        /// <summary>
        /// 获取公司部门实体
        /// </summary>
        /// <param name="id">主键编号</param>
        /// <returns></returns>
        public Model.CompanyStructure.Department GetModel(int id)
        {
            Model.CompanyStructure.Department departmentModel = null;
            DbCommand cmd = this._db.GetSqlStringCommand(SqlSelectDepartment);
            this._db.AddInParameter(cmd, "Id", DbType.Int32, id);

            using (IDataReader rdr = DbHelper.ExecuteReader(cmd, this._db))
            {
                if (rdr.Read())
                {
                    departmentModel = new Model.CompanyStructure.Department
                        {
                            Id = rdr.GetInt32(rdr.GetOrdinal("Id")),
                            DepartName = rdr.GetString(rdr.GetOrdinal("DepartName")),
                            DepartManger =
                                rdr.IsDBNull(rdr.GetOrdinal("DepartManger"))
                                    ? 0
                                    : rdr.GetInt32(rdr.GetOrdinal("DepartManger")),
                            PrevDepartId = rdr.GetInt32(rdr.GetOrdinal("PrevDepartId")),
                            ContactTel =
                                rdr.IsDBNull(rdr.GetOrdinal("ContactTel"))
                                    ? ""
                                    : rdr.GetString(rdr.GetOrdinal("ContactTel")),
                            ContactFax =
                                rdr.IsDBNull(rdr.GetOrdinal("ContactFax"))
                                    ? ""
                                    : rdr.GetString(rdr.GetOrdinal("ContactFax")),
                            Remark = rdr.IsDBNull(rdr.GetOrdinal("Remark")) ? "" : rdr.GetString(rdr.GetOrdinal("Remark")),
                            CompanyId = rdr.GetInt32(rdr.GetOrdinal("CompanyId")),
                            OperatorId = rdr.GetInt32(rdr.GetOrdinal("OperatorId")),
                            IssueTime = rdr.GetDateTime(rdr.GetOrdinal("IssueTime"))
                        };
                }
            }

            return departmentModel;
        }

        /// <summary>
        /// 删除部门信息
        /// </summary>
        /// <param name="id">主键集合</param>
        /// <returns>true:成功 false:失败</returns>
        public bool Delete(int id)
        {
            DbCommand cmd = this._db.GetSqlStringCommand(SqlDeleteDepartment);

            this._db.AddInParameter(cmd, "Id", DbType.Int32, id);

            return DbHelper.ExecuteSql(cmd, this._db) > 0 ? true : false;
        }

        /// <summary>
        /// 获取公司的所有部门信息
        /// </summary>
        /// <param name="companyId">公司编号</param>
        /// <param name="parentDepartId">父级部门编号</param>
        /// <returns>部门信息集合</returns>
        public IList<Model.CompanyStructure.Department> GetList(int companyId, int parentDepartId)
        {
            DbCommand cmd;
            IList<Model.CompanyStructure.Department> lsDepartment = new List<Model.CompanyStructure.Department>();
            string queryStr = string.Empty;

            //0为顶级部门
            if (parentDepartId == 0)
            {
                DbCommand cmd1 = this._db.GetSqlStringCommand(SqlGetIdByPid);
                this._db.AddInParameter(cmd1, "PrevDepartId", DbType.Int32, parentDepartId);
                this._db.AddInParameter(cmd1, "CompanyId", DbType.Int32, companyId);
                using (IDataReader rdr1 = DbHelper.ExecuteReader(cmd1, this._db))
                {
                    while (rdr1.Read())
                    {
                        parentDepartId = rdr1.GetInt32(rdr1.GetOrdinal("Id"));
                    }
                }
                queryStr = string.Format(" (PrevDepartId = {0} or PrevDepartId = 0)", parentDepartId);
            }
            else
            {
                queryStr = string.Format(" PrevDepartId = {0} ", parentDepartId);
            }

            cmd = this._db.GetSqlStringCommand(SqlSelectGetList + queryStr);
            this._db.AddInParameter(cmd, "CompanyId", DbType.Int32, companyId);

            using (IDataReader rdr = DbHelper.ExecuteReader(cmd, this._db))
            {
                while (rdr.Read())
                {
                    var dpartmentModel = new Model.CompanyStructure.Department
                        {
                            Id = rdr.GetInt32(rdr.GetOrdinal("Id")),
                            DepartName = rdr.GetString(rdr.GetOrdinal("DepartName")),
                            PrevDepartId = rdr.GetInt32(rdr.GetOrdinal("PrevDepartId")),
                            DepartManger = rdr.GetInt32(rdr.GetOrdinal("DepartManger")),
                            ContactTel =
                                rdr.IsDBNull(rdr.GetOrdinal("ContactTel"))
                                    ? ""
                                    : rdr.GetString(rdr.GetOrdinal("ContactTel")),
                            ContactFax =
                                rdr.IsDBNull(rdr.GetOrdinal("ContactFax"))
                                    ? ""
                                    : rdr.GetString(rdr.GetOrdinal("ContactFax")),
                            Remark = rdr.IsDBNull(rdr.GetOrdinal("Remark")) ? "" : rdr.GetString(rdr.GetOrdinal("Remark")),
                            CompanyId = rdr.GetInt32(rdr.GetOrdinal("CompanyId")),
                            OperatorId = rdr.GetInt32(rdr.GetOrdinal("OperatorId")),
                            IssueTime = rdr.GetDateTime(rdr.GetOrdinal("IssueTime"))
                        };
                    dpartmentModel.HasNextLev = HasChildDept(dpartmentModel.Id);
                    lsDepartment.Add(dpartmentModel);
                }
            }

            return lsDepartment;
        }

        /// <summary>
        /// 获取所有部门信息
        /// </summary>
        /// <param name="companyId"></param>
        /// <returns></returns>
        public IList<Model.CompanyStructure.Department> GetAllDept(int companyId)
        {
            IList<Model.CompanyStructure.Department> lsDepartment = new List<Model.CompanyStructure.Department>();

            DbCommand cmd = this._db.GetSqlStringCommand(SqlSelectGetAllDept);
            this._db.AddInParameter(cmd, "CompanyId", DbType.Int32, companyId);

            using (IDataReader rdr = DbHelper.ExecuteReader(cmd, this._db))
            {
                while (rdr.Read())
                {
                    var dpartmentModel = new Model.CompanyStructure.Department
                        {
                            Id = rdr.GetInt32(rdr.GetOrdinal("Id")),
                            DepartName = rdr.GetString(rdr.GetOrdinal("DepartName")),
                            PrevDepartId = rdr.GetInt32(rdr.GetOrdinal("PrevDepartId")),
                            DepartManger = rdr.GetInt32(rdr.GetOrdinal("DepartManger")),
                            ContactTel =
                                rdr.IsDBNull(rdr.GetOrdinal("ContactTel"))
                                    ? ""
                                    : rdr.GetString(rdr.GetOrdinal("ContactTel")),
                            ContactFax =
                                rdr.IsDBNull(rdr.GetOrdinal("ContactFax"))
                                    ? ""
                                    : rdr.GetString(rdr.GetOrdinal("ContactFax")),
                            Remark = rdr.IsDBNull(rdr.GetOrdinal("Remark")) ? "" : rdr.GetString(rdr.GetOrdinal("Remark")),
                            CompanyId = rdr.GetInt32(rdr.GetOrdinal("CompanyId")),
                            OperatorId = rdr.GetInt32(rdr.GetOrdinal("OperatorId")),
                            IssueTime = rdr.GetDateTime(rdr.GetOrdinal("IssueTime"))
                        };
                    dpartmentModel.HasNextLev = HasChildDept(dpartmentModel.Id);
                    lsDepartment.Add(dpartmentModel);
                }
            }

            return lsDepartment;
        }

        #endregion

        /// <summary>
        /// 是否有下级部门
        /// </summary>
        /// <param name="id">部门编号</param>
        /// <returns></returns>
        public bool HasChildDept(int id)
        {
            bool isExists = false;
            DbCommand cmd = this._db.GetSqlStringCommand(SqlHasNextLev);

            this._db.AddInParameter(cmd, "Id", DbType.String, id);

            using (IDataReader rdr = DbHelper.ExecuteReader(cmd, this._db))
            {
                if (rdr.Read())
                {
                    isExists = rdr.GetInt32(0) > 0 ? true : false;
                }
            }

            return isExists;
        }

        /// <summary>
        /// 判断该部门下是否有员工
        /// </summary>
        /// <param name="id">部门编号</param>
        /// <param name="companyId">公司编号</param>
        /// <returns></returns>
        public bool HasDeptUser(int id, int companyId)
        {
            bool isExists = false;
            DbCommand cmd = this._db.GetSqlStringCommand(SqlHasDeptUser);

            this._db.AddInParameter(cmd, "DepartID", DbType.Int32, id);
            this._db.AddInParameter(cmd, "CompanyId", DbType.Int32, companyId);

            using (IDataReader rdr = DbHelper.ExecuteReader(cmd, this._db))
            {
                if (rdr.Read())
                {
                    isExists = rdr.GetInt32(0) > 0 ? true : false;
                }
            }

            return isExists;
        }
    }
}
