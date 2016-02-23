using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EyouSoft.BLL.CompanyStructure
{
    /// <summary>
    /// 销售地区业务逻辑
    /// </summary>
    public class BSaleArea : BLLBase
    {
        private readonly IDAL.CompanyStructure.ISaleArea _dal =
            Component.Factory.ComponentFactory.CreateDAL<IDAL.CompanyStructure.ISaleArea>();

        /// <summary>
        /// 添加销售地区
        /// </summary>
        /// <param name="model">销售地区实体</param>
        /// <returns>
        /// 1：成功；
        /// 0：参数错误；
        /// -1：名称已经存在；
        /// -2：新增失败（sql错误）
        /// </returns>
        public int AddSaleArea(Model.CompanyStructure.MSaleArea model)
        {
            if (model == null || string.IsNullOrEmpty(model.SaleAreaName) || model.CompanyId <= 0) return 0;

            int dalRetCode = _dal.AddSaleArea(model);
            if (dalRetCode == 1)
            {
                var log = new Model.CompanyStructure.SysHandleLogs
                    {
                        EventTitle = "新增销售地区",
                        ModuleId = Model.EnumType.PrivsStructure.Privs2.系统设置_基础设置,
                        EventMessage = "新增销售地区，销售地区编号：" + model.SaleAreaId + "。"
                    };

                new SysHandleLogs().Add(log);
            }

            return dalRetCode;
        }

        /// <summary>
        /// 修改销售地区
        /// </summary>
        /// <param name="model">销售地区实体</param>
        /// <returns>
        /// 1：成功；
        /// 0：参数错误；
        /// -1：名称已经存在；
        /// -2：修改失败（sql错误）
        ///</returns>
        public int UpdateSaleArea(Model.CompanyStructure.MSaleArea model)
        {
            if (model == null || string.IsNullOrEmpty(model.SaleAreaName) || model.CompanyId <= 0 || model.SaleAreaId <= 0) return 0;

            int dalRetCode = _dal.UpdateSaleArea(model);
            if (dalRetCode == 1)
            {
                var log = new Model.CompanyStructure.SysHandleLogs
                    {
                        EventTitle = "修改销售地区",
                        ModuleId = Model.EnumType.PrivsStructure.Privs2.系统设置_基础设置,
                        EventMessage = "修改销售地区，销售地区编号：" + model.SaleAreaId + "。"
                    };

                new SysHandleLogs().Add(log);
            }

            return dalRetCode;
        }

        /// <summary>
        /// 删除销售地区
        /// </summary>
        /// <param name="id">销售地区编号</param>
        /// <returns>
        /// 1：成功；
        /// 0：参数错误；
        /// -1：已使用不允许删除；
        /// -2：多个删除的时候，删除可以删除的；不能删除的没有删除
        /// -3：删除失败（sql错误）
        /// </returns>
        public int DeleteSaleArea(params int[] id)
        {
            if (id == null || id.Length <= 0) return 0;

            var keyi = _dal.ExistsDeleteSaleArea(id);

            if (keyi == null || keyi.Length <= 0) return -1;

            int dalRetCode = _dal.DeleteSaleArea(keyi);
            if (dalRetCode == 1)
            {
                if (keyi.Length != id.Length) dalRetCode = -2;
                var log = new Model.CompanyStructure.SysHandleLogs
                {
                    EventTitle = "删除销售地区",
                    ModuleId = Model.EnumType.PrivsStructure.Privs2.系统设置_基础设置,
                    EventMessage = "删除销售地区，销售地区编号：" + GetIdsByArr(keyi) + "。"
                };

                new SysHandleLogs().Add(log);
            }

            return dalRetCode;
        }

        /// <summary>
        /// 获取销售地区信息
        /// </summary>
        /// <param name="id">销售地区编号</param>
        /// <returns></returns>
        public Model.CompanyStructure.MSaleArea GetSaleArea(int id)
        {
            if (id <= 0) return null;

            return _dal.GetSaleArea(id);
        }

        /// <summary>
        /// 获取销售地区信息
        /// </summary>
        /// <param name="seach">销售地区查询实体</param>
        /// <param name="companyId"></param>
        /// <returns></returns>
        public IList<Model.CompanyStructure.MSaleArea> GetSaleArea(int companyId, Model.CompanyStructure.MSearchSaleArea seach)
        {
            if (companyId <= 0) return null;

            return _dal.GetSaleArea(companyId, seach);
        }

        /// <summary>
        /// 获取销售地区信息
        /// </summary>
        /// <param name="companyId">公司编号</param>
        /// <param name="pageSize">每页记录数</param>
        /// <param name="pageIndex">当前页数</param>
        /// <param name="recordCount">总记录数</param>
        /// <param name="seach">销售地区查询实体</param>
        /// <returns></returns>
        public IList<Model.CompanyStructure.MSaleArea> GetSaleArea(int companyId, int pageSize, int pageIndex, ref int recordCount
            , Model.CompanyStructure.MSearchSaleArea seach)
        {
            if (companyId <= 0 || pageIndex <= 0 || pageSize <= 0) return null;

            return _dal.GetSaleArea(companyId, pageSize, pageIndex, ref recordCount, seach);
        }
    }
}
