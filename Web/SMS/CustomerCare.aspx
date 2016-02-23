<%@ Page Title="短信关怀-短信中心" Language="C#" MasterPageFile="~/masterpage/Front.Master"
    AutoEventWireup="true" CodeBehind="CustomerCare.aspx.cs" Inherits="Web.SMS.CustomerCare" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="lineprotitlebox">
        <table width="100%" border="0" cellspacing="0" cellpadding="0">
            <tr>
                <td width="15%" nowrap="nowrap">
                    <span class="lineprotitle">短信中心</span>
                </td>
                <td width="85%" nowrap="nowrap" align="right" style="padding: 0 10px 2px 0; color: #13509f;">
                    所在位置：短信中心 >> 客户关怀
                </td>
            </tr>
            <tr>
                <td colspan="2" height="2" bgcolor="#000000">
                </td>
            </tr>
        </table>
    </div>
    <div class="hr_10">
    </div>
    <form id="smsForm" runat="server">
    <input type="hidden" name="hidMethod" value="save" />
    <fieldset style="border: 2px #A3D3F8 solid;">
        <legend>设置</legend>
        <fieldset style="border: 1px #A3D3F8 solid; width: 50%; float: left; text-align: left;
            margin-right: 10px;">
            <legend>发送范围</legend>
            <table width="100%" border="0" align="center" cellpadding="0" cellspacing="1">
                <tr class="odd" style="line-height: 22px;">
                    <td width="18%" height="22px;" align="center">
                        匹配号码：
                    </td>
                    <td width="82%">
                        <asp:CheckBoxList ID="chkSendRange" runat="server" RepeatDirection="Horizontal">
                            <%--<asp:ListItem Text="匹配客户资料" Value="1"></asp:ListItem>--%>
                            <asp:ListItem Text="匹配供应商资料" Value="2"></asp:ListItem>
                            <%--<asp:ListItem Text="匹配部门人员" Value="3"></asp:ListItem>--%>
                        </asp:CheckBoxList>
                    </td>
                </tr>
                <tr class="even">
                    <td height="90" align="center">
                        输入号码：
                    </td>
                    <td>
                        &nbsp;<textarea id="txtMobile" runat="server" style="width: 270px; height: 70px;"></textarea>
                    </td>
                </tr>
                <tr class="odd">
                    <td height="30" colspan="2" align="left">
                        &nbsp;&nbsp;<a href="javascript:;" onclick="return Care.putInFile();">导入号码</a> （<font
                            class="fred">导入Excel和txt，选择发送范围后将不再允许导入</font>）
                    </td>
                </tr>
            </table>
        </fieldset>
        <fieldset style="border: 1px #A3D3F8 solid; width: 43%; text-align: right; float: left;">
            <legend>短信内容</legend>
            <table width="100%" border="0" cellspacing="1" cellpadding="0">
                <tr class="odd">
                    <td height="30" colspan="2" align="left">
                        &nbsp;
                        <input type="checkbox" id="chkName" runat="server" onclick="Care.changeName(this);" />
                        <label for="<%= chkName.ClientID %>">
                            匹配姓名</label>
                    </td>
                </tr>
                <tr class="even">
                    <td width="20%" height="90" align="center">
                        输入内容：
                    </td>
                    <td width="80%" align="left">
                        &nbsp;
                        <textarea id="txtSmsContent" style="width: 250px; height: 70px;" runat="server"></textarea>
                    </td>
                </tr>
                <tr class="odd">
                    <td height="30" align="center">
                        发送通道：
                    </td>
                    <td align="left">
                        &nbsp;&nbsp;
                        <select id="selSendChannel" runat="server">
                        </select>
                    </td>
                </tr>
            </table>
        </fieldset>
        <div class="clearboth">
        </div>
        <table width="600" border="0" align="left" cellpadding="0" cellspacing="0">
            <tr>
                <td width="363" height="40" align="left">
                    发送设置：<input type="radio" id="rdiFixTime" runat="server" checked="True" />固定时间发送发送设置
                    <input type="text" class="searchinput" style="width: 100px;" id="txtSendTime" runat="server"
                        onfocus="WdatePicker({dateFmt:'yyyy-MM-dd HH:mm'})" />
                </td>
                <td width="237" align="left">
                    <input type="radio" id="rdiCondit" runat="server" />
                    满足条件发送
                    <select id="selCondit" runat="server">
                        <option value="1">生日</option>
                        <option value="2">元旦</option>
                        <option value="3">春节</option>
                        <option value="4">元宵</option>
                        <option value="5">五一</option>
                        <option value="6">国庆</option>
                        <option value="7">中秋</option>
                        <option value="8">圣诞</option>
                    </select>
                </td>
            </tr>
            <tr>
                <td colspan="2" align="right">
                    <table width="420" border="0" cellpadding="0" cellspacing="0">
                        <tr>
                            <td height="40" align="center">
                            </td>
                            <td align="center" class="tjbtn02">
                                <a href="javascript:;" onclick="return Care.save();">保存</a>
                            </td>
                            <td height="40" align="center" class="tjbtn02">
                                <a href="javascript:;" onclick="return Care.reset();">重置</a>
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
        </table>
    </fieldset>
    </form>
    <div class="hr_10">
    </div>
    <fieldset style="border: 2px #A3D3F8 solid;" id="smsList">
        <legend>发送列表</legend>
    </fieldset>

    <script type="text/javascript">
        var Care =
           {
               //打开弹窗
               openDialog: function(p_url, p_title, p_width, p_height) {
                   Boxy.iframeDialog({ title: p_title, iframeUrl: p_url, width: p_width, height: p_height });
               },
               //从文件导入号码
               putInFile: function() {
                   var data = { txtMobiles: "<%=txtMobile.ClientID %>" };
                   if (!$("#<%=txtMobile.ClientID %>").attr("disabled")) {
                       Care.openDialog("/SMS/ImportTelFromFile.aspx?" + $.param(data), "从文件导入号码", "800px", "500px");
                   }
                   return false;
               },
               //验证手机号码
               validMobile: function(movalues) {
                   var dataMobile = [];

                   dataMobile = movalues.replace(/，/gi, ",").split(","); //导入时的手机号

                   var b = new Array();
                   var repeatM = new Array(); //重复手机号
                   var errM = new Array(); //格式错误手机号
                   var dataTelRight = new Array(); //正确的手机号
                   //循环验证手机号是否重复和号码格式
                   for (var i = 0, len = dataMobile.length; i < len; i++) {
                       //验证手机格式
                       if (!/^(13|15|18|14)\d{9}$/.test(dataMobile[i])) {
                           errM.push(dataMobile[i]); //压入格式错误的手机号
                           dataMobile[i] = null;
                           //设置当前为null
                       }
                       if (dataMobile[i] != null) {
                           if (b[dataMobile[i]] == null) {
                               b[dataMobile[i]] = i + 1;
                           }
                           else {
                               repeatM.push(dataMobile[i]); //重复号码压入重复数组
                               dataMobile[i] = null; //设置当前为null
                           }
                       }
                       if (dataMobile[i] != null) {//如果不为空则压入正确的号码数组
                           dataTelRight.push(dataMobile[i]);
                       }
                   }
                   var mobileMess = ""; //提示消息
                   if (repeatM.length > 0)
                       mobileMess += "\n号码重复" + repeatM.toString();
                   if (errM.length > 0)
                       mobileMess += "\n格式错误" + errM.toString();
                   if (mobileMess != "") {
                       alert("手机号错误提示！\n" + mobileMess)
                       return false;
                   }
                   else {
                       return dataMobile; //无任何错误时写入
                   }
               },
               save: function() {
                   var errMess = "";
                   if ($("#<%=chkSendRange.ClientID %>").find(":checkbox:checked").length == 0) {
                       var mvalues = $.trim($("#<%=txtMobile.ClientID %>").val());
                       if (mvalues == "") {
                           errMess += "未勾选匹配号码时必须输入发送号码！\n";
                       }
                       else {
                           if (!Care.validMobile(mvalues)) {
                               return false;
                           }
                       }
                   }

                   if ($.trim($("#<%=txtSmsContent.ClientID %>").val()) == "") {
                       errMess += "请填写短信内容！";
                   }
                   if (errMess != "") {
                       alert(errMess);
                       return false;
                   }
                   $("#<%=smsForm.ClientID %>").submit();
                   return false;
               },
               reset: function() {
                   $("#<%=smsForm.ClientID %>").get(0).reset();
                   if ($("input[name*='chkSendRange']:checked").length > 0) {
                       $("#<%=txtMobile.ClientID %>").attr("disabled", "disabled");
                   }
                   else {
                       $("#<%=txtMobile.ClientID %>").removeAttr("disabled")
                   }
                   return false;
               },
               del: function(sid) {
                   if (confirm("你确定要删除该短信记录吗？")) {
                       $.newAjax({
                           type: "post",
                           dataType: "json",
                           url: "CustomerCare.aspx",
                           data: { hidMethod: "del", sid: sid },
                           cache: false,
                           success: function(result) {
                               alert(result.message);
                               if (result.success == "1") {
                                   Care.getSmsList("")
                               }
                           }
                       });


                   }
                   return false;
               },
               stop: function(sid, tar) {
                   var sonFont = $(tar).find("font");
                   var nowM = ""; //当前操作
                   if (sonFont.html() == "停发") {
                       nowM = "stop";
                   }
                   else {
                       nowM = "start";
                   }
                   $.newAjax({
                       type: "post",
                       dataType: "json",
                       url: "CustomerCare.aspx",
                       data: { hidMethod: nowM, sid: sid },
                       cache: false,
                       success: function(result) {
                           alert(result.message);
                           if (result.success == "1") {
                               if (nowM == "stop") {
                                   sonFont.html("开启");
                               }
                               else {
                                   sonFont.html("停发");
                               }
                           }
                       }
                   });
               },
               getSmsList: function(tar, method, id) {
                   var page = tar = "" ? "1" : $(tar).attr("gotopage");
                   $.newAjax({
                       type: "GET",
                       dataType: "text",
                       url: "AjaxSendSMSList.aspx",
                       data: { Page: page, Method: method, sid: id },
                       cache: false,
                       success: function(result) {
                           $("#smsList").html(result);
                           if (method && method == "del") {
                               alert("删除成功！");
                           }
                       }
                   });
               },
               changeName: function(tar) {
                   var content = $("#<%=txtSmsContent.ClientID %>");
                   if ($(tar).attr("checked")) {
                       content.val('[姓名]' + content.val());
                   }
                   else {
                       content.val(content.val().replace(/\[姓名\]/g, ""));
                   }
               }

           }
        $(document).ready(function() {
            $("input[name*='chkSendRange']").change(function() {
                if ($("input[name*='chkSendRange']:checked").length > 0) {
                    $("#<%=txtMobile.ClientID %>").attr("disabled", "disabled");
                }
                else {
                    $("#<%=txtMobile.ClientID %>").removeAttr("disabled")
                }
            });
            Care.getSmsList("");

        });
    </script>

</asp:Content>
