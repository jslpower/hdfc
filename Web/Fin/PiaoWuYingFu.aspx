<%@ Page Title="应付票务款" Language="C#" MasterPageFile="~/MasterPage/Front.Master" AutoEventWireup="true"
    CodeBehind="PiaoWuYingFu.aspx.cs" Inherits="Web.Fin.PiaoWuYingFu" %>

<%@ Register Assembly="ControlLibrary" Namespace="Adpost.Common.ExporPage" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="lineprotitlebox">
        <table width="100%" border="0" cellspacing="0" cellpadding="0">
            <tr>
                <td>
                    <span class="lineprotitle">应付管理</span>
                </td>
                <td align="right" style="padding: 0 10px 2px 0; color: #13509f;">
                    <b>当前您所在位置</b> &gt;&gt; 财务管理 &gt;&gt; 应付管理
                </td>
            </tr>
            <tr>
                <td colspan="2" height="2" bgcolor="#000000">
                </td>
            </tr>
        </table>
    </div>
    <div class="lineCategorybox" style="height: 30px;">
        <table border="0" cellpadding="0" cellspacing="0" class="xtnav">
            <tr>
                <td width="100" align="center">
                    <a href="/Fin/DiJieYingFu.aspx">地接</a>
                </td>
                <td width="100" align="center" class="xtnav-on">
                    <a>票务</a>
                </td>
            </tr>
        </table>
    </div>
    <form method="get" id="form1" action="">
    <table width="99%" border="0" align="center" cellpadding="0" cellspacing="0">
        <tr>
            <td height="30">
                团号：
                <input type="text" size="20" id="tno" class="inputtext formsize100" name="tno" />
                出团时间：
                <input type="text" size="12" id="lst" onfocus="WdatePicker()" class="inputtext formsize70"
                    name="lst" />
                -
                <input type="text" size="12" id="let" onfocus="WdatePicker()" class="inputtext formsize70"
                    name="let" />
                是否结清：
                <select name="jq" id="jq" class="inputselect">
                    <option value="-1">请选择</option>
                    <option value="1">已结清</option>
                    <option value="0">未结清</option>
                </select>
                是否审核：
                <select name="sh" id="sh" class="inputselect">
                    <option value="-1">请选择</option>
                    <option value="1">已审核</option>
                    <option value="0">未审核</option>
                </select>
                是否月结：
                <select name="yj" id="yj" class="inputselect">
                    <option value="-1">请选择</option>
                    <option value="1">是</option>
                    <option value="0">否</option>
                </select>
                出票点：
                <input type="text" id="tGysName" name="tGysName" class="inputtext formsize70" />
                团队类型：
                <select name="tty" class="inputselect" id="tty">
                    <option value="-1">请选择</option>
                    <%= GetTourTypeOption() %>
                </select>
                <input type="image" src="/images/searchbtn.gif" style="vertical-align: top;" />
            </td>
        </tr>
    </table>
    </form>
    <div class="tablelist">
        <table width="100%" border="0" cellpadding="0" cellspacing="1" id="liststyle">
            <tr>
                <th width="60" height="30" align="center">
                    序号
                </th>
                <th align="center">
                    团号
                </th>
                <th align="center">
                    出团日期
                </th>
                <th align="center">
                    人数
                </th>
                <th align="center">
                    航班号/车次
                </th>
                <th align="center">
                    区间
                </th>
                <th align="center">
                    出票人
                </th>
                <th align="center">
                    出票点
                </th>
                <th align="center">
                    支付方式
                </th>
                <th align="center">
                    审核
                </th>
                <th align="center">
                    应付金额
                </th>
                <th align="center">
                    已付金额
                </th>
                <th align="center">
                    未付金额
                </th>
                <th align="center">
                    付款登记
                </th>
            </tr>
            <asp:Repeater runat="server" ID="rptPiaoWu">
                <ItemTemplate>
                    <tr class="<%# GetTrBgColorByTourState(Eval("TourState"),Eval("TourCode").ToString(),Container.ItemIndex +1 ) %>"
                        data-pladid="<%#Eval("PlanId") %>" data-pwtype="<%# Convert.ToInt32(Eval("TicketMode")) %>">
                        <td height="30" align="center">
                            <%# GetLineIndex(Container.ItemIndex) %>
                        </td>
                        <td align="center">
                            <%# Eval("TourCode")%>
                        </td>
                        <td align="center">
                            <%# this.ToDateTimeString(Eval("LDate"))%>
                        </td>
                        <td align="center">
                            <%# Eval("Adults") + "+" + Eval("Childs") %>
                        </td>
                        <td align="center">
                            <%# Eval("TrafficNumber")%>
                        </td>
                        <td align="center">
                            <%# Eval("Interval")%>
                        </td>
                        <td align="center">
                            <%# Eval("ChuPiaoRenName")%><%# GetNewImg(Eval("State"))%>
                        </td>
                        <td align="center">
                            <%# Eval("GysName")%>
                        </td>
                        <td align="center">
                            <%# Eval("PayType")%>
                        </td>
                        <td align="center">
                            <%# GetShenHeHtml(Eval("State"))%>
                        </td>
                        <td align="center">
                            <%# this.ToMoneyString(Eval("YingFuKuan")) %>
                        </td>
                        <td align="center">
                            <%# this.ToMoneyString(Eval("YiFuKuan")) %>
                        </td>
                        <td align="center">
                            <%# this.ToMoneyString(Eval("WeiFuKuan")) %>
                        </td>
                        <td align="center">
                            <%# GetDengJiHtml(Eval("State")) %>
                        </td>
                    </tr>
                </ItemTemplate>
            </asp:Repeater>
            <tr>
                <td height="30" colspan="10" align="right" bgcolor="#E3F1FC">
                    合计：
                </td>
                <td align="center" bgcolor="#E3F1FC">
                    <%= HeJi == null ? string.Empty : this.ToMoneyString(HeJi.YingFuKuan)%>
                </td>
                <td align="center" bgcolor="#E3F1FC">
                    <%= HeJi == null ? string.Empty : this.ToMoneyString(HeJi.YiFuKuan)%>
                </td>
                <td align="center" bgcolor="#E3F1FC">
                    <%= HeJi == null ? string.Empty : this.ToMoneyString(HeJi.WeiFuKuan)%>
                </td>
                <td align="center" bgcolor="#E3F1FC">
                    &nbsp;
                </td>
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
        var PiaoWuYingFu = {
            reload: function() {
                window.location.href = window.location.href;
            },
            ShenHe: function(obj) {
                var _data = { dotype: "update", planid: $.trim($(obj).closest("tr").attr("data-pladid")), isFin: "1" };
                Boxy.iframeDialog({
                    title: "审核票务安排",
                    iframeUrl: "/jidiaoCenter/TicketEdit.aspx",
                    data: _data,
                    width: "760px",
                    height: "600px"
                });
                return false;
            },
            DengJi: function(obj) {
                var tp = "pwc"; //出票
                var pwType = $.trim($(obj).closest("tr").attr("data-pwtype"));
                if (pwType == "<%= (int)EyouSoft.Model.EnumType.PlanStructure.TicketMode.出票 %>") {
                    tp = "pwc"; //出票
                }
                if (pwType == "<%= (int)EyouSoft.Model.EnumType.PlanStructure.TicketMode.退票 %>") {
                    tp = "pwt"; //退票
                }
                var _data = { itemId: $.trim($(obj).closest("tr").attr("data-pladid")), type: tp };
                Boxy.iframeDialog({
                    title: "票务付款登记",
                    iframeUrl: "/Fin/FuKuan.aspx",
                    data: _data,
                    width: "960px",
                    height: "550px",
                    afterHide: function() {
                        PiaoWuYingFu.reload();
                    }
                });
            }
        };

        $(document).ready(function() {

            utilsUri.initSearch();
            tableToolbar.init({ tableContainerSelector: "#liststyle" });

            $("#liststyle").find(".a_ShenHePiaoWu").each(function() {
                $(this).click(function() {
                    PiaoWuYingFu.ShenHe(this);
                    return false;
                });
            });
            $("#liststyle").find(".a_DengJiPiaoWu").each(function() {
                $(this).click(function() {
                    PiaoWuYingFu.DengJi(this);
                    return false;
                });
            });
        });
    </script>

</asp:Content>
