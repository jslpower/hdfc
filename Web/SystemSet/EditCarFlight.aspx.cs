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
    /// 编辑车次航班
    /// </summary>
    public partial class EditCarFlight : EyouSoft.Common.Page.BackPage
    {
        protected string Strmm = string.Empty;
        protected string StrHH = string.Empty;
        protected void Page_Load(object sender, EventArgs e)
        {
            string action = Utils.GetQueryStringValue("action").ToLower();
            int id = Utils.GetInt(Utils.GetQueryStringValue("cid"));
            string save = Utils.GetQueryStringValue("save");

            if (!string.IsNullOrEmpty(save))
            {
                this.RCWE(this.Save());
            }

            if (!IsPostBack)
            {
                this.InitDropDownList();
                BindHourAndmm("", "");
                if (action == "edit")
                {
                    this.InitPage(id);
                }
            }

        }

        private EyouSoft.Model.CompanyStructure.MCompanyTicket GetFormValue()
        {
            var model = new EyouSoft.Model.CompanyStructure.MCompanyTicket
                {
                    Brokerage = Utils.GetDecimal(Utils.GetFormValue(txtYongJin.UniqueID)),
                    CompanyId = this.SiteUserInfo.CompanyId,
                    Id = 0,
                    Interval = Utils.GetFormValue(txtQuJian.UniqueID),
                    IssueTime = DateTime.Now,
                    OperatorId = this.SiteUserInfo.UserId,
                    OperatorName = this.SiteUserInfo.Name,
                    OtherPrice = Utils.GetDecimal(Utils.GetFormValue(txtRanYou.UniqueID)),
                    TicketType = null,
                    TrafficNumber = Utils.GetFormValue(txtCarNo.UniqueID),
                    TrafficSeat = Utils.GetFormValue(txtXiBie.UniqueID),
                    _TrafficSeat = (EyouSoft.Model.EnumType.PlanStructure.TrafficSeat?)Utils.GetInt(Utils.GetFormValue(ddlXiBie.UniqueID)),
                    TrafficTime = Utils.GetDateTimeNullable(Utils.GetFormValue(txtDate.UniqueID) + " " + Utils.GetFormValue("sltHH") + ":" + Utils.GetFormValue("sltmm"))
                };
            int at = Utils.GetInt(Utils.GetFormValue(ddlType.UniqueID));
            if (at == 0 || at == 1)
            {
                model.TicketType = (EyouSoft.Model.EnumType.PlanStructure.TicketType)at;
            }
            else
            {
                model.TicketType = null;
            }

            return model;
        }

        private string Save()
        {
            string action = Utils.GetQueryStringValue("action").ToLower();
            int id = Utils.GetInt(Utils.GetQueryStringValue("cid"));

            if (action == "edit" && id <= 0) return UtilsCommons.AjaxReturnJson("0", "url错误，请重新打开此窗口！");

            var model = this.GetFormValue();

            var bll = new EyouSoft.BLL.CompanyStructure.BCompanyTicket();
            bool r;
            if (action == "edit")
            {
                model.Id = id;

                r = bll.Update(model);
            }
            else
            {
                r = bll.Add(model);
            }

            return UtilsCommons.AjaxReturnJson(r ? "1" : "0", string.Format("保存{0}！", r ? "成功" : "失败"));
        }

        private void InitDropDownList()
        {
            var list = EnumObj.GetList(typeof(EyouSoft.Model.EnumType.PlanStructure.TicketType));
            ddlType.DataSource = list;
            ddlType.DataTextField = "Text";
            ddlType.DataValueField = "Value";
            ddlType.DataBind();



            //绑定导游星级
            Array values = Enum.GetValues(typeof(EyouSoft.Model.EnumType.PlanStructure.TrafficSeat));
            foreach (var item in values)
            {
                int value = (int)Enum.Parse(typeof(EyouSoft.Model.EnumType.PlanStructure.TrafficSeat), item.ToString());
                string text = Enum.GetName(typeof(EyouSoft.Model.EnumType.PlanStructure.TrafficSeat), item);
                ListItem ddlItem = new ListItem();
                ddlItem.Text = text;
                ddlItem.Value = value.ToString();
                this.ddlXiBie.Items.Add(ddlItem);
            }
            this.ddlXiBie.Items.Insert(0, new ListItem("--请选择--", ""));
        }

        private void InitPage(int id)
        {
            if (id <= 0) return;

            var model = new EyouSoft.BLL.CompanyStructure.BCompanyTicket().GetModel(id);
            if (model == null) return;

            txtCarNo.Text = model.TrafficNumber;
            txtDate.Value = model.TrafficTime.HasValue ? model.TrafficTime.Value.ToShortDateString() : "";
            txtQuJian.Value = model.Interval;
            txtRanYou.Value = model.OtherPrice == 0 ? string.Empty : model.OtherPrice.ToString("F2");
            txtXiBie.Value = model.TrafficSeat;
            ddlXiBie.SelectedValue = ((int)model._TrafficSeat).ToString();
            txtYongJin.Value = model.Brokerage == 0 ? string.Empty : model.Brokerage.ToString("F2");

            if (model.TicketType.HasValue && ddlType.Items.FindByValue(((int)model.TicketType.Value).ToString()) != null)
            {
                ddlType.Items.FindByValue(((int)model.TicketType.Value).ToString()).Selected = true;
            }
            BindHourAndmm(model.TrafficTime.HasValue ? model.TrafficTime.Value.Hour.ToString() : "", model.TrafficTime.HasValue ? model.TrafficTime.Value.Minute.ToString() : "");
        }

        private void BindHourAndmm(string hh, string mm)
        {
            System.Text.StringBuilder strhh = new System.Text.StringBuilder();
            System.Text.StringBuilder strmm = new System.Text.StringBuilder();
            for (int i = 0; i < 61; i++)
            {
                if (mm == i.ToString())
                    strmm.AppendFormat("<option value='{0}' selected='selected'>{0}</option>", i.ToString());
                else
                    strmm.AppendFormat("<option value='{0}'>{0}</option>", i.ToString());
            }
            for (int j = 0; j < 25; j++)
            {
                if (hh == j.ToString())
                    strhh.AppendFormat("<option value='{0}' selected='selected'>{0}</option>", j.ToString());
                else
                    strhh.AppendFormat("<option value='{0}'>{0}</option>", j.ToString());
            }
            StrHH = strhh.ToString();
            Strmm = strmm.ToString();
        }
    }
}
