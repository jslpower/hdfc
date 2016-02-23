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
    public class DTourData : EyouSoft.Toolkit.DAL.DALBase, EyouSoft.IDAL.TourStructure.ITourData
    {
        #region 初始化db
        private Database _db = null;

        /// <summary>
        /// 初始化_db
        /// </summary>
        public DTourData()
        {
            _db = base.SystemStore;
        }
        #endregion




        #region IRouteData 成员

        /// <summary>
        /// 团队报价资料库
        /// </summary>
        /// <param name="model"></param>
        /// <returns>0:添加失败 大于0:添加成功</returns>
        public int Add(EyouSoft.Model.TourStructure.MTourData model)
        {
            DbCommand cmd = this._db.GetStoredProcCommand("proc_TourData_Add");
            this._db.AddInParameter(cmd, "CompanyId", DbType.Int32, model.CompanyId);
            this._db.AddInParameter(cmd, "AreaId", DbType.Int32, model.AreaId);
            this._db.AddInParameter(cmd, "RouteName", DbType.String, model.RouteName);
            this._db.AddInParameter(cmd, "TourDataType", DbType.Byte, (int)model.TourDataType);
            this._db.AddInParameter(cmd, "TourPort", DbType.String, model.TourPort);
            this._db.AddInParameter(cmd, "OperatorId", DbType.Int32, model.OperatorId);
            this._db.AddInParameter(cmd, "File", DbType.Xml, CreateFileXml(model.FileList));


            this._db.AddOutParameter(cmd, "Result", DbType.Int32, 4);
            DbHelper.RunProcedureWithResult(cmd, _db);
            return Convert.ToInt32(_db.GetParameterValue(cmd, "Result"));
        }

        /// <summary>
        /// 修改团队报价资料库
        /// </summary>
        /// <param name="model"></param>
        /// <returns>0:失败 1:成功 -1:审核过后不允许修改</returns>
        public int Update(EyouSoft.Model.TourStructure.MTourData model)
        {
            DbCommand cmd = this._db.GetStoredProcCommand("proc_TourData_Update");
            this._db.AddInParameter(cmd, "TourDataId", DbType.Int32, model.TourDataId);
            this._db.AddInParameter(cmd, "AreaId", DbType.Int32, model.AreaId);
            this._db.AddInParameter(cmd, "RouteName", DbType.String, model.RouteName);
            this._db.AddInParameter(cmd, "TourDataType", DbType.Byte, (int)model.TourDataType);
            this._db.AddInParameter(cmd, "TourPort", DbType.String, model.TourPort);
            this._db.AddInParameter(cmd, "File", DbType.Xml, CreateFileXml(model.FileList));


            this._db.AddOutParameter(cmd, "Result", DbType.Int32, 4);
            DbHelper.RunProcedureWithResult(cmd, _db);
            return Convert.ToInt32(_db.GetParameterValue(cmd, "Result"));
        }

        /// <summary>
        /// 删除报价资料库 -1:审核过的 不允许删除
        /// </summary>
        /// <param name="Id"></param>
        /// <returns>0:失败 1:成功</returns>
        public int Delete(int Id)
        {
            DbCommand cmd = this._db.GetStoredProcCommand("proc_TourData_Delete");
            this._db.AddInParameter(cmd, "TourDataId", DbType.Int32, Id);

            this._db.AddOutParameter(cmd, "Result", DbType.Int32, 4);
            DbHelper.RunProcedureWithResult(cmd, _db);
            return Convert.ToInt32(_db.GetParameterValue(cmd, "Result"));
        }


        /// <summary>
        /// 分页获取资料库列表
        /// </summary>
        /// <param name="companyId"></param>
        /// <param name="pageSize"></param>
        /// <param name="pageIndex"></param>
        /// <param name="recordCount"></param>
        /// <param name="search"></param>
        /// <returns></returns>
        public IList<EyouSoft.Model.TourStructure.MTourData> GetList(int companyId, int pageSize, int pageIndex, ref int recordCount, EyouSoft.Model.TourStructure.MSearchTourData search)
        {
            IList<EyouSoft.Model.TourStructure.MTourData> list = new List<EyouSoft.Model.TourStructure.MTourData>();

            StringBuilder fileds = new StringBuilder();
            fileds.Append(" TourDataId,CompanyId,IssueTime,AreaId");
            fileds.Append(",(select ContactName from tbl_CompanyUser where Id=tbl_TourData.OperatorId) as OperatorName");
            fileds.Append(",RouteName,TourDataType,TourPort,OperatorId");
            fileds.Append(",IsCheck,CheckId,CheckTime");
            fileds.Append(",(select * from tbl_TourDataFile where TourDataId=tbl_TourData.TourDataId for xml raw,root('Root')) as FileInfo");


            string tableName = "tbl_TourData";

            string OrderByString = " IssueTime desc ";

            StringBuilder query = new StringBuilder();

            query.AppendFormat(" CompanyId={0} ", companyId);

            if (search != null)
            {
                if (search.IsCheck.HasValue)
                {

                    query.AppendFormat(" and IsCheck='{0}' ", search.IsCheck.Value ? 1 : 0);
                }

                if (!string.IsNullOrEmpty(search.RouteName))
                {
                    query.AppendFormat(" and RouteName like '%{0}%' ", search.RouteName);
                }
                if (search.AreaId.HasValue)
                {
                    query.AppendFormat(" and AreaId like '%{0}%' ", search.AreaId);
                }
            }

            using (IDataReader dr = DbHelper.ExecuteReader1(this._db, pageSize, pageIndex, ref recordCount, tableName, fileds.ToString(), query.ToString(), OrderByString, null))
            {
                while (dr.Read())
                {
                    EyouSoft.Model.TourStructure.MTourData model = new EyouSoft.Model.TourStructure.MTourData();
                    model.TourDataId = dr.GetInt32(dr.GetOrdinal("TourDataId"));
                    model.CompanyId = dr.GetInt32(dr.GetOrdinal("CompanyId"));
                    model.IssueTime = dr.GetDateTime(dr.GetOrdinal("IssueTime"));
                    model.AreaId = dr.GetInt32(dr.GetOrdinal("AreaId"));
                    model.RouteName = !dr.IsDBNull(dr.GetOrdinal("RouteName")) ? dr.GetString(dr.GetOrdinal("RouteName")) : null;
                    model.TourPort = !dr.IsDBNull(dr.GetOrdinal("TourPort")) ? dr.GetString(dr.GetOrdinal("TourPort")) : null;
                    model.TourDataType = (EyouSoft.Model.EnumType.TourStructure.TourDataType)dr.GetByte(dr.GetOrdinal("TourDataType"));
                    model.OperatorId = dr.GetInt32(dr.GetOrdinal("OperatorId"));
                    model.OperatorName = !dr.IsDBNull(dr.GetOrdinal("OperatorName")) ? dr.GetString(dr.GetOrdinal("OperatorName")) : null;
                    model.IsCheck = dr.GetString(dr.GetOrdinal("IsCheck")) == "1";

                    model.FileList = new List<EyouSoft.Model.TourStructure.MFile>();
                    model.FileList = !dr.IsDBNull(dr.GetOrdinal("FileInfo")) ? GetFileByXml(dr.GetString(dr.GetOrdinal("FileInfo"))) : null;

                    list.Add(model);
                }
            }

            return list;



        }

        /// <summary>
        /// 获取model
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public EyouSoft.Model.TourStructure.MTourData GetModel(int id)
        {
            EyouSoft.Model.TourStructure.MTourData model = null;
            string sql = "select *,(select * from tbl_TourDataFile where TourDataId=tbl_TourData.TourDataId for xml raw,root('Root')) as FileInfo,(select ContactName from tbl_CompanyUser where Id=tbl_TourData.OperatorId) as OperatorName from tbl_TourData where TourDataId=@TourDataId";
            DbCommand cmd = this._db.GetSqlStringCommand(sql);
            this._db.AddInParameter(cmd, "TourDataId", DbType.Int32, id);

            using (IDataReader dr = DbHelper.ExecuteReader(cmd, this._db))
            {
                if (dr.Read())
                {
                    model = new EyouSoft.Model.TourStructure.MTourData();
                    model.TourDataId = dr.GetInt32(dr.GetOrdinal("TourDataId"));
                    model.CompanyId = dr.GetInt32(dr.GetOrdinal("CompanyId"));
                    model.IssueTime = dr.GetDateTime(dr.GetOrdinal("IssueTime"));
                    model.AreaId = dr.GetInt32(dr.GetOrdinal("AreaId"));
                    model.RouteName = !dr.IsDBNull(dr.GetOrdinal("RouteName")) ? dr.GetString(dr.GetOrdinal("RouteName")) : null;
                    model.TourPort = !dr.IsDBNull(dr.GetOrdinal("TourPort")) ? dr.GetString(dr.GetOrdinal("TourPort")) : null;
                    model.TourDataType = (EyouSoft.Model.EnumType.TourStructure.TourDataType)dr.GetByte(dr.GetOrdinal("TourDataType"));
                    model.OperatorId = dr.GetInt32(dr.GetOrdinal("OperatorId"));
                    model.OperatorName=!dr.IsDBNull(dr.GetOrdinal("OperatorName"))?dr.GetString(dr.GetOrdinal("OperatorName")):null;
                    model.IsCheck = dr.GetString(dr.GetOrdinal("IsCheck")) == "1";

                    model.FileList = new List<EyouSoft.Model.TourStructure.MFile>();
                    model.FileList = !dr.IsDBNull(dr.GetOrdinal("FileInfo")) ? GetFileByXml(dr.GetString(dr.GetOrdinal("FileInfo"))) : null;
                }
            }

            return model;
        }


        /// <summary>
        /// 审核
        /// </summary>
        /// <param name="Id"></param>
        /// <param name="OperatorId"></param>
        /// <returns>0:失败 1:成功 </returns>
        public int Check(int Id, int OperatorId)
        {
            string sql = "Update tbl_TourData set IsCheck=@IsCheck,CheckId=@CheckId,CheckTime=@CheckTime where TourDataId=@TourDataId";
            DbCommand cmd = this._db.GetSqlStringCommand(sql);
            this._db.AddInParameter(cmd, "TourDataId", DbType.Int32, Id);
            this._db.AddInParameter(cmd, "CheckId", DbType.Int32, OperatorId);
            this._db.AddInParameter(cmd, "IsCheck", DbType.AnsiStringFixedLength, 1);
            this._db.AddInParameter(cmd, "CheckTime", DbType.DateTime, DateTime.Now);

            return DbHelper.ExecuteSql(cmd, this._db);


        }

        #endregion


        #region private xml
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
