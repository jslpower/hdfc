<%@ Page Language="C#" MasterPageFile="~/MasterPage/Boxy.Master" AutoEventWireup="true"
    CodeBehind="TicketEdit.aspx.cs" Inherits="Web.jidiaoCenter.TicketEdit" Title="安排票务" %>

<%@ Register Src="../UserControl/OrderCustomer.ascx" TagName="OrderCustomer" TagPrefix="uc1" %>
<%@ Register Src="../UserControl/UploadControl.ascx" TagName="UploadControl" TagPrefix="uc2" %>
<%@ Register Src="../UserControl/SupperControl.ascx" TagName="SupperControl" TagPrefix="uc3" %>
<%@ Register Src="../UserControl/TourSelect.ascx" TagName="TourSelect" TagPrefix="uc4" %>
<%@ Register Src="../UserControl/SellsSelect.ascx" TagName="SellsSelect" TagPrefix="uc5" %>
<asp:Content ID="Content1" ContentPlaceHolderID="PageHead" runat="server">

    <script src="/JS/Newjquery.autocomplete.js" type="text/javascript"></script>

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="PageBody" runat="server">
    <form id="form1" runat="server">
    <div style="width: 740px; margin: 10px auto;">
        <table width="100%" cellspacing="1" cellpadding="0" border="0" align="center" id="Tableform">
            <tbody>
                <tr class="odd">
                    <th height="30" align="right">
                        <span class="fred">*</span>团号：
                    </th>
                    <td bgcolor="#E3F1FC" colspan="3">
                        <uc4:TourSelect ID="TourSelect1" runat="server" SupplierType="票务" />
                    </td>
                </tr>
                <tr class="odd">
                    <th height="30" align="right">
                        出/退票：
                    </th>
                    <td bgcolor="#E3F1FC">
                        <label>
                            <asp:RadioButton ID="radchu" Checked="true" runat="server" GroupName="radticket" />
                            出票
                        </label>
                        <label>
                            <span style="background: #f00; height: 20px; padding: 5px; display: inline-block;
                                color: #fff;">
                                <asp:RadioButton ID="radtui" runat="server" GroupName="radticket" />
                                退票</span>
                        </label>
                    </td>
                    <th align="right">
                        机票/火车票：
                    </th>
                    <td bgcolor="#E3F1FC">
                        <asp:DropDownList ID="dptticket" runat="server" CssClass="inputselect">
                            <asp:ListItem Value="0">机票</asp:ListItem>
                            <asp:ListItem Value="1">火车票</asp:ListItem>
                        </asp:DropDownList>
                    </td>
                </tr>
                <tr class="odd">
                    <th height="30" align="right">
                        <span class="fred">*</span>出票点：
                    </th>
                    <td bgcolor="#E3F1FC">
                        <uc3:SupperControl ID="SupperControl1" runat="server" SupplierType="票务" IsMust="true" />
                    </td>
                    <th align="right">
                        <font class="fred">*</font> 本社出票人：
                    </th>
                    <td bgcolor="#E3F1FC">
                        <uc5:SellsSelect ID="SellsSelect1" runat="server" />
                    </td>
                </tr>
                <tr class="odd">
                    <th height="30" align="right">
                        车次/航班：
                    </th>
                    <td bgcolor="#E3F1FC">
                        <asp:TextBox ID="txtcheci" runat="server" ReadOnly="true" Style="background-color: #dadada"
                            class="formsize100 inputtext offer"></asp:TextBox>
                        <input type="hidden" id="hidcheciid" runat="server" name="hidcheciid" value="" />
                        <a id="btnxuanyong" class="xuanyong offer" href="javascript:void(0);"></a>
                    </td>
                    <th align="right">
                        <span class="fred">*</span>开车/起飞时间：
                    </th>
                    <td bgcolor="#E3F1FC">
                        <asp:TextBox ID="txttime" onfocus="WdatePicker()" errmsg="请填写开车/起飞日期!" valid="required"
                            runat="server" class="formsize120 inputtext"></asp:TextBox>
                        <select id="sltHH" name="sltHH" class="inputselect" errmsg="请填写开车/起飞时间(小时)!" valid="required">
                            <%=StrHH %>
                        </select>
                        时
                        <select id="sltmm" name="sltmm" class="inputselect" errmsg="请填写开车/起飞时间(分钟)!" valid="required">
                            <%=Strmm %>
                        </select>
                        分
                    </td>
                </tr>
                <tr class="odd">
                    <th height="30" align="right">
                        <span class="fred">*</span>区间：
                    </th>
                    <td bgcolor="#E3F1FC">
                        <asp:TextBox ID="txtqujian" runat="server" class="formsize100 inputtext"></asp:TextBox>
                    </td>
                    <th align="right">
                        <span class="fred">*</span>单价：
                    </th>
                    <td bgcolor="#E3F1FC">
                        成人
                        <asp:TextBox ID="txtadultprice" valid="isMoney|range" min="0" errmsg="成人价格格式有误|成人单价不能小于0"
                            runat="server" class="formsize40 inputtext"></asp:TextBox>
                        儿童
                        <asp:TextBox ID="txtchildprice" valid="isMoney|range" min="0" errmsg="儿童价格格式有误|儿童价格不能小于0"
                            runat="server" class="formsize40 inputtext"></asp:TextBox>
                    </td>
                </tr>
                <tr class="odd">
                    <th height="30" align="right">
                        手续费/机燃：
                    </th>
                    <td bgcolor="#E3F1FC">
                        <asp:TextBox ID="txtOtherPrice" valid="isMoney" errmsg="手续费/机燃价格格式有误" runat="server"
                            class="formsize50 inputtext"></asp:TextBox>
                    </td>
                    <th align="right">
                        佣金：
                    </th>
                    <td bgcolor="#E3F1FC">
                        <asp:TextBox ID="txtBrokerage" valid="isMoney" errmsg="佣金价格格式有误" runat="server" class="formsize50 inputtext"></asp:TextBox>
                    </td>
                </tr>
                <tr class="odd">
                    <th height="30" align="right">
                        <span class="fred">*</span>人数：
                    </th>
                    <td bgcolor="#E3F1FC">
                        成人
                        <asp:TextBox ID="txtadults" valid="isInt|range" min="0" errmsg="成人数必须是整数|成人数不能小于0"
                            runat="server" class="formsize40 inputtext"></asp:TextBox>
                        儿童
                        <asp:TextBox ID="txtchilds" valid="isInt|range" min="0" errmsg="儿童数必须是整数|儿童数不能小于0"
                            runat="server" class="formsize40 inputtext"></asp:TextBox>
                    </td>
                    <th align="right">
                        舱位/席别：
                    </th>
                    <td bgcolor="#E3F1FC">
                        <asp:DropDownList ID="ddlTrafficSeat" runat="server" CssClass="inputselect">
                        </asp:DropDownList>
                        <asp:TextBox ID="txtTrafficSeat" Visible="false" runat="server" class="formsize100 inputtext"></asp:TextBox>
                    </td>
                </tr>
                <tr class="odd">
                    <th height="30" align="right">
                        支付方式：
                    </th>
                    <td bgcolor="#E3F1FC">
                        <select id="sltpaytype" name="sltpaytype" class="inputselect">
                            <%=PayTypeOption %>
                        </select>
                    </td>
                    <th align="right">
                        <span class="fred">*</span>合计：
                    </th>
                    <td bgcolor="#E3F1FC">
                        <asp:TextBox ID="txtSumpirce" valid="required|isMoney|range" min="1" errmsg="合计不能为空|合计格式有误|合计金额不能小于1"
                            runat="server" class="formsize50 inputtext"></asp:TextBox>
                    </td>
                </tr>
                <tr class="odd">
                    <th height="30" align="right">
                        月结：
                    </th>
                    <td bgcolor="#E3F1FC" colspan="3">
                        <asp:DropDownList ID="ddlyuejie" runat="server" CssClass="inputselect">
                            <asp:ListItem Value="1">是</asp:ListItem>
                            <asp:ListItem Value="0">否</asp:ListItem>
                        </asp:DropDownList>
                        <span runat="server" id="spanjisuantime"><span class="fred">*</span>结算时间：
                            <asp:TextBox ID="txtjiesuantime" valid="required" errmsg="请填写结算时间" onfocus="WdatePicker()"
                                runat="server" CssClass="formsize80 inputtext"></asp:TextBox></span>
                    </td>
                </tr>
                <tr class="odd">
                    <th height="30" align="right">
                        附件：
                    </th>
                    <td bgcolor="#E3F1FC" colspan="3">
                        <uc2:UploadControl ID="UploadControl1" runat="server" IsUploadMore="true" IsUploadSelf="true" />
                        <div style="width: 450px; float: left; margin-left: 5px;">
                            <asp:Repeater ID="rplfile" runat="server">
                                <ItemTemplate>
                                    <a target="_blank" href='<%#Eval("FilePath") %>' style="vertical-align: bottom">
                                        <%#Eval("FileName")%></a><span><img alt="" style="vertical-align: bottom; cursor: pointer;"
                                            src="/images/cha.gif" data-delimg="delimg" onclick="TicketEditPage.RemoveFile(this)" /><input
                                                type="hidden" data-id='<%#Eval("Id") %>' name="hidefile" value="<%#Eval("FilePath") %>" /></span>
                                </ItemTemplate>
                            </asp:Repeater>
                        </div>
                    </td>
                </tr>
            </tbody>
        </table>
    </div>
    <div style="width: 740px; margin: 10px auto;">
        <uc1:OrderCustomer ID="OrderCustomer1" runat="server" />
        <asp:PlaceHolder runat="server" ID="PhBtn">
            <table width="100%" cellspacing="0" cellpadding="0" border="0" align="center" style="margin: 10px auto;">
                <tbody>
                    <tr class="odd">
                        <td height="40" bgcolor="#E3F1FC" colspan="14">
                            <table cellspacing="0" cellpadding="0" border="0" align="center">
                                <tbody>
                                    <tr>
                                        <asp:PlaceHolder runat="server" ID="phshenqing">
                                            <td width="80" align="center" class="tjbtn02" style="padding-right: 50px;">
                                                <a class="btn" data-id="add" href="javascript:;">申请</a>
                                            </td>
                                        </asp:PlaceHolder>
                                        <asp:PlaceHolder runat="server" ID="phqueren">
                                            <td width="80" align="center" class="tjbtn02" style="padding-right: 50px;">
                                                <a class="btn" data-id="queren" href="javascript:;">结算确认</a>
                                            </td>
                                        </asp:PlaceHolder>
                                        <asp:PlaceHolder runat="server" ID="PhCheck">
                                            <td width="80" align="center" class="tjbtn02" style="padding-right: 50px;">
                                                <a class="btn" data-id="check" href="javascript:;">审核</a>
                                            </td>
                                        </asp:PlaceHolder>
                                        <asp:PlaceHolder runat="server" ID="PhCencer">
                                            <td width="80" align="center" class="tjbtn02" style="padding-right: 50px;">
                                                <a class="btn" data-id="cencer" href="javascript:;">取消审核</a>
                                            </td>
                                        </asp:PlaceHolder>
                                    </tr>
                                </tbody>
                            </table>
                        </td>
                    </tr>
                </tbody>
            </table>
        </asp:PlaceHolder>
    </div>
    <input type="hidden" id="hfileId" runat="server" value="" />
    <input type="hidden" id="hdpaytype" runat="server" value="0" />
    <input type="hidden" id="hdticketmode" runat="server" value="0" />
    </form>

    <script type="text/javascript">
        function CallBackFun(obj) {
            $("#<%=txtcheci.ClientID %>").val(obj.name);
            $("#<%=hidcheciid.ClientID %>").val(obj.id);
            $("#<%=txtqujian.ClientID %>").val(obj.qujian);
            $("#<%=txtOtherPrice.ClientID %>").val(obj.otherprice);
            $("#<%=txtBrokerage.ClientID %>").val(obj.yongjin);
            $("#<%=ddlTrafficSeat.ClientID %>").val(obj.seat);
            var sDate = obj.starttime;
            var dDate = new Date(Date.parse(sDate.replace(/-/g, "/")));
            $("#<%=txttime.ClientID %>").val(dDate.getFullYear() + "-" + (dDate.getMonth() + 1) + "-" + dDate.getDate());
            $("#sltHH").val(dDate.getHours());
            $("#sltmm").val(dDate.getMinutes());

        }
        $(function() {
            TicketEditPage.PageInit();
            TicketEditPage.BindBtn();
            TicketEditPage.xuanyong();
        })
        var TicketEditPage = {
            xuanyong: function() {
                $("#Tableform .offer").live("click", function() {
                    var url = "/jidiaoCenter/selectBanci.aspx?aid=btnxuanyong&";
                    var hideObj = $("#<%=hidcheciid.ClientID %>");
                    var showObj = $("#<%=txtcheci.ClientID %>");
                    var type = $("#<%=dptticket.ClientID %>").val();
                    url += $.param({ hideID: hideObj.val(), pIframeId: '<%= EyouSoft.Common.Utils.GetQueryStringValue("iframeId") %>', callback: "CallBackFun", type: type })
                    top.Boxy.iframeDialog({
                        iframeUrl: url,
                        title: "选择班次",
                        modal: true,
                        width: "930",
                        height: "350"
                    });
                })
            },
            PageInit: function() {
                $("#<%=txtadultprice.ClientID %>,#<%=txtchildprice.ClientID %>,#<%=txtBrokerage.ClientID %>,#<%=txtOtherPrice.ClientID %>,#<%=txtadults.ClientID %>,#<%=txtchilds.ClientID %>").keyup(function() {
                    TicketEditPage.AutoSumPrice();
                })
                $("#<%=dptticket.ClientID %>").change(function() {
                    TicketEditPage.AutoSumPrice();
                })
                $("#sltpaytype").change(function() {
                    $("#<%=hdpaytype.ClientID %>").val($(this).val());
                })
                $("#<%=radchu.ClientID %>").click(function() {
                    $("#<%=hdticketmode.ClientID %>").val("0");
                })
                $("#<%=radtui.ClientID %>").click(function() {
                    $("#<%=hdticketmode.ClientID %>").val("1");
                })
                $("#<%=ddlyuejie.ClientID %>").change(function() {
                    if ($(this).val() == "1") {
                        $("#<%=txtjiesuantime.ClientID %>").attr("valid", "required").attr("errmsg", "请填写结算时间");
                        $(this).next("span").show();
                    }
                    else {
                        $("#<%=txtjiesuantime.ClientID %>").removeAttr("valid").removeAttr("errmsg");
                        $(this).next("span").hide();
                    }
                })
            },
            BindBtn: function() {
                $(".btn").click(function() {
                    var type = $(this).attr("data-id");
                    var url = "/jidiaoCenter/TicketEdit.aspx?type=save&dotype=" + type + "&planid=" + '<%=Request.QueryString["planid"] %>';

                    if (!TicketEditPage.CheckForm() || !TicketEditPage.Check()) {
                        return;
                    }
                    $(this).html("提交中");
                    TicketEditPage.GoAjax(url, $(this));
                })
            },
            GoAjax: function(url, obj) {
                $(obj).unbind("click");
                $.newAjax({
                    type: "post",
                    cache: false,
                    url: url,
                    dataType: "json",
                    data: $("#<%=form1.ClientID %>").serialize(),
                    success: function(ret) {
                        if (ret.result == "1") {
                            parent.tableToolbar._showMsg(ret.msg, function() { parent.location.href = parent.location.href; });
                        }
                        else {
                            parent.tableToolbar._showMsg(ret.msg);
                        }
                    },
                    error: function() {
                        parent.tableToolbar._showMsg(tableToolbar.errorMsg);
                    }
                });
            },
            CheckForm: function() {
                return ValiDatorForm.validator($("#<%=form1.ClientID %>"), "parent");
            },
            AutoSumPrice: function() {
                var childs = tableToolbar.getInt($("#<%=txtchilds.ClientID %>").val());
                var adults = tableToolbar.getInt($("#<%=txtadults.ClientID %>").val());
                var childprice = tableToolbar.getFloat($("#<%=txtchildprice.ClientID %>").val());
                var adultprice = tableToolbar.getFloat($("#<%=txtadultprice.ClientID %>").val());
                var otherprice = tableToolbar.getFloat($("#<%=txtOtherPrice.ClientID %>").val());
                var Brokerage = tableToolbar.getFloat($("#<%=txtBrokerage.ClientID %>").val());
                var tickettype = $("#<%=dptticket.ClientID %>").val();
                var totalMoney = childprice * childs + adultprice * adults + otherprice * (adults + childs) - Brokerage;
                if (tickettype == "1") {
                    totalMoney = childprice * childs + adultprice * adults + otherprice * (adults + childs);
                }
                $("#<%=txtSumpirce.ClientID %>").val(totalMoney);
            },
            RemoveFile: function(obj) {
                $(obj).hide();
                var thisimg = $(obj).parent();
                thisimg.prev("a").hide();
                fileid += thisimg.find("input[type='hidden']").attr("data-id") + ",";
                $("#<%=hfileId.ClientID %>").val(fileid);
                thisimg.find("input[type='hidden']").val("");
                thisimg.hide();
                return false;
            },
            Check: function() {
                var result = true;
                var msg = "";
                var childs = tableToolbar.getInt($("#<%=txtchilds.ClientID %>").val());
                var adults = tableToolbar.getInt($("#<%=txtadults.ClientID %>").val());
                var childprice = tableToolbar.getFloat($("#<%=txtchildprice.ClientID %>").val());
                var adultprice = tableToolbar.getFloat($("#<%=txtadultprice.ClientID %>").val());
                if ((childprice + adultprice) <= 0) {
                    msg += "单价不能小于1<br />";
                }
                if ((childs + adults) <= 0) {
                    msg += "人数不能小于1";
                }

                if (msg != "") {
                    parent.tableToolbar._showMsg(msg);
                    result = false;
                }
                return result;
            }
        }
        
       
    </script>

</asp:Content>
