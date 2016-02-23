using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Practices.EnterpriseLibrary.Data;
using System.Data.Common;
using System.Data;

namespace EyouSoft.DAL.CompanyStructure
{
    /// <summary>
    /// 系统登录日志DAL
    /// Author xuqh 2011-01-22
    /// </summary>
    public class SysLoginLog : Toolkit.DAL.DALBase, IDAL.CompanyStructure.ISysLoginLog
    {
        #region static constants

        private const string SqlInsertSysLoginLog = " insert into tbl_SysLoginLog (ID,OperatorId,CompanyId,LoginTime,LoginIp,LoginType,BrowserType) values (@ID,@Operator,@CompanyId,@LoginTime,@LoginIp,@LoginType,@BrowserType); ";
        private const string SqlSelectSysLoginLog = " select a.*,b.UserName,b.ContactName from tbl_SysLoginLog as a left join tbl_CompanyUser as b on a.OperatorId = b.Id ";
        private readonly Database _db = null;

        #endregion

        #region 构造函数
        public SysLoginLog()
        {
            this._db = SystemStore;
        }
        #endregion

        #region ISysLoginLog 成员

        /// <summary>
        /// 添加
        /// </summary>
        /// <param name="model">系统登陆日志实体</param>
        /// <returns></returns>
        public bool Add(Model.CompanyStructure.SysLoginLog model)
        {
            model.ID = string.IsNullOrEmpty(model.ID) ? Guid.NewGuid().ToString() : model.ID;
            DbCommand cmd = this._db.GetSqlStringCommand(SqlInsertSysLoginLog);

            this._db.AddInParameter(cmd, "ID", DbType.AnsiStringFixedLength, model.ID);
            this._db.AddInParameter(cmd, "Operator", DbType.Int32, model.UserId);
            this._db.AddInParameter(cmd, "CompanyId", DbType.Int32, model.CompanyId);
            this._db.AddInParameter(cmd, "LoginTime", DbType.DateTime, DateTime.Now);
            this._db.AddInParameter(cmd, "LoginIp", DbType.String, model.LoginIp);
            this._db.AddInParameter(cmd, "LoginType", DbType.Byte, (int)model.LoginType);
            this._db.AddInParameter(cmd, "BrowserType", DbType.String, model.BrowserType);

            return Toolkit.DAL.DbHelper.ExecuteSql(cmd, this._db) > 0 ? true : false;
        }

        /// <summary>
        /// 获取登录日志实体
        /// </summary>
        /// <param name="id">主键编号</param>
        /// <returns>操作日志实体</returns>
        public Model.CompanyStructure.SysLoginLog GetModel(string id)
        {
            Model.CompanyStructure.SysLoginLog sysLoginLogModel = null;
            DbCommand cmd = this._db.GetSqlStringCommand(SqlSelectSysLoginLog);
            this._db.AddInParameter(cmd, "ID", DbType.String, id);

            using (IDataReader rdr = Toolkit.DAL.DbHelper.ExecuteReader(cmd, this._db))
            {
                if (rdr.Read())
                {
                    sysLoginLogModel = new Model.CompanyStructure.SysLoginLog
                        {
                            ID = rdr.GetString(rdr.GetOrdinal("Id")),
                            UserId = rdr.GetInt32(rdr.GetOrdinal("OperatorId")),
                            UserName = rdr["UserName"].ToString(),
                            ContactName = rdr["ContactName"].ToString(),
                            CompanyId = rdr.GetInt32(rdr.GetOrdinal("CompanyId")),
                            LoginTime = rdr.GetDateTime(rdr.GetOrdinal("LoginTime")),
                            LoginIp = rdr.GetString(rdr.GetOrdinal("LoginIp")),
                            LoginType =
                                (Model.EnumType.CompanyStructure.UserLoginType)rdr.GetInt32(rdr.GetOrdinal("LoginType")),
                            BrowserType = rdr["BrowserType"].ToString()
                        };
                }
            }

            return sysLoginLogModel;
        }

        /// <summary>
        /// 分页获取登录日志列表
        /// </summary>
        /// <param name="pageSize">每页现实条数</param>
        /// <param name="pageIndex">当前页码</param>
        /// <param name="recordCount">总记录数</param>
        /// <param name="model">系统登录日志查询实体</param>
        /// <returns></returns>
        public IList<Model.CompanyStructure.SysLoginLog> GetList(int pageSize, int pageIndex, ref int recordCount
            , Model.CompanyStructure.QuerySysLoginLog model)
        {
            IList<Model.CompanyStructure.SysLoginLog> totals = new List<Model.CompanyStructure.SysLoginLog>();

            string tableName = SqlSelectSysLoginLog;
            string orderByString = " LoginTime DESC ";
            string fields = " * ";

            var cmdQuery = new StringBuilder();
            cmdQuery.AppendFormat(
                " CompanyId = {0} and LoginType = {1} ",
                model.CompanyId,
                (int)Model.EnumType.CompanyStructure.UserLoginType.用户登录);
            if (model.UserId > 0)
                cmdQuery.AppendFormat(" and OperatorId = {0}", model.UserId);
            if (model.StartTime.HasValue)
                cmdQuery.AppendFormat(" and datediff(dd,'{0}',LoginTime) >= 0 ", model.StartTime.Value);
            if (model.EndTime.HasValue)
                cmdQuery.AppendFormat(" and datediff(dd,'{0}',LoginTime) <= 0 ", model.EndTime.Value);
            if (!string.IsNullOrEmpty(model.ContactName))
            {
                cmdQuery.AppendFormat(" and ContactName like '%{0}%' ", Toolkit.Utils.ToSqlLike(model.ContactName));
            }

            using (IDataReader rdr = Toolkit.DAL.DbHelper.ExecuteReader2(this._db, pageSize, pageIndex, ref recordCount, tableName
                , fields, cmdQuery.ToString(), orderByString, string.Empty))
            {
                while (rdr.Read())
                {
                    var sysHandleLogsInfo = new Model.CompanyStructure.SysLoginLog
                    {
                        ID = rdr.GetString(rdr.GetOrdinal("Id")),
                        UserId = rdr.GetInt32(rdr.GetOrdinal("OperatorId")),
                        UserName = rdr["UserName"].ToString(),
                        ContactName = rdr["ContactName"].ToString(),
                        CompanyId = rdr.GetInt32(rdr.GetOrdinal("CompanyId")),
                        LoginTime = rdr.GetDateTime(rdr.GetOrdinal("LoginTime")),
                        LoginIp = rdr.GetString(rdr.GetOrdinal("LoginIp")),
                        LoginType =
                            (Model.EnumType.CompanyStructure.UserLoginType)rdr.GetByte(rdr.GetOrdinal("LoginType")),
                        BrowserType = rdr["BrowserType"].ToString()
                    };

                    totals.Add(sysHandleLogsInfo);
                }
            }

            return totals;
        }

        #endregion
    }
}
