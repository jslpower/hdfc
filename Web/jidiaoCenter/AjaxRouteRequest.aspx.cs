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
using EyouSoft.Common;

namespace Web.jidiaoCenter
{
    public partial class AjaxRouteRequest : EyouSoft.Common.Page.BackPage
    {
        protected int recordcount = 0;
        protected int IsSelectMore = 0;
        protected void Page_Load(object sender, EventArgs e)
        {
            IsSelectMore = Utils.GetInt(Utils.GetQueryStringValue("isMore"));
            if (!IsPostBack)
            {
                InitPage();
            }
        }

        private void InitPage()
        {
            string rname = Utils.GetQueryStringValue("rname");
            int aid = Utils.GetInt(Utils.GetQueryStringValue("aid"));
            if (rname != "")
            {
                EyouSoft.BLL.TourStructure.BRoute bll = new EyouSoft.BLL.TourStructure.BRoute();
                IList<EyouSoft.Model.TourStructure.MRoute> list = bll.GetRouteList(SiteUserInfo.CompanyId, rname);
                if (list != null && list.Count > 0)
                {
                    recordcount = list.Count;
                    rptRoute.DataSource = list;
                    rptRoute.DataBind();
                }
            }
        }
        /// <summary>
        /// 生成选择框的html
        /// </summary>
        /// <param name="objId">编号</param>
        /// <param name="objName">名称</param>
        /// <param name="index">数据项索引</param>
        /// <returns></returns>
        protected string GetInputHtml(object objId, object objName, int index)
        {
            if (objId == null || objName == null || string.IsNullOrEmpty(objId.ToString())) return string.Empty;

            bool isCheck = false;

            string initId = Utils.GetQueryStringValue("initId");
            if (!string.IsNullOrEmpty(initId))
            {
                string[] tmpId = initId.Split(',');
                if (tmpId.Length > 0)
                {
                    isCheck = tmpId.Contains(objId);
                }
            }

            if (IsSelectMore == 1)
            {
                return
                    string.Format(
                        "<input type=\"checkbox\" name=\"ckbRoute\" {3} value=\"{0}\" data-name=\"{1}\" id=\"ckbRoute{2}\" /><label for=\"ckbRoute{2}\">{1}</label>",
                        objId.ToString(),
                        objName.ToString(),
                        index,
                        isCheck ? "checked" : string.Empty);
            }

            return
                string.Format(
                    "<input type=\"radio\" name=\"radRoute\" {3} value=\"{0}\" data-name=\"{1}\" id=\"radRoute{2}\" /><label for=\"radRoute{2}\">{1}</label>",
                    objId.ToString(),
                    objName.ToString(),
                    index,
                    isCheck ? "checked" : string.Empty);
        }

        /// <summary>
        /// 计算行索引
        /// </summary>
        /// <param name="index">循环项索引</param>
        /// <returns></returns>
        protected string GetTrHtml(int index)
        {
            if (index != 0 && index % 4 == 0)
            {
                string strHtm = "</tr><tr>";

                return string.Format(strHtm);
            }

            return string.Empty;
        }
    }
}
