<%@ Page Language="C#" MasterPageFile="~/MasterPage/Boxy.Master" AutoEventWireup="true"
    CodeBehind="selectBanci.aspx.cs" Inherits="Web.jidiaoCenter.selectBanci" Title="选用班次" %>

<asp:Content ID="Content1" ContentPlaceHolderID="PageHead" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="PageBody" runat="server">
    <form id="form1" method="get" runat="server">
    <div>
        <table width="99%" align="center" cellpadding="0" cellspacing="0" bgcolor="#e9f4f9"
            style="margin: 0 auto">
            <tr>
                <td width="90%" align="left">
                    班次/航班名称：
                    <input name="txtName" type="text" class="inputtext formsize100" id="txtName" value='<%=Request.QueryString["txtName"]%>' />
                    <input type="hidden" name="callback" id="callback" value="<%=EyouSoft.Common.Utils.GetQueryStringValue("callBack") %>" />
                    <input type="hidden" name="iframeid" id="iframeid" value="<%=EyouSoft.Common.Utils.GetQueryStringValue("iframeid") %>" />
                    <input type="hidden" name="pIframeID" id="pIframeID" value="<%=EyouSoft.Common.Utils.GetQueryStringValue("pIframeID") %>" />
                    <input type="hidden" name="hideID" id="hideID" value="<%=EyouSoft.Common.Utils.GetQueryStringValue("hideID") %>" />
                    <input type="hidden" name="aid" id="aid" value="<%=EyouSoft.Common.Utils.GetQueryStringValue("aid") %>" />
                </td>
            </tr>
        </table>
    </div>
    <div style="margin: 0 auto 0 auto; width: 99%;">
        <div id="AjaxDataList" style="width: 100%; padding-top: 10px">
        </div>
        <table cellspacing="0" cellpadding="0" border="0" align="center" id="TabBtn">
            <tbody>
                <tr>
                    <td width="76" height="40" align="center" class="tjbtn02">
                        <a href="javascript:;" id="a_btn">选用</a>
                    </td>
                    <td width="76" height="40" align="center" class="tjbtn02">
                        <a href="javascript:;" onclick="window.parent.Boxy.getIframeDialog('<%=EyouSoft.Common.Utils.GetQueryStringValue("iframeId") %>').hide();">
                            关闭</a>
                    </td>
                </tr>
            </tbody>
        </table>
    </div>
    </form>

    <script type="text/javascript">
        $(function() {
            SelectRoutePage.GetData();
            $("#txtName").keyup(function() {
                SelectRoutePage.GetData();
            })
            $("#a_btn").click(function() {
                if ($("#tblList").find("input[type='radio']:checked").length > 0) {
                    SelectRoutePage.SetValue();
                    SelectRoutePage.SelectValue();
                }
                else {
                    parent.Boxy.getIframeDialog('<%=Request.QueryString["iframeId"] %>').hide();
                }
                return false;
            })
            $("#btn_save").unbind("click").live("click", function() {
                var url = "/jidiaoCenter/AjaxBanciRequest.aspx?dotype=add";
                if (!SelectRoutePage.CheckForm()) {
                    return false;
                }
                SelectRoutePage.GoAjax(url, "add", $(this));
            })
            $("#btnxuan").unbind("click").live("click", function() {
                var url = "/jidiaoCenter/AjaxBanciRequest.aspx?dotype=add";
                if (!SelectRoutePage.CheckForm()) {
                    return false;
                }
                SelectRoutePage.GoAjax(url, "xuanyong", $(this));
            })

            var selectval = '<%=Request.QueryString["hideID"] %>';
            $("#tblList").find("input[type='radio']").each(function() {
                if ($(this).val() == selectval) {
                    $(this).attr("checked", "checked");
                }
            });
        })
        var SelectRoutePage = {
            GetData: function() {
                var url = "/jidiaoCenter/AjaxBanciRequest.aspx?txtName=" + $("#txtName").val();
                var data = {};
                data.iframeId = '<%= EyouSoft.Common.Utils.GetQueryStringValue("iframeId") %>';
                data.callBack = '<%= EyouSoft.Common.Utils.GetQueryStringValue("callBack") %>';
                data.pIframeId = '<%= EyouSoft.Common.Utils.GetQueryStringValue("pIframeId") %>';
                data.type = '<%=EyouSoft.Common.Utils.GetQueryStringValue("type") %>'
                SelectRoutePage.GetAjaxData(url + "&" + $.param(data));
                return false;
            },
            GetAjaxData: function(url) {
                //AJAX 加载数据
                $("#AjaxDataList").html("<div style='width:100%; text-align:center;'><img src='/images/loadingnew.gif' border='0' align='absmiddle'/>&nbsp;正在加载,请等待....&nbsp;</div>");
                $.newAjax({
                    type: "Get",
                    url: url,
                    cache: false,
                    success: function(result) {
                        $("#AjaxDataList").html(result);
                        var boxyParentWin = window.parent.Boxy.getIframeWindowByID('<%=Request.QueryString["pIframeID"] %>') || parent;

                        var data = {
                            id: '<%=EyouSoft.Common.Utils.GetQueryStringValue("hideID") %>'
                        }
                        //报账页面 团队支出 选用 2次选中功能
                        //选中单选钮
                        $(":radio[value='" + data.id + "']").attr("checked", "checked");
                    }
                });
            },
            _dataObj: {},
            selectValue: "",
            selectTxt: "",
            qujian: "",
            otherprice: "",
            starttime: "",
            yongjin: "",
            seat: "",
            SetNewValue: function(obj) {
                SelectRoutePage.selectValue = obj.Id;
                SelectRoutePage.selectTxt = obj.TrafficNumber;
                SelectRoutePage.yongjin = obj.Brokerage;
                SelectRoutePage.otherprice = obj.OtherPrice;
                SelectRoutePage.starttime = obj.TrafficTime;
                SelectRoutePage.qujian = obj.Interval;
                SelectRoutePage.seat = obj.TrafficSeat;

            },
            SetValue: function() {
                var valueArray = [], txtArray = [], qujian = [], otherprice = [], starttime = [], yongjin = [], seat = [];
                $("#tblList").find("input[type='radio']:checked").each(function() {
                    valueArray.push($(this).val());
                    txtArray.push($(this).attr("data-show"));
                    qujian.push($(this).attr("data-qujian"));
                    otherprice.push($(this).attr("data-otherprice"));
                    starttime.push($(this).attr("data-starttime"));
                    yongjin.push($(this).attr("data-yongjin"));
                    seat.push($(this).attr("data-seat"));
                })

                SelectRoutePage.selectValue = valueArray.join(',');
                SelectRoutePage.selectTxt = txtArray.join(',');
                SelectRoutePage.qujian = qujian.join(',');
                SelectRoutePage.otherprice = otherprice.join(',');
                SelectRoutePage.starttime = starttime.join(',');
                SelectRoutePage.yongjin = yongjin.join(',');
                SelectRoutePage.seat = seat.join(',');
            },
            SelectValue: function() {
                var data = {
                    callBack: '<%=Request.QueryString["callBack"]%>',
                    hideID: '<%=Request.QueryString["hideID"]%>',
                    iframeID: '<%=Request.QueryString["iframeId"]%>',
                    pIframeID: '<%=Request.QueryString["pIframeID"] %>'
                }

                var args = {
                    aid: '<%=Request.QueryString["aid"] %>',
                    id: SelectRoutePage.selectValue,
                    name: SelectRoutePage.selectTxt,
                    qujian: SelectRoutePage.qujian,
                    otherprice: SelectRoutePage.otherprice,
                    starttime: SelectRoutePage.starttime,
                    yongjin: SelectRoutePage.yongjin,
                    seat: SelectRoutePage.seat
                }
                //根据父级是否为弹窗传值
                if (data.pIframeID != "" && data.pIframeID.length > 0) {
                    //定义父级弹窗
                    var boxyParent = window.parent.Boxy.getIframeWindow(data.pIframeID) || window.parent.Boxy.getIframeWindowByID(data.pIframeID);
                    //判断是否存在回调方法

                    if (data.callBack != null && data.callBack.length > 0) {
                        if (data.callBack.indexOf('.') == -1) {
                            boxyParent[data.callBack](args);
                        }
                        else {
                            boxyParent[data.callBack.split('.')[0]][data.callBack.split('.')[1]](args);
                        }
                    }
                    //定义回调
                }
                else {
                    //判断是否存在回调方法
                    if (data.callBack != null && data.callBack.length > 0) {
                        if (data.callBack.indexOf('.') == -1) {
                            window.parent[data.callBack](args);
                        }
                        else {
                            window.parent[data.callBack.split('.')[0]][data.callBack.split('.')[1]](args);
                        }
                    }
                    //定义回调
                }
                parent.Boxy.getIframeDialog('<%=Request.QueryString["iframeId"] %>').hide();
            },
            GoAjax: function(url, dotype, obj) {
                $("#" + $(obj).attr("id")).unbind("click");
                $.newAjax({
                    type: "post",
                    cache: false,
                    url: url,
                    dataType: "json",
                    data: $("#btn_save").closest("form").serialize(),
                    success: function(ret) {
                        if (ret.result != "0") {
                            if (ret.obj != null) {
                                if (dotype == "xuanyong") {
                                    SelectRoutePage.SetNewValue(ret.obj);
                                    SelectRoutePage.SelectValue();
                                } else {
                                    parent.tableToolbar._showMsg(ret.msg, function() { location.href = location.href; });
                                }
                            }

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
                return ValiDatorForm.validator($("#btn_save").closest("form").get(0), "parent");
            }
        }
    

    </script>

</asp:Content>
