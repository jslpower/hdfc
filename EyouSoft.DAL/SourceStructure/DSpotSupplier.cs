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
    public class DSpotSupplier : EyouSoft.Toolkit.DAL.DALBase, EyouSoft.IDAL.SourceStructure.ISpotSupplier
    {
        #region 初始化db
        private Database _db = null;

        /// <summary>
        /// 初始化_db
        /// </summary>
        public DSpotSupplier()
        {
            _db = base.SystemStore;
        }
        #endregion



        #region ISpotSupplier 成员

        /// <summary>
        /// 供应商景点
        /// </summary>
        /// <param name="model"></param>
        /// <returns>0:失败 1:成功</returns>
        public int Add(EyouSoft.Model.SourceStructure.MSpotSupplier model)
        {
            DbCommand cmd = this._db.GetStoredProcCommand("proc_Supplier_Spot_Add");
            this._db.AddInParameter(cmd, "Id", DbType.AnsiStringFixedLength, model.Id);
            this._db.AddInParameter(cmd, "CompanyId", DbType.Int32, model.CompanyId);
            this._db.AddInParameter(cmd, "ProvinceId", DbType.Int32, model.ProvinceId);
            this._db.AddInParameter(cmd, "CityId", DbType.Int32, model.CityId);
            this._db.AddInParameter(cmd, "UnitName", DbType.String, model.UnitName);
            this._db.AddInParameter(cmd, "SupplierType", DbType.Byte, (int)model.SupplierType);
            this._db.AddInParameter(cmd, "LicenseKey", DbType.String, model.LicenseKey);
            this._db.AddInParameter(cmd, "AgreementFile", DbType.String, model.AgreementFile);
            this._db.AddInParameter(cmd, "UnitAddress", DbType.String, model.UnitAddress);
            this._db.AddInParameter(cmd, "UnitPolicy", DbType.String, model.UnitPolicy);
            this._db.AddInParameter(cmd, "Remark", DbType.String, model.Remark);
            this._db.AddInParameter(cmd, "OperatorId", DbType.Int32, model.OperatorId);
            this._db.AddInParameter(cmd, "Star", DbType.Byte, (int)model.SpotStar);
            this._db.AddInParameter(cmd, "TourGuide", DbType.String, model.TourGuide);
            this._db.AddInParameter(cmd, "StorePrice", DbType.Currency, model.StorePrice);
            this._db.AddInParameter(cmd, "WJPrice", DbType.Currency, model.WJPrice);
            this._db.AddInParameter(cmd, "DJPrice", DbType.Currency, model.DJPrice);
            this._db.AddInParameter(cmd, "ZKPrice", DbType.Currency, model.ZKPrice);
            this._db.AddInParameter(cmd, "Contact", DbType.Xml, CreateContactXml(model.ContactList));
            this._db.AddInParameter(cmd, "File", DbType.Xml, CreateFileXml(model.FileList));

            this._db.AddOutParameter(cmd, "Result", DbType.Int32, 4);
            DbHelper.RunProcedureWithResult(cmd, _db);
            return Convert.ToInt32(_db.GetParameterValue(cmd, "Result"));
        }

        /// <summary>
        /// 修改供应商景点
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public int Update(EyouSoft.Model.SourceStructure.MSpotSupplier model)
        {
            DbCommand cmd = this._db.GetStoredProcCommand("proc_Supplier_Spot_Update");
            this._db.AddInParameter(cmd, "Id", DbType.AnsiStringFixedLength, model.Id);
            this._db.AddInParameter(cmd, "CompanyId", DbType.Int32, model.CompanyId);
            this._db.AddInParameter(cmd, "ProvinceId", DbType.Int32, model.ProvinceId);
            this._db.AddInParameter(cmd, "CityId", DbType.Int32, model.CityId);
            this._db.AddInParameter(cmd, "UnitName", DbType.String, model.UnitName);
            this._db.AddInParameter(cmd, "SupplierType", DbType.Byte, (int)model.SupplierType);
            this._db.AddInParameter(cmd, "LicenseKey", DbType.String, model.LicenseKey);
            this._db.AddInParameter(cmd, "AgreementFile", DbType.String, model.AgreementFile);
            this._db.AddInParameter(cmd, "UnitAddress", DbType.String, model.UnitAddress);
            this._db.AddInParameter(cmd, "UnitPolicy", DbType.String, model.UnitPolicy);
            this._db.AddInParameter(cmd, "Remark", DbType.String, model.Remark);

            this._db.AddInParameter(cmd, "Star", DbType.Byte, (int)model.SpotStar);
            this._db.AddInParameter(cmd, "TourGuide", DbType.String, model.TourGuide);
            this._db.AddInParameter(cmd, "StorePrice", DbType.Currency, model.StorePrice);
            this._db.AddInParameter(cmd, "WJPrice", DbType.Currency, model.WJPrice);
            this._db.AddInParameter(cmd, "DJPrice", DbType.Currency, model.DJPrice);
            this._db.AddInParameter(cmd, "ZKPrice", DbType.Currency, model.ZKPrice);
            this._db.AddInParameter(cmd, "Contact", DbType.Xml, CreateContactXml(model.ContactList));
            this._db.AddInParameter(cmd, "File", DbType.Xml, CreateFileXml(model.FileList));

            this._db.AddOutParameter(cmd, "Result", DbType.Int32, 4);
            DbHelper.RunProcedureWithResult(cmd, _db);
            return Convert.ToInt32(_db.GetParameterValue(cmd, "Result"));
        }

        /// <summary>
        /// 删除供应商景点
        /// </summary>
        /// <param name="Id"></param>
        /// <returns>0:失败 1:成功</returns>
        public int Delete(string Id)
        {
            DbCommand cmd = this._db.GetStoredProcCommand("proc_Supplier_Spot_Delete");
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
        public EyouSoft.Model.SourceStructure.MSpotSupplier GetModel(string Id)
        {
            EyouSoft.Model.SourceStructure.MSpotSupplier model = null;

            StringBuilder query = new StringBuilder();
            query.Append("select Id,CompanyId,ProvinceId,CityId,UnitName,SupplierType");
            query.Append(" ,LicenseKey,AgreementFile,UnitAddress");
            query.Append(",UnitPolicy,Remark,OperatorId,Star,TourGuide,StorePrice,WJPrice,DJPrice,ZKPrice,");
            query.Append("(select * from tbl_SupplierContact where SupplierId=tbl_CompanySupplier.Id for xml raw,root('Root'))  as ContactInfo,");
            query.Append("(select * from tbl_SupplierFile where SupplierId=tbl_CompanySupplier.Id for xml raw,root('Root')) as FileInfo");
            query.Append(" from tbl_CompanySupplier ");
            query.Append(" inner join tbl_SupplierSpot");
            query.Append(" on tbl_CompanySupplier.Id=tbl_SupplierSpot.SupplierId");
            query.Append(" where Id=@Id ");

            DbCommand cmd = this._db.GetSqlStringCommand(query.ToString());
            this._db.AddInParameter(cmd, "Id", DbType.AnsiStringFixedLength, Id);
            using (IDataReader dr = DbHelper.ExecuteReader(cmd, this._db))
            {
                while (dr.Read())
                {
                    model = new EyouSoft.Model.SourceStructure.MSpotSupplier();
                    model.Id = dr.GetString(dr.GetOrdinal("Id"));
                    model.CompanyId = dr.GetInt32(dr.GetOrdinal("CompanyId"));
                    model.ProvinceId = dr.GetInt32(dr.GetOrdinal("ProvinceId"));
                    model.CityId = dr.GetInt32(dr.GetOrdinal("CityId"));
                    model.UnitName = !dr.IsDBNull(dr.GetOrdinal("UnitName")) ? dr.GetString(dr.GetOrdinal("UnitName")) : null;
                    model.SupplierType = (EyouSoft.Model.EnumType.CompanyStructure.SupplierType)dr.GetByte(dr.GetOrdinal("SupplierType"));
                    model.LicenseKey = !dr.IsDBNull(dr.GetOrdinal("LicenseKey")) ? dr.GetString(dr.GetOrdinal("LicenseKey")) : null;
                    model.AgreementFile = !dr.IsDBNull(dr.GetOrdinal("AgreementFile")) ? dr.GetString(dr.GetOrdinal("AgreementFile")) : null;
                    model.UnitAddress = !dr.IsDBNull(dr.GetOrdinal("UnitAddress")) ? dr.GetString(dr.GetOrdinal("UnitAddress")) : null;
                    model.UnitPolicy = !dr.IsDBNull(dr.GetOrdinal("UnitPolicy")) ? dr.GetString(dr.GetOrdinal("UnitPolicy")) : null;
                    model.Remark = !dr.IsDBNull(dr.GetOrdinal("Remark")) ? dr.GetString(dr.GetOrdinal("Remark")) : null;
                    model.OperatorId = dr.GetInt32(dr.GetOrdinal("OperatorId"));

                    model.SpotStar = (EyouSoft.Model.EnumType.SourceStructure.SpotStar)dr.GetByte(dr.GetOrdinal("Star"));
                    model.TourGuide = !dr.IsDBNull(dr.GetOrdinal("TourGuide")) ? dr.GetString(dr.GetOrdinal("TourGuide")) : null;
                    model.StorePrice = dr.GetDecimal(dr.GetOrdinal("StorePrice"));
                    model.WJPrice = dr.GetDecimal(dr.GetOrdinal("WJPrice"));
                    model.DJPrice = dr.GetDecimal(dr.GetOrdinal("DJPrice"));
                    model.ZKPrice = dr.GetDecimal(dr.GetOrdinal("ZKPrice"));

                    model.ContactList = new List<EyouSoft.Model.SourceStructure.MSupplierContact>();
                    model.ContactList = !dr.IsDBNull(dr.GetOrdinal("ContactInfo")) ? GetContactByXml(dr.GetString(dr.GetOrdinal("ContactInfo"))) : null;

                    model.FileList = new List<EyouSoft.Model.SourceStructure.MFileInfo>();
                    model.FileList = !dr.IsDBNull(dr.GetOrdinal("FileInfo")) ? GetFileByXml(dr.GetString(dr.GetOrdinal("FileInfo"))) : null;
                }
            }

            return model;
        }

        /// <summary>
        /// 供应商酒店分页列表
        /// </summary>
        /// <param name="companyId"></param>
        /// <param name="pageSize"></param>
        /// <param name="pageIndex"></param>
        /// <param name="recordCount"></param>
        /// <param name="search"></param>
        /// <returns></returns>
        public IList<EyouSoft.Model.SourceStructure.MPageSpot> GetList(
                int companyId,
               int pageSize,
               int pageIndex,
               ref int recordCount,
               EyouSoft.Model.SourceStructure.MSearchSpot search)
        {
            IList<EyouSoft.Model.SourceStructure.MPageSpot> list = new List<EyouSoft.Model.SourceStructure.MPageSpot>();

            string fileds = "Id,ProvinceName,CityName,UnitName,Star,ContactInfo,StorePrice,WJPrice,DJPrice,ZKPrice,IsNew,FileInfo";

            string tableName = "view_Spot";

            string orderByString = " UnitName asc ";

            StringBuilder query = new StringBuilder();

            query.AppendFormat(" CompanyId={0} and IsDelete='0' ", companyId);

            if (search != null)
            {
                if (search.ProvinceId.HasValue)
                {
                    query.AppendFormat(" and ProvinceId={0} ", search.ProvinceId.Value);
                }

                if (search.CityId.HasValue)
                {
                    query.AppendFormat(" and CityId={0} ", search.CityId.Value);
                }

                if (search.SpotStar.HasValue)
                {
                    query.AppendFormat(" and Star={0} ", (int)search.SpotStar.Value);
                }
                if (!string.IsNullOrEmpty(search.UnitName))
                {
                    query.AppendFormat(" and UnitName like '%{0}%' ", search.UnitName);
                }
            }

            using (IDataReader dr = DbHelper.ExecuteReader1(this._db, pageSize, pageIndex, ref recordCount, tableName, fileds, query.ToString(), orderByString, null))
            {
                while (dr.Read())
                {
                    EyouSoft.Model.SourceStructure.MPageSpot model = new EyouSoft.Model.SourceStructure.MPageSpot();
                    model.Id = dr.GetString(dr.GetOrdinal("Id"));
                    model.ProvinceName = !dr.IsDBNull(dr.GetOrdinal("ProvinceName")) ? dr.GetString(dr.GetOrdinal("ProvinceName")) : null;
                    model.CityName = !dr.IsDBNull(dr.GetOrdinal("CityName")) ? dr.GetString(dr.GetOrdinal("CityName")) : null;
                    model.UnitName = !dr.IsDBNull(dr.GetOrdinal("UnitName")) ? dr.GetString(dr.GetOrdinal("UnitName")) : null;
                    model.SpotStar = (EyouSoft.Model.EnumType.SourceStructure.SpotStar)dr.GetByte(dr.GetOrdinal("Star"));

                    model.StorePrice = dr.GetDecimal(dr.GetOrdinal("StorePrice"));
                    model.WJPrice = dr.GetDecimal(dr.GetOrdinal("WJPrice"));
                    model.DJPrice = dr.GetDecimal(dr.GetOrdinal("DJPrice"));
                    model.ZKPrice = dr.GetDecimal(dr.GetOrdinal("ZKPrice"));

                    model.ContactInfo = new EyouSoft.Model.SourceStructure.MSupplierContact();
                    model.ContactInfo = !dr.IsDBNull(dr.GetOrdinal("ContactInfo")) ? GetContactByXml(dr.GetString(dr.GetOrdinal("ContactInfo")))[0] : null;

                    model.IsNew = dr.GetInt32(dr.GetOrdinal("IsNew")) == 1;


                    model.FileList = new List<EyouSoft.Model.SourceStructure.MFileInfo>();
                    model.FileList = !dr.IsDBNull(dr.GetOrdinal("FileInfo")) ? GetFileByXml(dr.GetString(dr.GetOrdinal("FileInfo"))) : null;

                    list.Add(model);
                }
            }


            return list;

        }

        #endregion


        #region private  to  xml
        /// <summary>
        /// 获取联系人价格实体
        /// </summary>
        /// <param name="list"></param>
        /// <returns></returns>
        private string CreateContactXml(IList<EyouSoft.Model.SourceStructure.MSupplierContact> list)
        {
            if (list == null) return null;
            if (list.Count == 0) return null;
            StringBuilder xmlDoc = new StringBuilder();
            xmlDoc.Append("<Root>");

            foreach (EyouSoft.Model.SourceStructure.MSupplierContact model in list)
            {
                xmlDoc.Append("<Contact ");
                xmlDoc.AppendFormat("Id  =\"{0}\" ", string.IsNullOrEmpty(model.Id) ? Guid.NewGuid().ToString() : model.Id);
                xmlDoc.AppendFormat("ContactName  =\"{0}\" ", Utils.ReplaceXmlSpecialCharacter(model.ContactName));
                xmlDoc.AppendFormat("JobTitle =\"{0}\" ", Utils.ReplaceXmlSpecialCharacter(model.JobTitle));
                xmlDoc.AppendFormat("ContactFax =\"{0}\" ", model.ContactFax);
                xmlDoc.AppendFormat("ContactTel =\"{0}\" ", model.ContactTel);
                xmlDoc.AppendFormat("ContactMobile =\"{0}\" ", model.ContactMobile);
                xmlDoc.AppendFormat("QQ =\"{0}\" ", model.QQ);
                xmlDoc.AppendFormat("Email =\"{0}\" ", model.Email);
                if (model.Birthday.HasValue)
                {
                    xmlDoc.AppendFormat("Birthday =\"{0}\" ", model.Birthday.Value);
                }
                xmlDoc.Append(" />");
            }
            xmlDoc.Append("</Root>");
            return xmlDoc.ToString();
        }

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
        /// 获取联系人
        /// </summary>
        /// <param name="xml"></param>
        /// <returns></returns>
        private IList<EyouSoft.Model.SourceStructure.MSupplierContact> GetContactByXml(string xml)
        {
            if (string.IsNullOrEmpty(xml)) return null;
            IList<EyouSoft.Model.SourceStructure.MSupplierContact> list = new List<EyouSoft.Model.SourceStructure.MSupplierContact>();
            System.Xml.Linq.XElement xRoot = System.Xml.Linq.XElement.Parse(xml);
            var xRows = Utils.GetXElements(xRoot, "row");
            foreach (var xRow in xRows)
            {
                EyouSoft.Model.SourceStructure.MSupplierContact model = new EyouSoft.Model.SourceStructure.MSupplierContact();
                model.Id = Utils.GetXAttributeValue(xRow, "Id");
                model.ContactName = Utils.GetXAttributeValue(xRow, "ContactName");
                model.JobTitle = Utils.GetXAttributeValue(xRow, "JobTitle");
                model.ContactFax = Utils.GetXAttributeValue(xRow, "ContactFax");
                model.ContactTel = Utils.GetXAttributeValue(xRow, "ContactTel");
                model.ContactMobile = Utils.GetXAttributeValue(xRow, "ContactMobile");
                model.QQ = Utils.GetXAttributeValue(xRow, "QQ");
                model.Email = Utils.GetXAttributeValue(xRow, "Email");
                model.Birthday = Utils.GetDateTimeNullable(Utils.GetXAttributeValue(xRow, "Birthday"));
                list.Add(model);
            }
            return list;
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
