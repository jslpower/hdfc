<%@ Page Title="计调中心-团队报价资料库" Language="C#" AutoEventWireup="true" CodeBehind="TourData.aspx.cs"
    Inherits="Web.jidiaoCenter.TourData" MasterPageFile="~/MasterPage/Front.Master" %>

<%@ Register Assembly="ControlLibrary" Namespace="Adpost.Common.ExporPage" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="mainbody">
        <div class="lineprotitlebox">
            <table width="100%" border="0" cellspacing="0" cellpadding="0">
                <tr>
                    <td width="15%" nowrap="nowrap">
                        <span class="lineprotitle">计调中心</span>
                    </td>
                    <td width="85%" nowrap="nowrap" align="right" style="padding: 0 10px 2px 0; color: #13509f;">
                        <b>当前您所在位置：>> <a href="#">计调中心</a>>> 团队报价资料库
                    </td>
                </tr>
                <tr>
                    <td colspan="2" height="2" bgcolor="#000000">
                    </td>
                </tr>
            </table>
        </div>
        <div class="lineCategorybox">
            <ul class="xllist">
                <li>
                    <nobr><img src="/images/icon002.gif"> <a id="_routeall" data-id="" href="/jidiaoCenter/TourData.aspx" >所有线路</a></nobr>
                </li>
                <asp:Repeater ID="rpRoute" runat="server">
                    <ItemTemplate>
                        <li>
                            <nobr><img src="/images/icon002.gif"> <a data-id="<%#Eval("Id") %>" href="/jidiaoCenter/TourData.aspx?AreaId=<%#Eval("Id") %>"><%#Eval("AreaName")%></a></nobr>
                        </li>
                    </ItemTemplate>
                </asp:Repeater>
            </ul>
        </div>
        <div style="width: 99%;">
            <table width="100%" border="0" align="center" cellpadding="0" cellspacing="0">
                <tr>
                    <td width="10" valign="top">
                        <img src="../images/yuanleft.gif" />
                    </td>
                    <td>
                        <div class="searchbox">
                            <form action="/jidiaoCenter/TourData.aspx" id="frm" method="get">
                            <label>
                                名称：</label>
                            <input type="text" name="txtName" id="txtName" class="inputtext" value="<%=EyouSoft.Common.Utils.GetQueryStringValue("txtName")%>" />
                            <label>
                                审核状态：</label>
                            <select class="inputselect" name="ddlIsCheck">
                                <%=BindCheck(EyouSoft.Common.Utils.GetQueryStringValue("ddlIsCheck"))%>
                            </select>
                            <a href="javascript:;" id="btnSearch">
                                <img src="/images/searchbtn.gif" style="vertical-align: middle;" />
                            </a>
                            </form>
                        </div>
                    </td>
                    <td width="10" valign="top">
                        <img src="/images/yuanright.gif" />
                    </td>
                </tr>
            </table>
        </div>
        <div class="btnbox">
            <table border="0" align="left" cellpadding="0" cellspacing="0">
                <tr>
                    <td width="90" align="center">
                        <%if (add)
                          { %>
                        <a href="javascript:;" class="toolbar_add">新 增</a>
                        <%} %>
                    </td>
                    <td width="90" align="center">
                        <%if (update)
                          { %>
                        <a href="javascript:;" class="toolbar_update">修 改</a>
                        <%} %>
                    </td>
                    <td width="90" align="center">
                        <%if (del)
                          { %>
                        <a href="javascript:;" class="toolbar_delete">删 除</a>
                        <%} %>
                    </td>
                </tr>
            </table>
        </div>
        <div class="tablelist">
            <table id="liststyle" width="100%" border="0" cellpadding="0" cellspacing="1">
                <tr>
                    <th height="30" align="center">
                        <input type="checkbox" name="checkbox3" id="cbAll" />
                        全选
                    </th>
                    <th align="center" bgcolor="#BDDCF4">
                        线路名称
                    </th>
                    <th align="center" bgcolor="#bddcf4">
                        团型
                    </th>
                    <th align="center" bgcolor="#bddcf4">
                        进出港口
                    </th>
                    <th align="center" bgcolor="#bddcf4">
                        上传日期
                    </th>
                    <th align="center" bgcolor="#bddcf4">
                        上传人
                    </th>
                    <th align="center" bgcolor="#bddcf4">
                        操作
                    </th>
                </tr>
                <asp:Repeater ID="rpTourData" runat="server">
                    <ItemTemplate>
                        <tr tid="<%# Eval("TourDataId") %>" class="<%#Container.ItemIndex%2==0?"even":"odd" %>">
                            <td height="30" align="center">
                                <input type="checkbox" name="checkbox" id="cbItem" value="<%#Eval("TourDataId") %>">
                                <%# Container.ItemIndex + 1 + (this.pageIndex - 1) * this.pageSize%>
                            </td>
                            <td align="center">
                                <%#Eval("RouteName") %>
                            </td>
                            <td align="center">
                                <%#Eval("TourDataType")%>
                            </td>
                            <td align="center">
                                <%#Eval("TourPort")%>
                            </td>
                            <td align="center">
                                <%#ToDateTimeString( Eval("IssueTime"))%>
                            </td>
                            <td align="center">
                                <%#Eval("OperatorName")%>
                            </td>
                            <td align="center">
                                <%#Convert.ToBoolean(Eval("IsCheck"))?"已审核":"<a href='javascript:;' class='check'>审核</a>"%>
                            </td>
                        </tr>
                    </ItemTemplate>
                </asp:Repeater>
                <asp:Literal ID="Literal1" runat="server"></asp:Literal>
                <tr>
                    <td height="30" colspan="7" align="right" class="pageup">
                        <cc1:ExporPageInfoSelect ID="ExportPageInfo1" runat="server" />
                    </td>
                </tr>
            </table>
        </div>
    </div>

    <script type="text/javascript">
        $(function() {
            //线路选中问题
            $(".lineCategorybox").find("a").each(function() {
                var id = '<%=EyouSoft.Common.Utils.GetQueryStringValue("AreaId")%>';
                if (id == "") {
                    $("#_routeall").css("color", "red");
                    return;
                }
                else {
                    var dataid = $(this).attr("data-id");
                    if (dataid == id) {
                        $(this).css("color", "red");
                        $("#_routeall").removeClass("color");
                        return;
                    }
                }
            });

            $("#btnSearch").click(function() {
                $("#frm").submit();
            });

            TourData.BindBtn();

        });


        var TourData = {
            reload: function() {
                window.location.href = window.location.href;
                return false;
            },
            //ajax执行文件路径,默认为本页面
            ajaxurl: "/jidiaoCenter/TourData.aspx",
            //添加
            Add: function() {
                TourData.ShowBoxy({ iframeUrl: "/jidiaoCenter/TourDataEdit.aspx?dotype=add", title: "新增", width: "560px", height: "260px" });
            },
            //修改(弹窗)---objsArr选中的TR对象
            Update: function(ObjsArr) {
                TourData.ShowBoxy({ iframeUrl: "/jidiaoCenter/TourDataEdit.aspx?dotype=update&tourdataid=" + ObjsArr[0].find("input[type='checkbox']").val(), title: "修改", width: "560px", height: "260px" });
            },
            GetCheckBox: function(objArr) {
                //定义数组对象
                var list = new Array();
                //遍历按钮返回数组对象
                for (var i = 0; i < objArr.length; i++) {
                    //从数组对象中找到数据所在，并保存到数组对象中
                    if (objArr[i].find("input[type='checkbox']").val() != "on") {
                        list.push(objArr[i].find("input[type='checkbox']").val());
                    }
                }
                return list.join(',');
            },
            //删除(可多行)
            DelAll: function(objArr) {
                if (objArr.length == 1) {
                    //获取默认路径并重新拼接url（注：全局变量劲量不要改变，当常量就行）
                    TourData.ajaxurl = "/jidiaoCenter/TourData.aspx?dotype=delete&id=" + TourData.GetCheckBox(objArr);
                    //执行/ajax
                    TourData.GoAjax(TourData.ajaxurl);
                    return false;
                }
                else {
                    tableToolbar._showMsg("只能选择一行进行删除!");
                    return false;
                }
            },
            //Ajax请求
            GoAjax: function(url) {
                $.newAjax({
                    type: "post",
                    cache: false,
                    url: url,
                    dataType: "json",
                    success: function(ret) {
                        if (ret.result == "1") {
                            tableToolbar._showMsg(ret.msg, function() { location.reload(); });
                        }
                        else {
                            tableToolbar._showMsg(ret.msg, function() { location.reload(); });
                        }
                    },
                    error: function() {
                        tableToolbar._showMsg(tableToolbar.errorMsg);
                    }
                });
            },
            //显示弹窗
            ShowBoxy: function(data) {
                Boxy.iframeDialog({
                    iframeUrl: data.iframeUrl,
                    title: data.title,
                    modal: true,
                    width: data.width,
                    height: data.height,
                    afterHide: function() { TourData.reload(); }
                });
            },
            BindBtn: function() {
                //绑定Add事件
                $(".toolbar_add").click(function() {
                    var tourdataid = $(this).closest("tr").find("checkbox").val();
                    TourData.Add(tourdataid);
                    return false;
                });
                $(".check").click(function() {
                    var tid = $(this).closest("tr").attr("tid");
                    tableToolbar.ShowConfirmMsg("确定要审核吗？", function() {
                        var url = "/jidiaoCenter/TourData.aspx?dotype=check&id=" + tid;
                        TourData.GoAjax(url);
                        return false;
                    })
                });



                tableToolbar.init({
                    tableContainerSelector: "#liststyle", //表格选择器
                    objectName: "行", //

                    //修改-删除-取消-复制 为默认按钮，按钮class对应  toolbar_update  toolbar_delete  toolbar_cancel  toolbar_copy即可
                    updateCallBack: function(obj) {
                        //修改
                        TourData.Update(obj);
                    },
                    deleteCallBack: function(objsArr) {
                        //删除(批量)
                        TourData.DelAll(objsArr);
                    }
                });
            }
        };
    </script>

</asp:Content>
