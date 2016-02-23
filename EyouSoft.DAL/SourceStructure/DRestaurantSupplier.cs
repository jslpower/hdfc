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
    public class DRestaurantSupplier : EyouSoft.Toolkit.DAL.DALBase, EyouSoft.IDAL.SourceStructure.IRestaurantSupplier
    {
        #region 初始化db
        private Database _db = null;

        /// <summary>
        /// 初始化_db
        /// </summary>
        public DRestaurantSupplier()
        {
            _db = base.SystemStore;
        }
        #endregion




        #region IRestaurantSupplier 成员

        /// <summary>
        /// 供应商餐饮
        /// </summary>
        /// <param name="model"></param>
        /// <returns>0:失败 1:成功</returns>
        public int Add(EyouSoft.Model.SourceStructure.MRestaurantSupplier model)
        {
            DbCommand cmd = this._db.GetStoredProcCommand("proc_Supplier_Restaurant_Add");
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

            this._db.AddInParameter(cmd, "Introduce", DbType.String, model.Introduce);
            this._db.AddInParameter(cmd, "Cuisine ", DbType.Byte, (int)model.Cuisine);
            this._db.AddInParameter(cmd, "Contact", DbType.Xml, CreateContactXml(model.ContactList));
            this._db.AddInParameter(cmd, "File", DbType.Xml, CreateFileXml(model.FileList));

            this._db.AddOutParameter(cmd, "Result", DbType.Int32, 4);
            DbHelper.RunProcedureWithResult(cmd, _db);
            return Convert.ToInt32(_db.GetParameterValue(cmd, "Result"));
        }

        /// <summary>
        /// 修改供应商餐饮
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public int Update(EyouSoft.Model.SourceStructure.MRestaurantSupplier model)
        {
            DbCommand cmd = this._db.GetStoredProcCommand("proc_Supplier_Restaurant_Update");
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
            this._db.AddInParameter(cmd, "Introduce", DbType.String, model.Introduce);
            this._db.AddInParameter(cmd, "Cuisine ", DbType.Byte, (int)model.Cuisine);


            this._db.AddInParameter(cmd, "Contact", DbType.Xml, CreateContactXml(model.ContactList));
            this._db.AddInParameter(cmd, "File", DbType.Xml, CreateFileXml(model.FileList));

            this._db.AddOutParameter(cmd, "Result", DbType.Int32, 4);
            DbHelper.RunProcedureWithResult(cmd, _db);
            return Convert.ToInt32(_db.GetParameterValue(cmd, "Result"));
        }

        /// <summary>
        /// 删除供应商餐饮
        /// </summary>
        /// <param name="Id"></param>
        /// <returns>0:失败 1:成功</returns>
        public int Delete(string Id)
        {
            DbCommand cmd = this._db.GetStoredProcCommand("proc_Supplier_Restaurant_delete");
            this._db.AddInParameter(cmd, "Id", DbType.AnsiStringFixedLength, Id);

            this._db.AddOutParameter(cmd, "Result", DbType.Int32, 4);
            DbHelper.RunProcedureWithResult(cmd, _db);
            return Convert.ToInt32(_db.GetParameterValue(cmd, "Result"));
        }

        /// <summary>
        /// 获取model
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        public EyouSoft.Model.SourceStructure.MRestaurantSupplier GetModel(string Id)
        {
            EyouSoft.Model.SourceStructure.MRestaurantSupplier model = null;

            StringBuilder query = new StringBuilder();
            query.Append("SELECT Id,CompanyId,ProvinceId,CityId,UnitName");
            query.Append("      ,SupplierType,LicenseKey,AgreementFile");
            query.Append("      ,UnitAddress,UnitPolicy,Remark,OperatorId,Introduce,Cuisine,");
            query.Append("(select * from tbl_SupplierContact where SupplierId=tbl_CompanySupplier.Id for xml raw,root('Root'))as ContactInfo,");
            query.Append("(select * from tbl_SupplierFile where SupplierId=tbl_CompanySupplier.Id for xml raw,root('Root'))as FileInfo");
            query.Append("  FROM tbl_CompanySupplier inner join tbl_SupplierRestaurant ");
            query.Append("  on tbl_CompanySupplier.Id=tbl_SupplierRestaurant.SupplierId");
            query.Append(" Where Id=@Id");

            DbCommand cmd = this._db.GetSqlStringCommand(query.ToString());
            this._db.AddInParameter(cmd, "Id", DbType.AnsiStringFixedLength, Id);

            using (IDataReader dr = DbHelper.ExecuteReader(cmd, this._db))
            {
                while (dr.Read())
                {
                    model = new EyouSoft.Model.SourceStructure.MRestaurantSupplier();
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

                    model.Cuisine = (EyouSoft.Model.EnumType.SourceStructure.Cuisine)dr.GetByte(dr.GetOrdinal("Cuisine"));
                    model.Introduce = !dr.IsDBNull(dr.GetOrdinal("Introduce")) ? dr.GetString(dr.GetOrdinal("Introduce")) : null;

                    model.ContactList = new List<EyouSoft.Model.SourceStructure.MSupplierContact>();
                    model.ContactList = !dr.IsDBNull(dr.GetOrdinal("ContactInfo")) ? GetContactByXml(dr.GetString(dr.GetOrdinal("ContactInfo"))) : null;

                    model.FileList = new List<EyouSoft.Model.SourceStructure.MFileInfo>();
                    model.FileList = !dr.IsDBNull(dr.GetOrdinal("FileInfo")) ? GetFileByXml(dr.GetString(dr.GetOrdinal("FileInfo"))) : null;

                }

            }
            return model;
        }

        /// <summary>
        /// 获取分页信息
        /// </summary>
        /// <param name="companyId"></param>
        /// <param name="pageSize"></param>
        /// <param name="pageIndex"></param>
        /// <param name="recordCount"></param>
        /// <param name="search"></param>
        /// <returns></returns>
        public IList<EyouSoft.Model.SourceStructure.MPageRestaurant> GetList(int companyId, int pageSize, int pageIndex, ref int recordCount, EyouSoft.Model.SourceStructure.MSearchRestaurant search)
        {
            IList<EyouSoft.Model.SourceStructure.MPageRestaurant> list = new List<EyouSoft.Model.SourceStructure.MPageRestaurant>();

            string fileds = "Id,ProvinceName,CityName,UnitName,Cuisine,ContactInfo,FileInfo";

            string tableName = "view_Restaurant";

            string orderByString = " UnitName asc ";

            StringBuilder query = new StringBuilder();

            query.AppendFormat(" CompanyId={0}  and IsDelete='0' ", companyId);

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

                if (search.Cuisine.HasValue)
                {
                    query.AppendFormat(" and Cuisine={0} ", (int)search.Cuisine.Value);
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
                    EyouSoft.Model.SourceStructure.MPageRestaurant model = new EyouSoft.Model.SourceStructure.MPageRestaurant();
                    model.Id = dr.GetString(dr.GetOrdinal("Id"));
                    model.ProvinceName = !dr.IsDBNull(dr.GetOrdinal("ProvinceName")) ? dr.GetString(dr.GetOrdinal("ProvinceName")) : null;
                    model.CityName = !dr.IsDBNull(dr.GetOrdinal("CityName")) ? dr.GetString(dr.GetOrdinal("CityName")) : null;
                    model.UnitName = !dr.IsDBNull(dr.GetOrdinal("UnitName")) ? dr.GetString(dr.GetOrdinal("UnitName")) : null;
                    model.Cuisine = (EyouSoft.Model.EnumType.SourceStructure.Cuisine)dr.GetByte(dr.GetOrdinal("Cuisine"));

                    model.ContactInfo = new EyouSoft.Model.SourceStructure.MSupplierContact();
                    model.ContactInfo = !dr.IsDBNull(dr.GetOrdinal("ContactInfo")) ? GetContactByXml(dr.GetString(dr.GetOrdinal("ContactInfo")))[0] : null;


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
