using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using EyouSoft.Common.Page;
using EyouSoft.Common;

namespace Web.CommonPage
{
    public partial class SelectDepart : BackPage
    {
        protected int itemIndex = 1;
        protected int recordCount;
        List<string> dpList = new List<string>();//选择过的部门列表
        protected void Page_Load(object sender, EventArgs e)
        {
            EyouSoft.BLL.CompanyStructure.Department departBll = new EyouSoft.BLL.CompanyStructure.Department();
            IList<EyouSoft.Model.CompanyStructure.Department> departList = departBll.GetAllDept(CurrentUserCompanyID);
            string dpids = Utils.GetQueryStringValue("dpids");
            if (dpids != "")
            {
                dpList = dpids.Split(',').ToList();//产生选择过的部门
            }
            //绑定部门列表
            if (departList != null && departList.Count > 0)
            {
                rptDepart.DataSource = departList;
                rptDepart.DataBind();
            }
            else
            {
                rptDepart.EmptyText = "<tr><td align='center'>对不起，暂无部门信息</td></tr>";
            }
        }

        /// <summary>
        /// 绑定列表时获取换行操作
        /// </summary>
        /// <returns></returns>
        protected string GetListTr()
        {
            string strHtml = "";
            if (itemIndex == 1)
                strHtml = "<tr class='oddTr'>";
            else if (itemIndex % 10 == 1)
                strHtml = "</tr><tr class='oddTr'>";
            else if (itemIndex % 5 == 1)
                strHtml = "</tr><tr class='evenTr'>";
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
            if (recordCount % 5 != 0)
                return string.Format("<td colspan='{0}'></td></tr>", 5 - recordCount % 5);
            return "</tr>";
        }
        /// <summary>
        /// 获取当前信息选中过的那些部门
        /// </summary>
        /// <param name="departId"></param>
        /// <returns></returns>
        protected string GetDepartChecked(int departId)
        {
            if (dpList.Contains(departId.ToString()))
            {
                return "checked=\"checked\"";
            }
            return "";
        }
    }
}
