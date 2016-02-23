<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage/Boxy.Master" AutoEventWireup="true"
    CodeBehind="CustomerQuoteEdit.aspx.cs" Inherits="Web.CustomerManage.CustomerQuoteEdit" %>

<%@ Register Src="../UserControl/CustomerUnit.ascx" TagName="CustomerUnit" TagPrefix="uc5" %>
<asp:Content ID="Content1" ContentPlaceHolderID="PageHead" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="PageBody" runat="server">
    <form runat="server" id="form1">
    <div style="width: 720px; margin: 10px auto;">
        <table width="100%" border="0" align="center" cellpadding="0" cellspacing="1">
            <tr class="odd">
                <th width="120" height="30" align="right">
                    <span class="fred">*</span>出团日期：
                </th>
                <td bgcolor="#E3F1FC">
                    <input name="txtLeaveDate" type="text" runat="server" class="formsize80 inputtext"
                        id="txtLeaveDate" onfocus="WdatePicker()" valid="required|isDate" errmsg="请选择出团日期|出团日期格式错误" />
                </td>
                <th width="120" align="right">
                    <span class="fred">*</span>人数：
                </th>
                <td bgcolor="#E3F1FC">
                    <input name="txtPeopleNum" type="text" runat="server" class="formsize40 inputtext"
                        id="txtPeopleNum" valid="required|isPIntegers" errmsg="请填写人数|人数只能是数字" />
                </td>
            </tr>
            <tr class="odd">
                <th height="30" align="right">
                    <span class="fred">*</span>询价单位：
                </th>
                <td bgcolor="#E3F1FC">
                    <uc5:CustomerUnit ID="CustomerUnit1" runat="server" IsMoreSelect="false" IsRequired="true" />
                </td>
                <th align="right">
                    <span class="fred">*</span>询价时间：
                </th>
                <td bgcolor="#E3F1FC">
                    <input name="txtXunJiaDate" type="text" runat="server" class="formsize80 inputtext"
                        id="txtXunJiaDate" onfocus="WdatePicker()" valid="required|isDate" errmsg="请选择询价时间|询价时间格式错误" />
                </td>
            </tr>
            <tr class="odd">
                <th height="30" align="right">
                    询价人：
                </th>
                <td bgcolor="#E3F1FC">
                    <input name="txtXunJiaPeople" runat="server" type="text" class="formsize80 inputtext"
                        id="txtXunJiaPeople" />
                </td>
                <th align="right">
                    联系电话：
                </th>
                <td bgcolor="#E3F1FC">
                    <input name="txtTel" runat="server" type="text" class="formsize80 inputtext" id="txtTel" />
                </td>
            </tr>
            <tr class="odd">
                <th height="30" align="right">
                    手机：
                </th>
                <td bgcolor="#E3F1FC">
                    <input name="txtMobile" type="text" runat="server" class="formsize80 inputtext" id="txtMobile" />
                </td>
                <th align="right">
                    QQ：
                </th>
                <td bgcolor="#E3F1FC">
                    <input name="txtQQ" type="text" runat="server" class="formsize80 inputtext" id="txtQQ" />
                </td>
            </tr>
            <tr class="odd">
                <th align="right">
                    询价内容：
                </th>
                <td colspan="3" bgcolor="#E3F1FC">
                    <textarea name="txtNeiRong" cols="100" runat="server" rows="3" class="formsize450 inputarea"
                        id="txtNeiRong"></textarea>
                </td>
            </tr>
        </table>
        <table width="100%" border="0" align="center" cellpadding="0" cellspacing="0" style="margin: 10px auto;">
            <tr class="odd">
                <td height="40" colspan="14" bgcolor="#E3F1FC">
                    <table border="0" align="center" cellpadding="0" cellspacing="0">
                        <tr>
                            <td width="80" align="center" class="tjbtn02">
                                <asp:PlaceHolder runat="server" ID="plnSave"><a id="a_SaveQuote" href="javascript:void(0);">
                                    保存</a></asp:PlaceHolder>
                            </td>
                            <td width="80" align="center" class="tjbtn02">
                                <asp:PlaceHolder runat="server" ID="plnClose"><a id="a_CloseThis" href="javascript:void(0);">
                                    关闭</a></asp:PlaceHolder>
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
        </table>
    </div>
    </form>

    <script type="text/javascript">
        var EditCustomerQuote = {
            _data: {
                action: '<%= EyouSoft.Common.Utils.GetQueryStringValue("action") %>',
                qId: '<%= EyouSoft.Common.Utils.GetQueryStringValue("qId") %>',
                iframeId: '<%= EyouSoft.Common.Utils.GetQueryStringValue("iframeId") %>'
            },
            CheckForm: function() {
                return ValiDatorForm.validator($("#a_SaveQuote").closest("form").get(0), "parent");
            },
            BindBtn: function() {
                $("#a_SaveQuote").html("保存").css({ "color": "" }).click(function() {
                    EditCustomerQuote.Save();
                    return false;
                });
            },
            CloseThis: function() {
                window.parent.Boxy.getIframeDialog('<%= EyouSoft.Common.Utils.GetQueryStringValue("iframeId") %>').hide();
                return false;
            },
            Save: function() {
                if (!EditCustomerQuote.CheckForm()) {
                    return false;
                }
                $("#a_SaveQuote").html("正在提交").css({ "color": "#999999" }).unbind("click");

                $.newAjax({
                    type: "post",
                    cache: false,
                    dataType: "json",
                    url: "/CustomerManage/CustomerQuoteEdit.aspx?save=1&" + $.param(EditCustomerQuote._data),
                    data: $("#a_SaveQuote").closest("form").serialize(),
                    success: function(ret) {
                        if (ret.result == "1") {
                            tableToolbar._showMsg(ret.msg, function() {
                                window.parent.Boxy.getIframeDialog('<%= EyouSoft.Common.Utils.GetQueryStringValue("iframeId") %>').hide();
                            });
                        }
                        else {
                            tableToolbar._showMsg(ret.msg);
                            EditCustomerQuote.BindBtn();
                        }
                    },
                    error: function() {
                        tableToolbar._showMsg(tableToolbar.errorMsg);
                        EditCustomerQuote.BindBtn();
                    }
                });

                return false;
            }
        };

        $(document).ready(function() {
            EditCustomerQuote.BindBtn();
            $("#a_CloseThis").click(function() {
                EditCustomerQuote.CloseThis();
                return false;
            });

            return false;
        });
    </script>

</asp:Content>
