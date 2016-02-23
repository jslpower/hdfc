<%@ Page Title="线路区域管理" Language="C#" MasterPageFile="~/MasterPage/Front.Master"
    AutoEventWireup="true" CodeBehind="LineManage.aspx.cs" Inherits="Web.SystemSet.LineManage" %>

<%@ Register Assembly="ControlLibrary" Namespace="Adpost.Common.ExporPage" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="lineprotitlebox">
        <table width="100%" cellspacing="0" cellpadding="0" border="0">
            <tbody>
                <tr>
                    <td width="15%" nowrap="nowrap">
                        <span class="lineprotitle">线路区域管理</span>
                    </td>
                    <td width="85%" nowrap="nowrap" align="right" style="padding: 0 10px 2px 0; color: #13509f;">
                        所在位置&gt;&gt; <a href="#">系统设置</a>&gt;&gt; 线路区域管理
                    </td>
                </tr>
                <tr>
                    <td height="2" bgcolor="#000000" colspan="2">
                    </td>
                </tr>
            </tbody>
        </table>
    </div>
    <div style="height: 50px;" class="lineCategorybox">
        <table cellspacing="0" cellpadding="0" border="0" class="xtnav">
            <tbody>
                <tr>
                    <td width="100" align="center">
                        <a href="/SystemSet/CityManage.aspx">城市管理</a>
                    </td>
                    <td width="100" align="center" class="xtnav-on">
                        <a>线路区域管理</a>
                    </td>
                    <td width="100" align="center">
                        <a href="/SystemSet/YinHangZhangHu.aspx">公司账户</a>
                    </td>
                    <td width="100" align="center">
                        <a href="/SystemSet/SaleArea.aspx">销售地区管理</a>
                    </td>
                    <td width="100" align="center">
                        <a href="/SystemSet/CarFlight.aspx">车次航班管理</a>
                    </td>
                    <td width="100" align="center">
                        <a href="/SystemSet/Rating.aspx">信用评级管理</a>
                    </td>
                </tr>
            </tbody>
        </table>
    </div>
    <div class="btnbox">
        <table cellspacing="0" cellpadding="0" border="0" align="left">
            <tbody>
                <tr>
                    <td width="90" align="center">
                        <a href="javascript:;" id="add_bar">新 增</a>
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
                        线路区域名称
                    </th>
                    <th width="17%" bgcolor="#bddcf4" align="center">
                        操作
                    </th>
                </tr>
                <asp:Repeater runat="server" ID="rptList">
                    <ItemTemplate>
                        <tr class="<%#Container.ItemIndex%2==0 ? "even":"odd" %>" data-id='<%#Eval("Id") %>'>
                            <td align="center">
                                <%#Container.ItemIndex+1%>
                            </td>
                            <td align="center">
                                <%#Eval("AreaName") %>
                            </td>
                            <td align="center">
                                <a href="javascript:;" class="update_bar">修改 </a>|<a href="javascript:;" class="delete_bar">
                                    删除</a>
                            </td>
                        </tr>
                    </ItemTemplate>
                </asp:Repeater>
                <asp:Literal ID="lbemptymsg" runat="server"></asp:Literal>
                <tr>
                    <td height="30" align="right" class="pageup" colspan="4">
                        <cc1:ExporPageInfoSelect ID="ExporPageInfoSelect1" runat="server" />
                    </td>
                </tr>
            </tbody>
        </table>
    </div>

    <script type="text/javascript">
        $(function() {
            LineManageList.BindBtn();
        })
        var LineManageList = {
            //显示弹窗
            ShowBoxy: function(data) {
                Boxy.iframeDialog({
                    iframeUrl: data.iframeUrl,
                    title: data.title,
                    modal: true,
                    width: data.width,
                    height: data.height
                });
            },
            BindBtn: function() {
                $("#add_bar").click(function() {
                    LineManageList.ShowBoxy({ iframeUrl: "/SystemSet/lineAdd.aspx?dotype=add", title: "新增线路区域", width: "550px", height: "120px" });
                    return false;
                })
                $(".update_bar").click(function() {
                    var areaid = $(this).closest("tr").attr("data-id");
                    LineManageList.ShowBoxy({ iframeUrl: "/SystemSet/lineAdd.aspx?dotype=update&areaid=" + areaid, title: "修改线路区域", width: "550px", height: "120px" });
                    return false;
                })
                $(".delete_bar").click(function() {
                    var areaid = $(this).closest("tr").attr("data-id");
                    var url = "/SystemSet/LineManage.aspx?dotype=delete&areaid=" + areaid;
                    LineManageList.GoAjax(url);
                    return false;
                })
            },
            GoAjax: function(url) {
                $.newAjax({
                    type: "post",
                    cache: false,
                    url: url,
                    dataType: "json",
                    success: function(ret) {
                        //ajax回发提示
                        if (ret.result == "1") {
                            tableToolbar._showMsg(ret.msg, function() { location.reload(); });
                        }
                        else {
                            tableToolbar._showMsg(ret.msg, function() { location.reload(); });
                        }
                    },
                    error: function() {
                        tableToolbar._showMsg(tableToolbar.errorMsg);
                    }
                });
            }
        }
    </script>

</asp:Content>
