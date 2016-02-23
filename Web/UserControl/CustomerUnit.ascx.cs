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
using System.ComponentModel;

namespace Web.UserControl
{
    public partial class CustomerUnit : System.Web.UI.UserControl
    {
        #region 属性

        /// <summary>
        /// iframeId
        /// </summary>
        [Bindable(true)]
        public string ParentIframeId
        {
            get
            {
                return EyouSoft.Common.Utils.GetQueryStringValue("iframeId");
            }
        }

        /// <summary>
        /// 初始化客户单位编号
        /// </summary>
        [Bindable(true)]
        public string InitCustomerId { get; set; }

        /// <summary>
        /// 初始化客户单位名称
        /// </summary>
        [Bindable(true)]
        public string InitCustomerName { get; set; }

        /// <summary>
        /// 是否多选(false单选；true多选，默认false)
        /// </summary>
        [Bindable(true)]
        public bool IsMoreSelect { get; set; }

        /// <summary>
        /// 存放选中客户单位的隐藏域的客户端Id
        /// </summary>
        [Bindable(true)]
        public string HidClientId
        {
            get
            {
                return string.Format("hid{0}Id", this.ClientID);
            }
        }

        /// <summary>
        /// 存放选中客户单位的隐藏域的客户端Name
        /// </summary>
        [Bindable(true)]
        public string HidClientName
        {
            get
            {
                return string.Format("hid{0}Name", this.ClientID);
            }
        }

        /// <summary>
        /// 要初始化的客户联系人控件的客户端Id(这里只实现了下拉框的初始化)
        /// </summary>
        public string CustomerContactControlClientId { get; set; }

        /// <summary>
        /// 存放选中客户单位的联系人信息的隐藏域的客户端Id
        /// </summary>
        [Bindable(true)]
        public string HidContactClientId
        {
            get
            {
                return string.Format("hidContact{0}Id", this.ClientID);
            }
        }

        /// <summary>
        /// 存放选中客户单位的联系人信息的隐藏域的客户端Name
        /// </summary>
        [Bindable(true)]
        public string HidContactClientName
        {
            get
            {
                return string.Format("hidContact{0}Name", this.ClientID);
            }
        }

        /// <summary>
        /// 存放选中客户单位的非主要联系人信息的隐藏域的客户端Id
        /// </summary>
        [Bindable(true)]
        public string HidNoContactClientId
        {
            get
            {
                return string.Format("hidNoContact{0}Id", this.ClientID);
            }
        }

        /// <summary>
        /// 存放选中客户单位的非主要联系人信息的隐藏域的客户端Name
        /// </summary>
        [Bindable(true)]
        public string HidNoContactClientName
        {
            get
            {
                return string.Format("hidNoContact{0}Name", this.ClientID);
            }
        }

        bool _isrequired = true;
        /// <summary>
        /// 是否必填
        /// </summary>
        public bool IsRequired
        {
            get { return _isrequired; }
            set { _isrequired = value; }
        }
        #endregion

        private void Page_Load(object sender, EventArgs e)
        {

        }
    }
}