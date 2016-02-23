using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EyouSoft.IDAL.SourceStructure
{
    public interface IRestaurantSupplier
    {
        int Add(EyouSoft.Model.SourceStructure.MRestaurantSupplier model);

        int Update(EyouSoft.Model.SourceStructure.MRestaurantSupplier model);

        int Delete(string Id);

        EyouSoft.Model.SourceStructure.MRestaurantSupplier GetModel(string Id);

        IList<EyouSoft.Model.SourceStructure.MPageRestaurant> GetList(
        int companyId,
        int pageSize,
        int pageIndex,
        ref int recordCount,
        EyouSoft.Model.SourceStructure.MSearchRestaurant search);

    }
}
