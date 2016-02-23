<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="EditCommonSms.aspx.cs"
    Inherits="Web.SMS.EditCommonSms" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>短信中心_短信中心_编辑短语</title>
    <link href="/css/style.css" rel="stylesheet" type="text/css" />
    <style type="text/css">
        .errmsg
        {
            color: Red;
            margin-left: 5px;
        }
    </style>

    <script src="/js/jquery-1.4.4.js" type="text/javascript"></script>

    <script src="/js/ValiDatorForm.js" type="text/javascript"></script>

</head>
<body>
    <form id="smsForm" runat="server">
    <div>
        <input type="hidden" name="hidMethod" id="hidMethod" value="save" />
        <table width="500" border="0" align="center" cellpadding="0" cellspacing="1" style="margin: 20px auto;">
            <tr class="odd">
                <th width="18%" height="30" align="right">
                    类型：
                </th>
                <td width="82%" bgcolor="#E3F1FC">
                    <select id="selClass" runat="server" class="inputselect" valid="required" errmsg="类型不为空">
                    </select>
                    <img src="/images/add2.gif" /><a href="javascript:;" onclick="return SmsCommon.toggleClass();">新增</a>
                    <span id="spanClass" style="display: none">
                        <input type="text" id="txtSmsClass" style="widows: 80px" maxlength="15" /><input
                            type="button" id="btnSave" onclick="SmsCommon.addClass();" value="增加" /></span>
                    <a href="javascript:;" onclick="SmsCommon.delClass();">删除</a><span id="errMsg_<%=selClass.ClientID %>"
                        class="errmsg"></span>
                </td>
            </tr>
            <tr class="odd">
                <th width="18%" height="30" align="right">
                    短信内容：
                </th>
                <td width="82%" bgcolor="#E3F1FC">
                    <textarea cols="45" rows="5" id="txtSmsContent" runat="server" onkeyup="SmsCommon.fontNum(this);"
                        style="vertical-align: top" valid="required" errmsg="内容不为空" class="inputarea"></textarea>
                    <span id="errMsg_<%=txtSmsContent.ClientID %>" class="errmsg"></span>
                    <div style="color: Red;">
                        注：短信内容不能超过70个字，当前字数<font id="smsNum">0</font></div>
                </td>
            </tr>
            <tr class="odd">
                <td height="30" colspan="8" align="left" bgcolor="#E3F1FC">
                    <table width="340" border="0" cellspacing="0" cellpadding="0">
                        <tr>
                            <td width="106" height="40" align="center">
                            </td>
                            <td width="76" height="40" align="center" class="tjbtn02">
                                <a href="javascript:;" id="btn_save" onclick="return SmsCommon.save('');">保存</a>
                            </td>
                            <td width="158" height="40" align="center">
                                <span class="tjbtn02"><a href="javascript:;" onclick="window.parent.Boxy.getIframeDialog('<%=Request.QueryString["iframeId"]%>').hide();return false;">
                                    关闭</a></span>
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
        </table>
    </div>
    </form>

    <script type="text/javascript">
        $(document).ready(function() {
            FV_onBlur.initValid($("#<%=smsForm.ClientID %>").get(0));
        });
        var SmsCommon =
    {
        //保存表单
        save: function(method) {
            var formObj = $("#<%=smsForm.ClientID %>").get(0);
            var vResult = ValiDatorForm.validator(formObj, "span");
            if (!vResult) return false;
            if (method == "continue") {
                document.getElementById("hidMethod").value = "continue";
            }
            if ($("#txtSmsContent").val().length > 70) {
                alert("短信字数超过！");
                return false;
            }
            formObj.submit();
            return false;
        },
        fontNum: function(tar) {
            $("#smsNum").html($(tar).val().length);
        },
        delClass: function() {
            var classId = $("#<%=selClass.ClientID %>").val();
            if (classId == "0") {
                alert("请选择要删除的类别！");
                return false;
            }
            $.newAjax(
                      {
                          url: "/SMS/EditCommonSms.aspx",
                          data: { hidMethod: "delClass", classId: classId },
                          dataType: "json",
                          cache: false,
                          type: "post",
                          success: function(result) {
                              if (result.success == "1") {
                                  alert("已删除！");
                                  $("#<%=selClass.ClientID %> option:selected").remove();
                              }
                              else {
                                  alert("删除失败！");
                              }
                          },
                          error: function() {
                              alert("删除失败!");
                          }
                      })
            return false;
        },
        //添加客户类别
        addClass: function() {
            var className = $.trim($("#txtSmsClass").val());
            if (className == "") {
                alert("请填写类别！");
                return false;
            }
            $.newAjax(
                      {
                          url: "/SMS/EditCommonSms.aspx",
                          data: { hidMethod: "addClass", className: className },
                          dataType: "json",
                          cache: false,
                          type: "post",
                          success: function(result) {
                              if (result.success == "1") {
                                  alert("添加成功！");
                                  $("#txtSmsClass").val("");
                                  $("#<%=selClass.ClientID %>").append("<option value='" + result.message + "'>" + className + "</option>");
                              }
                              else {
                                  alert("添加失败！");
                              }
                          },
                          error: function() {
                              alert("添加失败!");
                          }
                      })
            return false;
        },
        toggleClass: function() {
            $("#spanClass").toggle();
            return false;
        }
    }
    </script>

</body>
</html>
