<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="system.aspx.cs" Inherits="Web.Webmaster._system" MasterPageFile="~/Webmaster/mpage.Master" %>

<%@ MasterType VirtualPath="~/Webmaster/mpage.Master" %>
<asp:Content runat="server" ContentPlaceHolderID="Scripts" ID="ScriptsContent">
    
</asp:Content>
<asp:Content runat="server" ContentPlaceHolderID="PageTitle" ID="TitleContent">
    查看子系统信息-<asp:Literal runat="server" ID="ltrSysName1"></asp:Literal>
</asp:Content>
<asp:Content runat="server" ContentPlaceHolderID="PageContent" ID="MainContent">
    <table cellpadding="2" cellspacing="1" style="font-size: 12px; width: 100%;">
        <tr>
            <td>
                系统编号：<asp:Literal runat="server" ID="ltrSysId"></asp:Literal>
            </td>
        </tr>
        <tr>
            <td>
                公司编号：<asp:Literal runat="server" ID="ltrCompanyId"></asp:Literal>
            </td>
        </tr>        
        <tr>
            <td>
                系统名称：<asp:Literal runat="server" ID="ltrSysName2"></asp:Literal>
            </td>
        </tr>
        <tr>
            <td>
                创建时间：<asp:Literal runat="server" ID="ltrIssueTime"></asp:Literal>
            </td>
        </tr>
        <tr class="trspace">
            <td></td>
        </tr>
        <tr>
            <td>
                联系姓名：<asp:Literal runat="server" ID="ltrFullname"></asp:Literal>
            </td>
        </tr>
        <tr>
            <td>
                联系电话：<asp:Literal runat="server" ID="ltrTelephone"></asp:Literal>
            </td>
        </tr>
        <tr>
            <td>
                联系手机：<asp:Literal runat="server" ID="ltrMobile"></asp:Literal>
            </td>
        </tr>
        <tr class="trspace">
            <td></td>
        </tr>
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
                登录密码：<asp:Literal runat="server" ID="ltrPassword"></asp:Literal>
            </td>
        </tr>
        <tr class="trspace">
            <td>
            </td>
        </tr> 
        <tr>
            <td>
                公司Log：<input type="file" id="fileUpLoad" name="fileUpLoad" /><asp:Literal runat="server" ID="ltrCompanyLog"></asp:Literal>
                <asp:HiddenField runat="server" ID="hidOldFile" />
            </td>
        </tr> 
        <tr>
            <td>
                最大子账号数量：<input type="text" runat="server" id="txtSonNum" name="txtSonNum" class="input_text" maxlength="72" style="width: 200px" />个，除系统自动添加的管理员外！
            </td>
        </tr> 
        <tr>
            <td>
                生日提醒提前天数：<input type="text" runat="server" id="txtDay" name="txtDay" class="input_text" maxlength="72" style="width: 200px" />天
            </td>
        </tr> 
        <tr class="trspace">
            <td>
            </td>
        </tr> 
        <tr>
            <td>
                <asp:HiddenField runat="server" ID="hidCompanyId" />
                <asp:Button ID="btnCreate" runat="server" Text="修改子系统配置" OnClick="btnCreate_Click" />
            </td>
        </tr>     
    </table>
</asp:Content>
<asp:Content runat="server" ContentPlaceHolderID="PageRemark" ID="RemarkContent">

</asp:Content>
