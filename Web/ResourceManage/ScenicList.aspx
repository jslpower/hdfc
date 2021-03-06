﻿<%@ Page Title="景点-供应商管理" Language="C#" MasterPageFile="~/MasterPage/Front.Master"
    AutoEventWireup="true" CodeBehind="ScenicList.aspx.cs" Inherits="Web.ResourceManage.ScenicList" %>

<%@ Register Assembly="ControlLibrary" Namespace="Adpost.Common.ExporPage" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="mainbody">
        <!-- InstanceBeginEditable name="EditRegion3" -->
        <div class="mainbody">
            <div class="lineprotitlebox">
                <table width="100%" border="0" cellspacing="0" cellpadding="0">
                    <tr>
                        <td width="15%" nowrap="nowrap">
                            <span class="lineprotitle">资源管理</span>
                        </td>
                        <td width="85%" nowrap="nowrap" align="right" style="padding: 0 10px 2px 0; color: #13509f;">
                            所在位置 &gt;&gt; 供应商管理 &gt;&gt; 景点
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
                        <img src="/images/yuanleft.gif" alt="" />
                    </td>
                    <td>
                        <form action="ScenicList.aspx" method="get" id="frm">
                        <div class="searchbox">
                            <label>
                                省份：</label>
                            <select name="txtProvince" id="txtProvince" class="inputselect">
                            </select>
                            <label>
                                城市：</label>
                            <select name="txtCity" id="txtCity" class="inputselect">
                            </select>
                            <label>
                                景点名称：</label>
                            <input type="text" class="inputtext" id="sight_name" name="sight_name" value="<%=EyouSoft.Common.Utils.GetQueryStringValue("sight_name") %>" />
                            <label>
                                星级：</label>
                            <select id="ddlscStar" name="ddlscStar" class="inputselect">
                                <%=getStarScenic(EyouSoft.Common.Utils.GetQueryStringValue("ddlscStar"))%>
                            </select>
                            <a href="javascript:void(0);" id="btnSearch">
                                <img src="/images/searchbtn.gif" style="vertical-align: top;" /></a>
                        </div>
                        </form>
                    </td>
                    <td width="10" valign="top">
                        <img src="/images/yuanright.gif" alt="" />
                    </td>
                </tr>
            </table>
            <div class="btnbox">
                <table border="0" align="left" cellpadding="0" cellspacing="0" style="width: 360px">
                    <tr>
                        <td width="90">
                            <%if (add)
                              { %>
                            <a href="javascript:;" class="add">新 增</a><%} %>
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
                            所在地
                        </th>
                        <th align="center" bgcolor="#bddcf4">
                            景点名称
                        </th>
                        <th align="center" bgcolor="#bddcf4">
                            等级
                        </th>
                        <th align="center" bgcolor="#bddcf4">
                            旺季门市
                        </th>
                        <th align="center" bgcolor="#bddcf4">
                            旺季折扣
                        </th>
                        <th align="center" bgcolor="#bddcf4">
                            淡季门市
                        </th>
                        <th align="center" bgcolor="#bddcf4">
                           淡季折扣
                        </th>
                        <th align="center" bgcolor="#bddcf4">
                            联系人
                        </th>
                        <th align="center" bgcolor="#bddcf4">
                            电话
                        </th>
                        <th align="center" bgcolor="#bddcf4">
                            附件
                        </th>
                        <th width="120" align="center" bgcolor="#bddcf4">
                            操作
                        </th>
                    </tr>
                    <asp:Repeater runat="server" ID="rptList">
                        <ItemTemplate>
                            <tr tid="<%# Eval("Id") %>" class="<%#Container.ItemIndex%2==0?"even":"odd" %>">
                                <td height="30" align="center">
                                    <%# Container.ItemIndex + 1 + (this.pageIndex - 1) * this.pageSize%>
                                </td>
                                <td align="center">
                                    <%#Eval("ProvinceName")%>&nbsp;<%#Eval("CityName")%>
                                </td>
                                <td align="center">
                                    <%#Eval("UnitName")%>
                                    <%#Convert.ToBoolean(Eval("IsNew"))?"<img src='/images/new.gif' />":null %>
                                </td>
                                <td align="center">
                                    <%#Eval("SpotStar")%>
                                </td>
                                <td align="center">
                                    <%#this.ToMoneyString(Eval("StorePrice"))%>
                                </td>
                                <td align="center">
                                    <%#this.ToMoneyString(Eval("WJPrice"))%>
                                </td>
                                <td align="center">
                                    <%#this.ToMoneyString(Eval("DJPrice"))%>
                                </td>
                                <td align="center">
                                    <%#this.ToMoneyString(Eval("ZKPrice"))%>
                                </td>
                                <td align="center">
                                    <%#Eval("ContactInfo")!=null?((EyouSoft.Model.SourceStructure.MSupplierContact)Eval("ContactInfo")).ContactName:null %>
                                </td>
                                <td align="center">
                                    <%#Eval("ContactInfo")!=null?((EyouSoft.Model.SourceStructure.MSupplierContact)Eval("ContactInfo")).ContactTel:null %>
                                </td>
                                <td align="center">
                                    <a href="javascript:void(0);" class="fujianlist" data-info="<%#EyouSoft.Common.UtilsCommons.GetFileInfo(Eval("FileList") as System.Collections.Generic.IList<EyouSoft.Model.SourceStructure.MFileInfo>)%>">
                                        <img src="/images/fujian_bg.gif" /></a>
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
                            <cc1:ExporPageInfoSelect ID="ExportPageInfo1" runat="server" />
                        </td>
                    </tr>
                </table>
            </div>
        </div>
        <!-- InstanceEndEditable -->
    </div>

    <script type="text/javascript">
        $(function() {
            $("a.show").click(function() {
                var that = $(this);
                var url = "/ResourceManage/ScenicAdd.aspx?dotype=show&";
                var tid = that.parent().parent().attr("tid");
                Boxy.iframeDialog({
                    iframeUrl: url + "tid=" + tid,
                    title: "查看",
                    modal: true,
                    width: "980px",
                    height: "520px"
                });
                return false;
            });

            $("a.add").click(function() {
                var url = "/ResourceManage/ScenicAdd.aspx?dotype=add";
                Boxy.iframeDialog({
                    iframeUrl: url,
                    title: "新增",
                    modal: true,
                    width: "980px",
                    height: "520px"
                });
                return false;
            });

            $("a.modify").click(function() {
                var that = $(this);
                var url = "/ResourceManage/ScenicAdd.aspx?dotype=update&";
                var tid = that.parent().parent().attr("tid");
                Boxy.iframeDialog({
                    iframeUrl: url + "tid=" + tid,
                    title: "修改",
                    modal: true,
                    width: "980px",
                    height: "520px"
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
                        url: "/ResourceManage/DinnerList.aspx?act=areadel",
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

            pcToobar.init({
                pID: "#txtProvince",
                cID: "#txtCity",
                pSelect: '<%= EyouSoft.Common.Utils.GetInt(EyouSoft.Common.Utils.GetQueryStringValue("txtProvince"),0) %>',
                cSelect: '<%= EyouSoft.Common.Utils.GetInt(EyouSoft.Common.Utils.GetQueryStringValue("txtCity"),0) %>',
                comID: '<%= this.SiteUserInfo.CompanyId %>',
                isCy: "0"
            });

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
        });
    </script>

</asp:Content>
