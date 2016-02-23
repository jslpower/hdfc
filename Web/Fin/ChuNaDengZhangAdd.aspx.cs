using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using EyouSoft.Common;

namespace Web.Fin
{
    public partial class ChuNaDengZhangAdd : EyouSoft.Common.Page.BackPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {

            string id = Utils.GetQueryStringValue("tid");
            string dotype = Utils.GetQueryStringValue("dotype").Trim();
            string type = Utils.GetQueryStringValue("type").Trim();

            PowerControl(dotype);

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



        /// <summary>
        /// 绑定要修改数据
        /// </summary>
        /// <param name="tid"></param>
        private void PageInit(string id, string dotype)
        {
            //绑定团号的选择
            //EyouSoft.BLL.TourStructure.BTour tourbll = new EyouSoft.BLL.TourStructure.BTour();
            //IList<EyouSoft.Model.TourStructure.MTourInfo> list = tourbll.GetList(SiteUserInfo.CompanyId, tourcode);
            //if (list != null && list.Count > 0)
            //{
            //    recordcount = list.Count;
            //    this.RptList.DataSource = list;
            //    this.RptList.DataBind();
            //}

            //绑定收款/付款
            Array values = Enum.GetValues(typeof(EyouSoft.Model.EnumType.FinStructure.DengJiMode));
            foreach (var item in values)
            {
                int value = (int)Enum.Parse(typeof(EyouSoft.Model.EnumType.FinStructure.DengJiMode), item.ToString());
                string text = Enum.GetName(typeof(EyouSoft.Model.EnumType.FinStructure.DengJiMode), item);

                this.ddlStatus.Items.Add(new ListItem(text, value.ToString()));
            }

            this.ddlStatus.Items.Insert(0, new ListItem("-请选择-", ""));

            //绑定账号
            EyouSoft.BLL.CompanyStructure.BYinHangZhangHu bll = new EyouSoft.BLL.CompanyStructure.BYinHangZhangHu();
            ddlBank.Items.Clear();
            IList<EyouSoft.Model.CompanyStructure.CompanyAccount> list = bll.GetZhangHus(CurrentUserCompanyID);
            if (list != null && list.Count != 0)
            {
                foreach (var t in list)
                {
                    if (t == null) continue;
                    ddlBank.Items.Add(
                        new ListItem(string.Format("{0}-{1}-{2}", t.BankName, t.AccountName, t.BankNo), t.Id));
                }
            }
            ddlBank.Items.Insert(0, new ListItem("-请选择-", ""));


            //支付方式

            //ShouFuKuanFangShi
            Array payvalues = Enum.GetValues(typeof(EyouSoft.Model.EnumType.FinStructure.ShouFuKuanFangShi));
            foreach (var item in payvalues)
            {
                int value = (int)Enum.Parse(typeof(EyouSoft.Model.EnumType.FinStructure.ShouFuKuanFangShi), item.ToString());
                string text = Enum.GetName(typeof(EyouSoft.Model.EnumType.FinStructure.ShouFuKuanFangShi), item);

                this.ddlPayType.Items.Add(new ListItem(text, value.ToString()));
            }

            ddlPayType.Items.Insert(0, new ListItem("-请选择-", ""));


            this.UploadControl1.CompanyID = SiteUserInfo.CompanyId;
            this.UploadControl1.IsUploadMore = true;
            this.UploadControl1.IsUploadSelf = true;

            this.hidDengZhangPeople.Value = SiteUserInfo.Name;
            this.hidDengZhangPeopleId.Value = SiteUserInfo.UserId.ToString();
            this.txtDengZhangPeople.Value = SiteUserInfo.Name;


            if (id != "" && dotype != "add")
            {
                EyouSoft.BLL.FinStructure.BChuNaDengZhang dengZhangBll = new EyouSoft.BLL.FinStructure.BChuNaDengZhang();
                EyouSoft.Model.FinStructure.MChuNaDengZhang model = dengZhangBll.GetChuNaDengZhang(id);
                if (model != null)
                {
                    this.ddlStatus.SelectedValue = ((int)model.DengZhangType).ToString();
                    this.TourSelect1.TourCode = model.TourNo;
                    // this.TourSelect1.TourId
                    this.hidDengZhangPeople.Value = model.DengZhangPeople;
                    this.hidDengZhangPeopleId.Value = model.DengZhangPeopleId.ToString();
                    this.txtDengZhangPeople.Value = model.DengZhangPeople;
                    this.txtDate.Value = ToDateTimeString(model.DengZhangDate);
                    this.txtPrice.Value = model.Price.ToString("f2");
                    this.ddlBank.SelectedValue = model.BankId;
                    this.ddlPayType.SelectedValue = ((int)model.PayMode).ToString();
                    this.txtReason.Value = model.Reason;
                    this.txtOtherPrice.Value = model.OtherPrice.ToString("f2");
                    this.ddlIsKaiPiao.Value = model.IsKaiPiao ? "1" : "0";

                    if (model.File != null && model.File.Count > 0)
                    {
                        this.rpFile.DataSource = model.File;
                        this.rpFile.DataBind();
                    }
                }
            }
            if (dotype == "show")
            {
                this.btn.Visible = false;
            }
        }


        /// <summary>
        /// 保存或修改信息
        /// </summary>
        private string PageSave(string id, string dotype)
        {
            //t为true 新增，false 修改
            bool t = string.IsNullOrEmpty(id) && dotype == "add";
            string msg = string.Empty;
            EyouSoft.BLL.FinStructure.BChuNaDengZhang bll = new EyouSoft.BLL.FinStructure.BChuNaDengZhang();
            EyouSoft.Model.FinStructure.MChuNaDengZhang model = new EyouSoft.Model.FinStructure.MChuNaDengZhang();

            #region 文件附件上传
            IList<EyouSoft.Model.FinStructure.MChuNaDengZhangFile> filelist = new List<EyouSoft.Model.FinStructure.MChuNaDengZhangFile>();

            //文件
            string[] UploadFile = Utils.GetFormValues(this.UploadControl1.ClientHideID);

            if (UploadFile.Length > 0)
            {
                for (int i = 0; i < UploadFile.Length; i++)
                {
                    if (UploadFile[i].Trim() != "")
                    {
                        EyouSoft.Model.FinStructure.MChuNaDengZhangFile fileModel = new EyouSoft.Model.FinStructure.MChuNaDengZhangFile();
                        fileModel.FilePath = UploadFile[i].ToString().Split('|')[1].ToString();
                        fileModel.FileName = UploadFile[i].ToString().Split('|')[0].ToString();
                        filelist.Add(fileModel);
                    }
                }
            }

            //旧文件
            string[] OldFile = Utils.GetFormValues("hidFilePath");
            if (OldFile.Length > 0)
            {
                for (int i = 0; i < OldFile.Length; i++)
                {
                    if (OldFile[i].Trim() != "")
                    {
                        EyouSoft.Model.FinStructure.MChuNaDengZhangFile fileModel = new EyouSoft.Model.FinStructure.MChuNaDengZhangFile();
                        fileModel.FilePath = OldFile[i].ToString().Split('|')[1].ToString();
                        fileModel.FileName = OldFile[i].ToString().Split('|')[0].ToString();
                        fileModel.FileId = Utils.GetInt(OldFile[i].ToString().Split('|')[2].ToString());
                        filelist.Add(fileModel);
                    }
                }
            }

            model.File = filelist;

            #endregion

            model.CompanyId = CurrentUserCompanyID;
            model.DengZhangType = (EyouSoft.Model.EnumType.FinStructure.DengJiMode)Utils.GetInt(Utils.GetFormValue(this.ddlStatus.UniqueID));
            model.TourNo = Utils.GetFormValue(this.TourSelect1.ClientTourCode);
            model.DengZhangPeopleId = Utils.GetInt(Utils.GetFormValue(this.hidDengZhangPeopleId.UniqueID));
            model.DengZhangPeople = Utils.GetFormValue(this.hidDengZhangPeople.UniqueID);
            model.DengZhangDate = Utils.GetDateTime(Utils.GetFormValue(this.txtDate.UniqueID));
            model.Price = Utils.GetDecimal(Utils.GetFormValue(this.txtPrice.UniqueID));
            model.BankId = Utils.GetFormValue(this.ddlBank.UniqueID);
            model.PayMode = (EyouSoft.Model.EnumType.FinStructure.ShouFuKuanFangShi)Utils.GetInt(Utils.GetFormValue(this.ddlPayType.UniqueID));
            model.Reason = Utils.GetFormValue(this.txtReason.UniqueID);
            model.OtherPrice = Utils.GetDecimal(Utils.GetFormValue(this.txtOtherPrice.UniqueID));
            model.IsKaiPiao = Utils.GetFormValue(this.ddlIsKaiPiao.UniqueID) == "1";
            model.IssueTime = DateTime.Now;
            model.OperatorId = SiteUserInfo.UserId;



            int result = 0;
            if (t)
            {
                /// 1：成功；
                /// 0：参数错误；
                /// -1：添加失败；
                result = bll.AddChuNaDengZhang(model);

                switch (result)
                {
                    case 0:
                    case -1:
                        msg = UtilsCommons.AjaxReturnJson("0", "新增失败");
                        break;
                    case 1:
                        msg = UtilsCommons.AjaxReturnJson("1", "新增成功");
                        break;
                    default:
                        break;
                }

            }
            else
            {
                /// 1：成功；
                /// 0：参数错误；
                /// -1：不是用户登记的数据，不能修改；
                /// -2：修改失败；
                model.DengZhangId = id;
                result = bll.UpdateChuNaDengZhang(model);

                switch (result)
                {
                    case -2:
                    case 0:
                        msg = UtilsCommons.AjaxReturnJson("0", "修改失败");
                        break;
                    case 1:
                        msg = UtilsCommons.AjaxReturnJson("1", "修改成功");
                        break;
                    case -1:
                        msg = UtilsCommons.AjaxReturnJson("0", "不是用户登记的数据，不能修改");
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
        protected void PowerControl(string dotype)
        {
            if (!string.IsNullOrEmpty(dotype))
            {
                if (!this.CheckGrant(EyouSoft.Model.EnumType.PrivsStructure.Privs3.财务管理_出纳登账_新增) && dotype.Equals("add"))
                {
                    this.btn.Visible = false;
                }

                if (!this.CheckGrant(EyouSoft.Model.EnumType.PrivsStructure.Privs3.财务管理_出纳登账_修改) && dotype.Equals("update"))
                {
                    this.btn.Visible = false;
                }
            }

        }
    }
}
