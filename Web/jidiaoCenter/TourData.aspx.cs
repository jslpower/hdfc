using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using EyouSoft.Common;

namespace Web.jidiaoCenter
{
    public partial class TourData : EyouSoft.Common.Page.BackPage
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
        protected bool check = false;//审核
        #endregion

        protected void Page_Load(object sender, EventArgs e)
        {
            PowControl();
            string id = Utils.GetQueryStringValue("id");
            string dotype = Utils.GetQueryStringValue("dotype");

            if (!string.IsNullOrEmpty(dotype))
            {
                if (dotype.Equals("delete"))
                {
                    Response.Clear();
                    Response.Write(DeleteData(id));
                    Response.End();
                }

                if (dotype.Equals("check"))
                {
                    Response.Clear();
                    Response.Write(CheckData(id));
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
            //IList<EyouSoft.Model.TourStructure.MTourData> GetList(int companyId, int pageSize, int pageIndex, ref int recordCount, EyouSoft.Model.TourStructure.MSearchTourData search)

            EyouSoft.Model.TourStructure.MSearchTourData search = new EyouSoft.Model.TourStructure.MSearchTourData();
            search.RouteName = Utils.GetQueryStringValue("txtName");
            search.IsCheck = !string.IsNullOrEmpty(Utils.GetQueryStringValue("ddlIsCheck")) ? (bool?)(Utils.GetQueryStringValue("ddlIsCheck") == "1") : null;
            search.AreaId = !string.IsNullOrEmpty(Utils.GetQueryStringValue("AreaId")) ? (int?)Utils.GetInt(Utils.GetQueryStringValue("AreaId")) : null;

            EyouSoft.BLL.TourStructure.BTourData bll = new EyouSoft.BLL.TourStructure.BTourData();

            IList<EyouSoft.Model.TourStructure.MTourData> list = bll.GetList(CurrentUserCompanyID, pageSize, pageIndex, ref recordCount, search);
            //绑定数据
            if (list != null && list.Count > 0)
            {
                UtilsCommons.Paging(pageSize, ref pageIndex, recordCount);
                this.rpTourData.DataSource = list;
                this.rpTourData.DataBind();
            }
            else
            {
                Literal1.Text = "<tr align=\"center\"> <td colspan=\"7\">没有相关数据</td></tr>";
            }

            //绑定线路
            EyouSoft.BLL.CompanyStructure.Area area_bll = new EyouSoft.BLL.CompanyStructure.Area();
            IList<EyouSoft.Model.CompanyStructure.Area> _list = area_bll.GetAreaByCompanyId(CurrentUserCompanyID);
            if (_list != null && _list.Count != 0)
            {
                this.rpRoute.DataSource = _list;
                this.rpRoute.DataBind();
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
        /// 删除操作
        /// </summary>
        /// <param name="id">删除ID</param>
        /// <returns></returns>
        private string DeleteData(string id)
        {
            string msg = string.Empty;
            if (!this.CheckGrant(EyouSoft.Model.EnumType.PrivsStructure.Privs3.计调中心_团队报价资料库_删除))
            {
                msg = UtilsCommons.AjaxReturnJson("0", "无删除权限！");
            }

            if (!String.IsNullOrEmpty(id))
            {
                EyouSoft.BLL.TourStructure.BTourData bll = new EyouSoft.BLL.TourStructure.BTourData();
                int result = bll.Delete(Utils.GetInt(id));
                if (result == 1)
                {
                    msg = UtilsCommons.AjaxReturnJson("1", "删除成功！");
                }
                else if (result == -1)
                {
                    msg = UtilsCommons.AjaxReturnJson("0", "资料已审核,不允许删除！");
                }
                else
                {
                    msg = UtilsCommons.AjaxReturnJson("0", "删除失败！");
                }
            }
            else
            {
                msg = UtilsCommons.AjaxReturnJson("0", "未知错误！");
            }
            return msg;
        }

        /// <summary>
        /// 审核
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        private string CheckData(string id)
        {
            string msg = string.Empty;
            if (!this.CheckGrant(EyouSoft.Model.EnumType.PrivsStructure.Privs3.计调中心_团队报价资料库_审核))
            {
                msg = UtilsCommons.AjaxReturnJson("1", "无审核权限！");
            }
            if (!String.IsNullOrEmpty(id))
            {
                EyouSoft.BLL.TourStructure.BTourData bll = new EyouSoft.BLL.TourStructure.BTourData();
                int result = bll.Check(Utils.GetInt(id), SiteUserInfo.UserId);
                if (result == 1)
                {
                    msg = UtilsCommons.AjaxReturnJson("1", "审核成功！");
                }

                else
                {
                    msg = UtilsCommons.AjaxReturnJson("0", "审核失败！");
                }
            }
            else
            {
                msg = UtilsCommons.AjaxReturnJson("0", "未知错误！");
            }
            return msg;

        }
        /// <summary>
        /// 绑定审核状态
        /// </summary>
        /// <param name="selectItem"></param>
        /// <returns></returns>
        protected string BindCheck(string selectItem)
        {
            System.Text.StringBuilder query = new System.Text.StringBuilder();
            query.Append("<option value=''>-请选择-</option>");
            if (selectItem.Equals("0"))
            {
                query.AppendFormat("<option value='1'>已审核</option>");
                query.AppendFormat("<option value='0' selected>未审核</option>");
            }

            else if (selectItem.Equals("1"))
            {
                query.AppendFormat("<option value='1' selected>已审核</option>");
                query.AppendFormat("<option value='0' >未审核</option>");
            }
            else
            {
                query.AppendFormat("<option value='1' >已审核</option>");
                query.AppendFormat("<option value='0' >未审核</option>");
            }
            return query.ToString();
        }




        #region 权限判断
        private void PowControl()
        {
            if (!this.CheckGrant(EyouSoft.Model.EnumType.PrivsStructure.Privs3.计调中心_团队报价资料库_栏目))
            {
                Utils.ResponseNoPermit(EyouSoft.Model.EnumType.PrivsStructure.Privs3.计调中心_团队报价资料库_栏目, true);
            }
            add = this.CheckGrant(EyouSoft.Model.EnumType.PrivsStructure.Privs3.计调中心_团队报价资料库_新增);
            update = this.CheckGrant(EyouSoft.Model.EnumType.PrivsStructure.Privs3.计调中心_团队报价资料库_修改);
            del = this.CheckGrant(EyouSoft.Model.EnumType.PrivsStructure.Privs3.计调中心_团队报价资料库_删除);
            check = this.CheckGrant(EyouSoft.Model.EnumType.PrivsStructure.Privs3.计调中心_团队报价资料库_审核);
        }
        #endregion
    }
}
