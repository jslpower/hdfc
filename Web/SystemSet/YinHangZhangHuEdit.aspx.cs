//汪奇志 2012-11-23~2012-12-05
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using EyouSoft.Common.Page;
using EyouSoft.Common;
using EyouSoft.Model.FinStructure;
using EyouSoft.Model.EnumType.CompanyStructure;
using Web.UserControl;

namespace Web.SystemSet
{
    /// <summary>
    /// 财务管理-银行账户新增、修改
    /// </summary>
    public partial class YinHangZhangHuEdit : BackPage
    {
        #region attributes
        /// <summary>
        /// 银行账户编号
        /// </summary>
        string ZhangHuId = string.Empty;
        /// <summary>
        /// 银行账户操作权限
        /// </summary>
        bool Privs_Insert = false;
        #endregion

        protected void Page_Load(object sender, EventArgs e)
        {
            ZhangHuId = Utils.GetQueryStringValue("zhanghuid");
            InitPrivs();

            if (Utils.GetQueryStringValue("doType") == "save") Save();

            InitInfo();
        }

        #region private members


        /// <summary>
        /// init privs
        /// </summary>
        void InitPrivs()
        {
            Privs_Insert = CheckGrant(EyouSoft.Model.EnumType.PrivsStructure.Privs3.系统设置_基础设置_公司账号栏目);
        }

        /// <summary>
        /// 初始化银行账户信息
        /// </summary>
        void InitInfo()
        {
            if (Privs_Insert) ltrOperatorHtml.Text = "<a href=\"javascript:void(0)\" id=\"i_a_save\">保存</a>";
            else ltrOperatorHtml.Text = "你没有银行账户操作权限";

            var info = new EyouSoft.BLL.CompanyStructure.BYinHangZhangHu().GetInfo(ZhangHuId, CurrentUserCompanyID);
            if (info == null) return;

            txtName.Value = info.AccountName;
            txtYinHangName.Value = info.BankName;
            txtZhangHao.Value = info.BankNo;

            switch (info.AccountState)
            {
                case EyouSoft.Model.EnumType.CompanyStructure.AccountState.未审批:
                    if (Privs_Insert) ltrOperatorHtml.Text = "<a href=\"javascript:void(0)\" id=\"i_a_save\">保存</a>";
                    else ltrOperatorHtml.Text = "你没有银行账户操作权限";
                    break;
                case EyouSoft.Model.EnumType.CompanyStructure.AccountState.可用:
                    ltrOperatorHtml.Text = "银行账户状态为可用";
                    break;
                case EyouSoft.Model.EnumType.CompanyStructure.AccountState.不可用:
                    ltrOperatorHtml.Text = "银行账户状态为不可用";
                    break;
                default: break;
            }
        }

        /// <summary>
        /// 新增或修改
        /// </summary>
        void Save()
        {
            if (!Privs_Insert) RCWE(UtilsCommons.AjaxReturnJson("-1000", "操作失败：没有操作权限。"));

            var info = GetFormInfo();
            info.Id = ZhangHuId;
            int bllRetCode = 4;

            if (string.IsNullOrEmpty(ZhangHuId)) bllRetCode = new EyouSoft.BLL.CompanyStructure.BYinHangZhangHu().Insert(info);
            else bllRetCode = new EyouSoft.BLL.CompanyStructure.BYinHangZhangHu().Update(info);

            if (bllRetCode == 1) RCWE(UtilsCommons.AjaxReturnJson("1", "操作成功"));
            else if (bllRetCode == -2) RCWE(UtilsCommons.AjaxReturnJson(bllRetCode.ToString(), "操作失败：已经存在相同的银行账号"));
            else RCWE(UtilsCommons.AjaxReturnJson(bllRetCode.ToString(), "操作失败：异常代码" + bllRetCode));
        }

        /// <summary>
        /// 获取表单信息
        /// </summary>
        /// <returns></returns>
        EyouSoft.Model.CompanyStructure.CompanyAccount GetFormInfo()
        {
            EyouSoft.Model.CompanyStructure.CompanyAccount info = new EyouSoft.Model.CompanyStructure.CompanyAccount();

            info.AccountMoney = Utils.GetDecimal(Utils.GetFormValue("txtJinE"));
            info.AccountName = Utils.GetFormValue("txtName");
            info.AccountType = Utils.GetEnumValue<AccountType>(Utils.GetFormValue("txtXingZhi"), AccountType.对公);
            info.BankName = Utils.GetFormValue("txtYinHangName");
            info.BankNo = Utils.GetFormValue("txtZhangHao");
            info.CompanyId = CurrentUserCompanyID;
            info.FilePath = string.Empty;
            info.OperatorId = SiteUserInfo.UserId;

            string file = Utils.GetFormValue("txtFilePath");
            if (!string.IsNullOrEmpty(file))
            {
                string[] _arr = file.Split('|');
                if (_arr.Length == 2) info.FilePath = _arr[1];
            }
            //info.FilePath = Utils.GetFormValue("txtFilePath");

            if (string.IsNullOrEmpty(info.FilePath)) info.FilePath = Utils.GetFormValue("txtYFilePath");

            return info;
        }

        /// <summary>
        /// 获取银行账户性质HTML
        /// </summary>
        /// <param name="selectValue">要选中的值</param>
        /// <returns></returns>
        string GetXingZhiHtml(string selectValue)
        {
            if (string.IsNullOrEmpty(selectValue)) selectValue = "";
            System.Text.StringBuilder s = new System.Text.StringBuilder();
            var items = EyouSoft.Common.EnumObj.GetList(typeof(AccountType));

            if (items != null && items.Count > 0)
            {
                foreach (var item in items)
                {
                    if (item.Value == selectValue) s.AppendFormat("<option value=\"{0}\" selected=\"selected\">{1}</option>", item.Value, item.Text);
                    else s.AppendFormat("<option value=\"{0}\">{1}</option>", item.Value, item.Text);
                }
            }

            return s.ToString();
        }
        #endregion
    }
}
