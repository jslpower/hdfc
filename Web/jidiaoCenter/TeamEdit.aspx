<%@ Page Language="C#" MasterPageFile="~/MasterPage/Boxy.Master" AutoEventWireup="true"
    CodeBehind="TeamEdit.aspx.cs" Inherits="Web.jidiaoCenter.TeamEdit" Title="新增登记" %>

<%@ Register Src="../UserControl/UploadControl.ascx" TagName="UploadControl" TagPrefix="uc1" %>
<%@ Register Src="../UserControl/SellsSelect.ascx" TagName="SellsSelect" TagPrefix="uc2" %>
<%@ Register Src="../UserControl/DaoyouControl.ascx" TagName="DaoyouControl" TagPrefix="uc3" %>
<%@ Register Src="../UserControl/DijiesheControl.ascx" TagName="DijiesheControl"
    TagPrefix="uc4" %>
<%@ Register Src="../UserControl/CustomerUnit.ascx" TagName="CustomerUnit" TagPrefix="uc5" %>
<%@ Register Src="../UserControl/RouteSelect.ascx" TagName="RouteSelect" TagPrefix="uc6" %>
<asp:Content ID="Content1" ContentPlaceHolderID="PageHead" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="PageBody" runat="server">

    <script src="/JS/Newjquery.autocomplete.js" type="text/javascript"></script>

    <form id="form1" runat="server">
    <div style="width: 940px; margin: 10px auto;">
        <table width="100%" cellspacing="1" cellpadding="0" border="0" align="center">
            <tbody>
                <tr class="odd">
                    <th width="90" height="30" align="right">
                        <span class="fred">*</span>团/散：
                    </th>
                    <td bgcolor="#E3F1FC" colspan="3">
                        <asp:DropDownList ID="ddltype" runat="server" CssClass="inputselect">
                            <asp:ListItem Value="0">团</asp:ListItem>
                            <asp:ListItem Value="1">散</asp:ListItem>
                        </asp:DropDownList>
                    </td>
                </tr>
                <tr class="odd">
                    <th height="30" align="right">
                        <span class="fred">*</span>出团日期：
                    </th>
                    <td bgcolor="#E3F1FC">
                        <asp:TextBox ID="txtLeaveDate" name="txtLeaveDate" class="formsize80 inputtext" onfocus="WdatePicker()"
                            runat="server" errmsg="请选择出团日期!" valid="required"></asp:TextBox>
                    </td>
                    <th align="right">
                        <span class="fred">*</span>回团日期：
                    </th>
                    <td bgcolor="#E3F1FC">
                        <asp:TextBox ID="txtBackDate" name="txtBackDate" class="formsize80 inputtext" onfocus="WdatePicker({minDate:'#F{$dp.$D(\'ctl00_PageBody_txtLeaveDate\')}'})"
                            runat="server" errmsg="请选择回团日期!" valid="required"></asp:TextBox>
                    </td>
                </tr>
                <tr class="odd">
                    <th height="30" align="right">
                        团号：
                    </th>
                    <td bgcolor="#E3F1FC">
                        <asp:TextBox ID="txtTourcode" runat="server" class="formsize120 inputtext"></asp:TextBox>
                    </td>
                    <th align="right">
                        <span class="fred">*</span>线路名称：
                    </th>
                    <td bgcolor="#E3F1FC">
                        <uc6:RouteSelect ID="RouteSelect1" runat="server" IsShowTitle="false" IsMoreSelect="false"
                            IsMust="true" />
                        &nbsp;是否添加到线路库:<asp:CheckBox ID="CheckIsRoute" runat="server" />
                    </td>
                </tr>
                <tr class="odd">
                    <th height="30" align="right">
                        <span class="fred">*</span>人数：
                    </th>
                    <td bgcolor="#E3F1FC">
                        成人
                        <asp:TextBox ID="txtAdultCount" min="0" runat="server" valid="isInt|range" errmsg="请输入正确的成人数!|成人数必须大于0!"
                            class="formsize40 inputtext"></asp:TextBox>
                        儿童
                        <asp:TextBox ID="txtChildCount" min="0" runat="server" valid="isInt|range" errmsg="请输入正确的儿童数!|儿童数必须大于0!"
                            class="formsize40 inputtext"></asp:TextBox>
                        全陪
                        <asp:TextBox ID="txtQuanCount" min="0" runat="server" valid="isInt|range" errmsg="请输入正确的全陪数!|全陪数必须大于0!"
                            class="formsize40 inputtext"></asp:TextBox>
                    </td>
                    <th align="right">
                        <span class="fred">*</span>销售员：
                    </th>
                    <td bgcolor="#E3F1FC">
                        <uc2:SellsSelect ID="SellsSelect1" runat="server" ReadOnly="true" />
                    </td>
                </tr>
                <tr class="odd">
                    <th height="30" align="right">
                        <span class="fred">*</span>组团社：
                    </th>
                    <td bgcolor="#E3F1FC" colspan="3">
                        <uc5:CustomerUnit ID="CustomerUnit1" runat="server" IsMoreSelect="false" IsRequired="true" />
                    </td>
                </tr>
                <tr class="odd">
                    <th align="right">
                        地接导游：
                    </th>
                    <td bgcolor="#E3F1FC" class="pandl4" colspan="3">
                        <uc3:DaoyouControl ID="DaoyouControl1" runat="server" />
                    </td>
                </tr>
                <tr class="odd">
                    <th height="30" align="right">
                        附件：
                    </th>
                    <td bgcolor="#E3F1FC" colspan="3">
                        <uc1:UploadControl ID="UploadControl1" runat="server" IsUploadMore="true" IsUploadSelf="true" />
                        <div style="width: 450px; float: left; margin-left: 5px;">
                            <asp:Repeater ID="rplfile" runat="server">
                                <ItemTemplate>
                                    <a target="_blank" href='<%#Eval("FilePath") %>' style="vertical-align: bottom">
                                        <%#Eval("FileName")%></a><span><img alt="" style="vertical-align: bottom; cursor: pointer;"
                                            src="/images/cha.gif" data-delimg="delimg" onclick="TeamEditPage.RemoveFile(this)" /><input
                                                type="hidden" data-id='<%#Eval("Id") %>' name="hidefile" value="<%#Eval("FilePath") %>" /></span>
                                </ItemTemplate>
                            </asp:Repeater>
                        </div>
                    </td>
                </tr>
                <tr class="odd">
                    <th height="30" align="right">
                        月结：
                    </th>
                    <td bgcolor="#E3F1FC" class="pandl4">
                        <asp:DropDownList ID="ddlyuejie" runat="server" CssClass="inputselect">
                            <asp:ListItem Value="1">是</asp:ListItem>
                            <asp:ListItem Value="0">否</asp:ListItem>
                        </asp:DropDownList>
                        <span runat="server" id="spanjisuantime"><span class="fred">*</span>结算时间：
                            <asp:TextBox ID="txtjiesuantime" valid="required" errmsg="请填写结算时间" onfocus="WdatePicker()"
                                runat="server" CssClass="formsize80 inputtext"></asp:TextBox></span>
                    </td>
                    <th align="right" class="pandl4">
                        出票：
                    </th>
                    <td bgcolor="#E3F1FC" class="pandl4">
                        <asp:DropDownList ID="ddlchupiao" runat="server" CssClass="inputselect">
                            <asp:ListItem Value="0">否</asp:ListItem>
                            <asp:ListItem Value="1">是</asp:ListItem>
                        </asp:DropDownList>
                    </td>
                </tr>
                <asp:PlaceHolder runat="server" ID="Phfin">
                    <tr class="odd">
                        <asp:PlaceHolder runat="server" ID="PhFanyong">
                            <th height="30" align="right">
                                返佣：
                            </th>
                            <td bgcolor="#E3F1FC" class="pandl4">
                                人数
                                <asp:TextBox ID="txtcount" CssClass="formsize40 inputtext" runat="server"></asp:TextBox>
                                金额
                                <asp:TextBox ID="txtmoney" CssClass="formsize40 inputtext" runat="server"></asp:TextBox>
                                元/人
                            </td>
                        </asp:PlaceHolder>
                        <asp:PlaceHolder runat="server" ID="PhMaoli">
                            <th align="right" class="pandl4">
                                毛利：
                            </th>
                            <td bgcolor="#E3F1FC" class="pandl4">
                                <asp:TextBox ID="txtprofit" CssClass="formsize40 inputtext" runat="server"></asp:TextBox>
                            </td>
                        </asp:PlaceHolder>
                    </tr>
                </asp:PlaceHolder>
                <tr class="odd">
                    <th height="30" align="right">
                        <span class="fred">*</span>合计金额：
                    </th>
                    <td bgcolor="#E3F1FC" class="pandl4" colspan="3">
                        <asp:TextBox ID="txtTotalMoney" runat="server" valid="isNumber|required" errmsg="价格格式有误|合计金额不能为空"
                            CssClass="formsize70 inputtext" Text="1"></asp:TextBox>
                    </td>
                </tr>
                <tr class="odd">
                    <th height="100" align="right">
                        备注
                    </th>
                    <td bgcolor="#E3F1FC" class="pandl4" colspan="3">
                        <asp:TextBox runat="server" ID="txtReamrk" CssClass="formsize450 inputtext" TextMode="MultiLine"
                            Height="90px"></asp:TextBox>
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
                                    <td width="80" align="center" class="tjbtn02" style="padding-right:50px">
                                        <a href="javascript:;" id="btn" runat="server">保存</a>
                                    </td>
                                    <asp:PlaceHolder runat="server" ID="phcheck">
                                        <td width="80" align="center" class="tjbtn02" style="padding-right:50px">
                                            <a href="javascript:;" id="check">审核</a>
                                        </td>
                                    </asp:PlaceHolder>
                                    <asp:PlaceHolder runat="server" ID="phcencer">
                                        <td width="80" align="center" class="tjbtn02" style="padding-right:50px">
                                            <a href="javascript:;" id="cencer">取消审核</a>
                                        </td>
                                    </asp:PlaceHolder>
                                    <asp:PlaceHolder runat="server" ID="PhEnd">
                                        <td width="80" align="center" class="tjbtn02" style="padding-right:50px">
                                            <a href="javascript:;" id="btn_end">操作结束</a>
                                        </td>
                                    </asp:PlaceHolder>
                                    <asp:PlaceHolder runat="server" ID="PhBack">
                                        <td width="80" align="center" class="tjbtn02">
                                            <a href="javascript:;" id="btn_back">退回计调</a>
                                        </td>
                                    </asp:PlaceHolder>
                                </tr>
                            </tbody>
                        </table>
                    </td>
                </tr>
            </tbody>
        </table>
    </div>
    <input type="hidden" id="hfileId" runat="server" value="" />
    </form>

    <script type="text/javascript">
        $(function(){
            TeamEditPage.PageInit();
        })
        var TeamEditPage={
            PageInit:function(){
                $("#<%=btn.ClientID %>").click(function(){
                    if(!TeamEditPage.CheckForm()){
                        return false;
                    }
                    var adultcount=tableToolbar.getInt($("#<%=txtAdultCount.ClientID %>").val());
                    var childcount=tableToolbar.getInt($("#<%=txtChildCount.ClientID %>").val());
                    var quanpeicount=tableToolbar.getInt($("#<%=txtQuanCount.ClientID %>").val());
                    if(adultcount+childcount+quanpeicount<1){
                        parent.tableToolbar._showMsg("请填写人数!");
                        return false;
                    }
                    $(this).html("提交中");
                    var url="/jidiaoCenter/TeamEdit.aspx?dotype="+'<%=Request.QueryString["dotype"]%>'+"&type=save&tourid="+'<%=Request.QueryString["tourid"]%>'+"&isfin="+'<%=Request.QueryString["isfin"]%>';
                    TeamEditPage.GoAjax(url,$(this));
                    return false;
                })
                $("#btn_end").click(function(){
                    var url="/jidiaoCenter/TeamEdit.aspx?dotype=end&type=save&tourid="+'<%=Request.QueryString["tourid"]%>';
                     $(this).html("提交中");
                    TeamEditPage.GoAjax(url,$(this));
                    return false;
                })
                 $("#btn_back").click(function(){
                    var url="/jidiaoCenter/TeamEdit.aspx?dotype=back&type=save&tourid="+'<%=Request.QueryString["tourid"]%>';
                     $(this).html("提交中");
                    TeamEditPage.GoAjax(url,$(this));
                    return false;
                })
                $("#check").click(function(){
                    var url="/jidiaoCenter/TeamEdit.aspx?dotype="+'<%=Request.QueryString["dotype"]%>'+"&type=save&tourid="+'<%=Request.QueryString["tourid"]%>'+"&isfin="+'<%=Request.QueryString["isfin"]%>';
                     $(this).html("提交中");
                    TeamEditPage.GoAjax(url,$(this));
                    return false;
                })
                 $("#cencer").click(function(){
                    var url="/jidiaoCenter/TeamEdit.aspx?dotype=cencer&type=save&tourid="+'<%=Request.QueryString["tourid"]%>';
                     $(this).html("提交中");
                    TeamEditPage.GoAjax(url,$(this));
                    return false;
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
        CheckForm: function() {
            return ValiDatorForm.validator($("#<%=form1.ClientID %>"), "parent");
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
