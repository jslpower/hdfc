<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="BankAdd.aspx.cs" Inherits="Web.Fin.BankAdd"
    EnableEventValidation="false" MasterPageFile="~/MasterPage/Boxy.Master" %>

<asp:Content ID="Content1" ContentPlaceHolderID="PageHead" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="PageBody" runat="server">
    <div style="width: 630px; margin: 10px auto;">
        <form action="/Fin/BankAdd.aspx" runat="server">
        <table width="100%" border="0" align="center" cellpadding="0" cellspacing="1">
            <tr class="odd">
                <th width="120" height="30" align="right">
                    日期：
                </th>
                <td bgcolor="#E3F1FC">
                    <input name="txtDate" type="text" class="inputtext" id="txtDate" runat="server" onfocus="WdatePicker()"
                        valid="required" errmsg="请输入日期" />
                </td>
            </tr>
            <tr class="odd">
                <th height="30" align="right">
                    银行账号：
                </th>
                <td bgcolor="#E3F1FC">
                    <asp:DropDownList ID="ddlBank" runat="server" CssClass="inputselect" valid="required"
                        errmsg="请选择银行账号">
                    </asp:DropDownList>
                </td>
            </tr>
            <tr class="odd">
                <th height="30" align="right">
                    银行余额：
                </th>
                <td bgcolor="#E3F1FC">
                    <input name="txtBankBalance" type="text" class="inputtext" id="txtBankBalance" runat="server"
                        valid="required|isMoney" errmsg="请输入余额|请输入正确的金额" />
                </td>
            </tr>
        </table>
        <table width="320" border="0" align="center" cellpadding="0" cellspacing="0">
            <tr>
                <td height="40" align="center">
                </td>
                <td height="40" align="center" class="tjbtn02">
                    <a href="javascript:;" class="save" id="btn" runat="server">保存</a>
                </td>
                <td height="40" align="center" class="tjbtn02">
                    <a href="javascript:;" id="linkCancel" onclick="parent.Boxy.getIframeDialog('<%=Request.QueryString["iframeId"] %>').hide()">
                        关闭</a>
                </td>
            </tr>
        </table>
        </form>
    </div>

    <script type="text/javascript">
        $(function() {
            BankAddPage.PageInit();
        })
        var BankAddPage = {
            PageInit: function() {
                $("input[readonly='readonly']").css({ "background-color": "#dadada" });
                $("#<%=btn.ClientID %>").click(function() {
                    if (!BankAddPage.CheckForm()) {
                        return false;
                    }
                    var url = "/Fin/BankAdd.aspx?dotype=" + '<%=Request.QueryString["dotype"]%>' + "&type=save&tid=" + '<%=Request.QueryString["tid"]%>';
                    BankAddPage.GoAjax(url);
                    return false;
                });
            },
            GoAjax: function(url) {
                BankAddPage.UnBind();
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
                            BankAddPage.Bind();
                        }
                    },
                    error: function() {
                        tableToolbar._showMsg(tableToolbar.errorMsg);
                        BankAddPage.Bind();
                    }
                });
            },
            CheckForm: function() {
                return ValiDatorForm.validator($("#<%=btn.ClientID %>").closest("form").get(0), "parent");
            },
            Bind: function() {
                var _selfs = $("#<%=this.btn.ClientID %>");
                _selfs.html("保存");
                _selfs.css("cursor", "pointer");
                _selfs.click(function() {
                    if (!BankAddPage.CheckForm()) {
                        return false;
                    }
                    var url = "/Fin/BankAdd.aspx?dotype=" + '<%=Request.QueryString["dotype"]%>' + "&type=save&tid=" + '<%=Request.QueryString["tid"]%>';
                    BankAddPage.GoAjax(url);
                    return false;
                });
            },
            UnBind: function() {
                $("#<%=this.btn.ClientID %>").html("提交中...");
                $("#<%=this.btn.ClientID %>").unbind("click");
            }
        }
    </script>

</asp:Content>
