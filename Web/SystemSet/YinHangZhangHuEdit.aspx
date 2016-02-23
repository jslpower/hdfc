<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="YinHangZhangHuEdit.aspx.cs"
    Inherits="Web.SystemSet.YinHangZhangHuEdit" MasterPageFile="~/MasterPage/Boxy.Master" %>

<%@ Register Src="~/UserControl/UploadControl.ascx" TagName="UploadControl" TagPrefix="uc1" %>
<asp:Content runat="server" ContentPlaceHolderID="PageBody" ID="PageBody">
    <div style="width: 630px; margin: 10px auto;">
        <table width="100%" border="0" align="center" cellpadding="0" cellspacing="1" id="i_table_form">
            <tr class="odd">
                <th width="120" height="30" align="right">
                    账户名称：
                </th>
                <td bgcolor="#E3F1FC" colspan="3">
                    <input name="txtName" type="text" class="formsize120 inputtext" id="txtName" runat="server"
                        valid="required" errmsg="请填写账户名称" />
                </td>
            </tr>
            <tr class="odd">
                <th height="30" align="right">
                    开户银行：
                </th>
                <td colspan="3" bgcolor="#E3F1FC">
                    <input name="txtYinHangName" type="text" class="formsize140 inputtext" id="txtYinHangName"
                        runat="server" valid="required" errmsg="请填写开户银行" />
                </td>
            </tr>
            <tr class="odd">
                <th height="30" align="right">
                    银行账号：
                </th>
                <td colspan="3" align="left" bgcolor="#E3F1FC">
                    <input name="txtZhangHao" type="text" class="formsize140 inputtext" id="txtZhangHao"
                        runat="server" valid="required" errmsg="请填写银行账号" />
                </td>
            </tr>
        </table>
        <table width="100%" border="0" align="center" cellpadding="0" cellspacing="0" style="margin: 10px auto;">
            <tr class="odd">
                <td height="30" colspan="14" align="left" bgcolor="#E3F1FC">
                    <table border="0" align="center" cellpadding="0" cellspacing="0">
                        <tr>
                            <td height="40" align="center" class="tjbtn02">
                                <asp:Literal runat="server" ID="ltrOperatorHtml" />
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
        </table>
    </div>

    <script type="text/javascript">
        var iPage = {
            reload: function() {
                window.location.href = window.location.href;
            },
            close: function() {
                var _win = top || window;
                _win.Boxy.getIframeDialog('<%=EyouSoft.Common.Utils.GetQueryStringValue("iframeId") %>').hide();
                return false;
            },
            save: function(obj) {
                var _data = { 
                    txtName: $.trim($("#<%=txtName.ClientID %>").val()),
                    txtYinHangName: $.trim($("#<%=txtYinHangName.ClientID %>").val()),
                    txtZhangHao: $.trim($("#<%=txtZhangHao.ClientID %>").val())
                };

                var validatorResult = ValiDatorForm.validator($("#i_table_form").get(0), "parent");
                if (!validatorResult) return;

                $(obj).unbind("click").css({ "color": "#999999" });

                $.newAjax({
                    type: "POST",
                    url: window.location.href + "&doType=save",
                    data: _data,
                    cache: false,
                    dataType: "json",
                    async: false,
                    success: function(response) {
                        if (response.result == "1") {
                            alert(response.msg);
                            iPage.close();
                        } else {
                            alert(response.msg);
                            $(obj).bind("click", function() { iPage.save(obj); }).css({ "color": "" });
                        }
                    },
                    error: function() {
                        $(obj).bind("click", function() { iPage.save(obj); }).css({ "color": "" });
                    }
                });
            }
        };

        $(document).ready(function() {
            $("#i_a_save").bind("click", function() { iPage.save(this); });
        });
    </script>

</asp:Content>
