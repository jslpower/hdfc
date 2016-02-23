using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.Common;
using EyouSoft.Toolkit.DAL;
using EyouSoft.Toolkit;
using Microsoft.Practices.EnterpriseLibrary.Data;
using EyouSoft.Model.CRM;
using System.Data;

namespace EyouSoft.DAL.CRM
{
    /// <summary>
    /// 客户资料数据访问
    /// </summary>
    public class DCustomer : DALBase, IDAL.CRM.ICustomer
    {
        private readonly Database _db;

        public DCustomer()
        {
            _db = this.SystemStore;
        }

        /// <summary>
        /// 添加客户资料
        /// </summary>
        /// <param name="model">客户资料实体</param>
        /// <returns>
        /// 1：成功；
        /// 0：参数错误；
        /// -1：添加失败；
        /// </returns>
        public int AddCustomer(MCustomer model)
        {
            if (model == null || string.IsNullOrEmpty(model.CustomerName) || model.SaleAreadId <= 0) return 0;

            model.Id = Guid.NewGuid().ToString();
            model.IssueTime = DateTime.Now;

            DbCommand dc = _db.GetSqlStringCommand("select 1");
            var strSql = new StringBuilder();
            //1、写客户信息
            strSql.Append(
                @" INSERT INTO [tbl_Customer] ([Id],[CompanyId],[ProviceId],[CityId],[SaleAreadId],[CustomerType],[CustomerName],[Licence]
                        ,[Address],[PostalCode],[BankCode],[ContactName],[Phone],[Mobile],[Fax],[Remark],[OperatorId],[IssueTime],[IsDelete],[CustomerRating])
                    VALUES (@Id,@CompanyId,@ProviceId,@CityId,@SaleAreadId,@CustomerType,@CustomerName,@Licence,@Address,@PostalCode
                        ,@BankCode,@ContactName,@Phone,@Mobile,@Fax,@Remark,@OperatorId,@IssueTime,@IsDelete,@CustomerRating); ");
            //2、写客户联系人信息
            if (model.Contact != null && model.Contact.Any())
            {
                for (int i = 0; i < model.Contact.Count; i++)
                {
                    strSql.AppendFormat(
                        @" INSERT INTO [tbl_CustomerContactInfo] ([CustomerId],[CompanyId],[Name],[Sex],[DepartmentId]
                                                ,[JobId],[Fax],[Tel],[Mobile],[qq],[BirthDay],[Email],[Remark])  
                                            VALUES (@Id,@CompanyId,'{0}','{1}','{2}'
                                                ,'{3}','{4}','{5}','{6}','{7}',@BirthDay{8},'{9}','{10}'); ",
                        model.Contact[i].Name,
                        (int)model.Contact[i].Sex,
                        model.Contact[i].DepartmentName,
                        model.Contact[i].Job,
                        model.Contact[i].Fax,
                        model.Contact[i].Tel,
                        model.Contact[i].Mobile,
                        model.Contact[i].Qq,
                        i,
                        model.Contact[i].Email,
                        model.Contact[i].Remark);
                    if (model.Contact[i].BirthDay.HasValue)
                    {
                        _db.AddInParameter(dc, string.Format("BirthDay{0}", i), DbType.DateTime, model.Contact[i].BirthDay.Value);
                    }
                    else
                    {
                        _db.AddInParameter(dc, string.Format("BirthDay{0}", i), DbType.DateTime, DBNull.Value);
                    }

                }
            }

            dc.CommandText = strSql.ToString();
            _db.AddInParameter(dc, "Id", DbType.AnsiStringFixedLength, model.Id);
            _db.AddInParameter(dc, "CompanyId", DbType.Int32, model.CompanyId);
            _db.AddInParameter(dc, "ProviceId", DbType.Int32, model.ProviceId);
            _db.AddInParameter(dc, "CityId", DbType.Int32, model.CityId);
            _db.AddInParameter(dc, "SaleAreadId", DbType.Int32, model.SaleAreadId);
            _db.AddInParameter(dc, "CustomerType", DbType.Byte, (int)model.CustomerType);
            _db.AddInParameter(dc, "CustomerName", DbType.String, model.CustomerName);
            _db.AddInParameter(dc, "Licence", DbType.String, model.Licence);
            _db.AddInParameter(dc, "Address", DbType.String, model.Address);
            _db.AddInParameter(dc, "PostalCode", DbType.String, model.PostalCode);
            _db.AddInParameter(dc, "BankCode", DbType.String, model.BankCode);
            _db.AddInParameter(dc, "ContactName", DbType.String, model.ContactName);
            _db.AddInParameter(dc, "Phone", DbType.String, model.Phone);
            _db.AddInParameter(dc, "Mobile", DbType.String, model.Mobile);
            _db.AddInParameter(dc, "Fax", DbType.String, model.Fax);
            _db.AddInParameter(dc, "Remark", DbType.String, model.Remark);
            _db.AddInParameter(dc, "OperatorId", DbType.Int32, model.OperatorId);
            _db.AddInParameter(dc, "IssueTime", DbType.DateTime, model.IssueTime);
            _db.AddInParameter(dc, "IsDelete", DbType.AnsiStringFixedLength, "0");
            _db.AddInParameter(dc, "CustomerRating", DbType.Int32, (int)model.CustomerRating);

            return DbHelper.ExecuteSqlTrans(dc, _db) > 0 ? 1 : -1;
        }

        /// <summary>
        /// 修改客户资料
        /// </summary>
        /// <param name="model">客户资料实体</param>
        /// <returns>
        /// 1：成功；
        /// 0：参数错误；
        /// -1：修改失败；
        /// </returns>
        public int UpdateCustomer(MCustomer model)
        {
            if (model == null || string.IsNullOrEmpty(model.CustomerName) || model.SaleAreadId <= 0 || string.IsNullOrEmpty(model.Id))
                return 0;

            DbCommand dc = _db.GetSqlStringCommand("select 1");
            var strSql = new StringBuilder();
            strSql.Append(
                " UPDATE [tbl_Customer] SET [ProviceId] = @ProviceId,[CityId] = @CityId,[SaleAreadId] = @SaleAreadId,[CustomerName] = @CustomerName,[Licence] = @Licence,[Address] = @Address,[PostalCode] = @PostalCode,[BankCode] = @BankCode,[ContactName] = @ContactName,[Phone] = @Phone,[Mobile] = @Mobile,[Fax] = @Fax,[Remark] = @Remark,[OperatorId] = @OperatorId,CustomerRating=@CustomerRating WHERE [Id] = @Id; ");

            //2、写客户联系人信息
            strSql.Append(" delete from tbl_CustomerContactInfo where CustomerId = @Id; ");
            if (model.Contact != null && model.Contact.Any())
            {
                for (int i = 0; i < model.Contact.Count; i++)
                {
                    strSql.AppendFormat(
                        @" INSERT INTO [tbl_CustomerContactInfo] ([CustomerId],[CompanyId],[Name],[Sex],[DepartmentId]
                                                ,[JobId],[Fax],[Tel],[Mobile],[qq],[BirthDay],[Email],[Remark])  
                                            VALUES (@Id,@CompanyId,'{0}','{1}','{2}'
                                                ,'{3}','{4}','{5}','{6}','{7}',@BirthDay{8},'{9}','{10}'); ",
                        model.Contact[i].Name,
                        (int)model.Contact[i].Sex,
                        model.Contact[i].DepartmentName,
                        model.Contact[i].Job,
                        model.Contact[i].Fax,
                        model.Contact[i].Tel,
                        model.Contact[i].Mobile,
                        model.Contact[i].Qq,
                        i,
                        model.Contact[i].Email,
                        model.Contact[i].Remark);
                    if (model.Contact[i].BirthDay.HasValue)
                    {
                        _db.AddInParameter(dc, string.Format("BirthDay{0}", i), DbType.DateTime, model.Contact[i].BirthDay.Value);
                    }
                    else
                    {
                        _db.AddInParameter(dc, string.Format("BirthDay{0}", i), DbType.DateTime, DBNull.Value);
                    }

                }
            }

            dc.CommandText = strSql.ToString();
            _db.AddInParameter(dc, "Id", DbType.AnsiStringFixedLength, model.Id);
            _db.AddInParameter(dc, "CompanyId", DbType.Int32, model.CompanyId);
            _db.AddInParameter(dc, "ProviceId", DbType.Int32, model.ProviceId);
            _db.AddInParameter(dc, "CityId", DbType.Int32, model.CityId);
            _db.AddInParameter(dc, "SaleAreadId", DbType.Int32, model.SaleAreadId);
            _db.AddInParameter(dc, "CustomerName", DbType.String, model.CustomerName);
            _db.AddInParameter(dc, "Licence", DbType.String, model.Licence);
            _db.AddInParameter(dc, "Address", DbType.String, model.Address);
            _db.AddInParameter(dc, "PostalCode", DbType.String, model.PostalCode);
            _db.AddInParameter(dc, "BankCode", DbType.String, model.BankCode);
            _db.AddInParameter(dc, "ContactName", DbType.String, model.ContactName);
            _db.AddInParameter(dc, "Phone", DbType.String, model.Phone);
            _db.AddInParameter(dc, "Mobile", DbType.String, model.Mobile);
            _db.AddInParameter(dc, "Fax", DbType.String, model.Fax);
            _db.AddInParameter(dc, "Remark", DbType.String, model.Remark);
            _db.AddInParameter(dc, "OperatorId", DbType.Int32, model.OperatorId);
            _db.AddInParameter(dc, "CustomerRating", DbType.Int32, (int)model.CustomerRating);
            return DbHelper.ExecuteSqlTrans(dc, _db) > 0 ? 1 : -1;
        }

        /// <summary>
        /// 删除客户资料
        /// </summary>
        /// <param name="id">客户编号</param>
        /// <returns>
        /// 1：成功；
        /// 0：参数错误；
        /// -1：客户已被使用，不能删除；
        /// -2：删除失败；
        /// </returns>
        public int DeleteCustomer(params string[] id)
        {
            if (id == null || id.Length <= 0) return 0;

            var strSql = new StringBuilder();
            var strSqlDel = new StringBuilder();
            strSqlDel.Append(" update tbl_Customer set IsDelete = '1' where ");
            strSql.Append(" declare @index int; ");
            strSql.Append(" declare @result int; ");
            strSql.Append(" set @result = -1; ");
            strSql.Append(" set @index = 0; ");
            if (id.Length == 1)
            {
                strSqlDel.AppendFormat(" Id = '{0}'; ", Utils.ToSqlLike(id[0]));

                strSql.AppendFormat(
                    " select @index = count(*) from tbl_CustomerCarefor where CustomerId = '{0}'; ",
                    Utils.ToSqlLike(id[0]));
                strSql.Append(" set @index = isnull(@index,0); ");
                strSql.AppendFormat(
                    " select @index = @index + isnull(count(*),0) from tbl_Tour where BuyCompanyId = '{0}'; ", Utils.ToSqlLike(id[0]));
            }
            else
            {
                strSqlDel.AppendFormat(" Id in ({0}); ", this.GetIdsByArr(id));

                strSql.AppendFormat(
                    " select @index = count(*) from tbl_CustomerCarefor where CustomerId in ({0}); ",
                    this.GetIdsByArr(id));
                strSql.Append(" set @index = isnull(@index,0); ");
                strSql.AppendFormat(
                    " select @index = @index + isnull(count(*),0) from tbl_Tour where BuyCompanyId in ({0}); ", this.GetIdsByArr(id));
            }
            strSql.Append(" if @index is null or @index <= 0 ");
            strSql.Append(" begin ");
            strSql.Append(strSqlDel.ToString());
            strSql.Append(" set @result = 1; ");
            strSql.Append(" end ");
            strSql.Append(" select @result; ");

            DbCommand dc = _db.GetSqlStringCommand(strSql.ToString());

            object obj = DbHelper.GetSingleBySqlTrans(dc, _db);

            if (obj == null) return -2;

            return Utils.GetInt(obj.ToString());
        }

        /// <summary>
        /// 获取客户资料
        /// </summary>
        /// <param name="id">客户编号</param>
        /// <returns></returns>
        public MCustomer GetCustomer(string id)
        {
            if (string.IsNullOrEmpty(id)) return null;

            var strSql = new StringBuilder();

            strSql.Append(
                " SELECT [Id],[CompanyId],[ProviceId],[CityId],[SaleAreadId],[CustomerType],[CustomerName],[Licence],[Address],[PostalCode],[BankCode],[ContactName],[Phone],[Mobile],[Fax],[Remark],[OperatorId],[IssueTime],[CustomerRating]");
            strSql.Append(
                " ,(select ProvinceName from tbl_CompanyProvince as a where a.Id = ProviceId) as ProvinceName  ");
            strSql.Append(
                " ,(select CityName from tbl_CompanyCity as b where b.Id = CityId) as CityName  ");
            strSql.Append(" FROM [tbl_Customer] where Id = @Id and IsDelete = '0'; ");
            strSql.Append(
                " SELECT [Id],[CustomerId],[CompanyId],[Name],[Sex],[DepartmentId],[JobId],[Fax],[Tel],[Mobile],[qq],[BirthDay],[Email],[Remark] ");
            strSql.Append(" FROM [tbl_CustomerContactInfo] where CustomerId = @Id; ");

            DbCommand dc = _db.GetSqlStringCommand(strSql.ToString());
            _db.AddInParameter(dc, "Id", DbType.AnsiStringFixedLength, id);

            MCustomer model = null;
            using (IDataReader dr = DbHelper.ExecuteReader(dc, _db))
            {
                #region 客户信息

                if (dr.Read())
                {
                    model = new MCustomer
                        {
                            Id = dr.IsDBNull(dr.GetOrdinal("Id")) ? string.Empty : dr.GetString(dr.GetOrdinal("Id")),
                            CompanyId =
                                dr.IsDBNull(dr.GetOrdinal("CompanyId")) ? 0 : dr.GetInt32(dr.GetOrdinal("CompanyId")),
                            ProviceId =
                                dr.IsDBNull(dr.GetOrdinal("ProviceId")) ? 0 : dr.GetInt32(dr.GetOrdinal("ProviceId")),
                            CityId = dr.IsDBNull(dr.GetOrdinal("CityId")) ? 0 : dr.GetInt32(dr.GetOrdinal("CityId")),
                            SaleAreadId =
                                dr.IsDBNull(dr.GetOrdinal("SaleAreadId")) ? 0 : dr.GetInt32(dr.GetOrdinal("SaleAreadId")),
                            CustomerName =
                                dr.IsDBNull(dr.GetOrdinal("CustomerName"))
                                    ? string.Empty
                                    : dr.GetString(dr.GetOrdinal("CustomerName")),
                            Licence =
                                dr.IsDBNull(dr.GetOrdinal("Licence"))
                                    ? string.Empty
                                    : dr.GetString(dr.GetOrdinal("Licence")),
                            Address =
                                dr.IsDBNull(dr.GetOrdinal("Address"))
                                    ? string.Empty
                                    : dr.GetString(dr.GetOrdinal("Address")),
                            PostalCode =
                                dr.IsDBNull(dr.GetOrdinal("PostalCode"))
                                    ? string.Empty
                                    : dr.GetString(dr.GetOrdinal("PostalCode")),
                            BankCode =
                                dr.IsDBNull(dr.GetOrdinal("BankCode"))
                                    ? string.Empty
                                    : dr.GetString(dr.GetOrdinal("BankCode")),
                            ContactName =
                                dr.IsDBNull(dr.GetOrdinal("ContactName"))
                                    ? string.Empty
                                    : dr.GetString(dr.GetOrdinal("ContactName")),
                            Phone =
                                dr.IsDBNull(dr.GetOrdinal("Phone")) ? string.Empty : dr.GetString(dr.GetOrdinal("Phone")),
                            Mobile =
                                dr.IsDBNull(dr.GetOrdinal("Mobile"))
                                    ? string.Empty
                                    : dr.GetString(dr.GetOrdinal("Mobile")),
                            Fax = dr.IsDBNull(dr.GetOrdinal("Fax")) ? string.Empty : dr.GetString(dr.GetOrdinal("Fax")),
                            Remark =
                                dr.IsDBNull(dr.GetOrdinal("Remark"))
                                    ? string.Empty
                                    : dr.GetString(dr.GetOrdinal("Remark")),
                            OperatorId =
                                dr.IsDBNull(dr.GetOrdinal("OperatorId")) ? 0 : dr.GetInt32(dr.GetOrdinal("OperatorId")),
                            ProvinceName =
                            dr.IsDBNull(dr.GetOrdinal("ProvinceName"))
                                ? string.Empty
                                : dr.GetString(dr.GetOrdinal("ProvinceName")),
                            CityName =
                            dr.IsDBNull(dr.GetOrdinal("CityName"))
                                ? string.Empty
                                : dr.GetString(dr.GetOrdinal("CityName")),
                            CustomerRating = (EyouSoft.Model.EnumType.CustomerStructure.CustomerRating)dr.GetByte(dr.GetOrdinal("CustomerRating"))
                        };
                    if (!dr.IsDBNull(dr.GetOrdinal("CustomerType")))
                    {
                        model.CustomerType =
                            (Model.EnumType.CustomerStructure.CustomerType)dr.GetByte(dr.GetOrdinal("CustomerType"));
                    }
                    if (!dr.IsDBNull(dr.GetOrdinal("IssueTime")))
                    {
                        model.IssueTime = dr.GetDateTime(dr.GetOrdinal("IssueTime"));
                    }
                }

                #endregion

                #region 客户联系人信息

                if (model != null)
                {
                    dr.NextResult();
                    model.Contact = new List<MCustomerContact>();
                    while (dr.Read())
                    {
                        var tmp = new MCustomerContact
                            {
                                Id = dr.IsDBNull(dr.GetOrdinal("Id")) ? 0 : dr.GetInt32(dr.GetOrdinal("Id")),
                                CustomerId =
                                    dr.IsDBNull(dr.GetOrdinal("CustomerId"))
                                        ? string.Empty
                                        : dr.GetString(dr.GetOrdinal("CustomerId")),
                                CompanyId =
                                    dr.IsDBNull(dr.GetOrdinal("CompanyId")) ? 0 : dr.GetInt32(dr.GetOrdinal("CompanyId")),
                                Name =
                                    dr.IsDBNull(dr.GetOrdinal("Name"))
                                        ? string.Empty
                                        : dr.GetString(dr.GetOrdinal("Name")),
                                DepartmentName =
                                    dr.IsDBNull(dr.GetOrdinal("DepartmentId"))
                                        ? string.Empty
                                        : dr.GetString(dr.GetOrdinal("DepartmentId")),
                                Job =
                                    dr.IsDBNull(dr.GetOrdinal("JobId"))
                                        ? string.Empty
                                        : dr.GetString(dr.GetOrdinal("JobId")),
                                Fax =
                                    dr.IsDBNull(dr.GetOrdinal("Fax")) ? string.Empty : dr.GetString(dr.GetOrdinal("Fax")),
                                Tel =
                                    dr.IsDBNull(dr.GetOrdinal("Tel")) ? string.Empty : dr.GetString(dr.GetOrdinal("Tel")),
                                Mobile =
                                    dr.IsDBNull(dr.GetOrdinal("Mobile"))
                                        ? string.Empty
                                        : dr.GetString(dr.GetOrdinal("Mobile")),
                                Qq = dr.IsDBNull(dr.GetOrdinal("qq")) ? string.Empty : dr.GetString(dr.GetOrdinal("qq")),
                                Email =
                                    dr.IsDBNull(dr.GetOrdinal("Email"))
                                        ? string.Empty
                                        : dr.GetString(dr.GetOrdinal("Email")),
                                Remark =
                                    dr.IsDBNull(dr.GetOrdinal("Remark"))
                                        ? string.Empty
                                        : dr.GetString(dr.GetOrdinal("Remark"))
                            };
                        if (!dr.IsDBNull(dr.GetOrdinal("Sex")))
                        {
                            tmp.Sex =
                                (Model.EnumType.CompanyStructure.Sex)Utils.GetInt(dr.GetString(dr.GetOrdinal("Sex")));
                        }
                        if (!dr.IsDBNull(dr.GetOrdinal("BirthDay")))
                        {
                            tmp.BirthDay = dr.GetDateTime(dr.GetOrdinal("BirthDay"));
                        }

                        model.Contact.Add(tmp);
                    }
                }

                #endregion
            }

            return model;
        }

        /// <summary>
        /// 获取客户资料
        /// </summary>
        /// <param name="companyId">公司编号</param>
        /// <param name="pageSize">每页记录数</param>
        /// <param name="pageIndex">当前页数</param>
        /// <param name="recordCount">总记录数</param>
        /// <param name="seach">客户资料查询实体</param>
        /// <returns></returns>
        public IList<MCustomer> GetCustomer(int companyId, int pageSize, int pageIndex, ref int recordCount, MSearchCustomer seach)
        {
            if (companyId <= 0 || pageSize <= 0 || pageIndex <= 0) return null;

            IList<MCustomer> list;
            string tableName = "tbl_Customer";
            var fileds = new StringBuilder("[Id],[CompanyId],[ProviceId],[CityId],[SaleAreadId],[CustomerType],[CustomerName],[Licence],[Address],[PostalCode],[BankCode],[ContactName],[Phone],[Mobile],[Fax],[Remark],[OperatorId],[IssueTime],[CustomerRating],[RatingId] ");
            fileds.Append(",(select RatingName from tbl_YingyongRating as c where c.Id=RatingId) as RantingName");
            fileds.Append(" ,(select ProvinceName from tbl_CompanyProvince as a where a.Id = ProviceId) as ProvinceName ");
            fileds.Append(" ,(select CityName from tbl_CompanyCity as b where b.Id = CityId) as CityName ");
            string orderbyStr = " IssueTime desc ";
            var strWhere = new StringBuilder();
            strWhere.AppendFormat(" CompanyId = {0} and IsDelete = '0' ", companyId);

            #region 查询条件

            if (seach != null)
            {
                if (seach.ProvinceId != null && seach.ProvinceId.Any())
                {
                    if (seach.ProvinceId.Length == 1)
                    {
                        strWhere.AppendFormat(" and ProviceId = {0} ", seach.ProvinceId[0]);
                    }
                    else
                    {
                        strWhere.AppendFormat(" and ProviceId in ({0}) ", this.GetIdsByArr(seach.ProvinceId));
                    }
                }
                if (seach.CityId != null && seach.CityId.Any())
                {
                    if (seach.CityId.Length == 1)
                    {
                        strWhere.AppendFormat(" and CityId = {0} ", seach.CityId[0]);
                    }
                    else
                    {
                        strWhere.AppendFormat(" and CityId in ({0}) ", this.GetIdsByArr(seach.CityId));
                    }
                }
                if (!string.IsNullOrEmpty(seach.CustomerName))
                {
                    strWhere.AppendFormat(" and CustomerName like '%{0}%' ", Utils.ToSqlLike(seach.CustomerName));
                }
                if (!string.IsNullOrEmpty(seach.ContactName))
                {
                    strWhere.Append(" and ( ");
                    strWhere.AppendFormat(" ContactName like '%{0}%' ", Utils.ToSqlLike(seach.ContactName));
                    strWhere.Append(" or ");
                    strWhere.AppendFormat(
                        " exists (select 1 from tbl_CustomerContactInfo as c where c.CustomerId = tbl_Customer.Id and c.Name like '%{0}%') ",
                        Utils.ToSqlLike(seach.ContactName));
                    strWhere.Append(" ) ");
                }
                if (!string.IsNullOrEmpty(seach.Phone))
                {
                    strWhere.Append(" and ( ");
                    strWhere.AppendFormat(" Phone like '%{0}%' ", Utils.ToSqlLike(seach.Phone));
                    strWhere.Append(" or ");
                    strWhere.AppendFormat(
                        " exists (select 1 from tbl_CustomerContactInfo as d where d.CustomerId = tbl_Customer.Id and d.Tel like '%{0}%') ",
                        Utils.ToSqlLike(seach.Phone));
                    strWhere.Append(" ) ");
                }
                if (!string.IsNullOrEmpty(seach.Address))
                {
                    strWhere.AppendFormat(" and Address like '%{0}%' ", Utils.ToSqlLike(seach.Address));
                }
                if (!string.IsNullOrEmpty(seach.Mobile))
                {
                    strWhere.Append(" and ( ");
                    strWhere.AppendFormat(" Mobile like '%{0}%' ", Utils.ToSqlLike(seach.Mobile));
                    strWhere.Append(" or ");
                    strWhere.AppendFormat(
                        " exists (select 1 from tbl_CustomerContactInfo as d where d.CustomerId = tbl_Customer.Id and d.Mobile like '%{0}%') ",
                        Utils.ToSqlLike(seach.Mobile));
                    strWhere.Append(" ) ");
                }

                if (seach.CustomerRating.HasValue)
                {

                    strWhere.AppendFormat(" and CustomerRating={0} ", (int)seach.CustomerRating);
                }

                if (!string.IsNullOrEmpty(seach.RatingId.ToString())&&seach.RatingId.ToString()!=""&seach.RatingId>0)
                {
                    strWhere.AppendFormat(" and RatingId={0} ", (int)seach.RatingId);
                }
            }

            #endregion

            using (IDataReader dr = DbHelper.ExecuteReader1(_db, pageSize, pageIndex, ref recordCount, tableName, fileds.ToString()
                , strWhere.ToString(), orderbyStr, string.Empty))
            {
                list = new List<MCustomer>();
                while (dr.Read())
                {
                    #region 实体赋值

                    var model = new MCustomer
                    {
                        Id = dr.IsDBNull(dr.GetOrdinal("Id")) ? string.Empty : dr.GetString(dr.GetOrdinal("Id")),
                        CompanyId =
                            dr.IsDBNull(dr.GetOrdinal("CompanyId")) ? 0 : dr.GetInt32(dr.GetOrdinal("CompanyId")),
                        ProviceId =
                            dr.IsDBNull(dr.GetOrdinal("ProviceId")) ? 0 : dr.GetInt32(dr.GetOrdinal("ProviceId")),
                        CityId = dr.IsDBNull(dr.GetOrdinal("CityId")) ? 0 : dr.GetInt32(dr.GetOrdinal("CityId")),
                        SaleAreadId =
                            dr.IsDBNull(dr.GetOrdinal("SaleAreadId")) ? 0 : dr.GetInt32(dr.GetOrdinal("SaleAreadId")),
                        CustomerName =
                            dr.IsDBNull(dr.GetOrdinal("CustomerName"))
                                ? string.Empty
                                : dr.GetString(dr.GetOrdinal("CustomerName")),
                        Licence =
                            dr.IsDBNull(dr.GetOrdinal("Licence"))
                                ? string.Empty
                                : dr.GetString(dr.GetOrdinal("Licence")),
                        Address =
                            dr.IsDBNull(dr.GetOrdinal("Address"))
                                ? string.Empty
                                : dr.GetString(dr.GetOrdinal("Address")),
                        PostalCode =
                            dr.IsDBNull(dr.GetOrdinal("PostalCode"))
                                ? string.Empty
                                : dr.GetString(dr.GetOrdinal("PostalCode")),
                        BankCode =
                            dr.IsDBNull(dr.GetOrdinal("BankCode"))
                                ? string.Empty
                                : dr.GetString(dr.GetOrdinal("BankCode")),
                        ContactName =
                            dr.IsDBNull(dr.GetOrdinal("ContactName"))
                                ? string.Empty
                                : dr.GetString(dr.GetOrdinal("ContactName")),
                        Phone =
                            dr.IsDBNull(dr.GetOrdinal("Phone")) ? string.Empty : dr.GetString(dr.GetOrdinal("Phone")),
                        Mobile =
                            dr.IsDBNull(dr.GetOrdinal("Mobile"))
                                ? string.Empty
                                : dr.GetString(dr.GetOrdinal("Mobile")),
                        Fax = dr.IsDBNull(dr.GetOrdinal("Fax")) ? string.Empty : dr.GetString(dr.GetOrdinal("Fax")),
                        Remark =
                            dr.IsDBNull(dr.GetOrdinal("Remark"))
                                ? string.Empty
                                : dr.GetString(dr.GetOrdinal("Remark")),
                        OperatorId =
                            dr.IsDBNull(dr.GetOrdinal("OperatorId")) ? 0 : dr.GetInt32(dr.GetOrdinal("OperatorId")),
                        ProvinceName =
                        dr.IsDBNull(dr.GetOrdinal("ProvinceName"))
                            ? string.Empty
                            : dr.GetString(dr.GetOrdinal("ProvinceName")),
                        CityName =
                        dr.IsDBNull(dr.GetOrdinal("CityName"))
                            ? string.Empty
                            : dr.GetString(dr.GetOrdinal("CityName")),
                        CustomerRating = (EyouSoft.Model.EnumType.CustomerStructure.CustomerRating)dr.GetByte(dr.GetOrdinal("CustomerRating")),
                        RatingId =
                           dr.IsDBNull(dr.GetOrdinal("RatingId"))
                           ? 0 : dr.GetInt32(dr.GetOrdinal("RatingId")),
                        RatingName = dr.IsDBNull(dr.GetOrdinal("RantingName"))
                                ? string.Empty
                                : dr.GetString(dr.GetOrdinal("RantingName"))
                    };
                    if (!dr.IsDBNull(dr.GetOrdinal("CustomerType")))
                    {
                        model.CustomerType =
                            (Model.EnumType.CustomerStructure.CustomerType)dr.GetByte(dr.GetOrdinal("CustomerType"));
                    }
                    if (!dr.IsDBNull(dr.GetOrdinal("IssueTime")))
                    {
                        model.IssueTime = dr.GetDateTime(dr.GetOrdinal("IssueTime"));
                    }

                    #endregion

                    list.Add(model);
                }
            }

            return list;
        }





        /// <summary>
        /// 获取客户资料
        /// </summary>
        /// <param name="companyId">公司编号</param>
        /// <param name="pageSize">每页记录数</param>
        /// <param name="pageIndex">当前页数</param>
        /// <param name="recordCount">总记录数</param>
        /// <param name="seach">客户资料查询实体</param>
        /// <returns></returns>
        public IList<MCustomer> GetCustomerContactInfo(int companyId, int pageSize, int pageIndex, ref int recordCount, MSearchCustomer seach)
        {
            if (companyId <= 0 || pageSize <= 0 || pageIndex <= 0) return null;

            IList<MCustomer> list;
            string tableName = "view_CustomerContact";
            var fileds = new StringBuilder("[Id],[CompanyId],[ProviceId],[CityId],[SaleAreadId],[CustomerType],[CustomerName],[Licence],[Address],[PostalCode],[BankCode],[ContactName],[Phone],[Mobile],[Fax],[Remark],[OperatorId],[IssueTime],[CustomerRating]");
            fileds.Append(" ,(select ProvinceName from tbl_CompanyProvince as a where a.Id = ProviceId) as ProvinceName ");
            fileds.Append(" ,(select CityName from tbl_CompanyCity as b where b.Id = CityId) as CityName ");
            string orderbyStr = " CustomerName asc ";
            var strWhere = new StringBuilder();
            strWhere.AppendFormat(" CompanyId = {0} and IsDelete = '0' ", companyId);

            #region 查询条件

            if (seach != null)
            {
                if (seach.ProvinceId != null && seach.ProvinceId.Any())
                {
                    if (seach.ProvinceId.Length == 1)
                    {
                        strWhere.AppendFormat(" and ProviceId = {0} ", seach.ProvinceId[0]);
                    }
                    else
                    {
                        strWhere.AppendFormat(" and ProviceId in ({0}) ", this.GetIdsByArr(seach.ProvinceId));
                    }
                }
                if (seach.CityId != null && seach.CityId.Any())
                {
                    if (seach.CityId.Length == 1)
                    {
                        strWhere.AppendFormat(" and CityId = {0} ", seach.CityId[0]);
                    }
                    else
                    {
                        strWhere.AppendFormat(" and CityId in ({0}) ", this.GetIdsByArr(seach.CityId));
                    }
                }
                if (!string.IsNullOrEmpty(seach.CustomerName))
                {
                    strWhere.AppendFormat(" and CustomerName like '%{0}%' ", Utils.ToSqlLike(seach.CustomerName));
                }
                if (!string.IsNullOrEmpty(seach.ContactName))
                {
                    strWhere.Append(" and ( ");
                    strWhere.AppendFormat(" ContactName like '%{0}%' ", Utils.ToSqlLike(seach.ContactName));
                    strWhere.Append(" or ");
                    strWhere.AppendFormat(
                        " exists (select 1 from tbl_CustomerContactInfo as c where c.CustomerId = tbl_Customer.Id and c.Name like '%{0}%') ",
                        Utils.ToSqlLike(seach.ContactName));
                    strWhere.Append(" ) ");
                }
                if (!string.IsNullOrEmpty(seach.Phone))
                {
                    strWhere.Append(" and ( ");
                    strWhere.AppendFormat(" Phone like '%{0}%' ", Utils.ToSqlLike(seach.Phone));
                    strWhere.Append(" or ");
                    strWhere.AppendFormat(
                        " exists (select 1 from tbl_CustomerContactInfo as d where d.CustomerId = tbl_Customer.Id and d.Tel like '%{0}%') ",
                        Utils.ToSqlLike(seach.Phone));
                    strWhere.Append(" ) ");
                }
                if (!string.IsNullOrEmpty(seach.Address))
                {
                    strWhere.AppendFormat(" and Address like '%{0}%' ", Utils.ToSqlLike(seach.Address));
                }
                if (!string.IsNullOrEmpty(seach.Mobile))
                {
                    strWhere.Append(" and ( ");
                    strWhere.AppendFormat(" Mobile like '%{0}%' ", Utils.ToSqlLike(seach.Mobile));
                    strWhere.Append(" or ");
                    strWhere.AppendFormat(
                        " exists (select 1 from tbl_CustomerContactInfo as d where d.CustomerId = tbl_Customer.Id and d.Mobile like '%{0}%') ",
                        Utils.ToSqlLike(seach.Mobile));
                    strWhere.Append(" ) ");
                }

                if (seach.CustomerRating.HasValue)
                {

                    strWhere.AppendFormat(" and CustomerRating={0} ", (int)seach.CustomerRating);
                }
            }

            #endregion

            using (IDataReader dr = DbHelper.ExecuteReader1(_db, pageSize, pageIndex, ref recordCount, tableName, fileds.ToString()
                , strWhere.ToString(), orderbyStr, string.Empty))
            {
                list = new List<MCustomer>();
                while (dr.Read())
                {
                    #region 实体赋值

                    var model = new MCustomer
                    {
                        Id = dr.IsDBNull(dr.GetOrdinal("Id")) ? string.Empty : dr.GetString(dr.GetOrdinal("Id")),
                        CompanyId =
                            dr.IsDBNull(dr.GetOrdinal("CompanyId")) ? 0 : dr.GetInt32(dr.GetOrdinal("CompanyId")),
                        ProviceId =
                            dr.IsDBNull(dr.GetOrdinal("ProviceId")) ? 0 : dr.GetInt32(dr.GetOrdinal("ProviceId")),
                        CityId = dr.IsDBNull(dr.GetOrdinal("CityId")) ? 0 : dr.GetInt32(dr.GetOrdinal("CityId")),
                        SaleAreadId =
                            dr.IsDBNull(dr.GetOrdinal("SaleAreadId")) ? 0 : dr.GetInt32(dr.GetOrdinal("SaleAreadId")),
                        CustomerName =
                            dr.IsDBNull(dr.GetOrdinal("CustomerName"))
                                ? string.Empty
                                : dr.GetString(dr.GetOrdinal("CustomerName")),
                        Licence =
                            dr.IsDBNull(dr.GetOrdinal("Licence"))
                                ? string.Empty
                                : dr.GetString(dr.GetOrdinal("Licence")),
                        Address =
                            dr.IsDBNull(dr.GetOrdinal("Address"))
                                ? string.Empty
                                : dr.GetString(dr.GetOrdinal("Address")),
                        PostalCode =
                            dr.IsDBNull(dr.GetOrdinal("PostalCode"))
                                ? string.Empty
                                : dr.GetString(dr.GetOrdinal("PostalCode")),
                        BankCode =
                            dr.IsDBNull(dr.GetOrdinal("BankCode"))
                                ? string.Empty
                                : dr.GetString(dr.GetOrdinal("BankCode")),
                        ContactName =
                            dr.IsDBNull(dr.GetOrdinal("ContactName"))
                                ? string.Empty
                                : dr.GetString(dr.GetOrdinal("ContactName")),
                        Phone =
                            dr.IsDBNull(dr.GetOrdinal("Phone")) ? string.Empty : dr.GetString(dr.GetOrdinal("Phone")),
                        Mobile =
                            dr.IsDBNull(dr.GetOrdinal("Mobile"))
                                ? string.Empty
                                : dr.GetString(dr.GetOrdinal("Mobile")),
                        Fax = dr.IsDBNull(dr.GetOrdinal("Fax")) ? string.Empty : dr.GetString(dr.GetOrdinal("Fax")),
                        Remark =
                            dr.IsDBNull(dr.GetOrdinal("Remark"))
                                ? string.Empty
                                : dr.GetString(dr.GetOrdinal("Remark")),
                        OperatorId =
                            dr.IsDBNull(dr.GetOrdinal("OperatorId")) ? 0 : dr.GetInt32(dr.GetOrdinal("OperatorId")),
                        ProvinceName =
                        dr.IsDBNull(dr.GetOrdinal("ProvinceName"))
                            ? string.Empty
                            : dr.GetString(dr.GetOrdinal("ProvinceName")),
                        CityName =
                        dr.IsDBNull(dr.GetOrdinal("CityName"))
                            ? string.Empty
                            : dr.GetString(dr.GetOrdinal("CityName")),
                        CustomerRating = (EyouSoft.Model.EnumType.CustomerStructure.CustomerRating)dr.GetByte(dr.GetOrdinal("CustomerRating"))
                    };
                    if (!dr.IsDBNull(dr.GetOrdinal("CustomerType")))
                    {
                        model.CustomerType =
                            (Model.EnumType.CustomerStructure.CustomerType)dr.GetByte(dr.GetOrdinal("CustomerType"));
                    }
                    if (!dr.IsDBNull(dr.GetOrdinal("IssueTime")))
                    {
                        model.IssueTime = dr.GetDateTime(dr.GetOrdinal("IssueTime"));
                    }

                    #endregion

                    list.Add(model);
                }
            }

            return list;
        }

        /// <summary>
        /// 获取客户资料
        /// </summary>
        /// <param name="companyId">公司编号</param>
        /// <param name="seach">客户资料查询实体</param>
        /// <returns></returns>
        public IList<MCustomer> GetCustomer(int companyId, MSearchCustomer seach)
        {
            if (companyId <= 0) return null;

            IList<MCustomer> list;
            var strSql = new StringBuilder();
            strSql.Append(" select  ");
            strSql.Append(
                " [Id],[CompanyId],[ProviceId],[CityId],[SaleAreadId],[CustomerType],[CustomerName],[Licence],[Address],[PostalCode],[BankCode],[ContactName],[Phone],[Mobile],[Fax],[Remark],[OperatorId],[IssueTime],[CustomerRating],[RatingId]");
            strSql.Append(",(select RatingName from tbl_YingyongRating as c where c.Id=RatingId) as RantingName");
            strSql.Append(" ,(select ProvinceName from tbl_CompanyProvince as a where a.Id = ProviceId) as ProvinceName ");
            strSql.Append(" ,(select CityName from tbl_CompanyCity as b where b.Id = CityId) as CityName ");
            strSql.Append(" from tbl_Customer where ");
            strSql.AppendFormat(" CompanyId = {0} and IsDelete = '0' ", companyId);

            #region 查询条件

            if (seach != null)
            {
                if (seach.ProvinceId != null && seach.ProvinceId.Any())
                {
                    if (seach.ProvinceId.Length == 1)
                    {
                        strSql.AppendFormat(" and ProviceId = {0} ", seach.ProvinceId[0]);
                    }
                    else
                    {
                        strSql.AppendFormat(" and ProviceId in ({0}) ", this.GetIdsByArr(seach.ProvinceId));
                    }
                }
                if (seach.CityId != null && seach.CityId.Any())
                {
                    if (seach.CityId.Length == 1)
                    {
                        strSql.AppendFormat(" and CityId = {0} ", seach.CityId[0]);
                    }
                    else
                    {
                        strSql.AppendFormat(" and CityId in ({0}) ", this.GetIdsByArr(seach.CityId));
                    }
                }
                if (!string.IsNullOrEmpty(seach.CustomerName))
                {
                    strSql.AppendFormat(" and CustomerName like '%{0}%' ", Utils.ToSqlLike(seach.CustomerName));
                }
                if (!string.IsNullOrEmpty(seach.ContactName))
                {
                    strSql.Append(" and ( ");
                    strSql.AppendFormat(" ContactName like '%{0}%' ", Utils.ToSqlLike(seach.ContactName));
                    strSql.Append(" or ");
                    strSql.AppendFormat(
                        " exists (select 1 from tbl_CustomerContactInfo as c where c.CustomerId = tbl_Customer.Id and c.Name like '%{0}%') ",
                        Utils.ToSqlLike(seach.ContactName));
                    strSql.Append(" ) ");
                }
                if (!string.IsNullOrEmpty(seach.Phone))
                {
                    strSql.Append(" and ( ");
                    strSql.AppendFormat(" Phone like '%{0}%' ", Utils.ToSqlLike(seach.Phone));
                    strSql.Append(" or ");
                    strSql.AppendFormat(
                        " exists (select 1 from tbl_CustomerContactInfo as d where d.CustomerId = tbl_Customer.Id and d.Tel like '%{0}%') ",
                        Utils.ToSqlLike(seach.ContactName));
                    strSql.Append(" ) ");
                }
                if (!string.IsNullOrEmpty(seach.Address))
                {
                    strSql.AppendFormat(" and Address like '%{0}%' ", Utils.ToSqlLike(seach.Address));
                }
                if (!string.IsNullOrEmpty(seach.Phone))
                {
                    strSql.Append(" and ( ");
                    strSql.AppendFormat(" Mobile like '%{0}%' ", Utils.ToSqlLike(seach.Phone));
                    strSql.Append(" or ");
                    strSql.AppendFormat(
                        " exists (select 1 from tbl_CustomerContactInfo as d where d.CustomerId = tbl_Customer.Id and d.Mobile like '%{0}%') ",
                        Utils.ToSqlLike(seach.ContactName));
                    strSql.Append(" ) ");
                }

                if (seach.CustomerRating.HasValue)
                {
                    strSql.AppendFormat(" and CustomerRating={0} ", (int)seach.CustomerRating.Value);
                }
            }

            #endregion

            strSql.Append(" order by IssueTime desc ");

            DbCommand dc = _db.GetSqlStringCommand(strSql.ToString());

            using (IDataReader dr = DbHelper.ExecuteReader(dc, _db))
            {
                list = new List<MCustomer>();
                while (dr.Read())
                {
                    #region 实体赋值

                    var model = new MCustomer
                    {
                        Id = dr.IsDBNull(dr.GetOrdinal("Id")) ? string.Empty : dr.GetString(dr.GetOrdinal("Id")),
                        CompanyId =
                            dr.IsDBNull(dr.GetOrdinal("CompanyId")) ? 0 : dr.GetInt32(dr.GetOrdinal("CompanyId")),
                        ProviceId =
                            dr.IsDBNull(dr.GetOrdinal("ProviceId")) ? 0 : dr.GetInt32(dr.GetOrdinal("ProviceId")),
                        CityId = dr.IsDBNull(dr.GetOrdinal("CityId")) ? 0 : dr.GetInt32(dr.GetOrdinal("CityId")),
                        SaleAreadId =
                            dr.IsDBNull(dr.GetOrdinal("SaleAreadId")) ? 0 : dr.GetInt32(dr.GetOrdinal("SaleAreadId")),
                        CustomerName =
                            dr.IsDBNull(dr.GetOrdinal("CustomerName"))
                                ? string.Empty
                                : dr.GetString(dr.GetOrdinal("CustomerName")),
                        Licence =
                            dr.IsDBNull(dr.GetOrdinal("Licence"))
                                ? string.Empty
                                : dr.GetString(dr.GetOrdinal("Licence")),
                        Address =
                            dr.IsDBNull(dr.GetOrdinal("Address"))
                                ? string.Empty
                                : dr.GetString(dr.GetOrdinal("Address")),
                        PostalCode =
                            dr.IsDBNull(dr.GetOrdinal("PostalCode"))
                                ? string.Empty
                                : dr.GetString(dr.GetOrdinal("PostalCode")),
                        BankCode =
                            dr.IsDBNull(dr.GetOrdinal("BankCode"))
                                ? string.Empty
                                : dr.GetString(dr.GetOrdinal("BankCode")),
                        ContactName =
                            dr.IsDBNull(dr.GetOrdinal("ContactName"))
                                ? string.Empty
                                : dr.GetString(dr.GetOrdinal("ContactName")),
                        Phone =
                            dr.IsDBNull(dr.GetOrdinal("Phone")) ? string.Empty : dr.GetString(dr.GetOrdinal("Phone")),
                        Mobile =
                            dr.IsDBNull(dr.GetOrdinal("Mobile"))
                                ? string.Empty
                                : dr.GetString(dr.GetOrdinal("Mobile")),
                        Fax = dr.IsDBNull(dr.GetOrdinal("Fax")) ? string.Empty : dr.GetString(dr.GetOrdinal("Fax")),
                        Remark =
                            dr.IsDBNull(dr.GetOrdinal("Remark"))
                                ? string.Empty
                                : dr.GetString(dr.GetOrdinal("Remark")),
                        OperatorId =
                            dr.IsDBNull(dr.GetOrdinal("OperatorId")) ? 0 : dr.GetInt32(dr.GetOrdinal("OperatorId")),
                        ProvinceName =
                        dr.IsDBNull(dr.GetOrdinal("ProvinceName"))
                            ? string.Empty
                            : dr.GetString(dr.GetOrdinal("ProvinceName")),
                        CityName =
                        dr.IsDBNull(dr.GetOrdinal("CityName"))
                            ? string.Empty
                            : dr.GetString(dr.GetOrdinal("CityName")),
                        CustomerRating = (EyouSoft.Model.EnumType.CustomerStructure.CustomerRating)dr.GetByte(dr.GetOrdinal("CustomerRating")),
                        RatingName = dr.IsDBNull(dr.GetOrdinal("RantingName"))
                                ? string.Empty
                                : dr.GetString(dr.GetOrdinal("RantingName"))
                    };
                    if (!dr.IsDBNull(dr.GetOrdinal("CustomerType")))
                    {
                        model.CustomerType =
                            (Model.EnumType.CustomerStructure.CustomerType)dr.GetByte(dr.GetOrdinal("CustomerType"));
                    }
                    if (!dr.IsDBNull(dr.GetOrdinal("IssueTime")))
                    {
                        model.IssueTime = dr.GetDateTime(dr.GetOrdinal("IssueTime"));
                    }

                    #endregion

                    list.Add(model);
                }
            }

            return list;
        }

        /// <summary>
        /// 根据客户编号获取联系人信息（不含主要联系人）
        /// </summary>
        /// <param name="customerId">客户编号</param>
        /// <returns></returns>
        public IList<MCustomerContact> GetCustomerContact(string customerId)
        {
            if (string.IsNullOrEmpty(customerId)) return null;

            var strSql = new StringBuilder();
            strSql.Append(
                " SELECT [Id],[CustomerId],[CompanyId],[Name],[Sex],[DepartmentId],[JobId],[Fax],[Tel],[Mobile],[qq],[BirthDay],[Email],[Remark] ");
            strSql.Append(" FROM [tbl_CustomerContactInfo] where CustomerId = @Id; ");

            DbCommand dc = _db.GetSqlStringCommand(strSql.ToString());
            _db.AddInParameter(dc, "Id", DbType.AnsiStringFixedLength, customerId);

            IList<MCustomerContact> list = null;
            using (IDataReader dr = DbHelper.ExecuteReader(dc, _db))
            {
                list = new List<MCustomerContact>();
                while (dr.Read())
                {
                    var tmp = new MCustomerContact
                    {
                        Id = dr.IsDBNull(dr.GetOrdinal("Id")) ? 0 : dr.GetInt32(dr.GetOrdinal("Id")),
                        CustomerId =
                            dr.IsDBNull(dr.GetOrdinal("CustomerId"))
                                ? string.Empty
                                : dr.GetString(dr.GetOrdinal("CustomerId")),
                        CompanyId =
                            dr.IsDBNull(dr.GetOrdinal("CompanyId")) ? 0 : dr.GetInt32(dr.GetOrdinal("CompanyId")),
                        Name =
                            dr.IsDBNull(dr.GetOrdinal("Name"))
                                ? string.Empty
                                : dr.GetString(dr.GetOrdinal("Name")),
                        DepartmentName =
                            dr.IsDBNull(dr.GetOrdinal("DepartmentId"))
                                ? string.Empty
                                : dr.GetString(dr.GetOrdinal("DepartmentId")),
                        Job =
                            dr.IsDBNull(dr.GetOrdinal("JobId"))
                                ? string.Empty
                                : dr.GetString(dr.GetOrdinal("JobId")),
                        Fax =
                            dr.IsDBNull(dr.GetOrdinal("Fax")) ? string.Empty : dr.GetString(dr.GetOrdinal("Fax")),
                        Tel =
                            dr.IsDBNull(dr.GetOrdinal("Tel")) ? string.Empty : dr.GetString(dr.GetOrdinal("Tel")),
                        Mobile =
                            dr.IsDBNull(dr.GetOrdinal("Mobile"))
                                ? string.Empty
                                : dr.GetString(dr.GetOrdinal("Mobile")),
                        Qq = dr.IsDBNull(dr.GetOrdinal("qq")) ? string.Empty : dr.GetString(dr.GetOrdinal("qq")),
                        Email =
                            dr.IsDBNull(dr.GetOrdinal("Email"))
                                ? string.Empty
                                : dr.GetString(dr.GetOrdinal("Email")),
                        Remark =
                            dr.IsDBNull(dr.GetOrdinal("Remark"))
                                ? string.Empty
                                : dr.GetString(dr.GetOrdinal("Remark"))
                    };
                    if (!dr.IsDBNull(dr.GetOrdinal("Sex")))
                    {
                        tmp.Sex =
                            (Model.EnumType.CompanyStructure.Sex)Utils.GetInt(dr.GetString(dr.GetOrdinal("Sex")));
                    }
                    if (!dr.IsDBNull(dr.GetOrdinal("BirthDay")))
                    {
                        tmp.BirthDay = dr.GetDateTime(dr.GetOrdinal("BirthDay"));
                    }

                    list.Add(tmp);
                }
            }

            return list;
        }

        /// <summary>
        /// 设置客户信用等级
        /// </summary>
        /// <param name="TourId"></param>
        /// <returns></returns>
        public bool SetRating(string cid, int rid, int OperatorId)
        {

            string sql = "UPDATE tbl_Customer SET RatingId = @RatingId,OperatorId = @OperatorId WHERE Id=@Id";
            DbCommand cmd = this._db.GetSqlStringCommand(sql);
            this._db.AddInParameter(cmd, "RatingId", DbType.Int32, rid);
            this._db.AddInParameter(cmd, "OperatorId", DbType.Int32, OperatorId);
            this._db.AddInParameter(cmd, "Id", DbType.AnsiStringFixedLength, cid);

            return DbHelper.ExecuteSql(cmd, this._db) == 1;
        }

        /// <summary>
        /// 判断客户是否存在
        /// </summary>
        /// <param name="customer"></param>
        /// <returns></returns>
        public bool IsExistsCustomerName(string customerName, string customerId)
        {
            string sql = "select top 1 1 from tbl_Customer  where CustomerName=@CustomerName";

            if (!string.IsNullOrEmpty(customerId))
            {
                sql += string.Format(" and Id<>'{0}'", customerId);
            }


            DbCommand cmd = this._db.GetSqlStringCommand(sql);
            this._db.AddInParameter(cmd, "CustomerName", DbType.String, customerName);
            return DbHelper.GetSingle(cmd, this._db) == null ? false : true;
        }

    }
}
