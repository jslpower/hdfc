<%@ Page Title="编辑角色" Language="C#" AutoEventWireup="true" CodeBehind="RoleEdit.aspx.cs"
    Inherits="Web.SystemSet.RoleEdit" MasterPageFile="~/MasterPage/Boxy.Master" %>

<%@ Register Assembly="ControlLibrary" Namespace="ControlLibrary" TagPrefix="asp" %>
<%@ Register Src="/UserControl/PermitList.ascx" TagName="perList" TagPrefix="uc1" %>
<asp:Content ID="PageHead" runat="server" ContentPlaceHolderID="PageHead">
</asp:Content>
<asp:Content ID="PageBody" runat="server" ContentPlaceHolderID="PageBody">
    <form id="form1" runat="server">
    <input type="hidden" name="hidMethod" id="hidMethod" value="save" />
    <table width="99%" border="0" align="center" cellpadding="0" cellspacing="0">
        <tr>
            <td height="40" align="center" style="font-size: 13px">
                <span style="color: Red; font-size: 12px;">*</span><strong>角色名称：</strong>
                <input type="text" name="txtRoleName" id="txtRoleName" value="<%=roleName %>" onblur="if(this.value==''){$('#rolMess').css('display','');}else{$('#rolMess').css('display','none')}" />
                <span id="rolMess" style="color: Red; display: none; font-size: 12px;">角色不能为空</span>
            </td>
        </tr>
    </table>
    <uc1:perList id="cu_perList" runat="server" />
    <table width="304" border="0" align="center" cellpadding="0" cellspacing="0">
        <tr>
            <td width="152" height="40" align="center" class="tjbtn02">
                <a href="javascript:;" onclick="return RoleEdit.setPer();">保存</a>
            </td>
            <td width="152" height="40" align="center" class="tjbtn02">
                <span class="tjbtn02"><a href="RoleList.aspx">关闭</a></span>
            </td>
        </tr>
    </table>
    </form>

    <script type="text/javascript">

        var RoleEdit =
          {
              //提交设置的权限
              setPer: function() {
                  var roleName = $.trim($('#txtRoleName').val()); //角色名\
                  if (roleName == "管理员" && '<%=EyouSoft.Common.Utils.GetQueryStringValue("method2") %>' != 'update') {
                      alert("管理员角色不能添加!");
                      return false;
                  }
                  if (roleName == "") {
                      $("#rolMess").css("display", ""); $("#txtRoleName").focus();
                      return false;
                  }
                  else {
                      $("#rolMess").css("display", "none");
                  }
                  $("#<%=form1.ClientID %>").get(0).submit();
                  return false;

              },

              changeRole: function(tar) {
                  var roleId = $(tar).val();
                  window.location = "/systemset/orginze/SetPermit.aspx?empId=<%=roleId %>&roleId=" + roleId;
              },
              chkA1: function(tar) {
                  var chkObj = $(tar);
                  var sonClass = "p" + chkObj.attr("class").subString(3);
              },
              chkA2: function(tar) {
              }
          };
        
    </script>

</asp:Content>
