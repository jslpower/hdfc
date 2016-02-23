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
using System.Collections.Generic;

namespace Web.jidiaoCenter
{
    /// <summary>
    /// 创建人：刘飞
    /// 时间：2012-12-28
    /// 描述：线路管理
    /// </summary>
    public partial class RouteList : EyouSoft.Common.Page.BackPage
    {
        #region 分页参数
        protected int pageIndex;
        protected int recordCount;
        protected int pageSize = 20;
        #endregion
        protected void Page_Load(object sender, EventArgs e)
        {
            PowerControl();
            string routeid = Utils.GetQueryStringValue("routeid");
            string dotype = Utils.GetQueryStringValue("dotype");
            if (dotype != "" && routeid.Length > 0)
            {
                AJAX(dotype, routeid);
            }
            if (!IsPostBack)
            {
                PageInit();
            }
        }
        private void PageInit()
        {
            string routename = Utils.GetQueryStringValue("routename");
            EyouSoft.BLL.TourStructure.BRoute bll = new EyouSoft.BLL.TourStructure.BRoute();
            IList<EyouSoft.Model.TourStructure.MRoute> list = bll.GetList(this.SiteUserInfo.CompanyId, pageSize, pageIndex, ref recordCount, routename);
            if (list != null && list.Count > 0)
            {
                this.RptList.DataSource = list;
                this.RptList.DataBind();
                BindExportPage();
            }
            else
            {
                lbemptymsg.Text = "<tr><td colspan='3' align='center' height='30px'>暂无数据!</td></tr>";
            }
        }

        /// <summary>
        /// ajax操作
        /// </summary>
        private void AJAX(string doType, string id)
        {
            string msg = string.Empty;
            //对应执行操作
            switch (doType.ToLower())
            {
                case "delete":
                    // 判断权限
                    if (this.CheckGrant(EyouSoft.Model.EnumType.PrivsStructure.Privs3.计调中心_线路管理_删除))
                    {
                        msg = this.DeleteData(id);
                    }
                    break;
            }
            //返回ajax操作结果
            Response.Clear();
            Response.Write(msg);
            Response.End();
        }
        private string DeleteData(string routeid)
        {
            string msg = "";
            if (!string.IsNullOrEmpty(routeid))
            {
                EyouSoft.BLL.TourStructure.BRoute bll = new EyouSoft.BLL.TourStructure.BRoute();
                if (bll.Delete(routeid))
                {
                    msg = UtilsCommons.AjaxReturnJson("1", "删除成功");
                }
                else
                {
                    msg = UtilsCommons.AjaxReturnJson("0", "删除失败!");
                }
            }
            return msg;
        }

        #region 绑定分页控件
        /// <summary>
        /// 绑定分页控件
        /// </summary>
        protected void BindExportPage()
        {
            this.ExporPageInfoSelect1.intPageSize = pageSize;
            this.ExporPageInfoSelect1.CurrencyPage = pageIndex;
            this.ExporPageInfoSelect1.intRecordCount = recordCount;
        }
        #endregion

        /// <summary>
        /// 权限判断
        /// </summary>
        private void PowerControl()
        {
            if (!this.CheckGrant(EyouSoft.Model.EnumType.PrivsStructure.Privs3.计调中心_线路管理_栏目))
            {
                Utils.ResponseNoPermit(EyouSoft.Model.EnumType.PrivsStructure.Privs3.计调中心_线路管理_栏目, false);
                return;
            }
        }
    }
}
