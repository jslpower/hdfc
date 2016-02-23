using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using EyouSoft.Common;
using EyouSoft.Common.Function;
using System.Text;

namespace Web.SystemSet
{
    public partial class DepartManage : EyouSoft.Common.Page.BackPage
    {
        protected string depStrHTML = string.Empty;//部门绑定数据
        protected string styleDiaplay = "none";//是否隐藏新增
        EyouSoft.BLL.CompanyStructure.Department departBll = new EyouSoft.BLL.CompanyStructure.Department();//初始化bll
        protected void Page_Load(object sender, EventArgs e)
        {
            PowControl();

            //如果部门Id不为空则获取子集部门
            string method = Utils.GetQueryStringValue("method");
            int departId = Utils.GetInt(Utils.GetQueryStringValue("departId"));

            //如果不是删除操作则获取部门
            if (method != "del")
            {
                if (method == "candel")//验证是否能删除
                {
                    if (departBll.HasChildDept(departId))
                    {
                        Utils.ResponseMeg(false, "该部门存在子部门不能被删除！");

                    }
                    else if (departBll.HasDeptUser(departId, CurrentUserCompanyID))
                    {
                        Utils.ResponseMeg(false, "该部门存在员工不能被删除！");
                    }
                    else
                    {
                        Utils.ResponseMeg(true, "");
                    }
                    return;
                }
                depStrHTML = GetSonDeparts(departId);
                if (depStrHTML == "")
                {
                    styleDiaplay = "";
                    depStrHTML = "<tr id='noDepart'><td>对不起，暂无部门信息！</td></tr>";
                }
                //如果部门id不是0则是ajax请求获取子部门操作
                if (departId != 0)
                {
                    Response.Clear();
                    Response.Write(depStrHTML);
                    Response.End();
                    return;
                }
            }
            else
            {   //删除部门
                bool result = departBll.Delete(CurrentUserCompanyID, departId);
                Utils.ResponseMeg(result, "");
            }
        }
        //获取下级部门
        protected string GetSonDeparts(int departId)
        {
            int step = Utils.GetInt(Utils.GetQueryStringValue("step"), 0);//获取当前级数
            step = step + 1;
            int padd = step * 20;//获取缩进
            IList<EyouSoft.Model.CompanyStructure.Department> list = departBll.GetList(CurrentUserCompanyID, departId);
            StringBuilder depBuilder = new StringBuilder();
            if (list != null && list.Count > 0)
            {
                bool isFirst = false;
                if (departId == 0)
                {
                    isFirst = true;
                }
                foreach (EyouSoft.Model.CompanyStructure.Department d in list)
                {
                    depBuilder.AppendFormat(
                           "<tr id=\"tr_{3}\" sid=\"{3}\" parentId=\"{6}\"><td height=\"28\" align=\"left\" valign=\"center\" {0} style=\"padding-left:{4}px;\">" +
                           "<img src=\"/images/organization_0{1}.gif\" alt=\"\" width=\"9\" height=\"9\" {8}/>" +
                           "<a href=\"javascript:;\" step='{5}' {7} id=\"son{3}\" ><strong id=\"strong{3}\">{2}</strong></a><a href=\"javascript:;\" onclick=\"return DM.addD('{3}',this);\">" +
                           "<font color=\"#0000FF\">[添加下级部门]</font></a><a href=\"#\" onclick=\"return DM.updateD('{3}',this);\">" +
                           "<font color=\"#0000FF\">[编辑]</font></a><a href=\"javascript:;\" onclick=\"return DM.delD('{3}',this,{6});\"><font color=\"#0000FF\" >{9}</font></a></td></tr>",
                           (step == 1 || step == 2 || step == 3 || step == 4) ? ("background=\"/images/organization_0" + step + ".gif\"") : "", isFirst ? 6 : d.HasNextLev ? 5 : 7, d.DepartName, d.Id, padd, step, d.PrevDepartId, d.HasNextLev ? "onclick=\"return DM.getSonD(this,'" + d.Id + "');\"" : "", d.HasNextLev ? "onclick=\"return DM.getSonD2(this);\"" : "", isFirst ? "" : "[删除]");
                    isFirst = false;
                    if (departId == 0)
                    {
                        if (step == 1)
                        {
                            step = 2;
                            padd = 40;
                        }
                    }
                }
            }
            return depBuilder.ToString();
        }


        private void PowControl()
        {
            if (!CheckGrant(global::EyouSoft.Model.EnumType.PrivsStructure.Privs3.系统设置_组织机构_栏目))
            {
                Utils.ResponseNoPermit(global::EyouSoft.Model.EnumType.PrivsStructure.Privs3.系统设置_组织机构_栏目, true);
                return;
            }

            if (!CheckGrant(global::EyouSoft.Model.EnumType.PrivsStructure.Privs3.系统设置_组织机构_部门管理栏目))
            {
                if (!CheckGrant(global::EyouSoft.Model.EnumType.PrivsStructure.Privs3.系统设置_组织机构_用户管理栏目))
                {
                    Utils.ResponseNoPermit(global::EyouSoft.Model.EnumType.PrivsStructure.Privs3.系统设置_组织机构_部门管理栏目, true);
                    return;
                }
                else
                {
                    Response.Redirect("UserList.aspx");
                }
            }

            if (!CheckGrant(global::EyouSoft.Model.EnumType.PrivsStructure.Privs3.系统设置_组织机构_用户管理栏目))
            {
                PlaceHolder1.Visible = false;
            }
        }
    }
}
