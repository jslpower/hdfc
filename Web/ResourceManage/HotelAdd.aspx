<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage/Boxy.Master" AutoEventWireup="true"
    CodeBehind="HotelAdd.aspx.cs" Inherits="Web.ResourceManage.HotelAdd" EnableEventValidation="false" %>

<%@ Register Src="../UserControl/UploadControl.ascx" TagName="UploadControl" TagPrefix="uc1" %>
<%@ Register Src="../UserControl/ContactAndAccount.ascx" TagName="Contact" TagPrefix="uc1" %>
<%@ Register Src="../UserControl/HotelPriceControl.ascx" TagName="HotelPriceControl"
    TagPrefix="uc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="PageHead" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="PageBody" runat="server">
    <form id="form1" runat="server" enctype="multipart/form-data">
    <div>
        <table width="970" border="0" align="center" cellpadding="0" cellspacing="1" style="margin-top: 10px;">
            <tr class="odd">
                <th width="95" height="30" align="center">
                    <span style="color: red">*</span>省份：
                </th>
                <td width="330" align="left">
                    <asp:DropDownList ID="ddlProvice" name="ddlProvice" valid="required|RegInteger" errmsg="请选择省份|请选择省份"
                        runat="server" CssClass="inputselect" />
                </td>
                <th width="80" align="center">
                    城市：
                </th>
                <td width="330" align="left">
                    <asp:DropDownList ID="ddlCity" runat="server" CssClass="inputselect" errmsg="请选择城市|请选择城市"
                        name="ddlCity" valid="required|RegInteger" />
                </td>
            </tr>
            <tr class="even">
                <th height="30" align="center">
                    <span style="color: red">*</span>单位名称：
                </th>
                <td align="left">
                    <input type="text" id="txtunitname" name="txtunitname" value="" runat="server" class="inputtext formsize180"
                        valid="required" errmsg="单位不能为空" />
                    <span id="errMsg_unionname" class="errmsg"></span>
                </td>
                <th align="center">
                    <span style="color: red">*</span>星级：
                </th>
                <td align="left">
                    <asp:DropDownList ID="ddlHotelStart" runat="server" CssClass="inputselect" valid="required|RegInteger"
                        errmsg="请选择酒店星级|请选择酒店星级">
                    </asp:DropDownList>
                </td>
            </tr>
            <tr class="odd">
                <th height="30" align="center">
                    地址：
                </th>
                <td colspan="3" align="left">
                    <input type="text" id="txtAddress" name="txtAddress" value="" runat="server" class="inputtext formsize260" />
                </td>
            </tr>
            <tr class="even">
                <th height="30" align="center">
                    酒店简介：
                </th>
                <td colspan="3" align="left">
                    <textarea id="txtIntroduce" name="txtIntroduce" cols="45" rows="5" class="inputarea formsize600"
                        runat="server"></textarea>
                </td>
            </tr>
            <tr class="odd">
                <th height="30" align="center">
                    酒店图片：
                </th>
                <td colspan="3">
                    <uc1:UploadControl ID="UploadControl2" runat="server" />
                    <div style="width: 450px; float: left; margin-left: 5px;">
                        <asp:Repeater ID="rpPic" runat="server">
                            <ItemTemplate>
                                <span class='upload_filename'><a href='<%#Eval("FilePath") %>' target="_blank">
                                    <%#Eval("FileName")%></a> <a href="javascript:void(0)" onclick="HotelAddPage.RemoveFile(this)"
                                        title='删除附件'>
                                        <img style='vertical-align: middle' src='/images/cha.gif'></a>
                                    <input type="hidden" id="hidPicPath" name="hidPicPath" value='<%#Eval("FileName")%>|<%#Eval("FilePath") %>' />
                                </span>
                            </ItemTemplate>
                        </asp:Repeater>
                    </div>
                </td>
            </tr>
            <tr class="odd">
                <th align="center">
                    联系人：
                </th>
                <td colspan="3" align="left">
                    <uc1:Contact ID="Contact1" runat="server" IsAccount="true" />
                </td>
            </tr>
            <tr class="odd">
                <th height="30" align="center">
                    价格：
                </th>
                <td colspan="3" align="left">
                    <uc1:HotelPriceControl runat="server" ID="HotelPriceControl1" />
                </td>
            </tr>
            <tr class="even">
                <th height="60" align="center">
                    备注：
                </th>
                <td colspan="3">
                    <textarea id="txtremark" name="txtremark" cols="45" rows="5" class="inputarea formsize600"
                        runat="server"></textarea>
                </td>
            </tr>
            <tr class="odd">
                <th align="center">
                    附件上传：
                </th>
                <td colspan="3">
                    <uc1:UploadControl ID="UploadControl1" runat="server" />
                    <div style="width: 450px; float: left; margin-left: 5px;">
                        <asp:Repeater ID="rplfile" runat="server">
                            <ItemTemplate>
                                <span class='upload_filename'><a href='<%#Eval("FilePath") %>'>
                                    <%#Eval("FileName")%></a> <a href="javascript:void(0)" onclick="HotelAddPage.RemoveFile(this)"
                                        title='删除附件'>
                                        <img style='vertical-align: middle' src='/images/cha.gif'></a>
                                    <input type="hidden" id="hidFilePath" name="hidFilePath" value='<%#Eval("FileName")%>|<%#Eval("FilePath") %>' />
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
            HotelAddPage.PageInit();
        })
        var HotelAddPage = {
            PageInit: function() {
                pcToobar.init({
                    pID: "#<%=ddlProvice.ClientID %>",
                    cID: "#<%=ddlCity.ClientID %>",
                    pSelect: '<%=this.Province %>',
                    cSelect: '<%=this.City %>',
                    comID: '<%=this.SiteUserInfo.CompanyId %>'
                });
                $("input[readonly='readonly']").css({ "background-color": "#dadada" });
                $("#<%=btn.ClientID %>").click(function() {
                    if (!HotelAddPage.CheckForm()) {
                        return false;
                    }
                    var url = "/ResourceManage/HotelAdd.aspx?dotype=" + '<%=Request.QueryString["dotype"]%>' + "&type=save&tid=" + '<%=Request.QueryString["tid"]%>';
                    HotelAddPage.GoAjax(url);
                    return false;
                });
            },
            GoAjax: function(url) {
                HotelAddPage.UnBind();
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
                            HotelAddPage.Bind();
                        }
                    },
                    error: function() {
                        tableToolbar._showMsg(tableToolbar.errorMsg);
                        HotelAddPage.Bind();
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
                    if (!HotelAddPage.CheckForm()) {
                        return false;
                    }
                    var url = "/ResourceManage/HotelAdd.aspx?dotype=" + '<%=Request.QueryString["dotype"]%>' + "&type=save&tid=" + '<%=Request.QueryString["tid"]%>';
                    HotelAddPage.GoAjax(url);
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
