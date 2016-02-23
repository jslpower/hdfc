using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using EyouSoft.Common;
using EyouSoft.Common.Page;
using System.Text;
using EyouSoft.Common.Function;

namespace Web.SystemSet
{
    /// <summary>
    /// 部门编辑
    /// </summary>
    public partial class DepartAdd : BackPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            string save = Utils.GetQueryStringValue("save");
            if (!string.IsNullOrEmpty(save))
            {
                this.SaveData();
                return;
            }
            string action = Utils.GetQueryStringValue("action").ToLower();
            int parentId = Utils.GetInt(Utils.GetQueryStringValue("parentId"));
            int departId = Utils.GetInt(Utils.GetQueryStringValue("departId"));

            if (!IsPostBack)
            {
                //判断权限
                if (!CheckGrant(EyouSoft.Model.EnumType.PrivsStructure.Privs3.系统设置_组织机构_部门管理栏目))
                {
                    Utils.ResponseNoPermit(EyouSoft.Model.EnumType.PrivsStructure.Privs3.系统设置_组织机构_部门管理栏目, false);
                    return;
                }

                InitDropDownList();
                if (action == "add")
                {
                    if (parentId <= 0)
                    {
                        Utils.ShowMsgAndCloseBoxy("url错误，请重新操作！", Utils.GetQueryStringValue("iframeId"), false);
                        return;
                    }
                    if (ddlDepartParent.Items.FindByValue(parentId.ToString()) != null)
                        ddlDepartParent.Items.FindByValue(parentId.ToString()).Selected = true;
                }
                else if (action == "edit")
                {
                    if (departId <= 0)
                    {
                        Utils.ShowMsgAndCloseBoxy("url错误，请重新操作！", Utils.GetQueryStringValue("iframeId"), false);
                        return;
                    }

                    InitDepartInfo(departId);
                }
                else
                {
                    Utils.ShowMsgAndCloseBoxy("url错误，请重新操作！", Utils.GetQueryStringValue("iframeId"), false);
                    return;
                }
            }
        }

        /// <summary>
        /// 初始化下拉框
        /// </summary>
        private void InitDropDownList()
        {
            ddlDepartParent.DataSource = new EyouSoft.BLL.CompanyStructure.Department().GetAllDept(CurrentUserCompanyID);
            ddlDepartParent.DataTextField = "DepartName";
            ddlDepartParent.DataValueField = "Id";
            ddlDepartParent.DataBind();
            ddlDepartParent.Items.Insert(0, new ListItem("请选择", "0"));

            var userlist = new EyouSoft.BLL.CompanyStructure.CompanyUser().GetList(
                CurrentUserCompanyID,
                new EyouSoft.Model.CompanyStructure.QueryCompanyUser
                    { UserType = EyouSoft.Model.EnumType.CompanyStructure.UserType.专线用户 });
            if (userlist != null && userlist.Any())
            {
                foreach (var t in userlist)
                {
                    if (t == null || t.PersonInfo == null) continue;

                    ddlDepartManage.Items.Add(new ListItem(t.PersonInfo.ContactName, t.ID.ToString()));
                }
            }
            ddlDepartManage.Items.Insert(0, new ListItem("请选择", "0"));
        }

        /// <summary>
        /// 初始化部门信息
        /// </summary>
        /// <param name="departId">部门编号</param>
        private void InitDepartInfo(int departId)
        {
            if (departId <= 0) return;

            var model = new EyouSoft.BLL.CompanyStructure.Department().GetModel(departId);
            if (model == null) return;

            txtDepName.Value = model.DepartName;//部门名称
            txtTel.Value = model.ContactTel;//联系电话
            txtRemark.Value = model.Remark;//备注
            txtFax.Value = model.ContactFax;//传真

            if (ddlDepartManage.Items.FindByValue(model.DepartManger.ToString()) != null)
                ddlDepartManage.Items.FindByValue(model.DepartManger.ToString()).Selected = true;
            if (ddlDepartParent.Items.FindByValue(model.PrevDepartId.ToString()) != null)
                ddlDepartParent.Items.FindByValue(model.PrevDepartId.ToString()).Selected = true;
        }

        /// <summary>
        /// 保存数据
        /// </summary>
        private void SaveData()
        {
            //判断权限
            if (!CheckGrant(EyouSoft.Model.EnumType.PrivsStructure.Privs3.系统设置_组织机构_部门管理栏目))
            {
                this.RCWE(
                    UtilsCommons.AjaxReturnJson(
                        "0",
                        string.Format(
                            "您没有{0}的权限，请联系管理员！", EyouSoft.Model.EnumType.PrivsStructure.Privs3.系统设置_组织机构_部门管理栏目)));
                return;
            }

            string action = Utils.GetQueryStringValue("action").ToLower();
            if (string.IsNullOrEmpty(action))
            {
                this.RCWE(UtilsCommons.AjaxReturnJson("0", "url错误，请重新打开此窗口！"));
                return;
            }
            int departId = Utils.GetInt(Utils.GetQueryStringValue("departId"));
            if (action == "edit" && departId <= 0)
            {
                this.RCWE(UtilsCommons.AjaxReturnJson("0", "url错误，请重新打开此窗口！"));
                return;
            }

            int r = 0;
            var bll = new EyouSoft.BLL.CompanyStructure.Department();
            var model = this.GetFormValus();
            if (action == "add")
            {
                r = bll.Add(model) ? 1 : 0;
            }
            else if (action == "edit")
            {
                model.Id = departId;
                r = bll.Update(model) ? 1 : 0;
            }

            switch (r)
            {
                case 0:
                    this.RCWE(UtilsCommons.AjaxReturnJson("0", "操作失败！"));
                    break;
                case 1:
                    this.RCWE(UtilsCommons.AjaxReturnJson("1", "操作成功！"));
                    break;
                default:
                    this.RCWE(UtilsCommons.AjaxReturnJson("0", "操作失败！"));
                    break;
            }
        }

        /// <summary>
        /// 获取表单值返回实体
        /// </summary>
        /// <returns></returns>
        private EyouSoft.Model.CompanyStructure.Department GetFormValus()
        {
            return new EyouSoft.Model.CompanyStructure.Department
                {
                    CompanyId = CurrentUserCompanyID,
                    ContactFax = Utils.GetFormValue(txtFax.UniqueID),
                    ContactTel = Utils.GetFormValue(txtTel.UniqueID),
                    DepartManger = Utils.GetInt(Utils.GetFormValue(ddlDepartManage.UniqueID)),
                    DepartName = Utils.GetFormValue(txtDepName.UniqueID),
                    OperatorId = this.SiteUserInfo.UserId,
                    PrevDepartId = Utils.GetInt(Utils.GetFormValue(ddlDepartParent.UniqueID))
                };
        }
    }
}
