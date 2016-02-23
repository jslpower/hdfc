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
using System.Collections.Generic;
using EyouSoft.Model.CRM;
using System.Text;

namespace Web.CustomerManage
{
    /// <summary>
    /// 创建人:刘飞
    /// 时间：2013-1-4
    /// 描述：客户关怀列表信息
    /// </summary>
    public partial class CustomerGuanhuai : EyouSoft.Common.Page.BackPage
    {
        #region 分页参数
        protected int pageIndex;
        protected int recordCount;
        protected int pageSize = 20;
        #endregion
        protected void Page_Load(object sender, EventArgs e)
        {
            PowerControl();
            string id = Utils.GetQueryStringValue("id");
            string dotype = Utils.GetQueryStringValue("dotype");
            if (dotype != null && dotype.Length > 0)
            {
                AJAX(dotype, id);
            }
            if (!IsPostBack)
            {
                PageInit();
            }

        }

        private void PageInit()
        {
            EyouSoft.BLL.CRM.BCustomerCare bll = new EyouSoft.BLL.CRM.BCustomerCare();
            EyouSoft.Model.CRM.MSearchCustomerCare searchmodel = new EyouSoft.Model.CRM.MSearchCustomerCare();
            searchmodel.CustomerName = Utils.GetQueryStringValue("unitname");
            searchmodel.StartVisitTime = Utils.GetDateTimeNullable(Utils.GetQueryStringValue("starttime"));
            searchmodel.EndVisitTime = Utils.GetDateTimeNullable(Utils.GetQueryStringValue("endtime"));
            searchmodel.VisitName = Utils.GetQueryStringValue("vistor");
            pageIndex = Utils.GetInt(Utils.GetQueryStringValue("Page"), 1);
            UtilsCommons.Paging(pageSize, ref pageIndex, recordCount);
            IList<EyouSoft.Model.CRM.MCustomerCare> list = bll.GetCustomerCare(this.SiteUserInfo.CompanyId, pageSize, pageIndex, ref recordCount, searchmodel);
            if (list != null && list.Count > 0)
            {
                this.rptList.DataSource = list;
                this.rptList.DataBind();
            }
            else
            {
                lbemptymsg.Text = "<tr><td colspan='6' align='center' height='30px'>暂无数据!</td></tr>";
            }
        }
        /// <summary>
        /// ajax操作
        /// </summary>
        private void AJAX(string doType, string id)
        {
            string msg = string.Empty;
            //对应执行操作
            switch (doType.ToLower())
            {
                case "delete":
                    // 判断权限
                    if (this.CheckGrant(EyouSoft.Model.EnumType.PrivsStructure.Privs3.客户管理_客户关怀_删除))
                    {
                        msg = this.DeleteData(id);
                    }
                    break;
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
            string msg = string.Empty;
            if (!String.IsNullOrEmpty(id))
            {
                EyouSoft.BLL.CRM.BCustomerCare bll = new EyouSoft.BLL.CRM.BCustomerCare();
                int idlength = id.Split(',').Length;
                int[] idarry = new int[idlength];
                if (id.Length > 0 && idlength > 0)
                {
                    for (int i = 0; i < idlength; i++)
                    {
                        idarry[i] = Utils.GetInt(id.Split(',')[i].ToString());
                    }
                }

                int result = bll.DeleteCustomerCare(idarry);
                switch (result)
                {
                    case 0:
                        msg = UtilsCommons.AjaxReturnJson("0", "参数错误");
                        break;
                    case 1:
                        msg = UtilsCommons.AjaxReturnJson("1", "删除成功");
                        break;
                    case -1:
                        msg = UtilsCommons.AjaxReturnJson("0", "删除失败");
                        break;
                    default:
                        msg = UtilsCommons.AjaxReturnJson("0", "无效的客户信息!");
                        break;
                }
            }
            return msg;
        }

        /// <summary>
        /// 获取联系人信息
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        protected string GetContactInfo(object obj)
        {
            IList<MCustomerContact> list = (IList<MCustomerContact>)obj;
            StringBuilder str = new StringBuilder();

            str.Append("<table cellspacing='0' cellpadding='0' border='0' width='100%' class='pp-tableclass'><tr class='pp-table-title'><th>经理/计调姓名</th><th>工作电话</th><th >手机</th><th>生日</th></tr>");
            if (list != null && list.Count > 0)
            {
                for (int i = 0; i < list.Count; i++)
                {
                    str.AppendFormat("<tr><td>{0}</td><td>{1}</td><td>{2}</td><td>{3}</td></tr>",
                        list[i].Name,
                        list[i].Tel,
                        list[i].Mobile,
                        list[i].BirthDay.HasValue ? Utils.GetDateTime(list[i].BirthDay.Value.ToString()).ToString("yyyy-MM-dd") : "");
                }
            }
            str.Append("</table>");
            return str.ToString();
        }

        #region 绑定分页控件
        /// <summary>
        /// 绑定分页控件
        /// </summary>
        protected void BindExportPage()
        {
            this.ExporPageInfoSelect1.intPageSize = pageSize;
            this.ExporPageInfoSelect1.CurrencyPage = pageIndex;
            this.ExporPageInfoSelect1.intRecordCount = recordCount;
        }
        #endregion

        /// <summary>
        /// 权限判断
        /// </summary>
        private void PowerControl()
        {
            if (!this.CheckGrant(EyouSoft.Model.EnumType.PrivsStructure.Privs3.客户管理_客户关怀_栏目))
            {
                Utils.ResponseNoPermit(EyouSoft.Model.EnumType.PrivsStructure.Privs3.客户管理_客户关怀_栏目, false);
                return;
            }
        }
    }
}
