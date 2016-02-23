using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using EyouSoft.Common;

namespace Web.ResourceManage
{
    public partial class Dinner : EyouSoft.Common.Page.BackPage
    {
        #region 设置分页变量
        protected int pageIndex = 1;
        protected int recordCount;
        protected int pageSize = 10;
        #endregion

        #region 权限变量
        protected bool add = false;//新增
        protected bool update = false;//修改
        protected bool del = false;//删除
        #endregion


        protected void Page_Load(object sender, EventArgs e)
        {
            PowControl();

            //Ajax for delete
            string act = Utils.GetQueryStringValue("act");
            if (!string.IsNullOrEmpty(act))
            {
                if (act.Equals("areadel"))
                {
                    Response.Clear();
                    Response.Write(this.Delete());
                    Response.End();
                }
            }

            if (!IsPostBack)
            {
                Bind();
            }
        }


        private void Bind()
        {
            pageIndex = Utils.GetInt(Utils.GetQueryStringValue("page"), 1);

            EyouSoft.Model.SourceStructure.MSearchRestaurant search = new EyouSoft.Model.SourceStructure.MSearchRestaurant();
            search.ProvinceId = !string.IsNullOrEmpty(Utils.GetQueryStringValue("txtProvince")) ? (int?)Utils.GetInt(Utils.GetQueryStringValue("txtProvince")) : null;
            search.CityId = !string.IsNullOrEmpty(Utils.GetQueryStringValue("txtCity")) ? (int?)Utils.GetInt(Utils.GetQueryStringValue("txtCity")) : null;
            search.UnitName = Utils.GetQueryStringValue("txtUnionName");
            search.Cuisine = !string.IsNullOrEmpty(Utils.GetQueryStringValue("ddlCuisine")) ? (EyouSoft.Model.EnumType.SourceStructure.Cuisine?)Utils.GetInt(Utils.GetQueryStringValue("ddlCuisine")) : null;

            EyouSoft.BLL.SourceStructure.BRestaurantSupplier bll = new EyouSoft.BLL.SourceStructure.BRestaurantSupplier();
            IList<EyouSoft.Model.SourceStructure.MPageRestaurant> list = bll.GetList(SiteUserInfo.CompanyId, pageSize, pageIndex, ref recordCount, search);
            //绑定数据
            if (list != null && list.Count > 0)
            {
                UtilsCommons.Paging(pageSize, ref pageIndex, recordCount);
                this.rpDinner.DataSource = list;
                this.rpDinner.DataBind();
            }
            else
            {
                Literal1.Text = "<tr align=\"center\"> <td colspan=\"9\">没有相关数据</td></tr>";
            }


            //设置分页
            BindPage();

        }

        /// <summary>
        /// 绑定分页控件
        /// </summary>
        private void BindPage()
        {
            this.ExportPageInfo1.intPageSize = pageSize;
            this.ExportPageInfo1.CurrencyPage = pageIndex;
            this.ExportPageInfo1.intRecordCount = recordCount;
        }


        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        private string Delete()
        {
            string msg = null;
            if (!this.CheckGrant(EyouSoft.Model.EnumType.PrivsStructure.Privs3.供应商管理_餐馆_删除))
            {
                msg = EyouSoft.Common.UtilsCommons.AjaxReturnJson("0", "无删除权限！");
            }
            string stid = Utils.GetFormValue("tid");
            EyouSoft.BLL.SourceStructure.BRestaurantSupplier bll = new EyouSoft.BLL.SourceStructure.BRestaurantSupplier();
            int flg = bll.Delete(stid);
            if (flg == 1)
            {
                msg = EyouSoft.Common.UtilsCommons.AjaxReturnJson("1", "删除成功！");
            }
            else
            {
                msg = EyouSoft.Common.UtilsCommons.AjaxReturnJson("0", "删除失败！");
            }

            return msg;
        }

        /// <summary>
        /// 绑定菜系
        /// </summary>
        /// <param name="selectId"></param>
        /// <returns></returns>
        protected string BindCuisine(string selectItem)
        {
            System.Text.StringBuilder query = new System.Text.StringBuilder();
            query.Append("<option value=''>-请选择菜系-</option>");
            //EyouSoft.Model.EnumType.PersonalCenterStructure.PlanCheckState
            Array values = Enum.GetValues(typeof(EyouSoft.Model.EnumType.SourceStructure.Cuisine));
            foreach (var item in values)
            {
                int value = (int)Enum.Parse(typeof(EyouSoft.Model.EnumType.SourceStructure.Cuisine), item.ToString());
                string text = Enum.GetName(typeof(EyouSoft.Model.EnumType.SourceStructure.Cuisine), item);
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


        #region 权限判断
        private void PowControl()
        {
            if (!this.CheckGrant(EyouSoft.Model.EnumType.PrivsStructure.Privs3.供应商管理_餐馆_栏目))
            {
                Utils.ResponseNoPermit(EyouSoft.Model.EnumType.PrivsStructure.Privs3.供应商管理_餐馆_栏目, true);
            }
            add = this.CheckGrant(EyouSoft.Model.EnumType.PrivsStructure.Privs3.供应商管理_餐馆_新增);
            update = this.CheckGrant(EyouSoft.Model.EnumType.PrivsStructure.Privs3.供应商管理_餐馆_修改);
            del = this.CheckGrant(EyouSoft.Model.EnumType.PrivsStructure.Privs3.供应商管理_餐馆_删除);

        }
        #endregion
    }
}
