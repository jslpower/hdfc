using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Practices.EnterpriseLibrary.Data;
using System.Data;
using System.Data.Common;

namespace EyouSoft.DAL.SMSStructure
{
    /// <summary>
    /// 短信中心-发送短信数据访问类
    /// </summary>
    public class SendMessage : EyouSoft.Toolkit.DAL.DALBase, EyouSoft.IDAL.SMSStructure.ISendMessage
    {
        #region static constants
        private const string SQL_SELECT_GETSENDCONTENT = "SELECT [SMSContent] FROM [SMS_SendTotal] WHERE [ID]=@SENDTOTALID AND [CompanyID]=@COMPANYID ";
        private const string SQL_INSERT_InsertSendPlan = "INSERT INTO [SMS_SendPlan]([ID],[CompanyID],[CompanyName],[UserID],[ContactName],[SMSType],[SMSContent],[MobileList],[IssueTime],[SendTime],[SendChannel],[EncryptMobiles]) VALUES(@ID,@CompanyID,@CompanyName,@UserID,@ContactName,@SMSType,@SMSContent,@MobileList,@IssueTime,@SendTime,@SendChannel,@EncryptMobiles)";
        /// <summary>
        /// 发送短信超时时异常编号
        /// </summary>
        private const int SendTimeOutEventCode = -2147483646;

        private readonly Database _db = null;
        #endregion static constants

        #region 构造函数
        public SendMessage()
        {
            this._db = base.SystemStore;
        }
        #endregion

        #region ISendMessage 成员

        /// <summary>
        /// 写入短信发送明细及统计信息，并更新账户余额
        /// </summary>
        /// <param name="sendMessageInfo">发送短信提交的业务实体</param>
        /// <param name="sendDetailsInfo">短信发送明细</param>
        /// <param name="sendResultInfo">发送短信/验证发送结果业务实体</param>
        /// <returns></returns>
        public bool InsertSendInfo(EyouSoft.Model.SMSStructure.SendMessageInfo sendMessageInfo, IList<EyouSoft.Model.SMSStructure.SendDetail> sendDetailsInfo, EyouSoft.Model.SMSStructure.SendResultInfo sendResultInfo)
        {
            DbCommand cmd = this._db.GetStoredProcCommand("proc_SMS_InsertSendMessageInfo");
            this._db.AddInParameter(cmd, "SendTotalId", DbType.String, sendResultInfo.SendTotalId);
            this._db.AddInParameter(cmd, "CompanyId", DbType.String, sendMessageInfo.CompanyId);
            this._db.AddInParameter(cmd, "CompanyName", DbType.String, sendMessageInfo.CompanyName);
            this._db.AddInParameter(cmd, "UserId", DbType.String, sendMessageInfo.UserId);
            this._db.AddInParameter(cmd, "UserFullName", DbType.String, sendMessageInfo.UserFullName);
            this._db.AddInParameter(cmd, "SMSType", DbType.Int32, sendMessageInfo.SMSType);
            this._db.AddInParameter(cmd, "SMSContent", DbType.String, sendMessageInfo.SMSContent);
            this._db.AddInParameter(cmd, "UseMoeny", DbType.Decimal, 0.01M * sendMessageInfo.SendChannel.PriceOne);
            this._db.AddInParameter(cmd, "SuccessCount", DbType.Int32, sendResultInfo.SuccessCount);
            this._db.AddInParameter(cmd, "ErrorCount", DbType.Int32, sendResultInfo.ErrorCount);
            this._db.AddInParameter(cmd, "TimeoutCount", DbType.Int32, sendResultInfo.TimeoutCount);
            this._db.AddInParameter(cmd, "FactCount", DbType.Int32, sendResultInfo.FactCount);
            this._db.AddInParameter(cmd, "TempFeeTakeId", DbType.String, sendResultInfo.TempFeeTakeId);
            this._db.AddInParameter(cmd, "SendMessageTime", DbType.DateTime, sendMessageInfo.SendTime);
            this._db.AddInParameter(cmd, "Mobiles", DbType.String, this.CreateSendMessageMobilesXML(sendDetailsInfo));
            this._db.AddOutParameter(cmd, "Result", DbType.Int32, 4);
            this._db.AddInParameter(cmd, "PHSSuccessCount", DbType.Int32, sendResultInfo.PHSSuccessCount);
            this._db.AddInParameter(cmd, "PHSErrorCount", DbType.Int32, sendResultInfo.PHSErrorCount);
            this._db.AddInParameter(cmd, "PHSTimeoutCount", DbType.Int32, sendResultInfo.PHSTimeoutCount);
            this._db.AddInParameter(cmd, "PHSFactCount", DbType.Int32, sendResultInfo.PHSFactCount);
            this._db.AddInParameter(cmd, "SendChannel", DbType.Int32, sendMessageInfo.SendChannel.Index);

            EyouSoft.Toolkit.DAL.DbHelper.RunProcedure(cmd, this._db);

            return Convert.ToInt32(this._db.GetParameterValue(cmd, "Result")) == 1 ? true : false;
        }

        /// <summary>
        /// 根据指定条件获取发送短信历史记录
        /// </summary>
        /// <param name="pageSize">每页记录数</param>
        /// <param name="pageIndex">当前页索引</param>
        /// <param name="recordCount">总记录数</param>
        /// <param name="companyId">公司编号</param>
        /// <param name="keyword">关键字</param>
        /// <param name="sendStatus">发送状态 0:所有 1:成功 2:失败</param>
        /// <param name="startTime">发送开始时间 为空时不做为查询条件</param>
        /// <param name="finishTime">发送截止时间 为空时不做为查询条件</param>
        /// <returns></returns>
        public IList<EyouSoft.Model.SMSStructure.SendDetail> GetSendHistorys(int pageSize, int pageIndex, ref int recordCount, string companyId, string keyword, int sendStatus, DateTime? startTime, DateTime? finishTime)
        {
            IList<EyouSoft.Model.SMSStructure.SendDetail> sendHistorys = new List<EyouSoft.Model.SMSStructure.SendDetail>();

            StringBuilder cmdQuery = new StringBuilder();
            string tableName = "SMS_SendDetail";
            string orderByString = "IssueTime DESC";
            string fields = " ID, CompanyID, UserID, SendTotalID, SMSType, MobileNumber, SMSContent, SendTime, ReturnResult, ReturnMsg, UseMoeny, SMSSplitCount,  IssueTime,IsEncrypt";

            #region 查询条件

            cmdQuery.AppendFormat(" CompanyID='{0}' ", companyId.ToString());

            if (!string.IsNullOrEmpty(keyword))
            {
                cmdQuery.AppendFormat(" AND (SMSContent LIKE '%{0}%') ", keyword);
            }

            switch (sendStatus)
            {
                case 0:
                    cmdQuery.AppendFormat(" AND(ReturnResult>{0}) ", SendTimeOutEventCode);
                    break;
                case 1:
                    cmdQuery.Append(" AND(ReturnResult=0) ");
                    break;
                case 2:
                    cmdQuery.AppendFormat(" AND(ReturnResult>{0}) AND (ReturnResult<0) ", SendTimeOutEventCode);
                    break;
                default:
                    cmdQuery.AppendFormat(" AND(ReturnResult>{0})  ", SendTimeOutEventCode);
                    break;
            }

            if (startTime.HasValue)
            {
                cmdQuery.AppendFormat(" AND datediff(dd,'{0}',SendTime) >= 0 ", startTime.Value);
            }

            if (finishTime.HasValue)
            {
                cmdQuery.AppendFormat(" AND datediff(dd,'{0}',SendTime) <= 0 ", finishTime.Value);
            }

            #endregion

            using (IDataReader rdr = EyouSoft.Toolkit.DAL.DbHelper.ExecuteReader1(this._db, pageSize, pageIndex, ref recordCount, tableName
                , fields, cmdQuery.ToString(), orderByString, string.Empty))
            {
                while (rdr.Read())
                {
                    EyouSoft.Model.SMSStructure.SendDetail sendDetailInfo = new EyouSoft.Model.SMSStructure.SendDetail();

                    sendDetailInfo.ID = rdr.GetString(rdr.GetOrdinal("ID"));
                    sendDetailInfo.CompanyID = rdr.GetInt32(rdr.GetOrdinal("CompanyID"));
                    sendDetailInfo.UserID = rdr.GetInt32(rdr.GetOrdinal("UserID"));
                    sendDetailInfo.SendTotalID = rdr.GetString(rdr.GetOrdinal("SendTotalID"));
                    sendDetailInfo.SMSType = rdr.GetInt32(rdr.GetOrdinal("SMSType"));
                    sendDetailInfo.MobileNumber = rdr["MobileNumber"].ToString();
                    sendDetailInfo.SMSContent = rdr["SMSContent"].ToString();
                    sendDetailInfo.SendTime = rdr.GetDateTime(rdr.GetOrdinal("SendTime"));
                    sendDetailInfo.ReturnResult = rdr.GetInt32(rdr.GetOrdinal("ReturnResult"));
                    sendDetailInfo.ReturnMsg = rdr["ReturnMsg"].ToString();
                    sendDetailInfo.UseMoeny = rdr.GetDecimal(rdr.GetOrdinal("UseMoeny"));
                    sendDetailInfo.IssueTime = rdr.GetDateTime(rdr.GetOrdinal("IssueTime"));
                    sendDetailInfo.IsEncrypt = rdr.GetString(rdr.GetOrdinal("IsEncrypt")) == "1" ? true : false;
                    sendDetailInfo.SMSContent = rdr.IsDBNull(rdr.GetOrdinal("SMSContent")) ? "" : rdr.GetString(rdr.GetOrdinal("SMSContent"));

                    sendHistorys.Add(sendDetailInfo);
                }
            }

            return sendHistorys;
        }

        /// <summary>
        /// 根据指定的短信发送统计编号获取发送号码列表
        /// </summary>
        /// <param name="totalId">短信发送统计编号</param>
        /// <param name="companyId">公司编号</param>
        /// <param name="sendStatus">发送状态 0:所有 1:成功 2:失败</param>
        /// <returns></returns>
        public IList<EyouSoft.Model.SMSStructure.SendDetail> GetSendDetails(string totalId, string companyId, int sendStatus)
        {
            IList<EyouSoft.Model.SMSStructure.SendDetail> sendDetails = new List<EyouSoft.Model.SMSStructure.SendDetail>();
            StringBuilder cmdText = new StringBuilder();

            cmdText.Append(" SELECT [ID], [CompanyID], [UserID],[MobileNumber],[ReturnResult],[IsEncrypt] FROM  [SMS_SendDetail] WHERE [CompanyId]=@COMPANYID AND [SendTotalID]=@SENDTOTALID ");

            switch (sendStatus)
            {
                case 0:
                    cmdText.AppendFormat(" AND(ReturnResult>{0})  ", SendTimeOutEventCode);
                    break;
                case 1:
                    cmdText.Append(" AND(ReturnResult=0) ");
                    break;
                case 2:
                    cmdText.AppendFormat(" AND(ReturnResult>{0}) AND (ReturnResult<0) ", SendTimeOutEventCode);
                    break;
                default:
                    cmdText.AppendFormat(" AND(ReturnResult>{0})  ", SendTimeOutEventCode);
                    break;
            }

            DbCommand cmd = this._db.GetSqlStringCommand(cmdText.ToString());
            this._db.AddInParameter(cmd, "COMPANYID", DbType.AnsiStringFixedLength, companyId);
            this._db.AddInParameter(cmd, "SENDTOTALID", DbType.AnsiStringFixedLength, totalId);


            using (IDataReader rdr = EyouSoft.Toolkit.DAL.DbHelper.ExecuteReader(cmd, this._db))
            {
                while (rdr.Read())
                {
                    EyouSoft.Model.SMSStructure.SendDetail sendDetailInfo = new EyouSoft.Model.SMSStructure.SendDetail();

                    sendDetailInfo.ID = rdr.GetString(rdr.GetOrdinal("ID"));
                    sendDetailInfo.CompanyID = rdr.GetInt32(rdr.GetOrdinal("CompanyID"));
                    sendDetailInfo.UserID = rdr.GetInt32(rdr.GetOrdinal("UserID"));
                    sendDetailInfo.MobileNumber = rdr["MobileNumber"].ToString();
                    sendDetailInfo.ReturnResult = rdr.GetInt32(rdr.GetOrdinal("ReturnResult"));
                    sendDetailInfo.IsEncrypt = rdr.GetString(rdr.GetOrdinal("IsEncrypt")) == "1" ? true : false;

                    sendDetails.Add(sendDetailInfo);
                }
            }

            return sendDetails;
        }

        /// <summary>
        /// 根据指定的短信发送统计编号获取发送短信的内容
        /// </summary>
        /// <param name="totalId">短信统计编号</param>
        /// <param name="companyId">公司编号</param>
        /// <returns></returns>
        public string GetSendContent(string totalId, string companyId)
        {
            string content = string.Empty;
            DbCommand cmd = this._db.GetSqlStringCommand(SQL_SELECT_GETSENDCONTENT);
            this._db.AddInParameter(cmd, "SENDTOTALID", DbType.AnsiStringFixedLength, totalId);
            this._db.AddInParameter(cmd, "COMPANYID", DbType.AnsiStringFixedLength, companyId);

            using (IDataReader rdr = EyouSoft.Toolkit.DAL.DbHelper.ExecuteReader(cmd, this._db))
            {
                if (rdr.Read())
                {
                    content = rdr[0].ToString();
                }
            }

            return content;
        }

        public IList<EyouSoft.Model.SMSStructure.SendDetail> GetAllSendHistorys(string companyId, string keyword, int sendStatus, DateTime? startTime, DateTime? finishTime)
        {
            return null;
        }

        /// <summary>
        /// 写入定时发送短信任务计划
        /// </summary>
        /// <param name="planInfo"></param>
        /// <returns></returns>
        public bool InsertSendPlan(EyouSoft.Model.SMSStructure.SendPlanInfo planInfo)
        {
            DbCommand cmd = this._db.GetSqlStringCommand(SQL_INSERT_InsertSendPlan);

            StringBuilder mobiles = new StringBuilder();
            StringBuilder encryptMobiles = new StringBuilder();

            if (planInfo.Mobiles != null && planInfo.Mobiles.Count > 0)
            {
                foreach (var mobile in planInfo.Mobiles)
                {
                    if (mobile.IsEncrypt)
                    {
                        encryptMobiles.AppendFormat("{0},", mobile.Mobile);
                    }
                    else
                    {
                        mobiles.AppendFormat("{0},", mobile.Mobile);
                    }
                }
            }

            this._db.AddInParameter(cmd, "ID", DbType.AnsiStringFixedLength, Guid.NewGuid().ToString());
            this._db.AddInParameter(cmd, "CompanyID", DbType.AnsiStringFixedLength, planInfo.CompanyId);
            this._db.AddInParameter(cmd, "CompanyName", DbType.String, planInfo.CompanyName);
            this._db.AddInParameter(cmd, "UserID", DbType.AnsiStringFixedLength, planInfo.UserId);
            this._db.AddInParameter(cmd, "ContactName", DbType.String, planInfo.ContactName);
            this._db.AddInParameter(cmd, "SMSType", DbType.Int32, planInfo.SMSType);
            this._db.AddInParameter(cmd, "SMSContent", DbType.String, planInfo.SMSContent);
            this._db.AddInParameter(cmd, "MobileList", DbType.String, mobiles.ToString().Trim(','));
            this._db.AddInParameter(cmd, "IssueTime", DbType.DateTime, planInfo.IssueTime);
            this._db.AddInParameter(cmd, "SendTime", DbType.DateTime, planInfo.SendTime);
            this._db.AddInParameter(cmd, "SendChannel", DbType.Int32, planInfo.SendChannel.Index);
            this._db.AddInParameter(cmd, "EncryptMobiles", DbType.String, encryptMobiles.ToString().Trim(','));

            return EyouSoft.Toolkit.DAL.DbHelper.ExecuteSql(cmd, this._db) == 1 ? true : false;
        }

        #endregion

        /// <summary>
        /// 创建发送发送到的手机XML数据
        /// </summary>
        /// <param name="sendDetailsInfo"></param>
        /// <returns></returns>
        private string CreateSendMessageMobilesXML(IList<EyouSoft.Model.SMSStructure.SendDetail> sendDetailsInfo)
        {
            StringBuilder xmlInfo = new StringBuilder();

            xmlInfo.Append("<ROOT>");

            foreach (EyouSoft.Model.SMSStructure.SendDetail sendDetailInfo in sendDetailsInfo)
            {
                xmlInfo.AppendFormat("<MobileInfo Number=\"{0}\" ReturnResult=\"{1}\" ReturnMsg=\"{2}\" FactCount=\"{3}\" GUID=\"{4}\" IsEncrypt=\"{5}\" />"
                    , sendDetailInfo.MobileNumber
                    , sendDetailInfo.ReturnResult.ToString()
                    , sendDetailInfo.ReturnMsg
                    , sendDetailInfo.FactCount
                    , sendDetailInfo.ID
                    , sendDetailInfo.IsEncrypt ? "1" : "0");
            }

            xmlInfo.Append("</ROOT>");

            return xmlInfo.ToString();
        }
    }
}
