<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ImportTelFromFile.aspx.cs"
    Inherits="Web.SMS.ImportTelFromFile" %>

<%@ Register Src="~/UserControl/ExcelFileUploadControl.ascx" TagName="upload" TagPrefix="uc1" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>从文件导入号码</title>
    <link href="/css/style.css" rel="stylesheet" type="text/css" />
    <link href="/css/swfupload/default.css" rel="stylesheet" type="text/css" />

    <script type="text/javascript" src="/js/jquery-1.4.4.js"></script>

    <script type="text/javascript" src="/js/swfupload/swfupload.js"></script>

    <script type="text/javascript" src="/js/loadExcel.js"></script>

    <script type="text/javascript">
        function loadexcel(array) {
            loadXls.init(array, "#tablelist", ".s1");
            $("#selectall").bind("click", function() {
                loadXls.selectAll(true);
            });
            $("#reset").bind("click", function() {
                loadXls.selectAll(false);
            });
            $("#selectback").bind("click", function() {
                loadXls.selectback();
            });
            $("#ok").bind("click", function() {
                if ($("#stel").val() == "-1") {
                    alert("请设置对应字段！");
                    return false;
                }
                var dataMobile = loadXls.bindIndex([$("#stel").val()]); //导入时的手机号

                if (!dataMobile || dataMobile == "") {
                    alert("请选择要导入的手机号！");
                    return false;
                }
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
                var nowBoxy = window.parent.Boxy.getIframeDialog('<%=Request.QueryString["iframeId"] %>');
                var txtMobile = window.parent.document.getElementById('<%= EyouSoft.Common.Utils.GetQueryStringValue("txtMobiles") %>');
                if (mobileMess != "") {
                    if (confirm("是否过滤重复或格式错误的手机号？\n" + mobileMess)) {
                        txtMobile.value = dataTelRight.toString(); //过滤后写入
                    }
                    else {
                        txtMobile.value = dataTelRight.concat(errM, repeatM).toString(); //不过滤写入
                    }
                }
                else {
                    txtMobile.value = dataMobile.toString(); //无任何错误时写入
                }
                nowBoxy.hide();


            });

        }
    </script>

</head>
<body>
    <div style="padding: 10px">
        <form id="form" runat="server">
        <uc1:upload ID="Upload1" UploadSuccessJavaScriptFunCallBack="loadexcel" runat="server"
            ContainsTxt="true" />
        <table cellspacing="0" cellpadding="0" width="98%" align="center" border="0">
            <tr>
                <td>
                    <fieldset>
                        <legend>源数据预览&nbsp;&nbsp;&nbsp;&nbsp;</legend>
                        <table height="30" cellspacing="0" cellpadding="0" width="98%" align="center" border="0">
                            <tr>
                                <td>
                                    <div id="tablelist">
                                    </div>
                                    <input type="button" id="selectall" value="全选" />
                                    <input type="button" id="reset" value="清空">
                                    <input type="button" id="selectback" value="反选" />
                                </td>
                            </tr>
                        </table>
                    </fieldset>
                </td>
            </tr>
        </table>
        <table cellspacing="0" cellpadding="0" width="98%" align="center" border="0">
            <tbody>
                <tr>
                    <td style="padding-top: 15px;">
                        <fieldset>
                            <legend>请设置对应字段</legend>
                            <table cellspacing="0" id="tbl_Cell" cellpadding="5" width="98%" align="center" border="0">
                                <tbody>
                                    <tr>
                                        <td>
                                            <label>
                                                手机号码：</label>
                                            <select id="stel" class="s1">
                                            </select>
                                        </td>
                                    </tr>
                                </tbody>
                            </table>
                            <table width="98%" border="0" align="center" cellpadding="0" cellspacing="0">
                                <tr>
                                    <td height="40" align="center" class="tjbtn02">
                                        <a href="javascript:;" id="ok">保存</a>
                                    </td>
                                </tr>
                            </table>
                        </fieldset>
                    </td>
                </tr>
            </tbody>
        </table>
        </form>
    </div>
</body>
</html>
