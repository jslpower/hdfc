<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="HotelPriceControl.ascx.cs"
    Inherits="Web.UserControl.HotelPriceControl" %>
<table id="pricesList" width="100%" border="0" cellpadding="0" cellspacing="1" bgcolor="#FFFFFF" class="table_white">
    <tr class="odd">
        <th width="20%" height="30" align="center">
            房型
        </th>
        <th width="20%" align="center">
            前台销售价
        </th>
        <th width="20%" align="center">
            结算价
        </th>
        <th width="24%" align="center">
            含早
        </th>
        <th width="120" align="center">
            操作
        </th>
    </tr>
    <asp:PlaceHolder runat="server" ID="phDefault">
        <tr class="TrHotelRoom">
            <td height="30" align="center">
                <input name="Name" type="text" class="searchinput inputtext" value="" />
            </td>
            <td align="center">
                <input name="SellingPrice" type="text" class="searchinput inputtext" value="" />
            </td>
            <td align="center">
                <input name="AccountingPrice" type="text" class="searchinput inputtext" value="" />
            </td>
            <td align="center">
                <select class="inputselect" name="sltdinner">
                    <option value="1">是</option>
                    <option value="0">否</option>
                </select>
            </td>
            <td align="center">
                <a href="javascript:void(0);" class="addprices">
                    <img alt="" src="/images/addimg.gif" /></a> <a href="javascript:viod(0);" class="delprices">
                        <img src="/images/delimg.gif" /></a>
            </td>
        </tr>
    </asp:PlaceHolder>
    <asp:Repeater runat="server" ID="rpList">
        <ItemTemplate>
            <tr class="TrHotelRoom">
                <td height="30" align="center">
                    <input name="Name" type="text" class="searchinput inputtext" value="<%#Eval("Name") %>" />
                </td>
                <td align="center">
                    <input name="SellingPrice" type="text" class="searchinput inputtext" value="<%#EyouSoft.Common.Utils.GetDecimal(Convert.ToString(Eval("SellingPrice"))).ToString("f2") %>" />
                </td>
                <td align="center">
                    <input name="AccountingPrice" type="text" class="searchinput inputtext" value="<%#EyouSoft.Common.Utils.GetDecimal(Convert.ToString(Eval("AccountingPrice"))).ToString("f2") %>" />
                </td>
                <td align="center">
                    <select class="inputselect" name="sltdinner">
                        <option value="1" <%#Eval("IsBreakfast").ToString()=="1"?"selected='selected'":"" %>>
                            是</option>
                        <option value="0" <%#Eval("IsBreakfast").ToString()=="0"?"selected='selected'":"" %>>
                            否</option>
                    </select>
                </td>
                <td align="center">
                    <a href="javascript:void(0);" class="addprices">
                        <img alt="" src="/images/addimg.gif" alt="" /></a> <a href="javascript:viod(0);"
                            class="delprices">
                            <img src="/images/delimg.gif" alt="" /></a>
                </td>
            </tr>
        </ItemTemplate>
    </asp:Repeater>
</table>

<script type="text/javascript">
    $(function(){
        $("#pricesList").autoAdd({tempRowClass:"TrHotelRoom",addButtonClass:"addprices",delButtonClass:"delprices"})
    })
</script>

