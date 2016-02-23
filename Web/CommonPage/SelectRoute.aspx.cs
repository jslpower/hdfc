using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using EyouSoft.Common;

namespace Web.CommonPage
{
    /// <summary>
    /// 线路选用页面
    /// </summary>
    public partial class SelectRoute : EyouSoft.Common.Page.BackPage
    {
        protected int IsSelectMore = 0;
        protected void Page_Load(object sender, EventArgs e)
        {
            IsSelectMore = Utils.GetInt(Utils.GetQueryStringValue("isMore"));
        }

    }
}
