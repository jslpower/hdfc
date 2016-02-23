<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage/Boxy.Master" AutoEventWireup="true"
    CodeBehind="EditCarFlight.aspx.cs" Inherits="Web.SystemSet.EditCarFlight" %>

<asp:Content ID="Content1" ContentPlaceHolderID="PageHead" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="PageBody" runat="server">
    <form runat="server" id="form1">
    <div style="width: 630px; margin: 10px auto;">
        <table width="100%" border="0" align="center" cellpadding="0" cellspacing="1" id="i_table_form">
            <tr class="odd">
                <th width="120" height="30" align="right">
                    机票/火车票：
                </th>
                <td bgcolor="#E3F1FC" colspan="3">
                    <asp:DropDownList runat="server" ID="ddlType" CssClass="inputselect">
                    </asp:DropDownList>
                </td>
            </tr>
            <tr class="odd">
                <th height="30" align="right">
                    <span class="fred">*</span>车次/航班：
                </th>
                <td colspan="3" bgcolor="#E3F1FC">
                    <asp:TextBox runat="server" ID="txtCarNo" CssClass="formsize140 inputtext" valid="required"
                        errmsg="请填写车次/航班"></asp:TextBox>
                </td>
            </tr>
            <tr class="odd">
                <th height="30" align="right">
                    <span class="fred">*</span>开车/起飞时间：
                </th>
                <td colspan="3" align="left" bgcolor="#E3F1FC">
                    <input name="txtDate" type="text" class="formsize140 inputtext" errmsg="请填写开车/起飞日期!"
                        valid="required" id="txtDate" runat="server" onfocus="WdatePicker()" />
                    <select id="sltHH" name="sltHH" class="inputselect" errmsg="请填写开车/起飞时间(小时)!" valid="required">
                        <%=StrHH %>
                    </select>
                    时
                    <select id="sltmm" name="sltmm" class="inputselect" errmsg="请填写开车/起飞时间(分钟)!" valid="required">
                        <%=Strmm %>
                    </select>
                    分
                </td>
            </tr>
            <tr class="odd">
                <th height="30" align="right">
                    <span class="fred">*</span> 区间：
                </th>
                <td colspan="3" align="left" bgcolor="#E3F1FC">
                    <input name="txtQuJian" type="text" valid="required" errmsg="请填写区间" class="formsize140 inputtext"
                        id="txtQuJian" runat="server" />
                </td>
            </tr>
            <tr class="odd">
                <th height="30" align="right">
                    手续费/机燃：
                </th>
                <td colspan="3" align="left" bgcolor="#E3F1FC">
                    <input name="txtRanYou" type="text" class="formsize140 inputtext" id="txtRanYou"
                        runat="server" valid="isMoney" errmsg="手续费/机燃格式错误" />
                </td>
            </tr>
            <tr class="odd">
                <th height="30" align="right">
                    佣金：
                </th>
                <td colspan="3" align="left" bgcolor="#E3F1FC">
                    <input name="txtYongJin" type="text" class="formsize140 inputtext" id="txtYongJin"
                        runat="server" valid="isMoney" errmsg="佣金格式错误" />
                </td>
            </tr>
            <tr class="odd">
                <th height="30" align="right">
                    舱位/席别：
                </th>
                <td colspan="3" align="left" bgcolor="#E3F1FC">
                    <asp:DropDownList runat="server" ID="ddlXiBie" CssClass="inputselect">
                    </asp:DropDownList>
                    <input name="txtXiBie" visible="false" type="text" class="formsize140 inputtext"
                        id="txtXiBie" runat="server" />
                </td>
            </tr>
        </table>
        <table width="100%" border="0" align="center" cellpadding="0" cellspacing="0" style="margin: 10px auto;">
            <tr class="odd">
                <td height="30" colspan="14" align="left" bgcolor="#E3F1FC">
                    <table border="0" align="center" cellpadding="0" cellspacing="0">
                        <tr>
                            <td height="40" align="center" class="tjbtn02">
                                <a href="javascript:void(0)" id="btnSave">保存</a>
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
        </table>
    </div>
    </form>

    <script type="text/javascript">
        var EditCarFlight = {
            close: function() {
                var _win = top || window;
                _win.Boxy.getIframeDialog('<%=EyouSoft.Common.Utils.GetQueryStringValue("iframeId") %>').hide();
                return false;
            },
            save: function() {
                var validatorResult = ValiDatorForm.validator($("#btnSave").closest("form").get(0), "parent");
                if (!validatorResult) return;

                $("#btnSave").unbind("click").css({ "color": "#999999" });

                var data = {
                    action: '<%= EyouSoft.Common.Utils.GetQueryStringValue("action") %>',
                    cid: '<%= EyouSoft.Common.Utils.GetQueryStringValue("cid") %>',
                    save: "1"
                }
                $.newAjax({
                    type: "POST",
                    url: "/SystemSet/EditCarFlight.aspx?" + $.param(data),
                    data: $("#btnSave").closest("form").serialize(),
                    cache: false,
                    dataType: "json",
                    async: false,
                    success: function(ret) {
                        if (ret.result == "1") {
                            tableToolbar._showMsg(ret.msg, function() { EditCarFlight.close(); });
                        } else {
                            tableToolbar._showMsg(ret.msg, function() {
                                $("#btnSave").bind("click", function() { EditCarFlight.save(); return false; }).css({ "color": "" });
                            });
                        }
                    },
                    error: function() {
                        $("#btnSave").bind("click", function() { EditCarFlight.save(); return false; }).css({ "color": "" });
                    }
                });
            }
        };

        $(document).ready(function() {
            $("#btnSave").bind("click", function() { EditCarFlight.save(obj); return false; });
        })
    </script>

</asp:Content>
