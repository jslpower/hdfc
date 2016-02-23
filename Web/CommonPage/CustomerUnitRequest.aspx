<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="CustomerUnitRequest.aspx.cs"
    Inherits="Web.CommonPage.CustomerUnitRequest" EnableEventValidation="false" %>

<%@ Register Src="../UserControl/ContactInfo.ascx" TagName="ContactInfo" TagPrefix="uc1" %>
<style type="text/css">
    #cTableList
    {
        border-collapse: collapse;
    }
    #cTableList td
    {
        border: 1px #b8c5ce solid;
        padding: 0 3px;
        width: 25%;
        height: 30px;
    }
</style>
<table width="100%" border="0" cellpadding="0" cellspacing="1" id="cTableList">
    <tr class="">
        <asp:repeater runat="server" id="rptCustomer">
                        <ItemTemplate>
                            
                            <td align="left">
                                <label>
                                    <%# GetInputHtml(Eval("Id"), Eval("CustomerName"), Eval("ContactName"), Eval("Phone"), Eval("Mobile"))%>
                                    <span>
                                        <%#Eval("CustomerName")%></span><br />
                                        &nbsp;&nbsp;&nbsp;&nbsp;[信用等级：<%#Eval("RatingName")%>]
                                </label>
                             <%#EyouSoft.Common.Utils.IsOutTrOrTd(Container.ItemIndex, recordcount, 4)%>
                        </ItemTemplate>
                    </asp:repeater>
        <asp:literal runat="server" id="lbemptymsg"></asp:literal>
</table>
<form runat="server" id="formUnit">
<table width="100%" border="0" cellpadding="0" cellspacing="1" id="Tabform" runat="server">
    <tr class="even">
        <th>
            <font class="fred">*</font> 组团社
        </th>
        <th>
            <font class="fred">*</font>所在地
        </th>
        <th>
            <font class="fred">*</font> 销售地区
        </th>
        <th>
            主要联系人
        </th>
        <th>
            联系电话
        </th>
        <th>
            手机
        </th>
    </tr>
    <tr>
        <td style="text-align: center">
            <asp:textbox runat="server" class="inputtext formsize100" id="txtunitname" valid="required"
                errmsg="请填写组团社"></asp:textbox>
        </td>
        <td style="text-align: center">
            <asp:dropdownlist id="ddlProvice" name="ddlProvice" valid="required|RegInteger" errmsg="请选择省份|请选择省份"
                runat="server" cssclass="inputselect" />
            <asp:dropdownlist id="ddlCity" runat="server" cssclass="inputselect" errmsg="请选择城市|请选择城市"
                name="ddlCity" valid="required|RegInteger" />
        </td>
        <td style="text-align: center">
            <select id="sltarea" name="sltarea" class="inputselect">
                <%=BindArea()%>
            </select>
        </td>
        <td style="text-align: center">
            <asp:textbox runat="server" class="inputtext formsize100" id="txtcontactname"></asp:textbox>
        </td>
        <td style="text-align: center">
            <asp:textbox runat="server" class="inputtext formsize100" id="txtcontacttel"></asp:textbox>
        </td>
        <td style="text-align: center">
            <asp:textbox runat="server" class="inputtext formsize100" valid="isMobile" errmsg="请填写正确的手机号!"
                id="txtcontactmobile"></asp:textbox>
        </td>
    </tr>
    <tr class="odd">
        <th align="center">
            联系人信息：
        </th>
        <td colspan="5">
            <uc1:ContactInfo ID="ContactInfo1" runat="server" />
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
<input type="hidden" value="-1" runat="server" id="hidareaid" name="hidareaid" />
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
        var datacount = '<%=recordcount %>';
        if (datacount > 0) {
            $("#TabBtn").show();
            $("#tab_save").hide();
        } else {
            $("#TabBtn").hide();
            $("#tab_save").show();
        }
        $("#sltarea").change(function() {
            $("#<%=hidareaid.ClientID %>").val($(this).val());
        })
        $("#btn_save,#btnxuan").click(function() {
            if ($("#<%=hidareaid.ClientID %>").val() == "-1") {
                parent.tableToolbar._showMsg("请选择销售地区");
                return false;
            }
        })
    })
</script>

