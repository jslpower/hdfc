using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EyouSoft.IDAL.BackgroundServices
{
    /// <summary>
    /// 客户关怀定时短信服务数据访问类接口
    /// </summary>
    public interface ICaringSmsTimerServices
    {
        /// <summary>
        /// 获取固定时间发送的客户关怀计划
        /// </summary>
        /// <param name="Time">固定发送时间</param>
        /// <returns></returns>
        IList<EyouSoft.Model.CompanyStructure.CustomerCareforSendInfo> GetTimeSend(DateTime Time);

        /// <summary>
        /// 获取固定节假日发送的客户关怀计划
        /// </summary>
        /// <param name="FixType">节假日类型</param>
        /// <returns></returns>
        IList<EyouSoft.Model.CompanyStructure.CustomerCareforSendInfo> GetFixTypeSend(params EyouSoft.Model.EnumType.CompanyStructure.CustomerCareForSendSpecialTime[] FixType);

        /// <summary>
        /// 更新当天是否已发送
        /// </summary>
        /// <param name="CustomerCareforId">客户关怀Id</param>
        /// <param name="IsSeded">当天是否已发送（1为是；0为否）</param>
        /// <returns>返回1成功，其他失败</returns>
        int UpdateIsSeded(int CustomerCareforId, bool IsSeded);
    }
}
