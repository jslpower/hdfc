<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="login.aspx.cs" Inherits="Web.Webmaster.login" %>

<%@ Register Src="~/UserControl/UploadControl.ascx" TagName="UploadControl" TagPrefix="uc1" %>
<!DOCTYPE html>
<html>
<head>
    <title>温州壹百项目管理系统WEBMASTER</title>
    <style type="text/css">	
	body {font-size: 14px; font-family: Verdana; margin-top:10px; margin-left:20px; margin-right:20px;}
    a:link {color: #31659C;text-decoration: none;}
    a:visited {color: #31659C;text-decoration: none;}
    a:hover {color: #CE6500;text-decoration: none;}
    form {margin:0px;padding:0px;}
    .input_text{border:1px solid #003c74;font-size: 11pt;width: 180px;cursor: text;height: 18px;}
	</style>

    <script type="text/javascript" src="/js/jquery-1.4.4.js"></script>

    <script type="text/javascript">
        var _p = parent;

        try {
            var _iframe = _p.opiframe || _p.window.frames["opiframe"] || _p.document.getElementById("opiframe").contentWindow;
            _p.location.href = this.location;
        }
        catch (e) { }

        function WebForm_OnSubmit_Validate() {
            if ($.trim($("#t_u").val()) == "") {
                alert('Please enter your login information.');
                $("#t_u").focus();
                return false;
            }
            if ($.trim($("#t_p").val()) == "") {
                alert('Please enter a password.');
                $("#t_p").focus();
                return false;
            }
            return true;
        }

        $(document).ready(function() {
            $("#t_u").focus();
            $("#<%=btnLogin.ClientID %>").bind("click", function() { return WebForm_OnSubmit_Validate(); });
        });
    </script>
    <%--text:000000 MD5:670b14728ad9902aecba32e22fa4f6bd--%>
</head>
<body>
    <form id="form1" runat="server">
    <div>
        <div>
            温州壹百项目管理系统，开发者推荐使用<a href="http://www.firefox.com.cn/" target="_blank">firefox</a>或<a href="http://www.google.cn/chrome/intl/zh-CN/landing_chrome.html?hl=zh-CN&brand=CHMI" target="_blank">google</a>浏览器。
            <br />
            <br />
        </div>
        <div>
            <p>
                登录账号：<input id="t_u" class="input_text" name="t_u" size="20" type="text" />
            </p>
            <p>
                登录密码：<input id="t_p" class="input_text" name="t_p" size="20" type="password" />
            </p>
            <p>
                <asp:Button runat="server" ID="btnLogin" onclick="btnLogin_Click" Text="login" />
            </p>
        </div>
        <div>
            <br />
            <br />
            <span style="font-family: Arial,Helvetica,sans-serif">Copyright &copy; 2008-2012 杭州易诺科技，All Rights Reserved.</span>
            <br />
            <br />
        </div>
    </div>
    
    <%--
    
    <table style="background: #efefef">
        <tr>
            <td style="width: 100px; height: 100px; background: #333">
                1
            </td>
            <td style="width: 100px; text-align: center;">
                <div style="position: relative; width: 100px; height: 100px; line-height:100px;">
                    盖章
                    <div style="background: #ff0000; position: absolute; left: 0px; top: 0px; width: 100px;height: 100px; display:none; ">
                        333
                    </div>
                </div>
            </td>
        </tr>
    </table>
    
    <link rel="stylesheet" type="text/css" href="/css/swfupload/default.css" />  
    <script type="text/javascript" src="/js/swfupload/swfupload.js"></script>
    <script type="text/javascript" src="/js/swfupload/handlers.js"></script>
    <div>
    <uc1:UploadControl ID="UploadFuJian" runat="server" CompanyID="1"  IsUploadMore="true" />
    <uc1:UploadControl ID="UploadFuJian1" runat="server" CompanyID="1"
        IsUploadMore="true" />
    <uc1:UploadControl ID="UploadFuJian2" runat="server" CompanyID="1"  />
    <input type="button" value="提交表单" id="btn2" />
    <input type="button" value="开始上传" onclick="btn1_fn()" id="btn1" />
    </div>
    <br/>
    <br />
    <br />
    <br />
    <br />
    <br />
    <br />
    <br />
    <br />
    <br />
    <br />
    <br />
    <br />
    
    <div style="clear:both">
    <uc1:UploadControl ID="UploadControl1" runat="server" CompanyID="1" IsUploadMore="true" />
    <uc1:UploadControl ID="UploadControl2" runat="server" CompanyID="1" 
        IsUploadMore="true" IsUploadSelf="true" />
    <uc1:UploadControl ID="UploadControl3" runat="server" CompanyID="1" />
    </div>
    
    
    <input type="button" value="提交表单" id="btn3" />
    <input type="button" value="开始上传" onclick="btn4_fn()" id="btn4" />
    
    <script type="text/javascript">
        function btn1_fn() {
            window["<%=UploadFuJian.ClientID %>"].startUpload();
            window["<%=UploadFuJian1.ClientID %>"].startUpload();
            window["<%=UploadFuJian2.ClientID %>"].startUpload();
        }

        function btn4_fn() {
            window["<%=UploadControl1.ClientID %>"].startUpload();
            window["<%=UploadControl2.ClientID %>"].startUpload();
            window["<%=UploadControl3.ClientID %>"].startUpload();
        }

        function s() {
            alert("S");
            $("#btn2").attr("disabled", "disabled")
        }

        function c() {
            alert("C")
            $("#btn2").removeAttr("disabled")
        }

        function s1() {
            alert("S1");
            $("#btn3").attr("disabled", "disabled")
        }

        function c1() {
            alert("C1")
            $("#btn3").removeAttr("disabled")
        }

        $(document).ready(function() {
            swfUploadHandler.init({ movies: [window["<%=UploadFuJian.ClientID %>"], window["<%=UploadFuJian1.ClientID %>"], window["<%=UploadFuJian2.ClientID %>"]], startFn: s, completeFn: c });
            swfUploadHandler.init({ movies: [window["<%=UploadControl1.ClientID %>"], window["<%=UploadControl2.ClientID %>"], window["<%=UploadControl3.ClientID %>"]], startFn: s1, completeFn: c1 });
        });  
    </script>--%>
    
    </form>
</body>
</html>
