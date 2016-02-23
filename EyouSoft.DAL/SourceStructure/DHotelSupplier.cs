using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Practices.EnterpriseLibrary.Data;
using EyouSoft.Toolkit;
using System.Data.Common;
using System.Data;
using EyouSoft.Toolkit.DAL;

namespace EyouSoft.DAL.SourceStructure
{
    public class DHotelSupplier : EyouSoft.Toolkit.DAL.DALBase, EyouSoft.IDAL.SourceStructure.IHotelSupplier
    {
        #region 初始化db
        private Database _db = null;

        /// <summary>
        /// 初始化_db
        /// </summary>
        public DHotelSupplier()
        {
            _db = base.SystemStore;
        }
        #endregion




        #region IHotelSupplier 成员

        /// <summary>
        /// 添加供应商酒店
        /// </summary>
        /// <param name="model"></param>
        /// <returns>0:添加失败 1:添加成功</returns>
        public int Add(EyouSoft.Model.SourceStructure.MHotelSupplier model)
        {
            DbCommand cmd = this._db.GetStoredProcCommand("proc_Supplier_Hotel_Add");
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
            this._db.AddInParameter(cmd, "Star", DbType.Byte, (int)model.HotelStar);
            this._db.AddInParameter(cmd, "Introduce", DbType.String, model.Introduce);
            this._db.AddInParameter(cmd, "RomePrice", DbType.Xml, CreateRoomPriceXml(model.RomePriceList));
            this._db.AddInParameter(cmd, "Contact", DbType.Xml, CreateContactXml(model.ContactList));
            this._db.AddInParameter(cmd, "File", DbType.Xml, CreateFileXml(model.FileList));

            this._db.AddOutParameter(cmd, "Result", DbType.Int32, 4);
            DbHelper.RunProcedureWithResult(cmd, _db);
            return Convert.ToInt32(_db.GetParameterValue(cmd, "Result"));
        }

        /// <summary>
        /// 修改供应商酒店
        /// </summary>
        /// <param name="model"></param>
        /// <returns>0:失败 1:成功</returns>
        public int Update(EyouSoft.Model.SourceStructure.MHotelSupplier model)
        {
            DbCommand cmd = this._db.GetStoredProcCommand("proc_Supplier_Hotel_Update");
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
            this._db.AddInParameter(cmd, "Star", DbType.Byte, (int)model.HotelStar);
            this._db.AddInParameter(cmd, "Introduce", DbType.String, model.Introduce);
            this._db.AddInParameter(cmd, "RomePrice", DbType.Xml, CreateRoomPriceXml(model.RomePriceList));
            this._db.AddInParameter(cmd, "Contact", DbType.Xml, CreateContactXml(model.ContactList));
            this._db.AddInParameter(cmd, "File", DbType.Xml, CreateFileXml(model.FileList));

            this._db.AddOutParameter(cmd, "Result", DbType.Int32, 4);
            DbHelper.RunProcedureWithResult(cmd, _db);
            return Convert.ToInt32(_db.GetParameterValue(cmd, "Result"));
        }

        /// <summary>
        /// 删除供应商酒店
        /// </summary>
        /// <param name="Id"></param>
        /// <returns>0:失败 1:成功</returns>
        public int Delete(string Id)
        {
            DbCommand cmd = this._db.GetStoredProcCommand("proc_Supplier_Hotel_Delete");
            this._db.AddInParameter(cmd, "Id", DbType.AnsiStringFixedLength, Id);

            this._db.AddOutParameter(cmd, "Result", DbType.Int32, 4);
            DbHelper.RunProcedureWithResult(cmd, _db);
            return Convert.ToInt32(_db.GetParameterValue(cmd, "Result"));
        }

        /// <summary>
        /// 获取Model
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        public EyouSoft.Model.SourceStructure.MHotelSupplier GetModel(string Id)
        {
            EyouSoft.Model.SourceStructure.MHotelSupplier model = null;

            StringBuilder query = new StringBuilder();
            query.Append("SELECT Id,CompanyId,ProvinceId,CityId,UnitName");
            query.Append(" ,SupplierType,LicenseKey,AgreementFile");
            query.Append(" ,UnitAddress,UnitPolicy,Remark,OperatorId,Star,Introduce");
            query.Append(" ,(select * from tbl_SupplierHotelRoomType ");
            query.Append(" where tbl_SupplierHotelRoomType.SupplierId=tbl_CompanySupplier.Id ");
            query.Append(" for xml raw,root('Root')) as RoomInfo,");
            query.Append(" (select * from tbl_SupplierContact");
            query.Append("  where tbl_SupplierContact.SupplierId=tbl_CompanySupplier.Id");
            query.Append(" for xml raw,root('Root')) as ContactInfo,");
            query.Append(" (select * from tbl_SupplierFile ");
            query.Append(" where tbl_SupplierFile.SupplierId=tbl_CompanySupplier.Id");
            query.Append(" for xml raw,root('Root')) as FileInfo");
            query.Append(" FROM tbl_CompanySupplier inner join tbl_SupplierHotel");
            query.Append(" on tbl_CompanySupplier.Id=tbl_SupplierHotel.SupplierId ");
            query.Append(" where Id=@Id");

            DbCommand cmd = this._db.GetSqlStringCommand(query.ToString());
            this._db.AddInParameter(cmd, "Id", DbType.AnsiStringFixedLength, Id);

            using (IDataReader dr = DbHelper.ExecuteReader(cmd, this._db))
            {
                while (dr.Read())
                {
                    model = new EyouSoft.Model.SourceStructure.MHotelSupplier();
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
                    model.HotelStar = (EyouSoft.Model.EnumType.SourceStructure.HotelStar)dr.GetByte(dr.GetOrdinal("Star"));
                    model.Introduce = !dr.IsDBNull(dr.GetOrdinal("Introduce")) ? dr.GetString(dr.GetOrdinal("Introduce")) : null;

                    model.RomePriceList = new List<EyouSoft.Model.SourceStructure.MHotelRomePrice>();
                    model.RomePriceList = !dr.IsDBNull(dr.GetOrdinal("RoomInfo")) ? GetRomePriceByXml(dr.GetString(dr.GetOrdinal("RoomInfo"))) : null;

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
        public IList<EyouSoft.Model.SourceStructure.MPageHotel> GetList(
                int companyId,
               int pageSize,
               int pageIndex,
               ref int recordCount,
               EyouSoft.Model.SourceStructure.MSearchHotel search)
        {
            IList<EyouSoft.Model.SourceStructure.MPageHotel> list = new List<EyouSoft.Model.SourceStructure.MPageHotel>();

            string fileds = "Id,ProvinceName,CityName,UnitName,Star,ContactInfo,FileInfo";

            string tableName = "view_Hotel";

            string orderByString = " UnitName asc ";

            StringBuilder query = new StringBuilder();

            query.AppendFormat(" CompanyId={0} and IsDelete='0' and SupplierType={1} ", companyId, (int)EyouSoft.Model.EnumType.CompanyStructure.SupplierType.酒店);

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

                if (search.HotelStar.HasValue)
                {
                    query.AppendFormat(" and Star={0} ", (int)search.HotelStar.Value);
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
                    EyouSoft.Model.SourceStructure.MPageHotel model = new EyouSoft.Model.SourceStructure.MPageHotel();
                    model.Id = dr.GetString(dr.GetOrdinal("Id"));
                    model.ProvinceName = !dr.IsDBNull(dr.GetOrdinal("ProvinceName")) ? dr.GetString(dr.GetOrdinal("ProvinceName")) : null;
                    model.CityName = !dr.IsDBNull(dr.GetOrdinal("CityName")) ? dr.GetString(dr.GetOrdinal("CityName")) : null;
                    model.UnitName = !dr.IsDBNull(dr.GetOrdinal("UnitName")) ? dr.GetString(dr.GetOrdinal("UnitName")) : null;
                    model.HotelStar = (EyouSoft.Model.EnumType.SourceStructure.HotelStar)dr.GetByte(dr.GetOrdinal("Star"));

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
        /// 获取房型价格信息
        /// </summary>
        /// <param name="list"></param>
        /// <returns></returns>
        private string CreateRoomPriceXml(IList<EyouSoft.Model.SourceStructure.MHotelRomePrice> list)
        {
            if (list == null) return null;
            if (list.Count == 0) return null;
            StringBuilder xmlDoc = new StringBuilder();
            xmlDoc.Append("<Root>");

            foreach (EyouSoft.Model.SourceStructure.MHotelRomePrice model in list)
            {
                xmlDoc.Append("<RomePrice ");
                xmlDoc.AppendFormat("Name =\"{0}\" ", Utils.ReplaceXmlSpecialCharacter(model.Name));
                xmlDoc.AppendFormat("SellingPrice =\"{0}\" ", model.SellingPrice);
                xmlDoc.AppendFormat("AccountingPrice =\"{0}\" ", model.AccountingPrice);
                xmlDoc.AppendFormat("IsBreakfast  =\"{0}\" ", model.IsBreakfast ? 1 : 0);
                xmlDoc.Append(" />");
            }

            xmlDoc.Append("</Root>");
            return xmlDoc.ToString();
        }
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
        /// 获取酒店房型的价格
        /// </summary>
        /// <param name="xml"></param>
        /// <returns></returns>
        private IList<EyouSoft.Model.SourceStructure.MHotelRomePrice> GetRomePriceByXml(string xml)
        {
            if (string.IsNullOrEmpty(xml)) return null;
            IList<EyouSoft.Model.SourceStructure.MHotelRomePrice> list = new List<EyouSoft.Model.SourceStructure.MHotelRomePrice>();
            System.Xml.Linq.XElement xRoot = System.Xml.Linq.XElement.Parse(xml);
            var xRows = Utils.GetXElements(xRoot, "row");
            foreach (var xRow in xRows)
            {
                EyouSoft.Model.SourceStructure.MHotelRomePrice model = new EyouSoft.Model.SourceStructure.MHotelRomePrice();
                model.Name = Utils.GetXAttributeValue(xRow, "Name");
                model.SellingPrice = Utils.GetDecimal(Utils.GetXAttributeValue(xRow, "SellingPrice"));
                model.AccountingPrice = Utils.GetDecimal(Utils.GetXAttributeValue(xRow, "AccountingPrice"));
                model.IsBreakfast = Utils.GetXAttributeValue(xRow, "IsBreakfast") == "1";
                list.Add(model);
            }
            return list;
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
