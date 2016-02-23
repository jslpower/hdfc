using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using EyouSoft.Common;
using EyouSoft.Common.Page;
using EyouSoft.Model.EnumType.CompanyStructure;

namespace Web.ResourceManage
{
    /// <summary>
    /// 创建:王磊
    /// 功能:景点列表
    /// </summary>
    public partial class ScenicList : BackPage
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
            //ajax for delete
            string act = Utils.GetQueryStringValue("act");
            if (!string.IsNullOrEmpty(act))
            {
                if (act.Equals("areadel"))
                {
                    Response.Clear();
                    Response.Write(DelScenic()); 
                    Response.End();
                }
            }

            if (!IsPostBack)
            {
                PageInit();
            }


        }
        #region 绑定页面数据
        protected void PageInit()
        {
            pageIndex = Utils.GetInt(Utils.GetQueryStringValue("Page"), 1);

            EyouSoft.Model.SourceStructure.MSearchSpot search = new EyouSoft.Model.SourceStructure.MSearchSpot();
            search.ProvinceId = !string.IsNullOrEmpty(Utils.GetQueryStringValue("txtProvince")) ? (int?)Utils.GetInt(Utils.GetQueryStringValue("txtProvince")) : null; ;
            search.CityId = !string.IsNullOrEmpty(Utils.GetQueryStringValue("txtCity")) ? (int?)Utils.GetInt(Utils.GetQueryStringValue("txtCity")) : null; ;
            search.UnitName = Utils.GetQueryStringValue("sight_name");
            search.SpotStar = !string.IsNullOrEmpty(Utils.GetQueryStringValue("ddlscStar")) ? (EyouSoft.Model.EnumType.SourceStructure.SpotStar?)Utils.GetInt(Utils.GetQueryStringValue("ddlscStar")) : null;

            EyouSoft.BLL.SourceStructure.BSpotSupplier bll = new EyouSoft.BLL.SourceStructure.BSpotSupplier();
            IList<EyouSoft.Model.SourceStructure.MPageSpot> list = bll.GetList(CurrentUserCompanyID, pageSize, pageIndex, ref recordCount, search);
            if (list != null && list.Count != 0)
            {
                UtilsCommons.Paging(pageSize, ref pageIndex, recordCount);
                this.rptList.DataSource = list;
                this.rptList.DataBind();
            }
            else
            {
                Literal1.Text = "<tr align=\"center\"> <td colspan=\"13\">没有相关数据</td></tr>";
            }

            BindExportPage();
        }
        #endregion



        #region 绑定分页控件
        /// <summary>
        /// 绑定分页控件
        /// </summary>
        protected void BindExportPage()
        {
            this.ExportPageInfo1.intPageSize = pageSize;
            this.ExportPageInfo1.CurrencyPage = pageIndex;
            this.ExportPageInfo1.intRecordCount = recordCount;
        }
        #endregion

        /// <summary>
        /// 删除
        /// </summary>
        /// <returns></returns>
        protected string DelScenic()  
        {
            string msg = null;
            if (!this.CheckGrant(EyouSoft.Model.EnumType.PrivsStructure.Privs3.供应商管理_景点_删除))
            {
                msg = EyouSoft.Common.UtilsCommons.AjaxReturnJson("0", "无删除权限！");
            }

            string stid = Utils.GetFormValue("tid");
            EyouSoft.BLL.SourceStructure.BSpotSupplier bll = new EyouSoft.BLL.SourceStructure.BSpotSupplier();
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

        #region 绑定星级
        protected string getStarScenic(string selectItem)
        {
            System.Text.StringBuilder query = new System.Text.StringBuilder();
            query.Append("<option value=''>-请选择-</option>");
            //EyouSoft.Model.EnumType.PersonalCenterStructure.PlanCheckState
            Array values = Enum.GetValues(typeof(EyouSoft.Model.EnumType.SourceStructure.SpotStar));
            foreach (var item in values)
            {
                int value = (int)Enum.Parse(typeof(EyouSoft.Model.EnumType.SourceStructure.SpotStar), item.ToString());
                string text = Enum.GetName(typeof(EyouSoft.Model.EnumType.SourceStructure.SpotStar), item);
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
        #endregion



        #region 权限判断
        private void PowControl()
        {
            if (!this.CheckGrant(EyouSoft.Model.EnumType.PrivsStructure.Privs3.供应商管理_景点_栏目))
            {
                Utils.ResponseNoPermit(EyouSoft.Model.EnumType.PrivsStructure.Privs3.供应商管理_景点_栏目, true);
            }
            add = this.CheckGrant(EyouSoft.Model.EnumType.PrivsStructure.Privs3.供应商管理_景点_新增);
            update = this.CheckGrant(EyouSoft.Model.EnumType.PrivsStructure.Privs3.供应商管理_景点_修改);
            del = this.CheckGrant(EyouSoft.Model.EnumType.PrivsStructure.Privs3.供应商管理_景点_删除);

        }
        #endregion
    }
}
