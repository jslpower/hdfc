<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage/Boxy.Master" AutoEventWireup="true"
    CodeBehind="SaleAreaEdit.aspx.cs" Inherits="Web.SystemSet.SaleAreaEdit" %>

<asp:Content ID="Content1" ContentPlaceHolderID="PageHead" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="PageBody" runat="server">
    <form runat="server" id="form1">
    <table width="580" border="0" align="center" cellpadding="0" cellspacing="1" style="margin: 10px auto;">
        <tr class="odd">
            <th width="21%" height="30" align="right">
                销售地区名称：
            </th>
            <td width="79%" bgcolor="#E3F1FC">
                <input name="txtSaleAreaName" runat="server" type="text" class="xtinput inputtext"
                    id="txtSaleAreaName" size="40" valid="required" errmsg="销售地区名称不能为空" />
            </td>
        </tr>
        <tr class="odd">
            <td height="40" colspan="8" align="left" bgcolor="#E3F1FC">
                <table border="0" align="center" cellpadding="0" cellspacing="0">
                    <tr>
                        <td width="80" align="center" class="tjbtn02">
                            <a href="javascript:void(0);" id="a_EditSaleArea">保存</a>
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
    </table>
    </form>

    <script type="text/javascript">
        var EditSaleAreaInfo = {
            data: {
                action: '<%= EyouSoft.Common.Utils.GetQueryStringValue("action") %>',
                sId: '<%= EyouSoft.Common.Utils.GetQueryStringValue("sId") %>'
            },
            BindBtn: function() {
                $("#a_EditSaleArea").click(function() {
                    EditSaleAreaInfo.save();
                    return false;
                });
                $("#a_EditSaleArea").html("保存").css({ "color": "" }); ;
            },
            save: function() {
                var isC = ValiDatorForm.validator($("#a_EditSaleArea").closest("form").get(0), "parent");
                if (!isC) return false;

                $("#a_EditSaleArea").unbind("click").css({ "color": "#999999" });
                $("#a_EditSaleArea").html("正在提交");

                $.newAjax({
                    type: "post",
                    url: "/SystemSet/SaleAreaEdit.aspx?Save=1&" + $.param(EditSaleAreaInfo.data),
                    cache: false,
                    data: $("#a_EditSaleArea").closest("form").serialize(),
                    dataType: "json",
                    success: function(ret) {
                        if (ret.result == "1") {
                            tableToolbar._showMsg(ret.msg, function() {
                                window.parent.Boxy.getIframeDialog('<%= EyouSoft.Common.Utils.GetQueryStringValue("iframeId") %>').hide();
                            });
                        }
                        else {
                            tableToolbar._showMsg(ret.msg);
                            EditSaleAreaInfo.BindBtn();
                        }
                    },
                    error: function() {
                        tableToolbar._showMsg(tableToolbar.errorMsg);
                        EditSaleAreaInfo.BindBtn();
                    }
                });
                return false;
            }
        };

        $(document).ready(function() {
            EditSaleAreaInfo.BindBtn();
            return false;
        });
    </script>

</asp:Content>
