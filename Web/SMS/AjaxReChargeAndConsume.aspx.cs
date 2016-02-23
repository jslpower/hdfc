using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using EyouSoft.Common;
using EyouSoft.Common.Function;

namespace Web.SMS
{   
    /// <summary>
    /// Ajax获取充值记录消费明细
    /// xuty 2011/1/24
    /// </summary>
    public partial class AjaxReChargeAndConsume : EyouSoft.Common.Page.BackPage
    {
        protected int recordCountR;//充值记录数
        protected int pageIndexR = 1;//充值页码
        protected int pageSizeR = 10;//充值页大小
        protected int recordCountC;//消费记录数
        protected int pageIndexC = 1;//消费页码
        protected int pageSizeC = 10;//消费页大小
        protected bool recharge = true;//隐藏充值明细
        protected bool consume = true;//隐藏充值明细
        EyouSoft.BLL.SMSStructure.Account accountBll = new EyouSoft.BLL.SMSStructure.Account();
        protected void Page_Load(object sender, EventArgs e)
        {
            //判断权限
            if (!CheckGrant(EyouSoft.Model.EnumType.PrivsStructure.Privs3.短信中心_短信中心_栏目))
            {
                Utils.ResponseNoPermit(EyouSoft.Model.EnumType.PrivsStructure.Privs3.短信中心_短信中心_栏目, true);
                return;
            }
            string type = Utils.GetQueryStringValue("type");
            if (type == "recharge")//显示充值
            {
                BindRecharge();
                consume = false;
            }
            else if (type == "consume")//显示消费
            {
                BindConsume();
                recharge = false;
            }
            else
            {
                //显示所有
                BindRecharge();
                BindConsume();
            }
        }
        /// <summary>
        /// 绑定充值明细
        /// </summary>
        protected void BindRecharge()
        {
            pageIndexR = Utils.GetInt(Utils.GetQueryStringValue("Page"),1);
            IList<EyouSoft.Model.SMSStructure.PayMoneyInfo> list = accountBll.GetPayMoneys(pageSizeR, pageIndexR, ref recordCountR, CurrentUserCompanyID);
            if (list != null && list.Count > 0)
            {
                rptRechargeList.DataSource = list;
                rptRechargeList.DataBind();
                BindExportPageR();
            }
            else
            {
                rptRechargeList.EmptyText = "<tr><td colspan='5' align='center'>对不起，暂无充值信息！</td></tr>";
                ExporPageInfoSelect1.Visible = false;
            }


        }
        /// <summary>
        /// 绑定消费明细
        /// </summary>
        protected void BindConsume()
        {
            pageIndexC = Utils.GetInt(Utils.GetQueryStringValue("Page"), 1);
            IList<EyouSoft.Model.SMSStructure.SendTotal> list = accountBll.GetExpenseDetails(pageSizeC, pageIndexC, ref recordCountC, CurrentUserCompanyID);
            if (list != null && list.Count > 0)
            {
                rptConsumeList.DataSource = list;
                rptConsumeList.DataBind();
                BindExportPageC();
            }
            else
            {
                rptConsumeList.EmptyText = "<tr><td colspan='5' align='center'>对不起，暂无消费信息！</td></tr>";
                ExporPageInfoSelect2.Visible = false;
            }
        }

        #region 绑定充值明细分页控件
        /// <summary>
        /// 绑定充值明细分页控件
        /// </summary>
        protected void BindExportPageR()
        {
            ExporPageInfoSelect1.CurrencyPage = pageIndexR;
            ExporPageInfoSelect1.intPageSize = pageSizeR;
            ExporPageInfoSelect1.intRecordCount = recordCountR;
            //添加点击事件用ajax获取订单数据
            ExporPageInfoSelect1.AttributesEventAdd("onclick", "return AccountInfo.getList(this,\"recharge\");", 1);

        }
        #endregion

        #region 绑定消费明细分页控件
        /// <summary>
        /// 绑定消费明细分页控件
        /// </summary>
        protected void BindExportPageC()
        {
            ExporPageInfoSelect2.CurrencyPage = pageIndexC;
            ExporPageInfoSelect2.intPageSize = pageSizeC;
            ExporPageInfoSelect2.intRecordCount = recordCountC;
            //添加点击事件用ajax获取订单数据
            ExporPageInfoSelect2.AttributesEventAdd("onclick", "return AccountInfo.getList(this,\"consume\");", 1);

        }
        #endregion

      
    }
}
