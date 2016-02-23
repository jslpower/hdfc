<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="domain.aspx.cs" Inherits="Web.Webmaster._domain" MasterPageFile="~/Webmaster/mpage.Master"%>

<%@ MasterType VirtualPath="~/Webmaster/mpage.Master"%>

<asp:Content runat="server" ContentPlaceHolderID="Scripts" ID="ScriptsContent">
    <script type="text/javascript">
        //添加域名tr 
        //isAddbtn=true显示添加按钮 isAddBtn=false显示删除按钮
        //v:域名值
        //url:域名跳转到的URL
        function addSysDomain(isAddbtn, v, url) {
            var s = [];
            s.push("<tr>");
            s.push('<td><span class="required">*</span>系统域名：');
            s.push('<input type="text" name="txtDomain" class="input_text" style="width: 200px" value="' + v + '" />');
            s.push('&nbsp;&nbsp;跳转路径：<input type="text" name="txtDomainPath" class="input_text" style="width: 200px" value="' + url + '" >');
            isAddbtn ? s.push(' <a href="javascript:addSysDomain(false,\'\',\'\')">添加</a>') : s.push(' <a href="javascript:void(0)" onclick="deleteSysDomain(this)">删除</a>');
            s.push('<span class="note">域名格式：xz.gocn.cn</span>');
            s.push("</td>");
            s.push("</tr>");

            $("#trDomainAfter").before(s.join(""));
            piframeResize()
        }

        //删除域名tr
        function deleteSysDomain(obj) {
            $(obj).parent().parent().remove();
            piframeResize()
        }
        
        //初始化域名
        function initDomains() {
            if (domains.length < 1) {
                addSysDomain(true, '', '');
                return;
            }

            for (var i = 0; i < domains.length; i++) {
                var _url = domains[i].Url == null ? "" : domains[i].Url;
                if (i == 0) { addSysDomain(true, domains[i].Domain, _url); }
                else { addSysDomain(false, domains[i].Domain, _url); }
            }
        }

        function WebForm_OnSubmit_Validate() {
            var _obj = new Object();
            var _has = false;
            
            $("input[name='txtDomain']").each(function() {
                var _s = $.trim(this.value).toLowerCase().replace("http://", "");
                if (_s == '') return;
                if (_obj[_s] == 'undefined' || _obj[_s] == undefined) _obj[_s] = 1;
                else _obj[_s] = _obj[_s] + 1;
            });

            for (var item in _obj) {
                if (_obj[item] > 1) {
                    alert("域名：" + item + " 重复");
                    return false;
                }

                _has = true;
            }

            if (!_has) {
                alert("至少要填写一个域名信息");
                return false;
            }

            return true;
        }

        //document ready
        $(document).ready(function() {
            initDomains();
        });
    </script>
</asp:Content>

<asp:Content runat="server" ContentPlaceHolderID="PageTitle" ID="TitleContent">
    子系统域名管理-<asp:Literal runat="server" ID="ltrSysName"></asp:Literal>
</asp:Content>

<asp:Content runat="server" ContentPlaceHolderID="PageContent" ID="MainContent">
   <table cellpadding="2" cellspacing="1" style="font-size: 12px; width: 100%;">
        <!--域名区域-->
        <tr id="trDomainAfter">
            <td class="trspace">
                &nbsp;
            </td>
        </tr>
        <tr>
            <td>
                <asp:Button ID="btnSubmit" runat="server" Text="保存域名信息" OnClick="btnSubmit_Click" OnClientClick="return WebForm_OnSubmit_Validate()" />
            </td>
        </tr>
    </table>
</asp:Content>

<asp:Content runat="server" ContentPlaceHolderID="PageRemark" ID="RemarkContent">

</asp:Content>
