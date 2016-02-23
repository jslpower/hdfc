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
    /// 组团社统计
    /// </summary>
    public partial class SaleArea : EyouSoft.Common.Page.BackPage
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

        /// <summary>
        /// 合计实体
        /// </summary>
        protected EyouSoft.Model.TongJiStructure.MSaleAreaTongJiHeJi HeJi = new EyouSoft.Model.TongJiStructure.MSaleAreaTongJiHeJi();

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
                if (!this.CheckGrant(EyouSoft.Model.EnumType.PrivsStructure.Privs3.统计中心_销售地区统计_栏目))
                {
                    Utils.ResponseNoPermit(EyouSoft.Model.EnumType.PrivsStructure.Privs3.统计中心_销售地区统计_栏目, false);
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
            var search = new EyouSoft.Model.TongJiStructure.MSearchSaleAreaTongJi();
            _pageIndex = Utils.GetInt(Utils.GetQueryStringValue("Page"), 1);
            //此查询时间查询下单时间
            search.StartTime = Utils.GetDateTimeNullable(Utils.GetQueryStringValue("st"));
            search.EndTime = Utils.GetDateTimeNullable(Utils.GetQueryStringValue("et"));

            var list = new EyouSoft.BLL.TongJiStructure.BTongJi().GetSaleAreaTongJi(
                CurrentUserCompanyID, PageSize, _pageIndex, ref _recordCount, search, HeJi);
            UtilsCommons.Paging(PageSize, ref _pageIndex, _recordCount);

            rptSaleArea.DataSource = list;
            rptSaleArea.DataBind();

            page1.intPageSize = PageSize;
            page1.intRecordCount = _recordCount;
            page1.CurrencyPage = _pageIndex;
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


        #endregion
    }
}
