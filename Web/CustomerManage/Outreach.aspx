<%@ Page Title="外联每日足迹" Language="C#" MasterPageFile="~/MasterPage/Front.Master" AutoEventWireup="true"
    CodeBehind="Outreach.aspx.cs" Inherits="Web.CustomerManage.Outreach" %>

<%@ Register Assembly="ControlLibrary" Namespace="Adpost.Common.ExporPage" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="lineprotitlebox">
        <table width="100%" border="0" cellspacing="0" cellpadding="0">
            <tr>
                <td width="15%" nowrap="nowrap">
                    <span class="lineprotitle">外联每天足迹</span>
                </td>
                <td width="85%" nowrap="nowrap" align="right" style="padding: 0 10px 2px 0; color: #13509f;">
                    <b>当前您所在位置</b>>> 客户询价 >> 外联每天足迹
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
                    销售日期：
                    <input name="st" type="text" class="searchinput formsize70 inputtext" id="st" size="12"
                        onfocus="WdatePicker()" />
                    -
                    <input name="et" type="text" class="searchinput formsize70 inputtext" id="et" size="12"
                        onfocus="WdatePicker()" />
                    销售单位：
                    <input name="sName" type="text" class="searchinput formsize100 inputtext" id="sName"
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
                    <a id="a_AddQutreach" href="javascript:void(0);">新增</a>
                </td>
                <td width="90" align="center">
                    <a id="a_EditQutreach" href="javascript:void(0);">修改</a>
                </td>
                <td width="90" align="center">
                    <a id="a_DelQutreach" href="javascript:void(0);">删除</a>
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
                    销售日期
                </th>
                <th align="center">
                    销售单位
                </th>
                <th align="center">
                    销售人
                </th>
                <th align="center">
                    联系电话
                </th>
                <th align="center">
                    单位地址
                </th>
                <th align="center">
                    查看
                </th>
            </tr>
            <asp:Repeater runat="server" ID="rptQutreach">
                <ItemTemplate>
                    <tr class="<%# Container.ItemIndex % 2 == 0 ? "even" : "odd" %>" data-qutreachid="<%# Eval("Id") %>">
                        <td height="30" align="center">
                            <input type="checkbox" name="ckbIndex" id="ckb<%# Container.ItemIndex %>" />
                            <%# GetLineIndex(Container.ItemIndex) %>
                        </td>
                        <td align="center">
                            <%# this.ToDateTimeString(Eval("SaleDate"))%>
                        </td>
                        <td align="center">
                            <%# Eval("SaleUnit")%>
                        </td>
                        <td align="center">
                            <%# Eval("SaleName")%>
                        </td>
                        <td align="center">
                            <%# Eval("Tel")%>
                        </td>
                        <td align="center">
                            <%# Eval("Address")%>
                        </td>
                        <td align="center">
                            <a href="javascript:void(0);" class="a_SeeQutreach">查看</a>
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

        var QutreachManage = {
            checkedValue: [],
            reload: function() {
                window.location.href = window.location.href;
            },
            GetCheckedValue: function() {
                QutreachManage.checkedValue.length = 0;
                $("#liststyle").find("input[name='ckbIndex']:checked").each(function() {
                    QutreachManage.checkedValue.push($.trim($(this).closest("tr").attr("data-qutreachid")));
                });
            },
            AddQuote: function() {
                var _data = { action: "add" };
                Boxy.iframeDialog({
                    title: "添加外联每日足迹",
                    iframeUrl: "/CustomerManage/OutreachEdit.aspx",
                    data: _data,
                    width: "790px",
                    height: "330px",
                    afterHide: function() {
                        QutreachManage.reload();
                    }
                });
                return false;
            },
            EditQuote: function() {
                QutreachManage.GetCheckedValue();
                if (QutreachManage.checkedValue == null || QutreachManage.checkedValue.length == 0) {
                    tableToolbar._showMsg("请选择要修改的外联每日足迹！");
                    return false;
                }
                if (QutreachManage.checkedValue != null && QutreachManage.checkedValue.length > 1) {
                    tableToolbar._showMsg("只能选择一条外联每日足迹信息进行修改！");
                    return false;
                }
                var _data = { action: "edit", qId: QutreachManage.checkedValue[0] };
                Boxy.iframeDialog({
                    title: "修改外联每日足迹",
                    iframeUrl: "/CustomerManage/OutreachEdit.aspx",
                    data: _data,
                    width: "790px",
                    height: "330px",
                    afterHide: function() {
                        QutreachManage.reload();
                    }
                });
                return false;
            },
            DelQuote: function() {
                QutreachManage.GetCheckedValue();
                if (QutreachManage.checkedValue == null || QutreachManage.checkedValue.length == 0) {
                    tableToolbar._showMsg("请选择要删除的外联每日足迹！");
                    return false;
                }
                var _data = { action: "del", qId: QutreachManage.checkedValue.join(',') };
                tableToolbar.ShowConfirmMsg("您确定要删除选中的外联每日足迹信息吗？", function() {
                    $.newAjax({
                        type: "post",
                        cache: false,
                        async: false,
                        url: "/CustomerManage/Outreach.aspx?" + $.param(_data),
                        dataType: "json",
                        success: function(ret) {
                            if (ret.result == "1") {
                                tableToolbar._showMsg(ret.msg, function() {
                                    QutreachManage.reload();
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
                var _data = { action: "see", qId: $.trim($(obj).closest("tr").attr("data-qutreachid")) };
                Boxy.iframeDialog({
                    title: "查看外联每日足迹",
                    iframeUrl: "/CustomerManage/OutreachEdit.aspx",
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

            $("#a_AddQutreach").click(function() {
                QutreachManage.AddQuote();
                return false;
            });
            $("#a_EditQutreach").click(function() {
                QutreachManage.EditQuote();
                return false;
            });
            $("#a_DelQutreach").click(function() {
                QutreachManage.DelQuote();
                return false;
            });
            $("#liststyle").find(".a_SeeQutreach").each(function() {
                $(this).click(function() {
                    QutreachManage.SeeQuote(this);
                    return false;
                });
            });
        });
    </script>

</asp:Content>
