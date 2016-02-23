<%@ Page Title="人员管理" Language="C#" MasterPageFile="~/MasterPage/Front.Master" AutoEventWireup="true"
    CodeBehind="UserList.aspx.cs" Inherits="Web.SystemSet.UserManage" %>

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
                            <span class="lineprotitle">组织机构</span>
                        </td>
                        <td width="85%" nowrap="nowrap" align="right" style="padding: 0 10px 2px 0; color: #13509f;">
                            <b>当前您所在位置</b>&gt;&gt; <a href="#">系统设置</a>&gt;&gt; 组织机构
                        </td>
                    </tr>
                    <tr>
                        <td height="2" bgcolor="#000000" colspan="2">
                        </td>
                    </tr>
                </tbody>
            </table>
        </div>
        <div class="lineCategorybox" style="height: 50px;">
            <table border="0" cellpadding="0" cellspacing="0" class="xtnav">
                <tr>
                    <asp:PlaceHolder ID="PlaceHolder1" runat="server">
                        <td width="100" align="center">
                            <a href="/SystemSet/DepartManage.aspx">部门名称</a>
                        </td>
                    </asp:PlaceHolder>
                    <td width="100" align="center" class="xtnav-on">
                        <a>部门人员</a>
                    </td>
                </tr>
            </table>
        </div>
        <form id="form1" method="get">
        <table width="99%" cellspacing="0" cellpadding="0" border="0" align="center">
            <tbody>
                <tr>
                    <td height="30">
                        姓名：
                        <input type="text" id="txtContactName" class="searchinput inputtext" name="txtContactName"
                            style="width: 150px;" value='<%=Request.QueryString["txtContactName"] %>' />
                        用户名：
                        <input type="text" id="txtUserName" class="searchinput inputtext" name="txtUserName"
                            style="width: 150px;" value='<%=Request.QueryString["txtUserName"] %>' />
                        <input type="submit" value="" class="search-btn" />
                    </td>
                </tr>
            </tbody>
        </table>
        </form>
        <div class="btnbox">
            <table border="0" align="left" cellpadding="0" cellspacing="0">
                <tr>
                    <td width="90" align="center">
                        <a href="javascript;" onclick="return DepartEmp.add();">新 增</a>
                    </td>
                    <td width="90" align="center">
                        <a href="javascript:;" onclick="return DepartEmp.update();">修 改</a>
                    </td>
                    <td width="90" align="center">
                        <a href="javascript:;" onclick="return DepartEmp.del('');">删 除</a>
                    </td>
                </tr>
            </table>
        </div>
        <div class="tablelist">
            <table width="100%" border="0" cellpadding="0" cellspacing="1" id="liststyle">
                <tr>
                    <th width="9%" align="center" bgcolor="#BDDCF4">
                        全选
                        <input type="checkbox" name="checkbox" id="chkAll" onclick="DepartEmp.checkAll(this);" />
                    </th>
                    <th width="15%" align="center" bgcolor="#BDDCF4">
                        所属部门
                    </th>
                    <th width="9%" align="center" bgcolor="#bddcf4">
                        姓名
                    </th>
                    <th width="10%" align="center" bgcolor="#bddcf4">
                        用户名
                    </th>
                    <th width="8%" align="center" bgcolor="#bddcf4">
                        性别
                    </th>
                    <th width="13%" align="center" bgcolor="#bddcf4">
                        电话
                    </th>
                    <th width="10%" align="center" bgcolor="#bddcf4">
                        手机
                    </th>
                    <th width="10%" align="center" bgcolor="#bddcf4">
                        QQ
                    </th>
                    <th width="8%" align="center" bgcolor="#bddcf4">
                        状态
                    </th>
                    <th width="8%" align="center" bgcolor="#bddcf4">
                        授权
                    </th>
                </tr>
                <asp:Repeater ID="rptEmployee" runat="server">
                    <ItemTemplate>
                        <tr class="<%#Container.ItemIndex%2==0 ? "even":"odd" %>">
                            <td height="35" align="center">
                                <%--checkbox isAllowDelete 是否允许删除 0:不可删除--%>
                                <%#(bool)Eval("IsAdmin") == true ? "<input type=\"checkbox\" isAllowDelete=\"0\" class=\"c1\" value='" + Eval("Id") + "'/>" : "<input type=\"checkbox\"  isAllowDelete=\"1\" class=\"c1\" value='" + Eval("Id") + "'/>"%>
                            </td>
                            <td height="35" align="center">
                                <%# Eval("DepartName") %>
                            </td>
                            <td height="35" align="center">
                                <%# ((EyouSoft.Model.CompanyStructure.ContactPersonInfo)Eval("PersonInfo")).ContactName%>
                            </td>
                            <td height="35" align="center">
                                <%#Eval("UserName")%>
                            </td>
                            <td height="35" align="center">
                                <%# ((EyouSoft.Model.CompanyStructure.ContactPersonInfo)Eval("PersonInfo")).ContactSex%>
                            </td>
                            <td align="center">
                                <%# ((EyouSoft.Model.CompanyStructure.ContactPersonInfo)Eval("PersonInfo")).ContactTel%>
                            </td>
                            <td align="center">
                                <%# ((EyouSoft.Model.CompanyStructure.ContactPersonInfo)Eval("PersonInfo")).ContactMobile%>
                            </td>
                            <td align="center">
                                <%# ((EyouSoft.Model.CompanyStructure.ContactPersonInfo)Eval("PersonInfo")).QQ%>
                            </td>
                            <td align="center">
                                <a href="javascript:;" onclick="return DepartEmp.setState('<%# Eval("Id") %>',this)">
                                    <%#((EyouSoft.Model.EnumType.CompanyStructure.UserStatus)Eval("UserStatus"))== EyouSoft.Model.EnumType.CompanyStructure.UserStatus .正常 ? "√" : "×"%></a>
                            </td>
                            <td align="center">
                                <a href="javascript:;" target="_blank" onclick="return DepartEmp.setPermit('<%# Eval("ID") %>')">
                                    授权</a>
                            </td>
                        </tr>
                    </ItemTemplate>
                </asp:Repeater>
                <asp:Literal ID="lbemptymsg" runat="server"></asp:Literal>
                <tr>
                    <td height="30" colspan="10" align="right" class="pageup">
                        <cc1:ExporPageInfoSelect ID="ExporPageInfoSelect1" runat="server" />
                    </td>
                </tr>
            </table>
        </div>
    </div>

    <script type="text/javascript">
        var DepartEmp =
                {
                    //打开弹窗
                    openDialog: function(p_url, p_title, p_width, p_height) {
                        Boxy.iframeDialog({ title: p_title, iframeUrl: p_url, width: p_width, height: p_height });
                    },
                    //修改员工
                    update: function() {
                        var chks = $("input:checked").not("#chkAll")
                        var eId = "";
                        if (chks.length != 1) {
                            tableToolbar._showMsg("请选择一位员工");
                            return false;
                        }
                        else
                            eId = chks.val();
                        DepartEmp.openDialog("/SystemSet/UserAdd.aspx?empId=" + eId, "修改员工", "800px", "480px");
                        return false;
                    },
                    //添加员工
                    add: function() {
                        DepartEmp.openDialog("/SystemSet/UserAdd.aspx", "新增员工", "800px", "480px");
                        return false;
                    },
                    del: function() {
                        var ids = "";
                        $(".c1:checked").each(function() {
                            if ($(this).attr("isallowdelete") == "0") {
                                tableToolbar._showMsg("管理员账号不能删除！");
                                $(this).attr("checked", false);
                                return true;
                            }
                            else{
                                ids += $(this).val() + ",";
                            }
                        });
                        if (ids != "") {
                            tableToolbar.ShowConfirmMsg("你确定要删除所选部门人员吗？",function(){
                                window.location = "/SystemSet/UserList.aspx?method=del&ids=" + ids;
                            })
                            
                        }
                        return false;
                    },
                    //授权
                    setPermit: function(eId) {
                        DepartEmp.openDialog("/SystemSet/SetPermit.aspx?empId=" + eId, "授权", "900px", "500px");
                        return false;
                    },
                    //设置状态
                    setState: function(sid, tar) {
                        var sonFont = $(tar);
                        var nowM = ""; //当前操作
                        if (sonFont.html() == "×") {
                            nowM = "start";
                        }
                        else {
                            nowM = "stop";
                        }
                        $.newAjax({
                            type: "get",
                            dataType: "json",
                            url: "UserList.aspx?method=setState",
                            data: { hidMethod: nowM, ids: sid },
                            cache: false,
                            success: function(result) {
                                if (result.success == "1") {
                                    if (nowM == "stop") {
                                       tableToolbar._showMsg("已关闭");
                                        sonFont.html("×");
                                    }
                                    else {
                                        tableToolbar._showMsg("已开启");
                                        sonFont.html("√");
                                    }
                                }
                                else {
                                    tableToolbar._showMsg("设置失败！");
                                }
                            }
                        });
                    },
                    checkAll: function(chk) {
                        var chked = $(chk).attr("checked");
                        $(".c1:checkbox").attr("checked", chked);
                    }

                }
    </script>

</asp:Content>
