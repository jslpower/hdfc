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
    public class DSupplier : EyouSoft.Toolkit.DAL.DALBase, EyouSoft.IDAL.SourceStructure.ISupplier
    {
        #region 初始化db
        private Database _db = null;

        /// <summary>
        /// 初始化_db
        /// </summary>
        public DSupplier()
        {
            _db = base.SystemStore;
        }
        #endregion



        #region ISupplier 成员

        /// <summary>
        /// 添加地接、机票的 供应商
        /// </summary>
        /// <param name="model"></param>
        /// <returns>1:成功 0：失败</returns>
        public int Add(EyouSoft.Model.SourceStructure.MSupplier model)
        {
            DbCommand cmd = this._db.GetStoredProcCommand("proc_Supplier_Add");
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


            this._db.AddInParameter(cmd, "Contact", DbType.Xml, CreateContactXml(model.ContactList));
            this._db.AddInParameter(cmd, "File", DbType.Xml, CreateFileXml(model.FileList));


            this._db.AddOutParameter(cmd, "Result", DbType.Int32, 4);
            DbHelper.RunProcedureWithResult(cmd, _db);
            return Convert.ToInt32(_db.GetParameterValue(cmd, "Result"));
        }

        /// <summary>
        /// 添加地接、机票的 供应商 
        /// </summary>
        /// <param name="model"></param>
        /// <returns>1:成功 0：失败</returns>
        public int Add(EyouSoft.Model.SourceStructure.MSupplier model, ref IList<string> list)
        {
            DbCommand cmd = this._db.GetStoredProcCommand("proc_Supplier_Add");
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


            this._db.AddInParameter(cmd, "Contact", DbType.Xml, CreateContactXml(model.ContactList));
            this._db.AddInParameter(cmd, "File", DbType.Xml, CreateFileXml(model.FileList));


            this._db.AddOutParameter(cmd, "Result", DbType.Int32, 4);
            //  DbHelper.RunProcedureWithResult(cmd, _db);
            using (IDataReader dr = DbHelper.ExecuteReader(cmd, _db))
            {
                while (dr.Read())
                {
                    list.Add(!dr.IsDBNull(dr.GetOrdinal("UserName")) ? dr.GetString(dr.GetOrdinal("UserName")) : null);
                }
            }

            return Convert.ToInt32(_db.GetParameterValue(cmd, "Result"));
        }

        /// <summary>
        /// 修改地接、机票的 供应商
        /// </summary>
        /// <param name="model"></param>
        /// <returns>1:成功 0：失败</returns>
        public int Update(EyouSoft.Model.SourceStructure.MSupplier model)
        {
            DbCommand cmd = this._db.GetStoredProcCommand("proc_Supplier_Update");
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


            this._db.AddInParameter(cmd, "Contact", DbType.Xml, CreateContactXml(model.ContactList));
            this._db.AddInParameter(cmd, "File", DbType.Xml, CreateFileXml(model.FileList));


            this._db.AddOutParameter(cmd, "Result", DbType.Int32, 4);
            DbHelper.RunProcedureWithResult(cmd, _db);
            return Convert.ToInt32(_db.GetParameterValue(cmd, "Result"));
        }


        /// <summary>
        /// 修改地接、机票的 供应商
        /// </summary>
        /// <param name="model"></param>
        /// <returns>1:成功 0：失败</returns>
        public int Update(EyouSoft.Model.SourceStructure.MSupplier model, ref IList<string> list)
        {
            DbCommand cmd = this._db.GetStoredProcCommand("proc_Supplier_Update");
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


            this._db.AddInParameter(cmd, "Contact", DbType.Xml, CreateContactXml(model.ContactList));
            this._db.AddInParameter(cmd, "File", DbType.Xml, CreateFileXml(model.FileList));


            this._db.AddOutParameter(cmd, "Result", DbType.Int32, 4);
            //  DbHelper.RunProcedureWithResult(cmd, _db);
            using (IDataReader dr = DbHelper.ExecuteReader(cmd, _db))
            {
                while (dr.Read())
                {
                    list.Add(!dr.IsDBNull(dr.GetOrdinal("UserName")) ? dr.GetString(dr.GetOrdinal("UserName")) : null);
                }
            }
            return Convert.ToInt32(_db.GetParameterValue(cmd, "Result"));
        }

        /// <summary>
        /// 删除地接、机票的 供应商
        /// </summary>
        /// <param name="Id"></param>
        /// <returns>
        /// 0:失败 1:成功 
        /// -1:地接供应商做过安排不允许删除
        /// -2:机票供应商做过安排不允许删除
        /// </returns>
        public int Delete(string Id)
        {
            DbCommand cmd = this._db.GetStoredProcCommand("proc_Supplier_Delete");
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
        public EyouSoft.Model.SourceStructure.MSupplier GetModel(string Id)
        {
            EyouSoft.Model.SourceStructure.MSupplier model = null;
            StringBuilder query = new StringBuilder();
            query.Append("SELECT Id,CompanyId,ProvinceId,CityId");
            query.Append(",UnitName,SupplierType,LicenseKey");
            query.Append(",AgreementFile,UnitAddress,UnitPolicy");
            query.Append(" ,Remark,OperatorId,");
            query.Append("(select B.*,C.UserName,C.Password,C.UserType from tbl_SupplierContact  as B");
            query.Append(" left join tbl_CompanyUser as C");
            query.Append(" on B.UserId=C.Id where B.SupplierId=A.Id for xml raw,root('Root')) as ContactInfo,");
            query.Append("(select * from tbl_SupplierFile where SupplierId=A.Id for xml raw,root('Root')) as FileInfo ");
            query.Append("  FROM tbl_CompanySupplier as A");
            query.Append(" where A.Id=@Id");

            DbCommand cmd = this._db.GetSqlStringCommand(query.ToString());
            this._db.AddInParameter(cmd, "Id", DbType.AnsiStringFixedLength, Id);

            using (IDataReader dr = DbHelper.ExecuteReader(cmd, this._db))
            {
                while (dr.Read())
                {
                    model = new EyouSoft.Model.SourceStructure.MSupplier();

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


                    model.ContactList = new List<EyouSoft.Model.SourceStructure.MSupplierContact>();
                    model.ContactList = !dr.IsDBNull(dr.GetOrdinal("ContactInfo")) ? GetContactByXml(dr.GetString(dr.GetOrdinal("ContactInfo"))) : null;

                    model.FileList = new List<EyouSoft.Model.SourceStructure.MFileInfo>();
                    model.FileList = !dr.IsDBNull(dr.GetOrdinal("FileInfo")) ? GetFileByXml(dr.GetString(dr.GetOrdinal("FileInfo"))) : null;
                }
            }

            return model;
        }

        /// <summary>
        /// 获取信息  用于选择补全控件
        /// </summary>
        /// <param name="CompanyId"></param>
        /// <param name="UnitName"></param>
        /// <param name="SupplierType"></param>
        /// <returns></returns>
        public IList<EyouSoft.Model.SourceStructure.MSupplier> GetList(int CompanyId, string UnitName, EyouSoft.Model.EnumType.CompanyStructure.SupplierType SupplierType)
        {
            IList<EyouSoft.Model.SourceStructure.MSupplier> list = new List<EyouSoft.Model.SourceStructure.MSupplier>();

            string sql = string.Format("select Id,UnitName from tbl_CompanySupplier where CompanyId={0} and IsDelete='0' and UnitName like '%{1}%' and SupplierType={2}", CompanyId, UnitName, (int)SupplierType);
            DbCommand cmd = this._db.GetSqlStringCommand(sql);

            using (IDataReader dr = DbHelper.ExecuteReader(cmd, this._db))
            {
                while (dr.Read())
                {
                    EyouSoft.Model.SourceStructure.MSupplier model = new EyouSoft.Model.SourceStructure.MSupplier();
                    model.Id = dr.GetString(dr.GetOrdinal("Id"));
                    model.UnitName = dr.GetString(dr.GetOrdinal("UnitName"));
                    list.Add(model);
                }
            }

            return list;
        }

        /// <summary>
        /// 根据公司编号  供应商名称获取 供应商集合 （用于选用）
        /// </summary>
        /// <param name="SupplierType"></param>
        /// <param name="companyId"></param>
        /// <param name="unitName"></param>
        /// <returns></returns>
        public IList<EyouSoft.Model.SourceStructure.MSupplier> GetList(EyouSoft.Model.EnumType.CompanyStructure.SupplierType SupplierType,
         int companyId, string unitName)
        {
            IList<EyouSoft.Model.SourceStructure.MSupplier> list = new List<EyouSoft.Model.SourceStructure.MSupplier>();

            StringBuilder query = new StringBuilder();
            query.Append("SELECT Id,CompanyId,ProvinceId,CityId");
            query.Append(",UnitName,SupplierType,LicenseKey");
            query.Append(",AgreementFile,UnitAddress,UnitPolicy");
            query.Append(" ,Remark,OperatorId,");
            query.Append("(select B.*,C.UserName,C.Password,C.UserType from tbl_SupplierContact  as B");
            query.Append(" left join tbl_CompanyUser as C");
            query.Append(" on B.UserId=C.Id where B.SupplierId=A.Id for xml raw,root('Root')) as ContactInfo,");
            query.Append("(select * from tbl_SupplierFile where SupplierId=A.Id for xml raw,root('Root')) as FileInfo ");
            query.Append("  FROM tbl_CompanySupplier as A");
            query.AppendFormat(" where A.CompanyId={0} and IsDelete='0' and A.UnitName like '%{1}%' and A.SupplierType={2}", companyId, unitName, (int)SupplierType);

            DbCommand cmd = this._db.GetSqlStringCommand(query.ToString());


            using (IDataReader dr = DbHelper.ExecuteReader(cmd, this._db))
            {
                while (dr.Read())
                {
                    EyouSoft.Model.SourceStructure.MSupplier model = new EyouSoft.Model.SourceStructure.MSupplier();

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


                    model.ContactList = new List<EyouSoft.Model.SourceStructure.MSupplierContact>();
                    model.ContactList = !dr.IsDBNull(dr.GetOrdinal("ContactInfo")) ? GetContactByXml(dr.GetString(dr.GetOrdinal("ContactInfo"))) : null;

                    model.FileList = new List<EyouSoft.Model.SourceStructure.MFileInfo>();
                    model.FileList = !dr.IsDBNull(dr.GetOrdinal("FileInfo")) ? GetFileByXml(dr.GetString(dr.GetOrdinal("FileInfo"))) : null;
                    list.Add(model);
                }

            }

            return list;


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
        public IList<EyouSoft.Model.SourceStructure.MPageSupplier> GetList(
         EyouSoft.Model.EnumType.CompanyStructure.SupplierType SupplierType,
         int companyId,
         int pageSize,
         int pageIndex,
         ref int recordCount,
         EyouSoft.Model.SourceStructure.MSupplierSearch search)
        {
            IList<EyouSoft.Model.SourceStructure.MPageSupplier> list = new List<EyouSoft.Model.SourceStructure.MPageSupplier>();

            string fileds = "Id,ProvinceName,CityName,UnitName,ContactInfo,FileInfo";

            string tableName = "view_Supplier";

            string orderByString = " UnitName asc";

            StringBuilder query = new StringBuilder();

            query.AppendFormat(" CompanyId={0} and IsDelete='0' and SupplierType={1}", companyId, (int)SupplierType);

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

                if (!string.IsNullOrEmpty(search.UnitName))
                {
                    query.AppendFormat(" and UnitName like '%{0}%' ", search.UnitName);
                }
            }

            using (IDataReader dr = DbHelper.ExecuteReader1(this._db, pageSize, pageIndex, ref recordCount, tableName, fileds, query.ToString(), orderByString, null))
            {
                while (dr.Read())
                {
                    EyouSoft.Model.SourceStructure.MPageSupplier model = new EyouSoft.Model.SourceStructure.MPageSupplier();
                    model.Id = dr.GetString(dr.GetOrdinal("Id"));
                    model.ProvinceName = !dr.IsDBNull(dr.GetOrdinal("ProvinceName")) ? dr.GetString(dr.GetOrdinal("ProvinceName")) : null;
                    model.CityName = !dr.IsDBNull(dr.GetOrdinal("CityName")) ? dr.GetString(dr.GetOrdinal("CityName")) : null;
                    model.UnitName = !dr.IsDBNull(dr.GetOrdinal("UnitName")) ? dr.GetString(dr.GetOrdinal("UnitName")) : null;

                    model.ContactInfo = new EyouSoft.Model.SourceStructure.MSupplierContact();
                    model.ContactInfo = !dr.IsDBNull(dr.GetOrdinal("ContactInfo")) ? GetContactByXml(dr.GetString(dr.GetOrdinal("ContactInfo")))[0] : null;

                    model.FileList = new List<EyouSoft.Model.SourceStructure.MFileInfo>();
                    model.FileList = !dr.IsDBNull(dr.GetOrdinal("FileInfo")) ? GetFileByXml(dr.GetString(dr.GetOrdinal("FileInfo"))) : null;
                    list.Add(model);
                }
            }


            return list;

        }


        /// <summary>
        /// 根据公司编号、供应商编号、供应商联系人的用户编号修改联系人信息
        /// </summary>
        /// <param name="companyId">公司编号</param>
        /// <param name="supplierId">供应商编号</param>
        /// <param name="contactUserId">供应商联系人的用户编号</param>
        /// <param name="model">供应商联系人实体</param>
        /// <returns>1：成功；其他失败</returns>
        public int UpdateSupplierContact(
            int companyId, string supplierId, int contactUserId, Model.SourceStructure.MSupplierContact model)
        {
            if (companyId <= 0 || string.IsNullOrEmpty(supplierId) || contactUserId <= 0 || model == null)
            {
                return 0;
            }

            var strSql = new StringBuilder();
            strSql.AppendLine(
                " UPDATE [tbl_SupplierContact] SET [ContactName] = @ContactName,[ContactFax] = @ContactFax,[ContactTel] = @ContactTel,[ContactMobile] = @ContactMobile,[QQ] = @QQ,[Email] = @Email,[Birthday] = @Birthday ");
            strSql.AppendFormat(" WHERE CompanyId = @CompanyId and SupplierId = @SupplierId and UserId = @UserId ; ");

            DbCommand dc = _db.GetSqlStringCommand(strSql.ToString());
            _db.AddInParameter(dc, "CompanyId", DbType.Int32, companyId);
            _db.AddInParameter(dc, "SupplierId", DbType.AnsiStringFixedLength, supplierId);
            _db.AddInParameter(dc, "UserId", DbType.Int32, contactUserId);
            _db.AddInParameter(dc, "ContactName", DbType.String, model.ContactName);
            _db.AddInParameter(dc, "ContactFax", DbType.String, model.ContactFax);
            _db.AddInParameter(dc, "ContactTel", DbType.String, model.ContactTel);
            _db.AddInParameter(dc, "ContactMobile", DbType.String, model.ContactMobile);
            _db.AddInParameter(dc, "QQ", DbType.String, model.QQ);
            _db.AddInParameter(dc, "Email", DbType.String, model.Email);
            if (model.Birthday.HasValue) _db.AddInParameter(dc, "Birthday", DbType.DateTime, model.Birthday);
            else _db.AddInParameter(dc, "Birthday", DbType.DateTime, DBNull.Value);

            return DbHelper.ExecuteSql(dc, _db) > 0 ? 1 : -1;
        }


        #endregion


        #region private to xml

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
                if (model.LoginInfo != null)
                {
                    xmlDoc.AppendFormat("UserId=\"{0}\" ", model.LoginInfo.UserId);
                    xmlDoc.AppendFormat("UserName=\"{0}\" ", Utils.ReplaceXmlSpecialCharacter(model.LoginInfo.UserName));
                    xmlDoc.AppendFormat("Password=\"{0}\" ", model.LoginInfo.PassWord.NoEncryptPassword);
                    xmlDoc.AppendFormat("MD5Password=\"{0}\" ", model.LoginInfo.PassWord.MD5Password);
                    xmlDoc.AppendFormat("UserType=\"{0}\" ", (int)model.LoginInfo.UserType);
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

                if (Utils.GetInt(Utils.GetXAttributeValue(xRow, "UserId")) != 0 && !string.IsNullOrEmpty(Utils.GetXAttributeValue(xRow, "UserName")))
                {
                    model.LoginInfo = new EyouSoft.Model.SourceStructure.MSupplierContactLoginInfo();
                    model.LoginInfo.UserId = Utils.GetXAttributeValue(xRow, "UserId");
                    model.LoginInfo.UserName = Utils.GetXAttributeValue(xRow, "UserName");
                    model.LoginInfo.PassWord = new EyouSoft.Model.CompanyStructure.PassWord();
                    model.LoginInfo.PassWord.NoEncryptPassword = Utils.GetXAttributeValue(xRow, "Password");
                    model.LoginInfo.UserType = (EyouSoft.Model.EnumType.CompanyStructure.UserType)Utils.GetInt(Utils.GetXAttributeValue(xRow, "UserType"));
                }

                list.Add(model);
            }
            return list;
        }



        #endregion
    }
}
