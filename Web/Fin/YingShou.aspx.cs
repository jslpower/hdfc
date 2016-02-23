using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using EyouSoft.Common;
using EyouSoft.Model.TourStructure;

namespace Web.Fin
{
    /// <summary>
    /// 应收管理
    /// </summary>
    public partial class YingShou : EyouSoft.Common.Page.BackPage
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
                if (!this.CheckGrant(EyouSoft.Model.EnumType.PrivsStructure.Privs3.财务管理_应收管理_栏目))
                {
                    Utils.ResponseNoPermit(EyouSoft.Model.EnumType.PrivsStructure.Privs3.财务管理_应收管理_栏目, false);
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
                if (!this.CheckGrant(EyouSoft.Model.EnumType.PrivsStructure.Privs3.财务管理_应收管理_修改))
                {
                    plnEditTour.Visible = false;
                }

                InitPage();
            }
        }

        /// <summary>
        /// 初始化页面
        /// </summary>
        private void InitPage()
        {
            _pageIndex = Utils.GetInt(Utils.GetQueryStringValue("Page"), 1);

            var list = new EyouSoft.BLL.TourStructure.BTour().GetList(
                this.SiteUserInfo.CompanyId, PageSize, _pageIndex, ref _recordCount, this.GetSearchModel());

            UtilsCommons.Paging(PageSize, ref _pageIndex, _recordCount);

            rptTour.DataSource = list;
            rptTour.DataBind();

            page1.CurrencyPage = _pageIndex;
            page1.intPageSize = PageSize;
            page1.intRecordCount = _recordCount;
        }

        /// <summary>
        /// 获取查询实体
        /// </summary>
        /// <returns></returns>
        private EyouSoft.Model.TourStructure.MSearchTour GetSearchModel()
        {
            var search = new EyouSoft.Model.TourStructure.MSearchTour
                {
                    IssueBeginDate = Utils.GetDateTimeNullable(Utils.GetQueryStringValue("ost")),
                    IssueEndDate = Utils.GetDateTimeNullable(Utils.GetQueryStringValue("oet")),
                    DiJieShe = Utils.GetQueryStringValue("dName"),
                    LBeginDate = Utils.GetDateTimeNullable(Utils.GetQueryStringValue("lst")),
                    LEndDate = Utils.GetDateTimeNullable(Utils.GetQueryStringValue("let")),
                    Planer = Utils.GetQueryStringValue("oName"),
                    RouteName = Utils.GetQueryStringValue("rName"),
                    SaleName = Utils.GetQueryStringValue("sName"),
                    TourCode = Utils.GetQueryStringValue("tno"),
                    ZuTuanShe = Utils.GetQueryStringValue("cName"),
                    OrderByTourState = 1
                };

            int yuejie = Utils.GetInt(Utils.GetQueryStringValue("yj"), -1);
            int chupiao = Utils.GetInt(Utils.GetQueryStringValue("cp"), -1);
            int shouqing = Utils.GetInt(Utils.GetQueryStringValue("sq"), -1);
            int js = Utils.GetInt(Utils.GetQueryStringValue("js"), -1);
            int tty = Utils.GetInt(Utils.GetQueryStringValue("tty"), -1);

            if (yuejie >= 0)
            {
                search.IsMonth = yuejie == 1;
            }
            else
            {
                search.IsMonth = null;
            }
            if (shouqing >= 0)
            {
                search.IsClean = shouqing == 1;
            }
            else
            {
                search.IsClean = null;
            }

            if (chupiao >= 0)
            {
                search.IsChuPiao = chupiao == 1;
            }
            else
            {
                search.IsChuPiao = null;
            }
            if (js >= 0)
            {
                search.IsEnd = js == 1;
            }
            else
            {
                search.IsEnd = null;
            }
            if (tty >= 0)
            {
                search.TourType = (EyouSoft.Model.EnumType.TourStructure.TourType)tty;
            }
            else
            {
                search.TourType = null;
            }

            //列表默认隐藏已收清的，查询后显示全部
            bool tmp = search.IssueBeginDate.HasValue || search.IssueEndDate.HasValue || search.LBeginDate.HasValue
                       || search.LEndDate.HasValue || !string.IsNullOrEmpty(search.DiJieShe)
                       || !string.IsNullOrEmpty(search.Planer) || !string.IsNullOrEmpty(search.RouteName)
                       || !string.IsNullOrEmpty(search.SaleName);
            bool tmp1 = !string.IsNullOrEmpty(search.TourCode) || !string.IsNullOrEmpty(search.ZuTuanShe)
                        || search.IsMonth.HasValue || search.IsChuPiao.HasValue || search.IsClean.HasValue
                        || search.IsEnd.HasValue || search.TourType.HasValue;
            if (tmp || tmp1)
            {

            }
            else
            {
                search.IsClean = false;
            }

            return search;
        }

        #region 前台函数

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
        /// 绑定团队类型
        /// </summary>
        /// <returns></returns>
        protected string GetTourTypeOption()
        {
            var list = EnumObj.GetList(typeof(EyouSoft.Model.EnumType.TourStructure.TourType));

            if (list == null || !list.Any()) return string.Empty;

            var strHtml = new System.Text.StringBuilder();

            foreach (var t in list)
            {
                if (t == null) continue;

                strHtml.AppendFormat("<option value=\"{0}\">{1}</option>", t.Value, t.Text);
            }

            return strHtml.ToString();
        }

        /// <summary>
        /// 获取取消团队的行背景颜色
        /// </summary>
        /// <param name="tourState">团队状态</param>
        /// <returns></returns>
        protected string GetTrBgColorByTourState(object tourState)
        {
            if (tourState == null) return string.Empty;

            var t = (EyouSoft.Model.EnumType.TourStructure.TourStatus)tourState;

            if (t == EyouSoft.Model.EnumType.TourStructure.TourStatus.已取消) return "huise";

            return string.Empty;
        }

        /// <summary>
        /// 团号日期加粗
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        protected string SetDayWeight(object obj)
        {
            if (obj == null) return string.Empty;

            string tourcode = obj.ToString();
            string start;
            string center;
            string end;
            if (tourcode.Length >= 6)
            {
                start = tourcode.Substring(0, 2);
                center = "<font class='font-weight'>" + tourcode.Substring(2, 4) + "</font>";
                end = tourcode.Substring(6);
            }
            else
            {
                return tourcode;
            }
            return start + center + end;
        }

        /// <summary>
        /// 获取团的状态
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        protected string GetTourStatus(object obj)
        {
            if (obj == null) return string.Empty;

            var tourstatus = (EyouSoft.Model.EnumType.TourStructure.TourStatus)obj;
            string str = string.Empty;
            switch (tourstatus)
            {
                case EyouSoft.Model.EnumType.TourStructure.TourStatus.未处理:
                    str = "<img alt='未处理' width='44' height='16' title='未处理' src='/images/wchuli.gif' alt='' />";
                    break;
                case EyouSoft.Model.EnumType.TourStructure.TourStatus.未出发:
                    str = "<img alt='未出发' width='44' height='16' title='未出发' src='/images/wchufa.gif' alt='' />";
                    break;
                case EyouSoft.Model.EnumType.TourStructure.TourStatus.行程中:
                    str = "<img alt='行程中' width='44' height='16' title='行程中' src='/images/xchzh.gif' alt='' />";
                    break;
                case EyouSoft.Model.EnumType.TourStructure.TourStatus.已回团:
                    str = "<img alt='已回团' width='44' height='16' title='已回团' src='/images/yhuit.gif' alt='' />";
                    break;
                case EyouSoft.Model.EnumType.TourStructure.TourStatus.已取消:
                    str = "<img width='44' height='16' title='已取消' src='/images/yiquxiao.gif' alt='已取消' />";
                    break;
                default:
                    break;
            }

            return str;
        }

        /// <summary>
        /// 获取组团社计调信息
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        protected string GetZuTuan(object obj)
        {
            string strDefault = "<table cellspacing='0' cellpadding='0' border='0' width='100%' class='pp-tableclass'><tr class='pp-table-title'><th>计调</th><th>电话</th><th >手机</th><th>生日</th></tr></table>";

            if (obj == null) return strDefault;

            var list = (IList<EyouSoft.Model.CRM.MCustomerContact>)obj;
            if (!list.Any()) return strDefault;

            var str = new System.Text.StringBuilder();
            str.Append("<table cellspacing='0' cellpadding='0' border='0' width='100%' class='pp-tableclass'><tr class='pp-table-title'><th>计调</th><th>电话</th><th >手机</th><th>生日</th></tr>");
            foreach (var t in list)
            {
                if (t == null) continue;

                str.AppendFormat(
                    "<tr><td>{0}</td><td>{1}</td><td>{2}</td><td>{3}</td></tr>",
                    t.Name,
                    t.Tel,
                    t.Mobile,
                    t.BirthDay.HasValue ? this.ToDateTimeString(t.BirthDay.Value) : string.Empty);
            }
            str.Append("</table>");
            return str.ToString();
        }

        /// <summary>
        /// 获取出票人信息
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        protected string GetTicketInfo(object obj)
        {
            IList<EyouSoft.Model.PlanStructure.MPlanTicket> list = (IList<EyouSoft.Model.PlanStructure.MPlanTicket>)obj;
            System.Text.StringBuilder str = new System.Text.StringBuilder();
            str.Append("<table cellspacing='0' cellpadding='0' border='0' width='100%' class='pp-tableclass'><tr class='pp-table-title'><th>出票点</th><th>区间</th><th>时间</th><th>班次</th></tr>");
            if (list != null && list.Count > 0)
            {
                for (int i = 0; i < list.Count; i++)
                {
                    str.AppendFormat(
                        "<tr><td><a class=\"ticketGYS\" data-gysid=\"{4}\" href=\"javascript:;\" onclick=\"jipiaoClick(this);return false;\" >{0}</a></td><td>{1}</td><td>{2}</td><td>{3}</td></tr>",
                        list[i].GysName,
                        list[i].Interval,
                        list[i].TrafficTime.HasValue ? list[i].TrafficTime.Value.ToString("yyyy-MM-dd HH:mm") : "",
                        list[i].TrafficNumber,
                        list[i].GysId);
                }
            }
            str.Append("</table>");
            return str.ToString();
        }

        /// <summary>
        /// 获取地接信息
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        protected string GetDijieInfo(object obj)
        {
            IList<MTourDiJie> list = (IList<MTourDiJie>)obj;
            System.Text.StringBuilder str = new System.Text.StringBuilder();
            str.Append("<table cellspacing='0' cellpadding='0' border='0' width='100%' class='pp-tableclass'><tr class='pp-table-title'><th>地接名称</th><th>计调</th><th>电话</th><th>QQ</th></tr>");
            if (list != null && list.Count > 0)
            {
                for (int i = 0; i < list.Count; i++)
                {
                    str.AppendFormat(
                        "<tr><td><a class=\"dijieGYS\" data-gysid=\"{4}\" href=\"javascript:;\" onclick=\"dijieClick(this);return false;\" >{0}</a></td><td>{1}</td><td>{2}</td><td>{3}</td></tr>",
                        list[i].DiJieName,
                        list[i].Name,
                        list[i].Phone,
                        list[i].QQ,
                        list[i].DiJieId);
                }
            }
            str.Append("</table>");
            return str.ToString();
        }

        /// <summary>
        /// 获取导游信息
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        protected string GetGuideInfo(object obj)
        {
            string strDefault = "<table cellspacing='0' cellpadding='0' border='0' width='100%' class='pp-tableclass'><tr class='pp-table-title'><th>导游姓名</th><th>电话</th></tr></table>";

            if (obj == null) return strDefault;
            var list = (IList<EyouSoft.Model.TourStructure.MTourGuide>)obj;
            if (!list.Any()) return strDefault;

            var str = new System.Text.StringBuilder();
            str.Append("<table cellspacing='0' cellpadding='0' border='0' width='100%' class='pp-tableclass'><tr class='pp-table-title'><th>导游姓名</th><th>电话</th></tr>");

            foreach (var t in list)
            {
                str.AppendFormat("<tr><td>{0}</td><td>{1}</td></tr>", t.GuideName, t.Phone);
            }
            str.Append("</table>");
            return str.ToString();
        }

        /// <summary>
        /// 获取收款信息/月结
        /// </summary>
        /// <param name="obj">收款信息集合</param>
        /// <param name="isMonth">是否月结</param>
        /// <param name="time">月结时间</param>
        /// <returns></returns>
        protected string GetPayInfo(object obj, object isMonth, object time)
        {
            if (isMonth == null) return string.Empty;

            var str = new System.Text.StringBuilder();
            var clear = (bool)isMonth;
            if (!clear)
            {
                string strDefault = "<table cellspacing='0' cellpadding='0' border='0' width='100%' class='pp-tableclass'><tr class='pp-table-title'><th>已收款</th><th>收款账号</th><th>收款日期</th><th>收款人</th><th>是否开票</th></tr></table>";
                if (obj == null) return strDefault;
                var list = (IList<EyouSoft.Model.FinStructure.MShouKuan>)obj;
                if (!list.Any()) return strDefault;

                str.Append("<table cellspacing='0' cellpadding='0' border='0' width='100%' class='pp-tableclass'><tr class='pp-table-title'><th>已收款</th><th>收款账号</th><th>收款日期</th><th>收款人</th><th>是否开票</th></tr>");

                foreach (var t in list)
                {
                    str.AppendFormat(
                        "<tr><td>{0}</td><td>{1}</td><td>{2}</td><td>{3}</td><td>{4}</td></tr>",
                        this.ToMoneyString(t.JinE),
                        t.ZhangHuCode,
                        this.ToDateTimeString(t.ShouKuanRiQi),
                        t.ShouKuanRenName,
                        t.IsKaiPiao ? "已开票" : "未开票");
                }
                str.Append("</table>");
            }
            else
            {
                str.AppendFormat(
                    "<table cellspacing='0' cellpadding='0' border='0' width='100%' class='pp-tableclass'><tr class='pp-table-title'><th>&nbsp;结算时间</th></tr><tr><td>{0}</td></tr></table>",
                    time == null ? string.Empty : Convert.ToDateTime(time).ToString("yyyy-MM-dd"));
            }
            return str.ToString();
        }

        /// <summary>
        /// 获取收款状态
        /// </summary>
        /// <param name="sumPrice">应收金额</param>
        /// <param name="checkMoney">已收已审核金额</param>
        /// <param name="isMonth">是否月结</param>
        /// <returns></returns>
        protected string GetShouKuanState(object sumPrice, object checkMoney, object isMonth)
        {
            var str = new System.Text.StringBuilder();
            var ism = (bool)isMonth;
            if (sumPrice != null && checkMoney != null)
            {
                str.Append(ism ? "月结" : this.ToMoneyString(checkMoney));
                if (Utils.GetDecimal(sumPrice.ToString()) != 0 && Utils.GetDecimal(sumPrice.ToString()) == Utils.GetDecimal(checkMoney.ToString()))
                {
                    str.Append("<img title=\"已收清\" src=\"/images/yshouq.gif\">");
                }
                else
                {
                    str.Append("<img title=\"未收清\" src=\"/images/wshq.gif\">");
                }
            }

            return str.ToString();
        }


        #endregion
    }
}
