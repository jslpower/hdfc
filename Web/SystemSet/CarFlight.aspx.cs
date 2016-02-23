using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using EyouSoft.Common;

namespace Web.SystemSet
{
    /// <summary>
    /// 车次/航班 管理
    /// </summary>
    public partial class CarFlight : EyouSoft.Common.Page.BackPage
    {
        /// <summary>
        /// 页记录数
        /// </summary>
        private const int PageSize = 10;
        /// <summary>
        /// 当前页数
        /// </summary>
        private int _pageIndex = 1;
        /// <summary>
        /// 总记录数
        /// </summary>
        private int _recordCount = 0;

        protected void Page_Load(object sender, EventArgs e)
        {
            string action = Utils.GetQueryStringValue("action").ToLower();
            if (action == "del")
            {
                this.RCWE(this.DeleteCarAndFlight(Utils.GetInt(Utils.GetQueryStringValue("cid"))));
            }

            if (!IsPostBack)
            {
                this.InitPage();
            }
        }

        private string DeleteCarAndFlight(int id)
        {
            if (id <= 0) return UtilsCommons.AjaxReturnJson("0", "url错误，请从新打开此窗口！");

            var r = new EyouSoft.BLL.CompanyStructure.BCompanyTicket().Delete(id);

            return UtilsCommons.AjaxReturnJson(r ? "1" : "0", string.Format("删除{0}！", r ? "成功" : "失败"));
        }

        private void InitPage()
        {
            var search = new EyouSoft.Model.CompanyStructure.MCompanyTicketSearch
                {
                    TrafficNumber = Utils.GetQueryStringValue("cf")
                };

            _pageIndex = Utils.GetInt(Utils.GetQueryStringValue("Page"), 1);
            int at = Utils.GetInt(Utils.GetQueryStringValue("at"), -1);
            if (at >= 0)
            {
                search.TicketType = (EyouSoft.Model.EnumType.PlanStructure.TicketType)at;
            }
            else
            {
                search.TicketType = null;
            }

            var list = new EyouSoft.BLL.CompanyStructure.BCompanyTicket().GetList(
                this.SiteUserInfo.CompanyId, PageSize, _pageIndex, ref _recordCount, search);

            UtilsCommons.Paging(PageSize, ref _pageIndex, _recordCount);

            RptList.DataSource = list;
            RptList.DataBind();

            ExporPageInfoSelect1.intPageSize = PageSize;
            ExporPageInfoSelect1.intRecordCount = _recordCount;
            ExporPageInfoSelect1.CurrencyPage = _pageIndex;
        }

        /// <summary>
        /// 获取行序号
        /// </summary>
        /// <param name="index">行索引</param>
        /// <returns></returns>
        protected int GetLineIndex(int index)
        {
            return (_pageIndex - 1) * PageSize + index + 1;
        }
        protected string GetTrafficTime(object obj)
        {
            DateTime? time = (DateTime?)obj;
            string strtime = string.Empty;
            if (time.HasValue)
                strtime = time.Value.ToString("yyyy-MM-dd HH:mm");
            return strtime;
        }

        protected string GetTypeHtml()
        {
            var list = EnumObj.GetList(typeof(EyouSoft.Model.EnumType.PlanStructure.TicketType));

            if (list == null || !list.Any()) return string.Empty;

            var strHtml = new System.Text.StringBuilder("<option value=\"-1\">请选择</option>");
            foreach (var t in list)
            {
                if (t == null) continue;

                strHtml.AppendFormat(" <option value=\"{0}\">{1}</option> ", t.Value, t.Text);
            }

            return strHtml.ToString();
        }
    }
}
