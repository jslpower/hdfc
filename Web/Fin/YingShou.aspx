<%@ Page Title="应收管理" Language="C#" MasterPageFile="~/MasterPage/Front.Master" AutoEventWireup="true"
    CodeBehind="YingShou.aspx.cs" Inherits="Web.Fin.YingShou" %>

<%@ Register Assembly="ControlLibrary" Namespace="Adpost.Common.ExporPage" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">

    <script type="text/javascript">
        function dijieClick(obj) {
            var data = { dotype: "update", tid: $(obj).attr("data-gysid") };
            Boxy.iframeDialog({
                iframeUrl: "/ResourceManage/GroundAdd.aspx?" + $.param(data),
                title: "修改地接社信息",
                modal: true,
                width: "980px",
                height: "500px",
                afterHide: function() { window.location.href = window.location.href; }
            });
            return false;
        }

        function jipiaoClick(obj) {
            var data = { dotype: "update", tid: $(obj).attr("data-gysid") };
            Boxy.iframeDialog({
                iframeUrl: "/ResourceManage/TicketAdd.aspx?" + $.param(data),
                title: "修改机票供应商",
                modal: true,
                width: "980px",
                height: "500px",
                afterHide: function() { window.location.href = window.location.href; }
            });
            return false;
        }
    </script>

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="lineprotitlebox">
        <table width="100%" border="0" cellspacing="0" cellpadding="0">
            <tr>
                <td width="15%" nowrap="nowrap">
                    <span class="lineprotitle">应收管理(新团审核)</span>
                </td>
                <td width="85%" nowrap="nowrap" align="right" style="padding: 0 10px 2px 0; color: #13509f;">
                    <b>当前您所在位置</b>>> 财务管理 >> 应收管理(新团审核)
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
    <form method="get" id="form1" action="">
    <table width="99%" border="0" align="center" cellpadding="0" cellspacing="0">
        <tr>
            <td width="10" valign="top">
                <img src="/images/yuanleft.gif" />
            </td>
            <td>
                <div class="searchbox">
                    团号：
                    <input type="text" size="20" id="tno" class="inputtext formsize100" name="tno" />
                    线路名称：
                    <input type="text" size="20" id="rName" class="inputtext formsize100" name="rName" />
                    销售员：
                    <input type="text" size="12" id="sName" class="inputtext formsize70" name="sName" />
                    出团时间：
                    <input type="text" size="12" id="lst" onfocus="WdatePicker()" class="inputtext formsize70"
                        name="lst" />
                    -
                    <input type="text" size="12" id="let" onfocus="WdatePicker()" class="inputtext formsize70"
                        name="let" />
                    <br />
                    下单时间：
                    <input type="text" size="12" id="ost" onfocus="WdatePicker()" class="inputtext formsize70"
                        name="ost" />
                    -
                    <input type="text" size="12" id="oet" onfocus="WdatePicker()" class="inputtext formsize70"
                        name="oet" />
                    计调员：
                    <input type="text" size="12" id="oName" class="inputtext formsize70" name="oName" />
                    组团社：
                    <input type="text" size="20" id="cName" class="inputtext formsize100" name="cName" />
                    地接社：
                    <input type="text" size="20" id="dName" class="inputtext formsize100" name="dName" />
                    <br />
                    月结：<select name="yj" class="inputselect" id="yj">
                        <option value="-1">请选择</option>
                        <option value="1">是</option>
                        <option value="0">否</option>
                    </select>
                    出票：
                    <select name="cp" class="inputselect" id="cp">
                        <option value="-1">请选择</option>
                        <option value="1">是</option>
                        <option value="0">否</option>
                    </select>
                    收清：
                    <select name="sq" class="inputselect" id="sq">
                        <option value="-1">请选择</option>
                        <option value="1">是</option>
                        <option value="0">否</option>
                    </select>
                    财务是否操作结束：
                    <select name="js" class="inputselect" id="js">
                        <option value="-1">请选择</option>
                        <option value="1">是</option>
                        <option value="0">否</option>
                    </select>
                    团队类型：
                    <select name="tty" class="inputselect" id="tty">
                        <option value="-1">请选择</option>
                        <%= GetTourTypeOption() %>
                    </select>
                    <input type="image" src="/images/searchbtn.gif" style="vertical-align: top;" />
                </div>
            </td>
            <td width="10" valign="top">
                <img src="/images/yuanright.gif" />
            </td>
        </tr>
    </table>
    </form>
    <div class="btnbox">
        <table width="99%" border="0" align="left" cellpadding="0" cellspacing="0">
            <tr>
                <td width="90" align="center">
                    <asp:PlaceHolder runat="server" ID="plnEditTour"><a href="javascript:void(0);" id="a_EditTour">
                        审核/退回</a> </asp:PlaceHolder>
                </td>
                <td align="left">
                </td>
            </tr>
        </table>
    </div>
    <div class="tablelist">
        <table width="100%" border="0" cellpadding="0" cellspacing="1" id="liststyle">
            <tr>
                <th height="30" align="center">
                    <input type="checkbox" name="ckbAll" id="ckbAll" />
                    <label for="ckbAll">
                        全选</label>
                </th>
                <th align="center">
                    团/散
                </th>
                <th align="center">
                    团号
                </th>
                <th align="center">
                    线路名称
                </th>
                <th align="center">
                    出团日期
                </th>
                <th align="center">
                    人数
                </th>
                <th align="center">
                    组团社名称
                </th>
                <th align="center">
                    出票点
                </th>
                <th align="center">
                    地接名称
                </th>
                <th align="center">
                    地接导游
                </th>
                <th align="center">
                    收款情况
                </th>
                <th align="center">
                    销售员
                </th>
                <th align="center">
                    操作员
                </th>
                <% if (IsShowYongJin)
                   { %>
                <th align="center">
                    返佣
                </th>
                <% } %>
            </tr>
            <asp:Repeater runat="server" ID="rptTour">
                <ItemTemplate>
                    <tr class="<%# Container.ItemIndex % 2 == 0 ? "even" : "odd" %> <%# GetTrBgColorByTourState(Eval("TourStatus")) %>"
                        data-tourid="<%#Eval("TourId") %>" data-comid='<%#Eval("BuyCompanyId") %>'>
                        <td height="30" align="center">
                            <input type="checkbox" name="ckbIndex" id="ckb<%# Container.ItemIndex %>" />
                            <%# GetLineIndex(Container.ItemIndex) %>
                        </td>
                        <td align="center">
                            <%# Eval("TourType").ToString() == "团" ? string.Format("<font class='fred'>{0}</font>", Eval("TourType").ToString()) : Eval("TourType").ToString()%>
                        </td>
                        <td align="center">
                            <a>
                                <%# SetDayWeight(Eval("TourCode"))%></a>
                        </td>
                        <td align="center">
                            <%#Eval("RouteName")%><%#GetTourStatus(Eval("TourStatus"))%>
                        </td>
                        <td align="center">
                            <%# this.ToDateTimeString(Eval("LDate"))%>
                        </td>
                        <td align="center">
                            <%#Eval("Adults")%>+<%#Eval("Childs")%>+<%#Eval("Accompanys")%>
                        </td>
                        <td align="center">
                            <span style="display: none;">
                                <%#GetZuTuan(Eval("CustomerContactList"))%></span> <a style="cursor: pointer" class="paopao"
                                    data-width="650" data-com="company">
                                    <%#Eval("BuyCompnayName")%><sup><font class='fred font14'><%#Eval("BuyCompanyCount")%></font></sup></a>
                        </td>
                        <td align="center">
                            <span style="display: none;">
                                <%#GetTicketInfo(Eval("PlanTicketList"))%></span> <a class="paopao" data-width="400">
                                    <%#Eval("TicketCompany")%></a>
                        </td>
                        <td align="center">
                            <span style="display: none;">
                                <%#GetDijieInfo(Eval("DijieList"))%></span> <a class="paopao" data-width="500">
                                    <%#Eval("DijieName")%></a>
                        </td>
                        <td align="center">
                            <span style="display: none;">
                                <%#GetGuideInfo(Eval("GuideList"))%></span> <a class="paopao" data-width="250">
                                    <%#Eval("GuideName")%></a>
                        </td>
                        <td align="center">
                            <span style="display: none;">
                                <%#GetPayInfo(Eval("ShouKuanList"), Eval("IsMonth"), Eval("MonthTime"))%></span>
                            <a class="paopao" href="javascript:void(0);" data-name="a_ShouKuan" data-width="<%# (bool)Eval("IsMonth") ? "140" : "650" %>">
                                <%# GetShouKuanState(Eval("SumPrice"), Eval("CheckMoney"), Eval("IsMonth"))%></a>
                        </td>
                        <td align="center">
                            <%#Eval("SaleName")%>
                        </td>
                        <td align="center">
                            <%#Eval("Planer")%>
                        </td>
                        <% if (IsShowYongJin)
                           { %>
                        <td align="center">
                            <%# this.ToMoneyString((int)Eval("RebatePeople") * (decimal)Eval("RebatePrice"))%>
                        </td>
                        <% } %>
                    </tr>
                </ItemTemplate>
            </asp:Repeater>
        </table>
        <table width="100%" border="0" cellspacing="0" cellpadding="0">
            <tr>
                <td align="right">
                    <cc1:ExporPageInfoSelect ID="page1" runat="server" />
                </td>
            </tr>
        </table>
    </div>

    <script type="text/javascript">
        $(function() {

            utilsUri.initSearch();
            tableToolbar.init({ tableContainerSelector: "#liststyle" });

            $("#liststyle").find("a[class='paopao']").each(function() {
                $(this).bt({
                    contentSelector: function() {
                        return $(this).prev("span").html();
                    },
                    positions: ['bottom'],
                    fill: '#effaff',
                    strokeStyle: '#2a9cd4',
                    noShadowOpts: { strokeStyle: "#2a9cd4" },
                    spikeLength: 5,
                    spikeGirth: 15,
                    width: $.trim($(this).attr("data-width")),
                    overlap: 0,
                    centerPointY: 4,
                    cornerRadius: 4,
                    shadow: true,
                    shadowColor: 'rgba(0,0,0,.5)',
                    cssStyles: { color: '#1351a0', 'line-height': '200%' }
                });
            });
            $("#liststyle .paopao").each(function() {
                var self = $(this);
                if (self.attr("data-com") == "company") {
                    self.click(function() {
                        var comid = self.closest("tr").attr("data-comid");
                        var data = {
                            id: comid,
                            dotype: "update"
                        };
                        Boxy.iframeDialog({
                            title: "修改组团社",
                            iframeUrl: "/CustomerManage/CustomerEdit.aspx",
                            data: data,
                            width: "920px",
                            height: "510px"
                        });
                        //PlanList.ShowBoxy({ iframeUrl: "/CustomerManage/CustomerEdit.aspx?" + $.param(data), title: "修改组团社", width: "920px", height: "510px" });
                        return false;
                    })
                }
            })

            $("#a_EditTour").click(function() {
                var cv = [];
                $("#liststyle").find("input[name='ckbIndex']:checked").each(function() {
                    cv.push($.trim($(this).closest("tr").attr("data-tourId")));
                });
                if (cv == null || cv.length <= 0) {
                    tableToolbar._showMsg("请选择要修改的应收项！");
                    return false;
                }
                if (cv != null && cv.length > 1) {
                    tableToolbar._showMsg("只能选择一项进行修改！");
                    return false;
                }
                var _data = { dotype: "update", tourid: cv[0], isFin: "1" };
                Boxy.iframeDialog({
                    title: "修改应收信息",
                    iframeUrl: "/jidiaoCenter/TeamEdit.aspx",
                    data: _data,
                    width: "980px",
                    height: "560px"
                });
                return false;
            });

            $("#liststyle").find("a[data-name='a_ShouKuan']").each(function() {
                $(this).click(function() {
                    var _data = { itemId: $.trim($(this).closest("tr").attr("data-tourId")) };
                    Boxy.iframeDialog({
                        title: "收款登记",
                        iframeUrl: "/Fin/ShouKuan.aspx",
                        data: _data,
                        width: "960px",
                        height: "550px",
                        afterHide: function() {
                            window.location.href = window.location.href;
                        }
                    });
                    return false;
                });
            });
        });
        
    </script>

</asp:Content>
