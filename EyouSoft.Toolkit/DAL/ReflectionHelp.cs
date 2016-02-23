using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using System.Data.Common;
using Microsoft.Practices.EnterpriseLibrary.Data;
using EyouSoft.Toolkit.DAL;
using System.Data;
using System.Data.SqlTypes;
using System.Data.Linq;
using System.Data.Linq.Mapping;

namespace EyouSoft.Toolkit.Function
{
    /// <summary>
    /// 反射帮助
    /// </summary>
    public class ReflectionHelp
    {
        /// <summary>
        /// 获得Model属性列表
        /// </summary>
        /// <param name="list">Model列表</param>
        /// <returns></returns>
        public static List<List<System.Reflection.PropertyInfo>> GetModelListPropertyInfo(List<object> list)
        {
            List<List<System.Reflection.PropertyInfo>> propList = new List<List<System.Reflection.PropertyInfo>>();
            for (int i = 0; i < list.Count; i++)
            {
                object obj = list[i];
                propList.Add(obj.GetType().GetProperties().ToList<System.Reflection.PropertyInfo>());
            }
            return propList;
        }


        /// <summary>
        /// 获得Model属性列表
        /// </summary>
        /// <param name="obj">Model</param>
        /// <returns></returns>
        public static List<System.Reflection.PropertyInfo> GetModelPropertyInfo(object obj)
        {
            return obj.GetType().GetProperties().ToList<System.Reflection.PropertyInfo>();
        }


        /// <summary>
        /// 获得单个Model的Xml语句
        /// </summary>
        /// <param name="obj">Model对象</param>
        /// <returns></returns>
        public static string GetModelXmlString(object obj)
        {
            if (obj != null)
            {
            List<PropertyInfo> propList = EyouSoft.Toolkit.Function.ReflectionHelp.GetModelPropertyInfo(obj);

                StringBuilder sbXml = new StringBuilder();
                sbXml.Append("<Root><Item>");
                foreach (PropertyInfo p in propList)
                {
                    if (p.PropertyType.IsGenericType&&p.PropertyType.IsInterface)
                    {
                       
                    }
                    else if (p.PropertyType.IsEnum||(p.PropertyType.IsGenericType&&p.PropertyType.IsValueType&&IsEnum(p.PropertyType.ToString())))
                    {
                        object[] o = p.GetCustomAttributes(typeof(ColumnAttribute), false);
                        if (o.Length > 0)
                        {
                            string Name = ((ColumnAttribute)o[0]).Name;
                            if (!((ColumnAttribute)o[0]).IsDbGenerated)
                            {
                                object enumValue=p.GetValue(obj, null);
                                sbXml.AppendFormat("<{0}>{1}</{0}>", Name, EyouSoft.Toolkit.Utils.ReplaceXmlSpecialCharacter( enumValue!= null ?((int)enumValue).ToString() : "0"));
                            }
                            
                        }
                        
                    }
                    
                    else if (p.PropertyType == typeof(bool))
                    {
                        object[] o = p.GetCustomAttributes(typeof(ColumnAttribute), false);
                        if (o.Length > 0)
                        {
                            string Name = ((ColumnAttribute)o[0]).Name;
                            if (!((ColumnAttribute)o[0]).IsDbGenerated)
                            {
                                string value = string.Empty;
                                object ovalue = p.GetValue(obj, null);
                                if (ovalue != null)
                                {
                                    if ((bool)ovalue)
                                    {
                                        value = "1";
                                    }
                                    else
                                    {
                                        value = "0";
                                    }
                                }
                                sbXml.AppendFormat("<{0}>{1}</{0}>", Name, EyouSoft.Toolkit.Utils.ReplaceXmlSpecialCharacter(value));
                            }

                        }
                    }
                    else if (p.PropertyType.IsClass && p.PropertyType != typeof(string))
                    {

                    }
                    else
                    {
                        object[] o = p.GetCustomAttributes(typeof(ColumnAttribute), false);
                        if (o.Length > 0)
                        {
                            string Name = ((ColumnAttribute)o[0]).Name;
                            if (!((ColumnAttribute)o[0]).IsDbGenerated)
                                sbXml.AppendFormat("<{0}>{1}</{0}>", Name, EyouSoft.Toolkit.Utils.ReplaceXmlSpecialCharacter(p.GetValue(obj, null) != null ? p.GetValue(obj, null).ToString() : string.Empty));
                        }

                    }
                }

               
                sbXml.Append("</Item></Root>");
                return sbXml.ToString();
            }
            else
            {
                return string.Empty;
            }
        }



        public static bool IsEnum(string pname)
        {
            if (pname.IndexOf("System.DateTime") >= 0)
            {
                return false;
            }
            else if (pname.IndexOf("System.Decimal") >= 0)
            {
                return false;
            }
            else if (pname.IndexOf("System.Int32") >= 0)
            {
                return false;
            }
            else if (pname.IndexOf("System.Int16") >= 0)
            {
                return false;
            }
            else if (pname.IndexOf("System.Int64") >= 0)
            {
                return false;
            }
            else if (pname.IndexOf("System.Boolean") >= 0)
            {
                return false;
            }
            else if (pname.IndexOf("System.Byte") >= 0)
            {
                return false;
            }
            else if (pname.IndexOf("System.Char") >= 0)
            {
                return false;
            }
            else if (pname.IndexOf("System.Double") >= 0)
            {
                return false;
            }
            else
            {
                return true;
            }
        }


        /// <summary>
        /// 获得列表Model的Xml语句
        /// </summary>
        /// <param name="list">Model列表</param>
        /// <returns></returns>
        public static string GetModelXmlString(IList<object> list)
        {
            if (list != null && list.Count>0)
            {
                List<object> l = (List<object>)list;
                //List<List<PropertyInfo>> propList = EyouSoft.Toolkit.Function.ReflectionHelp.GetModelListPropertyInfo(l);
                StringBuilder sbXml = new StringBuilder();
                sbXml.Append("<Root>");
                
                //foreach (List<PropertyInfo> lp in propList)
                //{
                   
                //}

                foreach (object lo in l)
                {
                    sbXml.Append("<Item>");
                    foreach (PropertyInfo p in lo.GetType().GetProperties())
                    {
                        if (p.PropertyType.IsGenericType&&p.PropertyType.IsInterface)//IList集合
                        {

                        }
                        else if (p.PropertyType.IsEnum || (p.PropertyType.IsGenericType && p.PropertyType.IsValueType && IsEnum(p.PropertyType.ToString())))//枚举
                        {
                            object[] obj = p.GetCustomAttributes(typeof(ColumnAttribute), false);
                            if (obj.Length > 0)
                            {
                                string Name = ((ColumnAttribute)obj[0]).Name;
                                object enumValue=p.GetValue(lo, null);
                                if (!((ColumnAttribute)obj[0]).IsDbGenerated)
                                    sbXml.AppendFormat("<{0}>{1}</{0}>", Name, EyouSoft.Toolkit.Utils.ReplaceXmlSpecialCharacter((( enumValue!= null ? ((int)enumValue).ToString() : "0")).ToString()));
                            }

                        }
                        else if (p.PropertyType == typeof(bool))
                        {
                            object[] o = p.GetCustomAttributes(typeof(ColumnAttribute), false);
                            if (o.Length > 0)
                            {
                                string Name = ((ColumnAttribute)o[0]).Name;
                                if (!((ColumnAttribute)o[0]).IsDbGenerated)
                                {
                                    string value = string.Empty;
                                    object ovalue = p.GetValue(lo, null);
                                    if (ovalue != null)
                                    {
                                        if ((bool)ovalue)
                                        {
                                            value = "1";
                                        }
                                        else
                                        {
                                            value = "0";
                                        }
                                    }
                                    sbXml.AppendFormat("<{0}>{1}</{0}>", Name, EyouSoft.Toolkit.Utils.ReplaceXmlSpecialCharacter(value));
                                }

                            }
                        }
                        else if (p.PropertyType.IsClass && p.PropertyType != typeof(string))//类
                        {

                        }
                        else//其他的类型
                        {
                            object[] obj = p.GetCustomAttributes(typeof(ColumnAttribute), false);
                            if (obj.Length > 0)
                            {
                                string Name = ((ColumnAttribute)obj[0]).Name;
                                if (!((ColumnAttribute)obj[0]).IsDbGenerated)
                                    sbXml.AppendFormat("<{0}>{1}</{0}>", Name, EyouSoft.Toolkit.Utils.ReplaceXmlSpecialCharacter(((p.GetValue(lo, null) != null ? (p.GetValue(lo, null)).ToString() : string.Empty)).ToString()));
                            }

                        }
                    }
                    sbXml.Append("</Item>");
                }

               
                
                sbXml.Append("</Root>");
                return sbXml.ToString();
            }
            else
            {
                return string.Empty;
            }
        }


        public void BindEnumToDDL(System.Web.UI.WebControls.DropDownList ddl, Enum e)
        {
            FieldInfo[]  fs=e.GetType().GetFields();
            DataTable dt = new DataTable();
            DataColumn dc = new DataColumn("Text");
            dt.Columns.Add(dc);
            dc = new DataColumn("Value");
            dt.Columns.Add(dc);
            DataRow dr;
            foreach (FieldInfo f in fs)
            {
                dr = dt.NewRow();
                dr[0] = f.Name;
                if(f.IsPublic)
                dr[1] = (e.GetType().InvokeMember(f.Name, BindingFlags.GetField, null, null, null)).ToString();
                dt.Rows.Add(dr);
            }
            ddl.DataSource = dt;
            ddl.DataValueField = "Value";
            ddl.DataTextField = "Text";
            ddl.DataBind();

            ddl.Items.Insert(0, new System.Web.UI.WebControls.ListItem("--请选择--", "0"));
        }



        /// <summary>
        /// 获得对象的Updatesql语句(只能用在对象属性名称和相对应的表的列一样)
        /// </summary>
        /// <param name="obj">对象</param>
        /// <param name="tableName">表名</param>
        /// <param name="identityColumnName">自增列名称</param>
        /// <param name="primaryColumnName">主键列名称(或者没有表中没有主键的外键列名称)</param>
        /// <returns></returns>
        public static string GetModelUpdateString(object obj, string tableName,string identityColumnName,string primaryColumnName)
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendFormat("update {0}", tableName);
            List<PropertyInfo> propList = GetModelPropertyInfo(obj);
            sb.Append(" set ");
            string whereSql = string.Empty;
            for (int i = 0; i < propList.Count; i++)
            {
                if (propList[i].Name.ToUpper() == identityColumnName.ToUpper())
                {

                }
                else
                {
                    if (!propList[i].PropertyType.IsClass && !propList[i].PropertyType.IsInterface)
                    {
                        if (propList[i].PropertyType.IsEnum == true)
                        {
                            sb.AppendFormat("{0}='{1}',", propList[i].Name, (int)(propList[i].GetValue(obj, null)));
                        }
                        else
                        {
                            if (propList[i].GetValue(obj, null) == null)
                            {

                            }
                            else
                            {
                                sb.AppendFormat("{0}='{1}',", propList[i].Name, propList[i].GetValue(obj, null));
                            }
                        }
                    }
                }
                if (propList[i].Name.ToUpper() == primaryColumnName.ToUpper())
                {
                    whereSql = " where " + propList[i].Name + "='" + propList[i].GetValue(obj, null) + "'";
                }
            }
            sb.Remove(sb.Length - 1, 1);
            sb.Append(whereSql);
            return sb.ToString();
        }
    }

    /// <summary>
    /// 比较类
    /// </summary>
    /// <typeparam name="T">任意类型</typeparam>
    public class EqualityComparer<T> : IEqualityComparer<T>
    {
        /// <summary>
        /// 需要比较的属性名称
        /// </summary>
        public IList<string> ComparerString
        {
            get;
            set;
        }

        public EqualityComparer()
        {
            ComparerString = new List<string>();
        }

        public bool Equals(T a, T b)
        {
            if (object.ReferenceEquals(a, b))
                return true;
            if (object.ReferenceEquals(a, null) || object.ReferenceEquals(b, null))
                return false;

            if (ComparerString == null || ComparerString.Count <= 0)
            {
                bool isSame = false;
                foreach (PropertyInfo p in a.GetType().GetProperties())
                {
                    isSame = p.GetValue(a, null) == b.GetType().GetProperty(p.Name).GetValue(b, null);
                    if (!isSame)
                        return false;
                }
                return true;
            }
            else
            {
                foreach (string str in ComparerString)
                {
                    if (a.GetType().GetProperty(str).GetValue(a, null) != b.GetType().GetProperty(str).GetValue(b, null))
                        return false;
                }
                return true;
            }
        }

        public int GetHashCode(T o)
        {
            if (object.ReferenceEquals(o, null))
                return 0;
            int hashResult = 0;
            if (ComparerString == null || ComparerString.Count <= 0)
            {

                foreach (PropertyInfo p in o.GetType().GetProperties())
                {
                    hashResult = hashResult ^ (p.GetValue(o, null) != null ? p.GetValue(o, null).GetHashCode() : 0);
                }
                return hashResult;
            }
            else
            {
                foreach (string str in ComparerString)
                {
                    hashResult = hashResult ^ (o.GetType().GetProperty(str).GetValue(o, null) != null ? o.GetType().GetProperty(str).GetValue(o, null).GetHashCode() : 0);
                }
                return hashResult;
            }
        }
    }
}
