<%@ Page Language="C#" MasterPageFile="~/MasterPage/Boxy.Master" AutoEventWireup="true"
    CodeBehind="DijieEdit.aspx.cs" Inherits="Web.jidiaoCenter.DijieEdit" Title="无标题页" %>

<%@ Register Src="../UserControl/UploadControl.ascx" TagName="UploadControl" TagPrefix="uc1" %>
<%@ Register Src="../UserControl/SupperControl.ascx" TagName="SupperControl" TagPrefix="uc2" %>
<%@ Register Src="../UserControl/TourSelect.ascx" TagName="TourSelect" TagPrefix="uc3" %>
<asp:Content ID="Content1" ContentPlaceHolderID="PageHead" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="PageBody" runat="server">
    <form id="form1" runat="server">
    <div style="width: 680px; margin: 10px auto;">
        <table width="100%" cellspacing="1" cellpadding="0" border="0" align="center">
            <tbody>
                <tr class="odd">
                    <th height="30" align="right">
                        <span class="fred">*</span>团号：
                    </th>
                    <td bgcolor="#E3F1FC">
                        <uc3:TourSelect ID="TourSelect1" runat="server" SupplierType="地接" />
                    </td>
                    <th align="right">
                        <span class="fred">*</span>地接社：
                    </th>
                    <td bgcolor="#E3F1FC">
                        <uc2:SupperControl ID="SupperControl1" runat="server" SupplierType="地接" IsMust="true" />
                    </td>
                </tr>
                <tr class="odd">
                    <th width="120" height="30" align="right">
                        房：
                    </th>
                    <td bgcolor="#E3F1FC">
                        <asp:TextBox ID="txthouse" valid="isMoney" errmsg="价格格式有误" runat="server" CssClass="formsize50 inputtext"></asp:TextBox>
                    </td>
                    <th width="120" align="right">
                        餐：
                    </th>
                    <td bgcolor="#E3F1FC">
                        <asp:TextBox ID="txtdinner" valid="isMoney" errmsg="价格格式有误" runat="server" CssClass="formsize50 inputtext"></asp:TextBox>
                    </td>
                </tr>
                <tr class="odd">
                    <th height="30" align="right">
                        车：
                    </th>
                    <td bgcolor="#E3F1FC">
                        <asp:TextBox ID="txtcar" valid="isMoney" errmsg="价格格式有误" runat="server" CssClass="formsize50 inputtext"></asp:TextBox>
                    </td>
                    <th align="right">
                        门：
                    </th>
                    <td bgcolor="#E3F1FC">
                        <asp:TextBox ID="txtdoor" valid="isMoney" errmsg="价格格式有误" runat="server" CssClass="formsize50 inputtext"></asp:TextBox>
                    </td>
                </tr>
                <tr class="odd">
                    <th height="30" align="right">
                        导：
                    </th>
                    <td bgcolor="#E3F1FC">
                        <asp:TextBox ID="txtdao" valid="isMoney" errmsg="价格格式有误" runat="server" CssClass="formsize50 inputtext"></asp:TextBox>
                    </td>
                    <th align="right">
                        大交通：
                    </th>
                    <td bgcolor="#E3F1FC">
                        <asp:TextBox ID="txttraffic" valid="isMoney" errmsg="价格格式有误" runat="server" CssClass="formsize50 inputtext"></asp:TextBox>
                    </td>
                </tr>
                <tr class="odd">
                    <th height="30" align="right">
                       <span class="fred"> 购物提成人头：</span>
                    </th>
                    <td bgcolor="#E3F1FC">
                        <asp:TextBox ID="txtcount" valid="isMoney" errmsg="价格格式有误" runat="server" CssClass="formsize50 inputtext"></asp:TextBox>
                    </td>
                    <th align="right">
                        加点费用：
                    </th>
                    <td bgcolor="#E3F1FC">
                        <asp:TextBox ID="txtjiadian" valid="isMoney" errmsg="价格格式有误" runat="server" CssClass="formsize50 inputtext"></asp:TextBox>
                    </td>
                </tr>
                <tr class="odd">
                    <th height="30" align="right">
                        <span class="fred">导游现收：</span>
                    </th>
                    <td bgcolor="#E3F1FC">
                        <asp:TextBox ID="txtxianshou" valid="isMoney" errmsg="价格格式有误" runat="server" CssClass="formsize50 inputtext"></asp:TextBox>
                    </td>
                    <th align="right">
                        导游现付：
                    </th>
                    <td bgcolor="#E3F1FC">
                        <asp:TextBox ID="txtxianfu" valid="isMoney" errmsg="价格格式有误" runat="server" CssClass="formsize50 inputtext"></asp:TextBox>
                    </td>
                </tr>
                <tr class="odd">
                    <th height="30" align="right">
                        其他：
                    </th>
                    <td bgcolor="#E3F1FC">
                        <asp:TextBox ID="txtOther" valid="isMoney" errmsg="价格格式有误" runat="server" CssClass="formsize50 inputtext"></asp:TextBox>
                    </td>
                    <th align="right">
                        支付方式：
                    </th>
                    <td bgcolor="#E3F1FC">
                        <select id="sltpaytype" name="sltpaytype" class="inputselect">
                            <%=PayTypeOption %>
                        </select>
                    </td>
                </tr>
                <tr class="odd">
                    <th height="30" align="right">
                        <span class="fred">*</span>合计金额：
                    </th>
                    <td bgcolor="#E3F1FC" colspan="3">
                        <asp:TextBox ID="txtTotalMoney" valid="isNumber|required" errmsg="价格格式有误|合计金额不能为空"
                            runat="server" CssClass="formsize50 inputtext"></asp:TextBox>
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
                        附件上传：
                    </th>
                    <td bgcolor="#E3F1FC" colspan="3">
                        <uc1:UploadControl ID="UploadControl1" runat="server" IsUploadSelf="true" IsUploadMore="true" />
                        <div style="width: 450px; float: left; margin-left: 5px;">
                            <asp:Repeater ID="rplfile" runat="server">
                                <ItemTemplate>
                                    <a target="_blank" href='<%#Eval("FilePath") %>' style="vertical-align: bottom">
                                        <%#Eval("FileName")%></a><span><img alt="" style="vertical-align: bottom; cursor: pointer;"
                                            src="/images/cha.gif" data-delimg="delimg" onclick="DijieEditPage.RemoveFile(this)" /><input
                                                type="hidden" data-id='<%#Eval("Id") %>' name="hidefile" value="<%#Eval("FilePath") %>" /></span>
                                </ItemTemplate>
                            </asp:Repeater>
                        </div>
                    </td>
                </tr>
                <tr class="odd">
                    <th align="right">
                        备注：
                    </th>
                    <td bgcolor="#E3F1FC" colspan="3">
                        <asp:TextBox ID="txtRemark" TextMode="MultiLine" Height="70px" runat="server" CssClass="formsize450 inputtext"></asp:TextBox>
                    </td>
                </tr>
            </tbody>
        </table>
        <asp:PlaceHolder runat="server" ID="PhBtn">
            <table width="100%" cellspacing="0" cellpadding="0" border="0" align="center" style="margin: 10px auto;">
                <tbody>
                    <tr class="odd">
                        <td height="40" bgcolor="#E3F1FC" colspan="14">
                            <table cellspacing="0" cellpadding="0" border="0" align="center">
                                <tbody>
                                    <tr>
                                        <asp:PlaceHolder runat="server" ID="phshenqing">
                                            <td width="80" align="center" class="tjbtn02" style="padding-right:50px;">
                                                <a class="btn" data-id="add" href="javascript:;">申请</a>
                                            </td>
                                        </asp:PlaceHolder>
                                        <asp:PlaceHolder runat="server" ID="phqueren">
                                            <td width="80" align="center" class="tjbtn02" style="padding-right:50px;">
                                                <a class="btn" data-id="queren" href="javascript:;">结算确认</a>
                                            </td>
                                        </asp:PlaceHolder>
                                        <asp:PlaceHolder runat="server" ID="PhCheck">
                                            <td width="80" align="center" class="tjbtn02" style="padding-right:50px;">
                                                <a class="btn" data-id="check" href="javascript:;">审核</a>
                                            </td>
                                        </asp:PlaceHolder>
                                        <asp:PlaceHolder runat="server" ID="PhCencer">
                                            <td width="80" align="center" class="tjbtn02">
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
    </form>

    <script type="text/javascript">
        $(function(){
            DijieEditPage.PageInit();
            DijieEditPage.BindBtn();
        })
        var DijieEditPage={
            PageInit:function(){
                 $("#<%=txthouse.ClientID %>,#<%=txtdinner.ClientID %>,#<%=txtcar.ClientID %>,#<%=txtdoor.ClientID %>,#<%=txtdao.ClientID %>,#<%=txttraffic.ClientID %>,#<%=txtOther.ClientID %>,#<%=txtcount.ClientID %>,#<%=txtxianshou.ClientID %>,#<%=txtxianfu.ClientID %>,#<%=txtjiadian.ClientID %>").keyup(function(){
                    DijieEditPage.AutoSumPrice();
                 })
                  $("#<%=ddlyuejie.ClientID %>").change(function(){
                    if($(this).val()=="1"){
                        $("#<%=txtjiesuantime.ClientID %>").attr("valid","required").attr("errmsg","请填写结算时间");
                        $(this).next("span").show();
                    }
                    else{
                        $("#<%=txtjiesuantime.ClientID %>").removeAttr("valid").removeAttr("errmsg");
                        $(this).next("span").hide();
                    }
                })
                  
            },
            BindBtn:function(){
                $("#sltpaytype").change(function(){
                    $("#<%=hdpaytype.ClientID %>").val($(this).val());
                })
                $(".btn").click(function(){
                    var type=$(this).attr("data-id");
                    var url="/jidiaoCenter/DijieEdit.aspx?type=save&dotype="+type+"&id="+'<%=Request.QueryString["id"] %>';
                    if(!DijieEditPage.CheckForm(this)){
                        return ;
                    }
                    $(this).html("提交中");
                    DijieEditPage.GoAjax(url,$(this));
                })
            },
            GoAjax: function(url,obj) {
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
                        tableToolbar._showMsg(tableToolbar.errorMsg);
                    }
                });
            },
            CheckForm: function(obj) {
                return ValiDatorForm.validator($("#<%=form1.ClientID %>"), "parent");
            },
            AutoSumPrice:function(){
                    var fang=tableToolbar.getFloat($("#<%=txthouse.ClientID %>").val());
                    var dinner=tableToolbar.getFloat($("#<%=txtdinner.ClientID %>").val());
                    var car=tableToolbar.getFloat($("#<%=txtcar.ClientID %>").val());
                    var door=tableToolbar.getFloat($("#<%=txtdoor.ClientID %>").val());
                    var dao=tableToolbar.getFloat($("#<%=txtdao.ClientID %>").val());
                    var traffic=tableToolbar.getFloat($("#<%=txttraffic.ClientID %>").val());
                    var other=tableToolbar.getFloat($("#<%=txtOther.ClientID %>").val());
                    var count=tableToolbar.getFloat($("#<%=txtcount.ClientID %>").val());
                    var xianshou=tableToolbar.getFloat($("#<%=txtxianshou.ClientID %>").val());
                    var xianfu=tableToolbar.getFloat($("#<%=txtxianfu.ClientID %>").val());
                    var jiadian=tableToolbar.getFloat($("#<%=txtjiadian.ClientID %>").val());
                    var totalMoney=fang+dinner+car+door+dao+other+traffic-count-xianshou+xianfu+jiadian;
                    $("#<%=txtTotalMoney.ClientID %>").val(totalMoney);
            },
            RemoveFile: function(obj) {
                $(obj).hide();
                var thisimg = $(obj).parent();
                thisimg.prev("a").hide();
                fileid+=thisimg.find("input[type='hidden']").attr("data-id")+",";
                $("#<%=hfileId.ClientID %>").val(fileid);
                thisimg.find("input[type='hidden']").val("");
                thisimg.hide();
                return false;
            }
        }
    </script>

</asp:Content>
