<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="GuideAdd.aspx.cs" Inherits="Web.ResourceManage.GuideAdd"
    MasterPageFile="~/MasterPage/Boxy.Master" EnableEventValidation="false" %>

<%@ Register Src="../UserControl/UploadControl.ascx" TagName="UploadControl" TagPrefix="uc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="PageHead" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="PageBody" runat="server">
    <form runat="server" id="form1">
    <table width="800" border="0" align="center" cellpadding="0" cellspacing="1" style="margin: 10px auto;">
        <tr class="odd" style="display:none;">
            <th width="95" height="30" align="center">
                <span style="color: red">*</span>省份：
            </th>
            <td width="330" align="left">
                <asp:DropDownList ID="ddlProvice" name="ddlProvice" 
                    runat="server" CssClass="inputselect" />
            </td>
            <th width="80" align="center">
                城市：
            </th>
            <td width="330" align="left">
                <asp:DropDownList ID="ddlCity" runat="server" CssClass="inputselect" 
                    name="ddlCity"  />
            </td>
        </tr>
        <tr class="odd" >
            <th width="95" height="30" align="center">
                <span style="color: red">*</span>旅行社名称：
            </th>
            <td width="330" align="left" colspan="3">
               <input type="text" name="txtGysName"  class="inputtext" id="txtGysName" valid="required"
                    errmsg="请输入旅行社名称" runat="server"/>
            </td>
        </tr>
        <tr class="even">
            <th height="30" align="right">
                <span style="color: red">*</span>导游姓名：
            </th>
            <td align="left">
                <input name="txtGuideName" type="text" class="inputtext" id="txtGuideName" valid="required"
                    errmsg="请输入姓名" runat="server" />
            </td>
            <th align="right">
                <span style="color: red">*</span> 手机：
            </th>
            <td align="left">
                <input name="txtMobile" type="text" class="inputtext" id="txtMobile" valid="required|isMobile"
                    errmsg="请输入手机号|手机号格式不正确" runat="server" />
            </td>
        </tr>
        <tr class="odd">
            <th height="30" align="right">
                生日：
            </th>
            <td align="left">
                <input name="txtBirthday" type="text" class="inputtext" id="txtBirthday" onfocus="WdatePicker()"
                    runat="server" />
            </td>
            <th align="right">
                带团时间：
            </th>
            <td align="left">
                <input name="txtTourTime" type="text" class="inputtext" id="txtTourTime" runat="server" />
            </td>
        </tr>
        <tr class="even">
            <th height="30" align="right">
                <span style="color: red">*</span>星级：
            </th>
            <td align="left">
                <asp:DropDownList ID="ddlStar" runat="server" CssClass="inputselect" valid="required|RegInteger"
                    errmsg="请选择导游星级|请选择导游星级">
                </asp:DropDownList>
            </td>
            <th align="right">
                所属社别：
            </th>
            <td align="left">
                <input name="txtBelongs" type="text" class="inputtext" id="txtBelongs" runat="server" />
            </td>
        </tr>
        <tr class="odd">
            <th height="30" align="right">
                导游图片：
            </th>
            <td colspan="3">
                <uc1:UploadControl ID="UploadControl1" runat="server" />
                <div style="width: 450px; float: left; margin-left: 5px;">
                    <asp:Repeater ID="rplfile" runat="server">
                        <ItemTemplate>
                            <span class='upload_filename'><a href='<%#Eval("FilePath") %>' target="_blank">
                                <%#Eval("FileName")%></a> <a href="javascript:void(0)" onclick="GuideAddPage.RemoveFile(this)"
                                    title='删除附件'>
                                    <img style='vertical-align: middle' src='/images/cha.gif'></a>
                                <input type="hidden" id="hidFilePath" name="hidFilePath" value='<%#Eval("FileName")%>|<%#Eval("FilePath") %>' />
                            </span>
                        </ItemTemplate>
                    </asp:Repeater>
                </div>
            </td>
        </tr>
        <tr class="even">
            <th align="right">
                导游介绍：
            </th>
            <td colspan="3">
                <textarea id="txtRemark" name="txtRemark" cols="45" rows="5" class="inputarea formsize600"
                    runat="server"></textarea>
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
    <input type="hidden" id="hfileId" runat="server" value="" />
    </form>

    <script type="text/javascript">
        $(function() {
            GuideAddPage.PageInit();
        })
        var GuideAddPage = {
            PageInit: function() {
//                pcToobar.init({
//                    pID: "#<%=ddlProvice.ClientID %>",
//                    cID: "#<%=ddlCity.ClientID %>",
//                    pSelect: '<%=this.Province %>',
//                    cSelect: '<%=this.City %>',
//                    comID: '<%=this.SiteUserInfo.CompanyId %>'
//                });
                $("input[readonly='readonly']").css({ "background-color": "#dadada" });
                $("#<%=btn.ClientID %>").click(function() {
                    if (!GuideAddPage.CheckForm()) {
                        return false;
                    }
                    var url = "/ResourceManage/GuideAdd.aspx?dotype=" + '<%=Request.QueryString["dotype"]%>' + "&type=save&tid=" + '<%=Request.QueryString["tid"]%>';
                    GuideAddPage.GoAjax(url);
                    return false;
                })
            },
            GoAjax: function(url) {
                GuideAddPage.UnBind();
                $.newAjax({
                    type: "post",
                    cache: false,
                    url: url,
                    dataType: "json",
                    data: $("#<%=btn.ClientID %>").closest("form").serialize(),
                    success: function(ret) {
                        if (ret.result == "1") {
                            parent.tableToolbar._showMsg(ret.msg, function() {
                                parent.location.href = parent.location.href;
                            });
                        }
                        else {
                            parent.tableToolbar._showMsg(ret.msg);
                            GuideAddPage.Bind();
                        }
                    },
                    error: function() {
                        parent.tableToolbar._showMsg(tableToolbar.errorMsg);
                        GuideAddPage.Bind();
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
                    if (!GuideAddPage.CheckForm()) {
                        return false;
                    }
                    var url = "/ResourceManage/GuideAdd.aspx?dotype=" + '<%=Request.QueryString["dotype"]%>' + "&type=save&tid=" + '<%=Request.QueryString["tid"]%>';
                    GuideAddPage.GoAjax(url);
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
