<%@ Page Title="用户授权" Language="C#" MasterPageFile="~/MasterPage/Boxy.Master" AutoEventWireup="true"
    CodeBehind="SetPermit.aspx.cs" Inherits="Web.SystemSet.SetPermit" %>

<%@ Register Src="/UserControl/PermitList.ascx" TagName="PermitList" TagPrefix="uc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="PageHead" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="PageBody" runat="server">
    <form id="form1" runat="server">
    <div>
        <table width="99%" border="0" align="center" cellpadding="0" cellspacing="0">
            <tr>
                <td height="40" align="center" style="font-size: 14px">
                    <strong>请选择角色:</strong>
                    <select id="selRole" runat="server">
                    </select>
                </td>
            </tr>
        </table>
        <uc1:PermitList ID="cu_perList" runat="server" />
        &nbsp;<table width="304" border="0" align="center" cellpadding="0" cellspacing="0">
            <tr>
                <td width="152" height="40" align="center" class="tjbtn02">
                    <a href="javascript:;" onclick="return SepPermit.setPer();return false;">授权</a>
                </td>
                <td width="152" height="40" align="center" class="tjbtn02">
                    <span class="tjbtn02"><a href="javascript:;" onclick="window.parent.Boxy.getIframeDialog('<%=Request.QueryString["iframeId"]%>').hide();return false;">
                        关闭</a></span>
                </td>
            </tr>
        </table>
    </div>
    </form>

    <script type="text/javascript">

        var SepPermit =
      {
          //提交设置的权限
          setPer: function() {
              var perArr = [];
              $("input[name='perItem']:checked").each(function() {
                  perArr.push($(this).val());
              })//获取选中权限
              var permitIds = perArr.toString();
              $.newAjax({
                  type: "post",
                  cache: false,
                  data: { perIds: permitIds, roleId: $("#<%=selRole.ClientID %>").val() },
                  url: "/SystemSet/SetPermit.aspx?empId=<%=empId %>&method=setPermit",
                  dataType: "json",
                  success: function(ret) {
                      //ajax回发提示
                      if (ret.result == "1") {
                          tableToolbar._showMsg(ret.msg, function() { parent.location.href = parent.location.href; });
                      }
                      else {
                          tableToolbar._showMsg(ret.msg, function() { parent.location.href = parent.location.href; });
                      }
                  },
                  error: function() {
                      tableToolbar._showMsg(tableToolbar.errorMsg);
                  }
              });
              return false;
          },
          changeRole: function(tar) {
              var roleId = $(tar).val();
              window.location = '/SystemSet/SetPermit.aspx?iframeId=<%=Request.QueryString["iframeId"] %>&method=getPermit&empId=<%=empId %>&roleId=' + roleId;
          },
          chkA1: function(tar) {
              var chkObj = $(tar);
              var sonClass = "p" + chkObj.attr("class").subString(3);
          },
          chkA2: function(tar) {
          }
      }
        
    </script>

</asp:Content>
