<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="AjaxRouteRequest.aspx.cs"
    Inherits="Web.jidiaoCenter.AjaxRouteRequest" %>
<style type="text/css">
    #tblroute
    {
        border-collapse: collapse;
    }
    #tblroute td
    {
        border: 1px #b8c5ce solid;
        padding: 0 3px;
    }
</style>
<table width="900" border="0" align="center" cellpadding="0" cellspacing="1" id="tblroute">
    <tr>
        <asp:repeater runat="server" id="rptRoute">
                    <ItemTemplate>
                        <%# GetTrHtml(Container.ItemIndex)%>
                        <td  height="30px" width="25%">
                            <%# GetInputHtml(Eval("RouteId"), Eval("RouteName"), Container.ItemIndex)%>
                        </td>
                        <%#EyouSoft.Common.Utils.IsOutTrOrTd(Container.ItemIndex, recordcount, 4)%>
                    </ItemTemplate>
                </asp:repeater>
                </tr>
   
</table>
