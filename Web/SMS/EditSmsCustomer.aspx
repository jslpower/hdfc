<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="EditSmsCustomer.aspx.cs"
    Inherits="Web.SMS.EditSmsCustomer" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>短信中心_短信中心_客户编辑</title>
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
    <form id="custForm" runat="server">
    <div>
        <input type="hidden" name="hidMethod" id="hidMethod" value="save" />
        <table width="600" border="0" align="center" cellpadding="0" cellspacing="1" style="margin: 0px auto;">
            <tr class="odd">
                <th width="18%" height="30" align="right">
                    手机号码：
                </th>
                <td width="82%" bgcolor="#E3F1FC">
                    <input id="txtMobile" type="text" class="xtinput inputtext" style="width: 100px"
                        runat="server" size="30" valid="required|isMobile" errmsg="手机号码不为空|手机号码格式不正确" />
                    <span id="errMsg_<%=txtMobile.ClientID %>" class="errmsg"></span>
                </td>
            </tr>
            <tr class="odd">
                <th width="18%" height="30" align="right">
                    单位名称：
                </th>
                <td width="82%" bgcolor="#E3F1FC">
                    <input type="text" class="xtinput inputtext" id="txtCompanyName" runat="server" size="40"
                        maxlength="30" />
                </td>
            </tr>
            <tr class="odd">
                <th width="18%" height="30" align="right">
                    姓名：
                </th>
                <td width="82%" bgcolor="#E3F1FC">
                    <input id="txtUserName" runat="server" type="text" class="xtinput inputtext" size="30"
                        valid="required" errmsg="姓名不为空" maxlength="20" />
                    <span id="errMsg_<%=txtUserName.ClientID %>" class="errmsg"></span>
                </td>
            </tr>
            <tr class="odd">
                <th width="18%" height="30" align="right">
                    客户分类：
                </th>
                <td width="82%" bgcolor="#E3F1FC">
                    <select id="selClass" runat="server" class="inputselect" valid="required" errmsg="类型不为空">
                    </select>
                    <img src="/images/add2.gif" /><a href="javascript:;" onclick="return SmsCust.toggleClass();">新增</a>
                    <span id="spanClass" style="display: none">
                        <input type="text" id="txtCustClass" style="widows: 80px" maxlength="15" /><input
                            type="button" id="btnSave" onclick="SmsCust.addClass();" value="增加" /></span>
                    <a href="javascript:;" onclick="SmsCust.delClass();">删除</a><span id="errMsg_<%=selClass.ClientID %>"
                        class="errmsg"></span>
                </td>
            </tr>
            <tr class="odd">
                <th width="18%" height="30" align="right">
                    备注：
                </th>
                <td width="82%" bgcolor="#E3F1FC">
                    <textarea cols="60" rows="5" runat="server" id="txtRemark" class="inputarea"></textarea>
                </td>
            </tr>
            <tr class="odd">
                <td height="30" colspan="8" align="left" bgcolor="#E3F1FC">
                    <table width="340" border="0" cellspacing="0" cellpadding="0">
                        <tr>
                            <td width="106" height="40" align="center">
                            </td>
                            <td width="76" height="40" align="center" class="tjbtn02">
                                <a href="javascript:;" id="btn_save" onclick="return SmsCust.save('');">保存</a>
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
            FV_onBlur.initValid($("#<%=custForm.ClientID %>").get(0));
        });
        var SmsCust =
    {
        //保存表单
        save: function(method) {
            var formObj = $("#<%=custForm.ClientID %>").get(0);
            var vResult = ValiDatorForm.validator(formObj, "span");
            if (!vResult) return false;
            if (method == "continue") {
                document.getElementById("hidMethod").value = "continue";
            }
            formObj.submit();
            return false;
        },
        delClass: function() {
            var classId = $("#<%=selClass.ClientID %>").val();
            if (classId == "0") {
                alert("请选择要删除的类别！");
                return false;
            }
            $.newAjax(
                      {
                          url: "/SMS/EditSmsCustomer.aspx",
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
            var className = $.trim($("#txtCustClass").val());
            if (className == "") {
                alert("请填写类别！");
                return false;
            }
            $.newAjax(
                      {
                          url: "/SMS/EditSmsCustomer.aspx",
                          data: { hidMethod: "addClass", className: className },
                          dataType: "json",
                          cache: false,
                          type: "post",
                          success: function(result) {
                              if (result.success == "1") {
                                  alert("添加成功！");
                                  $("#txtCustClass").val("");
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
