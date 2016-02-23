<%@ Page Title="发送短信_短信中心" Language="C#" MasterPageFile="~/MasterPage/Front.Master"
    AutoEventWireup="true" CodeBehind="SendSms.aspx.cs" Inherits="Web.SMS.SendSms" %>

<%@ Register Src="~/UserControl/SmsHeaderMenu.ascx" TagName="headerMenu" TagPrefix="uc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="mainbody">
        <uc1:headerMenu ID="cu_HeaderMenu" runat="server" TabIndex="1" />
        <form runat="server" id="SmsForm">
        <div class="btnbox" style="height: 5px;">
        </div>
        <input type="hidden" name="method" value="valid" id="method" />
        <div class="tablelist">
            <input type="hidden" name="sendAll" value="NotAll" id="sendAll" />
            <table width="100%" border="0" cellpadding="0" cellspacing="1">
                <tr>
                    <td width="14%" align="right" bgcolor="#BDDCF4">
                        <font color="#FF0000">*</font><strong>编辑发送内容：</strong>
                    </td>
                    <td width="86%" align="left" bgcolor="#e3f1fc" class="pandl4">
                        短信内容：当前短信共有 <span id="sNum">0</span> 字。移动分为 <span id="ysNum">0</span> 条发送，联通分为 <span
                            id="lsNum">0</span> 条发送，小灵通分为 <span id="xsNum">0</span> 条发送<br />
                        <textarea name="textContent" cols="70" rows="5" onkeyup="SendSms.fontNum(this);"
                            id="textContent" class="inputarea"></textarea>
                        <br />
                        <font color="#FF0000">*注：移动、联通短信内容不能超过70字!小灵通短信内容不超过45个字</font><br />
                        <a href="javascript:;" onclick="return SendSms.autoSms();">
                            <img src="/images/open-dx.gif" /><strong>自动填写发送内容 </strong></a>
                        <br />
                    </td>
                </tr>
                <tr>
                    <td align="right" valign="top" bgcolor="#BDDCF4" class="pandl4">
                        <font color="#FF0000">*</font><strong>手机号码：</strong>
                    </td>
                    <td align="left" bgcolor="#e3f1fc" class="pandl4">
                        <textarea id="txtMobiles" cols="70" rows="5" class="inputarea"></textarea>
                        <br />
                        <font color="#FF0000">*注：输入号码时在号码与号码之间必须用“，”号隔开</font><br />
                        <img src="/images/dx_goforward.gif" /><a href="javascript:;" onclick="return SendSms.putInCust();">从客户管理导入号码</a><br />
                        <img src="/images/dx_goforward.gif" /><a href="javascript:;" onclick="return SendSms.putInFile();">从文件导入号码</a><br />
                        （只能导入格式为.xls和.txt的文件，号码只能一行一个）
                    </td>
                </tr>
                <tr>
                    <td align="right" bgcolor="#BDDCF4">
                        <strong>发送方式：</strong>
                    </td>
                    <td align="left" bgcolor="#e3f1fc" class="pandl4">
                        <select name="selSendType" id="selSendType" onchange="return SendSms.selSendType()"
                            class="inputselect">
                            <option value="0" selected="selected">直接发送</option>
                            <option value="1">定时发送</option>
                        </select>
                        <input type="text" id="txtSendTime" name="txtSendTime" onfocus="WdatePicker({dateFmt:'yyyy-MM-dd HH:mm'})"
                            class="inputtext" style="display: none" />
                        定时短信不能取消
                    </td>
                </tr>
                <tr>
                    <td align="right" bgcolor="#BDDCF4">
                        <strong>选择发送通道：</strong>
                    </td>
                    <td align="left" bgcolor="#e3f1fc" class="pandl4">
                        <select name="selChannel" runat="server" id="selChannel" class="inputselect">
                        </select>
                    </td>
                </tr>
                <tr>
                    <td align="right" bgcolor="#BDDCF4">
                        <input type="checkbox" class="inputtext" id="chkSender" name="chkSender" value="hasSender"
                            onclick="return SendSms.selSender(this);" />
                        <strong>发信人：</strong>
                    </td>
                    <td align="left" bgcolor="#e3f1fc" class="pandl4">
                        <input name="txtSender" id="txtSender" type="text" class="searchinput2 inputtext"
                            size="35" />
                        打勾后短信内容将包含发信人信息（发信人要占用内容字数）
                    </td>
                </tr>
                <tr>
                    <td align="right" bgcolor="#e3f1fc">
                        <font color="#FFFFFF">&nbsp;</font>
                    </td>
                    <td align="left" bgcolor="#e3f1fc" class="pandl4">
                        <table border="0" align="left" cellpadding="0" cellspacing="0">
                            <tr>
                                <td width="134" height="40" align="center" class="jixusave">
                                    <font color="#FFFFFF">
                                        <input type="button" onclick="return SendSms.sendMess();" id="btnSend" value="提交并发送短信"
                                            style="background-image: url(/images/jixusave.gif); border: none; width: 134px;
                                            font-size: 14px; height: 31px; cursor: pointer; font-weight: bold" />
                                    </font>
                                </td>
                            </tr>
                        </table>
                        <div id="sendMess" style="color: Red; height: 40px; vertical-align: middle;">
                        </div>
                    </td>
                </tr>
                <tr>
                    <td align="right" bgcolor="#BDDCF4">
                        &nbsp;
                    </td>
                    <td align="left" bgcolor="#BDDCF4" class="pandl4">
                        <font color="#FF0000">
                            <asp:Literal ID="remainNum" runat="server" Visible="false"></asp:Literal></font><br />
                        <a href="javascript:;" onclick="return SendSms.recharge();" <%=showPay %>>
                            <img src="/images/dx-chongzhi.gif" /></a>
                    </td>
                </tr>
            </table>
        </div>
        </form>
    </div>

    <script src="/js/datepicker/WdatePicker.js" type="text/javascript"></script>

    <script type="text/javascript">
        var isLong = false;
        var SendSms = {
            //发送类别
            selSendType: function(tar) {
                if ($("#selSendType").val() == "0") {
                    $("#txtSendTime").hide();
                }
                else {
                    $("#txtSendTime").show();
                }
            },
            //切换发送通道
            selChannel: function(tar) {
                if ($(tar).find("option:selected").attr("IsLong") == "True") {
                    isLong = true;
                } else { isLong = false; }
                SendSms.fontNum(document.getElementById("textContent"));
            },
            //打开弹窗
            openDialog: function(p_url, p_title, p_width, p_height) {
                Boxy.iframeDialog({ title: p_title, iframeUrl: p_url, width: p_width, height: p_height, data: { txtMobiles: "txtMobiles"} }); return false;
            },
            //从文件导入号码
            putInFile: function() {
                return SendSms.openDialog("/SMS/ImportTelFromFile.aspx", "从文件导入号码", "800px", "500px");
            },
            //从客户导入号码
            putInCust: function() {
                return SendSms.openDialog("/SMS/CustomerDialog.aspx", "从客户导入号码", "800px", "500px");
            },
            //自动填写发送内容
            autoSms: function() {
                return SendSms.openDialog("/SMS/SmsDialog.aspx", "选择短信", "750px", "500px");
            },

            //统计字数
            fontNum: function(tar) {
                var cmsLen = $(tar).val().length; //短信字数
                var cmsLeny = Math.ceil(cmsLen / (isLong ? 210 : 70)); //移动联通条数
                var cmsLenx = Math.ceil(cmsLen / (isLong ? 210 : 45)); //小灵通条数
                $("#sNum").html(cmsLen);
                $("#ysNum,#lsNum").html(cmsLeny);
                $("#xsNum").html(cmsLenx);
            },
            //选中发信人
            selSender: function(tar) {
                if ($(tar).attr("checked")) { $("#textContent").val($("#txtSender").val() + $("#textContent").val()); }
            }, //发送按钮显示消息
            toolTip: function(step) {
                var sendBtn = $("#btnSend");
                switch (step) {
                    case 0:
                        sendBtn.val("提交并发送短信").removeAttr("disabled"); break;
                    case 1:
                        sendBtn.val("正在提交…").attr("disabled", "true"); break;
                    case 2:
                        sendBtn.val("正在发送…").attr("disabled", "true"); break;
                }
            },
            //发送短信
            sendMess: function() {
                if ($.trim($("#textContent").val()) == "") {
                    alert("请填写短信内容！"); return false;
                }
                var mobiles = SendSms.validMobile(); //验证手机号码
                if (mobiles == false) {
                    return false;
                }
                if (mobiles.length < 1) {
                    alert("请填写手机号码！");
                    return false;
                }
                if ($("#method").val() == "valid") {
                    SendSms.toolTip(1);
                }
                $.newAjax({
                    type: "post",
                    dataType: "json",
                    url: "/SMS/SendSms.aspx",
                    data: $("#<%=SmsForm.ClientID%>").serialize() + "&txtMobiles=" + mobiles.toString(),
                    cache: false,
                    success: function(result) {
                        var messtype = result.message.substring(0, 2); //获取返回的消息
                        var mess = result.message.substring(2);
                        if (result.success == "1" && messtype == "1@") {//如果是确认成功则执行发送
                            if (confirm(mess)) {//根据返回消息确认是否要发送
                                $("#method").val("send");
                                SendSms.toolTip(2); //修改按钮提示“正在发送”
                                SendSms.sendMess(); //执行发送
                            }
                            else {
                                $("#method").val("valid");
                                SendSms.toolTip(0); //恢复原始消息
                            }
                        }
                        else if (result.success == "1" && messtype == "ok") {
                            alert(mess);
                            $("#method").val("valid");
                            SendSms.toolTip(0); //恢复原始消息
                        }
                        else {

                            $("#method").val("valid");
                            SendSms.toolTip(0); //恢复原始消息
                            alert(mess);
                        }
                    },
                    error: function(XMLHttpRequest, textStatus, errorThrown) {
                        $("#method").val("valid");
                        SendSms.toolTip(0); //恢复原始消息
                        alert("发送失败！");
                    }
                });

            },
            recharge: function() {
                return SendSms.openDialog("/SMS/ReCharge.aspx", "账户充值", "620px", "350px");
            },
            //验证手机号码
            validMobile: function() {
                var dataMobile = [];
                if ($("#txtMobiles").val() != "")
                    dataMobile = $.trim($("#txtMobiles").val().replace(/，/gi, ",")).split(","); //导入时的手机号
                else
                    return dataMobile;
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
                    alert("手机号错误提示！\n" + mobileMess)
                    return false;
                }
                else {
                    return dataMobile; //无任何错误时写入
                }
            }

        }
    </script>

</asp:Content>
