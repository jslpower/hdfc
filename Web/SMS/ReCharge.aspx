<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ReCharge.aspx.cs" Inherits="Web.SMS.ReCharge" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>充值_短信中心</title>
    <link href="/css/style.css" rel="stylesheet" type="text/css" />
</head>
<body>
    <form id="ReChargeForm" runat="server">
    <div>
        <input type="hidden" name="method" value="recharge" />
        <table width="600" border="0" align="center" cellpadding="0" cellspacing="1" style="margin: 20px auto;">
            <tr class="odd">
                <th width="18%" height="30" align="right">
                    客户名称：
                </th>
                <td width="82%" bgcolor="#E3F1FC">
                    <input type="text" class="xtinput inputtext" id="txtCompanyName" disabled="disabled" runat="server"
                        size="40" />
                </td>
            </tr>
            <tr class="odd">
                <th width="18%" height="30" align="right">
                    充值人：
                </th>
                <td width="82%" bgcolor="#E3F1FC">
                    <input type="text" class="xtinput inputtext" disabled="disabled" id="txtUserName" value="端木宪章"
                        size="20" runat="server" />
                </td>
            </tr>
            <tr class="odd">
                <th width="18%" height="30" align="right">
                    充值时间：
                </th>
                <td width="82%" bgcolor="#E3F1FC">
                    <input type="text" class="xtinput inputtext" readonly="readonly" id="txtRechargeDate" style="width: 100px"
                        runat="server" size="30" onfocus="WdatePicker({dateFmt:'yyyy-MM-dd HH:mm'});" />
                </td>
            </tr>
            <tr class="odd">
                <th width="18%" height="30" align="right">
                    充值金额：
                </th>
                <td width="82%" bgcolor="#E3F1FC">
                    <input id="txtRechargeMoney" runat="server" type="text" class="xtinput inputtext" style="width: 100px"
                        maxlength="8" size="40" valid="required|isMoney" errmsg="输入充值金额|请输入正确充值金额" />
                    <span id="errMsg_<%=txtRechargeMoney.ClientID %>" class="errmsg"></span>
                </td>
            </tr>
            <tr class="odd">
                <td height="30" colspan="8" align="left" bgcolor="#E3F1FC">
                    <table width="340" border="0" cellspacing="0" cellpadding="0">
                        <tr>
                            <td width="106" height="40" align="center">
                            </td>
                            <td width="76" height="40" align="center" class="tjbtn02">
                                <a href="javascript:;" onclick="return Recharge.rechargeM();">我要充值</a>
                            </td>
                            <td width="158" height="40" align="center" class="jixusave">
                                <span id="span_PayMoneyError" style="display: none"></span>
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
        </table>
        <table width="600" border="0" align="center" cellpadding="0" cellspacing="0">
            <tr>
                <td width="324">
                    <strong>公司账户<br />
                        杭州易诺科技有限公司</strong><br />
                    <span style="font-size: 18px; font-weight: bold; color: #009933; font-family: Arial, Helvetica, sans-serif">
                        8001 2929 3708 0910 01</span><br />
                    中国银行杭州市高新技术开发区支行
                    <p>
                        汇款后我们将及时为您开通短信！<br />
                        详情请致电客服何小姐 0571-56884627</p>
                </td>
                <td width="276">
                    <strong>个人账户<br />
                        <strong>1、朱永蕾： 农行银行杭州科技城支行卡号 </strong>
                        <br />
                        <span style="font-size: 18px; font-weight: bold; color: #009933; font-family: Arial, Helvetica, sans-serif">
                            622848 0322 1100 65115</span>
                        <br />
                        <br />
                        <strong>2、朱永蕾：中国建设银行杭州</strong><br />
                        <span style="font-size: 18px; font-weight: bold; color: #009933; font-family: Arial, Helvetica, sans-serif">
                            6222 8015 4111 1051 601</span></strong>
                </td>
            </tr>
        </table>
    </div>
    </form>

    <script src="/js/jquery-1.4.4.js" type="text/javascript"></script>

    <script src="/js/ValiDatorForm.js" type="text/javascript"></script>

    <script src="/js/datepicker/WdatePicker.js" type="text/javascript"></script>

    <script type="text/javascript">
        $(document).ready(function() {
            FV_onBlur.initValid($("#<%=ReChargeForm.ClientID %>").get(0));
        });
        var Recharge =
       {
           //充值
           rechargeM: function() {
               var form = $("#<%=ReChargeForm.ClientID %>").get(0);
               var vResult = ValiDatorForm.validator(form, "span");
               if (!vResult) {
                   return false;
               }
               else {
                   if (confirm("你确定要充值吗？")) {
                       $.newAjax({
                           type: "post",
                           dataType: "json",
                           url: "ReCharge.aspx",
                           data: $(form).serialize(),
                           cache: false,
                           success: function(result) {
                               if (result.success == "1") {
                                   alert("充值成功，请等待审核");
                                   var objBoxy = window.parent.Boxy.getIframeDialog('<%=Request.QueryString["iframeId"] %>');
                                   if (objBoxy.options.data && objBoxy.options.data == "account") {
                                       window.parent.AccountInfo.getList("","");
                                   }
                                   objBoxy.hide();
                               }
                               else {
                                   alert(result.message);
                               }
                           }
                       });

                   }
               }
               return false;
           }
       }
    </script>

</body>
</html>
