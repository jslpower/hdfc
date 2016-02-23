<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="systemadd.aspx.cs" Inherits="Web.Webmaster._systemadd"
    MasterPageFile="~/Webmaster/mpage.Master" %>

<%@ MasterType VirtualPath="~/Webmaster/mpage.Master" %>
<asp:Content runat="server" ContentPlaceHolderID="Scripts" ID="ScriptsContent">

    <script type="text/javascript">
        function WebForm_OnSubmit_Validate() {
            if ($.trim($("#txtSysName").val()).length == 0) { alert("请输入系统名称"); return false; }
            if ($.trim($("#txtFullname").val()).length == 0) { alert("请输入联系姓名"); return false; }
            if ($.trim($("#txtUsername").val()).length == 0) { alert("请输入登录账号"); return false; }
            if ($.trim($("#txtPassword").val()).length == 0) { alert("请输入登录密码"); return false; }
            return true;
        }
    </script>

</asp:Content>
<asp:Content runat="server" ContentPlaceHolderID="PageTitle" ID="TitleContent">
    添加子系统
</asp:Content>
<asp:Content runat="server" ContentPlaceHolderID="PageContent" ID="MainContent">
    <table cellpadding="2" cellspacing="1" style="font-size: 12px; width: 100%;">
        <tr>
            <td>
                <span class="required">*</span>系统名称：<input type="text" id="txtSysName" name="txtSysName"
                    class="input_text" maxlength="72" style="width: 520px" />
            </td>
        </tr>
        <tr class="trspace">
            <td>
            </td>
        </tr>
        <tr>
            <td>
                <span class="required">*</span>联系姓名：<input type="text" id="txtFullname" name="txtFullname"
                    class="input_text" maxlength="72" style="width: 520px" />
            </td>
        </tr>
        <tr>
            <td>
                <span class="unrequired">*</span>联系电话：<input type="text" id="txtTelephone" name="txtTelephone"
                    class="input_text" maxlength="72" style="width: 520px" />
            </td>
        </tr>
        <tr>
            <td>
                <span class="unrequired">*</span>联系手机：<input type="text" id="txtMobile" name="txtMobile"
                    class="input_text" maxlength="72" style="width: 520px" />
            </td>
        </tr>
        <tr class="trspace">
            <td>
            </td>
        </tr>
        <tr>
            <td>
                <span class="required">*</span>登录账号：<input type="text" id="txtUsername" name="txtUsername"
                    class="input_text" maxlength="72" style="width: 200px" value="admin" /><span class="note">系统管理员账号</span>
            </td>
        </tr>
        <tr>
            <td>
                <span class="required">*</span>登录密码：<input type="password" id="txtPassword" name="txtPassword"
                    class="input_text" maxlength="72" style="width: 200px" />
            </td>
        </tr>
        <tr class="trspace">
            <td>
            </td>
        </tr>
        <tr>
            <td>
                公司Log：<input type="file" id="fileUpLoad" name="fileUpLoad" />
            </td>
        </tr>
        <tr>
            <td>
                最大子账号数量：<input type="text" id="txtSonNum" name="txtSonNum" class="input_text" maxlength="72" style="width: 200px" />个，除系统自动添加的管理员外！
            </td>
        </tr>
        <tr>
            <td>
                生日提醒提前天数：<input type="text" id="txtDay" name="txtDay" class="input_text" maxlength="72" style="width: 200px" />天
            </td>
        </tr>
        <tr class="trspace">
            <td>
            </td>
        </tr>
        <tr>
            <td>
                <asp:Button ID="btnCreate" runat="server" Text="添加子系统" OnClick="btnCreate_Click"
                    OnClientClick="return WebForm_OnSubmit_Validate()" />
            </td>
        </tr>
    </table>
</asp:Content>
<asp:Content runat="server" ContentPlaceHolderID="PageRemark" ID="RemarkContent">
    <ul class="decimal">
        <li>标记<span class="required">*</span>为必填项。</li>
        <li>公司及管理员账号的联系人信息均使用以上的联系人信息。</li>
        <li>子系统默认角色：管理员。</li>
        <li>子系统默认公司部门：总部。</li>
        <li>子系统公司名称默认为系统名称。</li>
    </ul>
</asp:Content>
