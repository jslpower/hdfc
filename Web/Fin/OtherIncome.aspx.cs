using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using EyouSoft.Common;

namespace Web.Fin
{
    public partial class OtherIncome : EyouSoft.Common.Page.BackPage
    {

        #region 设置分页变量
        protected int pageIndex = 1;
        protected int recordCount;
        protected int pageSize = 10;
        #endregion

        #region
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
                PageInit();
            }
        }

        /// <summary>
        /// 页面初始化
        /// </summary>
        protected void PageInit()
        {
            pageIndex = Utils.GetInt(Utils.GetQueryStringValue("page"), 1);

            EyouSoft.Model.FinStructure.MSearchOther search = new EyouSoft.Model.FinStructure.MSearchOther();
            search.StartTime = Utils.GetDateTimeNullable(Utils.GetQueryStringValue("txtBeginDate"));
            search.EndTime = Utils.GetDateTimeNullable(Utils.GetQueryStringValue("txtEndDate"));
            search.ShouZhiXingMu = Utils.GetQueryStringValue("txtOtherIncomeItem");

            EyouSoft.BLL.FinStructure.BQiTaShouKuan bll = new EyouSoft.BLL.FinStructure.BQiTaShouKuan();


            IList<EyouSoft.Model.FinStructure.MQiTaShouKuan> list = bll.GetFinCopeList(CurrentUserCompanyID, pageSize, pageIndex, ref recordCount, search);
            //绑定数据
            if (list != null && list.Count > 0)
            {
                UtilsCommons.Paging(pageSize, ref pageIndex, recordCount);
                this.rpBank.DataSource = list;
                this.rpBank.DataBind();
            }
            else
            {
                Literal1.Text = "<tr align=\"center\"> <td colspan=\"8\">没有相关数据</td></tr>";
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
        /// 获取附件路径
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        protected string GetFilePath(object obj)
        {
            string file = "<a href='javascript:void(0)'><img src='/images/fujian_bg.gif' alt='' width='15' height='14' /></a>";

            if (obj != null)
            {
                IList<EyouSoft.Model.FinStructure.MKuanFile> list = obj as IList<EyouSoft.Model.FinStructure.MKuanFile>;
                if (list != null && list.Count > 0)
                {
                    file = string.Format("<a target='_blank' href='{0}'> <img src='/images/fujian_bg.gif' alt='' width='15' height='14' /></a>", list.FirstOrDefault().FilePath);
                }
            }
            return file;
        }


        /// <summary>
        /// 删除操作
        /// </summary>
        /// <returns></returns>
        protected string Delete()
        {
            string msg = null;

            if (!this.CheckGrant(EyouSoft.Model.EnumType.PrivsStructure.Privs3.财务管理_其他收入_删除))
            {
                msg = EyouSoft.Common.UtilsCommons.AjaxReturnJson("0", "无删除权限！");
            }
            string stid = Utils.GetFormValue("tid");
            EyouSoft.BLL.FinStructure.BQiTaShouKuan bll = new EyouSoft.BLL.FinStructure.BQiTaShouKuan();
            int flg = bll.DeleteFinCope(stid);
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



        #region 判断权限
        /// <summary>
        /// 权限判断
        /// </summary>
        private void PowControl()
        {
            if (!this.CheckGrant(EyouSoft.Model.EnumType.PrivsStructure.Privs3.财务管理_其他收入_栏目))
            {
                Utils.ResponseNoPermit(EyouSoft.Model.EnumType.PrivsStructure.Privs3.财务管理_其他收入_栏目, true);
            }
            add = this.CheckGrant(EyouSoft.Model.EnumType.PrivsStructure.Privs3.财务管理_其他收入_新增);
            update = this.CheckGrant(EyouSoft.Model.EnumType.PrivsStructure.Privs3.财务管理_其他收入_修改);
            del = this.CheckGrant(EyouSoft.Model.EnumType.PrivsStructure.Privs3.财务管理_其他收入_删除);

        }
        #endregion
    }
}
