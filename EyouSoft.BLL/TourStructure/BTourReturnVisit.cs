using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using EyouSoft.BLL.CompanyStructure;

namespace EyouSoft.BLL.TourStructure
{
    public class BTourReturnVisit
    {
        private readonly EyouSoft.IDAL.TourStructure.ITourReturnVisit dal = EyouSoft.Component.Factory.ComponentFactory.CreateDAL<EyouSoft.IDAL.TourStructure.ITourReturnVisit>();





        /// <summary>
        /// 添加团队回访记录
        /// </summary>
        /// <param name="model"></param>
        /// <returns>0:添加失败 1:添加成功</returns>
        public int Add(EyouSoft.Model.TourStructure.MTourReturnVisit model)
        {
            model.VisitId = Guid.NewGuid().ToString();

            int flg = dal.Add(model);

            if (flg == 1)
            {
                var log = new Model.CompanyStructure.SysHandleLogs
                {
                    EventTitle = "新增团队回访记录",
                    ModuleId = Model.EnumType.PrivsStructure.Privs2.计调中心_回访提醒,
                    EventMessage = "新增团队回访记录，编号：" + model.VisitId + "。"
                };

                new SysHandleLogs().Add(log);
            }

            return flg;
        }

        /// <summary>
        /// 修改团队回访记录
        /// </summary>
        /// <param name="model"></param>
        /// <returns>-1:团队质量评估已反馈不允许修改 0:失败 1:成功</returns>
        public int Update(EyouSoft.Model.TourStructure.MTourReturnVisit model)
        {
            if (string.IsNullOrEmpty(model.TourId)) return 0;

            int flg = dal.Update(model);
            if (flg == 1)
            {
                var log = new Model.CompanyStructure.SysHandleLogs
                {
                    EventTitle = "修改团队回访记录",
                    ModuleId = Model.EnumType.PrivsStructure.Privs2.计调中心_回访提醒,
                    EventMessage = "修改团队回访记录，编号：" + model.VisitId + "。"
                };

                new SysHandleLogs().Add(log);
            }
            return flg;
        }

        /// <summary>
        /// 根据编号删除回访记录
        /// </summary>
        /// <param name="Id"></param>
        /// <returns>-1:团队质量评估已反馈不允许删除 0:失败 1:成功</returns>
        public int Delete(string Id)
        {
            if (string.IsNullOrEmpty(Id)) return 0;
            int flg = dal.Delete(Id);
            if (flg == 1)
            {
                var log = new Model.CompanyStructure.SysHandleLogs
                {
                    EventTitle = "删除团队回访记录",
                    ModuleId = Model.EnumType.PrivsStructure.Privs2.计调中心_回访提醒,
                    EventMessage = "删除团队回访记录，编号：" + Id + "。"
                };

                new SysHandleLogs().Add(log);
            }
            return flg;

        }

        /// <summary>
        /// 根据Id获取model
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        public EyouSoft.Model.TourStructure.MTourReturnVisit GetModel(string Id)
        {

            if (string.IsNullOrEmpty(Id)) return null;
            return dal.GetModel(Id);
        }


        /// <summary>
        /// 获取回访记录的列表
        /// </summary>
        /// <param name="TourId"></param>
        /// <returns></returns>
        public IList<EyouSoft.Model.TourStructure.MTourReturnVisit> GetList(string TourId)
        {
            if (string.IsNullOrEmpty(TourId)) return null;
            return dal.GetList(TourId);
        }

        /// <summary>
        /// 设置团队质量评估
        /// </summary>
        /// <param name="TourId"></param>
        /// <returns></returns>
        public bool SetTourScore(string tourId, EyouSoft.Model.EnumType.TourStructure.Score? score, int OperatorId)
        {
            if (string.IsNullOrEmpty(tourId) || !score.HasValue || OperatorId == 0) return false;
            if (dal.SetTourScore(tourId, score.Value, OperatorId))
            {
                var log = new Model.CompanyStructure.SysHandleLogs
                        {
                            EventTitle = "设置团队质量评估",
                            ModuleId = Model.EnumType.PrivsStructure.Privs2.计调中心_团队质量反馈,
                            EventMessage = "设置团队质量评估，编号：" + tourId + "。"
                        };

                new SysHandleLogs().Add(log);

                return true;
            }
            return false;
        }


        /// <summary>
        /// 获取回访提醒，团队质量反馈列表
        /// </summary>
        /// <param name="companyId">公司编号</param>
        /// <param name="pageSize">每页显示的条数</param>
        /// <param name="pageIndex">当前页</param>
        /// <param name="recordCount">总记录数</param>
        /// <param name="search">查询实体</param>
        /// <returns></returns>
        public IList<EyouSoft.Model.TourStructure.MPageTourReturnVisit> GetVisitTiXingList(
              int companyId,
              int pageSize,
              int pageIndex,
              ref int recordCount,
              EyouSoft.Model.TourStructure.MSeachVist search)
        {

            return dal.GetList(0, companyId, pageSize, pageIndex, ref recordCount, search);

        }


        /// <summary>
        /// 团队质量反馈列表
        /// </summary>
        /// <param name="companyId">公司编号</param>
        /// <param name="pageSize">每页显示的条数</param>
        /// <param name="pageIndex">当前页</param>
        /// <param name="recordCount">总记录数</param>
        /// <param name="search">查询实体</param>
        /// <returns></returns>
        public IList<EyouSoft.Model.TourStructure.MPageTourReturnVisit> GetVisitHuiKuiList(
              int companyId,
              int pageSize,
              int pageIndex,
              ref int recordCount,
              EyouSoft.Model.TourStructure.MSeachVist search)
        {

            return dal.GetList(1, companyId, pageSize, pageIndex, ref recordCount, search);

        }











    }
}
