using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Practices.EnterpriseLibrary.Data;
using System.Data.Common;
using System.Data;
using EyouSoft.Toolkit.DAL;
using EyouSoft.Toolkit;

namespace EyouSoft.DAL.SourceStructure
{
    public class DGuideSupplier : EyouSoft.Toolkit.DAL.DALBase, EyouSoft.IDAL.SourceStructure.IGuideSupplier
    {
        #region 初始化db
        private Database _db = null;

        /// <summary>
        /// 初始化_db
        /// </summary>
        public DGuideSupplier()
        {
            _db = base.SystemStore;
        }
        #endregion





        #region IGuideSupplier 成员
        /// <summary>
        /// 添加导游
        /// </summary>
        /// <param name="model"></param>
        /// <returns>0:添加失败 1:添加成功</returns>
        public int Add(EyouSoft.Model.SourceStructure.MGuideSupplier model)
        {
            DbCommand cmd = this._db.GetStoredProcCommand("proc_Supplier_Guide_Add");
            this._db.AddInParameter(cmd, "Id", DbType.AnsiStringFixedLength, model.Id);
            this._db.AddInParameter(cmd, "CompanyId", DbType.Int32, model.CompanyId);
            this._db.AddInParameter(cmd, "GysId", DbType.AnsiStringFixedLength, model.GysId);
            this._db.AddInParameter(cmd, "GysName", DbType.String, model.GysName);
            this._db.AddInParameter(cmd, "ProvinceId", DbType.Int32, model.ProvinceId);
            this._db.AddInParameter(cmd, "CityId", DbType.Int32, model.CityId);
            this._db.AddInParameter(cmd, "GuideName", DbType.String, model.GuideName);
            this._db.AddInParameter(cmd, "Phone", DbType.String, model.Phone);
            this._db.AddInParameter(cmd, "Birthday", DbType.DateTime, model.Birthday);
            this._db.AddInParameter(cmd, "TourTime", DbType.String, model.TourTime);
            this._db.AddInParameter(cmd, "Remark", DbType.String, model.Remark);
            this._db.AddInParameter(cmd, "Star", DbType.Byte, (int)model.GuideStar);
            this._db.AddInParameter(cmd, "Belongs", DbType.String, model.Belongs);
            this._db.AddInParameter(cmd, "OperatorId", DbType.Int32, model.OperatorId);
            this._db.AddInParameter(cmd, "File", DbType.Xml, CreateFileXml(model.FileList));

            this._db.AddOutParameter(cmd, "Result", DbType.Int32, 4);
            DbHelper.RunProcedureWithResult(cmd, _db);
            return Convert.ToInt32(_db.GetParameterValue(cmd, "Result"));
        }

        /// <summary>
        /// 修改导游
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public int Update(EyouSoft.Model.SourceStructure.MGuideSupplier model)
        {
            DbCommand cmd = this._db.GetStoredProcCommand("proc_Supplier_Guide_Update");
            this._db.AddInParameter(cmd, "Id", DbType.AnsiStringFixedLength, model.Id);
            this._db.AddInParameter(cmd, "CompanyId", DbType.Int32, model.CompanyId);
            this._db.AddInParameter(cmd, "GysId", DbType.AnsiStringFixedLength, model.GysId);
            this._db.AddInParameter(cmd, "GysName", DbType.String, model.GysName);
            this._db.AddInParameter(cmd, "ProvinceId", DbType.Int32, model.ProvinceId);
            this._db.AddInParameter(cmd, "CityId", DbType.Int32, model.CityId);
            this._db.AddInParameter(cmd, "GuideName", DbType.String, model.GuideName);
            this._db.AddInParameter(cmd, "Phone", DbType.String, model.Phone);
            this._db.AddInParameter(cmd, "Birthday", DbType.DateTime, model.Birthday);
            this._db.AddInParameter(cmd, "TourTime", DbType.String, model.TourTime);
            this._db.AddInParameter(cmd, "Remark", DbType.String, model.Remark);
            this._db.AddInParameter(cmd, "Star", DbType.Byte, (int)model.GuideStar);
            this._db.AddInParameter(cmd, "Belongs", DbType.String, model.Belongs);
            this._db.AddInParameter(cmd, "File", DbType.Xml, CreateFileXml(model.FileList));

            this._db.AddOutParameter(cmd, "Result", DbType.Int32, 4);
            DbHelper.RunProcedureWithResult(cmd, _db);
            return Convert.ToInt32(_db.GetParameterValue(cmd, "Result"));
        }

        /// <summary>
        /// 删除导游
        /// </summary>
        /// <param name="Id"></param>
        /// <returns>0:失败 1:成功 -1:确认件安排过该导游 不允许删除</returns>
        public int Delete(string Id)
        {
            DbCommand cmd = this._db.GetStoredProcCommand("proc_Supplier_Guide_Delete");
            this._db.AddInParameter(cmd, "Id", DbType.AnsiStringFixedLength, Id);

            this._db.AddOutParameter(cmd, "Result", DbType.Int32, 4);
            DbHelper.RunProcedureWithResult(cmd, _db);
            return Convert.ToInt32(_db.GetParameterValue(cmd, "Result"));

        }


        /// <summary>
        /// 获取实体
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        public EyouSoft.Model.SourceStructure.MGuideSupplier GetModel(string Id)
        {
            EyouSoft.Model.SourceStructure.MGuideSupplier model = null;

            StringBuilder query = new StringBuilder();
            query.Append("SELECT Id,CompanyId,GysId,GysName,ProvinceId,CityId,GuideName");
            query.Append(",Phone,Birthday,TourTime,Remark,Star,Belongs,OperatorId");
            query.Append(",(select * from tbl_SupplierFile where tbl_SupplierFile.SupplierId=tbl_SupplierGuide.Id for xml raw,root('Root')) as FileInfo");
            query.Append(" FROM tbl_SupplierGuide ");
            query.Append(" WHERE Id=@Id");

            DbCommand cmd = this._db.GetSqlStringCommand(query.ToString());
            this._db.AddInParameter(cmd, "Id", DbType.AnsiStringFixedLength, Id);
            using (IDataReader dr = DbHelper.ExecuteReader(cmd, this._db))
            {
                while (dr.Read())
                {
                    model = new EyouSoft.Model.SourceStructure.MGuideSupplier();
                    model.Id = dr.GetString(dr.GetOrdinal("Id"));
                    model.CompanyId = dr.GetInt32(dr.GetOrdinal("CompanyId"));
                    model.GysId = !dr.IsDBNull(dr.GetOrdinal("GysId")) ? dr.GetString(dr.GetOrdinal("GysId")) : null;
                    model.GysName = !dr.IsDBNull(dr.GetOrdinal("GysName")) ? dr.GetString(dr.GetOrdinal("GysName")) : null;
                    model.ProvinceId = !dr.IsDBNull(dr.GetOrdinal("ProvinceId")) ? dr.GetInt32(dr.GetOrdinal("ProvinceId")) : 0;
                    model.CityId = !dr.IsDBNull(dr.GetOrdinal("CityId")) ? dr.GetInt32(dr.GetOrdinal("CityId")) : 0;
                    model.GuideName = !dr.IsDBNull(dr.GetOrdinal("GuideName")) ? dr.GetString(dr.GetOrdinal("GuideName")) : null;
                    model.Phone = !dr.IsDBNull(dr.GetOrdinal("Phone")) ? dr.GetString(dr.GetOrdinal("Phone")) : null;
                    model.Birthday = !dr.IsDBNull(dr.GetOrdinal("Birthday")) ? (DateTime?)dr.GetDateTime(dr.GetOrdinal("Birthday")) : null;
                    model.TourTime = !dr.IsDBNull(dr.GetOrdinal("TourTime")) ? dr.GetString(dr.GetOrdinal("TourTime")) : null;

                    model.Remark = !dr.IsDBNull(dr.GetOrdinal("Remark")) ? dr.GetString(dr.GetOrdinal("Remark")) : null;
                    model.GuideStar = (EyouSoft.Model.EnumType.SourceStructure.GuideStar)dr.GetByte(dr.GetOrdinal("Star"));
                    model.Belongs = !dr.IsDBNull(dr.GetOrdinal("Belongs")) ? dr.GetString(dr.GetOrdinal("Belongs")) : null;
                    model.OperatorId = dr.GetInt32(dr.GetOrdinal("OperatorId"));

                    model.FileList = new List<EyouSoft.Model.SourceStructure.MFileInfo>();
                    model.FileList = !dr.IsDBNull(dr.GetOrdinal("FileInfo")) ? GetFileByXml(dr.GetString(dr.GetOrdinal("FileInfo"))) : null;
                }
            }

            return model;
        }

        /// <summary>
        /// 根据公司编号 导游名称获取导游集合
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        public IList<EyouSoft.Model.SourceStructure.MGuideSupplier> GetList(int companyId, string GuideName)
        {
            IList<EyouSoft.Model.SourceStructure.MGuideSupplier> list = new List<EyouSoft.Model.SourceStructure.MGuideSupplier>();

            StringBuilder query = new StringBuilder();
            query.Append("SELECT Id,CompanyId,ProvinceId,CityId,GysId,GysName,GuideName");
            query.Append(",Phone,Birthday,TourTime,Remark,Star,Belongs,OperatorId");
            query.Append(",(select * from tbl_SupplierFile where tbl_SupplierFile.SupplierId=tbl_SupplierGuide.Id for xml raw,root('Root')) as FileInfo");
            query.Append(" FROM tbl_SupplierGuide ");
            query.AppendFormat(" WHERE CompanyId={0} and GuideName like '%{1}%'", companyId, GuideName);
            DbCommand cmd = this._db.GetSqlStringCommand(query.ToString());


            using (IDataReader dr = DbHelper.ExecuteReader(cmd, this._db))
            {
                while (dr.Read())
                {
                    EyouSoft.Model.SourceStructure.MGuideSupplier model = new EyouSoft.Model.SourceStructure.MGuideSupplier();

                    model.Id = dr.GetString(dr.GetOrdinal("Id"));
                    model.CompanyId = dr.GetInt32(dr.GetOrdinal("CompanyId"));
                    model.GysId = !dr.IsDBNull(dr.GetOrdinal("GysId")) ? dr.GetString(dr.GetOrdinal("GysId")) : null;
                    model.GysName = !dr.IsDBNull(dr.GetOrdinal("GysName")) ? dr.GetString(dr.GetOrdinal("GysName")) : null;
                    model.ProvinceId = !dr.IsDBNull(dr.GetOrdinal("ProvinceId")) ? dr.GetInt32(dr.GetOrdinal("ProvinceId")) : 0;
                    model.CityId = !dr.IsDBNull(dr.GetOrdinal("CityId")) ? dr.GetInt32(dr.GetOrdinal("CityId")) : 0;
                    model.GuideName = !dr.IsDBNull(dr.GetOrdinal("GuideName")) ? dr.GetString(dr.GetOrdinal("GuideName")) : null;
                    model.Phone = !dr.IsDBNull(dr.GetOrdinal("Phone")) ? dr.GetString(dr.GetOrdinal("Phone")) : null;
                    model.Birthday = !dr.IsDBNull(dr.GetOrdinal("Birthday")) ? (DateTime?)dr.GetDateTime(dr.GetOrdinal("Birthday")) : null;
                    model.TourTime = !dr.IsDBNull(dr.GetOrdinal("TourTime")) ? dr.GetString(dr.GetOrdinal("TourTime")) : null;

                    model.Remark = !dr.IsDBNull(dr.GetOrdinal("Remark")) ? dr.GetString(dr.GetOrdinal("Remark")) : null;
                    model.GuideStar = (EyouSoft.Model.EnumType.SourceStructure.GuideStar)dr.GetByte(dr.GetOrdinal("Star"));
                    model.Belongs = !dr.IsDBNull(dr.GetOrdinal("Belongs")) ? dr.GetString(dr.GetOrdinal("Belongs")) : null;
                    model.OperatorId = dr.GetInt32(dr.GetOrdinal("OperatorId"));

                    model.FileList = new List<EyouSoft.Model.SourceStructure.MFileInfo>();
                    model.FileList = !dr.IsDBNull(dr.GetOrdinal("FileInfo")) ? GetFileByXml(dr.GetString(dr.GetOrdinal("FileInfo"))) : null;

                    list.Add(model);
                }
            }

            return list;
        }

        /// <summary>
        /// 获取分页列表
        /// </summary>
        /// <param name="companyId"></param>
        /// <param name="pageSize"></param>
        /// <param name="pageIndex"></param>
        /// <param name="recordCount"></param>
        /// <param name="GuideName"></param>
        /// <returns></returns>
        public IList<EyouSoft.Model.SourceStructure.MPageGuide> GetList(int companyId,
         int pageSize,
         int pageIndex,
         ref int recordCount,
         string GuideName, string GysName)
        {
            IList<EyouSoft.Model.SourceStructure.MPageGuide> list = new List<EyouSoft.Model.SourceStructure.MPageGuide>();

            string tableName = "tbl_SupplierGuide";

            string OrderByString = " GuideName asc,IssueTime desc";

            StringBuilder fileds = new StringBuilder();
            fileds.Append("Id,CompanyId,GuideName,GysId,GysName");
            fileds.Append(",(select ProvinceName from tbl_CompanyProvince where ID=tbl_SupplierGuide.ProvinceId) as ProvinceName");
            fileds.Append(",(select CityName from tbl_CompanyCity where ID=tbl_SupplierGuide.CityId) as CityName");
            fileds.Append(",Phone,Birthday,TourTime,Remark,Star,Belongs,OperatorId");
            fileds.Append(",(select * from tbl_SupplierFile where SupplierId=tbl_SupplierGuide.Id for xml raw,root('Root')) as FileInfo");
            fileds.Append(",(select count(1) from tbl_GuideFanKui where GuideId=tbl_SupplierGuide.Id) as FanKuiNum");


            StringBuilder query = new StringBuilder();
            query.AppendFormat("CompanyId={0} and IsDelete='0' ", companyId);
            //导游名称
            if (!string.IsNullOrEmpty(GuideName))
            {
                query.AppendFormat(" and GuideName like '%{0}%'", GuideName);

            }
            //旅行社名称
            if (!string.IsNullOrEmpty(GysName))
            {
                query.AppendFormat(" and GysName like '%{0}%'", GysName);
            }
            using (IDataReader dr = DbHelper.ExecuteReader1(this._db, pageSize, pageIndex, ref recordCount, tableName, fileds.ToString(), query.ToString(), OrderByString, null))
            {
                while (dr.Read())
                {
                    EyouSoft.Model.SourceStructure.MPageGuide model = new EyouSoft.Model.SourceStructure.MPageGuide();
                    model.Id = dr.GetString(dr.GetOrdinal("Id"));
                    model.CompanyId = dr.GetInt32(dr.GetOrdinal("CompanyId"));
                    model.GysId = !dr.IsDBNull(dr.GetOrdinal("GysId")) ? dr.GetString(dr.GetOrdinal("GysId")) : null;
                    model.GysName = !dr.IsDBNull(dr.GetOrdinal("GysName")) ? dr.GetString(dr.GetOrdinal("GysName")) : null;
                    model.ProvinceName = !dr.IsDBNull(dr.GetOrdinal("ProvinceName")) ? dr.GetString(dr.GetOrdinal("ProvinceName")) : null;
                    model.CityName = !dr.IsDBNull(dr.GetOrdinal("CityName")) ? dr.GetString(dr.GetOrdinal("CityName")) : null;
                    model.GuideName = !dr.IsDBNull(dr.GetOrdinal("GuideName")) ? dr.GetString(dr.GetOrdinal("GuideName")) : null;
                    model.Phone = !dr.IsDBNull(dr.GetOrdinal("Phone")) ? dr.GetString(dr.GetOrdinal("Phone")) : null;
                    model.Birthday = !dr.IsDBNull(dr.GetOrdinal("Birthday")) ? (DateTime?)dr.GetDateTime(dr.GetOrdinal("Birthday")) : null;
                    model.TourTime = !dr.IsDBNull(dr.GetOrdinal("TourTime")) ? dr.GetString(dr.GetOrdinal("TourTime")) : null;

                    model.Remark = !dr.IsDBNull(dr.GetOrdinal("Remark")) ? dr.GetString(dr.GetOrdinal("Remark")) : null;
                    model.GuideStar = (EyouSoft.Model.EnumType.SourceStructure.GuideStar)dr.GetByte(dr.GetOrdinal("Star"));
                    model.Belongs = !dr.IsDBNull(dr.GetOrdinal("Phone")) ? dr.GetString(dr.GetOrdinal("Phone")) : null;
                    model.OperatorId = dr.GetInt32(dr.GetOrdinal("OperatorId"));

                    model.FanKuiNum = dr.GetInt32(dr.GetOrdinal("FanKuiNum"));

                    model.FileList = new List<EyouSoft.Model.SourceStructure.MFileInfo>();
                    model.FileList = !dr.IsDBNull(dr.GetOrdinal("FileInfo")) ? GetFileByXml(dr.GetString(dr.GetOrdinal("FileInfo"))) : null;


                    list.Add(model);
                }

            }

            return list;

        }

        /// <summary>
        /// 添加导游反馈信息
        /// </summary>
        /// <param name="model"></param>
        /// <returns>1：成功 0：失败</returns>
        public int Add(EyouSoft.Model.SourceStructure.MGuideFanKui model)
        {

            string sql = "INSERT INTO tbl_GuideFanKui(GuideId,FanKuiType,FanKuiTime,FanKuiRemark) VALUES(@GuideId,@FanKuiType, @FanKuiTime, @FanKuiRemark)";
            DbCommand cmd = this._db.GetSqlStringCommand(sql);
            this._db.AddInParameter(cmd, "GuideId", DbType.AnsiStringFixedLength, model.GuideId);
            this._db.AddInParameter(cmd, "FanKuiType", DbType.Byte, (int)model.FanKuiType);
            this._db.AddInParameter(cmd, "FanKuiTime", DbType.DateTime, model.FanKuiTime.Value);
            this._db.AddInParameter(cmd, "FanKuiRemark", DbType.String, model.FanKuiRemark);

            return DbHelper.ExecuteSql(cmd, this._db);

        }

        /// <summary>
        /// 修改导游反馈信息
        /// </summary>
        /// <param name="model"></param>
        /// <returns>1：成功 0：失败</returns>
        public int Update(EyouSoft.Model.SourceStructure.MGuideFanKui model)
        {
            string sql = "UPDATE tbl_GuideFanKui SET FanKuiType = @FanKuiType,FanKuiTime = @FanKuiTime,FanKuiRemark = @FanKuiRemark WHERE Id = @Id";
            DbCommand cmd = this._db.GetSqlStringCommand(sql);
            this._db.AddInParameter(cmd, "Id", DbType.Int32, model.Id);
            this._db.AddInParameter(cmd, "FanKuiType", DbType.Byte, (int)model.FanKuiType);
            this._db.AddInParameter(cmd, "FanKuiTime", DbType.DateTime, model.FanKuiTime.Value);
            this._db.AddInParameter(cmd, "FanKuiRemark", DbType.String, model.FanKuiRemark);

            return DbHelper.ExecuteSql(cmd, this._db);
        }

        /// <summary>
        /// 删除导游反馈
        /// </summary>
        /// <param name="Id"></param>
        /// <returns>1：成功 0：失败</returns>
        public int Delete(int Id)
        {
            string sql = "Delete from  tbl_GuideFanKui where Id=@Id";
            DbCommand cmd = this._db.GetSqlStringCommand(sql);
            this._db.AddInParameter(cmd, "Id", DbType.Int32, Id);
            return DbHelper.ExecuteSql(cmd, this._db);
        }

        /// <summary>
        /// 获取反馈的集合
        /// </summary>
        /// <param name="GuidId"></param>
        /// <returns></returns>
        public IList<EyouSoft.Model.SourceStructure.MGuideFanKui> GetList(string GuideId)
        {
            IList<EyouSoft.Model.SourceStructure.MGuideFanKui> list = new List<EyouSoft.Model.SourceStructure.MGuideFanKui>();

            string sql = "Select * from  tbl_GuideFanKui where GuideId=@GuideId";
            DbCommand cmd = this._db.GetSqlStringCommand(sql);
            this._db.AddInParameter(cmd, "GuideId", DbType.AnsiStringFixedLength, GuideId);

            using (IDataReader dr = DbHelper.ExecuteReader(cmd, this._db))
            {
                EyouSoft.Model.SourceStructure.MGuideFanKui model = new EyouSoft.Model.SourceStructure.MGuideFanKui();
                model.Id = dr.GetInt32(dr.GetOrdinal("Id"));
                model.GuideId = dr.GetString(dr.GetOrdinal("GuideId"));
                model.FanKuiType = (EyouSoft.Model.EnumType.SourceStructure.FanKuiType)dr.GetByte(dr.GetOrdinal("FanKuiType"));
                model.FanKuiTime = dr.GetDateTime(dr.GetOrdinal("FanKuiTime"));
                model.FanKuiRemark = !dr.IsDBNull(dr.GetOrdinal("FanKuiRemark")) ? dr.GetString(dr.GetOrdinal("FanKuiRemark")) : null;
                list.Add(model);
            }

            return list;
        }

        /// <summary>
        /// 获取分页列表
        /// </summary>
        /// <param name="companyId"></param>
        /// <param name="pageSize"></param>
        /// <param name="pageIndex"></param>
        /// <param name="recordCount"></param>
        /// <param name="GuideName"></param>
        /// <returns></returns>
        public IList<EyouSoft.Model.SourceStructure.MGuideFanKui> GetList(
         string GuideId,
         int pageSize,
         int pageIndex,
         ref int recordCount)
        {
            IList<EyouSoft.Model.SourceStructure.MGuideFanKui> list = new List<EyouSoft.Model.SourceStructure.MGuideFanKui>();

            string fileds = "Id,GuideId,FanKuiType,FanKuiTime,FanKuiRemark";

            string orderByString = "FanKuiTime desc";

            string tableName = "tbl_GuideFanKui";

            string query = string.Format(" GuideId='{0}' ", GuideId); ;

            using (IDataReader dr = DbHelper.ExecuteReader1(this._db, pageSize, pageIndex, ref recordCount, tableName, fileds, query, orderByString, null))
            {
                while (dr.Read())
                {
                    EyouSoft.Model.SourceStructure.MGuideFanKui model = new EyouSoft.Model.SourceStructure.MGuideFanKui();
                    model.Id = dr.GetInt32(dr.GetOrdinal("Id"));
                    model.GuideId = dr.GetString(dr.GetOrdinal("GuideId"));
                    model.FanKuiType = (EyouSoft.Model.EnumType.SourceStructure.FanKuiType)dr.GetByte(dr.GetOrdinal("FanKuiType"));
                    model.FanKuiTime = dr.GetDateTime(dr.GetOrdinal("FanKuiTime"));
                    model.FanKuiRemark = !dr.IsDBNull(dr.GetOrdinal("FanKuiRemark")) ? dr.GetString(dr.GetOrdinal("FanKuiRemark")) : null;
                    list.Add(model);
                }
            }

            return list;
        }




        #endregion




        #region private to xml
        /// <summary>
        /// 获取附件信息
        /// </summary>
        /// <param name="list"></param>
        /// <param name="TourId"></param>
        /// <returns></returns>
        private string CreateFileXml(IList<EyouSoft.Model.SourceStructure.MFileInfo> list)
        {
            if (list == null) return null;
            if (list.Count == 0) return null;
            StringBuilder xmlDoc = new StringBuilder();
            xmlDoc.Append("<Root>");
            foreach (EyouSoft.Model.SourceStructure.MFileInfo model in list)
            {
                xmlDoc.Append("<File ");
                xmlDoc.AppendFormat("FileName=\"{0}\" ", model.FileName);
                xmlDoc.AppendFormat("FilePath=\"{0}\" ", model.FilePath);
                xmlDoc.AppendFormat("FileMode=\"{0}\" ", (int)model.FileMode);
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
        private IList<EyouSoft.Model.SourceStructure.MFileInfo> GetFileByXml(string xml)
        {
            if (string.IsNullOrEmpty(xml)) return null;
            IList<EyouSoft.Model.SourceStructure.MFileInfo> list = new List<EyouSoft.Model.SourceStructure.MFileInfo>();
            System.Xml.Linq.XElement xRoot = System.Xml.Linq.XElement.Parse(xml);
            var xRows = Utils.GetXElements(xRoot, "row");
            foreach (var xRow in xRows)
            {
                EyouSoft.Model.SourceStructure.MFileInfo model = new EyouSoft.Model.SourceStructure.MFileInfo();
                model.FileName = Utils.GetXAttributeValue(xRow, "FileName");
                model.FilePath = Utils.GetXAttributeValue(xRow, "FilePath");
                model.FileMode = (EyouSoft.Model.EnumType.SourceStructure.FileMode)Utils.GetInt(Utils.GetXAttributeValue(xRow, "FileMode"));
                list.Add(model);
            }
            return list;
        }

        #endregion
    }
}
