using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Practices.EnterpriseLibrary.Data;
using System.Data.Common;
using System.Data;
using EyouSoft.Toolkit.DAL;
using EyouSoft.Toolkit;

namespace EyouSoft.DAL.TourStructure
{
    public class DTourReturnVisit : EyouSoft.Toolkit.DAL.DALBase, EyouSoft.IDAL.TourStructure.ITourReturnVisit
    {
        #region 初始化db
        private Database _db = null;

        /// <summary>
        /// 初始化_db
        /// </summary>
        public DTourReturnVisit()
        {
            _db = base.SystemStore;
        }
        #endregion

        #region ITourReturnVisit 成员
        /// <summary>
        /// 添加团队回访记录
        /// </summary>
        /// <param name="model"></param>
        /// <returns>0:添加失败 1:添加成功</returns>
        public int Add(EyouSoft.Model.TourStructure.MTourReturnVisit model)
        {
            DbCommand cmd = this._db.GetStoredProcCommand("proc_TourReturnVisit_Add");
            this._db.AddInParameter(cmd, "VisitId", DbType.AnsiStringFixedLength, model.VisitId);
            this._db.AddInParameter(cmd, "TourId", DbType.AnsiStringFixedLength, model.TourId);
            this._db.AddInParameter(cmd, "QuanPeiName", DbType.String, model.QuanPeiName);
            this._db.AddInParameter(cmd, "QuanPeiPhone", DbType.String, model.QuanPeiPhone);
            this._db.AddInParameter(cmd, "VisitTime", DbType.DateTime, model.VisitTime);
            this._db.AddInParameter(cmd, "Vistor", DbType.String, model.Vistor);
            this._db.AddInParameter(cmd, "Score", DbType.Byte, (int)model.Score);
            this._db.AddInParameter(cmd, "CustomerOpinion", DbType.String, model.CustomerOpinion);
            this._db.AddInParameter(cmd, "OperatorId", DbType.Int32, model.OperatorId);

            this._db.AddOutParameter(cmd, "Result", DbType.Int32, 4);
            DbHelper.RunProcedureWithResult(cmd, _db);
            return Convert.ToInt32(_db.GetParameterValue(cmd, "Result"));
        }

        /// <summary>
        /// 修改团队回访记录
        /// </summary>
        /// <param name="model"></param>
        /// <returns>-1:团队质量评估已反馈不允许修改 0:失败 1:成功</returns>
        public int Update(EyouSoft.Model.TourStructure.MTourReturnVisit model)
        {
            DbCommand cmd = this._db.GetStoredProcCommand("proc_TourReturnVisit_Update");
            this._db.AddInParameter(cmd, "VisitId", DbType.AnsiStringFixedLength, model.VisitId);
            this._db.AddInParameter(cmd, "QuanPeiName", DbType.String, model.QuanPeiName);
            this._db.AddInParameter(cmd, "QuanPeiPhone", DbType.String, model.QuanPeiPhone);
            this._db.AddInParameter(cmd, "VisitTime", DbType.DateTime, model.VisitTime);
            this._db.AddInParameter(cmd, "Vistor", DbType.String, model.Vistor);
            this._db.AddInParameter(cmd, "Score", DbType.Byte, (int)model.Score);
            this._db.AddInParameter(cmd, "CustomerOpinion", DbType.String, model.CustomerOpinion);

            this._db.AddOutParameter(cmd, "Result", DbType.Int32, 4);
            DbHelper.RunProcedureWithResult(cmd, _db);
            return Convert.ToInt32(_db.GetParameterValue(cmd, "Result"));
        }

        /// <summary>
        /// 根据编号删除回访记录
        /// </summary>
        /// <param name="Id"></param>
        /// <returns>-1:团队质量评估已反馈不允许删除 0:失败 1:成功</returns>
        public int Delete(string Id)
        {
            DbCommand cmd = this._db.GetStoredProcCommand("proc_TourReturnVisit_Delete");
            this._db.AddInParameter(cmd, "VisitId", DbType.AnsiStringFixedLength, Id);

            this._db.AddOutParameter(cmd, "Result", DbType.Int32, 4);
            DbHelper.RunProcedureWithResult(cmd, _db);
            return Convert.ToInt32(_db.GetParameterValue(cmd, "Result"));
        }

        /// <summary>
        /// 根据Id获取model
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        public EyouSoft.Model.TourStructure.MTourReturnVisit GetModel(string Id)
        {
            EyouSoft.Model.TourStructure.MTourReturnVisit model = null;

            string sql = "select * from tbl_TourReturnVisit where VisitId=@VisitId";

            DbCommand cmd = this._db.GetSqlStringCommand(sql);
            this._db.AddInParameter(cmd, "VisitId", DbType.AnsiStringFixedLength, Id);

            using (IDataReader dr = DbHelper.ExecuteReader(cmd, this._db))
            {
                while (dr.Read())
                {
                    model = new EyouSoft.Model.TourStructure.MTourReturnVisit();
                    model.VisitId = dr.GetString(dr.GetOrdinal("VisitId"));
                    model.TourId = dr.GetString(dr.GetOrdinal("TourId"));
                    model.QuanPeiName = !dr.IsDBNull(dr.GetOrdinal("QuanPeiName")) ? dr.GetString(dr.GetOrdinal("QuanPeiName")) : null;
                    model.QuanPeiPhone = !dr.IsDBNull(dr.GetOrdinal("QuanPeiPhone")) ? dr.GetString(dr.GetOrdinal("QuanPeiPhone")) : null;
                    model.VisitTime = !dr.IsDBNull(dr.GetOrdinal("VisitTime")) ? (DateTime?)dr.GetDateTime(dr.GetOrdinal("VisitTime")) : null;
                    model.Vistor = !dr.IsDBNull(dr.GetOrdinal("Vistor")) ? dr.GetString(dr.GetOrdinal("Vistor")) : null;
                    model.Score = (EyouSoft.Model.EnumType.TourStructure.Score)dr.GetByte(dr.GetOrdinal("Score"));
                    model.CustomerOpinion = !dr.IsDBNull(dr.GetOrdinal("CustomerOpinion")) ? dr.GetString(dr.GetOrdinal("CustomerOpinion")) : null;
                    model.OperatorId = dr.GetInt32(dr.GetOrdinal("OperatorId"));
                }
            }

            return model;
        }

        /// <summary>
        /// 设置团队质量评估
        /// </summary>
        /// <param name="TourId"></param>
        /// <returns></returns>
        public bool SetTourScore(string tourId, EyouSoft.Model.EnumType.TourStructure.Score score, int OperatorId)
        {

            string sql = "UPDATE tbl_Tour SET Score = @Score,SOperatorId = @SOperatorId WHERE TourId=@TourId";
            DbCommand cmd = this._db.GetSqlStringCommand(sql);
            this._db.AddInParameter(cmd, "Score", DbType.Byte, (int)score);
            this._db.AddInParameter(cmd, "SOperatorId", DbType.Int32, OperatorId);
            this._db.AddInParameter(cmd, "TourId", DbType.AnsiStringFixedLength, tourId);

            return DbHelper.ExecuteSql(cmd, this._db) == 1;
        }


        /// <summary>
        /// 获取回访记录的列表
        /// </summary>
        /// <param name="TourId"></param>
        /// <returns></returns>
        public IList<EyouSoft.Model.TourStructure.MTourReturnVisit> GetList(string TourId)
        {

            IList<EyouSoft.Model.TourStructure.MTourReturnVisit> list = new List<EyouSoft.Model.TourStructure.MTourReturnVisit>();

            string sql = "select * from tbl_TourReturnVisit where TourId=@TourId";

            DbCommand cmd = this._db.GetSqlStringCommand(sql);
            this._db.AddInParameter(cmd, "TourId", DbType.AnsiStringFixedLength, TourId);

            using (IDataReader dr = DbHelper.ExecuteReader(cmd, this._db))
            {

                while (dr.Read())
                {
                    EyouSoft.Model.TourStructure.MTourReturnVisit model = new EyouSoft.Model.TourStructure.MTourReturnVisit();
                    model.VisitId = dr.GetString(dr.GetOrdinal("VisitId"));
                    model.TourId = dr.GetString(dr.GetOrdinal("TourId"));
                    model.QuanPeiName = !dr.IsDBNull(dr.GetOrdinal("QuanPeiName")) ? dr.GetString(dr.GetOrdinal("QuanPeiName")) : null;
                    model.QuanPeiPhone = !dr.IsDBNull(dr.GetOrdinal("QuanPeiPhone")) ? dr.GetString(dr.GetOrdinal("QuanPeiPhone")) : null;
                    model.VisitTime = !dr.IsDBNull(dr.GetOrdinal("VisitTime")) ? (DateTime?)dr.GetDateTime(dr.GetOrdinal("VisitTime")) : null;
                    model.Vistor = !dr.IsDBNull(dr.GetOrdinal("Vistor")) ? dr.GetString(dr.GetOrdinal("Vistor")) : null;
                    model.Score = (EyouSoft.Model.EnumType.TourStructure.Score)dr.GetByte(dr.GetOrdinal("Score"));
                    model.CustomerOpinion = !dr.IsDBNull(dr.GetOrdinal("CustomerOpinion")) ? dr.GetString(dr.GetOrdinal("CustomerOpinion")) : null;
                    model.OperatorId = dr.GetInt32(dr.GetOrdinal("OperatorId"));

                    list.Add(model);
                }
            }
            return list;
        }


        /// <summary>
        /// 获取回访提醒，团队质量反馈列表
        /// </summary>
        /// <param name="flg">0:回访提醒 1：团队质量反馈</param>
        /// <param name="companyId">公司编号</param>
        /// <param name="pageSize">每页显示的条数</param>
        /// <param name="pageIndex">当前页</param>
        /// <param name="recordCount">总记录数</param>
        /// <param name="search">查询实体</param>
        /// <returns></returns>
        public IList<EyouSoft.Model.TourStructure.MPageTourReturnVisit> GetList(
              int flg,
              int companyId,
              int pageSize,
              int pageIndex,
              ref int recordCount,
              EyouSoft.Model.TourStructure.MSeachVist search)
        {

            IList<EyouSoft.Model.TourStructure.MPageTourReturnVisit> list = new List<EyouSoft.Model.TourStructure.MPageTourReturnVisit>();

            string tableName = "tbl_Tour";
            StringBuilder fileds = new StringBuilder();
            fileds.Append(" TourId  ");
            fileds.Append(" ,CompanyId  ");
            fileds.Append(" ,TourCode  ");
            fileds.Append(" ,TourStatus ");
            fileds.Append(" ,TourType ");
            fileds.Append(" ,RouteName  ");
            fileds.Append(" ,LDate ");
            fileds.Append(" ,Adults  ");
            fileds.Append(" ,Childs  ");
            fileds.Append(" ,Accompanys ");
            fileds.Append(" ,Score  ");
            fileds.Append(" ,SOperatorId  ");
            fileds.Append(" ,(select CustomerName from tbl_Customer where Id=tbl_Tour.BuyCompanyId) as BuyCompnayName  ");
            fileds.Append(" ,(select  (select UnitName from tbl_CompanySupplier where Id=B.GysId ) as DiJieName,  ");
            fileds.Append("  (select top 1 ContactName,ContactTel,QQ  from tbl_SupplierContact where SupplierId=B.GysId for xml path,elements ) as DiJieContactInfo");
            fileds.Append("  from tbl_PlanDiJie as B where B.TourId =tbl_Tour.TourId for xml path,elements,root('Root')) as DiJieInfo");
            fileds.Append(" ,(SELECT C.Phone,   ");
            fileds.Append("  (select GuideName from tbl_SupplierGuide where Id=C.GuideId) as GuideName  ");
            fileds.Append("  FROM dbo.tbl_TourGuide as C where C.TourId=tbl_Tour.TourId for xml raw,root('Root'))as TourGuide ");
            fileds.Append(" ,(select * from tbl_TourReturnVisit where TourId=tbl_Tour.TourId order by IssueTime asc for xml raw,root('root')) as VisitInfo	  ");


            string OrderByString = " IsChuTuan asc,LDate desc ";

            StringBuilder query = new StringBuilder();

            //2013-4-8	回访提醒默认显示正行行程中的团队，和出发前3天和回来3天的内团。
            query.AppendFormat(" CompanyId={0} and IsDelete='0' and (TourStatus={1}  or (datediff(day,dateadd(day,-3,LDate),getdate())>=0 and datediff(day,dateadd(day,3,RDate),getdate())<=0) )", companyId, (int)EyouSoft.Model.EnumType.TourStructure.TourStatus.行程中);

            //if (flg == 0)
            //{
            //    //显示回访不足2次的团 或则 评估为差评的团    空星的全部为差评
            //    query.AppendFormat(" and ((select count(1) from tbl_TourReturnVisit where TourId=tbl_Tour.TourId) <2 or Score in ({0},{1},{2},{3},{4}))",
            //                       (int)EyouSoft.Model.EnumType.TourStructure.Score.六星,
            //                       (int)EyouSoft.Model.EnumType.TourStructure.Score.七星,
            //                       (int)EyouSoft.Model.EnumType.TourStructure.Score.八星,
            //                       (int)EyouSoft.Model.EnumType.TourStructure.Score.九星,
            //                       (int)EyouSoft.Model.EnumType.TourStructure.Score.十星
            //        );

            //}

            if (search != null)
            {
                if (search.LBeginDate.HasValue)
                {
                    query.AppendFormat(" and  datediff(day,LDate,'{0}')<=0 ", search.LBeginDate.Value);
                }

                if (search.LEndDate.HasValue)
                {
                    query.AppendFormat(" and  datediff(day,LDate,'{0}')>=0 ", search.LEndDate.Value);
                }
                if (!string.IsNullOrEmpty(search.TourCode))
                {
                    query.AppendFormat(" and TourCode like '%{0}%' ", search.TourCode);

                }

                if (search.IsVist.HasValue)
                {

                    if (search.IsVist.Value)
                    {
                        query.AppendFormat("and exists(select 1 from tbl_TourReturnVisit where TourId=tbl_Tour.TourId)");
                    }
                    else
                    {
                        query.AppendFormat("and not exists(select 1 from tbl_TourReturnVisit where TourId=tbl_Tour.TourId)");
                    }
                }

                if (search.TourType.HasValue)
                {
                    query.AppendFormat(" and TourType={0} ", (int)search.TourType.Value);
                }
            }

            using (IDataReader dr = DbHelper.ExecuteReader1(this._db, pageSize, pageIndex, ref recordCount, tableName, fileds.ToString(), query.ToString(), OrderByString, null))
            {
                while (dr.Read())
                {
                    EyouSoft.Model.TourStructure.MPageTourReturnVisit model = new EyouSoft.Model.TourStructure.MPageTourReturnVisit();
                    model.TourId = dr.GetString(dr.GetOrdinal("TourId"));
                    model.TourCode = dr.GetString(dr.GetOrdinal("TourCode"));
                    model.TourStatus = !dr.IsDBNull(dr.GetOrdinal("TourStatus")) ? (EyouSoft.Model.EnumType.TourStructure.TourStatus?)dr.GetByte(dr.GetOrdinal("TourStatus")) : null;
                    model.TourType = !dr.IsDBNull(dr.GetOrdinal("TourType")) ? (EyouSoft.Model.EnumType.TourStructure.TourType?)dr.GetByte(dr.GetOrdinal("TourType")) : null;
                    model.RouteName = !dr.IsDBNull(dr.GetOrdinal("RouteName")) ? dr.GetString(dr.GetOrdinal("RouteName")) : null;
                    model.LDate = dr.GetDateTime(dr.GetOrdinal("LDate"));
                    model.Adults = dr.GetInt32(dr.GetOrdinal("Adults"));
                    model.Childs = dr.GetInt32(dr.GetOrdinal("Childs"));
                    model.Accompanys = dr.GetInt32(dr.GetOrdinal("Accompanys"));
                    model.BuyCompnayName = !dr.IsDBNull(dr.GetOrdinal("BuyCompnayName")) ? dr.GetString(dr.GetOrdinal("BuyCompnayName")) : null;
                    model.Score = !dr.IsDBNull(dr.GetOrdinal("Score")) ? (EyouSoft.Model.EnumType.TourStructure.Score?)dr.GetByte(dr.GetOrdinal("Score")) : null;
                    model.SOperatorId = dr.GetInt32(dr.GetOrdinal("SOperatorId"));
                    model.DiJieList = new List<EyouSoft.Model.TourStructure.MTourDiJie>();
                    model.DiJieList = !dr.IsDBNull(dr.GetOrdinal("DiJieInfo")) ? GetTourDiJieByXml(dr.GetString(dr.GetOrdinal("DiJieInfo"))) : null;

                    model.GuideList = new List<EyouSoft.Model.TourStructure.MTourGuide>();
                    model.GuideList = !dr.IsDBNull(dr.GetOrdinal("TourGuide")) ? GetTourGuideByXml(dr.GetString(dr.GetOrdinal("TourGuide"))) : null;

                    model.VisitList = new List<EyouSoft.Model.TourStructure.MTourReturnVisit>();
                    model.VisitList = !dr.IsDBNull(dr.GetOrdinal("VisitInfo")) ? GetTourReturnVisitByXml(dr.GetString(dr.GetOrdinal("VisitInfo"))) : null;

                    list.Add(model);
                }
            }

            return list;

        }
        #endregion




        #region
        /// <summary>
        /// 根据xml获取团队地接集合
        /// </summary>
        /// <param name="xml"></param>
        /// <returns></returns>
        private IList<EyouSoft.Model.TourStructure.MTourDiJie> GetTourDiJieByXml(string xml)
        {
            if (string.IsNullOrEmpty(xml)) return null;
            xml = xml.Replace("&lt;", "<").Replace("&gt;", ">");
            IList<EyouSoft.Model.TourStructure.MTourDiJie> list = new List<EyouSoft.Model.TourStructure.MTourDiJie>();

            System.Xml.XmlDocument doc = new System.Xml.XmlDocument();
            doc.LoadXml(xml);
            System.Xml.XmlNode root = doc.SelectSingleNode("Root");
            if (root != null)
            {
                foreach (System.Xml.XmlNode node in root.ChildNodes)
                {
                    EyouSoft.Model.TourStructure.MTourDiJie model = new EyouSoft.Model.TourStructure.MTourDiJie();
                    model.DiJieName = node["DiJieName"] != null ? node["DiJieName"].InnerText : null;

                    if (node["DiJieContactInfo"] != null && node["DiJieContactInfo"].HasChildNodes)
                    {
                        foreach (System.Xml.XmlNode child in node["DiJieContactInfo"].ChildNodes)
                        {
                            model.Name = child["ContactName"] != null ? child["ContactName"].InnerText : null;
                            model.Phone = child["ContactTel"] != null ? child["ContactTel"].InnerText : null;
                            model.QQ = child["QQ"] != null ? child["QQ"].InnerText : null;
                        }
                    }

                    list.Add(model);
                }
            }
            return list;
        }


        /// <summary>
        /// 根据xml获取团队导游
        /// </summary>
        /// <param name="xml"></param>
        /// <returns></returns>
        private IList<EyouSoft.Model.TourStructure.MTourGuide> GetTourGuideByXml(string xml)
        {
            if (string.IsNullOrEmpty(xml)) return null;
            IList<EyouSoft.Model.TourStructure.MTourGuide> list = new List<EyouSoft.Model.TourStructure.MTourGuide>();
            System.Xml.Linq.XElement xRoot = System.Xml.Linq.XElement.Parse(xml);
            var xRows = Utils.GetXElements(xRoot, "row");
            foreach (var xRow in xRows)
            {
                EyouSoft.Model.TourStructure.MTourGuide model = new EyouSoft.Model.TourStructure.MTourGuide();
                model.Id = Utils.GetXAttributeValue(xRow, "Id");
                model.GuideName = Utils.GetXAttributeValue(xRow, "GuideName");
                model.Phone = Utils.GetXAttributeValue(xRow, "Phone");
                list.Add(model);
            }
            return list;

        }


        /// <summary>
        /// 根据xml获取回访信息
        /// </summary>
        /// <param name="xml"></param>
        /// <returns></returns>
        public IList<EyouSoft.Model.TourStructure.MTourReturnVisit> GetTourReturnVisitByXml(string xml)
        {
            if (string.IsNullOrEmpty(xml)) return null;

            IList<EyouSoft.Model.TourStructure.MTourReturnVisit> list = new List<EyouSoft.Model.TourStructure.MTourReturnVisit>();
            System.Xml.Linq.XElement xRoot = System.Xml.Linq.XElement.Parse(xml);
            var xRows = Utils.GetXElements(xRoot, "row");
            foreach (var xRow in xRows)
            {
                EyouSoft.Model.TourStructure.MTourReturnVisit model = new EyouSoft.Model.TourStructure.MTourReturnVisit();
                model.VisitId = Utils.GetXAttributeValue(xRow, "Id");
                model.TourId = Utils.GetXAttributeValue(xRow, "TourId");
                model.QuanPeiName = Utils.GetXAttributeValue(xRow, "QuanPeiName");
                model.QuanPeiPhone = Utils.GetXAttributeValue(xRow, "QuanPeiPhone");
                model.VisitTime = Utils.GetDateTimeNullable(Utils.GetXAttributeValue(xRow, "VisitTime"));
                model.Vistor = Utils.GetXAttributeValue(xRow, "Vistor");
                model.Score = (EyouSoft.Model.EnumType.TourStructure.Score)Utils.GetInt(Utils.GetXAttributeValue(xRow, "Score"));
                model.CustomerOpinion = Utils.GetXAttributeValue(xRow, "CustomerOpinion");
                model.OperatorId = Utils.GetInt(Utils.GetXAttributeValue(xRow, "OperatorId"));
                model.IssueTime = Utils.GetDateTime(Utils.GetXAttributeValue(xRow, "IssueTime"));
                list.Add(model);
            }
            return list;
        }

        #endregion

    }
}
