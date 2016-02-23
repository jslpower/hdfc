using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;
using EyouSoft.Common;

namespace Web.UserControl
{
    public partial class TourSelect : System.Web.UI.UserControl
    {
        protected string NoticeHTML = "valid=\"required\"errmsg=\"团号不能为空！\"";

        /// <summary>
        /// 设置供应商类型
        /// </summary>
        private EyouSoft.Model.EnumType.CompanyStructure.SupplierType _supplierType;
        public EyouSoft.Model.EnumType.CompanyStructure.SupplierType SupplierType
        {
            get { return _supplierType; }
            set { _supplierType = value; }
        }
       
        /// <summary>
        /// TourId
        /// </summary>
        private string _tourid;
        /// <summary>
        /// 存放TourId
        /// </summary>
        public string TourId
        {
            get { return _tourid; }
            set { _tourid = value; }
        }
        /// <summary>
        /// 团号(tourcode)
        /// </summary>
        private string _tourcode;
        /// <summary>
        /// 团号(tourcode)
        /// </summary>
        public string TourCode
        {
            get { return _tourcode; }
            set { _tourcode = value; }
        }

        string _callback;
        /// <summary>
        /// 获取或设置回调函数方法名
        /// </summary>
        public string CallBack
        {
            get
            {
                if (string.IsNullOrEmpty(_callback)) return ClientID + "._callBack";

                return _callback;
            }
            set { _callback = value; }
        }

        /// <summary>
        /// 获取IframeID
        /// </summary>
        protected string IframeID
        {
            get { return Utils.GetQueryStringValue("iframeId"); }
        }
        /// <summary>
        /// 是否必选默认值
        /// </summary>
        private bool _ismust = true;
        /// <summary>
        /// 获取或设置是否必选（默认：true）
        /// </summary>
        public bool IsMust
        {
            get { return _ismust; }
            set { _ismust = value; }
        }
        private bool _iseEable = true;
        /// <summary>
        /// 是否可用(默认可用)
        /// </summary>
        public bool IsEnable
        {
            get { return _iseEable; }
            set { _iseEable = value; }
        }
        /// <summary>
        /// 获取选用按钮ClientID
        /// </summary>
        public string btnID { get { return "btn_" + this.ClientID + "_ID"; } }

        /// <summary>
        /// 获取团队编号ClientID
        /// </summary>
        public string ClientTourID { get { return "hd_" + this.ClientID + "_ID"; } }

        /// <summary>
        /// 获取团号ClientID
        /// </summary>
        public string ClientTourCode { get { return "txt_" + this.ClientID + "_Name"; } }
        protected void Page_Load(object sender, EventArgs e)
        {

        }
    }
}