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
    /// 常用短语
    /// xuty 2011/1/21
    /// </summary>
    public partial class CommonSms : EyouSoft.Common.Page.BackPage
    {
        protected int pageIndex;
        protected int recordCount;
        protected int pageSize = 20;
        protected int itemIndex = 1;
        protected void Page_Load(object sender, EventArgs e)
        {
            //判断权限
            if (!CheckGrant(EyouSoft.Model.EnumType.PrivsStructure.Privs3.短信中心_短信中心_栏目))
            {
                Utils.ResponseNoPermit(EyouSoft.Model.EnumType.PrivsStructure.Privs3.短信中心_短信中心_栏目, true);
                return;
            }
            string method = Utils.GetQueryStringValue("method");//当前操作
            pageIndex = Utils.GetInt(Utils.GetQueryStringValue("Page"), 1);//获取当前页码
            EyouSoft.BLL.SMSStructure.CommonWords commonBll = new EyouSoft.BLL.SMSStructure.CommonWords();
            //绑定短语类别列表
            IList<EyouSoft.Model.SMSStructure.CommonWordClass> classList = commonBll.GetCommonWordsClass(CurrentUserCompanyID);
            if (classList != null && classList.Count > 0)
            {
                selSmsType.DataTextField = "ClassName";
                selSmsType.DataValueField = "ID";
                selSmsType.DataSource = classList;
                selSmsType.DataBind();
            }
            selSmsType.Items.Insert(0, new ListItem("请选择", ""));
            //删除短语
            if (method == "del")
            {
                string cid = Utils.GetQueryStringValue("cid");
                bool result=commonBll.DeleteCommonWords(cid.Split(','));
                Utils.ResponseMeg(result, string.Empty);
                return;
            }
            //绑定短语列表
            string smsKeyword = Request.QueryString["smsKeyword"];//短语关键字
            smsKeyword = !string.IsNullOrEmpty(smsKeyword) ? Utils.InputText(Server.UrlDecode(smsKeyword)) : "";
            string cmstype = Utils.GetQueryStringValue("smstype");//短语类别
            IList<EyouSoft.Model.SMSStructure.CommonWords> list = commonBll.GetList(pageSize, pageIndex, ref recordCount, CurrentUserCompanyID, smsKeyword, Utils.GetInt(cmstype));
            if (list != null && list.Count > 0)
            {
                rptSms.DataSource = list;
                rptSms.DataBind();
                BindExportPage();
            }
            else
            {
                rptSms.EmptyText = "<tr><td colspan='4' align='center'>对不起，暂无短语信息！</td></tr>";
                ExportPageInfo1.Visible = false;
            }
            //恢复查询条件
            txtKeyWord.Value = smsKeyword;//短语关键字
            selSmsType.Value = cmstype;//短语类别
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

       
    }
}
