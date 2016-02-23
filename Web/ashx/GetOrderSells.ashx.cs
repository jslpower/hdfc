using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using EyouSoft.Common;
using System.Text;

namespace Web.Ashx
{
    /// <summary>
    /// $codebehindclassname$ 的摘要说明
    /// </summary>


    /// <summary>
    /// 页面：DOM
    /// </summary>
    /// 创建人：戴银柱
    /// 创建时间：2011-9-20
    /// 说明：处理销售员，计调员，员工等输入匹配
    public class GetOrderSells : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            var q = Utils.GetQueryStringValue("q");
            var companyID = Utils.GetInt(Utils.GetQueryStringValue("companyID"));
            context.Response.ContentType = "text/plain";
            StringBuilder sb = new StringBuilder();
            if (q != "" && companyID >0)
            {
                var searchModel = new EyouSoft.Model.CompanyStructure.QueryCompanyUser();
                //searchModel.UserName = q;
                searchModel.ContactName = q;
                int recordCount = 0;
                IList<EyouSoft.Model.CompanyStructure.CompanyUser> userList = new EyouSoft.BLL.CompanyStructure.CompanyUser().GetList(companyID, 1, 10, ref recordCount, searchModel);
                if (userList != null && userList.Count > 0)
                {
                    for (int i = 0; i < userList.Count; i++)
                    {
                        sb.Append(userList[i].UserName + "|" + userList[i].ID + "|" + userList[i].DepartId + "|" + userList[i].DepartName + "\n");
                    }
                }
                else
                {
                    sb.Append("未找到该记录| | |");
                }
            }
            context.Response.Write(sb.ToString());
        }

        public bool IsReusable
        {
            get
            {
                return false;
            }
        }
    }
}
