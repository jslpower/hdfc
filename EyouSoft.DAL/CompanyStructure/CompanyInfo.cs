using System;
using System.Data;
using System.Data.Common;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Practices.EnterpriseLibrary.Data;
using EyouSoft.Toolkit.DAL;

namespace EyouSoft.DAL.CompanyStructure
{
    /// <summary>
    /// 专线商公司账户信息DAL
    /// </summary>
    public class CompanyInfo : DALBase, IDAL.CompanyStructure.ICompanyInfo
    {
        private readonly Database _db;

        #region 构造函数
        /// <summary>
        /// 构造函数
        /// </summary>
        public CompanyInfo()
        {
            _db = this.SystemStore;
        }
        #endregion 构造函数

        #region 实现接口公共方法

        /// <summary>
        /// 获取公司信息实体
        /// </summary>
        /// <param name="companyId">公司编号</param>
        /// <param name="systemId">系统编号</param>
        /// <returns></returns>
        public Model.CompanyStructure.CompanyInfo GetModel(int companyId, int systemId)
        {
            Model.CompanyStructure.CompanyInfo model = null;
            var strSql = new StringBuilder();
            strSql.AppendFormat(" SELECT * FROM tbl_CompanyInfo where id = {0} ", companyId);
            if (systemId > 0) strSql.AppendFormat(" AND SystemId = {0} ", systemId);
            strSql.AppendFormat(" ; select FileId,FilePath from tbl_ComapnyFile where CompanyId = {0} order by IssueTime desc ", companyId);
            DbCommand dc = this._db.GetSqlStringCommand(strSql.ToString());
            using (IDataReader dr = DbHelper.ExecuteReader(dc, this._db))
            {
                if (dr.Read())
                {
                    #region 公司信息

                    model = new Model.CompanyStructure.CompanyInfo
                    {
                        Id = dr.GetInt32(dr.GetOrdinal("Id")),
                        CompanyName =
                            dr.IsDBNull(dr.GetOrdinal("CompanyName"))
                                ? ""
                                : dr.GetString(dr.GetOrdinal("CompanyName")),
                        CompanyType =
                            dr.IsDBNull(dr.GetOrdinal("CompanyType"))
                                ? ""
                                : dr.GetString(dr.GetOrdinal("CompanyType")),
                        CompanyEnglishName =
                            dr.IsDBNull(dr.GetOrdinal("CompanyEnglishName"))
                                ? ""
                                : dr.GetString(dr.GetOrdinal("CompanyEnglishName")),
                        License = dr.IsDBNull(dr.GetOrdinal("License")) ? "" : dr.GetString(dr.GetOrdinal("License")),
                        ContactName =
                            dr.IsDBNull(dr.GetOrdinal("ContactName"))
                                ? ""
                                : dr.GetString(dr.GetOrdinal("ContactName")),
                        ContactTel =
                            dr.IsDBNull(dr.GetOrdinal("ContactTel")) ? "" : dr.GetString(dr.GetOrdinal("ContactTel")),
                        ContactMobile =
                            dr.IsDBNull(dr.GetOrdinal("ContactMobile"))
                                ? ""
                                : dr.GetString(dr.GetOrdinal("ContactMobile")),
                        ContactFax =
                            dr.IsDBNull(dr.GetOrdinal("ContactFax")) ? "" : dr.GetString(dr.GetOrdinal("ContactFax")),
                        CompanyAddress =
                            dr.IsDBNull(dr.GetOrdinal("CompanyAddress"))
                                ? ""
                                : dr.GetString(dr.GetOrdinal("CompanyAddress")),
                        CompanyZip =
                            dr.IsDBNull(dr.GetOrdinal("CompanyZip")) ? "" : dr.GetString(dr.GetOrdinal("CompanyZip")),
                        CompanySiteUrl =
                            dr.IsDBNull(dr.GetOrdinal("CompanySiteUrl"))
                                ? ""
                                : dr.GetString(dr.GetOrdinal("CompanySiteUrl")),
                        SystemId = dr.GetInt32(dr.GetOrdinal("SystemId")),
                        IssueTime = dr.GetDateTime(dr.GetOrdinal("IssueTime"))
                    };

                    #endregion
                }

                #region 公司附件信息

                if (model != null)
                {
                    dr.NextResult();
                    model.FilePath = new List<Model.CompanyStructure.CompanyFile>();
                    while (dr.Read())
                    {
                        var file = new Model.CompanyStructure.CompanyFile();
                        if (!dr.IsDBNull(dr.GetOrdinal("FileId")))
                            file.FileId = dr.GetString(dr.GetOrdinal("FileId"));
                        if (!dr.IsDBNull(dr.GetOrdinal("FilePath")))
                            file.FilePath = dr.GetString(dr.GetOrdinal("FilePath"));

                        model.FilePath.Add(file);
                    }
                }

                #endregion
            }
            return model;
        }

        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="model">公司信息实体</param>
        /// <returns></returns>
        public bool Update(Model.CompanyStructure.CompanyInfo model)
        {
            var strSql = new StringBuilder();

            strSql.Append(
                " update tbl_CompanyInfo set CompanyName = @CompanyName,CompanyType = @CompanyType,CompanyEnglishName = @CompanyEnglishName,License = @License,ContactName = @ContactName,ContactTel = @ContactTel,ContactMobile = @ContactMobile,ContactFax = @ContactFax,CompanyAddress = @CompanyAddress,CompanyZip = @CompanyZip,CompanySiteUrl = @CompanySiteUrl where Id = @Id; ");
            strSql.Append(
                " update tbl_Sys set SysName = @CompanyName where SysId = (select top 1 SystemId from tbl_CompanyInfo where tbl_CompanyInfo.Id = @Id); ");

            DbCommand dc = this._db.GetSqlStringCommand(strSql.ToString());
            this._db.AddInParameter(dc, "Id", DbType.Int32, model.Id);
            this._db.AddInParameter(dc, "CompanyName", DbType.String, model.CompanyName);
            this._db.AddInParameter(dc, "CompanyType", DbType.String, model.CompanyType);
            this._db.AddInParameter(dc, "CompanyEnglishName", DbType.String, model.CompanyEnglishName);
            this._db.AddInParameter(dc, "License", DbType.String, model.License);
            this._db.AddInParameter(dc, "ContactName", DbType.String, model.ContactName);
            this._db.AddInParameter(dc, "ContactTel", DbType.AnsiString, model.ContactTel);
            this._db.AddInParameter(dc, "ContactMobile", DbType.AnsiString, model.ContactMobile);
            this._db.AddInParameter(dc, "ContactFax", DbType.AnsiString, model.ContactFax);
            this._db.AddInParameter(dc, "CompanyAddress", DbType.String, model.CompanyAddress);
            this._db.AddInParameter(dc, "CompanyZip", DbType.String, model.CompanyZip);
            this._db.AddInParameter(dc, "CompanySiteUrl", DbType.String, model.CompanySiteUrl);

            return DbHelper.ExecuteSql(dc, _db) > 0 ? true : false;
        }

        /// <summary>
        /// 添加公司福建信息
        /// </summary>
        /// <param name="companyId">公司编号</param>
        /// <param name="list">附件路径集合</param>
        /// <returns>返回1成功，其他失败</returns>
        public int AddCompanyFile(int companyId, IList<string> list)
        {
            if (companyId <= 0 || list == null || !list.Any()) return 0;
            var strSql = new StringBuilder();
            DbCommand dc = _db.GetSqlStringCommand("sssss");
            for (int i = 0; i < list.Count; i++)
            {
                strSql.AppendFormat(
                    " insert into tbl_ComapnyFile (FileId,CompanyId,FilePath) values (@FileId{0},@CompanyId,@FilePath{0}); ",
                    i);
                _db.AddInParameter(
                    dc, string.Format("FileId{0}", i), DbType.AnsiStringFixedLength, Guid.NewGuid().ToString());
                _db.AddInParameter(dc, string.Format("FilePath{0}", i), DbType.String, list[i]);
            }
            _db.AddInParameter(dc, "CompanyId", DbType.Int32, companyId);

            dc.CommandText = strSql.ToString();

            return DbHelper.ExecuteSqlTrans(dc, _db) > 0 ? 1 : -1;
        }

        /// <summary>
        /// 删除公司附件
        /// </summary>
        /// <param name="fileId">附件编号</param>
        /// <returns></returns>
        public int DeleteCompanyFile(params string[] fileId)
        {
            if (fileId == null || fileId.Length <= 0) return 0;

            var strSql = new StringBuilder();
            var strSql2 = new StringBuilder();
            DbCommand dc = _db.GetSqlStringCommand("select 1");
            strSql.Append(" delete from tbl_ComapnyFile ");
            strSql2.Append(" insert into tbl_SysDeletedFileQue (FilePath) ");
            strSql.Append(" where ");
            if (fileId.Length == 1)
            {
                strSql.AppendFormat(" FileId = '{0}' ", fileId[0]);
                strSql2.AppendFormat(" select FilePath from tbl_ComapnyFile where FileId = '{0}' ", fileId[0]);
            }
            else
            {
                strSql.AppendFormat(" FileId in ({0}) ", GetIdsByArr(fileId));
                strSql2.AppendFormat(" select FilePath from tbl_ComapnyFile where FileId in ({0}) ", GetIdsByArr(fileId));
            }

            dc.CommandText = string.Format(" {0};{1} ", strSql2.ToString(), strSql.ToString());

            int r = DbHelper.ExecuteSqlTrans(dc, _db);

            return r > 0 ? 1 : -1;
        }

        #endregion
    }
}
