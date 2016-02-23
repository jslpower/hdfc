using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Practices.EnterpriseLibrary.Data;
using System.Xml;

namespace EyouSoft.Toolkit.DAL
{
    /// <summary>
    /// 数据层访问基类
    /// 读取配置文件，生成数据库可用连接
    /// </summary>
    public class DALBase
    {
        private readonly Database _systemstore = DatabaseFactory.CreateDatabase("SystemStore");

        /// <summary>
        /// 系统库
        /// </summary>
        protected Database SystemStore
        {
            get
            {
                return _systemstore;
            }
        }


        /// <summary>
        /// 数据库CHAR(1)转换成布尔类型，1→true 其它→false
        /// </summary>
        /// <param name="s">CHAR(1)</param>
        /// <returns></returns>
        public bool GetBoolean(string s)
        {
            return s == "1" ? true : false;
        }

        /// <summary>
        /// 将bool转换成char(1) true:1 false:0
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        public string GetBooleanToStr(bool s)
        {
            return s ? "1" : "0";
        }

        /// <summary>
        /// 将整形Id数组转换为半角逗号分割的字符串
        /// </summary>
        /// <param name="ids">整形Id数组</param>
        /// <returns>半角逗号分割的字符串</returns>
        protected string GetIdsByArr(params int[] ids)
        {

            if (ids == null || ids.Length < 1) return string.Empty;

            StringBuilder s = new StringBuilder();

            foreach (var item in ids)
            {
                s.Append(",");
                s.Append(item);
            }

            return s.ToString().Substring(1);
        }

        /// <summary>
        /// 将char(36)Id转为可以直接in的格式('***','***','***')
        /// </summary>
        /// <param name="ids">char(36)Id数组</param>
        /// <returns>半角逗号分割的带单引号的字符串</returns>
        protected string GetIdsByArr(params string[] ids)
        {
            if (ids == null || ids.Length < 1) return string.Empty;

            var s = new StringBuilder();
            foreach (var item in ids)
            {
                s.Append(",");
                s.AppendFormat("'{0}'", Utils.ToSqlLike(item));
            }

            return s.ToString().Substring(1);
        }

        /// <summary>
        /// 获取XML文档属性值
        /// </summary>
        /// <param name="xml"></param>
        ///  <param name="attribute">属性</param>
        /// <returns></returns>
        protected string GetValueByXml(string xml, string attribute)
        {
            if (string.IsNullOrEmpty(xml)) return "";
            System.Xml.Linq.XElement xRoot = System.Xml.Linq.XElement.Parse(xml);
            var xRows = Utils.GetXElements(xRoot, "row");
            foreach (var xRow in xRows)
            {
                return Utils.GetXAttributeValue(xRow, attribute);
            }
            return "";
        }
    }
}
