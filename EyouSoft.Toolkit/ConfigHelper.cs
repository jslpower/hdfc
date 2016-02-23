using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;

namespace EyouSoft.Toolkit.ConfigHelper
{
    /// <summary>
    /// Web.config 操作类
    /// </summary>
    public sealed class ConfigClass
    {
        /// <summary>
        /// 取得ConnectionStrings节点的配置
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public static string GetConnectionString(string key)
        {
            string returnVal = "";
            System.Configuration.ConnectionStringSettingsCollection connectionStrings = System.Configuration.ConfigurationManager.ConnectionStrings;
            if (null != connectionStrings[key])
            {
                returnVal = connectionStrings[key].ConnectionString;
            }
            connectionStrings = null;
            return returnVal;
        }

        /// <summary>
        /// 获取配置信息
        /// </summary>
        /// <param name="sectionName">父级节点名称</param>
        /// <returns></returns>
        public static object GetConfigurationSecion(string sectionName)
        {
            if (!string.IsNullOrEmpty(sectionName))
            {
                return System.Configuration.ConfigurationManager.GetSection(sectionName);
            }

            return null;
        }

        /// <summary>
        /// 取得配置文件中的字符串KEY
        /// </summary>
        /// <param name="sectionName">节点名称</param>
        /// <param name="key">KEY名</param>
        /// <returns>返回KEY值</returns>
        public static string GetConfigString(string sectionName, string key)
        {
            string _v = string.Empty;

            if (string.IsNullOrEmpty(sectionName))
            {
                _v = System.Configuration.ConfigurationSettings.AppSettings[key];
            }
            else
            {
                if (string.IsNullOrEmpty(key)) return _v;

                var settings = (System.Collections.Specialized.NameValueCollection)GetConfigurationSecion(sectionName);
                if (settings != null && settings.Count > 0) _v = settings[key];
            }

            if (string.IsNullOrEmpty(_v)) return string.Empty;

            return _v;
        }

        /// <summary>
        /// 取得默认节点(appSettings)的配置
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public static string GetConfigString(string key)
        {
            return GetConfigString("appSettings", key);
        }

    }
}
