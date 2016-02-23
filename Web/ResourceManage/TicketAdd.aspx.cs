using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using EyouSoft.Common.Function;
using EyouSoft.Common;
using EyouSoft.Common.Page;

namespace Web.ResourceManage
{
    public partial class TicketAdd : BackPage
    {

        #region form信息
        /// <summary>
        /// 省份编号
        /// </summary>
        protected int Province = 0;
        /// <summary>
        /// 城市编号
        /// </summary>
        protected int City = 0;
        #endregion

        protected void Page_Load(object sender, EventArgs e)
        {

            string id = Utils.GetQueryStringValue("tid");
            string dotype = Utils.GetQueryStringValue("dotype").Trim();
            string type = Utils.GetQueryStringValue("type").Trim();

            PowerControl(dotype);

            this.UploadControl1.CompanyID = SiteUserInfo.CompanyId;
            this.UploadControl1.IsUploadMore = true;
            this.UploadControl1.IsUploadSelf = true;
            this.ContactAndAccount1.IsAccount = true;
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
            if (!string.IsNullOrEmpty(id))
            {
                EyouSoft.BLL.SourceStructure.BSupplier bll = new EyouSoft.BLL.SourceStructure.BSupplier();

                EyouSoft.Model.SourceStructure.MSupplier model = bll.GetModel(id);
                if (model == null) return;
                Province = model.ProvinceId;
                City = model.CityId;
                this.unionname.Value = model.UnitName;
                this.txtAddress.Value = model.UnitAddress;
                this.police.InnerText = model.UnitPolicy;
                this.remark.InnerText = model.Remark;

                if (model.ContactList != null && model.ContactList.Count > 0)
                {
                    this.ContactAndAccount1.SetTravelList = model.ContactList;
                }

                if (model.FileList != null && model.FileList.Count > 0)
                {
                    this.rplfile.DataSource = model.FileList;
                    this.rplfile.DataBind();
                }
            }

            if (dotype.Equals("show"))
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

            EyouSoft.BLL.SourceStructure.BSupplier bll = new EyouSoft.BLL.SourceStructure.BSupplier();
            EyouSoft.Model.SourceStructure.MSupplier model = new EyouSoft.Model.SourceStructure.MSupplier();
            model.CompanyId = CurrentUserCompanyID;
            model.SupplierType = EyouSoft.Model.EnumType.CompanyStructure.SupplierType.票务;
            model.ProvinceId = Utils.GetInt(Utils.GetFormValue(this.ddlProvice.UniqueID));
            model.CityId = Utils.GetInt(Utils.GetFormValue(this.ddlCity.UniqueID));
            model.UnitName = Utils.GetFormValue(this.unionname.UniqueID);
            model.UnitAddress = Utils.GetFormValue(this.txtAddress.UniqueID);
            model.UnitPolicy = Utils.GetFormValue(this.police.UniqueID);
            model.Remark = Utils.GetFormValue(this.remark.UniqueID);
            model.CompanyId = this.SiteUserInfo.CompanyId;
            model.OperatorId = this.SiteUserInfo.UserId;

            #region  获取联系人
            this.ContactAndAccount1.Usertype = EyouSoft.Model.EnumType.CompanyStructure.UserType.票务用户;
            model.ContactList = ContactAndAccount1.GetDateList();
            #endregion

            #region 附件上传
            IList<EyouSoft.Model.SourceStructure.MFileInfo> filelist = new List<EyouSoft.Model.SourceStructure.MFileInfo>();

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


            model.FileList = filelist;

            #endregion

            //账号存在的集合
            IList<string> list = new List<string>();


            int result = 0;
            if (t)
            {
                result = bll.Add(model, ref list);
            }
            else
            {
                model.Id = id;
                result = bll.Update(model, ref list);
            }
            switch (result)
            {
                case 0:
                    msg = UtilsCommons.AjaxReturnJson("0", (t ? "新增" : "修改") + "失败");
                    break;
                case 1:
                    msg = UtilsCommons.AjaxReturnJson("1", (t ? "新增" : "修改") + "成功");
                    break;
                case -1:
                    msg = UtilsCommons.AjaxReturnJson("0", "超过最大分配账号数");
                    break;
                case -101:
                    // string user = Newtonsoft.Json.JsonConvert.SerializeObject(list);
                    if (list.Count != 0)
                    {
                        foreach (string s in list)
                        {
                            msg += s + "</br>";
                        }
                    }
                    msg = UtilsCommons.AjaxReturnJson("-101", msg + " 账号在系统中存在！</br>操作失败!");
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
                if (!this.CheckGrant(EyouSoft.Model.EnumType.PrivsStructure.Privs3.供应商管理_票务_新增) && dotype.Equals("add"))
                {
                    this.btn.Visible = false;
                }
                if (!this.CheckGrant(EyouSoft.Model.EnumType.PrivsStructure.Privs3.供应商管理_票务_修改) && dotype.Equals("update"))
                {
                    this.btn.Visible = false;
                }
            }

        }


    }
}
