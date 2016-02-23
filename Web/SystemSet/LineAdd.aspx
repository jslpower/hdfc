<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="LineAdd.aspx.cs" Inherits="Web.SystemSet.LineAdd"
    MasterPageFile="~/MasterPage/Boxy.Master" %>

<asp:Content ID="Content1" runat="server" ContentPlaceHolderID="PageHead">
</asp:Content>
<asp:Content ID="Content2" runat="server" ContentPlaceHolderID="PageBody">
    <form id="form1" runat="server">
    <input type="hidden" name="hidMethod" id="hidMethod" value="save" />
    <table width="520" cellspacing="1" cellpadding="0" border="0" align="center" style="margin: 20px auto;">
        <tbody>
            <tr class="odd">
                <th width="16%" height="30" align="right">
                    区域名称：
                </th>
                <td width="84%" bgcolor="#E3F1FC" class="pandl3">
                    <asp:TextBox CssClass="inputtext formsize100" ID="txtAreaName" name="txtAreaName"
                        runat="server"></asp:TextBox>
                    <span id="tip" class="errmsg" style="display: none">*请填写区域名称</span>
                </td>
            </tr>
            <tr class="odd">
                <td height="30" bgcolor="#E3F1FC" align="left" colspan="2">
                    <table width="248" cellspacing="0" cellpadding="0" border="0" align="center">
                        <tbody>
                            <tr>
                                <td width="96" height="40" align="center" class="tjbtn02">
                                    <a href="javascript:;" id="btn" hidefocus="true" runat="server">保存</a>
                                </td>
                            </tr>
                        </tbody>
                    </table>
                </td>
            </tr>
        </tbody>
    </table>
    </form>

    <script type="text/javascript">
        $(function() {
            LineAdd.BindBtn();
        })
        var LineAdd = {
            GoAjax: function(url) {
                $("#<%=btn.ClientID %>").html("提交中...");
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
                            parent.tableToolbar._showMsg(ret.msg, function() { LineAdd.BindBtn() });
                        }
                    },
                    error: function() {
                        tableToolbar._showMsg(tableToolbar.errorMsg, function() { LineAdd.BindBtn() });
                    }
                });
            },
            BindBtn: function() {
                //绑定Add事件
                $("#<%=btn.ClientID %>").click(function() {
                    if ($("#<%=txtAreaName.ClientID %>").val() == "") {
                        $("#tip").show();
                        return false;
                    }
                    var ajaxUrl = "/SystemSet/LineAdd.aspx?type=save&dotype=update&areaid=" + '<%=Request.QueryString["areaid"]%>';
                    LineAdd.GoAjax(ajaxUrl);
                    return false;
                })
                $("#<%=txtAreaName.ClientID %>").focus(function() {
                    $("#tip").hide();
                })
                $("#<%=btn.ClientID %>").html("保存");
            }

        }
   
    </script>

</asp:Content>
