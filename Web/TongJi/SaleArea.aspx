<%@ Page Title="销售地区统计" Language="C#" MasterPageFile="~/MasterPage/Front.Master"
    AutoEventWireup="true" CodeBehind="SaleArea.aspx.cs" Inherits="Web.TongJi.SaleArea" %>

<%@ Register Assembly="ControlLibrary" Namespace="Adpost.Common.ExporPage" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="lineprotitlebox">
        <table width="100%" border="0" cellspacing="0" cellpadding="0">
            <tr>
                <td width="15%" nowrap="nowrap">
                    <span class="lineprotitle">销售地区统计</span>
                </td>
                <td width="85%" nowrap="nowrap" align="right" style="padding: 0 10px 2px 0; color: #13509f;">
                    <b>当前您所在位置</b>>> 统计中心 >> 销售地区统计
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
                        时间：
                        <input name="st" type="text" class="searchinput inputtext" id="st" onfocus="WdatePicker()" />
                        -
                        <input name="et" type="text" class="searchinput inputtext" id="et" onfocus="WdatePicker()" />
                        <input type="image" src="/images/searchbtn.gif" style="vertical-align: top;" />
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
                    销售地区
                </th>
                <th align="center" bgcolor="#bddcf4">
                    总收入
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
            <asp:Repeater runat="server" ID="rptSaleArea">
                <ItemTemplate>
                    <tr class="<%# Container.ItemIndex % 2 == 0 ? "even" : "odd" %>">
                        <td height="30" align="center">
                            <%# GetLineIndex(Container.ItemIndex) %>
                        </td>
                        <td align="center">
                            <%# Eval("SaleAreaName")%>
                        </td>
                        <td align="center">
                            <font class="fred">
                                <%# this.ToMoneyString(Eval("ZongShouRu"))%></font>
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
                <td height="30" colspan="2" align="right" bgcolor="#E3F1FC">
                    合计：
                </td>
                <td align="center" bgcolor="#E3F1FC">
                    <%= HeJi == null ? string.Empty : this.ToMoneyString(HeJi.ShouRu) %>
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
        });
    </script>

</asp:Content>
