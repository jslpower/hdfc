using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EyouSoft.IDAL.CRM
{
    /// <summary>
    /// 生日中心数据接口
    /// </summary>
    public interface IBirthday
    {
        /// <summary>
        /// 添加礼物明细
        /// </summary>
        /// <param name="type">收到礼物对象类型</param>
        /// <param name="id">收到礼物对象编号</param>
        /// <param name="model">礼物基类</param>
        /// <returns>
        /// 1：成功；
        /// 0：参数错误；
        /// -1：新增失败；
        /// </returns>
        int AddBirthdayGift(
            Model.EnumType.CustomerStructure.BirthdayGiftType type, string id, Model.CRM.MBirthdayGiftBase model);

        /// <summary>
        /// 修改礼物明细
        /// </summary>
        /// <param name="model">礼物基类</param>
        /// <returns>
        /// 1：成功；
        /// 0：参数错误；
        /// -1：新增失败；
        /// </returns>
        int UpdateBirthdayGift(Model.CRM.MBirthdayGiftBase model);

        /// <summary>
        /// 删除礼物明细
        /// </summary>
        /// <param name="id">礼物明细编号</param>
        /// <returns>
        /// 1：成功；
        /// 0：参数错误；
        /// -1：删除失败；
        /// </returns>
        int DeleteBirthdayGift(params string[] id);

        /// <summary>
        /// 根据编号获取礼物明细
        /// </summary>
        /// <param name="id">礼物明细编号</param>
        /// <returns>
        /// 返回类型对应的子类
        /// </returns>
        object GetBirthdayGift(string id);

        /// <summary>
        /// 获取礼物明细
        /// </summary>
        /// <param name="type">收到礼物对象类型</param>
        /// <param name="id">收到礼物对象编号</param>
        /// <returns>返回类型对应的子类</returns>
        object GetBirthdayGift(Model.EnumType.CustomerStructure.BirthdayGiftType type, string id);

        /// <summary>
        /// 获取生日提醒列表（seach 有值以seach为准，为null则以days为准）
        /// </summary>
        /// <param name="type">生日提醒类型</param>
        /// <param name="companyId">公司编号</param>
        /// <param name="days">生日提醒提前天数</param>
        /// <param name="pageSize">每页记录数</param>
        /// <param name="pageIndex">当前页数</param>
        /// <param name="recordCount">总记录数</param>
        /// <param name="seach">生日查询实体</param>
        /// <returns>返回类型对应的子类</returns>
        object GetBirthdayList(
            Model.EnumType.CustomerStructure.BirthdayGiftType type,
            int companyId,
            int days,
            int pageSize,
            int pageIndex,
            ref int recordCount,
            Model.CRM.MSearchBirthday seach);

        /// <summary>
        /// 获取生日弹窗提醒
        /// </summary>
        /// <param name="companyId">公司编号</param>
        /// <param name="seach">查询实体</param>
        /// <returns></returns>
        IList<Model.CRM.MBirthdayWindow> GetBirthdayWindow(int companyId, Model.CRM.MSearchBirthday seach);
    }
}
