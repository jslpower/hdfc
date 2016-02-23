using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using EyouSoft.Common;
using EyouSoft.Common.Function;
using EyouSoft.Common.Page;

namespace Web.SystemSet
{
    public partial class UserAdd : BackPage
    {
        protected int empId;
        protected void Page_Load(object sender, EventArgs e)
        {
            //判断权限
            if (!CheckGrant(global::EyouSoft.Model.EnumType.PrivsStructure.Privs3.系统设置_组织机构_用户管理栏目))
            {
                Utils.ResponseNoPermit(global::EyouSoft.Model.EnumType.PrivsStructure.Privs3.系统设置_组织机构_用户管理栏目, false);
                return;
            }
            empId = Utils.GetInt(Utils.GetQueryStringValue("empId"));//获取员工Id
            string method = Utils.GetFormValue("hidMethod");//获取当前操作(保存/继续)
            string showMess = "数据保存成功！";//提示消息
            //如果当前操作无则初始加载(否则保存操作)
            EyouSoft.BLL.CompanyStructure.CompanyUser userBll = new EyouSoft.BLL.CompanyStructure.CompanyUser();//初始化bll
            EyouSoft.BLL.CompanyStructure.Department departBll = new EyouSoft.BLL.CompanyStructure.Department();//初始化bll
            if (method == "")
            {
                #region 初始化员工信息
                //所属部门
                IList<EyouSoft.Model.CompanyStructure.Department> departList = departBll.GetAllDept(CurrentUserCompanyID);
                selBdepart.DataTextField = "DepartName";
                selBdepart.DataValueField = "Id";
                selBdepart.DataSource = departList;
                selBdepart.DataBind();
                selBdepart.Items.Insert(0, new ListItem("选择部门", ""));
                //监管部门
                selMdepart.DataTextField = "DepartName";
                selMdepart.DataValueField = "Id";
                selMdepart.DataSource = departList;
                selMdepart.DataBind();
                selMdepart.Items.Insert(0, new ListItem("选择部门", ""));
                if (empId != 0) //如果员工Id不为空则加载数据
                {
                    EyouSoft.Model.CompanyStructure.CompanyUser userModel = userBll.GetUserInfo(empId);
                    if (userModel != null)
                    {
                        txtEmail.Value = userModel.PersonInfo.ContactEmail;
                        txtFax.Value = userModel.PersonInfo.ContactFax;
                        txtIntroduce.Value = userModel.PersonInfo.PeopProfile;
                        txtMoible.Value = userModel.PersonInfo.ContactMobile;
                        txtMSN.Value = userModel.PersonInfo.MSN;
                        txtQQ.Value = userModel.PersonInfo.QQ;
                        txtRemark.Value = userModel.PersonInfo.Remark;
                        txtTel.Value = userModel.PersonInfo.ContactTel;
                        rdiSex.SelectedValue = ((int)userModel.PersonInfo.ContactSex).ToString();
                        selMdepart.Value = userModel.SuperviseDepartId.ToString();
                        selBdepart.Value = userModel.DepartId.ToString();
                        txtUserName.Value = userModel.UserName;
                        txtUserName.Attributes.Add("readonly", "readonly");
                        txtUserName.Attributes.Add("style","background-color:#dadada");
                        txtPassword.Attributes.Add("value", userModel.PassWordInfo.NoEncryptPassword);
                        txtName.Value = userModel.PersonInfo.ContactName;
                        if (userModel.PersonInfo.Birthday.HasValue) 
                            txtBirthday.Value = userModel.PersonInfo.Birthday.Value.ToShortDateString();
                        txtAddress.Value = userModel.PersonInfo.Address;
                    }
                }
                #endregion
            }
            else
            {
                #region 保存员工信息
                bool result = false;
                //判断用户名是否已经存在

                string uName = Utils.GetFormValue(txtUserName.UniqueID);
                result = userBll.IsExists(empId, uName, CurrentUserCompanyID);
                if (result)
                {
                    MessageBox.Show(this, "用户名已存在");
                    return;
                }



                //验证数据完整性
                if (Utils.InputText(txtUserName.Value) == "" || Utils.InputText(txtPassword.Text) == "" || Utils.InputText(txtName.Value) == "")
                {
                    MessageBox.Show(this, "数据请填写完整！");
                    return;
                }
                EyouSoft.Model.CompanyStructure.CompanyUser userModel = new EyouSoft.Model.CompanyStructure.CompanyUser();
                //如果员工编号不为空且不是复制操作则修改操作(否则为新增)
                EyouSoft.Model.CompanyStructure.ContactPersonInfo PersonInfo = new EyouSoft.Model.CompanyStructure.ContactPersonInfo();

                PersonInfo.ContactEmail = Utils.InputText(txtEmail.Value);
                PersonInfo.ContactFax = Utils.InputText(txtFax.Value);
                PersonInfo.PeopProfile = Utils.InputText(txtIntroduce.Value, 250);
                PersonInfo.ContactMobile = Utils.InputText(txtMoible.Value);
                PersonInfo.MSN = Utils.InputText(txtMSN.Value);
                PersonInfo.ContactName = Utils.InputText(txtName.Value);
                PersonInfo.QQ = Utils.InputText(txtQQ.Value);
                PersonInfo.Remark = Utils.InputText(txtRemark.Value, 250);
                PersonInfo.ContactTel = Utils.InputText(txtTel.Value);
                PersonInfo.ContactSex = (EyouSoft.Model.EnumType.CompanyStructure.Sex)Utils.GetInt(rdiSex.SelectedValue);
                PersonInfo.Birthday = Utils.GetDateTimeNullable(Utils.GetFormValue(txtBirthday.UniqueID));
                PersonInfo.Address = Utils.GetFormValue(txtAddress.UniqueID);
                userModel.PersonInfo = PersonInfo;
                userModel.UserStatus = EyouSoft.Model.EnumType.CompanyStructure.UserStatus.正常;
                userModel.LastLoginTime = DateTime.Now;
                userModel.CompanyId = CurrentUserCompanyID;
                userModel.DepartId = Utils.GetInt(Utils.GetFormValue(selBdepart.UniqueID));
                userModel.SuperviseDepartName = Utils.GetFormValue("selMName");
                if (userModel.SuperviseDepartName == "选择部门")
                {
                    userModel.SuperviseDepartName = "";
                }
                
                userModel.DepartName = Utils.GetFormValue("selBName");
                userModel.IssueTime = DateTime.Now;
                userModel.PassWordInfo = new EyouSoft.Model.CompanyStructure.PassWord { NoEncryptPassword = Utils.InputText(txtPassword.Text) };
                userModel.SuperviseDepartId = Utils.GetInt(Utils.GetFormValue(selMdepart.UniqueID));
                userModel.UserName = Utils.InputText(txtUserName.Value);
                userModel.UserType = EyouSoft.Model.EnumType.CompanyStructure.UserType.专线用户;

                if (empId != 0)//修改
                {
                    userModel.ID = empId;
                    result = userBll.Update(userModel);
                }
                else
                {
                    int r = userBll.Add(userModel);//添加
                    switch (r)
                    {
                        case 0:
                            showMess = "数据保存失败！";
                            break;
                        case -1:
                            showMess = "子账号数量已满，请联系易诺客服！";
                            break;
                        case -2:
                            showMess = "数据保存失败！";
                            break;
                    }
                    result = true;
                }
                if (!result)
                {
                    showMess = "数据保存失败！";
                }
                //继续添加则刷新页面,否则关闭当前窗口
                if (method == "continue")
                {
                    MessageBox.ShowAndRedirect(this, showMess, "UserAdd.aspx");
                }
                else
                {
                    MessageBox.ResponseScript(this, string.Format(";alert('{0}');window.parent.location='/SystemSet/UserList.aspx';window.parent.Boxy.getIframeDialog('{1}').hide()", showMess, Utils.GetQueryStringValue("iframeId")));
                }
                #endregion
            }
        }
    }
}
