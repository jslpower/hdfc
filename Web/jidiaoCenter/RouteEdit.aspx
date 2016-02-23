<%@ Page Language="C#" MasterPageFile="~/MasterPage/Boxy.Master" AutoEventWireup="true"
    CodeBehind="RouteEdit.aspx.cs" Inherits="Web.jidiaoCenter.RouteEdit" Title="修改线路" %>

<asp:Content ID="Content1" ContentPlaceHolderID="PageHead" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="PageBody" runat="server">
    <form id="form1" runat="server">
    <div style="width: 400px; margin: 10px auto;">
        <table width="100%" cellspacing="1" cellpadding="0" border="0" align="center" style="margin: 10px auto;">
            <tbody>
                <tr class="odd">
                    <th width="120" height="30" align="right">
                        线路名称：
                    </th>
                    <td width="280" bgcolor="#E3F1FC">
                        <asp:TextBox ID="txtRouteName" CssClass="inputtext formsize120" runat="server"></asp:TextBox>
                        <input type="hidden" runat="server" id="hidrouteid" value="" />
                    </td>
                </tr>
            </tbody>
        </table>
        <table width="100%" cellspacing="0" cellpadding="0" border="0" align="center" style="margin: 10px auto;">
            <tbody>
                <tr class="odd">
                    <td height="40" bgcolor="#E3F1FC" colspan="14">
                        <table cellspacing="0" cellpadding="0" border="0" align="center">
                            <tbody>
                                <tr>
                                    <td width="80" align="center" class="tjbtn02">
                                        <a href="javascript:;" id="btnsave" runat="server">保存</a>
                                    </td>
                                </tr>
                            </tbody>
                        </table>
                    </td>
                </tr>
            </tbody>
        </table>
    </div>
    </form>

    <script type="text/javascript">
        $(function() {
            PingGuPage.PageInit();
        })
        var PingGuPage = {
            PageInit: function() {
                $("#<%=btnsave.ClientID %>").unbind("click").click(function() {
                    if ($("#<%=txtRouteName.ClientID %>").val() == "") {
                        parent.tableToolbar._showMsg("请填写线路名称!");
                        $(this).focus();
                        return false;
                    }
                    $(this).html("正在保存");
                    var url = "/jidiaoCenter/RouteEdit.aspx?dotype=save&routeid=" + '<%=Request.QueryString["routeid"] %>';
                    PingGuPage.GoAjax(url);
                })
            },
            GoAjax: function(url) {
                $("#<%=btnsave.ClientID %>").unbind("click");
                $.newAjax({
                    type: "post",
                    cache: false,
                    url: url,
                    dataType: "json",
                    data: $("#<%=form1.ClientID %>").serialize(),
                    success: function(ret) {
                        if (ret.result == "1") {
                            parent.tableToolbar._showMsg(ret.msg, function() { parent.location.href = parent.location.href; });
                        }
                        else {
                            parent.tableToolbar._showMsg(ret.msg);
                            window.location.reload();
                        }
                    },
                    error: function() {
                        tableToolbar._showMsg(tableToolbar.errorMsg);
                    }
                });
            }
        }
    </script>

</asp:Content>
