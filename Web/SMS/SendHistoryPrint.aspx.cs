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
    /// 短信发送历史打印
    /// </summary>
    public partial class SendHistoryPrint : EyouSoft.Common.Page.BackPage
    {
        protected int itemIndex = 1;
        protected int pageIndex;
        protected int recordCount;
        protected int pageSize = 20;
        protected void Page_Load(object sender, EventArgs e)
        {
            pageIndex = Utils.GetInt(Utils.GetQueryStringValue("Page"), 1);//获取当前页码
            string smsKeyword = Utils.GetQueryStringValue("smsKeyword");//关键字
            string sendStartDate = Utils.GetQueryStringValue("startdate");//发送岂是时间
            string sendEndDate = Utils.GetQueryStringValue("enddate");//发送结束时间
            string sendState = Utils.GetQueryStringValue("sendstate");//发送状态
            EyouSoft.BLL.SMSStructure.SendMessage messBll = new EyouSoft.BLL.SMSStructure.SendMessage();
            recordCount =Utils.GetInt(Utils.GetQueryStringValue("recordcount"));
            if (recordCount != 0)
            {
                pageSize = recordCount;
            }
            //绑定历史记录
            IList<EyouSoft.Model.SMSStructure.SendDetail> list = messBll.GetSendHistorys(pageSize, pageIndex, ref recordCount, CurrentUserCompanyID.ToString(), smsKeyword, Utils.GetInt(sendState), Utils.GetDateTimeNullable(sendStartDate), Utils.GetDateTimeNullable(sendEndDate));
            if (list != null && list.Count > 0)
            {
                rptSendHistory.DataSource = list;
                rptSendHistory.DataBind();
               
            }
            else
            {
                rptSendHistory.EmptyText = "<tr><td colspan='4' align='center'>对不起，暂无历史记录信息！</td></tr>";
            }
        }
      

    }
}
