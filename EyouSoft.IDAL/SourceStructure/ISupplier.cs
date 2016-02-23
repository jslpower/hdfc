using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EyouSoft.IDAL.SourceStructure
{
    public interface ISupplier
    {
        int Add(EyouSoft.Model.SourceStructure.MSupplier model);
        int Add(EyouSoft.Model.SourceStructure.MSupplier model, ref IList<string> list);

        int Update(EyouSoft.Model.SourceStructure.MSupplier model);
        int Update(EyouSoft.Model.SourceStructure.MSupplier model, ref IList<string> list);

        int Delete(string Id);

        EyouSoft.Model.SourceStructure.MSupplier GetModel(string Id);

        IList<EyouSoft.Model.SourceStructure.MSupplier> GetList(
        EyouSoft.Model.EnumType.CompanyStructure.SupplierType SupplierType,
        int companyId,
        string unitName);

        IList<EyouSoft.Model.SourceStructure.MPageSupplier> GetList(
        EyouSoft.Model.EnumType.CompanyStructure.SupplierType SupplierType,
        int companyId,
        int pageSize,
        int pageIndex,
        ref int recordCount,
        EyouSoft.Model.SourceStructure.MSupplierSearch search);


        IList<EyouSoft.Model.SourceStructure.MSupplier> GetList(int CompanyId, string UnitName, EyouSoft.Model.EnumType.CompanyStructure.SupplierType SupplierType);

        /// <summary>
        /// 根据公司编号、供应商编号、供应商联系人的用户编号修改联系人信息
        /// </summary>
        /// <param name="companyId">公司编号</param>
        /// <param name="supplierId">供应商编号</param>
        /// <param name="contactUserId">供应商联系人的用户编号</param>
        /// <param name="model">供应商联系人实体</param>
        /// <returns>1：成功；其他失败</returns>
        int UpdateSupplierContact(
            int companyId, string supplierId, int contactUserId, Model.SourceStructure.MSupplierContact model);
    }
}
