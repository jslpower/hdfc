<%@ Page Title="角色管理-系统设置" Language="C#" MasterPageFile="~/MasterPage/Front.Master" AutoEventWireup="true"
    CodeBehind="RoleList.aspx.cs" Inherits="Web.SystemSet.RoleList" %>

<%@ Register Assembly="ControlLibrary" Namespace="ControlLibrary" TagPrefix="cc2" %>
<%@ Register Assembly="ControlLibrary" Namespace="Adpost.Common.ExporPage" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="mainbody">
        <div class="lineprotitlebox">
            <table width="100%" cellspacing="0" cellpadding="0" border="0">
                <tbody>
                    <tr>
                        <td width="15%" nowrap="nowrap">
                            <span class="lineprotitle">角色管理</span>
                        </td>
                        <td width="85%" nowrap="nowrap" align="right" style="padding: 0 10px 2px 0; color: #13509f;">
                            <b>当前您所在位置</b>&gt;&gt; 系统设置 &gt;&gt; 角色管理
                        </td>
                    </tr>
                    <tr>
                        <td height="2" bgcolor="#000000" colspan="2">
                        </td>
                    </tr>
                </tbody>
            </table>
        </div>
        <div style="height: 30px;" class="lineCategorybox">
        </div>
        <div class="btnbox">
            <table cellspacing="0" cellpadding="0" border="0" align="left">
                <tbody>
                    <tr>
                        <td width="90" align="center">
                            <a id="add_menu" href="javascript:;">新 增</a>
                        </td>
                    </tr>
                </tbody>
            </table>
        </div>
        <div class="tablelist">
            <table width="100%" cellspacing="1" cellpadding="0" border="0">
                <tbody>
                    <tr>
                        <th width="36" bgcolor="#BDDCF4" align="center">
                            序号
                        </th>
                        <th bgcolor="#BDDCF4" align="center">
                            角色名称
                        </th>
                        <th width="10%" bgcolor="#bddcf4" align="center">
                            操作
                        </th>
                        <th width="8%" bgcolor="#bddcf4" align="center">
                            序号
                        </th>
                        <th bgcolor="#bddcf4" align="center">
                            角色名称
                        </th>
                        <th width="10%" bgcolor="#bddcf4" align="center">
                            操作
                        </th>
                    </tr>
                    <cc2:CustomRepeater ID="rptRoles" runat="server">
                        <ItemTemplate>
                            <%# GetListTr()%>
                            <td align="center">
                                <%# itemIndex2++%>
                            </td>
                            <td align="center">
                                <%# Eval("RoleName") %>
                            </td>
                            <td align="center">
                                <%#Eval("RoleName").ToString() != "管理员" ? "<a href=\"javascript:;\" onclick=\"return roleManage.update('" + Eval("Id") + "');\">修改</a>" : ""%>
                                <%#Eval("RoleName").ToString() != "管理员" ? "|  <a href=\"javascript:;\" onclick=\"return roleManage.del('" + Eval("Id") + "');\">删除</a>" : ""%>
                            </td>
                        </ItemTemplate>
                    </cc2:CustomRepeater>
                    <tr>
                        <td height="30" align="right" class="pageup" colspan="7">
                            <cc1:ExporPageInfoSelect ID="ExporPageInfoSelect1" runat="server" />
                        </td>
                    </tr>
                </tbody>
            </table>
        </div>
    </div>

    <script type="text/javascript">
        var roleManage = {
            del: function(theId) {
                var mess = "你确定要删除该角色吗？";
                var ids = "";
                if (theId) {
                    ids = theId;
                }
                else {
                    var idArr = [];
                    $(".c1:checked").each(function() {
                        idArr.push($(this).val());
                    });
                    ids = idArr.toString();
                    mess = "你确定要删除所选角色吗？";
                }
                if (ids != "") {
                    if (confirm(mess)) {
                        window.location = "/SystemSet/RoleList.aspx?dotype=del&roleIds=" + ids;
                    }
                }
                else {
                    alert("请选择要删除的角色！");
                }
                return false;
            },
            update: function(id) {
                window.location = "/SystemSet/RoleEdit.aspx?method2=update&roleId=" + id;
            }
        };

        $(function() {
            $("#add_menu").click(function() {
                window.location = "RoleEdit.aspx";
            })
        })
    </script>

</asp:Content>
