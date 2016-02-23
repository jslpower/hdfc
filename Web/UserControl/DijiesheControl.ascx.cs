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
    public partial class DijiesheControl : System.Web.UI.UserControl
    {
        /// <summary>
        /// 设置控件的数据源
        /// </summary>
        private IList<MTourDiJie> _setTravelList;

        public IList<MTourDiJie> SetTravelList
        {
            get { return _setTravelList; }
            set { _setTravelList = value; }
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
            if (this.SetTravelList != null && this.SetTravelList.Count > 0)
            {
                this.rptList.DataSource = this.SetTravelList;
                this.rptList.DataBind();
                this.Ph_Dijie.Visible = false;
            }
            else
            {
                this.Ph_Dijie.Visible = true;
            }

        }
    }
}