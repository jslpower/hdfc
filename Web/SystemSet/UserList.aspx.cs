using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using EyouSoft.Common;
using EyouSoft.Common.Function;

namespace Web.SystemSet
{
    public partial class UserManage : EyouSoft.Common.Page.BackPage
    {
        protected int pageIndex;
        protected int recordCount;
        protected int pageSize = 20;
        protected void Page_Load(object sender, EventArgs e)
        {
            PowControl();
            pageIndex = Utils.GetInt(Utils.GetQueryStringValue("Page"), 1);
            string method = Utils.GetQueryStringValue("method");
            string ids = Utils.GetQueryStringValue("ids");//获取员工
            EyouSoft.BLL.CompanyStructure.CompanyUser userBll = new EyouSoft.BLL.CompanyStructure.CompanyUser();//初始化bll
            bool result = false;

            #region 当前操作
            if (method == "del")
            {
                int delcount = ids.Split(',').Length;
                int[] array = new int[delcount];
                for (int i = 0; i < delcount; i++)
                {
                    if (ids.Split(',')[i].ToString().Trim().Length > 0)
                        array[i] = Utils.GetInt(ids.Split(',')[i].ToString());
                }
                result = userBll.Remove(CurrentUserCompanyID, array);
                MessageBox.Show(this, result ? "删除成功！" : "删除失败！");
            }
            if (method == "setState")
            {
                result = userBll.SetEnable(Utils.GetInt(ids), Utils.GetQueryStringValue("hidMethod") == "start" ? EyouSoft.Model.EnumType.CompanyStructure.UserStatus.正常 : EyouSoft.Model.EnumType.CompanyStructure.UserStatus.已停用);
                Utils.ResponseMeg(result, result ? "设置完成！" : "设置失败！");
                return;
            }
            #endregion
            //绑定部门人员
            EyouSoft.Model.CompanyStructure.QueryCompanyUser searchmodel = new EyouSoft.Model.CompanyStructure.QueryCompanyUser();
            searchmodel.ContactName = Utils.GetQueryStringValue("txtContactName");
            searchmodel.UserName = Utils.GetQueryStringValue("txtUserName");
            searchmodel.UserType = EyouSoft.Model.EnumType.CompanyStructure.UserType.专线用户;
            IList<EyouSoft.Model.CompanyStructure.CompanyUser> list = userBll.GetList(CurrentUserCompanyID, pageSize, pageIndex, ref recordCount, searchmodel);
            UtilsCommons.Paging(pageSize, ref pageIndex, recordCount);
            if (list != null && list.Count > 0)
            {
                rptEmployee.DataSource = list;
                rptEmployee.DataBind();
                BindExportPage();
            }
            else
            {
                lbemptymsg.Text = "<tr><td colspan='10' align='center'>对不起，暂无部门员工信息！</td></tr>";
                ExporPageInfoSelect1.Visible = false;
            }

        }
        #region 绑定分页控件
        /// <summary>
        /// 绑定分页控件
        /// </summary>
        protected void BindExportPage()
        {
            this.ExporPageInfoSelect1.PageLinkURL = Request.ServerVariables["SCRIPT_NAME"].ToString() + "?";
            this.ExporPageInfoSelect1.intPageSize = pageSize;
            this.ExporPageInfoSelect1.CurrencyPage = pageIndex;
            this.ExporPageInfoSelect1.intRecordCount = recordCount;
        }
        #endregion

        private void PowControl()
        {

            if (!CheckGrant(global::EyouSoft.Model.EnumType.PrivsStructure.Privs3.系统设置_组织机构_栏目))
            {
                Utils.ResponseNoPermit(global::EyouSoft.Model.EnumType.PrivsStructure.Privs3.系统设置_组织机构_栏目, true);
                return;
            }

            if (CheckGrant(global::EyouSoft.Model.EnumType.PrivsStructure.Privs3.系统设置_组织机构_用户管理栏目))
            {
                if (!CheckGrant(global::EyouSoft.Model.EnumType.PrivsStructure.Privs3.系统设置_组织机构_部门管理栏目))
                {
                    PlaceHolder1.Visible = false;
                }
            }
            else
            {
                Utils.ResponseNoPermit(global::EyouSoft.Model.EnumType.PrivsStructure.Privs3.系统设置_组织机构_用户管理栏目, true);
                return;
            }
        }
    }
}
