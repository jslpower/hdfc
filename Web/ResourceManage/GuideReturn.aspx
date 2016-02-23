<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="GuideReturn.aspx.cs" Inherits="Web.ResourceManage.WebForm1"
    MasterPageFile="~/MasterPage/Boxy.Master" %>

<%@ Register Assembly="ControlLibrary" Namespace="Adpost.Common.ExporPage" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="PageHead" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="PageBody" runat="server">
    <div style="width: 790px; margin: 10px auto;">
        <table width="100%" border="0" align="center" cellpadding="0" cellspacing="1">
            <tr class="odd">
                <th>
                    序号
                </th>
                <th>
                    反馈类型
                </th>
                <th height="30">
                    反馈时间
                </th>
                <th align="center">
                    反馈内容
                </th>
                <th width="100">
                    操作
                </th>
            </tr>
            <asp:Repeater runat="server" ID="rpGuideReturn">
                <ItemTemplate>
                    <tr tid="<%#Eval("Id") %>" type=" <%#Eval("FanKuiType")%>" time="<%#this.ToDateTimeString(Eval("FanKuiTime")) %>"
                        remark="<%#Eval("FanKuiRemark")%>" bgcolor="<%# Container.ItemIndex%2==0?"#e3f1fc":"#BDDCF4" %>">
                        <td align="center">
                            <%# Container.ItemIndex + 1 + (this.pageIndex - 1) * this.pageSize%>
                        </td>
                        <td align="center">
                            <%#Eval("FanKuiType")%>
                        </td>
                        <td height="30" align="center">
                            <%#this.ToDateTimeString(Eval("FanKuiTime")) %>
                        </td>
                        <td align="center">
                            <%#Eval("FanKuiRemark")%>
                        </td>
                        <td align="center">
                            <% if (update)
                               { %><a href="javascript:void(0);" class="modify">修改</a>
                            <%} if (del)
                               { %><a href="javascript:void(0);" class="del">删除</a><%} %>
                        </td>
                    </tr>
                </ItemTemplate>
            </asp:Repeater>
        </table>
        <table width="100%" border="0" cellspacing="0" cellpadding="0">
            <tr>
                <td height="30" align="right" class="pageup" colspan="13">
                    <cc1:ExporPageInfoSelect ID="ExporPageInfoSelect1" runat="server" LinkType="3" PageStyleType="NewButton"
                        CurrencyPageCssClass="RedFnt" />
                </td>
            </tr>
        </table>
        <form id="form1" runat="server">
        <table width="100%" border="0" align="center" cellpadding="0" cellspacing="1" style="margin-top: 10px;">
            <tr class="odd">
                <th width="120" height="30" align="right">
                    &nbsp;反馈类型：
                </th>
                <td bgcolor="#E3F1FC">
                    <asp:DropDownList ID="ddlFanKuiType" runat="server" CssClass="inputselect" Width="120">
                    </asp:DropDownList>
                </td>
            </tr>
            <tr class="even">
                <th height="30" align="right">
                    反馈时间：
                </th>
                <td bgcolor="#E3F1FC">
                    <input name="txtFanKuiTime" type="text" class="inputtext" style="width: 120px;" id="txtFanKuiTime"
                        onfocus="WdatePicker()" runat="server" valid="required" errmsg="反馈时间不能为空！" />
                </td>
            </tr>
            <tr class="odd">
                <th align="right">
                    反馈内容：
                </th>
                <td bgcolor="#E3F1FC">
                    <textarea name="txtFanKuiRemark" cols="80" rows="3" class="inputarea" id="txtFanKuiRemark"
                        runat="server" valid="required" errmsg="反馈内容不能为空！"></textarea>
                </td>
            </tr>
        </table>
        <table width="320" border="0" align="center" cellpadding="0" cellspacing="0">
            <tr>
                <td height="40" align="center" class="tjbtn02">
                    <input type="hidden" name="guideId" runat="server" id="hdguideId" />
                    <input type="hidden" name="fankuiId" runat="server" id="hdfankuiId" />
                    <% if (add)
                       { %>
                    <a href="javascript:;" class="add">保 存</a>
                    <% }%>
                </td>
            </tr>
        </table>
        </form>
    </div>

    <script type="text/javascript">
        $(function() {
            GuideReturn.PageInit();
        });
        var GuideReturn = {
            PageInit: function() {
                $("a.add").click(function() {
                    if (!GuideReturn.CheckForm()) {
                        return false;
                    }
                    var url = "/ResourceManage/GuideReturn.aspx?Type=save";
                    GuideReturn.GoAjax(url);
                    return false;
                });
                $("a.modify").click(function() {
                    GuideReturn.Update();
                });
                $("a.del").click(function() {
                    var id = $(this).closest("tr").attr("tid");
                    GuideReturn.Delete(id);
                });
            },
            GoAjax: function(url) {
                GuideReturn.UnBind();
                $.newAjax({
                    type: "post",
                    cache: false,
                    url: url,
                    dataType: "json",
                    data: $("a.add").closest("form").serialize(),
                    success: function(ret) {
                        if (ret.result == "1") {
                            parent.tableToolbar._showMsg(ret.msg, function() {
                                parent.location.href = parent.location.href;
                            });
                        }
                        else {
                            parent.tableToolbar._showMsg(ret.msg);
                            GuideReturn.Bind();
                        }
                    },
                    error: function() {
                        parent.tableToolbar._showMsg(tableToolbar.errorMsg);
                        GuideReturn.Bind();
                    }
                });
            },
            CheckForm: function() {
                return ValiDatorForm.validator($("a.add").closest("form").get(0), "parent");
            },
            Bind: function() {
                var _selfs = $("a.add");
                _selfs.html("保存");
                _selfs.css("cursor", "pointer");
                _selfs.click(function() {
                    if (!GuideAddPage.CheckForm()) {
                        return false;
                    }
                    var url = "/ResourceManage/GuideReturn.aspx?Type=save";
                    GuideAddPage.GoAjax(url);
                    return false;
                });
            },
            UnBind: function() {
                $("a.add").html("提交中...");
                $("a.add").unbind("click");
            },
            Update: function() {
                var _this = $("a.modify");
                $("#<%=this.hdfankuiId.ClientID %>").val(_this.closest("tr").attr("tid"));
                $("#<%=this.ddlFanKuiType.ClientID %>").val(_this.closest("tr").attr("type"));
                $("#<%=this.txtFanKuiTime.ClientID %>").val(_this.closest("tr").attr("time"));
                $("#<%=this.txtFanKuiRemark.ClientID %>").val(_this.closest("tr").attr("remark"));
            },
            Delete: function(id) {
                if (confirm("确认删除所选项?")) {
                    $.newAjax({
                        type: "post",
                        url: "/ResourceManage/GuideReturn.aspx?Type=del",
                        data: "id=" + id,
                        dataType: "json",
                        success: function(data) {
                            if (data.result == "1") {
                                parent.tableToolbar._showMsg(data.msg, function() {
                                    window.location.reload();
                                });
                            }
                            else {
                                parent.tableToolbar._showMsg(data.msg);
                            }
                        },
                        error: function() {
                            tableToolbar._showMsg("服务器忙！");
                        }
                    });
                }
            }
        }
    </script>

</asp:Content>
