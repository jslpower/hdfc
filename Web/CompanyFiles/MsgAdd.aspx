<%@ Page Title="公告通知编辑" Language="C#" MasterPageFile="/MasterPage/Front.Master" AutoEventWireup="true"
    CodeBehind="MsgAdd.aspx.cs" Inherits="Web.CompanyFiles.MsgAdd" ValidateRequest="false" %>

<%@ Register Src="/UserControl/UploadControl.ascx" TagName="UploadControl" TagPrefix="uc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">

    <script src="/Js/kindeditor-4.1/kindeditor-min.js" type="text/javascript"></script>

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="mainbody">
        <div class="lineprotitlebox">
            <table width="100%" border="0" cellspacing="0" cellpadding="0">
                <tr>
                    <td width="15%" nowrap="nowrap">
                        <span class="lineprotitle">公告通知</span>
                    </td>
                    <td width="85%" nowrap="nowrap" align="right" style="padding: 0 10px 2px 0; color: #13509f;">
                        <b>当前您所在位置</b>>> 公司文件 >>公告通知
                    </td>
                </tr>
                <tr>
                    <td colspan="2" height="2" bgcolor="#000000">
                    </td>
                </tr>
            </table>
        </div>
        <div class="lineCategorybox" style="height: 30px;">
        </div>
        <div class="tablelist">
            <form runat="server" id="InfoFrom" enctype="multipart/form-data">
            <input type="hidden" value="save" name="hidMethod" id="hidMethod" />
            <input type="hidden" value="" name="hidDel" id="hidDel" />
            <table width="800" border="0" align="center" cellpadding="0" cellspacing="1" bgcolor="#BDDCF4">
                <tr>
                    <th colspan="3" align="center" bgcolor="#BDDCF4">
                        <%=infoId==0?"添加信息":"修改信息" %>
                    </th>
                </tr>
                <tr>
                    <td width="16%" height="35" align="right" bgcolor="#e3f1fc">
                        <span style="color: Red">*</span><strong>标题：</strong>
                    </td>
                    <td width="84%" height="35" colspan="2" align="left" bgcolor="#FAFDFF" class="pandl3">
                        <input class="inputtext" id="txtInfoTitle" runat="server" type="text" size="50" />
                    </td>
                </tr>
                <tr>
                    <td height="35" align="right" bgcolor="#e3f1fc">
                        <span style="color: Red">*</span><strong>发布对象：</strong>
                    </td>
                    <td height="35" colspan="2" align="left" bgcolor="#FAFDFF" class="pandl3">
                        <input type="checkbox" name="chkPublishTo" value="cInner" <%=innerChecked %> />
                        公司内部 
                        <a href="javascript:;" onclick="return InfoEdit.selDeaprt();">
                            <input type="checkbox" class="selDeaprt" name="chkPublishTo" value="sDepart" <%=departChecked %> />
                            指定部门<img src="/images/icon_select.jpg" alt="" /></a>
                        <input type="hidden" id="txtDeparts" runat="server" />
                        <input style="display: none" type="checkbox" name="chkPublishTo" value="cTour" <%=tourChecked %> />
                    </td>
                </tr>
                <tr>
                    <td height="35" align="right" bgcolor="#e3f1fc">
                        <strong>内容：</strong>
                    </td>
                    <td height="35" colspan="2" align="left" bgcolor="#FAFDFF" class="pandl3">
                        <textarea id="txtInfoContent" name="txtInfoContent" style="width: 680px; height: 360px;"
                            runat="server"></textarea>
                    </td>
                </tr>
                <tr>
                    <td height="35" align="right" bgcolor="#e3f1fc">
                        <strong>添加附件：</strong>
                    </td>
                    <td height="35" colspan="2" align="left" bgcolor="#FAFDFF" class="pandl3">
                        <uc1:UploadControl ID="UploadControl1" runat="server" IsUploadSelf="true" />
                        <asp:Label ID="LabelFile" runat="server" Text=""></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td height="35" align="right" bgcolor="#e3f1fc">
                        <strong>发布人：</strong>
                    </td>
                    <td height="35" colspan="2" align="left" bgcolor="#FAFDFF" class="pandl3">
                        <input class="inputtext" id="txtAuthor" runat="server" type="text" readonly="readonly"
                            size="25" />
                    </td>
                </tr>
                <tr>
                    <td height="35" align="right" bgcolor="#e3f1fc">
                        <strong>发布时间：</strong>
                    </td>
                    <td height="35" colspan="2" align="left" bgcolor="#FAFDFF" class="pandl3">
                        <input class="inputtext" id="txtPublishDate" runat="server" type="text" onfocus="WdatePicker({dateFmt:'yyyy-MM-dd HH:mm:ss'})"
                            size="25" />
                    </td>
                </tr>
                <tr>
                    <td height="30" colspan="3" align="center">
                        <table border="0" align="center" cellpadding="0" cellspacing="0">
                            <tr>
                                <td width="137" height="20" align="center" class="tjbtn02">
                                    <a href="javascript:;" onclick="return InfoEdit.save('');">保存</a>
                                </td>
                                <td width="137" height="20" align="center" class="tjbtn02">
                                    <a href="/CompanyFiles/MsgManageList.aspx">返回</a>
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
            </table>
            </form>
        </div>
    </div>

    <script type="text/javascript">
        function RemoveFile(obj) {
            $(obj).parent().remove();
        };

        $(document).ready(function() {
            KEditer.init('<%=txtInfoContent.ClientID %>', {
                resizeMode: 0,
                items: keSimple,
                height: "360px",
                width: "680px"
            });
        });
        var InfoEdit =
			    {
			        //打开弹窗
			        openDialog: function(p_url, p_title) {
			            Boxy.iframeDialog({ title: p_title, iframeUrl: p_url, width: "750px", height: "250px" });
			        },
			        //指定部门
			        selDeaprt: function() {
			            InfoEdit.openDialog("/CommonPage/SelectDepart.aspx?infoId=<%=infoId %>&dpids=" + $("#<%=txtDeparts.ClientID %>").val(), '选择部门');
			            return false;
			        },
			        //保存
			        save: function(method) {
			            var isValid = true;
			            KEditer.sync();
			            var msg = "";
			            var title = $.trim($("#<%= txtInfoTitle.ClientID %>").val());
			            if (title == "") {
			                msg += "请填写标题！<br />";
			                isValid = false;
			            }
			            if ($(":checked").length < 1) {
			                msg += "请选择发布对象！";
			                isValid = false;
			            }
			            if (msg != "") {
			                tableToolbar._showMsg(msg);
			            }
			            if (!isValid) { return false; }
			            if (method == "continue") {
			                $("#hidMethod").val("continue");
			            }
			            $("#<%=InfoFrom.ClientID %>").submit();
			            return false;
			        },
			        //选择部门后回调
			        selDepartBack: function(dIds) {
			            $("#<%=txtDeparts.ClientID %>").val(dIds);
			            if (dIds != "") {
			                $("input[class=selDeaprt]").attr("checked", "checked");
			            }
			            else {
			                $("input[class=selDeaprt]").removeAttr("checked");
			            }
			        }
			    }
    </script>

</asp:Content>
