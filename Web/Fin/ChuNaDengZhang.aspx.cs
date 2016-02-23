using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using EyouSoft.Common;

namespace Web.Fin
{
    public partial class ChuNaDengZhang : EyouSoft.Common.Page.BackPage
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
                PageInit();
            }
        }

        /// <summary>
        /// 页面初始化
        /// </summary>
        protected void PageInit()
        {
            pageIndex = Utils.GetInt(Utils.GetQueryStringValue("page"), 1);

            EyouSoft.Model.FinStructure.MSearchChuNaDengZhang search = new EyouSoft.Model.FinStructure.MSearchChuNaDengZhang();
            search.StartTime = Utils.GetDateTimeNullable(Utils.GetQueryStringValue("txtBeginDate"));
            search.EndTime = Utils.GetDateTimeNullable(Utils.GetQueryStringValue("txtEndDate"));
            int tmp = Utils.GetInt(Utils.GetQueryStringValue("ddlStatus"), -1);
            if (tmp >= 0) search.DengZhangType = (EyouSoft.Model.EnumType.FinStructure.DengJiMode)tmp;


            EyouSoft.Model.FinStructure.MChuNaDengZhangHeJi heJi = new EyouSoft.Model.FinStructure.MChuNaDengZhangHeJi();

            EyouSoft.BLL.FinStructure.BChuNaDengZhang bll = new EyouSoft.BLL.FinStructure.BChuNaDengZhang();
            IList<EyouSoft.Model.FinStructure.MChuNaDengZhang> list = bll.GetChuNaDengZhang(CurrentUserCompanyID, pageSize, pageIndex, ref recordCount, search, ref heJi);
            //绑定数据
            if (list != null && list.Count > 0)
            {
                UtilsCommons.Paging(pageSize, ref pageIndex, recordCount);
                this.rpBank.DataSource = list;
                this.rpBank.DataBind();
            }
            else
            {
                Literal1.Text = "<tr align=\"center\"> <td colspan=\"10\">没有相关数据</td></tr>";
            }


            //设置分页
            BindPage();

            this.ltTotalMoney.Text = ToMoneyString(heJi.SumPrice);
            this.ltTotalShouXuMoney.Text = ToMoneyString(heJi.SumOtherPrice);
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
        /// 绑定状态
        /// </summary>
        /// <returns></returns>
        public string BindStatus(string selectItem)
        {
            System.Text.StringBuilder query = new System.Text.StringBuilder();
            query.Append("<option value='-1'>-请选择-</option>");
            //EyouSoft.Model.EnumType.PersonalCenterStructure.PlanCheckState
            Array values = Enum.GetValues(typeof(EyouSoft.Model.EnumType.FinStructure.DengJiMode));
            foreach (var item in values)
            {
                int value = (int)Enum.Parse(typeof(EyouSoft.Model.EnumType.FinStructure.DengJiMode), item.ToString());
                string text = Enum.GetName(typeof(EyouSoft.Model.EnumType.FinStructure.DengJiMode), item);
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
        /// <returns></returns>
        protected string Delete()
        {
            string msg = null;

            if (!this.CheckGrant(EyouSoft.Model.EnumType.PrivsStructure.Privs3.财务管理_出纳登账_删除))
            {
                msg = EyouSoft.Common.UtilsCommons.AjaxReturnJson("0", "无删除权限！");
            }
            string stid = Utils.GetFormValue("tid");
            EyouSoft.BLL.FinStructure.BChuNaDengZhang bll = new EyouSoft.BLL.FinStructure.BChuNaDengZhang();
            int flg = bll.DeleteChuNaDengZhang(stid);
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
        /// 获取附件路径
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        protected string GetFilePath(object obj)
        {
            string file = "<a href='javascript:void(0)'><img src='/images/fujian_bg.gif' alt='' width='15' height='14' /></a>";

            if (obj != null)
            {
                IList<EyouSoft.Model.FinStructure.MChuNaDengZhangFile> list = obj as IList<EyouSoft.Model.FinStructure.MChuNaDengZhangFile>;
                if (list != null && list.Count > 0)
                {
                    file = string.Format("<a target='_blank' href='{0}'> <img src='/images/fujian_bg.gif' alt='' width='15' height='14' /></a>", list.FirstOrDefault().FilePath);
                }
            }
            return file;
        }


        #region 判断权限
        /// <summary>
        /// 权限判断
        /// </summary>
        private void PowControl()
        {
            if (!this.CheckGrant(EyouSoft.Model.EnumType.PrivsStructure.Privs3.财务管理_出纳登账_栏目))
            {
                Utils.ResponseNoPermit(EyouSoft.Model.EnumType.PrivsStructure.Privs3.财务管理_出纳登账_栏目, true);
            }
            add = this.CheckGrant(EyouSoft.Model.EnumType.PrivsStructure.Privs3.财务管理_出纳登账_新增);
            update = this.CheckGrant(EyouSoft.Model.EnumType.PrivsStructure.Privs3.财务管理_出纳登账_修改);
            del = this.CheckGrant(EyouSoft.Model.EnumType.PrivsStructure.Privs3.财务管理_出纳登账_删除);

        }
        #endregion
    }
}
