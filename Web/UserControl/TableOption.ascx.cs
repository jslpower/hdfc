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

namespace Web.UserControl
{
    /// <summary>
    /// 创建人：刘飞
    /// 时间：2013-1-9
    /// 描述：用于客户管理-生日列表的选项卡
    /// </summary>
    public partial class TableOption : System.Web.UI.UserControl
    {
        private string _type = "yuangong";
        /// <summary>
        /// 选项卡类型：
        /// 员工：yuangong
        /// 导游：daoyou
        /// 组团社：zutuanshe
        /// 游客：youke
        /// 地接社：dijie
        /// 景点：jingdian
        /// </summary>
        public string Type
        {
            get { return _type; }
            set { this._type = value; }
        }
        protected void Page_Load(object sender, EventArgs e)
        {

        }
    }
}