using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using EyouSoft.Common.Page;
using EyouSoft.Common;
using EyouSoft.Common.Function;
using System.Text;

namespace Web.SystemSet
{
    public partial class CityManage : BackPage
    {

        protected int pageIndex;
        protected int recordCount;
        protected int pageSize = 5;

        protected string proAndCityHtml;//城市省份列表html
        EyouSoft.BLL.CompanyStructure.City cityBll = new EyouSoft.BLL.CompanyStructure.City();//初始化citybll
        EyouSoft.BLL.CompanyStructure.Province proBll = new EyouSoft.BLL.CompanyStructure.Province();//初始化probll
        protected void Page_Load(object sender, EventArgs e)
        {
            pageIndex = Utils.GetInt(Utils.GetQueryStringValue("Page"), 1);
            string method = Utils.GetQueryStringValue("method");
            int id = Utils.GetInt(Utils.GetQueryStringValue("id"));//省份或城市Id
            //获取当前操作
            bool result = false;
            if (method != "")
            {
                if (this.CheckGrant(EyouSoft.Model.EnumType.PrivsStructure.Privs3.系统设置_基础设置_城市管理栏目))
                {
                    switch (method)
                    {
                        case "delCity"://删除城市
                            result = cityBll.Delete(id);
                            break;
                        case "delPro"://删除省份
                            result = proBll.Delete(id);
                            break;
                        case "True"://设置为常用城市
                        case "False":
                            result = cityBll.SetFav(id, method == "True");
                            break;
                    }
                }
                Utils.ResponseMeg(result, result ? "" : "操作失败！");
                return;
            }
            //绑定城市省份数据
            GetProAndCityHTML();

        }

        /// <summary>
        /// 绑定城市省份数据
        /// </summary>
        protected void GetProAndCityHTML()
        {
            int itemIndex2 = 1;
            IList<EyouSoft.Model.CompanyStructure.Province> proList = SelfExportPage.GetList(pageIndex, pageSize, out recordCount, proBll.GetProvinceInfo(CurrentUserCompanyID));
            if (proList != null && proList.Count > 0)
            {
                StringBuilder proBuilder = new StringBuilder();
                int itemIndex = 1;
                itemIndex2 = (pageIndex - 1) * pageSize;
                foreach (EyouSoft.Model.CompanyStructure.Province pro in proList)
                {
                    itemIndex2++;
                    proBuilder.AppendFormat("<tr class='{3}'>" +
                        "<td {0} align=\"center\" valign=\"top\">{1}</td>" +
                        "<td {0} align=\"center\" valign=\"top\"><strong><a href=\"javascript:;\" onclick=\"return CityManage.updatePro('{4}');\">{2}</a></strong> <a href=\"javascript:;\" onclick=\"return CityManage.delPro('{4}',this);\">[删除]</a></td>",
                        (pro.CityList != null && pro.CityList.Count > 0) ? "rowspan='" + pro.CityList.Count + "'" : "", itemIndex2, pro.ProvinceName, itemIndex2 % 2 == 0 ? "even" : "odd", pro.Id);
                    if (pro.CityList != null && pro.CityList.Count > 0)
                    {
                        foreach (EyouSoft.Model.CompanyStructure.City c in pro.CityList)
                        {
                            if (itemIndex != 1)
                                proBuilder.AppendFormat("<tr class='{0}'>", itemIndex2 % 2 == 0 ? "even" : "odd");
                            proBuilder.AppendFormat(
                                "<td align=\"center\" ><a href=\"javascript:;\" onclick=\"return CityManage.updateCity('{0}');\">{1}</a></td>"
                                +
                                //"<td align=\"center\" ><input type=\"checkbox\" isFav='{3}'  {2} onclick=\"return CityManage.setCity('{0}',this)\" /></td>" +
                                "<td align=\"center\" ><a href=\"javascript:;\" onclick=\"return CityManage.delCity('{0}',this);\">删除</a></td></tr>",
                                c.Id,
                                c.CityName,
                                c.IsFav ? "checked=\"checked\"" : "",
                                !c.IsFav);
                            itemIndex++;
                        }
                    }
                    else
                    {
                        proBuilder.Append("<td align=\"center\" >&nbsp;</td><td align=\"center\" bgcolor=\"#E3F1FC\">&nbsp;</td><td align=\"center\" bgcolor=\"#E3F1FC\">&nbsp;</td></tr>");
                    }
                    itemIndex = 1;
                }
                proAndCityHtml = proBuilder.ToString();
                BindExportPage();
            }
            else
            {
                proAndCityHtml = "<tr><td colspan='5' align='center'>对不起，暂无城市省份信息！</td></tr>";
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
    }
}
