using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Web.CompanyFiles
{
    public partial class NoticeDetail : EyouSoft.Common.Page.BackPage
    {
        protected string ReturnUrl = string.Empty;

        protected void Page_Load(object sender, EventArgs e)
        {
            ReturnUrl = EyouSoft.Common.Utils.GetQueryStringValue("returnUrl");
            if (string.IsNullOrEmpty(ReturnUrl)) ReturnUrl = "/CompanyFiles/MsgManageList.aspx";

            if (!IsPostBack)
            {
                int Id = EyouSoft.Common.Utils.GetInt(EyouSoft.Common.Utils.GetQueryStringValue("Id"), 0);
                if (Id != 0)
                {
                    EyouSoft.BLL.CompanyStructure.News bll = new EyouSoft.BLL.CompanyStructure.News();
                    EyouSoft.Model.CompanyStructure.News model = bll.GetModel(Id);
                    if (model != null)
                    {
                        bll.SetClicks(Id);

                        bll.ReadNews(Id, SiteUserInfo.UserId);

                        this.ltTitle.Text = model.Title;
                        this.ltContent.Text = model.Content;
                        this.ltCreateTime.Text = model.IssueTime.ToString("yyyy-MM-dd");
                        this.ltOperatorName.Text = model.OperatorName;

                        if (!string.IsNullOrEmpty(model.UploadFiles))
                        {
                            this.hlFile.Visible = true;
                            this.hlFile.NavigateUrl = model.UploadFiles;
                        }
                    }


                }
            }
        }
    }
}
