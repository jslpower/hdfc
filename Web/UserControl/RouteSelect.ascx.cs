using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.ComponentModel;

namespace Web.UserControl
{
    /// <summary>
    /// 选用线路用户控件
    /// </summary>
    public partial class RouteSelect : System.Web.UI.UserControl
    {
        protected string NoticeHTML = "valid=\"required\"errmsg=\"线路不能为空！\"";
        /// <summary>
        /// 要初始化的线路Id
        /// </summary>
        [Bindable(true)]
        public string InitRouteId { get; set; }

        /// <summary>
        /// 要初始化的线路名称
        /// </summary>
        [Bindable(true)]
        public string InitRouteName { get; set; }

        /// <summary>
        /// 是否显示线路名称文字（线路名称：），默认不显示，只有input和选用图片
        /// </summary>
        [Bindable(true)]
        public bool IsShowTitle { get; set; }

        /// <summary>
        /// 是否多选，默认或者false  单选，true多选
        /// </summary>
        [Bindable(true)]
        public bool IsMoreSelect { get; set; }

        /// <summary>
        /// 是否必填
        /// </summary>
        [Bindable(true)]
        public bool IsMust { get; set; }

        /// <summary>
        /// 存放线路Id的隐藏域的客户端Id
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
        /// 存放线路名称的隐藏域的客户端Id
        /// </summary>
        [Bindable(true)]
        public string HidClientName
        {
            get
            {
                return string.Format("hid{0}Name", this.ClientID);
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            ltrRoute.Visible = IsShowTitle;
        }
    }
}