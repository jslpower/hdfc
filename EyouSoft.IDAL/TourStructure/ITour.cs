using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EyouSoft.IDAL.TourStructure
{
    #region 确认件登记短信提醒组团社数据接口

    /// <summary>
    /// 确认件登记短信提醒组团社数据接口
    /// </summary>
    public interface ISMSTourCustomer
    {
        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="model">确认件登记短信提醒组团社实体</param>
        /// <returns>1成功；其他失败</returns>
        int Add(Model.TourStructure.MSMSTourCustomer model);

        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="model">确认件登记短信提醒组团社实体</param>
        /// <returns>1成功；其他失败</returns>
        int Update(Model.TourStructure.MSMSTourCustomer model);

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="tourId">团队编号</param>
        /// <param name="smsId">短信提醒编号</param>
        /// <returns>1成功；其他失败</returns>
        int Delete(string tourId, params int[] smsId);

        /// <summary>
        /// 获取确认件登记短信提醒组团社实体
        /// </summary>
        /// <param name="smsId">短信提醒编号</param>
        /// <returns></returns>
        Model.TourStructure.MSMSTourCustomer GetModel(int smsId);

        /// <summary>
        /// 获取确认件登记短信提醒组团社列表
        /// </summary>
        /// <param name="companyId">公司编号</param>
        /// <param name="pageSize">每页记录数</param>
        /// <param name="pageIndex">当前页数</param>
        /// <param name="recordCount">总记录数</param>
        /// <param name="search">查询实体</param>
        /// <returns></returns>
        IList<Model.TourStructure.MSMSTourCustomer> GetList(
            int companyId,
            int pageSize,
            int pageIndex,
            ref int recordCount,
            Model.TourStructure.MSearchSMSTourCustomer search);
    }

    #endregion

    public interface ITour
    {

        int Add(EyouSoft.Model.TourStructure.MTourInfo model);

        int Update(EyouSoft.Model.TourStructure.MTourInfo model);

        int Update(string TourId, bool IsEnd);

        int Update_Fin(EyouSoft.Model.TourStructure.MTourInfo model);

        int Update(string TourId, EyouSoft.Model.EnumType.TourStructure.TourStatus TourStatus);

        int Delete(string TourId);


        int Delete_(string TourId);

        EyouSoft.Model.TourStructure.MTourInfo GetModel(string TourId);

        IList<EyouSoft.Model.TourStructure.MPageTour> GetList(
            int companyId,
            int pageSize,
            int pageIndex,
            ref int recordCount,
            EyouSoft.Model.TourStructure.MSearchTour search);

        void GetTodayTour(int companyId, ref int SanTour, ref int Tour);


        IList<EyouSoft.Model.TourStructure.MTour> GetList(int companyId, string TourCode, params int[] TourStatus);


    }
}
