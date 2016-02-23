using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.Xml.Linq;
using EyouSoft.Common;

namespace Web.jidiaoCenter
{
    /// <summary>
    /// 创建人:刘飞
    /// 时间：2013-1-9
    /// 描述：线路修改
    /// </summary>
    public partial class RouteEdit : EyouSoft.Common.Page.BackPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            string routeid = Utils.GetQueryStringValue("routeid");
            string dotype = Utils.GetQueryStringValue("dotype");
            if (dotype == "save")
            {
                Response.Clear();
                Response.Write(PageSave(routeid));
                Response.End();
            }
            if (!IsPostBack)
            {
                PageInit(routeid);
            }
        }
        private void PageInit(string routeid)
        {
            if (routeid != "")
            {
                EyouSoft.BLL.TourStructure.BRoute bll = new EyouSoft.BLL.TourStructure.BRoute();
                EyouSoft.Model.TourStructure.MRoute model = bll.GetRoute(routeid);
                if (model != null)
                {
                    this.txtRouteName.Text = model.RouteName;
                    this.hidrouteid.Value = routeid;
                }
            }
        }
        private string PageSave(string routeid)
        {
            string msg = string.Empty;
            if (routeid != "")
            {
                EyouSoft.BLL.TourStructure.BRoute bll = new EyouSoft.BLL.TourStructure.BRoute();
                string routename = Utils.GetFormValue(this.txtRouteName.UniqueID);
                int flg = bll.Update(routename, routeid, SiteUserInfo.CompanyId);
                if (flg == 1)
                {
                    msg = UtilsCommons.AjaxReturnJson("1", "修改成功!");
                }
                else if (flg == -1)
                {
                    msg = UtilsCommons.AjaxReturnJson("0", "线路名称已存在!");
                }
                else
                {
                    msg = UtilsCommons.AjaxReturnJson("0", "修改失败!");
                }
            }
            return msg;
        }
    }
}
