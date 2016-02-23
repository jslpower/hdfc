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
using EyouSoft.Common.Page;
using EyouSoft.Model.PlanStructure;
using System.Collections.Generic;
using System.Text;

namespace Web.jidiaoCenter
{
    /// <summary>
    /// 创建人：刘飞
    /// 时间：2013-1-7
    /// 描述：地接列表
    /// </summary>
    public partial class DijieList : BackPage
    {
        #region 分页参数
        protected int pageIndex = 1;
        protected int recordCount;
        protected int pageSize = 20;

        Dictionary<int, string> bgCode = new Dictionary<int, string>();
        Dictionary<int, string> bgColor = new Dictionary<int, string>();
        #endregion
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
                PageInit();
            }
        }
        /// <summary>
        /// 初始化
        /// </summary>
        private void PageInit()
        {
            string txttourcode = Utils.GetQueryStringValue("txttourcode");
            string txtleavedate = Utils.GetQueryStringValue("txtleavedate");
            string txtbackdate = Utils.GetQueryStringValue("txtbackdate");
            string sourcename = Utils.GetQueryStringValue("txtsourcename");
            string sourceid = Utils.GetQueryStringValue("hidsourceid");
            string yuejie = Utils.GetQueryStringValue("yuejie");
            string tourtype = Utils.GetQueryStringValue("tourtype");
            EyouSoft.BLL.PlanStructure.BPlanDiJie bll = new EyouSoft.BLL.PlanStructure.BPlanDiJie();
            MSeachDiJie searchmodel = new MSeachDiJie();
            searchmodel.DiJieId = sourceid;
            searchmodel.DiJieName = sourcename;
            searchmodel.LBeginDate = Utils.GetDateTimeNullable(txtleavedate);
            searchmodel.LEndDate = Utils.GetDateTimeNullable(txtbackdate);
            searchmodel.TourCode = txttourcode;
            if (yuejie == "1")
                searchmodel.IsMonth = true;
            if (yuejie == "0")
                searchmodel.IsMonth = false;

            if (tourtype == "0")
            {
                searchmodel.TourType = EyouSoft.Model.EnumType.TourStructure.TourType.团;
            }
            if (tourtype == "1")
            {
                searchmodel.TourType = EyouSoft.Model.EnumType.TourStructure.TourType.散;
            }
            pageIndex = Utils.GetInt(Utils.GetQueryStringValue("Page"), 1);
            int _comid = SiteUserInfo.CompanyId;
            if (SiteUserInfo.UserType == EyouSoft.Model.EnumType.CompanyStructure.UserType.地接用户)
            {
                searchmodel.DiJieId = SiteUserInfo.SupplierCompanyId;
                searchmodel.DiJieName = string.Empty;
                this.PhDijie.Visible = false;
            }
            decimal[] sum = new decimal[1];
            IList<EyouSoft.Model.PlanStructure.MPageDiJie> list = bll.GetList(SiteUserInfo.CompanyId, pageSize, pageIndex, ref recordCount, searchmodel, ref sum);
            UtilsCommons.Paging(pageSize, ref pageIndex, recordCount);

            if (list != null && list.Count > 0)
            {
                this.rptList.DataSource = list;
                this.rptList.DataBind();
                this.lbSumMoney.Text = ToMoneyString((object)sum[0]);
                BindExportPage();
            }
            else
            {
                this.lbemptymsg.Text = "<tr><td colspan='19' align='center' height='30px'>暂无数据!</td></tr>";
            }

        }

        /// <summary>
        /// 获取地接状态
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        protected string GetTourStatus(object obj)
        {
            EyouSoft.Model.EnumType.PlanStructure.DiJieStatus tourstatus = (EyouSoft.Model.EnumType.PlanStructure.DiJieStatus)obj;
            string str = "";
            switch (tourstatus)
            {
                case EyouSoft.Model.EnumType.PlanStructure.DiJieStatus.申请中:
                    str = "<img width='44' height='16' title='未处理' src='/images/wchuli.gif' alt='' />";
                    break;
                case EyouSoft.Model.EnumType.PlanStructure.DiJieStatus.已确认:
                    str = "<img width='44' height='16' title='已确认' src='/images/queren.gif' alt='' />";
                    break;
                case EyouSoft.Model.EnumType.PlanStructure.DiJieStatus.已审核:
                    str = "<img width='44' height='16' title='已审核' src='/images/shenhe.gif' alt='' />";
                    break;
                default:
                    break;
            }
            return str;
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
                    //判断权限
                    if (this.CheckGrant(EyouSoft.Model.EnumType.PrivsStructure.Privs3.计调中心_地接安排_删除))
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
                EyouSoft.BLL.PlanStructure.BPlanDiJie bll = new EyouSoft.BLL.PlanStructure.BPlanDiJie();
                int result = bll.Delete(id);
                /// -1:只有处在申请中的地接安排才能删除
                ///	-2:地接存在支出不能删除
                ///	1:成功 0：失败
                switch (result)
                {
                    case 1:
                        msg = UtilsCommons.AjaxReturnJson("1", "删除成功!");
                        break;
                    case 0:
                        msg = UtilsCommons.AjaxReturnJson("-1", "只有处在申请中的地接安排才能删除!");
                        break;
                    case -1:
                        msg = UtilsCommons.AjaxReturnJson("-2", "地接存在支出不能删除!");
                        break;
                    case -2:
                        msg = UtilsCommons.AjaxReturnJson("0", "删除失败!");
                        break;
                    default:
                        msg = UtilsCommons.AjaxReturnJson("0", "删除失败!");
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
        /// 获取地接社信息
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        protected string GetDijie(object obj, object sourcename)
        {
            IList<EyouSoft.Model.SourceStructure.MSupplierContact> list = (IList<EyouSoft.Model.SourceStructure.MSupplierContact>)obj;
            StringBuilder str = new StringBuilder();
            str.Append("<table cellspacing='0' cellpadding='0' border='0' width='100%' class='pp-tableclass'><tr class='pp-table-title'><th>地接名称</th><th>计调</th><th>电话</th><th>QQ</th></tr>");
            if (list != null && list.Count > 0)
            {
                for (int i = 0; i < list.Count; i++)
                {
                    str.AppendFormat("<tr><td>{0}</td><td>{1}</td><td>{2}</td><td>{3}</td></tr>", sourcename.ToString(), list[i].ContactName, list[i].ContactTel, list[i].QQ);
                }
            }
            str.Append("</table>");
            return str.ToString();
        }
        /// <summary>
        /// 获取大交通信息
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        protected string GetTraffic(object obj, string type)
        {
            IList<EyouSoft.Model.PlanStructure.MPlanTicket> list = (IList<EyouSoft.Model.PlanStructure.MPlanTicket>)obj;
            StringBuilder str = new StringBuilder();
            if (type == "list")
            {
                str.Append("<table cellspacing='0' cellpadding='0' border='0' width='100%' class='pp-tableclass'><tr class='pp-table-title'></tr><tr class='pp-table-title'><th>类型</th><th>区间</th><th>时间</th><th>车次/航班</th></tr>");
                if (list != null && list.Count > 0)
                {
                    for (int i = 0; i < list.Count; i++)
                    {
                        str.AppendFormat("<tr><td>{3}</td><td>{0}</td><td>{1}</td><td>{2}</td></tr>", list[i].Interval, list[i].TrafficTime, list[i].TrafficNumber, list[i].TicketType.ToString());
                    }
                }
                str.Append("</table>");
            }
            else if (type == "tickettype")
            {
                if (list != null && list.Count > 0)
                {
                    str.Append(list[0].TicketType.ToString());
                }
            }
            return str.ToString();
        }

        /// <summary>
        /// 获取取消团队的行背景颜色，同一个团号背景色相同
        /// </summary>
        /// <param name="tourState">团队状态</param>
        /// <returns></returns>
        protected string GetTrBgColorByTourState(object tourState, string TourCode, int index)
        {
            if (tourState == null) return string.Empty;

            var t = (EyouSoft.Model.EnumType.TourStructure.TourStatus)tourState;

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
            if (!this.CheckGrant(EyouSoft.Model.EnumType.PrivsStructure.Privs3.计调中心_地接安排_栏目))
            {
                Utils.ResponseNoPermit(EyouSoft.Model.EnumType.PrivsStructure.Privs3.计调中心_地接安排_栏目, false);
                return;
            }

            this.phadd.Visible = this.CheckGrant(EyouSoft.Model.EnumType.PrivsStructure.Privs3.计调中心_地接安排_新增);
            this.PhUpdate.Visible = this.CheckGrant(EyouSoft.Model.EnumType.PrivsStructure.Privs3.计调中心_地接安排_修改)
                || this.CheckGrant(EyouSoft.Model.EnumType.PrivsStructure.Privs3.计调中心_地接安排_确认);
            this.Phdelete.Visible = this.CheckGrant(EyouSoft.Model.EnumType.PrivsStructure.Privs3.计调中心_地接安排_删除);
        }

    }
}
