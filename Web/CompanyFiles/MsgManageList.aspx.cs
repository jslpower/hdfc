using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using EyouSoft.Common;
using EyouSoft.Common.Page;
using EyouSoft.BLL.CompanyStructure;
using EyouSoft.Common.Function;

namespace Web.CompanyFiles
{
    /// <summary>
    /// 2012-11-20 刘树超
    /// 信息管理列表
    /// </summary>
    public partial class MsgManage : BackPage
    {
        protected int pageIndex;
        protected int recordCount;
        protected int pageSize = 20;
        protected int itemIndex;

        protected bool IsEdit = false;

        protected bool IsDel = false;

        protected void Page_Load(object sender, EventArgs e)
        {
            #region 权限判断
            //专线用户不验证公告栏目权限
            if ((this.SiteUserInfo.UserType == EyouSoft.Model.EnumType.CompanyStructure.UserType.地接用户 
                || this.SiteUserInfo.UserType == EyouSoft.Model.EnumType.CompanyStructure.UserType.票务用户) 
                && !CheckGrant(EyouSoft.Model.EnumType.PrivsStructure.Privs3.公司文件_公告通知_栏目))
            {
                Utils.ResponseNoPermit(EyouSoft.Model.EnumType.PrivsStructure.Privs3.公司文件_公告通知_栏目, false);
                return;
            }
            if (!CheckGrant(EyouSoft.Model.EnumType.PrivsStructure.Privs3.公司文件_公告通知_新增))
            {
                plnAddMsg.Visible = false;
            }
            if (CheckGrant(EyouSoft.Model.EnumType.PrivsStructure.Privs3.公司文件_公告通知_修改))
            {
                IsEdit = true;
            }
            if (CheckGrant(EyouSoft.Model.EnumType.PrivsStructure.Privs3.公司文件_公告通知_删除))
            {
                IsDel = true;
            }

            #endregion

            pageIndex = Utils.GetInt(Utils.GetQueryStringValue("Page"), 1);

            string method = Utils.GetQueryStringValue("method");
            string ids = Utils.GetQueryStringValue("ids");//获取员工Id
            EyouSoft.BLL.CompanyStructure.News newsBll = new EyouSoft.BLL.CompanyStructure.News();//初始化newsBll
            //删除操作
            if (method == "del")
            {
                if (IsDel)
                {
                    ids = ids.TrimEnd(',');
                    int result = newsBll.Delete(ids.Split(',').Select(i => Utils.GetInt(i)).ToArray());
                    if (result == 1)
                    {
                        MessageBox.ShowAndRedirect(this, "删除成功！", "/CompanyFiles/MsgManageList.aspx");
                    }
                    return;
                }

                MessageBox.ShowAndRedirect(this, "您没有删除权限！", "/CompanyFiles/MsgManageList.aspx");
            }
            //绑定信息列表
            IList<EyouSoft.Model.CompanyStructure.NoticeNews> list = newsBll.GetAcceptNews(pageSize, pageIndex, ref recordCount, SiteUserInfo.UserId, CurrentUserCompanyID);
            if (list != null && list.Count > 0)
            {
                UtilsCommons.Paging(pageSize, ref pageIndex, recordCount);
                itemIndex = (pageIndex - 1) * pageSize + 1;
                rptInfo.DataSource = list;
                rptInfo.DataBind();
                BindExportPage();
            }
            else
            {
                rptInfo.EmptyText = "<tr><td colspan='7' align='center'>对不起，暂无信息！</td></tr>";
                ExportPageInfo1.Visible = false;
            }
        }

        #region 绑定分页控件
        /// <summary>
        /// 绑定分页控件
        /// </summary>
        protected void BindExportPage()
        {
            this.ExportPageInfo1.PageLinkURL = Request.ServerVariables["SCRIPT_NAME"].ToString() + "?";
            this.ExportPageInfo1.intPageSize = pageSize;
            this.ExportPageInfo1.CurrencyPage = pageIndex;
            this.ExportPageInfo1.intRecordCount = recordCount;
        }
        #endregion

        /// <summary>
        /// 获取操作列信息
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        protected string GetHandleHtml(object obj)
        {
            if (obj == null) return string.Empty;

            var strHtml = new System.Text.StringBuilder();
            if (IsEdit)
            {
                strHtml.AppendFormat(" <a href=\"/CompanyFiles/MsgAdd.aspx?infoId={0}\">修改 </a> ", obj.ToString());
            }
            if (!string.IsNullOrEmpty(strHtml.ToString()) && IsDel)
            {
                strHtml.Append(" | ");
            }
            if (IsDel)
            {
                strHtml.AppendFormat(" <a href=\"javascript:void(0);\" onclick=\"return InfoList.del('{0}')\">删除</a> ", obj.ToString());
            }

            return strHtml.ToString();
        }

    }
}
