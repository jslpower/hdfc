using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using EyouSoft.Common;
using EyouSoft.Common.Function;

namespace Web.SMS
{   
    /// <summary>
    /// 编辑常用短语
    /// xuty 2011/1/21
    /// </summary>
    public partial class EditCommonSms : EyouSoft.Common.Page.BackPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            //判断权限
            if (!CheckGrant(EyouSoft.Model.EnumType.PrivsStructure.Privs3.短信中心_短信中心_栏目))
            {
                Utils.ResponseNoPermit(EyouSoft.Model.EnumType.PrivsStructure.Privs3.短信中心_短信中心_栏目, true);
                return;
            }
            string sId = Utils.GetQueryStringValue("sid");//短语Id
            string method = Utils.GetFormValue("hidMethod");//当前操作

            EyouSoft.BLL.SMSStructure.CommonWords commonBll = new EyouSoft.BLL.SMSStructure.CommonWords();
            if (method != "")
            {
                bool result = false;
                Utils.InputText(txtSmsContent.Value,200);//短信内容
                Utils.InputText(selClass.Value);
                string showMess = "数据保存成功！";
                
                if (method == "save"||method=="continue")//如果是保存数据
                {
                    #region 保存短语
                    int classId = Utils.GetInt(Utils.GetFormValue(selClass.UniqueID));
                    string commonWord = Utils.InputText(txtSmsContent.Value);
                    EyouSoft.Model.SMSStructure.CommonWords wordModel = new EyouSoft.Model.SMSStructure.CommonWords
                    {
                        ClassID = classId,
                        CompanyID = CurrentUserCompanyID,
                        IssueTime = DateTime.Now,
                        UserID = SiteUserInfo.UserId,
                        WordContent = commonWord
                    };
                    if (sId != "")//短语id不为空修改
                    {
                        wordModel.ID = sId;
                        result = commonBll.UpdateCommonWords(wordModel);
                    }
                    else//短语id为空添加
                    {
                        result = commonBll.AddCommonWords(wordModel);
                    }
                    if (!result) { showMess = "数据保存失败"; }
                    if (method == "save")//如果是保存则关闭弹窗否则刷新页面
                    {
                        MessageBox.ResponseScript(this, string.Format(";alert('{0}');window.parent.Boxy.getIframeDialog('{1}').hide();window.parent.location='/SMS/CommonSms.aspx';", showMess, Utils.GetQueryStringValue("iframeId")));
                    }
                    else
                    {    
                        MessageBox.ShowAndRedirect(this, showMess, "EditCommonSms.aspx");
                    }
                    return;
                    #endregion
                }
                if (method == "addClass")//添加类别
                {
                    #region 添加类别
                    string className=Utils.GetFormValue("className");
                    EyouSoft.Model.SMSStructure.CommonWordClass wordClass =
                        new EyouSoft.Model.SMSStructure.CommonWordClass
                       {
                           ClassName = className,
                           CompanyID = CurrentUserCompanyID,
                           IssueTime = DateTime.Now,
                           UserID = SiteUserInfo.UserId
                       };
                  
                    int classId=commonBll.AddCommonWordsClass(wordClass);
                    //添加类别
                    Utils.ResponseMeg(classId!=0, classId.ToString());
                    return;
                    #endregion
                }
                if (method == "delClass")//删除类别
                {
                    #region 删除类别
                    int classId=Utils.GetInt(Utils.GetFormValue("classId"));
                    //删除类别
                    result = commonBll.DeleteCommonWordsClass(classId);
                    Utils.ResponseMeg(result, string.Empty);
                    return;
                    #endregion
                }
                
            }
            else
            {
                #region 初始化数据
                //绑定短语类别
                IList<EyouSoft.Model.SMSStructure.CommonWordClass> classList= commonBll.GetCommonWordsClass(CurrentUserCompanyID);
                if (classList != null && classList.Count > 0)
                {
                    selClass.DataTextField = "ClassName";
                    selClass.DataValueField = "ID";
                    selClass.DataSource = classList;
                    selClass.DataBind();
                }
                selClass.Items.Insert(0, new ListItem("请选择", ""));
                //初始化短语
                if (sId != "")
                {  
                    EyouSoft.Model.SMSStructure.CommonWords wordModel = commonBll.GetCommonWords(sId);
                    if (wordModel != null)
                    {
                        txtSmsContent.Value = wordModel.WordContent;
                        selClass.Value = wordModel.ClassID.ToString();
                    }
                }
                #endregion
            }
        }
    }
}
