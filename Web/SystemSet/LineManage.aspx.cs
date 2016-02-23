using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using EyouSoft.Common;

namespace Web.SystemSet
{
    public partial class LineManage : EyouSoft.Common.Page.BackPage
    {
        private int pagesize = 20;
        private int pagecount = 0;
        private int pageindex = 1;
        protected void Page_Load(object sender, EventArgs e)
        {
            string dotype = Utils.GetQueryStringValue("dotype");
            string areaid = Utils.GetQueryStringValue("areaid");
            if (dotype != null && dotype.Length > 0)
            {
                AJAX(dotype, areaid);
            }
            if (!IsPostBack)
            {
                PageInit();
            }
        }
        private void PageInit()
        {
            pageindex = Utils.GetInt(Utils.GetQueryStringValue("page")) == 0 ? 1 : Utils.GetInt(Utils.GetQueryStringValue("page"));
            EyouSoft.BLL.CompanyStructure.Area areabll = new EyouSoft.BLL.CompanyStructure.Area();
            IList<EyouSoft.Model.CompanyStructure.Area> List = areabll.GetList(pagesize, pageindex, ref pagecount, this.SiteUserInfo.CompanyId);
            if (List != null && List.Count > 0)
            {
                this.rptList.DataSource = List;
                this.rptList.DataBind();
                BindExportPage();
            }
            else
            {
                lbemptymsg.Text = "<tr><td colspan='3' align='center'>暂无线路区域!</td></tr>";
            }
        }

        /// <summary>
        /// ajax操作
        /// </summary>
        private void AJAX(string doType, string id)
        {
            string msg = string.Empty;
            if (doType == "delete")
            {
                if (this.CheckGrant(EyouSoft.Model.EnumType.PrivsStructure.Privs3.系统设置_基础设置_线路区域栏目))
                {
                    msg = this.DeleteData(id);
                }
                msg = this.DeleteData(id);
            }
            //返回ajax操作结果
            Response.Clear();
            Response.Write(msg);
            Response.End();
        }

        /// <summary>
        /// 删除操作
        /// </summary>
        /// <param name="id">删除ID</param>
        /// <returns></returns>
        private string DeleteData(string id)
        {
            if (string.IsNullOrEmpty(id)) return UtilsCommons.AjaxReturnJson("0", "线路区域错误!");

            EyouSoft.BLL.CompanyStructure.Area bllarea = new EyouSoft.BLL.CompanyStructure.Area();
            int bllRetCode = bllarea.Delete(Utils.GetInt(id));


            if (bllRetCode == 1) return UtilsCommons.AjaxReturnJson("1", "删除成功!");
            else if (bllRetCode == -1) return UtilsCommons.AjaxReturnJson("0", "删除失败：线路区域已使用，不能删除!");
            else return UtilsCommons.AjaxReturnJson("0", "删除失败!");
        }

        #region 绑定分页控件
        /// <summary>
        /// 绑定分页控件
        /// </summary>
        protected void BindExportPage()
        {
            this.ExporPageInfoSelect1.PageLinkURL = Request.ServerVariables["SCRIPT_NAME"].ToString() + "?";
            this.ExporPageInfoSelect1.intPageSize = pagesize;
            this.ExporPageInfoSelect1.CurrencyPage = pageindex;
            this.ExporPageInfoSelect1.intRecordCount = pagecount;
        }
        #endregion
    }
}
