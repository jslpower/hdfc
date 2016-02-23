<%@ Page Language="C#" MasterPageFile="~/MasterPage/Boxy.Master" AutoEventWireup="true"
    CodeBehind="PingGu.aspx.cs" Inherits="Web.jidiaoCenter.PingGu" Title="质量评估" %>

<asp:Content ID="Content1" ContentPlaceHolderID="PageHead" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="PageBody" runat="server">
    <form runat="server" id="form1">
    <div style="width: 500px; margin: 10px auto;">
        <table width="100%" cellspacing="1" cellpadding="0" border="0" align="center" style="margin: 10px auto;">
            <tbody>
                <tr class="odd">
                    <th width="120" height="30" align="right">
                        评分：
                    </th>
                    <td width="440" bgcolor="#E3F1FC">
                        <asp:DropDownList ID="dptscore" runat="server" CssClass="inputselect">
                           
                        </asp:DropDownList>
                        
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
        $(function(){
            PingGuPage.PageInit();
        })
        var PingGuPage={
            PageInit:function(){
                $("#<%=btnsave.ClientID %>").click(function(){
                    if($("#<%=dptscore.ClientID %>").val()=="-1"){
                        parent.tableToolbar._showMsg("请选择评分!");
                        return false;
                    }
                    $(this).html("正在保存");
                    var url="/jidiaoCenter/PingGu.aspx?dotype=save&tourid="+'<%=Request.QueryString["tourid"] %>';
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
                    data: $("#<%=btnsave.ClientID %>").closest("form").serialize(),
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
            }
        }
    </script>

</asp:Content>
