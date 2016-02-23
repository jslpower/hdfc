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
    /// 创建人：刘飞
    /// 创建时间：2012-11-20
    /// 说明：处理国家，省份，城市，县区
    public class GetProvinceAndCity : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "text/plain";
            string getType = Utils.GetQueryStringValue("get");
            StringBuilder sb = new StringBuilder();
            int pID = Utils.GetInt(Utils.GetQueryStringValue("pid"));
            string companyID = Utils.GetQueryStringValue("companyID");
            int isCy = Utils.GetInt(Utils.GetQueryStringValue("isCy"), 0);//为1取常用城市，其他取所有
            switch (getType)
            {
                case "p":
                    var bllpro = new EyouSoft.BLL.CompanyStructure.Province();
                    IList<EyouSoft.Model.CompanyStructure.Province> pList = null;
                    pList = isCy == 1
                                ? bllpro.GetHasFavCityProvince(Utils.GetInt(companyID))
                                : bllpro.GetProvinceInfo(Utils.GetInt(companyID));

                    if (pList != null && pList.Count > 0)
                    {
                        sb.Append("{\"list\":[");
                        for (int i = 0; i < pList.Count; i++)
                        {
                            sb.Append("{\"id\":\"" + pList[i].Id.ToString() + "\",\"name\":\"" + pList[i].ProvinceName + "\"},");
                        }
                        if (sb.Length > 1)
                        {
                            sb.Remove(sb.Length - 1, 1);
                        }
                        sb.Append("]}");
                    }
                    else
                    {
                        sb.Append("{\"list\":[]}");
                    }

                    break;
                case "c":
                    EyouSoft.BLL.CompanyStructure.City bllcity = new EyouSoft.BLL.CompanyStructure.City();
                    bool? shiFouChangYong = null;
                    if (isCy == 1) shiFouChangYong = true;
                    IList<EyouSoft.Model.CompanyStructure.City> cList = bllcity.GetList(
                        Utils.GetInt(companyID), pID, shiFouChangYong);
                    if (cList != null && cList.Count > 0)
                    {
                        sb.Append("{\"list\":[");
                        for (int i = 0; i < cList.Count; i++)
                        {
                            sb.Append("{\"id\":\"" + cList[i].Id.ToString() + "\",\"name\":\"" + cList[i].CityName + "\"},");
                        }
                        if (sb.Length > 1)
                        {
                            sb.Remove(sb.Length - 1, 1);
                        }
                        sb.Append("]}");
                    }
                    else
                    {
                        sb.Append("{\"list\":[]}");
                    }
                    break;
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
