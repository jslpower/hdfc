using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EyouSoft.IDAL.TourStructure
{
    public interface ITourReturnVisit
    {

        int Add(EyouSoft.Model.TourStructure.MTourReturnVisit model);

        int Update(EyouSoft.Model.TourStructure.MTourReturnVisit model);

        int Delete(string Id);

        EyouSoft.Model.TourStructure.MTourReturnVisit GetModel(string Id);

        IList<EyouSoft.Model.TourStructure.MTourReturnVisit> GetList(string TourId);

        bool SetTourScore(string tourId, EyouSoft.Model.EnumType.TourStructure.Score score, int OperatorId);


        IList<EyouSoft.Model.TourStructure.MPageTourReturnVisit> GetList(
                      int flg,
                      int companyId,
                      int pageSize,
                      int pageIndex,
                      ref int recordCount,
                      EyouSoft.Model.TourStructure.MSeachVist search);
    }
}
