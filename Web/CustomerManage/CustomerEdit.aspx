<%@ Page Language="C#" MasterPageFile="~/MasterPage/Boxy.Master" AutoEventWireup="true"
    CodeBehind="CustomerEdit.aspx.cs" Inherits="Web.CustomerManage.CustomerEdit"
    Title="客户资料" EnableEventValidation="false" %>

<%@ Register Src="../UserControl/ContactInfo.ascx" TagName="ContactInfo" TagPrefix="uc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="PageHead" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="PageBody" runat="server">
    <form id="form1" runat="server">
    <table width="880" cellspacing="1" cellpadding="0" border="0" align="center" style="margin: 10px auto;">
        <tbody>
            <tr class="odd">
                <th width="100" height="30" align="right">
                    <span class="fred">*</span>省份：
                </th>
                <td width="310" align="left">
                    <asp:DropDownList ID="ddlProvice" name="ddlProvice" valid="required|RegInteger" errmsg="请选择省份|请选择省份"
                        runat="server" CssClass="inputselect" />
                </td>
                <th align="right">
                    <span class="fred">*</span>城市：
                </th>
                <td width="320" align="left">
                    <asp:DropDownList ID="ddlCity" runat="server" CssClass="inputselect" errmsg="请选择城市|请选择城市"
                        name="ddlCity" valid="required|RegInteger" />
                </td>
            </tr>
            <tr class="even">
                <th height="30" align="right">
                    <span class="fred">*</span>销售地区：
                </th>
                <td align="left">
                    <select id="sltarea" name="sltarea" class="inputselect">
                        <%=BindArea(Areaid)%>
                    </select>
                </td>
                <th align="right">
                    <span class="fred">*</span>单位名称：
                </th>
                <td align="left">
                    <input type="text" class="inputtext formsize180" id="txtName" name="txtName" runat="server"
                        valid="required" errmsg="请填写单位名称" />
                </td>
            </tr>
            <tr class="odd">
                <th height="30" align="right">
                    许可证号：
                </th>
                <td align="left">
                    <input type="text" id="txtLicense" class="inputtext formsize180" name="txtLicense"
                        runat="server" />
                </td>
                <th align="right">
                    客户评级：
                </th>
                <td align="left">
                    <asp:DropDownList runat="server" ID="ddlCustomerRating" class="inputselect">
                    </asp:DropDownList>
                </td>
            </tr>
            <tr class="even">
                <th align="right">
                    公司地址：
                </th>
                <td align="left" colspan="3">
                    <input type="text" class="inputtext formsize260" id="txtAddress" name="txtAddress"
                        runat="server" />
                </td>
            </tr>
            <tr class="odd">
                <th height="30" align="right">
                    邮编：
                </th>
                <td align="left">
                    <input type="text" id="txtPostalCode" class="inputtext formsize80" name="txtPostalCode"
                        runat="server" />
                </td>
                <th align="right">
                    银行账号：
                </th>
                <td align="left">
                    <input type="text" class="inputtext formsize180" id="txtBankAccount" name="txtBankAccount"
                        runat="server" />
                </td>
            </tr>
            <tr class="even">
                <th height="30" align="right">
                    主要联系人：
                </th>
                <td align="left" colspan="3">
                    <input type="text" id="txtContactName" class="inputtext formsize80" name="txtContactName"
                        runat="server" />
                    <b>电话：</b>
                    <input type="text" id="txtPhone" class="inputtext formsize80" name="txtPhone" runat="server" />
                    <b>手机：</b>
                    <input type="text" id="txtMobile" errmsg="请填写正确的手机号!" valid="isMobile" class="inputtext formsize80"
                        name="txtMobile" runat="server" />
                    <b>传真：</b>
                    <input type="text" id="txtFax" class="inputtext formsize80" name="txtFax" runat="server" />
                </td>
            </tr>
            <tr class="odd">
                <th align="right">
                    备注：
                </th>
                <td align="left" colspan="3">
                    <asp:TextBox ID="txtRemark" TextMode="MultiLine" Height="70px" CssClass="formsize450 inputtext"
                        runat="server"></asp:TextBox>
                </td>
            </tr>
            <tr class="odd">
                <th align="right">
                    联系人信息：
                </th>
                <td colspan="3">
                    <uc1:ContactInfo ID="ContactInfo1" runat="server" />
                </td>
            </tr>
        </tbody>
    </table>
    <table width="320" cellspacing="0" cellpadding="0" border="0" align="center">
        <tbody>
            <tr>
                <td height="40" align="center">
                </td>
                <td height="40" align="center" class="tjbtn02">
                    <a href="javascript:;" id="btn" runat="server">保存</a>
                </td>
                <td height="40" align="center" class="tjbtn02">
                    <a onclick="parent.Boxy.getIframeDialog(parent.Boxy.queryString('iframeId')).hide()"
                        id="linkCancel" href="javascript:;">关闭</a>
                </td>
            </tr>
        </tbody>
    </table>
    <input type="hidden" value="-1" runat="server" id="hidareaid" name="hidareaid" />
    </form>

    <script type="text/javascript">
        $(function() {
            CustomerEdit.PageInit();
            CustomerEdit.BindBtn();
        })
        var CustomerEdit = {
            PageInit: function() {
                pcToobar.init({
                    pID: "#<%=ddlProvice.ClientID %>",
                    cID: "#<%=ddlCity.ClientID %>",
                    pSelect: '<%=this.Province %>',
                    cSelect: '<%=this.City %>',
                    comID: '<%=this.SiteUserInfo.CompanyId %>'
                });
                $("input[readonly='readonly']").css({ "background-color": "#dadada" });
            },
            BindBtn: function() {
                $("#<%=btn.ClientID %>").unbind("click").live("click", function() {
                    if (!CustomerEdit.CheckForm()) {
                        return false;
                    }
                    if ($("#<%=hidareaid.ClientID %>").val() == "-1") {
                        tableToolbar._showMsg("请选择销售地区");
                        return false;
                    }
                    var error = CustomerEdit.ValidateUser();
                    if (error != "") {
                        tableToolbar._showMsg(error);
                        return false;
                    }

                    var dotype = '<%=Request.QueryString["dotype"] %>';
                    var url = "/CustomerManage/CustomerEdit.aspx?type=save&dotype=" + dotype + "&id=" + '<%=Request.QueryString["id"] %>';
                    CustomerEdit.GoAjax(url);
                    return false;
                })
                $("#sltarea").change(function() {
                    $("#<%=hidareaid.ClientID %>").val($(this).val());
                })
            },
            GoAjax: function(url) {
                $("#<%=btn.ClientID %>").unbind("click");
                $.newAjax({
                    type: "post",
                    cache: false,
                    url: url,
                    dataType: "json",
                    data: $("#<%=btn.ClientID %>").closest("form").serialize(),
                    success: function(ret) {
                        if (ret.result == "1") {
                            parent.tableToolbar._showMsg(ret.msg, function() { parent.location.href = parent.location.href; });
                        }
                        else {
                            parent.tableToolbar._showMsg(ret.msg);
                        }
                    },
                    error: function() {
                        tableToolbar._showMsg(tableToolbar.errorMsg);
                    }
                });
            },
            CheckForm: function() {
                return ValiDatorForm.validator($("#<%=btn.ClientID %>").closest("form").get(0), "parent");
            },
            ValidateUser: function() {
                var error = "";
                $(".TrContact").each(function() {
                    var _tr = $(this);
                    var contact_Name = _tr.find("input[name='contact_Name']").val();
                    if (contact_Name != "") {
                        var contact_mobile = _tr.find("input[name='contact_mobile']").val();
                        if (contact_mobile == "") {
                            error += "手机号不能为空！<br />";
                        }

                        var contact_qq = _tr.find("input[name='contact_qq']").val();
                        if (contact_qq == "") {
                            error += "QQ号不能为空！<br />";
                        }

                    }
                });

                return error;
            }
        }
    </script>

</asp:Content>
