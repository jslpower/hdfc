using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EyouSoft.IDAL.FinStructure
{
    /// <summary>
    /// 银行余额数据接口
    /// </summary>
    public interface IBankBalance
    {
        /// <summary>
        /// 添加银行明细
        /// </summary>
        /// <param name="model">银行明细实体</param>
        /// <returns>
        /// 1：成功；
        /// 0：参数错误；
        /// -1：添加失败；
        /// </returns>
        int AddBankBalance(Model.FinStructure.MBankBalance model);

        /// <summary>
        /// 删除银行余额明细
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        int DeleteBankBalance(int id);

        /// <summary>
        /// 获取银行明细列表
        /// </summary>
        /// <param name="companyId">公司编号</param>
        /// <param name="pageSize">每页记录数</param>
        /// <param name="pageIndex">当前页数</param>
        /// <param name="recordCount">总记录数</param>
        /// <param name="search">银行明细查询实体</param>
        /// <param name="totalBalance">银行余额</param>
        /// <returns></returns>
        IList<Model.FinStructure.MBankBalance> GetBankBalance(
            int companyId,
            int pageSize,
            int pageIndex,
            ref int recordCount,
            Model.FinStructure.MSearchBankBalance search,
            out decimal totalBalance);
    }
}
