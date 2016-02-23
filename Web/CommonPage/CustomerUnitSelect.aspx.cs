using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;
using EyouSoft.Common;
using System.Collections.Generic;

namespace Web.CommonPage
{
    /// <summary>
    /// 客户单位选用页面
    /// </summary>
    public partial class CustomerUnitSelect : EyouSoft.Common.Page.BackPage
    {
        protected int IsSelectMore;
        protected void Page_Load(object sender, EventArgs e)
        {
            IsSelectMore = Utils.GetInt(Utils.GetQueryStringValue("isMore"));
            
        }       
    }
}
