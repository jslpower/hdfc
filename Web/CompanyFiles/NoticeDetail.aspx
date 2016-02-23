<%@ Page Title="公告通知查看" Language="C#" AutoEventWireup="true" MasterPageFile="~/MasterPage/Front.Master"
    CodeBehind="NoticeDetail.aspx.cs" Inherits="Web.CompanyFiles.NoticeDetail" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="mainbody">
        <div class="lineprotitlebox">
            <table width="100%" cellspacing="0" cellpadding="0" border="0">
                <tbody>
                    <tr>
                        <td width="15%" nowrap="nowrap">
                            <span class="lineprotitle">公告通知</span>
                        </td>
                        <td width="85%" nowrap="nowrap" align="right" style="padding: 0 10px 2px 0; color: #13509f;">
                            <b>当前您所在位置</b>&gt;&gt; 公司文件 &gt;&gt;公告通知
                        </td>
                    </tr>
                    <tr>
                        <td height="2" bgcolor="#000000" colspan="2">
                        </td>
                    </tr>
                </tbody>
            </table>
        </div>
        <div style="height: 50px;" class="lineCategorybox">
        </div>
        <div class="tablelist">
            <form runat="server">
            <table width="880" cellspacing="1" cellpadding="0" border="0" bgcolor="#BDDCF4" align="center">
                <tbody>
                    <tr>
                        <th bgcolor="#BDDCF4" align="center" colspan="3">
                            公告通知查看
                        </th>
                    </tr>
                    <tr>
                        <td width="16%" height="35" bgcolor="#e3f1fc" align="right">
                            <strong>标题：</strong>
                        </td>
                        <td height="35" bgcolor="#FAFDFF" align="left" class="pandl3" colspan="2">
                            <label>
                                <asp:Literal runat="server" ID="ltTitle"></asp:Literal>
                            </label>
                        </td>
                    </tr>
                    <tr>
                        <td height="35" bgcolor="#BDDCF4" align="right">
                            <strong>内容：</strong>
                        </td>
                        <td height="100" bgcolor="#FAFDFF" align="left" class="pandl3" colspan="2" style="line-height: normal">
                            <asp:Literal runat="server" ID="ltContent"></asp:Literal>
                        </td>
                    </tr>
                    <tr>
                        <td height="35" bgcolor="#e3f1fc" align="right">
                            <strong>附件：</strong>
                        </td>
                        <td height="35" bgcolor="#FAFDFF" align="left" class="pandl3" colspan="2">
                            <asp:HyperLink Target="_blank" runat="server" ID="hlFile" Visible="false">点击下载</asp:HyperLink>
                        </td>
                    </tr>
                    <tr>
                        <td height="35" bgcolor="#BDDCF4" align="right">
                            <strong>发布人：</strong>
                        </td>
                        <td height="35" bgcolor="#FAFDFF" align="left" class="pandl3" colspan="2">
                            <label>
                                <asp:Literal ID="ltOperatorName" runat="server"></asp:Literal>
                            </label>
                        </td>
                    </tr>
                    <tr>
                        <td height="35" bgcolor="#e3f1fc" align="right">
                            <strong>发布时间：</strong>
                        </td>
                        <td height="35" bgcolor="#FAFDFF" align="left" class="pandl3" colspan="2">
                            <label>
                                <asp:Literal runat="server" ID="ltCreateTime"></asp:Literal>
                            </label>
                        </td>
                    </tr>
                    <tr>
                        <td height="30" align="center" colspan="3">
                            <table cellspacing="0" cellpadding="0" border="0" align="center">
                                <tbody>
                                    <tr>
                                        <td width="86" height="40" align="center" class="tjbtn02">
                                            <a href="<%= ReturnUrl %>">返回</a>
                                        </td>
                                    </tr>
                                </tbody>
                            </table>
                        </td>
                    </tr>
                </tbody>
            </table>
            </form>
        </div>
    </div>
</asp:Content>
