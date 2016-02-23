using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using EyouSoft.Common;

namespace Web.Webmaster
{
    /// <summary>
    /// 子系统列表
    /// </summary>
    public partial class _systems : WebmasterPageBase
    {        
        #region attributes
        /// <summary>
        /// 每页显示记录数
        /// </summary>
        protected int PageSize = 20;
        /// <summary>
        /// 当前页索引
        /// </summary>
        protected int PageIndex = 1;
        #endregion

        protected void Page_Load(object sender, EventArgs e)
        {
            if (Utils.GetQueryStringValue("dotype") == "logout") { Logout(); return; }

            PageIndex = Utils.GetInt(Utils.GetQueryStringValue("page"), 1);
            int recordCount = 0;

            IList<EyouSoft.Model.SysStructure.MSysInfo> items = new EyouSoft.BLL.SysStructure.BSys().GetSyss(PageSize, PageIndex, ref recordCount, null);

            if (items != null && items.Count > 0)
            {
                this.rptSys.DataSource = items;
                this.rptSys.DataBind();
                this.phNotFound.Visible = false;

                RegisterScript(string.Format("pConfig.pageSize={0};pConfig.pageIndex={1};pConfig.recordCount={2};", PageSize, PageIndex, recordCount));
            }
            else
            {
                this.phNotFound.Visible = true;
            }
        }

        #region protected members
        /// <summary>
        /// rptSys_ItemDataBound
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void rptSys_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemIndex == -1) return;
            Literal ltrHandles = (Literal)e.Item.FindControl("ltrHandles");
            if (ltrHandles == null) return;

            EyouSoft.Model.SysStructure.MSysInfo info= (EyouSoft.Model.SysStructure.MSysInfo)e.Item.DataItem;

            string s = string.Empty;

            if (info.OnlineStatus == EyouSoft.Model.EnumType.CompanyStructure.UserOnlineStatus.Online)
            {
                s += string.Format("<a href=\"systems.aspx?cid={0}&uid={1}&dotype=logout\">退出</a>&nbsp;", info.CompanyId, info.UserId);
            }

            ltrHandles.Text = s;
        }
        #endregion

        #region private members
        /// <summary>
        /// 强制退出
        /// </summary>
        void Logout()
        {
            EyouSoft.Security.Membership.UserProvider.Logout(Utils.GetInt(Utils.GetQueryStringValue("cid")), Utils.GetInt(Utils.GetQueryStringValue("uid")));
            RegisterAlertAndRedirectScript("已成功退出", "systems.aspx");
        }
        #endregion
    }
}
