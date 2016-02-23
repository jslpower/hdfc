using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using EyouSoft.Common;

namespace Web.Fin
{
    public partial class OtherIncomeAdd : EyouSoft.Common.Page.BackPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            PowerControl();
            string id = Utils.GetQueryStringValue("tid");
            string dotype = Utils.GetQueryStringValue("dotype").Trim();
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



        /// <summary>
        /// 绑定要修改数据
        /// </summary>
        /// <param name="tid"></param>
        private void PageInit(string id, string dotype)
        {


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



            this.hidShouKuanRen.Value = SiteUserInfo.Name;
            this.hidShouKuanRenId.Value = SiteUserInfo.UserId.ToString();
            this.txtShouKuanRen.Value = SiteUserInfo.Name;


            if (id != "" && dotype != "add")
            {
                EyouSoft.BLL.FinStructure.BQiTaShouKuan qiTaShouKuanBll = new EyouSoft.BLL.FinStructure.BQiTaShouKuan();
                EyouSoft.Model.FinStructure.MQiTaShouKuan model = qiTaShouKuanBll.GetFinCope(id);
                if (model != null)
                {
                    this.txtDate.Value = ToDateTimeString(model.ShouKuanRiQi);
                    this.txtPayItem.Value = model.ItemName;
                    this.txtPrice.Value = model.JinE.ToString("f2");
                    this.hidShouKuanRen.Value = model.ShouKuanRenName;
                    this.hidShouKuanRenId.Value = model.ShouKuanRenId.ToString();
                    this.txtShouKuanRen.Value = model.ShouKuanRenName;

                    this.ddlBank.SelectedValue = model.ZhangHuId;
                    this.ddlIsKaiPiao.SelectedValue = model.IsKaiPiao ? "1" : "0";
                    this.ddlPayType.SelectedValue = ((int)model.FangShi).ToString();

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
            EyouSoft.BLL.FinStructure.BQiTaShouKuan bll = new EyouSoft.BLL.FinStructure.BQiTaShouKuan();

            EyouSoft.Model.FinStructure.MQiTaShouKuan model = new EyouSoft.Model.FinStructure.MQiTaShouKuan();

            #region 文件附件上传
            IList<EyouSoft.Model.FinStructure.MKuanFile> filelist = new List<EyouSoft.Model.FinStructure.MKuanFile>();

            //文件
            string[] UploadFile = Utils.GetFormValues(this.UploadControl1.ClientHideID);

            if (UploadFile.Length > 0)
            {
                for (int i = 0; i < UploadFile.Length; i++)
                {
                    if (UploadFile[i].Trim() != "")
                    {
                        EyouSoft.Model.FinStructure.MKuanFile fileModel = new EyouSoft.Model.FinStructure.MKuanFile();
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
                        EyouSoft.Model.FinStructure.MKuanFile fileModel = new EyouSoft.Model.FinStructure.MKuanFile();
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
            model.ShouKuanRiQi = Utils.GetDateTime(Utils.GetFormValue(this.txtDate.UniqueID));
            model.ItemName = Utils.GetFormValue(this.txtPayItem.UniqueID);
            model.JinE = Utils.GetDecimal(Utils.GetFormValue(this.txtPrice.UniqueID));
            model.ShouKuanRenId = Utils.GetInt(Utils.GetFormValue(this.hidShouKuanRenId.UniqueID));
            model.ShouKuanRenName = Utils.GetFormValue(this.hidShouKuanRen.UniqueID);
            model.ZhangHuId = Utils.GetFormValue(this.ddlBank.UniqueID);
            model.FangShi = (EyouSoft.Model.EnumType.FinStructure.ShouFuKuanFangShi)Utils.GetInt(Utils.GetFormValue(this.ddlPayType.UniqueID));
            model.IsKaiPiao = Utils.GetFormValue(this.ddlIsKaiPiao.UniqueID) == "1";



            model.IsKaiPiao = Utils.GetFormValue(this.ddlIsKaiPiao.UniqueID) == "1";
            model.IssueTime = DateTime.Now;
            model.OperatorId = SiteUserInfo.UserId;



            int result = 0;
            if (t)
            {
                /// 1：成功；
                /// 0：参数错误；
                /// -1：收款总登记和超过应收；
                /// -2：添加失败
                result = bll.AddFinCope(model);
                switch (result)
                {
                    case -2:
                    case 0:
                        msg = UtilsCommons.AjaxReturnJson("0", "新增失败");
                        break;
                    case 1:
                        msg = UtilsCommons.AjaxReturnJson("1", "新增成功");
                        break;
                    case -1:
                        msg = UtilsCommons.AjaxReturnJson("0", "付款总登记和超过应付,新增失败");
                        break;
                    default:
                        break;
                }
            }
            else
            {
                model.DengJiId = id;
                /// 1：成功；
                /// 0：参数错误；
                /// -1：收款总登记和超过应收；
                /// -2：修改失败；
                result = bll.UpdateFinCope(model);
                switch (result)
                {
                    case 0:
                    case -2:
                        msg = UtilsCommons.AjaxReturnJson("0", "修改失败");
                        break;
                    case 1:
                        msg = UtilsCommons.AjaxReturnJson("1", "修改成功");
                        break;
                    case -1:
                        msg = UtilsCommons.AjaxReturnJson("0", "收款总登记和超过应收.修改失败");
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
            if (!this.CheckGrant(EyouSoft.Model.EnumType.PrivsStructure.Privs3.财务管理_其他收入_新增))
            {
                this.btn.Visible = false;
            }

        }
    }
}

