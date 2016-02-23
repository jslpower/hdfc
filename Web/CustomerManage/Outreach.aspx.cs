using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using EyouSoft.Common;

namespace Web.CustomerManage
{
    /// <summary>
    /// 外联每日足迹
    /// </summary>
    public partial class Outreach : EyouSoft.Common.Page.BackPage
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
                this.DelOutreach();
                return;
            }

            if (!IsPostBack)
            {
                if (!this.CheckGrant(EyouSoft.Model.EnumType.PrivsStructure.Privs3.客户询价_外联每天足迹_栏目))
                {
                    Utils.ResponseNoPermit(EyouSoft.Model.EnumType.PrivsStructure.Privs3.客户询价_外联每天足迹_栏目, false);
                    return;
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
            var list = new EyouSoft.BLL.CustomerQuote.BOutreach().GetOutreach(
                CurrentUserCompanyID,
                PageSize,
                _pageIndex,
                ref _recordCount,
                new EyouSoft.Model.CustomerQuote.MSearchOutreach
                {
                    SaleUnitName = Utils.GetQueryStringValue("sName"),
                    StartSaleTime = Utils.GetDateTimeNullable(Utils.GetQueryStringValue("st")),
                    EndSaleTime = Utils.GetDateTimeNullable(Utils.GetQueryStringValue("et"))
                });
            UtilsCommons.Paging(PageSize, ref _pageIndex, _recordCount);

            rptQutreach.DataSource = list;
            rptQutreach.DataBind();

            page1.intPageSize = PageSize;
            page1.intRecordCount = _recordCount;
            page1.CurrencyPage = _pageIndex;
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
        /// 删除询价信息
        /// </summary>
        private void DelOutreach()
        {
            if (!this.CheckGrant(EyouSoft.Model.EnumType.PrivsStructure.Privs3.客户询价_外联每天足迹_删除))
            {
                this.RCWE(
                    UtilsCommons.AjaxReturnJson(
                        "0",
                        string.Format("您没有{0}的权限，请联系管理员！", EyouSoft.Model.EnumType.PrivsStructure.Privs3.客户询价_外联每天足迹_删除)));
                return;
            }
            string action = Utils.GetQueryStringValue("action").ToLower();
            string qId = Utils.GetQueryStringValue("qId");
            if (string.IsNullOrEmpty(action) || action != "del" || string.IsNullOrEmpty(qId))
            {
                this.RCWE(UtilsCommons.AjaxReturnJson("0", "url错误，请刷新页面后重新操作！"));
                return;
            }

            var ids = qId.Split(',');
            IList<int> l = ids.Select(t => Utils.GetInt(t)).Where(tmp => tmp > 0).ToList();
            if (!l.Any())
            {
                this.RCWE(UtilsCommons.AjaxReturnJson("0", "url错误，请刷新页面后重新操作！"));
                return;
            }

            int r = new EyouSoft.BLL.CustomerQuote.BOutreach().DeleteOutreach(l.ToArray());

            switch (r)
            {
                case 0:
                    this.RCWE(UtilsCommons.AjaxReturnJson("0", "删除失败！"));
                    break;
                case 1:
                    this.RCWE(UtilsCommons.AjaxReturnJson("1", "删除成功！"));
                    break;
                default:
                    this.RCWE(UtilsCommons.AjaxReturnJson("0", "删除失败！"));
                    break;
            }
        }
    }
}
