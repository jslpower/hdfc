using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EyouSoft.IDAL.SourceStructure
{
    public interface ISpotSupplier
    {
        int Add(EyouSoft.Model.SourceStructure.MSpotSupplier model);

        int Update(EyouSoft.Model.SourceStructure.MSpotSupplier model);

        int Delete(string Id);

        EyouSoft.Model.SourceStructure.MSpotSupplier GetModel(string Id);

        IList<EyouSoft.Model.SourceStructure.MPageSpot> GetList(
        int companyId,
        int pageSize,
        int pageIndex,
        ref int recordCount,
        EyouSoft.Model.SourceStructure.MSearchSpot search);
    }
}
