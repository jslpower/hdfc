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
    public partial class Customer : EyouSoft.Common.Page.BackPage
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
        protected EyouSoft.Model.TongJiStructure.MCustomerTongJiHeJi HeJi = new EyouSoft.Model.TongJiStructure.MCustomerTongJiHeJi();


        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (!this.CheckGrant(EyouSoft.Model.EnumType.PrivsStructure.Privs3.统计中心_组团社统计_栏目))
                {
                    Utils.ResponseNoPermit(EyouSoft.Model.EnumType.PrivsStructure.Privs3.统计中心_组团社统计_栏目, false);
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
            var search = new EyouSoft.Model.TongJiStructure.MSearchCustomerTongJi();
            _pageIndex = Utils.GetInt(Utils.GetQueryStringValue("Page"), 1);
            int pid = Utils.GetInt(Utils.GetQueryStringValue("pid"));
            int cid = Utils.GetInt(Utils.GetQueryStringValue("cid"));
            int oi = Utils.GetInt(Utils.GetQueryStringValue("oi"));
            if (pid > 0) search.ProvinceId = new int[] { pid };
            if (cid > 0) search.CityId = new int[] { cid };
            //此查询时间影响交易次数。交易人数、交易金额、拜访次数、拜访支出(查询下单时间 和 拜访时间)
            search.StartTime = Utils.GetDateTimeNullable(Utils.GetQueryStringValue("st"));
            search.EndTime = Utils.GetDateTimeNullable(Utils.GetQueryStringValue("et"));
            if (oi >= 2 && oi <= 11) search.OrderByIndex = oi;

            var list = new EyouSoft.BLL.TongJiStructure.BTongJi().GetCustomerTongJi(
                CurrentUserCompanyID, PageSize, _pageIndex, ref _recordCount, search, HeJi);
            UtilsCommons.Paging(PageSize, ref _pageIndex, _recordCount);

            rptCustomer.DataSource = list;
            rptCustomer.DataBind();

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
