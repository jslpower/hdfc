using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using EyouSoft.Common;
using EyouSoft.Common.Page;
using EyouSoft.BLL.CompanyStructure;
using EyouSoft.Common.Function;

namespace Web.SystemSet
{
    public partial class RoleList : BackPage
    {

        #region  分页参数
        protected int itemIndex = 1;
        protected int pageIndex = 1;
        protected int recordCount;
        protected int pageSize = 20;
        protected int itemIndex2 = 1;
        #endregion
        protected void Page_Load(object sender, EventArgs e)
        {
            #region 权限判断
            if (!CheckGrant(global::EyouSoft.Model.EnumType.PrivsStructure.Privs3.系统设置_角色管理_栏目))
            {
                Utils.ResponseNoPermit(global::EyouSoft.Model.EnumType.PrivsStructure.Privs3.系统设置_角色管理_栏目, true);
                return;
            }
            #endregion

            string roleIds = Utils.GetQueryStringValue("roleIds");
            pageIndex = Utils.GetInt(Utils.GetQueryStringValue("Page"), 1);            
            SysRoleManage roleBll = new SysRoleManage();
            if (roleIds != "")
            {

                int[] roleIdArr = roleIds.TrimEnd(',').Split(',').Select(i => Utils.GetInt(i)).ToArray();
                bool result = roleBll.Delete(CurrentUserCompanyID, roleIdArr);
                MessageBox.ShowAndRedirect(this, result ? "删除成功！" : "删除失败！", "/systemset/RoleList.aspx");
            }
            IList<EyouSoft.Model.CompanyStructure.SysRoleManage> list = roleBll.GetList(pageSize, pageIndex, ref recordCount, CurrentUserCompanyID);
            if (list != null && list.Count > 0)
            {
                UtilsCommons.Paging(pageSize, ref pageIndex, recordCount);
                itemIndex2 = (pageIndex - 1) * pageSize + 1;
                rptRoles.DataSource = list;
                rptRoles.DataBind();
                pageBind();
            }

        }

        #region 获取角色列表
        /// <summary>
        /// 绑定列表时获取换行操作
        /// </summary>
        /// <returns></returns>
        protected string GetListTr()
        {
            string strHtml = "";
            if (itemIndex == 1)
                strHtml = "<tr class='oddTr' bgcolor='#e3f1fc'>";
            else if (itemIndex % 4 == 1)
                strHtml = "</tr><tr class='oddTr'  bgcolor='#e3f1fc'> ";
            else if (itemIndex % 2 == 1)
                strHtml = "</tr><tr class='evenTr' bgcolor='#BDDCF4'>";
            itemIndex++;
            return strHtml;
        }
        /// <summary>
        /// 获取绑定列表的最后一项
        /// </summary>
        /// <returns></returns>
        protected string GetLastTr()
        {
            if (recordCount == 0)
                return "";
            if (itemIndex % 1 == 0)
                return "<td colspan='4'>&nbsp;</td></tr>";
            return "</tr>";
        }
        #endregion

        #region 绑定分页
        /// <summary>
        /// 绑定分页
        /// </summary>
        protected void pageBind()
        {
            this.ExporPageInfoSelect1.intPageSize = pageSize;
            this.ExporPageInfoSelect1.CurrencyPage = pageIndex;
            this.ExporPageInfoSelect1.intRecordCount = recordCount;
        }
        #endregion
    }
}
