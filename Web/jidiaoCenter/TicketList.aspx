<%@ Page Language="C#" MasterPageFile="~/MasterPage/Front.Master" AutoEventWireup="true"
    CodeBehind="TicketList.aspx.cs" Inherits="Web.jidiaoCenter.TicketList" Title="票务安排" %>

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
                            <b>当前您所在位置：</b> &gt;&gt; 计调中心 &gt;&gt; 票务安排
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
                            <input type="text" size="20" id="tourcode" class="inputtext formsize100" name="tourcode"
                                value='<%=Request.QueryString["tourcode"] %>' />
                            时间：
                            <input type="text" size="12" id="starttime" onfocus="WdatePicker()" class="inputtext formsize70"
                                name="starttime" value='<%=Request.QueryString["starttime"] %>' value='<%=Request.QueryString["starttime"] %>' />
                            -
                            <input type="text" size="12" id="endtime" onfocus="WdatePicker()" class="inputtext formsize70"
                                name="endtime" value='<%=Request.QueryString["endtime"] %>' value='<%=Request.QueryString["endtime"] %>' />
                            出票点:<input type="text" size="20" class="inputtext formsize100" name="ticket" id="ticket"
                                value='<%=Request.QueryString["ticket"] %>' />
                            状态：
                            <select id="sltstatus" name="sltstatus" class="inputselect">
                                <%=statusStr %>
                            </select>
                            类型：
                            <select id="slttype" name="slttype" class="inputselect">
                                <%=typeStr %>
                            </select>
                            月结：<select name="yuejie" class="inputselect">
                                <option value="-1">请选择</option>
                                <option value="1" <%=Request.QueryString["yuejie"]=="1"?"selected='selected'":"" %>>
                                    是</option>
                                <option value="0" <%=Request.QueryString["yuejie"]=="0"?"selected='selected'":"" %>>
                                    否</option>
                            </select>
                            团队类型:<select name="tourtype" id="tourtype" class="inputselect"><option value="-1">请选择</option>
                                <option value="0" <%=Request.QueryString["tourtype"]=="0"?"selected='selected'":"" %>>
                                    团</option>
                                <option value="1" <%=Request.QueryString["tourtype"]=="1"?"selected='selected'":"" %>>
                                    散</option>
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
                        <asp:PlaceHolder runat="server" Visible="false" ID="Phadd">
                            <td width="90" align="center">
                                <a class="toolbar_add" href="javascript:;">新增</a>
                            </td>
                        </asp:PlaceHolder>
                        <asp:PlaceHolder runat="server" ID="Phupdate">
                            <td width="90" align="center">
                                <a class="toolbar_update" href="javascript:;">修改</a>
                            </td>
                        </asp:PlaceHolder>
                        <asp:PlaceHolder runat="server" ID="Phdelete">
                            <td width="90" align="center">
                                <a class="toolbar_delete" href="javascript:;">删除</a>
                            </td>
                        </asp:PlaceHolder>
                        <td align="center" width="200">
                            注解： <span class="toursearch" data-class="shenqing" style="cursor: pointer" onclick="PlanList.SearchTourStatus(this)">
                                <img width="16" height="16" title="出票申请" src="/images/chpshq.gif" alt="" />
                                出票申请 </span><span class="toursearch" style="cursor: pointer" data-class="chupiao"
                                    onclick="PlanList.SearchTourStatus(this)">
                                    <img width="16" height="16" title="已出票" src="/images/ychp.gif" alt="" />
                                    已出票</span>
                        </td>
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
                            <input type="checkbox" id="checkbox3" name="checkbox3" />全选
                        </th>
                        <th align="center" rowspan="2">
                            <a class="ticketModel" data-class="out" href="javascript:;" style="cursor: pointer">
                                出票</a>/<a class="ticketModel" data-class="back" href="javascript:;" style="cursor: pointer">
                                    退票</a>
                        </th>
                        <th align="center" rowspan="2">
                            机票/火车票
                        </th>
                        <th align="center" rowspan="2">
                            团号
                        </th>
                        <th align="center" rowspan="2">
                            车次/航班
                        </th>
                        <th align="center" rowspan="2">
                            舱位/席别
                        </th>
                        <th align="center" rowspan="2">
                            开车/起飞时间
                        </th>
                        <th align="center" rowspan="2">
                            出票点
                        </th>
                        <th align="center" rowspan="2">
                            出票人
                        </th>
                        <th align="center" rowspan="2">
                            区间
                        </th>
                        <th align="center" rowspan="2">
                            票数
                        </th>
                        <th align="center" colspan="2">
                            单价
                        </th>
                        <th align="center" rowspan="2">
                            手续费/机燃
                        </th>
                        <th align="center" colspan="2">
                            人数
                        </th>
                        <th align="center" rowspan="2">
                            佣金
                        </th>
                        <th align="center" rowspan="2">
                            合计
                        </th>
                    </tr>
                    <tr>
                        <th align="center" class="nojiacu">
                            成人
                        </th>
                        <th align="center" class="nojiacu">
                            儿童
                        </th>
                        <th align="center" class="nojiacu">
                            成人
                        </th>
                        <th align="center" class="nojiacu">
                            儿童
                        </th>
                    </tr>
                    <asp:Repeater runat="server" ID="RptList">
                        <ItemTemplate>
                            <tr class="<%# GetTrBgColorByTourState(Eval("TourStatus"),Eval("TicketMode"),Eval("TourCode").ToString(),Container.ItemIndex +1 ) %>">
                                <td height="30" align="center">
                                    <input type="checkbox" data-value="planId" data-tourcode='<%#Eval("TourCode") %>'
                                        data-tourid='<%#Eval("TourId") %>' id="checkbox" name="checkbox" value='<%#Eval("PlanId") %>'
                                        data-mode='<%#(int)((EyouSoft.Model.EnumType.PlanStructure.TicketMode)Eval("TicketMode")) %>' />
                                    <%#Container.ItemIndex+1 %>
                                </td>
                                <td align="center">
                                    <%#((EyouSoft.Model.EnumType.PlanStructure.TicketMode)Eval("TicketMode")).ToString()%>
                                </td>
                                <td align="center">
                                    <%#((EyouSoft.Model.EnumType.PlanStructure.TicketType)Eval("TicketType")).ToString()%>
                                </td>
                                <td align="center">
                                    <%#GetTicketstatus(Eval("TicketStatus"), Eval("TourCode"))%>
                                </td>
                                <td align="center">
                                    <%#Eval("TrafficNumber")%>
                                </td>
                                <td align="center">
                                    <%#Convert.ToInt32(Eval("_TrafficSeat"))==0?"":Eval("_TrafficSeat") %>
                                </td>
                                <td align="center">
                                    <%--     <%#EyouSoft.Common.Utils.GetDateTime(Eval("TrafficTime").ToString()).ToString("yyyy-MM-dd hh:mm:ss")%>--%>
                                    <%#GetTrafficTime(Eval("TrafficTime"))%>
                                </td>
                                <td align="center">
                                    <%#Eval("GysName")%>
                                </td>
                                <td align="center">
                                    <%#((EyouSoft.Model.EnumType.PlanStructure.TicketStatus)Eval("TicketStatus"))==EyouSoft.Model.EnumType.PlanStructure.TicketStatus.已出票?"<font class='fred'>"+Convert.ToString(Eval("Ticketer"))+"</font>":Convert.ToString(Eval("Ticketer"))%>
                                </td>
                                <td align="center">
                                    <%#Eval("Interval")%>
                                </td>
                                <td align="center">
                                    <%#EyouSoft.Common.Utils.GetInt(Convert.ToString(Eval("Adults"))) + EyouSoft.Common.Utils.GetInt(Convert.ToString(Eval("Childs")))%>
                                </td>
                                <td align="center">
                                    <%#this.ToMoneyString(Eval("AdultPrice"))%>
                                </td>
                                <td align="center">
                                    <%#this.ToMoneyString(Eval("ChildPrice"))%>
                                </td>
                                <td align="center">
                                    <%# this.ToMoneyString(Eval("OtherPrice"))%>
                                </td>
                                <td align="center">
                                    <%#Eval("Adults")%>
                                </td>
                                <td align="center">
                                    <%#Eval("Childs")%>
                                </td>
                                <td align="center">
                                    <%#this.ToMoneyString(Eval("Brokerage"))%>
                                </td>
                                <td align="center">
                                    <%#this.ToMoneyString(Eval("SumPrice"))%>
                                </td>
                            </tr>
                        </ItemTemplate>
                    </asp:Repeater>
                    <asp:Literal ID="lbemptymsg" runat="server"></asp:Literal>
                    <tr>
                        <td height="30" bgcolor="#E3F1FC" align="right" colspan="10">
                            合计：
                        </td>
                        <td align="center" bgcolor="#E3F1FC">
                            <asp:Label ID="lbsumcount" runat="server" Text="0"></asp:Label>
                        </td>
                        <td align="center" bgcolor="#E3F1FC">
                            &nbsp;
                        </td>
                        <td align="center" bgcolor="#E3F1FC">
                            &nbsp;
                        </td>
                        <td align="center" bgcolor="#E3F1FC">
                            &nbsp;
                        </td>
                        <td align="center" bgcolor="#E3F1FC">
                            <asp:Label ID="lbadults" runat="server" Text="0"></asp:Label>
                        </td>
                        <td align="center" bgcolor="#E3F1FC">
                            <asp:Label ID="lbchilds" runat="server" Text="0"></asp:Label>
                        </td>
                        <td align="center" bgcolor="#E3F1FC">
                            &nbsp;
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
            PlanList.BindBtn();
        });
        var PlanList = {
            reload: function() {
                window.location.href = window.location.href;
                return false;
            },
            //ajax执行文件路径,默认为本页面
            ajaxurl: "/jidiaoCenter/TicketList.aspx",
            //添加
            Add: function(tourcode, tourid) {
                PlanList.ShowBoxy({ iframeUrl: "/jidiaoCenter/TicketEdit.aspx?dotype=add&planid=&tourcode=" + tourcode + "&tourid=" + tourid, title: "新增票务安排", width: "760px", height: "700px" });
            },
            //修改(弹窗)---objsArr选中的TR对象
            Update: function(ObjsArr) {
                PlanList.ShowBoxy({ iframeUrl: "/jidiaoCenter/TicketEdit.aspx?dotype=update&planid=" + ObjsArr[0].find("input[type='checkbox']").val(), title: "修改票务安排", width: "780px", height: "700px" });
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
            GetMode: function(objArr) {
                //定义数组对象
                var list = new Array();
                //遍历按钮返回数组对象
                for (var i = 0; i < objArr.length; i++) {
                    //从数组对象中找到数据所在，并保存到数组对象中
                    if (objArr[i].find("input[type='checkbox']").val() != "on") {
                        list.push(objArr[i].find("input[type='checkbox']").attr("data-mode"));
                    }
                }
                return list.join(',');
            },
            //删除(可多行)
            DelAll: function(objArr) {
                if (objArr.length == 1) {
                    //获取默认路径并重新拼接url（注：全局变量劲量不要改变，当常量就行）
                    PlanList.ajaxurl = "/jidiaoCenter/TicketList.aspx?dotype=delete&id=" + PlanList.GetCheckBox(objArr) + "&mode=" + PlanList.GetMode(objArr);
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
                    var objRow = $("#liststyle").find("input[type='checkbox'][data-value='planId']:checked");
                    var count = objRow.length;
                    var tourcode = "";
                    var tourid = ""
                    if (count > 1) {
                        tableToolbar._showMsg("只能选择一行数据新增!");
                        return false;
                    }
                    if (count == 1) {
                        tourcode = objRow.attr("data-tourcode");
                        tourid = objRow.attr("data-tourid");
                    }
                    PlanList.Add(tourcode, tourid);
                    return false;
                });

                $(".ticketModel").click(function() {
                    var type = $(this).attr("data-class");
                    window.location.href = "/jidiaoCenter/TicketList.aspx?type=" + type;
                })
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
            SearchTourStatus: function(obj) {
                if ($(obj).attr("data-class") == "shenqing") {
                    url = "/jidiaoCenter/TicketList.aspx?sltstatus=0";
                } else {
                    url = "/jidiaoCenter/TicketList.aspx?sltstatus=2";
                }
                location.href = url;
            }
        };
    </script>

</asp:Content>
