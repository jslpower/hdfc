using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using EyouSoft.Common;

namespace Web.jidiaoCenter
{
    public partial class TourDataAdd : EyouSoft.Common.Page.BackPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            string tourdataid = Utils.GetQueryStringValue("tourdataid");
            string dotype = Utils.GetQueryStringValue("dotype").Trim();
            string type = Utils.GetQueryStringValue("type").Trim();

            PowerControl(dotype);
            switch (type)
            {
                case "save":
                    Response.Clear();
                    Response.Write(PageSave(tourdataid, dotype));
                    Response.End();
                    break;
                default:
                    break;
            }
            if (!IsPostBack)
            {
                PageInit(tourdataid, dotype);
            }
        }


        private void PageInit(string tourdataid, string dotype)
        {
            EyouSoft.BLL.CompanyStructure.Area bll = new EyouSoft.BLL.CompanyStructure.Area();
            IList<EyouSoft.Model.CompanyStructure.Area> list = bll.GetAreaByCompanyId(CurrentUserCompanyID);
            if (list != null && list.Count != 0)
            {
                this.ddlArea.DataSource = list;
                this.ddlArea.DataTextField = "AreaName";
                this.ddlArea.DataValueField = "Id";
                this.ddlArea.DataBind();
            }
            this.ddlArea.Items.Insert(0, new ListItem("请选择", ""));

            //支付方式

            //ShouFuKuanFangShi
            Array payvalues = Enum.GetValues(typeof(EyouSoft.Model.EnumType.TourStructure.TourDataType));
            foreach (var item in payvalues)
            {
                int value = (int)Enum.Parse(typeof(EyouSoft.Model.EnumType.TourStructure.TourDataType), item.ToString());
                string text = Enum.GetName(typeof(EyouSoft.Model.EnumType.TourStructure.TourDataType), item);

                this.ddlTourDataType.Items.Add(new ListItem(text, value.ToString()));
            }

            ddlTourDataType.Items.Insert(0, new ListItem("-请选择-", ""));

            this.UploadControl1.CompanyID = CurrentUserCompanyID;
            this.UploadControl1.IsUploadMore = true;
            this.UploadControl1.IsUploadSelf = true;

            this.txtOperatorName.Value = SiteUserInfo.Name;
            this.txtIssueTime.Value = ToDateTimeString(DateTime.Now);

            if (!string.IsNullOrEmpty(tourdataid) && dotype.Trim() == "update")
            {
                EyouSoft.BLL.TourStructure.BTourData tourdatabll = new EyouSoft.BLL.TourStructure.BTourData();
                EyouSoft.Model.TourStructure.MTourData model = tourdatabll.GetModel(Utils.GetInt(tourdataid));
                if (model != null)
                {
                    this.ddlArea.SelectedValue = model.AreaId.ToString();
                    this.txtRouteName.Value = model.RouteName;
                    this.ddlTourDataType.SelectedValue = ((int)model.TourDataType).ToString();
                    this.txtTourPort.Value = model.TourPort;
                    this.txtIssueTime.Value = ToDateTimeString(model.IssueTime);
                    this.txtOperatorName.Value = model.OperatorName;


                    if (model.FileList != null && model.FileList.Count != 0)
                    {
                        this.rplfile.DataSource = model.FileList;
                        this.rplfile.DataBind();
                    }

                    if (model.IsCheck == true)
                    {
                        this.btn.Visible = false;
                    }
                }

            }

        }


        /// <summary>
        /// 保存
        /// </summary>
        /// <param name="tourdataid"></param>
        /// <param name="dotype"></param>
        /// <returns></returns>
        protected string PageSave(string tourdataid, string dotype)
        {
            bool t = string.IsNullOrEmpty(tourdataid) && dotype == "add";
            string msg = string.Empty;

            EyouSoft.Model.TourStructure.MTourData model = new EyouSoft.Model.TourStructure.MTourData();
            model.TourDataId = Utils.GetInt(tourdataid);
            model.AreaId = Utils.GetInt(Utils.GetFormValue(this.ddlArea.UniqueID));
            model.RouteName = Utils.GetFormValue(this.txtRouteName.UniqueID);
            model.TourDataType = (EyouSoft.Model.EnumType.TourStructure.TourDataType)Utils.GetInt(Utils.GetFormValue(this.ddlTourDataType.UniqueID));
            model.TourPort = Utils.GetFormValue(this.txtTourPort.UniqueID);

            model.CompanyId = CurrentUserCompanyID;
            model.OperatorId = SiteUserInfo.UserId;
            model.IssueTime = DateTime.Now;

            #region 文件上传

            IList<EyouSoft.Model.TourStructure.MFile> filelist = new List<EyouSoft.Model.TourStructure.MFile>();

            //文件
            string[] UploadFile = Utils.GetFormValues(this.UploadControl1.ClientHideID);

            if (UploadFile.Length > 0)
            {
                for (int i = 0; i < UploadFile.Length; i++)
                {
                    if (UploadFile[i].Trim() != "")
                    {
                        EyouSoft.Model.TourStructure.MFile fileModel = new EyouSoft.Model.TourStructure.MFile();
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
                        EyouSoft.Model.TourStructure.MFile fileModel = new EyouSoft.Model.TourStructure.MFile();
                        fileModel.FilePath = OldFile[i].ToString().Split('|')[1].ToString();
                        fileModel.FileName = OldFile[i].ToString().Split('|')[0].ToString();

                        filelist.Add(fileModel);
                    }
                }
            }

            model.FileList = filelist;

            #endregion
            EyouSoft.BLL.TourStructure.BTourData bll = new EyouSoft.BLL.TourStructure.BTourData();
            if (t)
            {

                if (bll.Add(model) == 1)
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
                int flg = bll.Update(model);
                if (flg == 1)
                {
                    msg = UtilsCommons.AjaxReturnJson("1", "修改成功!");
                }
                else if (flg == -1)
                {
                    msg = UtilsCommons.AjaxReturnJson("0", "该资料已审核，不允许修改!");
                }
                else
                {
                    msg = UtilsCommons.AjaxReturnJson("0", "修改失败!");
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
                if (!this.CheckGrant(EyouSoft.Model.EnumType.PrivsStructure.Privs3.计调中心_团队报价资料库_新增) && dotype.Equals("add"))
                {
                    this.btn.Visible = false;
                }

                if (!this.CheckGrant(EyouSoft.Model.EnumType.PrivsStructure.Privs3.计调中心_团队报价资料库_修改) && dotype.Equals("update"))
                {
                    this.btn.Visible = false;
                }
            }
        }


    }




}
