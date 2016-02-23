using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Text;
using EyouSoft.Common;


namespace Web.SMS
{   
    /// <summary>
    /// 发送短信
    /// xuty 2011/1/22
    /// </summary>
    public partial class SendSms : EyouSoft.Common.Page.BackPage
    {
        protected string showPay = "style='display:none'";
        protected void Page_Load(object sender, EventArgs e)
        {
            //判断权限
            if (!CheckGrant(EyouSoft.Model.EnumType.PrivsStructure.Privs3.短信中心_短信中心_栏目))
            {
                Utils.ResponseNoPermit(EyouSoft.Model.EnumType.PrivsStructure.Privs3.短信中心_短信中心_栏目, true);
                return;
            }
            //当前操作
            string method = Utils.GetFormValue("method");
            //验证或发送
            if (method == "valid" || method == "send")
            {
                #region 发送短信

                #region 获取发送手机号并验证
                List<EyouSoft.Model.SMSStructure.AcceptMobileInfo> moibleList = new List<EyouSoft.Model.SMSStructure.AcceptMobileInfo>();
                string[] moibleArr = Utils.InputText(Utils.GetFormValue("txtMobiles")).Split(',', '，');
                StringBuilder errBuilder = new StringBuilder();
                string resultMess = string.Empty;
                bool result = false;
                foreach (string mobile in moibleArr)
                {
                    if (!Utils.IsMobile(mobile))//验证手机格式如果不正确则输出错误手机号
                    {
                        errBuilder.AppendFormat("{0}，", mobile);
                    }
                    else
                    {
                        EyouSoft.Model.SMSStructure.AcceptMobileInfo mobileInfo = new EyouSoft.Model.SMSStructure.AcceptMobileInfo();
                        mobileInfo.IsEncrypt = false;
                        mobileInfo.Mobile = mobile;
                        moibleList.Add(mobileInfo);
                    }
                }
                resultMess = errBuilder.ToString();
                EyouSoft.BLL.SMSStructure.SendMessage sendBll = new EyouSoft.BLL.SMSStructure.SendMessage();
                string smsContent = Utils.InputText(Utils.GetFormValue("textContent"));
                string forbidWords = sendBll.IsIncludeKeyWord(smsContent);//获取禁止发送的词
                if (!String.IsNullOrEmpty(forbidWords))
                {
                    resultMess += "短信包含禁止发送的关键词：" + forbidWords;
                }
                #endregion

                //如果验证手机格式都通过则发送
                if (resultMess == "")
                {
                    #region 构造短息信息实体
                    //发送实体
                    EyouSoft.Model.SMSStructure.SendMessageInfo sendMessageInfo = new EyouSoft.Model.SMSStructure.SendMessageInfo();
                    //发信人公司ID
                    sendMessageInfo.CompanyId = CurrentUserCompanyID;
                    //发信人公司名
                    sendMessageInfo.CompanyName = SiteUserInfo.CompanyName;
                    //发送通道
                    sendMessageInfo.SendChannel = new EyouSoft.Model.SMSStructure.SMSChannelList()[Utils.GetInt(Utils.GetFormValue(this.selChannel.UniqueID))];
                    //发送类型
                    sendMessageInfo.SendType = (EyouSoft.Model.SMSStructure.SendType)Utils.GetInt(Utils.GetFormValue("selSendType"));
                    //发送时间
                    sendMessageInfo.SendTime = DateTime.Now;
                    if (sendMessageInfo.SendType == EyouSoft.Model.SMSStructure.SendType.定时发送)
                    {
                        sendMessageInfo.SendTime = Utils.GetDateTime(Utils.GetFormValue("txtSendTime"));
                    }
                    //短信内容
                    sendMessageInfo.SMSContent = smsContent;
                    //号码集合
                    sendMessageInfo.Mobiles = moibleList;
                    //发送ID
                    sendMessageInfo.UserId = SiteUserInfo.UserId;
                    //发信人姓名(如果勾选了发信人)
                    if (Utils.GetFormValue("chkSender") == "hasSender")
                    {
                        sendMessageInfo.UserFullName = Utils.GetFormValue("txtSender");
                    }
                    #endregion
                    //初始化发送bll
                   
                    if (method == "valid")
                    {
                        #region 发送短信之前验证
                        //发送短信之前验证，返回验证结果实体
                        EyouSoft.Model.SMSStructure.SendResultInfo resultInfoBefore = sendBll.ValidateSend(sendMessageInfo);
                        if (resultInfoBefore != null)
                        {
                            result = resultInfoBefore.IsSucceed;
                            if (resultInfoBefore.IsSucceed == true)
                            {
                                //验证成功返回帐户余额以及此次所要发送的短信条数
                                resultMess = string.Format("1@您的账户当前余额为：{0}，此次消费金额为：{1}，将共发送短信{2}条！是否确定发送短信？"
                                    , resultInfoBefore.AccountMoney.ToString("C2")
                                    , resultInfoBefore.CountFee.ToString("C2")
                                    , (resultInfoBefore.WaitSendMobileCount + resultInfoBefore.WaitSendPHSCount).ToString());
                            }
                            else if (resultInfoBefore.ErrorMessage.IndexOf("余额不足") > -1)
                            {
                                //余额不足
                                resultMess = "@0" + resultInfoBefore.ErrorMessage;
                            }
                            else
                            {  //其他错误
                                resultMess = "@2" + resultInfoBefore.ErrorMessage;
                            }
                        }
                        #endregion
                    }
                    else
                    {
                        #region 执行发送
                        //执行发送，返回发送结果实体
                        EyouSoft.Model.SMSStructure.SendResultInfo resultInfo = sendBll.Send(sendMessageInfo);
                        if (resultInfo != null)
                        {
                            result = resultInfo.IsSucceed;
                            if (resultInfo.IsSucceed == true)
                            {
                                //发送成功
                                decimal costFee = sendMessageInfo.SendType == EyouSoft.Model.SMSStructure.SendType.直接发送 ? resultInfo.SendFee : resultInfo.CountFee;
                                resultMess = "ok@您本次共发送短信" + sendMessageInfo.SMSContentSendComplete.Length + "个字" + (!string.IsNullOrEmpty(sendMessageInfo.UserFullName) ? "（包含发信人）" : "") + "!发送移动、联通共" + resultInfo.MobileSendNumberCount + "个号码、发送小灵通共" + resultInfo.PHSSendNumberCount + "个号码、实际共消费金额为:" + costFee.ToString("C2") + "、实际发送短信" + (resultInfo.MobileSendCount + resultInfo.PHSSendCount) + "条";
                            }
                            else if (resultInfo.ErrorMessage.IndexOf("余额不足") > -1)
                            {   //余额不足
                                resultMess = "0@" + resultInfo.ErrorMessage;
                            }
                            else
                            {   //其他错误
                                resultMess = "2@" + resultInfo.ErrorMessage;
                            }
                        }
                        #endregion
                    }
                }
                else
                {   
                    //手机格式未通过输出消息
                    resultMess = "0@" + resultMess;
                    result = false;
                }
                Utils.ResponseMeg(result, resultMess);
                #endregion
            }
            else
            {
                #region 初次加载
                //获取账户余额，如果少于等于0则显示充值按钮和短信提示信息
                //用户账户Bll
                EyouSoft.BLL.SMSStructure.Account accountBll = new EyouSoft.BLL.SMSStructure.Account();
                decimal accountMoney = accountBll.GetAccountMoney(CurrentUserCompanyID);
                if (accountMoney <= 0)
                {
                    showPay = "style='display:none'";
                    remainNum.Text = "你的短信剩余条数为0！";
                    remainNum.Visible = true;
                    showPay = "style='display:none'";
                }
                InitSendChannel();
                #endregion
            }
         }

        #region 绑定发送通道
        /// <summary>
        /// 初始化短信发送通道
        /// </summary>
        private void InitSendChannel()
        {
            EyouSoft.Model.SMSStructure.SMSChannelList channel = new EyouSoft.Model.SMSStructure.SMSChannelList();
            for (int i = 0; i < channel.Count; i++)
            {
                ListItem channelItem = new ListItem(channel[i].ChannelName, channel[i].Index.ToString());
                channelItem.Attributes.Add("IsLong", channel[i].IsLong.ToString());
                this.selChannel.Items.Add(channelItem);
            }
            this.selChannel.Attributes.Add("onchange", "SendSms.selChannel(this);");

            
        }
        #endregion
    }
}
