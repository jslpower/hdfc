<%@ Page Language="C#" MasterPageFile="~/MasterPage/Front.Master" AutoEventWireup="true"
    CodeBehind="DijieList.aspx.cs" Inherits="Web.jidiaoCenter.DijieList" Title="地接安排" %>

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
                            <b>当前您所在位置：</b> &gt;&gt; 计调中心 &gt;&gt; 地接安排
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
                            团号：
                            <input type="text" size="20" id="txttourcode" class="inputtext formsize100" name="txttourcode"
                                value='<%=Request.QueryString["txttourcode"] %>' />
                            出团日期：
                            <input type="text" size="12" id="txtleavedate" onfocus="WdatePicker()" class="inputtext formsize70"
                                name="txtleavedate" value='<%=Request.QueryString["txtleavedate"] %>' />
                            -
                            <input type="text" size="12" id="txtbackdate" onfocus="WdatePicker()" class="inputtext formsize70"
                                name="txtbackdate" value='<%=Request.QueryString["txtbackdate"] %>' />
                            <asp:PlaceHolder runat="server" ID="PhDijie">地接社： <span>
                                <input type="text" size="20" id="txtsourcename" style="background-color: #dadada;"
                                    readonly="readonly" class="inputtext formsize100 Offers" name="txtsourcename"
                                    value='<%=Request.QueryString["txtsourcename"] %>' />
                                <input type="hidden" value="" id="hidsourceid" name="hidsourceid" />
                                <a href="javascript:;" class="xuanyong Offers"></a></span></asp:PlaceHolder>
                            月结：<select name="yuejie" class="inputselect">
                                <option value="-1">请选择</option>
                                <option value="1" <%=Request.QueryString["yuejie"]=="1"?"selected='selected'":"" %>>
                                    是</option>
                                <option value="0" <%=Request.QueryString["yuejie"]=="0"?"selected='selected'":"" %>>
                                    否</option>
                            </select>
                            团队类型：
                            <select class="inputselect" name="tourtype">
                                <option value="-1">请选择</option>
                                <option value="0" <%=Request.QueryString["tourtype"]=="0"?"selected='selected'":"" %>>团</option>
                                <option value="1" <%=Request.QueryString["tourtype"]=="1"?"selected='selected'":"" %>>散</option>
                            </select>
                            <input type="submit" class="search-btn" value="" />
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
            <table width="99%" cellspacing="0" cellpadding="0" border="0">
                <tbody>
                    <tr>
                        <asp:PlaceHolder runat="server" ID="phadd" Visible="false">
                            <td width="90" align="center">
                                <a class="toolbar_add" href="javascript:void(0)">新增</a>
                            </td>
                        </asp:PlaceHolder>
                        <asp:PlaceHolder runat="server" ID="PhUpdate" Visible="false">
                            <td width="90" align="center">
                                <a class="toolbar_update" href="javascript:void(0)">修改</a>
                            </td>
                        </asp:PlaceHolder>
                        <asp:PlaceHolder runat="server" ID="Phdelete" Visible="false">
                            <td width="90" align="center">
                                <a href="javascript:void(0)" class="toolbar_delete">删除</a>
                            </td>
                        </asp:PlaceHolder>
                        <td align="left">
                            &nbsp;
                        </td>
                    </tr>
                </tbody>
            </table>
        </div>
        <div class="tablelist">
            <table width="100%" cellspacing="1" cellpadding="0" border="0" id="liststyle">
                <tbody>
                    <tr>
                        <th width="60" align="center" rowspan="2">
                            <input type="checkbox" id="checkbox3" name="checkbox3">全选
                        </th>
                        <th align="center" rowspan="2">
                            团号
                        </th>
                        <th align="center" rowspan="2">
                            线路名称
                        </th>
                        <th align="center" rowspan="2">
                            出团日期
                        </th>
                        <th align="center" rowspan="2">
                            人数
                        </th>
                        <th align="center" rowspan="2">
                            地接名称
                        </th>
                        <th align="center" rowspan="2">
                            大交通
                        </th>
                        <th height="25" align="center" colspan="11">
                            成本
                        </th>
                        <th align="center" rowspan="2">
                            合计金额
                        </th>
                    </tr>
                    <tr>
                        <th height="25" align="center" class="nojiacu">
                            房
                        </th>
                        <th align="center" class="nojiacu">
                            餐
                        </th>
                        <th align="center" class="nojiacu">
                            车
                        </th>
                        <th align="center" class="nojiacu">
                            门
                        </th>
                        <th align="center" class="nojiacu">
                            导
                        </th>
                        <th align="center" class="nojiacu">
                            大交通
                        </th>
                        <th align="center" class="nojiacu">
                            购物提成人头
                        </th>
                        <th align="center" class="nojiacu">
                            加点费用
                        </th>
                        <th align="center" class="nojiacu">
                            导游现收
                        </th>
                        <th align="center" class="nojiacu">
                            导游现付
                        </th>
                        <th align="center" class="nojiacu">
                            其他
                        </th>
                    </tr>
                    <asp:Repeater runat="server" ID="rptList">
                        <ItemTemplate>
                            <tr class="<%# GetTrBgColorByTourState(Eval("TourStatus"),Eval("TourCode").ToString(),Container.ItemIndex +1 ) %>">
                                <td height="30" align="center">
                                    <input type="checkbox" id="checkbox" name="checkbox" value="<%#Eval("PlanId") %>">
                                    <%#Container.ItemIndex+1 %>
                                </td>
                                <td align="center">
                                    <%#Eval("TourCode")%>
                                </td>
                                <td align="center">
                                    <%#Eval("RouteName")%>
                                </td>
                                <td align="center">
                                    <%#EyouSoft.Common.Utils.GetDateTime(Convert.ToString(Eval("LDate"))).ToString("yyyy-MM-dd")%>
                                </td>
                                <td align="center">
                                    <%#EyouSoft.Common.Utils.GetInt(Convert.ToString(Eval("Adults"))) + EyouSoft.Common.Utils.GetInt(Convert.ToString(Eval("Childs"))) + EyouSoft.Common.Utils.GetInt(Convert.ToString(Eval("Accompanys")))%>
                                </td>
                                <td align="center">
                                    <span style="display: none;">
                                        <%#GetDijie(Eval("ContactList"), Eval("GysName"))%></span> <a class="paopao" href="javascript:;"
                                            bt-xtitle="" title="">
                                            <%#Eval("GysName")%><%#GetTourStatus(Eval("DiJieStatus"))%>
                                        </a>
                                </td>
                                <td align="center">
                                    <span style="display: none;">
                                        <%#GetTraffic(Eval("PlanTicketList"),"list")%></span> <a class="paopao" href="javascript:;"
                                            bt-xtitle="" title="">
                                            <%#GetTraffic(Eval("PlanTicketList"),"tickettype")%></a>
                                </td>
                                <td align="center">
                                    <%#this.ToMoneyString(Eval("Hotel"))%>
                                </td>
                                <td align="center">
                                    <%#this.ToMoneyString(Eval("Dining"))%>
                                </td>
                                <td align="center">
                                    <%#this.ToMoneyString(Eval("Car"))%>
                                </td>
                                <td align="center">
                                    <%#this.ToMoneyString(Eval("Ticket"))%>
                                </td>
                                <td align="center">
                                    <%#this.ToMoneyString(Eval("Guide"))%>
                                </td>
                                <td align="center">
                                    <%#this.ToMoneyString(Eval("Traffic"))%>
                                </td>
                                <td align="center">
                                    <%#this.ToMoneyString(Eval("Head"))%>
                                </td>
                                <td align="center">
                                    <%#this.ToMoneyString(Eval("AddPrice"))%>
                                </td>
                                <td align="center">
                                    <%#this.ToMoneyString(Eval("GuideIncome"))%>
                                </td>
                                <td align="center">
                                    <%#this.ToMoneyString(Eval("GuidePay"))%>
                                </td>
                                <td align="center">
                                    <%#this.ToMoneyString(Eval("Other"))%>
                                </td>
                                <td align="center">
                                    <%#this.ToMoneyString(Eval("SumPrice"))%>
                                </td>
                            </tr>
                        </ItemTemplate>
                    </asp:Repeater>
                    <asp:Literal ID="lbemptymsg" runat="server"></asp:Literal>
                    <tr>
                        <td height="30" bgcolor="#E3F1FC" align="right" colspan="18">
                            合计：
                        </td>
                        <td align="center" bgcolor="#E3F1FC">
                            <asp:Label ID="lbSumMoney" runat="server" Text="0"></asp:Label>
                        </td>
                    </tr>
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
                width: 400,
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
            ajaxurl: "/jidiaoCenter/DijieList.aspx",
            //添加
            Add: function() {
                PlanList.ShowBoxy({ iframeUrl: "/jidiaoCenter/DijieEdit.aspx?dotype=add", title: "新增安排", width: "700px", height: "550px" });
            },
            //修改(弹窗)---objsArr选中的TR对象
            Update: function(ObjsArr) {
                PlanList.ShowBoxy({ iframeUrl: "/jidiaoCenter/DijieEdit.aspx?dotype=update&id=" + ObjsArr[0].find("input[type='checkbox']").val(), title: "修改安排", width: "700px", height: "550px" });
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
                    PlanList.ajaxurl = "/jidiaoCenter/DijieList.aspx?dotype=delete&id=" + PlanList.GetCheckBox(objArr);
                    //执行/ajax
                    PlanList.GoAjax(PlanList.ajaxurl);
                } else {
                    tableToolbar._showMsg("只能选择一行进行删除");
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
                    afterHide: function() { PlanList.reload(); }
                });
            },
            BindBtn: function() {
                //绑定Add事件
                $(".toolbar_add").click(function() {
                    PlanList.Add();
                    return false;
                });
                $(".Offers").live("click", function() {
                    $(this).parent().find("a[class='xuanyong']").attr("id", "btn_" + parseInt(Math.random() * 100000));
                    var url = "/CommonPage/UserSupper.aspx?aid=" + $(this).parent().find("a[class='xuanyong']").attr("id") + "&";
                    var hideObj = $(this).parent().find("input[name='hidsourceid']");
                    var showObj = $(this).parent().find("input[name='txtsourcename']");
                    if (!hideObj.attr("id")) {
                        hideObj.attr("id", "hideID_" + parseInt(Math.random() * 10000000));
                    }
                    if (!showObj.attr("id")) {
                        showObj.attr("id", "ShowID_" + parseInt(Math.random() * 10000000));
                    }
                    url += $.param({ suppliertype: '<%=EyouSoft.Model.EnumType.CompanyStructure.SupplierType.地接 %>', hideID: $("#" + hideObj.attr("id")).val(), pIframeId: '<%= EyouSoft.Common.Utils.GetQueryStringValue("iframeId") %>', callback: "PlanList.CallBackDaoyou", isadd: 0 })
                    top.Boxy.iframeDialog({
                        iframeUrl: url,
                        title: "选择地接社",
                        modal: true,
                        width: "880",
                        height: "350"
                    });
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
            },
            CallBackDaoyou: function(obj) {
                $("#txtsourcename").val(obj.name);
                $("#hidsourceid").val(obj.id);
            }
        };
    </script>

</asp:Content>
