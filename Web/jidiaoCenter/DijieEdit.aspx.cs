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
using EyouSoft.Model.PlanStructure;
using System.Collections.Generic;

namespace Web.jidiaoCenter
{
    /// <summary>
    /// 创建人：刘飞
    /// 时间：2013-1-7
    /// 描述:新增/修改地接安排
    /// </summary>
    public partial class DijieEdit : EyouSoft.Common.Page.BackPage
    {
        protected string PayTypeOption = string.Empty;
        protected void Page_Load(object sender, EventArgs e)
        {
            PowerControl();
            string dotype = Utils.GetQueryStringValue("dotype");
            string id = Utils.GetQueryStringValue("id");
            string type = Utils.GetQueryStringValue("type").Trim();
            switch (type)
            {
                case "save":
                    Response.Clear();
                    Response.Write(PageSave(id, dotype));
                    Response.End();
                    break;
                default:
                    break;
            }
            if (!IsPostBack)
            {
                PageInit(id, dotype);
            }
        }
        private void PageInit(string id, string dotype)
        {
            string isfin = Utils.GetQueryStringValue("isfin");
            string type = "";
            this.UploadControl1.CompanyID = SiteUserInfo.CompanyId;

            if (!string.IsNullOrEmpty(id) && dotype == "update")
            {
                EyouSoft.BLL.PlanStructure.BPlanDiJie bll = new EyouSoft.BLL.PlanStructure.BPlanDiJie();
                MPlanDiJieInfo model = bll.GetModel(id);
                if (model != null)
                {
                    this.txtcar.Text = model.Car.ToString("f2");
                    this.txtcount.Text = model.Head.ToString("f2");
                    this.txtdao.Text = model.Guide.ToString("f2");
                    this.txtdinner.Text = model.Dining.ToString("f2");
                    this.txtdoor.Text = model.Ticket.ToString("f2");
                    this.txthouse.Text = model.Hotel.ToString("f2");
                    this.txtjiadian.Text = model.AddPrice.ToString("f2");
                    this.txtOther.Text = model.Other.ToString("f2");
                    this.txtRemark.Text = model.Remark;
                    this.txtTotalMoney.Text = model.SumPrice.ToString("f2");
                    this.txttraffic.Text = model.Traffic.ToString("f2");
                    this.txtxianfu.Text = model.GuidePay.ToString("f2");
                    this.txtxianshou.Text = model.GuideIncome.ToString("f2");
                    this.TourSelect1.TourCode = model.TourCode;
                    this.TourSelect1.TourId = model.TourId;
                    this.TourSelect1.IsEnable = false;
                    this.hdpaytype.Value = ((int)(model.PayType)).ToString();
                    type = ((int)model.PayType).ToString();
                    this.txtjiesuantime.Text = model.MonthTime.HasValue ? Utils.GetDateTime(model.MonthTime.ToString()).ToString("yyy-MM-dd") : "";
                    if (ddlyuejie.Items.FindByValue(Convert.ToInt32(model.IsMonth).ToString()) != null)
                        ddlyuejie.Items.FindByValue(Convert.ToInt32(model.IsMonth).ToString()).Selected = true;
                    if (!model.IsMonth)
                    {
                        this.spanjisuantime.Attributes.Add("style", "display:none");
                        this.txtjiesuantime.Attributes.Remove("valid");
                    }
                    if (model.DiJieStatus == EyouSoft.Model.EnumType.PlanStructure.DiJieStatus.申请中)
                    {
                        this.phshenqing.Visible = false;
                        this.PhCencer.Visible = false;
                        this.PhCheck.Visible = false;

                    }
                    if (model.DiJieStatus == EyouSoft.Model.EnumType.PlanStructure.DiJieStatus.已确认)
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
                    if (model.DiJieStatus == EyouSoft.Model.EnumType.PlanStructure.DiJieStatus.已审核)
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
                    if (model.FileList != null && model.FileList.Count > 0)
                    {
                        this.rplfile.DataSource = model.FileList;
                        this.rplfile.DataBind();
                    }
                    if (SiteUserInfo.UserType == EyouSoft.Model.EnumType.CompanyStructure.UserType.地接用户)
                    {
                        this.SupperControl1.HideID = SiteUserInfo.SupplierCompanyId;
                        var sModel = new EyouSoft.BLL.SourceStructure.BSupplier().GetModel(SiteUserInfo.SupplierCompanyId);
                        this.SupperControl1.Name = sModel == null ? string.Empty : sModel.UnitName;
                        this.SupperControl1.IsEnable = false;
                    }
                    else
                    {
                        this.SupperControl1.HideID = model.GysId;
                        this.SupperControl1.Name = model.GysName;
                    }
                }
            }
            else
            {
                if (isfin == "")
                {
                    this.phqueren.Visible = false;
                    //this.phsave.Visible = false;
                    this.PhCheck.Visible = false;
                    this.PhCencer.Visible = false;
                }
            }
            if (SiteUserInfo.UserType == EyouSoft.Model.EnumType.CompanyStructure.UserType.地接用户)
            {
                this.SupperControl1.HideID = SiteUserInfo.SupplierCompanyId;
                var sModel = new EyouSoft.BLL.SourceStructure.BSupplier().GetModel(SiteUserInfo.SupplierCompanyId);
                this.SupperControl1.Name = sModel == null ? string.Empty : sModel.UnitName;
                this.SupperControl1.IsEnable = false;
                this.SupperControl1.SupplierType = EyouSoft.Model.EnumType.CompanyStructure.SupplierType.地接;

                this.PhCencer.Visible = false;
                this.PhCheck.Visible = false;
            }
            //初始化收付款方式
            PayTypeOption = UtilsCommons.GetEnumDDL(EyouSoft.Common.EnumObj.GetList(typeof(EyouSoft.Model.EnumType.FinStructure.ShouFuKuanFangShi)), type, false);

        }
        /// <summary>
        /// 保存
        /// </summary>
        /// <param name="id"></param>
        /// <param name="dotype"></param>
        /// <returns></returns>
        private string PageSave(string id, string dotype)
        {
            //t为true 新增，false 修改
            bool t = string.IsNullOrEmpty(id) && dotype == "add";
            string msg = string.Empty;
            EyouSoft.BLL.PlanStructure.BPlanDiJie bll = new EyouSoft.BLL.PlanStructure.BPlanDiJie();
            MPlanDiJieInfo model = new MPlanDiJieInfo();

            #region 获取表单
            decimal car = Utils.GetDecimal(Utils.GetFormValue(this.txtcar.UniqueID));
            decimal count = Utils.GetDecimal(Utils.GetFormValue(this.txtcount.UniqueID));
            decimal daoyou = Utils.GetDecimal(Utils.GetFormValue(this.txtdao.UniqueID));
            decimal dinner = Utils.GetDecimal(Utils.GetFormValue(this.txtdinner.UniqueID));
            decimal door = Utils.GetDecimal(Utils.GetFormValue(this.txtdoor.UniqueID));
            decimal house = Utils.GetDecimal(Utils.GetFormValue(this.txthouse.UniqueID));
            decimal jiadian = Utils.GetDecimal(Utils.GetFormValue(this.txtjiadian.UniqueID));
            decimal other = Utils.GetDecimal(Utils.GetFormValue(this.txtOther.UniqueID));
            decimal totalmoney = Utils.GetDecimal(Utils.GetFormValue(this.txtTotalMoney.UniqueID));
            decimal traffic = Utils.GetDecimal(Utils.GetFormValue(this.txttraffic.UniqueID));
            decimal xianfu = Utils.GetDecimal(Utils.GetFormValue(this.txtxianfu.UniqueID));
            decimal xianshou = Utils.GetDecimal(Utils.GetFormValue(this.txtxianshou.UniqueID));
            string remark = Utils.GetFormValue(this.txtRemark.UniqueID);
            string tourcode = Utils.GetFormValue(this.TourSelect1.ClientTourCode);
            string tourid = Utils.GetFormValue(this.TourSelect1.ClientTourID);
            int paytype = Utils.GetInt(Utils.GetFormValue(this.hdpaytype.UniqueID));
            string sourceid = Utils.GetFormValue(this.SupperControl1.ClientValue);
            string sourcename = Utils.GetFormValue(this.SupperControl1.ClientText);
            #endregion

            #region 附件上传
            string[] UploadFile = Utils.GetFormValues(this.UploadControl1.ClientHideID);
            string[] oldUploadfile = Utils.GetFormValues("hidefile");
            IList<EyouSoft.Model.TourStructure.MFile> filelist = new List<EyouSoft.Model.TourStructure.MFile>();
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
            model.AddPrice = jiadian;
            model.Car = car;
            model.CompanyId = SiteUserInfo.CompanyId;

            model.Dining = dinner;
            model.Guide = daoyou;
            model.GuideIncome = xianshou;
            model.GuidePay = xianfu;
            model.GysId = sourceid;
            model.GysName = sourcename;
            model.Head = count;
            model.Hotel = house;
            model.IssueTime = DateTime.Now;
            model.OperatorId = SiteUserInfo.UserId;
            model.Other = other;
            model.PayType = (EyouSoft.Model.EnumType.FinStructure.ShouFuKuanFangShi)paytype;
            model.Remark = remark;
            model.SumPrice = totalmoney;
            model.Ticket = door;
            model.TourCode = tourcode;
            model.TourId = tourid;
            model.Traffic = traffic;
            model.IsMonth = Utils.GetFormValue(this.ddlyuejie.UniqueID) == "1" ? true : false;
            if (Utils.GetFormValue(this.ddlyuejie.UniqueID) == "1")
            {
                model.MonthTime = Utils.GetDateTimeNullable(Utils.GetFormValue(this.txtjiesuantime.UniqueID));
            }
            MPlanDiJieInfo PlanModel = new MPlanDiJieInfo();
            PlanModel = model;
            #endregion

            #region 保存
            if (t)
            {
                model.DiJieStatus = EyouSoft.Model.EnumType.PlanStructure.DiJieStatus.申请中;
                int result = bll.Add(PlanModel);
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
                model.PlanId = id;
                int result = 0;
                if (dotype == "queren")
                {
                    result = bll.Update(PlanModel);
                    result = bll.Update(id, EyouSoft.Model.EnumType.PlanStructure.DiJieStatus.已确认);
                    result = bll.Update(id, EyouSoft.Model.EnumType.PlanStructure.DiJieStatus.已审核);
                }
                if (dotype == "cencer")
                {
                    result = bll.Update(id, EyouSoft.Model.EnumType.PlanStructure.DiJieStatus.已确认);
                }
                if (dotype == "check")
                {
                    result = bll.Update(PlanModel);
                    result = bll.Update(id, EyouSoft.Model.EnumType.PlanStructure.DiJieStatus.已审核);
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
                        else if (dotype == "cencer")
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
                        else
                            msg = UtilsCommons.AjaxReturnJson("0", "操作失败!");
                        break;
                    case -1:
                        msg = UtilsCommons.AjaxReturnJson("0", "地接状态审核后不能进行修改操作!");
                        break;
                    case -2:
                        msg = UtilsCommons.AjaxReturnJson("0", "供应商不存在!");
                        break;
                    default:
                        msg = UtilsCommons.AjaxReturnJson("0", "操作失败!");
                        break;
                }
            }
            #endregion
            return msg;
        }

        /// <summary>
        /// 权限判断
        /// </summary>
        protected void PowerControl()
        {
            if (!this.CheckGrant(EyouSoft.Model.EnumType.PrivsStructure.Privs3.计调中心_地接安排_新增))
            {
                this.phshenqing.Visible = false;
            }
            if (!this.CheckGrant(EyouSoft.Model.EnumType.PrivsStructure.Privs3.计调中心_地接安排_修改))
            {
                //this.phsave.Visible = false;
            }
            if (!this.CheckGrant(EyouSoft.Model.EnumType.PrivsStructure.Privs3.计调中心_地接安排_确认))
            {
                this.phqueren.Visible = false;
            }
            if (!this.CheckGrant(EyouSoft.Model.EnumType.PrivsStructure.Privs3.财务管理_应付管理_地接审核))
            {
                this.PhCheck.Visible = false;
                this.PhCencer.Visible = false;
            }

        }
    }
}
