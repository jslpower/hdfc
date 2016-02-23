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
using EyouSoft.Model.EnumType.PlanStructure;

namespace Web.jidiaoCenter
{
    /// <summary>
    /// 创建人： 刘飞
    /// 日期：2012-12-26
    /// 描述：票务安排列表
    /// </summary>
    public partial class TicketList : EyouSoft.Common.Page.BackPage
    {
        #region 分页参数
        protected int pageIndex;
        protected int recordCount;
        protected int pageSize = 20;
        #endregion
        protected string statusStr = string.Empty;
        protected string typeStr = string.Empty;

        Dictionary<int, string> bgCode = new Dictionary<int, string>();
        Dictionary<int, string> bgColor = new Dictionary<int, string>();

        protected void Page_Load(object sender, EventArgs e)
        {
            string id = Utils.GetQueryStringValue("id");
            string dotype = Utils.GetQueryStringValue("dotype");
            string mode = Utils.GetQueryStringValue("mode");
            PowerControl();
            if (dotype != "" && dotype.Length > 0)
            {
                AJAX(dotype, id, mode);
            }
            if (!IsPostBack)
            {
                PageInit();
            }
        }
        private void PageInit()
        {
            string tourcode = Utils.GetQueryStringValue("tourcode");
            DateTime? starttime = Utils.GetDateTimeNullable(Utils.GetQueryStringValue("starttime"));
            DateTime? endtime = Utils.GetDateTimeNullable(Utils.GetQueryStringValue("endtime"));
            string status = Utils.GetQueryStringValue("sltstatus");
            string type = Utils.GetQueryStringValue("slttype");
            string yuejie = Utils.GetQueryStringValue("yuejie");
            string ticketmodel = Utils.GetQueryStringValue("type");
            string ticketname = Utils.GetQueryStringValue("ticket");
            string tourtype = Utils.GetQueryStringValue("tourtype");
            EyouSoft.BLL.PlanStructure.BPlanTicket bll = new EyouSoft.BLL.PlanStructure.BPlanTicket();
            EyouSoft.Model.PlanStructure.MSearchTicket searchmodel = new EyouSoft.Model.PlanStructure.MSearchTicket();
            if (ticketmodel != "")
            {
                if (ticketmodel == "out")
                    searchmodel.TicketMode = EyouSoft.Model.EnumType.PlanStructure.TicketMode.出票;
                else
                    searchmodel.TicketMode = EyouSoft.Model.EnumType.PlanStructure.TicketMode.退票;
            }
            searchmodel.GysName = ticketname;
            if (tourtype != "" && tourtype != "-1")
            {
                searchmodel.TourType = (EyouSoft.Model.EnumType.TourStructure.TourType)Utils.GetInt(tourtype);
            }
            searchmodel.LBeginDate = starttime;
            searchmodel.LEndDate = endtime;
            if (status != "" && status != "-1")
            {
                searchmodel.TicketStatus = (EyouSoft.Model.EnumType.PlanStructure.TicketStatus)Utils.GetInt(status);
            }
            if (type != "" && type != "-1")
            {
                searchmodel.TicketType = (EyouSoft.Model.EnumType.PlanStructure.TicketType)Utils.GetInt(type);
            }
            if (yuejie == "1")
                searchmodel.IsMonth = true;
            if (yuejie == "0")
                searchmodel.IsMonth = false;
            searchmodel.TourCode = tourcode;
            pageIndex = Utils.GetInt(Utils.GetQueryStringValue("Page"), 1);
            int _comid = SiteUserInfo.CompanyId;
            if (SiteUserInfo.UserType == EyouSoft.Model.EnumType.CompanyStructure.UserType.票务用户)
            {
                searchmodel.GysId = SiteUserInfo.SupplierCompanyId;
            }
            decimal[] sum = new decimal[3];
            IList<EyouSoft.Model.PlanStructure.MPlanTicket> list = bll.GetList(SiteUserInfo.CompanyId, pageSize, pageIndex, ref recordCount, searchmodel, ref sum);

            UtilsCommons.Paging(pageSize, ref pageIndex, recordCount);
            if (list != null && list.Count > 0)
            {
                this.RptList.DataSource = list;
                this.RptList.DataBind();
                int adults = Utils.GetInt(sum[0].ToString());
                int childs = Utils.GetInt(sum[1].ToString());
                this.lbadults.Text = adults.ToString();
                this.lbchilds.Text = childs.ToString();
                this.lbSumMoney.Text = this.ToMoneyString((object)sum[2]);
                this.lbsumcount.Text = (adults + childs).ToString();
                BindExportPage();
            }
            else
            {
                this.lbemptymsg.Text = "<tr><td colspan='18' align='center' height='30px'>暂无数据!</td></tr>";
            }
            Bindstatus(status);
            BindTraffic(type);
        }

        protected string GetTrafficTime(object obj)
        {
            DateTime? time = (DateTime?)obj;
            string strtime = string.Empty;
            if (time.HasValue)
                strtime = time.Value.ToString("yyyy-MM-dd HH:mm");
            return strtime;
        }

        /// <summary>
        /// 绑定团队状态下拉选项
        /// </summary>
        /// <param name="selectvalue"></param>
        private void Bindstatus(string selectvalue)
        {
            statusStr = UtilsCommons.GetEnumDDL(EyouSoft.Common.EnumObj.GetList(typeof(EyouSoft.Model.EnumType.PlanStructure.TicketStatus)), selectvalue, "-1", "请选择");
        }
        /// <summary>
        /// 绑定票务类型下拉选项
        /// </summary>
        /// <param name="selectvalue"></param>
        private void BindTraffic(string selectvalue)
        {
            typeStr = UtilsCommons.GetEnumDDL(EyouSoft.Common.EnumObj.GetList(typeof(EyouSoft.Model.EnumType.PlanStructure.TicketType)), selectvalue, "-1", "请选择");
        }

        /// <summary>
        /// ajax操作
        /// </summary>
        private void AJAX(string doType, string id, string mode)
        {
            string msg = string.Empty;
            //对应执行操作
            switch (doType.ToLower())
            {
                case "delete":
                    if (mode.Trim() == "0")
                    {
                        //判断权限
                        if (this.CheckGrant(EyouSoft.Model.EnumType.PrivsStructure.Privs3.计调中心_票务安排_出票删除))
                        {
                            msg = this.DeleteData(id);
                        }
                        else
                        {
                            msg = UtilsCommons.AjaxReturnJson("0", "您没有出票删除的权限!");
                        }
                    }
                    else if (mode.Trim() == "1")
                    {
                        //判断权限
                        if (this.CheckGrant(EyouSoft.Model.EnumType.PrivsStructure.Privs3.计调中心_票务安排_退票删除))
                        {
                            msg = this.DeleteData(id);
                        }
                        else
                        {
                            msg = UtilsCommons.AjaxReturnJson("0", "您没有退票删除的权限!");
                        }
                    }
                    else
                    {
                        msg = UtilsCommons.AjaxReturnJson("0", "无效的票务类型");
                    }

                    break;
            }
            //返回ajax操作结果
            Response.Clear();
            Response.Write(msg);
            Response.End();
        }

        /// <summary>
        /// 删除操作
        /// </summary>
        /// <param name="id">删除ID</param>
        /// <returns></returns>
        private string DeleteData(string id)
        {
            string msg = "";
            if (!String.IsNullOrEmpty(id))
            {
                EyouSoft.BLL.PlanStructure.BPlanTicket bll = new EyouSoft.BLL.PlanStructure.BPlanTicket();
                /// -1:只有处在申请中的出票、退票安排才能删除
                ///	-2:票务存在支出不能删除	
                ///	1:成功 0：失败
                int result = bll.Delete(id);
                switch (result)
                {
                    case 1:
                        msg = UtilsCommons.AjaxReturnJson("1", "删除成功!");
                        break;
                    case 0:
                        msg = UtilsCommons.AjaxReturnJson("0", "删除失败!");
                        break;
                    case -1:
                        msg = UtilsCommons.AjaxReturnJson("0", "只有处在申请中的出票、退票安排才能删除!");
                        break;
                    case -2:
                        msg = UtilsCommons.AjaxReturnJson("0", "票务存在支出不能删除!");
                        break;
                    default:
                        break;
                }
            }
            else
            {
                msg = UtilsCommons.AjaxReturnJson("0", "无效的团队信息!");
            }
            return msg;
        }

        /// <summary>
        /// 获取票务状态
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        protected string GetTicketstatus(object obj, object tourcode)
        {
            TicketStatus status = (TicketStatus)obj;
            string TourCode = tourcode.ToString();
            string str = string.Empty;
            switch (status)
            {
                case TicketStatus.已申请:
                    str = "<font class='fred'>" + TourCode + "</font><img width='16' height='16' title='已申请' src='/images/chpshq.gif'>";
                    break;
                case TicketStatus.已出票:
                    str = "<font style='color:Green'>" + TourCode + "</font><img width='16' height='16' title='已出票' src='/images/ychp.gif'>";
                    break;
                default:
                    str = TourCode;
                    break;
            }
            return str;
        }

        /// <summary>
        /// 获取取消团队的行背景颜色，同一个团号背景色相同
        /// </summary>
        /// <param name="tourState">团队状态</param>
        /// <returns></returns>
        protected string GetTrBgColorByTourState(object tourState, object ticketMode, string TourCode, int index)
        {
            if (tourState == null) return string.Empty;

            var t = (EyouSoft.Model.EnumType.TourStructure.TourStatus)tourState;

            var p = (EyouSoft.Model.EnumType.PlanStructure.TicketMode)ticketMode;

            bgCode.Add(index, TourCode);
            if (index == 1)
            {
                bgColor.Add(index, index % 2 != 0 ? "even" : "odd");
            }

            else if (index > 1)
            {

                if (t == EyouSoft.Model.EnumType.TourStructure.TourStatus.已取消)
                {
                    bgColor.Add(index, "huise");
                }
                else if (p == EyouSoft.Model.EnumType.PlanStructure.TicketMode.退票)
                {
                    bgColor.Add(index, "tuipiao");
                }
                else
                {
                    if (bgCode[index - 1] == TourCode)
                    {
                        bgColor.Add(index, bgColor[index - 1]);
                    }
                    else
                    {
                        bgColor.Add(index, bgColor[index - 1] == "even" ? "odd" : "even");
                    }
                }
            }

            return bgColor[index];
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
            if (!this.CheckGrant(EyouSoft.Model.EnumType.PrivsStructure.Privs3.计调中心_票务安排_栏目))
            {
                Utils.ResponseNoPermit(EyouSoft.Model.EnumType.PrivsStructure.Privs3.计调中心_票务安排_栏目, false);
                return;
            }
            this.Phadd.Visible = this.CheckGrant(EyouSoft.Model.EnumType.PrivsStructure.Privs3.计调中心_票务安排_退票申请);
            this.Phupdate.Visible = this.CheckGrant(EyouSoft.Model.EnumType.PrivsStructure.Privs3.计调中心_票务安排_出票确认)
                || this.CheckGrant(EyouSoft.Model.EnumType.PrivsStructure.Privs3.计调中心_票务安排_出票修改)
                || this.CheckGrant(EyouSoft.Model.EnumType.PrivsStructure.Privs3.计调中心_票务安排_退票确认)
                || this.CheckGrant(EyouSoft.Model.EnumType.PrivsStructure.Privs3.计调中心_票务安排_退票修改);
            this.Phdelete.Visible = this.CheckGrant(EyouSoft.Model.EnumType.PrivsStructure.Privs3.计调中心_票务安排_出票删除)
                || this.CheckGrant(EyouSoft.Model.EnumType.PrivsStructure.Privs3.计调中心_票务安排_退票删除);
        }
    }
}
