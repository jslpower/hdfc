using System;

namespace EyouSoft.BLL.CompanyStructure
{
    /// <summary>
    /// 公司系统配置BLL
    /// </summary>
    public class CompanySetting : BLLBase
    {
        private readonly IDAL.CompanyStructure.ICompanySetting _dal =
            Component.Factory.ComponentFactory.CreateDAL<IDAL.CompanyStructure.ICompanySetting>();

        #region public members

        /// <summary>
        /// 设置系统配置信息
        /// </summary>
        /// <param name="model">系统配置实体</param>
        /// <returns>true：成功 false:失败</returns>
        public bool SetCompanySetting(Model.CompanyStructure.CompanyFieldSetting model)
        {
            if (model == null) return false;

            bool dalResult = this._dal.SetCompanySetting(model);

            if (dalResult)
            {
                Cache.Facade.EyouSoftCache.Remove(string.Format(Cache.Tag.TagName.ComSetting, model.CompanyId));
            }

            return dalResult;
        }

        /// <summary>
        /// 设置公司的LOGO
        /// </summary>
        /// <param name="companyId">公司编号</param>
        /// <param name="logo">LOGO文件路径</param>
        /// <returns></returns>
        public bool SetCompanyLogo(int companyId, string logo)
        {
            if (companyId <= 0 || string.IsNullOrEmpty(logo))
                return false;
            bool dalResult = this._dal.SetValue(companyId, "CompanyLogo", logo);

            if (dalResult)
            {
                Cache.Facade.EyouSoftCache.Remove(string.Format(Cache.Tag.TagName.ComSetting, companyId));
            }

            return dalResult;
        }

        /// <summary>
        /// 获取指定公司的系统配置信息
        /// </summary>
        /// <param name="companyId">公司编号</param>
        /// <returns></returns>
        public Model.CompanyStructure.CompanyFieldSetting GetSetting(int companyId)
        {
            if (companyId <= 0) return null;

            return Security.Membership.UserProvider.GetComSetting(companyId);
        }

        /// <summary>
        /// 获取指定公司的LOGO
        /// </summary>
        /// <param name="companyId">公司编号</param>
        /// <returns></returns>
        public string GetCompanyLogo(int companyId)
        {
            if (companyId <= 0) return string.Empty;
                
            var model = GetSetting(companyId);
            return model == null ? string.Empty : model.CompanyLogo;
        }

        #endregion
    }
}
