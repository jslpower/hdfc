<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="OrderCustomer.ascx.cs"
    Inherits="Web.UserControl.OrderCustomer" %>
<div>
    <span class="formtableT">游客信息</span> <a href="javascript:void(0);" id="ToExcel"><span
        class="fbred">从EXECL导入</span></a>
    <table width="100%" cellspacing="1" cellpadding="0" border="0" align="center" id="tbl_Customer_AutoAdd">
        <tbody>
            <tr class="odd">
                <th width="36" height="30">
                    编号
                </th>
                <th>
                    姓名
                </th>
                <th>
                    性别
                </th>
                <th>
                    证件类型
                </th>
                <th>
                    证件号码
                </th>
                <th>
                    类型
                </th>
                <th>
                    联系电话
                </th>
                <th width="110">
                    操作
                </th>
            </tr>
            <asp:PlaceHolder runat="server" ID="plnAdd">
                <tr class="odd tempRow">
                    <td height="30" align="center">
                        <b class="index">1</b>
                    </td>
                    <td align="center">
                        <input type="hidden" name="hid_OrderCustomer_CustomerId" value="" />
                        <input type="text" class="formsize40 inputtext" name="txt_OrderCustomer_Name" />
                    </td>
                    <td align="center">
                        <%= GetCustomerSex(string.Empty)%>
                    </td>
                    <td align="center">
                        <%= GetCustomerCard(string.Empty)%>
                    </td>
                    <td align="center">
                        <input type="text" class="formsize120 inputtext" name="txt_OrderCustomer_CardNo">
                    </td>
                    <td align="center">
                        <%= GetCustomerType(string.Empty) %>
                    </td>
                    <td align="center">
                        <input type="text" class="formsize100 inputtext" name="txt_OrderCustomer_Tel">
                    </td>
                    <td align="center">
                        <a href="javascript:void(0)">
                            <img src="/images/addimg.gif" class="addbtn" height="20" width="48" alt="" /></a>&nbsp;
                        <a href="javascript:void(0)">
                            <img src="/images/delimg.gif" class="delbtn" height="20" width="48" alt="" /></a>
                        <br />
                    </td>
                </tr>
            </asp:PlaceHolder>
            <asp:Repeater runat="server" ID="rptCustomer">
                <ItemTemplate>
                    <tr bgcolor="<%# Container.ItemIndex % 2 == 0 ? "#E3F1FC" : "#BDDCF4" %>" class="tempRow"
                        data-ticketstatus="<%# (int)Eval("TravellerType") %>" data-isedit="1">
                        <td height="30" align="center">
                            <b class="index">
                                <%#Container.ItemIndex+1 %>
                            </b>
                        </td>
                        <td align="center">
                            <input type="hidden" name="hid_OrderCustomer_CustomerId" value="<%# Eval("TravellerId") %>" />
                            <input type="text" class="formsize40 inputtext" name="txt_OrderCustomer_Name" value="<%# Eval("TravellerName")  %>" />
                        </td>
                        <td align="center">
                            <%# GetCustomerSex((int)Eval("Sex"))%>
                        </td>
                        <td align="center">
                            <%# GetCustomerCard((int)Eval("CardType"))%>
                        </td>
                        <td align="center">
                            <input type="text" class="formsize120 inputtext" name="txt_OrderCustomer_CardNo"
                                value="<%# Eval("CardNumber") %>">
                        </td>
                        <td align="center">
                            <%# GetCustomerType((int)Eval("TravellerType"))%>
                        </td>
                        <td align="center">
                            <input type="text" class="formsize100 inputtext" name="txt_OrderCustomer_Tel" value="<%# Eval("Contact") %>">
                        </td>
                        <td align="center">
                            <a href="javascript:void(0)">
                                <img class="addbtn" src="/images/addimg.gif" height="20" width="48" alt="" /></a>&nbsp;
                            <a href="javascript:void(0)">
                                <img src="/images/delimg.gif" class="delbtn" height="20" width="48" alt="" /></a>
                            <br />
                        </td>
                    </tr>
                </ItemTemplate>
            </asp:Repeater>
        </tbody>
    </table>
</div>

<script type="text/javascript">

    var OrderCustomerControl = {
        clearTr: function() {
            $("#tbl_Customer_AutoAdd").find(".tempRow").remove();
        },
        selectDropDownList: function(cId, text) {
            var ddl = document.getElementById("ddl_" + cId);
            if (ddl.length == 0) return;
            for (var i = 0; i < ddl.length; i++) {
                if (ddl.options[i].text == text)
                    ddl.options[i].selected = true;
            }
        },
        deleteTr: function(obj) {
            var trCount = $("#tbl_Customer_AutoAdd").find("input[name='hid_OrderCustomer_CustomerId']").length;
            if (trCount > 1) {
                $(obj).closest("tr").remove();
            }
            else {
                tableToolbar._showMsg("至少要保留一行！");
            }
        },
        addCustomer: function(args) {
            if (!args) {
                args = ['', '男', '身份证', '','成人', ''];
            }
            var s1 = Math.round(Math.random() * new Date().getTime());
            var s2 = Math.round(Math.random() * new Date().getTime());
            var s3 = Math.round(Math.random() * new Date().getTime());
            var s4 = Math.round(Math.random() * new Date().getTime());
            var s5 = Math.round(Math.random() * new Date().getTime());
            var trCount = $("#tbl_Customer_AutoAdd").find("input[name='hid_OrderCustomer_CustomerId']").length;
            var trTemplate = "";

            trTemplate += '<tr class="odd tempRow">';
            //索引
            trTemplate += '<td height="30" align="center">';
            trTemplate += ('<b class="index">' + (trCount + 1) + '</b>');
            trTemplate += '</td>';
            //姓名
            trTemplate += '<td align="center">';
            trTemplate += ('<input type="hidden" name="hid_OrderCustomer_CustomerId" value="" />');
            trTemplate += ('<input type="text" class="formsize40 inputtext" name="txt_OrderCustomer_Name" value="' + args[0] + '" />');
             //性别
            trTemplate += '<td align="center">';
            trTemplate += ('<select class="inputselect" name="ddl_OrderCustomer_CustomerSex" id="ddl_' + s3 + '">  <option value="0">未知</option>  <option value="1">女</option>  <option value="2" selected="selected">男</option>  </select> ');
            trTemplate += '</td>';
            trTemplate += '</td>';
            //证件类型
            trTemplate += '<td align="center">';
            trTemplate += ('<select class="inputselect" name="ddl_OrderCustomer_CustomerCard" id="ddl_' + s2 + '">  <option value="0">未知</option>  <option value="1" selected="selected">身份证</option>  <option value="2">军官证</option>  <option value="3">台胞证</option>  <option value="4">港澳通行证</option>  <option value="5">户口本</option>  </select> ');
            trTemplate += '</td>';
            //证件号码
            trTemplate += '<td align="center">';
            trTemplate += ('<input type="text" class="formsize120 inputtext" name="txt_OrderCustomer_CardNo" value="' + args[3] + '" />');
            trTemplate += '</td>';
            //类型
            trTemplate += '<td align="center">';
            trTemplate += ('<select class="inputselect" name="ddl_OrderCustomer_CustomerType" id="ddl_' + s1 + '">  <option value="0">儿童</option>  <option value="1" selected="selected">成人</option>  <option value="2">军残</option>  </select> ');
            trTemplate += '</td>';
            //电话
            trTemplate += '<td align="center">';
            trTemplate += ('<input type="text" class="formsize100 inputtext" name="txt_OrderCustomer_Tel" value="' + args[5] + '" />');
            trTemplate += '</td>';
            //操作
            trTemplate += '<td align="center">';
            trTemplate += '<a href="javascript:void(0);" id="a_' + s4 + '"><img src="/images/addimg.gif" class="addbtn" height="20" width="48"></a>&nbsp;&nbsp;<a href="javascript:void(0);" id="a_' + s5 + '"><img src="/images/delimg.gif" class="delbtn" height="20" width="48"></a><br>';
            trTemplate += '</td>';
            trTemplate += '</tr>';

            $("#tbl_Customer_AutoAdd").append(trTemplate);
            OrderCustomerControl.selectDropDownList(s1, args[1]);
            OrderCustomerControl.selectDropDownList(s2, args[2]);
            OrderCustomerControl.selectDropDownList(s3, args[4]);
            $("#a_" + s4).click(function() {
                OrderCustomerControl.addCustomer(null);
                return false;
            });
            $("#a_" + s5).click(function() {
                OrderCustomerControl.deleteTr(this);
                return false;
            });
        }
    };

    $(document).ready(function() {
        $("#tbl_Customer_AutoAdd").find(".addbtn").bind("click", function() {
            OrderCustomerControl.addCustomer();
            return false;
        });
        $("#tbl_Customer_AutoAdd").find(".delbtn").bind("click", function() {
            OrderCustomerControl.deleteTr(this);
            return false;
        });


        $("#ToExcel").bind("click", function() {
            parent.Boxy.iframeDialog({
                iframeUrl: "/CommonPage/LoadVisitors.aspx",
                width: "853px",
                height: "514px",
                async: false,
                title: "导入游客信息",
                modal: true,
                data: {
                    topId: '<%= EyouSoft.Common.Utils.GetQueryStringValue("iframeId") %>',
                    type: "customerLoad"
                }
            });
            return false;
        });
    });
</script>

