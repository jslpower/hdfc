using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EyouSoft.IDAL.BackgroundServices
{
    /// <summary>
    /// 定时短信服务数据类访问接口
    /// </summary>
    public interface ISmsTimerServices
    {
        /// <summary>
        /// 获得要发送的短信
        /// </summary>
        /// <returns></returns>
        Queue<EyouSoft.Model.SMSStructure.SendPlanInfo> GetSends();

        /// <summary>
        /// 保存发送短信结果
        /// </summary>
        /// <param name="planId">任务编号</param>
        /// <param name="state">发送结果</param>
        /// <param name="stateDesc">结果描述</param>
        /// <returns></returns>
        bool SaveSendResult(string planId, EyouSoft.Model.EnumType.SmsStructure.PlanStatus state, string stateDesc);
    }
}
