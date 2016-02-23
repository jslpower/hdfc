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
using System.Text;

namespace Web.CustomerManage
{
    /// <summary>
    /// 创建人：刘飞
    /// 时间：2012-12-31
    /// 描述：客户资料管理
    /// </summary>
    public partial class CustomerList : EyouSoft.Common.Page.BackPage
    {
        #region 分页参数
        protected int pageIndex;
        protected int recordCount;
        protected int pageSize = 20;
        #endregion
        protected void Page_Load(object sender, EventArgs e)
        {
            string id = Utils.GetQueryStringValue("id");
            string dotype = Utils.GetQueryStringValue("dotype");
            PowerControl();
            if (dotype != null && dotype.Length > 0)
            {
                AJAX(dotype, id);
            }
            //导出处理
            if (UtilsCommons.IsToXls()) ListToExcel(id, dotype);
            if (!IsPostBack)
            {
                PageInit(id, dotype);
            }
        }
        private void PageInit(string id, string dotype)
        {
            EyouSoft.BLL.CRM.BCustomer bll = new EyouSoft.BLL.CRM.BCustomer();
            EyouSoft.Model.CRM.MSearchCustomer searchmodel = new EyouSoft.Model.CRM.MSearchCustomer();
            searchmodel.Address = Utils.GetQueryStringValue("address").Trim();
            searchmodel.ContactName = Utils.GetQueryStringValue("contactname").Trim();
            searchmodel.CustomerName = Utils.GetQueryStringValue("unitname").Trim();
            searchmodel.Phone = Utils.GetQueryStringValue("contacttel").Trim();
            searchmodel.RatingId = Utils.GetInt(Utils.GetQueryStringValue("ddlrating"));
            string CustomerRating = Utils.GetQueryStringValue("ddlCustomerRating");

            searchmodel.CustomerRating = !string.IsNullOrEmpty(CustomerRating) ? (EyouSoft.Model.EnumType.CustomerStructure.CustomerRating?)Utils.GetInt(CustomerRating) : null;

            int pid = Utils.GetInt(Utils.GetQueryStringValue("ddlProvice"));
            int cid = Utils.GetInt(Utils.GetQueryStringValue("ddlCity"));
            if (pid > 0)
            {
                searchmodel.ProvinceId = new int[] { pid };
            }
            if (cid > 0)
            {
                searchmodel.CityId = new int[] { cid };
            }
            pageIndex = Utils.GetInt(Utils.GetQueryStringValue("Page"), 1);

            IList<EyouSoft.Model.CRM.MCustomer> list = bll.GetCustomer(this.SiteUserInfo.CompanyId, pageSize, pageIndex, ref recordCount, searchmodel);
            UtilsCommons.Paging(pageSize, ref pageIndex, recordCount);
            if (list != null && list.Count > 0)
            {
                this.rptlist.DataSource = list;
                this.rptlist.DataBind();
                BindExportPage();
            }
            else
            {
                lbemptymsg.Text = "<tr><td colspan='7' align='center' height='30px'>暂无数据!</td></tr>";
            }
        }

        #region 导出Excel
        /// <summary>
        /// 导出Excel
        /// </summary>
        private void ListToExcel(string id, string dotype)
        {
            int toXlsRecordCount = UtilsCommons.GetToXlsRecordCount();
            if (toXlsRecordCount < 1) ResponseToXls(string.Empty);
            var s = new StringBuilder();
            s.Append("序号\t所在地\t单位名称\t信用等级\t联系人\t电话\t手机\t地址\n");
            pageIndex = Utils.GetInt(Utils.GetQueryStringValue("page"));
            EyouSoft.BLL.CRM.BCustomer bll = new EyouSoft.BLL.CRM.BCustomer();
            EyouSoft.Model.CRM.MSearchCustomer searchmodel = new EyouSoft.Model.CRM.MSearchCustomer();

            pageIndex = Utils.GetInt(Utils.GetQueryStringValue("Page"), 1);
            UtilsCommons.Paging(pageSize, ref pageIndex, recordCount);
            IList<EyouSoft.Model.CRM.MCustomer> list = bll.GetCustomer(this.SiteUserInfo.CompanyId, toXlsRecordCount, pageIndex, ref recordCount, searchmodel);
            UtilsCommons.Paging(toXlsRecordCount, ref pageIndex, recordCount);
            if (list != null && list.Count > 0)
            {
                for (int i = 0; i < list.Count; i++)
                {
                    s.AppendFormat("{0}\t{1}\t{2}\t{3}\t{4}\t{5}\t{6}\t{7}\n",
                        (i + 1).ToString(),
                        list[i].ProvinceName + " " + list[i].CityName,
                        list[i].CustomerName+"("+list[i].CustomerRating+")",
                        list[i].RatingName,
                        list[i].ContactName,
                        list[i].Phone,
                        list[i].Mobile,
                        list[i].Address);
                }
            }

            ResponseToXls(s.ToString());
        }
        #endregion


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
                    if (this.CheckGrant(EyouSoft.Model.EnumType.PrivsStructure.Privs3.客户管理_客户资料_删除))
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
        /// 返回信用等级名称
        /// </summary>
        /// <param name="RatingId"></param>
        /// <returns></returns>
        protected string ReturnRatingName(object RatingId)
        {
            string RatingName = "设置等级";
            EyouSoft.BLL.CompanyStructure.Rating Ratingbll = new EyouSoft.BLL.CompanyStructure.Rating();
            EyouSoft.Model.CompanyStructure.Rating rModel = Ratingbll.GetModel(Convert.ToInt32(RatingId));
            if (rModel != null)
            {
                RatingName = rModel.RatingName;
            }
            return RatingName;
        }


        protected string BindRating(string selectItem)
        {
            EyouSoft.BLL.CompanyStructure.Rating bll = new EyouSoft.BLL.CompanyStructure.Rating();
            IList<EyouSoft.Model.CompanyStructure.Rating> List = bll.GetRatingByCompanyId(this.SiteUserInfo.CompanyId);
            System.Text.StringBuilder query = new System.Text.StringBuilder();
            query.Append("<option value=''>-请选择-</option>");
            if (List.Count > 0 && List != null)
            {
                for (int i = 0; i < List.Count; i++)
                {
                    if (List[i].Id.ToString() == selectItem)
                    {
                        query.AppendFormat("<option value='{0}' selected='selected'>{1}</option>", List[i].Id.ToString(), List[i].RatingName.ToString());
                    }
                    else
                    {
                        query.AppendFormat("<option value='{0}'>{1}</option>", List[i].Id.ToString(), List[i].RatingName.ToString());
                    }
                }
            }
            return query.ToString();
        }

        /// <summary>
        /// 绑定客户信用等级
        /// </summary>
        protected string BindCustomerRating(string selectItem)
        {
            System.Text.StringBuilder query = new System.Text.StringBuilder();
            query.Append("<option value='-1'>-请选择-</option>");
            //EyouSoft.Model.EnumType.PersonalCenterStructure.PlanCheckState
            Array values = Enum.GetValues(typeof(EyouSoft.Model.EnumType.CustomerStructure.CustomerRating));
            foreach (var item in values)
            {
                int value = (int)Enum.Parse(typeof(EyouSoft.Model.EnumType.CustomerStructure.CustomerRating), item.ToString());
                string text = Enum.GetName(typeof(EyouSoft.Model.EnumType.CustomerStructure.CustomerRating), item);
                if (value.ToString().Equals(selectItem))
                {
                    query.AppendFormat("<option value='{0}' selected='selected'>{1}</option>", value, text);
                }
                else
                {
                    query.AppendFormat("<option value='{0}' >{1}</option>", value, text);

                }
            }
            return query.ToString();
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
                EyouSoft.BLL.CRM.BCustomer bll = new EyouSoft.BLL.CRM.BCustomer();
                int result = bll.DeleteCustomer(id);
                switch (result)
                {
                    case 0:
                        msg = UtilsCommons.AjaxReturnJson("0", "参数错误");
                        break;
                    case 1:
                        msg = UtilsCommons.AjaxReturnJson("1", "删除成功");
                        break;
                    case -1:
                        msg = UtilsCommons.AjaxReturnJson("0", "客户已被使用，不能删除");
                        break;
                    case -2:
                        msg = UtilsCommons.AjaxReturnJson("0", "删除失败");
                        break;
                    default:
                        msg = UtilsCommons.AjaxReturnJson("0", "无效的客户信息!");
                        break;
                }
            }
            return msg;
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
            if (!this.CheckGrant(EyouSoft.Model.EnumType.PrivsStructure.Privs3.客户管理_客户资料_栏目))
            {
                Utils.ResponseNoPermit(EyouSoft.Model.EnumType.PrivsStructure.Privs3.客户管理_客户资料_栏目, false);
                return;
            }
            this.phadd.Visible = this.CheckGrant(EyouSoft.Model.EnumType.PrivsStructure.Privs3.客户管理_客户资料_新增);
            this.Phupdate.Visible = this.CheckGrant(EyouSoft.Model.EnumType.PrivsStructure.Privs3.客户管理_客户资料_修改);
            this.phdelete.Visible = this.CheckGrant(EyouSoft.Model.EnumType.PrivsStructure.Privs3.客户管理_客户资料_删除);
            this.phdaochu.Visible = this.CheckGrant(EyouSoft.Model.EnumType.PrivsStructure.Privs3.客户管理_客户资料_栏目);
        }
    }
}
