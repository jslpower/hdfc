<%@ Page Title="导游-供应商管理" Language="C#" AutoEventWireup="true" CodeBehind="GuideList.aspx.cs"
    Inherits="Web.ResourceManage.GuideList" MasterPageFile="~/MasterPage/Front.Master" %>

<%@ Register Assembly="ControlLibrary" Namespace="Adpost.Common.ExporPage" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="mainbody">
        <div class="lineprotitlebox">
            <table width="100%" border="0" cellspacing="0" cellpadding="0">
                <tr>
                    <td width="15%" nowrap="nowrap">
                        <span class="lineprotitle">供应商管理</span>
                    </td>
                    <td width="85%" nowrap="nowrap" align="right" style="padding: 0 10px 2px 0; color: #13509f;">
                        <b>当前您所在位置：</b> >> 供应商管理 >> 导游
                    </td>
                </tr>
                <tr>
                    <td colspan="2" height="2" bgcolor="#000000">
                    </td>
                </tr>
            </table>
        </div>
        <div class="hr_10">
        </div>
        <table width="99%" border="0" align="center" cellpadding="0" cellspacing="0">
            <tr>
                <td width="10" valign="top">
                    <img src="../images/yuanleft.gif" />
                </td>
                <td>
                    <form id="frm" action="/ResourceManage/GuideList.aspx" method="get">
                    <div class="searchbox">
                    <label>旅行社名称：</label>
                  <input name="txtGysName" type="text" class="inputtext" value="<%=EyouSoft.Common.Utils.GetQueryStringValue("txtGysName") %>" />
                        <label>
                            导游姓名：</label>
                        <input name="txtGuideName" type="text" class="inputtext" value="<%=EyouSoft.Common.Utils.GetQueryStringValue("txtGuideName") %>" />
                        <label>
                            <a href="javascript:;" id="btnSearch">
                                <img src="../images/searchbtn.gif" style="vertical-align: top;" alt="" /></a></label>
                    </div>
                    </form>
                </td>
                <td width="10" valign="top">
                    <img src="../images/yuanright.gif" />
                </td>
            </tr>
        </table>
        <div class="btnbox">
            <table border="0" align="left" cellpadding="0" cellspacing="0">
                <tr>
                    <td width="90">
                        <% if (add)
                           { %>
                        <a href="javascript:;" class="add">新 增</a>
                        <% }%>
                    </td>
                </tr>
            </table>
        </div>
        <div class="tablelist">
            <table width="100%" border="0" cellpadding="0" cellspacing="1">
                <tr>
                    <th width="60" height="30" align="center" bgcolor="#BDDCF4">
                        序号
                    </th>
                    <th align="center" bgcolor="#BDDCF4">
                        旅行社名称
                    </th>
                    <th align="center" bgcolor="#bddcf4">
                        导游姓名
                    </th>
                    <th align="center" bgcolor="#bddcf4">
                        手机
                    </th>
                    <th align="center" bgcolor="#bddcf4">
                        照片
                    </th>
                    <th width="120" align="center" bgcolor="#bddcf4">
                        导游反馈
                    </th>
                    <th align="center" bgcolor="#bddcf4">
                        操作
                    </th>
                </tr>
                <asp:Repeater ID="rpGuide" runat="server">
                    <ItemTemplate>
                        <tr tid="<%# Eval("Id") %>" bgcolor="<%# Container.ItemIndex%2==0?"#e3f1fc":"#BDDCF4" %>">
                            <td height="30" align="center">
                                <%# Container.ItemIndex + 1 + (this.pageIndex - 1) * this.pageSize%>
                            </td>
                            <td align="center">
                               <%-- <%# Eval("ProvinceName") %>&nbsp;
                                <%# Eval("CityName") %>--%>
                                <%#Eval("GysName") %>
                            </td>
                            <td align="center">
                                <%#Eval("GuideName")%>
                            </td>
                            <td align="center">
                                <%#Eval("Phone")%>
                            </td>
                            <td align="center">
                               <a href="javascript:void(0);" class="fujianlist" data-info="<%#EyouSoft.Common.UtilsCommons.GetFileInfo(Eval("FileList") as System.Collections.Generic.IList<EyouSoft.Model.SourceStructure.MFileInfo>)%>">
                                    <img src="/images/fujian_bg.gif" /></a>
                            </td>
                            <td align="center">
                                <a href="javascript:;" class="link1">共计
                                    <%#Eval("FanKuiNum")%>次</a>
                            </td>

                            <td align="center">
                                <a href="javascript:void(0);" class="show">查看 </a>
                                <% if (update)
                                   { %><a href="javascript:void(0);" class="modify">修改</a>
                                <%} if (del)
                                   { %><a href="javascript:void(0);" class="del">删除</a><%} %>
                            </td>
                        </tr>
                    </ItemTemplate>
                </asp:Repeater>
                <asp:Literal ID="Literal1" runat="server"></asp:Literal>
            </table>
            <table width="100%" border="0" cellspacing="0" cellpadding="0">
                <tr>
                    <td height="30" align="right" class="pageup" colspan="13">
                        <cc1:ExporPageInfoSelect ID="ExporPageInfoSelect1" runat="server" />
                    </td>
                </tr>
            </table>
        </div>
    </div>
    <!-- InstanceEndEditable -->
    <div class="clearboth">
    </div>

    <script type="text/javascript">
        $(function() {
            //反馈
            $("a.link1").click(function() {
                var that = $(this);
                var url = "/ResourceManage/GuideReturn.aspx?type=return&";
                var tid = that.parent().parent().attr("tid");
                Boxy.iframeDialog({
                    iframeUrl: url + "tid=" + tid,
                    title: "反馈",
                    modal: true,
                    width: "840px",
                    height: "280px",
                    afterHide: function() { reload(); }
                });
                return false;
            });

            $("a.show").click(function() {
                var that = $(this);
                var url = "/ResourceManage/GuideAdd.aspx?dotype=show&";
                var tid = that.parent().parent().attr("tid");
                Boxy.iframeDialog({
                    iframeUrl: url + "tid=" + tid,
                    title: "查看",
                    modal: true,
                    width: "840px",
                    height: "340px"
                });
                return false;


            });


            $("a.add").click(function() {
                var url = "/ResourceManage/GuideAdd.aspx?dotype=add";
                Boxy.iframeDialog({
                    iframeUrl: url,
                    title: "新增",
                    modal: true,
                    width: "840px",
                    height: "340px"
                });
                return false;
            });

            $("a.modify").click(function() {
                var that = $(this);
                var url = "/ResourceManage/GuideAdd.aspx?dotype=update&";
                var tid = that.parent().parent().attr("tid");
                Boxy.iframeDialog({
                    iframeUrl: url + "tid=" + tid,
                    title: "修改",
                    modal: true,
                    width: "840px",
                    height: "340px"
                });
                return false;
            });

            $("a.del").click(function() {
                if (confirm("确认删除所选项?")) {
                    var that = $(this);
                    var tid = that.parent().parent().attr("tid");
                    $.newAjax({
                        type: "POST",
                        data: { "tid": tid },
                        url: "/ResourceManage/GuideList.aspx?act=areadel",
                        dataType: 'json',
                        success: function(data) {
                            if (data.result) {
                                if (data.result == 1) {
                                    alert(data.msg);
                                    window.location.reload();
                                }
                                else {
                                    alert(data.msg);
                                }
                            }
                        },
                        error: function() {
                            alert("服务器繁忙!");
                        }
                    });
                }
                return false;
            });

            $("#btnSearch").click(function() {
                $("#frm").submit();
            });


        });

        function reload() {
            window.location.href = window.location.href;
            return false;
        }
        
        $('.fujianlist').each(function() {
            var data = $(this).attr("data-info");
            if ($(data).length == 0) {
                $(this).remove();
            }
        });

        $('.fujianlist').bt({
            contentSelector: function() {
                var data = $(this).attr("data-info");
                if (data) {
                    return data;
                }
                return "";
            },
            positions: ['bottom'],
            fill: '#effaff',
            strokeStyle: '#2a9cd4',
            noShadowOpts: { strokeStyle: "#2a9cd4" },
            spikeLength: 5,
            spikeGirth: 15,
            width: 200,
            overlap: 0,
            centerPointY: 4,
            cornerRadius: 4,
            shadow: true,
            shadowColor: 'rgba(0,0,0,.5)',
            cssStyles: { color: '#1351a0', 'line-height': '200%' }
        });
    </script>

</asp:Content>
