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
    public partial class LoginLog : BackPage
    {
        /// <summary>
        /// 时间：2012-11-19  操作人：刘树超
        /// 登录日志
        /// </summary>

        protected int pageIndex, recordCount, pagesize = 10;

        protected void Page_Load(object sender, EventArgs e)
        {
            #region  权限判断
            if (!CheckGrant(global::EyouSoft.Model.EnumType.PrivsStructure.Privs3.系统设置_系统日志_登录日志栏目))
            {
                if (!CheckGrant(global::EyouSoft.Model.EnumType.PrivsStructure.Privs3.系统设置_系统日志_操作日志栏目))
                {
                    Utils.ResponseNoPermit(global::EyouSoft.Model.EnumType.PrivsStructure.Privs3.系统设置_系统日志_登录日志栏目, true);
                    return;
                }
                else
                {
                    Response.Redirect("OperationLog.aspx");
                }
            }
            else
            {
                InitPageDate();
                if (!CheckGrant(global::EyouSoft.Model.EnumType.PrivsStructure.Privs3.系统设置_系统日志_操作日志栏目))
                {
                    PlaceHolder2.Visible = false;
                }
            }
            #endregion


        }
        /// <summary>
        /// 初始化页面
        /// </summary>

        protected void InitPageDate()
        {
            EyouSoft.Model.CompanyStructure.QuerySysLoginLog serchModel = new EyouSoft.Model.CompanyStructure.QuerySysLoginLog();
            serchModel.CompanyId = CurrentUserCompanyID;
            serchModel.ContactName = Utils.GetQueryStringValue("txtname");
            serchModel.StartTime = Utils.GetDateTimeNullable(Utils.GetQueryStringValue("txtstime"));
            serchModel.EndTime = Utils.GetDateTimeNullable(Utils.GetQueryStringValue("txteTime"));
            pageIndex = Utils.GetInt(Utils.GetQueryStringValue("page")) == 0 ? 1 : Utils.GetInt(Utils.GetQueryStringValue("page"));
            IList<EyouSoft.Model.CompanyStructure.SysLoginLog> loglist = new EyouSoft.BLL.CompanyStructure.SysLoginLog().GetList(pagesize, pageIndex, ref recordCount, serchModel);
            if (serchModel.StartTime != null)
            {
                this.txtsTime.Value = Convert.ToDateTime(serchModel.StartTime).ToString("yyyy-MM-dd");
            }
            if (serchModel.EndTime != null)
            {
                this.txteTime.Value = Convert.ToDateTime(serchModel.EndTime).ToString("yyyy-MM-dd");
            }
            this.txtName.Value = serchModel.ContactName;
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
