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

namespace Web.jidiaoCenter
{
    /// <summary>
    /// 创建人：刘飞
    /// 时间：2012-12-28
    /// 描述：质量评分
    /// </summary>
    public partial class PingGu : EyouSoft.Common.Page.BackPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            string tourid = Utils.GetQueryStringValue("tourid");
            string score = Utils.GetQueryStringValue("score");
            string dotype = Utils.GetQueryStringValue("dotype");
            if (dotype == "save")
            {
                Response.Clear();
                Response.Write(PageSave(tourid));
                Response.End();
            }
            if (!IsPostBack)
            {
                PageInit(score);
            }
        }
        private void PageInit(string score)
        {
            //绑定导游星级
            Array values = Enum.GetValues(typeof(EyouSoft.Model.EnumType.TourStructure.Score));
            foreach (var item in values)
            {
                int value = (int)Enum.Parse(typeof(EyouSoft.Model.EnumType.TourStructure.Score), item.ToString());
                string text = Enum.GetName(typeof(EyouSoft.Model.EnumType.TourStructure.Score), item);
                ListItem ddlItem = new ListItem();
                ddlItem.Text = text;
                ddlItem.Value = value.ToString();
                this.dptscore.Items.Add(ddlItem);
            }
          
            if (this.dptscore.Items.FindByValue(score) != null)
                this.dptscore.Items.FindByValue(score).Selected = true;
        }
        private string PageSave(string tourid)
        {
            EyouSoft.BLL.TourStructure.BTourReturnVisit bll = new EyouSoft.BLL.TourStructure.BTourReturnVisit();
            int score = Utils.GetInt(Utils.GetFormValue(this.dptscore.UniqueID));
            if (bll.SetTourScore(tourid, (EyouSoft.Model.EnumType.TourStructure.Score)score, SiteUserInfo.UserId))
            {
                return UtilsCommons.AjaxReturnJson("1", "评估成功");
            }
            else
            {
                return UtilsCommons.AjaxReturnJson("0", "评估失败");
            }
        }
    }
}
