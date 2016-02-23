using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.Xml.Linq;
using EyouSoft.Common;
using EyouSoft.Common.Page;
using System.Collections.Generic;

namespace Web.SystemSet
{
    public partial class SetPermit : BackPage
    {
        protected int roleId;
        protected int empId;
        EyouSoft.BLL.CompanyStructure.SysRoleManage roleBll;
        protected void Page_Load(object sender, EventArgs e)
        {
            //判断权限
            if (!CheckGrant(global::EyouSoft.Model.EnumType.PrivsStructure.Privs3.系统设置_组织机构_用户管理栏目))
            {
                Utils.ResponseNoPermit(global::EyouSoft.Model.EnumType.PrivsStructure.Privs3.系统设置_组织机构_用户管理栏目, false);
                return;
            }
            this.cu_perList.SysId = SiteUserInfo.SysId;
            string method = Utils.GetQueryStringValue("method");//获取当期操作
            empId = Utils.GetInt(Utils.GetQueryStringValue("empId"));//获取要设置的员工Id
            roleBll = new EyouSoft.BLL.CompanyStructure.SysRoleManage();
            EyouSoft.BLL.CompanyStructure.CompanyUser userBll = new EyouSoft.BLL.CompanyStructure.CompanyUser();//初始化bll
            if (method == "setPermit")
            {
                #region 设置权限
                //设置权限
                roleId = Utils.GetInt(Utils.GetFormValue("roleId"));
                string[] perIds = Utils.GetFormValue("perIds").Split(',');//获取选中的权限
                if (userBll.SetPermission(empId, roleId, perIds))
                {
                    Ajax(UtilsCommons.AjaxReturnJson("1", "授权成功!"));
                }
                else
                {
                   Ajax(UtilsCommons.AjaxReturnJson("0", "授权失败,请重试!"));
                }
                return;
                #endregion
            }
            else
            {
                #region 初始化数据或切换角色
                int recordCount = 0;
                //绑定角色下拉框
                IList<EyouSoft.Model.CompanyStructure.SysRoleManage> roleList = roleBll.GetList(100000, 1, ref recordCount, CurrentUserCompanyID);
                if (roleList != null)
                {
                    selRole.DataTextField = "RoleName";
                    selRole.DataValueField = "Id";
                    selRole.DataSource = roleList;
                    selRole.DataBind();
                    selRole.Attributes.Add("onchange", "SepPermit.changeRole(this);");
                }
                if (method == "getPermit")//切换角色
                {
                    roleId = Utils.GetInt(Utils.GetQueryStringValue("roleId"));//获取角色
                    if (roleId != 0)
                    {
                        //获取角色拥有的权限
                        BindPermit(roleId);
                    }
                }
                else if (method == "")//初始化数据
                {
                    EyouSoft.Model.CompanyStructure.CompanyUser userModel = userBll.GetUserInfo(empId);
                    selRole.Value = userModel.RoleID.ToString();
                    if (userModel != null)
                    {
                        if (!string.IsNullOrEmpty(userModel.PermissionList))
                        {
                            string[] permits = userModel.PermissionList.Split(',');
                            cu_perList.SetPermitList = permits;
                        }
                        else
                        {
                            if (userModel.RoleID == 0)
                            {
                                BindPermit(roleList != null && roleList.Count > 0 ? roleList[0].Id : 0);
                            }
                        }
                    }
                    else
                    {
                        BindPermit(roleList != null && roleList.Count > 0 ? roleList[0].Id : 0);
                    }
                }
                #endregion
            }
        }
        /// <summary>
        /// 绑定角色的权限
        /// </summary>
        /// <param name="roleIdp"></param>
        protected void BindPermit(int roleIdp)
        {
            EyouSoft.Model.CompanyStructure.SysRoleManage roleModel = roleBll.GetModel(CurrentUserCompanyID, roleIdp);
            string[] permits = roleModel.RoleChilds.Split(',');
            cu_perList.SetPermitList = permits;
            selRole.Value = roleId.ToString();
        }
        private void Ajax(string msg)
        {
            HttpContext.Current.Response.Clear();
            HttpContext.Current.Response.Write(msg);
            HttpContext.Current.Response.End();
        }
    }
}
