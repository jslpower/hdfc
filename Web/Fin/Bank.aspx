<%@ Page Title="银行余额-财务管理" Language="C#" AutoEventWireup="true" CodeBehind="Bank.aspx.cs"
    Inherits="Web.Fin.Bank" MasterPageFile="~/MasterPage/Front.Master" %>

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
                        <b>当前用您所在位置：</b> >> 财务管理 >> 银行余额
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
                    <form id="frm" action="/Fin/Bank.aspx" method="get">
                    <div class="searchbox">
                        <label>
                            登记日期：</label>
                        <input type="text" id="txtBeginDate" name="txtBeginDate" class="inputtext" onfocus="WdatePicker()"
                            value="<%=EyouSoft.Common.Utils.GetQueryStringValue("txtBeginDate")%>" />
                        —
                        <input type="text" id="txtEndDate" name="txtEndDate" class="inputtext" onfocus="WdatePicker()"
                            value="<%=EyouSoft.Common.Utils.GetQueryStringValue("txtEndDate")%>" />
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
                    <th width="36" height="30" align="center" bgcolor="#BDDCF4">
                        序号
                    </th>
                    <th align="center" bgcolor="#BDDCF4">
                        日期
                    </th>
                    <th align="center" bgcolor="#bddcf4">
                        银行账号
                    </th>
                    <th align="center" bgcolor="#bddcf4">
                        账户余额
                    </th>
                    <th align="center" bgcolor="#bddcf4">
                        操作
                    </th>
                </tr>
                <asp:Repeater ID="rpBank" runat="server">
                    <ItemTemplate>
                        <tr tid="<%# Eval("Id") %>" class="<%#Container.ItemIndex%2==0?"even":"odd" %>">
                            <td height="30" align="center">
                                <%# Container.ItemIndex + 1 + (this.pageIndex - 1) * this.pageSize%>
                            </td>
                            <td align="center">
                                <%#this.ToDateTimeString( Eval("Date"))%>
                            </td>
                            <td align="center">
                                <%#Eval("BankName")%>
                            </td>
                            <td align="center">
                                <%#this.ToMoneyString(Eval("Balance")) %>
                            </td>
                             <td align="center">
                                <%if (del)
                                  { 
                                %>
                                <a href="javascript:void(0);" class="del">删除</a>
                                <%   
                                    } %>
                            </td>
                        </tr>
                    </ItemTemplate>
                </asp:Repeater>
                <asp:Literal ID="Literal1" runat="server"></asp:Literal>
                <tr>
                    <td height="30" colspan="3" align="right" bgcolor="#E3F1FC">
                        合计：
                    </td>
                    <td align="center" bgcolor="#E3F1FC">
                        <asp:Literal runat="server" ID="ltTotalMoney"></asp:Literal>
                    </td>
                    <td bgcolor="#E3F1FC">&nbsp;</td>
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
            $("a.add").click(function() {
                var url = "/Fin/BankAdd.aspx?dotype=add";
                Boxy.iframeDialog({
                    iframeUrl: url,
                    title: "登记",
                    modal: true,
                    width: "650px",
                    height: "200px"
                });
                return false;
            });

            $("#btnSearch").click(function() {
                $("#frm").submit();
            });


            $("a.del").click(function() {
                if (confirm("确认删除所选项?")) {
                    var that = $(this);
                    var tid = that.parent().parent().attr("tid");
                    $.newAjax({
                        type: "POST",
                        data: { "tid": tid },
                        url: "/Fin/Bank.aspx?act=areadel",
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

        });
    </script>

</asp:Content>
