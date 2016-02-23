using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.Common;
using EyouSoft.Model;
using EyouSoft.IDAL;
using Microsoft.Practices.EnterpriseLibrary.Data;

namespace EyouSoft.DAL.SMSStructure
{
    /// <summary>
    /// 短信中心-客户信息及客户类型数据访问类
    /// </summary>
    /// Author:汪奇志 2010-06-10 
    ///  增加GetMobiles函数 ，根据条件获取接收短信号--李焕超--2010-12-17
    ///  修改：xuqh 2011-01-21
    public class CustomerList : EyouSoft.Toolkit.DAL.DALBase, EyouSoft.IDAL.SMSStructure.ICustomerList
    {
        #region static constants
        //static constants
        private const string SQL_INSERT_INSERTCATEGORY = "INSERT INTO [SMS_CustomerClass] ([CompanyID] ,[UserID] ,[ClassName]) VALUES(@COMPANYID,@USERID,@CLASSNAME);SELECT @@IDENTITY";
        private const string SQL_DELETE_DELETECATEGORY = "DELETE FROM [SMS_CustomerClass] WHERE [ID]=@CategoryId";
        private const string SQL_SELECT_GETCATEGORYS = "SELECT [ID] ,[CompanyID] ,[UserID] ,[ClassName] ,[IssueTime]  FROM [SMS_CustomerClass] WHERE [CompanyId]=@COMPANYID";
        private const string SQL_DELETE_DELETECUSTOMER = "DELETE FROM [SMS_CustomerList] ";
        private const string SQL_SELECT_GETCUSTOMERINFO = "SELECT [ID] ,[CompanyID] ,[UserID] ,[CustomerCompanyName] ,[CustomerContactName] ,[ClassID] ,[ReMark] ,[MobileNumber] ,[IssueTime], ProvinceId, CityId  FROM [SMS_CustomerList] WHERE [ID]=@CUSTOMERID";
        private const string SQL_INSERT_InsertCustomer = "IF NOT EXISTS(SELECT 1 FROM [SMS_CustomerList] WHERE [CompanyID]=@COMPANYID AND [MobileNumber]=@MOBILE) BEGIN INSERT INTO [SMS_CustomerList] ([ID],[CompanyID] ,[UserID] ,[CustomerCompanyName] ,[CustomerContactName] ,[ClassID] ,[ReMark] ,[MobileNumber], [ProvinceId], [CityId]) VALUES(@ID,@COMPANYID,@USERID,@CUSTOMERCOMPANYNAME,@CUSTOMERCONTACTNAME,@CLASSID,@REMARK,@MOBILE,@ProvinceId,@CityId) END";
        private const string SQL_UPDATE_UpdateCustomer = "IF NOT EXISTS(SELECT 1 FROM [SMS_CustomerList] WHERE [CompanyID]=@COMPANYID AND [MobileNumber]=@MOBILE AND [ID]<>@CUSTOMERID) BEGIN UPDATE [SMS_CustomerList] SET [CustomerCompanyName]=@CUSTOMERCOMPANYNAME,[CustomerContactName]=@CUSTOMERCONTACTNAME,[ClassID]=@CLASSID,[ReMark]=@REMARK,[MobileNumber]=@MOBILE,[ProvinceId]=@ProvinceId,[CityId]=@CityId WHERE [ID]=@CUSTOMERID END";
        private const string SQL_SELECT_IsExistsCustomerMobile = "SELECT COUNT(*) FROM SMS_CustomerList WHERE CompanyID=@CompanyId AND MobileNumber=@Mobile ";
        #endregion static constants

        private readonly Database _db = null;

        #region constructure
        /// <summary>
        /// default constructure
        /// </summary>
        public CustomerList()
        {
            this._db = base.SystemStore;
        }
        #endregion

        #region 成员方法
        /// <summary>
        /// 添加客户类型信息
        /// </summary>
        /// <param name="classInfo">客户类型业务实体</param>
        /// <returns>添加成功TRUE|添加失败FALSE</returns>
        public virtual int AddCustomClass(EyouSoft.Model.SMSStructure.CustomerClass classInfo)
        {
            DbCommand cmd = this._db.GetSqlStringCommand(SQL_INSERT_INSERTCATEGORY);

            this._db.AddInParameter(cmd, "COMPANYID", DbType.Int32, classInfo.CompanyID);
            this._db.AddInParameter(cmd, "USERID", DbType.Int32, classInfo.UserID);
            this._db.AddInParameter(cmd, "CLASSNAME", DbType.String, classInfo.ClassName);

            object obj = EyouSoft.Toolkit.DAL.DbHelper.GetSingle(cmd, this._db);

            if (obj == null)
            {
                return 0;
            }
            else
            {
                return Convert.ToInt32(obj);
            }
        }

        /// <summary>
        /// 删除客户类型信息
        /// </summary>
        /// <param name="CategoryId">类型编号</param>
        /// <returns></returns>
        public virtual bool DeleteCustomClass(int Id)
        {
            DbCommand cmd = this._db.GetSqlStringCommand(SQL_DELETE_DELETECATEGORY);

            this._db.AddInParameter(cmd, "CategoryId", DbType.Int32, Id);

            return EyouSoft.Toolkit.DAL.DbHelper.ExecuteSql(cmd, this._db) > 0 ? true : false;
        }

        /// <summary>
        /// 跟据公司ID获取该公司所有客户类型信息
        /// </summary>
        /// <param name="companyId">公司编号</param>
        /// <returns></returns>
        public virtual IList<EyouSoft.Model.SMSStructure.CustomerClass> GetCustomerClass(int CompanyID)
        {
            IList<EyouSoft.Model.SMSStructure.CustomerClass> categorys = new List<EyouSoft.Model.SMSStructure.CustomerClass>();
            DbCommand cmd = this._db.GetSqlStringCommand(SQL_SELECT_GETCATEGORYS);
            this._db.AddInParameter(cmd, "COMPANYID", DbType.AnsiStringFixedLength, CompanyID);

            using (IDataReader rdr = EyouSoft.Toolkit.DAL.DbHelper.ExecuteReader(cmd, this._db))
            {
                while (rdr.Read())
                {
                    categorys.Add(new EyouSoft.Model.SMSStructure.CustomerClass(
                            rdr.GetInt32(rdr.GetOrdinal("ID")),
                            rdr.GetInt32(rdr.GetOrdinal("CompanyID")),
                            rdr.GetInt32(rdr.GetOrdinal("UserID")),
                            rdr.GetString(rdr.GetOrdinal("ClassName")),
                            rdr.GetDateTime(rdr.GetOrdinal("IssueTime"))
                            )
                        );
                }
            }

            return categorys;
        }

        /// <summary>
        /// 添加客户信息
        /// </summary>
        /// <param name="customerInfo">客户信息业务实体</param>
        /// <returns>返回处理结果</returns>
        public virtual bool AddCustomerList(EyouSoft.Model.SMSStructure.CustomerList customerInfo)
        {
            DbCommand cmd = this._db.GetSqlStringCommand(SQL_INSERT_InsertCustomer);

            this._db.AddInParameter(cmd, "ID", DbType.AnsiStringFixedLength, Guid.NewGuid().ToString());
            this._db.AddInParameter(cmd, "COMPANYID", DbType.String, customerInfo.CompanyID);
            this._db.AddInParameter(cmd, "USERID", DbType.String, customerInfo.UserID);
            this._db.AddInParameter(cmd, "CUSTOMERCOMPANYNAME", DbType.String, customerInfo.CustomerCompanyName);
            this._db.AddInParameter(cmd, "CUSTOMERCONTACTNAME", DbType.String, customerInfo.CustomerContactName);
            this._db.AddInParameter(cmd, "CLASSID", DbType.String, customerInfo.ClassID);
            this._db.AddInParameter(cmd, "REMARK", DbType.String, customerInfo.ReMark);
            this._db.AddInParameter(cmd, "MOBILE", DbType.String, customerInfo.MobileNumber);
            this._db.AddInParameter(cmd, "ProvinceId", DbType.Int32, customerInfo.ProvinceId);
            this._db.AddInParameter(cmd, "CityId", DbType.Int32, customerInfo.CityId);

            return EyouSoft.Toolkit.DAL.DbHelper.ExecuteSql(cmd, this._db) > 0 ? true : false;
        }

        /// <summary>
        /// 分页获取信息
        /// </summary>
        /// <param name="pageSize">每页记录数</param>
        /// <param name="pageIndex">当前页索引</param>
        /// <param name="recordCount">总记录数</param>
        /// <param name="companyId">公司编号</param>
        /// <param name="customerCompanyName">客户公司名称 为空时不做为查询条件</param>
        /// <param name="customerUserFullName">客户联系人姓名 为空时不做为查询条件</param>
        /// <param name="mobile">手机号码 为空时不做为查询条件</param>
        /// <param name="categoryId">客户类型编号 -1时不做为查询条件</param>
        /// <returns></returns>
        public virtual IList<EyouSoft.Model.SMSStructure.CustomerList> GetList(int pageSize, int pageIndex, ref int recordCount, string companyId, string customerCompanyName, string customerUserFullName, string mobile, int categoryId)
        {
            IList<EyouSoft.Model.SMSStructure.CustomerList> customers = new List<EyouSoft.Model.SMSStructure.CustomerList>();

            StringBuilder cmdQuery = new StringBuilder();
            string tableName = "view_SMS_Customers";
            string orderByString = "IssueTime DESC";
            string fields = " ID, CompanyID, UserID, CustomerCompanyName, CustomerContactName, ClassID, ReMark, MobileNumber, IssueTime, ClassName";

            cmdQuery.Append(" 1=1 ");
            cmdQuery.AppendFormat(" and CompanyID='{0}' ", companyId);

            //switch (source)
            //{
            //    case EyouSoft.Model.SMSStructure.CustomerList.CustomerSource.所有客户:  //注意TongYCompanyId<>companyId是用来排除自己的
            //        cmdQuery.AppendFormat(" AND (CompanyID='{0}' OR CompanyID='0') AND TongYCompanyId <> '{0}' ", companyId);
            //        break;
            //    case EyouSoft.Model.SMSStructure.CustomerList.CustomerSource.我的客户:
            //        cmdQuery.AppendFormat(" AND CompanyID='{0}' ", companyId);
            //        break;
            //    case EyouSoft.Model.SMSStructure.CustomerList.CustomerSource.平台组团社客户://注意TongYCompanyId<>companyId是用来排除自己的
            //        cmdQuery.AppendFormat(" AND CompanyID='0' AND TongYCompanyId <> '{0}' ", companyId);
            //        break;
            //}

            //单位名称
            if (!string.IsNullOrEmpty(customerCompanyName))
            {
                cmdQuery.AppendFormat(" AND (CustomerCompanyName LIKE '%{0}%')", customerCompanyName);
            }

            //姓名
            if (!string.IsNullOrEmpty(customerUserFullName))
            {
                cmdQuery.AppendFormat(" AND (CustomerContactName LIKE '%{0}%')", customerUserFullName);
            }

            //手机
            if (!string.IsNullOrEmpty(mobile))
            {
                cmdQuery.AppendFormat(" AND (MobileNumber LIKE '%{0}%')", mobile);
            }

            //分类
            if (categoryId != -1 && categoryId != 0)
            {
                cmdQuery.AppendFormat(" AND ClassID={0}", categoryId.ToString());
            }

            using (IDataReader rdr = EyouSoft.Toolkit.DAL.DbHelper.ExecuteReader1(this._db, pageSize, pageIndex, ref recordCount, tableName
                , fields, cmdQuery.ToString(), orderByString, string.Empty))
            {
                EyouSoft.Model.SMSStructure.CustomerList model = null;

                while (rdr.Read())
                {
                    model = new EyouSoft.Model.SMSStructure.CustomerList();

                    model.ID = rdr.GetString(rdr.GetOrdinal("ID")).Trim();
                    model.CompanyID = rdr.GetInt32(rdr.GetOrdinal("CompanyID"));
                    model.UserID = rdr.GetInt32(rdr.GetOrdinal("UserID"));
                    model.CustomerCompanyName = rdr["CustomerCompanyName"].ToString();
                    model.CustomerContactName = rdr["CustomerContactName"].ToString();
                    model.ClassID = rdr.GetInt32(rdr.GetOrdinal("ClassID"));
                    model.ClassName = rdr["ClassName"].ToString();
                    model.ReMark = rdr["ReMark"].ToString();
                    model.MobileNumber = rdr["MobileNumber"].ToString();
                    model.IssueTime = rdr.GetDateTime(rdr.GetOrdinal("IssueTime"));

                    customers.Add(model);
                }
            }

            return customers;

        }

        /// <summary>
        /// 获取客户总数
        /// </summary>
        /// <returns></returns>
        //public virtual int GetCustomersCount()
        //{
        //    int RecordCount = 0;
        //    StringBuilder cmdQuery = new StringBuilder();
        //    cmdQuery.Append("select count(*) from view_SMS_Customers ");
        //    cmdQuery.Append("where id='0'");
        //    DbCommand cmd = this._db.GetSqlStringCommand(cmdQuery.ToString());
        //    using (IDataReader rdr = EyouSoft.Toolkit.DAL.DbHelper.ExecuteReader(cmd, this._db))
        //    {

        //        if (rdr.Read())
        //        {
        //            RecordCount = rdr.GetInt32(0);
        //        }

        //    }

        //    return RecordCount;
        //}

        /// <summary>
        /// 根据指定条件获取接收短信号码
        /// </summary>
        /// <param name="companyId">公司编号</param>
        /// <param name="customerCompanyName">客户公司名称 为空时不做为查询条件</param>
        /// <param name="customerUserFullName">客户联系人姓名 为空时不做为查询条件</param>
        /// <param name="mobile">手机号码 为空时不做为查询条件</param>
        /// <param name="categoryId">客户类型编号 -1时不做为查询条件</param>
        /// <param name="source">客户来源</param>
        /// <param name="provinceId">省份ID</param>
        /// <param name="cityId">城市ID</param>
        /// <returns></returns>
        public virtual IList<EyouSoft.Model.SMSStructure.AcceptMobileInfo> GetMobiles(string companyId, string mobile, int categoryId, EyouSoft.Model.SMSStructure.CustomerList.CustomerSource source, int provinceId, int cityId)
        {
            //IList<EyouSoft.Model.SMSStructure.AcceptMobileInfo> AcceptMobiles = new List<EyouSoft.Model.SMSStructure.AcceptMobileInfo>();

            //StringBuilder cmdQuery = new StringBuilder();
            //string tableName = "view_SMS_Customers";
            //string primaryKey = "Id";
            //string orderByString = "IssueTime DESC";
            //string fields = " ID,ClassID,MobileNumber ";
            //cmdQuery.AppendFormat(" select {0} from {1} where",fields,tableName); 
            //cmdQuery.Append(" 1=1 ");

            //switch (source)
            //{
            //    case EyouSoft.Model.SMSStructure.CustomerInfo.CustomerSource.所有客户:  //注意TongYCompanyId<>companyId是用来排除自己的
            //        cmdQuery.AppendFormat(" AND (CompanyID='{0}' OR CompanyID='0') AND TongYCompanyId <> '{0}' ", companyId);
            //        break;
            //    case EyouSoft.Model.SMSStructure.CustomerInfo.CustomerSource.我的客户:
            //        cmdQuery.AppendFormat(" AND CompanyID='{0}' ", companyId);
            //        break;
            //    case EyouSoft.Model.SMSStructure.CustomerInfo.CustomerSource.平台组团社客户://注意TongYCompanyId<>companyId是用来排除自己的
            //        cmdQuery.AppendFormat(" AND CompanyID='0' AND TongYCompanyId <> '{0}' ", companyId);
            //        break;
            //}

            //if (provinceId > 0)
            //    cmdQuery.AppendFormat(" AND ProvinceId={0}", provinceId);

            //if (cityId > 0)
            //    cmdQuery.AppendFormat(" AND CityId={0}", cityId);

            //if (!string.IsNullOrEmpty(mobile))
            //{
            //    cmdQuery.AppendFormat(" AND (MobileNumber LIKE '%{0}%')", mobile);
            //}

            //if (categoryId != -1 && categoryId != 0)
            //{
            //    cmdQuery.AppendFormat(" AND ClassID={0}", categoryId.ToString());
            //}

            //DbCommand cmd = this._db.GetSqlStringCommand(cmdQuery.ToString());
            //cmd.CommandType=CommandType.Text;

            //using (IDataReader rdr = DbHelper.ExecuteReader(cmd,this._db))
            //{
            //    EyouSoft.Model.SMSStructure.AcceptMobileInfo model = null;

            //    while (rdr.Read())
            //    {
            //        model = new EyouSoft.Model.SMSStructure.AcceptMobileInfo();
            //        model.Mobile = rdr.IsDBNull(rdr.GetOrdinal("MobileNumber")) ? "" : rdr.GetString(rdr.GetOrdinal("MobileNumber"));
            //        if (rdr.GetString(rdr.GetOrdinal("id")) == "0")
            //            model.IsEncrypt = true;
            //        AcceptMobiles.Add(model);
            //    }
            //}

            //return AcceptMobiles;
            return null;

        }

        /// <summary>
        /// 删除客户信息
        /// </summary>
        /// <param name="customerId">客户编号</param>
        /// <returns></returns>
        public virtual bool DeleteCustomerList(string[] customerIds)
        {
            //[ID]=@CUSTOMERID
            if (customerIds == null || customerIds.Length <= 0)
                return false;

            string strIds = string.Empty;
            foreach (string str in customerIds)
            {
                strIds += "'" + str.Trim() + "',";
            }
            strIds = strIds.Trim(',');

            DbCommand cmd = this._db.GetSqlStringCommand(SQL_DELETE_DELETECUSTOMER + "where ID in (" + strIds + ");");
            //this._db.AddInParameter(cmd, "CUSTOMERID", DbType.AnsiStringFixedLength, customerId);
            return EyouSoft.Toolkit.DAL.DbHelper.ExecuteSql(cmd, this._db) > 0 ? true : false;
        }

        /// <summary>
        /// 获取客户信息实体
        /// </summary>
        /// <param name="customerId">客户编号</param>
        /// <returns></returns>
        public virtual EyouSoft.Model.SMSStructure.CustomerList GetCustomer(string customerId)
        {
            EyouSoft.Model.SMSStructure.CustomerList customerInfo = null;
            DbCommand cmd = this._db.GetSqlStringCommand(SQL_SELECT_GETCUSTOMERINFO);
            this._db.AddInParameter(cmd, "CUSTOMERID", DbType.AnsiStringFixedLength, customerId);

            using (IDataReader rdr = EyouSoft.Toolkit.DAL.DbHelper.ExecuteReader(cmd, this._db))
            {
                if (rdr.Read())
                {
                    customerInfo = new EyouSoft.Model.SMSStructure.CustomerList(rdr.GetString(rdr.GetOrdinal("ID"))
                        , rdr.GetInt32(rdr.GetOrdinal("CompanyID"))
                        , rdr.GetInt32(rdr.GetOrdinal("UserID"))
                        , rdr["CustomerCompanyName"].ToString()
                        , rdr["CustomerContactName"].ToString()
                        , rdr.GetInt32(rdr.GetOrdinal("ClassID"))
                        , rdr["ReMark"].ToString()
                        , rdr["MobileNumber"].ToString()
                        , rdr.GetDateTime(rdr.GetOrdinal("IssueTime"))
                        , rdr.GetInt32(rdr.GetOrdinal("ProvinceId"))
                        , rdr.GetInt32(rdr.GetOrdinal("CityId"))
                        );
                }
            }

            return customerInfo;
        }

        /// <summary>
        /// 更新客户信息
        /// </summary>
        /// <param name="customerInfo">客户信息业务实体</param>
        /// <returns>返回处理结果</returns>
        public virtual bool UpdateCustomerList(EyouSoft.Model.SMSStructure.CustomerList customerInfo)
        {
            DbCommand cmd = this._db.GetSqlStringCommand(SQL_UPDATE_UpdateCustomer);

            this._db.AddInParameter(cmd, "CUSTOMERCOMPANYNAME", DbType.String, customerInfo.CustomerCompanyName);
            this._db.AddInParameter(cmd, "CUSTOMERCONTACTNAME", DbType.String, customerInfo.CustomerContactName);
            this._db.AddInParameter(cmd, "CLASSID", DbType.Int32, customerInfo.ClassID);
            this._db.AddInParameter(cmd, "REMARK", DbType.String, customerInfo.ReMark);
            this._db.AddInParameter(cmd, "MOBILE", DbType.String, customerInfo.MobileNumber);
            this._db.AddInParameter(cmd, "CUSTOMERID", DbType.String, customerInfo.ID);
            this._db.AddInParameter(cmd, "COMPANYID", DbType.String, customerInfo.CompanyID);
            this._db.AddInParameter(cmd, "ProvinceId", DbType.Int32, customerInfo.ProvinceId);
            this._db.AddInParameter(cmd, "CityId", DbType.Int32, customerInfo.CityId);

            return EyouSoft.Toolkit.DAL.DbHelper.ExecuteSql(cmd, this._db) > 0 ? true : false;
        }

        /// <summary>
        /// 判断客户手机号码是否存在
        /// </summary>
        /// <param name="companyId">公司编号</param>
        /// <param name="mobile">客户号码</param>
        /// <param name="customerId">客户编号</param>
        /// <returns></returns>
        public virtual bool IsExistCustomerMobile(int companyId, string customerId, string mobile)
        {
            bool isExists = false;
            DbCommand cmd = this._db.GetSqlStringCommand(SQL_SELECT_IsExistsCustomerMobile);

            this._db.AddInParameter(cmd, "CompanyId", DbType.AnsiStringFixedLength, companyId);
            this._db.AddInParameter(cmd, "Mobile", DbType.String, mobile);

            if (!string.IsNullOrEmpty(customerId))
            {
                cmd.CommandText = SQL_SELECT_IsExistsCustomerMobile + " AND Id<>@CUSTOMERID";
                this._db.AddInParameter(cmd, "CUSTOMERID", DbType.AnsiStringFixedLength, customerId);
            }

            using (IDataReader rdr = EyouSoft.Toolkit.DAL.DbHelper.ExecuteReader(cmd, this._db))
            {
                if (rdr.Read())
                {
                    isExists = rdr.GetInt32(0) > 0 ? true : false;
                }
            }

            return isExists;
        }
        #endregion
    }

}
