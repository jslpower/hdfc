using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using EyouSoft.BLL.CompanyStructure;

namespace EyouSoft.BLL.FinStructure
{
    /// <summary>
    /// 出纳登账业务逻辑
    /// </summary>
    public class BChuNaDengZhang : BLLBase
    {
        private readonly IDAL.FinStructure.IChuNaDengZhang _dal =
            Component.Factory.ComponentFactory.CreateDAL<IDAL.FinStructure.IChuNaDengZhang>();

        /// <summary>
        /// 添加出纳登账信息（只做财务管理-出纳登账-新增使用）
        /// </summary>
        /// <param name="model">出纳登账实体</param>
        /// <returns>
        /// 1：成功；
        /// 0：参数错误；
        /// -1：添加失败；
        /// </returns>
        public int AddChuNaDengZhang(Model.FinStructure.MChuNaDengZhang model)
        {
            if (model == null || model.CompanyId <= 0 || string.IsNullOrEmpty(model.TourNo) || model.OperatorId <= 0)
                return 0;

            int dalRetCode = _dal.AddChuNaDengZhang(model);
            if (dalRetCode == 1)
            {
                var log = new Model.CompanyStructure.SysHandleLogs
                {
                    EventTitle = "新增出纳登账",
                    ModuleId = Model.EnumType.PrivsStructure.Privs2.财务管理_出纳登账,
                    EventMessage = "新增出纳登账，出纳登账编号：" + model.DengZhangId + "。"
                };

                new SysHandleLogs().Add(log);
            }

            return dalRetCode;
        }

        /// <summary>
        /// 修改出纳登账信息
        /// </summary>
        /// <param name="model">出纳登账实体</param>
        /// <returns>
        /// 1：成功；
        /// 0：参数错误；
        /// -1：不是用户登记的数据，不能修改；
        /// -2：修改失败；
        /// </returns>
        public int UpdateChuNaDengZhang(Model.FinStructure.MChuNaDengZhang model)
        {
            if (model == null || string.IsNullOrEmpty(model.DengZhangId) || model.CompanyId <= 0 || string.IsNullOrEmpty(model.TourNo)
                || model.OperatorId <= 0)
            {
                return 0;
            }

            int dalRetCode = _dal.UpdateChuNaDengZhang(model);
            if (dalRetCode == 1)
            {
                var log = new Model.CompanyStructure.SysHandleLogs
                {
                    EventTitle = "修改出纳登账",
                    ModuleId = Model.EnumType.PrivsStructure.Privs2.财务管理_出纳登账,
                    EventMessage = "修改出纳登账，出纳登账编号：" + model.DengZhangId + "。"
                };

                new SysHandleLogs().Add(log);
            }

            return dalRetCode;
        }

        /// <summary>
        /// 删除出纳登账信息
        /// </summary>
        /// <param name="id">出纳登账编号</param>
        /// <returns>
        /// 1：成功；
        /// 0：参数错误；
        /// -1：单条删除时，不是用户登记的数据，不能删除；
        /// -2：多条删除时，删除能删除的，不能删除的没有删除；
        /// -3：删除失败；
        /// </returns>
        public int DeleteChuNaDengZhang(params string[] id)
        {
            if (id == null || id.Length <= 0) return 0;

            int dalRetCode = _dal.DeleteChuNaDengZhang(id);
            if (dalRetCode == 1 || dalRetCode == -2)
            {
                var log = new Model.CompanyStructure.SysHandleLogs
                {
                    EventTitle = "删除出纳登账",
                    ModuleId = Model.EnumType.PrivsStructure.Privs2.财务管理_出纳登账,
                    EventMessage = "删除出纳登账，出纳登账编号：" + this.GetIdsByArr(id) + "。"
                };

                new SysHandleLogs().Add(log);
            }

            return dalRetCode;
        }

        /// <summary>
        /// 获取出纳登账信息
        /// </summary>
        /// <param name="id">出纳登账编号</param>
        /// <returns></returns>
        public Model.FinStructure.MChuNaDengZhang GetChuNaDengZhang(string id)
        {
            if (string.IsNullOrEmpty(id)) return null;

            return _dal.GetChuNaDengZhang(id);
        }

        /// <summary>
        /// 获取出纳登账信息
        /// </summary>
        /// <param name="companyId"> 公司编号</param>
        /// <param name="pageSize">每页记录数</param>
        /// <param name="pageIndex">当前页数</param>
        /// <param name="recordCount">总记录数</param>
        /// <param name="search">查询实体</param>
        /// <param name="heJi">合计实体（赋值null或者新实例化一个对象）</param>
        /// <returns></returns>
        public IList<Model.FinStructure.MChuNaDengZhang> GetChuNaDengZhang(
            int companyId,
            int pageSize,
            int pageIndex,
            ref int recordCount,
            Model.FinStructure.MSearchChuNaDengZhang search,
            ref Model.FinStructure.MChuNaDengZhangHeJi heJi)
        {
            if (companyId <= 0 || pageSize <= 0 || pageIndex <= 0) return null;

            return _dal.GetChuNaDengZhang(companyId, pageSize, pageIndex, ref recordCount, search,ref heJi);
        }
    }
}
