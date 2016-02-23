using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EyouSoft.BLL.CompanyStructure
{ /// <summary>
    /// 客户关怀
    /// </summary>
    /// 创建人：李焕超 2011-01-26
    public class CustomerCareFor
    {
        private readonly EyouSoft.IDAL.CompanyStructure.ICustomerCareFor Dal = EyouSoft.Component.Factory.ComponentFactory.CreateDAL<EyouSoft.IDAL.CompanyStructure.ICustomerCareFor>();

        /// <summary>
        /// 添加
        /// </summary>
        /// <param name="Model"></param>
        /// <returns></returns>
        public bool Add(EyouSoft.Model.CompanyStructure.CustomerCareforInfo Model)
        { return Dal.Add(Model); }

        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="Model"></param>
        /// <returns></returns>
        public bool Update(EyouSoft.Model.CompanyStructure.CustomerCareforInfo Model)
        { return Dal.Update(Model); }

        /// <summary>
        /// 获取列表
        /// </summary>
        /// <param name="CompanyId"></param>
        /// <returns></returns>
        public IList<EyouSoft.Model.CompanyStructure.CustomerCareforInfo> GetList(string CompanyId, int PageSize, int PageIndex, ref int RecordCount)
        { return Dal.GetList(CompanyId, PageSize, PageIndex,ref RecordCount); }

        /// <summary>
        /// 停发
        /// </summary>
        /// <param name="CareForId"></param>
        /// <returns></returns>
        public bool StopIt(int CareForId)
        { return Dal.StopIt(CareForId); }

        /// <summary>
        /// 开启
        /// </summary>
        /// <param name="CareForId">编号</param>
        /// <returns></returns>
        public bool StartIt(int CareForId)
        {
            return Dal.StartIt(CareForId);
        }

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="CareForId">编号</param>
        /// <returns></returns>
        public bool DeletIt(int CareForId)
        {
          return  Dal.DeletIt(CareForId);
        }

         /// <summary>
        /// 返回实体
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        public EyouSoft.Model.CompanyStructure.CustomerCareforInfo GetModel(int Id)
        { return Dal.GetModel(Id); }

 
    }
}
