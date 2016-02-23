using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.Common;
using Microsoft.Practices.EnterpriseLibrary.Data;
using EyouSoft.Toolkit.DAL;
using System.Data;

namespace EyouSoft.DAL.BackgroundServices
{
    /// <summary>
    /// 客户关怀定时短信服务数据访问类
    /// </summary>
    public class CaringSmsTimerServices : EyouSoft.Toolkit.DAL.DALBase, EyouSoft.IDAL.BackgroundServices.ICaringSmsTimerServices
    {
        private readonly Database _db = null;

        /// <summary>
        /// 构造函数
        /// </summary>
        public CaringSmsTimerServices()
        {
            _db = base.SystemStore;
        }

        #region ICaringSmsTimerServices 成员

        /// <summary>
        /// 获取固定时间发送的客户关怀计划
        /// </summary>
        /// <param name="Time">固定发送时间</param>
        /// <returns></returns>
        public IList<EyouSoft.Model.CompanyStructure.CustomerCareforSendInfo> GetTimeSend(DateTime Time)
        {
            DbCommand dc = _db.GetStoredProcCommand("proc_CustomerCarefor_GetWillSendList");
            _db.AddInParameter(dc, "IsTime", DbType.Int32, 1);
            _db.AddInParameter(dc, "CurrTime", DbType.DateTime, Time);
            _db.AddInParameter(dc, "FixTypes", DbType.String, DBNull.Value);

            IList<EyouSoft.Model.CompanyStructure.CustomerCareforSendInfo> list = new List<EyouSoft.Model.CompanyStructure.CustomerCareforSendInfo>();
            using (IDataReader dr = DbHelper.RunReaderProcedure(dc, _db))
            {
                EyouSoft.Model.CompanyStructure.CustomerCareforSendInfo tModel = null;
                while (dr.Read())
                {
                    tModel = new EyouSoft.Model.CompanyStructure.CustomerCareforSendInfo();

                    if (!dr.IsDBNull(dr.GetOrdinal("Id")))
                        tModel.CustomerCareforId = int.Parse(dr["Id"].ToString());
                    if (!dr.IsDBNull(dr.GetOrdinal("CompanyId")))
                        tModel.CompanyId = int.Parse(dr["CompanyId"].ToString());
                    tModel.MobileCode = dr["MobileCode"].ToString();
                    tModel.Content = dr["Content"].ToString();
                    if (!dr.IsDBNull(dr.GetOrdinal("ChannelId")))
                        tModel.ChannelId = int.Parse(dr["ChannelId"].ToString());
                    if (!dr.IsDBNull(dr.GetOrdinal("OperatorId")))
                        tModel.OperatorId = int.Parse(dr["OperatorId"].ToString());

                    IList<EyouSoft.Model.CompanyStructure.NameAndMobile> listName = new List<EyouSoft.Model.CompanyStructure.NameAndMobile>();
                    if (!dr.IsDBNull(dr.GetOrdinal("CustomerInfo")) && !string.IsNullOrEmpty(dr["CustomerInfo"].ToString()))
                        listName = listName.Union(this.GetNameAndMobileByXML(dr["CustomerInfo"].ToString(), "tcci", "Name", "Mobile")).ToList();
                    if (!dr.IsDBNull(dr.GetOrdinal("SupplierInfo")) && !string.IsNullOrEmpty(dr["SupplierInfo"].ToString()))
                        listName = listName.Union(this.GetNameAndMobileByXML(dr["SupplierInfo"].ToString(), "tsc", "ContactName", "ContactMobile")).ToList();
                    if (!dr.IsDBNull(dr.GetOrdinal("DepartmentInfo")) && !string.IsNullOrEmpty(dr["DepartmentInfo"].ToString()))
                        listName = listName.Union(this.GetNameAndMobileByXML(dr["DepartmentInfo"].ToString(), "tbl_CompanyUser", "ContactName", "ContactMobile")).ToList();

                    tModel.NameAndMobile = listName;

                    list.Add(tModel);
                }
            }

            //更新是否发送状态
            if (list != null && list.Count > 0)
            {
                foreach (var item in list)
                {
                    UpdateIsSeded(item.CustomerCareforId, true);
                }
            }

            return list;
        }

        /// <summary>
        /// 获取固定节假日发送的客户关怀计划
        /// </summary>
        /// <param name="FixType">节假日类型</param>
        /// <returns></returns>
        public IList<EyouSoft.Model.CompanyStructure.CustomerCareforSendInfo> GetFixTypeSend(params EyouSoft.Model.EnumType.CompanyStructure.CustomerCareForSendSpecialTime[] FixType)
        {
            if (FixType == null || FixType.Length <= 0)
                return null;

            string strFixTypes = string.Empty;
            foreach (EyouSoft.Model.EnumType.CompanyStructure.CustomerCareForSendSpecialTime t in FixType)
            {
                strFixTypes += ((int)t).ToString() + ",";
            }
            strFixTypes = strFixTypes.Trim(',');

            DbCommand dc = _db.GetStoredProcCommand("proc_CustomerCarefor_GetWillSendList");
            _db.AddInParameter(dc, "IsTime", DbType.Int32, 2);
            _db.AddInParameter(dc, "CurrTime", DbType.DateTime, DBNull.Value);
            _db.AddInParameter(dc, "FixTypes", DbType.String, strFixTypes);

            IList<EyouSoft.Model.CompanyStructure.CustomerCareforSendInfo> list = new List<EyouSoft.Model.CompanyStructure.CustomerCareforSendInfo>();
            using (IDataReader dr = DbHelper.RunReaderProcedure(dc, _db))
            {
                EyouSoft.Model.CompanyStructure.CustomerCareforSendInfo tModel = null;
                while (dr.Read())
                {
                    tModel = new EyouSoft.Model.CompanyStructure.CustomerCareforSendInfo();

                    if (!dr.IsDBNull(dr.GetOrdinal("Id")))
                        tModel.CustomerCareforId = int.Parse(dr["Id"].ToString());
                    if (!dr.IsDBNull(dr.GetOrdinal("CompanyId")))
                        tModel.CompanyId = int.Parse(dr["CompanyId"].ToString());
                    tModel.MobileCode = dr["MobileCode"].ToString();
                    tModel.Content = dr["Content"].ToString();
                    if (!dr.IsDBNull(dr.GetOrdinal("ChannelId")))
                        tModel.ChannelId = int.Parse(dr["ChannelId"].ToString());
                    if (!dr.IsDBNull(dr.GetOrdinal("OperatorId")))
                        tModel.OperatorId = int.Parse(dr["OperatorId"].ToString());

                    IList<EyouSoft.Model.CompanyStructure.NameAndMobile> listName = new List<EyouSoft.Model.CompanyStructure.NameAndMobile>();
                    if (!dr.IsDBNull(dr.GetOrdinal("CustomerInfo")) && !string.IsNullOrEmpty(dr["CustomerInfo"].ToString()))
                        listName = listName.Union(this.GetNameAndMobileByXML(dr["CustomerInfo"].ToString(), "tcci", "Name", "Mobile")).ToList();
                    if (!dr.IsDBNull(dr.GetOrdinal("SupplierInfo")) && !string.IsNullOrEmpty(dr["SupplierInfo"].ToString()))
                        listName = listName.Union(this.GetNameAndMobileByXML(dr["SupplierInfo"].ToString(), "tsc", "ContactName", "ContactMobile")).ToList();
                    if (!dr.IsDBNull(dr.GetOrdinal("DepartmentInfo")) && !string.IsNullOrEmpty(dr["DepartmentInfo"].ToString()))
                        listName = listName.Union(this.GetNameAndMobileByXML(dr["DepartmentInfo"].ToString(), "tbl_CompanyUser", "ContactName", "ContactMobile")).ToList();

                    tModel.NameAndMobile = listName;

                    if (!(string.IsNullOrEmpty(tModel.MobileCode) && tModel.NameAndMobile.Count <= 0))
                        list.Add(tModel);
                }
            }

            //更新是否发送状态
            if (list != null && list.Count > 0)
            {
                foreach (var item in list)
                {
                    UpdateIsSeded(item.CustomerCareforId, true);
                }
            }

            return list;
        }

        /// <summary>
        /// 更新当天是否已发送
        /// </summary>
        /// <param name="CustomerCareforId">客户关怀Id</param>
        /// <param name="IsSeded">当天是否已发送</param>
        /// <returns>返回1成功，其他失败</returns>
        public int UpdateIsSeded(int CustomerCareforId, bool IsSeded)
        {
            if (CustomerCareforId <= 0)
                return 0;

            string strSql = string.Format(" update SMS_CustomerCarefor set IsSeded = '{0}' where Id = {1} ", IsSeded ? "1" : "0", CustomerCareforId);

            DbCommand dc = _db.GetSqlStringCommand(strSql);

            return DbHelper.ExecuteSql(dc, _db) > 0 ? 1 : 0;
        }
        #endregion

        #region 私有函数

        /// <summary>
        /// 解析SqlXML获取人员信息
        /// </summary>
        /// <param name="strXML">SqlXML</param>
        /// <param name="strTableName">表名称</param>
        /// <param name="strNameValue">Name字段名称</param>
        /// <param name="strMobileValue">Mobile字段名称</param>
        /// <returns></returns>
        public IList<EyouSoft.Model.CompanyStructure.NameAndMobile> GetNameAndMobileByXML(string strXML, string strTableName, string strNameValue, string strMobileValue)
        {
            if (string.IsNullOrEmpty(strXML) || string.IsNullOrEmpty(strTableName) || string.IsNullOrEmpty(strNameValue) || string.IsNullOrEmpty(strMobileValue))
                return null;

            System.Xml.XmlAttributeCollection attList = null;
            System.Xml.XmlDocument xml = null;
            System.Xml.XmlNodeList xmlNodeList = null;
            xml = new System.Xml.XmlDocument();
            xml.LoadXml(strXML);
            xmlNodeList = xml.GetElementsByTagName(strTableName);
            if (xmlNodeList == null || xmlNodeList.Count <= 0)
                return null;

            List<Model.CompanyStructure.NameAndMobile> list = new List<Model.CompanyStructure.NameAndMobile>();
            Model.CompanyStructure.NameAndMobile model = null;
            foreach (System.Xml.XmlNode node in xmlNodeList)
            {
                attList = node.Attributes;
                if (attList != null && attList.Count > 0)
                {
                    if (attList[strMobileValue] == null || string.IsNullOrEmpty(attList[strMobileValue].ToString())) continue;
                    model = new EyouSoft.Model.CompanyStructure.NameAndMobile();
                    if (attList[strNameValue] != null && !string.IsNullOrEmpty(attList[strNameValue].Value))
                        model.ContactName = attList[strNameValue].Value;
                    if (attList[strMobileValue] != null)
                        model.ContactMobile = attList[strMobileValue].Value;

                    list.Add(model);
                }
            }
            xml = null;
            attList = null;
            xmlNodeList = null;

            return list;
        }

        #endregion
    }
}
