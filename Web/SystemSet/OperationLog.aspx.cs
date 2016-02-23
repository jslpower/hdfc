using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using EyouSoft.Common;
using EyouSoft.Common.Page;
using EyouSoft.BLL.CompanyStructure;

namespace Web.SystemSet
{

    public partial class OperationLog : BackPage
    {

        /// <summary>
        /// 时间：2012-11-19  操作人：刘树超
        /// 操作日志
        /// </summary>
        protected int pageIndex, recordCount, pagesize = 10;

        protected void Page_Load(object sender, EventArgs e)
        {
            #region  权限判断
            if (CheckGrant(global::EyouSoft.Model.EnumType.PrivsStructure.Privs3.系统设置_系统日志_操作日志栏目))
            {
                if (!CheckGrant(global::EyouSoft.Model.EnumType.PrivsStructure.Privs3.系统设置_系统日志_登录日志栏目))
                {
                    PlaceHolder1.Visible = false;
                }
                InitPageDate();
            }
            else
            {
                Utils.ResponseNoPermit(global::EyouSoft.Model.EnumType.PrivsStructure.Privs3.系统设置_系统日志_操作日志栏目, true);
                return;
            }
            #endregion

        }
        protected void InitPageDate()
        {
            EyouSoft.Model.CompanyStructure.QueryHandleLog serchModel = new EyouSoft.Model.CompanyStructure.QueryHandleLog();
            serchModel.CompanyId = CurrentUserCompanyID;

            serchModel.OperatorName = Utils.GetQueryStringValue("txtname");
            serchModel.HandStartTime = Utils.GetDateTimeNullable(Utils.GetQueryStringValue("txtsTime"));
            DateTime? end = Utils.GetDateTimeNullable(Utils.GetQueryStringValue("txteTime"));
            DateTime? endbox = end;
            if (end != null)
            {
                end = Convert.ToDateTime(end).AddDays(1);
                serchModel.HandEndTime = end;
            }
            pageIndex = Utils.GetInt(Utils.GetQueryStringValue("page")) == 0 ? 1 : Utils.GetInt(Utils.GetQueryStringValue("page"));
            if (serchModel.HandStartTime != null)
            {
                this.txtsTime.Value = Convert.ToDateTime(serchModel.HandStartTime).ToString("yyyy-MM-dd");
            }
            if (endbox != null)
            {
                this.txteTime.Value = Convert.ToDateTime(endbox).ToString("yyyy-MM-dd");
            }
            this.txtName.Value = serchModel.OperatorName;
            IList<EyouSoft.Model.CompanyStructure.SysHandleLogs> loglist = new EyouSoft.BLL.CompanyStructure.SysHandleLogs().GetList(pagesize, pageIndex, ref recordCount, serchModel);

            if (loglist != null && loglist.Count > 0)
            {
                UtilsCommons.Paging(pagesize, ref pageIndex, recordCount);
                rpt_logList.DataSource = loglist;
                rpt_logList.DataBind();
                pageBind();
            }
        }

        #region 绑定分页
        /// <summary>
        /// 绑定分页
        /// </summary>
        protected void pageBind()
        {
            this.ExporPageInfoSelect1.intPageSize = pagesize;
            this.ExporPageInfoSelect1.CurrencyPage = pageIndex;
            this.ExporPageInfoSelect1.intRecordCount = recordCount;
        }
        #endregion

    }
}
