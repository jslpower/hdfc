using System;
using System.Collections.Generic;

namespace EyouSoft.BLL.CompanyStructure
{
    /// <summary>
    /// 客户信用等级BLL
    /// Author:邓保朝 2013-09-29 
    /// </summary>
    public class Rating : BLLBase
    {
        #region constructure

        /// <summary>
        /// 客户信用等级数据层
        /// </summary>
        private readonly IDAL.CompanyStructure.IRating _dal = Component.Factory.ComponentFactory.CreateDAL<IDAL.CompanyStructure.IRating>();
        /// <summary>
        ///  操作日志业务逻辑
        /// </summary>
        private readonly SysHandleLogs _handleLogsBll = new SysHandleLogs();

        #endregion

        #region 成员方法

        /// <summary>
        /// 验证是否已经存在同名的客户信用等级
        /// </summary>
        /// <param name="RatingName">客户信用等级名称</param>
        /// <param name="companyId">公司编号</param>
        /// <param name="id">要排除的客户信用等级编号</param>
        /// <returns>true:存在 false:不存在</returns>
        public bool IsExists(string RatingName, int companyId, int id)
        {
            if (string.IsNullOrEmpty(RatingName) || companyId <= 0) return true;

            return this._dal.IsExists(RatingName, companyId, id);
        }

        /// <summary>
        /// 添加
        /// </summary>
        /// <param name="model">客户信用等级实体</param>
        /// <returns>true:成功 false:失败</returns>
        public bool Add(Model.CompanyStructure.Rating model)
        {
            if (model == null || string.IsNullOrEmpty(model.RatingName)) return false;

            bool result = this._dal.Add(model);

            if (result) this._handleLogsBll.Add(AddLogs("新增", model.Id.ToString(), model.RatingName));

            return result;
        }

        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="model">客户信用等级实体</param>
        /// <returns>true:成功 false:失败</returns>
        public bool Update(Model.CompanyStructure.Rating model)
        {
            if (model == null || string.IsNullOrEmpty(model.RatingName) || model.Id <= 0) return false;

            bool result = this._dal.Update(model);

            if (result) this._handleLogsBll.Add(AddLogs("修改", model.Id.ToString(), model.RatingName));

            return result;
        }

        /// <summary>
        /// 获取客户信用等级实体
        /// </summary>
        /// <param name="id">主键编号</param>
        /// <returns></returns>
        public Model.CompanyStructure.Rating GetModel(int id)
        {
            if (id <= 0) return null;

            return this._dal.GetModel(id);
        }

        /// <summary>
        /// 删除客户信用等级
        /// </summary>
        /// <param name="RatingId">客户信用等级编号</param>
        /// <returns>true:成功 false:失败</returns>
        public int Delete(int RatingId)
        {
            if (RatingId < 1) return 0;
            if (_dal.IsShiYong(RatingId)) return -1;

            bool result = this._dal.Delete(RatingId);

            if (result)
            {
                this._handleLogsBll.Add(AddLogs("删除", RatingId.ToString(), string.Empty));
                return 1;
            }

            return -100;
        }

        /// <summary>
        /// 分页获取公司客户信用等级集合
        /// </summary>
        /// <param name="pageSize">每页显示条数</param>
        /// <param name="pageIndex">当前页码</param>
        /// <param name="recordCount">总记录数</param>
        /// <param name="companyId">公司编号</param>
        /// <returns>公司客户信用等级集合</returns>
        public IList<Model.CompanyStructure.Rating> GetList(int pageSize, int pageIndex, ref int recordCount, int companyId)
        {
            if (companyId <= 0 || pageIndex <= 0 || pageSize <= 0) return null;

            return this._dal.GetList(pageSize, pageIndex, ref recordCount, companyId);
        }

        /// <summary>
        /// 获取当前公司的所有客户信用等级信息
        /// </summary>
        /// <param name="companyId">公司ID</param>
        /// <returns></returns>
        public IList<Model.CompanyStructure.Rating> GetRatingByCompanyId(int companyId)
        {
            if (companyId <= 0) return null;

            return this._dal.GetRatingByCompanyId(companyId);
        }

        /// <summary>
        /// 获取指定公司客户信用等级排序信息(最小及最大排序号)
        /// </summary>
        /// <param name="companyId">公司编号</param>
        /// <param name="min">最小排序号</param>
        /// <param name="max">最大排序号</param>
        public void GetRatingSortId(int companyId, out int min, out int max)
        {
            min = 0; max = 0;
            if (companyId < 1) return;
            this._dal.GetRatingSortId(companyId, out min, out max);
        }
        #endregion

        /// <summary>
        /// 添加日志记录
        /// </summary>
        /// <param name="actionName">操作名称</param>
        /// <param name="RatingId">操作的客户信用等级编号（可以是多个）</param>
        /// <param name="RatingName">客户信用等级名称</param>
        /// <returns></returns>
        private Model.CompanyStructure.SysHandleLogs AddLogs(string actionName, string RatingId, string RatingName)
        {
            var model = new Model.CompanyStructure.SysHandleLogs
                {
                    ModuleId = Model.EnumType.PrivsStructure.Privs2.系统设置_基础设置,
                    EventCode = Model.CompanyStructure.SysHandleLogsNO.EventCode,
                    EventMessage =
                        DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "{0}在"
                        + Model.EnumType.PrivsStructure.Privs2.系统设置_基础设置 + actionName + "了客户信用等级，编号为" + RatingId + "，名称为"
                        + RatingName,
                    EventTitle = actionName + Model.EnumType.PrivsStructure.Privs2.系统设置_基础设置 + " 客户信用等级"
                };

            return model;
        }
    }
}
