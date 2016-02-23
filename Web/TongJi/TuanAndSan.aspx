<%@ Page Title="团散统计" Language="C#" MasterPageFile="~/MasterPage/Front.Master" AutoEventWireup="true"
    CodeBehind="TuanAndSan.aspx.cs" Inherits="Web.TongJi.TuanAndSan" %>

<%@ Register Assembly="ControlLibrary" Namespace="Adpost.Common.ExporPage" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="lineprotitlebox">
        <table width="100%" border="0" cellspacing="0" cellpadding="0">
            <tr>
                <td width="15%" nowrap="nowrap">
                    <span class="lineprotitle">团散统计</span>
                </td>
                <td width="85%" nowrap="nowrap" align="right" style="padding: 0 10px 2px 0; color: #13509f;">
                    <b>当前您所在位置</b>>> 统计中心 >> 团散统计
                </td>
            </tr>
            <tr>
                <td colspan="2" height="2" bgcolor="#000000">
                </td>
            </tr>
        </table>
    </div>
    <div class="hr_10">
    </div>
    <div class="tablelist" id="con_two_2">
        <form id="form1" action="" method="get">
        <table width="99%" border="0" align="center" cellpadding="0" cellspacing="0">
            <tr>
                <td width="10" valign="top">
                    <img src="/images/yuanleft.gif" />
                </td>
                <td>
                    <div class="searchbox">
                        省份：<select id="pid" name="pid" class="inputselect">
                        </select>
                        城市：
                        <select id="cid" name="cid" class="inputselect">
                        </select>
                        出团时间：
                        <input name="lst" type="text" class="searchinput inputtext" id="lst" onfocus="WdatePicker()" />
                        -
                        <input name="let" type="text" class="searchinput inputtext" id="let" onfocus="WdatePicker()" />
                        下单时间：
                        <input name="ost" type="text" class="searchinput inputtext" id="ost" onfocus="WdatePicker()" />
                        -
                        <input name="oet" type="text" class="searchinput inputtext" id="oet" onfocus="WdatePicker()" />
                        <br />
                        团/散：
                        <select name="ts" id="ts">
                            <%= GetSelectHtml() %>
                        </select>
                        组团社：
                        <input name="cName" type="text" class="searchinput formsize140 inputtext" id="cName" />
                        计调员：
                        <input name="oName" type="text" class="searchinput inputtext" id="oName" />
                        销售员：
                        <input name="sName" type="text" class="searchinput inputtext" id="sName" />
                        <input type="image" src="/images/searchbtn.gif" style="vertical-align: top;" />
                    </div>
                </td>
                <td width="10" valign="top">
                    <img src="/images/yuanright.gif" />
                </td>
            </tr>
        </table>
        </form>
        <div class="hr_10">
        </div>
        <table width="100%" border="0" cellpadding="0" cellspacing="1" id="liststyle">
            <tr>
                <th width="36" height="30" align="center" bgcolor="#BDDCF4">
                    序号
                </th>
                <th align="center" bgcolor="#bddcf4">
                    组团社
                </th>
                <th align="center" bgcolor="#bddcf4">
                    计调
                </th>
                <th align="center" bgcolor="#bddcf4">
                    出发时间
                </th>
                <th align="center" bgcolor="#bddcf4">
                    团号
                </th>
                <th align="center" bgcolor="#bddcf4">
                    人数
                </th>
                <th align="center" bgcolor="#bddcf4">
                    线路
                </th>
                <th align="center">
                    大交通
                </th>
                <th align="center" bgcolor="#bddcf4">
                    总收入
                </th>
                <th align="center" bgcolor="#bddcf4">
                    已收
                </th>
                <th align="center" bgcolor="#bddcf4">
                    未收
                </th>
                <th align="center" bgcolor="#bddcf4">
                    机票支出
                </th>
                <th align="center" bgcolor="#bddcf4">
                    地接支出
                </th>
                <% if (IsShowYongJin)
                   { %>
                <th align="center" bgcolor="#bddcf4">
                    佣金
                </th>
                <% } %>
                <% if (IsShowMaoLi)
                   { %>
                <th align="center" bgcolor="#bddcf4">
                    毛利
                </th>
                <% } %>
            </tr>
            <asp:Repeater runat="server" ID="rptTuanAndSan">
                <ItemTemplate>
                    <tr class="<%# Container.ItemIndex % 2 == 0 ? "even" : "odd" %>">
                        <td height="30" align="center">
                            <%# GetLineIndex(Container.ItemIndex) %>
                        </td>
                        <td align="center">
                            <%# Eval("CustomerName")%>
                        </td>
                        <td align="center">
                            <%# Eval("PlanName")%>
                        </td>
                        <td align="center">
                            <%# this.ToDateTimeString(Eval("LeaveDate"))%>
                        </td>
                        <td align="center">
                            <%# Eval("TourNo")%>
                        </td>
                        <td align="center">
                            <%# Eval("Adults")%>+<%# Eval("Childs")%>
                        </td>
                        <td align="center">
                            <%# Eval("RouteName")%>
                        </td>
                        <td align="center">
                            <span style="display: none;">
                                <%# GetDaJiaoTongFuDong(Eval("TicketFloat"))%></span><a class="paopao" href="javascript:void(0);"
                                    title=""><%# Eval("HeavyTraffic")%></a>
                        </td>
                        <td align="center">
                            <font color="green">
                                <%# this.ToMoneyString(Eval("ShouRuSumPrice"))%></font>
                        </td>
                        <td align="center">
                            <font color="blue">
                                <%# this.ToMoneyString(Eval("YiShou"))%></font>
                        </td>
                        <td align="center">
                            <font class="fred">
                                <%# this.ToMoneyString(Eval("WeiShou"))%></font>
                        </td>
                        <td align="center">
                            <%# this.ToMoneyString(Eval("JiPiaoZhiChu"))%>
                        </td>
                        <td align="center">
                            <%# this.ToMoneyString(Eval("DiJieZhiChu"))%>
                        </td>
                        <% if (IsShowYongJin)
                           { %>
                        <td align="center">
                            <%# this.ToMoneyString(Eval("YongJin"))%>
                        </td>
                        <% } %>
                        <% if (IsShowMaoLi)
                           { %>
                        <td align="center">
                            <%# this.ToMoneyString(Eval("MaoLi2"))%>
                        </td>
                        <% } %>
                    </tr>
                </ItemTemplate>
            </asp:Repeater>
            <tr>
                <td height="30" colspan="5" align="right" bgcolor="#E3F1FC">
                    合计：
                </td>
                <td align="center" bgcolor="#E3F1FC">
                    <%= HeJi == null ? string.Empty : HeJi.Adults + "+" + HeJi.Childs %>
                </td>
                <td align="center" bgcolor="#E3F1FC">
                    &nbsp;
                </td>
                <td align="center" bgcolor="#E3F1FC">
                    &nbsp;
                </td>
                <td align="center" bgcolor="#E3F1FC">
                    <%= HeJi == null ? string.Empty : this.ToMoneyString(HeJi.ShouRu) %>
                </td>
                <td align="center" bgcolor="#E3F1FC">
                    <%= HeJi == null ? string.Empty : this.ToMoneyString(HeJi.YiShou) %>
                </td>
                <td align="center" bgcolor="#E3F1FC">
                    <%= HeJi == null ? string.Empty : this.ToMoneyString(HeJi.WeiShou) %>
                </td>
                <td align="center" bgcolor="#E3F1FC">
                    <%= HeJi == null ? string.Empty : this.ToMoneyString(HeJi.JiPiaoZhiChu)%>
                </td>
                 <td align="center" bgcolor="#E3F1FC">
                    <%= HeJi == null ? string.Empty : this.ToMoneyString(HeJi.DiJieZhiChu)%>
                </td>
                <% if (IsShowYongJin)
                   { %>
                <td align="center" bgcolor="#E3F1FC">
                    <%= HeJi == null ? string.Empty : this.ToMoneyString(HeJi.YongJin) %>
                </td>
                <% } %>
                <% if (IsShowMaoLi)
                   { %>
                <td align="center" bgcolor="#E3F1FC">
                    <%= HeJi == null ? string.Empty : this.ToMoneyString(HeJi.MaoLi2) %>
                </td>
                <% } %>
            </tr>
        </table>
        <table width="100%" border="0" cellspacing="0" cellpadding="0">
            <tr>
                <td align="right">
                    <cc1:ExporPageInfoSelect runat="server" ID="page1" />
                </td>
            </tr>
        </table>
    </div>

    <script type="text/javascript">
        $(document).ready(function() {
            utilsUri.initSearch();
            tableToolbar.init({ tableContainerSelector: "#liststyle" });

            pcToobar.init({
                pID: "#pid",
                cID: "#cid",
                comID: '<%=this.SiteUserInfo.CompanyId %>',
                gSelect: "1",
                pSelect: '<%= EyouSoft.Common.Utils.GetQueryStringValue("pid") %>',
                cSelect: '<%= EyouSoft.Common.Utils.GetQueryStringValue("cid") %>',
                isCy: "0"
            });

            $("#liststyle").find('.paopao').bt({
                contentSelector: function() {
                    return $(this).prev("span").html();
                },
                positions: ['bottom'],
                fill: '#effaff',
                strokeStyle: '#2a9cd4',
                noShadowOpts: { strokeStyle: "#2a9cd4" },
                spikeLength: 5,
                spikeGirth: 15,
                width: 400,
                overlap: 0,
                centerPointY: 4,
                cornerRadius: 4,
                shadow: true,
                shadowColor: 'rgba(0,0,0,.5)',
                cssStyles: { color: '#1351a0', 'line-height': '200%' }
            });
        });
    </script>

</asp:Content>
