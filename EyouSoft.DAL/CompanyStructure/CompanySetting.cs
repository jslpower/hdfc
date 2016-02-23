using System.Text;
using Microsoft.Practices.EnterpriseLibrary.Data;
using System.Data.Common;

namespace EyouSoft.DAL.CompanyStructure
{
    using System;
    using System.Data;

    /// <summary>
    /// 公司系统配置DAL
    /// </summary>
    public class CompanySetting : Toolkit.DAL.DALBase, IDAL.CompanyStructure.ICompanySetting
    {
        #region static constants

        private const string SqlBcthSetSeting = "if not exists(select 1 from tbl_CompanySetting where id = {0} and fieldname = '{1}') begin 	insert into tbl_CompanySetting(id,fieldname,fieldvalue) values({0},'{1}','{2}') end else begin update tbl_CompanySetting set fieldvalue='{2}' where id = {0} and fieldname = '{1}' end ;";

        private const string SqlGetValue = "select FieldValue from tbl_CompanySetting where Id = @Id and FieldName = @FieldName";
        private const string SqlSetSetting = " delete tbl_CompanySetting where Id = @CompanyId and FieldName = @FieldName; insert into tbl_CompanySetting(id,FieldName,FieldValue) values(@CompanyId,@FieldName,@FieldValue);";

        private readonly Database _db;
        #endregion

        #region 构造函数

        public CompanySetting()
        {
            this._db = SystemStore;
        }

        #endregion

        #region ICompanySetting 成员

        /// <summary>
        /// 设置系统配置信息
        /// </summary>
        /// <param name="model">系统配置实体</param>
        /// <returns>true：成功 false:失败</returns>
        public bool SetCompanySetting(Model.CompanyStructure.CompanyFieldSetting model)
        {
            if (model == null) return false;

            var strSql = new StringBuilder();
            strSql.AppendFormat(SqlBcthSetSeting, model.CompanyId, "CompanyLogo", model.CompanyLogo);
            strSql.AppendFormat(SqlBcthSetSeting, model.CompanyId, "MaxSonUserNum", model.MaxSonUserNum);
            strSql.AppendFormat(SqlBcthSetSeting, model.CompanyId, "BirthdayReminderDays", model.BirthdayReminderDays);

            DbCommand dc = this._db.GetSqlStringCommand(strSql.ToString());
            return Toolkit.DAL.DbHelper.ExecuteSqlTrans(dc, this._db) > 0 ? true : false;
        }

        /// <summary>
        /// 获取指定公司的配置信息
        /// </summary>
        /// <param name="companyId"></param>
        /// <param name="fileKey"></param>
        /// <returns></returns>
        public string GetValue(int companyId, string fileKey)
        {
            if (companyId <= 0 || string.IsNullOrEmpty(fileKey)) return string.Empty;

            string fieldValue = string.Empty;
            DbCommand cmd = this._db.GetSqlStringCommand(SqlGetValue);
            this._db.AddInParameter(cmd, "Id", DbType.Int32, companyId);
            this._db.AddInParameter(cmd, "FieldName", DbType.String, fileKey);

            using (IDataReader rdr = Toolkit.DAL.DbHelper.ExecuteReader(cmd, this._db))
            {
                if (rdr.Read())
                {
                    fieldValue = rdr.IsDBNull(rdr.GetOrdinal("FieldValue"))
                                     ? string.Empty
                                     : rdr.GetString(rdr.GetOrdinal("FieldValue"));
                }
            }

            return fieldValue;
        }

        /// <summary>
        /// 设置指定公司的配置信息
        /// </summary>
        /// <param name="companyId">公司编号</param>
        /// <param name="fieldKey">配置key</param>
        /// <param name="fieldValue">配置value</param>
        /// <returns></returns>
        public bool SetValue(int companyId, string fieldKey, string fieldValue)
        {
            if (companyId <= 0 || string.IsNullOrEmpty(fieldKey)) return false;

            DbCommand dc = this._db.GetSqlStringCommand(SqlSetSetting);
            this._db.AddInParameter(dc, "CompanyId", DbType.Int32, companyId);
            this._db.AddInParameter(dc, "FieldName", DbType.String, fieldKey);
            this._db.AddInParameter(dc, "FieldValue", DbType.String, fieldValue);
            return Toolkit.DAL.DbHelper.ExecuteSqlTrans(dc, this._db) > 0 ? true : false;
        }

        #endregion
    }
}
