using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using EyouSoft.IDAL.BackgroundServices;
using EyouSoft.BackgroundServices.IDAL;

namespace EyouSoft.Services.BackgroundServices
{
    /// <summary>
    /// 定时短信服务
    /// </summary>
    public class SmsTimer : BackgroundServiceBase, IBackgroundService
    {
        private readonly ISmsTimerServices dal;
        private Queue<EyouSoft.Model.SMSStructure.SendPlanInfo> _queue = null;

        #region constructure
        public SmsTimer(IPluginService pluginService, ISmsTimerServices smsTimerService)
            : base(pluginService)
        {
            this.dal = smsTimerService;
            ID = new Guid("{fa6cb875-261c-45e3-b28b-6cd147b79c2b}");
            Name = "定时短信服务";
            Category = "Background Services";
        }
        #endregion

        #region private members
        /// <summary>
        /// 发送短信
        /// </summary>
        /// <param name="info">计划信息</param>
        private void SendSMS(EyouSoft.Model.SMSStructure.SendPlanInfo info)
        {
            EyouSoft.BackgroundServices.SMSSOAP.SMSChannel channel = SmsUtils.GetSmsSendChannel(info.SendChannel.Index);

            EyouSoft.BackgroundServices.SMSSOAP.SendMessageInfo messageInfo = new EyouSoft.BackgroundServices.SMSSOAP.SendMessageInfo();
            EyouSoft.BackgroundServices.SMSSOAP.AcceptMobileInfo[] mobiles = new EyouSoft.BackgroundServices.SMSSOAP.AcceptMobileInfo[info.Mobiles.Count];
            for (int i = 0; i < info.Mobiles.Count; i++)
            {
                mobiles[i] = new EyouSoft.BackgroundServices.SMSSOAP.AcceptMobileInfo();
                mobiles[i].Mobile = info.Mobiles[i].Mobile;
                mobiles[i].IsEncrypt = info.Mobiles[i].IsEncrypt;
            }
            messageInfo.Mobiles = mobiles;
            messageInfo.CompanyId = info.CompanyId;
            messageInfo.CompanyName = info.CompanyName;
            messageInfo.SendChannel = channel;
            messageInfo.SendTime = info.SendTime;
            messageInfo.SendType = EyouSoft.BackgroundServices.SMSSOAP.SendType.直接发送;
            messageInfo.SMSContent = info.SMSContent;
            messageInfo.SMSType = info.SMSType;
            messageInfo.UserFullName = info.ContactName;
            messageInfo.UserId = info.UserId;

            EyouSoft.BackgroundServices.SMSSOAP.SMSAPI api = EyouSoft.Services.BackgroundServices.SmsUtils.GetSmsApi();

            EyouSoft.BackgroundServices.SMSSOAP.SendResultInfo result = api.Send(messageInfo);

            /*
            EyouSoft.Model.EnumType.SmsStructure.PlanStatus state = EyouSoft.Model.EnumType.SmsStructure.PlanStatus.未发送;
            string stateDesc = string.Empty;

            if (result.IsSucceed)
            {
                state = EyouSoft.Model.EnumType.SmsStructure.PlanStatus.发送成功;
            }
            else
            {
                state = EyouSoft.Model.EnumType.SmsStructure.PlanStatus.发送失败;
                stateDesc = result.ErrorMessage;
            }

            dal.SaveSendResult(info.PlanId, state, stateDesc);*/
        }
        #endregion

        #region IBackgroundService Members

        public bool ExecuteOnAll
        {
            get
            {
                return bool.Parse(GetSetting("ExecuteOnAll"));
            }
            set
            {
                SaveSetting("ExecuteOnAll", value.ToString());
            }
        }

        public TimeSpan Interval
        {
            get
            {
                return new TimeSpan(long.Parse(GetSetting("Interval")));
            }
            set
            {
                SaveSetting("Interval", value.Ticks.ToString());
            }
        }

        public void Run()
        {
            SmsUtils.WLog("短信中心定时短信后台服务开启\r\n", "Config/BackgroundServicesLog.txt");

            try
            {
                //获取要发送的短信集合
                if (this._queue == null || this._queue.Count == 0)
                {
                    this._queue = this.dal.GetSends();
                }

                //发送短信
                if (this._queue != null && this._queue.Count > 0)
                {
                    SmsUtils.WLog(string.Format("短信中心定时短信后台服务发送短信记录数：{0}", this._queue.Count), "Config/BackgroundServicesLog.txt");
                    while (true)
                    {
                        if (this._queue.Count == 0) break;
                        var info = this._queue.Dequeue();
                        this.SendSMS(info);
                    }
                }
            }
            catch { }
        }
        #endregion
    }
}
