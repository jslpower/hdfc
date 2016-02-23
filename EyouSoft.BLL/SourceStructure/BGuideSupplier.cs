using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using EyouSoft.BLL.CompanyStructure;

namespace EyouSoft.BLL.SourceStructure
{
    public class BGuideSupplier : BLLBase
    {
        private readonly EyouSoft.IDAL.SourceStructure.IGuideSupplier dal = EyouSoft.Component.Factory.ComponentFactory.CreateDAL<EyouSoft.IDAL.SourceStructure.IGuideSupplier>();


        /// <summary>
        /// 添加导游
        /// </summary>
        /// <param name="model"></param>
        /// <returns>0:添加失败 1:添加成功</returns>
        public int Add(EyouSoft.Model.SourceStructure.MGuideSupplier model)
        {
            if (model.CompanyId == 0
               || string.IsNullOrEmpty(model.GuideName)
                  || model.OperatorId == 0
                // || model.ProvinceId == 0
                //|| model.CityId == 0
               || string.IsNullOrEmpty(model.GysName)
                ) return 0;


            model.Id = Guid.NewGuid().ToString();

            int flg = dal.Add(model);
            if (flg == 1)
            {
                var log = new Model.CompanyStructure.SysHandleLogs
                {
                    EventTitle = "添加导游",
                    ModuleId = Model.EnumType.PrivsStructure.Privs2.供应商管理_导游,
                    EventMessage = "添加导游，编号：" + model.Id + "。"
                };
                new SysHandleLogs().Add(log);
            }

            return flg;
        }

        /// <summary>
        /// 修改导游
        /// </summary>
        /// <param name="model"></param>
        /// <returns>0:失败 1:成功</returns>
        public int Update(EyouSoft.Model.SourceStructure.MGuideSupplier model)
        {
            if (string.IsNullOrEmpty(model.Id)
                 || model.CompanyId == 0
                 || string.IsNullOrEmpty(model.GuideName)
                // || model.ProvinceId == 0
                // || model.CityId == 0
                || string.IsNullOrEmpty(model.GysName)
                ) return 0;

            int flg = dal.Update(model);
            if (flg == 1)
            {
                var log = new Model.CompanyStructure.SysHandleLogs
                {
                    EventTitle = "修改导游",
                    ModuleId = Model.EnumType.PrivsStructure.Privs2.供应商管理_导游,
                    EventMessage = "修改导游，编号：" + model.Id + "。"
                };
                new SysHandleLogs().Add(log);
            }

            return flg;
        }

        /// <summary>
        /// 删除导游
        /// </summary>
        /// <param name="Id"></param>
        /// <returns>0:失败 1:成功 -1:确认件安排过该导游 不允许删除</returns>
        public int Delete(string Id)
        {
            if (string.IsNullOrEmpty(Id)) return 0;
            int flg = dal.Delete(Id);
            if (flg == 1)
            {
                var log = new Model.CompanyStructure.SysHandleLogs
                {
                    EventTitle = "删除导游",
                    ModuleId = Model.EnumType.PrivsStructure.Privs2.供应商管理_导游,
                    EventMessage = "删除导游，编号：" + Id + "。"
                };
                new SysHandleLogs().Add(log);
            }

            return flg;
        }


        /// <summary>
        /// 获取实体
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        public EyouSoft.Model.SourceStructure.MGuideSupplier GetModel(string Id)
        {
            if (string.IsNullOrEmpty(Id)) return null;
            return dal.GetModel(Id);
        }

        /// <summary>
        /// 获取分页列表
        /// </summary>
        /// <param name="companyId"></param>
        /// <param name="pageSize"></param>
        /// <param name="pageIndex"></param>
        /// <param name="recordCount"></param>
        /// <param name="GuideName"></param>
        /// <returns></returns>
        public IList<EyouSoft.Model.SourceStructure.MPageGuide> GetList(int companyId,
         int pageSize,
         int pageIndex,
         ref int recordCount,
         string GuideName,
         string GysName)
        {
            if (companyId == 0) return null;
            return dal.GetList(companyId, pageSize, pageIndex, ref recordCount, GuideName, GysName);
        }

        /// <summary>
        /// 添加导游反馈信息
        /// </summary>
        /// <param name="model"></param>
        /// <returns>1：成功 0：失败</returns>
        public int Add(EyouSoft.Model.SourceStructure.MGuideFanKui model)
        {
            if (string.IsNullOrEmpty(model.GuideId)
                || !model.FanKuiTime.HasValue) return 0;

            int flg = dal.Add(model);

            if (flg == 1)
            {
                var log = new Model.CompanyStructure.SysHandleLogs
                {
                    EventTitle = "添加导游反馈信息",
                    ModuleId = Model.EnumType.PrivsStructure.Privs2.供应商管理_导游,
                    EventMessage = "添加导游反馈信息，导游编号：" + model.GuideId + "。"
                };
                new SysHandleLogs().Add(log);
            }
            return flg;

        }

        /// <summary>
        /// 修改导游反馈信息
        /// </summary>
        /// <param name="model"></param>
        /// <returns>1：成功 0：失败</returns>
        public int Update(EyouSoft.Model.SourceStructure.MGuideFanKui model)
        {
            if (string.IsNullOrEmpty(model.GuideId)
                || !model.FanKuiTime.HasValue) return 0;

            int flg = dal.Update(model);

            if (flg == 1)
            {
                var log = new Model.CompanyStructure.SysHandleLogs
                {
                    EventTitle = "修改导游反馈信息",
                    ModuleId = Model.EnumType.PrivsStructure.Privs2.供应商管理_导游,
                    EventMessage = "修改导游反馈信息，导游编号：" + model.GuideId + "。"
                };
                new SysHandleLogs().Add(log);
            }
            return flg;
        }

        /// <summary>
        /// 删除导游反馈
        /// </summary>
        /// <param name="Id"></param>
        /// <returns>1：成功 0：失败</returns>
        public int Delete(int Id)
        {
            if (Id == 0) return 0;
            int flg = dal.Delete(Id);

            if (flg == 1)
            {
                var log = new Model.CompanyStructure.SysHandleLogs
                {
                    EventTitle = "删除导游反馈",
                    ModuleId = Model.EnumType.PrivsStructure.Privs2.供应商管理_导游,
                    EventMessage = "删除导游反馈，编号：" + Id + "。"
                };
                new SysHandleLogs().Add(log);
            }
            return flg;
        }

        /// <summary>
        /// 获取反馈的集合
        /// </summary>
        /// <param name="GuidId"></param>
        /// <returns></returns>
        public IList<EyouSoft.Model.SourceStructure.MGuideFanKui> GetList(string GuideId)
        {
            if (string.IsNullOrEmpty(GuideId)) return null;
            return dal.GetList(GuideId);
        }


        public IList<EyouSoft.Model.SourceStructure.MGuideFanKui> GetList(string GuideId,
         int pageSize,
         int pageIndex,
         ref int recordCount)
        {
            if (string.IsNullOrEmpty(GuideId)) return null;
            return dal.GetList(GuideId, pageSize, pageIndex, ref recordCount);
        }


        /// <summary>
        /// 根据公司编号 导游名称获取导游集合
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        public IList<EyouSoft.Model.SourceStructure.MGuideSupplier> GetList(int companyId, string GuideName)
        {

            if (companyId == 0) return null;
            return dal.GetList(companyId, GuideName);
        }


    }
}
