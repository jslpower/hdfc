using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace EyouSoft.Services.BackgroundServices
{
    public class SmsUtils
    {
        /// <summary>
        /// 获取短信发送通道
        /// </summary>
        /// <param name="channelIndex"></param>
        /// <returns></returns>
        public static EyouSoft.BackgroundServices.SMSSOAP.SMSChannel GetSmsSendChannel(int channelIndex)
        {
            EyouSoft.Model.SMSStructure.SMSChannel channel = new EyouSoft.Model.SMSStructure.SMSChannelList()[channelIndex];

            EyouSoft.BackgroundServices.SMSSOAP.SMSChannel sendChannel = new EyouSoft.BackgroundServices.SMSSOAP.SMSChannel();
            sendChannel.ChannelName = channel.ChannelName;
            sendChannel.Index = channel.Index;
            sendChannel.IsLong = channel.IsLong;
            sendChannel.PriceOne = channel.PriceOne;
            sendChannel.Pw = channel.Pw;
            sendChannel.UserName = channel.UserName;

            return sendChannel;
        }

        /// <summary>
        /// get sms send api
        /// </summary>
        /// <returns></returns>
        public static EyouSoft.BackgroundServices.SMSSOAP.SMSAPI GetSmsApi()
        {
            EyouSoft.BackgroundServices.SMSSOAP.SMSAPI api = new EyouSoft.BackgroundServices.SMSSOAP.SMSAPI();
            EyouSoft.BackgroundServices.SMSSOAP.APISoapHeader header = new EyouSoft.BackgroundServices.SMSSOAP.APISoapHeader();
            header.SecretKey = EyouSoft.Toolkit.ConfigHelper.ConfigClass.GetConfigString("TMIS_APIKey");
            api.APISoapHeaderValue = header;

            return api;
        }

        /// <summary>
        /// 写入日志
        /// </summary>
        /// <param name="s">内容</param>
        /// <param name="path">相对路径</param>
        public static void WLog(string s,string path)
        {
#if DEBUG
            string fPath = EyouSoft.Toolkit.Utils.GetMapPath(path);
            if (!File.Exists(fPath))
            {
                FileStream fs = File.Create(fPath);
                fs.Close();
            }

            try
            {
                StreamWriter sw = new StreamWriter(fPath, true, System.Text.Encoding.UTF8);
                sw.Write(DateTime.Now.ToString() + s + "\r\n");
                sw.Close();
            }
            catch { }
#endif
        }
    }
}
