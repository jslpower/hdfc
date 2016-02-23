/*Author:汪奇志 2011-02-11*/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EyouSoft.Model.EnumType
{
    public class SmsStructure
    {
        /// <summary>
        /// 定时短信任务发送结果
        /// </summary>
        public enum PlanStatus
        {
            /// <summary>
            /// 未发送
            /// </summary>
            未发送 = 0,
            /// <summary>
            /// 发送成功
            /// </summary>
            发送成功 = 1,
            /// <summary>
            /// 发送失败
            /// </summary>
            发送失败 = 2
        }
    }
}
