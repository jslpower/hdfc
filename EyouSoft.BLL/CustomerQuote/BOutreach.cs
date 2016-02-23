using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using EyouSoft.BLL.CompanyStructure;

namespace EyouSoft.BLL.CustomerQuote
{
    /// <summary>
    /// 外联每日足迹业务逻辑
    /// </summary>
    public class BOutreach : BLLBase
    {
        private readonly IDAL.CustomerQuote.IOutreach _dal =
            Component.Factory.ComponentFactory.CreateDAL<IDAL.CustomerQuote.IOutreach>();

        /// <summary>
        /// 添加外联每日足迹
        /// </summary>
        /// <param name="model">外联每日足迹实体</param>
        /// <returns>
        /// 1：成功；
        /// 0：参数错误；
        /// -1：添加失败；
        /// </returns>
        public int AddOutreach(Model.CustomerQuote.MOutreach model)
        {
            if (model == null || model.CompanyId <= 0 || string.IsNullOrEmpty(model.SaleUnitId)) return 0;

            int dalRetCode = _dal.AddOutreach(model);
            if (dalRetCode == 1)
            {
                var log = new Model.CompanyStructure.SysHandleLogs
                {
                    EventTitle = "新增外联每日足迹",
                    ModuleId = Model.EnumType.PrivsStructure.Privs2.客户询价_客户日常询价,
                    EventMessage = "新增外联每日足迹，外联每日足迹编号：" + model.Id + "。"
                };

                new SysHandleLogs().Add(log);
            }

            return dalRetCode;
        }

        /// <summary>
        /// 修改外联每日足迹
        /// </summary>
        /// <param name="model">外联每日足迹实体</param>
        /// <returns>
        /// 1：成功；
        /// 0：参数错误；
        /// -1：修改失败；
        /// </returns>
        public int UpdateOutreach(Model.CustomerQuote.MOutreach model)
        {
            if (model == null || model.Id <= 0 || string.IsNullOrEmpty(model.SaleUnitId)) return 0;

            int dalRetCode = _dal.UpdateOutreach(model);
            if (dalRetCode == 1)
            {
                var log = new Model.CompanyStructure.SysHandleLogs
                {
                    EventTitle = "修改外联每日足迹",
                    ModuleId = Model.EnumType.PrivsStructure.Privs2.客户询价_客户日常询价,
                    EventMessage = "修改外联每日足迹，外联每日足迹编号：" + model.Id + "。"
                };

                new SysHandleLogs().Add(log);
            }

            return dalRetCode;
        }

        /// <summary>
        /// 删除外联每日足迹
        /// </summary>
        /// <param name="id">外联每日足迹编号</param>
        /// <returns>
        /// 1：成功；
        /// 0：参数错误；
        /// -1：删除失败；
        /// </returns>
        public int DeleteOutreach(params int[] id)
        {
            if (id == null || id.Length <= 0) return 0;

            int dalRetCode = _dal.DeleteOutreach(id);
            if (dalRetCode == 1)
            {
                var log = new Model.CompanyStructure.SysHandleLogs
                {
                    EventTitle = "删除外联每日足迹",
                    ModuleId = Model.EnumType.PrivsStructure.Privs2.客户询价_客户日常询价,
                    EventMessage = "删除外联每日足迹，外联每日足迹编号：" + this.GetIdsByArr(id) + "。"
                };

                new SysHandleLogs().Add(log);
            }

            return dalRetCode;
        }

        /// <summary>
        /// 获取外联每日足迹
        /// </summary>
        /// <param name="id">外联每日足迹编号</param>
        /// <returns></returns>
        public Model.CustomerQuote.MOutreach GetOutreach(int id)
        {
            if (id <= 0) return null;

            return _dal.GetOutreach(id);
        }

        /// <summary>
        /// 获取外联每日足迹
        /// </summary>
        /// <param name="companyId">公司编号</param>
        /// <param name="pageSize">每页记录数</param>
        /// <param name="pageIndex">当前页数</param>
        /// <param name="recordCount">总记录数</param>
        /// <param name="seach">外联每日足迹查询实体</param>
        /// <returns></returns>
        public IList<Model.CustomerQuote.MOutreach> GetOutreach(
            int companyId,
            int pageSize,
            int pageIndex,
            ref int recordCount,
            Model.CustomerQuote.MSearchOutreach seach)
        {
            if (companyId <= 0 || pageSize <= 0 || pageIndex <= 0) return null;

            return _dal.GetOutreach(companyId, pageSize, pageIndex, ref recordCount, seach);
        }
    }
}
