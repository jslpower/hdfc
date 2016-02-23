﻿<%@ Page Title="餐饮-供应商管理" Language="C#" AutoEventWireup="true" CodeBehind="DinnerList.aspx.cs"
    Inherits="Web.ResourceManage.Dinner" MasterPageFile="~/MasterPage/Front.Master" %>

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
                        <b>当前您所在位置：</b> >> 供应商管理 >> 餐馆
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
                    <form id="frm" action="/ResourceManage/DinnerList.aspx" method="get">
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
                            单位名称：</label>
                        <input type="text" class="inputtext" id="txtUnionName" name="txtUnionName" value="<%=EyouSoft.Common.Utils.GetQueryStringValue("txtUnionName") %>" />
                        <label>
                            菜系：</label>
                        <select name="ddlCuisine" id="ddlCuisine" class="inputselect">
                            <%=BindCuisine(EyouSoft.Common.Utils.GetQueryStringValue("ddlCuisine")) %>
                        </select>
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
                    <td width="90" align="center">
                        <%if (add)
                          { %>
                        <a href="javascript:;" class="add">新 增</a><%} %>
                    </td>
                </tr>
            </table>
        </div>
        <div class="tablelist">
            <table width="100%" border="0" cellpadding="0" cellspacing="1" >
                <tr>
                    <th width="3%" height="30" align="center" bgcolor="#BDDCF4">
                       序号
                    </th>
                    <th width="12%" align="center" bgcolor="#BDDCF4">
                        所在地
                    </th>
                    <th width="12%" align="center" bgcolor="#bddcf4">
                        单位名称
                    </th>
                    <th width="12%" align="center" bgcolor="#bddcf4">
                        菜系
                    </th>
                    <th width="12%" align="center" bgcolor="#bddcf4">
                        联系人
                    </th>
                    <th width="12%" align="center" bgcolor="#bddcf4">
                        电话
                    </th>
                    <th width="12%" align="center" bgcolor="#bddcf4">
                        传真
                    </th>
                    <th width="12%" align="center" bgcolor="#bddcf4">
                        附件
                    </th>
                    <th width="12%" align="center" bgcolor="#bddcf4">
                        操作
                    </th>
                </tr>
                <asp:Repeater ID="rpDinner" runat="server">
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
                            </td>
                            <td align="center">
                                <%#Eval("Cuisine")%>
                            </td>
                            <td align="center">
                                <%#Eval("ContactInfo")!=null?((EyouSoft.Model.SourceStructure.MSupplierContact)Eval("ContactInfo")).ContactName:null %>
                            </td>
                            <td align="center">
                                <%#Eval("ContactInfo")!=null?((EyouSoft.Model.SourceStructure.MSupplierContact)Eval("ContactInfo")).ContactTel:null %>
                            </td>
                            <td align="center">
                                <%#Eval("ContactInfo")!=null?((EyouSoft.Model.SourceStructure.MSupplierContact)Eval("ContactInfo")).ContactFax:null %>
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
                    <td align="right">
                        <cc1:ExporPageInfoSelect ID="ExportPageInfo1" runat="server" />
                    </td>
                </tr>
            </table>
        </div>
    </div>
    <!-- InstanceEndEditable -->
    <div class="clearboth">
    </div>
    <!--mainbodyOut end-->

    <script type="text/javascript">
        $(function() {
            $("a.show").click(function() {
                var that = $(this);
                var url = "/ResourceManage/DinnerAdd.aspx?dotype=show&";
                var tid = that.parent().parent().attr("tid");
                Boxy.iframeDialog({
                    iframeUrl: url + "tid=" + tid,
                    title: "查看",
                    modal: true,
                    width: "980px",
                    height: "480px"
                });
                return false;
            });

            $("a.add").click(function() {
                var url = "/ResourceManage/DinnerAdd.aspx?dotype=add";
                Boxy.iframeDialog({
                    iframeUrl: url,
                    title: "新增",
                    modal: true,
                    width: "980px",
                    height: "480px"
                });
                return false;
            });
            
            $("a.modify").click(function() {
                var that = $(this);
                var url = "/ResourceManage/DinnerAdd.aspx?dotype=update&";
                var tid = that.parent().parent().attr("tid");
                Boxy.iframeDialog({
                    iframeUrl: url + "tid=" + tid,
                    title: "修改",
                    modal: true,
                    width: "980px",
                    height: "480px"
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
