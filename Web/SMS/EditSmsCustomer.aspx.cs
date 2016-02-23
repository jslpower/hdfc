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
    /// 短息客户编辑
    /// xuty 2011/1/20
    /// </summary>
    public partial class EditSmsCustomer : EyouSoft.Common.Page.BackPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            //判断权限
            if (!CheckGrant(EyouSoft.Model.EnumType.PrivsStructure.Privs3.短信中心_短信中心_栏目))
            {
                Utils.ResponseNoPermit(EyouSoft.Model.EnumType.PrivsStructure.Privs3.短信中心_短信中心_栏目, true);
                return;
            }
            string custId = Utils.GetQueryStringValue("custid");//客户Id
            string method = Utils.GetFormValue("hidMethod");//当前操作
            EyouSoft.BLL.SMSStructure.CustomerList custBll = new EyouSoft.BLL.SMSStructure.CustomerList();
            if (method != "")
            {
                #region 保存客户
                bool result = false;
                string showMess = "数据保存成功！";
                if (method == "save"||method=="continue")
                {
                    #region 保存客户
                    EyouSoft.Model.SMSStructure.CustomerList custModel = new EyouSoft.Model.SMSStructure.CustomerList();
                    custModel.CustomerCompanyName=Utils.InputText(txtCompanyName.Value);//单位名
                    custModel.MobileNumber=Utils.InputText(txtMobile.Value);//手机
                    custModel.ReMark=Utils.InputText(txtRemark.Value,200);//备注
                    custModel.CustomerContactName=Utils.InputText(txtUserName.Value);//姓名
                    custModel.ClassID =Utils.GetInt(Utils.GetFormValue(selClass.UniqueID));
                    custModel.CompanyID = CurrentUserCompanyID;
                    custModel.IssueTime = DateTime.Now;
                    custModel.UserID = SiteUserInfo.UserId;
                    custModel.ID = custId;
                   
                    if (custBll.IsExistCustomerMobile(CurrentUserCompanyID, custId, custModel.MobileNumber))
                    {
                        MessageBox.Show(this, "该号码已经存在！");
                        IList<EyouSoft.Model.SMSStructure.CustomerClass> classList = custBll.GetCustomerClass(CurrentUserCompanyID);
                        if (classList != null && classList.Count > 0)
                        {
                            selClass.DataTextField = "ClassName";
                            selClass.DataValueField = "ID";
                            selClass.DataSource = classList;
                            selClass.DataBind();
                        }
                        selClass.Items.Insert(0, new ListItem("请选择", ""));
                        return;
                    }
                    if (custId != "")
                    {
                        result = custBll.UpdateCustomerList(custModel);
                    }
                    else
                    {
                        result = custBll.AddCustomerList(custModel);
                    }
                    if (!result)
                    {
                        showMess = "数据保存失败！";
                    }
                    if (method == "save")
                    {
                        MessageBox.ResponseScript(this, string.Format(";alert('{0}');window.parent.Boxy.getIframeDialog('{1}').hide();window.parent.location='/SMS/SmsCustomerList.aspx';", showMess, Utils.GetQueryStringValue("iframeId")));
                    }
                    else
                    {
                        MessageBox.ShowAndRedirect(this, showMess, "EditSmsCustomer.aspx");
                    }
                    return;
                    #endregion
                }
                if (method == "addClass")
                {
                    #region 添加客户类别
                    string className=Utils.GetFormValue("className");
                    EyouSoft.Model.SMSStructure.CustomerClass custClass =
                        new EyouSoft.Model.SMSStructure.CustomerClass
                       {
                           ClassName = className,
                           CompanyID = CurrentUserCompanyID,
                           IssueTime = DateTime.Now,
                           UserID = SiteUserInfo.UserId
                       };

                    int classId = custBll.AddCustomClass(custClass);
                    //添加类别
                    Utils.ResponseMeg(classId != 0, classId.ToString());
                    return;
                    #endregion
                }
                if (method == "delClass")
                {
                    #region 删除客户类别
                    int classId =Utils.GetInt(Utils.GetFormValue("classId"));
                    //删除类别
                    result=custBll.DeleteCustomClass(classId);
                    Utils.ResponseMeg(result, string.Empty);
                    return;
                    #endregion
                }
                #endregion
            }
            else
            {
                #region 初始化客户
                //绑定客户类别
                IList<EyouSoft.Model.SMSStructure.CustomerClass> classList = custBll.GetCustomerClass(CurrentUserCompanyID);
                if (classList != null && classList.Count > 0)
                {
                    selClass.DataTextField = "ClassName";
                    selClass.DataValueField = "ID";
                    selClass.DataSource = classList;
                    selClass.DataBind();
                }
                selClass.Items.Insert(0, new ListItem("请选择", ""));
                //初始化客户
                if (custId != "")
                {
                    EyouSoft.Model.SMSStructure.CustomerList custModel = custBll.GetCustomer(custId);
                    if (custModel != null)
                    {
                        txtUserName.Value = custModel.CustomerContactName;
                        txtRemark.Value = custModel.ReMark;
                        txtMobile.Value = custModel.MobileNumber;
                        txtCompanyName.Value = custModel.CustomerCompanyName;
                        selClass.Value = custModel.ClassID.ToString();
                    }
                }
                #endregion
            }
        }
    }
}
