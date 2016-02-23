using System;
using System.Data;
using System.Collections.Generic;

namespace EyouSoft.IDAL.CompanyStructure
{
    /// <summary>
    /// 客户关怀
    /// </summary>
    /// 创建人：李焕超 2011-01-26
    public interface ICustomerCareFor
    {
        /// <summary>
        /// 添加
        /// </summary>
        /// <param name="Model"></param>
        /// <returns></returns>
        bool Add(EyouSoft.Model.CompanyStructure.CustomerCareforInfo Model);
      
        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="Model"></param>
        /// <returns></returns>
        bool Update(EyouSoft.Model.CompanyStructure.CustomerCareforInfo Model);
       
        /// <summary>
        /// 获取列表
        /// </summary>
        /// <param name="CompanyId"></param>
        /// <returns></returns>
        IList<EyouSoft.Model.CompanyStructure.CustomerCareforInfo> GetList(string CompanyId, int PageSize, int PageIndex, ref int RecordCount);

        /// <summary>
        /// 停发
        /// </summary>
        /// <param name="CareForId"></param>
        /// <returns></returns>
        bool StopIt(int CareForId);

        /// <summary>
        /// 开启
        /// </summary>
        /// <param name="CareForId">编号</param>
        /// <returns></returns>
        bool StartIt(int CareForId);

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="CareForId">编号</param>
        /// <returns></returns>
        bool DeletIt(int CareForId);

         /// <summary>
        /// 返回实体
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        EyouSoft.Model.CompanyStructure.CustomerCareforInfo GetModel(int Id);


        

    }
}
