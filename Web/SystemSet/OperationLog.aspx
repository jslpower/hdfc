<%@ Page Title="操作日志" Language="C#" MasterPageFile="~/MasterPage/Front.Master" AutoEventWireup="true"
    CodeBehind="OperationLog.aspx.cs" Inherits="Web.SystemSet.OperationLog" %>

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
                            <span class="lineprotitle">系统日志</span>
                        </td>
                        <td width="85%" nowrap="nowrap" align="right" style="padding: 0 10px 2px 0; color: #13509f;">
                           <b>当前您所在位置</b>&gt;&gt; 系统设置 &gt;&gt; 系统日志
                        </td>
                    </tr>
                    <tr>
                        <td height="2" bgcolor="#000000" colspan="2">
                        </td>
                    </tr>
                </tbody>
            </table>
        </div>
        <div style="height: 35px;" class="lineCategorybox">
            <table cellspacing="0" cellpadding="0" border="0" class="xtnav">
                <tbody>
                    <tr>
                        <asp:PlaceHolder ID="PlaceHolder1" runat="server">
                            <td width="100" align="center">
                                <a href="LoginLog.aspx">登录日志</a>
                            </td>
                        </asp:PlaceHolder>
                        <td width="100" align="center" class="xtnav-on">
                            <a>操作日志</a>
                        </td>
                    </tr>
                </tbody>
            </table>
        </div>
        <table width="99%" cellspacing="0" cellpadding="0" border="0" align="center">
            <tbody>
                <tr>
                    <td height="30">
                        操作人：
                        <input type="text" size="9" id="txtName" class="formsize80 searchinput2" name="txtName"
                            runat="server">
                        操作时间：
                        <input type="text" size="9" id="txtsTime" class="searchinput2" name="txtsTime" onfocus="WdatePicker()"
                            runat="server">
                        -
                        <input type="text" size="9" id="txteTime" class="searchinput2" name="txtsTime" onfocus="WdatePicker()"
                            runat="server">
                        <a id="search" href="javascript:;">
                            <img style="vertical-align: top;" src="../images/searchbtn.gif"></a>
                    </td>
                </tr>
            </tbody>
        </table>
        <div class="tablelist">
            <table width="100%" cellspacing="1" cellpadding="0" border="0">
                <tbody>
                    <tr>
                        <th width="36" height="30" bgcolor="#BDDCF4" align="center">
                            编号
                        </th>
                        <th width="11%" bgcolor="#BDDCF4" align="center">
                            操作人
                        </th>
                        <th width="15%" bgcolor="#bddcf4" align="center">
                            操作模块
                        </th>
                        <th bgcolor="#bddcf4" align="center">
                            操作内容
                        </th>
                        <th width="17%" bgcolor="#bddcf4" align="center">
                            操作时间
                        </th>
                        <th width="16%" bgcolor="#bddcf4" align="center">
                            IP
                        </th>
                    </tr>
                    <asp:Repeater ID="rpt_logList" runat="server">
                        <ItemTemplate>
                            <tr bgcolor="<%# Container.ItemIndex%2==0?"#e3f1fc":"#BDDCF4" %>">
                                <td align="center" height="35">
                                    <%# Container.ItemIndex + 1 + (this.pageIndex - 1) * this.pagesize%>
                                </td>
                                <td height="35" align="center">
                                    <%#Eval("OperatorName") %>
                                </td>
                                <td height="35" align="center">
                                    <%# ((EyouSoft.Model.EnumType.PrivsStructure.Privs2)Eval("ModuleId")).ToString() %>
                                </td>
                                <td align="center">
                                    <%#Eval("EventMessage")%>
                                </td>
                                <td align="center">
                                    <%#Eval("EventTime")%>
                                </td>
                                <td align="center">
                                    <%#Eval("EventIp")%>
                                </td>
                            </tr>
                        </ItemTemplate>
                    </asp:Repeater>
                    <tr>
                        <td height="30" align="right" class="pageup" colspan="6">
                            <cc1:ExporPageInfoSelect ID="ExporPageInfoSelect1" runat="server" />
                        </td>
                    </tr>
                </tbody>
            </table>
        </div>
    </div>

    <script type="text/javascript">
        var getserchData = {
            search: function() {
                var txtName = $("#<%=txtName.ClientID %>").val();
                var txtsTime = $("#<%=txtsTime.ClientID %>").val();
                var txteTime = $("#<%=txteTime.ClientID %>").val();
                window.location = "/SystemSet/OperationLog.aspx?txtname=" + encodeURIComponent(txtName) + "&txtstime=" + txtsTime + "&txtetime=" + txteTime;
            }
        };
        $(function() {
            $("#search").click(function() {
                getserchData.search();
                return false;
            })
        });
    </script>

</asp:Content>
