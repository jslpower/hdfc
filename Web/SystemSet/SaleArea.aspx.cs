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
    ///  销售地区管理
    /// </summary>
    public partial class SaleArea : EyouSoft.Common.Page.BackPage
    {
        /// <summary>
        /// 每页记录数
        /// </summary>
        private const int PageSize = 20;

        /// <summary>
        /// 当前页码
        /// </summary>
        private int _pageIndex = 1;

        /// <summary>
        /// 总记录数
        /// </summary>
        private int _recordCount;

        protected void Page_Load(object sender, EventArgs e)
        {
            string action = Utils.GetQueryStringValue("action").ToLower();
            if (action == "del")
            {
                DelSaleArea();
                return;
            }

            if (!IsPostBack)
            {
                if (!CheckGrant(EyouSoft.Model.EnumType.PrivsStructure.Privs3.系统设置_基础设置_销售地区栏目))
                {
                    Utils.ResponseNoPermit(EyouSoft.Model.EnumType.PrivsStructure.Privs3.系统设置_基础设置_销售地区栏目, false);
                    return;
                }

                InitPage();
            }
        }

        private void InitPage()
        {
            _pageIndex = Utils.GetInt(Utils.GetQueryStringValue("page"), 1);
            var t = new EyouSoft.BLL.CompanyStructure.BSaleArea().GetSaleArea(
                CurrentUserCompanyID, PageSize, _pageIndex, ref _recordCount, null);
            UtilsCommons.Paging(PageSize, ref _pageIndex, _recordCount);
            rptSaleArea.DataSource = t;
            rptSaleArea.DataBind();

            paging.CurrencyPage = _pageIndex;
            paging.intPageSize = PageSize;
            paging.intRecordCount = _recordCount;
        }

        /// <summary>
        /// 计算行索引
        /// </summary>
        /// <param name="index">当前行索引</param>
        /// <returns></returns>
        protected int GetLineIndex(int index)
        {
            return PageSize * (_pageIndex - 1) + index + 1;
        }

        /// <summary>
        /// 计算行样式
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        protected string GetLineClass(int index)
        {
            //是否最后的记录
            if (index + 1 == _recordCount)
            {
                if (index == 0 || (index + 1) % 2 != 0)
                {
                    return "<td>&nbsp;</td><td>&nbsp;</td><td>&nbsp;</td>";
                }

                return string.Empty;
            }
            if (index != 0 && (index + 1) % 2 == 0)
            {
                if (((index + 1) / 2) % 2 == 0)
                {
                    return "</tr><tr class=\"even\">";
                }

                return "</tr><tr class=\"odd\">";
            }

            return string.Empty;
        }

        /// <summary>
        /// 删除销售区域
        /// </summary>
        private void DelSaleArea()
        {
            if (!CheckGrant(EyouSoft.Model.EnumType.PrivsStructure.Privs3.系统设置_基础设置_销售地区栏目))
            {
                this.RCWE(
                    UtilsCommons.AjaxReturnJson(
                        "0",
                        string.Format(
                            "您没有{0}的权限，请联系管理员！", EyouSoft.Model.EnumType.PrivsStructure.Privs3.系统设置_基础设置_销售地区栏目)));
                return;
            }
            int sId = Utils.GetInt(Utils.GetQueryStringValue("sId"));
            if (sId <= 0)
            {
                this.RCWE(UtilsCommons.AjaxReturnJson("0", "url错误，请刷新页面后重新操作！"));
                return;
            }

            int r = new EyouSoft.BLL.CompanyStructure.BSaleArea().DeleteSaleArea(sId);

            switch (r)
            {
                case 0:
                    this.RCWE(UtilsCommons.AjaxReturnJson("0", "删除失败！"));
                    break;
                case -1:
                    this.RCWE(UtilsCommons.AjaxReturnJson("0", "此销售地区已使用，不允许删除！"));
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
