using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EyouSoft.IDAL.CompanyStructure
{
    /// <summary>
    /// 专线商公司账户信息IDAL
    /// </summary>
    public interface ICompanyInfo
    {
        /// <summary>
        /// 获取公司信息实体
        /// </summary>
        /// <param name="companyId">公司编号</param>
        /// <param name="systemId">系统编号</param>
        /// <returns></returns>
        Model.CompanyStructure.CompanyInfo GetModel(int companyId, int systemId);

        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="model">公司信息实体</param>
        /// <returns></returns>
        bool Update(Model.CompanyStructure.CompanyInfo model);

        /// <summary>
        /// 添加公司福建信息
        /// </summary>
        /// <param name="companyId">公司编号</param>
        /// <param name="list">附件路径集合</param>
        /// <returns>返回1成功，其他失败</returns>
        int AddCompanyFile(int companyId, IList<string> list);

        /// <summary>
        /// 删除公司附件
        /// </summary>
        /// <param name="fileId">附件编号</param>
        /// <returns>返回1成功，其他失败</returns>
        int DeleteCompanyFile(params string[] fileId);
    }
}
