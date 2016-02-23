<%@ Page Title="登录日志" Language="C#" MasterPageFile="~/MasterPage/Front.Master" AutoEventWireup="true"
    CodeBehind="LoginLog.aspx.cs" Inherits="Web.SystemSet.LoginLog" %>

<%@ Register Assembly="ControlLibrary" Namespace="Adpost.Common.ExporPage" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="mainbody">
        <div class="lineprotitlebox">
            <table border="0" cellpadding="0" cellspacing="0" width="100%">
                <tr>
                    <td nowrap="nowrap" width="15%">
                        <span class="lineprotitle">系统日志</span>
                    </td>
                    <td align="right" nowrap="nowrap" style="padding: 0 10px 2px 0; color: #13509f;"
                        width="85%">
                        <b>当前您所在位置</b>&gt;&gt; 系统设置 &gt;&gt; 系统日志
                    </td>
                </tr>
                <tr>
                    <td bgcolor="#000000" colspan="2" height="2">
                    </td>
                </tr>
            </table>
        </div>
        <div class="lineCategorybox" style="height: 35px;">
            <table border="0" cellpadding="0" cellspacing="0" class="xtnav">
                <tr>
                    <asp:PlaceHolder ID="PlaceHolder1" runat="server">
                        <td align="center" class="xtnav-on" width="100">
                            <a>登录日志</a>
                        </td>
                    </asp:PlaceHolder>
                    <asp:PlaceHolder ID="PlaceHolder2" runat="server">
                        <td align="center" width="100">
                            <a href="OperationLog.aspx">操作日志</a>
                        </td>
                    </asp:PlaceHolder>
                </tr>
            </table>
        </div>
        <table align="center" border="0" cellpadding="0" cellspacing="0" width="99%">
            <tr>
                <td height="30">
                    姓名：
                    <input id="txtName" class="formsize80 searchinput2" name="txtName" size="9" type="text"
                        runat="server" />
                    登录时间：
                    <input id="txtsTime" class="searchinput2" name="txtsTime" size="9" type="text" onfocus="WdatePicker()"
                        runat="server" />
                    -
                    <input id="txteTime" class="searchinput2" name="txteTime" size="9" type="text" onfocus="WdatePicker()"
                        runat="server" />
                    <a id="search" href="javascript:;">
                        <img src="/images/searchbtn.gif" /></a>
                </td>
            </tr>
        </table>
        <div class="tablelist">
            <table border="0" cellpadding="0" cellspacing="1" width="100%">
                <tr>
                    <th align="center" bgcolor="#BDDCF4" width="36">
                        序号
                    </th>
                    <th align="center" bgcolor="#BDDCF4" width="21%">
                        姓名
                    </th>
                    <th align="center" bgcolor="#bddcf4" width="25%">
                        账号
                    </th>
                    <th align="center" bgcolor="#bddcf4">
                        登录时间
                    </th>
                    <th align="center" bgcolor="#bddcf4" width="22%">
                        登录IP
                    </th>
                </tr>
                <asp:Repeater ID="rpt_logList" runat="server">
                    <ItemTemplate>
                        <tr bgcolor="<%# Container.ItemIndex%2==0?"#e3f1fc":"#BDDCF4" %>">
                            <td align="center" height="35">
                                <%# Container.ItemIndex + 1 + (this.pageIndex - 1) * this.pagesize%>
                            </td>
                            <td align="center" height="35">
                                <%#Eval("ContactName")%>
                            </td>
                            <td align="center" height="35">
                                <%#Eval("UserName")%>
                            </td>
                            <td align="center">
                                <%#Eval("LoginTime")%>
                            </td>
                            <td align="center">
                                <%#Eval("LoginIp")%>
                            </td>
                        </tr>
                    </ItemTemplate>
                </asp:Repeater>
                <tr>
                    <td align="right" class="pageup" colspan="5" height="30">
                        <cc1:ExporPageInfoSelect ID="ExporPageInfoSelect1" runat="server" />
                    </td>
                </tr>
            </table>
        </div>
    </div>

    <script type="text/javascript">
        var getserchData = {
            search: function() {
                var txtName = $("#<%=txtName.ClientID %>").val();
                var txtsTime = $("#<%=txtsTime.ClientID %>").val();
                var txteTime = $("#<%=txteTime.ClientID %>").val();
                window.location = "/SystemSet/LoginLog.aspx?txtname=" + encodeURIComponent(txtName) + "&txtstime=" + txtsTime + "&txtetime=" + txteTime;
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
