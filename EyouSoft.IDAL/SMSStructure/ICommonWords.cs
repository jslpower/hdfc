using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EyouSoft.IDAL.SMSStructure
{
    /// <summary>
    /// 短信中心-短信常用短语列表及常用短语类型数据访问接口
    /// </summary>
    public interface ICommonWords
    {
        /// <summary>
        /// 添加常用短语
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        bool AddCommonWords(EyouSoft.Model.SMSStructure.CommonWords model);

        /// <summary>
        /// 添加常用短语类型
        /// </summary>
        /// <param name="?"></param>
        /// <returns></returns>
        int AddCommonWordsClass(EyouSoft.Model.SMSStructure.CommonWordClass model);

        /// <summary>
        /// 更新常用短语
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        bool UpdateCommonWords(EyouSoft.Model.SMSStructure.CommonWords model);

        /// <summary>
        /// 删除一条常用短语
        /// </summary>
        /// <param name="Id">常用短语编号</param>
        /// <returns></returns>
        bool DeleteCommonWords(string[] Ids);

        /// <summary>
        /// 删除一条常用短语类型
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        bool DeleteCommonWordsClass(int Id);

        /// <summary>
        /// 获取一个常用短语实体
        /// </summary>
        /// <param name="Id">常用短语编号</param>
        /// <returns></returns>
        EyouSoft.Model.SMSStructure.CommonWords GetCommonWords(string Id);

        /// <summary>
        /// 根据公司ID获取所有常用短语类型实体
        /// </summary>
        /// <param name="CompanyID"></param>
        /// <returns></returns>
        IList<EyouSoft.Model.SMSStructure.CommonWordClass> GetCommonWordsClass(int CompanyID);

        /// <summary>
        /// 分页获取常用短语列表
        /// </summary>
        /// <param name="pageSize">每页条数</param>
        /// <param name="pageIndex">开始页码</param>
        /// <param name="RecordCount">总数</param>
        /// <param name="companyId">公司编号</param>
        /// <param name="keywords">关键字为空不做查询条件</param>
        /// <param name="ClassId">常用短语类型ID</param>
        /// <returns></returns>
        IList<EyouSoft.Model.SMSStructure.CommonWords> GetList(int pageSize, int pageIndex, ref int RecordCount,int companyId,string keywords,int ClassId);
    }
}
