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
using System.Collections.Generic;

namespace Web.jidiaoCenter
{
    public partial class AjaxSelectTourRequest : EyouSoft.Common.Page.BackPage
    {
        protected int recordcount = 0;
        protected void Page_Load(object sender, EventArgs e)
        {
            string tourcode = Utils.GetQueryStringValue("tourcode");
            string type = Utils.GetQueryStringValue("type");
            if (!IsPostBack)
            {
                PageInit(tourcode, type);
            }
        }
        /// <summary>
        /// 初始化/搜索
        /// </summary>
        /// <param name="tourcode"></param>
        private void PageInit(string tourcode, string type)
        {
            EyouSoft.BLL.TourStructure.BTour bll = new EyouSoft.BLL.TourStructure.BTour();
            if (type == "地接")
            {
                // int[] param = new int[] { (int)EyouSoft.Model.EnumType.TourStructure.TourStatus.未处理 };
                IList<EyouSoft.Model.TourStructure.MTour> list = bll.GetList(SiteUserInfo.CompanyId, tourcode);
                if (list != null && list.Count > 0)
                {
                    recordcount = list.Count;
                    this.RptList.DataSource = list;
                    this.RptList.DataBind();
                }
            }
            else if (type == "票务")
            {
                // int[] param = new int[] { (int)EyouSoft.Model.EnumType.TourStructure.TourStatus.未处理 };
                IList<EyouSoft.Model.TourStructure.MTour> list = bll.GetList(SiteUserInfo.CompanyId, tourcode);
                if (list != null && list.Count > 0)
                {
                    recordcount = list.Count;
                    this.RptList.DataSource = list;
                    this.RptList.DataBind();
                }
            }
            else
            {
                IList<EyouSoft.Model.TourStructure.MTour> list = bll.GetList(SiteUserInfo.CompanyId, tourcode);
                if (list != null && list.Count > 0)
                {
                    recordcount = list.Count;
                    this.RptList.DataSource = list;
                    this.RptList.DataBind();
                }
            }
        }
    }
}
