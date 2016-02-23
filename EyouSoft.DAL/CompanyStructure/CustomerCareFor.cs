using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using EyouSoft.Model.CompanyStructure;
using System.Data.SqlClient;
using Microsoft.Practices.EnterpriseLibrary.Data;
using System.Data;
using System.Data.Common;
using EyouSoft.Toolkit.DAL;

namespace EyouSoft.DAL.CompanyStructure
{
    public class CustomerCareFor : EyouSoft.Toolkit.DAL.DALBase, EyouSoft.IDAL.CompanyStructure.ICustomerCareFor
    {

        private readonly Database _db = null;
        /// <summary>
        /// 构造函数
        /// </summary>
        public CustomerCareFor()
        {
            _db = this.SystemStore;
        }
        /// <summary>
        /// 添加
        /// </summary>
        /// <param name="Model"></param>
        /// <returns></returns>
        public bool Add(EyouSoft.Model.CompanyStructure.CustomerCareforInfo Model)
        {
            StringBuilder sql = new StringBuilder("INSERT INTO [SMS_CustomerCarefor]([CompanyId],[MobileCode],[IsMatchCustomerInfo],[IsMatchSupplierInfo],[IsMatchDepartmentInfo],[Content],[Time],[FixType],[ChannelId],[IsEnabled],[OperatorId],[IssueTime])");
            sql.Append("values(@CompanyId,@MobileCode,@IsMatchCustomerInfo,@IsMatchSupplierInfo,@IsMatchDepartmentInfo,@Content,@Time,@FixType,@ChannelId,@IsEnabled,@OperatorId,@IssueTime)");
            DbCommand cmd = this._db.GetSqlStringCommand(sql.ToString());
            this._db.AddInParameter(cmd, "ChannelId", DbType.Int32, Model.ChannelId);
            this._db.AddInParameter(cmd, "CompanyId", DbType.Int32, Model.CompanyId);
            this._db.AddInParameter(cmd, "Content", DbType.String, Model.Content);
            this._db.AddInParameter(cmd, "FixType", DbType.Int32, (int)Model.FixType);
            this._db.AddInParameter(cmd, "IsEnabled", DbType.StringFixedLength, Model.IsEnabled ? '1' : '0');
            this._db.AddInParameter(cmd, "IsMatchCustomerInfo", DbType.StringFixedLength, Model.IsMatchCustomerInfo ? '1' : '0');
            this._db.AddInParameter(cmd, "IsMatchDepartmentInfo", DbType.StringFixedLength, Model.IsMatchDepartmentInfo ? '1' : '0');
            this._db.AddInParameter(cmd, "IsMatchSupplierInfo", DbType.StringFixedLength, Model.IsMatchSupplierInfo ? '1' : '0');
            this._db.AddInParameter(cmd, "IsSeded", DbType.StringFixedLength, Model.IsSeded ? '1' : '0');
            this._db.AddInParameter(cmd, "IssueTime", DbType.DateTime, Model.IssueTime);
            this._db.AddInParameter(cmd, "MobileCode", DbType.String, Model.MobileCode);
            this._db.AddInParameter(cmd, "OperatorId", DbType.Int32, Model.OperatorId);
            this._db.AddInParameter(cmd, "Time", DbType.DateTime, Model.Time);

            return DbHelper.ExecuteSql(cmd, this._db) > 0 ? true : false;
        }

        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="Model"></param>
        /// <returns></returns>
        public bool Update(EyouSoft.Model.CompanyStructure.CustomerCareforInfo Model)
        {
            if (Model == null || Model.Id <= 0)
                return false;
            StringBuilder sql = new StringBuilder("update [SMS_CustomerCarefor] set [CompanyId]=@CompanyId,[MobileCode]=@MobileCode,[IsMatchCustomerInfo]=@IsMatchCustomerInfo,");
            sql.Append(" [IsMatchSupplierInfo]=@IsMatchSupplierInfo,[IsMatchDepartmentInfo]=@IsMatchDepartmentInfo,[Content]=@Content,[Time]=@Time,[FixType]=@FixType,[ChannelId]=@ChannelId,");
            sql.Append(" [IsEnabled]=@IsEnabled,[OperatorId]=@OperatorId,[IssueTime]=@IssueTime ");
            sql.AppendFormat(" where id={0}", Model.Id);
            DbCommand cmd = this._db.GetSqlStringCommand(sql.ToString());
            this._db.AddInParameter(cmd, "ChannelId", DbType.Int32, Model.ChannelId);
            this._db.AddInParameter(cmd, "CompanyId", DbType.Int32, Model.CompanyId);
            this._db.AddInParameter(cmd, "Content", DbType.String, Model.Content);
            this._db.AddInParameter(cmd, "FixType", DbType.Int32, (int)Model.FixType);
            this._db.AddInParameter(cmd, "IsEnabled", DbType.StringFixedLength, Model.IsEnabled ? '1' : '0');
            this._db.AddInParameter(cmd, "IsMatchCustomerInfo", DbType.StringFixedLength, Model.IsMatchCustomerInfo ? '1' : '0');
            this._db.AddInParameter(cmd, "IsMatchDepartmentInfo", DbType.StringFixedLength, Model.IsMatchDepartmentInfo ? '1' : '0');
            this._db.AddInParameter(cmd, "IsMatchSupplierInfo", DbType.StringFixedLength, Model.IsMatchSupplierInfo ? '1' : '0');
            this._db.AddInParameter(cmd, "IsSeded", DbType.StringFixedLength, Model.IsSeded ? '1' : '0');
            this._db.AddInParameter(cmd, "IssueTime", DbType.DateTime, Model.IssueTime);
            this._db.AddInParameter(cmd, "MobileCode", DbType.String, Model.MobileCode);
            this._db.AddInParameter(cmd, "OperatorId", DbType.Int32, Model.OperatorId);
            this._db.AddInParameter(cmd, "Time", DbType.DateTime, Model.Time);

            return DbHelper.ExecuteSql(cmd, this._db) > 0 ? true : false;

        }

        /// <summary>
        /// 获取列表
        /// </summary>
        /// <param name="CompanyId">公司编号</param>
        /// <returns></returns>
        public IList<EyouSoft.Model.CompanyStructure.CustomerCareforInfo> GetList(string CompanyId, int PageSize, int PageIndex, ref int RecordCount)
        {
            string cmdQuery = string.Format(" Companyid={0}", CompanyId);
            string tableName = "SMS_CustomerCarefor";
            string field = "*";
            IList<EyouSoft.Model.CompanyStructure.CustomerCareforInfo> careForList = new List<EyouSoft.Model.CompanyStructure.CustomerCareforInfo>();
            EyouSoft.Model.CompanyStructure.CustomerCareforInfo model = null;
            using (IDataReader rd = DbHelper.ExecuteReader1(this._db, PageSize, PageIndex, ref RecordCount, tableName, field, cmdQuery, "IssueTime desc", string.Empty))
            {
                while (rd.Read())
                {
                    model = new CustomerCareforInfo();
                    model.Id = rd.GetInt32(rd.GetOrdinal("id"));
                    model.Content = rd.IsDBNull(rd.GetOrdinal("Content")) ? "" : rd.GetString(rd.GetOrdinal("Content"));
                    model.IsEnabled = rd.GetString(rd.GetOrdinal("IsEnabled")) == "1" ? true : false;
                    model.IsMatchCustomerInfo = rd.GetString(rd.GetOrdinal("IsMatchCustomerInfo")) == "1" ? true : false;
                    model.IsMatchDepartmentInfo = rd.GetString(rd.GetOrdinal("IsMatchDepartmentInfo")) == "1" ? true : false;
                    model.IsMatchSupplierInfo = rd.GetString(rd.GetOrdinal("IsMatchSupplierInfo")) == "1" ? true : false;
                    model.IsSeded = rd.GetString(rd.GetOrdinal("IsSeded")) == "1" ? true : false;
                    model.IssueTime = rd.IsDBNull(rd.GetOrdinal("IssueTime")) ? DateTime.Parse("2000-01-01") : rd.GetDateTime(rd.GetOrdinal("IssueTime"));
                    model.MobileCode = rd.IsDBNull(rd.GetOrdinal("MobileCode")) ? "" : rd.GetString(rd.GetOrdinal("MobileCode"));
                    model.OperatorId = rd.GetInt32(rd.GetOrdinal("OperatorId"));
                    model.FixType = rd.IsDBNull(rd.GetOrdinal("FixType")) ? EyouSoft.Model.EnumType.CompanyStructure.CustomerCareForSendSpecialTime.无 : (EyouSoft.Model.EnumType.CompanyStructure.CustomerCareForSendSpecialTime)Enum.Parse(typeof(EyouSoft.Model.EnumType.CompanyStructure.CustomerCareForSendSpecialTime), rd.GetInt32(rd.GetOrdinal("FixType")).ToString());
                    model.Time = rd.IsDBNull(rd.GetOrdinal("Time")) ? DateTime.Parse("2000-01-01") : rd.GetDateTime(rd.GetOrdinal("Time"));
                    careForList.Add(model);
                }
            }
            return careForList;
        }

        /// <summary>
        /// 返回实体
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        public EyouSoft.Model.CompanyStructure.CustomerCareforInfo GetModel(int Id)
        {
            string sql = "select * from SMS_CustomerCarefor  where id=@CareForId ";
            DbCommand cmd = this._db.GetSqlStringCommand(sql);
            this._db.AddInParameter(cmd, "CareForId", DbType.Int32, Id);
            EyouSoft.Model.CompanyStructure.CustomerCareforInfo model = null;
            using (IDataReader rd = DbHelper.ExecuteReader(cmd, this._db))
            {
                if (rd.Read())
                {
                    model = new CustomerCareforInfo();
                    model.ChannelId = rd.IsDBNull(rd.GetOrdinal("ChannelId")) ? 0 : rd.GetInt32(rd.GetOrdinal("ChannelId"));
                    model.CompanyId = rd.IsDBNull(rd.GetOrdinal("CompanyId")) ? 0 : rd.GetInt32(rd.GetOrdinal("CompanyId"));
                    model.Content = rd.IsDBNull(rd.GetOrdinal("Content")) ? "" : rd.GetString(rd.GetOrdinal("Content"));
                    model.FixType = rd.IsDBNull(rd.GetOrdinal("FixType")) ? EyouSoft.Model.EnumType.CompanyStructure.CustomerCareForSendSpecialTime.无 : (EyouSoft.Model.EnumType.CompanyStructure.CustomerCareForSendSpecialTime)Enum.Parse(typeof(EyouSoft.Model.EnumType.CompanyStructure.CustomerCareForSendSpecialTime), rd.GetInt32(rd.GetOrdinal("FixType")).ToString());
                    model.Id = rd.IsDBNull(rd.GetOrdinal("Id")) ? 0 : rd.GetInt32(rd.GetOrdinal("Id"));
                    model.IsEnabled = rd.IsDBNull(rd.GetOrdinal("IsEnabled")) ? false : rd.GetString(rd.GetOrdinal("IsEnabled")) == "1" ? true : false;
                    model.IsMatchCustomerInfo = rd.IsDBNull(rd.GetOrdinal("IsMatchCustomerInfo")) ? false : rd.GetString(rd.GetOrdinal("IsMatchCustomerInfo")) == "1" ? true : false;
                    model.IsMatchDepartmentInfo = rd.IsDBNull(rd.GetOrdinal("IsMatchDepartmentInfo")) ? false : rd.GetString(rd.GetOrdinal("IsMatchDepartmentInfo")) == "1" ? true : false;
                    model.IsMatchSupplierInfo = rd.IsDBNull(rd.GetOrdinal("IsMatchSupplierInfo")) ? false : rd.GetString(rd.GetOrdinal("IsMatchSupplierInfo")) == "1" ? true : false;
                    model.IsSeded = rd.IsDBNull(rd.GetOrdinal("IsSeded")) ? false : rd.GetString(rd.GetOrdinal("IsSeded")) == "1" ? true : false;
                    model.IssueTime = rd.IsDBNull(rd.GetOrdinal("IssueTime")) ? DateTime.Parse("2011-01-28") : rd.GetDateTime(rd.GetOrdinal("IssueTime"));
                    model.MobileCode = rd.IsDBNull(rd.GetOrdinal("MobileCode")) ? "" : rd.GetString(rd.GetOrdinal("MobileCode"));
                    model.OperatorId = rd.IsDBNull(rd.GetOrdinal("OperatorId")) ? 0 : rd.GetInt32(rd.GetOrdinal("OperatorId"));
                    model.Time = rd.IsDBNull(rd.GetOrdinal("Time")) ? DateTime.Parse("2011-01-28") : rd.GetDateTime(rd.GetOrdinal("Time"));
                }
            }
            return model;
        }

        /// <summary>
        /// 停发
        /// </summary>
        /// <param name="CareForId">编号</param>
        /// <returns></returns>
        public bool StopIt(int CareForId)
        {
            string sql = " update SMS_CustomerCarefor set IsEnabled=0 where id=@CareForId ";
            DbCommand cmd = this._db.GetSqlStringCommand(sql);
            this._db.AddInParameter(cmd, "CareForId", DbType.Int32, CareForId);
            return DbHelper.ExecuteSql(cmd, this._db) > 0 ? true : false;
        }

        /// <summary>
        /// 开启
        /// </summary>
        /// <param name="CareForId">编号</param>
        /// <returns></returns>
        public bool StartIt(int CareForId)
        {
            string sql = " update SMS_CustomerCarefor set IsEnabled=1 where id=@CareForId ";
            DbCommand cmd = this._db.GetSqlStringCommand(sql);
            this._db.AddInParameter(cmd, "CareForId", DbType.Int32, CareForId);
            return DbHelper.ExecuteSql(cmd, this._db) > 0 ? true : false;
        }

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="CareForId">编号</param>
        /// <returns></returns>
        public bool DeletIt(int CareForId)
        {
            string sql = " DELETE FROM SMS_CustomerCarefor  where id=@CareForId ";
            DbCommand cmd = this._db.GetSqlStringCommand(sql);
            this._db.AddInParameter(cmd, "CareForId", DbType.Int32, CareForId);
            return DbHelper.ExecuteSql(cmd, this._db) > 0 ? true : false;
        }
    }
}
