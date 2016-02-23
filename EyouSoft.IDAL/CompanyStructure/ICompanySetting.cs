
namespace EyouSoft.IDAL.CompanyStructure
{
    /// <summary>
    /// 公司系统配置数据层接口
    /// </summary>
    /// 鲁功源 2011-01-21
    public interface ICompanySetting
    {
        /// <summary>
        /// 设置系统配置信息
        /// </summary>
        /// <param name="model">系统配置实体</param>
        /// <returns>true：成功 false:失败</returns>
        bool SetCompanySetting(Model.CompanyStructure.CompanyFieldSetting model);

        /// <summary>
        /// 获取指定公司的配置信息
        /// </summary>
        /// <param name="companyId"></param>
        /// <param name="fileKey"></param>
        /// <returns></returns>
        string GetValue(int companyId, string fileKey);

        /// <summary>
        /// 设置指定公司的配置信息
        /// </summary>
        /// <param name="companyId">公司编号</param>
        /// <param name="fieldKey">配置key</param>
        /// <param name="fieldValue">配置value</param>
        /// <returns></returns>
        bool SetValue(int companyId, string fieldKey, string fieldValue);

    }
}
