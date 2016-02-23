using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Practices.EnterpriseLibrary.Data;
using System.Data.Common;
using EyouSoft.Toolkit.DAL;
using System.Data;
using EyouSoft.Toolkit;

namespace EyouSoft.DAL.PlanStructure
{
    public class DPlanDiJie : EyouSoft.Toolkit.DAL.DALBase, EyouSoft.IDAL.PlanStructure.IPlanDiJie
    {

        #region 初始化db
        private Database _db = null;

        /// <summary>
        /// 初始化_db
        /// </summary>
        public DPlanDiJie()
        {
            _db = base.SystemStore;
        }
        #endregion

        #region IPlanDiJie 成员

        /// <summary>
        /// 添加地接
        /// </summary>
        /// <param name="model"></param>
        /// <returns>1:成功 0：失败
        /// -1:供应商不存在
        /// </returns>
        public int Add(EyouSoft.Model.PlanStructure.MPlanDiJieInfo model)
        {
            DbCommand cmd = this._db.GetStoredProcCommand("proc_Dijie_Add");
            this._db.AddInParameter(cmd, "PlanId", DbType.AnsiStringFixedLength, model.PlanId);
            this._db.AddInParameter(cmd, "CompanyId", DbType.Int32, model.CompanyId);
            this._db.AddInParameter(cmd, "TourId", DbType.AnsiStringFixedLength, model.TourId);
            this._db.AddInParameter(cmd, "GysId", DbType.AnsiStringFixedLength, model.GysId);
            this._db.AddInParameter(cmd, "Hotel", DbType.Currency, model.Hotel);
            this._db.AddInParameter(cmd, "Dining", DbType.Currency, model.Dining);
            this._db.AddInParameter(cmd, "Car", DbType.Currency, model.Car);
            this._db.AddInParameter(cmd, "Ticket", DbType.Currency, model.Ticket);
            this._db.AddInParameter(cmd, "Guide", DbType.Currency, model.Guide);
            this._db.AddInParameter(cmd, "Traffic", DbType.Currency, model.Traffic);
            this._db.AddInParameter(cmd, "Head", DbType.Currency, model.Head);
            this._db.AddInParameter(cmd, "AddPrice", DbType.Currency, model.AddPrice);
            this._db.AddInParameter(cmd, "GuideIncome", DbType.Currency, model.GuideIncome);
            this._db.AddInParameter(cmd, "GuidePay", DbType.Currency, model.GuidePay);
            this._db.AddInParameter(cmd, "Other", DbType.Currency, model.Other);
            this._db.AddInParameter(cmd, "PayType", DbType.Byte, (int)model.PayType.Value);
            this._db.AddInParameter(cmd, "SumPrice", DbType.Currency, model.SumPrice);
            this._db.AddInParameter(cmd, "Remark", DbType.String, model.Remark);
            this._db.AddInParameter(cmd, "OperatorId", DbType.Int32, model.OperatorId);
            this._db.AddInParameter(cmd, "DiJieStatus", DbType.Byte, (int)model.DiJieStatus.Value);
            this._db.AddInParameter(cmd, "IsMonth", DbType.AnsiStringFixedLength, model.IsMonth ? 1 : 0);
            this._db.AddInParameter(cmd, "MonthTime", DbType.DateTime, model.MonthTime);
            this._db.AddInParameter(cmd, "File", DbType.Xml, CreateFileXml(model.FileList));

            this._db.AddOutParameter(cmd, "Result", DbType.Int32, 4);
            DbHelper.RunProcedureWithResult(cmd, _db);
            return Convert.ToInt32(_db.GetParameterValue(cmd, "Result"));
        }

        /// <summary>
        /// 修改地接安排
        /// </summary>
        /// <param name="model"></param>
        /// <returns>-1:只有地接状态在申请中才能修改	]
        /// -2:供应商不存在
        /// 1:成功 0：失败</returns>
        public int Update(EyouSoft.Model.PlanStructure.MPlanDiJieInfo model)
        {
            DbCommand cmd = this._db.GetStoredProcCommand("proc_Dijie_Update");
            this._db.AddInParameter(cmd, "PlanId", DbType.AnsiStringFixedLength, model.PlanId);
            this._db.AddInParameter(cmd, "CompanyId", DbType.Int32, model.CompanyId);
            this._db.AddInParameter(cmd, "TourId", DbType.AnsiStringFixedLength, model.TourId);
            this._db.AddInParameter(cmd, "GysId", DbType.AnsiStringFixedLength, model.GysId);
            this._db.AddInParameter(cmd, "Hotel", DbType.Currency, model.Hotel);
            this._db.AddInParameter(cmd, "Dining", DbType.Currency, model.Dining);
            this._db.AddInParameter(cmd, "Car", DbType.Currency, model.Car);
            this._db.AddInParameter(cmd, "Ticket", DbType.Currency, model.Ticket);
            this._db.AddInParameter(cmd, "Guide", DbType.Currency, model.Guide);
            this._db.AddInParameter(cmd, "Traffic", DbType.Currency, model.Traffic);
            this._db.AddInParameter(cmd, "Head", DbType.Currency, model.Head);
            this._db.AddInParameter(cmd, "AddPrice", DbType.Currency, model.AddPrice);
            this._db.AddInParameter(cmd, "GuideIncome", DbType.Currency, model.GuideIncome);
            this._db.AddInParameter(cmd, "GuidePay", DbType.Currency, model.GuidePay);
            this._db.AddInParameter(cmd, "Other", DbType.Currency, model.Other);
            this._db.AddInParameter(cmd, "PayType", DbType.Byte, (int)model.PayType.Value);
            this._db.AddInParameter(cmd, "SumPrice", DbType.Currency, model.SumPrice);
            this._db.AddInParameter(cmd, "Remark", DbType.String, model.Remark);
            this._db.AddInParameter(cmd, "IsMonth", DbType.AnsiStringFixedLength, model.IsMonth ? 1 : 0);
            this._db.AddInParameter(cmd, "MonthTime", DbType.DateTime, model.MonthTime);
            this._db.AddInParameter(cmd, "File", DbType.Xml, CreateFileXml(model.FileList));

            this._db.AddOutParameter(cmd, "Result", DbType.Int32, 4);
            DbHelper.RunProcedureWithResult(cmd, _db);
            return Convert.ToInt32(_db.GetParameterValue(cmd, "Result"));
        }

        /// <summary>
        /// 修改地接状态
        /// </summary>
        /// <param name="PlanId"></param>
        /// <param name="DiJieStatus"></param>
        /// <returns>
        /// 1:成功 0：失败
        /// -1:地接安排已确认过
        /// -2:财务已审核无法再确认
        /// -3:地接安排未确认无法审核
        /// -4:财务已审核无法再审核
        /// </returns>
        public int Update(string PlanId, EyouSoft.Model.EnumType.PlanStructure.DiJieStatus DiJieStatus)
        {
            DbCommand cmd = this._db.GetStoredProcCommand("proc_Dijie_Update_Status");
            this._db.AddInParameter(cmd, "PlanId", DbType.AnsiStringFixedLength, PlanId);
            this._db.AddInParameter(cmd, "DiJieStatus", DbType.Byte, (int)DiJieStatus);

            this._db.AddOutParameter(cmd, "Result", DbType.Int32, 4);
            DbHelper.RunProcedureWithResult(cmd, _db);
            return Convert.ToInt32(_db.GetParameterValue(cmd, "Result"));
        }

        /// <summary>
        /// 删除地接
        /// </summary>
        /// <param name="Id"></param>
        /// <returns>
        /// -1:只有处在申请中的地接安排才能删除
        ///	-2:地接存在支出不能删除
        ///	1:成功 0：失败
        /// </returns>
        public int Delete(string Id)
        {
            DbCommand cmd = this._db.GetStoredProcCommand("proc_Dijie_Delete");
            this._db.AddInParameter(cmd, "PlanId", DbType.AnsiStringFixedLength, Id);
            this._db.AddOutParameter(cmd, "Result", DbType.Int32, 4);
            DbHelper.RunProcedureWithResult(cmd, _db);
            return Convert.ToInt32(_db.GetParameterValue(cmd, "Result"));
        }

        /// <summary>
        /// 获取实体
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        public EyouSoft.Model.PlanStructure.MPlanDiJieInfo GetModel(string Id)
        {
            EyouSoft.Model.PlanStructure.MPlanDiJieInfo model = null;
            StringBuilder query = new StringBuilder();
            query.Append(" SELECT ");
            query.Append("PlanId,CompanyId,TourId,GysId,IsMonth,MonthTime ");
            query.Append(",(select TourCode from tbl_Tour where TourId=tbl_PlanDiJie.TourId) as TourCode ");
            query.Append(",(select UnitName from tbl_CompanySupplier where Id=tbl_PlanDiJie.GysId) as GysName ");
            query.Append(",Hotel,Dining,Car,Ticket,Guide,Traffic ");
            query.Append(",Head,AddPrice,GuideIncome,GuidePay ");
            query.Append(",Other,PayType,SumPrice,Remark,IssueTime,DiJieStatus, ");
            query.Append("(select * from tbl_PlanDiJieFile where PlanId=tbl_PlanDiJie.PlanId for xml raw,root('Root'))as FileInfo ");
            query.Append(" FROM ");
            query.Append("tbl_PlanDiJie");
            query.Append(" where PlanId=@PlanId");
            DbCommand cmd = this._db.GetSqlStringCommand(query.ToString());
            this._db.AddInParameter(cmd, "PlanId", DbType.AnsiStringFixedLength, Id);

            using (IDataReader dr = DbHelper.ExecuteReader(cmd, this._db))
            {
                while (dr.Read())
                {
                    model = new EyouSoft.Model.PlanStructure.MPlanDiJieInfo();
                    model.PlanId = dr.GetString(dr.GetOrdinal("PlanId"));
                    model.CompanyId = dr.GetInt32(dr.GetOrdinal("CompanyId"));
                    model.TourId = dr.GetString(dr.GetOrdinal("TourId"));
                    model.TourCode = dr.GetString(dr.GetOrdinal("TourCode"));
                    model.GysId = dr.GetString(dr.GetOrdinal("GysId"));
                    model.GysName = !dr.IsDBNull(dr.GetOrdinal("GysName")) ? dr.GetString(dr.GetOrdinal("GysName")) : null;
                    model.Hotel = dr.GetDecimal(dr.GetOrdinal("Hotel"));
                    model.Dining = dr.GetDecimal(dr.GetOrdinal("Dining"));
                    model.Car = dr.GetDecimal(dr.GetOrdinal("Car"));
                    model.Ticket = dr.GetDecimal(dr.GetOrdinal("Ticket"));
                    model.Guide = dr.GetDecimal(dr.GetOrdinal("Guide"));
                    model.Traffic = dr.GetDecimal(dr.GetOrdinal("Traffic"));
                    model.Head = dr.GetDecimal(dr.GetOrdinal("Head"));
                    model.AddPrice = dr.GetDecimal(dr.GetOrdinal("AddPrice"));
                    model.GuideIncome = dr.GetDecimal(dr.GetOrdinal("GuideIncome"));
                    model.GuidePay = dr.GetDecimal(dr.GetOrdinal("GuidePay"));
                    model.Other = dr.GetDecimal(dr.GetOrdinal("Other"));
                    model.PayType = (EyouSoft.Model.EnumType.FinStructure.ShouFuKuanFangShi?)dr.GetByte(dr.GetOrdinal("PayType"));
                    model.SumPrice = dr.GetDecimal(dr.GetOrdinal("SumPrice"));
                    model.Remark = !dr.IsDBNull(dr.GetOrdinal("Remark")) ? dr.GetString(dr.GetOrdinal("Remark")) : null;
                    model.IssueTime = (DateTime?)dr.GetDateTime(dr.GetOrdinal("IssueTime"));
                    model.DiJieStatus = (EyouSoft.Model.EnumType.PlanStructure.DiJieStatus)dr.GetByte(dr.GetOrdinal("DiJieStatus"));
                    model.IsMonth = dr.GetString(dr.GetOrdinal("IsMonth")) == "1";
                    model.MonthTime = !dr.IsDBNull(dr.GetOrdinal("MonthTime")) ? (DateTime?)dr.GetDateTime(dr.GetOrdinal("MonthTime")) : null;

                    model.FileList = new List<EyouSoft.Model.TourStructure.MFile>();
                    model.FileList = !dr.IsDBNull(dr.GetOrdinal("FileInfo")) ? GetFileByXml(dr.GetString(dr.GetOrdinal("FileInfo"))) : null;
                }
            }
            return model;
        }

        /// <summary>
        /// 获取地接列表
        /// </summary>
        /// <param name="companyId"></param>
        /// <param name="pageSize"></param>
        /// <param name="pageIndex"></param>
        /// <param name="recordCount"></param>
        /// <param name="search"></param>
        /// <returns>Sum[]:0:SumPrice</returns>
        public IList<EyouSoft.Model.PlanStructure.MPageDiJie> GetList(
        int companyId,
        int pageSize,
        int pageIndex,
        ref int recordCount,
        EyouSoft.Model.PlanStructure.MSeachDiJie search, ref decimal[] Sum)
        {
            IList<EyouSoft.Model.PlanStructure.MPageDiJie> list = new List<EyouSoft.Model.PlanStructure.MPageDiJie>();

            StringBuilder fileds = new StringBuilder();
            fileds.Append("PlanId,GysId,GysName,ContactInfo,Hotel,Dining,Car,Ticket,Guide,Traffic,IsMonth,MonthTime");
            fileds.Append(",PlanPiaoInfo,Head,AddPrice,GuideIncome,GuidePay,Other,SumPrice");
            fileds.Append(",DiJieStatus,IssueTime,TourCode,RouteName,LDate,Adults,Childs,Accompanys,TourStatus,TourType");

            string tableName = "view_DiJie";

            string orderByString = " TourStatus asc,IssueTime desc ";

            string sumFileds = "Sum(SumPrice) as TotalSumPrice";

            StringBuilder query = new StringBuilder();
            query.AppendFormat(" CompanyId={0} ", companyId);

            if (search != null)
            {
                if (string.IsNullOrEmpty(search.TourCode)
                    && search.LBeginDate.HasValue == false
                    && search.LEndDate.HasValue == false
                    && string.IsNullOrEmpty(search.DiJieName))
                {
                    //财务操作结束后，该团就自动隐藏，用搜索可查询到
                    query.Append(" and IsEnd='0' ");
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
                    query.AppendFormat(" and  datediff(day,LDate,'{0}')>=0 ", search.LEndDate.Value);
                }

                if (!string.IsNullOrEmpty(search.DiJieId))
                {
                    query.AppendFormat(" and GysId='{0}' ", search.DiJieId);
                }

                if (!string.IsNullOrEmpty(search.DiJieName))
                {
                    query.AppendFormat(" and GysName like '%{0}%' ", search.DiJieName);
                }

                if (search.IsMonth.HasValue)
                {
                    query.AppendFormat(" and IsMonth='{0}' ", search.IsMonth.Value ? 1 : 0);
                }
                if (search.TourType.HasValue)
                {
                    query.AppendFormat(" and TourType={0} ", (int)search.TourType.Value);
                }

            }
            else
            {
                //财务操作结束后，该团就自动隐藏，用搜索可查询到
                query.Append(" and IsEnd='0' ");
            }

            using (IDataReader dr = DbHelper.ExecuteReader1(this._db, pageSize, pageIndex, ref recordCount, tableName, fileds.ToString(), query.ToString(), orderByString, sumFileds))
            {
                while (dr.Read())
                {
                    EyouSoft.Model.PlanStructure.MPageDiJie model = new EyouSoft.Model.PlanStructure.MPageDiJie();
                    model.PlanId = dr.GetString(dr.GetOrdinal("PlanId"));
                    model.TourCode = dr.GetString(dr.GetOrdinal("TourCode"));
                    model.RouteName = !dr.IsDBNull(dr.GetOrdinal("RouteName")) ? dr.GetString(dr.GetOrdinal("RouteName")) : null;
                    model.LDate = dr.GetDateTime(dr.GetOrdinal("LDate"));
                    model.Adults = dr.GetInt32(dr.GetOrdinal("Adults"));
                    model.Childs = dr.GetInt32(dr.GetOrdinal("Childs"));
                    model.Accompanys = dr.GetInt32(dr.GetOrdinal("Accompanys"));
                    model.GysName = !dr.IsDBNull(dr.GetOrdinal("GysName")) ? dr.GetString(dr.GetOrdinal("GysName")) : null;

                    model.ContactList = new List<EyouSoft.Model.SourceStructure.MSupplierContact>();
                    model.ContactList = !dr.IsDBNull(dr.GetOrdinal("ContactInfo")) ? GetSupplierContactByXml(dr.GetString(dr.GetOrdinal("ContactInfo"))) : null;

                    model.PlanTicketList = new List<EyouSoft.Model.PlanStructure.MPlanTicket>();
                    model.PlanTicketList = !dr.IsDBNull(dr.GetOrdinal("PlanPiaoInfo")) ? GetPlanTicketByXml(dr.GetString(dr.GetOrdinal("PlanPiaoInfo"))) : null;

                    model.Hotel = dr.GetDecimal(dr.GetOrdinal("Hotel"));
                    model.Dining = dr.GetDecimal(dr.GetOrdinal("Dining"));
                    model.Car = dr.GetDecimal(dr.GetOrdinal("Car"));
                    model.Ticket = dr.GetDecimal(dr.GetOrdinal("Ticket"));
                    model.Guide = dr.GetDecimal(dr.GetOrdinal("Guide"));
                    model.Traffic = dr.GetDecimal(dr.GetOrdinal("Traffic"));
                    model.Head = dr.GetDecimal(dr.GetOrdinal("Head"));
                    model.AddPrice = dr.GetDecimal(dr.GetOrdinal("AddPrice"));
                    model.GuideIncome = dr.GetDecimal(dr.GetOrdinal("GuideIncome"));
                    model.GuidePay = dr.GetDecimal(dr.GetOrdinal("GuidePay"));
                    model.Other = dr.GetDecimal(dr.GetOrdinal("Other"));
                    model.SumPrice = dr.GetDecimal(dr.GetOrdinal("SumPrice"));
                    model.TourStatus = (EyouSoft.Model.EnumType.TourStructure.TourStatus)dr.GetByte(dr.GetOrdinal("TourStatus"));
                    model.TourType = (EyouSoft.Model.EnumType.TourStructure.TourType?)dr.GetByte(dr.GetOrdinal("TourType"));
                    model.IsMonth = dr.GetString(dr.GetOrdinal("IsMonth")) == "1";
                    model.MonthTime = !dr.IsDBNull(dr.GetOrdinal("MonthTime")) ? (DateTime?)dr.GetDateTime(dr.GetOrdinal("MonthTime")) : null;


                    model.DiJieStatus = (EyouSoft.Model.EnumType.PlanStructure.DiJieStatus?)dr.GetByte(dr.GetOrdinal("DiJieStatus"));
                    list.Add(model);
                }

                if (dr.NextResult())
                {
                    if (dr.Read())
                    {
                        Sum[0] = !dr.IsDBNull(dr.GetOrdinal("TotalSumPrice")) ? dr.GetDecimal(dr.GetOrdinal("TotalSumPrice")) : 0;
                    }
                }
            }

            return list;
        }

        #endregion


        #region

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


        /// <summary>
        /// 根据xml获取客户单位联系人
        /// </summary>
        /// <param name="xml"></param>
        /// <returns></returns>
        private IList<EyouSoft.Model.SourceStructure.MSupplierContact> GetSupplierContactByXml(string xml)
        {
            if (string.IsNullOrEmpty(xml)) return null;
            IList<EyouSoft.Model.SourceStructure.MSupplierContact> list = new List<EyouSoft.Model.SourceStructure.MSupplierContact>();
            System.Xml.Linq.XElement xRoot = System.Xml.Linq.XElement.Parse(xml);
            var xRows = Utils.GetXElements(xRoot, "row");
            foreach (var xRow in xRows)
            {
                EyouSoft.Model.SourceStructure.MSupplierContact model = new EyouSoft.Model.SourceStructure.MSupplierContact();

                model.ContactName = Utils.GetXAttributeValue(xRow, "ContactName");
                model.ContactTel = Utils.GetXAttributeValue(xRow, "ContactTel");
                model.QQ = Utils.GetXAttributeValue(xRow, "QQ");
                list.Add(model);
            }
            return list;
        }


        /// <summary>
        /// 根据xml获取票务安排信息
        /// </summary>
        /// <param name="xml"></param>
        /// <returns></returns>
        private IList<EyouSoft.Model.PlanStructure.MPlanTicket> GetPlanTicketByXml(string xml)
        {
            if (string.IsNullOrEmpty(xml)) return null;
            IList<EyouSoft.Model.PlanStructure.MPlanTicket> list = new List<EyouSoft.Model.PlanStructure.MPlanTicket>();
            System.Xml.Linq.XElement xRoot = System.Xml.Linq.XElement.Parse(xml);
            var xRows = Utils.GetXElements(xRoot, "row");
            foreach (var xRow in xRows)
            {
                EyouSoft.Model.PlanStructure.MPlanTicket model = new EyouSoft.Model.PlanStructure.MPlanTicket();
                model.TicketType = (EyouSoft.Model.EnumType.PlanStructure.TicketType)Utils.GetInt(Utils.GetXAttributeValue(xRow, "TicketType"));
                model.Interval = Utils.GetXAttributeValue(xRow, "Interval");
                //model.TrafficTime = Utils.GetXAttributeValue(xRow, "TrafficTime");
                model.TrafficTime = Utils.GetDateTimeNullable(Utils.GetXAttributeValue(xRow, "TrafficTime"));
                model.TrafficNumber = Utils.GetXAttributeValue(xRow, "TrafficNumber");

                list.Add(model);
            }

            return list;
        }
        #endregion
    }
}
