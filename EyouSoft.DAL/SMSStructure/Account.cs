using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.Common;
using System.Data;
using Microsoft.Practices.EnterpriseLibrary.Data;

namespace EyouSoft.DAL.SMSStructure
{
    /// <summary>
    /// 短信中心-用户账户信息数据访问类
    /// Author:2011-01-21
    /// </summary>
    public class Account : EyouSoft.Toolkit.DAL.DALBase, EyouSoft.IDAL.SMSStructure.IAccount
    {
        #region static constants
        private const string SQL_SELECT_GETACCOUNTINFO = "SELECT [ID],[CompanyID],[AccountMoney] FROM [SMS_Account] WHERE [CompanyID]=@COMPANYID";
        private const string SQL_SELECT_GETACCOUNTMONEY = "SELECT [AccountMoney] FROM [SMS_Account] WHERE [CompanyID]=@COMPANYID";
        private const string SQL_INSERT_PAYMONEY = "INSERT INTO [SMS_PayMoney] ([ID],[CompanyID] ,[CompanyName] ,[OperatorID] ,[OperatorName] ,[PayMoney] ,[PayTime], [PaySMSNumber], [OperatorMobile], [OperatorTel]) VALUES(@ID,@COMPANYID ,@COMPANYNAME ,@USERID ,@USERFULLNAME ,@PAYMONEY ,@PAYTIME, @PaySMSNumber, @OperatorMobile,@OperatorTel)";
        private const string SQL_SELECT_GETACCOUNTEXPENSECOLLECTINFO = "SELECT COUNT(*) AS [SentMessageCount],(SELECT SUM([UseMoeny]) FROM [SMS_SendTotal] WHERE [CompanyID]=@COMPANYID) AS [ExpenseAmount] FROM [SMS_SendDetail] WHERE [CompanyID]=@COMPANYID";
        private const string SQL_SELECT_GETACCOUNTSMSNUMBER = "SELECT [AccountSMSNumber] FROM [SMS_Account] WHERE [CompanyID]=@COMPANYID";
        private const string SQL_SELECT_HASNOCHECKPAY = "SELECT COUNT(*) FROM SMS_PayMoney WHERE CompanyID=@CompanyID AND IsChecked=0";
        private const string SQL_DELETE_NoPassCheckPayMoney = "DELETE FROM SMS_PayMoney WHERE ID=@PayMoneyId AND IsChecked<>1";
        private const string SQL_INSERT_SetAccountBaseInfo = "IF NOT EXISTS(SELECT 1 FROM [SMS_Account] WHERE [CompanyID]=@CompanyId) BEGIN INSERT INTO [SMS_Account](ID,CompanyID,AccountMoney) VALUES(@ID,@CompanyId,0) END";
        private const string SQL_SELECT_IsExistsAccount = "SELECT COUNT(*) FROM SMS_Account WHERE CompanyID=@CompanyId";
        private readonly Database _db = null;
        #endregion static constants

        #region 构造函数
        public Account()
        {
            this._db = base.SystemStore;
        }
        #endregion

        #region IAccount 成员

        /// <summary>
        /// 获取账户余额
        /// </summary>
        /// <param name="companyId">公司编号</param>
        /// <returns></returns>
        public decimal GetAccountMoney(int companyId)
        {
            decimal accountMoney = 0;
            DbCommand cmd = this._db.GetSqlStringCommand(SQL_SELECT_GETACCOUNTMONEY);
            this._db.AddInParameter(cmd, "COMPANYID", DbType.AnsiStringFixedLength, companyId);

            using (IDataReader rdr = EyouSoft.Toolkit.DAL.DbHelper.ExecuteReader(cmd, this._db))
            {
                if (rdr.Read())
                {
                    accountMoney = rdr.GetDecimal(0);
                }
            }

            return accountMoney;
        }

        /// <summary>
        /// 获取账户剩余短信条数
        /// </summary>
        /// <param name="companyId">公司编号</param>
        /// <returns></returns>
        public int GetAccountSMSNumber(int companyId)
        {
            int number = 0;
            DbCommand cmd = this._db.GetSqlStringCommand(SQL_SELECT_GETACCOUNTSMSNUMBER);
            this._db.AddInParameter(cmd, "COMPANYID", DbType.AnsiStringFixedLength, companyId);

            using (IDataReader rdr = EyouSoft.Toolkit.DAL.DbHelper.ExecuteReader(cmd, this._db))
            {
                if (rdr.Read())
                {
                    number = rdr.GetInt32(0);
                }
            }

            return number;
        }


        /// <summary>
        /// 获取指定公司的账户信息
        /// </summary>
        /// <param name="companyId">公司编号</param>
        /// <returns></returns>
        public EyouSoft.Model.SMSStructure.AccountInfo GetAccountInfo(int companyId)
        {
            EyouSoft.Model.SMSStructure.AccountInfo model = null;
            DbCommand cmd = this._db.GetSqlStringCommand(SQL_SELECT_GETACCOUNTINFO);
            this._db.AddInParameter(cmd, "COMPANYID", DbType.AnsiStringFixedLength, companyId);
            using (IDataReader rdr = EyouSoft.Toolkit.DAL.DbHelper.ExecuteReader(cmd, this._db))
            {
                if (rdr.Read())
                {
                    model = new EyouSoft.Model.SMSStructure.AccountInfo();
                    model.ID = rdr.IsDBNull(1) ? string.Empty : rdr.GetString(0);
                    model.CompanyID = rdr.IsDBNull(1) ? 0 : rdr.GetInt32(1);
                    model.AccountMoney = rdr.IsDBNull(2) ? 0 : rdr.GetDecimal(2);
                }
            }
            return model;
        }

        /// <summary>
        /// 是否存在未审核的充值
        /// </summary>
        /// <param name="companyId">公司编号</param>
        /// <returns></returns>
        public bool HasNoCheckPay(int companyId)
        {
            bool tmp = false;
            DbCommand cmd = this._db.GetSqlStringCommand(SQL_SELECT_HASNOCHECKPAY);
            this._db.AddInParameter(cmd, "COMPANYID", DbType.AnsiStringFixedLength, companyId);

            using (IDataReader rdr = EyouSoft.Toolkit.DAL.DbHelper.ExecuteReader(cmd, this._db))
            {
                if (rdr.Read())
                {
                    tmp = rdr.GetInt32(0) > 0 ? true : false;
                }
            }

            return tmp;
        }

        /// <summary>
        /// 扣除账户余额
        /// </summary>
        /// <param name="companyId">公司编号</param>
        /// <param name="userId">用户编号</param>
        /// <param name="money">扣除金额</param>
        /// <param name="smscount">扣除短信条数</param>
        /// <param name="tempFeeTakeId">金额扣除临时表编号</param>
        /// <param name="sendTotalId">短信发送统计表编号</param>
        /// <returns>0失败 1成功</returns>
        public bool DeductAccountMoney(int companyId, string userId, decimal money, int smscount, string tempFeeTakeId, string sendTotalId)
        {
            DbCommand cmd = this._db.GetStoredProcCommand("proc_SMS_DeductAccountMoney");
            this._db.AddInParameter(cmd, "TempFeeTakeId", DbType.String, tempFeeTakeId);
            this._db.AddInParameter(cmd, "CompanyId", DbType.String, companyId);
            this._db.AddInParameter(cmd, "UserId", DbType.String, userId);
            this._db.AddInParameter(cmd, "SendTotalId", DbType.String, sendTotalId);
            this._db.AddInParameter(cmd, "Money", DbType.Decimal, money);
            this._db.AddInParameter(cmd, "SMSNumber", DbType.Int32, smscount);
            this._db.AddOutParameter(cmd, "Result", DbType.Int32, 4);

            EyouSoft.Toolkit.DAL.DbHelper.RunProcedure(cmd, this._db);

            return Convert.ToInt32(this._db.GetParameterValue(cmd, "Result")) == 1 ? true : false;
        }

        /// <summary>
        /// 账户充值
        /// </summary>
        /// <param name="payMoneyInfo">短信充值支付记录实体</param>
        /// <returns></returns>
        public bool InsertPayMoney(EyouSoft.Model.SMSStructure.PayMoneyInfo payMoneyInfo)
        {
            DbCommand cmd = this._db.GetSqlStringCommand(SQL_INSERT_PAYMONEY);

            this._db.AddInParameter(cmd, "ID", DbType.AnsiStringFixedLength, Guid.NewGuid().ToString());
            this._db.AddInParameter(cmd, "COMPANYID", DbType.String, payMoneyInfo.CompanyID);
            this._db.AddInParameter(cmd, "COMPANYNAME", DbType.String, payMoneyInfo.CompanyName);
            this._db.AddInParameter(cmd, "USERID", DbType.String, payMoneyInfo.OperatorID);
            this._db.AddInParameter(cmd, "USERFULLNAME", DbType.String, payMoneyInfo.OperatorName);
            this._db.AddInParameter(cmd, "PAYMONEY", DbType.Decimal, payMoneyInfo.PayMoney);
            this._db.AddInParameter(cmd, "PAYTIME", DbType.DateTime, payMoneyInfo.PayTime);
            this._db.AddInParameter(cmd, "PaySMSNumber", DbType.Int32, payMoneyInfo.PaySMSNumber);
            this._db.AddInParameter(cmd, "OperatorMobile", DbType.String, payMoneyInfo.OperatorMobile);
            //this._db.AddInParameter(cmd, "OperatorMQId", DbType.String, payMoneyInfo.OperatorMQId);
            this._db.AddInParameter(cmd, "OperatorTel", DbType.String, payMoneyInfo.OperatorTel);

            return EyouSoft.Toolkit.DAL.DbHelper.ExecuteSql(cmd, this._db) > 0 ? true : false;
        }

        /// <summary>
        /// 获取账户充值明细
        /// </summary>
        /// <param name="pageSize">每页记录数</param>
        /// <param name="pageIndex">当前页索引</param>
        /// <param name="recordCount">总数</param>
        /// <param name="companyId">公司编号</param>
        /// <param name="companyName">公司名称</param>
        /// <param name="payStartTime">充值开始时间 为null时不做为查询条件</param>
        /// <param name="payFinishTime">充值截止时间 为null时不做为查询条件</param>
        /// <param name="operatorStartTime">操作开始时间 为null时不做为查询条件</param>
        /// <param name="operatorFinishTime">操作截止时间 为null时不做为查询条件</param>
        /// <param name="provinceId">省份编号 为null时不做为查询条件</param>
        /// <param name="cityId">城市编号 为null时不做为查询条件</param>
        /// <param name="checkStatus">审核状态 -1:所有状态 0:未审核 1:审核通过  2:审核未通过</param>
        /// <param name="userAreas">用户分管的区域范围 为null时不做为查询条件</param>
        /// <returns></returns>
        public IList<EyouSoft.Model.SMSStructure.PayMoneyInfo> GetPayMoneys(int pageSize, int pageIndex, ref int recordCount, int companyId, string companyName, DateTime? payStartTime, DateTime? payFinishTime, DateTime? operatorStartTime, DateTime? operatorFinishTime, int? provinceId, int? cityId, int checkStatus, string userAreas)
        {
            IList<EyouSoft.Model.SMSStructure.PayMoneyInfo> paymoneys = new List<EyouSoft.Model.SMSStructure.PayMoneyInfo>();

            StringBuilder cmdQuery = new StringBuilder();
            string tableName = "SMS_PayMoney";
            string orderByString = "PayTime DESC,OperatorTime DESC";
            StringBuilder fields = new StringBuilder();
            fields.Append(" [ID],[CompanyID],[CompanyName],[OperatorID],[OperatorName],[PayMoney],[PayTime],[OperatorTime],[IsChecked],[CheckTime],[CheckUserName],[CheckOperatorName],[OperatorTel],[OperatorMobile],[PaySMSNumber],[UseMoney]");
            fields.Append(" ,(SELECT ID FROM tbl_SysProvince WHERE ID=SMS_PayMoney.CompanyID) AS ProvinceId ");
            fields.Append(" ,(SELECT ID FROM tbl_SysCity WHERE ID=SMS_PayMoney.CompanyID) AS CityId ");

            #region 查询字段
            cmdQuery.Append(" 1=1 ");

            if (companyId > 0)
            {
                cmdQuery.AppendFormat(" AND CompanyID='{0}' ", companyId.ToString());
            }

            if (!string.IsNullOrEmpty(companyName))
            {
                cmdQuery.AppendFormat(" AND CompanyName LIKE '%{0}%' ", companyName);
            }

            if (payStartTime.HasValue)
            {
                cmdQuery.AppendFormat(" AND PayTime>='{0}' ", payStartTime.Value.ToString());
            }

            if (payFinishTime.HasValue)
            {
                cmdQuery.AppendFormat(" AND PayTime<'{0}' ", payFinishTime.Value.ToString());
            }

            if (operatorStartTime.HasValue)
            {
                cmdQuery.AppendFormat(" AND OperatorTime>='{0}' ", operatorStartTime.Value.ToString());
            }

            if (operatorFinishTime.HasValue)
            {
                cmdQuery.AppendFormat(" AND OperatorTime<'{0}' ", operatorFinishTime.Value.ToString());
            }

            /*if (provinceId.HasValue)
            {
                cmdQuery.AppendFormat(" AND ProvinceId={0} ", provinceId.Value);
            }

            if (cityId.HasValue)
            {
                cmdQuery.AppendFormat(" AND CityId={0} ", cityId.Value);
            }*/

            if (checkStatus != -1)
            {
                cmdQuery.AppendFormat(" AND IsChecked={0} ", checkStatus);
            }

            /*if (!string.IsNullOrEmpty(userAreas))
            {
                cmdQuery.AppendFormat(" AND CityId IN({0}) ", userAreas);
            }*/

            if (provinceId.HasValue || cityId.HasValue || !string.IsNullOrEmpty(userAreas))
            {
                cmdQuery.Append(" AND EXISTS(SELECT 1 FROM tbl_CompanyInfo WHERE Id=SMS_PayMoney.CompanyID ");

                if (provinceId.HasValue)
                {
                    cmdQuery.AppendFormat(" AND ProvinceId={0} ", provinceId.Value);
                }

                if (cityId.HasValue)
                {
                    cmdQuery.AppendFormat(" AND CityId={0} ", cityId.Value);
                }

                if (!string.IsNullOrEmpty(userAreas))
                {
                    cmdQuery.AppendFormat(" AND CityId IN({0}) ", userAreas);
                }

                cmdQuery.Append(" ) ");
            }
            #endregion

            using (IDataReader rdr = EyouSoft.Toolkit.DAL.DbHelper.ExecuteReader1(this._db, pageSize, pageIndex, ref recordCount, tableName
                , fields.ToString(), cmdQuery.ToString(), orderByString, string.Empty))
            {
                while (rdr.Read())
                {
                    EyouSoft.Model.SMSStructure.PayMoneyInfo payMoneyInfo = new EyouSoft.Model.SMSStructure.PayMoneyInfo();

                    payMoneyInfo.ID = rdr.GetString(rdr.GetOrdinal("ID"));
                    payMoneyInfo.CompanyID = rdr.GetInt32(rdr.GetOrdinal("CompanyID"));
                    payMoneyInfo.CompanyName = rdr["CompanyName"].ToString();
                    payMoneyInfo.OperatorID = rdr.GetInt32(rdr.GetOrdinal("OperatorID"));
                    payMoneyInfo.OperatorName = rdr["OperatorName"].ToString();
                    payMoneyInfo.PayMoney = rdr.GetDecimal(rdr.GetOrdinal("PayMoney"));
                    payMoneyInfo.PaySMSNumber = rdr.GetInt32(rdr.GetOrdinal("PaySMSNumber"));
                    payMoneyInfo.PayTime = rdr.GetDateTime(rdr.GetOrdinal("PayTime"));
                    payMoneyInfo.OperatorTime = rdr.GetDateTime(rdr.GetOrdinal("OperatorTime"));
                    payMoneyInfo.IsChecked = rdr.GetInt32(rdr.GetOrdinal("IsChecked"));

                    if (!rdr.IsDBNull(rdr.GetOrdinal("CheckTime")))
                    {
                        payMoneyInfo.CheckTime = rdr.GetDateTime(rdr.GetOrdinal("CheckTime"));
                    }

                    if (!rdr.IsDBNull(rdr.GetOrdinal("CheckUserName")))
                    {
                        payMoneyInfo.CheckUserName = rdr["CheckUserName"].ToString();
                    }

                    if (!rdr.IsDBNull(rdr.GetOrdinal("CheckOperatorName")))
                    {
                        payMoneyInfo.CheckOperatorName = rdr["CheckOperatorName"].ToString();
                    }

                    if (!rdr.IsDBNull(rdr.GetOrdinal("OperatorTel")))
                    {
                        payMoneyInfo.OperatorTel = rdr["OperatorTel"].ToString();
                    }

                    if (!rdr.IsDBNull(rdr.GetOrdinal("OperatorMobile")))
                    {
                        payMoneyInfo.OperatorMobile = rdr["OperatorMobile"].ToString();
                    }

                    //if (!rdr.IsDBNull(rdr.GetOrdinal("OperatorMQId")))
                    //{
                    //    payMoneyInfo.OperatorMQId = rdr.GetString(rdr.GetOrdinal("OperatorMQId"));
                    //}

                    if (!rdr.IsDBNull(rdr.GetOrdinal("UseMoney")))
                    {
                        payMoneyInfo.UseMoney = rdr.GetDecimal(rdr.GetOrdinal("UseMoney"));
                    }

                    if (!rdr.IsDBNull(rdr.GetOrdinal("ProvinceId")))
                    {
                        payMoneyInfo.ProvinceId = rdr.GetInt32(rdr.GetOrdinal("ProvinceId"));
                    }

                    if (!rdr.IsDBNull(rdr.GetOrdinal("CityId")))
                    {
                        payMoneyInfo.CityId = rdr.GetInt32(rdr.GetOrdinal("CityId"));
                    }

                    paymoneys.Add(payMoneyInfo);
                }
            }

            return paymoneys;
        }

        /// <summary>
        /// 获得已充值过的公司汇总
        /// </summary>
        /// <param name="pageSize">每页记录数</param>
        /// <param name="pageIndex">当前页索引</param>
        /// <param name="recordCount">总记录数</param>
        /// <param name="companyName">公司名称 为空时不做为查询条件</param>
        /// <returns></returns>
        public IList<EyouSoft.Model.SMSStructure.AccountDetailInfo> GetAllPayedCompanys(int pageSize, int pageIndex, ref int recordCount, string companyName)
        {
            IList<EyouSoft.Model.SMSStructure.AccountDetailInfo> items = new List<EyouSoft.Model.SMSStructure.AccountDetailInfo>();
            StringBuilder strSql = new StringBuilder();
            string tableName = "view_SMS_AllPayedCompany";
            string fields = "CompanyID,CompanyName,ContactName,ContactTel,ContactMobile,ContactMQ,AccountMoney,AccountSMSNumber";
            string orderByString = "AccountSMSNumber,AccountMoney,CompanyID";
            StringBuilder cmdQuery = new StringBuilder();

            if (!string.IsNullOrEmpty(companyName))
            {
                cmdQuery.AppendFormat("CompanyName LIKE '%{0}%'", companyName);
            }

            using (IDataReader rdr = EyouSoft.Toolkit.DAL.DbHelper.ExecuteReader1(this._db, pageSize, pageIndex, ref recordCount, tableName
                , fields, cmdQuery.ToString(), orderByString, string.Empty))
            {
                while (rdr.Read())
                {
                    EyouSoft.Model.SMSStructure.AccountDetailInfo item = new EyouSoft.Model.SMSStructure.AccountDetailInfo();
                    item.CompanyID = rdr.GetInt32(rdr.GetOrdinal("CompanyID"));
                    item.CompanyName = rdr["CompanyName"].ToString();
                    item.ContactName = rdr["ContactName"].ToString();
                    item.Tel = rdr["ContactTel"].ToString();
                    item.Mobile = rdr["ContactMobile"].ToString();
                    //item.MQId = rdr["ContactMQ"].ToString();
                    item.AccountMoney = Convert.ToDecimal(rdr["AccountMoney"].ToString());
                    //item.AccountSMSNumber = Convert.ToInt32(rdr["AccountSMSNumber"].ToString());
                    items.Add(item);
                }
            }
            return items;
            //return null;
        }

        /// <summary>
        /// 获取账户消费明细
        /// </summary>
        /// <param name="pageSize">每页记录数</param>
        /// <param name="pageIndex">当前页索引</param>
        /// <param name="recordCount">总记录数</param>
        /// <param name="companyId">公司编号</param>
        /// <returns></returns>
        public IList<EyouSoft.Model.SMSStructure.SendTotal> GetExpenseDetails(int pageSize, int pageIndex, ref int recordCount, int companyId)
        {
            IList<EyouSoft.Model.SMSStructure.SendTotal> totals = new List<EyouSoft.Model.SMSStructure.SendTotal>();

            StringBuilder cmdQuery = new StringBuilder();
            string tableName = "SMS_SendTotal";
            string orderByString = "IssueTime DESC";
            string fields = " ID,   IssueTime, UseMoeny, SuccessCount, ErrorCount,SendChannel,SMSContent";

            cmdQuery.AppendFormat(" CompanyID='{0}' ", companyId);

            using (IDataReader rdr = EyouSoft.Toolkit.DAL.DbHelper.ExecuteReader1(this._db, pageSize, pageIndex, ref recordCount, tableName
                , fields, cmdQuery.ToString(), orderByString, string.Empty))
            {
                while (rdr.Read())
                {
                    EyouSoft.Model.SMSStructure.SendTotal sendTotalInfo = new EyouSoft.Model.SMSStructure.SendTotal();

                    sendTotalInfo.ID = rdr.GetString(rdr.GetOrdinal("ID"));
                    sendTotalInfo.IssueTime = rdr.GetDateTime(rdr.GetOrdinal("IssueTime"));
                    sendTotalInfo.SuccessCount = rdr.GetInt32(rdr.GetOrdinal("SuccessCount"));
                    sendTotalInfo.ErrorCount = rdr.GetInt32(rdr.GetOrdinal("ErrorCount"));
                    sendTotalInfo.UseMoeny = rdr.GetDecimal(rdr.GetOrdinal("UseMoeny"));
                    sendTotalInfo.SendChannel = new EyouSoft.Model.SMSStructure.SMSChannelList()[rdr.GetInt32(rdr.GetOrdinal("SendChannel"))];
                    sendTotalInfo.SMSContent = rdr.IsDBNull(rdr.GetOrdinal("SMSContent")) ? "" : rdr.GetString(rdr.GetOrdinal("SMSContent"));

                    totals.Add(sendTotalInfo);
                }
            }

            return totals;
        }

        /// <summary>
        /// 充值审核
        /// </summary>
        /// <param name="checkPayMoneyInfo">账户充值业务实体</param>
        /// <returns></returns>
        /// 实体需要设置如下信息：
        /// PayMoneyId：充值支付编号
        /// IsChecked：审核状态 0:未审核 1:审核通过  2:审核未通过
        /// CheckTime：审核时间
        /// CheckUserName：审核人用户名
        /// CheckUserFullName：审核人姓名
        public bool CheckPayMoney(EyouSoft.Model.SMSStructure.PayMoneyInfo checkPayMoneyInfo)
        {
            DbCommand cmd = this._db.GetStoredProcCommand("proc_SMS_CheckPayMoney");

            this._db.AddInParameter(cmd, "PayMoneyId", DbType.String, checkPayMoneyInfo.ID);
            this._db.AddInParameter(cmd, "CheckStatus", DbType.Int32, checkPayMoneyInfo.IsChecked);
            this._db.AddInParameter(cmd, "CheckTime", DbType.DateTime, checkPayMoneyInfo.CheckTime);
            this._db.AddInParameter(cmd, "CheckUserName", DbType.String, checkPayMoneyInfo.CheckUserName);
            this._db.AddInParameter(cmd, "CheckUserFullName", DbType.String, checkPayMoneyInfo.CheckOperatorName);
            this._db.AddOutParameter(cmd, "Result", DbType.Int32, 4);
            this._db.AddInParameter(cmd, "UseMoney", DbType.Decimal, checkPayMoneyInfo.UseMoney);

            EyouSoft.Toolkit.DAL.DbHelper.RunProcedure(cmd, this._db);

            return Convert.ToInt32(this._db.GetParameterValue(cmd, "Result")) == 1 ? true : false;
        }

        /// <summary>
        /// 删除未通过审核[未审核/审核未通过]的充值记录
        /// </summary>
        /// <param name="PayMoneyId">充值记录ID</param>
        /// <returns></returns>
        public bool DeleteNoPassCheckPayMoney(string PayMoneyId)
        {
            DbCommand cmd = this._db.GetSqlStringCommand(SQL_DELETE_NoPassCheckPayMoney);
            this._db.AddInParameter(cmd, "PayMoneyId", DbType.AnsiStringFixedLength, PayMoneyId);

            return EyouSoft.Toolkit.DAL.DbHelper.ExecuteSql(cmd, this._db) > 0 ? true : false;
        }

        /// <summary>
        /// 获取消费明细汇总信息
        /// </summary>
        /// <param name="companyId">公司编号</param>
        /// <returns></returns>
        public EyouSoft.Model.SMSStructure.AccountExpenseCollectInfo GetAccountExpenseCollectInfo(int companyId)
        {
            EyouSoft.Model.SMSStructure.AccountExpenseCollectInfo accountExpenseCollectInfo = new EyouSoft.Model.SMSStructure.AccountExpenseCollectInfo();
            DbCommand cmd = this._db.GetSqlStringCommand(SQL_SELECT_GETACCOUNTEXPENSECOLLECTINFO);
            this._db.AddInParameter(cmd, "COMPANYID", DbType.AnsiStringFixedLength, companyId);

            using (IDataReader rdr = EyouSoft.Toolkit.DAL.DbHelper.ExecuteReader(cmd, this._db))
            {
                if (rdr.Read())
                {
                    if (!rdr.IsDBNull(rdr.GetOrdinal("SentMessageCount")))
                    {
                        accountExpenseCollectInfo.SentMessageCount = rdr.GetInt32(rdr.GetOrdinal("SentMessageCount"));
                    }

                    if (!rdr.IsDBNull(rdr.GetOrdinal("ExpenseAmount")))
                    {
                        accountExpenseCollectInfo.ExpenseAmount = rdr.GetDecimal(rdr.GetOrdinal("ExpenseAmount"));
                    }
                }
            }

            return accountExpenseCollectInfo;
        }

        /// <summary>
        /// 设置短信中心账户基础数据
        /// </summary>
        /// <param name="companyId">公司编号</param>
        /// <returns></returns>
        public bool SetAccountBaseInfo(int companyId)
        {
            DbCommand cmd = this._db.GetSqlStringCommand(SQL_INSERT_SetAccountBaseInfo);

            this._db.AddInParameter(cmd, "ID", DbType.AnsiStringFixedLength, Guid.NewGuid().ToString());
            this._db.AddInParameter(cmd, "CompanyId", DbType.AnsiStringFixedLength, companyId);

            return EyouSoft.Toolkit.DAL.DbHelper.ExecuteSql(cmd, this._db) > 0 ? true : false;
        }

        /// <summary>
        /// 判断是否存在公司账户
        /// </summary>
        /// <param name="companyId">公司编号</param>
        /// <returns></returns>
        public bool IsExistsAccount(int companyId)
        {
            bool tmp = false;
            DbCommand cmd = this._db.GetSqlStringCommand(SQL_SELECT_IsExistsAccount);
            this._db.AddInParameter(cmd, "COMPANYID", DbType.AnsiStringFixedLength, companyId);

            using (IDataReader rdr = EyouSoft.Toolkit.DAL.DbHelper.ExecuteReader(cmd, this._db))
            {
                if (rdr.Read())
                {
                    tmp = rdr.GetInt32(0) > 0 ? true : false;
                }
            }

            return tmp;
        }

        /// <summary>
        /// 短信剩余统计
        /// </summary>
        /// <param name="pageSize">每页记录数</param>
        /// <param name="pageIndex">当前页索引</param>
        /// <param name="recordCount">总数</param>
        /// <param name="expression">剩余金额的数值表达式</param>
        /// <param name="provinceId">省份编号 为null时不做为查询条件</param>
        /// <param name="cityId">城市编号 为null时不做为查询条件</param>
        /// <param name="companyName">公司名称 为null时不做为查询条件</param>
        /// <param name="userAreas">用户分管的区域范围 为null时不做为查询条件</param>
        /// <returns></returns>
        public IList<EyouSoft.Model.SMSStructure.AccountDetailInfo> RemnantStats(int pageSize, int pageIndex, ref int recordCount, double expression, int? provinceId, int? cityId, string companyName, string userAreas)
        {
            IList<EyouSoft.Model.SMSStructure.AccountDetailInfo> items = new List<EyouSoft.Model.SMSStructure.AccountDetailInfo>();
            string tableName = "view_SMS_AllPayedCompany";
            string fields = "CompanyID,CompanyName,ContactName,ContactTel,ContactMobile,ContactMQ,AccountMoney,AccountSMSNumber,ProvinceId,CityId,ContactQQ";
            string orderByString = "AccountSMSNumber,AccountMoney,CompanyID";
            StringBuilder cmdQuery = new StringBuilder();

            cmdQuery.AppendFormat(" AccountMoney<={0} ", expression);

            if (provinceId.HasValue)
            {
                cmdQuery.AppendFormat(" AND ProvinceId={0} ", provinceId.Value);
            }

            if (cityId.HasValue)
            {
                cmdQuery.AppendFormat(" AND CityId={0} ", cityId.Value);
            }

            if (!string.IsNullOrEmpty(companyName))
            {
                cmdQuery.AppendFormat(" AND CompanyName LIKE '%{0}%' ", companyName);
            }

            if (!string.IsNullOrEmpty(userAreas))
            {
                cmdQuery.AppendFormat(" AND CityId IN({0}) ", userAreas);
            }

            using (IDataReader rdr = EyouSoft.Toolkit.DAL.DbHelper.ExecuteReader1(this._db, pageSize, pageIndex, ref recordCount, tableName
                , fields, cmdQuery.ToString(), orderByString, string.Empty))
            {
                while (rdr.Read())
                {
                    EyouSoft.Model.SMSStructure.AccountDetailInfo item = new EyouSoft.Model.SMSStructure.AccountDetailInfo();

                    item.CompanyID = rdr.GetInt32(rdr.GetOrdinal("CompanyID"));
                    item.CompanyName = rdr["CompanyName"].ToString();
                    item.ContactName = rdr["ContactName"].ToString();
                    item.Tel = rdr["ContactTel"].ToString();
                    item.Mobile = rdr["ContactMobile"].ToString();
                    //item.MQId = rdr["ContactMQ"].ToString();
                    item.AccountMoney = Convert.ToDecimal(rdr["AccountMoney"].ToString());
                    //item.AccountSMSNumber = Convert.ToInt32(rdr["AccountSMSNumber"].ToString());
                    item.PrivinceId = rdr.GetInt32(rdr.GetOrdinal("ProvinceId"));
                    item.CityId = rdr.GetInt32(rdr.GetOrdinal("CityId"));
                    item.QQ = rdr["ContactQQ"].ToString();

                    items.Add(item);
                }
            }
            return items;
        }

        #endregion
    }
}
