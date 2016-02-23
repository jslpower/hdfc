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
    /// 账户信息
    /// xuty 2011/1/24
    /// </summary>
    public partial class AccountInfo : EyouSoft.Common.Page.BackPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {   
            
            //判断权限
            if (!CheckGrant(EyouSoft.Model.EnumType.PrivsStructure.Privs3.短信中心_短信中心_栏目))
            {
                Utils.ResponseNoPermit(EyouSoft.Model.EnumType.PrivsStructure.Privs3.短信中心_短信中心_栏目, true);
                return;
            }
            //获取账户余额
            EyouSoft.BLL.SMSStructure.Account accountBll=new EyouSoft.BLL.SMSStructure.Account();
            litRemainMoney.Text = accountBll.GetAccountMoney(CurrentUserCompanyID).ToString("F2");
        }
    }
}
