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
    public partial class SupperControl : System.Web.UI.UserControl
    {
        /// <summary>
        /// 设置供应商类型
        /// </summary>
        private EyouSoft.Model.EnumType.CompanyStructure.SupplierType _supplierType;
        public EyouSoft.Model.EnumType.CompanyStructure.SupplierType SupplierType
        {
            get { return _supplierType; }
            set { _supplierType = value; }
        }
        protected string NoticeHTML = "valid=\"required\"errmsg=\"供应商不能为空！\"";
        /// <summary>
        /// 供应商ID
        /// </summary>
        private string _hideID;
        /// <summary>
        /// 存放供应商ID
        /// </summary>
        public string HideID
        {
            get { return _hideID; }
            set { _hideID = value; }
        }
        /// <summary>
        /// 供应商名称
        /// </summary>
        private string _name;
        /// <summary>
        /// 存放供应商名称
        /// </summary>
        public string Name
        {
            get { return _name; }
            set { _name = value; }
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
        private bool _ismust = false;
        /// <summary>
        /// 获取或设置是否必选（默认：false）
        /// </summary>
        public bool IsMust
        {
            get { return _ismust; }
            set { _ismust = value; }
        }
        private int _isAdd = 1;
        /// <summary>
        /// 控制是否显示添加栏目（0：不显示，1：显示）
        /// </summary>
        public int IsAdd
        {
            get { return _isAdd; }
            set { _isAdd = value; }
        }
        private int _alltype = 0;
        /// <summary>
        /// 是否显示所有供应商类型（0：不显示，1：显示）
        /// </summary>
        public int AllType
        {
            get { return _alltype; }
            set { _alltype = value; }
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
        /// 获取供应商编号ClientID
        /// </summary>
        public string ClientValue { get { return "hd_" + this.ClientID + "_ID"; } }

        /// <summary>
        /// 获取供应商名称ClientID
        /// </summary>
        public string ClientText { get { return "txt_" + this.ClientID + "_Name"; } }
        protected void Page_Load(object sender, EventArgs e)
        {

        }
    }
}