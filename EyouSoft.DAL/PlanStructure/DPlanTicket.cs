using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.Common;
using Microsoft.Practices.EnterpriseLibrary.Data;
using System.Data;
using EyouSoft.Toolkit;
using EyouSoft.Toolkit.DAL;

namespace EyouSoft.DAL.PlanStructure
{
    public class DPlanTicket : EyouSoft.Toolkit.DAL.DALBase, EyouSoft.IDAL.PlanStructure.IPlanTicket
    {
        #region 初始化db
        private Database _db = null;

        /// <summary>
        /// 初始化_db
        /// </summary>
        public DPlanTicket()
        {
            _db = base.SystemStore;
        }
        #endregion

        #region IPlanTicket 成员

        /// <summary>
        /// 添加票务安排
        /// </summary>
        /// <param name="model"></param>
        /// <returns>
        /// 1:成功 0:失败 
        /// -1:供应商不存在
        /// </returns>
        public int Add(EyouSoft.Model.PlanStructure.MPlanTicketInfo model)
        {
            DbCommand cmd = this._db.GetStoredProcCommand("proc_PlanPiao_Add");
            this._db.AddInParameter(cmd, "PlanId", DbType.AnsiStringFixedLength, model.PlanId);
            this._db.AddInParameter(cmd, "CompanyId", DbType.Int32, model.CompanyId);
            this._db.AddInParameter(cmd, "TourId", DbType.AnsiStringFixedLength, model.TourId);
            this._db.AddInParameter(cmd, "TicketType", DbType.Byte, (int)model.TicketType);
            this._db.AddInParameter(cmd, "TicketMode", DbType.Byte, model.TicketMode);
            this._db.AddInParameter(cmd, "GysId", DbType.AnsiStringFixedLength, model.GysId);
            this._db.AddInParameter(cmd, "TicketerId", DbType.Int32, model.TicketerId);
            this._db.AddInParameter(cmd, "TrafficNumber", DbType.String, model.TrafficNumber);
            this._db.AddInParameter(cmd, "TrafficTime", DbType.DateTime, model.TrafficTime);
            this._db.AddInParameter(cmd, "Interval", DbType.String, model.Interval);
            this._db.AddInParameter(cmd, "Adults", DbType.Int32, model.Adults);
            this._db.AddInParameter(cmd, "Childs", DbType.Int32, model.Childs);
            this._db.AddInParameter(cmd, "AdultPrice", DbType.Currency, model.AdultPrice);
            this._db.AddInParameter(cmd, "ChildPrice", DbType.Currency, model.ChildPrice);
            this._db.AddInParameter(cmd, "OtherPrice", DbType.Currency, model.OtherPrice);
            this._db.AddInParameter(cmd, "Brokerage", DbType.Currency, model.Brokerage);
            this._db.AddInParameter(cmd, "TrafficSeat", DbType.String, model.TrafficSeat);
            this._db.AddInParameter(cmd, "_TrafficSeat", DbType.Byte, (int)model._TrafficSeat);
            this._db.AddInParameter(cmd, "PayType", DbType.Byte, (int)model.PayType);
            this._db.AddInParameter(cmd, "SumPrice", DbType.Currency, model.SumPrice);
            this._db.AddInParameter(cmd, "OperatorId", DbType.Int32, model.OperatorId);
            this._db.AddInParameter(cmd, "TicketStatus", DbType.Byte, (int)model.TicketStatus);
            this._db.AddInParameter(cmd, "IsMonth", DbType.AnsiStringFixedLength, model.IsMonth ? 1 : 0);
            this._db.AddInParameter(cmd, "MonthTime", DbType.DateTime, model.MonthTime);
            this._db.AddInParameter(cmd, "Traveller", DbType.Xml, CreateTravellerXml(model.TravellerList));
            this._db.AddInParameter(cmd, "File", DbType.Xml, CreateFileXml(model.FileList));

            this._db.AddOutParameter(cmd, "Result", DbType.Int32, 4);
            DbHelper.RunProcedureWithResult(cmd, _db);
            return Convert.ToInt32(_db.GetParameterValue(cmd, "Result"));

        }

        /// <summary>
        /// 修改票务安排
        /// </summary>
        /// <param name="model"></param>
        /// <returns>
        /// -1:只有出票、退票状态在申请中才能修改	
        /// -2:供应商不存在
        /// 1:成功 0：失败
        /// </returns>
        public int Update(EyouSoft.Model.PlanStructure.MPlanTicketInfo model)
        {
            DbCommand cmd = this._db.GetStoredProcCommand("proc_PlanPiao_Update");
            this._db.AddInParameter(cmd, "PlanId", DbType.AnsiStringFixedLength, model.PlanId);
            this._db.AddInParameter(cmd, "CompanyId", DbType.Int32, model.CompanyId);
            this._db.AddInParameter(cmd, "TourId", DbType.AnsiStringFixedLength, model.TourId);
            this._db.AddInParameter(cmd, "TicketType", DbType.Byte, (int)model.TicketType);
            this._db.AddInParameter(cmd, "TicketMode", DbType.Byte, model.TicketMode);
            this._db.AddInParameter(cmd, "GysId", DbType.AnsiStringFixedLength, model.GysId);
            this._db.AddInParameter(cmd, "TicketerId", DbType.Int32, model.TicketerId);
            this._db.AddInParameter(cmd, "TrafficNumber", DbType.String, model.TrafficNumber);
            this._db.AddInParameter(cmd, "TrafficTime", DbType.DateTime, model.TrafficTime);
            this._db.AddInParameter(cmd, "Interval", DbType.String, model.Interval);
            this._db.AddInParameter(cmd, "Adults", DbType.Int32, model.Adults);
            this._db.AddInParameter(cmd, "Childs", DbType.Int32, model.Childs);
            this._db.AddInParameter(cmd, "AdultPrice", DbType.Currency, model.AdultPrice);
            this._db.AddInParameter(cmd, "ChildPrice", DbType.Currency, model.ChildPrice);
            this._db.AddInParameter(cmd, "OtherPrice", DbType.Currency, model.OtherPrice);
            this._db.AddInParameter(cmd, "Brokerage", DbType.Currency, model.Brokerage);
            this._db.AddInParameter(cmd, "TrafficSeat", DbType.String, model.TrafficSeat);
            this._db.AddInParameter(cmd, "_TrafficSeat", DbType.Byte, (int)model._TrafficSeat);
            this._db.AddInParameter(cmd, "PayType", DbType.Byte, (int)model.PayType);
            this._db.AddInParameter(cmd, "SumPrice", DbType.Currency, model.SumPrice);
            this._db.AddInParameter(cmd, "IsMonth", DbType.AnsiStringFixedLength, model.IsMonth ? 1 : 0);
            this._db.AddInParameter(cmd, "MonthTime", DbType.DateTime, model.MonthTime);
            this._db.AddInParameter(cmd, "Traveller", DbType.Xml, CreateTravellerXml(model.TravellerList));
            this._db.AddInParameter(cmd, "File", DbType.Xml, CreateFileXml(model.FileList));

            this._db.AddOutParameter(cmd, "Result", DbType.Int32, 4);
            DbHelper.RunProcedureWithResult(cmd, _db);
            return Convert.ToInt32(_db.GetParameterValue(cmd, "Result"));
        }

        /// <summary>
        /// 修改票务安排
        /// </summary>
        /// <param name="model"></param>
        /// <returns>
        /// 1:成功 0：失败
        /// -1:票务安排已确认
        /// -2:财务已审核出票无法再确认
        /// -3:票务安排未确认无法审核出票
        /// -4:财务已审核无法再审核出票
        /// </returns>
        public int Update(string PlanId, EyouSoft.Model.EnumType.PlanStructure.TicketStatus TicketStatus)
        {
            DbCommand cmd = this._db.GetStoredProcCommand("proc_PlanPiao_Update_Status");
            this._db.AddInParameter(cmd, "PlanId", DbType.AnsiStringFixedLength, PlanId);
            this._db.AddInParameter(cmd, "TicketStatus", DbType.Byte, (int)TicketStatus);
            this._db.AddOutParameter(cmd, "Result", DbType.Int32, 4);
            DbHelper.RunProcedureWithResult(cmd, _db);
            return Convert.ToInt32(_db.GetParameterValue(cmd, "Result"));
        }


        /// <summary>
        /// 删除票务安排
        /// </summary>
        /// <param name="Id"></param>
        /// <returns>
        /// -1:只有处在申请中的出票、退票安排才能删除
        ///	-2:票务存在支出不能删除	
        ///	1:成功 0：失败
        /// </returns>
        public int Delete(string Id)
        {
            DbCommand cmd = this._db.GetStoredProcCommand("proc_PlanPiao_Delete");
            this._db.AddInParameter(cmd, "PlanId", DbType.AnsiStringFixedLength, Id);

            this._db.AddOutParameter(cmd, "Result", DbType.Int32, 4);
            DbHelper.RunProcedureWithResult(cmd, _db);
            return Convert.ToInt32(_db.GetParameterValue(cmd, "Result"));
        }

        /// <summary>
        /// 获取model
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        public EyouSoft.Model.PlanStructure.MPlanTicketInfo GetModel(string Id)
        {
            EyouSoft.Model.PlanStructure.MPlanTicketInfo model = null;
            StringBuilder query = new StringBuilder();
            query.Append("SELECT PlanId,CompanyId,TourId,TicketType,TicketMode,GysId,IsMonth,MonthTime");
            query.Append(",(select TourCode from tbl_Tour where TourId=tbl_PlanPiao.TourId) as TourCode ");
            query.Append(",(select UnitName from tbl_CompanySupplier where Id=tbl_PlanPiao.GysId) as GysName");
            query.Append(",(select ContactName from tbl_CompanyUser where Id=tbl_PlanPiao.TicketerId) as Ticketer");
            query.Append(",TicketerId,TrafficNumber,TrafficTime,Interval,Adults,Childs,AdultPrice");
            query.Append(",ChildPrice,OtherPrice,Brokerage,TrafficSeat,_TrafficSeat,PayType,SumPrice");
            query.Append(",OperatorId,IssueTime,TicketStatus,");
            query.Append("(select * from tbl_PlanPiaoTraveller where PlanId=tbl_PlanPiao.PlanId for xml raw,root('Root'))as Traveller,");
            query.Append("(select * from tbl_PlanPiaoFile where PlanId=tbl_PlanPiao.PlanId for xml raw,root('Root')) as FileInfo ");
            query.Append("  FROM tbl_PlanPiao where PlanId=@PlanId ");


            DbCommand cmd = this._db.GetSqlStringCommand(query.ToString());
            this._db.AddInParameter(cmd, "PlanId", DbType.AnsiStringFixedLength, Id);
            using (IDataReader dr = DbHelper.ExecuteReader(cmd, this._db))
            {

                while (dr.Read())
                {
                    model = new EyouSoft.Model.PlanStructure.MPlanTicketInfo();
                    model.PlanId = dr.GetString(dr.GetOrdinal("PlanId"));
                    model.CompanyId = dr.GetInt32(dr.GetOrdinal("CompanyId"));
                    model.TourId = dr.GetString(dr.GetOrdinal("TourId"));
                    model.TourCode = dr.GetString(dr.GetOrdinal("TourCode"));
                    model.TicketType = (EyouSoft.Model.EnumType.PlanStructure.TicketType?)dr.GetByte(dr.GetOrdinal("TicketType"));
                    model.GysId = dr.GetString(dr.GetOrdinal("GysId"));
                    model.GysName = !dr.IsDBNull(dr.GetOrdinal("GysName")) ? dr.GetString(dr.GetOrdinal("GysName")) : null;
                    model.TicketerId = dr.GetInt32(dr.GetOrdinal("TicketerId"));
                    model.Ticketer = !dr.IsDBNull(dr.GetOrdinal("Ticketer")) ? dr.GetString(dr.GetOrdinal("Ticketer")) : null;
                    model.TrafficNumber = !dr.IsDBNull(dr.GetOrdinal("TrafficNumber")) ? dr.GetString(dr.GetOrdinal("TrafficNumber")) : null;
                    //model.TrafficTime = !dr.IsDBNull(dr.GetOrdinal("TrafficTime")) ? dr.GetString(dr.GetOrdinal("TrafficTime")) : "";
                    model.TrafficTime = !dr.IsDBNull(dr.GetOrdinal("TrafficTime")) ? (DateTime?)dr.GetDateTime(dr.GetOrdinal("TrafficTime")) : null;
                    model.Interval = !dr.IsDBNull(dr.GetOrdinal("Interval")) ? dr.GetString(dr.GetOrdinal("Interval")) : null;
                    model.Adults = dr.GetInt32(dr.GetOrdinal("Adults"));
                    model.Childs = dr.GetInt32(dr.GetOrdinal("Childs"));
                    model.AdultPrice = dr.GetDecimal(dr.GetOrdinal("AdultPrice"));
                    model.ChildPrice = dr.GetDecimal(dr.GetOrdinal("ChildPrice"));
                    model.OtherPrice = dr.GetDecimal(dr.GetOrdinal("OtherPrice"));
                    model.Brokerage = dr.GetDecimal(dr.GetOrdinal("Brokerage"));
                    model.TicketMode = (EyouSoft.Model.EnumType.PlanStructure.TicketMode?)dr.GetByte(dr.GetOrdinal("TicketMode"));
                    model.TrafficSeat = !dr.IsDBNull(dr.GetOrdinal("TrafficSeat")) ? dr.GetString(dr.GetOrdinal("TrafficSeat")) : null;
                    model._TrafficSeat = (EyouSoft.Model.EnumType.PlanStructure.TrafficSeat?)dr.GetByte(dr.GetOrdinal("_TrafficSeat"));
                    model.PayType = (EyouSoft.Model.EnumType.FinStructure.ShouFuKuanFangShi?)dr.GetByte(dr.GetOrdinal("PayType"));
                    model.SumPrice = dr.GetDecimal(dr.GetOrdinal("SumPrice"));
                    model.OperatorId = dr.GetInt32(dr.GetOrdinal("OperatorId"));
                    model.IssueTime = (DateTime?)dr.GetDateTime(dr.GetOrdinal("IssueTime"));
                    model.TicketStatus = (EyouSoft.Model.EnumType.PlanStructure.TicketStatus?)dr.GetByte(dr.GetOrdinal("TicketStatus"));
                    model.TravellerList = new List<EyouSoft.Model.PlanStructure.MTraveller>();
                    model.TravellerList = !dr.IsDBNull(dr.GetOrdinal("Traveller")) ? GetTravellerByXml(dr.GetString(dr.GetOrdinal("Traveller"))) : null;
                    model.FileList = new List<EyouSoft.Model.TourStructure.MFile>();
                    model.FileList = !dr.IsDBNull(dr.GetOrdinal("FileInfo")) ? GetFileByXml(dr.GetString(dr.GetOrdinal("FileInfo"))) : null;


                    model.IsMonth = dr.GetString(dr.GetOrdinal("IsMonth")) == "1";
                    model.MonthTime = !dr.IsDBNull(dr.GetOrdinal("MonthTime")) ? (DateTime?)dr.GetDateTime(dr.GetOrdinal("MonthTime")) : null;
                }
            }
            return model;

        }

        /// <summary>
        /// 分页获取列表
        /// </summary>
        /// <param name="companyId"></param>
        /// <param name="pageSize"></param>
        /// <param name="pageIndex"></param>
        /// <param name="recordCount"></param>
        /// <param name="search"></param>
        /// <returns>Sum[0]:Adults,Sum[1]:Childs,Sum[2]:SumPrice</returns>
        public IList<EyouSoft.Model.PlanStructure.MPlanTicket> GetList(
            int companyId,
            int pageSize,
            int pageIndex,
            ref int recordCount,
            EyouSoft.Model.PlanStructure.MSearchTicket search, ref decimal[] Sum)
        {
            IList<EyouSoft.Model.PlanStructure.MPlanTicket> list = new List<EyouSoft.Model.PlanStructure.MPlanTicket>();

            StringBuilder fileds = new StringBuilder();
            fileds.Append(" PlanId,CompanyId,TourId,TicketType,TicketMode");
            fileds.Append(",GysId,IsMonth,MonthTime,TourCode,IsEnd,LDate");
            fileds.Append(",TourType,TourStatus,GysName,Ticketer,TicketerId");
            fileds.Append(",TrafficNumber,TrafficTime,Interval,Adults");
            fileds.Append(",Childs,AdultPrice,ChildPrice,OtherPrice,Brokerage");
            fileds.Append(",TrafficSeat,_TrafficSeat,PayType,SumPrice,OperatorId,IssueTime,TicketStatus");

            string tableName = "view_Piao";

            string orderByString = " TourStatus asc,IssueTime desc ";



            string sumFiled = "Sum(Adults) as SumAdults,Sum(Childs) as SumChilds,Sum(SumPrice) as TotalSumPrice";

            StringBuilder query = new StringBuilder();

            query.AppendFormat("CompanyId={0}", companyId);

            if (search != null)
            {

                if (string.IsNullOrEmpty(search.TourCode)
                    && search.LEndDate.HasValue == false
                    && search.LBeginDate.HasValue == false
                    && search.TicketType.HasValue == false
                    && search.TicketStatus.HasValue == false)
                {
                    //财务操作结束后，该团就自动隐藏，用搜索可查询到
                    query.Append("  and IsEnd='0' ");
                }

                if (!string.IsNullOrEmpty(search.TourCode))
                {

                    query.AppendFormat(" and TourCode like '%{0}%' ", search.TourCode);
                }

                if (search.LBeginDate.HasValue)
                {
                    query.AppendFormat(" and  datediff(day,LDate,'{0}')<=0 ", search.LBeginDate.Value);
                }

                if (search.LEndDate.HasValue)
                {
                    query.AppendFormat(" and  datediff(day,LDate,'{0}')>=0  ", search.LEndDate.Value);
                }

                if (search.TicketType.HasValue)
                {
                    query.AppendFormat(" and TicketType={0} ", (int)search.TicketType.Value);
                }

                if (search.TicketStatus.HasValue)
                {
                    query.AppendFormat(" and TicketStatus={0} ", (int)search.TicketStatus.Value);
                }

                if (!string.IsNullOrEmpty(search.GysId))
                {
                    query.AppendFormat(" and GysId='{0}' ", search.GysId);
                }


                if (search.IsMonth.HasValue)
                {
                    query.AppendFormat(" and IsMonth='{0}' ", search.IsMonth.Value ? 1 : 0);
                }

                if (search.TicketMode.HasValue)
                {

                    query.AppendFormat(" and TicketMode={0} ", (int)search.TicketMode.Value);
                }

                if (!string.IsNullOrEmpty(search.GysName))
                {
                    query.AppendFormat(" and exists(select 1 from tbl_CompanySupplier where Id=view_Piao.GysId and UnitName like '%{0}%')", search.GysName);

                }

                if (search.TourType.HasValue)
                {
                    query.AppendFormat(" and  TourType={0} ", (int)search.TourType.Value);
                }

            }
            else
            {
                //财务操作结束后，该团就自动隐藏，用搜索可查询到
                query.Append(" and  IsEnd='0' ");
            }

            using (IDataReader dr = DbHelper.ExecuteReader1(this._db, pageSize, pageIndex, ref recordCount, tableName, fileds.ToString(), query.ToString(), orderByString, sumFiled))
            {
                while (dr.Read())
                {

                    EyouSoft.Model.PlanStructure.MPlanTicket model = new EyouSoft.Model.PlanStructure.MPlanTicket();
                    model.PlanId = dr.GetString(dr.GetOrdinal("PlanId"));
                    model.CompanyId = dr.GetInt32(dr.GetOrdinal("CompanyId"));
                    model.TourId = dr.GetString(dr.GetOrdinal("TourId"));
                    model.TourCode = dr.GetString(dr.GetOrdinal("TourCode"));
                    model.TicketType = (EyouSoft.Model.EnumType.PlanStructure.TicketType?)dr.GetByte(dr.GetOrdinal("TicketType"));
                    model.GysId = dr.GetString(dr.GetOrdinal("GysId"));
                    model.GysName = !dr.IsDBNull(dr.GetOrdinal("GysName")) ? dr.GetString(dr.GetOrdinal("GysName")) : null;
                    model.TicketerId = dr.GetInt32(dr.GetOrdinal("TicketerId"));
                    model.Ticketer = !dr.IsDBNull(dr.GetOrdinal("Ticketer")) ? dr.GetString(dr.GetOrdinal("Ticketer")) : null;
                    model.TrafficNumber = !dr.IsDBNull(dr.GetOrdinal("TrafficNumber")) ? dr.GetString(dr.GetOrdinal("TrafficNumber")) : null;
                    model.Interval = !dr.IsDBNull(dr.GetOrdinal("Interval")) ? dr.GetString(dr.GetOrdinal("Interval")) : null;
                    model.Adults = dr.GetInt32(dr.GetOrdinal("Adults"));
                    model.Childs = dr.GetInt32(dr.GetOrdinal("Childs"));
                    model.AdultPrice = dr.GetDecimal(dr.GetOrdinal("AdultPrice"));
                    model.ChildPrice = dr.GetDecimal(dr.GetOrdinal("ChildPrice"));
                    model.OtherPrice = dr.GetDecimal(dr.GetOrdinal("OtherPrice"));
                    model.TicketMode = (EyouSoft.Model.EnumType.PlanStructure.TicketMode?)dr.GetByte(dr.GetOrdinal("TicketMode"));
                    model.Brokerage = dr.GetDecimal(dr.GetOrdinal("Brokerage"));
                    model.TrafficSeat = !dr.IsDBNull(dr.GetOrdinal("TrafficSeat")) ? dr.GetString(dr.GetOrdinal("TrafficSeat")) : null;
                    model._TrafficSeat = (EyouSoft.Model.EnumType.PlanStructure.TrafficSeat?)dr.GetByte(dr.GetOrdinal("_TrafficSeat"));
                    model.PayType = (EyouSoft.Model.EnumType.FinStructure.ShouFuKuanFangShi?)dr.GetByte(dr.GetOrdinal("PayType"));
                    model.SumPrice = dr.GetDecimal(dr.GetOrdinal("SumPrice"));
                    model.OperatorId = dr.GetInt32(dr.GetOrdinal("OperatorId"));
                    model.IssueTime = (DateTime?)dr.GetDateTime(dr.GetOrdinal("IssueTime"));
                    model.TrafficTime = !dr.IsDBNull(dr.GetOrdinal("TrafficTime")) ? (DateTime?)dr.GetDateTime(dr.GetOrdinal("TrafficTime")) : null;
                    //model.TrafficTime = (DateTime?)dr.GetDateTime(dr.GetOrdinal("TrafficTime"));
                    model.TicketStatus = (EyouSoft.Model.EnumType.PlanStructure.TicketStatus?)dr.GetByte(dr.GetOrdinal("TicketStatus"));
                    model.IsMonth = dr.GetString(dr.GetOrdinal("IsMonth")) == "1";
                    model.MonthTime = !dr.IsDBNull(dr.GetOrdinal("MonthTime")) ? (DateTime?)dr.GetDateTime(dr.GetOrdinal("MonthTime")) : null;
                    model.TourStatus = (EyouSoft.Model.EnumType.TourStructure.TourStatus?)dr.GetByte(dr.GetOrdinal("TourStatus"));

                    list.Add(model);
                }


                if (dr.NextResult())
                {
                    if (dr.Read())
                    {
                        Sum[0] = !dr.IsDBNull(dr.GetOrdinal("SumAdults")) ? dr.GetInt32(dr.GetOrdinal("SumAdults")) : 0;
                        Sum[1] = !dr.IsDBNull(dr.GetOrdinal("SumChilds")) ? dr.GetInt32(dr.GetOrdinal("SumChilds")) : 0;
                        Sum[2] = !dr.IsDBNull(dr.GetOrdinal("TotalSumPrice")) ? dr.GetDecimal(dr.GetOrdinal("TotalSumPrice")) : 0;
                    }
                }
            }
            return list;
        }

        #endregion


        #region



        /// <summary>
        /// 将订单游客的集合转换为xml
        /// </summary>
        /// <param name="list"></param>
        /// <returns></returns>
        private string CreateTravellerXml(IList<EyouSoft.Model.PlanStructure.MTraveller> list)
        {
            if (list == null || list.Count == 0) return null;
            StringBuilder xmlDoc = new StringBuilder();
            xmlDoc.Append("<Root>");
            foreach (EyouSoft.Model.PlanStructure.MTraveller model in list)
            {
                xmlDoc.Append("<Traveller ");
                xmlDoc.AppendFormat("TravellerId=\"{0}\" ", !string.IsNullOrEmpty(model.TravellerId) ? model.TravellerId : Guid.NewGuid().ToString());
                xmlDoc.AppendFormat("TravellerName=\"{0}\" ", Utils.ReplaceXmlSpecialCharacter(model.TravellerName));
                xmlDoc.AppendFormat("TravellerType=\"{0}\" ", (int)model.TravellerType);
                xmlDoc.AppendFormat("CardType=\"{0}\" ", (int)model.CardType);
                xmlDoc.AppendFormat("CardNumber=\"{0}\" ", Utils.ReplaceXmlSpecialCharacter(model.CardNumber));
                if (model.Brithday.HasValue)
                {
                    xmlDoc.AppendFormat("Birthday=\"{0}\" ", model.Brithday.Value);
                }

                xmlDoc.AppendFormat("Gender=\"{0}\" ", (int)model.Sex);
                xmlDoc.AppendFormat("Contact=\"{0}\" ", model.Contact);
                xmlDoc.Append(" />");
            }
            xmlDoc.Append("</Root>");
            return xmlDoc.ToString();
        }

        /// <summary>
        /// 将订单游客的xml转换为集合
        /// </summary>
        /// <param name="xml"></param>
        /// <returns></returns>
        public IList<EyouSoft.Model.PlanStructure.MTraveller> GetTravellerByXml(string xml)
        {
            if (string.IsNullOrEmpty(xml)) return null;
            IList<EyouSoft.Model.PlanStructure.MTraveller> list = new List<EyouSoft.Model.PlanStructure.MTraveller>();
            System.Xml.Linq.XElement xRoot = System.Xml.Linq.XElement.Parse(xml);
            var xRows = Utils.GetXElements(xRoot, "row");
            foreach (var xRow in xRows)
            {
                EyouSoft.Model.PlanStructure.MTraveller item = new EyouSoft.Model.PlanStructure.MTraveller()
                {
                    TravellerId = Utils.GetXAttributeValue(xRow, "TravellerId"),
                    TourId = Utils.GetXAttributeValue(xRow, "TourId"),
                    TravellerName = Utils.GetXAttributeValue(xRow, "TravellerName"),
                    TravellerType = (EyouSoft.Model.EnumType.TourStructure.TravellerType)Utils.GetInt(Utils.GetXAttributeValue(xRow, "TravellerType")),
                    CardType = (EyouSoft.Model.EnumType.TourStructure.CardType)Utils.GetInt(Utils.GetXAttributeValue(xRow, "CardType")),
                    CardNumber = Utils.GetXAttributeValue(xRow, "CardNumber"),
                    Brithday = Utils.GetDateTimeNullable(Utils.GetXAttributeValue(xRow, "Birthday")),
                    Sex = (EyouSoft.Model.EnumType.CompanyStructure.Sex)Utils.GetInt(Utils.GetXAttributeValue(xRow, "Gender")),
                    Contact = Utils.GetXAttributeValue(xRow, "Contact")
                };
                list.Add(item);
            }
            return list;
        }

        /// <summary>
        /// 获取附件信息
        /// </summary>
        /// <param name="list"></param>
        /// <param name="TourId"></param>
        /// <returns></returns>
        private string CreateFileXml(IList<EyouSoft.Model.TourStructure.MFile> list)
        {
            if (list == null) return null;
            if (list.Count == 0) return null;
            StringBuilder xmlDoc = new StringBuilder();
            xmlDoc.Append("<Root>");
            foreach (EyouSoft.Model.TourStructure.MFile model in list)
            {
                xmlDoc.Append("<File ");
                xmlDoc.AppendFormat("FileName=\"{0}\" ", model.FileName);
                xmlDoc.AppendFormat("FilePath=\"{0}\" ", model.FilePath);
                xmlDoc.Append(" />");
            }
            xmlDoc.Append("</Root>");
            return xmlDoc.ToString();
        }


        /// <summary>
        /// 根据xml获取文件
        /// </summary>
        /// <param name="xml"></param>
        /// <returns></returns>
        private IList<EyouSoft.Model.TourStructure.MFile> GetFileByXml(string xml)
        {
            if (string.IsNullOrEmpty(xml)) return null;
            IList<EyouSoft.Model.TourStructure.MFile> list = new List<EyouSoft.Model.TourStructure.MFile>();
            System.Xml.Linq.XElement xRoot = System.Xml.Linq.XElement.Parse(xml);
            var xRows = Utils.GetXElements(xRoot, "row");
            foreach (var xRow in xRows)
            {
                EyouSoft.Model.TourStructure.MFile model = new EyouSoft.Model.TourStructure.MFile();
                model.Id = Utils.GetXAttributeValue(xRow, "Id");
                model.FileName = Utils.GetXAttributeValue(xRow, "FileName");
                model.FilePath = Utils.GetXAttributeValue(xRow, "FilePath");
                list.Add(model);
            }
            return list;
        }


        #endregion
    }
}
