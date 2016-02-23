<%@ Page Title="文档编辑" Language="C#" AutoEventWireup="true" CodeBehind="FileAdd.aspx.cs"
    Inherits="Web.CompanyFiles.FileAdd" MasterPageFile="~/MasterPage/Boxy.Master" %>

<%@ Register Src="~/UserControl/UploadControl.ascx" TagName="UploadControl" TagPrefix="uc1" %>
<asp:Content ID="PageHead" runat="server" ContentPlaceHolderID="PageHead">
</asp:Content>
<asp:Content ID="PageBody" runat="server" ContentPlaceHolderID="PageBody">
    <form id="frmFile" action="">
    <table width="500" cellspacing="1" cellpadding="0" border="0" align="center" style="margin: 20px auto;">
        <tbody>
            <tr class="odd">
                <th width="21%" height="30" align="right">
                    文档名称：
                </th>
                <td width="79%" bgcolor="#E3F1FC">
                    <input type="text" size="60" id="fileName" class="inputtext" name="fileName" runat="server" />
                </td>
            </tr>
            <tr class="odd">
                <th width="21%" height="30" align="right">
                    上传文档：
                </th>
                <td width="79%" bgcolor="#E3F1FC">
                    <uc1:UploadControl runat="server" ID="UploadControl1" />
                    <asp:Label runat="server" ID="lblFilePath"></asp:Label>
                </td>
            </tr>
            <tr class="odd">
                <th width="21%" height="30" align="right">
                    上传时间：
                </th>
                <td width="79%" bgcolor="#E3F1FC">
                    <input type="text" disabled="disabled" id="CreateTime" class="inputtext" name="CreateTime"
                        runat="server" />
                </td>
            </tr>
            <tr class="odd">
                <th width="21%" height="30" align="right">
                    上传人：
                </th>
                <td width="79%" bgcolor="#E3F1FC">
                    <input type="text" disabled="disabled" id="OperatorName" class="inputtext" name="OperatorName"
                        runat="server" />
                </td>
            </tr>
            <tr class="odd">
                <td height="30" bgcolor="#E3F1FC" align="left" colspan="8">
                    <table width="340" cellspacing="0" cellpadding="0" border="0">
                        <tbody>
                            <tr>
                                <td width="106" height="40" align="center">
                                </td>
                                <td width="76" height="40" align="center" class="tjbtn02">
                                    <input type="hidden" id="hidCreateTime" runat="server" />
                                    <input type="hidden" id="hidOperatorId" runat="server" />
                                    <a id="btnSave" href="javascript:void(0);" runat="server" visible="false">保存</a>
                                    <a id="btnUpdate" href="javascript:void(0);" runat="server" visible="false">修改</a>
                                </td>
                                <td width="158" height="40" align="center" class="tjbtn02">
                                    <a href="javascript:;" onclick="window.parent.Boxy.getIframeDialog('<%=EyouSoft.Common.Utils.GetQueryStringValue("iframeId") %>').hide();">
                                        关闭</a>
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
        var File = {
            Save: function() {
                if (File.Validate()) {
                    $("#<%= btnSave.ClientID %>").html("提交中...");
                    File.UnBind();
                    var Data = { Type: "Save" };
                    $.newAjax({
                        type: "post",
                        url: "/CompanyFiles/FileAdd.aspx?" + $.param(Data),
                        data: $("#frmFile").serialize(),
                        dataType: "json",
                        success: function(data) {
                            if (data.result == "1") {
                                parent.tableToolbar._showMsg(data.msg, function() {
                                    window.parent.Boxy.getIframeDialog('<%=EyouSoft.Common.Utils.GetQueryStringValue("iframeId") %>').hide();
                                    window.parent.location.reload();
                                });
                            }
                            else {
                                parent.tableToolbar._showMsg(data.msg);
                                File.Bind();
                            }

                        },
                        error: function() {
                            tableToolbar._showMsg("服务器忙！");
                            File.Bind();
                        }
                    });
                }
            },
            UnBind: function() {
                $("#<%= btnSave.ClientID %>").unbind("click");
                $("#<%= btnUpdate.ClientID %>").unbind("click");
            },
            Bind: function() {
                var _selfs = $("#<%= btnSave.ClientID %>");
                _selfs.html("保存");
                _selfs.css("cursor", "pointer");
                _selfs.click(function() {
                    if (File.Validate()) {
                        File.Save();
                        return false;
                    }
                });

                var _selfu = $("#<%= btnUpdate.ClientID %>");
                _selfu.html("修改");
                _selfu.css("cursor", "pointer");
                _selfu.click(function() {
                    if (File.Validate()) {
                        File.Update();
                        return false;
                    }
                });
            },
            Validate: function() {

                var fileName = $("#<%= fileName.ClientID %>").val();
                if (fileName == "") {
                    parent.tableToolbar._showMsg("文档名称不能为空!");
                    return false;
                }
                var _filea = $("#" + '<%=this.UploadControl1.ClientHideID %>').val();
                var _fileb = $("#hidFilePath").val();
                if (_filea == "" && _fileb == "") {
                    parent.tableToolbar._showMsg("附件不能为空!");
                    return false;
                }

                return true;
            },
            DelFile: function(obj) {
                $(obj).parent().remove();
            },
            Update: function() {
                if (File.Validate()) {
                    $("#<%= btnUpdate.ClientID %>").html("提交中...");
                    File.UnBind();
                    var Data = { Type: "Update", DocumentId: '<%=EyouSoft.Common.Utils.GetQueryStringValue("DocumentId") %>' };
                    $.newAjax({
                        type: "post",
                        url: "/CompanyFiles/FileAdd.aspx?" + $.param(Data),
                        data: $("#frmFile").serialize(),
                        dataType: "json",
                        success: function(data) {
                            if (data.result == "1") {
                                parent.tableToolbar._showMsg(data.msg, function() {
                                    window.parent.Boxy.getIframeDialog('<%=EyouSoft.Common.Utils.GetQueryStringValue("iframeId") %>').hide();
                                    window.parent.location.reload();
                                });
                            }
                            else {
                                parent.tableToolbar._showMsg(data.msg);
                                File.Bind();
                            }

                        },
                        error: function() {
                            tableToolbar._showMsg("服务器忙！");
                            File.Bind();
                        }
                    });
                }
            }

        };

        $(function() {
            //保存
            $("#<%= btnSave.ClientID %>").click(function() {
                File.Save();
            });

            $("#<%= btnUpdate.ClientID %>").click(function() {
                // alert($("#frmFile").serialize());
                File.Update();
            });
        });
    </script>

</asp:Content>
