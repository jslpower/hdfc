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
    /// 客户关怀
    /// xuty 2011/1/19
    /// </summary>
    public partial class CustomerCare : EyouSoft.Common.Page.BackPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {   
            //判断权限
            if (!CheckGrant(EyouSoft.Model.EnumType.PrivsStructure.Privs3.短信中心_短信中心_栏目))
            {
                Utils.ResponseNoPermit(EyouSoft.Model.EnumType.PrivsStructure.Privs3.短信中心_短信中心_栏目, true);
                return;
            }
            string method = Utils.GetFormValue("hidMethod");
            int sid = Utils.GetInt(Utils.GetQueryStringValue("sid"));//获取设置编号
            var careBll = new EyouSoft.BLL.CompanyStructure.CustomerCareFor();
            //保存
            if (method != "")
            {
                #region 保存客户关怀
                bool result = false;
                if (method == "save")
                {
                    EyouSoft.Model.CompanyStructure.CustomerCareforInfo careModel = new EyouSoft.Model.CompanyStructure.CustomerCareforInfo();
                    careModel.ChannelId = Utils.GetInt(Utils.GetFormValue(selSendChannel.UniqueID));
                    careModel.CompanyId = CurrentUserCompanyID;
                    careModel.Content = Utils.InputText(txtSmsContent.Value);

                    string resultMess = "";
                    EyouSoft.BLL.SMSStructure.SendMessage sendBll = new EyouSoft.BLL.SMSStructure.SendMessage();

                    string forbidWords = sendBll.IsIncludeKeyWord(careModel.Content);//获取禁止发送的词
                    if (!String.IsNullOrEmpty(forbidWords))
                    {
                        resultMess += "短信包含禁止发送的关键词：" + forbidWords;
                    }
                    if (resultMess != "")
                    {
                        InitSendChannel();
                        selSendChannel.Value = careModel.ChannelId.ToString();
                        MessageBox.Show(this, resultMess);
                        return;
                    }
                    List<string> list = new List<string>();
                    foreach (ListItem item in chkSendRange.Items)
                    {
                        if (item.Selected) list.Add(item.Value);
                    }
                    careModel.IsEnabled = true;
                    careModel.IsMatchCustomerInfo = list.Contains("1");
                    careModel.IsMatchSupplierInfo = list.Contains("2");
                    careModel.IsMatchDepartmentInfo = list.Contains("3");
                    careModel.IssueTime = DateTime.Now;
                    careModel.MobileCode = Utils.InputText(txtMobile.Value);
                    careModel.OperatorId = SiteUserInfo.UserId;
                    if (rdiFixTime.Checked)//如果是固定时间发送
                    {
                        careModel.Time = Utils.GetDateTime(Utils.InputText(txtSendTime.Value),DateTime.Now);
                        careModel.FixType = (EyouSoft.Model.EnumType.CompanyStructure.CustomerCareForSendSpecialTime)0;
                    }
                    else
                    {
                        //满足条件发送
                        careModel.Time = Utils.GetDateTimeNullable(Utils.InputText(txtSendTime.Value));
                        careModel.FixType = (EyouSoft.Model.EnumType.CompanyStructure.CustomerCareForSendSpecialTime)Utils.GetInt(selCondit.Value);
                    }
                    if (sid != 0)
                    {
                        //修改
                        careModel.Id = sid;
                        result=careBll.Update(careModel);
                    }
                    else
                    {
                        //添加
                        result=careBll.Add(careModel);
                    }
                    MessageBox.ShowAndRedirect(this, result ? "短信设置已保存！" : "短信设置失败！", "/SMS/CustomerCare.aspx");
                    
                    return;
                }
                else
                {
                    sid = Utils.GetInt(Utils.GetFormValue("sid"));
                    if (method == "stop")
                    {
                        //停发
                        result=careBll.StopIt(sid);
                        Utils.ResponseMeg(result, result ? "停发成功！" : "停发失败！");
                    }
                    else if (method == "start")
                    {
                        result = careBll.StartIt(sid);
                        Utils.ResponseMeg(result, result ? "开启成功！" : "开启失败！");
                    }
                    if (method == "del")
                    {
                        result=careBll.DeletIt(sid);
                        Utils.ResponseMeg(result, result ? "删除成功！" : "删除失败！");
                    }
                    return;
                }
                #endregion
            }
            else
            {
                #region 初始化客户关怀
                InitSendChannel();
                if (sid != 0)
                {
                    EyouSoft.Model.CompanyStructure.CustomerCareforInfo careModel = careBll.GetModel(sid);
                    //显示初始信息
                    if (careModel != null)
                    {
                        //chkSendRange.Items[0].Selected = careModel.IsMatchCustomerInfo;//发送条件
                        chkSendRange.Items[0].Selected = careModel.IsMatchSupplierInfo;
                        //chkSendRange.Items[2].Selected = careModel.IsMatchDepartmentInfo;
                        if (careModel.IsMatchCustomerInfo || careModel.IsMatchSupplierInfo || careModel.IsMatchDepartmentInfo)//如果选中了发送范围则输入号码不能编辑
                        {
                            txtMobile.Disabled = true;
                           
                        }
                        else
                        {
                            txtMobile.Value = careModel.MobileCode;
                        }
                        chkName.Checked = careModel.Content.IndexOf("[姓名]") != -1;//匹配姓名
                        selSendChannel.Value = careModel.ChannelId.ToString();//发送通道
                        rdiCondit.Checked = (int)careModel.FixType != 0;//满足发送条件
                        txtSendTime.Value =(int)careModel.FixType == 0&&careModel.Time.HasValue?careModel.Time.Value.ToString("yyyy-MM-dd HH:mm"):"";//发送时间
                        rdiFixTime.Checked = (int)careModel.FixType == 0;//固定时间
                        selCondit.Value = (int)careModel.FixType!= 0?((int)careModel.FixType).ToString():"";
                        txtSmsContent.Value = careModel.Content;//发送内容
                    }
                }
                #endregion
            }
        }

        /// <summary>
        /// 初始化发送通道
        /// </summary>
        private void InitSendChannel()
        {
            EyouSoft.Model.SMSStructure.SMSChannelList channel = new EyouSoft.Model.SMSStructure.SMSChannelList();
            IList<EyouSoft.Model.SMSStructure.SMSChannel> channels = new List<EyouSoft.Model.SMSStructure.SMSChannel>();
            for (int i = 0; i < channel.Count; i++)
            {
                channels.Add(channel[i]);

                this.selSendChannel.Items.Add(new ListItem(channel[i].ChannelName, channel[i].Index.ToString()));
            }
        }
    }
}
