using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;
using EyouSoft.Model.EnumType.CompanyStructure;
using EyouSoft.Common;

namespace Web.jidiaoCenter
{
    public partial class AjaxSelerRequest : EyouSoft.Common.Page.BackPage
    {
        #region 分页参数
        /// <summary>
        /// 每页显示条数(常量)
        /// </summary>
        /// 当变量需要在前台使用时可换成protected修饰
        private int pageSize = 20;
        /// <summary>
        /// 当前页数
        /// </summary>
        ///  当变量需要在前台使用时可换成protected修饰
        private int pageIndex = 1;
        /// <summary>
        /// 总记录条数
        /// </summary>
        ///  当变量需要在前台使用时可换成protected修饰
        protected int recordCount = 0;

        protected int listCount = 0;
        #endregion
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                string userName = Utils.GetQueryStringValue("userName");
                if (!string.IsNullOrEmpty(userName))
                {
                    DataInit(userName);
                }
            }
        }
        #region 私有方法
        /// <summary>
        /// 初始化
        /// </summary>
        private void DataInit(string userName)
        {
            var searchModel = new EyouSoft.Model.CompanyStructure.QueryCompanyUser();
            var userStatus = new UserStatus[1];
            userStatus[0] = UserStatus.正常;
            searchModel.ContactName = userName;
            searchModel.UserStatus = userStatus;
            searchModel.UserType = EyouSoft.Model.EnumType.CompanyStructure.UserType.专线用户;
            var userList = new EyouSoft.BLL.CompanyStructure.CompanyUser().GetList(SiteUserInfo.CompanyId, pageSize, pageIndex, ref recordCount, searchModel);

            if (userList != null && userList.Count > 0)
            {
                listCount = userList.Count;
                this.rptList.DataSource = userList;
                this.rptList.DataBind();
            }
            else
            {
                this.lblMsg.Text = "没有相关数据!";
            }
        }
        #endregion
    }
}
