<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="self.aspx.cs" Inherits="Web.Webmaster.self" MasterPageFile="~/Webmaster/mpage.Master" %>

<%@ MasterType VirtualPath="~/Webmaster/mpage.Master" %>
<asp:Content runat="server" ContentPlaceHolderID="Scripts" ID="ScriptsContent">
    
</asp:Content>
<asp:Content runat="server" ContentPlaceHolderID="PageTitle" ID="TitleContent">
    我的信息管理
</asp:Content>
<asp:Content runat="server" ContentPlaceHolderID="PageContent" ID="MainContent">
    <table cellpadding="2" cellspacing="1" style="font-size: 12px; width: 100%;">
        <tr>
            <td>
                用户编号：<asp:Literal runat="server" ID="ltrUserId"></asp:Literal>
            </td>
        </tr>
        <tr>
            <td>
                登录账号：<asp:Literal runat="server" ID="ltrUsername"></asp:Literal>
            </td>
        </tr>
        <tr>
            <td>
                登录密码：<input id="t_p" class="input_text" name="t_p" type="password" maxlength="20" style="width:200px" /><span class="note">密码为空时不做修改</span>
            </td>
        </tr>
        <tr class="trspace">
            <td></td>
        </tr>        
        <tr>
            <td>
                <asp:Button ID="btnUpdate" runat="server" Text="修改登录密码" OnClick="btnUpdate_Click" />
            </td>
        </tr>
        <tr class="trspace">
            <td>
            </td>
        </tr>
        <tr>
            <td>&nbsp;</td>
        </tr>
        <tr>
            <td>&nbsp;</td>
        </tr>
        <tr>
            <td>
                <input id="t_s" class="input_text" name="t_s" type="text" maxlength="20" style="width: 200px" />
                <asp:Button ID="btnMD5Encrypt" runat="server" Text="查看MD5值" OnClick="btnMD5Encrypt_Click" />
            </td>
        </tr>
        <tr>
            <td>
                &nbsp;
            </td>
        </tr>
        <tr>
            <td>
                <input id="t_cache_name" class="input_text" name="t_cache_name" type="text" maxlength="20"
                    style="width: 200px" />
                <asp:Button ID="btnClearCache" runat="server" Text="清除缓存" OnClick="btnRemoveCache_Click" />
            </td>
        </tr>
    </table>
</asp:Content>
<asp:Content runat="server" ContentPlaceHolderID="PageRemark" ID="RemarkContent">
</asp:Content>
