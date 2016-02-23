<%@ Page Title="添加城市" Language="C#" AutoEventWireup="true" CodeBehind="CityAdd.aspx.cs"
    Inherits="Web.SystemSet.CityAdd" MasterPageFile="~/MasterPage/Boxy.Master" %>

<asp:Content ID="Content1" runat="server" ContentPlaceHolderID="PageHead">
</asp:Content>
<asp:Content ID="Content2" runat="server" ContentPlaceHolderID="PageBody">
    <form id="ce_form" runat="server">
    <input type="hidden" name="hidMethod" id="hidMethod" value="save" />
    <table width="500" cellspacing="1" cellpadding="0" border="0" align="center" style="margin: 20px auto;">
        <tbody>
            <tr class="odd">
                <th width="18%" height="30" align="right">
                    所属省份：
                </th>
                <td bgcolor="#E3F1FC">
                    <select id="selProvince" name="selProvince" runat="server" valid="required" errmsg="省份不为空"
                        class="inputselect">
                    </select>
                </td>
            </tr>
            <tr class="odd">
                <th width="18%" height="30" align="right">
                    城市名称：
                </th>
                <td bgcolor="#E3F1FC">
                    <input name="txtCityName" type="text" class="inputtext" id="txtCityName" maxlength="20"
                        size="40" value="<%=cityName %>" valid="required" errmsg="城市不为空" />
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
                                    <a href="javascript:;" id="btnsave" onclick="return save('');" runat="server">保存</a>
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
        //保存城市信息
        function save(method) {
            var isSuccess = ValiDatorForm.validator($("#<%=ce_form.ClientID %>").get(0), "parent");
            if (!isSuccess) { return false; }
            $.newAjax(
            { url: "/SystemSet/CityAdd.aspx",
                data: { cityId: "<%=cId %>", cityName: $("#txtCityName").val(), isExist: "isExist" },
                dataType: "json",
                cache: false,
                async: false,
                type: "post",
                success: function(result) {
                    if (result.success == "1") {
                        parent.tableToolbar._showMsg("该城市已经存在！");
                        isSuccess = false;
                    }
                }
            })
            if (!isSuccess) { return false; }
            if (method == "continue") {
                document.getElementById("hidMethod").value = "continue";
            }
            //提交表单
            $("#<%=ce_form.ClientID %>").get(0).submit();
            return false;
        }
    </script>

</asp:Content>
