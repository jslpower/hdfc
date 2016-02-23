using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EyouSoft.BLL.CompanyStructure
{
    public class BCompanyTicket
    {
        private readonly EyouSoft.IDAL.CompanyStructure.ICompanyTicket dal = EyouSoft.Component.Factory.ComponentFactory.CreateDAL<EyouSoft.IDAL.CompanyStructure.ICompanyTicket>();

        #region ICompanyTicket 成员

        /// <summary>
        /// 获取model
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        public EyouSoft.Model.CompanyStructure.MCompanyTicket GetModel(int Id)
        {
            if (Id <= 0) return null;

            return dal.GetModel(Id);
        }

        /// <summary>
        /// 添加
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public bool Add(EyouSoft.Model.CompanyStructure.MCompanyTicket model)
        {

            if (model.CompanyId == 0
                || !model.TicketType.HasValue
                || string.IsNullOrEmpty(model.TrafficNumber)
                || model.OperatorId == 0)
            {
                return false;
            }

            int flg = dal.Add(model);
            if (flg != 0)
            {
                var log = new Model.CompanyStructure.SysHandleLogs
                {

                    EventTitle = "新增公司票务的基础信息登记",
                    ModuleId = Model.EnumType.PrivsStructure.Privs2.计调中心_确认件登记,
                    EventMessage = "新增公司票务的基础信息登记，编号：" + flg + "。"
                };

                new SysHandleLogs().Add(log);

                return true;
            }

            return false;

        }

        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public bool Update(EyouSoft.Model.CompanyStructure.MCompanyTicket model)
        {
            if (model.Id == 0
               || model.CompanyId == 0
               || !model.TicketType.HasValue
               || string.IsNullOrEmpty(model.TrafficNumber)
               || model.OperatorId == 0)
            {
                return false;
            }
            if (dal.Update(model))
            {

                var log = new Model.CompanyStructure.SysHandleLogs
                {

                    EventTitle = "修改公司票务的基础信息登记",
                    ModuleId = Model.EnumType.PrivsStructure.Privs2.计调中心_确认件登记,
                    EventMessage = "修改公司票务的基础信息登记，编号：" + model.Id + "。"
                };

                new SysHandleLogs().Add(log);

                return true;
            }

            return false;

        }

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        public bool Delete(int Id)
        {
            if (Id == 0) return false;
            if (dal.Delete(Id))
            {
                var log = new Model.CompanyStructure.SysHandleLogs
                {
                    EventTitle = "删除公司票务的基础信息登记",
                    ModuleId = Model.EnumType.PrivsStructure.Privs2.计调中心_确认件登记,
                    EventMessage = "删除公司票务的基础信息登记，编号：" + Id + "。"
                };

                new SysHandleLogs().Add(log);

                return true;
            }
            return false;
        }

        /// <summary>
        /// 查询
        /// </summary>
        /// <param name="search"></param>
        /// <returns></returns>
        public IList<EyouSoft.Model.CompanyStructure.MCompanyTicket> GetList(int top, int CompanyId, EyouSoft.Model.CompanyStructure.MCompanyTicketSearch search)
        {
            if (CompanyId == 0) return null;

            return dal.GetList(top, CompanyId, search);
        }


        /// <summary>
        /// 查询分页
        /// </summary>
        /// <param name="search"></param>
        /// <returns></returns>
        public IList<EyouSoft.Model.CompanyStructure.MCompanyTicket> GetList(int CompanyId, int PageSize, int PageIndex, ref int RecordCount, EyouSoft.Model.CompanyStructure.MCompanyTicketSearch search)
        {
            if (CompanyId == 0) return null;
            return dal.GetList(CompanyId, PageSize, PageIndex, ref RecordCount, search);
        }

        #endregion
    }
}
