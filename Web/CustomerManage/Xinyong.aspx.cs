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

namespace Web.jidiaoCenter
{
    /// <summary>
    /// 创建人：邓保朝
    /// 时间：2013-09-29
    /// 描述：信用等级
    /// </summary>
    public partial class Xinyong : EyouSoft.Common.Page.BackPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            string rid = Utils.GetQueryStringValue("rid");
            string cid = Utils.GetQueryStringValue("cid");
            string dotype = Utils.GetQueryStringValue("dotype");
            if (dotype == "save")
            {
                Response.Clear();
                Response.Write(PageSave(cid));
                Response.End();
            }
            if (!IsPostBack)
            {
                PageInit(rid);
            }
        }
        private void PageInit(string rid)
        {
            EyouSoft.BLL.CompanyStructure.Rating bll = new EyouSoft.BLL.CompanyStructure.Rating();
            IList<EyouSoft.Model.CompanyStructure.Rating> List = bll.GetRatingByCompanyId(this.SiteUserInfo.CompanyId);
            if (List.Count > 0 && List != null)
            {
                for (int i = 0; i < List.Count; i++)
                {
                    dptlist.Items.Add(new ListItem(List[i].RatingName.ToString(), List[i].Id.ToString()));
                }
            }

            if (this.dptlist.Items.FindByValue(rid) != null)
                this.dptlist.Items.FindByValue(rid).Selected = true;
        }
        private string PageSave(string cid)
        {
            EyouSoft.BLL.CRM.BCustomer bll = new EyouSoft.BLL.CRM.BCustomer();
            int rid = Utils.GetInt(Utils.GetFormValue(this.dptlist.UniqueID));
            bool pan = bll.SetRating(cid, rid, SiteUserInfo.UserId);
            if (pan)
            {
                return UtilsCommons.AjaxReturnJson("1", "信用等级设置成功");
            }
            else
            {
                return UtilsCommons.AjaxReturnJson("0", "信用等级设置失败");
            }
        }
    }
}
