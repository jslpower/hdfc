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
using System.Text;
using EyouSoft.Model.TourStructure;

namespace Web.jidiaoCenter
{
    public partial class FankuiList : EyouSoft.Common.Page.BackPage
    {
        #region 分页参数
        protected int pageIndex;
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

            pageIndex = Utils.GetInt(Utils.GetQueryStringValue("page"), 1);
            string tourcode = Utils.GetQueryStringValue("tourcode");
            DateTime? starttime = Utils.GetDateTimeNullable(Utils.GetQueryStringValue("starttime"));
            DateTime? endtime = Utils.GetDateTimeNullable(Utils.GetQueryStringValue("endtime"));
            string isvisist = Utils.GetQueryStringValue("sltvisit");
            EyouSoft.BLL.TourStructure.BTourReturnVisit bll = new EyouSoft.BLL.TourStructure.BTourReturnVisit();
            EyouSoft.Model.TourStructure.MSeachVist searchmodel = new EyouSoft.Model.TourStructure.MSeachVist();
            searchmodel.LBeginDate = starttime;
            searchmodel.LEndDate = endtime;
            searchmodel.TourCode = tourcode;
            if (isvisist != "-1" && isvisist != "")
                searchmodel.IsVist = Convert.ToBoolean(Utils.GetInt(isvisist));
            pageIndex = Utils.GetInt(Utils.GetQueryStringValue("Page"), 1);
            IList<EyouSoft.Model.TourStructure.MPageTourReturnVisit> list = bll.GetVisitHuiKuiList(SiteUserInfo.CompanyId, pageSize, pageIndex, ref recordCount, searchmodel);

            UtilsCommons.Paging(pageSize, ref pageIndex, recordCount);
            if (list != null && list.Count > 0)
            {
                this.rptList.DataSource = list;
                this.rptList.DataBind();
                BindExportPage();
            }
            else
            {
                lbemptymsg.Text = "<tr><td colspan='9' align='center' height='30px'>暂无数据!</td></tr>";
            }
        }
        /// <summary>
        /// 获取团队状态(行程中的团显示行程中的图标)
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        protected string GetTourStatus(object obj)
        {
            string str = string.Empty;
            if (obj != null)
            {
                EyouSoft.Model.EnumType.TourStructure.TourStatus tourstatus = (EyouSoft.Model.EnumType.TourStructure.TourStatus)obj;
                if (tourstatus == EyouSoft.Model.EnumType.TourStructure.TourStatus.行程中)
                {
                    str = "<img alt='行程中' src='/images/xchzh.gif' width='44px' height='16px' />";
                }
            }
            return str;
        }
        /// <summary>
        /// 获取评分
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        protected string GetScore(object obj)
        {
            string str = string.Empty;
            EyouSoft.Model.EnumType.TourStructure.Score score = (EyouSoft.Model.EnumType.TourStructure.Score)obj;
            switch (score)
            {
                //case EyouSoft.Model.EnumType.TourStructure.Score.暂无:
                //    str = "评估";
                //    break;
                //case EyouSoft.Model.EnumType.TourStructure.Score.一星:
                //    str = "★";
                //    break;
                //case EyouSoft.Model.EnumType.TourStructure.Score.二星:
                //    str = "★★";
                //    break;
                //case EyouSoft.Model.EnumType.TourStructure.Score.三星:
                //    str = "★★★";
                //    break;
                //case EyouSoft.Model.EnumType.TourStructure.Score.四星:
                //    str = "★★★★";
                //    break;
                //case EyouSoft.Model.EnumType.TourStructure.Score.五星:
                //    str = "★★★★★";
                //    break;
                //case EyouSoft.Model.EnumType.TourStructure.Score.六星:
                //    str = "☆";
                //    break;
                //case EyouSoft.Model.EnumType.TourStructure.Score.七星:
                //    str = "☆☆";
                //    break;
                //case EyouSoft.Model.EnumType.TourStructure.Score.八星:
                //    str = "☆☆☆";
                //    break;
                //case EyouSoft.Model.EnumType.TourStructure.Score.九星:
                //    str = "☆☆☆☆";
                //    break;
                //case EyouSoft.Model.EnumType.TourStructure.Score.十星:
                //    str = "☆☆☆☆☆";
                //    break;
                default:
                    break;
            }
            return str;
        }

        /// <summary>
        /// 获取地接联系人
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        protected string GetDijieInfo(object obj)
        {
            IList<EyouSoft.Model.TourStructure.MTourDiJie> list = (IList<EyouSoft.Model.TourStructure.MTourDiJie>)obj;
            StringBuilder str = new StringBuilder();
            str.Append("<table cellspacing='0' cellpadding='0' border='0' width='100%' class='pp-tableclass'><tr class='pp-table-title'><th>地接名称</th><th>计调</th><th>电话</th><th>QQ</th></tr>");
            if (list != null && list.Count > 0)
            {
                for (int i = 0; i < list.Count; i++)
                {
                    str.AppendFormat("<tr><td>{3}</td><td>{0}</td><td>{1}</td><td>{2}</td></tr>", list[i].Name, list[i].Phone, list[i].QQ, list[i].DiJieName);
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
            StringBuilder str = new StringBuilder();
            str.Append("<table cellspacing='0' cellpadding='0' border='0' width='100%' class='pp-tableclass'><tr class='pp-table-title'><th>姓名</th><th>电话</th></tr>");
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
            if (!this.CheckGrant(EyouSoft.Model.EnumType.PrivsStructure.Privs3.计调中心_团队质量反馈_栏目))
            {
                Utils.ResponseNoPermit(EyouSoft.Model.EnumType.PrivsStructure.Privs3.计调中心_团队质量反馈_栏目, false);
                return;
            }
        }
    }
}
