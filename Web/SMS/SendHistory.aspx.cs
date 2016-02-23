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
    /// 短信发送历史记录
    /// xuty 2011/1/21
    /// </summary>
    public partial class SendHistory : EyouSoft.Common.Page.BackPage
    {
        protected int itemIndex = 1;
        protected int pageIndex;
        protected int recordCount;
        protected int pageSize = 20;
        protected void Page_Load(object sender, EventArgs e)
        {
            //判断权限
            if (!CheckGrant(EyouSoft.Model.EnumType.PrivsStructure.Privs3.短信中心_短信中心_栏目))
            {
                Utils.ResponseNoPermit(EyouSoft.Model.EnumType.PrivsStructure.Privs3.短信中心_短信中心_栏目, true);
                return;
            }
            pageIndex = Utils.GetInt(Utils.GetQueryStringValue("Page"), 1);//获取当前页码
            itemIndex = (pageIndex - 1) * pageSize + 1;
            //查询关键字
            string smsKeyword =Request.QueryString["smsKeyword"];//关键字
            smsKeyword = !string.IsNullOrEmpty(smsKeyword) ? Utils.InputText(Server.UrlDecode(smsKeyword)) : "";
            string sendStartDate = Utils.GetQueryStringValue("startdate");//发送起始时间
            string sendEndDate = Utils.GetQueryStringValue("enddate");//发送结束时间
            string sendState = Utils.GetQueryStringValue("sendstate");//发送状态
            EyouSoft.BLL.SMSStructure.SendMessage messBll = new EyouSoft.BLL.SMSStructure.SendMessage();

            IList<EyouSoft.Model.SMSStructure.SendDetail> list=null;
           //导出Excel
            if (Utils.GetQueryStringValue("method") == "downexcel")
            {  
                string strPageSize=Utils.GetQueryStringValue("recordcount");
                pageSize = Utils.GetInt(strPageSize == "0" ? "1" : strPageSize);
                list= messBll.GetSendHistorys(pageSize, pageIndex, ref recordCount, CurrentUserCompanyID.ToString(), smsKeyword, Utils.GetInt(sendState), Utils.GetDateTimeNullable(sendStartDate), Utils.GetDateTimeNullable(sendEndDate));
                DownLoadExcel(list);
                return;
            }
            //绑定发送历史
            list = messBll.GetSendHistorys(pageSize, pageIndex, ref recordCount, CurrentUserCompanyID.ToString(), smsKeyword, Utils.GetInt(sendState), Utils.GetDateTimeNullable(sendStartDate), Utils.GetDateTimeNullable(sendEndDate));
            if (list != null && list.Count > 0)
            {
                rptSendHistory.DataSource = list;
                rptSendHistory.DataBind();
                BindExportPage();
            }
            else
            {
                rptSendHistory.EmptyText = "<tr><td colspan='4' align='center'>对不起，暂无历史记录信息！</td></tr>";
                ExportPageInfo1.Visible = false;
            }
            //恢复查询条件
            txtKeyWord.Value = smsKeyword;//关键字
            txtSendEndDate.Value = sendEndDate;//发送结束时间
            txtSendStartDate.Value = sendStartDate;//发送起始时间
            selSendState.Value = sendState;//发送状态
        }
        #region 绑定分页控件
        /// <summary>
        /// 绑定分页控件
        /// </summary>
        protected void BindExportPage()
        {
            this.ExportPageInfo1.intPageSize = pageSize;
            this.ExportPageInfo1.CurrencyPage = pageIndex;
            this.ExportPageInfo1.intRecordCount = recordCount;
        }
        #endregion

       
        /// <summary>
        /// 导出客户列表
        /// </summary>
        /// <param name="list"></param>
        protected void DownLoadExcel(IList<EyouSoft.Model.SMSStructure.SendDetail> list)
        {
            StringBuilder strBuilder = new StringBuilder();
            strBuilder.Append("发送时间\t号码\t发送内容\t价格\t状态\n");
            foreach (EyouSoft.Model.SMSStructure.SendDetail s in list)
            {
                strBuilder.AppendFormat("{0}\t{1}\t{2}\t{3}\t{4}\n", s.SendTime.ToString("yyyy-MM-dd HH:mm"),s.MobileNumber, s.SMSContent, s.UseMoeny.ToString(),s.ReturnResult==0?"发送成功":"发送失败");
            }
            Response.Clear();
            Response.ContentEncoding = System.Text.Encoding.Default;
            Response.AppendHeader("Content-Disposition", "attachment;filename=" + DateTime.Now.ToString("yyyyMMddHHmmssss") + ".xls");
            Response.ContentType = "application/ms-excel";
            Response.Write(strBuilder.ToString());
            Response.End();
        }
    }
}
