<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="smscashier.aspx.cs" Inherits="Web.Webmaster.smscashier"
    MasterPageFile="~/Webmaster/mpage.Master" %>

<%@ MasterType VirtualPath="~/Webmaster/mpage.Master" %>
<asp:Content runat="server" ContentPlaceHolderID="Scripts" ID="ScriptsContent">
    <style type="text/css">
        .trspace
        {
            height: 10px;
            font-size: 0px;
        }
        .tblLB
        {
            border-top: 1px solid #999;
            border-left: 1px solid #999;
            width: 100%;
            margin-bottom: 10px;
        }
        .tblLB thead
        {
            text-align: center;
            background: #efefef;
            height: 34px;
        }
        .tblLB tfoot
        {
            text-align: center;
            height: 26px;
            background: #fff;
        }
        .tblLB td
        {
            border-right: 1px solid #999;
            border-bottom: 1px solid #999;
            line-height: 32px;
            text-align: center;
        }
        .white_content td
        {
            line-height: normal;
            border: 0px;
            line-height: 25px;
        }
        .white_content
        {
            display: none;
            position: absolute;
            top: 10%;
            left: 48%;
            width: 300px;
            height: 150px;
            padding: 2px;
            border: 4px solid orange;
            background-color: white;
            z-index: 1002;
            overflow: auto;
        }
    </style>

    <script type="text/javascript" src="/js/ajaxpagecontrols.js"></script>

    <script type="text/javascript">
        var pConfig = {
            pageSize: 15,
            pageIndex: 1,
            recordCount: 0,
            showPrev: true,
            showNext: true,
            showDisplayText: false,
            cssClassName: 'page_change'
        }

        $(document).ready(function() {
            if (pConfig.recordCount > 0) {
                AjaxPageControls.replace("page_change", pConfig);
            }
        });

        //查询按钮事件
        function search() {
            var data = { cn: $.trim($("#txtCompanyName").val()), status: $("#txtStatus").val() };
            window.location.href = "smscashier.aspx?s=&" + $.param(data);
        }

        //审核按钮(列表)事件
        //pid:充值编号
        //obj:link dom
        function openCashierBox(pid, obj) {
            $('div.white_content').hide();
            $("#div_" + pid).css({ top: $(obj).position().top - 5, left: $(obj).position().left - 320 });
            $("#div_" + pid).show();
        }

        //审核按钮(浮动DIV)事件
        //pid:充值编号
        //status:true:通过 false:不通过
        function cashier(pid, status) {
            $.ajax({
                url: 'smscashier.aspx',
                data: { cashier: 1, pid: pid, status: status, amount: $("#txtUsableAmount_" + pid).val(), wn: $("#txtWebmasterName_" + pid).val() },
                cache: false,
                success: function(v) {
                    if (v == 1) {
                        alert("操作成功！");
                    } else {
                        alert("操作失败！");
                    }
                    window.location.href = window.location.href;
                }
            });
        }
    </script>

</asp:Content>
<asp:Content runat="server" ContentPlaceHolderID="PageTitle" ID="TitleContent">
    短信充值审核
</asp:Content>
<asp:Content runat="server" ContentPlaceHolderID="PageContent" ID="MainContent">
    <table cellpadding="2" cellspacing="1" style="font-size: 12px; width: 100%;">
        <tr>
            <td>
                充值状态：<select id="txtStatus" name="txtStatus">
                    <option value="-1" selected="selected">所有</option>
                    <option value="0">未审核</option>
                    <option value="1">审核通过</option>
                    <option value="2">审核未通过</option>
                </select>
                公司名称：<input type="text" id="txtCompanyName" name="txtCompanyName" class="input_text"
                    maxlength="72" style="width: 150px" />
                <input type="button" value="查询" onclick="search()" />
            </td>
        </tr>
    </table>
    <asp:Repeater runat="server" ID="rptRecharges" OnItemDataBound="rptRecharges_ItemDataBound">
        <HeaderTemplate>
            <table class="tblLB" cellpadding="0" cellspacing="0">
                <thead>
                    <td>
                        单位名称
                    </td>
                    <td>
                        充值人
                    </td>
                    <td>
                        电话
                    </td>
                    <td>
                        手机
                    </td>
                    <td>
                        <sapn style="color: red">充值金额</span>/<sapn style="color:green">
                    可用金额</sapn>
                    </td>
                    <td>
                        充值时间
                    </td>
                    <td style="">
                        操作
                    </td>
                </thead>
        </HeaderTemplate>
        <ItemTemplate>
            <tr onmouseover="changeTrBgColor(this,'#c8dae6')" onmouseout="changeTrBgColor(this,'#ffffff')">
                <td>
                    <%#Eval("CompanyName") %>
                </td>
                <td>
                    <%#Eval("OperatorName")%>
                </td>
                <td>
                    <%#Eval("OperatorTel")%>
                </td>
                <td>
                    <%#Eval("OperatorMobile")%>
                </td>
                <td>
                    <sapn style="color: red"><%#string.Format("{0:C2}",Eval("PayMoney"))%></span>/<span style="color:green"><%#string.Format("{0:C2}",Eval("UseMoney"))%></span>
                </td>
                <td>
                    <%#Eval("PayTime")%>
                </td>
                <td>
                    <asp:Literal runat="server" ID="ltrStatus"></asp:Literal>
                    <asp:PlaceHolder runat="server" ID="phCashierBox">
                        <div style='text-align: right; z-index: 10000; display: none;' class='white_content'
                            id='div_<%#Eval("Id") %>'>
                            <table cellspacing="0" cellpadding="0" border="0" width="100%">
                                <tbody>
                                    <tr>
                                        <td align="left">
                                            &nbsp;充值金额：<input value='<%#Eval("PayMoney") %>' type='text' id='txtPayAmount' disabled='disabled'
                                                class="input_text" style="width: 180px">
                                        </td>
                                    </tr>
                                    <tr>
                                        <td align="left">
                                            &nbsp;可用金额：<input type='text' value='<%#Eval("PayMoney") %>' id='txtUsableAmount_<%#Eval("Id") %>'
                                                class="input_text" style="width: 180px">
                                        </td>
                                    </tr>
                                    <tr>
                                        <td align="left">
                                            &nbsp;审 核 人：<input value='<%=WebmasterRealname %>' type='text' id="txtWebmasterName_<%#Eval("Id") %>"
                                                class="input_text" style="width: 180px">
                                        </td>
                                    </tr>
                                    <tr>
                                        <td align="left">
                                            &nbsp;审核时间：<input value='<%=DateTime.Now %>' type='text' disabled='disabled' class="input_text"
                                                style="width: 180px">
                                        </td>
                                    </tr>
                                    <tr class="trspace">
                                        <td>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td align="center">
                                            <input value='审核通过' type='button' onclick="cashier('<%#Eval("Id") %>',true)">
                                            &nbsp;&nbsp;<input value='审核不通过' type='button' onclick="cashier('<%#Eval("Id") %>',false)">
                                            &nbsp;&nbsp;<input value="关闭" type="button" onclick=" $('#div_<%#Eval("Id") %>').hide(); return false;" />
                                        </td>
                                    </tr>
                            </table>
                            <iframe scrolling="no" frameborder="0" style="position: absolute; visibility: inherit;
                                top: 0px; left: 0px; width: 100%; height: 100%; z-index: -1;" marginwidth="0"
                                marginheight="0"></iframe>
                        </div>
                    </asp:PlaceHolder>
                </td>
            </tr>
        </ItemTemplate>
        <FooterTemplate>
            <tfoot>
                <td colspan="7">
                    <div id="page_change" style="width: 100%; text-align: center; margin: 0px auto 0px;
                        margin-top: 2px; margin-bottom: 2px; clear: both">
                </td>
            </tfoot>
            </table>
        </FooterTemplate>
    </asp:Repeater>
    <asp:PlaceHolder runat="server" ID="phNotFound" Visible="false">
        <div style="line-height: 60px;">
            未找到任何充值信息.
        </div>
    </asp:PlaceHolder>
</asp:Content>
<asp:Content runat="server" ContentPlaceHolderID="PageRemark" ID="RemarkContent">
</asp:Content>
