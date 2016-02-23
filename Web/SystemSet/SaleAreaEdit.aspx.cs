using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using EyouSoft.Common;

namespace Web.SystemSet
{
    /// <summary>
    /// 编辑销售地区
    /// </summary>
    public partial class SaleAreaEdit : EyouSoft.Common.Page.BackPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            string save = Utils.GetQueryStringValue("save");
            if (!string.IsNullOrEmpty(save))
            {
                Save();
                return;
            }
            string action = Utils.GetQueryStringValue("action").ToLower();
            int sId = Utils.GetInt(Utils.GetQueryStringValue("sId"));

            if (!IsPostBack)
            {
                if (!CheckGrant(EyouSoft.Model.EnumType.PrivsStructure.Privs3.系统设置_基础设置_销售地区栏目))
                {
                    Utils.ShowMsgAndCloseBoxy(
                        string.Format("您没有{0}的权限，请联系管理员！", EyouSoft.Model.EnumType.PrivsStructure.Privs3.系统设置_基础设置_销售地区栏目),
                        Utils.GetQueryStringValue("iframeId"),
                        false);
                    return;
                }

                if (action == "edit")
                {
                    if (sId <= 0)
                    {
                        Utils.ShowMsgAndCloseBoxy("url错误，请重新操作！", Utils.GetQueryStringValue("iframeId"), false);
                        return;
                    }

                    InitPage(sId);
                }
            }
        }

        /// <summary>
        /// 初始化销售地区
        /// </summary>
        /// <param name="sId">销售地区编号</param>
        private void InitPage(int sId)
        {
            if (sId <= 0) return;

            var model = new EyouSoft.BLL.CompanyStructure.BSaleArea().GetSaleArea(sId);
            if (model == null) return;

            txtSaleAreaName.Value = model.SaleAreaName;
        }

        /// <summary>
        /// 保存数据
        /// </summary>
        private void Save()
        {
            if (!CheckGrant(EyouSoft.Model.EnumType.PrivsStructure.Privs3.系统设置_基础设置_销售地区栏目))
            {
                this.RCWE(
                    UtilsCommons.AjaxReturnJson(
                        "0",
                        string.Format(
                            "您没有{0}的权限，请联系管理员！", EyouSoft.Model.EnumType.PrivsStructure.Privs3.系统设置_基础设置_销售地区栏目)));
                return;
            }
            string action = Utils.GetQueryStringValue("action").ToLower();
            int sId = Utils.GetInt(Utils.GetQueryStringValue("sId"));
            if (string.IsNullOrEmpty(action))
            {
                this.RCWE(UtilsCommons.AjaxReturnJson("0", "url错误，请重新打开此窗口！"));
                return;
            }
            if (action == "edit" && sId <= 0)
            {
                this.RCWE(UtilsCommons.AjaxReturnJson("0", "url错误，请重新打开此窗口！"));
                return;
            }

            var bll = new EyouSoft.BLL.CompanyStructure.BSaleArea();
            var model = new EyouSoft.Model.CompanyStructure.MSaleArea { CompanyId = CurrentUserCompanyID, SaleAreaName = Utils.GetFormValue(txtSaleAreaName.UniqueID) };

            int r = 0;
            if (action == "add")
            {
                r = bll.AddSaleArea(model);
            }
            else if (action == "edit")
            {
                model.SaleAreaId = sId;
                r = bll.UpdateSaleArea(model);
            }

            switch (r)
            {
                case 1:
                    this.RCWE(UtilsCommons.AjaxReturnJson("1", "操作成功！"));
                    break;
                case 0:
                    this.RCWE(UtilsCommons.AjaxReturnJson("0", "操作失败！"));
                    break;
                case -1:
                    this.RCWE(UtilsCommons.AjaxReturnJson("0", "销售地区名称已经存在！"));
                    break;
                default:
                    this.RCWE(UtilsCommons.AjaxReturnJson("0", "操作失败！"));
                    break;
            }
        }
    }
}
