using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using EyouSoft.Common;
using EyouSoft.Common.Function;

namespace Web.ResourceManage
{
    public partial class GuideAdd : EyouSoft.Common.Page.BackPage
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

            this.UploadControl1.FileTypes = "*.jpg;*.gif;*.jpeg;*.png;*.bmp";

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
            //绑定导游星级
            Array values = Enum.GetValues(typeof(EyouSoft.Model.EnumType.SourceStructure.GuideStar));
            foreach (var item in values)
            {
                int value = (int)Enum.Parse(typeof(EyouSoft.Model.EnumType.SourceStructure.GuideStar), item.ToString());
                string text = Enum.GetName(typeof(EyouSoft.Model.EnumType.SourceStructure.GuideStar), item);
                ListItem ddlItem = new ListItem();
                ddlItem.Text = text;
                ddlItem.Value = value.ToString();
                this.ddlStar.Items.Add(ddlItem);
            }
            this.ddlStar.Items.Insert(0, new ListItem("--请选择--", ""));


            if (id != "" && dotype != "add")
            {
                EyouSoft.BLL.SourceStructure.BGuideSupplier bll = new EyouSoft.BLL.SourceStructure.BGuideSupplier();
                EyouSoft.Model.SourceStructure.MGuideSupplier model = bll.GetModel(id);
                if (model != null)
                {
                    Province = model.ProvinceId.ToString();
                    City = model.CityId.ToString();

                    txtGysName.Value = model.GysName;
                    txtGuideName.Value = model.GuideName;
                    txtMobile.Value = model.Phone;
                    txtBirthday.Value = this.ToDateTimeString(model.Birthday);
                    txtTourTime.Value = model.TourTime;
                    ddlStar.SelectedValue = ((int)model.GuideStar).ToString();
                    txtBelongs.Value = model.Belongs;
                    txtRemark.Value = model.Remark;

                    if (model.FileList != null && model.FileList.Count > 0)
                    {
                        this.rplfile.DataSource = model.FileList;
                        this.rplfile.DataBind();


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
            EyouSoft.BLL.SourceStructure.BGuideSupplier bll = new EyouSoft.BLL.SourceStructure.BGuideSupplier();
            EyouSoft.Model.SourceStructure.MGuideSupplier model = new EyouSoft.Model.SourceStructure.MGuideSupplier();

           
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
                        fileModel.FileMode = EyouSoft.Model.EnumType.SourceStructure.FileMode.图片;
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
            model.GuideName = Utils.GetFormValue(txtGuideName.UniqueID);

            model.GysName = Utils.GetFormValue(txtGysName.UniqueID);

            model.Phone = Utils.GetFormValue(txtMobile.UniqueID);
            model.Birthday = Utils.GetDateTimeNullable(Utils.GetFormValue(txtBirthday.UniqueID));
            model.TourTime = Utils.GetFormValue(txtTourTime.UniqueID);
            model.GuideStar = (EyouSoft.Model.EnumType.SourceStructure.GuideStar)Utils.GetInt(Utils.GetFormValue(ddlStar.UniqueID));
            model.Belongs = Utils.GetFormValue(txtBelongs.UniqueID);
            model.Remark = Utils.GetFormValue(txtRemark.UniqueID);
            model.OperatorId = SiteUserInfo.UserId;

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
                if (!this.CheckGrant(EyouSoft.Model.EnumType.PrivsStructure.Privs3.供应商管理_导游_新增)&&dotype.Equals("add"))
                {
                    this.btn.Visible = false;
                }
                if (!this.CheckGrant(EyouSoft.Model.EnumType.PrivsStructure.Privs3.供应商管理_导游_修改)&&dotype.Equals("update"))
                {
                    this.btn.Visible = false;
                }
            }
        }

    }
}
