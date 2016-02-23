<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="AjaxReChargeAndConsume.aspx.cs"
    Inherits="Web.SMS.AjaxReChargeAndConsume" %>

<%@ Register Assembly="ControlLibrary" Namespace="Adpost.Common.ExporPage" TagPrefix="uc2" %>
<%@ Register Assembly="ControlLibrary" Namespace="ControlLibrary" TagPrefix="asp" %>
<% if (recharge)
   { %>
<tr id="tr_recharge">
    <td width="15%" align="right" valign="middle" bgcolor="#bddcf4" class="yue">
        <strong>充值明细：</strong>
    </td>
    <td width="85%">
        <table width="100%" border="0" cellpadding="0" cellspacing="1">
            <tr>
                <th width="25%" height="26" align="center" bgcolor="#bddcf4">
                    充值时间
                </th>
                <th width="25%" height="26" align="center" bgcolor="#bddcf4">
                    <strong>充值金额</strong>
                </th>
                <th width="25%" height="26" align="center" bgcolor="#bddcf4">
                    充值状态
                </th>
                <th width="25%" height="26" align="center" bgcolor="#bddcf4">
                    <strong>充值人</strong>
                </th>
            </tr>
            <asp:customrepeater id="rptRechargeList" runat="server">
                           <ItemTemplate>
                        <tr>
                          <td width="25%" height="26"  align="center" bgcolor="#e3f1fc"><%# Convert.ToDateTime(Eval("PayTime")).ToString("yyyy-MM-dd HH:mm") %></td>
                          <td width="25%" height="26" align="center" bgcolor="#e3f1fc"><%#Convert.ToDecimal( Eval("PayMoney").ToString()).ToString("F2")%>元</td>
                          <td width="25%" height="26" align="left" bgcolor="#e3f1fc" class="pandl3"><%# Eval("IsChecked").ToString() == "1" ? "充值成功    获得可用金额" + Eval("UseMoney","{0:f2}") + "元" : "未通过"%></td>
                          <td width="25%" height="26" align="center" bgcolor="#e3f1fc"><%#Eval("OperatorName")%></td>
                        </tr>
                        </ItemTemplate>
                           <AlternatingItemTemplate>
                        <tr>
                          <td width="25%" height="26"  align="center" bgcolor="#bddcf4"><%# Convert.ToDateTime(Eval("PayTime")).ToString("yyyy-MM-dd HH:mm") %></td>
                          <td width="25%" height="26" align="center" bgcolor="#bddcf4"><%#Convert.ToDecimal( Eval("PayMoney").ToString()).ToString("F2")%>元</td>
                          <td width="25%" height="26" align="left" bgcolor="#bddcf4" class="pandl3"><%# Eval("IsChecked").ToString() == "1" ? "充值成功    获得可用金额" + Eval("UseMoney","{0:f2}") + "元" : "未通过"%></td>
                          <td width="25%" height="26" align="center" bgcolor="#bddcf4"><%#Eval("OperatorName")%></td>
                        </tr>
                        </AlternatingItemTemplate>
                        </asp:customrepeater>
        </table>
        <table width="100%" border="0" cellspacing="0" cellpadding="0">
            <tr>
                <td align="right">
                    <uc2:ExporPageInfoSelect ID="ExporPageInfoSelect1" runat="server" PageStyleType="NewButton"
                        LinkType="2" HrefType="JsHref"></uc2:ExporPageInfoSelect>
                </td>
            </tr>
        </table>
    </td>
</tr>
<%} %>
<%if (consume)
  { %>
<tr id="tr_consume">
    <td align="right" valign="middle" bgcolor="#bddcf4" class="yue">
        <strong>消费明细：</strong>
    </td>
    <td>
        <table width="100%" border="0" cellpadding="0" cellspacing="1">
            <tr>
                <th width="20%" height="28" align="center" bgcolor="#BDDCF4">
                    发送时间
                </th>
                <th width="15%" height="28" align="center" bgcolor="#bddcf4">
                    <strong>号码</strong>
                </th>
                <th width="31%" height="28" align="center" bgcolor="#bddcf4">
                    发送内容
                </th>
                <th width="10%" height="28" align="center" bgcolor="#bddcf4">
                    <strong>价格</strong>
                </th>
                <th width="14%" height="28" align="center" bgcolor="#bddcf4">
                    发送通道
                </th>
            </tr>
            <asp:customrepeater id="rptConsumeList" runat="server">
                           <ItemTemplate>
                        <tr>
                          <td width="20%" height="28"  align="center" bgcolor="#e3f1fc"><%# Convert.ToDateTime(Eval("IssueTime")).ToString("yyyy-MM-dd HH:mm") %></td>
                          <td width="15%" height="28" align="center" bgcolor="#e3f1fc"><a href="javascript:;" onclick="return AccountInfo.getSD('1','<%# Eval("ID") %>','查看号码');">发送成功(<%# Eval("SuccessCount")%>)</a><div><a href="javascript:;" onclick="return AccountInfo.getSD('2','<%# Eval("ID") %>','查看号码');">发送失败(<%# Eval("ErrorCount")%>)</a></div></td>
                          <td width="31%" height="28" align="left" bgcolor="#e3f1fc" class="pandl3"><%# Eval("SMSContent") %></td>
                          <td width="10%" height="28" align="center" bgcolor="#e3f1fc"><%#Convert.ToDecimal(Eval("UseMoeny").ToString()).ToString("F2")%>元</td>
                          <td width="14%" height="28" align="center" bgcolor="#e3f1fc"><span class="pandl3"><%# ((EyouSoft.Model.SMSStructure.SMSChannel)Eval("SendChannel")).ChannelName %></span></td>
                        </tr>
                          </ItemTemplate>
                           <AlternatingItemTemplate>
                        <tr>
                          <td width="13%" height="28"  align="center" bgcolor="#BDDCF4"><%# Convert.ToDateTime(Eval("IssueTime")).ToString("yyyy-MM-dd HH:mm") %></td>
                          <td width="15%" height="28" align="center" bgcolor="#BDDCF4"><a href="javascript:;" onclick="return AccountInfo.getSD('1','<%# Eval("ID") %>','查看号码');">发送成功(<%# Eval("SuccessCount")%>)</a><div><a href="javascript:;" onclick="return AccountInfo.getSD('2','<%# Eval("ID") %>','查看号码');">发送失败(<%# Eval("ErrorCount")%>)</a></div></td>
                          <td width="31%" height="28" align="left" bgcolor="#BDDCF4" class="pandl3"><%# Eval("SMSContent") %></td>
                          <td width="10%" height="28" align="center" bgcolor="#BDDCF4"><%#Convert.ToDecimal(Eval("UseMoeny").ToString()).ToString("F2")%>元</td>
                          <td width="14%" height="28" align="center" bgcolor="#BDDCF4"><span class="pandl3"><%# ((EyouSoft.Model.SMSStructure.SMSChannel)Eval("SendChannel")).ChannelName %></span></td>
                        </tr>
                        </AlternatingItemTemplate>
                       </asp:customrepeater>
        </table>
        <table width="100%" border="0" cellspacing="0" cellpadding="0">
            <tr>
                <td align="right">
                    <uc2:ExporPageInfoSelect ID="ExporPageInfoSelect2" runat="server" PageStyleType="NewButton"
                        LinkType="2" HrefType="JsHref"></uc2:ExporPageInfoSelect>
                </td>
            </tr>
        </table>
    </td>
</tr>
<% }%>