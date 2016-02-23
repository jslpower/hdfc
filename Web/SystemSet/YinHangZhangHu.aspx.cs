//汪奇志 2012-11-23~2012-12-05
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using EyouSoft.Common.Page;
using EyouSoft.Common;
using EyouSoft.Model.EnumType.FinStructure;
using EyouSoft.Model.EnumType.CompanyStructure;

namespace Web.SystemSet
{
    /// <summary>
    /// 财务管理-银行账号表
    /// </summary>
    public partial class YinHangZhangHu : BackPage
    {
        #region attributes
        /// <summary>
        /// 删除权限
        /// </summary>
        bool Privs_Delete = false;
        /// <summary>
        /// 新增权限
        /// </summary>
        protected bool Privs_Insert = false;
        #endregion

        protected void Page_Load(object sender, EventArgs e)
        {
            InitPrivs();

            if (Utils.GetQueryStringValue("doType") == "delete") Delete();
            switch (Utils.GetQueryStringValue("doType"))
            {
                case "delete": Delete(); break;
                default: break;
            }

            InitRpts();
        }

        #region private members
        /// <summary>
        /// init privs
        /// </summary>
        void InitPrivs()
        {
            if (!CheckGrant(EyouSoft.Model.EnumType.PrivsStructure.Privs3.系统设置_基础设置_公司账号栏目))
            {
                if (!string.IsNullOrEmpty(Utils.GetQueryStringValue("doType"))) RCWE(UtilsCommons.AjaxReturnJson("-1000", "操作失败：没有操作权限。"));
                else Utils.ResponseNoPermit(EyouSoft.Model.EnumType.PrivsStructure.Privs3.系统设置_基础设置_公司账号栏目, true);                
            }

            Privs_Insert = Privs_Delete = CheckGrant(EyouSoft.Model.EnumType.PrivsStructure.Privs3.系统设置_基础设置_公司账号栏目);

            phInsert.Visible = Privs_Insert;
        }

        /// <summary>
        /// init repeater
        /// </summary>
        void InitRpts()
        {
            var items = new EyouSoft.BLL.CompanyStructure.BYinHangZhangHu().GetZhangHus(CurrentUserCompanyID);

            if (items != null && items.Count > 0)
            {
                rpts.DataSource = items;
                rpts.DataBind();

                //合计信息

                rpts.Visible = phHeJi.Visible = true;
                phPaging.Visible = phEmpty.Visible = false;
            }
            else
            {
                rpts.Visible = phHeJi.Visible = phPaging.Visible = false;
                phEmpty.Visible = true;
            }
        }

        /// <summary>
        /// 删除银行账号信息
        /// </summary>
        void Delete()
        {
            if (!Privs_Delete) RCWE(UtilsCommons.AjaxReturnJson("-1000", "操作失败：没有操作权限。"));

            string zhanghuid = Utils.GetFormValue("zhanghuid");
            int bllRetCode = new EyouSoft.BLL.CompanyStructure.BYinHangZhangHu().Delete(zhanghuid, CurrentUserCompanyID);

            if (bllRetCode == 1) RCWE(UtilsCommons.AjaxReturnJson("1", "操作成功"));
            else RCWE(UtilsCommons.AjaxReturnJson(bllRetCode.ToString(), "操作失败：异常代码" + bllRetCode));
        }

        #endregion

        #region protected members
        /// <summary>
        /// 获取操作列HTML
        /// </summary>
        /// <param name="status">状态</param>
        /// <returns></returns>
        protected string GetOperatorHtml(object status)
        {
            string s = string.Empty;
            AccountState _status = (AccountState)status;

            if (_status == AccountState.未审批)
            {
                if (Privs_Insert) s += "<a href=\"javascript:void(0)\" class=\"i_update\">修改</a> ";
                else s += "<a href=\"javascript:void(0)\" class=\"i_update\" i_chakan=\"1\">查看</a> ";

                if (Privs_Delete) s += "<a href=\"javascript:void(0)\" class=\"i_delete\">删除</a> ";
            }
            else
            {
                s += "<a href=\"javascript:void(0)\" class=\"i_update\" i_chakan=\"1\">查看</a> ";
            }

            return s.ToString();
        }
        #endregion
    }
}
