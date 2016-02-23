<%@ Page Title="发送历史打印" Language="C#" MasterPageFile="~/masterpage/Print.Master"
    AutoEventWireup="true" CodeBehind="SendHistoryPrint.aspx.cs" Inherits="Web.SMS.SendHistoryPrint" %>

<%@ Register Assembly="ControlLibrary" Namespace="ControlLibrary" TagPrefix="asp" %>
<asp:Content ID="Content1" ContentPlaceHolderID="PrintC1" runat="server">
    <div style="width: 800px; margin: auto;">
        <table width="100%" border="1" cellpadding="0" cellspacing="1" style="border-collapse: collapse">
            <tr>
                <th width="10%" align="center">
                    编号
                </th>
                <th width="12%" align="center">
                    <strong>发送时间</strong>
                </th>
                <th width="12%" align="center">
                    号码
                </th>
                <th width="45%" align="center">
                    发送内容
                </th>
                <th width="11%" align="center">
                    价格
                </th>
                <th width="10%" align="center">
                    状态
                </th>
            </tr>
            <asp:CustomRepeater runat="server" ID="rptSendHistory">
                <ItemTemplate>
                    <tr>
                        <td align="center">
                            <%=itemIndex++ %>
                        </td>
                        <td align="center">
                            <%# Convert.ToDateTime(Eval("SendTime")).ToString("yyyy-MM-dd HH:mm") %>
                        </td>
                        <td align="center">
                            <%# Eval("MobileNumber") %>
                        </td>
                        <td align="left" class="pandl3">
                            <%# Eval("SMSContent") %>
                        </td>
                        <td align="center">
                            <%#((decimal)Eval("UseMoeny")).ToString("F2")%>
                        </td>
                        <td align="center">
                            <%# Eval("ReturnResult").ToString()=="0"?"发送成功":"发送失败" %>
                        </td>
                    </tr>
                </ItemTemplate>
                <AlternatingItemTemplate>
                    <tr>
                        <td align="center">
                            <%=itemIndex++ %>
                        </td>
                        <td align="center">
                            <%# Convert.ToDateTime(Eval("SendTime")).ToString("yyyy-MM-dd HH:mm") %>
                        </td>
                        <td align="center">
                            <%# Eval("MobileNumber") %>
                        </td>
                        <td align="left" class="pandl3">
                            <%# Eval("SMSContent") %>
                        </td>
                        <td align="center">
                            <%#((decimal)Eval("UseMoeny")).ToString("F2")%>
                        </td>
                        <td align="center">
                            <%# Eval("ReturnResult").ToString()=="0"?"发送成功":"发送失败" %>
                        </td>
                    </tr>
                </AlternatingItemTemplate>
            </asp:CustomRepeater>
        </table>
    </div>
</asp:Content>
