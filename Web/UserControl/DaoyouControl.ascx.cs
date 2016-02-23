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
using System.Collections.Generic;
using EyouSoft.Model.TourStructure;

namespace Web.UserControl
{
    public partial class DaoyouControl : System.Web.UI.UserControl
    {
        /// <summary>
        /// 设置控件的数据源
        /// </summary>
        private IList<MTourGuide> _setGuideList;

        public IList<MTourGuide> SetGuideList
        {
            get { return _setGuideList; }
            set { _setGuideList = value; }
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                SetDataList();
            }
        }

        /// <summary>
        /// 页面初始化时绑定数据
        /// </summary>
        private void SetDataList()
        {
            if (this.SetGuideList != null && this.SetGuideList.Count > 0)
            {
                this.rptlist.DataSource = this.SetGuideList;
                this.rptlist.DataBind();
                this.Ph_Daoyou.Visible = false;
            }
            else
            {
                this.Ph_Daoyou.Visible = true;
            }
        }
    }
}