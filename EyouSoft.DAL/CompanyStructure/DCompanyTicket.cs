using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using EyouSoft.Toolkit.DAL;
using Microsoft.Practices.EnterpriseLibrary.Data;
using System.Data.Common;
using System.Data;

namespace EyouSoft.DAL.CompanyStructure
{
    public class DCompanyTicket : DALBase, EyouSoft.IDAL.CompanyStructure.ICompanyTicket
    {

        #region 构造函数

        private readonly Database _db;

        public DCompanyTicket()
        {
            this._db = SystemStore;
        }

        #endregion


        #region ICompanyTicket 成员

        /// <summary>
        /// 添加
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public int Add(EyouSoft.Model.CompanyStructure.MCompanyTicket model)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("insert into tbl_CompanyTicket(");
            strSql.Append("CompanyId,TicketType,TrafficNumber,TrafficTime,Interval,OtherPrice,Brokerage,TrafficSeat,_TrafficSeat,OperatorId");
            strSql.Append(") values (");
            strSql.Append("@CompanyId,@TicketType,@TrafficNumber,@TrafficTime,@Interval,@OtherPrice,@Brokerage,@TrafficSeat,@_TrafficSeat,@OperatorId");
            strSql.Append(") ");
            strSql.Append(";select @@IDENTITY");

            DbCommand cmd = this._db.GetSqlStringCommand(strSql.ToString());


            this._db.AddInParameter(cmd, "CompanyId", DbType.Int32, model.CompanyId);
            this._db.AddInParameter(cmd, "TicketType", DbType.Byte, (int)model.TicketType.Value);
            this._db.AddInParameter(cmd, "TrafficNumber", DbType.String, model.TrafficNumber);
            this._db.AddInParameter(cmd, "TrafficTime", DbType.DateTime, model.TrafficTime);
            this._db.AddInParameter(cmd, "Interval", DbType.String, model.Interval);
            this._db.AddInParameter(cmd, "OtherPrice", DbType.Currency, model.OtherPrice);
            this._db.AddInParameter(cmd, "Brokerage", DbType.Currency, model.Brokerage);
            this._db.AddInParameter(cmd, "TrafficSeat", DbType.String, model.TrafficSeat);
            this._db.AddInParameter(cmd, "_TrafficSeat", DbType.Byte, (int)model._TrafficSeat);
            this._db.AddInParameter(cmd, "OperatorId", DbType.Int32, model.OperatorId);

            model.Id = Convert.ToInt32(DbHelper.GetSingle(cmd, this._db));

            return model.Id;

        }

        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public bool Update(EyouSoft.Model.CompanyStructure.MCompanyTicket model)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update tbl_CompanyTicket set ");
            strSql.Append(" TicketType = @TicketType , ");
            strSql.Append(" TrafficNumber = @TrafficNumber , ");
            strSql.Append(" TrafficTime = @TrafficTime , ");
            strSql.Append(" Interval = @Interval , ");
            strSql.Append(" OtherPrice = @OtherPrice , ");
            strSql.Append(" Brokerage = @Brokerage , ");
            strSql.Append(" TrafficSeat = @TrafficSeat,  ");
            strSql.Append(" _TrafficSeat = @_TrafficSeat  ");
            strSql.Append(" where Id=@Id ");

            DbCommand cmd = this._db.GetSqlStringCommand(strSql.ToString());

            this._db.AddInParameter(cmd, "Id", DbType.Int32, model.Id);
            this._db.AddInParameter(cmd, "CompanyId", DbType.Int32, model.CompanyId);
            this._db.AddInParameter(cmd, "TicketType", DbType.Byte, (int)model.TicketType.Value);
            this._db.AddInParameter(cmd, "TrafficNumber", DbType.String, model.TrafficNumber);
            this._db.AddInParameter(cmd, "TrafficTime", DbType.DateTime, model.TrafficTime);
            this._db.AddInParameter(cmd, "Interval", DbType.String, model.Interval);
            this._db.AddInParameter(cmd, "OtherPrice", DbType.Currency, model.OtherPrice);
            this._db.AddInParameter(cmd, "Brokerage", DbType.Currency, model.Brokerage);
            this._db.AddInParameter(cmd, "TrafficSeat", DbType.String, model.TrafficSeat);
            this._db.AddInParameter(cmd, "_TrafficSeat", DbType.Byte, (int)model._TrafficSeat);
            this._db.AddInParameter(cmd, "OperatorId", DbType.Int32, model.OperatorId);
            this._db.AddInParameter(cmd, "IssueTime", DbType.DateTime, DateTime.Now);

            return Convert.ToInt32(DbHelper.ExecuteSql(cmd, this._db)) > 0 ? true : false;

        }

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        public bool Delete(int Id)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("delete from tbl_CompanyTicket ");
            strSql.Append(" where Id=@Id");
            DbCommand cmd = this._db.GetSqlStringCommand(strSql.ToString());

            this._db.AddInParameter(cmd, "Id", DbType.Int32, Id);
            return Convert.ToInt32(DbHelper.ExecuteSql(cmd, this._db)) > 0 ? true : false;
        }

        /// <summary>
        /// 获取model
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        public EyouSoft.Model.CompanyStructure.MCompanyTicket GetModel(int Id)
        {
            EyouSoft.Model.CompanyStructure.MCompanyTicket model = null;

            StringBuilder strSql = new StringBuilder();
            strSql.Append("select Id, CompanyId, TicketType, TrafficNumber, TrafficTime, Interval, OtherPrice, Brokerage, TrafficSeat,_TrafficSeat,OperatorId,IssueTime,  ");
            strSql.Append(" (select ContactName from tbl_CompanyUser where Id=tbl_CompanyTicket.OperatorId) as OperatorName ");
            strSql.Append("  from tbl_CompanyTicket ");
            strSql.Append(" where Id=@Id");

            DbCommand cmd = this._db.GetSqlStringCommand(strSql.ToString());

            this._db.AddInParameter(cmd, "Id", DbType.Int32, Id);

            using (IDataReader dr = DbHelper.ExecuteReader(cmd, this._db))
            {
                while (dr.Read())
                {
                    model = new EyouSoft.Model.CompanyStructure.MCompanyTicket();
                    model.Id = dr.GetInt32(dr.GetOrdinal("Id"));
                    model.CompanyId = dr.GetInt32(dr.GetOrdinal("CompanyId"));
                    model.TicketType = (EyouSoft.Model.EnumType.PlanStructure.TicketType)dr.GetByte(dr.GetOrdinal("TicketType"));
                    model.TrafficNumber = !dr.IsDBNull(dr.GetOrdinal("TrafficNumber")) ? dr.GetString(dr.GetOrdinal("TrafficNumber")) : null;
                    //model.TrafficTime = !dr.IsDBNull(dr.GetOrdinal("TrafficTime")) ? dr.GetString(dr.GetOrdinal("TrafficTime")) : null;
                    model.TrafficTime = !dr.IsDBNull(dr.GetOrdinal("TrafficTime")) ? (DateTime?)dr.GetDateTime(dr.GetOrdinal("TrafficTime")) : null;
                    model.Interval = !dr.IsDBNull(dr.GetOrdinal("Interval")) ? dr.GetString(dr.GetOrdinal("Interval")) : null;
                    model.OtherPrice = dr.GetDecimal(dr.GetOrdinal("OtherPrice"));
                    model.Brokerage = dr.GetDecimal(dr.GetOrdinal("Brokerage"));
                    model.TrafficSeat = !dr.IsDBNull(dr.GetOrdinal("TrafficSeat")) ? dr.GetString(dr.GetOrdinal("TrafficSeat")) : null;
                    model._TrafficSeat = (EyouSoft.Model.EnumType.PlanStructure.TrafficSeat?)dr.GetByte(dr.GetOrdinal("_TrafficSeat"));
                    model.OperatorId = dr.GetInt32(dr.GetOrdinal("OperatorId"));
                    model.IssueTime = dr.GetDateTime(dr.GetOrdinal("IssueTime"));
                    model.OperatorName = !dr.IsDBNull(dr.GetOrdinal("OperatorName")) ? dr.GetString(dr.GetOrdinal("OperatorName")) : null;
                }
            }
            return model;

        }

        /// <summary>
        /// 查询
        /// </summary>
        /// <param name="search"></param>
        /// <returns></returns>
        public IList<EyouSoft.Model.CompanyStructure.MCompanyTicket> GetList(int top, int CompanyId, EyouSoft.Model.CompanyStructure.MCompanyTicketSearch search)
        {
            IList<EyouSoft.Model.CompanyStructure.MCompanyTicket> list = new List<EyouSoft.Model.CompanyStructure.MCompanyTicket>();

            StringBuilder strSql = new StringBuilder();
            strSql.Append("select ");
            if (top > 0)
            {
                strSql.Append(" top " + top.ToString());
            }
            strSql.Append(" Id, CompanyId, TicketType, TrafficNumber, TrafficTime, Interval, OtherPrice, Brokerage, TrafficSeat,_TrafficSeat,OperatorId,IssueTime,  ");
            strSql.Append(" (select ContactName from tbl_CompanyUser where Id=tbl_CompanyTicket.OperatorId) as OperatorName ");
            strSql.Append("  from tbl_CompanyTicket ");

            strSql.AppendFormat(" where CompanyId={0} ", CompanyId);
            if (search != null)
            {
                if (!string.IsNullOrEmpty(search.TrafficNumber))
                {
                    strSql.AppendFormat(" and TrafficNumber like '%{0}%'", search.TrafficNumber);
                }

                if (search.TicketType.HasValue)
                {
                    strSql.AppendFormat(" and TicketType ={0} ", (int)search.TicketType.Value);
                }

                if (!string.IsNullOrEmpty(search.OperatorName))
                {

                    strSql.AppendFormat(" and exists(select 1 from tbl_CompanyUser where Id=tbl_CompanyTicket.OperatorId and ContactName like '%{0}%' ) ", search.OperatorName);
                }
            }
            DbCommand cmd = this._db.GetSqlStringCommand(strSql.ToString());
            using (IDataReader dr = DbHelper.ExecuteReader(cmd, this._db))
            {
                while (dr.Read())
                {
                    EyouSoft.Model.CompanyStructure.MCompanyTicket model = new EyouSoft.Model.CompanyStructure.MCompanyTicket();
                    model.Id = dr.GetInt32(dr.GetOrdinal("Id"));
                    model.CompanyId = dr.GetInt32(dr.GetOrdinal("CompanyId"));
                    model.TicketType = (EyouSoft.Model.EnumType.PlanStructure.TicketType)dr.GetByte(dr.GetOrdinal("TicketType"));
                    model.TrafficNumber = !dr.IsDBNull(dr.GetOrdinal("TrafficNumber")) ? dr.GetString(dr.GetOrdinal("TrafficNumber")) : null;
                    //model.TrafficTime = !dr.IsDBNull(dr.GetOrdinal("TrafficTime")) ? dr.GetString(dr.GetOrdinal("TrafficTime")) : null;
                    model.TrafficTime = !dr.IsDBNull(dr.GetOrdinal("TrafficTime")) ? (DateTime?)dr.GetDateTime(dr.GetOrdinal("TrafficTime")) : null;
                    model.Interval = !dr.IsDBNull(dr.GetOrdinal("Interval")) ? dr.GetString(dr.GetOrdinal("Interval")) : null;
                    model.OtherPrice = dr.GetDecimal(dr.GetOrdinal("OtherPrice"));
                    model.Brokerage = dr.GetDecimal(dr.GetOrdinal("Brokerage"));
                    model.TrafficSeat = !dr.IsDBNull(dr.GetOrdinal("TrafficSeat")) ? dr.GetString(dr.GetOrdinal("TrafficSeat")) : null;
                    model._TrafficSeat = (EyouSoft.Model.EnumType.PlanStructure.TrafficSeat?)dr.GetByte(dr.GetOrdinal("_TrafficSeat"));
                    model.OperatorId = dr.GetInt32(dr.GetOrdinal("OperatorId"));
                    model.IssueTime = dr.GetDateTime(dr.GetOrdinal("IssueTime"));
                    model.OperatorName = !dr.IsDBNull(dr.GetOrdinal("OperatorName")) ? dr.GetString(dr.GetOrdinal("OperatorName")) : null;

                    list.Add(model);
                }
            }
            return list;
        }


        /// <summary>
        /// 查询分页
        /// </summary>
        /// <param name="search"></param>
        /// <returns></returns>
        public IList<EyouSoft.Model.CompanyStructure.MCompanyTicket> GetList(int CompanyId, int PageSize, int PageIndex, ref int RecordCount, EyouSoft.Model.CompanyStructure.MCompanyTicketSearch search)
        {
            IList<EyouSoft.Model.CompanyStructure.MCompanyTicket> list = new List<EyouSoft.Model.CompanyStructure.MCompanyTicket>();

            string tableName = "tbl_CompanyTicket";
            string orderByString = " IssueTime desc ";

            StringBuilder fields = new StringBuilder();
            fields.Append(" Id, CompanyId, TicketType, TrafficNumber, TrafficTime, Interval, OtherPrice, Brokerage, TrafficSeat,_TrafficSeat,OperatorId,IssueTime,  ");
            fields.Append(" (select ContactName from tbl_CompanyUser where Id=tbl_CompanyTicket.OperatorId) as OperatorName ");

            StringBuilder query = new StringBuilder();
            query.AppendFormat(" CompanyId={0} ", CompanyId);

            if (search != null)
            {
                if (!string.IsNullOrEmpty(search.TrafficNumber))
                {
                    query.AppendFormat(" and TrafficNumber like '%{0}%'", search.TrafficNumber);
                }

                if (search.TicketType.HasValue)
                {
                    query.AppendFormat(" and TicketType ={0} ", (int)search.TicketType.Value);
                }

                if (!string.IsNullOrEmpty(search.OperatorName))
                {
                    query.AppendFormat(" and exists(select 1 from tbl_CompanyUser where Id=tbl_CompanyTicket.OperatorId and ContactName like '%{0}%' ) ", search.OperatorName);
                }
            }

            using (IDataReader dr = DbHelper.ExecuteReader1(this._db, PageSize, PageIndex, ref RecordCount, tableName, fields.ToString(), query.ToString(), orderByString, null))
            {
                while (dr.Read())
                {
                    EyouSoft.Model.CompanyStructure.MCompanyTicket model = new EyouSoft.Model.CompanyStructure.MCompanyTicket();
                    model.Id = dr.GetInt32(dr.GetOrdinal("Id"));
                    model.CompanyId = dr.GetInt32(dr.GetOrdinal("CompanyId"));
                    model.TicketType = (EyouSoft.Model.EnumType.PlanStructure.TicketType)dr.GetByte(dr.GetOrdinal("TicketType"));
                    model.TrafficNumber = !dr.IsDBNull(dr.GetOrdinal("TrafficNumber")) ? dr.GetString(dr.GetOrdinal("TrafficNumber")) : null;
                    //model.TrafficTime = !dr.IsDBNull(dr.GetOrdinal("TrafficTime")) ? dr.GetString(dr.GetOrdinal("TrafficTime")) : null;
                    model.TrafficTime = !dr.IsDBNull(dr.GetOrdinal("TrafficTime")) ? (DateTime?)dr.GetDateTime(dr.GetOrdinal("TrafficTime")) : null;
                    model.Interval = !dr.IsDBNull(dr.GetOrdinal("Interval")) ? dr.GetString(dr.GetOrdinal("Interval")) : null;
                    model.OtherPrice = dr.GetDecimal(dr.GetOrdinal("OtherPrice"));
                    model.Brokerage = dr.GetDecimal(dr.GetOrdinal("Brokerage"));
                    model.TrafficSeat = !dr.IsDBNull(dr.GetOrdinal("TrafficSeat")) ? dr.GetString(dr.GetOrdinal("TrafficSeat")) : null;
                    model._TrafficSeat = (EyouSoft.Model.EnumType.PlanStructure.TrafficSeat?)dr.GetByte(dr.GetOrdinal("_TrafficSeat"));
                    model.OperatorId = dr.GetInt32(dr.GetOrdinal("OperatorId"));
                    model.IssueTime = dr.GetDateTime(dr.GetOrdinal("IssueTime"));
                    model.OperatorName = !dr.IsDBNull(dr.GetOrdinal("OperatorName")) ? dr.GetString(dr.GetOrdinal("OperatorName")) : null;

                    list.Add(model);
                }
            }

            return list;
        }

        #endregion
    }
}
