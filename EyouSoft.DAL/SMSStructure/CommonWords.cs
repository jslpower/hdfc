using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.Common;
using Microsoft.Practices.EnterpriseLibrary.Data;
using System.Data;

namespace EyouSoft.DAL.SMSStructure
{
    /// <summary>
    /// 短信中心-短信常用短语列表及类型数据访问类
    /// Author xuqh 2011-01-21
    /// </summary>
    public class CommonWords : EyouSoft.Toolkit.DAL.DALBase, EyouSoft.IDAL.SMSStructure.ICommonWords
    {
        #region static constants
        private const string SQL_INSERT_INSERTCATEGORY = "INSERT INTO [SMS_CommonWordClass]([CompanyID] ,[UserID] ,[ClassName]) VALUES(@COMPANYID,@USERID,@CLASSNAME);SELECT @@IDENTITY";
        private const string SQL_SELECT_GETCATEGORYS = "SELECT [ID] ,[CompanyID] ,[UserID] ,[ClassName] ,[IssueTime]  FROM [SMS_CommonWordClass] WHERE [CompanyID]=@COMPANYID";
        private const string SQL_INSERT_INSERTTEMPLATE = "INSERT INTO [SMS_CommonWords] ([ID],[CompanyID] ,[UserID] ,[ClassID] ,[WordContent]) VALUES(@ID,@COMPANYID,@USERID,@CLASSID,@WORDCONTENT)";
        private const string SQL_DELETE_DELETECATEGORY = "DELETE FROM [SMS_CommonWordClass] WHERE [ID]=@CategoryId";
        private const string SQL_DELETE_DELETETEMPLATE = "DELETE FROM [SMS_CommonWords] ";
        private const string SQL_SELECT_GETTEMPLATEINFO = "SELECT [ID] ,[CompanyID] ,[UserID] ,[ClassID] ,[WordContent] ,[IssueTime]  FROM [SMS_CommonWords] WHERE [ID]=@TEMPLATEID";
        private const string SQL_UPDATE_UPDATETEMPLATE = "UPDATE [SMS_CommonWords] SET [ClassID]=@CATEGORYID,[WordContent]=@CONTENT WHERE [ID]=@TEMPLATEID";
        private readonly Database _db = null;
        #endregion static constants

        #region 构造函数
        public CommonWords()
        {
            this._db = base.SystemStore;
        }
        #endregion

        #region ICommonWords 成员

        /// <summary>
        /// 添加常用短语
        /// </summary>
        /// <param name="model">常用短语实体</param>
        /// <returns></returns>
        public bool AddCommonWords(EyouSoft.Model.SMSStructure.CommonWords model)
        {
            DbCommand cmd = this._db.GetSqlStringCommand(SQL_INSERT_INSERTTEMPLATE);

            this._db.AddInParameter(cmd, "ID", DbType.AnsiStringFixedLength, Guid.NewGuid().ToString());
            this._db.AddInParameter(cmd, "COMPANYID", DbType.String, model.CompanyID);
            this._db.AddInParameter(cmd, "USERID", DbType.String, model.UserID);
            this._db.AddInParameter(cmd, "CLASSID", DbType.String, model.ClassID);
            this._db.AddInParameter(cmd, "WORDCONTENT", DbType.String, model.WordContent);

            return EyouSoft.Toolkit.DAL.DbHelper.ExecuteSql(cmd, this._db) == 1 ? true : false;
        }

        /// <summary>
        /// 添加常用短语类型
        /// </summary>
        /// <param name="model">常用短语实体</param>
        /// <returns></returns>
        public int AddCommonWordsClass(EyouSoft.Model.SMSStructure.CommonWordClass model)
        {
            DbCommand cmd = this._db.GetSqlStringCommand(SQL_INSERT_INSERTCATEGORY);
            this._db.AddInParameter(cmd, "COMPANYID", DbType.Int32, model.CompanyID);
            this._db.AddInParameter(cmd, "USERID", DbType.String, model.UserID);
            this._db.AddInParameter(cmd, "CLASSNAME", DbType.String, model.ClassName);

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
        /// 更新常用短语
        /// </summary>
        /// <param name="model">常用短语实体</param>
        /// <returns></returns>
        public bool UpdateCommonWords(EyouSoft.Model.SMSStructure.CommonWords model)
        {
            DbCommand cmd = this._db.GetSqlStringCommand(SQL_UPDATE_UPDATETEMPLATE);
            this._db.AddInParameter(cmd, "CATEGORYID", DbType.Int32, model.ClassID);
            this._db.AddInParameter(cmd, "CONTENT", DbType.String, model.WordContent);
            this._db.AddInParameter(cmd, "TEMPLATEID", DbType.String, model.ID);

            return EyouSoft.Toolkit.DAL.DbHelper.ExecuteSql(cmd, this._db) > 0 ? true : false;
        }


        /// <summary>
        /// 删除常用短语
        /// </summary>
        /// <param name="Id">常用短语编号</param>
        /// <returns></returns>
        public bool DeleteCommonWords(string[] Ids)
        {
            //[ID]=@TEMPLATEID
            if (Ids == null || Ids.Length <= 0)
                return false;

            string strIds = string.Empty;
            foreach (string str in Ids)
            {
                strIds += "'" + str.Trim() + "',";
            }
            strIds = strIds.Trim(',');

            DbCommand cmd = this._db.GetSqlStringCommand(SQL_DELETE_DELETETEMPLATE + " where ID in (" + strIds + ");");

            return EyouSoft.Toolkit.DAL.DbHelper.ExecuteSql(cmd, this._db) > 0 ? true : false;
        }

        /// <summary>
        /// 删除一条常用短语类型
        /// </summary>
        /// <param name="Id">类型编号</param>
        /// <returns></returns>
        public bool DeleteCommonWordsClass(int Id)
        {
            DbCommand cmd = this._db.GetSqlStringCommand(SQL_DELETE_DELETECATEGORY);

            this._db.AddInParameter(cmd, "CategoryId", DbType.Int32, Id);

            return EyouSoft.Toolkit.DAL.DbHelper.ExecuteSql(cmd, this._db) > 0 ? true : false;
        }

        /// <summary>
        /// 获取一个常用短语实体
        /// </summary>
        /// <param name="Id">常用短语编号</param>
        /// <returns></returns>
        public EyouSoft.Model.SMSStructure.CommonWords GetCommonWords(string Id)
        {
            EyouSoft.Model.SMSStructure.CommonWords templateInfo = null;
            DbCommand cmd = this._db.GetSqlStringCommand(SQL_SELECT_GETTEMPLATEINFO);
            this._db.AddInParameter(cmd, "TEMPLATEID", DbType.AnsiStringFixedLength, Id);

            using (IDataReader rdr = EyouSoft.Toolkit.DAL.DbHelper.ExecuteReader(cmd, this._db))
            {
                if (rdr.Read())
                {
                    templateInfo = new EyouSoft.Model.SMSStructure.CommonWords(rdr.GetString(rdr.GetOrdinal("ID")).Trim()
                        , rdr.GetInt32(rdr.GetOrdinal("CompanyID"))
                        , rdr.GetInt32(rdr.GetOrdinal("UserID"))
                        , rdr.GetInt32(rdr.GetOrdinal("ClassID"))
                        , rdr.IsDBNull(rdr.GetOrdinal("WordContent")) ? "" : rdr["WordContent"].ToString()
                        , rdr.GetDateTime(rdr.GetOrdinal("IssueTime"))
                        , string.Empty);
                }
            }

            return templateInfo;
        }

        /// <summary>
        /// 根据公司编号获取该公司所有常用短语类型信息
        /// </summary>
        /// <param name="CompanyID">公司编号</param>
        /// <returns></returns>
        public IList<EyouSoft.Model.SMSStructure.CommonWordClass> GetCommonWordsClass(int CompanyID)
        {
            DbCommand cmd = this._db.GetSqlStringCommand(SQL_SELECT_GETCATEGORYS);
            this._db.AddInParameter(cmd, "COMPANYID", DbType.AnsiStringFixedLength, CompanyID);

            IList<EyouSoft.Model.SMSStructure.CommonWordClass> categorys = new List<EyouSoft.Model.SMSStructure.CommonWordClass>();

            using (IDataReader rdr = EyouSoft.Toolkit.DAL.DbHelper.ExecuteReader(cmd, this._db))
            {
                while (rdr.Read())
                {
                    categorys.Add(new EyouSoft.Model.SMSStructure.CommonWordClass(rdr.GetInt32(rdr.GetOrdinal("Id"))
                        , rdr.GetInt32(rdr.GetOrdinal("CompanyID"))
                        , rdr.GetInt32(rdr.GetOrdinal("UserID"))
                        , rdr.IsDBNull(rdr.GetOrdinal("ClassName")) ? "" : rdr.GetString(rdr.GetOrdinal("ClassName"))
                        , rdr.GetDateTime(rdr.GetOrdinal("IssueTime"))));
                }
            }

            return categorys;
        }


        /// <summary>
        /// 分页获取常用短语列表
        /// </summary>
        /// <param name="pageSize">每页条数</param>
        /// <param name="pageIndex">当前页码</param>
        /// <param name="RecordCount">总数</param>
        /// <param name="keywords">关键字（为空时不做查询条件）</param>
        /// <param name="ClassId">类型编号（-1时不做查询条件）</param>
        /// <returns></returns>
        public IList<EyouSoft.Model.SMSStructure.CommonWords> GetList(int pageSize, int pageIndex, ref int recordCount, int companyId, string keywords, int ClassId)
        {
            IList<EyouSoft.Model.SMSStructure.CommonWords> templates = new List<EyouSoft.Model.SMSStructure.CommonWords>();

            StringBuilder cmdQuery = new StringBuilder();
            string tableName = "View_SMS_Template";
            string orderByString = "IssueTime DESC";
            string fields = "ID, CompanyID, UserID, ClassID, WordContent, IssueTime, ClassName";

            cmdQuery.AppendFormat(" CompanyID='{0}' ", companyId);

            if (!string.IsNullOrEmpty(keywords))
            {
                cmdQuery.AppendFormat(" AND (WordContent LIKE '%{0}%')", keywords);
            }

            if (ClassId > 0)
            {
                cmdQuery.AppendFormat(" AND ClassID={0}", ClassId.ToString());
            }

            using (IDataReader rdr = EyouSoft.Toolkit.DAL.DbHelper.ExecuteReader1(this._db, pageSize, pageIndex, ref recordCount, tableName
                , fields, cmdQuery.ToString(), orderByString, string.Empty))
            {
                while (rdr.Read())
                {
                    templates.Add(new EyouSoft.Model.SMSStructure.CommonWords(rdr.GetString(rdr.GetOrdinal("ID"))
                        , rdr.GetInt32(rdr.GetOrdinal("CompanyID"))
                        , rdr.GetInt32(rdr.GetOrdinal("UserID"))
                        , rdr.GetInt32(rdr.GetOrdinal("ClassID"))
                        , rdr.IsDBNull(rdr.GetOrdinal("WordContent")) ? "" : rdr["WordContent"].ToString()
                        , rdr.GetDateTime(rdr.GetOrdinal("IssueTime"))
                        , rdr["ClassName"].ToString()
                        ));
                }
            }

            return templates;
        }

        #endregion
    }
}
