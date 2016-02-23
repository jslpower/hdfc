<%@ Page Title="销售地区管理" Language="C#" MasterPageFile="~/MasterPage/Front.Master"
    AutoEventWireup="true" CodeBehind="SaleArea.aspx.cs" Inherits="Web.SystemSet.SaleArea" %>

<%@ Register Assembly="ControlLibrary" Namespace="Adpost.Common.ExporPage" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
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
    <div class="lineCategorybox" style="height: 50px;">
        <table border="0" cellpadding="0" cellspacing="0" class="xtnav">
            <tr>
                <td width="100" align="center">
                    <a href="/SystemSet/CityManage.aspx">城市管理</a>
                </td>
                <td width="100" align="center">
                    <a href="/SystemSet/LineManage.aspx">线路区域管理</a>
                </td>
                <td width="100" align="center">
                    <a href="/SystemSet/YinHangZhangHu.aspx">公司账户</a>
                </td>
                <td width="100" align="center" class="xtnav-on">
                    <a>销售地区管理</a>
                </td>
                <td width="100" align="center">
                    <a href="/SystemSet/CarFlight.aspx">车次航班管理</a>
                </td>
                <td width="100" align="center">
                    <a href="/SystemSet/Rating.aspx">信用评级管理</a>
                </td>
            </tr>
        </table>
    </div>
    <div class="btnbox">
        <table border="0" align="left" cellpadding="0" cellspacing="0">
            <tr>
                <td width="90" align="center">
                    <a href="javascript:void(0);" id="a_AddSaleArea">新 增</a>
                </td>
            </tr>
        </table>
    </div>
    <div class="tablelist">
        <table width="100%" border="0" cellpadding="0" cellspacing="1" id="liststyle">
            <tr>
                <th width="36" height="30" align="center" bgcolor="#BDDCF4">
                    编号
                </th>
                <th align="center" bgcolor="#BDDCF4">
                    销售地区名称
                </th>
                <th align="center" bgcolor="#bddcf4">
                    操作
                </th>
                <th width="36" align="center" bgcolor="#bddcf4">
                    编号
                </th>
                <th align="center" bgcolor="#bddcf4">
                    销售地区名称
                </th>
                <th align="center" bgcolor="#bddcf4">
                    操作
                </th>
            </tr>
            <tr class="even">
                <asp:Repeater runat="server" ID="rptSaleArea">
                    <ItemTemplate>
                        <td height="30" align="center">
                            <%# GetLineIndex(Container.ItemIndex) %>
                        </td>
                        <td align="center">
                            <%# Eval("SaleAreaName")%>
                        </td>
                        <td align="center" data-saleareaid="<%# Eval("SaleAreaId") %>">
                            <a href="javascript:void(0);" class="a_EditSaleArea">修改 </a>| <a href="javascript:void(0);"
                                class="a_DelSaleArea">删除</a>
                        </td>
                        <%# GetLineClass(Container.ItemIndex)%>
                    </ItemTemplate>
                </asp:Repeater>
            </tr>
            <tr>
                <td height="30" colspan="6" align="right" class="pageup">
                    <cc1:ExporPageInfoSelect ID="paging" runat="server" />
                </td>
            </tr>
        </table>
    </div>

    <script type="text/javascript">
        var SaleAreaManage = {
            reload: function() {
                window.location.href = window.location.href;
            },
            AddSaleArea: function(obj) {
                var _data = { action: "add" };
                Boxy.iframeDialog({
                    title: "添加销售地区",
                    iframeUrl: "/SystemSet/SaleAreaEdit.aspx",
                    data: _data,
                    width: "600px",
                    height: "230px",
                    afterHide: function() {
                        SaleAreaManage.reload();
                    }
                });
                return false;
            },
            EditSaleArea: function(obj) {
                var sId = $.trim($(obj).closest("td").attr("data-saleAreaId"));
                var _data = { action: "edit", sId: sId };
                Boxy.iframeDialog({
                    title: "修改销售地区",
                    iframeUrl: "/SystemSet/SaleAreaEdit.aspx",
                    data: _data,
                    width: "600px",
                    height: "230px",
                    afterHide: function() {
                        SaleAreaManage.reload();
                    }
                });
                return false;
            },
            DelSaleArea: function(obj) {
                tableToolbar.ShowConfirmMsg("您确定要删除此销售地区吗？", function() {
                    var sId = $.trim($(obj).closest("td").attr("data-saleAreaId"));
                    var _data = { action: "del", sId: sId };
                    $.newAjax({
                        type: "post",
                        cache: false,
                        dataType: "json",
                        async: false, //禁止删除销售区域的同时进行其他操作
                        url: "/SystemSet/SaleArea.aspx?" + $.param(_data),
                        success: function(ret) {
                            if (ret.result == "1") {
                                tableToolbar._showMsg(ret.msg, function() {
                                    SaleAreaManage.reload();
                                });
                            }
                            else {
                                tableToolbar._showMsg(ret.msg);
                            }
                        },
                        error: function() {
                            tableToolbar._showMsg(tableToolbar.errorMsg);
                        }
                    });
                });
                return false;
            }
        };

        $(document).ready(function() {
            tableToolbar.init({ tableContainerSelector: "#liststyle" });
            $("#liststyle").find(".a_EditSaleArea").each(function() {
                $(this).bind("click", function() { SaleAreaManage.EditSaleArea(this); });
            });
            $("#liststyle").find(".a_DelSaleArea").each(function() {
                $(this).bind("click", function() { SaleAreaManage.DelSaleArea(this); });
            });
            $("#a_AddSaleArea").click(function() {
                SaleAreaManage.AddSaleArea(this);
            });
        });
    </script>

</asp:Content>
