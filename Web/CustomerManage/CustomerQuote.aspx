<%@ Page Title="客户日常询价" Language="C#" MasterPageFile="~/MasterPage/Front.Master"
    AutoEventWireup="true" CodeBehind="CustomerQuote.aspx.cs" Inherits="Web.CustomerManage.CustomerQuote" %>

<%@ Register Assembly="ControlLibrary" Namespace="Adpost.Common.ExporPage" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="lineprotitlebox">
        <table width="100%" border="0" cellspacing="0" cellpadding="0">
            <tr>
                <td width="15%" nowrap="nowrap">
                    <span class="lineprotitle">客户日常询价</span>
                </td>
                <td width="85%" nowrap="nowrap" align="right" style="padding: 0 10px 2px 0; color: #13509f;">
                    <b>当前您所在位置：</b>>> 客户询价 >> 客户日常询价
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
    <form id="form1" action="" method="get">
    <table width="99%" border="0" align="center" cellpadding="0" cellspacing="0">
        <tr>
            <td width="10" valign="top">
                <img src="/images/yuanleft.gif" />
            </td>
            <td>
                <div class="searchbox">
                    询价时间：
                    <input name="st" type="text" class="searchinput formsize70 inputtext" id="st" size="12"
                        onfocus="WdatePicker()" />
                    -
                    <input name="et" type="text" class="searchinput formsize70 inputtext" id="et" size="12"
                        onfocus="WdatePicker()" />
                    询价单位：
                    <input name="cName" type="text" class="searchinput formsize100 inputtext" id="cName"
                        size="20" />
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
        <table border="0" align="left" cellpadding="0" cellspacing="0">
            <tr>
                <td width="90" align="center">
                    <a id="a_AddQuote" href="javascript:void(0);">新增</a>
                </td>
                <td width="90" align="center">
                    <a id="a_EditQuote" href="javascript:void(0);">修改</a>
                </td>
                <td width="90" align="center">
                    <a id="a_DelQuote" href="javascript:void(0);">删除</a>
                </td>
            </tr>
        </table>
    </div>
    <div class="tablelist">
        <table width="100%" border="0" cellpadding="0" cellspacing="1" id="liststyle">
            <tr>
                <th width="60" height="30" align="center">
                    <input type="checkbox" name="ckbAll" id="ckbAll" /><label for="ckbAll">全选</label>
                </th>
                <th align="center">
                    出团日期
                </th>
                <th align="center">
                    人数
                </th>
                <th align="center">
                    询价单位
                </th>
                <th align="center">
                    询价时间
                </th>
                <th align="center">
                    询价人
                </th>
                <th align="center">
                    联系电话
                </th>
                <th align="center">
                    手机
                </th>
                <th align="center">
                    计调员
                </th>
                <th align="center">
                    查看
                </th>
            </tr>
            <asp:Repeater runat="server" ID="rptQuote">
                <ItemTemplate>
                    <tr class="<%# Container.ItemIndex % 2 == 0 ? "even" : "odd" %>" data-quoteid="<%# Eval("QuoteId") %>">
                        <td height="30" align="center">
                            <input type="checkbox" name="ckbIndex" id="ckb<%# Container.ItemIndex %>" />
                            <%# GetLineIndex(Container.ItemIndex) %>
                        </td>
                        <td align="center">
                            <%# this.ToDateTimeString(Eval("LeaveDate"))%>
                        </td>
                        <td align="center">
                            <%# Eval("PeopleNum")%>
                        </td>
                        <td align="center">
                            <%# Eval("Costomer")%><sup><font class="fred font14"><%# Eval("YearQuoteCount")%></font></sup>
                        </td>
                        <td align="center">
                            <%# this.ToDateTimeString(Eval("QuoteDate"))%>
                        </td>
                        <td align="center">
                            <%# Eval("ContactName")%>
                        </td>
                        <td align="center">
                            <%# Eval("ContactTel")%>
                        </td>
                        <td align="center">
                            <%# Eval("ContactMobile")%>
                        </td>
                        <td align="center">
                            <%# Eval("Operator")%>
                        </td>
                        <td align="center">
                            <a href="javascript:void(0);" class="a_SeeQuote">查看</a>
                        </td>
                    </tr>
                </ItemTemplate>
            </asp:Repeater>
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

        var QuoteManage = {
            checkedValue: [],
            reload: function() {
                window.location.href = window.location.href;
            },
            GetCheckedValue: function() {
                QuoteManage.checkedValue.length = 0;
                $("#liststyle").find("input[name='ckbIndex']:checked").each(function() {
                    QuoteManage.checkedValue.push($.trim($(this).closest("tr").attr("data-quoteId")));
                });
            },
            AddQuote: function() {
                var _data = { action: "add" };
                Boxy.iframeDialog({
                    title: "添加客户日常询价",
                    iframeUrl: "/CustomerManage/CustomerQuoteEdit.aspx",
                    data: _data,
                    width: "790px",
                    height: "330px",
                    afterHide: function() {
                        QuoteManage.reload();
                    }
                });
                return false;
            },
            EditQuote: function() {
                QuoteManage.GetCheckedValue();
                if (QuoteManage.checkedValue == null || QuoteManage.checkedValue.length == 0) {
                    tableToolbar._showMsg("请选择要修改的询价！");
                    return false;
                }
                if (QuoteManage.checkedValue != null && QuoteManage.checkedValue.length > 1) {
                    tableToolbar._showMsg("只能选择一条询价信息进行修改！");
                    return false;
                }
                var _data = { action: "edit", qId: QuoteManage.checkedValue[0] };
                Boxy.iframeDialog({
                    title: "修改客户日常询价",
                    iframeUrl: "/CustomerManage/CustomerQuoteEdit.aspx",
                    data: _data,
                    width: "790px",
                    height: "330px",
                    afterHide: function() {
                        QuoteManage.reload();
                    }
                });
                return false;
            },
            DelQuote: function() {
                QuoteManage.GetCheckedValue();
                if (QuoteManage.checkedValue == null || QuoteManage.checkedValue.length == 0) {
                    tableToolbar._showMsg("请选择要删除的询价！");
                    return false;
                }
                var _data = { action: "del", qId: QuoteManage.checkedValue.join(',') };
                tableToolbar.ShowConfirmMsg("您确定要删除选中的询价信息吗？", function() {
                    $.newAjax({
                        type: "post",
                        cache: false,
                        async: false,
                        url: "/CustomerManage/CustomerQuote.aspx?" + $.param(_data),
                        dataType: "json",
                        success: function(ret) {
                            if (ret.result == "1") {
                                tableToolbar._showMsg(ret.msg, function() {
                                    QuoteManage.reload();
                                });
                            }
                            else {
                                tableToolbar._showMsg(ret.msg);
                            }
                        },
                        error: function() { tableToolbar._showMsg(tableToolbar.errorMsg); }
                    });
                });
                return false;
            },
            SeeQuote: function(obj) {
                var _data = { action: "see", qId: $.trim($(obj).closest("tr").attr("data-quoteId")) };
                Boxy.iframeDialog({
                    title: "查看客户日常询价",
                    iframeUrl: "/CustomerManage/CustomerQuoteEdit.aspx",
                    data: _data,
                    width: "790px",
                    height: "330px"
                });
                return false;
            }
        };

        $(document).ready(function() {
            utilsUri.initSearch();
            tableToolbar.init({ tableContainerSelector: "#liststyle" });

            $("#a_AddQuote").click(function() {
                QuoteManage.AddQuote();
                return false;
            });
            $("#a_EditQuote").click(function() {
                QuoteManage.EditQuote();
                return false;
            });
            $("#a_DelQuote").click(function() {
                QuoteManage.DelQuote();
                return false;
            });
            $("#liststyle").find(".a_SeeQuote").each(function() {
                $(this).click(function() {
                    QuoteManage.SeeQuote(this);
                    return false;
                });
            });
        });
    </script>

</asp:Content>
