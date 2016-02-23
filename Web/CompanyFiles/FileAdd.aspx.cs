using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using EyouSoft.Common;

namespace Web.CompanyFiles
{
    public partial class FileAdd : EyouSoft.Common.Page.BackPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            //Ajax
            string type = Utils.GetQueryStringValue("Type");
            if (!string.IsNullOrEmpty(type))
            {
                if (type.Equals("Save"))
                {
                    Response.Clear();
                    Response.Write(Save());
                    Response.End();
                }
                if (type.Equals("Update"))
                {
                    Response.Clear();
                    Response.Write(Update());
                    Response.End();
                }
            }

            if (!IsPostBack)
            {
                this.UploadControl1.IsUploadSelf = true;
                this.UploadControl1.CompanyID = SiteUserInfo.CompanyId;

                string _Type = Utils.GetQueryStringValue("do");
                if (!string.IsNullOrEmpty(_Type))
                {
                    if (_Type.Equals("add"))//新增
                    {
                        if (!CheckGrant(EyouSoft.Model.EnumType.PrivsStructure.Privs3.公司文件_文档管理_新增))
                        {
                            this.RCWE(
                                UtilsCommons.AjaxReturnJson(
                                    "0",
                                    string.Format(
                                        "您没有{0}的权限，请线路管理员！", EyouSoft.Model.EnumType.PrivsStructure.Privs3.公司文件_文档管理_新增)));
                            return;
                        }
                        else
                        {
                            this.btnSave.Visible = true;
                            this.CreateTime.Value = DateTime.Now.ToString("yyyy-MM-dd");
                            this.OperatorName.Value = SiteUserInfo.Name;
                        }
                    }
                    else if (_Type.Equals("update"))//修改
                    {
                        if (!CheckGrant(EyouSoft.Model.EnumType.PrivsStructure.Privs3.公司文件_文档管理_修改))
                        {
                            this.RCWE(
                                UtilsCommons.AjaxReturnJson(
                                    "0",
                                    string.Format(
                                        "您没有{0}的权限，请线路管理员！", EyouSoft.Model.EnumType.PrivsStructure.Privs3.公司文件_文档管理_修改)));
                            return;
                        }
                        else
                        {
                            this.btnUpdate.Visible = true;
                            int documentId = Utils.GetInt(Utils.GetQueryStringValue("DocumentId"), 0);
                            if (documentId != 0)
                            {
                                InitData(documentId);
                            }
                        }
                    }
                    else//查看
                    {
                        int documentId = Utils.GetInt(Utils.GetQueryStringValue("DocumentId"), 0);
                        if (documentId != 0)
                        {
                            InitData(documentId);
                        }
                    }
                }
                //if修改
            }
        }

        /// <summary>
        /// 初始化界面信息
        /// </summary>
        /// <param name="id"></param>
        private void InitData(int id)
        {
            var bll = new EyouSoft.BLL.CompanyStructure.PersonDocument();
            EyouSoft.Model.CompanyStructure.PersonDocument model = bll.GetModel(id);
            if (model != null)
            {
                this.fileName.Value = model.DocumentName;
                this.OperatorName.Value = model.OperatorName;
                this.CreateTime.Value = model.CreateTime.ToString("yyyy-MM-dd");
                this.lblFilePath.Text = string.Format("<span class='upload_filename'><a target='_blank' href='{0}'>文档附件</a><a href=\"javascript:void(0)\" onclick=\"File.DelFile(this)\" title='删除附件'><img style='vertical-align:middle' src='/images/cha.gif'></a><input type=\"hidden\" id=\"hidFilePath\" name=\"hidFilePath\" value='{0}'/></span>", model.FilePath);
                this.hidOperatorId.Value = model.OperatorId.ToString();
                this.hidCreateTime.Value = model.CreateTime.ToString();
            }
        }

        /// <summary>
        /// 保存
        /// </summary>
        /// <returns></returns>
        private string Save()
        {
            string msg = string.Empty;

            string FileName = Utils.GetFormValue(fileName.UniqueID);
            if (string.IsNullOrEmpty(FileName))
            {
                msg += "文档名称不能为空！</br>";
            }
            string filePath = Utils.GetFormValue(this.UploadControl1.ClientHideID);
            if (!string.IsNullOrEmpty(filePath))
            {
                filePath = filePath.Split('|')[1];
            }
            if (string.IsNullOrEmpty(filePath))
            {
                msg += "附件不能为空！</br>";
            }

            var model = new EyouSoft.Model.CompanyStructure.PersonDocument();
            model.CompanyId = SiteUserInfo.CompanyId;
            model.CreateTime = DateTime.Now;
            model.OperatorId = SiteUserInfo.UserId;
            model.FilePath = filePath;
            model.DocumentName = FileName;


            if (msg.Length <= 0)
            {
                var bll = new EyouSoft.BLL.CompanyStructure.PersonDocument();
                if (bll.Add(model))
                {
                    msg = EyouSoft.Common.UtilsCommons.AjaxReturnJson("1", "添加成功！");
                }
                else
                {
                    msg = EyouSoft.Common.UtilsCommons.AjaxReturnJson("0", "添加失败！");
                }
            }
            else
            {
                msg = EyouSoft.Common.UtilsCommons.AjaxReturnJson("0", msg);
            }

            return msg;
        }

        /// <summary>
        /// 修改
        /// </summary>
        /// <returns></returns>
        public string Update()
        {
            string msg = string.Empty;

            string FileName = Utils.GetFormValue(fileName.UniqueID);
            if (string.IsNullOrEmpty(FileName))
            {
                msg += "文档名称不能为空！</br>";
            }
            string filePath = Utils.GetFormValue(this.UploadControl1.ClientHideID);
            if (!string.IsNullOrEmpty(filePath))
            {
                filePath = filePath.Split('|')[1];
            }
            else
            {
                filePath = Utils.GetFormValue("hidFilePath");
            }
            if (string.IsNullOrEmpty(filePath))
            {
                msg += "附件不能为空！</br>";
            }

            var model = new EyouSoft.Model.CompanyStructure.PersonDocument();
            model.DocumentId = Utils.GetInt(Utils.GetQueryStringValue("DocumentId"));
            model.CompanyId = SiteUserInfo.CompanyId;
            model.CreateTime = Utils.GetDateTime(Utils.GetFormValue(this.hidCreateTime.UniqueID));
            model.OperatorId = Utils.GetInt(Utils.GetFormValue(this.hidOperatorId.UniqueID));
            model.FilePath = filePath;
            model.DocumentName = FileName;

            var bll = new EyouSoft.BLL.CompanyStructure.PersonDocument();
            if (msg.Length <= 0)
            {
                if (bll.Update(model))
                {
                    msg = EyouSoft.Common.UtilsCommons.AjaxReturnJson("1", "修改成功！");
                }
                else
                {
                    msg = EyouSoft.Common.UtilsCommons.AjaxReturnJson("0", "修改失败！");
                }
            }
            else
            {
                msg = EyouSoft.Common.UtilsCommons.AjaxReturnJson("0", msg);
            }

            return msg;
        }
    }
}
