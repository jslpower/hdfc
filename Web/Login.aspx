<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Login.aspx.cs" Inherits="Web.Login" %>

<!DOCTYPE html>
<html>
<head>
    <title>系统登录-<%=CompanyName %>-管理系统</title>
    <link href="/css/login.css" rel="stylesheet" type="text/css" />
    <link href="/css/boxy.css" rel="stylesheet" type="text/css" />
    <!--[if lt IE 7]>
        <script type="text/javascript" src="/js/unitpngfix.js"></script>
    <![endif]-->
</head>
<body>
    <form id="form1" runat="server">
    <div style="position: absolute; top: 50%; left: 50%; width: 962px; height: 526px;
        margin: -263px 0 0 -481px;">
        <table width="960" border="0" align="center" cellpadding="0" cellspacing="0">
            <tr>
                <td height="19">
                    <table width="960" height="85" border="0" align="center" cellpadding="0" cellspacing="0">
                        <tr style="background: url(/images/topbg.jpg) no-repeat; width: 960px; height: 85px;">
                            <td width="60%" valign="top" class="login_logo">
                                <img src="<%=LogoFilePath %>" alt="<%=CompanyName %>" />
                            </td>
                            <td width="40%" align="center">
                                <table width="78%" height="52" border="0" cellpadding="0" cellspacing="0">
                                    <tr>
                                        <td class="wz">
                                            咨询热线：<%= ContactTel%>
                                            <a href="javascript:void(0);" class="wz" onclick="SetHome(this,window.location)">设为首页
                                            </a>| <a href="javascript:void(0);" class="wz" onclick="AddFavorite(window.location,document.title)">
                                                加入收藏</a>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="wz">
                                            现在时间：<%=DateTime.Now.ToString("yyyy年M月d日 dddd")   %>
                                            <span id="span_clock">00:00:00</span>
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                        <tr>
                            <td height="5" colspan="2" valign="top" bgcolor="#114287">
                            </td>
                        </tr>
                    </table>
                    <table width="960" height="274" border="0" align="center" cellpadding="0" cellspacing="0">
                        <tr>
                            <td background="/images/11.jpg">
                                &nbsp;
                            </td>
                            <td width="491" height="394" background="/images/12.jpg">
                                <table width="96%" height="224" border="0" cellpadding="0" cellspacing="0">
                                    <tr>
                                        <td height="69">
                                            &nbsp;
                                        </td>
                                    </tr>
                                    <tr>
                                        <td height="155" align="center" valign="top">
                                            <table width="75%" height="131" border="0" cellpadding="0" cellspacing="0">
                                                <tr>
                                                    <td width="17%">
                                                        &nbsp;
                                                    </td>
                                                    <td width="18%" align="right" class="wz">
                                                        用户名：
                                                    </td>
                                                    <td colspan="2" align="left">
                                                        <label>
                                                            <input type="text" name="t_u" id="t_u" tabindex="1" class="kang" />
                                                        </label>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        &nbsp;
                                                    </td>
                                                    <td align="right" class="wz">
                                                        密 码：
                                                    </td>
                                                    <td colspan="2" align="left">
                                                        <input type="password" name="t_p" id="t_p" tabindex="2" class="kang" />
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        &nbsp;
                                                    </td>
                                                    <td align="right" class="wz">
                                                        验证码：
                                                    </td>
                                                    <td width="25%" align="left">
                                                        <input type="text" name="t_vc" id="t_vc" class="kangone" style="width: 60px" tabindex="3" />
                                                    </td>
                                                    <td width="40%" align="left">
                                                        <img alt="点击更换验证码" title="点击更换验证码" style="cursor: pointer; margin-top: -4px;" onclick="this.src='/commonpage/validatecode.ashx?ValidateCodeName=SYS_YIBAI_VC&id='+Math.random();return false;"
                                                            align="middle" width="60" height="20" id="img1" src="/commonpage/validatecode.ashx?ValidateCodeName=SYS_YIBAI_VC&t=<%=DateTime.Now.ToString("HHmmssffff") %>" />
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        &nbsp;
                                                    </td>
                                                    <td align="right" class="wz">
                                                        &nbsp;
                                                    </td>
                                                    <td align="left">
                                                        <a href="javascript:void(0)" id="lnkLogin" tabindex="4">
                                                            <img src="/images/bottm.jpg" width="64" height="30" border="0" /></a>
                                                    </td>
                                                    <td>
                                                        <div style="text-align: center">
                                                            <span id="span_msg" style="color: red"></span>
                                                        </div>
                                                    </td>
                                                </tr>
                                            </table>
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                    </table>
                    <table width="960" border="0" align="center" cellpadding="0" cellspacing="0">
                        <tr>
                            <td height="5" bgcolor="#114287">
                            </td>
                        </tr>
                    </table>
                    <table width="960" height="36" border="0" align="center" cellpadding="0" cellspacing="0"
                        style="margin-top: 1px;">
                        <tr>
                            <td height="36" align="center" background="/images/end.jpg" class="wz">
                                版权所有：<%= CompanyName %>
                                技术支持：杭州易诺科技
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
        </table>
    </div>
    </form>

    <script src="/js/jquery-1.4.4.js" type="text/javascript"></script>

    <script src="/js/slogin.js" type="text/javascript"></script>

    <script type="text/javascript">
        function clock() { var _t = new Date(); var _w = _t.getDay(); var _h = _t.getHours(); var _m = _t.getMinutes(); var _s = _t.getSeconds(); var _h1 = _h; var _m1 = _m; var _s1 = _s; if (_h < 10) { _h1 = "0" + _h }; if (_m < 10) { _m1 = "0" + _m }; if (_s < 10) { _s1 = "0" + _s }; $("#span_clock").html(_h1 + ":" + _m1 + ":" + _s1); }
        function getVC() {
            var c = document.cookie, ckcode = "", tenName = "";
            for (var i = 0; i < c.split(";").length; i++) {
                tenName = c.split(";")[i].split("=")[0];
                ckcode = c.split(";")[i].split("=")[1];
                if ($.trim(tenName) == "SYS_YIBAI_VC") {
                    break;
                } else {
                    ckcode = "";
                }
            }
            return $.trim(ckcode);
        };

        function setMsg(s) {
            $("#span_msg").html(s);
        }

        function login() {
            var u = $.trim($("#t_u").val()), p = $.trim($("#t_p").val()), vc = $.trim($("#t_vc").val());
            if (u == "") {
                setMsg("请输入用户名");
                $("#t_u").focus();
                return false;
            }
            if (p == "") {
                setMsg("请输入密码");
                $("#t_w").focus();
                return false;
            }
            if (vc == "" || vc != getVC()) {
                setMsg("请输入正确的验证码");
                return;
            }

            //显示登录状态
            setMsg("正在登录中....");
            //防止重复登陆
            $("#lnkLogin").unbind().css("cursor", "default");

            blogin5({ u: u, p: p, vc: vc }
                , function(h) {//login success callback
                    setMsg("登录成功，正进入系统....");
                    setTimeout(function() {
                        var s = '<%=Request.QueryString["returnurl"] %>';
                        if (s == "") s = "/default.aspx";
                        window.location.href = s;
                    }, 300);
                }
                , function(m) {//login error callback
                    setMsg(m);
                    $("#lnkLogin").click(function() { login(); return false; }).css("cursor", "pointer");
                });
        }

        function AddFavorite(sURL, sTitle) {
            try {
                window.external.addFavorite(sURL, sTitle);
            }
            catch (e) {
                try {
                    window.sidebar.addPanel(sTitle, sURL, "");
                }
                catch (e) {
                    alert("加入收藏失败，请使用Ctrl+D进行添加");
                }
            }
        }

        function SetHome(obj, vrl) {
            try {
                obj.style.behavior = 'url(#default#homepage)'; obj.setHomePage(vrl);
            }
            catch (e) {
                if (window.netscape) {
                    try {
                        netscape.security.PrivilegeManager.enablePrivilege("UniversalXPConnect");
                    }
                    catch (e) {
                        alert("此操作被浏览器拒绝！\n请在浏览器地址栏输入“about:config”并回车\n然后将 [signed.applets.codebase_principal_support]的值设置为'true',双击即可。");
                    }
                    var prefs = Components.classes['@mozilla.org/preferences-service;1'].getService(Components.interfaces.nsIPrefBranch);
                    prefs.setCharPref('browser.startup.homepage', vrl);
                }
            }
        }

        $(document).ready(function() {
            setInterval(clock, 1000);
            $("#t_u").focus();
            $("#lnkLogin").click(function() { login(); return false; });
            $("#t_u,#t_p,#t_vc").keypress(function(e) { if (e.keyCode == 13) { login(); return false; } });

            $("#t_u").val("admin");
            $("#t_p").val("000000");
            $("#t_vc").val(getVC());
        });
    </script>

</body>
</html>
