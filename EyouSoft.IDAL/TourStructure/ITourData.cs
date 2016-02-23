using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EyouSoft.IDAL.TourStructure
{
    public interface ITourData
    {
        int Add(EyouSoft.Model.TourStructure.MTourData model);

        int Update(EyouSoft.Model.TourStructure.MTourData model);

        int Delete(int Id);

        EyouSoft.Model.TourStructure.MTourData GetModel(int id);

        IList<EyouSoft.Model.TourStructure.MTourData> GetList(int companyId,
        int pageSize,
        int pageIndex,
        ref int recordCount,
        EyouSoft.Model.TourStructure.MSearchTourData search);


        int Check(int Id, int OperatorId);
    }
}
