using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EyouSoft.IDAL.PlanStructure
{
    public interface IPlanTicket
    {

        int Add(EyouSoft.Model.PlanStructure.MPlanTicketInfo model);

        int Update(EyouSoft.Model.PlanStructure.MPlanTicketInfo model);

        int Update(string PlanId, EyouSoft.Model.EnumType.PlanStructure.TicketStatus TicketStatus);

        int Delete(string Id);

        EyouSoft.Model.PlanStructure.MPlanTicketInfo GetModel(string Id);


        IList<EyouSoft.Model.PlanStructure.MPlanTicket> GetList(
            int companyId,
            int pageSize,
            int pageIndex,
            ref int recordCount,
            EyouSoft.Model.PlanStructure.MSearchTicket search, ref decimal[] Sum);
    }
}
