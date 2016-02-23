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
using EyouSoft.Model.CompanyStructure;
using System.Collections.Generic;

namespace Web.jidiaoCenter
{
    public partial class AjaxBanciRequest : EyouSoft.Common.Page.BackPage
    {
        protected int listcount = 0;
        protected string Strmm = string.Empty;
        protected string StrHH = string.Empty;
        protected void Page_Load(object sender, EventArgs e)
        {
            string name = Utils.GetQueryStringValue("txtName");
            string dotype = Utils.GetQueryStringValue("dotype");
            int tickettype = Utils.GetInt(Utils.GetQueryStringValue("type"));
            this.phadd.Visible = false;
            if (dotype == "add")
            {
                Response.Clear();
                Response.Write(PageSave(name));
                Response.End();
            }
            if (!IsPostBack)
            {
                PageInit(name, tickettype);
            }

        }
        /// <summary>
        /// 初始化
        /// </summary>
        /// <param name="name"></param>
        private void PageInit(string name, int type)
        {
            //绑定席别
            Array values = Enum.GetValues(typeof(EyouSoft.Model.EnumType.PlanStructure.TrafficSeat));
            foreach (var item in values)
            {
                int value = (int)Enum.Parse(typeof(EyouSoft.Model.EnumType.PlanStructure.TrafficSeat), item.ToString());
                string text = Enum.GetName(typeof(EyouSoft.Model.EnumType.PlanStructure.TrafficSeat), item);
                ListItem ddlItem = new ListItem();
                ddlItem.Text = text;
                ddlItem.Value = value.ToString();
                this.ddlseat.Items.Add(ddlItem);
            }
            this.ddlseat.Items.Insert(0, new ListItem("--请选择--", ""));
            BindHourAndmm();

            MCompanyTicketSearch searchmodel = new MCompanyTicketSearch();
            EyouSoft.BLL.CompanyStructure.BCompanyTicket bll = new EyouSoft.BLL.CompanyStructure.BCompanyTicket();
            searchmodel.TrafficNumber = name;
            searchmodel.TicketType = (EyouSoft.Model.EnumType.PlanStructure.TicketType)type;
            IList<EyouSoft.Model.CompanyStructure.MCompanyTicket> list = bll.GetList(32, SiteUserInfo.CompanyId, searchmodel);
            if (list != null && list.Count > 0)
            {
                this.RptDataList.DataSource = list;
                this.RptDataList.DataBind();
                listcount = list.Count;
                this.phadd.Visible = false;
                this.phdatalist.Visible = true;
            }
            else
            {
                this.phadd.Visible = true;
                this.phdatalist.Visible = false;
            }
        }

        private void BindHourAndmm()
        {
            System.Text.StringBuilder strhh = new System.Text.StringBuilder();
            System.Text.StringBuilder strmm = new System.Text.StringBuilder();
            for (int i = 0; i < 61; i++)
            {
                strmm.AppendFormat("<option value='{0}'>{0}</option>", i.ToString());
            }
            for (int j = 0; j < 25; j++)
            {
                strhh.AppendFormat("<option value='{0}'>{0}</option>", j.ToString());
            }
            StrHH = strhh.ToString();
            Strmm = strmm.ToString();
        }

        /// <summary>
        /// 保存操作
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        private string PageSave(string name)
        {
            string msg = "";
            EyouSoft.BLL.CompanyStructure.BCompanyTicket bll = new EyouSoft.BLL.CompanyStructure.BCompanyTicket();
            MCompanyTicket model = new MCompanyTicket();

            string banciname = Utils.GetFormValue(this.txtbanciname.UniqueID);
            int tickettype = Utils.GetInt(Utils.GetFormValue(this.ddltickettype.UniqueID));
            DateTime? starttime = Utils.GetDateTimeNullable(Utils.GetFormValue(this.txtstarttime.UniqueID) + " " + Utils.GetFormValue("sltHH") + ":" + Utils.GetFormValue("sltmm"));
            string qujian = Utils.GetFormValue(this.txtqujian.UniqueID);
            decimal otherprice = Utils.GetDecimal(Utils.GetFormValue(this.txtoherprice.UniqueID));
            decimal yongjin = Utils.GetDecimal(Utils.GetFormValue(this.txtyongjin.UniqueID));
            string seat = Utils.GetFormValue(this.txtseat.UniqueID);
            int _seat = Utils.GetInt(Utils.GetFormValue(this.ddlseat.UniqueID));
            model.Brokerage = yongjin;
            model.CompanyId = SiteUserInfo.CompanyId;
            model.Interval = qujian;
            model.IssueTime = DateTime.Now;
            model.OperatorId = SiteUserInfo.UserId;
            model.OperatorName = SiteUserInfo.Username;
            model.OtherPrice = otherprice;
            model.TicketType = (EyouSoft.Model.EnumType.PlanStructure.TicketType)tickettype;
            model.TrafficNumber = banciname;
            model.TrafficSeat = seat;
            model._TrafficSeat = (EyouSoft.Model.EnumType.PlanStructure.TrafficSeat?)_seat;
            model.TrafficTime = starttime;

            if (bll.Add(model))
            {
                msg = UtilsCommons.AjaxReturnJson1("1", "添加成功!", model);
            }
            else
            {
                msg = UtilsCommons.AjaxReturnJson("0", "添加失败!");
            }
            return msg;
        }
    }
}
