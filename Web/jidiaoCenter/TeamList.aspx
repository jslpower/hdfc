<%@ Page Language="C#" MasterPageFile="~/MasterPage/Front.Master" AutoEventWireup="true"
    CodeBehind="TeamList.aspx.cs" Inherits="Web.jidiaoCenter.TeamList" Title="确认件登记" %>

<%@ Register Assembly="ControlLibrary" Namespace="Adpost.Common.ExporPage" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">

    <script type="text/javascript">
        function dijieClick(obj) {
            var data = { dotype: "update", tid: $(obj).attr("data-gysid") };
            Boxy.iframeDialog({
                iframeUrl: "/ResourceManage/GroundAdd.aspx?" + $.param(data),
                title: "修改地接社信息",
                modal: true,
                width: "980px",
                height: "500px",
                afterHide: function() { window.location.href = window.location.href; }
            });
            return false;
        }

        function jipiaoClick(obj) {
            var data = { dotype: "update", tid: $(obj).attr("data-gysid") };
            Boxy.iframeDialog({
                iframeUrl: "/ResourceManage/TicketAdd.aspx?" + $.param(data),
                title: "修改机票供应商",
                modal: true,
                width: "980px",
                height: "500px",
                afterHide: function() { window.location.href = window.location.href; }
            });
            return false;
        }
    </script>

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
                            <b>当前您所在位置：</b> &gt;&gt; 计调中心 &gt;&gt; 确认件登记
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
                            线路名称：
                            <input type="text" size="20" id="routename" class="inputtext formsize100" name="routename"
                                value='<%=Request.QueryString["routename"] %>' />
                            销售员：
                            <input type="text" size="12" id="seller" class="inputtext formsize70" name="seller"
                                value='<%=Request.QueryString["seller"] %>' />
                            出团时间：
                            <input type="text" size="12" id="leavetime" onfocus="WdatePicker()" class="inputtext formsize70"
                                name="leavetime" value='<%=Request.QueryString["leavetime"] %>' />
                            -
                            <input type="text" size="12" id="backtime" onfocus="WdatePicker()" class="inputtext formsize70"
                                name="backtime" value='<%=Request.QueryString["backtime"] %>' />
                            <br />
                            下单时间：
                            <input type="text" size="12" id="starttime" onfocus="WdatePicker()" class="inputtext formsize70"
                                name="starttime" value='<%=Request.QueryString["starttime"] %>' />
                            -
                            <input type="text" size="12" id="endtime" onfocus="WdatePicker()" class="inputtext formsize70"
                                name="endtime" value='<%=Request.QueryString["endtime"] %>' />
                            计调员：
                            <input type="text" size="12" id="planer" class="inputtext formsize70" name="planer"
                                value='<%=Request.QueryString["planer"] %>' />
                            组团社：
                            <input type="text" size="20" id="zutuanshe" class="inputtext formsize100" name="zutuanshe"
                                value='<%=Request.QueryString["zutuanshe"] %>' />
                            地接社：
                            <input type="text" size="20" id="dijieshe" class="inputtext formsize100" name="dijieshe"
                                value='<%=Request.QueryString["dijieshe"] %>' />
                            <br>
                            月结：<select name="yuejie" class="inputselect">
                                <option value="-1">请选择</option>
                                <option value="1" <%=Request.QueryString["yuejie"]=="1"?"selected='selected'":"" %>>
                                    是</option>
                                <option value="0" <%=Request.QueryString["yuejie"]=="0"?"selected='selected'":"" %>>
                                    否</option>
                            </select>
                            出票：
                            <select name="chupiao" class="inputselect">
                                <option value="-1">请选择</option>
                                <option value="1" <%=Request.QueryString["chupiao"]=="1"?"selected='selected'":"" %>>
                                    是</option>
                                <option value="0" <%=Request.QueryString["chupiao"]=="0"?"selected='selected'":"" %>>
                                    否</option>
                            </select>
                            收清：
                            <select name="shouqing" class="inputselect">
                                <option value="-1">请选择</option>
                                <option value="1" <%=Request.QueryString["shouqing"]=="1"?"selected='selected'":"" %>>
                                    是</option>
                                <option value="0" <%=Request.QueryString["shouqing"]=="0"?"selected='selected'":"" %>>
                                    否</option>
                            </select>
                            是否操作结束:
                            <select name="end" class="inputselect">
                                <option value="-1">请选择</option>
                                <option value="1" <%=Request.QueryString["end"]=="1"?"selected='selected'":"" %>>是</option>
                                <option value="0" <%=Request.QueryString["end"]=="0"?"selected='selected'":"" %>>否</option>
                            </select>
                            团队状态：
                            <select name="tourstatus" class="inputselect">
                                <%=TourStatus%>
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
            <table width="99%" cellspacing="0" cellpadding="0" border="0" align="left">
                <tbody>
                    <tr>
                        <td width="90" align="center">
                            <a class="toolbar_add" href="javascript:;">新增</a>
                        </td>
                        <td width="90" align="center">
                            <a class="toolbar_update" href="javascript:;">修改</a>
                        </td>
                        <td width="90" align="center">
                            <a href="javascript:;" class="toolbar_delete">删除</a>
                        </td>
                        <asp:PlaceHolder runat="server" ID="phForceDel">
                            <td width="90" align="center">
                                <a href="javascript:void(0);" id="a_ForceDel">强制删除</a>
                            </td>
                        </asp:PlaceHolder>
                        <asp:PlaceHolder runat="server" ID="plnQuXiao" Visible="false">
                            <td width="90" align="center">
                                <a href="javascript:void(0);" id="a_QuXiao">取 消</a>
                            </td>
                        </asp:PlaceHolder>
                        <td align="right" class="fred">
                            当日登记<asp:Label ID="lbtuancount" CssClass="fred" runat="server" Text="0"></asp:Label>个团，<asp:Label
                                ID="lbsancount" runat="server" CssClass="fred" Text="0"></asp:Label>个散
                        </td>
                    </tr>
                </tbody>
            </table>
        </div>
        <div class="tablelist">
            <table width="100%" cellspacing="1" cellpadding="0" border="0" id="liststyle">
                <tbody>
                    <tr>
                        <th width="60" height="30" align="center">
                            <input type="checkbox" id="checkbox3" name="checkbox3">全选
                        </th>
                        <th align="center">
                            <a class="tuanorsan" data-class="tuan" href="javascript:;" style="cursor: pointer">团</a>/
                            <a class="tuanorsan" data-class="san" href="javascript:;" style="cursor: pointer">散</a>
                        </th>
                        <th align="center">
                            团号
                        </th>
                        <th align="center">
                            计调员
                        </th>
                        <th align="center">
                            线路名称
                        </th>
                        <th align="center">
                            人数
                        </th>
                        <th align="center">
                            组团社名称
                        </th>
                        <th align="center">
                            出票点
                        </th>
                        <th align="center">
                            地接名称
                        </th>
                        <th align="center">
                            地接导游
                        </th>
                        <th align="center">
                            收款情况
                        </th>
                        <%if (IsShowYongJin)
                          { %>
                        <th align="center">
                            返佣
                        </th>
                        <%} %>
                    </tr>
                    <asp:Repeater ID="rptList" runat="server">
                        <ItemTemplate>
                            <tr class="<%# Container.ItemIndex % 2 == 0 ? "even" : "odd"%> <%#((EyouSoft.Model.EnumType.TourStructure.TourStatus)Eval("TourStatus"))==EyouSoft.Model.EnumType.TourStructure.TourStatus.已取消?"huise":"" %>"
                                data-comid='<%#Eval("BuyCompanyId") %>'>
                                <td height="30" align="center">
                                    <input type="checkbox" name="checkbox" value="<%#Eval("TourId") %>" data-comid='<%#Eval("BuyCompanyId") %>'>
                                </td>
                                <td align="center">
                                    <%#GetTourType(Eval("TourType"))%>
                                </td>
                                <td align="center">
                                    <span style="display: none;">
                                        <%#GetTourInfo(Eval("IssueTime"), Eval("LDate"), Eval("RDate"))%></span> <a class="paopao"
                                            href="javascript:;" data-width="340" bt-xtitle="" title="">
                                            <%#SetDayWeight(Eval("TourCode"), Eval("TourStatus"))%></a><%#Daojishi(Eval("LDate"))%>
                                </td>
                                <td align="center">
                                    <%#Eval("Planer")%>
                                </td>
                                <td align="center">
                                    <%#Eval("RouteName")%><%#GetTourStatus(Eval("TourStatus"))%>
                                </td>
                                <td align="center">
                                    <%#Eval("Adults")%>+<%#Eval("Childs")%>+<%#Eval("Accompanys")%>
                                </td>
                                <td align="center">
                                    <span style="display: none;">
                                        <%#GetZuTuan(Eval("CustomerContactList"))%></span> <a class="paopao" data-com="company"
                                            data-width="640" href="javascript:;" bt-xtitle="" title="">
                                            <%#Eval("BuyCompnayName")%><sup><font class="fred font14"><%#Eval("BuyCompanyCount")%></font></sup></a>
                                    <!-- &nbsp;&nbsp;<a href="javascript:void(0)" title="点击给组团社发送提醒短信" class="duanxin" data-bcid="<%# Eval("BuyCompanyId") %>"
                                        data-ldate="<%# Eval("LDate","{0:yyyy-MM-dd}") %>" data-tourid="<%#Eval("TourId") %>"><img
                                            src="/images/shouji.gif" alt="点击给组团社发送提醒短信" style="cursor: pointer" /></a>-->
                                </td>
                                <td align="center">
                                    <span style="display: none;">
                                        <%#GetTicketInfo(Eval("PlanTicketList"))%></span> <a class="paopao" data-width="400"
                                            href="javascript:;" bt-xtitle="" title="">
                                            <%#Eval("TicketCompany")%></a>
                                </td>
                                <td align="center">
                                    <span style="display: none;">
                                        <%#GetDijieInfo(Eval("DijieList"))%></span> <a class="paopao" data-width="400" href="javascript:;"
                                            bt-xtitle="" title="">
                                            <%#Eval("DijieName")%></a>
                                </td>
                                <td align="center">
                                    <span style="display: none;">
                                        <%#GetGuideInfo(Eval("GuideList"))%></span> <a class="paopao" data-width="220" href="javascript:;"
                                            bt-xtitle="" title="">
                                            <%#Eval("GuideName")%></a>
                                </td>
                                <td align="center">
                                    <span style="display: none;">
                                        <%#GetPayInfo(Eval("ShouKuanList"), Eval("IsMonth"), Eval("MonthTime"))%></span>
                                    <a class='paopao' data-name='<%=isShoukuan?"a_ShouKuan":"" %>' data-width='<%#(bool)Eval("IsMonth") ? "140" : "540"%>'
                                        href="javascript:;" bt-xtitle="" title="">
                                        <%#(bool)Eval("IsMonth") ? "月结" : this.ToMoneyString(Eval("CheckMoney"))%>
                                        <%#(bool)Eval("IsClean") ? "<img src='/images/yshouq.gif' title='' bt-xtitle='已收清'>" : ""%></a>
                                </td>
                                <%if (IsShowYongJin)
                                  { %>
                                <td align="center">
                                    <%#this.ToMoneyString(Convert.ToInt32(Eval("RebatePeople"))*Convert.ToDecimal(Eval("RebatePrice")))%>
                                </td>
                                <%} %>
                            </tr>
                        </ItemTemplate>
                    </asp:Repeater>
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
            $("#liststyle").find("a[class='paopao']").each(function() {
                $(this).bt({
                    contentSelector: function() {
                        return $(this).prev("span").html();
                    },
                    positions: ['bottom'],
                    fill: '#effaff',
                    strokeStyle: '#2a9cd4',
                    noShadowOpts: { strokeStyle: "#2a9cd4" },
                    spikeLength: 5,
                    spikeGirth: 15,
                    overlap: 0,
                    width: $(this).attr("data-width"),
                    centerPointY: 4,
                    cornerRadius: 4,
                    shadow: true,
                    shadowColor: 'rgba(0,0,0,.5)',
                    cssStyles: { color: '#1351a0', 'line-height': '200%' }
                });
            })
            PlanList.BindBtn();
        });
        var PlanList = {
            reload: function() {
                window.location.href = window.location.href;
                return false;
            },
            //ajax执行文件路径,默认为本页面
            ajaxurl: "/jidiaoCenter/TeamList.aspx",
            //添加
            Add: function() {
                PlanList.ShowBoxy({ iframeUrl: "/jidiaoCenter/TeamEdit.aspx?dotype=add", title: "新增登记", width: "980px", height: "560px" });
            },
            //修改(弹窗)---objsArr选中的TR对象
            Update: function(ObjsArr) {
                PlanList.ShowBoxy({ iframeUrl: "/jidiaoCenter/TeamEdit.aspx?dotype=update&tourid=" + ObjsArr[0].find("input[type='checkbox']").val(), title: "修改登记", width: "980px", height: "560px" });
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
                    PlanList.ajaxurl = "/jidiaoCenter/TeamList.aspx?dotype=delete&id=" + PlanList.GetCheckBox(objArr);
                    //执行/ajax
                    PlanList.GoAjax(PlanList.ajaxurl);
                    return false;
                }
                else {
                    tableToolbar._showMsg("只能选择一行进行删除!");
                    return false;
                }
            },
            //强制删除(不受任何限制的删除确认件)
            ForceDelAll: function() {
                var obj = $("#liststyle").find("input[type='checkbox'][id!='checkbox3']:checked");
                if (obj.length == 1) {
                    tableToolbar.ShowConfirmMsg("您确定要删除选中确认件信息吗？", function() {
                        var id = $(obj).val();
                        //获取默认路径并重新拼接url（注：全局变量劲量不要改变，当常量就行）
                        PlanList.ajaxurl = "/jidiaoCenter/TeamList.aspx?dotype=forceDelete&id=" + id;
                        //执行/ajax
                        PlanList.GoAjax(PlanList.ajaxurl);
                        return false;
                    });
                }
                else {
                    tableToolbar._showMsg("必须而且只能选择一行进行删除!");
                    return false;
                }
            },
            quXiao: function() {
                var obj = $("#liststyle").find("input[type='checkbox'][id!='checkbox3']:checked");
                if (obj.length == 1) {
                    tableToolbar.ShowConfirmMsg("您确定要取消选中确认件信息吗？", function() {
                        var id = $(obj).val();
                        //获取默认路径并重新拼接url（注：全局变量劲量不要改变，当常量就行）
                        PlanList.ajaxurl = "/jidiaoCenter/TeamList.aspx?dotype=quXiao&id=" + id;
                        //执行/ajax
                        PlanList.GoAjax(PlanList.ajaxurl);
                        return false;
                    });
                }
                else {
                    tableToolbar._showMsg("必须而且只能选择一行进行取消!");
                    return false;
                }
            },
            //Ajax请求
            GoAjax: function(url) {
                $.newAjax({
                    type: "post",
                    cache: false,
                    url: url,
                    async: false,
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
                $("#liststyle .duanxin").click(function() {
                    var data = {
                        tourId: $(this).attr("data-tourid"),
                        bcid: $(this).attr("data-bcid"),
                        ld: $(this).attr("data-ldate"),
                        action: "add"
                    };
                    PlanList.ShowBoxy({ iframeUrl: "/jidiaoCenter/DuanxinEdit.aspx?" + $.param(data), title: "发送短信", width: "980px", height: "500px" });
                    return false;
                })
                $("#liststyle .paopao").each(function() {
                    var self = $(this);
                    if (self.attr("data-com") == "company") {
                        self.click(function() {
                            var comid = self.closest("tr").attr("data-comid");
                            var data = {
                                id: comid,
                                dotype: "update"
                            };
                            PlanList.ShowBoxy({ iframeUrl: "/CustomerManage/CustomerEdit.aspx?" + $.param(data), title: "修改组团社", width: "920px", height: "510px" });
                            return false;
                        })
                    }
                })
                $("#liststyle").find("a[data-name='a_ShouKuan']").each(function() {
                    $(this).click(function() {
                        var _data = { itemId: $.trim($(this).closest("tr").find("input[type='checkbox']").val()) };
                        Boxy.iframeDialog({
                            title: "收款登记",
                            iframeUrl: "/Fin/ShouKuan.aspx",
                            data: _data,
                            width: "960px",
                            height: "550px",
                            afterHide: function() {
                                window.location.href = window.location.href;
                            }
                        });
                        return false;
                    });
                });
                //绑定Add事件
                $(".toolbar_add").click(function() {
                    var tourid = $(this).closest("tr").find("checkbox").val();
                    PlanList.Add(tourid);
                    return false;
                });
                $(".tuanorsan").click(function() {
                    var type = $(this).attr("data-class");
                    window.location.href = "/jidiaoCenter/TeamList.aspx?type=" + type;
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

                $("#a_ForceDel").click(function() {
                    PlanList.ForceDelAll();
                    return false;
                });
                $("#a_QuXiao").click(function() {
                    PlanList.quXiao();
                    return false;
                });
            }
        };
    </script>

</asp:Content>
