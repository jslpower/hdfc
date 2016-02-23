<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="allprivs.aspx.cs" Inherits="Web.Webmaster.allprivs" MasterPageFile="~/Webmaster/mpage.Master" %>

<%@ MasterType VirtualPath="~/Webmaster/mpage.Master" %>
<asp:content runat="server" contentplaceholderid="Scripts" id="ScriptsContent">
    <style type="text/css">
    .trspace{height:10px;  font-size:0px;}
    .note {color:#999; margin-left:5px; }
    .required {color:#ff0000}
    .unrequired{color:#fff}
    ul {list-style:none; margin:0px; padding:0px;}
    ul li{list-style:none;}
    .pBig{font-weight:bold;line-height:30px;font-size:14px; clear:both; margin-top:10px; background:#eee}
    .pSmall{float:left;width:24%;}
    .pSmall li{line-height:22px}
    .pSmall li.pSmallTitle{font-weight:bold;line-height:24px;}
    .pSmallSpace{clear:both;width:100%; height:10px;}
    .pno{color:#ff0000; font-weight:normal;}
    .lh24 { line-height:24px;}

    .tblmenus{border-top:1px solid #ddd;border-left:1px solid #ddd;width: 100%;margin-bottom: 10px;}    
    .tblmenus .thead{text-align:left;background: #efefef; height:35px; font-size:14px;}
    .tblmenus td{border-right:1px solid #ddd;border-bottom:1px solid #ddd; height:35px;}
    .m1classname li { float:left;}
    .m1classname li span{background:url(/images/menuicon.gif) no-repeat -9999px; position:relative; left:0px; top:0px;width:19px; height:18px; display:inline-block; margin-right:2px}
    .m1classname li span.zutuan{ background-position:0 0;}
    .m1classname li span.diejietd{ background-position:0 -27px;}
    .m1classname li span.cjtd{ background-position:-2px -57px;}
    .m1classname li span.tongyefx{ background-position:-2px -80px;}
    .m1classname li span.dxyw{ background-position:0 -110px;}
    .m1classname li span.jidiaozx{ background-position:0 -135px;}
    .m1classname li span.daoyouzx{ background-position:0 -163px;}
    .m1classname li span.ziyuanyk{ background-position:0 -190px;}
    .m1classname li span.zilianggl{ background-position:0 -216px;}
    .m1classname li span.kehugl{ background-position:0 -243px;}
    .m1classname li span.ziyuangl{ background-position:0 -276px;}
    .m1classname li span.hetonggl{ background-position:0 -297px;}
    .m1classname li span.caiwugl{ background-position:0 -322px;}
    .m1classname li span.tongjifx{ background-position:0 -352px;}
    .m1classname li span.xingzzx{ background-position:0 -376px;}
    .m1classname li span.xitongsz{ background-position:0 -406px;}
    .m1classname li span.xsshoukuan{ background-position:0 -560px;}
	</style>
	
	<script type="text/javascript">
	    var allPrivs = [];
	    function setSecond() {
	        var firstValue = $("#txtThirdFirst").val();
	        var data = [];
	        for (var i = 0; i < allPrivs.length; i++) {
	            if (allPrivs[i].MenuId == firstValue) { data = allPrivs[i].Menu2s; break; }
	        }            
	        var spanThirdSecondHtml = [];
	        spanThirdSecondHtml.push('<select id="txtThirdSecond" name="txtThirdSecond"><option value="0">请选择二级栏目</option>');
	        for (var i = 0; i < data.length; i++) {
	            spanThirdSecondHtml.push('<option value="' + data[i].MenuId + '">' + data[i].Name + '</option>');
	        }
	        spanThirdSecondHtml.push('</select>');	        
	        $("#spanThirdSecond").html(spanThirdSecondHtml.join(''));
	    }

	    function setUpdateMenu2UrlSecond() {
	        var firstValue = $("#txtUpdateMenu2UrlFirst").val();
	        var data = [];
	        for (var i = 0; i < allPrivs.length; i++) {
	            if (allPrivs[i].MenuId == firstValue) { data = allPrivs[i].Menu2s; break; }
	        }
	        var spanThirdSecondHtml = [];
	        spanThirdSecondHtml.push('<select id="txtUpdateMenu2UrlSecond" name="txtUpdateMenu2UrlSecond" onchange="setUpdateMenu2UrlSecondUrl(this)"><option value="0" url="">请选择二级栏目</option>');
	        for (var i = 0; i < data.length; i++) {
	            spanThirdSecondHtml.push('<option value="' + data[i].MenuId + '" url="' + data[i].Url + '">' + data[i].Name + '</option>');
	        }
	        spanThirdSecondHtml.push('</select>');
	        $("#spanUpdateMenu2UrlSecond").html(spanThirdSecondHtml.join(''));

	        $("#txtUpdateSecondUrl").val('');
	    }

	    function setUpdateMenu2UrlSecondUrl(obj) {
	        $("#txtUpdateSecondUrl").val($(obj).find(":checked").attr("url"));
	    }
	
	    function init() {
	        if (allPrivs == null || allPrivs.length < 1) return;
	        var spanSecondFirstHtml = [];
	        var spanThirdFirstHtml = [];
	        var spanThirdSecondHtml = [];
	        var spanUpdateMenu2UrlFirstHtml = [];
	        var spanUpdateMenu2UrlSecondHtml = [];

	        spanSecondFirstHtml.push('<select id="txtSecondFirst" name="txtSecondFirst"><option value="0">请选择一级栏目</option>');
	        spanThirdFirstHtml.push('<select id="txtThirdFirst" name="txtThirdFirst" onchange="setSecond()"><option value="0">请选择一级栏目</option>');
	        spanThirdSecondHtml.push('<select id="txtThirdSecond" name="txtThirdSecond"><option value="0">请选择二级栏目</option></select>');

	        spanUpdateMenu2UrlFirstHtml.push('<select id="txtUpdateMenu2UrlFirst" name="txtUpdateMenu2UrlFirst" onchange="setUpdateMenu2UrlSecond()"><option value="0">请选择一级栏目</option>');
	        spanUpdateMenu2UrlSecondHtml.push('<select id="txtUpdateMenu2UrlSecond" name="txtUpdateMenu2UrlSecond" onchange="setUpdateMenu2UrlSecondUrl(this)"><option value="0" url="">请选择二级栏目</option></select>');

	        for (var i = 0; i < allPrivs.length; i++) {
	            spanSecondFirstHtml.push('<option value="' + allPrivs[i].MenuId + '">' + allPrivs[i].Name + '</option>');
	            spanThirdFirstHtml.push('<option value="' + allPrivs[i].MenuId + '">' + allPrivs[i].Name + '</option>')
	            spanUpdateMenu2UrlFirstHtml.push('<option value="' + allPrivs[i].MenuId + '">' + allPrivs[i].Name + '</option>');
	        }
	        
	        spanSecondFirstHtml.push('</select>');
	        spanThirdFirstHtml.push('</select>');
	        spanUpdateMenu2UrlFirstHtml.push('</select>');
	        spanUpdateMenu2UrlSecondHtml.push('</select>');

	        $("#spanSecondFirst").html(spanSecondFirstHtml.join(''));
	        $("#spanThirdFirst").html(spanThirdFirstHtml.join(''));
	        $("#spanThirdSecond").html(spanThirdSecondHtml.join(''));
	        $("#spanUpdateMenu2UrlFirst").html(spanUpdateMenu2UrlFirstHtml.join(''));
	        $("#spanUpdateMenu2UrlSecond").html(spanUpdateMenu2UrlSecondHtml.join(''));
	    }

	    function WebForm_OnSubmit_Validate1() {
	        if ($.trim($("#txtFirstName").val()).length < 1) { alert("一级栏目名称不能为空"); return false; }
	        var _mClassName = $("input[name = 'radClassName']:checked").val();
	        /*if (_mClassName == "" || _mClassName == "undefined" || _mClassName == undefined) { alert("请选择一级栏目小图标。"); return false; }*/
	        if (!confirm("你确定要添加一级栏目吗？")) return false;
	        
	        return true;
	    }
	    function WebForm_OnSubmit_Validate2() {
	        if ($("#txtSecondFirst").val() == "0") { alert("请选择一级栏目"); return false; }
	        if ($.trim($("#txtSecondName").val()).length < 1) { alert("二级栏目名称不能为空"); return false; }
	        if ($.trim($("#txtSecondUrl").val()).length < 1) { alert("二级栏目链接不能为空"); return false; }
	        if (!confirm("你确定要添加二级栏目吗？")) return false;
	        
	        return true;
	    }
	    function WebForm_OnSubmit_Validate3() {
	        if ($("#txtThirdFirst").val() == "0") { alert("请选择一级栏目"); return false; }
	        if ($("#txtThirdSecond").val() == "0") { alert("请选择二级栏目"); return false; }
	        if ($("#txtPrivsType").val() == "-1") { alert("请选择权限类别"); return false; }
	        if ($.trim($("#txtPrivsName").val()).length < 1) { alert("明细权限名称不能为空"); return false; }
	        if (!confirm("你确定要添加明细权限吗？")) return false;
	        
	        return true;
	    }

	    function WebForm_OnSubmit_Validate4() {
	        if ($("#txtUpdateMenu2UrlFirst").val() == "0") { alert("请选择一级栏目"); return false; }
	        if ($("#txtUpdateMenu2UrlSecond").val() == "0") { alert("请选择二级栏目"); return false; }
	        if ($.trim($("#txtUpdateSecondUrl").val()).length < 1) { alert("请输入链接"); return false; }
	        if (!confirm("你确定要修改二级栏目链接吗？")) return false;
	        
	        return true;
	    }

	    $(document).ready(function() {
	        init();
	        $("#<%=btnAddFirst.ClientID %>").bind("click", function() { return WebForm_OnSubmit_Validate1(); });
	        $("#<%=btnAddSecond.ClientID %>").bind("click", function() { return WebForm_OnSubmit_Validate2(); });
	        $("#<%=btnAddThird.ClientID %>").bind("click", function() { return WebForm_OnSubmit_Validate3(); });
	    });	    
    </script>
	
</asp:content>
<asp:content runat="server" contentplaceholderid="PageTitle" id="TitleContent">
    系统权限基础数据（注：请保持服务器端同步）
</asp:content>
<asp:content runat="server" contentplaceholderid="PageContent" id="MainContent">
    <table cellpadding="2" cellspacing="1" style="font-size: 12px; width: 100%;">
        <tr>
            <td>
                添加一级栏目：
                <input type="text" id="txtFirstName" name="txtFirstName" class="input_text" maxlength="50" style="width: 150px" />                 
            </td>
        </tr>
        <%--<tr>
            <td>
                <ul class="m1classname">
                    <li></li>
                    <li><input type="radio" name="radClassName" id="radClassName0" value="zutuan" /><label for="radClassName0"><span class="zutuan">&nbsp;</span></label></li>
                    <li><input type="radio" name="radClassName" id="radClassName1" value="diejietd" /><label for="radClassName1"><span class="diejietd">&nbsp;</span></label></li>
                    <li><input type="radio" name="radClassName" id="radClassName2" value="cjtd" /><label for="radClassName2"><span class="cjtd">&nbsp;</span></label></li>
                    <li><input type="radio" name="radClassName" id="radClassName3" value="tongyefx" /><label for="radClassName3"><span class="tongyefx">&nbsp;</span></label></li>
                    <li><input type="radio" name="radClassName" id="radClassName4" value="dxyw" /><label for="radClassName4"><span class="dxyw">&nbsp;</span></label></li>
                    <li><input type="radio" name="radClassName" id="radClassName5" value="jidiaozx" /><label for="radClassName5"><span class="jidiaozx">&nbsp;</span></label></li>
                    <li><input type="radio" name="radClassName" id="radClassName6" value="daoyouzx" /><label for="radClassName6"><span class="daoyouzx">&nbsp;</span></label></li>
                    <li><input type="radio" name="radClassName" id="radClassName7" value="ziyuanyk" /><label for="radClassName7"><span class="ziyuanyk">&nbsp;</span></label></li>
                    <li><input type="radio" name="radClassName" id="radClassName8" value="zilianggl" /><label for="radClassName8"><span class="zilianggl">&nbsp;</span></label></li>
                    <li><input type="radio" name="radClassName" id="radClassName9" value="hetonggl" /><label for="radClassName9"><span class="hetonggl">&nbsp;</span></label></li>
                    <li><input type="radio" name="radClassName" id="radClassName10" value="caiwugl" /><label for="radClassName10"><span class="caiwugl">&nbsp;</span></label></li>
                    <li><input type="radio" name="radClassName" id="radClassName11" value="tongjifx" /><label for="radClassName11"><span class="tongjifx">&nbsp;</span></label></li>
                    <li><input type="radio" name="radClassName" id="radClassName12" value="xingzzx" /><label for="radClassName12"><span class="xingzzx">&nbsp;</span></label></li>
                    <li><input type="radio" name="radClassName" id="radClassName13" value="xitongsz" /><label for="radClassName13"><span class="xitongsz">&nbsp;</span></label></li>
                    <li><input type="radio" name="radClassName" id="radClassName14" value="ziyuangl" /><label for="radClassName14"><span class="ziyuangl">&nbsp;</span></label></li>
                    <li><input type="radio" name="radClassName" id="radClassName15" value="kehugl" /><label for="radClassName15"><span class="kehugl">&nbsp;</span></label></li>
                    <li><input type="radio" name="radClassName" id="radClassName16" value="xsshoukuan" /><label for="radClassName16"><span class="xsshoukuan">&nbsp;</span></label></li>
                </ul>
            </td>
        </tr>--%>
        <tr>
            <td style="height:30px;">
                <asp:Button ID="btnAddFirst" runat="server" Text="添加一级栏目" OnClick="btnAddFirst_Click" /><br /><br />
            </td>
        </tr>
        <tr>
            <td>
                添加二级栏目：<span id="spanSecondFirst"></span>
                名称：<input type="text" id="txtSecondName" name="txtSecondName" class="input_text" maxlength="50" style="width: 150px" /> 
                链接：<input type="text" id="txtSecondUrl" name="txtSecondUrl" class="input_text" maxlength="255" style="width: 250px" />
            </td>
        </tr>
        <tr>
            <td style="height:30px;">
                <asp:Button ID="btnAddSecond" runat="server" Text="添加二级栏目" OnClick="btnAddSecond_Click" /><br /><br />
            </td>
        </tr>
        <tr>
            <td>
                添加明细权限：
                <span id="spanThirdFirst"></span>
                <span id="spanThirdSecond"></span>
                <span>
                    <select name="txtPrivsType" id="txtPrivsType">
                        <option value="-1">请选择权限类别</option>
                        <option value="<%=(int)EyouSoft.Model.EnumType.SysStructure.PrivsType.其它 %>"><%=EyouSoft.Model.EnumType.SysStructure.PrivsType.其它%></option>
                        <option value="<%=(int)EyouSoft.Model.EnumType.SysStructure.PrivsType.栏目 %>"><%=EyouSoft.Model.EnumType.SysStructure.PrivsType.栏目%></option>
                    </select>
                </span>
                名称：<input type="text" id="txtPrivsName" name="txtPrivsName" class="input_text" maxlength="50" style="width:150px" />
            </td>
        </tr>
        <tr>
            <td style="height:30px;">
                <asp:Button ID="btnAddThird" runat="server" Text="添加明细权限" OnClick="btnAddThird_Click" /><br /><br />
            </td>
        </tr>
        <tr id="trPermissions">
            <td>
                <asp:Literal runat="server" ID="ltrPrivs"></asp:Literal>                
            </td>
        </tr>
    </table>
</asp:content>
<asp:content runat="server" contentplaceholderid="PageRemark" id="RemarkContent">
    <ul class="decimal">
        <li>系统发布后，添加一级栏目、二级栏目、明细权限、修改二级栏目链接等操作时要求服务器端保持同步。以确保栏目号、权限号对应。</li>
        <li>权限类别-栏目：属于二级栏目的栏目权限时标记。无栏目权限的用户看不到该二级栏目。</li>
    </ul>
</asp:content>
