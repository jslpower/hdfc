using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EyouSoft.IDAL.PlanStructure
{
    public interface IPlanDiJie
    {

        int Add(EyouSoft.Model.PlanStructure.MPlanDiJieInfo model);

        int Update(EyouSoft.Model.PlanStructure.MPlanDiJieInfo model);

        int Update(string PlanId, EyouSoft.Model.EnumType.PlanStructure.DiJieStatus DiJieStatus);

        int Delete(string Id);

        EyouSoft.Model.PlanStructure.MPlanDiJieInfo GetModel(string Id);

        IList<EyouSoft.Model.PlanStructure.MPageDiJie> GetList(
               int companyId,
               int pageSize,
               int pageIndex,
               ref int recordCount,
               EyouSoft.Model.PlanStructure.MSeachDiJie search, ref decimal[] Sum);


    }
}
