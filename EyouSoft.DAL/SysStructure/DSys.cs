using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.Common;
using System.Data;
using EyouSoft.Toolkit.DAL;
using EyouSoft.Toolkit;
using Microsoft.Practices.EnterpriseLibrary.Data;
using System.Xml.Linq;

namespace EyouSoft.DAL.SysStructure
{
    /// <summary>
    /// 系统管理(WEBMASTER)数据访问类
    /// </summary>
    public class DSys : EyouSoft.Toolkit.DAL.DALBase, EyouSoft.IDAL.SysStructure.ISys
    {
        #region static constants
        //static constants       
        const string SQL_SELECT_GetPrivs3 = " SELECT [Id],[ParentId],[Name],[SortId],[IsEnable] FROM [tbl_SysPrivs3] WHERE [ParentId]=@Privs2Id AND [IsEnable] = '1' ORDER BY [SortId]";
        const string SQL_UPDATE_SetWebmasterPwd = "UPDATE tbl_Webmaster SET [Password] = @Password,MD5Password = @MD5Password WHERE [Id] = @Id and [Username] = @Username";
        const string SQL_SELECT_GetDomains = "SELECT A.SysId,A.[Domain],A.Url,(SELECT B.Id FROM tbl_CompanyInfo AS B WHERE B.SystemId = @SysId) AS CompanyId FROM [tbl_SysDomain] AS A WHERE A.SysId = @SysId";
        const string SQL_SELECT_GetSysRoleId = "SELECT [Id] FROM [tbl_SysRoleManage] WHERE [CompanyId]=@CompanyId AND RoleName='管理员'";
        const string SQL_INSERT_InsertPrivs1 = "INSERT INTO [tbl_SysPrivs1]([Name],[SortId],[IsEnable],[ClassName]) VALUES (@Name,@SortId,@IsEnable,@ClassName);SELECT SCOPE_IDENTITY();";
        const string SQL_INSERT_InsertPrivs2 = "INSERT INTO [tbl_SysPrivs2]([ParentId],[Name],[Url],[SortId],[IsEnable]) VALUES(@ParentId,@Name,@Url,@SortId,@IsEnable);SELECT SCOPE_IDENTITY();";
        const string SQL_INSERT_InsertPrivs3 = "INSERT INTO [tbl_SysPrivs3]([ParentId],[Name],[SortId],[IsEnable],[PrivsType]) VALUES(@ParentId,@Name,@SortId,@IsEnable,@PrivsType);SELECT SCOPE_IDENTITY();";
        const string SQL_SELECT_IsExistsPrivsType = "SELECT COUNT(*) FROM [tbl_SysPrivs3] WHERE [ParentId]=@ParentId AND [PrivsType]=@PrivsType";
        const string SQL_SELECT_IsExistsPrivsName = "SELECT COUNT(*) FROM [tbl_SysPrivs3] WHERE [ParentId]=@ParentId AND [Name]=@Name";
        const string SQL_SELECT_IsExistsMenu2Name = "SELECT COUNT(*) FROM [tbl_SysPrivs2] WHERE [ParentId]=@ParentId AND [Name]=@Name";
        const string SQL_SELECT_IsExistsMenu1Name = "SELECT COUNT(*) FROM [tbl_SysPrivs1] WHERE [Name]=@Name";
        const string SQL_SELECT_GetComPrivsInfo = "SELECT [Module],[Part],[Permission] FROM [tbl_Sys] WHERE [SysId]=(SELECT A.[SystemId] FROM [tbl_CompanyInfo] AS A WHERE A.Id=@CompanyId)";
        #endregion

        #region constructor
        /// <summary>
        /// database
        /// </summary>
        Database _db = null;

        /// <summary>
        /// default constructor
        /// </summary>
        public DSys()
        {
            _db = SystemStore;
        }
        #endregion

        #region private members
        /// <summary>
        /// 根据SqlXML生成二级栏目集合
        /// </summary>
        /// <param name="sqlXml">二级栏目SalXml</param>
        /// <param name="privs1Id">一级栏目编号</param>
        /// <returns></returns>
        IList<Model.SysStructure.MSysMenu2Info> ParsePrivs2Xml(string sqlXml, int privs1Id)
        {
            IList<Model.SysStructure.MSysMenu2Info> list = null;
            if (string.IsNullOrEmpty(sqlXml)) return list;

            XElement xRoot = XElement.Parse(sqlXml);
            var xRow = Utils.GetXElements(xRoot, "row");

            if (xRow == null || xRow.Count() < 1) return list;

            list = new List<Model.SysStructure.MSysMenu2Info>();
            foreach (var t in xRow)
            {
                if (t == null) continue;

                var model = new Model.SysStructure.MSysMenu2Info
                {
                    MenuId = Utils.GetInt(Utils.GetXAttributeValue(t, "Id")),
                    Name = Utils.GetXAttributeValue(t, "Name"),
                    Url = Utils.GetXAttributeValue(t, "Url"),
                    Privs = ParsePrivs3Xml(Utils.GetXAttributeValue(t, "Privs3Xml"), Utils.GetInt(Utils.GetXAttributeValue(t, "Id"))),
                    FirstId = privs1Id
                };

                list.Add(model);
            }

            return list;
        }

        /// <summary>
        /// 根据SqlXml生成权限集合
        /// </summary>
        /// <param name="sqlXml">权限Xml</param>
        /// <param name="privs2Id">二级栏目编号</param>
        /// <returns></returns>
        IList<Model.SysStructure.MSysPrivsInfo> ParsePrivs3Xml(string sqlXml, int privs2Id)
        {
            IList<Model.SysStructure.MSysPrivsInfo> list = null;
            if (string.IsNullOrEmpty(sqlXml)) return list;

            XElement xRoot = XElement.Parse(sqlXml);
            var xRow = Utils.GetXElements(xRoot, "row");
            if (xRow == null || xRow.Count() < 1) return list;

            list = new List<Model.SysStructure.MSysPrivsInfo>();
            foreach (var t in xRow)
            {
                if (t == null) continue;

                var model = new Model.SysStructure.MSysPrivsInfo
                {
                    PrivsId = Utils.GetInt(Utils.GetXAttributeValue(t, "Id")),
                    Name = Utils.GetXAttributeValue(t, "Name"),
                    Remark = Utils.GetXAttributeValue(t, "Remark"),
                    SecondId = privs2Id
                };

                list.Add(model);
            }

            return list;
        }
        #endregion

        #region EyouSoft.IDAL.SysStructure.ISys
        /// <summary>
        /// 创建子系统
        /// </summary>
        /// <param name="info">系统信息业务实体</param>
        /// <returns></returns>
        public int CreateSys(EyouSoft.Model.SysStructure.MSysInfo info)
        {
            DbCommand dc = _db.GetStoredProcCommand("proc_Sys_Create");
            _db.AddOutParameter(dc, "SysId", DbType.Int32, 4);
            _db.AddInParameter(dc, "SysName", DbType.String, info.SysName);
            _db.AddOutParameter(dc, "CompanyId", DbType.Int32, 4);
            _db.AddInParameter(dc, "FullName", DbType.String, info.FullName);
            _db.AddInParameter(dc, "Telephone", DbType.String, info.Telephone);
            _db.AddInParameter(dc, "Mobile", DbType.String, info.Mobile);
            _db.AddOutParameter(dc, "UserId", DbType.Int32, 4);
            _db.AddInParameter(dc, "Username", DbType.String, info.Username);
            _db.AddInParameter(dc, "NoEncryptPassword", DbType.String, info.Password.NoEncryptPassword);
            _db.AddInParameter(dc, "MD5Password", DbType.String, info.Password.MD5Password);
            _db.AddInParameter(dc, "IssueTime", DbType.DateTime, info.IssueTime);
            _db.AddOutParameter(dc, "RetCode", DbType.Int32, 4);
            if (info.CompanySetting != null)
            {
                if (!string.IsNullOrEmpty(info.CompanySetting.CompanyLogo))
                    _db.AddInParameter(dc, "CompanyLog", DbType.String, info.CompanySetting.CompanyLogo);
                if(info.CompanySetting.MaxSonUserNum > 0)
                    _db.AddInParameter(dc, "MaxSonUserNum", DbType.Int32, info.CompanySetting.MaxSonUserNum);
                if (info.CompanySetting.BirthdayReminderDays > 0)
                    _db.AddInParameter(dc, "BirthdayReminderDays", DbType.Int32, info.CompanySetting.BirthdayReminderDays);
            }

            DbHelper.RunProcedure(dc, _db);

            object obj = _db.GetParameterValue(dc, "RetCode");
            if (obj == null || string.IsNullOrEmpty(obj.ToString())) return 0;

            return Utils.GetInt(obj.ToString());
        }

        /// <summary>
        /// 设置系统域名
        /// </summary>
        /// <param name="sysId">系统编号</param>
        /// <param name="domains">域名信息集合</param>
        /// <returns></returns>
        public int SetSysDomains(int sysId, IList<EyouSoft.Model.SysStructure.SystemDomain> domains)
        {
            var strSQL = new StringBuilder();
            strSQL.Append("DELETE FROM [tbl_SysDomain] WHERE [SysId] = @SysId;");
            foreach (var t in domains)
            {
                strSQL.AppendFormat(" INSERT INTO [tbl_SysDomain] (SysId,Domain,Url) VALUES ('{0}','{1}','{2}') ; ", sysId, t.Domain, t.Url);
            }

            DbCommand dc = _db.GetSqlStringCommand(strSQL.ToString());
            _db.AddInParameter(dc, "SysId", DbType.AnsiStringFixedLength, sysId);

            return DbHelper.ExecuteSql(dc, _db) > 0 ? 1 : 0;
        }

        /// <summary>
        /// 设置子系统一级栏目、二级栏目、明细权限
        /// </summary>
        /// <param name="sysId">子系统编号</param>
        /// <param name="privs">权限信息</param>
        /// <returns></returns>
        public int SetComPrivs(int sysId, EyouSoft.Model.SysStructure.MComPrivsInfo privs)
        {
            string strSQL = "UPDATE [tbl_Sys] SET [Module]=@Privs1,[Part]=@Privs2,[Permission]=@Privs3 WHERE [SysId]=@SysId";
            DbCommand cmd = _db.GetSqlStringCommand(strSQL);
            _db.AddInParameter(cmd, "Privs1", DbType.String, Utils.GetSqlIdStrByArray(privs.Privs1));
            _db.AddInParameter(cmd, "Privs2", DbType.String, Utils.GetSqlIdStrByArray(privs.Privs2));
            _db.AddInParameter(cmd, "Privs3", DbType.String, Utils.GetSqlIdStrByArray(privs.Privs3));
            _db.AddInParameter(cmd, "SysId", DbType.Int32, sysId);

            return DbHelper.ExecuteSql(cmd, _db) > 0 ? 1 : 0;
        }

        /// <summary>
        /// 设置角色权限为子系统开通的所有权限
        /// </summary>
        /// <param name="roleId">角色编号</param>
        /// <param name="sysId">系统编号</param>
        /// <returns></returns>
        public int SetRoleBySysPrivs(int roleId, int sysId)
        {
            var strSQL = new StringBuilder();
            strSQL.Append(" UPDATE [tbl_SysRoleManage] SET RoleChilds =  ");
            strSQL.Append(" ISNULL((SELECT [Permission] FROM [tbl_Sys] WHERE [SysId]=@SysId),'') ");
            strSQL.Append(" WHERE Id = @RoleId ");

            DbCommand dc = _db.GetSqlStringCommand(strSQL.ToString());
            _db.AddInParameter(dc, "SysId", DbType.Int32, sysId);
            _db.AddInParameter(dc, "RoleId", DbType.Int32, roleId);

            return DbHelper.ExecuteSql(dc, _db) > 0 ? 1 : 0;
        }

        /// <summary>
        /// 设置用户权限为子系统开通的所有权限
        /// </summary>
        /// <param name="userId">用户编号</param>
        /// <param name="sysId">系统编号</param>
        /// <returns></returns>
        public int SetUserBySysPrivs(int userId, int sysId)
        {
            var strSQL = new StringBuilder();
            strSQL.Append(" UPDATE [tbl_CompanyUser] SET PermissionList =  ");
            strSQL.Append(" ISNULL((SELECT [Permission] FROM [tbl_Sys] WHERE [SysId]=@SysId),'') ");
            strSQL.Append(" WHERE Id = @UserId ");

            DbCommand dc = _db.GetSqlStringCommand(strSQL.ToString());
            _db.AddInParameter(dc, "SysId", DbType.Int32, sysId);
            _db.AddInParameter(dc, "UserId", DbType.Int32, userId);

            return DbHelper.ExecuteSql(dc, _db) > 0 ? 1 : 0;
        }

        /// <summary>
        /// 设置用户角色
        /// </summary>
        /// <param name="userId">用户编号</param>
        /// <param name="roleId">角色编号</param>
        /// <returns></returns>
        public bool SetUserRole(int userId, int roleId)
        {
            string strSQL = "UPDATE [tbl_CompanyUser] SET [RoleID]=@RoleId WHERE [Id]=@UserId";
            DbCommand cmd = _db.GetSqlStringCommand(strSQL);
            _db.AddInParameter(cmd, "RoleId", DbType.Int32, roleId);
            _db.AddInParameter(cmd, "UserId", DbType.Int32, userId);

            return DbHelper.ExecuteSql(cmd, _db) > 0;
        }

        /// <summary>
        /// 获取子系统信息集合
        /// </summary>
        /// <param name="pageSize">每页显示记录数</param>
        /// <param name="pageIndex">当前页索引</param>        
        /// <param name="recordCount">总记录数</param>
        /// <param name="searchInfo">查询信息</param>
        /// <returns></returns>
        public IList<EyouSoft.Model.SysStructure.MSysInfo> GetSyss(int pageSize, int pageIndex, ref int recordCount, EyouSoft.Model.SysStructure.MSysSearchInfo searchInfo)
        {
            IList<Model.SysStructure.MSysInfo> items = new List<Model.SysStructure.MSysInfo>();

            StringBuilder fields = new StringBuilder();
            fields.Append(" SysId,SysName,CreateTime ");
            fields.Append(" ,(SELECT A.Id FROM [tbl_CompanyInfo] AS A WHERE A.[SystemId]=tbl_Sys.SysId) AS CompanyId ");
            fields.Append(" ,(SELECT TOP(1) A.Id AS UserId,A.UserName,A.[Password],A.MD5Password,A.ContactName,A.ContactTel,A.ContactMobile,A.OnlineStatus FROM tbl_CompanyUser AS A WHERE A.CompanyId=(SELECT B.Id FROM [tbl_CompanyInfo] AS B WHERE B.SystemId=tbl_Sys.SysId) AND A.IsAdmin = '1' AND A.IsDelete='0' FOR XML RAW,ROOT('root'))  AS AdminXML ");

            string tableName = "tbl_Sys";
            string orderByString = " [CreateTime] ASC ";

            using (IDataReader rdr = DbHelper.ExecuteReader1(_db, pageSize, pageIndex, ref recordCount, tableName, fields.ToString(), string.Empty, orderByString, "SUM(SysId),Sum(CompanyId)"))
            {
                while (rdr.Read())
                {
                    var item = new Model.SysStructure.MSysInfo();
                    if (!rdr.IsDBNull(rdr.GetOrdinal("SysId")))
                        item.SysId = rdr.GetInt32(rdr.GetOrdinal("SysId"));
                    if (!rdr.IsDBNull(rdr.GetOrdinal("SysName")))
                        item.SysName = rdr.GetString(rdr.GetOrdinal("SysName"));
                    if (!rdr.IsDBNull(rdr.GetOrdinal("CreateTime")))
                        item.IssueTime = rdr.GetDateTime(rdr.GetOrdinal("CreateTime"));
                    if (!rdr.IsDBNull(rdr.GetOrdinal("CompanyId")))
                        item.CompanyId = rdr.GetInt32(rdr.GetOrdinal("CompanyId"));

                    string xml = rdr["AdminXML"].ToString();
                    if (!string.IsNullOrEmpty(xml))
                    {
                        XElement xRoot = XElement.Parse(xml);
                        var xRow = Utils.GetXElement(xRoot, "row");

                        item.UserId = Utils.GetInt(Utils.GetXAttributeValue(xRow, "UserId"));
                        item.Username = Utils.GetXAttributeValue(xRow, "UserName");
                        item.FullName = Utils.GetXAttributeValue(xRow, "ContactName");
                        item.Telephone = Utils.GetXAttributeValue(xRow, "ContactTel");
                        item.Mobile = Utils.GetXAttributeValue(xRow, "ContactMobile");
                        item.Password = new Model.CompanyStructure.PassWord { NoEncryptPassword = Utils.GetXAttributeValue(xRow, "Password") };
                        item.OnlineStatus = Utils.GetEnumValue<EyouSoft.Model.EnumType.CompanyStructure.UserOnlineStatus>(Utils.GetXAttributeValue(xRow, "OnlineStatus"), EyouSoft.Model.EnumType.CompanyStructure.UserOnlineStatus.Offline);
                    }

                    items.Add(item);
                }

                rdr.NextResult();

                if (rdr.Read())
                {

                }
            }

            return items;
        }

        /// <summary>
        /// 获取子系统信息
        /// </summary>
        /// <param name="sysId">子系统编号</param>
        /// <returns></returns>
        public EyouSoft.Model.SysStructure.MSysInfo GetSysInfo(int sysId)
        {
            Model.SysStructure.MSysInfo item = null;
            StringBuilder strSQL = new StringBuilder();
            strSQL.Append(" SELECT ");
            strSQL.Append(" SysId,SysName,CreateTime ");
            strSQL.Append(" ,(SELECT A.Id FROM [tbl_CompanyInfo] AS A WHERE A.[SystemId]=tbl_Sys.SysId) AS CompanyId ");
            strSQL.Append(" ,(SELECT TOP(1) A.Id AS UserId,A.UserName,A.[Password],A.MD5Password,A.ContactName,A.ContactTel,A.ContactMobile,A.OnlineStatus FROM tbl_CompanyUser AS A WHERE A.CompanyId=(SELECT B.Id FROM [tbl_CompanyInfo] AS B WHERE B.SystemId=tbl_Sys.SysId) AND A.IsAdmin = '1' AND A.IsDelete='0' FOR XML RAW,ROOT('root'))  AS AdminXML ");
            strSQL.Append(" FROM tbl_Sys WHERE SysId=@SysId ");

            DbCommand cmd = _db.GetSqlStringCommand(strSQL.ToString());
            _db.AddInParameter(cmd, "SysId", DbType.AnsiStringFixedLength, sysId);

            using (IDataReader rdr = DbHelper.ExecuteReader(cmd, _db))
            {
                if (rdr.Read())
                {
                    item = new Model.SysStructure.MSysInfo();
                    if (!rdr.IsDBNull(rdr.GetOrdinal("SysId")))
                        item.SysId = rdr.GetInt32(rdr.GetOrdinal("SysId"));
                    if (!rdr.IsDBNull(rdr.GetOrdinal("SysName")))
                        item.SysName = rdr.GetString(rdr.GetOrdinal("SysName"));
                    if (!rdr.IsDBNull(rdr.GetOrdinal("CreateTime")))
                        item.IssueTime = rdr.GetDateTime(rdr.GetOrdinal("CreateTime"));
                    if (!rdr.IsDBNull(rdr.GetOrdinal("CompanyId")))
                        item.CompanyId = rdr.GetInt32(rdr.GetOrdinal("CompanyId"));

                    string xml = rdr["AdminXML"].ToString();
                    if (!string.IsNullOrEmpty(xml))
                    {
                        XElement xRoot = XElement.Parse(xml);
                        var xRow = Utils.GetXElement(xRoot, "row");

                        item.UserId = Utils.GetInt(Utils.GetXAttributeValue(xRow, "UserId"));
                        item.Username = Utils.GetXAttributeValue(xRow, "UserName");
                        item.FullName = Utils.GetXAttributeValue(xRow, "ContactName");
                        item.Telephone = Utils.GetXAttributeValue(xRow, "ContactTel");
                        item.Mobile = Utils.GetXAttributeValue(xRow, "ContactMobile");
                        item.Password = new Model.CompanyStructure.PassWord { NoEncryptPassword = Utils.GetXAttributeValue(xRow, "Password") };
                        item.OnlineStatus = Utils.GetEnumValue<EyouSoft.Model.EnumType.CompanyStructure.UserOnlineStatus>(Utils.GetXAttributeValue(xRow, "OnlineStatus"), EyouSoft.Model.EnumType.CompanyStructure.UserOnlineStatus.Offline);
                    }
                }
            }

            return item;
        }

        /// <summary>
        /// 获取一级栏目信息集合
        /// </summary>
        /// <returns></returns>
        public IList<EyouSoft.Model.SysStructure.MSysMenu1Info> GetPrivs1()
        {
            var strSql = new StringBuilder(" SELECT A.[Id],A.[Name],A.[SortId],A.[IsEnable],A.[ClassName] ");
            strSql.Append(" ,(SELECT B.Id,B.Name,B.Url,(SELECT C.Id,C.Name,C.Remark FROM tbl_SysPrivs3 AS C WHERE C.ParentId = B.Id AND C.IsEnable = '1' ORDER BY C.SortId ASC FOR XML RAW,ROOT('root')) AS Privs3Xml FROM tbl_SysPrivs2 AS B WHERE B.ParentId = A.Id AND B.IsEnable = '1' ORDER BY B.SortId ASC FOR XML RAW,ROOT('root')) AS Privs2Xml ");
            strSql.Append(" FROM [tbl_SysPrivs1] AS A ");
            strSql.Append(" WHERE A.IsEnable = '1' ORDER BY A.SortId ");

            DbCommand cmd = _db.GetSqlStringCommand(strSql.ToString());

            IList<Model.SysStructure.MSysMenu1Info> items = new List<Model.SysStructure.MSysMenu1Info>();
            using (IDataReader rdr = DbHelper.ExecuteReader(cmd, _db))
            {
                while (rdr.Read())
                {
                    var item = new Model.SysStructure.MSysMenu1Info();
                    if (!rdr.IsDBNull(rdr.GetOrdinal("Id")))
                        item.MenuId = rdr.GetInt32(rdr.GetOrdinal("Id"));
                    if (!rdr.IsDBNull(rdr.GetOrdinal("Name")))
                        item.Name = rdr.GetString(rdr.GetOrdinal("Name"));
                    if (!rdr.IsDBNull(rdr.GetOrdinal("Privs2Xml")))
                        item.Menu2s = ParsePrivs2Xml(rdr.GetString(rdr.GetOrdinal("Privs2Xml")), item.MenuId);
                    item.ClassName = rdr["ClassName"].ToString();

                    items.Add(item);
                }
            }

            return items;
        }

        /// <summary>
        /// 获取明细权限信息集合
        /// </summary>
        /// <param name="privs2Id">二级栏目编号</param>
        /// <returns></returns>
        public IList<EyouSoft.Model.SysStructure.MSysPrivsInfo> GetPrivs3(int privs2Id)
        {
            IList<Model.SysStructure.MSysPrivsInfo> items = null;

            DbCommand cmd = _db.GetSqlStringCommand(SQL_SELECT_GetPrivs3);
            _db.AddInParameter(cmd, "Privs2Id", DbType.Int32, privs2Id);

            using (IDataReader rdr = DbHelper.ExecuteReader(cmd, _db))
            {
                items = new List<Model.SysStructure.MSysPrivsInfo>();
                while (rdr.Read())
                {
                    var item = new Model.SysStructure.MSysPrivsInfo();
                    if (!rdr.IsDBNull(rdr.GetOrdinal("Id")))
                        item.PrivsId = rdr.GetInt32(rdr.GetOrdinal("Id"));
                    if (!rdr.IsDBNull(rdr.GetOrdinal("Name")))
                        item.Name = rdr.GetString(rdr.GetOrdinal("Name"));

                    items.Add(item);
                }
            }

            return items;
        }

        /// <summary>
        /// set webmaster pwd
        /// </summary>
        /// <param name="webmasterId">webmaster id</param>
        /// <param name="username">webmaster username</param>
        /// <param name="pwd">webmaster pwd info</param>
        /// <returns></returns>
        public bool SetWebmasterPwd(int webmasterId, string username, EyouSoft.Model.CompanyStructure.PassWord pwd)
        {
            DbCommand cmd = _db.GetSqlStringCommand(SQL_UPDATE_SetWebmasterPwd);
            _db.AddInParameter(cmd, "Password", DbType.String, pwd.NoEncryptPassword);
            _db.AddInParameter(cmd, "MD5Password", DbType.String, pwd.MD5Password);
            _db.AddInParameter(cmd, "Id", DbType.Int32, webmasterId);
            _db.AddInParameter(cmd, "Username", DbType.String, username);

            return DbHelper.ExecuteSql(cmd, _db) > 0;
        }

        /// <summary>
        /// 获取系统域名信息集合
        /// </summary>
        /// <param name="sysId">系统编号</param>
        /// <returns></returns>
        public IList<EyouSoft.Model.SysStructure.SystemDomain> GetDomains(int sysId)
        {
            IList<Model.SysStructure.SystemDomain> items = null;

            DbCommand cmd = _db.GetSqlStringCommand(SQL_SELECT_GetDomains);
            _db.AddInParameter(cmd, "SysId", DbType.Int32, sysId);

            using (IDataReader rdr = DbHelper.ExecuteReader(cmd, _db))
            {
                items = new List<Model.SysStructure.SystemDomain>();
                while (rdr.Read())
                {
                    var item = new Model.SysStructure.SystemDomain();
                    if (!rdr.IsDBNull(rdr.GetOrdinal("SysId")))
                        item.SysId = rdr.GetInt32(rdr.GetOrdinal("SysId"));
                    if (!rdr.IsDBNull(rdr.GetOrdinal("Domain")))
                        item.Domain = rdr.GetString(rdr.GetOrdinal("Domain"));
                    if (!rdr.IsDBNull(rdr.GetOrdinal("Url")))
                        item.Url = rdr.GetString(rdr.GetOrdinal("Url"));
                    if (!rdr.IsDBNull(rdr.GetOrdinal("CompanyId")))
                        item.CompanyId = rdr.GetInt32(rdr.GetOrdinal("CompanyId"));

                    items.Add(item);
                }
            }

            return items;
        }

        /// <summary>
        /// 判断域名是否重复，返回重复的域名信息集合
        /// </summary>
        /// <param name="sysId">系统编号</param>
        /// <param name="domains">域名集合</param>
        /// <returns></returns>
        public IList<string> IsExistsDomains(int sysId, IList<string> domains)
        {
            IList<string> items = null;

            var strSql = new StringBuilder(" SELECT [Domain] FROM [tbl_SysDomain] WHERE SysId <> @SysId ");
            if (domains.Count == 1)
            {
                strSql.AppendFormat(" AND [Domain] = '{0}' ", domains[0]);
            }
            else
            {
                strSql.AppendFormat(" AND [Domain] IN ({0}) ", Utils.GetSqlInExpression(domains));
            }

            DbCommand cmd = _db.GetSqlStringCommand(strSql.ToString());
            _db.AddInParameter(cmd, "SysId", DbType.Int32, sysId);

            using (IDataReader rdr = DbHelper.ExecuteReader(cmd, _db))
            {
                items = new List<string>();
                while (rdr.Read())
                {
                    if (!rdr.IsDBNull(rdr.GetOrdinal("Domain")))
                        items.Add(rdr.GetString(rdr.GetOrdinal("Domain")));
                }
            }

            return items;
        }

        /// <summary>
        /// 获取子系统角色(管理员)编号
        /// </summary>
        /// <param name="companyId">公司编号</param>
        /// <returns></returns>
        public int GetSysRoleId(int companyId)
        {
            DbCommand cmd = _db.GetSqlStringCommand(SQL_SELECT_GetSysRoleId);
            _db.AddInParameter(cmd, "CompanyId", DbType.Int32, companyId);

            using (IDataReader rdr = DbHelper.ExecuteReader(cmd, _db))
            {
                if (rdr.Read())
                {
                    return rdr.GetInt32(0);
                }
            }

            return 0;
        }

        /// <summary>
        /// 基础权限管理-写入一级栏目
        /// </summary>
        /// <param name="info">一级栏目信息业务实体</param>
        /// <returns></returns>
        public int InsertPrivs1(EyouSoft.Model.SysStructure.MSysMenu1Info info)
        {
            DbCommand cmd = _db.GetSqlStringCommand(SQL_INSERT_InsertPrivs1);
            _db.AddInParameter(cmd, "Name", DbType.String, info.Name);
            _db.AddInParameter(cmd, "SortId", DbType.Int32, 0);
            _db.AddInParameter(cmd, "IsEnable", DbType.AnsiStringFixedLength, "1");
            _db.AddInParameter(cmd, "ClassName", DbType.String, info.ClassName);

            using (IDataReader rdr = DbHelper.ExecuteReader(cmd, _db))
            {
                if (rdr.Read())
                {
                    return Convert.ToInt32(rdr.GetDecimal(0));
                }
            }

            return 0;
        }

        /// <summary>
        /// 基础权限管理-写入二级栏目
        /// </summary>
        /// <param name="info">二级栏目信息业务实体</param>
        /// <returns></returns>
        public int InsertPrivs2(EyouSoft.Model.SysStructure.MSysMenu2Info info)
        {
            DbCommand cmd = _db.GetSqlStringCommand(SQL_INSERT_InsertPrivs2);
            _db.AddInParameter(cmd, "ParentId", DbType.Int32, info.FirstId);
            _db.AddInParameter(cmd, "Name", DbType.String, info.Name);
            _db.AddInParameter(cmd, "Url", DbType.String, info.Url);
            _db.AddInParameter(cmd, "SortId", DbType.Int32, 0);
            _db.AddInParameter(cmd, "IsEnable", DbType.AnsiStringFixedLength, "1");

            using (IDataReader rdr = DbHelper.ExecuteReader(cmd, _db))
            {
                if (rdr.Read())
                {
                    return Convert.ToInt32(rdr.GetDecimal(0));
                }
            }

            return 0;
        }

        /// <summary>
        /// 基础权限管理-写入明细权限
        /// </summary>
        /// <param name="info">权限信息业务实体</param>
        /// <returns></returns>
        public int InsertPrivs3(EyouSoft.Model.SysStructure.MSysPrivsInfo info)
        {
            DbCommand cmd = _db.GetSqlStringCommand(SQL_INSERT_InsertPrivs3);
            _db.AddInParameter(cmd, "ParentId", DbType.Int32, info.SecondId);
            _db.AddInParameter(cmd, "Name", DbType.String, info.Name);
            _db.AddInParameter(cmd, "SortId", DbType.Int32, 0);
            _db.AddInParameter(cmd, "IsEnable", DbType.AnsiStringFixedLength, "1");
            _db.AddInParameter(cmd, "PrivsType", DbType.Byte, info.PrivsType);

            using (IDataReader rdr = DbHelper.ExecuteReader(cmd, _db))
            {
                if (rdr.Read())
                {
                    return Convert.ToInt32(rdr.GetDecimal(0));
                }
            }

            return 0;
        }

        /// <summary>
        /// 相同二级栏目下是否存在相同的权限类别
        /// </summary>
        /// <param name="secondId">二级栏目编号</param>
        /// <param name="privsType">权限类别</param>
        /// <returns></returns>
        public bool IsExistsPrivs3Type(int privs2Id, EyouSoft.Model.EnumType.SysStructure.PrivsType privsType)
        {
            DbCommand cmd = _db.GetSqlStringCommand(SQL_SELECT_IsExistsPrivsType);
            _db.AddInParameter(cmd, "ParentId", DbType.Int32, privs2Id);
            _db.AddInParameter(cmd, "PrivsType", DbType.Byte, privsType);

            using (IDataReader rdr = DbHelper.ExecuteReader(cmd, _db))
            {
                if (rdr.Read())
                {
                    return rdr.GetInt32(0) > 0;
                }
            }

            return true;
        }

        /// <summary>
        ///  相同二级栏目下是否存在相同的权限名称
        /// </summary>
        /// <param name="privs2Id">二级栏目编号</param>
        /// <param name="privsName">权限名称</param>
        /// <returns></returns>
        public bool IsExistsPrivs3Name(int privs2Id, string privsName)
        {
            DbCommand cmd = _db.GetSqlStringCommand(SQL_SELECT_IsExistsPrivsName);
            _db.AddInParameter(cmd, "ParentId", DbType.Int32, privs2Id);
            _db.AddInParameter(cmd, "Name", DbType.String, privsName);

            using (IDataReader rdr = DbHelper.ExecuteReader(cmd, _db))
            {
                if (rdr.Read())
                {
                    return rdr.GetInt32(0) > 0;
                }
            }

            return true;
        }

        /// <summary>
        /// 相同一级栏目下是否存在相同的二级栏目名称
        /// </summary>
        /// <param name="privs1Id">一级栏目编号</param>
        /// <param name="menu2Name">二级栏目名称</param>
        /// <returns></returns>
        public bool IsExistsPrivs2Name(int privs1Id, string menu2Name)
        {
            DbCommand cmd = _db.GetSqlStringCommand(SQL_SELECT_IsExistsMenu2Name);
            _db.AddInParameter(cmd, "ParentId", DbType.Int32, privs1Id);
            _db.AddInParameter(cmd, "Name", DbType.String, menu2Name);

            using (IDataReader rdr = DbHelper.ExecuteReader(cmd, _db))
            {
                if (rdr.Read())
                {
                    return rdr.GetInt32(0) > 0;
                }
            }

            return true;
        }

        /// <summary>
        /// 是否存在相同的一级栏目名称
        /// </summary>
        /// <param name="privs1Name">一级栏目名称</param>
        /// <returns></returns>
        public bool IsExistsPrivs1Name(string privs1Name)
        {
            DbCommand cmd = _db.GetSqlStringCommand(SQL_SELECT_IsExistsMenu1Name);
            _db.AddInParameter(cmd, "Name", DbType.String, privs1Name);

            using (IDataReader rdr = DbHelper.ExecuteReader(cmd, _db))
            {
                if (rdr.Read())
                {
                    return rdr.GetInt32(0) > 0;
                }
            }

            return true;
        }

        /// <summary>
        /// 获取公司一级栏目、二级栏目、明细权限信息业务实体
        /// </summary>
        /// <param name="companyId">公司编号</param>
        /// <returns></returns>
        public EyouSoft.Model.SysStructure.MComPrivsInfo GetComPrivsInfo(int companyId)
        {
            EyouSoft.Model.SysStructure.MComPrivsInfo info = null;
            DbCommand cmd = _db.GetSqlStringCommand(SQL_SELECT_GetComPrivsInfo);
            _db.AddInParameter(cmd, "CompanyId", DbType.Int32, companyId);

            using (IDataReader rdr = DbHelper.ExecuteReader(cmd, _db))
            {
                if (rdr.Read())
                {
                    info = new EyouSoft.Model.SysStructure.MComPrivsInfo();

                    info.Privs1 = Utils.Split(rdr[0].ToString(), ",");
                    info.Privs2 = Utils.Split(rdr[1].ToString(), ",");
                    info.Privs3 = Utils.Split(rdr[2].ToString(), ",");
                }
            }

            return info;
        }

        #endregion
    }
}
