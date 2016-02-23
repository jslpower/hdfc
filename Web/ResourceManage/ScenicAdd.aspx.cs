using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using EyouSoft.Common.Page;
using EyouSoft.Common;
using EyouSoft.Common.Function;

namespace Web.ResourceManage
{
    /// <summary>
    /// 景点增加
    /// 作者：王磊
    /// </summary>
    public partial class ScenicAdd : BackPage
    {
        protected string Province = "0";
        protected string City = "0";
        protected void Page_Load(object sender, EventArgs e)
        {
            
            string id = Utils.GetQueryStringValue("tid");
            string dotype = Utils.GetQueryStringValue("dotype").Trim();
            string type = Utils.GetQueryStringValue("type").Trim();

            PowerControl(dotype);

            this.UploadControl1.CompanyID = SiteUserInfo.CompanyId;
            this.UploadControl1.IsUploadMore = true;
            this.UploadControl1.IsUploadSelf = true;
            this.UploadControl1.FileTypes = "*.xls;*.xlsx;*.rar;*.zip;*.7z;*.pdf;*.doc;*.docx;*.dot;*.swf;";


            this.UploadControl2.CompanyID = SiteUserInfo.CompanyId;
            this.UploadControl2.IsUploadMore = true;
            this.UploadControl2.IsUploadSelf = true;
            this.UploadControl2.FileTypes = "*.jpg;*.gif;*.jpeg;*.png;*.bmp";

            this.Contact1.IsAccount = false;
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
            //绑定菜系
            Array values = Enum.GetValues(typeof(EyouSoft.Model.EnumType.SourceStructure.SpotStar));
            foreach (var item in values)
            {
                int value = (int)Enum.Parse(typeof(EyouSoft.Model.EnumType.SourceStructure.SpotStar), item.ToString());
                string text = Enum.GetName(typeof(EyouSoft.Model.EnumType.SourceStructure.SpotStar), item);
                ListItem ddlItem = new ListItem();
                ddlItem.Text = text;
                ddlItem.Value = value.ToString();
                this.ddlStar.Items.Add(ddlItem);
            }
            this.ddlStar.Items.Insert(0, new ListItem("--请选择--", ""));


            if (id != "" && dotype != "add")
            {
                EyouSoft.BLL.SourceStructure.BSpotSupplier bll = new EyouSoft.BLL.SourceStructure.BSpotSupplier();
                EyouSoft.Model.SourceStructure.MSpotSupplier model = bll.GetModel(id);
                if (model != null)
                {
                    Province = model.ProvinceId.ToString();
                    City = model.CityId.ToString();
                    txtunitname.Value = model.UnitName;
                    ddlStar.SelectedValue = ((int)model.SpotStar).ToString();
                    txtAddress.Value = model.UnitAddress;
                    txtTourGuide.Value = model.TourGuide;
                    txtremark.Value = model.Remark;
                    txtStorePrice.Value = model.StorePrice.ToString("f2");
                    txtWJPrice.Value = model.WJPrice.ToString("f2");
                    txtDJPrice.Value = model.DJPrice.ToString("f2");
                    txtZKPrice.Value = model.ZKPrice.ToString("f2");

                    if (model.ContactList != null && model.ContactList.Count > 0)
                    {
                        this.Contact1.SetTravelList = model.ContactList;
                    }
                    if (model.FileList != null && model.FileList.Count > 0)
                    {
                        this.rplfile.DataSource = model.FileList.Where(c => c.FileMode == EyouSoft.Model.EnumType.SourceStructure.FileMode.文件);
                        this.rplfile.DataBind();

                        this.rpPic.DataSource = model.FileList.Where(c => c.FileMode == EyouSoft.Model.EnumType.SourceStructure.FileMode.图片);
                        this.rpPic.DataBind();
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
            EyouSoft.BLL.SourceStructure.BSpotSupplier bll = new EyouSoft.BLL.SourceStructure.BSpotSupplier();
            EyouSoft.Model.SourceStructure.MSpotSupplier model = new EyouSoft.Model.SourceStructure.MSpotSupplier();

            #region 文件附件上传
            IList<EyouSoft.Model.SourceStructure.MFileInfo> filelist = new List<EyouSoft.Model.SourceStructure.MFileInfo>();

            //文件
            string[] UploadFile = Utils.GetFormValues(this.UploadControl1.ClientHideID);

            if (UploadFile.Length > 0)
            {
                for (int i = 0; i < UploadFile.Length; i++)
                {
                    if (UploadFile[i].Trim() != "")
                    {
                        EyouSoft.Model.SourceStructure.MFileInfo fileModel = new EyouSoft.Model.SourceStructure.MFileInfo();
                        fileModel.FilePath = UploadFile[i].ToString().Split('|')[1].ToString();
                        fileModel.FileName = UploadFile[i].ToString().Split('|')[0].ToString();
                        fileModel.FileMode = EyouSoft.Model.EnumType.SourceStructure.FileMode.文件;
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
                        EyouSoft.Model.SourceStructure.MFileInfo fileModel = new EyouSoft.Model.SourceStructure.MFileInfo();
                        fileModel.FilePath = OldFile[i].ToString().Split('|')[1].ToString();
                        fileModel.FileName = OldFile[i].ToString().Split('|')[0].ToString();
                        fileModel.FileMode = EyouSoft.Model.EnumType.SourceStructure.FileMode.文件;
                        filelist.Add(fileModel);
                    }
                }
            }

            //图片
            string[] UploadPic = Utils.GetFormValues(this.UploadControl2.ClientHideID);

            if (UploadPic.Length > 0)
            {
                for (int i = 0; i < UploadPic.Length; i++)
                {
                    if (UploadPic[i].Trim() != "")
                    {
                        EyouSoft.Model.SourceStructure.MFileInfo fileModel = new EyouSoft.Model.SourceStructure.MFileInfo();
                        fileModel.FilePath = UploadPic[i].ToString().Split('|')[1].ToString();
                        fileModel.FileName = UploadPic[i].ToString().Split('|')[0].ToString();
                        fileModel.FileMode = EyouSoft.Model.EnumType.SourceStructure.FileMode.图片;
                        filelist.Add(fileModel);
                    }
                }
            }

            //旧图片
            string[] OldPic = Utils.GetFormValues("hidPicPath");
            if (OldPic.Length > 0)
            {
                for (int i = 0; i < OldPic.Length; i++)
                {
                    if (OldPic[i].Trim() != "")
                    {
                        EyouSoft.Model.SourceStructure.MFileInfo fileModel = new EyouSoft.Model.SourceStructure.MFileInfo();
                        fileModel.FilePath = OldPic[i].ToString().Split('|')[1].ToString();
                        fileModel.FileName = OldPic[i].ToString().Split('|')[0].ToString();
                        fileModel.FileMode = EyouSoft.Model.EnumType.SourceStructure.FileMode.图片;
                        filelist.Add(fileModel);
                    }
                }
            }

            model.FileList = filelist;

            #endregion


            model.ProvinceId = Utils.GetInt(Utils.GetFormValue(ddlProvice.UniqueID));
            model.CityId = Utils.GetInt(Utils.GetFormValue(ddlCity.UniqueID));
            model.CompanyId = SiteUserInfo.CompanyId;
            model.UnitName = Utils.GetFormValue(txtunitname.UniqueID);
            model.SpotStar = (EyouSoft.Model.EnumType.SourceStructure.SpotStar)Utils.GetInt(Utils.GetFormValue(ddlStar.UniqueID));
            model.UnitAddress = Utils.GetFormValue(txtAddress.UniqueID);
            model.TourGuide = Utils.GetFormValue(txtTourGuide.UniqueID);
            model.ContactList = Contact1.GetDateList();
            model.Remark = Utils.GetFormValue(this.txtremark.UniqueID);
            model.IssueTime = DateTime.Now;
            model.OperatorId = SiteUserInfo.UserId;
            model.SupplierType = EyouSoft.Model.EnumType.CompanyStructure.SupplierType.景点;

            model.StorePrice = Utils.GetDecimal(Utils.GetFormValue(txtStorePrice.UniqueID));
            model.WJPrice = Utils.GetDecimal(Utils.GetFormValue(txtWJPrice.UniqueID));
            model.DJPrice = Utils.GetDecimal(Utils.GetFormValue(txtDJPrice.UniqueID));
            model.ZKPrice = Utils.GetDecimal(Utils.GetFormValue(txtZKPrice.UniqueID));
            model.ZKPrice = Utils.GetDecimal(Utils.GetFormValue(txtZKPrice.UniqueID));

            int result = 0;
            if (t)
            {
                result = bll.Add(model);
            }
            else
            {
                model.Id = id;
                result = bll.Update(model);
            }
            switch (result)
            {
                case 0:
                    msg = UtilsCommons.AjaxReturnJson("0", (t ? "新增" : "修改") + "失败");
                    break;
                case 1:
                    msg = UtilsCommons.AjaxReturnJson("1", (t ? "新增" : "修改") + "成功");
                    break;
                default:
                    break;
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
                if (!this.CheckGrant(EyouSoft.Model.EnumType.PrivsStructure.Privs3.供应商管理_餐馆_新增)&&dotype.Equals("add"))
                {
                    this.btn.Visible = false;
                }
                if (!this.CheckGrant(EyouSoft.Model.EnumType.PrivsStructure.Privs3.供应商管理_餐馆_修改) && dotype.Equals("update"))
                {
                    this.btn.Visible = false;
                }
            }

        }






    }
}
