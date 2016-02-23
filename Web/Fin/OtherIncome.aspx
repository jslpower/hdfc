<%@ Page Title="其它收入-财务管理" Language="C#" AutoEventWireup="true" CodeBehind="OtherIncome.aspx.cs"
    Inherits="Web.Fin.OtherIncome" MasterPageFile="~/MasterPage/Front.Master" %>

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
                        <b>当前用您所在位置：</b> >> 财务管理 >> 其它收入
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
                    <form id="frm" action="/Fin/OtherIncome.aspx" method="get">
                    <div class="searchbox">
                        <label>
                            时间：</label>
                        <input type="text" id="txtBeginDate" name="txtBeginDate" class="inputtext" onfocus="WdatePicker()"
                            value="<%=EyouSoft.Common.Utils.GetQueryStringValue("txtBeginDate")%>" />
                        —
                        <input type="text" id="txtEndDate" name="txtEndDate" class="inputtext" onfocus="WdatePicker()"
                            value="<%=EyouSoft.Common.Utils.GetQueryStringValue("txtEndDate")%>" />
                        <label>
                            收入项目：</label>
                        <input type="text" id="txtOtherIncomeItem" class="inputtext" name="txtOtherIncomeItem"
                            value="<%=EyouSoft.Common.Utils.GetQueryStringValue("txtOtherIncomeItem") %>" />
                        <a href="javascript:;" id="btnSearch">
                            <img src="../images/searchbtn.gif" style="vertical-align: top;" alt="" /></a></label>
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
                    <th width="36" height="30" align="center">
                        序号
                    </th>
                    <th align="center" bgcolor="#BDDCF4">
                        收入日期
                    </th>
                    <th align="center" bgcolor="#bddcf4">
                        收入项目
                    </th>
                    <th align="center" bgcolor="#bddcf4">
                        收入金额
                    </th>
                    <th align="center" bgcolor="#bddcf4">
                        收款人
                    </th>
                    <th align="center" bgcolor="#bddcf4">
                        银行账号
                    </th>
                    <th align="center">
                        支付方式
                    </th>
                    <th width="110" align="center" bgcolor="#bddcf4">
                        附件查看
                    </th>
                    <th width="110" align="center" bgcolor="#bddcf4">
                        操作
                    </th>
                </tr>
                <asp:Repeater ID="rpBank" runat="server">
                    <ItemTemplate>
                        <tr tid="<%#Eval("DengJiId") %>" class="<%#Container.ItemIndex%2==0?"even":"odd" %>">
                            <td height="30" align="center">
                                <%# Container.ItemIndex + 1 + (this.pageIndex - 1) * this.pageSize%>
                            </td>
                            <td align="center">
                                <%#ToDateTimeString(Eval("ShouKuanRiQi"))%>
                            </td>
                            <td align="center">
                                <%#Eval("ItemName")%>
                            </td>
                            <td align="center">
                                <%#ToMoneyString(Eval("JinE"))%>
                            </td>
                            <td align="center">
                                <%#Eval("ShouKuanRenName")%>
                            </td>
                            <td align="center">
                                <%#Eval("ZhangHuCode")%>
                            </td>
                            <td align="center">
                                <%#Eval("FangShi")%>
                            </td>
                            <td align="center">
                                <%#GetFilePath(Eval("File"))%>
                            </td>
                            <td align="center">
                                <a href="javascript:void(0);" class="show">查看 </a>
                                <% if (update)
                                   { %>
                                <a href='javascript:void(0);' class='modify'>修改</a>
                                <%} if (del)
                                   { %>
                                <a href='javascript:void(0);' class='del'>删除</a>
                                <%} %>
                            </td>
                        </tr>
                    </ItemTemplate>
                </asp:Repeater>
                <asp:Literal ID="Literal1" runat="server"></asp:Literal>
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
                var url = "/Fin/OtherIncomeAdd.aspx?dotype=show&";
                var tid = that.parent().parent().attr("tid");
                Boxy.iframeDialog({
                    iframeUrl: url + "tid=" + tid,
                    title: "查看",
                    modal: true,
                    width: "650x",
                    height: "295px"
                });
                return false;
            });

            $("a.add").click(function() {
                var url = "/Fin/OtherIncomeAdd.aspx?dotype=add";
                Boxy.iframeDialog({
                    iframeUrl: url,
                    title: "登记",
                    modal: true,
                    width: "650x",
                    height: "295px"
                });
                return false;
            });


            $("a.modify").click(function() {
                var that = $(this);
                var url = "/Fin/OtherIncomeAdd.aspx?dotype=update&";
                var tid = that.parent().parent().attr("tid");
                Boxy.iframeDialog({
                    iframeUrl: url + "tid=" + tid,
                    title: "修改",
                    modal: true,
                    width: "650x",
                    height: "295px"
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
                        url: "/Fin/OtherIncome.aspx?act=areadel",
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
