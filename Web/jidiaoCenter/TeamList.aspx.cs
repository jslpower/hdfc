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
using EyouSoft.Model.TourStructure;

namespace Web.jidiaoCenter
{
    public partial class TeamList : EyouSoft.Common.Page.BackPage
    {
        #region 分页参数
        protected int pageIndex = 1;
        protected int recordCount;
        protected int pageSize = 20;
        #endregion

        protected bool isShoukuan = false;
        /// <summary>
        /// 是否显示佣金
        /// </summary>
        protected bool IsShowYongJin = false;

        /// <summary>
        /// 是否显示毛利
        /// </summary>
        protected bool IsShowMaoLi = false;
        /// <summary>
        /// 团队状态项
        /// </summary>
        protected string TourStatus = string.Empty;
        protected void Page_Load(object sender, EventArgs e)
        {
            string id = Utils.GetQueryStringValue("id");
            string dotype = Utils.GetQueryStringValue("dotype");
            PowerControl();
            if (dotype != null && dotype.Length > 0)
            {
                AJAX(dotype, id);
            }
            if (!IsPostBack)
            {
                if (this.CheckGrant(EyouSoft.Model.EnumType.PrivsStructure.Privs3.计调中心_确认件登记_返佣))
                {
                    IsShowYongJin = true;
                }
                if (this.CheckGrant(EyouSoft.Model.EnumType.PrivsStructure.Privs3.计调中心_确认件登记_毛利))
                {
                    IsShowMaoLi = true;
                }
                if (this.CheckGrant(EyouSoft.Model.EnumType.PrivsStructure.Privs3.计调中心_确认件登记_取消))
                {
                    plnQuXiao.Visible = true;
                }
                PageInit();
            }
        }
        private void PageInit()
        {
            string tourcode = Utils.GetQueryStringValue("tourcode");
            string routename = Utils.GetQueryStringValue("routename");
            string seller = Utils.GetQueryStringValue("seller");
            DateTime? leavetime = Utils.GetDateTimeNullable(Utils.GetQueryStringValue("leavetime"));
            DateTime? backtime = Utils.GetDateTimeNullable(Utils.GetQueryStringValue("backtime"));
            DateTime? starttime = Utils.GetDateTimeNullable(Utils.GetQueryStringValue("starttime"));
            DateTime? endtime = Utils.GetDateTimeNullable(Utils.GetQueryStringValue("endtime"));
            string planer = Utils.GetQueryStringValue("planer");
            string zutuanshe = Utils.GetQueryStringValue("zutuanshe");
            string dijieshe = Utils.GetQueryStringValue("dijieshe");
            string yuejie = Utils.GetQueryStringValue("yuejie");
            string chupiao = Utils.GetQueryStringValue("chupiao");
            string shouqing = Utils.GetQueryStringValue("shouqing");
            string type = Utils.GetQueryStringValue("type");
            string end = Utils.GetQueryStringValue("end");
            string tourstatus = Utils.GetQueryStringValue("tourstatus");

            EyouSoft.BLL.TourStructure.BTour bll = new EyouSoft.BLL.TourStructure.BTour();
            EyouSoft.Model.TourStructure.MSearchTour searchmodel = new EyouSoft.Model.TourStructure.MSearchTour();
            searchmodel.DiJieShe = dijieshe;
            if (chupiao == "1")
                searchmodel.IsChuPiao = true;
            if (chupiao == "0")
                searchmodel.IsChuPiao = false;
            if (shouqing == "1")
                searchmodel.IsClean = true;
            if (shouqing == "0")
                searchmodel.IsClean = false;
            if (yuejie == "1")
                searchmodel.IsMonth = true;
            if (yuejie == "0")
                searchmodel.IsMonth = false;
            if (end == "0")
                searchmodel.IsEnd = false;
            if (end == "1")
                searchmodel.IsEnd = true;
            searchmodel.IssueBeginDate = starttime;
            searchmodel.IssueEndDate = endtime;
            searchmodel.LBeginDate = leavetime;
            searchmodel.LEndDate = backtime;
            searchmodel.Planer = planer;
            searchmodel.RouteName = routename;
            searchmodel.SaleName = seller;
            searchmodel.TourCode = tourcode;
            searchmodel.ZuTuanShe = zutuanshe;
            if (tourstatus != "" && tourstatus != "-1")
            {
                searchmodel.TourStatus = (EyouSoft.Model.EnumType.TourStructure.TourStatus)Utils.GetInt(tourstatus);
            }

            if (type != "")
            {
                if (type.Trim() == "tuan")
                {
                    searchmodel.TourType = EyouSoft.Model.EnumType.TourStructure.TourType.团;
                }
                else
                {
                    searchmodel.TourType = EyouSoft.Model.EnumType.TourStructure.TourType.散;
                }
            }



            pageIndex = Utils.GetInt(Utils.GetQueryStringValue("Page"), 1);
            IList<EyouSoft.Model.TourStructure.MPageTour> list = bll.GetList(this.SiteUserInfo.CompanyId, pageSize, pageIndex, ref recordCount, searchmodel);
            UtilsCommons.Paging(pageSize, ref pageIndex, recordCount);
            if (list != null && list.Count > 0)
            {
                this.rptList.DataSource = list;
                this.rptList.DataBind();
                BindExportPage();
            }
            int tuancount = 0;
            int sancount = 0;
            bll.GetTodayTour(SiteUserInfo.CompanyId, ref sancount, ref tuancount);
            this.lbsancount.Text = sancount.ToString();
            this.lbtuancount.Text = tuancount.ToString();

            TourStatus = UtilsCommons.GetEnumDDL(EyouSoft.Common.EnumObj.GetList(typeof(EyouSoft.Model.EnumType.TourStructure.TourStatus)), tourstatus, true, "-1", "请选择");

        }
        /// <summary>
        /// ajax操作
        /// </summary>
        private void AJAX(string doType, string id)
        {
            string msg = UtilsCommons.AjaxReturnJson("0", "操作失败");
            //对应执行操作
            switch (doType.ToLower())
            {
                case "delete":
                    // 判断权限
                    if (this.CheckGrant(EyouSoft.Model.EnumType.PrivsStructure.Privs3.计调中心_确认件登记_删除))
                    {
                        msg = this.DeleteData(id);
                    }
                    break;
                case "forcedelete":
                    msg = this.ForceDeleteData(id);
                    break;
                case "quxiao":
                    msg = this.QuXiao(id);
                    break;
            }
            //返回ajax操作结果
            Response.Clear();
            Response.Write(msg);
            Response.End();
        }

        /// <summary>
        /// 取消确认件
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        private string QuXiao(string id)
        {
            if (string.IsNullOrEmpty(id)) return UtilsCommons.AjaxReturnJson("0", "url错误，刷新后重试！");

            int r = new EyouSoft.BLL.TourStructure.BTour().Update(id, EyouSoft.Model.EnumType.TourStructure.TourStatus.已取消);

            switch (r)
            {
                case -1:
                    return UtilsCommons.AjaxReturnJson("-1", "团队操作已结束，不能取消！");
                case 1:
                    return UtilsCommons.AjaxReturnJson("1", "取消成功！");
                default:
                    return UtilsCommons.AjaxReturnJson("0", "取消失败！");
            }
        }

        /// <summary>
        /// 删除操作
        /// </summary>
        /// <param name="id">删除ID</param>
        /// <returns></returns>
        private string DeleteData(string id)
        {
            string msg = string.Empty;
            if (!String.IsNullOrEmpty(id))
            {
                EyouSoft.BLL.TourStructure.BTour bll = new EyouSoft.BLL.TourStructure.BTour();
                int result = bll.Delete(id);
                switch (result)
                {
                    case 0:
                        msg = UtilsCommons.AjaxReturnJson("0", "删除失败");
                        break;
                    case 1:
                        msg = UtilsCommons.AjaxReturnJson("1", "删除成功");
                        break;
                    case -1:
                        msg = UtilsCommons.AjaxReturnJson("0", "做过地接安排不允许删除");
                        break;
                    case -2:
                        msg = UtilsCommons.AjaxReturnJson("0", "做过出票或退票安排不允许删除");
                        break;
                    case -3:
                        msg = UtilsCommons.AjaxReturnJson("0", "财务审核后不能删除");
                        break;
                    case -4:
                        msg = UtilsCommons.AjaxReturnJson("0", "操作结束不能删除");
                        break;
                    default:
                        msg = UtilsCommons.AjaxReturnJson("0", "无效的团队信息!");
                        break;
                }
            }
            return msg;
        }

        /// <summary>
        /// 强制删除确认件
        /// </summary>          
        /// <param name="id">确认件编号</param>
        /// <returns></returns>
        private string ForceDeleteData(string id)
        {
            string msg;
            if (string.IsNullOrEmpty(id))
            {
                msg = UtilsCommons.AjaxReturnJson("0", "无效的确认件信息！");
                return msg;
            }
            if (!this.CheckGrant(EyouSoft.Model.EnumType.PrivsStructure.Privs3.计调中心_确认件登记_强制删除))
            {
                msg = UtilsCommons.AjaxReturnJson(
                    "0",
                    string.Format("您没有{0}的权限，请联系管理员！", EyouSoft.Model.EnumType.PrivsStructure.Privs3.计调中心_确认件登记_强制删除));
                return msg;
            }

            var r = new EyouSoft.BLL.TourStructure.BTour().Delete_(id);
            switch (r)
            {
                case 0:
                    msg = UtilsCommons.AjaxReturnJson("0", "删除失败");
                    break;
                case 1:
                    msg = UtilsCommons.AjaxReturnJson("1", "删除成功");
                    break;
                default:
                    msg = UtilsCommons.AjaxReturnJson("0", "无效的团队信息!");
                    break;
            }

            return msg;
        }

        protected string SetDayWeight(object obj, object status)
        {
            EyouSoft.Model.EnumType.TourStructure.TourStatus _status = (EyouSoft.Model.EnumType.TourStructure.TourStatus)status;
            string tourcode = obj.ToString();
            string start = "";
            string center = "";
            string end = "";
            string TourStr = "";
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
            TourStr = start + center + end;
            if (_status == EyouSoft.Model.EnumType.TourStructure.TourStatus.已回团)
            {
                TourStr = "<span class='fred'>" + TourStr + "</span>";
            }
            else if (_status == EyouSoft.Model.EnumType.TourStructure.TourStatus.行程中)
            {
                TourStr = string.Format("<span style='color:Green'>{0}</span>", TourStr);
            }
            return TourStr;
        }

        protected string GetTourType(object obj)
        {
            EyouSoft.Model.EnumType.TourStructure.TourType type = (EyouSoft.Model.EnumType.TourStructure.TourType)obj;
            if (type == EyouSoft.Model.EnumType.TourStructure.TourType.团)
            {
                return "<font class='fred'>" + EyouSoft.Model.EnumType.TourStructure.TourType.团.ToString() + "</font>";
            }
            else
            {
                return EyouSoft.Model.EnumType.TourStructure.TourType.散.ToString();
            }
        }
        /// <summary>
        /// 获取团的状态
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        protected string GetTourStatus(object obj)
        {
            EyouSoft.Model.EnumType.TourStructure.TourStatus tourstatus = (EyouSoft.Model.EnumType.TourStructure.TourStatus)obj;
            string str = "";
            switch (tourstatus)
            {
                case EyouSoft.Model.EnumType.TourStructure.TourStatus.未处理:
                    str = "<img width='44' height='16' title='未处理' src='/images/wchuli.gif' alt='' />";
                    break;
                case EyouSoft.Model.EnumType.TourStructure.TourStatus.未出发:
                    str = "<img width='44' height='16' title='未出发' src='/images/wchufa.gif' alt='' />";
                    break;
                case EyouSoft.Model.EnumType.TourStructure.TourStatus.行程中:
                    str = "<img width='44' height='16' title='行程中' src='/images/xchzh.gif' alt='' />";
                    break;
                case EyouSoft.Model.EnumType.TourStructure.TourStatus.已回团:
                    str = "<img width='44' height='16' title='已回团' src='/images/yhuit.gif' alt='' />";
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
        /// 是否显示倒计时
        /// </summary>
        /// <param name="_leavedate"></param>
        /// <returns></returns>
        protected string Daojishi(object _leavedate)
        {
            DateTime leavedate = Convert.ToDateTime(_leavedate);
            TimeSpan ts = leavedate.Date - DateTime.Now.Date;
            if (ts.TotalDays >= 0 && ts.TotalDays <= 3)
            {
                return "<sup><font class='fred font14'>" + Utils.GetInt(ts.TotalDays.ToString()) + "</font></sup>";
            }
            else
            {
                return "";
            }
        }

        /// <summary>
        /// 获取团队信息
        /// </summary>
        /// <param name="_startDate"></param>
        /// <param name="_leavedate"></param>
        /// <param name="_backdate"></param>
        /// <returns></returns>
        protected string GetTourInfo(object _startDate, object _leavedate, object _backdate)
        {
            DateTime startDate = Convert.ToDateTime(_startDate);
            DateTime leaveDate = Convert.ToDateTime(_leavedate);
            DateTime backDate = Convert.ToDateTime(_backdate);
            System.Text.StringBuilder str = new System.Text.StringBuilder();
            str.AppendFormat("<table cellspacing='0' cellpadding='0' border='0' width='340px' class='pp-tableclass'><tr class='pp-table-title'><th>下单日期</th><th>出团日期</th><th>回团日期</th></tr><tr><td>{0}</td><td>{1}</td><td>{2}</td></tr></table>", startDate.ToString("yyyy-MM-dd"), leaveDate.ToString("yyyy-MM-dd"), backDate.ToString("yyyy-MM-dd"));
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
            IList<MTourGuide> list = (IList<MTourGuide>)obj;
            System.Text.StringBuilder str = new System.Text.StringBuilder();
            str.Append("<table cellspacing='0' cellpadding='0' border='0' width='100%' class='pp-tableclass'><tr class='pp-table-title'><th>导游姓名</th><th>电话</th></tr>");
            if (list != null && list.Count > 0)
            {
                for (int i = 0; i < list.Count; i++)
                {
                    str.AppendFormat("<tr><td>{0}</td><td>{1}</td></tr>", list[i].GuideName, list[i].Phone);
                }
            }
            str.Append("</table>");
            return str.ToString();
        }
        /// <summary>
        /// 获取组团社计调信息
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        protected string GetZuTuan(object obj)
        {
            IList<EyouSoft.Model.CRM.MCustomerContact> list = (IList<EyouSoft.Model.CRM.MCustomerContact>)obj;
            System.Text.StringBuilder str = new System.Text.StringBuilder();
            str.Append("<table cellspacing='0' cellpadding='0' border='0' width='100%' class='pp-tableclass'><tr class='pp-table-title'><th>计调</th><th>电话</th><th >手机</th><th>生日</th></tr>");
            if (list != null && list.Count > 0)
            {
                for (int i = 0; i < list.Count; i++)
                {
                    str.AppendFormat("<tr><td>{0}</td><td>{1}</td><td>{2}</td><td>{3}</td></tr>", list[i].Name, list[i].Tel, list[i].Mobile, list[i].BirthDay.HasValue ? Utils.GetDateTime(list[i].BirthDay.ToString()).ToString("yyyy-MM-dd") : "");
                }
            }
            str.Append("</table>");
            return str.ToString();
        }
        /// <summary>
        /// 获取收款信息/月结
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        protected string GetPayInfo(object obj, object _ismonth, object _time)
        {
            System.Text.StringBuilder str = new System.Text.StringBuilder();
            bool ismonth = (bool)_ismonth;
            DateTime time = Convert.ToDateTime(_time);
            if (!ismonth)
            {
                IList<EyouSoft.Model.FinStructure.MShouKuan> list = (IList<EyouSoft.Model.FinStructure.MShouKuan>)obj;
                str.Append("<table cellspacing='0' cellpadding='0' border='0' width='100%' class='pp-tableclass'><tr class='pp-table-title'><th>已收款</th><th>收款账号</th><th>收款日期</th><th>收款人</th><th>是否开票</th></tr>");
                if (list != null && list.Count > 0)
                {
                    for (int i = 0; i < list.Count; i++)
                    {
                        str.AppendFormat("<tr><td>{0}</td><td>{1}</td><td>{2}</td><td>{3}</td><td>{4}</td></tr>", list[i].JinE.ToString("f2"), list[i].ZhangHuCode, list[i].ShouKuanRiQi.ToString("yyyy-MM-dd"), list[i].ShouKuanRenName, list[i].IsKaiPiao ? "已开票" : "未开票");
                    }
                }
            }
            else
            {
                str.AppendFormat("<table cellspacing='0' cellpadding='0' border='0' width='100%' class='pp-tableclass'><tr class='pp-table-title'><th>结算时间</th></tr><tr><td>{0}</td></tr>", time.ToString("yyyy-MM-dd"));
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
            if (!this.CheckGrant(EyouSoft.Model.EnumType.PrivsStructure.Privs3.计调中心_确认件登记_栏目))
            {
                Utils.ResponseNoPermit(EyouSoft.Model.EnumType.PrivsStructure.Privs3.计调中心_确认件登记_栏目, false);
                return;
            }
            if (!this.CheckGrant(EyouSoft.Model.EnumType.PrivsStructure.Privs3.计调中心_确认件登记_强制删除))
            {
                phForceDel.Visible = false;
            }
            if (this.CheckGrant(EyouSoft.Model.EnumType.PrivsStructure.Privs3.财务管理_应收管理_收款新增))
            {
                this.isShoukuan = true;
            }
        }
    }
}
