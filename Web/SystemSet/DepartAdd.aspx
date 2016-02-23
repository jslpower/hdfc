<%@ Page Title="添加部门" Language="C#" AutoEventWireup="true" CodeBehind="DepartAdd.aspx.cs"
    Inherits="Web.SystemSet.DepartAdd" MasterPageFile="~/MasterPage/Boxy.Master" %>

<asp:Content ID="PageHead" runat="server" ContentPlaceHolderID="PageHead">
</asp:Content>
<asp:Content ID="PageBody" runat="server" ContentPlaceHolderID="PageBody">
    <form id="form1" runat="server">
    <table width="600" border="0" align="center" cellpadding="0" cellspacing="1" style="margin: 20px auto;">
        <tr class="odd">
            <th width="13%" height="30" align="right">
                部门名称：
            </th>
            <td width="36%" bgcolor="#E3F1FC">
                <input type="text" class="inputtext" id="txtDepName" size="20" runat="server" valid="required"
                    maxlength="15" errmsg="部门名称不能为空" />
            </td>
            <th width="15%" height="30" align="right">
                部门主管：
            </th>
            <td width="36%" bgcolor="#E3F1FC">
                <asp:DropDownList runat="server" ID="ddlDepartManage" CssClass="inputselect">
                </asp:DropDownList>
            </td>
        </tr>
        <tr class="odd">
            <th width="13%" height="30" align="right">
                上级部门：
            </th>
            <td colspan="3" bgcolor="#E3F1FC">
                <asp:DropDownList runat="server" ID="ddlDepartParent" CssClass="inputselect" valid="isNo"
                    noValue="0" errmsg="请选择上级部门">
                </asp:DropDownList>
            </td>
        </tr>
        <tr class="odd">
            <th width="13%" height="30" align="right">
                联系电话：
            </th>
            <td width="36%" bgcolor="#E3F1FC">
                <input type="text" class="inputtext" runat="server" id="txtTel" size="20" valid="isPhone"
                    errmsg="联系电话格式不正确" />
            </td>
            <th width="15%" height="30" align="right">
                传真：
            </th>
            <td width="36%" bgcolor="#E3F1FC">
                <input type="text" runat="server" class="inputtext" id="txtFax" size="25" />
            </td>
        </tr>
        <tr class="odd">
            <th width="13%" height="30" align="right">
                备注：
            </th>
            <td colspan="3" bgcolor="#E3F1FC">
                <textarea name="txtRemark" id="txtRemark" cols="55" rows="5" runat="server" class="inputarea"></textarea>
            </td>
        </tr>
        <tr class="odd">
            <td height="30" colspan="4" align="center" bgcolor="#E3F1FC">
                <table border="0" align="center" cellpadding="0" cellspacing="0">
                    <tr>
                        <td width="76" height="40" align="center" class="tjbtn02">
                            <a href="javascript:void(0);" id="a_AddDepart">保存</a>
                        </td>
                        <td width="158" height="40" align="center">
                            <span class="tjbtn02"><a href="javascript:void(0);" onclick="window.parent.Boxy.getIframeDialog('<%=Request.QueryString["iframeId"]%>').hide();return false;">
                                关闭</a></span>
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
    </table>
    </form>

    <script type="text/javascript">

        var DepartEdit = {
            data: {
                action: '<%= EyouSoft.Common.Utils.GetQueryStringValue("action") %>',
                departId: '<%= EyouSoft.Common.Utils.GetQueryStringValue("departId") %>'
            },
            save: function() {
                var isC = ValiDatorForm.validator($("#a_AddDepart").closest("form").get(0), "parent");
                if (!isC) return false;
                $("#a_AddDepart").unbind("click");
                $("#a_AddDepart").html("正在提交");

                $.newAjax({
                    type: "post",
                    cache: false,
                    url: "/SystemSet/DepartAdd.aspx?save=1&" + $.param(DepartEdit.data),
                    dataType: "json",
                    data: $("#a_AddDepart").closest("form").serialize(),
                    success: function(ret) {
                        if (ret.result == "1") {
                            tableToolbar._showMsg(ret.msg, function() {
                                window.parent.Boxy.getIframeDialog('<%= EyouSoft.Common.Utils.GetQueryStringValue("iframeId") %>').hide();
                                parent.location.href = parent.location.href;
                            });
                        } else {
                            tableToolbar._showMsg(ret.msg);
                            DepartEdit.BindBtn();
                        }
                    },
                    error: function() {
                        tableToolbar._showMsg(tableToolbar.errorMsg);
                        DepartEdit.BindBtn();
                    }
                });
            },
            BindBtn: function() {
                $("#a_AddDepart").click(function() {
                    DepartEdit.save();
                    return false;
                });
                $("#a_AddDepart").html("保存");
            }
        };

        $(document).ready(function() {
            DepartEdit.BindBtn();
            return false;
        });
    </script>

</asp:Content>
