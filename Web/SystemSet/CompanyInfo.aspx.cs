using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using EyouSoft.Common;
using EyouSoft.Common.Page;
using EyouSoft.BLL.CompanyStructure;
using EyouSoft.Common.Function;
using EyouSoft.Model.CompanyStructure;

namespace Web.SystemSet
{
    public partial class CompanyInfo : BackPage
    {

        protected void Page_Load(object sender, EventArgs e)
        {
            #region 权限判断
            if (!CheckGrant(global::EyouSoft.Model.EnumType.PrivsStructure.Privs3.系统设置_公司信息_栏目))
            {
                Utils.ResponseNoPermit(global::EyouSoft.Model.EnumType.PrivsStructure.Privs3.系统设置_公司信息_栏目, true);
                return;
            }
            #endregion

            PageInit();
        }
        /// <summary>
        /// 页面初始化
        /// </summary>
        protected void PageInit()
        {
            this.UploadControl1.CompanyID = this.SiteUserInfo.CompanyId;
            this.UploadControl1.IsUploadMore = true;
            this.UploadControl1.IsUploadSelf = true;
            EyouSoft.BLL.CompanyStructure.CompanyInfo companyBll = new EyouSoft.BLL.CompanyStructure.CompanyInfo();
            EyouSoft.Model.CompanyStructure.CompanyInfo infoModel = null;//公司信息实体
            string method = Utils.GetFormValue("hidMethod");
            if (method == "save")
            {
                #region 保存公司信息
                if (Utils.InputText(txtCompanyName.Value) == "")
                {
                    MessageBox.Show(this, "公司名称不为空");
                    return;
                }
                //保存
                EyouSoft.Model.CompanyStructure.CompanyAccount account = new EyouSoft.Model.CompanyStructure.CompanyAccount();//公司账户


                #region 附件上传
                string[] UploadFile = Utils.GetFormValues(this.UploadControl1.ClientHideID);
                string[] oldUploadfile = Utils.GetFormValues("hidefile");
                IList<CompanyFile> filelist = new List<CompanyFile>();
                if (UploadFile.Length > 0)
                {
                    for (int i = 0; i < UploadFile.Length; i++)
                    {
                        if (UploadFile[i].Trim() != "")
                        {
                            EyouSoft.Model.CompanyStructure.CompanyFile fileModel = new CompanyFile();
                            fileModel.FilePath = UploadFile[i].ToString().Split('|')[1].ToString();
                            fileModel.FileId = "";
                            filelist.Add(fileModel);
                        }
                    }
                }
                infoModel = new EyouSoft.Model.CompanyStructure.CompanyInfo();
                infoModel.FilePath = new List<CompanyFile>();
                infoModel.FilePath = filelist;

                #endregion
                infoModel.CompanyAddress = Utils.InputText(txtAddress.Value);//地址
                infoModel.ContactName = Utils.InputText(txtAdmin.Value);//负责人
                account.CompanyId = CurrentUserCompanyID;//公司编号
                infoModel.CompanyZip = Utils.InputText(txtEmail.Value);//邮箱
                infoModel.CompanyEnglishName = Utils.InputText(txtEngName.Value);//公司英文名
                infoModel.ContactFax = Utils.InputText(txtFax.Value);//公司传真
                infoModel.License = Utils.InputText(txtLicence.Value);//公司许可证
                infoModel.ContactMobile = Utils.InputText(txtMoible.Value);//公司手机
                infoModel.CompanyName = Utils.InputText(txtCompanyName.Value);//公司名
                infoModel.ContactTel = Utils.InputText(txtTel.Value);//电话
                infoModel.CompanyType = Utils.InputText(txtType.Value);//旅行社类别
                infoModel.CompanySiteUrl = Utils.InputText(txtWeb.Value);//网站
                infoModel.SystemId = CurrentUserCompanyID;//系统号
                infoModel.Id = CurrentUserCompanyID;//公司号
                string deletefileId = Utils.GetFormValue(this.hfileId.UniqueID);

                bool result = false;
                result = companyBll.Update(infoModel);
                if (deletefileId.Length > 0 && deletefileId.Split(',').Length > 0)
                {
                    int fileidcount = deletefileId.Split(',').Length;
                    string[] filearr = new string[fileidcount - 1];
                    for (int i = 0; i < fileidcount; i++)
                    {
                        if (deletefileId.Split(',')[i].ToString().Trim() != "")
                            filearr[i] = deletefileId.Split(',')[i].ToString();
                    }
                    if (companyBll.DeleteCompanyFile(filearr) != 1)
                    {
                        MessageBox.Show(this, "未能删除附件！");
                    }
                }
                IList<string> stringlist = new List<string>();
                for (int i = 0; i < filelist.Count; i++)
                {
                    stringlist.Add(filelist[i].FilePath);
                }
                if (stringlist.Count > 0)
                {
                    if (companyBll.AddCompanyFile(this.SiteUserInfo.CompanyId, stringlist) != 1)
                    {
                        MessageBox.Show(this, "附件上传失败！");
                    }
                }
                MessageBox.ShowAndRedirect(this, result ? "保存成功！" : "保存失败！", "/systemset/CompanyInfo.aspx");
                #endregion
            }
            else
            {
                #region 初始化公司信息
                //初始化
                infoModel = companyBll.GetModel(CurrentUserCompanyID, CurrentUserCompanyID);
                if (infoModel != null)
                {
                    txtAddress.Value = infoModel.CompanyAddress;//地址
                    txtAdmin.Value = infoModel.ContactName;//负责人
                    txtEmail.Value = infoModel.CompanyZip;//邮箱
                    txtEngName.Value = infoModel.CompanyEnglishName;//公司英文名
                    txtFax.Value = infoModel.ContactFax;//公司传真
                    txtLicence.Value = infoModel.License;//公司许可证
                    txtMoible.Value = infoModel.ContactMobile;//公司手机
                    txtCompanyName.Value = infoModel.CompanyName;//公司名
                    txtTel.Value = infoModel.ContactTel;//电话
                    txtType.Value = infoModel.CompanyType;//旅行社类别
                    txtWeb.Value = infoModel.CompanySiteUrl;//网站
                    if (infoModel.FilePath != null && infoModel.FilePath.Count > 0)
                    {
                        this.rplfile.DataSource = infoModel.FilePath;
                        this.rplfile.DataBind();
                    }
                }
                #endregion
            }
        }

    }
}
