<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ChuNaDengZhangAdd.aspx.cs"
    Inherits="Web.Fin.ChuNaDengZhangAdd" MasterPageFile="~/MasterPage/Boxy.Master" %>

<%@ Register Src="../UserControl/UploadControl.ascx" TagName="UploadControl" TagPrefix="uc1" %>
<%@ Register Src="../UserControl/TourSelect.ascx" TagName="TourSelect" TagPrefix="uc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="PageHead" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="PageBody" runat="server">
    <form id="Form1" runat="server">
    <div style="width: 730px; margin: 10px auto;">
        <table width="100%" border="0" align="center" cellpadding="0" cellspacing="1">
            <tr class="odd">
                <th width="130" height="30" align="right">
                    <span style="color: red">*</span>收款/付款：
                </th>
                <td bgcolor="#E3F1FC">
                    <asp:DropDownList runat="server" ID="ddlStatus" CssClass="inputselect" Valid="required"
                        errmsg="请选择款项方式">
                    </asp:DropDownList>
                </td>
                <th width="120" align="right">
                    <span style="color: red">*</span>团号：
                </th>
                <td bgcolor="#E3F1FC">
                    <uc1:TourSelect runat="server" ID="TourSelect1" IsMust="true" />
                </td>
            </tr>
            <tr class="odd">
                <th width="120" height="30" align="right">
                    <span style="color: red">*</span>收款（付款）日期：
                </th>
                <td bgcolor="#E3F1FC">
                    <input name="txtDate" valid="required" errmsg="请选择收款（付款）日期" type="text" class="inputtext"
                        id="txtDate" runat="server" onfocus="WdatePicker()" />
                </td>
                <th align="right">
                    <span style="color: red">*</span>金额：
                </th>
                <td bgcolor="#E3F1FC">
                    <input name="txtPrice" type="text" class="inputtext" id="txtPrice" runat="server"
                        valid="required|isMoney" errmsg="请填写金额|金额格式不正确" />
                </td>
            </tr>
            <tr class="odd">
                <th height="30" align="right">
                    <span style="color: red">*</span>银行账号：
                </th>
                <td bgcolor="#E3F1FC">
                    <asp:DropDownList ID="ddlBank" runat="server" CssClass="inputselect" Valid="required"
                        errmsg="请选择银行账号">
                    </asp:DropDownList>
                </td>
                <th align="right">
                    操作人：
                </th>
                <td bgcolor="#E3F1FC">
                    <input type="hidden" name="hidDengZhangPeopleId" id="hidDengZhangPeopleId" runat="server" />
                    <input type="hidden" name="hidDengZhangPeople" id="hidDengZhangPeople" runat="server" />
                    <input runat="server" name="txtDengZhangPeople" type="text" class="inputtext" id="txtDengZhangPeople"
                        disabled="disabled" />
                </td>
            </tr>
            <tr class="odd">
                <th height="30" align="right">
                    <span style="color: red">*</span>支付方式：
                </th>
                <td colspan="3" bgcolor="#E3F1FC">
                    <asp:DropDownList runat="server" ID="ddlPayType" CssClass="inputselect" Valid="required"
                        errmsg="请选择支付方式">
                    </asp:DropDownList>
                </td>
            </tr>
            <tr class="odd">
                <th align="right">
                    收支款原因：
                </th>
                <td colspan="3" bgcolor="#E3F1FC">
                    <textarea id="txtReason" name="txtReason" cols="45" rows="5" class="inputarea" runat="server"></textarea>
                </td>
            </tr>
            <tr class="odd">
                <th height="30" align="right">
                    手续费：
                </th>
                <td bgcolor="#E3F1FC">
                    <input name="txtOtherPrice" id="txtOtherPrice" runat="server" type="text" class="inputtext" />
                </td>
                <th align="right">
                    是否开票：
                </th>
                <td bgcolor="#E3F1FC">
                    <select id="ddlIsKaiPiao" runat="server" class="inputselect">
                        <option value="1">已开票</option>
                        <option value="0">未开票</option>
                    </select>
                </td>
            </tr>
            <tr class="odd">
                <th height="30" align="right">
                    附件：
                </th>
                <td colspan="3" bgcolor="#E3F1FC">
                    <uc1:UploadControl ID="UploadControl1" runat="server" />
                    <div style="width: 450px; float: left; margin-left: 5px;">
                        <asp:Repeater ID="rpFile" runat="server">
                            <ItemTemplate>
                                <span class='upload_filename'><a href='<%#Eval("FilePath") %>' target="_blank">
                                    <%#Eval("FileName")%></a> <a href="javascript:void(0)" onclick="ChuNaDengZhangAdd.RemoveFile(this)"
                                        title='删除附件'>
                                        <img style='vertical-align: middle' src='/images/cha.gif'></a>
                                    <input type="hidden" id="hidFilePath" name="hidFilePath" value='<%#Eval("FileName")%>|<%#Eval("FilePath") %>|<%#Eval("FileId") %>' />
                                </span>
                            </ItemTemplate>
                        </asp:Repeater>
                    </div>
                </td>
            </tr>
        </table>
        <table width="320" border="0" align="center" cellpadding="0" cellspacing="0">
            <tr>
                <td height="40" align="center">
                </td>
                <td height="40" align="center" class="tjbtn02">
                    <a href="javascript:;" class="save" id="btn" runat="server">保存</a>
                </td>
                <td height="40" align="center" class="tjbtn02">
                    <a href="javascript:;" id="linkCancel" onclick="parent.Boxy.getIframeDialog('<%=Request.QueryString["iframeId"] %>').hide()">
                        关闭</a>
                </td>
            </tr>
        </table>
    </div>
    </form>

    <script type="text/javascript">
        $(function() {
            ChuNaDengZhangAdd.PageInit();
        })
        var ChuNaDengZhangAdd = {
            PageInit: function() {
                $("input[readonly='readonly']").css({ "background-color": "#dadada" });
                $("#<%=btn.ClientID %>").click(function() {
                    if (!ChuNaDengZhangAdd.CheckForm()) {
                        return false;
                    }
                    var url = "/Fin/ChuNaDengZhangAdd.aspx?dotype=" + '<%=Request.QueryString["dotype"]%>' + "&type=save&tid=" + '<%=Request.QueryString["tid"]%>';
                    ChuNaDengZhangAdd.GoAjax(url);
                    return false;
                });
            },
            GoAjax: function(url) {
                ChuNaDengZhangAdd.UnBind();
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
                            ChuNaDengZhangAdd.Bind();
                        }
                    },
                    error: function() {
                        tableToolbar._showMsg(tableToolbar.errorMsg);
                        ChuNaDengZhangAdd.Bind();
                    }
                });
            },
            CheckForm: function() {
                return ValiDatorForm.validator($("#<%=btn.ClientID %>").closest("form").get(0), "parent");
            },
            RemoveFile: function(obj) {
                $(obj).parent().remove();
                return false;
            },
            Bind: function() {
                var _selfs = $("#<%=this.btn.ClientID %>");
                _selfs.html("保存");
                _selfs.css("cursor", "pointer");
                _selfs.click(function() {
                    if (!ChuNaDengZhangAdd.CheckForm()) {
                        return false;
                    }
                    var url = "/Fin/ChuNaDengZhangAdd.aspx?dotype=" + '<%=Request.QueryString["dotype"]%>' + "&type=save&tid=" + '<%=Request.QueryString["tid"]%>';
                    ChuNaDengZhangAdd.GoAjax(url);
                    return false;
                });
            },
            UnBind: function() {
                $("#<%=this.btn.ClientID %>").html("提交中...");
                $("#<%=this.btn.ClientID %>").unbind("click");
            }
        }
    </script>

</asp:Content>
