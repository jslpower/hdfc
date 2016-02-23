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
using EyouSoft.Model.CRM;

namespace Web.CustomerManage
{
    public partial class ZutuansheList : EyouSoft.Common.Page.BackPage
    {
        #region 分页参数
        protected int pageIndex = 1;
        protected int recordCount;
        protected int pageSize = 20;
        #endregion
        protected void Page_Load(object sender, EventArgs e)
        {
            PowerControl();
            if (!IsPostBack)
            {
                PageInit();
            }
        }
        private void PageInit()
        {
            EyouSoft.BLL.CRM.BContactBirthday bll = new EyouSoft.BLL.CRM.BContactBirthday();
            EyouSoft.Model.CRM.MSearchBirthday searchmodel = new EyouSoft.Model.CRM.MSearchBirthday();
            searchmodel.EndBirthday = Utils.GetDateTimeNullable(Utils.GetQueryStringValue("birthdayend"));
            searchmodel.Name = Utils.GetQueryStringValue("name");
            searchmodel.StartBirthday = Utils.GetDateTimeNullable(Utils.GetQueryStringValue("birthdaystart"));
            pageIndex = Utils.GetInt(Utils.GetQueryStringValue("Page"), 1);
            IList<MContactBirthday> list = bll.GetBirthdayList(SiteUserInfo.CompanyId, pageSize, pageIndex, ref recordCount, searchmodel);
            UtilsCommons.Paging(pageSize, ref pageIndex, recordCount);
            if (list != null && list.Count > 0)
            {
                this.RptList.DataSource = list;
                this.RptList.DataBind();
                BindExportPage();
            }
            else
            {
                lbemptymsg.Text = "<tr><td colspan='8' align='center' height='30px'>暂无数据!</td></tr>";
            }
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
            if (!this.CheckGrant(EyouSoft.Model.EnumType.PrivsStructure.Privs3.客户管理_生日中心_栏目))
            {
                Utils.ResponseNoPermit(EyouSoft.Model.EnumType.PrivsStructure.Privs3.客户管理_生日中心_栏目, false);
                return;
            }
        }
    }
}
