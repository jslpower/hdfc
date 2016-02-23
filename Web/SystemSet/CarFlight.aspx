<%@ Page Title="车次航班管理" Language="C#" MasterPageFile="~/MasterPage/Front.Master"
    AutoEventWireup="true" CodeBehind="CarFlight.aspx.cs" Inherits="Web.SystemSet.CarFlight" %>

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
                            <span class="lineprotitle">基础设置</span>
                        </td>
                        <td width="85%" nowrap="nowrap" align="right" style="padding: 0 10px 2px 0; color: #13509f;">
                            <b>当前您所在位置：</b> &gt;&gt; 系统设置 &gt;&gt; 基础设置
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
                        <td width="100" align="center">
                            <a href="/SystemSet/LineManage.aspx">线路区域管理</a>
                        </td>
                        <td width="100" align="center">
                            <a href="/SystemSet/YinHangZhangHu.aspx">公司账户</a>
                        </td>
                        <td width="100" align="center">
                            <a href="/SystemSet/SaleArea.aspx">销售地区管理</a>
                        </td>
                        <td width="100" align="center" class="xtnav-on">
                            <a>车次航班管理</a>
                        </td>
                        <td width="100" align="center">
                        <a href="/SystemSet/Rating.aspx">信用评级管理</a>
                    </td>
                    </tr>
                </tbody>
            </table>
        </div>
        <div class="btnbox">
            <asp:PlaceHolder runat="server" ID="phInsert">
                <table border="0" align="left" cellpadding="0" cellspacing="0">
                    <tr>
                        <td width="90" align="center">
                            <a href="javascript:void(0)" id="a_Add_CarFlight">新 增</a>
                        </td>
                    </tr>
                </table>
            </asp:PlaceHolder>
        </div>
        <div class="hr_10">
        </div>
        <table width="99%" cellspacing="0" cellpadding="0" border="0" align="center">
            <tbody>
                <tr>
                    <td width="10" valign="top">
                        <img src="/images/yuanleft.gif" alt="" />
                    </td>
                    <td>
                        <form method="get" id="form1">
                        <div class="searchbox">
                            车次/航班：
                            <input type="text" size="30" id="cf" class="inputtext formsize140" name="cf" />
                            机票/火车票：
                            <select id="at" name="at">
                                <%= GetTypeHtml() %>
                            </select>
                            <input type="submit" value="" class="search-btn" />
                        </div>
                        </form>
                    </td>
                    <td width="10" valign="top">
                        <img src="/images/yuanright.gif" alt="" />
                    </td>
                </tr>
            </tbody>
        </table>
        <div class="tablelist">
            <table width="100%" cellspacing="1" cellpadding="0" border="0" id="liststyle">
                <tbody>
                    <tr>
                        <th width="36" height="30" align="center">
                            序号
                        </th>
                        <th align="center">
                            车次/航班
                        </th>
                        <th align="center">
                            机票/火车票
                        </th>
                        <th align="center">
                            开车/起飞时间
                        </th>
                        <th align="center">
                            区间
                        </th>
                        <th align="center">
                            手续费/机燃
                        </th>
                        <th align="center">
                            佣金
                        </th>
                        <th align="center">
                            舱位/席别
                        </th>
                        <th align="center">
                            操作
                        </th>
                    </tr>
                    <asp:Repeater runat="server" ID="RptList">
                        <ItemTemplate>
                            <tr class="<%# Container.ItemIndex % 2 == 0 ? "even" : "odd"%>" data-id='<%#Eval("Id") %>'>
                                <td height="30" align="center">
                                    <%# GetLineIndex(Container.ItemIndex)%>
                                </td>
                                <td align="center">
                                    <%#Eval("TrafficNumber")%>
                                </td>
                                <td align="center">
                                    <%# Eval("TicketType") == null ? string.Empty : ((EyouSoft.Model.EnumType.PlanStructure.TicketType)Eval("TicketType")).ToString()%>
                                </td>
                                <td align="center">
                                    <%# GetTrafficTime(Eval("TrafficTime"))%>
                                </td>
                                <td align="center">
                                    <%#Eval("Interval")%>
                                </td>
                                <td align="center">
                                    <%# this.ToMoneyString(Eval("OtherPrice"))%>
                                </td>
                                <td align="center">
                                    <%# this.ToMoneyString(Eval("Brokerage"))%>
                                </td>
                                <td align="center">
                                    <%#Convert.ToInt32(Eval("_TrafficSeat")) == 0 ? "": Eval("_TrafficSeat")%>
                                </td>
                                <td align="center">
                                    <a class="update" href="javascript:;">修改</a> | <a href="javascript:;" class="delete">
                                        删除</a>
                                </td>
                            </tr>
                        </ItemTemplate>
                    </asp:Repeater>
                </tbody>
            </table>
            <table width="100%" cellspacing="0" cellpadding="0" border="0">
                <tbody>
                    <tr>
                        <td align="right">
                            <cc1:ExporPageInfoSelect ID="ExporPageInfoSelect1" runat="server" />
                        </td>
                    </tr>
                </tbody>
            </table>
        </div>
    </div>

    <script type="text/javascript">

        $(document).ready(function() {
            CarAndFlight.BindBtn();

            utilsUri.initSearch();
        });

        var CarAndFlight = {
            reload: function() {
                window.location.href = window.location.href;
                return false;
            },
            add: function() {
                CarAndFlight.ShowBoxy({ iframeUrl: "/SystemSet/EditCarFlight.aspx?action=add", title: "新增", width: "620px", height: "350px" });
            },
            edit: function(cid) {
                CarAndFlight.ShowBoxy({ iframeUrl: "/SystemSet/EditCarFlight.aspx?action=edit&cid=" + cid, title: "修改", width: "620px", height: "350px" });
            },
            delCar: function(cid) {
                var url = "CarFlight.aspx?action=del&cid=" + cid;
                CarAndFlight.GoAjax(url);
                return false;
            },
            //显示弹窗
            ShowBoxy: function(data) {
                Boxy.iframeDialog({
                    iframeUrl: data.iframeUrl,
                    title: data.title,
                    modal: true,
                    width: data.width,
                    height: data.height,
                    afterHide: function() { CarAndFlight.reload(); }
                });
            },
            BindBtn: function() {
                //绑定Update事件
                $(".update").click(function() {
                    var cid = $(this).closest("tr").attr("data-id");
                    CarAndFlight.edit(cid);
                    return false;
                });
                $(".delete").click(function() {
                    var cid = $(this).closest("tr").attr("data-id");
                    tableToolbar.ShowConfirmMsg("确定要删除此数据吗？", function() {
                        CarAndFlight.delCar(cid);
                    });
                    return false;
                })
                $("#a_Add_CarFlight").click(function() {
                    CarAndFlight.add();
                    return false;
                });
                tableToolbar.init({
                    tableContainerSelector: "#liststyle", //表格选择器
                    objectName: "行"
                });
            },
            //Ajax请求
            GoAjax: function(url) {
                $.newAjax({
                    type: "post",
                    cache: false,
                    url: url,
                    dataType: "json",
                    success: function(ret) {
                        if (ret.result == "1") {
                            tableToolbar._showMsg(ret.msg, function() { CarAndFlight.reload(); });
                        }
                        else {
                            tableToolbar._showMsg(ret.msg, function() { CarAndFlight.reload(); });
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
