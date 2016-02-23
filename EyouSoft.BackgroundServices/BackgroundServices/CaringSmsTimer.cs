using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using EyouSoft.IDAL.BackgroundServices;
using EyouSoft.Toolkit;
using EyouSoft.BackgroundServices.IDAL;

namespace EyouSoft.Services.BackgroundServices
{
    /// <summary>
    /// 客户关怀定时短信服务
    /// </summary>
    public class CaringSmsTimer:BackgroundServiceBase, IBackgroundService
    {
        private readonly ICaringSmsTimerServices dal;

        public CaringSmsTimer(IPluginService pluginService, ICaringSmsTimerServices caringSmsTimerService)
            : base(pluginService)
        {
            this.dal = caringSmsTimerService;
            ID = new Guid("{2882b72b-04ab-486e-8ec3-0473244f34fa}");
            Name = "客户关怀定时短信服务";
            Category = "Background Services";
        }

        #region private members
        /// <summary>
        /// 获取客户关怀节假日短信发送类型
        /// </summary>
        /// <returns></returns>
        private EyouSoft.Model.EnumType.CompanyStructure.CustomerCareForSendSpecialTime[] GetSpecialTimeTypes()
        {
            DateTime now = DateTime.Today;
            Toolkit.LunarCalendar lunarCalendar = Toolkit.Utils.GetLunarCalendar(now);
            IList<EyouSoft.Model.EnumType.CompanyStructure.CustomerCareForSendSpecialTime> items = new List<EyouSoft.Model.EnumType.CompanyStructure.CustomerCareForSendSpecialTime>();

            items.Add(EyouSoft.Model.EnumType.CompanyStructure.CustomerCareForSendSpecialTime.生日);

            if (lunarCalendar.Month == 1 && lunarCalendar.Day==1)//lunarCalendar.Month == 12 && lunarCalendar.DaysInMonth == lunarCalendar.Day
                items.Add(EyouSoft.Model.EnumType.CompanyStructure.CustomerCareForSendSpecialTime.春节);

            if (now.Month == 10 && now.Day == 1)
                items.Add(EyouSoft.Model.EnumType.CompanyStructure.CustomerCareForSendSpecialTime.国庆);

            if (now.Month == 12 && now.Day == 25)
                items.Add(EyouSoft.Model.EnumType.CompanyStructure.CustomerCareForSendSpecialTime.圣诞);

            if (now.Month == 5 && now.Day == 1)
                items.Add(EyouSoft.Model.EnumType.CompanyStructure.CustomerCareForSendSpecialTime.五一);

            if (now.Month == 1 && now.Day == 1)
                items.Add(EyouSoft.Model.EnumType.CompanyStructure.CustomerCareForSendSpecialTime.元旦);

            if (lunarCalendar.Month == 1 && lunarCalendar.Day == 15)
                items.Add(EyouSoft.Model.EnumType.CompanyStructure.CustomerCareForSendSpecialTime.元宵);

            if (lunarCalendar.Month == 8 && lunarCalendar.Day == 15)
                items.Add(EyouSoft.Model.EnumType.CompanyStructure.CustomerCareForSendSpecialTime.中秋);

            EyouSoft.Model.EnumType.CompanyStructure.CustomerCareForSendSpecialTime[] returnTypes = new EyouSoft.Model.EnumType.CompanyStructure.CustomerCareForSendSpecialTime[items.Count];
            int i = 0;
            foreach (var item in items)
            {
                returnTypes[i++] = item;
            }

            return returnTypes;
        }

        /// <summary>
        /// 发送短信
        /// </summary>
        /// <param name="info"></param>
        private void SendMessage(EyouSoft.Model.CompanyStructure.CustomerCareforSendInfo info)
        {
            if (info == null) return;
            if (string.IsNullOrEmpty(info.MobileCode) && (info.NameAndMobile == null || info.NameAndMobile.Count < 1)) return;

            EyouSoft.BackgroundServices.SMSSOAP.SMSChannel channel = SmsUtils.GetSmsSendChannel(info.ChannelId);

            #region 输入号码
            if (!string.IsNullOrEmpty(info.MobileCode))//输入号码
            {
                EyouSoft.BackgroundServices.SMSSOAP.SendMessageInfo messageInfo = new EyouSoft.BackgroundServices.SMSSOAP.SendMessageInfo();
                string[] mobileArr = info.MobileCode.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                EyouSoft.BackgroundServices.SMSSOAP.AcceptMobileInfo[] mobiles = new EyouSoft.BackgroundServices.SMSSOAP.AcceptMobileInfo[mobileArr.Length];
                for (int i = 0; i < mobileArr.Length; i++)
                {
                    mobiles[i] = new EyouSoft.BackgroundServices.SMSSOAP.AcceptMobileInfo();
                    mobiles[i].Mobile = mobileArr[i];
                    mobiles[i].IsEncrypt = false;
                }

                SmsUtils.WLog(string.Format("客户关怀后台服务发送固定时间(指定的时间值)将要发送短信条数：{0}", mobiles.Length), "Config/BackgroundServicesLog.txt");

                messageInfo.Mobiles = mobiles;
                messageInfo.CompanyId = info.CompanyId;
                messageInfo.CompanyName = "";
                messageInfo.SendChannel = channel;
                messageInfo.SendTime = DateTime.Now;
                messageInfo.SendType = EyouSoft.BackgroundServices.SMSSOAP.SendType.直接发送;
                messageInfo.SMSContent = info.Content;
                messageInfo.SMSType =0;
                messageInfo.UserFullName = "";
                messageInfo.UserId = info.OperatorId;

                EyouSoft.BackgroundServices.SMSSOAP.SMSAPI api = EyouSoft.Services.BackgroundServices.SmsUtils.GetSmsApi();

                EyouSoft.BackgroundServices.SMSSOAP.SendResultInfo result = api.Send(messageInfo);
            }
            #endregion

            #region 匹配号码
            if (info.NameAndMobile != null && info.NameAndMobile.Count > 0)//匹配号码
            {
                SmsUtils.WLog(string.Format("客户关怀后台服务发送节假日及生日短信将要发送短信条数：{0}", info.NameAndMobile.Count), "Config/BackgroundServicesLog.txt");
                foreach (var nameAndMobile in info.NameAndMobile)
                {
                    if (string.IsNullOrEmpty(nameAndMobile.ContactMobile)) continue;

                    EyouSoft.BackgroundServices.SMSSOAP.SendMessageInfo messageInfo = new EyouSoft.BackgroundServices.SMSSOAP.SendMessageInfo();
                    EyouSoft.BackgroundServices.SMSSOAP.AcceptMobileInfo[] mobiles = new EyouSoft.BackgroundServices.SMSSOAP.AcceptMobileInfo[1];
                    mobiles[0] = new EyouSoft.BackgroundServices.SMSSOAP.AcceptMobileInfo();
                    mobiles[0].Mobile = nameAndMobile.ContactMobile;
                    mobiles[0].IsEncrypt = false;

                    messageInfo.Mobiles = mobiles;
                    messageInfo.CompanyId = info.CompanyId;
                    messageInfo.CompanyName = "";
                    messageInfo.SendChannel = channel;
                    messageInfo.SendTime = DateTime.Now;
                    messageInfo.SendType = EyouSoft.BackgroundServices.SMSSOAP.SendType.直接发送;
                    messageInfo.SMSContent = info.Content.Replace("[姓名]", nameAndMobile.ContactName);
                    messageInfo.SMSType = 0;
                    messageInfo.UserFullName = "";
                    messageInfo.UserId = info.OperatorId;

                    EyouSoft.BackgroundServices.SMSSOAP.SMSAPI api = EyouSoft.Services.BackgroundServices.SmsUtils.GetSmsApi();

                    EyouSoft.BackgroundServices.SMSSOAP.SendResultInfo result = api.Send(messageInfo);
                }
            }
            #endregion
        }

        /// <summary>
        /// 发送固定时间(指定的时间值)短信
        /// </summary>
        private void SendFixedTimeMessage()
        {
            IList<EyouSoft.Model.CompanyStructure.CustomerCareforSendInfo> items = dal.GetTimeSend(DateTime.Now);

            if (items != null && items.Count > 0)
            {
                SmsUtils.WLog(string.Format("客户关怀后台服务发送固定时间(指定的时间值)短信记录数：{0}", items.Count), "Config/BackgroundServicesLog.txt");
                foreach (var item in items)
                {
                    this.SendMessage(item);
                }
            }
        }

        /// <summary>
        /// 发送节假日及生日短信
        /// </summary>
        private void SendHolidaysMessage()
        {
            DateTime now = DateTime.Now;
            //客户关怀固定节假日及生日短信发送时间(点时间)验证
            int caringSmsSendHours =Utils.GetInt( Toolkit.ConfigHelper.ConfigClass.GetConfigString("CaringSmsSendHours"),9);
            if (now.Hour != caringSmsSendHours) return;

            IList<EyouSoft.Model.CompanyStructure.CustomerCareforSendInfo> items = dal.GetFixTypeSend(this.GetSpecialTimeTypes());

            if (items != null && items.Count > 0)
            {
                SmsUtils.WLog(string.Format("客户关怀后台服务发送节假日及生日短信记录数：{0}", items.Count), "Config/BackgroundServicesLog.txt");
                foreach (var item in items)
                {
                    this.SendMessage(item);
                }
            }            
        }
        #endregion

        #region IBackgroundService 成员

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
            SmsUtils.WLog("客户关怀后台服务开启\r\n", "Config/BackgroundServicesLog.txt");
            this.SendFixedTimeMessage();
            this.SendHolidaysMessage();
        }

        #endregion
    }
}
