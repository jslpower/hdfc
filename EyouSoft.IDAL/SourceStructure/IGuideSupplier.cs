using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EyouSoft.IDAL.SourceStructure
{
    public interface IGuideSupplier
    {
        int Add(EyouSoft.Model.SourceStructure.MGuideSupplier model);

        int Update(EyouSoft.Model.SourceStructure.MGuideSupplier model);

        int Delete(string Id);

        EyouSoft.Model.SourceStructure.MGuideSupplier GetModel(string Id);


        IList<EyouSoft.Model.SourceStructure.MPageGuide> GetList(int companyId,
        int pageSize,
        int pageIndex,
        ref int recordCount,
        string GuideName,
        string GysName);


        int Add(EyouSoft.Model.SourceStructure.MGuideFanKui model);

        int Update(EyouSoft.Model.SourceStructure.MGuideFanKui model);

        int Delete(int Id);

        IList<EyouSoft.Model.SourceStructure.MGuideSupplier> GetList(int companyId, string GuideName);

        IList<EyouSoft.Model.SourceStructure.MGuideFanKui> GetList(string GuideId);


        IList<EyouSoft.Model.SourceStructure.MGuideFanKui> GetList(string GuideId,
        int pageSize,
        int pageIndex,
        ref int recordCount);


    }
}
