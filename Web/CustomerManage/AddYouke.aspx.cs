using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.Xml.Linq;
using EyouSoft.Common;
using System.Collections.Generic;
using EyouSoft.Model.CRM;

namespace Web.CustomerManage
{
    public partial class AddYouke : EyouSoft.Common.Page.BackPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            string travelid = Utils.GetQueryStringValue("travelid");
            string dotype = Utils.GetQueryStringValue("dotype");
            string giftid = Utils.GetQueryStringValue("giftid");
            PowerControl();
            switch (dotype)
            {
                case "save":
                    Response.Clear();
                    Response.Write(PageSave(travelid, giftid));
                    Response.End();
                    break;
                case "delete":
                    Response.Clear();
                    Response.Write(DeleteDate(giftid));
                    Response.End();
                    break;
                default:
                    break;
            }
            if (!IsPostBack)
            {
                PageInit(travelid, giftid, dotype);
            }
        }
        /// <summary>
        /// 初始化
        /// </summary>
        /// <param name="userid"></param>
        /// <param name="dotype"></param>
        private void PageInit(string travelid, string giftid, string dotype)
        {
            BindList(travelid);
            if (travelid != "" && dotype == "show")
            {
                EyouSoft.BLL.CRM.BTravellerBirthday bll = new EyouSoft.BLL.CRM.BTravellerBirthday();
                EyouSoft.Model.CRM.MTravellerBirthdayGift model = new EyouSoft.Model.CRM.MTravellerBirthdayGift();
                model = bll.GetBirthdayGift(giftid);
                if (model != null)
                {
                    this.txtdate.Text = model.SendGiftTime.ToString("yyyy-MM-dd");
                    this.txtremark.Text = model.Remark;
                }
            }
        }
        /// <summary>
        /// 绑定用户礼物列表
        /// </summary>
        /// <param name="userid"></param>
        private void BindList(string travelid)
        {
            EyouSoft.BLL.CRM.BTravellerBirthday bll = new EyouSoft.BLL.CRM.BTravellerBirthday();
            IList<MTravellerBirthdayGift> list = bll.GetBirthdayGiftList(travelid);
            if (list != null && list.Count > 0)
            {
                this.rptlist.DataSource = list;
                this.rptlist.DataBind();
            }
            else
            {
                this.PhList.Visible = false;
            }
        }
        /// <summary>
        /// 保存
        /// </summary>
        /// <param name="userid"></param>
        /// <param name="giftid"></param>
        /// <returns></returns>
        private string PageSave(string travelid, string giftid)
        {
            string msg = string.Empty;
            //true 新增，false 修改
            bool t = !string.IsNullOrEmpty(giftid) ? false : true;
            EyouSoft.BLL.CRM.BTravellerBirthday bll = new EyouSoft.BLL.CRM.BTravellerBirthday();
            EyouSoft.Model.CRM.MTravellerBirthdayGift model = new EyouSoft.Model.CRM.MTravellerBirthdayGift();
            model.CompanyId = SiteUserInfo.CompanyId;
            model.IssueTime = DateTime.Now;
            model.OperatorId = SiteUserInfo.UserId;
            model.Remark = Utils.GetFormValue(this.txtremark.UniqueID);
            model.SendGiftTime = Utils.GetDateTime(Utils.GetFormValue(txtdate.UniqueID));
            model.TravellerId = travelid;
            int result = 0;
            if (t)
            {
                result = bll.AddBirthdayGift(model);
            }
            else
            {
                model.Id = giftid;
                result = bll.UpdateBirthdayGift(model);
            }
            switch (result)
            {
                case 0:
                    msg = UtilsCommons.AjaxReturnJson("0", "参数错误");
                    break;
                case 1:
                    msg = UtilsCommons.AjaxReturnJson("1", (t ? "新增" : "修改") + "成功");
                    break;
                case -1:
                    msg = UtilsCommons.AjaxReturnJson("0", (t ? "新增" : "修改") + "失败");
                    break;
                default:
                    break;
            }
            return msg;
        }

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="giftid"></param>
        /// <returns></returns>
        private string DeleteDate(string giftid)
        {
            string msg = string.Empty;
            if (!string.IsNullOrEmpty(giftid))
            {
                EyouSoft.BLL.CRM.BTravellerBirthday bll = new EyouSoft.BLL.CRM.BTravellerBirthday();
                int result = bll.DeleteBirthdayGift(giftid);
                switch (result)
                {
                    case 0:
                        msg = UtilsCommons.AjaxReturnJson("0", "参数错误");
                        break;
                    case 1:
                        msg = UtilsCommons.AjaxReturnJson("1", "删除成功!");
                        break;
                    case -1:
                        msg = UtilsCommons.AjaxReturnJson("0", "删除失败!");
                        break;
                    default:
                        break;
                }
            }
            return msg;
        }


        /// <summary>
        /// 权限判断
        /// </summary>
        protected void PowerControl()
        {
            if (!this.CheckGrant(EyouSoft.Model.EnumType.PrivsStructure.Privs3.客户管理_生日中心_栏目))
            {
                this.btn.Visible = false;
            }
            if (!this.CheckGrant(EyouSoft.Model.EnumType.PrivsStructure.Privs3.客户管理_生日中心_栏目))
            {
                this.btn.Visible = false;
            }

        }
    }
}
