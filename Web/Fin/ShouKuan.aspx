<%@ Page Title="收款登记" Language="C#" MasterPageFile="~/MasterPage/Boxy.Master" AutoEventWireup="true"
    CodeBehind="ShouKuan.aspx.cs" Inherits="Web.Fin.ShouKuan" %>

<%@ Register Src="/UserControl/UploadControl.ascx" TagName="UploadControl" TagPrefix="uc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="PageHead" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="PageBody" runat="server">
    <div style="width: 940px; margin: 10px auto;">
        <span class="formtableT">收款记录</span>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
        应收金额：<span class="fred"><asp:Literal runat="server" ID="ltrYingShou"></asp:Literal></span>&nbsp;&nbsp;&nbsp;&nbsp;
        已收金额：<asp:Literal runat="server" ID="ltrYiShou"></asp:Literal>
        <table width="100%" border="0" align="center" cellpadding="0" cellspacing="1" id="liststyle">
            <tr class="odd">
                <th height="30">
                    <p>
                        收款日期</p>
                </th>
                <th>
                    操作人
                </th>
                <th>
                    金额
                </th>
                <th align="center">
                    银行账号
                </th>
                <th align="center">
                    支付方式
                </th>
                <th>
                    是否开票
                </th>
                <th>
                    附件
                </th>
                <th>
                    手续费
                </th>
                <th>
                    备注
                </th>
                <th width="100">
                    操作
                </th>
            </tr>
            <asp:Repeater runat="server" ID="rptShouKuan">
                <ItemTemplate>
                    <tr class="<%# Container.ItemIndex % 2 == 0 ? "even" : "odd"%>" data-id="<%#Eval("DengJiId") %>">
                        <td height="30" align="center">
                            <%# this.ToDateTimeString(Eval("ShouKuanRiQi"))%>
                        </td>
                        <td align="center">
                            <%# Eval("ShouKuanRenName")%>
                        </td>
                        <td align="center">
                            <%# this.ToMoneyString(Eval("JinE"))%>
                        </td>
                        <td align="center" class="pandl3">
                            <%# Eval("ZhangHuCode")%>
                        </td>
                        <td align="center">
                            <%# Eval("FangShi")%>
                        </td>
                        <td align="center">
                            <%# (bool)Eval("IsKaiPiao") ? "已开票" : "未开票"%>
                        </td>
                        <td align="center">
                            <%# GetFilePath(Eval("File")) %>
                        </td>
                        <td align="center">
                            <%# this.ToMoneyString(Eval("OtherPrice"))%>
                        </td>
                        <td align="center">
                            <%# Eval("ShouKuanBeiZhu")%>
                        </td>
                        <td align="center">
                            <% if(IsShowEdit) {%>
                            <a href="javascript:void(0);" class="a_EditShouKuan">修改</a>&nbsp;
                            <% } %>
                            <% if (IsShowDel){%>
                            <a href="javascript:void(0);" class="a_DelShouKuan">删除</a>
                            <% } %>
                        </td>
                    </tr>
                </ItemTemplate>
            </asp:Repeater>
        </table>
        <form id="form1" runat="server">
        <table width="100%" border="0" align="center" cellpadding="0" cellspacing="1" style="margin-top: 10px;">
            <tr class="odd">
                <th height="30" align="right">
                    <span class="fred">*</span>收款日期：
                </th>
                <td bgcolor="#E3F1FC">
                    <input name="txtDate" runat="server" type="text" class="formsize100 inputtext" id="txtDate"
                        onfocus="WdatePicker()" valid="required|isDate" errmsg="请选择收款日期|收款日期格式错误" />
                </td>
                <th align="right">
                    <span class="fred">*</span>操作人：
                </th>
                <td bgcolor="#E3F1FC">
                    <input name="txtOperator" runat="server" type="text" class="formsize100 inputtext"
                        id="txtOperator" valid="required" errmsg="请填写操作人" readonly="readonly" />
                    <input type="hidden" runat="server" id="hidOperatorId" />
                </td>
            </tr>
            <tr class="odd">
                <th width="120" height="30" align="right">
                    <span class="fred">*</span>金额：
                </th>
                <td bgcolor="#E3F1FC">
                    <input name="txtMoney" runat="server" type="text" class="formsize80 inputtext" id="txtMoney"
                        valid="required|isNumber" errmsg="请填写金额|金额格式不正确" />
                </td>
                <th width="120" align="right">
                    <span class="fred">*</span>银行账号：
                </th>
                <td bgcolor="#E3F1FC">
                    <asp:DropDownList runat="server" ID="ddlBankId" CssClass="inputselect" valid="isNo"
                        noValue="-1" errmsg="请选择银行账号">
                    </asp:DropDownList>
                </td>
            </tr>
            <tr class="odd">
                <th height="30" align="right">
                    支付方式：
                </th>
                <td bgcolor="#E3F1FC">
                    <asp:DropDownList runat="server" ID="ddlFangShi" CssClass="inputselect">
                    </asp:DropDownList>
                </td>
                <th align="right">
                    是否开票：
                </th>
                <td bgcolor="#E3F1FC">
                    <asp:DropDownList runat="server" ID="ddlKaiPiao" CssClass="inputselect">
                        <asp:ListItem Text="未开票" Value="0"></asp:ListItem>
                        <asp:ListItem Text="已开票" Value="1"></asp:ListItem>
                    </asp:DropDownList>
                </td>
            </tr>
            <tr class="odd">
                <th height="30" align="right">
                    附件：
                </th>
                <td colspan="3" bgcolor="#E3F1FC">
                    <uc1:UploadControl ID="UploadControl1" runat="server" IsUploadMore="false" IsUploadSelf="true" />
                </td>
            </tr>
            <tr class="odd">
                <th height="30" align="right">
                    手续费：
                </th>
                <td colspan="3" bgcolor="#E3F1FC">
                    <input name="txtOtherPrice" runat="server" type="text" class="formsize80 inputtext"
                        id="txtOtherPrice" valid="isMoney" errmsg="手续费格式不正确" />
                </td>
            </tr>
            <tr class="odd">
                <th align="right">
                    备注：
                </th>
                <td colspan="3" bgcolor="#E3F1FC">
                    <textarea name="txtRemark" runat="server" rows="3" class="formsize450 inputarea"
                        id="txtRemark"></textarea>
                </td>
            </tr>
        </table>
        <table width="100%" border="0" align="center" cellpadding="0" cellspacing="0" style="margin: 10px auto;">
            <tr class="odd">
                <td height="40" colspan="14" bgcolor="#E3F1FC">
                    <table border="0" align="center" cellpadding="0" cellspacing="0">
                        <tr>
                            <td width="80" align="center" class="tjbtn02">
                                <a href="javascript:void(0);" id="a_SaveShouKuan">保存</a>
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
        </table>
        </form>
    </div>

    <script type="text/javascript">
        var ShouKuanDengJi = {
            _data: {
                action: '<%= EyouSoft.Common.Utils.GetQueryStringValue("action") %>',
                itemId: '<%= EyouSoft.Common.Utils.GetQueryStringValue("itemId") %>',
                sId: '<%= EyouSoft.Common.Utils.GetQueryStringValue("sId") %>',
                iframeId: '<%= EyouSoft.Common.Utils.GetQueryStringValue("iframeId") %>'
            },
            reload: function() {
                var tmp = {
                    itemId: '<%= EyouSoft.Common.Utils.GetQueryStringValue("itemId") %>',
                    iframeId: '<%= EyouSoft.Common.Utils.GetQueryStringValue("iframeId") %>'
                };
                window.location.href = "/Fin/ShouKuan.aspx?" + $.param(tmp);
            },
            BindListEdit: function() {
                $("#liststyle").find(".a_EditShouKuan").each(function() {
                    var tmp = {
                        action: "edit",
                        itemId: '<%= EyouSoft.Common.Utils.GetQueryStringValue("itemId") %>',
                        sId: $.trim($(this).closest("tr").attr("data-id")),
                        iframeId: '<%= EyouSoft.Common.Utils.GetQueryStringValue("iframeId") %>'
                    };
                    $(this).click(function() {
                        window.location.href = "/Fin/ShouKuan.aspx?" + $.param(tmp);
                        return false;
                    });
                });
            },
            BindListDel: function() {
                $("#liststyle").find(".a_DelShouKuan").each(function() {
                    var tmp = {
                        action: "del",
                        itemId: '<%= EyouSoft.Common.Utils.GetQueryStringValue("itemId") %>',
                        sId: $.trim($(this).closest("tr").attr("data-id")),
                        iframeId: '<%= EyouSoft.Common.Utils.GetQueryStringValue("iframeId") %>'
                    };
                    $(this).click(function() {
                        tableToolbar.ShowConfirmMsg("您确定要删除此收款信息吗？", function() {
                            $.newAjax({
                                type: "post",
                                cache: false,
                                async: false,
                                url: "/Fin/ShouKuan.aspx?" + $.param(tmp),
                                dataType: "json",
                                success: function(ret) {
                                    if (ret.result == "1") {
                                        tableToolbar._showMsg(ret.msg, function() {
                                            ShouKuanDengJi.reload();
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
                    });
                });
            },
            CheckForm: function() {
                return ValiDatorForm.validator($("#a_SaveShouKuan").closest("form").get(0), "parent");
            },
            BindBtn: function() {
                $("#a_SaveShouKuan").html("保存").css({ "color": "" }).click(function() {
                    ShouKuanDengJi.Save();
                    return false;
                });
            },
            Save: function() {
                if (!ShouKuanDengJi.CheckForm()) {
                    return false;
                }
                $("#a_SaveShouKuan").html("正在提交").css({ "color": "#999999" }).unbind("click");

                $.newAjax({
                    type: "post",
                    cache: false,
                    dataType: "json",
                    url: "/Fin/ShouKuan.aspx?save=1&" + $.param(ShouKuanDengJi._data),
                    data: $("#a_SaveShouKuan").closest("form").serialize(),
                    success: function(ret) {
                        if (ret.result == "1") {
                            tableToolbar._showMsg(ret.msg, function() {
                                ShouKuanDengJi.reload();
                            });
                        }
                        else {
                            tableToolbar._showMsg(ret.msg);
                            ShouKuanDengJi.BindBtn();
                        }
                    },
                    error: function() {
                        tableToolbar._showMsg(tableToolbar.errorMsg);
                        ShouKuanDengJi.BindBtn();
                    }
                });

                return false;
            }
        };

        $(document).ready(function() {
            ShouKuanDengJi.BindListEdit();
            ShouKuanDengJi.BindListDel();
            ShouKuanDengJi.BindBtn();
        });
    </script>

</asp:Content>
