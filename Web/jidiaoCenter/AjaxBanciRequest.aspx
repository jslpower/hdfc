<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="AjaxBanciRequest.aspx.cs"
    Inherits="Web.jidiaoCenter.AjaxBanciRequest" %>

<style type="text/css">
    .tablelist
    {
        border-collapse: collapse;
    }
    .tablelist td
    {
        border: 1px #b8c5ce solid;
        padding: 0 3px;
        height: 30px;
    }
</style>
<asp:placeholder runat="server" id="phdatalist">
            
                <table width="100%" cellspacing="0" cellpadding="0" bgcolor="#FFFFFF" align="center"
                    id="tblList" style="margin: 0 auto" class="tablelist">
                    <tr class="">
                        <asp:Repeater runat="server" ID="RptDataList">
                            <ItemTemplate>
                                <td align="left" height="30px" width="25%">
                                    <label>
                                        <input name="1" type="radio" value="<%#Eval("Id") %>" data-tickettype='<%#(int)((EyouSoft.Model.EnumType.PlanStructure.TicketType)Eval("TicketType")) %>'
                                            data-otherprice='<%#EyouSoft.Common.Utils.GetDecimal(Convert.ToString(Eval("OtherPrice"))).ToString("f2") %>'
                                            data-qujian='<%#Eval("Interval") %>' data-yongjin='<%#EyouSoft.Common.Utils.GetDecimal(Convert.ToString(Eval("Brokerage"))).ToString("f2") %>' data-show="<%#Eval("TrafficNumber")%>"
                                            data-seat='<%#(int)Eval("_TrafficSeat") %>' data-starttime='<%#EyouSoft.Common.UtilsCommons.GetDateString(Eval("TrafficTime"),"yyyy-MM-dd HH:mm") %>' />
                                        <span>
                                            <%#Eval("TrafficNumber")%></span>
                                    </label>
                                    <%#EyouSoft.Common.Utils.IsOutTrOrTd(Container.ItemIndex, listcount, 4)%>
                            </ItemTemplate>
                        </asp:Repeater>
                        <asp:Literal runat="server" ID="lbemptymsg"></asp:Literal>
                </table>
        </asp:placeholder>
<form runat="server" id="form1">
<asp:placeholder runat="server" id="phadd">

            <table width="100%" border="0" cellpadding="0" cellspacing="1" id="Tabform" runat="server" class="tablelist">
                <tr class="even">
                    <th>
                        <font class="fred">*</font> 班次名称
                    </th>
                    <th>
                        <font class="fred">*</font> 票务类别
                    </th>
                    <th>
                       <font class="fred">*</font>  发班时间
                    </th>
                    <th>
                        <font class="fred">*</font> 区间
                    </th>
                    <th>
                        手续费/机燃
                    </th>
                    <th>
                        佣金
                    </th>
                    <th>
                        席别
                    </th>
                </tr>
                <tr>
                    <td style="text-align: center;">
                        <asp:TextBox runat="server" class="inputtext formsize100" ID="txtbanciname" valid="required"
                            errmsg="请填写班次名称"></asp:TextBox>
                    </td>
                    <td style="text-align: center">
                        <asp:DropDownList ID="ddltickettype" name="ddltickettype" valid="required" errmsg="请选择票务类别"
                            runat="server" CssClass="inputselect">
                            <asp:ListItem Value="0">机票</asp:ListItem>
                            <asp:ListItem Value="1">火车票</asp:ListItem>
                        </asp:DropDownList>
                    </td>
                    <td style="text-align: center; width:230px;">
                        <asp:TextBox runat="server" onfocus="WdatePicker()" class="inputtext formsize80" valid="required" errmsg="请填发班时间"
                            ID="txtstarttime"></asp:TextBox>
                            <select id="sltHH" name="sltHH" class="inputselect" errmsg="请填写开车/起飞时间(小时)!" valid="required">
                        <%=StrHH %>
                        </select>
                        时
                        <select id="sltmm" name="sltmm" class="inputselect" errmsg="请填写开车/起飞时间(分钟)!" valid="required">
                        <%=Strmm %>
                        </select>
                        分
                    </td>
                    <td style="text-align: center">
                        <asp:TextBox runat="server" class="inputtext formsize100" ID="txtqujian" errmsg="请填写区间!" valid="required"></asp:TextBox>
                    </td>
                    <td style="text-align: center">
                        <asp:TextBox runat="server" class="inputtext formsize100" ID="txtoherprice"></asp:TextBox>
                    </td>
                    <td style="text-align: center">
                        <asp:TextBox runat="server" class="inputtext formsize100" ID="txtyongjin"></asp:TextBox>
                    </td>
                    <td style="text-align: center; display:none;">
                        <asp:TextBox runat="server" class="inputtext formsize100" ID="txtseat"  ></asp:TextBox>
                    </td>
                    <td style="text-align: center">
                        <asp:DropDownList  runat="server" ID="ddlseat" class="inputselect"></asp:DropDownList>
                    </td>
                </tr>
            </table>
            <table width="100%" cellspacing="0" cellpadding="0" border="0" align="center" style="margin: 10px auto;"
                id="tab_save">
                <tr class="odd">
                    <td height="30" bgcolor="#E3F1FC" align="left" colspan="14">
                        <table cellspacing="0" cellpadding="0" border="0" align="center">
                            <tbody>
                                <tr>
                                    <td width="80" height="40" align="center" class="tjbtn02">
                                        <a href="javascript:;" id="btn_save">保存</a>
                                    </td>
                                    <td width="80" height="40" align="center" class="tjbtn02">
                                        <a href="javascript:;" id="btnxuan">保存并选用</a>
                                    </td>
                                </tr>
                            </tbody>
                        </table>
                    </td>
                </tr>
            </table>
            
        </asp:placeholder>
</form>

<script type="text/javascript">
    $(function() {
        var datacount = '<%=listcount %>';
        if (datacount > 0) {
            $("#<%=phdatalist.ClientID %>").show();
            $("#<%=phadd.ClientID %>").hide();
            $("#TabBtn").show();
        } else {
            $("#<%=phdatalist.ClientID %>").hide();
            $("#<%=phadd.ClientID %>").show();
            $("#TabBtn").hide();
        }
    })
</script>

