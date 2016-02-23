<%@ Page Title="编辑外联每日足迹" Language="C#" MasterPageFile="~/MasterPage/Boxy.Master"
    AutoEventWireup="true" CodeBehind="OutreachEdit.aspx.cs" Inherits="Web.CustomerManage.OutreachEdit" %>

<%@ Register Src="../UserControl/CustomerUnit.ascx" TagName="CustomerUnit" TagPrefix="uc5" %>
<asp:Content ID="Content1" ContentPlaceHolderID="PageHead" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="PageBody" runat="server">
    <form runat="server" id="form1">
    <div style="width: 720px; margin: 10px auto;">
        <table width="100%" border="0" align="center" cellpadding="0" cellspacing="1">
            <tr class="odd">
                <th width="120" height="30" align="right">
                    <span class="fred">*</span>销售日期：
                </th>
                <td bgcolor="#E3F1FC">
                    <input name="txtSaleDate" type="text" class="formsize80 inputtext" id="txtSaleDate"
                        runat="server" onfocus="WdatePicker()" valid="required|isDate" errmsg="请选择销售日期|销售日期格式错误" />
                </td>
                <th width="120" align="right">
                    <span class="fred">*</span>销售单位：
                </th>
                <td bgcolor="#E3F1FC">
                    <uc5:CustomerUnit ID="CustomerUnit1" runat="server" IsMoreSelect="false" IsRequired="true" />
                </td>
            </tr>
            <tr class="odd">
                <th height="30" align="right">
                    销售人：
                </th>
                <td bgcolor="#E3F1FC">
                    <input name="txtSalePeople" type="text" class="formsize80 inputtext" id="txtSalePeople"
                        runat="server" />
                </td>
                <th align="right">
                    联系电话：
                </th>
                <td bgcolor="#E3F1FC">
                    <input name="txtTel" type="text" class="formsize80 inputtext" id="txtTel" runat="server" />
                </td>
            </tr>
            <tr class="odd">
                <th height="30" align="right">
                    单位地址：
                </th>
                <td colspan="3" bgcolor="#E3F1FC">
                    <input name="txtAddress" type="text" class="formsize180 inputtext" id="txtAddress"
                        runat="server" />
                </td>
            </tr>
            <tr class="odd">
                <th align="right">
                    备注：
                </th>
                <td colspan="3" bgcolor="#E3F1FC">
                    <textarea name="txtRemark" cols="100" rows="3" class="formsize450 inputarea" id="txtRemark"
                        runat="server"></textarea>
                </td>
            </tr>
        </table>
        <table width="100%" border="0" align="center" cellpadding="0" cellspacing="0" style="margin: 10px auto;">
            <tr class="odd">
                <td height="40" colspan="14" bgcolor="#E3F1FC">
                    <table border="0" align="center" cellpadding="0" cellspacing="0">
                        <tr>
                            <td width="80" align="center" class="tjbtn02">
                                <asp:PlaceHolder runat="server" ID="plnSave"><a id="a_SaveQutreach" href="javascript:void(0);">
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
        var EditCustomerQutreach = {
            _data: {
                action: '<%= EyouSoft.Common.Utils.GetQueryStringValue("action") %>',
                qId: '<%= EyouSoft.Common.Utils.GetQueryStringValue("qId") %>',
                iframeId: '<%= EyouSoft.Common.Utils.GetQueryStringValue("iframeId") %>'
            },
            CheckForm: function() {
                return ValiDatorForm.validator($("#a_SaveQutreach").closest("form").get(0), "parent");
            },
            BindBtn: function() {
                $("#a_SaveQutreach").html("保存").css({ "color": "" }).click(function() {
                    EditCustomerQutreach.Save();
                    return false;
                });
            },
            CloseThis: function() {
                window.parent.Boxy.getIframeDialog('<%= EyouSoft.Common.Utils.GetQueryStringValue("iframeId") %>').hide();
                return false;
            },
            Save: function() {
                if (!EditCustomerQutreach.CheckForm()) {
                    return false;
                }
                $("#a_SaveQutreach").html("正在提交").css({ "color": "#999999" }).unbind("click");

                $.newAjax({
                    type: "post",
                    cache: false,
                    dataType: "json",
                    url: "/CustomerManage/OutreachEdit.aspx?save=1&" + $.param(EditCustomerQutreach._data),
                    data: $("#a_SaveQutreach").closest("form").serialize(),
                    success: function(ret) {
                        if (ret.result == "1") {
                            tableToolbar._showMsg(ret.msg, function() {
                                window.parent.Boxy.getIframeDialog('<%= EyouSoft.Common.Utils.GetQueryStringValue("iframeId") %>').hide();
                            });
                        }
                        else {
                            tableToolbar._showMsg(ret.msg);
                            EditCustomerQutreach.BindBtn();
                        }
                    },
                    error: function() {
                        tableToolbar._showMsg(tableToolbar.errorMsg);
                        EditCustomerQutreach.BindBtn();
                    }
                });

                return false;
            }
        };

        $(document).ready(function() {
            EditCustomerQutreach.BindBtn();
            $("#a_CloseThis").click(function() {
                EditCustomerQutreach.CloseThis();
                return false;
            });

            return false;
        });
    </script>

</asp:Content>
