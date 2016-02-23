using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Practices.EnterpriseLibrary.Data;
using EyouSoft.Toolkit.DAL;
using System.Data.Common;
using System.Data;

namespace EyouSoft.DAL.CompanyStructure
{
    /// <summary>
    /// 用户信息数据层
    /// </summary>
    public class CompanyUser : DALBase, IDAL.CompanyStructure.ICompanyUser
    {
        #region 变量

        private const string SqlCompanyUserRemove = "update tbl_CompanyUser set IsDelete = '1' where Id in({0}) AND IsAdmin = '0' ";
        private const string SqlCompanyUserDelete = " delete tbl_CompanyUser where Id in({0}) ";
        private const string SqlSetUserEnable = " update tbl_CompanyUser set UserStatus = @UserStatus where Id = @Id;";

        private Database _db;

        #endregion

        #region 构造函数
        /// <summary>
        /// 构造函数
        /// </summary>
        public CompanyUser()
        {
            _db = this.SystemStore;


        }
        #endregion

        #region ICompanyUser 成员

        /// <summary>
        /// 判断E-MAIL是否已存在
        /// </summary>
        /// <param name="email">email地址</param>
        /// <param name="userId">当前修改Email的用户ID</param>
        /// <returns></returns>
        public bool IsExistsEmail(string email, int userId)
        {
            if (string.IsNullOrEmpty(email)) return false;

            var strSql = new StringBuilder();
            strSql.Append(" select count(Id) from tbl_CompanyUser where ContactEmail = @ContactEmail ");
            if (userId > 0) strSql.AppendFormat(" and Id <> {0} ", userId);
            DbCommand dc = _db.GetSqlStringCommand(strSql.ToString());
            _db.AddInParameter(dc, "ContactEmail", DbType.String, email);

            var obj = DbHelper.GetSingle(dc, _db);
            return obj == null || Toolkit.Utils.GetInt(obj.ToString()) <= 0 ? false : true;
        }
        /// <summary>
        /// 判断用户名是否已存在
        /// </summary>
        /// <param name="userName">用户名</param>
        /// <param name="companyId">当前公司编号</param>
        /// <param name="id">用户编号</param>
        /// <returns></returns>
        public bool IsExists(int id, string userName, int companyId)
        {
            if (string.IsNullOrEmpty(userName) || companyId <= 0) return false;

            var strSql = new StringBuilder();
            strSql.Append(" select count(Id) from tbl_CompanyUser where UserName = @UserName and CompanyId = @CompanyId ");
            if (id > 0) strSql.AppendFormat(" and Id <> {0} ", id);
            DbCommand dc = _db.GetSqlStringCommand(strSql.ToString());
            _db.AddInParameter(dc, "UserName", DbType.String, userName);
            _db.AddInParameter(dc, "CompanyId", DbType.Int32, companyId);

            var obj = DbHelper.GetSingle(dc, _db);
            return obj == null || Toolkit.Utils.GetInt(obj.ToString()) <= 0 ? false : true;
        }
        /// <summary>
        /// 真实删除用户信息
        /// </summary>
        /// <param name="userIdList">用户ID列表</param>
        /// <returns></returns>
        public bool Delete(params int[] userIdList)
        {
            if (userIdList == null || userIdList.Length <= 0) return false;

            DbCommand dc = this._db.GetSqlStringCommand(string.Format(SqlCompanyUserDelete, GetIdsByArr(userIdList)));
            return DbHelper.ExecuteSql(dc, this._db) > 0 ? true : false;
        }
        /// <summary>
        /// 移除用户(即虚拟删除用户)
        /// </summary>
        /// <param name="userIdList">用户ID列表</param>
        /// <returns></returns>
        public bool Remove(params int[] userIdList)
        {
            if (userIdList == null || userIdList.Length <= 0) return false;

            DbCommand dc = this._db.GetSqlStringCommand(string.Format(SqlCompanyUserRemove, GetIdsByArr(userIdList)));
            return DbHelper.ExecuteSql(dc, this._db) > 0 ? true : false;
        }
        /// <summary>
        /// 添加用户信息
        /// </summary>
        /// <param name="model">用户信息实体</param>
        /// <returns>true:成功 false:失败</returns>
        public bool Add(Model.CompanyStructure.CompanyUser model)
        {
            if (model == null || model.PassWordInfo == null || model.PersonInfo == null) return false;

            var strSql = new StringBuilder();

            #region sql 拼接

            strSql.Append(" declare @userId int; ");
            strSql.Append(
                @" INSERT INTO [tbl_CompanyUser]
                    ([CompanyId],[UserName],[Password],[MD5Password],[ContactName],[ContactSex],[ContactTel],[ContactFax],[ContactMobile]
                    ,[ContactEmail],[QQ],[MSN],[JobName],[RoleID],[PermissionList],[PeopProfile],[Remark]
                    ,[IsDelete],[UserStatus],[IsAdmin],[IssueTime],[DepartId],[SuperviseDepartId],[OnlineStatus],[OnlineSessionId]
                    ,[Birthday],[UserType],[Address]) ");
            strSql.Append(" VALUES ");
            strSql.Append(
                @" (@CompanyId,@UserName,@Password,@MD5Password,@ContactName,@ContactSex,@ContactTel,@ContactFax,@ContactMobile
                    ,@ContactEmail,@QQ,@MSN,@JobName,@RoleID,@PermissionList,@PeopProfile,@Remark
                    ,@IsDelete,@UserStatus,@IsAdmin,@IssueTime,@DepartId,@SuperviseDepartId,@OnlineStatus,@OnlineSessionId
                    ,@Birthday,@UserType,@Address); ");
            strSql.Append(" select @userId = @@identity; ");
            strSql.Append(" select @userId; ");

            #endregion

            DbCommand dc = _db.GetSqlStringCommand(strSql.ToString());

            #region 参数 赋值

            _db.AddInParameter(dc, "CompanyId", DbType.Int32, model.CompanyId);
            _db.AddInParameter(dc, "UserName", DbType.String, model.UserName);
            _db.AddInParameter(dc, "Password", DbType.String, model.PassWordInfo.NoEncryptPassword);
            _db.AddInParameter(dc, "MD5Password", DbType.String, model.PassWordInfo.MD5Password);
            _db.AddInParameter(dc, "ContactName", DbType.String, model.PersonInfo.ContactName);
            _db.AddInParameter(dc, "ContactSex", DbType.AnsiStringFixedLength, (int)model.PersonInfo.ContactSex);
            _db.AddInParameter(dc, "ContactTel", DbType.String, model.PersonInfo.ContactTel);
            _db.AddInParameter(dc, "ContactFax", DbType.String, model.PersonInfo.ContactFax);
            _db.AddInParameter(dc, "ContactMobile", DbType.String, model.PersonInfo.ContactMobile);
            _db.AddInParameter(dc, "ContactEmail", DbType.String, model.PersonInfo.ContactEmail);
            _db.AddInParameter(dc, "QQ", DbType.String, model.PersonInfo.QQ);
            _db.AddInParameter(dc, "MSN", DbType.String, model.PersonInfo.MSN);
            _db.AddInParameter(dc, "JobName", DbType.String, model.PersonInfo.JobName);
            _db.AddInParameter(dc, "RoleID", DbType.Int32, model.RoleID);
            _db.AddInParameter(dc, "PermissionList", DbType.String, model.PermissionList);
            _db.AddInParameter(dc, "PeopProfile", DbType.String, model.PersonInfo.PeopProfile);
            _db.AddInParameter(dc, "Remark", DbType.String, model.PersonInfo.Remark);
            _db.AddInParameter(dc, "IsDelete", DbType.AnsiStringFixedLength, '0');
            _db.AddInParameter(dc, "UserStatus", DbType.Byte, (int)Model.EnumType.CompanyStructure.UserStatus.正常);
            _db.AddInParameter(dc, "IsAdmin", DbType.AnsiStringFixedLength, model.IsAdmin ? "1" : "0");
            _db.AddInParameter(dc, "IssueTime", DbType.DateTime, DateTime.Now);
            _db.AddInParameter(dc, "DepartId", DbType.Int32, model.DepartId);
            _db.AddInParameter(dc, "SuperviseDepartId", DbType.Int32, model.SuperviseDepartId);
            _db.AddInParameter(dc, "OnlineStatus", DbType.Byte, (int)Model.EnumType.CompanyStructure.UserOnlineStatus.Offline);
            _db.AddInParameter(dc, "OnlineSessionId", DbType.AnsiStringFixedLength, string.Empty);
            if (model.PersonInfo.Birthday.HasValue)
            {
                _db.AddInParameter(dc, "Birthday", DbType.DateTime, model.PersonInfo.Birthday.Value);
            }
            else
            {
                _db.AddInParameter(dc, "Birthday", DbType.DateTime, DBNull.Value);
            }
            _db.AddInParameter(dc, "UserType", DbType.Byte, (int)model.UserType);
            _db.AddInParameter(dc, "Address", DbType.String, model.PersonInfo.Address);

            #endregion

            object obj = DbHelper.GetSingle(dc, _db);
            if (obj == null || Toolkit.Utils.GetInt(obj.ToString()) <= 0)
            {
                return false;
            }

            model.ID = Toolkit.Utils.GetInt(obj.ToString());
            return true;
        }
        /// <summary>
        /// 修改用户基本信息[不更改密码]
        /// </summary>
        /// <param name="model">用户信息实体</param>
        /// <returns>true:成功 false:失败</returns>
        public bool Update(Model.CompanyStructure.CompanyUser model)
        {
            if (model == null || model.PassWordInfo == null || model.PersonInfo == null || model.ID <= 0) return false;

            var strSql = new StringBuilder();

            #region sql 拼接

            strSql.Append(
                @" UPDATE [tbl_CompanyUser] SET 
                [UserName] = @UserName,[Password] = @Password,[MD5Password] = @MD5Password,[ContactName] = @ContactName,
                [ContactSex] = @ContactSex,[ContactTel] = @ContactTel,[ContactFax] = @ContactFax,[ContactMobile] = @ContactMobile,
                [ContactEmail] = @ContactEmail,[QQ] = @QQ,[MSN] = @MSN,[JobName] = @JobName,[PeopProfile] = @PeopProfile,
                [Remark] = @Remark,[DepartId] = @DepartId,[SuperviseDepartId] = @SuperviseDepartId,[Birthday] = @Birthday,[Address] = @Address ");
            strSql.Append("  WHERE Id = @Id; ");

            #endregion

            DbCommand dc = _db.GetSqlStringCommand(strSql.ToString());

            #region 参数 赋值

            _db.AddInParameter(dc, "UserName", DbType.String, model.UserName);
            _db.AddInParameter(dc, "Password", DbType.String, model.PassWordInfo.NoEncryptPassword);
            _db.AddInParameter(dc, "MD5Password", DbType.String, model.PassWordInfo.MD5Password);
            _db.AddInParameter(dc, "ContactName", DbType.String, model.PersonInfo.ContactName);
            _db.AddInParameter(dc, "ContactSex", DbType.AnsiStringFixedLength, (int)model.PersonInfo.ContactSex);
            _db.AddInParameter(dc, "ContactTel", DbType.String, model.PersonInfo.ContactTel);
            _db.AddInParameter(dc, "ContactFax", DbType.String, model.PersonInfo.ContactFax);
            _db.AddInParameter(dc, "ContactMobile", DbType.String, model.PersonInfo.ContactMobile);
            _db.AddInParameter(dc, "ContactEmail", DbType.String, model.PersonInfo.ContactEmail);
            _db.AddInParameter(dc, "QQ", DbType.String, model.PersonInfo.QQ);
            _db.AddInParameter(dc, "MSN", DbType.String, model.PersonInfo.MSN);
            _db.AddInParameter(dc, "JobName", DbType.String, model.PersonInfo.JobName);
            _db.AddInParameter(dc, "PeopProfile", DbType.String, model.PersonInfo.PeopProfile);
            _db.AddInParameter(dc, "Remark", DbType.String, model.PersonInfo.Remark);
            _db.AddInParameter(dc, "DepartId", DbType.Int32, model.DepartId);
            _db.AddInParameter(dc, "SuperviseDepartId", DbType.Int32, model.SuperviseDepartId);
            _db.AddInParameter(dc, "Id", DbType.Int32, model.ID);
            if (model.PersonInfo.Birthday.HasValue)
            {
                _db.AddInParameter(dc, "Birthday", DbType.DateTime, model.PersonInfo.Birthday.Value);
            }
            else
            {
                _db.AddInParameter(dc, "Birthday", DbType.DateTime, DBNull.Value);
            }
            _db.AddInParameter(dc, "Address", DbType.String, model.PersonInfo.Address);

            #endregion

            return DbHelper.ExecuteSqlTrans(dc, _db) > 0 ? true : false;
        }

        /// <summary>
        /// 简单修改用户基本信息
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public bool SimpleUpdate(Model.CompanyStructure.CompanyUser model)
        {
            if (model == null || model.PassWordInfo == null || model.PersonInfo == null || model.ID <= 0) return false;

            var strSql = new StringBuilder();

            #region sql 拼接

            strSql.Append(
                @" UPDATE [tbl_CompanyUser] SET 
                [Password] = @Password,[MD5Password] = @MD5Password,[ContactName] = @ContactName,
                [ContactSex] = @ContactSex,[ContactTel] = @ContactTel,[ContactFax] = @ContactFax,[ContactMobile] = @ContactMobile,
                [ContactEmail] = @ContactEmail,[QQ] = @QQ,[MSN] = @MSN,
                [Birthday] = @Birthday ");
            strSql.Append("  WHERE Id = @Id; ");

            #endregion

            DbCommand dc = _db.GetSqlStringCommand(strSql.ToString());

            #region 参数 赋值

            _db.AddInParameter(dc, "Password", DbType.String, model.PassWordInfo.NoEncryptPassword);
            _db.AddInParameter(dc, "MD5Password", DbType.String, model.PassWordInfo.MD5Password);
            _db.AddInParameter(dc, "ContactName", DbType.String, model.PersonInfo.ContactName);
            _db.AddInParameter(dc, "ContactSex", DbType.AnsiStringFixedLength, (int)model.PersonInfo.ContactSex);
            _db.AddInParameter(dc, "ContactTel", DbType.String, model.PersonInfo.ContactTel);
            _db.AddInParameter(dc, "ContactFax", DbType.String, model.PersonInfo.ContactFax);
            _db.AddInParameter(dc, "ContactMobile", DbType.String, model.PersonInfo.ContactMobile);
            _db.AddInParameter(dc, "ContactEmail", DbType.String, model.PersonInfo.ContactEmail);
            _db.AddInParameter(dc, "QQ", DbType.String, model.PersonInfo.QQ);
            _db.AddInParameter(dc, "MSN", DbType.String, model.PersonInfo.MSN);
            _db.AddInParameter(dc, "Id", DbType.Int32, model.ID);
            if (model.PersonInfo.Birthday.HasValue)
            {
                _db.AddInParameter(dc, "Birthday", DbType.DateTime, model.PersonInfo.Birthday.Value);
            }
            else
            {
                _db.AddInParameter(dc, "Birthday", DbType.DateTime, DBNull.Value);
            }

            #endregion

            return DbHelper.ExecuteSqlTrans(dc, _db) > 0 ? true : false;
        }

        /// <summary>
        /// 修改密码
        /// </summary>
        /// <param name="id">用户编号</param>
        /// <param name="password">密码实体</param>
        /// <returns>true:成功 false:失败</returns>
        public bool UpdatePassWord(int id, Model.CompanyStructure.PassWord password)
        {
            if (id <= 0 || password == null || string.IsNullOrEmpty(password.NoEncryptPassword)
                || string.IsNullOrEmpty(password.MD5Password)) return false;

            DbCommand dc =
                _db.GetSqlStringCommand(
                    " Update tbl_CompanyUser set Password = @Password,MD5Password = @MD5Password where Id = @Id ");

            #region 参数 赋值

            _db.AddInParameter(dc, "Password", DbType.String, password.NoEncryptPassword);
            _db.AddInParameter(dc, "MD5Password", DbType.String, password.MD5Password);
            _db.AddInParameter(dc, "Id", DbType.Int32, id);

            #endregion

            return DbHelper.ExecuteSqlTrans(dc, _db) > 0 ? true : false;
        }
        /// <summary>
        /// 根据用户编号获取用户信息实体
        /// </summary>
        /// <param name="userId">用户编号</param>
        /// <returns>用户信息实体</returns>
        public Model.CompanyStructure.CompanyUser GetUserInfo(int userId)
        {
            if (userId <= 0) return null;

            var strSql = new StringBuilder();
            strSql.Append(
                " SELECT [Id],[CompanyId],[UserName],[Password],[MD5Password],[ContactName],[ContactSex],[ContactTel],[ContactFax],[ContactMobile],[ContactEmail],[QQ],[MSN],[JobName],[LastLoginIP],[LastLoginTime],[RoleID],[PermissionList],[PeopProfile],[Remark],[IsDelete],[UserStatus],[IsAdmin],[IssueTime],[DepartId],[SuperviseDepartId],[UserType],[Birthday],[Address],[SupplierCompanyId] ");
            strSql.Append(" ,(select DepartName from tbl_CompanyDepartment where tbl_CompanyUser.[DepartId] = tbl_CompanyDepartment.Id) as DepartName ");
            strSql.Append(" ,(select DepartName from tbl_CompanyDepartment where tbl_CompanyUser.[SuperviseDepartId] = tbl_CompanyDepartment.Id) as SuperviseDepartName ");
            strSql.Append(" FROM [tbl_CompanyUser] where [Id]=@Id;  ");

            DbCommand dc = _db.GetSqlStringCommand(strSql.ToString());
            _db.AddInParameter(dc, "Id", DbType.Int32, userId);

            Model.CompanyStructure.CompanyUser model = null;
            using (IDataReader dr = DbHelper.ExecuteReader(dc, _db))
            {
                if (dr.Read())
                {
                    model = new Model.CompanyStructure.CompanyUser();
                    model.PassWordInfo = new EyouSoft.Model.CompanyStructure.PassWord();
                    model.PersonInfo = new EyouSoft.Model.CompanyStructure.ContactPersonInfo();

                    #region 用户信息

                    if (!dr.IsDBNull(dr.GetOrdinal("Id"))) model.ID = dr.GetInt32(dr.GetOrdinal("Id"));
                    if (!dr.IsDBNull(dr.GetOrdinal("CompanyId"))) model.CompanyId = dr.GetInt32(dr.GetOrdinal("CompanyId"));
                    if (!dr.IsDBNull(dr.GetOrdinal("UserName"))) model.UserName = dr.GetString(dr.GetOrdinal("UserName"));
                    if (!dr.IsDBNull(dr.GetOrdinal("Password")))
                        model.PassWordInfo.NoEncryptPassword = dr.GetString(dr.GetOrdinal("Password"));
                    if (!dr.IsDBNull(dr.GetOrdinal("ContactName")))
                        model.PersonInfo.ContactName = dr.GetString(dr.GetOrdinal("ContactName"));
                    if (!dr.IsDBNull(dr.GetOrdinal("ContactSex")))
                        model.PersonInfo.ContactSex =
                            (Model.EnumType.CompanyStructure.Sex)
                            Toolkit.Utils.GetInt(dr.GetString(dr.GetOrdinal("ContactSex")));
                    if (!dr.IsDBNull(dr.GetOrdinal("ContactTel")))
                        model.PersonInfo.ContactTel = dr.GetString(dr.GetOrdinal("ContactTel"));
                    if (!dr.IsDBNull(dr.GetOrdinal("ContactFax")))
                        model.PersonInfo.ContactFax = dr.GetString(dr.GetOrdinal("ContactFax"));
                    if (!dr.IsDBNull(dr.GetOrdinal("ContactMobile")))
                        model.PersonInfo.ContactMobile = dr.GetString(dr.GetOrdinal("ContactMobile"));
                    if (!dr.IsDBNull(dr.GetOrdinal("ContactEmail")))
                        model.PersonInfo.ContactEmail = dr.GetString(dr.GetOrdinal("ContactEmail"));
                    if (!dr.IsDBNull(dr.GetOrdinal("QQ")))
                        model.PersonInfo.QQ = dr.GetString(dr.GetOrdinal("QQ"));
                    if (!dr.IsDBNull(dr.GetOrdinal("MSN")))
                        model.PersonInfo.MSN = dr.GetString(dr.GetOrdinal("MSN"));
                    if (!dr.IsDBNull(dr.GetOrdinal("JobName")))
                        model.PersonInfo.JobName = dr.GetString(dr.GetOrdinal("JobName"));
                    if (!dr.IsDBNull(dr.GetOrdinal("LastLoginIP"))) model.LastLoginIP = dr.GetString(dr.GetOrdinal("LastLoginIP"));
                    if (!dr.IsDBNull(dr.GetOrdinal("LastLoginTime"))) model.LastLoginTime = dr.GetDateTime(dr.GetOrdinal("LastLoginTime"));
                    if (!dr.IsDBNull(dr.GetOrdinal("RoleID"))) model.RoleID = dr.GetInt32(dr.GetOrdinal("RoleID"));
                    if (!dr.IsDBNull(dr.GetOrdinal("PermissionList"))) model.PermissionList = dr.GetString(dr.GetOrdinal("PermissionList"));
                    if (!dr.IsDBNull(dr.GetOrdinal("PeopProfile")))
                        model.PersonInfo.PeopProfile = dr.GetString(dr.GetOrdinal("PeopProfile"));
                    if (!dr.IsDBNull(dr.GetOrdinal("Remark")))
                        model.PersonInfo.Remark = dr.GetString(dr.GetOrdinal("Remark"));
                    if (!dr.IsDBNull(dr.GetOrdinal("IsDelete")))
                        model.IsDeleted = this.GetBoolean(dr.GetString(dr.GetOrdinal("IsDelete")));
                    if (!dr.IsDBNull(dr.GetOrdinal("UserStatus")))
                        model.UserStatus =
                            (Model.EnumType.CompanyStructure.UserStatus)
                            dr.GetByte(dr.GetOrdinal("UserStatus"));
                    if (!dr.IsDBNull(dr.GetOrdinal("IsAdmin")))
                        model.IsAdmin = this.GetBoolean(dr.GetString(dr.GetOrdinal("IsAdmin")));
                    if (!dr.IsDBNull(dr.GetOrdinal("IssueTime")))
                        model.IssueTime = dr.GetDateTime(dr.GetOrdinal("IssueTime"));
                    if (!dr.IsDBNull(dr.GetOrdinal("DepartId"))) model.DepartId = dr.GetInt32(dr.GetOrdinal("DepartId"));
                    if (!dr.IsDBNull(dr.GetOrdinal("SuperviseDepartId")))
                        model.SuperviseDepartId = dr.GetInt32(dr.GetOrdinal("SuperviseDepartId"));
                    if (!dr.IsDBNull(dr.GetOrdinal("Birthday"))) model.PersonInfo.Birthday = dr.GetDateTime(dr.GetOrdinal("Birthday"));
                    if (!dr.IsDBNull(dr.GetOrdinal("UserType")))
                        model.UserType = (Model.EnumType.CompanyStructure.UserType)dr.GetByte(dr.GetOrdinal("UserType"));
                    if (!dr.IsDBNull(dr.GetOrdinal("Address")))
                        model.PersonInfo.Address = dr.GetString(dr.GetOrdinal("Address"));
                    if (!dr.IsDBNull(dr.GetOrdinal("SupplierCompanyId")))
                        model.SupplierCompanyId = dr.GetString(dr.GetOrdinal("SupplierCompanyId"));
                    if (!dr.IsDBNull(dr.GetOrdinal("DepartName")))
                        model.DepartName = dr.GetString(dr.GetOrdinal("DepartName"));
                    if (!dr.IsDBNull(dr.GetOrdinal("SuperviseDepartName")))
                        model.SuperviseDepartName = dr.GetString(dr.GetOrdinal("SuperviseDepartName"));

                    #endregion
                }
            }

            return model;
        }

        /// <summary>
        /// 根据用户名及密码获取用户信息实体
        /// </summary>
        /// <param name="userName">用户名</param>
        /// <param name="pwd">MD5密码</param>
        /// <returns>用户信息实体</returns>
        public Model.CompanyStructure.CompanyUser GetUserInfo(string userName, string pwd)
        {
            if (string.IsNullOrEmpty(userName) || string.IsNullOrEmpty(pwd)) return null;

            var strSql = new StringBuilder();
            strSql.Append(
                " SELECT top 1 [Id],[CompanyId],[UserName],[Password],[MD5Password],[ContactName],[ContactSex],[ContactTel],[ContactFax],[ContactMobile],[ContactEmail],[QQ],[MSN],[JobName],[LastLoginIP],[LastLoginTime],[RoleID],[PermissionList],[PeopProfile],[Remark],[IsDelete],[UserStatus],[IsAdmin],[IssueTime],[DepartId],[SuperviseDepartId],[UserType],[Birthday],[Address],[SupplierCompanyId] FROM [tbl_CompanyUser] where UserName = @UserName and MD5Password = @MD5Password; ");

            DbCommand dc = _db.GetSqlStringCommand(strSql.ToString());
            _db.AddInParameter(dc, "UserName", DbType.String, userName);
            _db.AddInParameter(dc, "MD5Password", DbType.String, pwd);

            Model.CompanyStructure.CompanyUser model = null;
            using (IDataReader dr = DbHelper.ExecuteReader(dc, _db))
            {
                if (dr.Read())
                {
                    model = new Model.CompanyStructure.CompanyUser();

                    #region 用户信息

                    if (!dr.IsDBNull(dr.GetOrdinal("Id"))) model.ID = dr.GetInt32(dr.GetOrdinal("Id"));
                    if (!dr.IsDBNull(dr.GetOrdinal("CompanyId"))) model.CompanyId = dr.GetInt32(dr.GetOrdinal("CompanyId"));
                    if (!dr.IsDBNull(dr.GetOrdinal("UserName"))) model.UserName = dr.GetString(dr.GetOrdinal("UserName"));
                    if (!dr.IsDBNull(dr.GetOrdinal("Password")))
                        model.PassWordInfo.NoEncryptPassword = dr.GetString(dr.GetOrdinal("Password"));
                    if (!dr.IsDBNull(dr.GetOrdinal("ContactName")))
                        model.PersonInfo.ContactName = dr.GetString(dr.GetOrdinal("ContactName"));
                    if (!dr.IsDBNull(dr.GetOrdinal("ContactSex")))
                        model.PersonInfo.ContactSex =
                            (Model.EnumType.CompanyStructure.Sex)
                            Toolkit.Utils.GetInt(dr.GetString(dr.GetOrdinal("ContactSex")));
                    if (!dr.IsDBNull(dr.GetOrdinal("ContactTel")))
                        model.PersonInfo.ContactTel = dr.GetString(dr.GetOrdinal("ContactTel"));
                    if (!dr.IsDBNull(dr.GetOrdinal("ContactFax")))
                        model.PersonInfo.ContactFax = dr.GetString(dr.GetOrdinal("ContactFax"));
                    if (!dr.IsDBNull(dr.GetOrdinal("ContactMobile")))
                        model.PersonInfo.ContactMobile = dr.GetString(dr.GetOrdinal("ContactMobile"));
                    if (!dr.IsDBNull(dr.GetOrdinal("ContactEmail")))
                        model.PersonInfo.ContactEmail = dr.GetString(dr.GetOrdinal("ContactEmail"));
                    if (!dr.IsDBNull(dr.GetOrdinal("QQ")))
                        model.PersonInfo.QQ = dr.GetString(dr.GetOrdinal("QQ"));
                    if (!dr.IsDBNull(dr.GetOrdinal("MSN")))
                        model.PersonInfo.MSN = dr.GetString(dr.GetOrdinal("MSN"));
                    if (!dr.IsDBNull(dr.GetOrdinal("JobName")))
                        model.PersonInfo.JobName = dr.GetString(dr.GetOrdinal("JobName"));
                    if (!dr.IsDBNull(dr.GetOrdinal("LastLoginIP"))) model.LastLoginIP = dr.GetString(dr.GetOrdinal("LastLoginIP"));
                    if (!dr.IsDBNull(dr.GetOrdinal("LastLoginTime"))) model.LastLoginTime = dr.GetDateTime(dr.GetOrdinal("LastLoginTime"));
                    if (!dr.IsDBNull(dr.GetOrdinal("RoleID"))) model.RoleID = dr.GetInt32(dr.GetOrdinal("RoleID"));
                    if (!dr.IsDBNull(dr.GetOrdinal("PermissionList"))) model.PermissionList = dr.GetString(dr.GetOrdinal("PermissionList"));
                    if (!dr.IsDBNull(dr.GetOrdinal("PeopProfile")))
                        model.PersonInfo.PeopProfile = dr.GetString(dr.GetOrdinal("PeopProfile"));
                    if (!dr.IsDBNull(dr.GetOrdinal("Remark")))
                        model.PersonInfo.Remark = dr.GetString(dr.GetOrdinal("Remark"));
                    if (!dr.IsDBNull(dr.GetOrdinal("IsDelete")))
                        model.IsDeleted = this.GetBoolean(dr.GetString(dr.GetOrdinal("IsDelete")));
                    if (!dr.IsDBNull(dr.GetOrdinal("UserStatus")))
                        model.UserStatus =
                            (Model.EnumType.CompanyStructure.UserStatus)
                            dr.GetByte(dr.GetOrdinal("UserStatus"));
                    if (!dr.IsDBNull(dr.GetOrdinal("IsAdmin")))
                        model.IsAdmin = this.GetBoolean(dr.GetString(dr.GetOrdinal("IsAdmin")));
                    if (!dr.IsDBNull(dr.GetOrdinal("IssueTime")))
                        model.IssueTime = dr.GetDateTime(dr.GetOrdinal("IssueTime"));
                    if (!dr.IsDBNull(dr.GetOrdinal("DepartId"))) model.DepartId = dr.GetInt32(dr.GetOrdinal("DepartId"));
                    if (!dr.IsDBNull(dr.GetOrdinal("SuperviseDepartId")))
                        model.SuperviseDepartId = dr.GetInt32(dr.GetOrdinal("SuperviseDepartId"));
                    if (!dr.IsDBNull(dr.GetOrdinal("Birthday"))) model.PersonInfo.Birthday = dr.GetDateTime(dr.GetOrdinal("Birthday"));
                    if (!dr.IsDBNull(dr.GetOrdinal("UserType")))
                        model.UserType = (Model.EnumType.CompanyStructure.UserType)dr.GetByte(dr.GetOrdinal("UserType"));
                    if (!dr.IsDBNull(dr.GetOrdinal("Address")))
                        model.PersonInfo.Address = dr.GetString(dr.GetOrdinal("Address"));
                    if (!dr.IsDBNull(dr.GetOrdinal("SupplierCompanyId")))
                        model.SupplierCompanyId = dr.GetString(dr.GetOrdinal("SupplierCompanyId"));

                    #endregion
                }
            }

            return model;
        }
        /// <summary>
        /// 获取指定公司的管理员账户
        /// </summary>
        /// <param name="companyId">公司编号</param>
        /// <returns>管理员用户信息实体</returns>
        public Model.CompanyStructure.CompanyUser GetAdminModel(int companyId)
        {
            if (companyId <= 0) return null;

            var strSql = new StringBuilder();
            strSql.AppendFormat(
                " SELECT top 1 [Id],[CompanyId],[UserName],[Password],[MD5Password],[ContactName],[ContactSex],[ContactTel],[ContactFax],[ContactMobile],[ContactEmail],[QQ],[MSN],[JobName],[LastLoginIP],[LastLoginTime],[RoleID],[PermissionList],[PeopProfile],[Remark],[IsDelete],[UserStatus],[IsAdmin],[IssueTime],[DepartId],[SuperviseDepartId],[UserType],[Birthday],[Address],[SupplierCompanyId] FROM [tbl_CompanyUser] where [CompanyId]=@CompanyId and IsAdmin = '1' and IsDelete = '0' and UserStatus = {0} order by IssueTime asc; ",
                Model.EnumType.CompanyStructure.UserStatus.正常);

            DbCommand dc = _db.GetSqlStringCommand(strSql.ToString());
            _db.AddInParameter(dc, "CompanyId", DbType.Int32, companyId);

            Model.CompanyStructure.CompanyUser model = null;
            using (IDataReader dr = DbHelper.ExecuteReader(dc, _db))
            {
                if (dr.Read())
                {
                    model = new Model.CompanyStructure.CompanyUser();

                    #region 用户信息

                    if (!dr.IsDBNull(dr.GetOrdinal("Id"))) model.ID = dr.GetInt32(dr.GetOrdinal("Id"));
                    if (!dr.IsDBNull(dr.GetOrdinal("CompanyId"))) model.CompanyId = dr.GetInt32(dr.GetOrdinal("CompanyId"));
                    if (!dr.IsDBNull(dr.GetOrdinal("UserName"))) model.UserName = dr.GetString(dr.GetOrdinal("UserName"));
                    if (!dr.IsDBNull(dr.GetOrdinal("Password")))
                        model.PassWordInfo.NoEncryptPassword = dr.GetString(dr.GetOrdinal("Password"));
                    if (!dr.IsDBNull(dr.GetOrdinal("ContactName")))
                        model.PersonInfo.ContactName = dr.GetString(dr.GetOrdinal("ContactName"));
                    if (!dr.IsDBNull(dr.GetOrdinal("ContactSex")))
                        model.PersonInfo.ContactSex =
                            (Model.EnumType.CompanyStructure.Sex)
                            Toolkit.Utils.GetInt(dr.GetString(dr.GetOrdinal("ContactSex")));
                    if (!dr.IsDBNull(dr.GetOrdinal("ContactTel")))
                        model.PersonInfo.ContactTel = dr.GetString(dr.GetOrdinal("ContactTel"));
                    if (!dr.IsDBNull(dr.GetOrdinal("ContactFax")))
                        model.PersonInfo.ContactFax = dr.GetString(dr.GetOrdinal("ContactFax"));
                    if (!dr.IsDBNull(dr.GetOrdinal("ContactMobile")))
                        model.PersonInfo.ContactMobile = dr.GetString(dr.GetOrdinal("ContactMobile"));
                    if (!dr.IsDBNull(dr.GetOrdinal("ContactEmail")))
                        model.PersonInfo.ContactEmail = dr.GetString(dr.GetOrdinal("ContactEmail"));
                    if (!dr.IsDBNull(dr.GetOrdinal("QQ")))
                        model.PersonInfo.QQ = dr.GetString(dr.GetOrdinal("QQ"));
                    if (!dr.IsDBNull(dr.GetOrdinal("MSN")))
                        model.PersonInfo.MSN = dr.GetString(dr.GetOrdinal("MSN"));
                    if (!dr.IsDBNull(dr.GetOrdinal("JobName")))
                        model.PersonInfo.JobName = dr.GetString(dr.GetOrdinal("JobName"));
                    if (!dr.IsDBNull(dr.GetOrdinal("LastLoginIP"))) model.LastLoginIP = dr.GetString(dr.GetOrdinal("LastLoginIP"));
                    if (!dr.IsDBNull(dr.GetOrdinal("LastLoginTime"))) model.LastLoginTime = dr.GetDateTime(dr.GetOrdinal("LastLoginTime"));
                    if (!dr.IsDBNull(dr.GetOrdinal("RoleID"))) model.RoleID = dr.GetInt32(dr.GetOrdinal("RoleID"));
                    if (!dr.IsDBNull(dr.GetOrdinal("PermissionList"))) model.PermissionList = dr.GetString(dr.GetOrdinal("PermissionList"));
                    if (!dr.IsDBNull(dr.GetOrdinal("PeopProfile")))
                        model.PersonInfo.PeopProfile = dr.GetString(dr.GetOrdinal("PeopProfile"));
                    if (!dr.IsDBNull(dr.GetOrdinal("Remark")))
                        model.PersonInfo.Remark = dr.GetString(dr.GetOrdinal("Remark"));
                    if (!dr.IsDBNull(dr.GetOrdinal("IsDelete")))
                        model.IsDeleted = this.GetBoolean(dr.GetString(dr.GetOrdinal("IsDelete")));
                    if (!dr.IsDBNull(dr.GetOrdinal("UserStatus")))
                        model.UserStatus =
                            (Model.EnumType.CompanyStructure.UserStatus)
                            dr.GetByte(dr.GetOrdinal("UserStatus"));
                    if (!dr.IsDBNull(dr.GetOrdinal("IsAdmin")))
                        model.IsAdmin = this.GetBoolean(dr.GetString(dr.GetOrdinal("IsAdmin")));
                    if (!dr.IsDBNull(dr.GetOrdinal("IssueTime")))
                        model.IssueTime = dr.GetDateTime(dr.GetOrdinal("IssueTime"));
                    if (!dr.IsDBNull(dr.GetOrdinal("DepartId"))) model.DepartId = dr.GetInt32(dr.GetOrdinal("DepartId"));
                    if (!dr.IsDBNull(dr.GetOrdinal("SuperviseDepartId")))
                        model.SuperviseDepartId = dr.GetInt32(dr.GetOrdinal("SuperviseDepartId"));
                    if (!dr.IsDBNull(dr.GetOrdinal("Birthday"))) model.PersonInfo.Birthday = dr.GetDateTime(dr.GetOrdinal("Birthday"));
                    if (!dr.IsDBNull(dr.GetOrdinal("UserType")))
                        model.UserType = (Model.EnumType.CompanyStructure.UserType)dr.GetByte(dr.GetOrdinal("UserType"));
                    if (!dr.IsDBNull(dr.GetOrdinal("Address")))
                        model.PersonInfo.Address = dr.GetString(dr.GetOrdinal("Address"));
                    if (!dr.IsDBNull(dr.GetOrdinal("SupplierCompanyId")))
                        model.SupplierCompanyId = dr.GetString(dr.GetOrdinal("SupplierCompanyId"));

                    #endregion
                }
            }

            return model;
        }

        /// <summary>
        /// 获取指定公司的所有用户信息
        /// </summary>
        /// <param name="companyId">公司编号</param>
        /// <param name="pageSize">每页显示条数</param>
        /// <param name="pageIndex">当前页索引</param>
        /// <param name="recordCount">总记录数</param>
        /// <param name="model">查询实体</param>
        /// <returns></returns>
        public IList<Model.CompanyStructure.CompanyUser> GetList(int companyId, int pageSize, int pageIndex, ref int recordCount
            , Model.CompanyStructure.QueryCompanyUser model)
        {
            if (companyId <= 0 || pageSize <= 0 || pageIndex <= 0) return null;

            IList<Model.CompanyStructure.CompanyUser> totals = new List<Model.CompanyStructure.CompanyUser>();

            #region sql 拼接

            string tableName = " tbl_CompanyUser ";
            string orderByString = "IssueTime DESC";
            var fields = new StringBuilder();
            fields.Append(" Id,CompanyId,UserName,Password,MD5Password,ContactName,ContactSex,ContactTel,ContactFax,ContactMobile,ContactEmail,QQ,MSN,JobName,LastLoginIP,LastLoginTime,RoleID,PermissionList,PeopProfile,Remark,IsDelete,UserStatus,IsAdmin,IssueTime,DepartId,SuperviseDepartId,(select DepartName from tbl_CompanyDepartment as b where b.Id = tbl_CompanyUser.DepartId) as DepartName,(select DepartName from tbl_CompanyDepartment as b where b.Id = tbl_CompanyUser.SuperviseDepartId) as SuperviseDepartName,[UserType],[Birthday],[Address],[SupplierCompanyId] ");

            var cmdQuery = new StringBuilder();
            cmdQuery.AppendFormat(" CompanyId = {0} ", companyId);
            if (model != null)
            {
                if (model.IsDelete.HasValue)
                {
                    cmdQuery.AppendFormat(" and IsDelete = '{0}' ", model.IsDelete.Value ? "1" : "0");
                }
                else
                {
                    cmdQuery.Append(" and IsDelete = '0' ");
                }
                if (model.IsAdmin.HasValue)
                {
                    cmdQuery.AppendFormat(" and IsAdmin = '{0}' ", model.IsAdmin.Value ? "1" : "0");
                }
                if (model.UserId != null && model.UserId.Length > 0)
                {
                    cmdQuery.AppendFormat(" and Id in ({0}) ", GetIdsByArr(model.UserId));
                }
                if (model.DepartId != null && model.DepartId.Length > 0)
                {
                    cmdQuery.AppendFormat(" and DepartId in ({0}) ", GetIdsByArr(model.DepartId));
                }
                if (!string.IsNullOrEmpty(model.UserName))
                {
                    cmdQuery.AppendFormat(" and UserName like '%{0}%' ", Toolkit.Utils.ToSqlLike(model.UserName));
                }
                if (!string.IsNullOrEmpty(model.ContactName))
                {
                    cmdQuery.AppendFormat(" and ContactName like '%{0}%' ", Toolkit.Utils.ToSqlLike(model.ContactName));
                }
                if (model.UserStatus != null && model.UserStatus.Length > 0)
                {
                    if (model.UserStatus.Length == 1)
                    {
                        cmdQuery.AppendFormat(" and UserStatus = {0} ", (int)model.UserStatus[0]);
                    }
                    else
                    {

                        string strIds = string.Empty;
                        foreach (var t in model.UserStatus)
                        {
                            strIds += ((int)t) + ",";
                        }
                        if (!string.IsNullOrEmpty(strIds)) strIds = strIds.TrimEnd(',');
                        cmdQuery.AppendFormat(" and UserStatus in ({0}) ", strIds);
                    }
                }
                if (model.UserType.HasValue)
                {
                    cmdQuery.AppendFormat(" and UserType = {0} ", (int)model.UserType.Value);
                }
            }

            #endregion

            Model.CompanyStructure.CompanyUser companyUserModel;
            using (IDataReader rdr = DbHelper.ExecuteReader1(this._db, pageSize, pageIndex, ref recordCount, tableName, fields.ToString()
                , cmdQuery.ToString(), orderByString, string.Empty))
            {
                while (rdr.Read())
                {
                    #region 用户基本信息

                    companyUserModel = new Model.CompanyStructure.CompanyUser
                        {
                            ID = rdr.GetInt32(rdr.GetOrdinal("ID")),
                            CompanyId = rdr.GetInt32(rdr.GetOrdinal("CompanyId")),
                            UserName = rdr.GetString(rdr.GetOrdinal("UserName")),
                            DepartName =
                                rdr.IsDBNull(rdr.GetOrdinal("DepartName"))
                                    ? string.Empty
                                    : rdr.GetString(rdr.GetOrdinal("DepartName")),
                            IsAdmin = rdr.GetString(rdr.GetOrdinal("IsAdmin")) == "1" ? true : false,
                            IsDeleted = rdr.GetString(rdr.GetOrdinal("IsDelete")) == "1" ? true : false,
                            UserStatus =
                                (Model.EnumType.CompanyStructure.UserStatus)rdr.GetByte(rdr.GetOrdinal("UserStatus")),
                            IssueTime = rdr.GetDateTime(rdr.GetOrdinal("IssueTime")),
                            LastLoginIP = rdr.IsDBNull(rdr.GetOrdinal("LastLoginIP")) ? "" : rdr["LastLoginIP"].ToString(),
                            LastLoginTime = rdr.GetDateTime(rdr.GetOrdinal("LastLoginTime")),
                            PermissionList =
                                rdr.IsDBNull(rdr.GetOrdinal("PermissionList"))
                                    ? ""
                                    : rdr.GetString(rdr.GetOrdinal("PermissionList")),
                            RoleID = rdr.GetInt32(rdr.GetOrdinal("RoleID")),
                            SuperviseDepartId = rdr.GetInt32(rdr.GetOrdinal("SuperviseDepartId")),
                            SuperviseDepartName =
                                rdr.IsDBNull(rdr.GetOrdinal("SuperviseDepartName"))
                                    ? string.Empty
                                    : rdr.GetString(rdr.GetOrdinal("SuperviseDepartName")),
                            UserType = (Model.EnumType.CompanyStructure.UserType)rdr.GetByte(rdr.GetOrdinal("UserType")),
                            SupplierCompanyId =
                                rdr.IsDBNull(rdr.GetOrdinal("SupplierCompanyId"))
                                    ? string.Empty
                                    : rdr.GetString(rdr.GetOrdinal("SupplierCompanyId"))
                        };
                    companyUserModel.UserName = rdr.GetString(rdr.GetOrdinal("UserName"));
                    #endregion

                    //用户密码信息
                    companyUserModel.PassWordInfo = new Model.CompanyStructure.PassWord
                    {
                        NoEncryptPassword = rdr.GetString(rdr.GetOrdinal("Password"))
                    };

                    #region 联系人信息
                    companyUserModel.PersonInfo = new Model.CompanyStructure.ContactPersonInfo();

                    companyUserModel.PersonInfo.ContactEmail = rdr.IsDBNull(rdr.GetOrdinal("ContactEmail")) ? "" : rdr.GetString(rdr.GetOrdinal("ContactEmail"));
                    companyUserModel.PersonInfo.ContactFax = rdr.IsDBNull(rdr.GetOrdinal("ContactFax")) ? "" : rdr.GetString(rdr.GetOrdinal("ContactFax"));
                    companyUserModel.PersonInfo.ContactMobile = rdr.IsDBNull(rdr.GetOrdinal("ContactMobile")) ? "" : rdr.GetString(rdr.GetOrdinal("ContactMobile"));
                    companyUserModel.PersonInfo.ContactName = rdr.IsDBNull(rdr.GetOrdinal("ContactName")) ? "" : rdr.GetString(rdr.GetOrdinal("ContactName"));
                    companyUserModel.PersonInfo.ContactSex = rdr.IsDBNull(rdr.GetOrdinal("ContactSex")) ? Model.EnumType.CompanyStructure.Sex.未知 : (Model.EnumType.CompanyStructure.Sex)Enum.Parse(typeof(Model.EnumType.CompanyStructure.Sex), rdr.GetString(rdr.GetOrdinal("ContactSex")));
                    companyUserModel.PersonInfo.ContactTel = rdr.IsDBNull(rdr.GetOrdinal("ContactTel")) ? "" : rdr.GetString(rdr.GetOrdinal("ContactTel"));
                    companyUserModel.PersonInfo.JobName = rdr.IsDBNull(rdr.GetOrdinal("JobName")) ? "" : rdr.GetString(rdr.GetOrdinal("JobName"));
                    companyUserModel.PersonInfo.MSN = rdr.IsDBNull(rdr.GetOrdinal("MSN")) ? "" : rdr.GetString(rdr.GetOrdinal("MSN"));
                    companyUserModel.PersonInfo.PeopProfile = rdr.IsDBNull(rdr.GetOrdinal("PeopProfile")) ? "" : rdr.GetString(rdr.GetOrdinal("PeopProfile"));
                    companyUserModel.PersonInfo.QQ = rdr.IsDBNull(rdr.GetOrdinal("QQ")) ? "" : rdr.GetString(rdr.GetOrdinal("QQ"));
                    companyUserModel.PersonInfo.Remark = rdr.IsDBNull(rdr.GetOrdinal("Remark")) ? "" : rdr.GetString(rdr.GetOrdinal("Remark"));
                    companyUserModel.PersonInfo.Address = rdr.IsDBNull(rdr.GetOrdinal("Address")) ? "" : rdr.GetString(rdr.GetOrdinal("Address"));

                    if (!rdr.IsDBNull(rdr.GetOrdinal("Birthday")))
                        companyUserModel.PersonInfo.Birthday = rdr.GetDateTime(rdr.GetOrdinal("Birthday"));
                    #endregion

                    totals.Add(companyUserModel);
                }
            }

            return totals;
        }

        /// <summary>
        /// 获取指定公司的所有用户信息
        /// </summary>
        /// <param name="companyId">公司编号</param>
        /// <param name="model">查询实体</param>
        /// <returns></returns>
        public IList<Model.CompanyStructure.CompanyUser> GetList(int companyId, Model.CompanyStructure.QueryCompanyUser model)
        {
            if (companyId <= 0) return null;

            IList<Model.CompanyStructure.CompanyUser> totals = new List<Model.CompanyStructure.CompanyUser>();

            #region sql 拼接

            var strSql = new StringBuilder();
            strSql.Append(" select ");
            strSql.Append(" Id,CompanyId,UserName,Password,MD5Password,ContactName,ContactSex,ContactTel,ContactFax,ContactMobile,ContactEmail,QQ,MSN,JobName,LastLoginIP,LastLoginTime,RoleID,PermissionList,PeopProfile,Remark,IsDelete,UserStatus,IsAdmin,IssueTime,DepartId,SuperviseDepartId,(select DepartName from tbl_CompanyDepartment as b where b.Id = tbl_CompanyUser.DepartId) as DepartName,(select DepartName from tbl_CompanyDepartment as b where b.Id = tbl_CompanyUser.SuperviseDepartId) as SuperviseDepartName,[UserType],[Birthday],[Address],[SupplierCompanyId] ");
            strSql.Append(" from tbl_CompanyUser ");
            strSql.AppendFormat(" where CompanyId = {0} ", companyId);
            if (model != null)
            {
                if (model.IsDelete.HasValue)
                {
                    strSql.AppendFormat(" and IsDelete = '{0}' ", model.IsDelete.Value ? "1" : "0");
                }
                else
                {
                    strSql.Append(" and IsDelete = '0' ");
                }
                if (model.IsAdmin.HasValue)
                {
                    strSql.AppendFormat(" and IsAdmin = '{0}' ", model.IsAdmin.Value ? "1" : "0");
                }
                if (model.UserId != null && model.UserId.Length > 0)
                {
                    strSql.AppendFormat(" and Id in ({0}) ", GetIdsByArr(model.UserId));
                }
                if (model.DepartId != null && model.DepartId.Length > 0)
                {
                    strSql.AppendFormat(" and DepartId in ({0}) ", GetIdsByArr(model.DepartId));
                }
                if (!string.IsNullOrEmpty(model.UserName))
                {
                    strSql.AppendFormat(" and UserName like '%{0}%' ", Toolkit.Utils.ToSqlLike(model.UserName));
                }
                if (!string.IsNullOrEmpty(model.ContactName))
                {
                    strSql.AppendFormat(" and ContactName like '%{0}%' ", Toolkit.Utils.ToSqlLike(model.ContactName));
                }
                if (model.UserStatus != null && model.UserStatus.Length > 0)
                {
                    if (model.UserStatus.Length == 1)
                    {
                        strSql.AppendFormat(" and UserStatus = {0} ", (int)model.UserStatus[0]);
                    }
                    else
                    {

                        string strIds = string.Empty;
                        foreach (var t in model.UserStatus)
                        {
                            strIds += ((int)t) + ",";
                        }
                        if (!string.IsNullOrEmpty(strIds)) strIds = strIds.TrimEnd(',');
                        strSql.AppendFormat(" and UserStatus in ({0}) ", strIds);
                    }
                }
                if (model.UserType.HasValue)
                {
                    strSql.AppendFormat(" and UserType = {0} ", (int)model.UserType.Value);
                }
            }

            strSql.Append(" order by IssueTime desc ");

            #endregion

            DbCommand dc = _db.GetSqlStringCommand(strSql.ToString());

            Model.CompanyStructure.CompanyUser companyUserModel;
            using (IDataReader rdr = DbHelper.ExecuteReader(dc, _db))
            {
                while (rdr.Read())
                {
                    #region 用户基本信息

                    companyUserModel = new Model.CompanyStructure.CompanyUser
                    {
                        ID = rdr.GetInt32(rdr.GetOrdinal("ID")),
                        CompanyId = rdr.GetInt32(rdr.GetOrdinal("CompanyId")),
                        UserName = rdr.GetString(rdr.GetOrdinal("UserName")),
                        DepartName =
                            rdr.IsDBNull(rdr.GetOrdinal("DepartName"))
                                ? string.Empty
                                : rdr.GetString(rdr.GetOrdinal("DepartName")),
                        IsAdmin = rdr.GetString(rdr.GetOrdinal("IsAdmin")) == "1" ? true : false,
                        IsDeleted = rdr.GetString(rdr.GetOrdinal("IsDelete")) == "1" ? true : false,
                        UserStatus =
                            (Model.EnumType.CompanyStructure.UserStatus)rdr.GetByte(rdr.GetOrdinal("UserStatus")),
                        IssueTime = rdr.GetDateTime(rdr.GetOrdinal("IssueTime")),
                        LastLoginIP = rdr.IsDBNull(rdr.GetOrdinal("LastLoginIP")) ? "" : rdr["LastLoginIP"].ToString(),
                        LastLoginTime = rdr.GetDateTime(rdr.GetOrdinal("LastLoginTime")),
                        PermissionList =
                            rdr.IsDBNull(rdr.GetOrdinal("PermissionList"))
                                ? ""
                                : rdr.GetString(rdr.GetOrdinal("PermissionList")),
                        RoleID = rdr.GetInt32(rdr.GetOrdinal("RoleID")),
                        SuperviseDepartId = rdr.GetInt32(rdr.GetOrdinal("SuperviseDepartId")),
                        SuperviseDepartName =
                            rdr.IsDBNull(rdr.GetOrdinal("SuperviseDepartName"))
                                ? string.Empty
                                : rdr.GetString(rdr.GetOrdinal("SuperviseDepartName")),
                        UserType = (Model.EnumType.CompanyStructure.UserType)rdr.GetByte(rdr.GetOrdinal("UserType")),
                        SupplierCompanyId =
                                rdr.IsDBNull(rdr.GetOrdinal("SupplierCompanyId"))
                                    ? string.Empty
                                    : rdr.GetString(rdr.GetOrdinal("SupplierCompanyId"))
                    };
                    companyUserModel.UserName = rdr.GetString(rdr.GetOrdinal("UserName"));
                    #endregion

                    //用户密码信息
                    companyUserModel.PassWordInfo = new Model.CompanyStructure.PassWord
                    {
                        NoEncryptPassword = rdr.GetString(rdr.GetOrdinal("Password"))
                    };

                    #region 联系人信息
                    companyUserModel.PersonInfo = new Model.CompanyStructure.ContactPersonInfo
                    {
                        ContactEmail = rdr.IsDBNull(rdr.GetOrdinal("ContactEmail")) ? "" : rdr.GetString(rdr.GetOrdinal("ContactEmail")),
                        ContactFax = rdr.IsDBNull(rdr.GetOrdinal("ContactFax")) ? "" : rdr.GetString(rdr.GetOrdinal("ContactFax")),
                        ContactMobile = rdr.IsDBNull(rdr.GetOrdinal("ContactMobile")) ? "" : rdr.GetString(rdr.GetOrdinal("ContactMobile")),
                        ContactName = rdr.IsDBNull(rdr.GetOrdinal("ContactName")) ? "" : rdr.GetString(rdr.GetOrdinal("ContactName")),
                        ContactTel = rdr.IsDBNull(rdr.GetOrdinal("ContactTel")) ? "" : rdr.GetString(rdr.GetOrdinal("ContactTel")),
                        JobName = rdr.IsDBNull(rdr.GetOrdinal("JobName")) ? "" : rdr.GetString(rdr.GetOrdinal("JobName")),
                        MSN = rdr.IsDBNull(rdr.GetOrdinal("MSN")) ? "" : rdr.GetString(rdr.GetOrdinal("MSN")),
                        PeopProfile = rdr.IsDBNull(rdr.GetOrdinal("PeopProfile")) ? "" : rdr.GetString(rdr.GetOrdinal("PeopProfile")),
                        QQ = rdr.IsDBNull(rdr.GetOrdinal("QQ")) ? "" : rdr.GetString(rdr.GetOrdinal("QQ")),
                        Remark = rdr.IsDBNull(rdr.GetOrdinal("Remark")) ? "" : rdr.GetString(rdr.GetOrdinal("Remark")),
                        Address = rdr.IsDBNull(rdr.GetOrdinal("Address")) ? "" : rdr.GetString(rdr.GetOrdinal("Address"))
                    };
                    if (!rdr.IsDBNull(rdr.GetOrdinal("Birthday")))
                        companyUserModel.PersonInfo.Birthday = rdr.GetDateTime(rdr.GetOrdinal("Birthday"));
                    if (!rdr.IsDBNull(rdr.GetOrdinal("ContactSex")))
                        companyUserModel.PersonInfo.ContactSex =
                            (Model.EnumType.CompanyStructure.Sex)
                            Enum.Parse(
                                typeof(Model.EnumType.CompanyStructure.Sex), rdr.GetString(rdr.GetOrdinal("ContactSex")));
                    #endregion

                    totals.Add(companyUserModel);
                }
            }

            return totals;
        }

        /// <summary>
        /// 设置用户启用状态
        /// </summary>
        /// <param name="id">用户编号</param>
        /// <param name="status">用户状态</param>
        /// <returns>true:成功 false:失败</returns>
        public bool SetEnable(int id, Model.EnumType.CompanyStructure.UserStatus status)
        {
            if (id <= 0) return false;

            DbCommand cmd = this._db.GetSqlStringCommand(SqlSetUserEnable);
            this._db.AddInParameter(cmd, "Id", DbType.Int32, id);
            this._db.AddInParameter(cmd, "UserStatus", DbType.Byte, (int)status);

            return DbHelper.ExecuteSql(cmd, this._db) > 0 ? true : false;
        }

        /// <summary>
        /// 设置用户权限
        /// </summary>
        /// <param name="userId">用户编号</param>
        /// <param name="roleId">角色编号</param>
        /// <param name="permissionList">权限集合</param>
        /// <returns>是否成功</returns>
        public bool SetPermission(int userId, int roleId, params string[] permissionList)
        {
            if (userId <= 0 || permissionList == null) return false;

            string permissionStr = string.Empty;
            foreach (string str in permissionList)
            {
                permissionStr += str + ",";
            }
            permissionStr = permissionStr.Trim(',');

            var strSql = new StringBuilder();
            strSql.Append(" update tbl_CompanyUser set RoleID = @RoleId,PermissionList = @PermissionList where Id = @Id ");

            DbCommand cmd = this._db.GetSqlStringCommand(strSql.ToString());
            this._db.AddInParameter(cmd, "Id", DbType.Int32, userId);
            this._db.AddInParameter(cmd, "RoleId", DbType.Int32, roleId);
            this._db.AddInParameter(cmd, "PermissionList", DbType.String, permissionStr);

            return DbHelper.ExecuteSql(cmd, _db) > 0 ? true : false;
        }

        /// <summary>
        /// 验证公司是否可以添加子账户
        /// </summary>
        /// <param name="companyId">公司编号</param>
        /// <returns>
        /// true：可以添加
        /// false：不能添加，子账号已满
        /// </returns>
        public bool IsAddUser(int companyId)
        {
            if (companyId <= 0) return false;

            var strSql = new StringBuilder();
            strSql.Append(
                " SELECT FieldValue as MaxUserSum FROM tbl_CompanySetting as a where a.Id = @CompanyId and a.FieldName = 'MaxSonUserNum'; ");
            strSql.Append(
                " select count(*) as UserCount from tbl_CompanyUser as b where b.CompanyId = @CompanyId and b.IsDelete = '0' and b.IsSystem = '0' ");
            strSql.AppendFormat(" and b.UserType = {0} ", (int)Model.EnumType.CompanyStructure.UserType.专线用户);

            DbCommand dc = _db.GetSqlStringCommand(strSql.ToString());
            _db.AddInParameter(dc, "CompanyId", DbType.Int32, companyId);

            int maxUserSum = 0;
            int userCount = 0;
            using (IDataReader dr = DbHelper.ExecuteReader(dc, _db))
            {
                if (dr.Read())
                {
                    if (!dr.IsDBNull(dr.GetOrdinal("MaxUserSum")))
                    {
                        maxUserSum = Toolkit.Utils.GetInt(dr.GetString(dr.GetOrdinal("MaxUserSum")));
                    }
                }

                dr.NextResult();
                if (dr.Read())
                {
                    if (!dr.IsDBNull(dr.GetOrdinal("UserCount")))
                    {
                        userCount = dr.GetInt32(dr.GetOrdinal("UserCount"));
                    }
                }
            }

            if (maxUserSum >= (userCount + 1)) return true;

            return false;
        }

        #endregion
    }
}
