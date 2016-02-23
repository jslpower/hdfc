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
using EyouSoft.Common;

namespace Web.CommonPage
{
    public partial class UserSupper : EyouSoft.Common.Page.BackPage
    {
        /// <summary>
        /// 供应商类型
        /// </summary>
        protected EyouSoft.Model.EnumType.CompanyStructure.SupplierType type;
        protected string strsuppertype = string.Empty;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                //名称
                string Name = Utils.GetQueryStringValue("txtName");
                string isall = Utils.GetQueryStringValue("isall");
                string selectType = Utils.GetQueryStringValue("slttype");
                if (!string.IsNullOrEmpty(isall) && isall == "1")
                {
                    string suppertype = "";
                    if (!string.IsNullOrEmpty(selectType))
                    {
                        suppertype = Utils.GetQueryStringValue("slttype") == "-1" ? "2" : Utils.GetQueryStringValue("slttype");
                    }
                    else
                    {
                        suppertype = Utils.GetQueryStringValue("suppliertype");
                    }
                    type = (EyouSoft.Model.EnumType.CompanyStructure.SupplierType)Enum.Parse(typeof(EyouSoft.Model.EnumType.CompanyStructure.SupplierType), suppertype);
                }
                else
                {
                    type = (EyouSoft.Model.EnumType.CompanyStructure.SupplierType)Enum.Parse(typeof(EyouSoft.Model.EnumType.CompanyStructure.SupplierType), (Utils.GetQueryStringValue("suppliertype")));
                }
                strsuppertype = UtilsCommons.GetEnumDDL(EyouSoft.Common.EnumObj.GetList(typeof(EyouSoft.Model.EnumType.CompanyStructure.SupplierType)), ((int)type).ToString(), false);
            }
        }
    }
}
