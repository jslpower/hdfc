<%@ Page Language="C#" MasterPageFile="~/MasterPage/Boxy.Master" AutoEventWireup="true"
    CodeBehind="DuanxinEdit.aspx.cs" Inherits="Web.jidiaoCenter.DuanxinEdit" Title="发送短信" %>

<%@ Register Assembly="ControlLibrary" Namespace="ControlLibrary" TagPrefix="asp" %>
<asp:Content ID="Content1" ContentPlaceHolderID="PageHead" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="PageBody" runat="server">
    <div style="width: 960px; margin: 10px auto;">
        <span class="formtableT">短信提醒记录</span>
        <table width="100%" border="0" cellspacing="1" cellpadding="0" id="liststyle">
            <tr class="odd">
                <th width="6%" height="30" align="center">
                    编号
                </th>
                <th width="15%" height="30" align="center">
                    团号
                </th>
                <th width="15%" align="center">
                    手机号码
                </th>
                <th width="15%" align="center">
                    发送内容
                </th>
                <th width="20%" align="center">
                    发送时间
                </th>
                <th width="15%" align="center">
                    操作
                </th>
            </tr>
            <asp:CustomRepeater ID="rptSmsList" runat="server">
                <ItemTemplate>
                    <tr class="<%# Container.ItemIndex % 2 == 0 ? "even" : "odd"%>" data-id="<%#Eval("Id") %>"
                        data-tourid="<%# Eval("TourId") %>" data-isenabled="<%# (bool)Eval("IsEnabled") ? "1" : "0" %>">
                        <td height="30" align="center">
                            <%# Container.ItemIndex + 1 %>
                        </td>
                        <td align="center">
                            <%#Eval("TourNo")%>
                        </td>
                        <td align="center">
                            <%#Eval("MobileCode")%>
                        </td>
                        <td align="center">
                            <div style='word-wrap: break-word; width: 200px; overflow: hidden; padding-left: 3px;'>
                                <%# Eval("Content") %>
                            </div>
                        </td>
                        <td align="center">
                            <%#EyouSoft.Common.UtilsCommons.GetDateString(Eval("Time"),"yyyy-MM-dd")%>
                        </td>
                        <td align="center">
                            <a href="javascript:;" class="a_editDuanXin"><font class="fblue">修改</font></a> <a
                                href="javascript:;" class="a_deleteDuanXin"><font class="fblue">删除</font></a>
                            <a href="javascript:;" class="a_stopDuanXin"><font class="fblue">
                                <%# !(bool)Eval("IsEnabled")?"启用":"停发" %></font></a>
                        </td>
                    </tr>
                </ItemTemplate>
            </asp:CustomRepeater>
        </table>
        <form runat="server" id="form1">
        <table width="100%" cellspacing="1" cellpadding="0" border="0" align="center">
            <tbody>
                <tr class="odd">
                    <th width="120" height="30" align="right">
                        <span class="fred">*</span>输入号码：
                    </th>
                    <td bgcolor="#E3F1FC">
                        <asp:TextBox ID="txtMobile" TextMode="MultiLine" CssClass="inputarea formsize260"
                            runat="server"></asp:TextBox>
                    </td>
                </tr>
                <tr class="odd">
                    <th height="30" align="right">
                        <span class="fred">*</span>输入内容：
                    </th>
                    <td bgcolor="#E3F1FC">
                        <asp:TextBox ID="txtContent" Style="width: 250px; height: 70px;" TextMode="MultiLine"
                            CssClass="inputarea formsize260" runat="server"></asp:TextBox>
                    </td>
                </tr>
                <tr class="odd">
                    <th height="30" align="right">
                        <span class="fred">*</span>发送通道：
                    </th>
                    <td bgcolor="#E3F1FC">
                        <select id="selSendChannel" runat="server">
                        </select>
                    </td>
                </tr>
                <tr class="odd">
                    <th height="30" align="right">
                        <span class="fred">*</span>发送时间：
                    </th>
                    <td bgcolor="#E3F1FC">
                        <asp:TextBox ID="txtsetTime" CssClass="inputtext formsize120" onfocus="WdatePicker({dateFmt:'yyyy-MM-dd HH:mm'})"
                            runat="server"></asp:TextBox>
                    </td>
                </tr>
            </tbody>
        </table>
        <table width="100%" cellspacing="0" cellpadding="0" border="0" align="center" style="margin: 10px auto;">
            <tbody>
                <tr class="odd">
                    <td height="40" bgcolor="#E3F1FC" colspan="14">
                        <table cellspacing="0" cellpadding="0" border="0" align="center">
                            <tbody>
                                <tr>
                                    <td width="80" align="center" class="tjbtn02">
                                        <a href="javascript:;" id="a_SaveDuanXin">保存</a>
                                    </td>
                                </tr>
                            </tbody>
                        </table>
                    </td>
                </tr>
            </tbody>
        </table>
        </form>
    </div>

    <script type="text/javascript">
        $(document).ready(function() {
            DuanXin.BindListEdit();
            DuanXin.BindListDel();
            DuanXin.BindBtn();
            DuanXin.BindStop();
        });
        var DuanXin = {
            _data: {
                action: '<%= EyouSoft.Common.Utils.GetQueryStringValue("action") %>',
                tourId: '<%= EyouSoft.Common.Utils.GetQueryStringValue("tourId") %>',
                bcid: '<%= EyouSoft.Common.Utils.GetQueryStringValue("bcid") %>',
                ld: '<%= EyouSoft.Common.Utils.GetQueryStringValue("ld") %>',
                Id: '<%= EyouSoft.Common.Utils.GetQueryStringValue("Id") %>',
                iframeId: '<%= EyouSoft.Common.Utils.GetQueryStringValue("iframeId") %>'
            },
            reload: function() {
                var tmp = {
                    tourId: '<%= EyouSoft.Common.Utils.GetQueryStringValue("tourId") %>',
                    bcid: '<%= EyouSoft.Common.Utils.GetQueryStringValue("bcid") %>',
                    ld: '<%= EyouSoft.Common.Utils.GetQueryStringValue("ld") %>',
                    iframeId: '<%= EyouSoft.Common.Utils.GetQueryStringValue("iframeId") %>'
                };
                window.location.href = "/jidiaoCenter/DuanxinEdit.aspx?" + $.param(tmp);
            },
            BindListEdit: function() {
                $("#liststyle").find(".a_editDuanXin").each(function() {
                    var tmp = {
                        action: 'edit',
                        Id: $(this).closest("tr").attr("data-id"),
                        tourId: '<%= EyouSoft.Common.Utils.GetQueryStringValue("tourId") %>',
                        iframeId: '<%= EyouSoft.Common.Utils.GetQueryStringValue("iframeId") %>'
                    };
                    $(this).click(function() {
                        window.location.href = "/jidiaoCenter/DuanxinEdit.aspx?" + $.param(tmp);
                        return false;
                    });
                });
            },
            BindListDel: function() {
                $("#liststyle").find(".a_deleteDuanXin").each(function() {
                    var tmp = {
                        action: 'del',
                        tourId: $(this).closest("tr").attr("data-tourid"),
                        Id: $(this).closest("tr").attr("data-id"),
                        iframeId: '<%= EyouSoft.Common.Utils.GetQueryStringValue("iframeId") %>'
                    };
                    $(this).click(function() {
                        tableToolbar.ShowConfirmMsg("您确定要删除此短息提醒吗？", function() {
                            $.ajax({
                                type: "post",
                                cache: false,
                                async: false,
                                url: "/jidiaoCenter/DuanxinEdit.aspx?" + $.param(tmp),
                                dataType: "json",
                                success: function(ret) {
                                    if (ret.result == "1") {
                                        tableToolbar._showMsg(ret.msg, function() {
                                            DuanXin.reload();
                                        });
                                    }
                                    else {
                                        tableToolbar._showMsg(ret.msg);
                                    }
                                },
                                error: function() { tableToolbar._showMsg(tableToolbar.errorMsg); }
                            });
                        });

                        return false;
                    });
                });
            },
            BindBtn: function() {
                $("#a_SaveDuanXin").html("保存").css({ "color": "" }).click(function() {
                    DuanXin.Save();
                    return false;
                });
            },
            BindStop: function() {
                $("#liststyle").find(".a_stopDuanXin").each(function() {
                    var tmp = {
                        action: 'set',
                        ise: $(this).closest("tr").attr("data-isenabled"),
                        Id: $(this).closest("tr").attr("data-id"),
                        iframeId: '<%= EyouSoft.Common.Utils.GetQueryStringValue("iframeId") %>'
                    };
                    $(this).click(function() {
                        $.ajax({
                            type: "post",
                            cache: false,
                            async: false,
                            url: "/jidiaoCenter/DuanxinEdit.aspx?" + $.param(tmp),
                            dataType: "json",
                            success: function(ret) {
                                if (ret.result == "1") {
                                    tableToolbar._showMsg(ret.msg, function() {
                                        DuanXin.reload();
                                    });
                                }
                                else {
                                    tableToolbar._showMsg(ret.msg);
                                }
                            },
                            error: function() { tableToolbar._showMsg(tableToolbar.errorMsg); }
                        });
                        return false;
                    });
                });
            },
            //验证手机号码
            validMobile: function(movalues) {
                var dataMobile = [];
                dataMobile = movalues.replace(/，/gi, ",").split(","); //导入时的手机号
                var b = new Array();
                var repeatM = new Array(); //重复手机号
                var errM = new Array(); //格式错误手机号
                var dataTelRight = new Array(); //正确的手机号
                //循环验证手机号是否重复和号码格式
                for (var i = 0, len = dataMobile.length; i < len; i++) {
                    //验证手机格式
                    if (!/^(13|15|18|14)\d{9}$/.test(dataMobile[i])) {
                        errM.push(dataMobile[i]); //压入格式错误的手机号
                        dataMobile[i] = null;
                        //设置当前为null
                    }
                    if (dataMobile[i] != null) {
                        if (b[dataMobile[i]] == null) {
                            b[dataMobile[i]] = i + 1;
                        }
                        else {
                            repeatM.push(dataMobile[i]); //重复号码压入重复数组
                            dataMobile[i] = null; //设置当前为null
                        }
                    }
                    if (dataMobile[i] != null) {//如果不为空则压入正确的号码数组
                        dataTelRight.push(dataMobile[i]);
                    }
                }
                var mobileMess = ""; //提示消息
                if (repeatM.length > 0)
                    mobileMess += "\n号码重复" + repeatM.toString();
                if (errM.length > 0)
                    mobileMess += "\n格式错误" + errM.toString();
                if (mobileMess != "") {
                    tableToolbar._showMsg("手机号错误提示！<br />" + mobileMess);
                    return false;
                }
                else {
                    return dataMobile; //无任何错误时写入
                }
            },
            CheckForm: function() {
                var errMess = "";
                var mvalues = $.trim($("#<%=txtMobile.ClientID %>").val());
                if (mvalues == "") {
                    errMess += "必须输入发送号码！<br />";
                }
                else {
                    if (!DuanXin.validMobile(mvalues)) {
                        return false;
                    }
                }
                if ($.trim($("#<%=txtContent.ClientID %>").val()) == "") {
                    errMess += "必须输入发送内容！<br />";
                }
                if ($.trim($("#<%=txtsetTime.ClientID %>").val()) == "") {
                    errMess += "必须选择发送时间！<br />";
                }
                if (errMess != "") {
                    tableToolbar._showMsg(errMess);
                    return false;
                }

                return true;
            },
            Save: function() {
                if (!DuanXin.CheckForm()) {
                    return false;
                }
                $("#a_SaveDuanXin").html("正在提交").css({ "color": "#999999" }).unbind("click");

                $.ajax({
                    type: "post",
                    cache: false,
                    dataType: "json",
                    url: "/jidiaoCenter/DuanxinEdit.aspx?save=1&" + $.param(DuanXin._data),
                    data: $("#a_SaveDuanXin").closest("form").serialize(),
                    success: function(ret) {
                        if (ret.result == "1") {
                            tableToolbar._showMsg(ret.msg, function() {
                                DuanXin.reload();
                            });
                        }
                        else {
                            tableToolbar._showMsg(ret.msg);
                            DuanXin.BindBtn();
                        }
                    },
                    error: function() {
                        tableToolbar._showMsg(tableToolbar.errorMsg);
                        DuanXin.BindBtn();
                    }
                });

                return false;
            }
        }
    </script>

</asp:Content>
