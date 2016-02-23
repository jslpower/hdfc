using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using EyouSoft.BLL.CompanyStructure;

namespace EyouSoft.BLL.SourceStructure
{
    public class BRestaurantSupplier : BLLBase
    {
        private readonly EyouSoft.IDAL.SourceStructure.IRestaurantSupplier dal = EyouSoft.Component.Factory.ComponentFactory.CreateDAL<EyouSoft.IDAL.SourceStructure.IRestaurantSupplier>();

        /// <summary>
        /// 供应商餐饮
        /// </summary>
        /// <param name="model"></param>
        /// <returns>0:失败 1:成功</returns>
        public int Add(EyouSoft.Model.SourceStructure.MRestaurantSupplier model)
        {
            if (model.CompanyId == 0
                || string.IsNullOrEmpty(model.UnitName)
                || model.OperatorId == 0
                || model.ProvinceId == 0
                || model.CityId == 0) return 0;

            if (model.ContactList == null) return 0;
            if (model.ContactList.Count == 0) return 0;

            model.Id = Guid.NewGuid().ToString();

            int flg = dal.Add(model);
            if (flg == 1)
            {
                var log = new Model.CompanyStructure.SysHandleLogs
                {
                    EventTitle = "添加供应商餐饮",
                    ModuleId = Model.EnumType.PrivsStructure.Privs2.供应商管理_餐馆,
                    EventMessage = "添加供应商餐饮，编号：" + model.Id + "。"
                };
                new SysHandleLogs().Add(log);
            }

            return flg;
        }

        /// <summary>
        /// 修改供应商餐饮
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public int Update(EyouSoft.Model.SourceStructure.MRestaurantSupplier model)
        {
            if (string.IsNullOrEmpty(model.Id)
                || model.CompanyId == 0
                || string.IsNullOrEmpty(model.UnitName)
                || model.ProvinceId == 0
                || model.CityId == 0) return 0;

            if (model.ContactList == null) return 0;
            if (model.ContactList.Count == 0) return 0;

            int flg = dal.Update(model);
            if (flg == 1)
            {
                var log = new Model.CompanyStructure.SysHandleLogs
                {
                    EventTitle = "修改供应商餐饮",
                    ModuleId = Model.EnumType.PrivsStructure.Privs2.供应商管理_餐馆,
                    EventMessage = "修改供应商餐饮，编号：" + model.Id + "。"
                };
                new SysHandleLogs().Add(log);
            }

            return flg;
        }

        /// <summary>
        /// 删除供应商餐饮
        /// </summary>
        /// <param name="Id"></param>
        /// <returns>0:失败 1:成功</returns>
        public int Delete(string Id)
        {
            if (string.IsNullOrEmpty(Id)) return 0;
            int flg = dal.Delete(Id);
            if (flg == 1)
            {
                var log = new Model.CompanyStructure.SysHandleLogs
                {
                    EventTitle = "删除供应商餐饮",
                    ModuleId = Model.EnumType.PrivsStructure.Privs2.供应商管理_餐馆,
                    EventMessage = "删除供应商餐饮，编号：" + Id + "。"
                };

                new SysHandleLogs().Add(log);
            }
            return flg;
        }

        /// <summary>
        /// 获取model
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        public EyouSoft.Model.SourceStructure.MRestaurantSupplier GetModel(string Id)
        {
            if (string.IsNullOrEmpty(Id)) return null;
            return dal.GetModel(Id);
        }

        /// <summary>
        /// 获取分页信息
        /// </summary>
        /// <param name="companyId"></param>
        /// <param name="pageSize"></param>
        /// <param name="pageIndex"></param>
        /// <param name="recordCount"></param>
        /// <param name="search"></param>
        /// <returns></returns>
        public IList<EyouSoft.Model.SourceStructure.MPageRestaurant> GetList(int companyId, int pageSize, int pageIndex, ref int recordCount, EyouSoft.Model.SourceStructure.MSearchRestaurant search)
        {
            if (companyId == 0) return null;
            return dal.GetList(companyId, pageSize, pageIndex, ref recordCount, search);

        }

    }
}
