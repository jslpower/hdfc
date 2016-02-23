using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EyouSoft.IDAL.CompanyStructure
{
    /// <summary>
    /// 个人中心-文档管理数据层接口
    /// </summary>
    /// 鲁功源  2011-01-17
    public interface IPersonDocument
    {
        /// <summary>
        /// 添加
        /// </summary>
        /// <param name="model">文档管理实体</param>
        /// <returns>true:成功 false:失败</returns>
        bool Add(EyouSoft.Model.CompanyStructure.PersonDocument model);
        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="model">文档管理实体</param>
        /// <returns>true:成功 false:失败</returns>
        bool Update(EyouSoft.Model.CompanyStructure.PersonDocument model);
        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="Ids">主键编号集合</param>
        /// <returns>true:成功 false:失败</returns>
        bool Delete(params string[] Ids);
        /// <summary>
        /// 获取实体
        /// </summary>
        /// <param name="Id">主键编号</param>
        /// <returns>文档管理实体</returns>
        EyouSoft.Model.CompanyStructure.PersonDocument GetModel(int Id);
        /// <summary>
        /// 分页获取列表
        /// </summary>
        /// <param name="pageSize">每页现实条数</param>
        /// <param name="pageIndex">当前页码</param>
        /// <param name="RecordCount">总记录数</param>
        /// <param name="CompanyId">公司编号 =0返回所有</param>
        /// <param name="OperatorId">上传人编号 =0返回所有</param>
        /// <returns>文档管理列表</returns>
        IList<EyouSoft.Model.CompanyStructure.PersonDocument> GetList(int pageSize, int pageIndex, ref int RecordCount, int CompanyId, int OperatorId);
    }
}
