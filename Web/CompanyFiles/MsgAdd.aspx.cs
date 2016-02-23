using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using EyouSoft.Common;
using EyouSoft.Common.Function;
using EyouSoft.Common.Page;
using System.Text;

namespace Web.CompanyFiles
{
    /// <summary>
    ///  信息修改
    ///  刘树超 2012-11-20
    /// </summary>
    public partial class MsgAdd : BackPage
    {


        #region 变量
        protected string departIds;
        public string innerChecked = string.Empty;//发布对象公司内部
        public string departChecked = string.Empty;//发布对象选中部门
        public string tourChecked = string.Empty;//发布对象组团社
        protected int infoId;//信息ID
        EyouSoft.BLL.CompanyStructure.News newsBll = new EyouSoft.BLL.CompanyStructure.News();//初始化newsBll
        EyouSoft.Model.CompanyStructure.News newModel = new EyouSoft.Model.CompanyStructure.News();
        #endregion

        protected void Page_Load(object sender, EventArgs e)
        {
            UploadControl1.CompanyID = CurrentUserCompanyID;

            infoId = Utils.GetInt(Utils.GetQueryStringValue("infoId"));

            #region 权限判断

            if (infoId <= 0)
            {
                if (!CheckGrant(EyouSoft.Model.EnumType.PrivsStructure.Privs3.公司文件_公告通知_新增))
                {
                    Utils.ResponseNoPermit(EyouSoft.Model.EnumType.PrivsStructure.Privs3.公司文件_公告通知_新增, true);
                    return;
                }
            }
            else if (infoId > 0)
            {
                if (!CheckGrant(EyouSoft.Model.EnumType.PrivsStructure.Privs3.公司文件_公告通知_修改))
                {
                    Utils.ResponseNoPermit(EyouSoft.Model.EnumType.PrivsStructure.Privs3.公司文件_公告通知_修改, true);
                    return;
                }
            }

            #endregion

            string method = Utils.GetFormValue("hidMethod");
            //如果当前操作为保存或者保存继续添加
            if (method == "save" || method == "continue")
            {
                #region 保存信息
                string[] publichTo = Utils.GetFormValues("chkPublishTo");//发布对象
                if (publichTo != null && publichTo.Length > 0)
                {
                    IList<EyouSoft.Model.CompanyStructure.NewsAccept> acceptList = new List<EyouSoft.Model.CompanyStructure.NewsAccept>();
                    if (publichTo.Contains("cInner"))
                    {
                        EyouSoft.Model.CompanyStructure.NewsAccept acceptModel = new EyouSoft.Model.CompanyStructure.NewsAccept();
                        acceptModel.AcceptType = EyouSoft.Model.EnumType.CompanyStructure.AcceptType.所有;
                        acceptList.Add(acceptModel);
                    }
                    else if (publichTo.Contains("sDepart"))
                    {
                        if (!string.IsNullOrEmpty(txtDeparts.Value))
                        {
                            string[] departs = txtDeparts.Value.Split(',');
                            foreach (string dId in departs)
                            {
                                EyouSoft.Model.CompanyStructure.NewsAccept acceptModel = new EyouSoft.Model.CompanyStructure.NewsAccept { AcceptType = EyouSoft.Model.EnumType.CompanyStructure.AcceptType.指定部门, AcceptId = Utils.GetInt(dId) };
                                acceptList.Add(acceptModel);
                            }
                        }
                    }
                    if (publichTo.Contains("cTour"))
                    {
                        EyouSoft.Model.CompanyStructure.NewsAccept acceptModel = new EyouSoft.Model.CompanyStructure.NewsAccept { AcceptType = EyouSoft.Model.EnumType.CompanyStructure.AcceptType.指定组团 };
                        acceptList.Add(acceptModel);
                    }
                    newModel.AcceptList = acceptList;

                }
                else
                {
                    MessageBox.Show(this, "请填写完整数据！");
                    return;
                }
                newModel.CompanyId = CurrentUserCompanyID;

                newModel.Title = Utils.InputText(txtInfoTitle.Value);//发布标题
                newModel.Content = Utils.EditInputText(txtInfoContent.Value);//内容
                Utils.InputText(txtDeparts.Value);//发布的部门
                newModel.OperatorId = SiteUserInfo.UserId;//发布人
                newModel.OperatorName = SiteUserInfo.Username;
                newModel.IssueTime = Utils.GetDateTime(Utils.InputText(txtPublishDate.Value));

                #region 获取附件
                string filename = Utils.GetFormValue(UploadControl1.ClientHideID);
                string oldname = Utils.GetFormValue("hidefile");
                if (string.IsNullOrEmpty(filename))
                {
                    newModel.UploadFiles = oldname;
                }
                else
                {
                    newModel.UploadFiles = filename.Split('|')[1].ToString();
                }
                #endregion

                int execInt = 0;
                if (infoId != 0)
                {
                    //修改
                    newModel.ID = infoId;
                    execInt = newsBll.Update(newModel);
                }
                else
                {
                    //新增
                    execInt = newsBll.Add(newModel);
                }
                if (execInt == 1)
                {
                    //保存成功跳转列表或重新发布信息
                    MessageBox.ShowAndRedirect(this, "信息保存成功", method == "save" ? "/CompanyFiles/MsgManageList.aspx" : "/CompanyFiles/MsgAdd.aspx");
                }
                else
                {
                    MessageBox.Show(this, "信息保存失败！");
                }
                #endregion
            }
            else
            {

                if (infoId != 0)
                {
                    #region 初始化数据
                    newModel = newsBll.GetModel(infoId);
                    if (newModel != null)
                    {
                        //初始化数据
                        IList<EyouSoft.Model.CompanyStructure.NewsAccept> acceptList = newModel.AcceptList;
                        if (acceptList != null && acceptList.Count > 0)
                        {
                            if (acceptList.Where(i => i.AcceptType == EyouSoft.Model.EnumType.CompanyStructure.AcceptType.所有).Count() > 0)
                            {
                                innerChecked = "checked=\"checked\"";
                            }
                            IEnumerable<EyouSoft.Model.CompanyStructure.NewsAccept> acceptListDepart = acceptList.Where(i => i.AcceptType == EyouSoft.Model.EnumType.CompanyStructure.AcceptType.指定部门);
                            if (acceptListDepart.Count() > 0)
                            {
                                StringBuilder strBuilder = new StringBuilder();
                                foreach (var i in acceptListDepart)
                                {
                                    strBuilder.AppendFormat("{0},", i.AcceptId);
                                }
                                txtDeparts.Value = strBuilder.ToString().TrimEnd(',');
                                departChecked = "checked=\"checked\"";

                            }
                            if (acceptList.Where(i => i.AcceptType == EyouSoft.Model.EnumType.CompanyStructure.AcceptType.指定组团).Count() > 0)
                            {
                                tourChecked = "checked=\"checked\"";
                            }
                        }
                        txtAuthor.Value = newModel.OperatorName;
                        txtInfoTitle.Value = newModel.Title;
                        txtPublishDate.Value = newModel.IssueTime.ToString("yyyy-MM-dd HH:mm:ss");
                        txtInfoContent.Value = newModel.Content;
                        if (newModel != null && !string.IsNullOrEmpty(newModel.UploadFiles))
                        {
                            this.LabelFile.Text = string.Format("<span class='upload_filename'>&nbsp;<a href='{0}' target='_blank'>查看</a><a href='javascript:void(0);' onclick='RemoveFile(this);return false;'> <img style='vertical-align:middle' src='/images/cha.gif'></a><input type='hidden' name='hidefile' value='{0}'/></span>", newModel.UploadFiles);
                        }
                    }
                    #endregion
                }
                else
                {
                    txtAuthor.Value = SiteUserInfo.Username;
                    txtPublishDate.Value = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                }
            }
        }
    }
}
