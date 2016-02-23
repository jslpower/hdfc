<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="YinHangZhangHu.aspx.cs"
    Inherits="Web.SystemSet.YinHangZhangHu" MasterPageFile="~/MasterPage/Front.Master"
    Title="银行账号表-财务管理" %>

<%@ MasterType VirtualPath="~/MasterPage/Front.Master" %>
<%@ Register Assembly="ControlLibrary" Namespace="Adpost.Common.ExporPage" TagPrefix="cc1" %>
<asp:Content ContentPlaceHolderID="ContentPlaceHolder1" ID="PageBody" runat="server">
    <div class="lineprotitlebox">
        <table width="100%" border="0" cellspacing="0" cellpadding="0">
            <tr>
                <td width="15%" nowrap="nowrap">
                    <span class="lineprotitle">基础设置</span>
                </td>
                <td width="85%" nowrap="nowrap" align="right" style="padding: 0 10px 2px 0; color: #13509f;">
                    <b>当前您所在位置</b>>> 系统设置 >> 基础设置
                </td>
            </tr>
            <tr>
                <td colspan="2" height="2" bgcolor="#000000">
                </td>
            </tr>
        </table>
    </div>
    <div style="height: 50px;" class="lineCategorybox">
        <table cellspacing="0" cellpadding="0" border="0" class="xtnav">
            <tbody>
                <tr>
                    <td width="100" align="center">
                        <a href="/SystemSet/CityManage.aspx">城市管理</a>
                    </td>
                    <td width="100" align="center">
                        <a href="/SystemSet/LineManage.aspx">线路区域管理</a>
                    </td>
                    <td width="100" align="center" class="xtnav-on">
                        <a>公司账户</a>
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
    <div class="hr_10">
    </div>
    <div class="btnbox">
        <asp:PlaceHolder runat="server" ID="phInsert">
            <table border="0" align="left" cellpadding="0" cellspacing="0">
                <tr>
                    <td width="90" align="center">
                        <a href="javascript:void(0)" id="i_insert">新 增</a>
                    </td>
                </tr>
            </table>
        </asp:PlaceHolder>
    </div>
    <div class="tablelist">
        <table width="100%" border="0" cellpadding="0" cellspacing="1" id="liststyle">
            <tr class="odd" style="height: 30px;">
                <th width="36" align="center">
                    序号
                </th>
                <th width="15%" align="center">
                    账户名称
                </th>
                <th width="30%" align="center">
                    开户银行
                </th>
                <th align="center">
                    银行账号
                </th>
                <th width="80" align="center">
                    操作
                </th>
            </tr>
            <asp:Repeater runat="server" ID="rpts">
                <ItemTemplate>
                    <tr class="<%#Container.ItemIndex%2==0?"even":"odd" %>" i_zhanghuid="<%#Eval("Id") %>"
                        i_status="<%#(int)Eval("AccountState") %>" style="height: 30px;">
                        <td align="center">
                            <%# Container.ItemIndex + 1%>
                        </td>
                        <td align="center">
                            <%#Eval("AccountName")%>
                        </td>
                        <td align="center">
                            <%#Eval("BankName")%>
                        </td>
                        <td align="center">
                            <%#Eval("BankNo")%>
                        </td>
                        <td align="center">
                            <%#GetOperatorHtml(Eval("AccountState"))%>
                        </td>
                    </tr>
                </ItemTemplate>
            </asp:Repeater>
            <asp:PlaceHolder runat="server" ID="phEmpty">
                <tr>
                    <td class="even" colspan="8" style="height: 30px; text-align: center;">
                        暂无任何银行账号信息。
                    </td>
                </tr>
            </asp:PlaceHolder>
            <asp:PlaceHolder ID="phHeJi" runat="server"></asp:PlaceHolder>
        </table>
        <asp:PlaceHolder runat="server" ID="phPaging">
            <table width="100%" border="0" cellspacing="0" cellpadding="0">
                <tr>
                    <td align="right">
                        <cc1:ExporPageInfoSelect ID="paging" runat="server" />
                    </td>
                </tr>
            </table>
        </asp:PlaceHolder>
    </div>

    <script type="text/javascript">
        var iPage = {
            reload: function() {
                window.location.href = window.location.href;
            },
            //新增
            insert: function(obj) {
                var _data = {}
                Boxy.iframeDialog({ title: "银行账号登记", iframeUrl: "yinhangzhanghuedit.aspx", width: "670px", height: "300px", data: _data, afterHide: function() { iPage.reload(); } });
                return false;
            },
            //修改、查看
            update: function(obj) {
                var _$tr = $(obj).closest("tr");
                var _data = { zhanghuid: _$tr.attr("i_zhanghuid") };
                var _title = "银行账号修改";
                if ($(obj).attr("i_chakan") == "1") _title = "银行账号查看";
                Boxy.iframeDialog({ title: _title, iframeUrl: "yinhangzhanghuedit.aspx", width: "670px", height: "300px", data: _data, afterHide: function() { iPage.reload(); } });
                return false;
            },
            //删除
            del: function(obj) {
                if (!confirm("银行账号信息删除后不可恢复，你确定要删除吗？")) return;
                var _$tr = $(obj).closest("tr");
                var _data = { zhanghuid: _$tr.attr("i_zhanghuid") };

                $(obj).unbind("click").css({ "color": "#999999" });

                $.newAjax({
                    type: "POST",
                    url: utilsUri.createUri(window.location.href, { doType: "delete" }),
                    data: _data,
                    cache: false,
                    dataType: "json",
                    async: false,
                    success: function(response) {
                        if (response.result == "1") {
                            alert(response.msg);
                            iPage.reload();
                        } else {
                            alert(response.msg);
                            $(obj).bind("click", function() { iPage.del(obj); }).css({ "color": "" });
                        }
                    },
                    error: function() {
                        $(obj).bind("click", function() { iPage.del(obj); }).css({ "color": "" });
                    }
                });
            }
        };

        $(document).ready(function() {
            tableToolbar.init({ tableContainerSelector: "#liststyle" });
            $("#i_insert").bind("click", function() { return iPage.insert(this); });
            $(".i_update").bind("click", function() { return iPage.update(this); });
            $(".i_delete").bind("click", function() { return iPage.del(this); return false; });
        });
    </script>

</asp:Content>
