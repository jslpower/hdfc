<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ChuNaDengZhang.aspx.cs"
    Title="出纳登账-财务管理" Inherits="Web.Fin.ChuNaDengZhang" MasterPageFile="~/MasterPage/Front.Master" %>

<%@ Register Assembly="ControlLibrary" Namespace="Adpost.Common.ExporPage" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="mainbody">
        <div class="lineprotitlebox">
            <table width="100%" border="0" cellspacing="0" cellpadding="0">
                <tr>
                    <td width="15%" nowrap="nowrap">
                        <span class="lineprotitle">财务管理</span>
                    </td>
                    <td width="85%" nowrap="nowrap" align="right" style="padding: 0 10px 2px 0; color: #13509f;">
                        <b>当前您所在位置：</b> >> 财务管理 >> 出纳登账
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
        <table width="99%" border="0" align="center" cellpadding="0" cellspacing="0">
            <tr>
                <td width="10" valign="top">
                    <img src="../images/yuanleft.gif" />
                </td>
                <td>
                    <form id="frm" action="/Fin/ChuNaDengZhang.aspx" method="get">
                    <div class="searchbox">
                        <label>
                            时间：</label>
                        <input type="text" id="txtBeginDate" name="txtBeginDate" class="inputtext" onfocus="WdatePicker()"
                            value="<%=EyouSoft.Common.Utils.GetQueryStringValue("txtBeginDate")%>" />
                        —
                        <input type="text" id="txtEndDate" name="txtEndDate" class="inputtext" onfocus="WdatePicker()"
                            value="<%=EyouSoft.Common.Utils.GetQueryStringValue("txtEndDate")%>" />
                        <label>
                            类型：</label>
                        <select class="inputselect" name="ddlStatus" id="ddlStatus">
                            <%=BindStatus(EyouSoft.Common.Utils.GetQueryStringValue("ddlStatus"))%>
                        </select>
                        <a href="javascript:;" id="btnSearch">
                            <img src="../images/searchbtn.gif" style="vertical-align: top;" alt="" /></a>
                    </div>
                    </form>
                </td>
                <td width="10" valign="top">
                    <img src="../images/yuanright.gif" />
                </td>
            </tr>
        </table>
        <div class="btnbox">
            <table border="0" align="left" cellpadding="0" cellspacing="0">
                <tr>
                    <td width="90" align="center">
                        <%if (add)
                          { %>
                        <a href="javascript:;" class="add">新 增</a><%} %>
                    </td>
                </tr>
            </table>
        </div>
        <div class="tablelist">
            <table width="100%" border="0" cellpadding="0" cellspacing="1">
                <tr>
                    <th width="36" height="30" align="center" bgcolor="#BDDCF4">
                        序号
                    </th>
                    <th align="center" bgcolor="#BDDCF4">
                        收款/付款
                    </th>
                    <th align="center" bgcolor="#BDDCF4">
                        团号
                    </th>
                    <th align="center" bgcolor="#BDDCF4">
                        收款（付款）日期
                    </th>
                    <th align="center" bgcolor="#bddcf4">
                        金额
                    </th>
                    <th align="center" bgcolor="#bddcf4">
                        银行账号
                    </th>
                    <th align="center" bgcolor="#bddcf4">
                        支付方式
                    </th>
                    <th align="center" bgcolor="#bddcf4">
                        收支款原因
                    </th>
                    <th align="center" bgcolor="#bddcf4">
                        手续费
                    </th>
                    <th align="center" bgcolor="#bddcf4">
                        附件查看
                    </th>
                    <th align="center" bgcolor="#bddcf4">
                        操作
                    </th>
                </tr>
                <asp:Repeater ID="rpBank" runat="server">
                    <ItemTemplate>
                        <tr tid="<%# Eval("DengZhangId") %>" class="<%#Container.ItemIndex%2==0?"even":"odd" %>">
                            <td height="30" align="center">
                                <%# Container.ItemIndex + 1 + (this.pageIndex - 1) * this.pageSize%>
                            </td>
                            <td align="center">
                                <%#Eval("DengZhangType") %>
                            </td>
                            <td align="center">
                                <%#Eval("TourNo")%>
                            </td>
                            <td align="center">
                                <%#ToDateTimeString(Eval("DengZhangDate"))%>
                                <%#Convert.ToBoolean(Eval("IsNew")) ? "<img src='/images/new.gif' alt='' />" : null %>
                            </td>
                            <td align="center">
                                <%#ToMoneyString(Eval("Price"))%>
                            </td>
                            <td align="center">
                                <%#Eval("BankName")%>
                            </td>
                            <td align="center">
                                <%#Eval("PayMode")%>
                            </td>
                            <td align="center">
                                <%#Eval("Reason")%>
                            </td>
                            <td align="center">
                                <%#ToMoneyString(Eval("OtherPrice"))%>
                            </td>
                            <td align="center">
                                <%#GetFilePath(Eval("File"))%>
                            </td>
                            <td align="center">
                                <a href="javascript:void(0);" class="show">查看 </a>
                                <% if (update)
                                   { %>
                                <%#Convert.ToBoolean( Eval("IsEdit"))?"<a href='javascript:void(0);' class='modify'>修改</a>":null %>
                                <%} if (del)
                                   { %>
                                <%#Convert.ToBoolean(Eval("IsEdit")) ? " <a href='javascript:void(0);' class='del'>删除</a>" : null%>
                                <%} %>
                            </td>
                        </tr>
                    </ItemTemplate>
                </asp:Repeater>
                <asp:Literal ID="Literal1" runat="server"></asp:Literal>
                <tr class="even">
                    <td height="30" colspan="4" align="right">
                        金额：
                    </td>
                    <td align="center">
                        <asp:Literal runat="server" ID="ltTotalMoney"></asp:Literal>
                    </td>
                    <td align="center">
                        &nbsp;
                    </td>
                    <td align="center">
                        &nbsp;
                    </td>
                    <td align="center">
                        &nbsp;
                    </td>
                    <td align="center">
                        <asp:Literal runat="server" ID="ltTotalShouXuMoney"></asp:Literal>
                    </td>
                    <td align="center">
                        &nbsp;
                    </td>
                    <td align="center">
                        &nbsp;
                    </td>
                </tr>
            </table>
            <table width="100%" border="0" cellspacing="0" cellpadding="0">
                <tr>
                    <td align="right">
                        <cc1:ExporPageInfoSelect ID="ExportPageInfo1" runat="server" />
                    </td>
                </tr>
            </table>
        </div>
    </div>
    <!-- InstanceEndEditable -->
    <div class="clearboth">
    </div>
    <!--mainbodyOut end-->

    <script type="text/javascript">
        $(function() {

            $("a.show").click(function() {
                var that = $(this);
                var url = "/Fin/ChuNaDengZhangAdd.aspx?dotype=show&";
                var tid = that.parent().parent().attr("tid");
                Boxy.iframeDialog({
                    iframeUrl: url + "tid=" + tid,
                    title: "查看",
                    modal: true,
                    width: "750x",
                    height: "420px"
                });
                return false;
            });

            $("a.add").click(function() {
                var url = "/Fin/ChuNaDengZhangAdd.aspx?dotype=add";
                Boxy.iframeDialog({
                    iframeUrl: url,
                    title: "登记",
                    modal: true,
                    width: "750x",
                    height: "420px"
                });
                return false;
            });

            $("a.modify").click(function() {
                var that = $(this);
                var url = "/Fin/ChuNaDengZhangAdd.aspx?dotype=update&";
                var tid = that.parent().parent().attr("tid");
                Boxy.iframeDialog({
                    iframeUrl: url + "tid=" + tid,
                    title: "修改",
                    modal: true,
                    width: "750x",
                    height: "420px"
                });
                return false;
            });

            $("a.del").click(function() {
                if (confirm("确认删除所选项?")) {
                    var that = $(this);
                    var tid = that.parent().parent().attr("tid");
                    $.newAjax({
                        type: "POST",
                        data: { "tid": tid },
                        url: "/Fin/ChuNaDengZhang.aspx?act=areadel",
                        dataType: 'json',
                        success: function(data) {
                            if (data.result) {
                                if (data.result == 1) {
                                    alert(data.msg);
                                    window.location.reload();
                                }
                                else {
                                    alert(data.msg);
                                }
                            }
                        },
                        error: function() {
                            alert("服务器繁忙!");
                        }
                    });
                }
                return false;
            });


            $("#btnSearch").click(function() {
                $("#frm").submit();
            });


        });
    </script>

</asp:Content>
