<%@ Page Language="C#" MasterPageFile="~/MasterPage/Front.Master" AutoEventWireup="true"
    CodeBehind="CustomerList.aspx.cs" Inherits="Web.CustomerManage.CustomerList"
    Title="客户管理" %>

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
                            <b>当前您所在位置：</b> &gt;&gt; 客户管理 &gt;&gt; 客户资料
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
                                省份：</label>
                            <select id="ddlProvice" name="ddlProvice" class="inputselect">
                            </select>
                            <label>
                                城市：</label>
                            <select id="ddlCity" name="ddlCity" class="inputselect">
                            </select>
                            <label>
                                单位名称：</label>
                            <input type="text" id="unitname" class="inputtext formsize120" name="unitname" value='<%=Request.QueryString["unitname"] %>' />
                            <label>
                                联系人：</label>
                            <input type="text" id="contactname" class="inputtext formsize80" name="contactname"
                                value='<%=EyouSoft.Common.Utils.GetQueryStringValue("contactname") %>' />
                            <label>
                                联系电话：</label>
                            <input type="text" id="contacttel" class="inputtext formsize80" name="contacttel"
                                value='<%=EyouSoft.Common.Utils.GetQueryStringValue("contacttel") %>' />
                                <br />
                            <label>
                                客户评级：</label>
                            <select class="inputselect" id="ddlCustomerRating" name="ddlCustomerRating">
                                <%=BindCustomerRating(EyouSoft.Common.Utils.GetQueryStringValue("ddlCustomerRating"))%>
                            </select>
                            <label>
                                地址：</label>
                            <input type="text" size="20" id="address" class="inputtext formsize180" name="address"
                                value='<%=EyouSoft.Common.Utils.GetQueryStringValue("address") %>' />
                                <label>
                                信用等级：</label>
                                <select class="inputselect" id="ddlrating" name="ddlrating">
                                    <%=BindRating(EyouSoft.Common.Utils.GetQueryStringValue("ddlrating")) %>
                                </select>
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
                        <asp:PlaceHolder runat="server" ID="phadd">
                            <td width="90" align="center">
                                <a class="toolbar_add" href="javascript:;">新增</a>
                            </td>
                        </asp:PlaceHolder>
                        <asp:PlaceHolder runat="server" ID="Phupdate">
                            <td width="90" align="center">
                                <a class="toolbar_update" href="javascript:;">修改</a>
                            </td>
                        </asp:PlaceHolder>
                        <asp:PlaceHolder runat="server" ID="phdelete">
                            <td width="90" align="center">
                                <a class="toolbar_delete" href="javascript:;">删除</a>
                            </td>
                        </asp:PlaceHolder>
                        <asp:PlaceHolder runat="server" ID="phdaochu">
                            <td width="90" align="center">
                                <a class="toolbar_daochu" onclick="toXls1();return false;" href="javascript:;">导出</a>
                            </td>
                        </asp:PlaceHolder>
                    </tr>
                </tbody>
            </table>
        </div>
        <div class="tablelist">
            <table width="100%" cellspacing="1" cellpadding="0" border="0" id="liststyle">
                <tbody>
                    <tr>
                        <th width="36" height="30" bgcolor="#BDDCF4" align="center">
                            <input type="checkbox" id="checkbox3" name="checkbox3" />序号
                        </th>
                        <th bgcolor="#BDDCF4" align="center">
                            所在地
                        </th>
                        <th bgcolor="#bddcf4" align="center">
                            单位名称
                        </th>
                         <th bgcolor="#bddcf4" align="center">
                            信用等级
                        </th>
                        <th bgcolor="#bddcf4" align="center">
                            联系人
                        </th>
                        <th width="140" bgcolor="#bddcf4" align="center">
                            电话
                        </th>
                        <th width="140" bgcolor="#bddcf4" align="center">
                            手机
                        </th>
                        <th bgcolor="#bddcf4" align="center">
                            地址
                        </th>
                    </tr>
                    <asp:Repeater runat="server" ID="rptlist">
                        <ItemTemplate>
                            <tr class="<%# Container.ItemIndex % 2 == 0 ? "even" : "odd"%>">
                                <td height="30" align="center">
                                    <input type="checkbox" name="checkbox" value='<%#Eval("Id") %>' />
                                </td>
                                <td align="center">
                                    <%#Eval("ProvinceName")%>&nbsp;
                                    <%#Eval("CityName")%>
                                </td>
                                <td align="center">
                                    <%#Eval("CustomerName")%><span style="color: Red">(<%#Eval("CustomerRating")%>)</span>
                                </td>
                                <td align="center">
                                    <a class="xinyong"  data-id='<%#Eval("Id") %>' data-score='<%#Eval("RatingId")%>'
                                        href="javascript:;"><span class="pandl3">
                                        <%#Convert.ToInt32(Eval("RatingId")) == 0 ? "设置等级" : ReturnRatingName(Eval("RatingId"))%>
                                            </span></a>
                                </td>
                                <td align="center">
                                    <%#Eval("ContactName")%>
                                </td>
                                <td align="center">
                                    <%#Eval("Phone")%>
                                </td>
                                <td align="center">
                                    <%#Eval("Mobile")%>
                                </td>
                                <td align="center">
                                    <%#Eval("Address")%>
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
            CustomerPage.PageInit();
            CustomerPage.BindBtn();
        })
        var CustomerPage = {
            PageInit: function() {
                pcToobar.init({
                    pID: "#ddlProvice",
                    cID: "#ddlCity",
                    comID: '<%=this.SiteUserInfo.CompanyId %>',
                    gSelect: "1",
                    pSelect: '<%=Request.QueryString["ddlProvice"] %>',
                    cSelect: '<%=Request.QueryString["ddlCity"] %>'
                });
            },
            reload: function() {
                window.location.href = window.location.href;
                return false;
            },
            //ajax执行文件路径,默认为本页面
            ajaxurl: "/jidiaoCenter/CustomerList.aspx",
            //添加
            Add: function() {
                CustomerPage.ShowBoxy({ iframeUrl: "/CustomerManage/CustomerEdit.aspx?dotype=add&id=", title: "新增", width: "920px", height: "510px" });
            },
            //修改(弹窗)---objsArr选中的TR对象
            Update: function(ObjsArr) {
                CustomerPage.ShowBoxy({ iframeUrl: "/CustomerManage/CustomerEdit.aspx?dotype=update&id=" + ObjsArr[0].find("input[type='checkbox']").val(), title: "修改", width: "920px", height: "510px" });
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
            //删除
            DelAll: function(objArr) {
                if (objArr.length == 1) {
                    //获取默认路径并重新拼接url（注：全局变量劲量不要改变，当常量就行）
                    CustomerPage.ajaxurl = "/CustomerManage/CustomerList.aspx?dotype=delete&id=" + CustomerPage.GetCheckBox(objArr);
                    //执行/ajax
                    CustomerPage.GoAjax(CustomerPage.ajaxurl);
                } else {
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
                    afterHide: function() { CustomerPage.reload(); }
                });
            },
            Score: function(rid, cid) {
            CustomerPage.ShowBoxy({ iframeUrl: "/CustomerManage/Xinyong.aspx?rid=" + rid + "&cid=" + cid, title: "信用等级", width: "520px", height: "150px" });
            },
            BindBtn: function() {
                //绑定Add事件
                $(".toolbar_add").click(function() {
                    CustomerPage.Add();
                    return false;
                });
                tableToolbar.init({
                    tableContainerSelector: "#liststyle", //表格选择器
                    objectName: "行", //
                    updateCallBack: function(obj) {
                        //修改
                        CustomerPage.Update(obj);
                    },
                    deleteCallBack: function(objsArr) {
                        //删除(批量)
                        CustomerPage.DelAll(objsArr);
                    }
                });
                $(".xinyong").click(function() {
                    var rid = $(this).attr("data-score");
                    var cid = $(this).attr("data-id");
                    CustomerPage.Score(rid, cid);
                })
            }
        }
    </script>

</asp:Content>
