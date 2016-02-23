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
using EyouSoft.Common.Page;
using System.Collections.Generic;
using EyouSoft.Model.TourStructure;
using EyouSoft.Model.PlanStructure;

namespace Web.jidiaoCenter
{
    /// <summary>
    /// 刘飞
    /// 创建时间：2012-12-20
    /// 功能描述：新增/修改 确认件登记
    /// </summary>
    public partial class TeamEdit : BackPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            PowerControl();
            this.UploadControl1.CompanyID = SiteUserInfo.CompanyId;
            string tourid = Utils.GetQueryStringValue("tourid");
            string dotype = Utils.GetQueryStringValue("dotype").Trim();
            string type = Utils.GetQueryStringValue("type").Trim();
            switch (type)
            {
                case "save":
                    Response.Clear();
                    Response.Write(PageSave(tourid, dotype));
                    Response.End();
                    break;
                default:
                    break;
            }
            if (!IsPostBack)
            {
                PageInit(tourid, dotype);
            }

        }
        /// <summary>
        /// 初始化
        /// </summary>
        /// <param name="tourid">团队编号</param>
        /// <param name="dotype">修改/新增</param>
        private void PageInit(string tourid, string dotype)
        {
            if (!string.IsNullOrEmpty(tourid) && dotype.Trim() == "update")
            {
                EyouSoft.BLL.TourStructure.BTour bll = new EyouSoft.BLL.TourStructure.BTour();
                EyouSoft.Model.TourStructure.MTourInfo model = new EyouSoft.Model.TourStructure.MTourInfo();
                model = bll.GetModel(tourid);
                if (model != null)
                {
                    if (model.GuideList != null && model.GuideList.Count > 0)
                    {
                        this.DaoyouControl1.SetGuideList = model.GuideList;
                    }
                    this.txtAdultCount.Text = model.Adults.ToString();
                    this.txtBackDate.Text = model.RDate.HasValue ? Utils.GetDateTime(model.RDate.ToString()).ToString("yyyy-MM-dd") : "";
                    this.txtChildCount.Text = model.Childs.ToString();
                    this.txtjiesuantime.Text = model.MonthTime.HasValue ? Utils.GetDateTime(model.MonthTime.ToString()).ToString("yyy-MM-dd") : "";
                    this.txtLeaveDate.Text = model.LDate.HasValue ? Utils.GetDateTime(model.LDate.ToString()).ToString("yyyy-MM-dd") : "";
                    this.txtQuanCount.Text = model.Accompanys.ToString();
                    this.txtTotalMoney.Text = model.SumPrice.ToString("f2");
                    this.txtReamrk.Text = model.Remark;
                    this.txtTourcode.Text = model.TourCode;

                    if (model.FileList != null && model.FileList.Count > 0)
                    {
                        this.rplfile.DataSource = model.FileList;
                        this.rplfile.DataBind();
                    }
                    if (ddlchupiao.Items.FindByValue(Convert.ToInt32(model.IsChuPiao).ToString()) != null)
                        ddlchupiao.Items.FindByValue(Convert.ToInt32(model.IsChuPiao).ToString()).Selected = true;
                    if (ddlyuejie.Items.FindByValue(Convert.ToInt32(model.IsMonth).ToString()) != null)
                        ddlyuejie.Items.FindByValue(Convert.ToInt32(model.IsMonth).ToString()).Selected = true;
                    if (ddltype.Items.FindByValue(((int)model.TourType).ToString()) != null)
                        ddltype.Items.FindByValue(((int)model.TourType).ToString()).Selected = true;
                    if (!model.IsMonth)
                    {
                        this.spanjisuantime.Attributes.Add("style", "display:none");
                        this.txtjiesuantime.Attributes.Remove("valid");
                    }
                    this.SellsSelect1.SellsID = model.SaleId.ToString();
                    this.SellsSelect1.SellsName = model.SaleName;
                    this.RouteSelect1.InitRouteId = model.RouteId;
                    this.RouteSelect1.InitRouteName = model.RouteName;
                    this.CustomerUnit1.InitCustomerId = model.BuyCompanyId;
                    this.CustomerUnit1.InitCustomerName = model.BuyCompnayName;
                    string isFin = Utils.GetQueryStringValue("isfin");
                    if (isFin != "")
                    {
                        this.txtprofit.Text = model.Profit.ToString("f2");
                        this.txtcount.Text = model.RebatePeople.ToString();
                        this.txtmoney.Text = model.RebatePrice.ToString("f2");
                        this.btn.Visible = false;
                        if (!this.CheckGrant(EyouSoft.Model.EnumType.PrivsStructure.Privs3.计调中心_确认件登记_返佣))
                        {
                            this.PhFanyong.Visible = false;
                        }
                        if (!this.CheckGrant(EyouSoft.Model.EnumType.PrivsStructure.Privs3.计调中心_确认件登记_毛利))
                        {
                            this.PhMaoli.Visible = false;
                        }
                        if (model.TourStatus == EyouSoft.Model.EnumType.TourStructure.TourStatus.未处理)
                        {
                            this.phcencer.Visible = false;
                        }
                        else
                        {
                            this.phcheck.Visible = false;
                        }
                        if (model.IsEnd)
                        {
                            this.PhBack.Visible = true;
                            this.btn.Visible = false;
                            this.PhEnd.Visible = false;
                            this.phcencer.Visible = false;
                            this.phcheck.Visible = false;
                        }
                        else
                        {
                            this.PhBack.Visible = false;
                        }
                    }
                    else
                    {
                        this.PhBack.Visible = false;
                        this.PhEnd.Visible = false;
                        this.Phfin.Visible = false;
                        this.phcheck.Visible = false;
                        this.phcencer.Visible = false;
                        if (model.IsEnd)
                        {
                            this.btn.Visible = false;
                        }
                    }
                }
            }
            else
            {
                this.PhEnd.Visible = false;
                this.PhBack.Visible = false;
                this.Phfin.Visible = false;
                this.phcencer.Visible = false;
                this.phcheck.Visible = false;
            }



            this.txtTourcode.Attributes.Add("readonly", "readonly");
            this.txtTourcode.Attributes.Add("style", "background-color:#dadada");
        }
        /// <summary>
        /// 保存
        /// </summary>
        /// <param name="tourid">团队编号</param>
        /// <param name="dotype">修改/新增</param>
        /// <returns></returns>
        private string PageSave(string tourid, string dotype)
        {
            //t为true 新增，false 修改
            bool t = string.IsNullOrEmpty(tourid) && dotype == "add";
            string msg = UtilsCommons.AjaxReturnJson("0", "操作失败!");
            EyouSoft.BLL.TourStructure.BTour bll = new EyouSoft.BLL.TourStructure.BTour();
            EyouSoft.Model.TourStructure.MTourInfo model = new EyouSoft.Model.TourStructure.MTourInfo();

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

            #region 实体赋值
            model.Accompanys = Utils.GetInt(Utils.GetFormValue(this.txtQuanCount.UniqueID));
            model.Adults = Utils.GetInt(Utils.GetFormValue(this.txtAdultCount.UniqueID));
            model.BuyCompanyId = Utils.GetFormValue(this.CustomerUnit1.HidClientName);
            model.BuyCompnayName = Utils.GetFormValue("txtCustomerName");
            model.Childs = Utils.GetInt(Utils.GetFormValue(this.txtChildCount.UniqueID));
            model.CompanyId = SiteUserInfo.CompanyId;
            model.GuideList = UtilsCommons.GetGuidetData();
            model.IsChuPiao = Utils.GetFormValue(this.ddlchupiao.UniqueID) == "1" ? true : false;
            model.IsMonth = Utils.GetFormValue(this.ddlyuejie.UniqueID) == "1" ? true : false;
            model.IssueTime = DateTime.Now;
            model.LDate = Utils.GetDateTimeNullable(Utils.GetFormValue(this.txtLeaveDate.UniqueID));
            if (Utils.GetFormValue(this.ddlyuejie.UniqueID) == "1")
            {
                model.MonthTime = Utils.GetDateTimeNullable(Utils.GetFormValue(this.txtjiesuantime.UniqueID));
            }
            model.OperatorId = SiteUserInfo.UserId;
            model.RDate = Utils.GetDateTimeNullable(Utils.GetFormValue(this.txtBackDate.UniqueID));
            if (model.RDate < model.LDate)
            {
                return UtilsCommons.AjaxReturnJson("0", "回团时间必须大于出团时间");
            }
            model.RouteId = Utils.GetFormValue(this.RouteSelect1.HidClientName);
            model.RouteName = Utils.GetFormValue("txtRouteName");
            model.SaleId = Utils.GetInt(Utils.GetFormValue(this.SellsSelect1.SellsIDClient));
            model.SaleName = Utils.GetFormValue(this.SellsSelect1.SellsNameClient);
            model.SumPrice = Utils.GetDecimal(Utils.GetFormValue(this.txtTotalMoney.UniqueID));
            model.Remark = Utils.GetFormValue(this.txtReamrk.UniqueID);
            model.TourType = (EyouSoft.Model.EnumType.TourStructure.TourType)Utils.GetInt(Utils.GetFormValue(this.ddltype.UniqueID));
            if (this.CheckIsRoute.Checked == true)
            {
                model.IsRouteHuiTian = true;
            }
            else
            {
                model.IsRouteHuiTian = false;
            }
            #endregion
            //新增
            if (t)
            {
                model.TourStatus = EyouSoft.Model.EnumType.TourStructure.TourStatus.未处理;
                int result = bll.Add(model);
                int resultdijie = 0;
                EyouSoft.BLL.PlanStructure.BPlanDiJie blldijie = new EyouSoft.BLL.PlanStructure.BPlanDiJie();
                MPlanDiJieInfo modeldijie = new MPlanDiJieInfo();

                modeldijie.CompanyId = SiteUserInfo.CompanyId;
                modeldijie.IsMonth = false;
                modeldijie.PayType = EyouSoft.Model.EnumType.FinStructure.ShouFuKuanFangShi.财务现付;
                modeldijie.SumPrice = 0;
                modeldijie.IssueTime = DateTime.Now;
                modeldijie.OperatorId = SiteUserInfo.UserId;
                var tmpModel = bll.GetModel(model.TourId);
                modeldijie.TourCode = tmpModel == null ? string.Empty : tmpModel.TourCode;
                modeldijie.TourId = model.TourId;
                modeldijie.DiJieStatus = EyouSoft.Model.EnumType.PlanStructure.DiJieStatus.申请中;
                modeldijie.GysId = string.Empty;
                modeldijie.GysName = string.Empty;

                resultdijie = blldijie.Add(modeldijie);
                if (resultdijie != 1)
                {
                    msg = UtilsCommons.AjaxReturnJson("0", "回填地接失败!");
                }
                if (model.IsChuPiao)
                {
                    EyouSoft.BLL.PlanStructure.BPlanTicket bllticket = new EyouSoft.BLL.PlanStructure.BPlanTicket();
                    EyouSoft.Model.PlanStructure.MPlanTicketInfo modelticket = new EyouSoft.Model.PlanStructure.MPlanTicketInfo();

                    modelticket.TourCode = tmpModel == null ? string.Empty : tmpModel.TourCode;
                    modelticket.TourId = model.TourId;
                    modelticket.Adults = model.Adults;
                    modelticket.Childs = model.Childs;
                    modelticket.CompanyId = SiteUserInfo.CompanyId;
                    modelticket.IssueTime = DateTime.Now;
                    modelticket.OperatorId = SiteUserInfo.UserId;
                    modelticket.PayType = EyouSoft.Model.EnumType.FinStructure.ShouFuKuanFangShi.财务现付;
                    modelticket.SumPrice = 0M;
                    modelticket.Ticketer = SiteUserInfo.Name;
                    modelticket.TicketerId = SiteUserInfo.UserId;
                    modelticket.TicketMode = EyouSoft.Model.EnumType.PlanStructure.TicketMode.出票;
                    modelticket.TicketType = EyouSoft.Model.EnumType.PlanStructure.TicketType.机票;
                    modelticket.IsMonth = false;
                    modelticket.TicketStatus = EyouSoft.Model.EnumType.PlanStructure.TicketStatus.已申请;
                    modelticket.GysId = string.Empty;
                    modelticket.GysName = string.Empty;
                    modelticket._TrafficSeat = EyouSoft.Model.EnumType.PlanStructure.TrafficSeat.经济舱;

                    int resultTicket = 0;
                    resultTicket = bllticket.Add(modelticket);
                    if (resultTicket != 1)
                    {
                        msg = UtilsCommons.AjaxReturnJson("0", "回填票务失败!");
                    }
                }
                switch (result)
                {
                    case 1:
                        msg = UtilsCommons.AjaxReturnJson("1", "新增成功!");
                        break;
                    case 0:
                        msg = UtilsCommons.AjaxReturnJson("0", "新增失败!");
                        break;
                    default:
                        break;
                }
            }
            else
            {
                int result = 0;
                if (dotype == "end")
                {
                    result = bll.Update_End(tourid);
                    //确认登记件已操作结束
                }
                if (dotype == "back")
                {
                    result = bll.Update_Back(tourid);
                    switch (result)
                    {
                        case 0:
                            msg = UtilsCommons.AjaxReturnJson("0", "操作失败");
                            break;
                        case 1:
                            msg = UtilsCommons.AjaxReturnJson("1", "已退回计调");
                            break;
                        case -1:
                            msg = UtilsCommons.AjaxReturnJson("0", "确认登记件已退回计调");
                            break;
                        default:
                            break;
                    }
                    return msg;
                }
                if (dotype == "cencer")
                {
                    result = bll.Update(tourid, EyouSoft.Model.EnumType.TourStructure.TourStatus.未处理);
                    switch (result)
                    {
                        case 0:
                            msg = UtilsCommons.AjaxReturnJson("0", "操作失败");
                            break;
                        case 1:
                            msg = UtilsCommons.AjaxReturnJson("1", "取消成功");
                            break;
                        case -1:
                            msg = UtilsCommons.AjaxReturnJson("0", "团队操作已结束,无法取消");
                            break;
                        default:
                            break;
                    }
                    return msg;
                }
                if (dotype == "update")
                {
                    if (Utils.GetQueryStringValue("isfin") == "")
                    {
                        model.TourId = tourid;
                        result = bll.Update(model);
                        //确认登记件财务已操作结束 无法修改
                    }
                    else
                    {
                        model.Profit = Utils.GetDecimal(Utils.GetFormValue(this.txtprofit.UniqueID));
                        model.RebatePeople = Utils.GetInt(Utils.GetFormValue(this.txtcount.UniqueID));
                        model.RebatePrice = Utils.GetDecimal(Utils.GetFormValue(this.txtmoney.UniqueID));
                        model.TourId = tourid;
                        model.ConfirmOperatorId = SiteUserInfo.UserId;
                        result = bll.Update_Fin(model);
                        switch (result)
                        {
                            case 0:
                                msg = UtilsCommons.AjaxReturnJson("0", "审核失败");
                                break;
                            case 1:
                                msg = UtilsCommons.AjaxReturnJson("1", "审核成功");
                                break;
                            case -1:
                                msg = UtilsCommons.AjaxReturnJson("0", "确认登记件财务已操作结束 无法修改");
                                break;
                            default:
                                break;
                        }
                        return msg;
                    }
                }
                switch (result)
                {
                    case 1:
                        if (dotype == "end")
                            msg = UtilsCommons.AjaxReturnJson("1", "已结束!");
                        else
                            msg = UtilsCommons.AjaxReturnJson("1", "修改成功!");
                        break;
                    case 0:
                        msg = UtilsCommons.AjaxReturnJson("0", "操作失败!");
                        break;
                    case -1:
                        msg = UtilsCommons.AjaxReturnJson("0", "确认登记件财务已操作结束 无法修改！");
                        break;
                    default:
                        break;
                }
            }
            return msg;
        }

        /// <summary>
        /// 权限判断
        /// </summary>
        protected void PowerControl()
        {
            if (!this.CheckGrant(EyouSoft.Model.EnumType.PrivsStructure.Privs3.计调中心_确认件登记_新增))
            {
                this.btn.Visible = false;
            }
            if (!this.CheckGrant(EyouSoft.Model.EnumType.PrivsStructure.Privs3.计调中心_确认件登记_修改))
            {
                this.btn.Visible = false;
            }
            if (!this.CheckGrant(EyouSoft.Model.EnumType.PrivsStructure.Privs3.财务管理_应收管理_修改))
            {
                this.PhBack.Visible = false;
                this.PhEnd.Visible = false;
                this.phcheck.Visible = false;
                this.phcencer.Visible = false;
            }
        }
    }
}
