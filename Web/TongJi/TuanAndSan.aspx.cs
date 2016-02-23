using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using EyouSoft.Common;

namespace Web.TongJi
{
    /// <summary>
    /// 团散统计
    /// </summary>
    public partial class TuanAndSan : EyouSoft.Common.Page.BackPage
    {
        /// <summary>
        /// 页记录数
        /// </summary>
        private const int PageSize = 15;
        /// <summary>
        /// 当前页数
        /// </summary>
        private int _pageIndex = 1;
        /// <summary>
        /// 总记录数
        /// </summary>
        private int _recordCount = 0;

        /// <summary>
        /// 合计实体
        /// </summary>
        protected EyouSoft.Model.TongJiStructure.MTourAndSanHeJi HeJi = new EyouSoft.Model.TongJiStructure.MTourAndSanHeJi();

        /// <summary>
        /// 是否显示佣金
        /// </summary>
        protected bool IsShowYongJin = false;

        /// <summary>
        /// 是否显示毛利
        /// </summary>
        protected bool IsShowMaoLi = false;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (!this.CheckGrant(EyouSoft.Model.EnumType.PrivsStructure.Privs3.统计中心_团散统计_栏目))
                {
                    Utils.ResponseNoPermit(EyouSoft.Model.EnumType.PrivsStructure.Privs3.统计中心_团散统计_栏目, false);
                    return;
                }

                if (this.CheckGrant(EyouSoft.Model.EnumType.PrivsStructure.Privs3.计调中心_确认件登记_返佣))
                {
                    IsShowYongJin = true;
                }
                if (this.CheckGrant(EyouSoft.Model.EnumType.PrivsStructure.Privs3.计调中心_确认件登记_毛利))
                {
                    IsShowMaoLi = true;
                }


                InitPage();
            }
        }

        /// <summary>
        /// 初始化页面
        /// </summary>
        private void InitPage()
        {
            var search = new EyouSoft.Model.TongJiStructure.MSearchTourAndSan();
            _pageIndex = Utils.GetInt(Utils.GetQueryStringValue("Page"), 1);
            int pid = Utils.GetInt(Utils.GetQueryStringValue("pid"));
            int cid = Utils.GetInt(Utils.GetQueryStringValue("cid"));
            int ts = Utils.GetInt(Utils.GetQueryStringValue("ts"), -1);
            if (pid > 0) search.ProvinceId = new int[] { pid };
            if (cid > 0) search.CityId = new int[] { cid };
            if (ts == 0 || ts == 1) search.TourType = (EyouSoft.Model.EnumType.TourStructure.TourType)ts;
            else search.TourType = null;
            search.StartLeaveDate = Utils.GetDateTimeNullable(Utils.GetQueryStringValue("lst"));
            search.EndLeaveDate = Utils.GetDateTimeNullable(Utils.GetQueryStringValue("let"));
            search.StartOrderDate = Utils.GetDateTimeNullable(Utils.GetQueryStringValue("ost"));
            search.EndOrderDate = Utils.GetDateTimeNullable(Utils.GetQueryStringValue("oet"));
            search.CustomerName = Utils.GetQueryStringValue("cName");
            search.OperatorName = Utils.GetQueryStringValue("oName");
            search.SaleName = Utils.GetQueryStringValue("sName");
            var list = new EyouSoft.BLL.TongJiStructure.BTongJi().GetTourAndSan(
                CurrentUserCompanyID, PageSize, _pageIndex, ref _recordCount, search, HeJi);
            UtilsCommons.Paging(PageSize, ref _pageIndex, _recordCount);

            rptTuanAndSan.DataSource = list;
            rptTuanAndSan.DataBind();

            page1.intPageSize = PageSize;
            page1.intRecordCount = _recordCount;
            page1.CurrencyPage = _pageIndex;
        }

        #region 前台函数

        /// <summary>
        /// 获取团散下拉框的item
        /// </summary>
        /// <returns></returns>
        protected string GetSelectHtml()
        {
            var strHtml = new System.Text.StringBuilder("<option value=\"-1\">请选择</option>");
            strHtml.AppendLine();
            var list = EnumObj.GetList(typeof(EyouSoft.Model.EnumType.TourStructure.TourType));
            if (list != null && list.Any())
            {
                foreach (var t in list)
                {
                    strHtml.AppendFormat(" <option value=\"{0}\">{1}</option> ", t.Value, t.Text);
                    strHtml.AppendLine();
                }
            }

            return strHtml.ToString();
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

        /// <summary>
        /// 获取大交通浮动
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        protected string GetDaJiaoTongFuDong(object obj)
        {
            if (obj == null) return string.Empty;
            var list = (IList<EyouSoft.Model.PlanStructure.MPlanTicketFloat>)obj;
            if (!list.Any()) return string.Empty;

            var strHtml = new System.Text.StringBuilder();
            strHtml.AppendLine(
                "<table cellspacing='0' cellpadding='0' border='0' width='100%' class='pp-tableclass'>");
            strHtml.AppendLine("<tr class='pp-table-title'><th>区间</th><th>时间</th><th>车次/航班</th></tr>");
            foreach (var t in list)
            {
                if (t == null) continue;

                strHtml.AppendFormat(
                    "<tr><td>{0}</td><td>{1}</td><td>{2}</td></tr>",
                    t.Interval,
                    this.ToDateTimeString(t.TrafficTime),
                    t.TrafficNumber);
                strHtml.AppendLine();
            }
            strHtml.Append("</table>");

            return strHtml.ToString();
        }

        #endregion
    }
}
