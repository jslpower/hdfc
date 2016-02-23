using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Practices.EnterpriseLibrary.Data;
using System.Data.Common;
using EyouSoft.Toolkit.DAL;
using System.Data;

namespace EyouSoft.DAL.BackgroundServices
{
    /// <summary>
    /// 定时短信服务数据访问类
    /// </summary>
    public class SmsTimerServices : EyouSoft.Toolkit.DAL.DALBase,EyouSoft.IDAL.BackgroundServices.ISmsTimerServices
    {
        #region static constants
        //static constants
        private const string SQL_SELECT_GetSends = "SELECT [ID],[CompanyID],[CompanyName],[UserID],[ContactName],[SMSType],[SMSContent],[MobileList],[IssueTime],[SendTime],[SendChannel],[SendState],[EncryptMobiles] FROM [SMS_SendPlan] WHERE [SendState]='0' AND [SendTime]<=GETDATE() ORDER BY [SendTime]";
        private const string SQL_UPDATE_SaveSendResult = "UPDATE [SMS_SendPlan] SET [SendState]=@SendState,[StateText]=@StateText,[RealSendTime]=GETDATE() WHERE [ID]=@PlanId";
        #endregion

        #region constructor
        /// <summary>
        /// database
        /// </summary>
        private Database _db = null;

        /// <summary>
        /// default constructor
        /// </summary>
        public SmsTimerServices()
        {
            this._db = base.SystemStore;
        }
        #endregion

        #region private members
        /// <summary>
        /// 根据接收短信手机号码字符串获取手机号码集合
        /// </summary>
        /// <param name="mobiles">手机号码字符串 ","间隔</param>
        /// <param name="isEncrypt">是否在显示时加密</param>
        /// <returns></returns>
        private List<EyouSoft.Model.SMSStructure.AcceptMobileInfo> GetAcceptMobiles(string mobiles, bool isEncrypt)
        {
            List<EyouSoft.Model.SMSStructure.AcceptMobileInfo> list = new List<EyouSoft.Model.SMSStructure.AcceptMobileInfo>();
            if (string.IsNullOrEmpty(mobiles)) return list;

            string[] arr = mobiles.Split(',');

            foreach (string mobile in arr)
            {
                list.Add(new EyouSoft.Model.SMSStructure.AcceptMobileInfo() { IsEncrypt = isEncrypt, Mobile = mobile });
            }

            return list;
        }
        #endregion

        #region ISmsTimerServices 成员

        /// <summary>
        /// 获得要发送的短信
        /// </summary>
        /// <returns></returns>
        public Queue<EyouSoft.Model.SMSStructure.SendPlanInfo> GetSends()
        {
            Queue<EyouSoft.Model.SMSStructure.SendPlanInfo> queue = new Queue<EyouSoft.Model.SMSStructure.SendPlanInfo>();
            DbCommand cmd = this._db.GetSqlStringCommand(SQL_SELECT_GetSends);

            using (IDataReader rdr = DbHelper.ExecuteReader(cmd, this._db))
            {
                while (rdr.Read())
                {
                    EyouSoft.Model.SMSStructure.SendPlanInfo item = new EyouSoft.Model.SMSStructure.SendPlanInfo();
                    item.PlanId = rdr.GetString(rdr.GetOrdinal("ID"));
                    item.CompanyId = rdr.GetInt32(rdr.GetOrdinal("CompanyID"));
                    item.CompanyName = rdr.IsDBNull(rdr.GetOrdinal("CompanyName")) ? "" : rdr.GetString(rdr.GetOrdinal("CompanyName"));
                    item.UserId = rdr.GetInt32(rdr.GetOrdinal("UserID"));
                    item.ContactName = rdr.IsDBNull(rdr.GetOrdinal("ContactName")) ? "" : rdr.GetString(rdr.GetOrdinal("ContactName"));
                    item.SMSType = rdr.GetInt32(rdr.GetOrdinal("SMSType"));
                    item.SMSContent = rdr.IsDBNull(rdr.GetOrdinal("SMSContent")) ? "" : rdr.GetString(rdr.GetOrdinal("SMSContent"));
                    string mobile = rdr.IsDBNull(rdr.GetOrdinal("MobileList")) ? "" : rdr.GetString(rdr.GetOrdinal("MobileList"));
                    string encryptMobiles = rdr.IsDBNull(rdr.GetOrdinal("EncryptMobiles")) ? "" : rdr.GetString(rdr.GetOrdinal("EncryptMobiles"));

                    //显示时不加密手机号
                    item.Mobiles = this.GetAcceptMobiles(mobile, false);

                    item.IssueTime = rdr.GetDateTime(rdr.GetOrdinal("IssueTime"));
                    item.SendTime = rdr.GetDateTime(rdr.GetOrdinal("SendTime"));
                    item.SendChannel = new EyouSoft.Model.SMSStructure.SMSChannel();
                    item.SendChannel.Index = rdr.GetInt32(rdr.GetOrdinal("SendChannel"));

                    queue.Enqueue(item);
                }
            }

            //更新是否发送状态
            if (queue != null && queue.Count > 0)
            {
                foreach (var item in queue)
                {
                    SaveSendResult(item.PlanId, EyouSoft.Model.EnumType.SmsStructure.PlanStatus.发送成功, "");
                }
            }

            return queue;
        }

        /// <summary>
        /// 保存发送短信结果
        /// </summary>
        /// <param name="planId">任务编号</param>
        /// <param name="state">发送结果</param>
        /// <param name="stateDesc">结果描述</param>
        /// <returns></returns>
        public bool SaveSendResult(string planId, EyouSoft.Model.EnumType.SmsStructure.PlanStatus state, string stateDesc)
        {
            Queue<EyouSoft.Model.SMSStructure.SendPlanInfo> queue = new Queue<EyouSoft.Model.SMSStructure.SendPlanInfo>();
            DbCommand cmd = this._db.GetSqlStringCommand(SQL_UPDATE_SaveSendResult);
            this._db.AddInParameter(cmd, "SendState", DbType.AnsiStringFixedLength, (int)state);
            this._db.AddInParameter(cmd, "StateText", DbType.String, stateDesc);
            this._db.AddInParameter(cmd, "PlanId", DbType.AnsiStringFixedLength, planId);

            return DbHelper.ExecuteSql(cmd, this._db) > 0 ? true : false;
        }
        #endregion
    }
}
