//2011-09-23 汪奇志
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using Microsoft.Practices.EnterpriseLibrary.Data;
using EyouSoft.Toolkit.DAL;
using EyouSoft.Toolkit;
using EyouSoft.Model.SSOStructure;
using EyouSoft.Model.CompanyStructure;
using EyouSoft.Model.SysStructure;
using System.Xml.Linq;

namespace EyouSoft.Security.Membership
{
    /// <summary>
    /// 系统用户登录数据访问类
    /// </summary>
    internal class DUserLogin : DALBase, IUserLogin
    {
        #region static constants
        //static constants
        const string SQL_SELECT_Login = "SELECT [Id],[UserName],[CompanyId],[ContactName],[ContactTel],[ContactMobile],[LastLoginIP],[LastLoginTime],[DepartId],(select b.DepartName from tbl_CompanyDepartment as b where b.Id = tbl_CompanyUser.DepartId and B.CompanyId = tbl_CompanyUser.CompanyId) as DepartName,[PermissionList],[UserStatus],[IsAdmin],[SuperviseDepartId],[RoleID],[ContactFax],[OnlineStatus],[OnlineSessionId],[UserType],[SupplierCompanyId] FROM [tbl_CompanyUser] ";
        const string SQL_INSERT_LoginLogwr = "INSERT INTO [tbl_SysLoginLog]([ID],[CompanyId],[OperatorId],[LoginTime],[LoginIp],[LoginType],[BrowserType]) VALUES (@LogId,@CompanyId,@OperatorId,@IssueTime,@Ip,@LoginType,@Client);";
        const string SQL_SELECT_GetDomain = "SELECT A.[SysId],A.[Domain],A.[Url],(SELECT B.[Id],B.[CompanyName] FROM [tbl_CompanyInfo] AS B WHERE B.[SystemId]=A.[SysId] FOR XML RAW,ROOT('root')) AS CompanyXml FROM [tbl_SysDomain] AS A WHERE A.[Domain]=@Domain";
        const string SQL_SELECT_GetCompany = "SELECT [CompanyName],[SystemId] FROM [tbl_CompanyInfo] WHERE [Id]=@CID";
        const string SQL_SELECT_GetRolePrivs = "SELECT [RoleChilds] FROM [tbl_SysRoleManage] WHERE [Id]=@RoleId";
        const string SQL_SELECT_GetSettings = "SELECT [FieldName],[FieldValue] FROM tbl_CompanySetting WHERE [Id]=@CompanyId;";
        const string SQL_UPDATE_SetOnlineStatus = "UPDATE [tbl_CompanyUser] SET [OnlineStatus]=@OnlineStatus,[OnlineSessionId]=@OnlineSessionId WHERE [Id]=@UserId";
        #endregion

        #region constructor
        /// <summary>
        /// database
        /// </summary>
        Database _db = null;

        /// <summary>
        /// default constructor
        /// </summary>
        public DUserLogin()
        {
            _db = SystemStore;
        }
        #endregion        

        #region private members
        /// <summary>
        /// 获取用户信息
        /// </summary>
        /// <param name="cmd"></param>
        /// <returns></returns>
        private MUserInfo ReadUserInfo(DbCommand cmd)
        {
            MUserInfo info = null;
            string privs = string.Empty;
            using (IDataReader rdr = DbHelper.ExecuteReader(cmd, SystemStore))
            {
                if (rdr.Read())
                {
                    info = new MUserInfo();
                    info.CompanyId = rdr.GetInt32(rdr.GetOrdinal("CompanyId"));
                    info.DeptId = rdr.GetInt32(rdr.GetOrdinal("DepartId"));
                    info.DeptName = rdr["DepartName"].ToString();
                    info.Fax = rdr["ContactFax"].ToString();
                    info.IsAdmin = GetBoolean(rdr.GetString(rdr.GetOrdinal("IsAdmin")));
                    info.Status = (Model.EnumType.CompanyStructure.UserStatus)rdr.GetByte(rdr.GetOrdinal("UserStatus"));
                    info.JGDeptId = rdr.GetInt32(rdr.GetOrdinal("SuperviseDepartId"));
                    if (!rdr.IsDBNull(rdr.GetOrdinal("LastLoginTime"))) info.LastLoginTime = rdr.GetDateTime(rdr.GetOrdinal("LastLoginTime"));
                    info.Mobile = rdr["ContactMobile"].ToString();
                    info.Name = rdr["ContactName"].ToString();                    
                    info.Telephone = rdr["ContactTel"].ToString();
                    info.UserId = rdr.GetInt32(rdr.GetOrdinal("Id"));
                    info.Username = rdr.GetString(rdr.GetOrdinal("UserName"));
                    info.RoleId = rdr.GetInt32(rdr.GetOrdinal("RoleID"));
                    info.LastLoginIp = rdr["LastLoginIP"].ToString();
                    info.OnlineSessionId = rdr["OnlineSessionId"].ToString();
                    info.OnlineStatus = (Model.EnumType.CompanyStructure.UserOnlineStatus)rdr.GetByte(rdr.GetOrdinal("OnlineStatus"));
                    privs = rdr["PermissionList"].ToString();
                    info.UserType = (Model.EnumType.CompanyStructure.UserType)rdr.GetByte(rdr.GetOrdinal("UserType"));
                    info.SupplierCompanyId = rdr.IsDBNull(rdr.GetOrdinal("SupplierCompanyId"))
                                                 ? string.Empty
                                                 : rdr.GetString(rdr.GetOrdinal("SupplierCompanyId"));
                }
            }

            if (info != null)
            {
                //权限处理
                if (!string.IsNullOrEmpty(privs))
                {
                    string[] arr = privs.Split(',');
                    int count = arr.Length;
                    info.Privs = new int[count];

                    for (int i = 0; i < count; i++)
                    {
                        info.Privs[i] = Utils.GetInt(arr[i], -1);
                    }
                }
                else
                {
                    info.Privs = new int[] { -1 };
                }

                //公司名称及系统编号处理
                string companyName;
                int sysId;
                GetCompany(info.CompanyId, out sysId, out companyName);
                info.CompanyName = companyName;
                info.SysId = sysId;
            }

            return info;
        }

        /// <summary>
        /// 获取公司名称及系统编号
        /// </summary>
        /// <param name="companyId">公司编号</param>
        /// <param name="sysId">系统编号</param>
        /// <param name="name">公司名称</param>
        private void GetCompany(int companyId, out int sysId, out string name)
        {
            sysId = 0;
            name = string.Empty;
            DbCommand cmd = _db.GetSqlStringCommand(SQL_SELECT_GetCompany);
            _db.AddInParameter(cmd, "CID", DbType.AnsiStringFixedLength, companyId);

            using (IDataReader rdr = DbHelper.ExecuteReader(cmd, _db))
            {
                if (rdr.Read())
                {
                    name = rdr[0].ToString();
                    sysId = rdr.GetInt32(1);                    
                }
            }
        }


        /// <summary>
        /// 获取角色权限集合
        /// </summary>
        /// <param name="roleId">角色编号</param>
        /// <returns></returns>
        private int[] GetRolePrivs(int roleId)
        {
            DbCommand cmd = _db.GetSqlStringCommand(SQL_SELECT_GetRolePrivs);
            _db.AddInParameter(cmd, "RoleId", DbType.Int32, roleId);

            string privs = string.Empty;
            using (IDataReader rdr = DbHelper.ExecuteReader(cmd, _db))
            {
                if (rdr.Read())
                {
                    privs = rdr["RoleChilds"].ToString();
                }
            }

            if (!string.IsNullOrEmpty(privs))
            {
                string[] arr = privs.Split(',');
                int count = arr.Length;
                int[] retArr = new int[count];

                for (int i = 0; i < count; i++)
                {
                    retArr[i] = Utils.GetInt(arr[i], -1);
                }

                return retArr;
            }

            return new int[] { -1 };
        }

        #endregion

        #region IUserLogin 成员
        /// <summary>
        /// 用户登录，根据系统公司编号、用户名、用户密码获取用户信息
        /// </summary>
        /// <param name="companyId">系统公司编号</param>
        /// <param name="username">登录账号</param>
        /// <param name="pwd">登录密码</param>
        /// <returns></returns>
        public MUserInfo Login(int companyId, string username, PassWord pwd)
        {
            DbCommand cmd = _db.GetSqlStringCommand(SQL_SELECT_Login + " WHERE [CompanyId]=@CID AND [UserName]=@UN AND [MD5Password]=@MD5PWD AND [IsDelete]='0' ");
            _db.AddInParameter(cmd, "CID", DbType.Int32, companyId);
            _db.AddInParameter(cmd, "UN", DbType.String, username);
            _db.AddInParameter(cmd, "MD5PWD", DbType.String, pwd.MD5Password);

            return ReadUserInfo(cmd);
        }

        /// <summary>
        /// 用户登录，根据用户编号获取用户信息
        /// </summary>
        /// <param name="userid">用户编号</param>
        /// <returns></returns>
        public MUserInfo Login(int userid)
        {
            DbCommand cmd = _db.GetSqlStringCommand(SQL_SELECT_Login + " WHERE [Id]=@UID AND [IsDelete]='0' ");
            _db.AddInParameter(cmd, "UID", DbType.Int32, userid);
            
            return ReadUserInfo(cmd);
        }

        /// <summary>
        /// 用户登录，根据系统公司编号、用户名获取用户信息
        /// </summary>
        /// <param name="companyId">系统公司编号</param>
        /// <param name="username">登录账号</param>
        /// <returns></returns>
        public MUserInfo Login(int companyId, string username)
        {
            DbCommand cmd = _db.GetSqlStringCommand(SQL_SELECT_Login+" WHERE [CompanyId]=@CID AND [UserName]=@UN AND [IsDelete]='0' ");
            _db.AddInParameter(cmd, "CID", DbType.Int32, companyId);
            _db.AddInParameter(cmd, "UN", DbType.String, username);

            return ReadUserInfo(cmd);
        }

        /// <summary>
        /// 写登录日志，用户登录时更新最后登录时间、在线状态、会话标识
        /// </summary>
        /// <param name="info">登录用户信息</param>
        /// <param name="loginType">登录类型</param>
        public void LoginLogwr(MUserInfo info, Model.EnumType.CompanyStructure.UserLoginType loginType)
        {
            string cmdText = SQL_INSERT_LoginLogwr;

            if (loginType == Model.EnumType.CompanyStructure.UserLoginType.用户登录)
            {
                cmdText = SQL_INSERT_LoginLogwr + " UPDATE [tbl_CompanyUser] SET [LastLoginTime]=@IssueTime,[OnlineStatus]=@OnlineStatus,[OnlineSessionId]=@OnlineSessionId,[LastLoginIP] = @Ip WHERE [Id]=@OperatorId;";
            }

            DbCommand cmd = _db.GetSqlStringCommand(cmdText);

            _db.AddInParameter(cmd, "LogId", DbType.AnsiStringFixedLength, Guid.NewGuid().ToString());
            _db.AddInParameter(cmd, "CompanyId", DbType.AnsiStringFixedLength, info.CompanyId);
            _db.AddInParameter(cmd, "OperatorId", DbType.AnsiStringFixedLength, info.UserId);
            _db.AddInParameter(cmd, "IssueTime", DbType.DateTime, DateTime.Now);
            _db.AddInParameter(cmd, "Ip", DbType.String, Utils.GetRemoteIP());
            _db.AddInParameter(cmd, "LoginType", DbType.Byte, loginType);
            _db.AddInParameter(cmd, "Client", DbType.String, new EyouSoft.Toolkit.BrowserInfo().ToJsonString());
            _db.AddInParameter(cmd, "OnlineStatus", DbType.Byte, info.OnlineStatus);
            _db.AddInParameter(cmd, "OnlineSessionId", DbType.AnsiString, info.OnlineSessionId);

            DbHelper.ExecuteSql(cmd, _db);
        }

        /// <summary>
        /// 获取域名信息
        /// </summary>
        /// <param name="domain">域名</param>
        /// <returns></returns>
        public SystemDomain GetDomain(string domain)
        {
            SystemDomain info = null;
            DbCommand cmd = _db.GetSqlStringCommand(SQL_SELECT_GetDomain);
            _db.AddInParameter(cmd, "Domain", DbType.String, domain);

            using (IDataReader rdr = DbHelper.ExecuteReader(cmd, _db))
            {
                if (rdr.Read())
                {
                    info = new SystemDomain()
                    {                        
                        Domain = domain,
                        SysId = rdr.GetInt32(0),
                        Url = rdr.IsDBNull(2) ? string.Empty : rdr.GetString(2)
                    };

                    string xml = rdr[3].ToString();

                    if (!string.IsNullOrEmpty(xml))
                    {
                        XElement xroot = XElement.Parse(xml);
                        var xrow = Utils.GetXElement(xroot, "row");

                        info.CompanyId = Utils.GetInt(Utils.GetXAttributeValue(xrow, "Id"));
                        info.CompanyName = Utils.GetXAttributeValue(xrow, "CompanyName");
                    }
                }
            }

            return info;
        }

        /// <summary>
        /// 获取公司配置信息
        /// </summary>
        /// <param name="companyId">公司编号</param>
        /// <returns></returns>
        public CompanyFieldSetting GetComSetting(int companyId)
        {
            DbCommand cmd = _db.GetSqlStringCommand(SQL_SELECT_GetSettings);
            _db.AddInParameter(cmd, "CompanyId", DbType.AnsiStringFixedLength, companyId);

            var setting = new CompanyFieldSetting();
            
            using (IDataReader rdr = DbHelper.ExecuteReader(cmd, _db))
            {
                while (rdr.Read())
                {
                    string key = rdr["FieldName"].ToString();
                    string value = rdr["FieldValue"].ToString();

                    if (string.IsNullOrEmpty(key)) continue;

                    #region 配置键为字符串处理
                    switch (key)
                    {
                        case "CompanyLogo": setting.CompanyLogo = value; break;
                        case "MaxSonUserNum": setting.MaxSonUserNum = Utils.GetInt(value); break;
                        case "BirthdayReminderDays": setting.BirthdayReminderDays = Utils.GetInt(value); break;
                        case "UserLoginLimitType": 
                            setting.UserLoginLimitType = (Model.EnumType.CompanyStructure.UserLoginLimitType)Utils.GetInt(value);
                            break;
                        default: break;
                    }
                    #endregion
                }

                setting.CompanyId = companyId;
            }

            return setting;
        }

        /// <summary>
        /// 设置用户在线状态
        /// </summary>
        /// <param name="userId">用户编号</param>
        /// <param name="status">在线状态</param>
        /// <param name="onlineSessionId">用户会话状态标识</param>
        /// <returns></returns>
        public bool SetOnlineStatus(int userId, Model.EnumType.CompanyStructure.UserOnlineStatus status, string onlineSessionId)
        {
            DbCommand cmd = _db.GetSqlStringCommand(SQL_UPDATE_SetOnlineStatus);
            _db.AddInParameter(cmd, "OnlineStatus", DbType.Byte, status);
            _db.AddInParameter(cmd, "UserId", DbType.Int32, userId);
            _db.AddInParameter(cmd, "OnlineSessionId", DbType.AnsiString, onlineSessionId);

            return DbHelper.ExecuteSql(cmd, _db) > 0;
        }
        #endregion
    }
}
