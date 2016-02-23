using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Text;
using System.Web.UI;
using System.Web.UI.WebControls;
using EyouSoft.Common;
using EyouSoft.Common.Function;

namespace Web.SMS
{
    /// <summary>
    /// 客观关怀下的短信发送列表
    /// xuty 2011/1/20
    /// </summary>
    public partial class AjaxSendSMSList : EyouSoft.Common.Page.BackPage
    {
        protected int recordCount;
        protected int pageIndex = 1;
        protected int pageSize = 10;
        protected int itemIndex;//序号
        protected void Page_Load(object sender, EventArgs e)
        {
            //判断权限
            if (!CheckGrant(EyouSoft.Model.EnumType.PrivsStructure.Privs3.短信中心_短信中心_栏目))
            {
                Utils.ResponseNoPermit(EyouSoft.Model.EnumType.PrivsStructure.Privs3.短信中心_短信中心_栏目, true);
                return;
            }
            pageIndex = Utils.GetInt(Utils.GetQueryStringValue("Page"), 1);
            itemIndex = (pageIndex - 1) * pageSize + 1;
            //客户关怀bll
            EyouSoft.BLL.CompanyStructure.CustomerCareFor careBll = new EyouSoft.BLL.CompanyStructure.CustomerCareFor();
            //绑定客户关怀列表
            IList<EyouSoft.Model.CompanyStructure.CustomerCareforInfo> list = careBll.GetList(CurrentUserCompanyID.ToString(), pageSize, pageIndex, ref recordCount);
            if (list != null && list.Count > 0)
            {
                rptSmsList.DataSource = list;
                rptSmsList.DataBind();
                BindExportPage();
            }
            else
            {
                rptSmsList.EmptyText = "<tr><td colspan='10' align='center'>对不起，暂无短信发送信息！</td></tr>";
                ExporPageInfoSelect1.Visible = false;
            }
        }
        #region 绑定分页控件
        /// <summary>
        /// 绑定分页控件
        /// </summary>
        protected void BindExportPage()
        {
            ExporPageInfoSelect1.CurrencyPage = pageIndex;
            ExporPageInfoSelect1.intPageSize = pageSize;
            ExporPageInfoSelect1.intRecordCount = recordCount;
            //添加点击事件用ajax获取订单数据
            ExporPageInfoSelect1.AttributesEventAdd("onclick", "return Care.getSmsList(this)", 1);

        }
        #endregion

        /// <summary>
        /// 绑定列表时获取发送范围
        /// </summary>
        /// <param name="IsMatchCustomerInfo"></param>
        /// <param name="IsMatchSupplierInfo"></param>
        /// <param name="IsMatchDepartmentInfo"></param>
        /// <param name="mobiles"></param>
        /// <returns></returns>
        protected string GetSendInfo(bool IsMatchCustomerInfo, bool IsMatchSupplierInfo, bool IsMatchDepartmentInfo, object mobiles)
        {
            StringBuilder strBuilder = new StringBuilder();
            if (IsMatchCustomerInfo)//是否是匹配客户资料
            {
                strBuilder.Append("匹配客户资料，");
            }
            if (IsMatchSupplierInfo)//是否是匹配供应商资料
            {
                strBuilder.Append("匹配供应商资料，");
            }
            if (IsMatchDepartmentInfo)//是否匹配部门人员
            {
                strBuilder.Append("匹配部门人员，");
            }
            if (mobiles != null)//如果手机号不为空
            {
                strBuilder.Append((string)mobiles);
            }
            return strBuilder.ToString().TrimEnd('，');
        }
    }
}
