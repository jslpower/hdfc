using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using EyouSoft.Common;
using EyouSoft.Common.Page;
using System.Text;

namespace Web.ResourceManage
{

    public partial class GroundList : BackPage
    {
        #region 变量
        /// <summary>
        /// 每页显示记录条数
        /// </summary>
        protected int pageSize = 10;
        /// <summary>
        /// 显示第几页
        /// </summary>
        protected int pageIndex = 1;
        /// <summary>
        /// 总记录条数
        /// </summary>
        protected int recordCount;



        //权限变量
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
                    Response.Write(AreaDel());
                    Response.End();
                }
            }

            if (!IsPostBack)
            {
                DataInit();
            }
        }

        /// <summary>
        /// 删除数据
        /// </summary>
        private string AreaDel()
        {
            string msg = null;
            if (!this.CheckGrant(EyouSoft.Model.EnumType.PrivsStructure.Privs3.供应商管理_地接_删除))
            {
                msg = EyouSoft.Common.UtilsCommons.AjaxReturnJson("0", "无删除权限！");
            }
            string stid = Utils.GetFormValue("tid");
            EyouSoft.BLL.SourceStructure.BSupplier bll = new EyouSoft.BLL.SourceStructure.BSupplier();
            int flg = bll.Delete(stid);
            if (flg == 1)
            {
                msg = EyouSoft.Common.UtilsCommons.AjaxReturnJson("1", "删除成功！");
            }
            else if (flg == -1)
            {
                msg = EyouSoft.Common.UtilsCommons.AjaxReturnJson("0", "地接供应商做过安排不允许删除！");
            }
            else if (flg == -2)
            {
                msg = EyouSoft.Common.UtilsCommons.AjaxReturnJson("0", "机票供应商做过安排不允许删除！");
            }
            else
            {
                msg = EyouSoft.Common.UtilsCommons.AjaxReturnJson("0", "删除失败！");
            }

            return msg;
        }

        /// <summary>
        /// 初使化
        /// </summary>
        private void DataInit()
        {
            //初使化条件
            pageIndex = Utils.GetInt(Utils.GetQueryStringValue("page"), 1);

            EyouSoft.Model.SourceStructure.MSupplierSearch search = new EyouSoft.Model.SourceStructure.MSupplierSearch();
            search.ProvinceId = !string.IsNullOrEmpty(Utils.GetQueryStringValue("txtProvince")) ? (int?)Utils.GetInt(Utils.GetQueryStringValue("txtProvince")) : null;
            search.CityId = !string.IsNullOrEmpty(Utils.GetQueryStringValue("txtCity")) ? (int?)Utils.GetInt(Utils.GetQueryStringValue("txtCity")) : null;
            search.UnitName = Utils.GetQueryStringValue("txtUnionName");

            EyouSoft.BLL.SourceStructure.BSupplier bll = new EyouSoft.BLL.SourceStructure.BSupplier();
            IList<EyouSoft.Model.SourceStructure.MPageSupplier> list = bll.GetGroundList(CurrentUserCompanyID, pageSize, pageIndex, ref recordCount, search);
            if (list != null && list.Count > 0)
            {
                UtilsCommons.Paging(pageSize, ref pageIndex, recordCount);
                this.rptList.DataSource = list;
                this.rptList.DataBind();
            }
            else
            {
                Literal1.Text = "<tr align=\"center\"> <td colspan=\"8\">没有相关数据</td></tr>";
            }

            //设置分页
            BindPage();

        }


        #region 绑定分页控件
        /// <summary>
        /// 绑定分页控件
        /// </summary>
        protected void BindPage()
        {
            this.ExporPageInfoSelect1.intPageSize = pageSize;
            this.ExporPageInfoSelect1.CurrencyPage = pageIndex;
            this.ExporPageInfoSelect1.intRecordCount = recordCount;
        }
        #endregion

        #region 权限判断
        private void PowControl()
        {
            if (!this.CheckGrant(EyouSoft.Model.EnumType.PrivsStructure.Privs3.供应商管理_地接_栏目))
            {
                Utils.ResponseNoPermit(EyouSoft.Model.EnumType.PrivsStructure.Privs3.供应商管理_地接_栏目, true);
            }
            add = this.CheckGrant(EyouSoft.Model.EnumType.PrivsStructure.Privs3.供应商管理_地接_新增);
            update = this.CheckGrant(EyouSoft.Model.EnumType.PrivsStructure.Privs3.供应商管理_地接_修改);
            del = this.CheckGrant(EyouSoft.Model.EnumType.PrivsStructure.Privs3.供应商管理_地接_删除);

        }
        #endregion
    }
}
