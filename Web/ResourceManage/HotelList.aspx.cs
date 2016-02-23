using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using EyouSoft.Common;
using EyouSoft.Common.Page;

namespace Web.ResourceManage
{
    public partial class HotelList : BackPage
    {
        #region Private Mebers
        protected int PageSize = 10;  //每页显示的记录
        protected int PageIndex = 1;  //页码
        protected int RecordCount = 0; //总记录数
        #endregion


        //权限变量
        protected bool add = false;//新增
        protected bool update = false;//修改
        protected bool del = false;//删除



        protected void Page_Load(object sender, EventArgs e)
        {
            PowControl();
            string type = Utils.GetQueryStringValue("act");
            if (!string.IsNullOrEmpty(type))
            {
                if (type.Equals("areadel"))
                {
                    Response.Clear();
                    Response.Write(DelHotel());
                    Response.End();
                }
            }


            if (!this.Page.IsPostBack)
            {
                DataInit();

            }
        }


        /// <summary>
        /// 绑定酒店星级
        /// </summary>
        protected string BindHotelStar(string selectItem)
        {
            System.Text.StringBuilder query = new System.Text.StringBuilder();
            query.Append("<option value=''>-请选择酒店星级-</option>");
            //EyouSoft.Model.EnumType.PersonalCenterStructure.PlanCheckState
            Array values = Enum.GetValues(typeof(EyouSoft.Model.EnumType.SourceStructure.HotelStar));
            foreach (var item in values)
            {
                int value = (int)Enum.Parse(typeof(EyouSoft.Model.EnumType.SourceStructure.HotelStar), item.ToString());
                string text = Enum.GetName(typeof(EyouSoft.Model.EnumType.SourceStructure.HotelStar), item);
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





        #region 删除
        /// <summary>
        /// 删除数据
        /// </summary>
        private string DelHotel()
        {
            string msg = null;
            if (!this.CheckGrant(EyouSoft.Model.EnumType.PrivsStructure.Privs3.供应商管理_酒店_删除))
            {
                msg = EyouSoft.Common.UtilsCommons.AjaxReturnJson("0", "无删除权限！");
            }

            string stid = Utils.GetFormValue("tid");
            EyouSoft.BLL.SourceStructure.BHotelSupplier bll = new EyouSoft.BLL.SourceStructure.BHotelSupplier();
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
        #endregion

        #region 初始化
        protected void DataInit()
        {
            //初使化条件
            PageIndex = Utils.GetInt(Utils.GetQueryStringValue("page"), 1);

            EyouSoft.Model.SourceStructure.MSearchHotel search = new EyouSoft.Model.SourceStructure.MSearchHotel();
            search.ProvinceId = !string.IsNullOrEmpty(Utils.GetQueryStringValue("txtProvince")) ? (int?)Utils.GetInt(Utils.GetQueryStringValue("txtProvince")) : null;
            search.CityId = !string.IsNullOrEmpty(Utils.GetQueryStringValue("txtCity")) ? (int?)Utils.GetInt(Utils.GetQueryStringValue("txtCity")) : null;
            search.UnitName = Utils.GetQueryStringValue("txtUnionName");
            search.HotelStar = !string.IsNullOrEmpty(Utils.GetQueryStringValue("ddlHotelStar")) ? (EyouSoft.Model.EnumType.SourceStructure.HotelStar?)Utils.GetInt(Utils.GetQueryStringValue("ddlHotelStar")) : null;

            EyouSoft.BLL.SourceStructure.BHotelSupplier bll = new EyouSoft.BLL.SourceStructure.BHotelSupplier();

            IList<EyouSoft.Model.SourceStructure.MPageHotel> list = null;
            list = bll.GetList(CurrentUserCompanyID, PageSize, PageIndex, ref RecordCount, search);
            if (list.Count > 0 && list != null)
            {
                UtilsCommons.Paging(PageSize, ref PageIndex, RecordCount);
                //列表数据绑定           
                this.RepList.DataSource = list;
                this.RepList.DataBind();
            }
            else
            {
                Literal1.Text = "<tr align=\"center\"> <td colspan=\"9\">没有相关数据</td></tr>";
            }
            //设置分页
            BindPage();

            list = null;
        }
        #endregion

        #region 绑定分页控件
        /// <summary>
        /// 绑定分页控件
        /// </summary>
        protected void BindPage()
        {
            this.ExporPageInfoSelect1.intPageSize = PageSize;
            this.ExporPageInfoSelect1.CurrencyPage = PageIndex;
            this.ExporPageInfoSelect1.intRecordCount = RecordCount;
        }
        #endregion

        #region 权限判断
        private void PowControl()
        {
            if (!this.CheckGrant(EyouSoft.Model.EnumType.PrivsStructure.Privs3.供应商管理_酒店_栏目))
            {
                Utils.ResponseNoPermit(EyouSoft.Model.EnumType.PrivsStructure.Privs3.供应商管理_酒店_栏目, true);
            }
            add = this.CheckGrant(EyouSoft.Model.EnumType.PrivsStructure.Privs3.供应商管理_酒店_新增);
            update = this.CheckGrant(EyouSoft.Model.EnumType.PrivsStructure.Privs3.供应商管理_酒店_修改);
            del = this.CheckGrant(EyouSoft.Model.EnumType.PrivsStructure.Privs3.供应商管理_酒店_删除);

        }
        #endregion
    }
}
