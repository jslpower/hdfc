<%@ Page Title="公司信息" Language="C#" MasterPageFile="~/MasterPage/Front.Master" AutoEventWireup="true"
    CodeBehind="CompanyInfo.aspx.cs" Inherits="Web.SystemSet.CompanyInfo" %>

<%@ Register Src="../UserControl/UploadControl.ascx" TagName="UploadControl" TagPrefix="uc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="mainbody">
        <!-- InstanceBeginEditable name="EditRegion3" -->
        <div class="mainbody">
            <div class="lineprotitlebox">
                <table width="100%" cellspacing="0" cellpadding="0" border="0">
                    <tbody>
                        <tr>
                            <td width="15%" nowrap="nowrap">
                                <span class="lineprotitle">公司信息</span>
                            </td>
                            <td width="85%" nowrap="nowrap" align="right" style="padding: 0 10px 2px 0; color: #13509f;">
                                <b>当前您所在位置</b>&gt;&gt; 系统设置 &gt;&gt; 公司信息
                            </td>
                        </tr>
                        <tr>
                            <td height="2" bgcolor="#000000" colspan="2">
                            </td>
                        </tr>
                    </tbody>
                </table>
            </div>
            <div style="height: 30px;" class="lineCategorybox">
            </div>
            <form runat="server" id="companyFrom">
            <input type="hidden" id="hfileId" runat="server" />
            <div class="tablelist">
                <input type="hidden" name="hidMethod" id="hidMethod" value="save" />
                <table width="780" cellspacing="1" cellpadding="0" border="0" bgcolor="#BDDCF4" align="center">
                    <tbody>
                        <tr>
                            <th colspan="3" align="center" bgcolor="#BDDCF4">
                                填写公司信息
                            </th>
                        </tr>
                        <tr>
                            <td width="16%" height="35" align="right" bgcolor="#e3f1fc">
                                <span style="color: Red; font-size: 12px;">*</span><strong>公司名称：</strong>
                            </td>
                            <td height="35" colspan="2" align="left" bgcolor="#FAFDFF" class="pandl3">
                                <input id="txtCompanyName" runat="server" type="text" size="50" valid="required"
                                    errmsg="公司名称不为空" class="inputtext" />
                                <span id="errMsg_<%=txtCompanyName.ClientID %>" class="errmsg"></span>
                            </td>
                        </tr>
                        <tr>
                            <td height="35" align="right" bgcolor="#e3f1fc">
                                <strong>旅行社类别：</strong>
                            </td>
                            <td height="35" colspan="2" align="left" bgcolor="#FAFDFF" class="pandl3">
                                <input id="txtType" runat="server" type="text" size="50" class="inputtext" />
                            </td>
                        </tr>
                        <tr>
                            <td height="35" align="right" bgcolor="#e3f1fc">
                                <strong>公司英文名称：</strong>
                            </td>
                            <td height="35" colspan="2" align="left" bgcolor="#FAFDFF" class="pandl3">
                                <input id="txtEngName" runat="server" type="text" size="50" class="inputtext" />
                            </td>
                        </tr>
                        <tr>
                            <td height="35" align="right" bgcolor="#e3f1fc">
                                <strong>许可证号：</strong>
                            </td>
                            <td height="35" colspan="2" align="left" bgcolor="#FAFDFF" class="pandl3">
                                <input id="txtLicence" runat="server" type="text" size="50" class="inputtext" />
                            </td>
                        </tr>
                        <tr>
                            <td height="35" align="right" bgcolor="#e3f1fc">
                                <strong>公司负责人：</strong>
                            </td>
                            <td height="35" colspan="2" align="left" bgcolor="#FAFDFF" class="pandl3">
                                <input id="txtAdmin" runat="server" type="text" size="50" class="inputtext" />
                            </td>
                        </tr>
                        <tr>
                            <td height="35" align="right" bgcolor="#e3f1fc">
                                <strong>电话：</strong>
                            </td>
                            <td height="35" colspan="2" align="left" bgcolor="#FAFDFF" class="pandl3">
                                <input id="txtTel" runat="server" type="text" size="50" valid="isPhone" errmsg="格式不正确"
                                    class="inputtext" />
                                <span id="errMsg_<%=txtTel.ClientID %>" class="errmsg"></span>
                            </td>
                        </tr>
                        <tr>
                            <td height="35" align="right" bgcolor="#e3f1fc">
                                <strong>手机：</strong>
                            </td>
                            <td height="35" colspan="2" align="left" bgcolor="#FAFDFF" class="pandl3">
                                <input id="txtMoible" runat="server" type="text" size="50" valid="isMobile" errmsg="格式不正确"
                                    class="inputtext" />
                                <span id="errMsg_<%=txtMoible.ClientID %>" class="errmsg"></span>
                            </td>
                        </tr>
                        <tr>
                            <td height="35" align="right" bgcolor="#e3f1fc">
                                <strong>传真：</strong>
                            </td>
                            <td height="35" colspan="2" align="left" bgcolor="#FAFDFF" class="pandl3">
                                <input id="txtFax" runat="server" type="text" size="50" class="inputtext" />
                            </td>
                        </tr>
                        <tr>
                            <td height="35" align="right" bgcolor="#e3f1fc">
                                <strong>地址：</strong>
                            </td>
                            <td height="35" colspan="2" align="left" bgcolor="#FAFDFF" class="pandl3">
                                <input id="txtAddress" runat="server" type="text" size="90" class="inputtext" />
                            </td>
                        </tr>
                        <tr>
                            <td height="35" align="right" bgcolor="#e3f1fc">
                                <strong>邮编：</strong>
                            </td>
                            <td height="35" colspan="2" align="left" bgcolor="#FAFDFF" class="pandl3">
                                <input id="txtEmail" runat="server" type="text" size="50" class="inputtext" />
                            </td>
                        </tr>
                        <tr>
                            <td height="35" align="right" bgcolor="#e3f1fc">
                                <strong>公司网站：</strong>
                            </td>
                            <td height="35" colspan="2" align="left" bgcolor="#FAFDFF" class="pandl3">
                                <input id="txtWeb" runat="server" type="text" size="50" class="inputtext" />
                            </td>
                        </tr>
                        <tr>
                            <td height="35" bgcolor="#e3f1fc" align="right">
                                <strong>附件上传：</strong>
                            </td>
                            <td height="35" bgcolor="#FAFDFF" align="left" class="pandl3" colspan="2">
                                <uc1:UploadControl ID="UploadControl1" runat="server" />
                                <div style="width: 450px; float: left; margin-left: 5px;">
                                    <asp:Repeater ID="rplfile" runat="server">
                                        <ItemTemplate>
                                            <a target="_blank" href='<%#Eval("FilePath") %>' style="vertical-align: bottom">查看</a><span><img
                                                alt="" style="vertical-align: bottom; cursor: pointer;" src="/images/cha.gif"
                                                data-delimg="delimg" /><input type="hidden" data-id='<%#Eval("FileId") %>' name="hidefile"
                                                    value="<%#Eval("FilePath") %>" /></span>
                                        </ItemTemplate>
                                    </asp:Repeater>
                                </div>
                            </td>
                        </tr>
                        <tr>
                            <td height="30" align="center" colspan="3">
                                <table cellspacing="0" cellpadding="0" border="0" align="left">
                                    <tbody>
                                        <tr>
                                            <td width="114" height="40" align="center">
                                            </td>
                                            <td width="84" height="40" align="center" class="tjbtn02">
                                                <a href="javascript:;" id="btn_save" onclick="return save();">保存</a>
                                            </td>
                                        </tr>
                                    </tbody>
                                </table>
                            </td>
                        </tr>
                    </tbody>
                </table>
            </div>
            </form>
        </div>
        <!-- InstanceEndEditable -->
    </div>
    <div class="clearboth">
    </div>

    <script type="text/javascript">
        $(document).ready(function() {
            var fileid = "";
            FV_onBlur.initValid($("#btn_save").closest("form").get(0));

            $(".tablelist").find("img[data-delimg='delimg']").click(function() {
                var thisimg = $(this).parent();
                thisimg.prev("a").hide();
                fileid += thisimg.find("input[type='hidden']").attr("data-id") + ",";
                $("#<%=hfileId.ClientID %>").val(fileid);
                thisimg.find("input[type='hidden']").val("");
                thisimg.hide();
                return false;
            })
        });

        //保存表单
        function save(method) {
            var form = $("#btn_save").closest("form").get(0);
            var vResult = ValiDatorForm.validator(form, "span");
            if (!vResult) return false;
            form.submit();
            return false;
        }
   
    </script>

</asp:Content>
