using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using EyouSoft.BLL.CompanyStructure;

namespace EyouSoft.BLL.FinStructure
{
    /// <summary>
    /// 银行余额业务逻辑
    /// </summary>
    public class BBankBalance : BLLBase
    {
        private readonly IDAL.FinStructure.IBankBalance _dal =
            Component.Factory.ComponentFactory.CreateDAL<IDAL.FinStructure.IBankBalance>();

        /// <summary>
        /// 添加银行明细
        /// </summary>
        /// <param name="model">银行明细实体</param>
        /// <returns>
        /// 1：成功；
        /// 0：参数错误；
        /// -1：添加失败；
        /// </returns>
        public int AddBankBalance(Model.FinStructure.MBankBalance model)
        {
            if (model == null || string.IsNullOrEmpty(model.BankId)
                || model.CompanyId <= 0 || model.OperatorId <= 0) return 0;

            int dalRetCode = _dal.AddBankBalance(model);
            if (dalRetCode == 1)
            {
                var log = new Model.CompanyStructure.SysHandleLogs
                {
                    EventTitle = "新增银行余额",
                    ModuleId = Model.EnumType.PrivsStructure.Privs2.财务管理_银行余额,
                    EventMessage = "新增银行余额，编号：" + model.Id + "。"
                };

                new SysHandleLogs().Add(log);
            }

            return dalRetCode;
        }


        /// <summary>
        /// 删除银行余额明细
        /// </summary>
        /// <param name="id"></param>
        /// <returns>1成功 0失败</returns>
        public int DeleteBankBalance(int id)
        {
            if (id == 0) return 0;
            int flg = _dal.DeleteBankBalance(id);
            if (flg == 1)
            {
                var log = new Model.CompanyStructure.SysHandleLogs
                {
                    EventTitle = "删除银行余额",
                    ModuleId = Model.EnumType.PrivsStructure.Privs2.财务管理_银行余额,
                    EventMessage = "删除银行余额，编号：" + id + "。"
                };

                new SysHandleLogs().Add(log);
            }
            return flg;

        }

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
        public IList<Model.FinStructure.MBankBalance> GetBankBalance(
            int companyId,
            int pageSize,
            int pageIndex,
            ref int recordCount,
            Model.FinStructure.MSearchBankBalance search,
            out decimal totalBalance)
        {
            totalBalance = 0;
            if (companyId <= 0 || pageSize <= 0 || pageIndex <= 0) return null;

            return _dal.GetBankBalance(companyId, pageSize, pageIndex, ref recordCount, search, out totalBalance);
        }
    }



}
