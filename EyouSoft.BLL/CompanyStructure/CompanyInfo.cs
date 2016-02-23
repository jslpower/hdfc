using System;
using System.Collections.Generic;
using System.Linq;

namespace EyouSoft.BLL.CompanyStructure
{
    /// <summary>
    /// 专线商公司信息BLL
    /// </summary>
    public class CompanyInfo : BLLBase
    {
        private readonly IDAL.CompanyStructure.ICompanyInfo _dal =
            Component.Factory.ComponentFactory.CreateDAL<IDAL.CompanyStructure.ICompanyInfo>();

        #region 公共方法

        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="model">公司信息实体</param>
        /// <returns></returns>
        public bool Update(Model.CompanyStructure.CompanyInfo model)
        {
            if (model == null || model.Id <= 0) return false;

            bool isTrue = this._dal.Update(model);

            #region LGWR

            if (isTrue)
            {
                var logInfo = new Model.CompanyStructure.SysHandleLogs
                    {
                        EventCode = Model.CompanyStructure.SysHandleLogsNO.EventCode,
                        EventMessage =
                            DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "{0}在"
                            + Model.EnumType.PrivsStructure.Privs2.系统设置_公司信息 + "修改了公司信息数据。",
                        EventTitle = "修改公司信息",
                        ModuleId = Model.EnumType.PrivsStructure.Privs2.系统设置_公司信息
                    };

                new SysHandleLogs().Add(logInfo);
            }

            #endregion

            return isTrue;
        }
        /// <summary>
        /// 获取公司信息实体
        /// </summary>
        /// <param name="companyId">公司编号</param>
        /// <param name="systemId">系统编号</param>
        /// <returns></returns>
        public Model.CompanyStructure.CompanyInfo GetModel(int companyId, int systemId)
        {
            if (companyId <= 0) return null;
            return this._dal.GetModel(companyId, systemId);
        }

        /// <summary>
        /// 获取公司信息实体
        /// </summary>
        /// <param name="companyId">公司编号</param>
        /// <returns></returns>
        public Model.CompanyStructure.CompanyInfo GetModel(int companyId)
        {
            if (companyId <= 0) return null;
            return this._dal.GetModel(companyId, 0);
        }

        /// <summary>
        /// 添加公司附件信息
        /// </summary>
        /// <param name="companyId">公司编号</param>
        /// <param name="list">附件路径集合</param>
        /// <returns>返回1成功，其他失败</returns>
        public int AddCompanyFile(int companyId, IList<string> list)
        {
            if (companyId <= 0 || list == null || !list.Any()) return 0;

            return _dal.AddCompanyFile(companyId, list);
        }

        /// <summary>
        /// 删除公司附件
        /// </summary>
        /// <param name="fileId">附件编号</param>
        /// <returns>返回1成功，其他失败</returns>
        public int DeleteCompanyFile(params string[] fileId)
        {
            if (fileId == null || fileId.Length <= 0) return 0;

            return _dal.DeleteCompanyFile(fileId);
        }

        #endregion 公共方法

    }
}
