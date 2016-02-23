using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using EyouSoft.Common;
using EyouSoft.Common.Page;
using EyouSoft.BLL.CompanyStructure;
using System.Text;
using EyouSoft.Common.Function;

namespace Web.SystemSet
{
    public partial class RoleEdit : BackPage
    {

        protected string roleName;//角色名称
        protected int roleId;//
        protected string method1 = "";
        protected void Page_Load(object sender, EventArgs e)
        {
            #region 权限判断
            if (!CheckGrant(global::EyouSoft.Model.EnumType.PrivsStructure.Privs3.系统设置_角色管理_栏目))
            {
                Utils.ResponseNoPermit(global::EyouSoft.Model.EnumType.PrivsStructure.Privs3.系统设置_角色管理_栏目, true);
                return;
            }
            #endregion

            this.cu_perList.SysId = SiteUserInfo.SysId;
            method1 = Utils.GetQueryStringValue("method");//判断是否为复制
            string method2 = Utils.GetFormValue("hidMethod");//判断是否为保存
            string showMess = "数据保存成功！";
            roleId = Utils.GetInt(Utils.GetQueryStringValue("roleId"));
            EyouSoft.Model.CompanyStructure.SysRoleManage roleModel = null;
            EyouSoft.BLL.CompanyStructure.SysRoleManage roleBll = new EyouSoft.BLL.CompanyStructure.SysRoleManage();//初始化角色bll
            EyouSoft.BLL.CompanyStructure.CompanyUser user = new CompanyUser();
            
            //如果当前操作不为保存则为初次加载
            if (method2 != "save")
            {
                #region 初始化加载数据
                if (roleId != 0)
                {//如果角色编号不为0则加载角色信息
                    roleModel = roleBll.GetModel(CurrentUserCompanyID, roleId);
                    
                    if (roleModel != null)
                    {
                        roleName = roleModel.RoleName;
                        cu_perList.SetPermitList = !string.IsNullOrEmpty(roleModel.RoleChilds) ? roleModel.RoleChilds.Split(',').ToArray() : null;
                    }//获取设置角色拥有的权限
                }
                #endregion
            }
            else
            {
                #region 保存数据
                bool result = false;
                string perItem = Utils.GetFormValue("perItem");//获取权限信息
                roleName = Utils.GetFormValue("txtRoleName");//获取角色名
                if (roleName == "")
                {
                    MessageBox.Show(this, "角色名称不为空！");
                    return;
                }

                roleModel = new EyouSoft.Model.CompanyStructure.SysRoleManage();
                roleModel.RoleName = roleName;
                roleModel.RoleChilds = perItem;

                roleModel.CompanyId = CurrentUserCompanyID;
                if (roleId > 0)
                {
                    //修改角色
                    roleModel.Id = roleId;
                    result = roleBll.Update(roleModel);

                }
                else
                {
                    //添加新角色
                    result = roleBll.Add(roleModel);
                }
                if (!result)
                {
                    showMess = "数据保存失败！";
                }
                MessageBox.ResponseScript(this, string.Format(";alert('{0}'); window.parent.location='/SystemSet/RoleList.aspx';window.parent.Boxy.getIframeDialog('{1}').hide()", showMess, Request.QueryString["iframeId"]));
                return;
                #endregion
            }

        }

    }
}
