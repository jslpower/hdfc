using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Web.CommonPage
{
    public partial class LoadVisitors : EyouSoft.Common.Page.BackPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            FileUpload1.CompanyID = SiteUserInfo.CompanyId;
        }
    }
}
