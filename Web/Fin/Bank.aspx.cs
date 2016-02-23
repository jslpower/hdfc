using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using EyouSoft.Common;

namespace Web.Fin
{
    public partial class Bank : EyouSoft.Common.Page.BackPage
    {
        #region 设置分页变量
        protected int pageIndex = 1;
        protected int recordCount;
        protected int pageSize = 10;
        #endregion

        #region 权限变量
        protected bool add = false;//新增
        protected bool del = false;
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
            //        public IList<MBankBalance> GetBankBalance(int companyId, int pageSize, int pageIndex, ref int recordCount
            //, MSearchBankBalance search)

            pageIndex = Utils.GetInt(Utils.GetQueryStringValue("page"), 1);

            EyouSoft.Model.FinStructure.MSearchBankBalance search = new EyouSoft.Model.FinStructure.MSearchBankBalance();
            search.StartDate = Utils.GetDateTimeNullable(Utils.GetQueryStringValue("txtBeginDate"));
            search.EndDate = Utils.GetDateTimeNullable(Utils.GetQueryStringValue("txtEndDate"));

            decimal totalBalance = 0;

            EyouSoft.BLL.FinStructure.BBankBalance bll = new EyouSoft.BLL.FinStructure.BBankBalance();
            IList<EyouSoft.Model.FinStructure.MBankBalance> list = bll.GetBankBalance(CurrentUserCompanyID, pageSize, pageIndex, ref recordCount, search, out totalBalance);
            //绑定数据
            if (list != null && list.Count > 0)
            {
                UtilsCommons.Paging(pageSize, ref pageIndex, recordCount);
                this.rpBank.DataSource = list;
                this.rpBank.DataBind();
            }
            else
            {
                Literal1.Text = "<tr align=\"center\"> <td colspan=\"5\">没有相关数据</td></tr>";
            }


            //设置分页
            BindPage();

            this.ltTotalMoney.Text = ToMoneyString(totalBalance);
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



        private string Delete()
        {
            string msg = null;
            if (!this.CheckGrant(EyouSoft.Model.EnumType.PrivsStructure.Privs3.财务管理_银行余额_删除))
            {
                msg = EyouSoft.Common.UtilsCommons.AjaxReturnJson("0", "无删除权限！");
            }
            string stid = Utils.GetFormValue("tid");
            EyouSoft.BLL.FinStructure.BBankBalance bll = new EyouSoft.BLL.FinStructure.BBankBalance();
            int flg = bll.DeleteBankBalance(Utils.GetInt(stid));
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
            if (!this.CheckGrant(EyouSoft.Model.EnumType.PrivsStructure.Privs3.财务管理_银行余额_栏目))
            {
                Utils.ResponseNoPermit(EyouSoft.Model.EnumType.PrivsStructure.Privs3.财务管理_银行余额_栏目, true);
            }
            add = this.CheckGrant(EyouSoft.Model.EnumType.PrivsStructure.Privs3.财务管理_银行余额_新增);
            del = this.CheckGrant(EyouSoft.Model.EnumType.PrivsStructure.Privs3.财务管理_银行余额_删除);
        }
        #endregion
    }
}
