<%@ Page Language="C#" MasterPageFile="~/MasterPage/Front.Master" AutoEventWireup="true"
    CodeBehind="CustomerGuanhuai.aspx.cs" Inherits="Web.CustomerManage.CustomerGuanhuai"
    Title="客户关怀" %>

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
                            <span class="lineprotitle">客户管理</span>
                        </td>
                        <td width="85%" nowrap="nowrap" align="right" style="padding: 0 10px 2px 0; color: #13509f;">
                            <b>当前您所在位置：</b> &gt;&gt; 客户管理 &gt;&gt; 客户关怀
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
                        <form id="form1" method="get">
                        <div class="searchbox">
                            <label>
                                拜访人：</label>
                            <input type="text" id="vistor" class="searchinput inputtext" name="vistor" value='<%=Request.QueryString["vistor"] %>' />
                            <label>
                                拜访日期：</label>
                            <input type="text" size="12" id="starttime" onfocus="WdatePicker()" class="inputtext formsize70"
                                name="starttime" value='<%=Request.QueryString["starttime"] %>' />
                            -
                            <input type="text" size="12" id="endtime" onfocus="WdatePicker()" class="inputtext formsize70"
                                name="endtime" value='<%=Request.QueryString["endtime"] %>' />
                            <label>
                                组团社：</label>
                            <input type="text" id="unitname" class="inputtext formsize140" name="unitname" value='<%=Request.QueryString["unitname"] %>' />
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
        <div class="btnbox">
            <table cellspacing="0" cellpadding="0" border="0" align="left">
                <tbody>
                    <tr>
                        <td width="90" align="center">
                            <a class="toolbar_add" href="javascript:;">新增</a>
                        </td>
                        <td width="90" align="center">
                            <a class="toolbar_update" href="javascript:;">修改</a>
                        </td>
                        <td width="90" align="center">
                            <a class="toolbar_delete" href="javascript:;">删除</a>
                        </td>
                    </tr>
                </tbody>
            </table>
        </div>
        <div class="tablelist">
            <table width="100%" cellspacing="1" cellpadding="0" border="0" id="liststyle">
                <tbody>
                    <tr>
                        <th width="36" height="30" bgcolor="#BDDCF4" align="center">
                            <input type="checkbox" id="checkbox3" name="checkbox3" />全选
                        </th>
                        <th bgcolor="#bddcf4" align="center">
                            拜访人
                        </th>
                        <th bgcolor="#bddcf4" align="center">
                            拜访日期
                        </th>
                        <th bgcolor="#bddcf4" align="center">
                            组团社
                        </th>
                        <th bgcolor="#bddcf4" align="center">
                            支出费用
                        </th>
                        <th bgcolor="#bddcf4" align="center">
                            支出理由
                        </th>
                    </tr>
                    <asp:Repeater runat="server" ID="rptList">
                        <ItemTemplate>
                            <tr class="<%# Container.ItemIndex % 2 == 0 ? "even" : "odd"%>">
                                <td height="30" align="center">
                                    <input type="checkbox" name="checkbox" value='<%#Eval("CareId") %>'>
                                </td>
                                <td align="center">
                                    <%#Eval("VisitName")%>
                                </td>
                                <td align="center">
                                    <%#Convert.ToDateTime(Eval("VisitTime")).ToString("yyyy-MM-dd")%>
                                </td>
                                <td align="center">
                                    <span style="display: none">
                                        <%#GetContactInfo(Eval("Contact"))%></span> <a class="paopao" href="javascript:;"
                                            bt-xtitle="" title="">
                                            <%#Eval("CustomerName")%></a>
                                </td>
                                <td align="center">
                                    <%#Convert.ToDecimal(Eval("PayMoney")).ToString("f2")%>
                                </td>
                                <td align="center">
                                   <%#Eval("PayReason")%>
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
        $(function() {
            $('.paopao').bt({
                contentSelector: function() { 
                return $(this).prev("span").html();
                },
                positions: ['bottom'],
                fill: '#effaff',
                strokeStyle: '#2a9cd4',
                noShadowOpts: { strokeStyle: "#2a9cd4" },
                spikeLength: 5,
                spikeGirth: 15,
                width: 600,
                overlap: 0,
                centerPointY: 4,
                cornerRadius: 4,
                shadow: true,
                shadowColor: 'rgba(0,0,0,.5)',
                cssStyles: { color: '#1351a0', 'line-height': '200%' }
            });
            PlanList.BindBtn();
        });
        var PlanList = {
            reload: function() {
                window.location.href = window.location.href;
                return false;
            },
            //ajax执行文件路径,默认为本页面
            ajaxurl: "/CustomerManage/CustomerGuanhuai.aspx",
            //添加
            Add: function() {
                PlanList.ShowBoxy({ iframeUrl: "/CustomerManage/GuanhuaiEdit.aspx?dotype=add", title: "新增", width: "720px", height: "310px" });
            },
            //修改(弹窗)---objsArr选中的TR对象
            Update: function(ObjsArr) {
                PlanList.ShowBoxy({ iframeUrl: "/CustomerManage/GuanhuaiEdit.aspx?dotype=update&id=" + ObjsArr[0].find("input[type='checkbox']").val(), title: "修改", width: "720px", height: "310px" });
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
                //获取默认路径并重新拼接url（注：全局变量劲量不要改变，当常量就行）
                PlanList.ajaxurl = "/CustomerManage/CustomerGuanhuai.aspx?dotype=delete&id=" + PlanList.GetCheckBox(objArr);
                //执行/ajax
                PlanList.GoAjax(PlanList.ajaxurl);
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
                    afterHide: function() { PlanList.reload(); }
                });
            },
            BindBtn: function() {
                //绑定Add事件
                $(".toolbar_add").click(function() {
                    PlanList.Add();
                    return false;
                });
                tableToolbar.init({
                    tableContainerSelector: "#liststyle", //表格选择器
                    objectName: "行", //

                    //修改-删除-取消-复制 为默认按钮，按钮class对应  toolbar_update  toolbar_delete  toolbar_cancel  toolbar_copy即可
                    updateCallBack: function(obj) {
                        //修改
                        PlanList.Update(obj);
                    },
                    deleteCallBack: function(objsArr) {
                        //删除(批量)
                        PlanList.DelAll(objsArr);
                    }
                });
            }
        };
    </script>

</asp:Content>
