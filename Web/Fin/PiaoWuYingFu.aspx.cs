﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using EyouSoft.Common;
using System.Text;

namespace Web.Fin
{
    /// <summary>
    /// 票务应付管理
    /// </summary>
    public partial class PiaoWuYingFu : EyouSoft.Common.Page.BackPage
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
        protected EyouSoft.Model.FinStructure.MPlanHeJi HeJi = new EyouSoft.Model.FinStructure.MPlanHeJi();

        Dictionary<int, string> bgCode = new Dictionary<int, string>();
        Dictionary<int, string> bgColor = new Dictionary<int, string>();

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!this.CheckGrant(EyouSoft.Model.EnumType.PrivsStructure.Privs3.财务管理_应付管理_票务栏目))
            {
                Utils.ResponseNoPermit(EyouSoft.Model.EnumType.PrivsStructure.Privs3.财务管理_应付管理_票务栏目, false);
                return;
            }

            if (!IsPostBack)
            {
                this.InitPage();
            }
        }

        #region 初始化方法

        /// <summary>
        /// 初始化页面
        /// </summary>
        private void InitPage()
        {
            _pageIndex = Utils.GetInt(Utils.GetQueryStringValue("Page"), 1);

            var list = new EyouSoft.BLL.FinStructure.BPiaoFuKuan().GetPiaoList(
                CurrentUserCompanyID, PageSize, _pageIndex, ref _recordCount, this.GetSearchModel(), HeJi);

            UtilsCommons.Paging(PageSize, ref _pageIndex, _recordCount);

            rptPiaoWu.DataSource = list;
            rptPiaoWu.DataBind();

            page1.intPageSize = PageSize;
            page1.intRecordCount = _recordCount;
            page1.CurrencyPage = _pageIndex;
        }

        /// <summary>
        /// 获取查询实体
        /// </summary>
        /// <returns></returns>
        private EyouSoft.Model.FinStructure.MSearchPiaoList GetSearchModel()
        {
            var search = new EyouSoft.Model.FinStructure.MSearchPiaoList
            {
                TourNo = Utils.GetQueryStringValue("tno"),
                StartLeaveDate = Utils.GetDateTimeNullable(Utils.GetQueryStringValue("lst")),
                EndLeaveDate = Utils.GetDateTimeNullable(Utils.GetQueryStringValue("let")),
                GysName = Utils.GetQueryStringValue("tGysName")
            };

            int jq = Utils.GetInt(Utils.GetQueryStringValue("jq"), -1);
            int sh = Utils.GetInt(Utils.GetQueryStringValue("sh"), -1);
            int yj = Utils.GetInt(Utils.GetQueryStringValue("yj"), -1);
            int tty = Utils.GetInt(Utils.GetQueryStringValue("tty"), -1);
            if (jq < 0)
            {
                search.IsJieQing = null;
            }
            else
            {
                search.IsJieQing = jq == 1;
            }
            if (sh < 0)
            {
                search.IsCheck = null;
            }
            else
            {
                search.IsCheck = sh == 1;
            }
            if (yj < 0)
            {
                search.IsYueJie = null;
            }
            else
            {
                search.IsYueJie = yj == 1;
            }
            if (tty >= 0)
            {
                search.TourType = (EyouSoft.Model.EnumType.TourStructure.TourType)tty;
            }
            else
            {
                search.TourType = null;
            }

            //没有查询条件则隐藏已结清的
            if (string.IsNullOrEmpty(search.TourNo) && !search.StartLeaveDate.HasValue
                && !search.EndLeaveDate.HasValue && !search.IsJieQing.HasValue && !search.IsCheck.HasValue && !search.IsYueJie.HasValue
                && !search.TourType.HasValue && string.IsNullOrEmpty(search.GysName))
            {
                search.IsJieQing = false;
            }

            return search;
        }

        #endregion

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

            var strHtml = new StringBuilder();

            foreach (var t in list)
            {
                if (t == null) continue;

                strHtml.AppendFormat("<option value=\"{0}\">{1}</option>", t.Value, t.Text);
            }

            return strHtml.ToString();
        }

        /// <summary>
        /// 获取取消团队的行背景颜色，同一个团号背景色相同
        /// </summary>
        /// <param name="tourState">团队状态</param>
        /// <returns></returns>
        protected string GetTrBgColorByTourState(object tourState,string TourCode,int index)
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

        /// <summary>
        /// 获取审核html
        /// </summary>
        /// <param name="obj">审核状态</param>
        /// <returns></returns>
        protected string GetShenHeHtml(object obj)
        {
            if (!this.CheckGrant(EyouSoft.Model.EnumType.PrivsStructure.Privs3.财务管理_应付管理_票务审核))
            {
                return "无权限";
            }

            if (obj == null) return string.Empty;

            var state = (EyouSoft.Model.EnumType.PlanStructure.TicketStatus)obj;

            var str = new StringBuilder();
            switch (state)
            {
                case EyouSoft.Model.EnumType.PlanStructure.TicketStatus.已出票:
                    str.AppendFormat("<a href=\"javascript:void(0);\" class=\"a_ShenHePiaoWu\">已审核</a>");
                    break;
                default:
                    str.AppendFormat("<a href=\"javascript:void(0);\" class=\"a_ShenHePiaoWu\">未审核</a>");
                    break;
            }

            return str.ToString();
        }

        /// <summary>
        /// 获取登记html
        /// </summary>
        /// <param name="obj">审核状态</param>
        /// <returns></returns>
        protected string GetDengJiHtml(object obj)
        {
            if (obj == null) return string.Empty;

            var state = (EyouSoft.Model.EnumType.PlanStructure.TicketStatus)obj;

            var str = new StringBuilder();
            switch (state)
            {
                case EyouSoft.Model.EnumType.PlanStructure.TicketStatus.已确认:
                    str.AppendFormat("未审核");
                    break;
                case EyouSoft.Model.EnumType.PlanStructure.TicketStatus.已出票:
                    str.AppendFormat("<a href=\"javascript:void(0);\" class=\"a_DengJiPiaoWu\">登记</a>");
                    break;
            }

            return str.ToString();
        }

        /// <summary>
        /// 获取New图片
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        protected string GetNewImg(object obj)
        {
            if (obj == null) return string.Empty;

            var state = (EyouSoft.Model.EnumType.PlanStructure.TicketStatus)obj;

            if (state == EyouSoft.Model.EnumType.PlanStructure.TicketStatus.已出票)
            {
                return string.Empty;
            }

            return string.Format("<img src=\"/images/new.gif\" alt=\"\" width=\"28\" height=\"11\" />");

        }

        #endregion
    }
}