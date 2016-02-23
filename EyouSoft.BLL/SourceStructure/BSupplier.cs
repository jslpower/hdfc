using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using EyouSoft.BLL.CompanyStructure;

namespace EyouSoft.BLL.SourceStructure
{
    public class BSupplier : BLLBase
    {
        private readonly EyouSoft.IDAL.SourceStructure.ISupplier dal = EyouSoft.Component.Factory.ComponentFactory.CreateDAL<EyouSoft.IDAL.SourceStructure.ISupplier>();



        /// <summary>
        /// 添加地接、机票的 供应商
        /// </summary>
        /// <param name="model"></param>
        /// <returns>
        /// 1:成功 0：失败
        /// -1：超过最大分配账号数
        /// </returns>
        public int Add(EyouSoft.Model.SourceStructure.MSupplier model)
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
                var log = new Model.CompanyStructure.SysHandleLogs();
                if (model.SupplierType == EyouSoft.Model.EnumType.CompanyStructure.SupplierType.地接)
                {

                    log.EventTitle = "新增地接";
                    log.ModuleId = Model.EnumType.PrivsStructure.Privs2.供应商管理_地接;
                    log.EventMessage = "新增地接，地接编号：" + model.Id + "。";
                }

                if (model.SupplierType == EyouSoft.Model.EnumType.CompanyStructure.SupplierType.票务)
                {
                    log.EventTitle = "新增票务";
                    log.ModuleId = Model.EnumType.PrivsStructure.Privs2.供应商管理_地接;
                    log.EventMessage = "新增票务，票务编号：" + model.Id + "。";
                }

                new SysHandleLogs().Add(log);
            }
            return flg;

        }


        /// <summary>
        /// 添加地接、机票的 供应商
        /// </summary>
        /// <param name="model"></param>
        /// <returns>
        /// 1:成功 0：失败
        /// -1：超过最大分配账号数
        /// </returns>
        public int Add(EyouSoft.Model.SourceStructure.MSupplier model, ref IList<string> list)
        {
            if (model.CompanyId == 0
               || string.IsNullOrEmpty(model.UnitName)
               || model.OperatorId == 0
               || model.ProvinceId == 0
               || model.CityId == 0) return 0;

            if (model.ContactList == null) return 0;
            if (model.ContactList.Count == 0) return 0;

            model.Id = Guid.NewGuid().ToString();

            int flg = dal.Add(model, ref list);

            if (flg == 1)
            {
                var log = new Model.CompanyStructure.SysHandleLogs();
                if (model.SupplierType == EyouSoft.Model.EnumType.CompanyStructure.SupplierType.地接)
                {

                    log.EventTitle = "新增地接";
                    log.ModuleId = Model.EnumType.PrivsStructure.Privs2.供应商管理_地接;
                    log.EventMessage = "新增地接，地接编号：" + model.Id + "。";
                }

                if (model.SupplierType == EyouSoft.Model.EnumType.CompanyStructure.SupplierType.票务)
                {
                    log.EventTitle = "新增票务";
                    log.ModuleId = Model.EnumType.PrivsStructure.Privs2.供应商管理_地接;
                    log.EventMessage = "新增票务，票务编号：" + model.Id + "。";
                }

                new SysHandleLogs().Add(log);
            }
            return flg;

        }

        /// <summary>
        /// 修改地接、机票的 供应商
        /// </summary>
        /// <param name="model"></param>
        /// <returns>
        /// 1:成功 0：失败
        /// -1：超过最大分配账号数
        /// </returns>
        public int Update(EyouSoft.Model.SourceStructure.MSupplier model)
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
                var log = new Model.CompanyStructure.SysHandleLogs();
                if (model.SupplierType == EyouSoft.Model.EnumType.CompanyStructure.SupplierType.地接)
                {

                    log.EventTitle = "修改地接";
                    log.ModuleId = Model.EnumType.PrivsStructure.Privs2.供应商管理_地接;
                    log.EventMessage = "修改地接，地接编号：" + model.Id + "。";
                }

                if (model.SupplierType == EyouSoft.Model.EnumType.CompanyStructure.SupplierType.票务)
                {
                    log.EventTitle = "修改票务";
                    log.ModuleId = Model.EnumType.PrivsStructure.Privs2.供应商管理_地接;
                    log.EventMessage = "修改票务，票务编号：" + model.Id + "。";
                }

                new SysHandleLogs().Add(log);
            }
            return flg;


        }

        /// <summary>
        /// 修改地接、机票的 供应商
        /// </summary>
        /// <param name="model"></param>
        /// <returns>
        /// 1:成功 0：失败
        /// -1：超过最大分配账号数
        /// </returns>
        public int Update(EyouSoft.Model.SourceStructure.MSupplier model, ref IList<string> list)
        {
            if (string.IsNullOrEmpty(model.Id)
                || model.CompanyId == 0
                || string.IsNullOrEmpty(model.UnitName)
                || model.ProvinceId == 0
                || model.CityId == 0) return 0;
            if (model.ContactList == null) return 0;
            if (model.ContactList.Count == 0) return 0;

            int flg = dal.Update(model, ref list);
            if (flg == 1)
            {
                var log = new Model.CompanyStructure.SysHandleLogs();
                if (model.SupplierType == EyouSoft.Model.EnumType.CompanyStructure.SupplierType.地接)
                {

                    log.EventTitle = "修改地接";
                    log.ModuleId = Model.EnumType.PrivsStructure.Privs2.供应商管理_地接;
                    log.EventMessage = "修改地接，地接编号：" + model.Id + "。";
                }

                if (model.SupplierType == EyouSoft.Model.EnumType.CompanyStructure.SupplierType.票务)
                {
                    log.EventTitle = "修改票务";
                    log.ModuleId = Model.EnumType.PrivsStructure.Privs2.供应商管理_地接;
                    log.EventMessage = "修改票务，票务编号：" + model.Id + "。";
                }

                new SysHandleLogs().Add(log);
            }
            return flg;


        }

        /// <summary>
        /// 删除地接、机票的 供应商
        /// </summary>
        /// <param name="Id"></param>
        /// <returns>
        /// 0:失败 1:成功 
        /// -1:地接供应商做过安排不允许删除
        /// -2:机票供应商做过安排不允许删除
        /// </returns>
        public int Delete(string Id)
        {
            if (string.IsNullOrEmpty(Id)) return 0;
            int flg = dal.Delete(Id);
            if (flg == 1)
            {
                var log = new Model.CompanyStructure.SysHandleLogs
                {
                    EventTitle = "删除地接或票务",
                    ModuleId = Model.EnumType.PrivsStructure.Privs2.计调中心_确认件登记,
                    EventMessage = "删除地接或票务，编号：" + Id + "。"
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
        public EyouSoft.Model.SourceStructure.MSupplier GetModel(string Id)
        {
            if (string.IsNullOrEmpty(Id)) return null;
            return dal.GetModel(Id);
        }

        /// <summary>
        /// 获取分页地接信息
        /// </summary>
        /// <param name="companyId"></param>
        /// <param name="pageSize"></param>
        /// <param name="pageIndex"></param>
        /// <param name="recordCount"></param>
        /// <param name="search"></param>
        /// <returns></returns>
        public IList<EyouSoft.Model.SourceStructure.MPageSupplier> GetGroundList(
         int companyId,
         int pageSize,
         int pageIndex,
         ref int recordCount,
         EyouSoft.Model.SourceStructure.MSupplierSearch search)
        {
            if (companyId == 0) return null;
            return dal.GetList(EyouSoft.Model.EnumType.CompanyStructure.SupplierType.地接, companyId, pageSize, pageIndex, ref recordCount, search);
        }

        /// <summary>
        /// 分页获取票务信息
        /// </summary>
        /// <param name="companyId"></param>
        /// <param name="pageSize"></param>
        /// <param name="pageIndex"></param>
        /// <param name="recordCount"></param>
        /// <param name="search"></param>
        /// <returns></returns>
        public IList<EyouSoft.Model.SourceStructure.MPageSupplier> GetTicketList(
        int companyId,
        int pageSize,
        int pageIndex,
        ref int recordCount,
        EyouSoft.Model.SourceStructure.MSupplierSearch search)
        {
            if (companyId == 0) return null;
            return dal.GetList(EyouSoft.Model.EnumType.CompanyStructure.SupplierType.票务, companyId, pageSize, pageIndex, ref recordCount, search);

        }


        /// <summary>
        /// 获取地接选用信息 
        /// </summary>
        /// <param name="companyId"></param>
        /// <param name="unitName"></param>
        /// <returns></returns>
        public IList<EyouSoft.Model.SourceStructure.MSupplier> GetGroundList(int companyId, string unitName)
        {
            if (companyId == 0) return null;
            return dal.GetList(EyouSoft.Model.EnumType.CompanyStructure.SupplierType.地接, companyId, unitName);

        }

        /// <summary>
        /// 获取信息  用于选择补全控件
        /// </summary>
        /// <param name="CompanyId"></param>
        /// <param name="UnitName"></param>
        /// <param name="SupplierType"></param>
        /// <returns></returns>
        public IList<EyouSoft.Model.SourceStructure.MSupplier> GetList(int CompanyId, string UnitName, EyouSoft.Model.EnumType.CompanyStructure.SupplierType SupplierType)
        {
            if (CompanyId == 0) return null;
            return dal.GetList(CompanyId, UnitName, SupplierType);
        }


        /// <summary>
        /// 根据公司编号、供应商编号、供应商联系人的用户编号修改联系人信息
        /// </summary>
        /// <param name="companyId">公司编号</param>
        /// <param name="supplierId">供应商编号</param>
        /// <param name="contactUserId">供应商联系人的用户编号</param>
        /// <param name="model">供应商联系人实体</param>
        /// <returns>1：成功；其他失败</returns>
        public int UpdateSupplierContact(
            int companyId, string supplierId, int contactUserId, Model.SourceStructure.MSupplierContact model)
        {
            if (companyId <= 0 || string.IsNullOrEmpty(supplierId) || contactUserId <= 0 || model == null)
            {
                return 0;
            }

            return dal.UpdateSupplierContact(companyId, supplierId, contactUserId, model);
        }
    }
}
