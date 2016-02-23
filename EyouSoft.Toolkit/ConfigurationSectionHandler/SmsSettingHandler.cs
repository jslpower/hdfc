//汪奇志 2012-04-13
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;
using System.Xml;

namespace EyouSoft.Toolkit.ConfigurationSectionHandler
{
    #region 短信自定义配置节处理程序
    /// <summary>
    /// 短信自定义配置节处理程序
    /// </summary>
    public class SmsSettingHandler : IConfigurationSectionHandler
    {
        #region IConfigurationSectionHandler 成员
        public object Create(object parent, object configContext, System.Xml.XmlNode section)
        {
            XmlNodeList nodes;
            string index, name, username, password;
            List<SmsSettingInfo> list = new List<SmsSettingInfo>();
            nodes = section.SelectNodes("add");

            foreach (XmlNode node in nodes)
            {
                index = node.Attributes.GetNamedItem("Index").Value;
                name = node.Attributes.GetNamedItem("Name").Value;
                username = node.Attributes.GetNamedItem("Username").Value;
                password = node.Attributes.GetNamedItem("Password").Value;

                list.Add(new SmsSettingInfo()
                {
                    Index = int.Parse(index),
                    Name = name,
                    Username = username,
                    Password = password
                });
            }
            return list;
        }
        #endregion
    }
    #endregion

    #region 短信配置信息业务实体
    /// <summary>
    /// 短信配置信息业务实体
    /// </summary>
    public class SmsSettingInfo
    {
        /// <summary>
        /// 通道索引
        /// </summary>
        public int Index { get; set; }
        /// <summary>
        /// 通道名称
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// 用户名
        /// </summary>
        public string Username { get; set; }
        /// <summary>
        /// 密码
        /// </summary>
        public string Password { get; set; }
    }
    #endregion
}
