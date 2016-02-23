<%@ Page Title="新增" Language="C#" AutoEventWireup="true" CodeBehind="UserAdd.aspx.cs"
    Inherits="Web.SystemSet.UserAdd" MasterPageFile="~/MasterPage/Boxy.Master" %>

<asp:Content ID="PageHead" runat="server" ContentPlaceHolderID="PageHead">
</asp:Content>
<asp:Content ID="PageBody" runat="server" ContentPlaceHolderID="PageBody">
    <form id="form1" runat="server">
    <input type="hidden" name="hidMethod" id="hidMethod" value="save" />
    <table width="780" border="0" align="center" cellpadding="0" cellspacing="1" style="margin: 20px auto;"
        id="tablelist">
        <tr class="odd">
            <th width="10%" height="30" align="right">
                <span style="color: red">*</span>用户名：
            </th>
            <td width="15%" bgcolor="#E3F1FC">
                <input type="text" class="xtinput inputtext" id="txtUserName" name="uName" maxlength="15"
                    size="15" runat="server" valid="required" errmsg="用户名不为空" />
            </td>
            <th width="10%" height="30" align="right">
                <span style="color: red">*</span>密码：
            </th>
            <td width="15%" bgcolor="#E3F1FC">
                <asp:TextBox runat="server" TextMode="Password" CssClass="inputtext formsize100"
                    ID="txtPassword" valid="required" errmsg="密码不为空"></asp:TextBox>
            </td>
            <th width="10%" height="30" align="right">
                <span style="color: red">*</span>姓名：
            </th>
            <td width="15%" bgcolor="#E3F1FC">
                <input type="text" runat="server" class="xtinput inputtext" id="txtName" size="15"
                    maxlength="15" valid="required" errmsg="姓名不为空" />
            </td>
            <th width="10%" height="30" align="right">
                性别：
            </th>
            <td width="15%" bgcolor="#E3F1FC">
                <asp:RadioButtonList ID="rdiSex" runat="server" RepeatDirection="Horizontal">
                    <asp:ListItem Text="男" Value="2" Selected="True"></asp:ListItem>
                    <asp:ListItem Text="女" Value="1"></asp:ListItem>
                </asp:RadioButtonList>
            </td>
        </tr>
        <tr class="odd">
            <th width="10%" height="30" align="right">
                <span style="color: red">*</span>所属部门：
            </th>
            <td width="15%" bgcolor="#E3F1FC">
                <select id="selBdepart" runat="server" valid="required" errmsg="所属部门不为空" class="inputselect formsize100">
                </select>
                <span id="errMsg_<%=selBdepart.ClientID %>" class="errmsg" style="display: block">
                </span>
                <input type="hidden" name="selBName" id="selBName" />
            </td>
            <th width="10%" height="30" align="right">
                兼管部门：
            </th>
            <td width="15%" bgcolor="#E3F1FC">
                <select id="selMdepart" runat="server" class="inputselect formsize100">
                </select>
                <input type="hidden" name="selMName" id="selMName" />
            </td>
            <th width="10%" height="30" align="right">
                电话：
            </th>
            <td width="15%" bgcolor="#E3F1FC">
                <input type="text" class="xtinput inputtext" runat="server" id="txtTel" size="15"
                    maxlength="15" />
            </td>
            <th width="10%" height="30" align="right">
                传真：
            </th>
            <td width="15%" bgcolor="#E3F1FC">
                <input type="text" class="xtinput inputtext" runat="server" id="txtFax" size="15"
                    maxlength="30" />
            </td>
        </tr>
        <tr class="odd">
            <th width="10%" height="30" align="right">
                <span style="color: red">*</span>手机：
            </th>
            <td width="15%" bgcolor="#E3F1FC">
                <input type="text" class="xtinput inputtext" id="txtMoible" runat="server" size="15"
                    valid="required|isMobile" errmsg="手机不为空|格式不正确" />
            </td>
            <th width="10%" height="30" align="right">
                QQ：
            </th>
            <td width="15%" bgcolor="#E3F1FC">
                <input type="text" runat="server" class="xtinput inputtext" id="txtQQ" size="15"
                    maxlength="12" />
            </td>
            <th width="10%" height="30" align="right">
                MSN：
            </th>
            <td height="30" bgcolor="#E3F1FC">
                <input type="text" runat="server" class="xtinput inputtext" id="txtMSN" size="18"
                    maxlength="30" />
            </td>
            <th width="10%" height="30" align="right">
                E-mail：
            </th>
            <td height="30" bgcolor="#E3F1FC">
                <input type="text" runat="server" class="xtinput inputtext" id="txtEmail" size="18"
                    maxlength="30" />
            </td>
        </tr>
        <tr class="odd">
            <th width="10%" height="30" align="right">
                出生日期：
            </th>
            <td width="15%" bgcolor="#E3F1FC">
                <input type="text" class="xtinput inputtext" id="txtBirthday" runat="server" size="15" onfocus="WdatePicker()" />
            </td>
            <th width="10%" height="30" align="right">
                住址：
            </th>
            <td colspan="5" bgcolor="#E3F1FC">
                <input type="text" class="xtinput inputtext" id="txtAddress" style="width:390px" runat="server" />
            </td>
        </tr>
        <tr class="odd">
            <th width="10%" height="30" align="right">
                简介：
            </th>
            <td style="padding: 4px 0px;" height="30" colspan="7" bgcolor="#E3F1FC">
                <textarea cols="80" rows="5" id="txtIntroduce" class="inputtext" style="height: 100px"
                    runat="server"></textarea>
            </td>
        </tr>
        <tr class="odd">
            <th width="10%" height="30" align="right">
                备注：
            </th>
            <td style="padding: 4px 0px;" height="30" colspan="7" bgcolor="#E3F1FC">
                <textarea cols="80" rows="5" id="txtRemark" class="inputtext" style="height: 100px"
                    runat="server"></textarea>
            </td>
        </tr>
        <tr class="odd">
            <td height="30" colspan="8" align="left" bgcolor="#E3F1FC">
                <table width="304" border="0" align="center" cellpadding="0" cellspacing="0">
                    <tr>
                        <td width="76" height="40" align="center" class="tjbtn02">
                            <a href="javascript:;" id="btn_save" onclick="return Ee.save('');">保存</a>
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
        var Ee =
            {
                //保存表单
                save: function(method) {
                    var form = $("#btn_save").closest("form").get(0);
                    var vResult = ValiDatorForm.validator(form, "parent");
                    if (!vResult) return false;
                    if (method == "continue") {
                        document.getElementById("hidMethod").value = "continue";
                    }
                    $("#selBName").val($("#<%=selBdepart.ClientID %>").find("option:selected").html());
                    $("#selMName").val($("#<%=selMdepart.ClientID %>").find("option:selected").html());

                    if (!vResult) return false;
                    form.submit();
                    return false;
                }
            }
    </script>

</asp:Content>
