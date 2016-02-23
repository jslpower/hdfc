<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="systems.aspx.cs" Inherits="Web.Webmaster._systems" MasterPageFile="~/Webmaster/mpage.Master"%>

<%@ MasterType VirtualPath="~/Webmaster/mpage.Master"%>

<asp:Content runat="server" ContentPlaceHolderID="Scripts" ID="ScriptsContent">
    <style type="text/css">
    .tblLB{border-top:1px solid #ddd;border-left:1px solid #ddd;width: 100%;margin-bottom: 10px;}
    .tblLB thead{text-align:center;background: #efefef; height:35px;}
    .tblLB tfoot{text-align: center; height: 35px; background: #efefef;}
    .tblLB td{border-right:1px solid #ddd;border-bottom:1px solid #ddd; height:35px; text-align:center;}
    </style>
    <script type="text/javascript" src="/js/ajaxpagecontrols.js"></script>
    <script type="text/javascript">
        //分页配置
        var pConfig = {
            pageSize: 15,
            pageIndex: 1,
            recordCount: 0,
            showPrev: true,
            showNext: true,
            showDisplayText: false,
            cssClassName: 'page_change'
        }
        //分页初始化
        $(document).ready(function() {
            if (pConfig.recordCount > 0) {
                AjaxPageControls.replace("page_change", pConfig);
            }
        });
    </script>
</asp:Content>

<asp:Content runat="server" ContentPlaceHolderID="PageTitle" ID="TitleContent">
    子系统管理
</asp:Content>

<asp:Content runat="server" ContentPlaceHolderID="PageContent" ID="MainContent">
    <asp:Repeater runat="server" ID="rptSys" OnItemDataBound="rptSys_ItemDataBound">
        <HeaderTemplate>
            <table class="tblLB" cellpadding="0" cellspacing="0">
                <thead>
                    <td>
                        序号
                    </td>
                    <td>
                        系统名称
                    </td>
                    <td>
                        联系人
                    </td>
                    <td>
                        联系电话
                    </td>
                    <td>
                        管理员账号
                    </td>
                    <td>
                        管理员密码
                    </td>
                    <td style="">
                        操作
                    </td>
                </thead>
        </HeaderTemplate>
        <ItemTemplate>
            <tr onmouseover="changeTrBgColor(this,'#eeeeee')" onmouseout="changeTrBgColor(this,'#ffffff')">
                <td>
                    <%#(PageIndex-1)*PageSize+Container.ItemIndex + 1%>
                </td>
                <td>
                    <%#Eval("SysName") %>
                </td>
                <td>
                    <%#Eval("FullName") %>
                </td>
                <td>
                    <%#Eval("Telephone") %>
                </td>
                <td>
                    <%#Eval("Username") %>
                </td>
                <td>
                    <%#((EyouSoft.Model.CompanyStructure.PassWord)Eval("Password")).NoEncryptPassword%>
                </td>                
                <td style="text-align:left;">&nbsp;&nbsp;
                    <a href="system.aspx?sysid=<%#Eval("SysId") %>&cid=<%#Eval("CompanyId") %>">查看</a>&nbsp;
                    <a href="domain.aspx?sysid=<%#Eval("SysId") %>&cid=<%#Eval("CompanyId") %>">域名</a>&nbsp;
                    <a href="privs.aspx?sysid=<%#Eval("SysId") %>&cid=<%#Eval("CompanyId") %>&uid=<%#Eval("UserId") %>">权限</a>&nbsp;
                    <%--<a href="setting.aspx?sysid=<%#Eval("SysId") %>&cid=<%#Eval("CompanyId") %>">配置</a>&nbsp;--%>
                    <asp:Literal ID="ltrHandles" runat="server"></asp:Literal>                    
                </td>
            </tr>
        </ItemTemplate>
        <FooterTemplate>
            </table>
            <div id="page_change" style="width: 100%; text-align: center; margin: 5px auto 0px; margin: 30px 0; clear: both"></div>
        </FooterTemplate>
    </asp:Repeater>
    
    <asp:PlaceHolder runat="server" ID="phNotFound" Visible="false">
        <div style="line-height:60px;">
            <span class="required">未能找到任何子系统信息。</span>
        </div>
    </asp:PlaceHolder>
</asp:Content>

<asp:Content runat="server" ContentPlaceHolderID="PageRemark" ID="RemarkContent">

</asp:Content>
