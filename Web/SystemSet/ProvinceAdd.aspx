<%@ Page Title="添加省份" Language="C#" AutoEventWireup="true" CodeBehind="ProvinceAdd.aspx.cs"
    Inherits="Web.SystemSet.ProvinceAdd" MasterPageFile="~/MasterPage/Boxy.Master" %>

<asp:Content ID="PageHead" runat="server" ContentPlaceHolderID="PageHead">
</asp:Content>
<asp:Content ID="PageBody" runat="server" ContentPlaceHolderID="PageBody">
    <form id="peForm" runat="server">
    <input type="hidden" name="hidMethod" id="hidMethod" value="save" />
    <table width="500" cellspacing="1" cellpadding="0" border="0" align="center" style="margin: 20px auto;">
        <tbody>
            <tr class="odd">
                <th width="18%" height="30" align="right">
                    省份名称：
                </th>
                <td bgcolor="#E3F1FC">
                    <input name="txtProvinceName" id="txtProvinceName" type="text" maxlength="20" class="inputtext"
                        size="40" value="<%=provinceName %>" valid="required" errmsg="省份不为空" />
                </td>
            </tr>
            <tr class="odd">
                <td height="30" bgcolor="#E3F1FC" align="left" colspan="2">
                    <table width="324" cellspacing="0" cellpadding="0" border="0">
                        <tbody>
                            <tr>
                                <td width="90" height="40" align="center">
                                </td>
                                <td width="76" height="40" align="center" class="tjbtn02">
                                    <a href="javascript:;" onclick="return save('');" id="btnsave" runat="server">保存</a>
                                </td>
                            </tr>
                        </tbody>
                    </table>
                </td>
            </tr>
        </tbody>
    </table>
    </form>

    <script type="text/javascript">
       //保存省份信息
        function save(method) {
            var isSuccess = ValiDatorForm.validator($("#<%=peForm.ClientID %>").get(0), "parent");
            if (!isSuccess) return false;
            $.newAjax(
            { url: "/SystemSet/ProvinceAdd.aspx",
                data: { pId: "<%=pId %>", pName: $("#txtProvinceName").val(), isExist: "isExist" },
                dataType: "json",
                cache: false,
                async: false,
                type: "post",
                success: function(result) {
                    if (result.success == "1") {
                        parent.tableToolbar._showMsg("该省份已经存在！");
                        isSuccess = false;
                    }

                },
                error: function() {
                    parent.tableToolbar._showMsg("数据保存失败！");
                }
            })
            if (!isSuccess) { return false; }
            if (method == "continue") {
                document.getElementById("hidMethod").value = "continue";
            }
            $("#<%=peForm.ClientID %>").get(0).submit();
            return false;
            
        }
    </script>

</asp:Content>
