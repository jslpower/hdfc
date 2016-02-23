<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="PermitList.ascx.cs"
    Inherits="Web.UserControl.ucSystemSet.PermitList" %>
<%@ Register Assembly="ControlLibrary" Namespace="ControlLibrary" TagPrefix="asp" %>

<script type="text/javascript">
    function clChk(pid, tar) {
        var chked = $(tar).attr("checked");
        $("input[cl='" + pid + "']").attr("checked", chked);
    }
    function caChk(pid, tar) {
        var chked = $(tar).attr("checked");
        $("input[ca='" + pid + "']").attr("checked", chked);
    }
</script>

<asp:CustomRepeater ID="rptPerList" runat="server">
    <ItemTemplate>
        <table width="850" border="0" align="center" cellpadding="0" cellspacing="0">
            <tr>
                <td height="28" align="left" valign="top" background="/images/bg001.gif">
                    <table width="840" border="0" align="center" cellpadding="0" cellspacing="0">
                        <tr>
                            <td width="92" height="25" align="center" class="shouquanbg">
                                <%# Eval("Name")%>
                            </td>
                            <td width="748" height="25">
                                <span class="pandl3">
                                    <input type="checkbox" name="checkbox27" onclick="caChk('<%# Eval("MenuId") %>',this)" />
                                </span>全选<span class="pandl3"> </span>
                                <%# Eval("Name")%>菜单
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
        </table>
        <%# GetPermitHtml(Eval("Menu2s"))%>
    </ItemTemplate>
</asp:CustomRepeater>
