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
using System.Text;

namespace Web.jidiaoCenter
{
    /// <summary>
    /// 确认件登记短息提醒组团社
    /// </summary>
    public partial class DuanxinEdit : EyouSoft.Common.Page.BackPage
    {
        protected int recordCount = 0;
        protected int pageIndex = 1;
        protected int pageSize = 10;
        protected void Page_Load(object sender, EventArgs e)
        {
            string action = Utils.GetQueryStringValue("action").ToLower();
            int smsId = Utils.GetInt(Utils.GetQueryStringValue("Id"));
            string bcId = Utils.GetQueryStringValue("bcid");
            string tourId = Utils.GetQueryStringValue("tourid");
            DateTime? ld = Utils.GetDateTimeNullable(Utils.GetQueryStringValue("ld"));
            string save = Utils.GetQueryStringValue("save");

            if (!string.IsNullOrEmpty(save))
            {
                this.RCWE(this.Save());
            }

            if (action == "del")
            {
                this.RCWE(this.DelDuanXin(smsId, tourId));
            }
            else if (action == "set")
            {
                string ise = Utils.GetQueryStringValue("ise");
                this.RCWE(this.SetIsEnabled(smsId, ise == "1"));
            }

            if (!IsPostBack)
            {
                this.InitSendChannel();
                this.InitList(tourId);
                this.InitPage(action, smsId, tourId, bcId, ld);
            }
        }

        /// <summary>
        /// 获取表单值
        /// </summary>
        /// <returns></returns>
        private EyouSoft.Model.TourStructure.MSMSTourCustomer GetFormValue()
        {
            var model = new EyouSoft.Model.TourStructure.MSMSTourCustomer
                {
                    ChannelId = Utils.GetInt(Utils.GetFormValue(selSendChannel.UniqueID)),
                    CompanyId = this.SiteUserInfo.CompanyId,
                    Content = Utils.GetFormValue(txtContent.UniqueID),
                    FixType = EyouSoft.Model.EnumType.CompanyStructure.CustomerCareForSendSpecialTime.无,
                    Id = 0,
                    IsEnabled = true,
                    IsMatchCustomerInfo = false,
                    IsMatchDepartmentInfo = false,
                    IsMatchSupplierInfo = false,
                    IsSeded = false,
                    IssueTime = DateTime.Now,
                    MobileCode = Utils.GetFormValue(txtMobile.UniqueID),
                    OperatorId = this.SiteUserInfo.UserId,
                    Time = Utils.GetDateTimeNullable(Utils.GetFormValue(txtsetTime.UniqueID)),
                    TourId = Utils.GetQueryStringValue("tourid"),
                    TourNo = string.Empty
                };

            return model;
        }

        private string Save()
        {
            string action = Utils.GetQueryStringValue("action").ToLower();
            int smsId = Utils.GetInt(Utils.GetQueryStringValue("Id"));
            string tourId = Utils.GetQueryStringValue("tourid");

            if (action == "edit")
            {
                if (smsId <= 0) return UtilsCommons.AjaxReturnJson("0", "url错误，请重新打开此窗口！");
            }
            else
            {
                if (string.IsNullOrEmpty(tourId)) return UtilsCommons.AjaxReturnJson("0", "url错误，请重新打开此窗口！");
            }

            var model = this.GetFormValue();

            var strErr = new StringBuilder();

            if (string.IsNullOrEmpty(model.MobileCode)) strErr.Append("请输入发送号码！<br />");
            if (string.IsNullOrEmpty(model.Content)) strErr.Append("请输入发送内容！<br />");
            if (!model.Time.HasValue) strErr.Append("请选择发送时间！<br />");

            if (!string.IsNullOrEmpty(strErr.ToString())) return UtilsCommons.AjaxReturnJson("0", strErr.ToString());

            var bll = new EyouSoft.BLL.TourStructure.BSMSTourCustomer();

            int r = 0;
            if (action == "edit")
            {
                model.Id = smsId;

                r = bll.Update(model);
            }
            else
            {
                r = bll.Add(model);
            }

            switch (r)
            {
                case 1:
                    return UtilsCommons.AjaxReturnJson("1", "保存成功！");
                default:
                    return UtilsCommons.AjaxReturnJson("0", "保存失败！");
            }
        }

        /// <summary>
        /// 设置短息状态是否启用
        /// </summary>
        /// <param name="smsId">短息编号</param>
        /// <param name="isEnabled">是否启用</param>
        /// <returns></returns>
        private string SetIsEnabled(int smsId, bool isEnabled)
        {
            if (smsId <= 0) return UtilsCommons.AjaxReturnJson("0", "url错误，请重新打开此窗口！");

            var bll = new EyouSoft.BLL.TourStructure.BSMSTourCustomer();
            bool r = isEnabled ? bll.StopIt(smsId) : bll.StartIt(smsId);

            return UtilsCommons.AjaxReturnJson(r ? "1" : "0", string.Format("操作{0}！", r ? "成功" : "失败"));
        }

        /// <summary>
        /// 删除短息
        /// </summary>
        /// <param name="smsId">短息编号</param>
        /// <param name="tourId">团队编号</param>
        /// <returns></returns>
        private string DelDuanXin(int smsId, string tourId)
        {
            if (smsId <= 0 || string.IsNullOrEmpty(tourId)) return UtilsCommons.AjaxReturnJson("0", "url错误，请重新打开此窗口！");

            int r = new EyouSoft.BLL.TourStructure.BSMSTourCustomer().Delete(tourId, smsId);

            switch (r)
            {
                case 1:
                    return UtilsCommons.AjaxReturnJson("1", "删除成功！");
                default:
                    return UtilsCommons.AjaxReturnJson("0", "删除失败！");
            }
        }

        /// <summary>
        /// 初始化
        /// </summary>
        /// <param name="bcid">客户单位编号</param>
        /// <param name="action">操作类型(新增，修改，删除，停用，启用)</param>
        /// <param name="tourId">团队编号</param>
        /// <param name="ldate">出团时间</param>
        /// <param name="smsId">短信提醒编号</param>
        private void InitPage(string action, int smsId, string tourId, string bcid, DateTime? ldate)
        {
            if (action == "edit")
            {
                this.InitEditPage(smsId);
            }
            else if (action == "add")
            {
                this.IntiAddPage(tourId, bcid, ldate);
            }
        }

        /// <summary>
        /// 初始化列表
        /// </summary>
        /// <param name="tourId">团队编号</param>
        private void InitList(string tourId)
        {
            if (string.IsNullOrEmpty(tourId)) return;

            var list = new EyouSoft.BLL.TourStructure.BSMSTourCustomer().GetList(
                this.SiteUserInfo.CompanyId,
                pageSize,
                pageIndex,
                ref recordCount,
                new EyouSoft.Model.TourStructure.MSearchSMSTourCustomer { TourId = tourId });

            rptSmsList.DataSource = list;
            rptSmsList.DataBind();
        }

        /// <summary>
        /// 新增初始化
        /// </summary>
        /// <param name="tourId">团队编号</param>
        /// <param name="bcid">客户单位编号</param>
        /// <param name="ldate">出团时间</param>
        private void IntiAddPage(string tourId, string bcid, DateTime? ldate)
        {
            if (string.IsNullOrEmpty(tourId) || string.IsNullOrEmpty(bcid)) return;

            var model = new EyouSoft.BLL.CRM.BCustomer().GetCustomer(bcid);

            if (model == null) return;

            var strMobiel = new StringBuilder();
            strMobiel.AppendFormat("{0},", model.Mobile);

            if (model.Contact != null)
            {
                foreach (var t in model.Contact)
                {
                    if (t == null || string.IsNullOrEmpty(t.Mobile)
                        || !EyouSoft.Common.Function.StringValidate.IsMobileOrPHS(t.Mobile)) continue;

                    strMobiel.AppendFormat("{0},", t.Mobile);
                }
            }

            txtMobile.Text = strMobiel.ToString().TrimEnd(',');
            txtContent.Text = string.Empty;
            txtsetTime.Text = ldate.HasValue ? ldate.Value.ToString("yyyy-MM-dd") + " 09:00" : string.Empty;
        }

        /// <summary>
        /// 修改初始化
        /// </summary>
        /// <param name="smsId"></param>
        private void InitEditPage(int smsId)
        {
            if (smsId <= 0) return;

            var model = new EyouSoft.BLL.TourStructure.BSMSTourCustomer().GetModel(smsId);

            if (model == null) return;

            txtContent.Text = model.Content;
            txtMobile.Text = model.MobileCode;
            txtsetTime.Text = model.Time.HasValue ? model.Time.Value.ToString("yyyy-MM-dd HH:mm") : string.Empty;

            if (selSendChannel.Items.FindByValue(model.ChannelId.ToString()) != null)
            {
                selSendChannel.Items.FindByValue(model.ChannelId.ToString()).Selected = true;
            }
        }

        /// <summary>
        /// 初始化发送通道
        /// </summary>
        private void InitSendChannel()
        {
            var channel = new EyouSoft.Model.SMSStructure.SMSChannelList();
            IList<EyouSoft.Model.SMSStructure.SMSChannel> channels = new List<EyouSoft.Model.SMSStructure.SMSChannel>();
            for (int i = 0; i < channel.Count; i++)
            {
                channels.Add(channel[i]);

                this.selSendChannel.Items.Add(new ListItem(channel[i].ChannelName, channel[i].Index.ToString()));
            }
        }
    }
}
