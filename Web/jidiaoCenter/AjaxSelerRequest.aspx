<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="AjaxSelerRequest.aspx.cs"
    Inherits="Web.jidiaoCenter.AjaxSelerRequest" %>

<%@ Import Namespace="EyouSoft.Model.CompanyStructure" %>
<style type="text/css">
    #tab_seller
    {
        border-collapse: collapse;
    }
    #tab_seller td
    {
        border: 1px #b8c5ce solid;
        padding: 0 3px;
        height:30px;
        width:25%;
    }
</style>
<table id="tab_seller" width="100%" cellspacing="0" cellpadding="0" class="alertboxbk1"
    border="0" bgcolor="#FFFFFF" style="border-collapse: collapse; margin: 5px 0;">
    <tbody>
        <tr>
            <asp:repeater id="rptList" runat="server">
                                        <ItemTemplate>
                                            <td align="left">
                                                <label>
                                                    <%if (EyouSoft.Common.Utils.GetQueryStringValue("sModel") == "1")
                                                      { %>
                                                    <input <%#((ContactPersonInfo)Eval("PersonInfo")).ContactName==Request.QueryString["rname"]?"checked=checked":"" %> type="radio" name="contactID" value="<%#Eval("Id")%>" />
                                                    <%}
                                                      else
                                                      { %>
                                                    <input type="checkbox" name="contactID" value="<%#Eval("Id")%>" />
                                                    <%} %>
                                                    <span data-tel='<%#((EyouSoft.Model.CompanyStructure.ContactPersonInfo)Eval("PersonInfo")).ContactTel%>'
                                                        data-deptid='<%#Eval("DepartId") %>' data-deptname='<%#Eval("DepartName") %>'>
                                                        <%#((ContactPersonInfo)Eval("PersonInfo")).ContactName%></span>
                                                </label>
                                           
                                            <%#EyouSoft.Common.Utils.IsOutTrOrTd(Container.ItemIndex, listCount, 4)%>
                                        </ItemTemplate>
                                    </asp:repeater>
        
    </tbody>
</table>
<div style="width: 100%; text-align: center; background-color: #ffffff">
    <asp:label id="lblMsg" runat="server" text=""></asp:label>
</div>
