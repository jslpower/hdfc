using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EyouSoft.IDAL.CompanyStructure
{
    public interface ICompanyTicket
    {
        /// <summary>
        /// 增加一条数据
        /// </summary>
        int Add(EyouSoft.Model.CompanyStructure.MCompanyTicket model);
        /// <summary>
        /// 更新一条数据
        /// </summary>
        bool Update(EyouSoft.Model.CompanyStructure.MCompanyTicket model);
        /// <summary>
        /// 删除数据
        /// </summary>
        bool Delete(int Id);
        /// <summary>
        /// 得到一个对象实体
        /// </summary>
        EyouSoft.Model.CompanyStructure.MCompanyTicket GetModel(int Id);
        /// <summary>
        /// 获得数据列表
        /// </summary>
        IList<EyouSoft.Model.CompanyStructure.MCompanyTicket> GetList(int top, int CompanyId, EyouSoft.Model.CompanyStructure.MCompanyTicketSearch search);
        /// <summary>
        /// 查询分页
        /// </summary>
        /// <param name="search"></param>
        /// <returns></returns>
        IList<EyouSoft.Model.CompanyStructure.MCompanyTicket> GetList(int CompanyId, int PageSize, int PageIndex, ref int RecordCount, EyouSoft.Model.CompanyStructure.MCompanyTicketSearch search);


    }
}
