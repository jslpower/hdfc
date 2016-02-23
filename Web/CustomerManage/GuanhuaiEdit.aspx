<%@ Page Language="C#" MasterPageFile="~/MasterPage/Boxy.Master" AutoEventWireup="true"
    CodeBehind="GuanhuaiEdit.aspx.cs" Inherits="Web.CustomerManage.GuanhuaiEdit"
    Title="客户关怀" %>

<%@ Register src="../UserControl/CustomerUnit.ascx" tagname="CustomerUnit" tagprefix="uc1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="PageHead" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="PageBody" runat="server">
    <form id="form1" runat="server">
    <table width="680" cellspacing="1" cellpadding="0" border="0" align="center" style="margin: 10px auto;">
        <tbody>
            <tr class="odd">
                <th width="120" height="30" align="right">
                    <font class="fred">*</font>拜访人：
                </th>
                <td align="left">
                    <input type="text" id="txtvistor" valid="required" errmsg="请填写拜访人!" class="searchinput inputtext" runat="server" name="txtvistor" />
                </td>
                <th align="right">
                    <font class="fred">*</font>拜访日期：
                </th>
                <td width="120" align="left">
                    <input type="text" id="txtvistTime" valid="required" errmsg="请填写拜访日期!" onfocus="WdatePicker()" class="searchinput inputtext" runat="server" name="txtvistTime" />
                </td>
            </tr>
            <tr class="even">
                <th height="30" align="right">
                    <font class="fred">*</font>组团社：
                </th>
                <td align="left">
                    <uc1:CustomerUnit ID="CustomerUnit1" runat="server" IsRequired="true" />
                </td>
                <th align="right">
                    支出费用：
                </th>
                <td align="left">
                    <input type="text" id="txtoutMoney" valid="isMoney" errmsg="费用格式有误!" class="searchinput inputtext" runat="server" name="txtoutMoney" />
                </td>
            </tr>
            <tr class="odd">
                <th align="right">
                    支出理由：
                </th>
                <td align="left" colspan="3">
                    <asp:TextBox ID="txtoutliYou" runat="server" CssClass="formsize450 inputtext" TextMode="MultiLine" Height="70px" name="txtoutLiYou"></asp:TextBox>
                </td>
            </tr>
            <tr class="odd">
                <th align="right">
                    客户喜好：
                </th>
                <td align="left" colspan="3">
                    <asp:TextBox ID="txtXiHao" CssClass="inputtext formsize450" Height="70px" runat="server"></asp:TextBox>
                </td>
            </tr>
        </tbody>
    </table>
    <table width="320" cellspacing="0" cellpadding="0" border="0" align="center">
        <tbody>
            <tr>
                <td height="40" align="center">
                </td>
                <td height="40" align="center" class="tjbtn02">
                    <a href="javascript:;" id="btn" runat="server">保存</a>
                </td>
                <td height="40" align="center" class="tjbtn02">
                    <a onclick="parent.Boxy.getIframeDialog(parent.Boxy.queryString('iframeId')).hide()"
                        id="linkCancel" href="javascript:;">关闭</a>
                </td>
            </tr>
        </tbody>
    </table>
    
    
    <script type="text/javascript">
        $(function(){
            GuanhuaiEdit.PageInit();
            GuanhuaiEdit.BindBtn();
        })
        var GuanhuaiEdit={
            PageInit:function(){
                $("input[readonly='readonly']").css({ "background-color": "#dadada" });
            },
            BindBtn:function(){
                $("#<%=btn.ClientID %>").click(function(){
                    if(!GuanhuaiEdit.CheckForm()){
                        return false;
                    }
                    var dotype='<%=Request.QueryString["dotype"] %>';
                    var url="/CustomerManage/GuanhuaiEdit.aspx?type=save&dotype="+dotype+"&id="+'<%=Request.QueryString["id"] %>';
                    $(this).html("正在保存");
                    GuanhuaiEdit.GoAjax(url);
                    return false;
                })
            },
            GoAjax: function(url) {
                $("#<%=btn.ClientID %>").unbind("click");
                $.newAjax({
                    type: "post",
                    cache: false,
                    url: url,
                    dataType: "json",
                    data: $("#<%=btn.ClientID %>").closest("form").serialize(),
                    success: function(ret) {
                        if (ret.result == "1") {
                            parent.tableToolbar._showMsg(ret.msg, function() { parent.location.href = parent.location.href; });
                        }
                        else {
                            parent.tableToolbar._showMsg(ret.msg);
                        }
                    },
                    error: function() {
                        tableToolbar._showMsg(tableToolbar.errorMsg);
                    }
                });
            },
            CheckForm: function() {
                return ValiDatorForm.validator($("#<%=btn.ClientID %>").closest("form").get(0), "parent");
            }
         }
    </script>
    </form>
</asp:Content>
