<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage/Boxy.Master" AutoEventWireup="true"
    CodeBehind="EditUserInfo.aspx.cs" Inherits="Web.SystemSet.EditUserInfo" %>

<asp:Content ID="Content1" ContentPlaceHolderID="PageHead" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="PageBody" runat="server">
    <form id="form1" runat="server">
    <table width="780" border="0" align="center" cellpadding="0" cellspacing="1" style="margin: 20px auto;"
        id="tablelist">
        <tr class="odd">
            <th width="10%" height="30" align="right">
                用户名：
            </th>
            <td width="15%" bgcolor="#E3F1FC">
                <asp:Literal runat="server" ID="ltrUserName"></asp:Literal>
            </td>
            <th width="10%" height="30" align="right">
                <span style="color: red">*</span>密码：
            </th>
            <td width="15%" bgcolor="#E3F1FC">
                <asp:TextBox runat="server" TextMode="Password" CssClass="inputtext formsize100"
                    ID="txtPassword" valid="required" errmsg="密码不能为空"></asp:TextBox>
            </td>
            <th width="10%" height="30" align="right">
                重复密码：
            </th>
            <td width="15%" bgcolor="#E3F1FC">
                <asp:TextBox runat="server" TextMode="Password" CssClass="inputtext formsize100"
                    ID="txtPassWordCheck" valid="required" errmsg="重复密码不能为空"></asp:TextBox>
            </td>
            <th width="10%" height="30" align="right">
                <span style="color: red">*</span>姓名：
            </th>
            <td width="15%" bgcolor="#E3F1FC">
                <input type="text" runat="server" class="xtinput inputtext" id="txtName" size="15"
                    maxlength="15" valid="required" errmsg="姓名不为空" />
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
                性别：
            </th>
            <td width="15%" bgcolor="#E3F1FC">
                <asp:RadioButtonList ID="rdiSex" runat="server" RepeatDirection="Horizontal">
                    <asp:ListItem Text="男" Value="2" Selected="True"></asp:ListItem>
                    <asp:ListItem Text="女" Value="1"></asp:ListItem>
                </asp:RadioButtonList>
            </td>
            <th width="10%" height="30" align="right">
                出生日期：
            </th>
            <td width="15%" bgcolor="#E3F1FC">
                <input type="text" class="xtinput inputtext" id="txtBirthday" runat="server" size="15"
                    onfocus="WdatePicker()" />
            </td>
            <th width="10%" height="30" align="right">
                电话：
            </th>
            <td width="15%" bgcolor="#E3F1FC">
                <input type="text" class="xtinput inputtext" runat="server" id="txtTel" size="15"
                    maxlength="15" />
            </td>
        </tr>
        <tr class="odd">
            <th width="10%" height="30" align="right">
                传真：
            </th>
            <td width="15%" bgcolor="#E3F1FC">
                <input type="text" class="xtinput inputtext" runat="server" id="txtFax" size="15"
                    maxlength="30" />
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
            <td height="30" colspan="8" align="left" bgcolor="#E3F1FC">
                <table width="304" border="0" align="center" cellpadding="0" cellspacing="0">
                    <tr>
                        <td width="76" height="40" align="center" class="tjbtn02">
                            <a href="javascript:void(0);" id="btn_save">保存</a>
                        </td>
                        <td width="158" height="40" align="center">
                            <span class="tjbtn02"><a href="javascript:void(0);" id="a_btnClose">关闭</a></span>
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
    </table>
    </form>

    <script type="text/javascript">
        var EditUserInfo = {
            _data: {
                iframeId: '<%= EyouSoft.Common.Utils.GetQueryStringValue("iframeId") %>'
            },
            CheckForm: function() {
                return ValiDatorForm.validator($("#btn_save").closest("form").get(0), "parent");
            },
            BindBtn: function() {
                $("#btn_save").html("保存").css({ "color": "" }).click(function() {
                    EditUserInfo.Save();
                    return false;
                });
            },
            Close: function() {
                window.parent.Boxy.getIframeDialog('<%= EyouSoft.Common.Utils.GetQueryStringValue("iframeId") %>').hide();
                return false;
            },
            Save: function() {
                if (!EditUserInfo.CheckForm()) {
                    return false;
                }
                var pwd = $("#<%= txtPassword.ClientID %>").val();
                var pwd2 = $("#<%= txtPassWordCheck.ClientID %>").val();
                if (pwd != pwd2) {
                    tableToolbar._showMsg("请确保两次输入的密码一致！");
                    return false;
                }
                $("#btn_save").html("正在提交").css({ "color": "#999999" }).unbind("click");

                $.newAjax({
                    type: "post",
                    cache: false,
                    dataType: "json",
                    url: "/SystemSet/EditUserInfo.aspx?save=1&" + $.param(EditUserInfo._data),
                    data: $("#btn_save").closest("form").serialize(),
                    success: function(ret) {
                        if (ret.result == "1") {
                            tableToolbar._showMsg(ret.msg, function() {
                                parent.location.href = parent.location.href;
                            });
                        }
                        else {
                            tableToolbar._showMsg(ret.msg);
                            EditUserInfo.BindBtn();
                        }
                    },
                    error: function() {
                        tableToolbar._showMsg(tableToolbar.errorMsg);
                        EditUserInfo.BindBtn();
                    }
                });

                return false;
            }
        };

        $(document).ready(function() {
            EditUserInfo.BindBtn();
            $("#a_btnClose").click(function() {
                EditUserInfo.Close();
            });
        });
        
    </script>

</asp:Content>
