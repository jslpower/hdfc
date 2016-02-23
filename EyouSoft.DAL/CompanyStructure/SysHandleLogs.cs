using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Practices.EnterpriseLibrary.Data;
using System.Data.Common;
using System.Data;

namespace EyouSoft.DAL.CompanyStructure
{
    /// <summary>
    /// 系统操作日志DAL
    /// </summary>
    public class SysHandleLogs : Toolkit.DAL.DALBase, IDAL.CompanyStructure.ISysHandleLogs
    {

        #region static constants
        private const string SqlInsertSysHandleLogs = "insert into tbl_SysHandleLogs (ID,OperatorId,DepatId,CompanyId,ModuleId,EventCode,EventMessage,EventTitle,EventIp) values (@ID,@OperatorId,@DepatId,@CompanyId,@ModuleId,@EventCode,@EventMessage,@EventTitle,@EventIp) ;";
        private const string SqlSelectSysHandleLogs = " SELECT a.ID, a.OperatorId, a.DepatId, a.CompanyId, a.ModuleId, a.EventCode, a.EventMessage, a.EventTitle, a.EventTime, a.EventIp, ISNULL(b.ContactName, '') AS UserName, ISNULL(c.DepartName, '') AS DepartName FROM         tbl_SysHandleLogs AS a LEFT OUTER JOIN tbl_CompanyUser AS b ON a.OperatorId = b.Id LEFT OUTER JOIN dbo.tbl_CompanyDepartment AS c ON a.DepatId = c.Id ";
        private readonly Database _db;
        #endregion

        #region 构造函数
        public SysHandleLogs()
        {
            this._db = SystemStore;
        }
        #endregion

        #region ISysHandleLogs 成员

        /// <summary>
        /// 添加操作日志
        /// </summary>
        /// <param name="model">系统操作日志实体</param>
        /// <returns>true:成功 false:失败</returns>
        public bool Add(Model.CompanyStructure.SysHandleLogs model)
        {
            model.ID = string.IsNullOrEmpty(model.ID) ? Guid.NewGuid().ToString() : model.ID;
            DbCommand cmd = this._db.GetSqlStringCommand(SqlInsertSysHandleLogs);

            this._db.AddInParameter(cmd, "ID", DbType.AnsiStringFixedLength, model.ID);
            this._db.AddInParameter(cmd, "OperatorId", DbType.Int32, model.OperatorId);
            this._db.AddInParameter(cmd, "DepatId", DbType.Int32, model.DepatId);
            this._db.AddInParameter(cmd, "CompanyId", DbType.Int32, model.CompanyId);
            this._db.AddInParameter(cmd, "ModuleId", DbType.Int32, (int)model.ModuleId);
            this._db.AddInParameter(cmd, "EventCode", DbType.Int32, model.EventCode);
            this._db.AddInParameter(cmd, "EventMessage", DbType.String, model.EventMessage);
            this._db.AddInParameter(cmd, "EventTitle", DbType.String, model.EventTitle);
            this._db.AddInParameter(cmd, "EventIp", DbType.String, model.EventIp);

            return Toolkit.DAL.DbHelper.ExecuteSql(cmd, this._db) > 0 ? true : false;
        }

        /// <summary>
        /// 获取操作日志实体
        /// </summary>
        /// <param name="id">主键编号</param>
        /// <returns>操作日志实体</returns>
        public Model.CompanyStructure.SysHandleLogs GetModel(string id)
        {
            Model.CompanyStructure.SysHandleLogs sysHandleLogsModel = null;
            DbCommand cmd = this._db.GetSqlStringCommand(SqlSelectSysHandleLogs + " where a.ID = @ID ");
            this._db.AddInParameter(cmd, "ID", DbType.String, id);

            using (IDataReader rdr = Toolkit.DAL.DbHelper.ExecuteReader(cmd, this._db))
            {
                if (rdr.Read())
                {
                    sysHandleLogsModel = new Model.CompanyStructure.SysHandleLogs
                        {
                            ID = rdr.GetString(rdr.GetOrdinal("ID")),
                            OperatorId = rdr.GetInt32(rdr.GetOrdinal("OperatorId")),
                            DepatId = rdr.GetInt32(rdr.GetOrdinal("DepatId")),
                            CompanyId = rdr.GetInt32(rdr.GetOrdinal("CompanyId")),
                            ModuleId =
                                (Model.EnumType.PrivsStructure.Privs2)
                                rdr.GetInt32(rdr.GetOrdinal("ModuleId")),
                            EventCode = rdr.GetInt32(rdr.GetOrdinal("EventCode")),
                            EventMessage = rdr.GetString(rdr.GetOrdinal("EventMessage")),
                            EventTitle = rdr.GetString(rdr.GetOrdinal("EventTitle")),
                            EventTime = rdr.GetDateTime(rdr.GetOrdinal("EventTime")),
                            EventIp = rdr.GetString(rdr.GetOrdinal("EventIp")),
                            OperatorName =
                                rdr.IsDBNull(rdr.GetOrdinal("UserName")) ? "" : rdr.GetString(rdr.GetOrdinal("UserName")),
                            DepartName =
                                rdr.IsDBNull(rdr.GetOrdinal("DepartName"))
                                    ? ""
                                    : rdr.GetString(rdr.GetOrdinal("DepartName"))
                        };
                }
            }

            return sysHandleLogsModel;
        }

        /// <summary>
        /// 分页获取操作日志列表
        /// </summary>
        /// <param name="pageSize">每页现实条数</param>
        /// <param name="pageIndex">当前页码</param>
        /// <param name="recordCount">总记录数</param>
        /// <param name="model">系统操作日志查询实体</param>
        /// <returns></returns>
        public IList<Model.CompanyStructure.SysHandleLogs> GetList(int pageSize, int pageIndex, ref int recordCount,
            Model.CompanyStructure.QueryHandleLog model)
        {
            IList<Model.CompanyStructure.SysHandleLogs> totals = new List<Model.CompanyStructure.SysHandleLogs>();

            string tableName = SqlSelectSysHandleLogs;
            string orderByString = "EventTime DESC";
            string fields = " Id, OperatorId, DepatId, CompanyId, ModuleId,EventCode,EventMessage,EventTitle,EventTime,EventIp,UserName,DepartName";

            var cmdQuery = new StringBuilder();
            cmdQuery.AppendFormat(" CompanyId = {0}", model.CompanyId);
            if (model != null)
            {
                if (model.DepartId > 0)
                    cmdQuery.AppendFormat(" and DepatId={0}", model.DepartId);
                if (model.OperatorId > 0)
                    cmdQuery.AppendFormat(" and OperatorId={0}", model.OperatorId);
                if (model.HandStartTime != null)
                    cmdQuery.AppendFormat(" and EventTime >= '{0}'", model.HandStartTime);
                if (model.HandEndTime != null)
                    cmdQuery.AppendFormat(" and EventTime <= '{0}' ", model.HandEndTime);
                if (!string.IsNullOrEmpty(model.OperatorName))
                {
                    cmdQuery.AppendFormat(" and  UserName like '%{0}%' ", Toolkit.Utils.ToSqlLike(model.OperatorName));
                }
            }

            using (IDataReader rdr = Toolkit.DAL.DbHelper.ExecuteReader2(this._db, pageSize, pageIndex, ref recordCount, tableName
                , fields, cmdQuery.ToString(), orderByString, string.Empty))
            {
                while (rdr.Read())
                {
                    var sysHandleLogsInfo = new Model.CompanyStructure.SysHandleLogs
                        {
                            ID = rdr.GetString(rdr.GetOrdinal("ID")),
                            OperatorId = rdr.GetInt32(rdr.GetOrdinal("OperatorId")),
                            DepatId = rdr.GetInt32(rdr.GetOrdinal("DepatId")),
                            CompanyId = rdr.GetInt32(rdr.GetOrdinal("CompanyId")),
                            ModuleId =
                                (Model.EnumType.PrivsStructure.Privs2)
                                rdr.GetInt32(rdr.GetOrdinal("ModuleId")),
                            EventCode = rdr.GetInt32(rdr.GetOrdinal("EventCode")),
                            EventMessage = rdr.GetString(rdr.GetOrdinal("EventMessage")),
                            EventTitle = rdr.GetString(rdr.GetOrdinal("EventTitle")),
                            EventTime = rdr.GetDateTime(rdr.GetOrdinal("EventTime")),
                            EventIp = rdr.GetString(rdr.GetOrdinal("EventIp")),
                            OperatorName = rdr.GetString(rdr.GetOrdinal("UserName")),
                            DepartName = rdr.GetString(rdr.GetOrdinal("DepartName"))
                        };

                    totals.Add(sysHandleLogsInfo);
                }
            }

            return totals;
        }

        #endregion
    }
}
