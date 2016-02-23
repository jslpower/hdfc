<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="TourDataEdit.aspx.cs" Inherits="Web.jidiaoCenter.TourDataAdd"
    MasterPageFile="~/MasterPage/Boxy.Master" %>

<%@ Register Src="/UserControl/UploadControl.ascx" TagName="UploadControl" TagPrefix="uc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="PageHead" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="PageBody" runat="server">
    <form id="form1" runat="server">
    <table width="540" border="0" align="center" cellpadding="0" cellspacing="1" style="margin-top: 10px;">
        <tr class="odd">
            <th height="30" align="right">
                分类：
            </th>
            <td colspan="3" bgcolor="#E3F1FC">
                <asp:DropDownList runat="server" ID="ddlArea" CssClass="inputselect" valid="required"
                    errmsg="请选择分类">
                </asp:DropDownList>
            </td>
        </tr>
        <tr class="odd">
            <th height="30" align="right">
                线路名称：
            </th>
            <td bgcolor="#E3F1FC">
                <input name="txtRouteName" type="text" class="inputtext" id="txtRouteName" runat="server"
                    valid="required" errmsg="请输入线路名称" />
            </td>
            <th align="right">
                团型：
            </th>
            <td bgcolor="#E3F1FC">
                <asp:DropDownList runat="server" ID="ddlTourDataType" CssClass="inputselect" valid="required"
                    errmsg="请选择团型">
                </asp:DropDownList>
            </td>
        </tr>
        <tr class="odd">
            <th height="30" align="right">
                进出港口：
            </th>
            <td colspan="3" bgcolor="#E3F1FC">
                <input name="txtTourPort" type="text" class="inputtext" id="txtTourPort" runat="server" />
            </td>
        </tr>
        <tr class="odd">
            <th height="30" align="right">
                资料上传
            </th>
            <td colspan="3" bgcolor="#E3F1FC">
                <uc1:UploadControl ID="UploadControl1" runat="server" />
                <div style="float: left; margin-left: 5px;">
                    <asp:Repeater ID="rplfile" runat="server">
                        <ItemTemplate>
                            <span class='upload_filename'><a target="_blank" href='<%#Eval("FilePath") %>'>
                                <%#Eval("FileName")%></a> <a href="javascript:void(0)" onclick="TourDataEditPage.RemoveFile(this)"
                                    title='删除附件'>
                                    <img style='vertical-align: middle' src='/images/cha.gif'></a>
                                <input type="hidden" id="hidFilePath" name="hidFilePath" value='<%#Eval("FileName")%>|<%#Eval("FilePath") %>' />
                            </span>
                        </ItemTemplate>
                    </asp:Repeater>
                </div>
            </td>
        </tr>
        <tr class="odd">
            <th height="30" align="right">
                上传日期：
            </th>
            <td bgcolor="#E3F1FC">
                <input name="txtIssueTime" type="text" class="inputtext" id="txtIssueTime" runat="server"
                    disabled="disabled" />
            </td>
            <th align="right">
                上传人：
            </th>
            <td bgcolor="#E3F1FC">
                <input name="txtOperatorName" type="text" class="inputtext" id="txtOperatorName"
                    runat="server" disabled="disabled" />
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
    </form>

    <script type="text/javascript">
        $(function() {
            TourDataEditPage.PageInit();
        })
        var TourDataEditPage = {
            PageInit: function() {
                $("#<%=btn.ClientID %>").click(function() {
                    if (!TourDataEditPage.CheckForm()) {
                        return false;
                    }
                    var url = "/jidiaoCenter/TourDataEdit.aspx?dotype=" + '<%=Request.QueryString["dotype"]%>' + "&type=save&tourdataid=" + '<%=Request.QueryString["tourdataid"]%>';
                    TourDataEditPage.GoAjax(url);
                    return false;
                });
            },
            GoAjax: function(url) {
                TourDataEditPage.UnBind();
                $.newAjax({
                    type: "post",
                    cache: false,
                    url: url,
                    dataType: "json",
                    data: $("#<%=form1.ClientID %>").serialize(),
                    success: function(ret) {
                        if (ret.result == "1") {
                            parent.tableToolbar._showMsg(ret.msg, function() { parent.location.href = parent.location.href; });
                        }
                        else {
                            parent.tableToolbar._showMsg(ret.msg);
                            TourDataEditPage.Bind();
                        }
                    },
                    error: function() {
                        tableToolbar._showMsg(tableToolbar.errorMsg);
                        TourDataEditPage.Bind();
                    }
                });
            },
            CheckForm: function() {
                return ValiDatorForm.validator($("#<%=form1.ClientID %>"), "parent");
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
                    if (!TourDataEditPage.CheckForm()) {
                        return false;
                    }
                    var url = "/jidiaoCenter/TourDataEdit.aspx?dotype=" + '<%=Request.QueryString["dotype"]%>' + "&type=save&tourdataid=" + '<%=Request.QueryString["tourdataid"]%>';
                    TourDataEditPage.GoAjax(url);
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
