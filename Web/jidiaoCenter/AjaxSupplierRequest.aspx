<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="AjaxSupplierRequest.aspx.cs"
    Inherits="Web.jidiaoCenter.AjaxSupplierRequest" EnableEventValidation="false" %>

<%@ Register Assembly="ControlLibrary" Namespace="Adpost.Common.ExporPage" TagPrefix="cc1" %>
<style type="text/css">
    #tblList
    {
        border-collapse: collapse;
    }
    #tblList td
    {
        border: 1px #b8c5ce solid;
        padding: 0 3px;
    }
</style>
<!--paopao end-->
<table width="100%" cellspacing="0" cellpadding="0" bgcolor="#FFFFFF" align="center"
    id="tblList" style="margin: 0 auto" class="tablelist">
    <tr class="">
        <asp:repeater runat="server" id="RptguideList">
             <ItemTemplate>
               <td align="left" height="30px" width="25%">
               <label>
            <input name="1" type="radio" value="<%#Eval("Id") %>" data-show="<%#Eval("GuideName")%>" data-contactname="" data-qq="" data-tel="<%#Eval("Phone")%>" <%#Eval("GuideName")==Request.QueryString["name"]?"checked=checked":"" %> /> 
                 <span><%#Eval("GuideName")%></span>
                </label>
             <%#EyouSoft.Common.Utils.IsOutTrOrTd(Container.ItemIndex, listcount, 4)%>
            </ItemTemplate>  
        </asp:repeater>
        <asp:repeater runat="server" id="RptDateList">
             <ItemTemplate>
               <td align="left" height="30px" width="25%">
               <label>
            <input name="1" type="radio" value="<%#Eval("Id") %>" data-show="<%#Eval("UnitName")%>" data-contactname="<%#GetContactInfo(Eval("ContactList"),"name")%>" data-qq="<%#GetContactInfo(Eval("ContactList"),"qq")%>" data-tel="<%#GetContactInfo(Eval("ContactList"),"tel")%>" <%#Eval("UnitName")==Request.QueryString["name"]?"checked=checked":"" %> /> 
                <span><%#Eval("UnitName")%></span>
                </label>
                
             <%#EyouSoft.Common.Utils.IsOutTrOrTd(Container.ItemIndex, listcount, 4)%>
            </ItemTemplate>  
        </asp:repeater>
        <asp:literal runat="server" id="lbemptymsg"></asp:literal>
</table>
<form runat="server" id="formUnit">
<table width="100%" border="0" cellpadding="0" cellspacing="1" id="Tabform" runat="server">
    <tr class="even">
        <th>
            <font class="fred">*</font> 单位名称
        </th>
        <th>
            <font class="fred">*</font>所在地
        </th>
        <th>
            <font class="fred">*</font>联系人
        </th>
        <th>
            联系电话
        </th>
    </tr>
    <tr>
        <td style="text-align: center">
            <asp:textbox runat="server" class="inputtext formsize100" id="txtunitname" valid="required"
                errmsg="请填写单位名称"></asp:textbox>
        </td>
        <td style="text-align: center">
            <asp:dropdownlist id="ddlProvice" name="ddlProvice" valid="required" errmsg="请选择省份"
                runat="server" cssclass="inputselect" />
            <asp:dropdownlist id="ddlCity" runat="server" cssclass="inputselect" errmsg="请选择城市"
                name="ddlCity" valid="required" />
        </td>
        <td style="text-align: center">
            <asp:textbox runat="server" class="inputtext formsize100" valid="required" errmsg="请填写联系人"
                id="txtcontactname"></asp:textbox>
        </td>
        <td style="text-align: center">
            <asp:textbox runat="server" class="inputtext formsize100" id="txtcontacttel"></asp:textbox>
        </td>
    </tr>
</table>
<table width="100%" border="0" cellpadding="0" cellspacing="1" id="TabGuide" runat="server">
    <tr class="even">
        <th>
            <font class="fred">*</font>旅行社名称
        </th>
        <th>
            <font class="fred">*</font>导游名称
        </th>
        <th>
            <font class="fred">*</font>星级
        </th>
        <th>
           联系电话
        </th>
    </tr>
    <tr>
        <td style="text-align: center;">
            <asp:textbox runat="server" class="inputtext formsize100" id="txtgysname" valid="required" errmsg="请填写旅行社名称"></asp:textbox>
        </td>
        <td style="text-align: center">
            <asp:textbox runat="server" class="inputtext formsize100" id="txtguidename" valid="required"
                errmsg="请填写导游名称"></asp:textbox>
        </td>
        <td style="text-align: center; display: none;">
            <asp:dropdownlist id="ddlProvice_guide" name="ddlProvice_guide" 
                runat="server" cssclass="inputselect" />
            <asp:dropdownlist id="ddlCity_guide" runat="server" cssclass="inputselect" 
                name="ddlCity_guide"  />
        </td>
        <td style="text-align: center">
            <asp:dropdownlist id="ddlStar" runat="server" cssclass="inputselect" valid="required"
                errmsg="请选择导游星级">
                </asp:dropdownlist>
        </td>
        <td style="text-align: center">
            <asp:textbox runat="server" class="inputtext formsize100" id="txtcontacttel_guide" ></asp:textbox>
        </td>
    </tr>
</table>
<table width="100%" cellspacing="0" cellpadding="0" border="0" align="center" style="margin: 10px auto;"
    id="tab_save">
    <tr class="odd">
        <td height="30" bgcolor="#E3F1FC" align="left" colspan="14">
            <table cellspacing="0" cellpadding="0" border="0" align="center">
                <tbody>
                    <tr>
                        <td width="80" height="40" align="center" class="tjbtn02">
                            <a href="javascript:;" id="btn_save">保存</a>
                        </td>
                        <td width="80" height="40" align="center" class="tjbtn02">
                            <a href="javascript:;" id="btnxuan">保存并选用</a>
                        </td>
                    </tr>
                </tbody>
            </table>
        </td>
    </tr>
</table>
</form>

<script type="text/javascript">
    $(function() {
        pcToobar.init({
            pID: "#<%=ddlProvice.ClientID %>",
            cID: "#<%=ddlCity.ClientID %>",
            pSelect: '-1',
            cSelect: '-1',
            comID: '<%=this.SiteUserInfo.CompanyId %>'
        });
        pcToobar.init({
            pID: "#<%=ddlProvice_guide.ClientID %>",
            cID: "#<%=ddlCity_guide.ClientID %>",
            pSelect: '-1',
            cSelect: '-1',
            comID: '<%=this.SiteUserInfo.CompanyId %>'
        });
        var datacount = '<%=listcount %>';
        if (datacount > 0) {
            $("#TabBtn").show();
            $("#tab_save").hide();
        } else {
            $("#TabBtn").hide();
            $("#tab_save").show();
        }
    })
</script>

