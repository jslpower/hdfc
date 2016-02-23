<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="AjaxSelectTourRequest.aspx.cs"
    Inherits="Web.jidiaoCenter.AjaxSelectTourRequest" %>

<%@ Register Assembly="ControlLibrary" Namespace="Adpost.Common.ExporPage" TagPrefix="cc1" %>
<style type="text/css">
    #TourList
    {
        border-collapse: collapse;
    }
    #TourList td
    {
        border: 1px #b8c5ce solid;
        padding: 0 3px;
    }
</style>
<table width="100%" cellspacing="0" cellpadding="0" bgcolor="#FFFFFF" align="center"
    id="TourList" style="margin: 10 auto" class="tablelist">
    <tr class="">
        <asp:repeater runat="server" id="RptList">
             <ItemTemplate>
               <td align="left" height="30px" width="25%">
               <label>
            <input name="1" type="radio" value="<%#Eval("TourId") %>" data-show="<%#Eval("TourCode")%>" <%#Eval("TourCode")==Request.QueryString["tourcode"]?"checked=checked":"" %> /> 
                <span><%#Eval("TourCode")%></span>
                </label>
             <%#EyouSoft.Common.Utils.IsOutTrOrTd(Container.ItemIndex, recordcount, 4)%>
            </ItemTemplate>  
        </asp:repeater>
</table>
