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
using EyouSoft.Model.TourStructure;

namespace Web.jidiaoCenter
{
    /// <summary>
    /// 创建人：刘飞
    /// 时间：2012-12-26
    /// 描述:票务新增/修改/审核/确认/取消审核
    /// </summary>
    public partial class TicketEdit : EyouSoft.Common.Page.BackPage
    {
        protected string PayTypeOption = string.Empty;
        protected string Strmm = string.Empty;
        protected string StrHH = string.Empty;
        protected void Page_Load(object sender, EventArgs e)
        {
            PowerControl();
            this.UploadControl1.CompanyID = SiteUserInfo.CompanyId;
            string planid = Utils.GetQueryStringValue("planid");
            string dotype = Utils.GetQueryStringValue("dotype").Trim();
            string type = Utils.GetQueryStringValue("type").Trim();
            string tourcode = Utils.GetQueryStringValue("tourcode").Trim();
            string tourid = Utils.GetQueryStringValue("tourid").Trim();
            switch (type)
            {
                case "save":
                    Response.Clear();
                    Response.Write(PageSave(planid, dotype));
                    Response.End();
                    break;
                default:
                    break;
            }
            if (!IsPostBack)
            {
                PageInit(planid, dotype, tourcode, tourid);
            }

        }


        /// <summary>
        /// 初始化
        /// </summary>
        /// <param name="id"></param>
        /// <param name="dotype"></param>
        private void PageInit(string id, string dotype, string tourcode, string tourid)
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
                this.ddlTrafficSeat.Items.Add(ddlItem);
            }
            this.ddlTrafficSeat.Items.Insert(0, new ListItem("--请选择--", ""));


            string type = "";
            string isfin = Utils.GetQueryStringValue("isfin");
            if (id != "" && dotype == "update")
            {
                EyouSoft.BLL.PlanStructure.BPlanTicket bll = new EyouSoft.BLL.PlanStructure.BPlanTicket();
                EyouSoft.Model.PlanStructure.MPlanTicketInfo model = new EyouSoft.Model.PlanStructure.MPlanTicketInfo();
                model = bll.GetModel(id);
                if (model != null)
                {
                    if (model.TravellerList != null && model.TravellerList.Count > 0)
                    {
                        this.OrderCustomer1.CustomerList = model.TravellerList;
                    }
                    this.txtadultprice.Text = model.AdultPrice.ToString("f2");
                    this.txtadults.Text = model.Adults.ToString();
                    this.txtBrokerage.Text = model.Brokerage.ToString("f2");
                    this.txtcheci.Text = model.TrafficNumber;
                    this.txtchildprice.Text = model.ChildPrice.ToString("f2");
                    this.txtchilds.Text = model.Childs.ToString();
                    this.txtOtherPrice.Text = model.OtherPrice.ToString("f2");
                    this.txtqujian.Text = model.Interval;
                    this.txtSumpirce.Text = model.SumPrice.ToString("f2");
                    this.SellsSelect1.SellsName = model.Ticketer;
                    this.SellsSelect1.SellsID = model.TicketerId.ToString();
                    this.TourSelect1.TourCode = model.TourCode;
                    this.TourSelect1.TourId = model.TourId;
                    this.TourSelect1.IsEnable = false;
                    this.txtTrafficSeat.Text = model.TrafficSeat;
                    this.ddlTrafficSeat.SelectedValue = ((int)model._TrafficSeat).ToString();
                    this.hdticketmode.Value = ((int)model.TicketMode).ToString();
                    type = ((int)model.PayType).ToString();
                    //以出票改为已确认，已审核改为以出票
                    this.txtjiesuantime.Text = model.MonthTime.HasValue ? Utils.GetDateTime(model.MonthTime.ToString()).ToString("yyy-MM-dd") : "";
                    if (ddlyuejie.Items.FindByValue(Convert.ToInt32(model.IsMonth).ToString()) != null)
                        ddlyuejie.Items.FindByValue(Convert.ToInt32(model.IsMonth).ToString()).Selected = true;

                    string time = model.TrafficTime.ToString();
                    this.txttime.Text = model.TrafficTime.HasValue ? model.TrafficTime.Value.ToShortDateString() : "";


                    BindHourAndmm(model.TrafficTime.HasValue ? model.TrafficTime.Value.Hour.ToString() : "", model.TrafficTime.HasValue ? model.TrafficTime.Value.Minute.ToString() : "");
                    if (!model.IsMonth)
                    {
                        this.spanjisuantime.Attributes.Add("style", "display:none");
                        this.txtjiesuantime.Attributes.Remove("valid");
                    }
                    this.phshenqing.Visible = false;
                    if (model.TicketStatus == EyouSoft.Model.EnumType.PlanStructure.TicketStatus.已申请)
                    {
                        this.PhCencer.Visible = false;
                        this.PhCheck.Visible = false;

                    }
                    if (model.TicketStatus == EyouSoft.Model.EnumType.PlanStructure.TicketStatus.已确认)
                    {
                        if (isfin != "")
                        {
                            this.PhCencer.Visible = false;
                            this.phqueren.Visible = false;
                            this.phshenqing.Visible = false;
                        }
                        else
                        {
                            this.PhCencer.Visible = false;
                            this.phshenqing.Visible = false;
                            this.PhCheck.Visible = false;
                        }

                    }
                    if (model.TicketStatus == EyouSoft.Model.EnumType.PlanStructure.TicketStatus.已出票)
                    {
                        if (isfin == "")
                        {
                            this.PhBtn.Visible = false;
                        }
                        else
                        {
                            this.PhCheck.Visible = false;
                            this.phqueren.Visible = false;
                            this.phshenqing.Visible = false;
                            //this.phsave.Visible = false;
                        }
                    }
                    if (dptticket.Items.FindByValue(((int)model.TicketType).ToString()) != null)
                        dptticket.Items.FindByValue(((int)model.TicketType).ToString()).Selected = true;
                    if (model.TicketMode == EyouSoft.Model.EnumType.PlanStructure.TicketMode.出票)
                    {
                        radchu.Checked = true;
                        radtui.Checked = false;
                        //if (!this.CheckGrant(EyouSoft.Model.EnumType.PrivsStructure.Privs3.计调中心_票务安排_出票确认))
                        //{
                        //    this.phqueren.Visible = false;
                        //}
                        if (!this.CheckGrant(EyouSoft.Model.EnumType.PrivsStructure.Privs3.计调中心_票务安排_出票修改))
                        {
                            //this.phsave.Visible = false;
                        }
                    }
                    else
                    {
                        radtui.Checked = true;
                        radchu.Checked = false;
                        //if (!this.CheckGrant(EyouSoft.Model.EnumType.PrivsStructure.Privs3.计调中心_票务安排_退票确认))
                        //{
                        //    this.phqueren.Visible = false;
                        //}
                        if (!this.CheckGrant(EyouSoft.Model.EnumType.PrivsStructure.Privs3.计调中心_票务安排_退票修改))
                        {
                            //this.phsave.Visible = false;
                        }
                    }
                    //this.radchu.Enabled = false;
                    //this.radtui.Enabled = false;

                    if (SiteUserInfo.UserType == EyouSoft.Model.EnumType.CompanyStructure.UserType.票务用户)
                    {
                        this.SupperControl1.HideID = SiteUserInfo.SupplierCompanyId;
                        var sModel = new EyouSoft.BLL.SourceStructure.BSupplier().GetModel(SiteUserInfo.SupplierCompanyId);
                        this.SupperControl1.Name = sModel == null ? string.Empty : sModel.UnitName;
                        this.SupperControl1.IsEnable = false;
                        this.PhCencer.Visible = false;
                        this.PhCheck.Visible = false;
                    }
                    else
                    {
                        this.SupperControl1.HideID = model.GysId;
                        this.SupperControl1.Name = model.GysName;
                    }

                    if (model.FileList != null && model.FileList.Count > 0)
                    {
                        this.rplfile.DataSource = model.FileList;
                        this.rplfile.DataBind();
                    }
                }
            }
            else
            {
                BindHourAndmm("", "");
                this.TourSelect1.TourCode = tourcode;
                this.TourSelect1.TourId = tourid;
                this.TourSelect1.IsEnable = true;
                if (isfin == "")
                {
                    this.PhCencer.Visible = false;
                    this.PhCheck.Visible = false;
                    this.phqueren.Visible = false;
                    //this.phsave.Visible = false;
                }
                if (SiteUserInfo.UserType == EyouSoft.Model.EnumType.CompanyStructure.UserType.票务用户)
                {
                    this.SupperControl1.HideID = SiteUserInfo.SupplierCompanyId;
                    var sModel = new EyouSoft.BLL.SourceStructure.BSupplier().GetModel(SiteUserInfo.SupplierCompanyId);
                    this.SupperControl1.Name = sModel == null ? string.Empty : sModel.UnitName;
                    this.SupperControl1.IsEnable = false;
                    this.PhCencer.Visible = false;
                    this.PhCheck.Visible = false;
                }
                else
                {
                    this.SellsSelect1.SellsName = SiteUserInfo.Name;
                    this.SellsSelect1.SellsID = SiteUserInfo.UserId.ToString();
                }
            }


            //初始化收付款方式
            PayTypeOption = UtilsCommons.GetEnumDDL(EyouSoft.Common.EnumObj.GetList(typeof(EyouSoft.Model.EnumType.FinStructure.ShouFuKuanFangShi)), type, false);
            this.UploadControl1.CompanyID = SiteUserInfo.CompanyId;
        }

        /// <summary>
        /// 保存
        /// </summary>
        /// <param name="id"></param>
        /// <param name="dotype"></param>
        /// <returns></returns>
        private string PageSave(string id, string dotype)
        {
            string msg = string.Empty;
            //t为true 新增，false 修改
            bool t = string.IsNullOrEmpty(id) && dotype == "add";
            EyouSoft.BLL.PlanStructure.BPlanTicket bll = new EyouSoft.BLL.PlanStructure.BPlanTicket();
            EyouSoft.Model.PlanStructure.MPlanTicketInfo model = new EyouSoft.Model.PlanStructure.MPlanTicketInfo();
            model.TourCode = Utils.GetFormValue(this.TourSelect1.ClientTourCode);
            model.TourId = Utils.GetFormValue(this.TourSelect1.ClientTourID);
            model.AdultPrice = Utils.GetDecimal(Utils.GetFormValue(this.txtadultprice.UniqueID));
            model.Adults = Utils.GetInt(Utils.GetFormValue(this.txtadults.UniqueID));
            model.Brokerage = Utils.GetDecimal(Utils.GetFormValue(this.txtBrokerage.UniqueID));
            model.ChildPrice = Utils.GetDecimal(Utils.GetFormValue(this.txtchildprice.UniqueID));
            model.Childs = Utils.GetInt(Utils.GetFormValue(this.txtchilds.UniqueID));
            model.CompanyId = SiteUserInfo.CompanyId;
            model.GysId = Utils.GetFormValue(this.SupperControl1.ClientValue);
            model.GysName = Utils.GetFormValue(this.SupperControl1.ClientText);
            model.Interval = Utils.GetFormValue(this.txtqujian.UniqueID);
            model.IssueTime = DateTime.Now;
            model.OperatorId = SiteUserInfo.UserId;
            model.OtherPrice = Utils.GetDecimal(Utils.GetFormValue(this.txtOtherPrice.UniqueID));
            model.PayType = (EyouSoft.Model.EnumType.FinStructure.ShouFuKuanFangShi)Utils.GetInt(Utils.GetFormValue(this.hdpaytype.UniqueID));
            model.SumPrice = Utils.GetDecimal(Utils.GetFormValue(this.txtSumpirce.UniqueID));
            model.Ticketer = Utils.GetFormValue(this.SellsSelect1.SellsNameClient);
            model.TicketerId = Utils.GetInt(Utils.GetFormValue(this.SellsSelect1.SellsIDClient));
            if (Utils.GetFormValue(this.hdticketmode.UniqueID) == "0")
                model.TicketMode = EyouSoft.Model.EnumType.PlanStructure.TicketMode.出票;
            else
                model.TicketMode = EyouSoft.Model.EnumType.PlanStructure.TicketMode.退票;
            model.TicketType = (EyouSoft.Model.EnumType.PlanStructure.TicketType)Utils.GetInt(Utils.GetFormValue(this.dptticket.UniqueID));
            model.TrafficNumber = Utils.GetFormValue(this.txtcheci.UniqueID);
            model.TrafficSeat = Utils.GetFormValue(this.txtTrafficSeat.UniqueID);
            model._TrafficSeat = (EyouSoft.Model.EnumType.PlanStructure.TrafficSeat?)Utils.GetInt(Utils.GetFormValue(this.ddlTrafficSeat.UniqueID));

            model.TrafficTime = Utils.GetDateTimeNullable(Utils.GetFormValue(this.txttime.UniqueID) + ' ' + Utils.GetFormValue("sltHH") + ":" + Utils.GetFormValue("sltmm"));
            model.TravellerList = OrderCustomer1.GetCustomerList();
            model.IsMonth = Utils.GetFormValue(this.ddlyuejie.UniqueID) == "1" ? true : false;
            if (Utils.GetFormValue(this.ddlyuejie.UniqueID) == "1")
            {
                model.MonthTime = Utils.GetDateTimeNullable(Utils.GetFormValue(this.txtjiesuantime.UniqueID));
            }

            #region 附件上传
            string[] UploadFile = Utils.GetFormValues(this.UploadControl1.ClientHideID);
            string[] oldUploadfile = Utils.GetFormValues("hidefile");
            IList<MFile> filelist = new List<EyouSoft.Model.TourStructure.MFile>();
            if (UploadFile.Length > 0)
            {
                for (int i = 0; i < UploadFile.Length; i++)
                {
                    if (UploadFile[i].Trim() != "")
                    {
                        EyouSoft.Model.TourStructure.MFile fileModel = new EyouSoft.Model.TourStructure.MFile();
                        fileModel.FilePath = UploadFile[i].ToString().Split('|')[1].ToString();
                        fileModel.FileName = UploadFile[i].ToString().Split('|')[0].ToString();
                        fileModel.Id = "";
                        filelist.Add(fileModel);
                    }
                }
            }
            model.FileList = filelist;

            #endregion
            //新增
            if (t)
            {
                model.TicketStatus = EyouSoft.Model.EnumType.PlanStructure.TicketStatus.已申请;
                int result = bll.Add(model);
                if (result == 1)
                {
                    msg = UtilsCommons.AjaxReturnJson("1", "新增成功!");
                }
                else
                {
                    msg = UtilsCommons.AjaxReturnJson("0", "新增失败!");
                }
            }
            else
            {
                model.PlanId = id;
                model.TicketStatus = EyouSoft.Model.EnumType.PlanStructure.TicketStatus.已申请;
                int result = 0;
                if (dotype == "queren")
                {
                    result = bll.Update(model);
                    result = bll.Update(id, EyouSoft.Model.EnumType.PlanStructure.TicketStatus.已确认);
                    result = bll.Update(id, EyouSoft.Model.EnumType.PlanStructure.TicketStatus.已出票);
                }
                if (dotype == "cencer")
                {
                    result = bll.Update(id, EyouSoft.Model.EnumType.PlanStructure.TicketStatus.已确认);
                }
                if (dotype == "check")
                {
                    result = bll.Update(model);
                    result = bll.Update(id, EyouSoft.Model.EnumType.PlanStructure.TicketStatus.已出票);
                }

                switch (result)
                {
                    case 1:
                        if (dotype == "queren")
                        {
                            msg = UtilsCommons.AjaxReturnJson("1", "已确认!");
                        }
                        else if (dotype == "check")
                        {
                            msg = UtilsCommons.AjaxReturnJson("1", "已审核!");
                        }
                        else if (dotype == "cercer")
                        {
                            msg = UtilsCommons.AjaxReturnJson("1", "已取消审核!");
                        }
                        else
                            msg = UtilsCommons.AjaxReturnJson("1", "操作成功!");
                        break;
                    case 0:
                        if (dotype == "queren")
                        {
                            msg = UtilsCommons.AjaxReturnJson("0", "确认失败!");
                        }
                        else if (dotype == "check")
                        {
                            msg = UtilsCommons.AjaxReturnJson("0", "审核失败!");
                        }
                        else if (dotype == "cercer")
                        {
                            msg = UtilsCommons.AjaxReturnJson("0", "取消审核失败!");
                        }
                        else
                            msg = UtilsCommons.AjaxReturnJson("0", "操作失败!");
                        break;
                    case -1:
                        msg = UtilsCommons.AjaxReturnJson("0", "出票、退票状态审核后不能修改!");
                        break;
                    case -2:
                        msg = UtilsCommons.AjaxReturnJson("0", "供应商不存在!");
                        break;
                    default:
                        msg = UtilsCommons.AjaxReturnJson("0", "操作失败!");
                        break;
                }
            }
            return msg;
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

        /// <summary>
        /// 权限判断
        /// </summary>
        protected void PowerControl()
        {
            if (!this.CheckGrant(EyouSoft.Model.EnumType.PrivsStructure.Privs3.计调中心_票务安排_出票申请) && !this.CheckGrant(EyouSoft.Model.EnumType.PrivsStructure.Privs3.计调中心_票务安排_退票申请))
            {
                this.phshenqing.Visible = false;
            }
            if (!this.CheckGrant(EyouSoft.Model.EnumType.PrivsStructure.Privs3.财务管理_应付管理_票务审核))
            {
                this.PhCheck.Visible = false;
                this.PhCencer.Visible = false;
            }
        }
    }
}
