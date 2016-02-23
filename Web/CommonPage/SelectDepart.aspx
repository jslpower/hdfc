<%@ Page Title="选择部门" Language="C#" AutoEventWireup="true" CodeBehind="SelectDepart.aspx.cs"
    Inherits="Web.CommonPage.SelectDepart" MasterPageFile="~/MasterPage/Boxy.Master" %>

<%@ Register Assembly="ControlLibrary" Namespace="ControlLibrary" TagPrefix="asp" %>
<asp:Content ID="PageHead" runat="server" ContentPlaceHolderID="PageHead">
</asp:Content>
<asp:Content ID="PageBody" runat="server" ContentPlaceHolderID="PageBody">
    <form runat="server" id="departFrom">
    <table width="750" border="0" align="center" cellpadding="0" cellspacing="1" style="margin: 20px auto;">
        <tr class="odd">
            <th colspan="5" bgcolor="#BDDCF4">
                请选择部门
            </th>
        </tr>
        <asp:CustomRepeater runat="server" ID="rptDepart">
            <ItemTemplate>
                <%# GetListTr()%>
                <td width="25%" height="28" class="pandl3">
                    <input type="checkbox" id="ckbDepart<%# Container.ItemIndex %>" name="txtDepartInfo"
                        value='<%# Eval("Id") %>' <%# GetDepartChecked((int)Eval("Id"))%> />
                    <label for="ckbDepart<%# Container.ItemIndex %>">
                        <%# Eval("DepartName") %></label>
                </td>
            </ItemTemplate>
        </asp:CustomRepeater>
        <%=GetLastTr() %>
        <tr class="odd">
            <td height="30" colspan="17" align="center" bgcolor="#BDDCF4">
                <table width="340" border="0" align="center" cellpadding="0" cellspacing="0">
                    <tr>
                        <td height="40" align="center" class="jixusave">
                            <a href="javascript:;" onclick="return selectDepart();">选择部门</a>
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
    </table>
    </form>

    <script type="text/javascript">
        //选择部门写入父窗口的隐藏域
        function selectDepart() {
            var selDeparts = [];
            $(":checked").each(function() {
                selDeparts.push($(this).val());
            });
            window.parent.InfoEdit.selDepartBack(selDeparts.toString());
            window.parent.Boxy.getIframeDialog('<%=EyouSoft.Common.Utils.GetQueryStringValue("iframeId") %>').hide();
            return false;
        }
    </script>

</asp:Content>
