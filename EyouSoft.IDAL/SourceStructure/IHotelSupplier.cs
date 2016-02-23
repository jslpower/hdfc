using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EyouSoft.IDAL.SourceStructure
{
    public interface IHotelSupplier
    {

        int Add(EyouSoft.Model.SourceStructure.MHotelSupplier model);

        int Update(EyouSoft.Model.SourceStructure.MHotelSupplier model);

        int Delete(string Id);

        EyouSoft.Model.SourceStructure.MHotelSupplier GetModel(string Id);

        IList<EyouSoft.Model.SourceStructure.MPageHotel> GetList(
             int companyId,
            int pageSize,
            int pageIndex,
            ref int recordCount,
            EyouSoft.Model.SourceStructure.MSearchHotel search
            );
    }
}
