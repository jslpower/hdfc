<%@ Page Language="C#" MasterPageFile="~/MasterPage/Front.Master" AutoEventWireup="true"
    CodeBehind="DijiesheList.aspx.cs" Inherits="Web.CustomerManage.DijiesheList"
    Title="地接社" %>

<%@ Register Assembly="ControlLibrary" Namespace="Adpost.Common.ExporPage" TagPrefix="cc1" %>
<%@ Register Src="../UserControl/TableOption.ascx" TagName="TableOption" TagPrefix="uc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="mainbody">
        <div class="lineprotitlebox">
            <table width="100%" cellspacing="0" cellpadding="0" border="0">
                <tbody>
                    <tr>
                        <td width="15%" nowrap="nowrap">
                            <span class="lineprotitle">个人中心</span>
                        </td>
                        <td width="85%" nowrap="nowrap" align="right" style="padding: 0 10px 2px 0; color: #13509f;">
                            所在位置&gt;&gt; <a href="javascript:;">客户管理</a>&gt;&gt; 生日中心
                        </td>
                    </tr>
                    <tr>
                        <td height="2" bgcolor="#000000" colspan="2">
                        </td>
                    </tr>
                </tbody>
            </table>
        </div>
        <div style="height: 30px;" class="lineCategorybox">
            <uc1:TableOption ID="TableOption1" runat="server" Type="dijie" />
        </div>
        <div class="hr_10">
        </div>
        <div style="width: 99%;">
            <table width="100%" align="center" cellspacing="0" cellpadding="0" border="0">
                <tbody>
                    <tr>
                        <td width="10" valign="top">
                            <img src="/images/yuanleft.gif" alt="" />
                        </td>
                        <td>
                            <form method="get" id="form1">
                            <div class="searchbox">
                                姓名：
                                <input type="text" id="name" class="searchinput inputtext" name="name" value='<%=Request.QueryString["name"] %>' />
                                生日：
                                <input type="text" onfocus="WdatePicker({dateFmt:'MM-dd'})" id="birthdaystart" class="searchinput inputtext"
                                    name="birthdaystart" value='<%=Request.QueryString["birthdaystart"] %>' />-
                                <input type="text" onfocus="WdatePicker({dateFmt:'MM-dd'})" id="birthdayend" class="searchinput inputtext"
                                    name="birthdayend" value='<%=Request.QueryString["birthdayend"] %>' />
                                <input type="submit" value="" class="search-btn" />
                            </div>
                            </form>
                        </td>
                        <td width="10" valign="top">
                            <img src="/images/yuanright.gif" alt="" />
                        </td>
                    </tr>
                </tbody>
            </table>
        </div>
        <div class="tablelist">
            <table width="100%" cellspacing="1" cellpadding="0" border="0" id="liststyle">
                <tbody>
                    <tr>
                        <th width="36" height="30" align="center" bgcolor="#BDDCF4">
                            编号
                        </th>
                        <th align="center" bgcolor="#bddcf4">
                            单位名称
                        </th>
                        <th align="center" bgcolor="#bddcf4">
                            姓名
                        </th>
                        <th align="center" bgcolor="#bddcf4">
                            生日
                        </th>
                        <th align="center" bgcolor="#bddcf4">
                            手机号码
                        </th>
                    </tr>
                    <asp:Repeater runat="server" ID="RptList">
                        <ItemTemplate>
                            <tr class="<%# Container.ItemIndex % 2 == 0 ? "even" : "odd"%>">
                                <td height="30" align="center">
                                    <%#Container.ItemIndex+1 %>
                                </td>
                                <td align="center">
                                    <%#Eval("GroundName")%>
                                </td>
                                <td align="center">
                                    <a class="toolbar_add" href="javascript:;" data-id='<%#Eval("GroundId") %>'>
                                        <%#Eval("Name")%></a>
                                </td>
                                <td align="center">
                                    <%#EyouSoft.Common.Utils.GetDateTime(Convert.ToString(Eval("Birthday"))).ToString("yyyy-MM-dd")%>
                                </td>
                                <td align="center">
                                    <%#Eval("Mobile")%>
                                </td>
                            </tr>
                        </ItemTemplate>
                    </asp:Repeater>
                    <asp:Literal ID="lbemptymsg" runat="server"></asp:Literal>
                </tbody>
            </table>
            <table width="100%" cellspacing="0" cellpadding="0" border="0">
                <tbody>
                    <tr>
                        <td align="right" class="pageup">
                            <cc1:ExporPageInfoSelect ID="ExporPageInfoSelect1" runat="server" />
                        </td>
                    </tr>
                </tbody>
            </table>
        </div>
    </div>

    <script type="text/javascript">
        var DiJiePage={
            PageInit:function(){
                $("#liststyle").find("a[class='toolbar_add']").click(function(){
                    var contactId=$(this).attr("data-id");
                    DiJiePage.ShowBoxy({ iframeUrl: "/CustomerManage/Adddijie.aspx?dotype=add"+"&contactId="+contactId, title: "生日礼物", width: "520px", height: "310px" });
                })
                tableToolbar.init({
                    tableContainerSelector: "#liststyle"
                });
            },
            reload: function() {
                window.location.href = window.location.href;
                return false;
            },
            //显示弹窗
            ShowBoxy: function(data) {
                Boxy.iframeDialog({
                    iframeUrl: data.iframeUrl,
                    title: data.title,
                    modal: true,
                    width: data.width,
                    height: data.height,
                    afterHide: function() { YuangongPage.reload(); }
                });
            }
        }
        $(function(){
            DiJiePage.PageInit();
        })
    </script>

</asp:Content>
