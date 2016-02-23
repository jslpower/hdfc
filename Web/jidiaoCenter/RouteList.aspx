<%@ Page Language="C#" MasterPageFile="~/MasterPage/Front.Master" AutoEventWireup="true"
    CodeBehind="RouteList.aspx.cs" Inherits="Web.jidiaoCenter.RouteList" Title="线路管理" %>

<%@ Register Assembly="ControlLibrary" Namespace="Adpost.Common.ExporPage" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="mainbody">
        <div class="lineprotitlebox">
            <table width="100%" cellspacing="0" cellpadding="0" border="0">
                <tbody>
                    <tr>
                        <td width="15%" nowrap="nowrap">
                            <span class="lineprotitle">计调中心</span>
                        </td>
                        <td width="85%" nowrap="nowrap" align="right" style="padding: 0 10px 2px 0; color: #13509f;">
                            <b>当前您所在位置：</b> &gt;&gt; 计调中心 &gt;&gt; 线路管理
                        </td>
                    </tr>
                    <tr>
                        <td height="2" bgcolor="#000000" colspan="2">
                        </td>
                    </tr>
                </tbody>
            </table>
        </div>
        <div class="hr_10">
        </div>
        <table width="99%" cellspacing="0" cellpadding="0" border="0" align="center">
            <tbody>
                <tr>
                    <td width="10" valign="top">
                        <img src="/images/yuanleft.gif" alt="" />
                    </td>
                    <td>
                        <form method="get" id="form1">
                        <div class="searchbox">
                            线路名称：
                            <input type="text" size="30" id="routename" class="inputtext formsize140" name="routename"
                                value='<%=Request.QueryString["routename"] %>' />
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
        <div class="tablelist">
            <table width="100%" cellspacing="1" cellpadding="0" border="0" id="liststyle">
                <tbody>
                    <tr>
                        <th width="36" height="30" align="center">
                            序号
                        </th>
                        <th align="center">
                            线路名称
                        </th>
                        <th align="center">
                            操作
                        </th>
                    </tr>
                    <asp:Repeater runat="server" ID="RptList">
                        <ItemTemplate>
                            <tr class="<%# Container.ItemIndex % 2 == 0 ? "even" : "odd"%>" data-id='<%#Eval("RouteId") %>'
                                data-name='<%#Eval("RouteName") %>'>
                                <td height="30" align="center">
                                    <%#Container.ItemIndex+1 %>
                                </td>
                                <td align="center">
                                    <%#Eval("RouteName") %>
                                </td>
                                <td align="center">
                                    <a class="update" href="javascript:;">修改</a> | <a href="javascript:;" class="delete">
                                        删除</a>
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
                        <td align="right">
                            <cc1:ExporPageInfoSelect ID="ExporPageInfoSelect1" runat="server" />
                        </td>
                    </tr>
                </tbody>
            </table>
        </div>
    </div>

    <script type="text/javascript">
        $(function(){
            RouteList.BindBtn();
        })
        var RouteList = {
            reload: function() {
                window.location.href = window.location.href;
                return false;
            },
            //修改
            Update: function(routeid,routename) {
                RouteList.ShowBoxy({ iframeUrl: "/jidiaoCenter/RouteEdit.aspx?routeid="+routeid, title: "修改", width: "400px", height: "150px" });
            },
            Delete:function(routeid){
                var url="/jidiaoCenter/RouteList.aspx?dotype=delete&routeid="+routeid;
                RouteList.GoAjax(url);
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
                    afterHide: function() { RouteList.reload(); }
                });
            },
            BindBtn: function() {
                //绑定Update事件
                $(".update").click(function() {
                    var routeid=$(this).closest("tr").attr("data-id");
                    var routename=$(this).closest("tr").attr("data-name")
                    RouteList.Update(routeid,routename);
                    return false;
                });
                $(".delete").click(function(){
                    var routeid=$(this).closest("tr").attr("data-id");
                    tableToolbar.ShowConfirmMsg("确定要删除此线路？",function(){
                        RouteList.Delete(routeid);
                    });
                    return false;
                })
                tableToolbar.init({
                    tableContainerSelector: "#liststyle", //表格选择器
                    objectName: "行"
                });
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
                            tableToolbar._showMsg(ret.msg, function() { RouteList.reload(); });
                        }
                        else {
                            tableToolbar._showMsg(ret.msg, function() { RouteList.reload();  });
                        }
                    },
                    error: function() {
                        tableToolbar._showMsg(tableToolbar.errorMsg);
                    }
                });
            }
        };
    </script>

</asp:Content>
