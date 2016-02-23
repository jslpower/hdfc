<%@ Page Title="组团社统计" Language="C#" MasterPageFile="~/MasterPage/Front.Master" AutoEventWireup="true"
    CodeBehind="Customer.aspx.cs" Inherits="Web.TongJi.Customer" %>

<%@ Register Assembly="ControlLibrary" Namespace="Adpost.Common.ExporPage" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="lineprotitlebox">
        <table width="100%" border="0" cellspacing="0" cellpadding="0">
            <tr>
                <td width="15%" nowrap="nowrap">
                    <span class="lineprotitle">组团社统计</span>
                </td>
                <td width="85%" nowrap="nowrap" align="right" style="padding: 0 10px 2px 0; color: #13509f;">
                    <b>当前您所在位置</b> >> 统计中心 >> 组团社统计
                </td>
            </tr>
            <tr>
                <td colspan="2" height="2" bgcolor="#000000">
                </td>
            </tr>
        </table>
    </div>
    <div class="hr_10">
    </div>
    <div class="tablelist" id="con_two_2">
        <form id="form1" action="" method="get">
        <table width="99%" border="0" align="center" cellpadding="0" cellspacing="0">
            <tr>
                <td width="10" valign="top">
                    <img src="/images/yuanleft.gif" />
                </td>
                <td>
                    <div class="searchbox">
                        省份：
                        <select id="pid" name="pid" class="inputselect">
                        </select>
                        城市：
                        <select id="cid" name="cid" class="inputselect">
                        </select>
                        时间：
                        <input name="st" type="text" class="searchinput inputtext" id="st" onfocus="WdatePicker()" />
                        -
                        <input name="et" type="text" class="searchinput inputtext" id="et" onfocus="WdatePicker()" />
                        <input type="image" src="/images/searchbtn.gif" style="vertical-align: top;" />
                    </div>
                </td>
                <td width="10" valign="top">
                    <img src="/images/yuanright.gif" />
                </td>
            </tr>
        </table>
        </form>
        <div class="hr_10">
        </div>
        <table width="100%" border="0" cellpadding="0" cellspacing="1" id="liststyle">
            <tr>
                <th width="36" height="30" align="center" bgcolor="#BDDCF4">
                    序号
                </th>
                <th align="center" bgcolor="#bddcf4">
                    所在地
                </th>
                <th align="center" bgcolor="#bddcf4">
                    公司名称
                </th>
                <th align="center" bgcolor="#bddcf4">
                    联系人
                </th>
                <th align="center" bgcolor="#bddcf4">
                    电话
                </th>
                <th align="center" bgcolor="#bddcf4">
                    手机
                </th>
                <th align="center" bgcolor="#bddcf4">
                    <a href="javascript:void(0);" class="a_Orderby" data-orderindex="2"><b>↑</b></a>交易人数<a
                        href="javascript:void(0);" class="a_Orderby" data-orderindex="3"><b>↓</b></a>
                </th>
                <th align="center" bgcolor="#bddcf4">
                    <a href="javascript:void(0);" class="a_Orderby" data-orderindex="4"><b>↑</b></a>交易次数<a
                        href="javascript:void(0);" class="a_Orderby" data-orderindex="5"><b>↓</b></a>
                </th>
                <th align="center" bgcolor="#bddcf4">
                    <a href="javascript:void(0);" class="a_Orderby" data-orderindex="6"><b>↑</b></a>交易金额<a
                        href="javascript:void(0);" class="a_Orderby" data-orderindex="7"><b>↓</b></a>
                </th>
                <th align="center" bgcolor="#bddcf4">
                    <a href="javascript:void(0);" class="a_Orderby" data-orderindex="8"><b>↑</b></a>拜访次数<a
                        href="javascript:void(0);" class="a_Orderby" data-orderindex="9"><b>↓</b></a>
                </th>
                <th align="center" bgcolor="#bddcf4">
                    <a href="javascript:void(0);" class="a_Orderby" data-orderindex="10"><b>↑</b></a>拜访支出<a
                        href="javascript:void(0);" class="a_Orderby" data-orderindex="11"><b>↓</b></a>
                </th>
            </tr>
            <asp:Repeater runat="server" ID="rptCustomer">
                <ItemTemplate>
                    <tr class="<%# Container.ItemIndex % 2 == 0 ? "even" : "odd" %>">
                        <td height="30" align="center">
                            <%# GetLineIndex(Container.ItemIndex) %>
                        </td>
                        <td align="center">
                            <%# Eval("ProvinceName")%>&nbsp;&nbsp;<%# Eval("CityName")%>
                        </td>
                        <td align="center">
                            <%# Eval("CustomerName")%>
                        </td>
                        <td align="center">
                            <%# Eval("ContactName")%>
                        </td>
                        <td align="center">
                            <%# Eval("ContactTel")%>
                        </td>
                        <td align="center">
                            <%# Eval("ContactMobile")%>
                        </td>
                        <td align="center">
                            <%# Eval("JiaoYiRenShu")%>
                        </td>
                        <td align="center">
                            共计<%# Eval("JiaoYiCiShu")%>次
                        </td>
                        <td align="center">
                            <%# this.ToMoneyString(Eval("JiaoYiJinE"))%>
                        </td>
                        <td align="center">
                            共计<%# Eval("BaiFangCiShu")%>次
                        </td>
                        <td align="center">
                            <%# this.ToMoneyString(Eval("BaiFangJinE"))%>
                        </td>
                    </tr>
                </ItemTemplate>
            </asp:Repeater>
            <tr>
                <td height="30" colspan="6" align="right" bgcolor="#E3F1FC">
                    合计：
                </td>
                <td align="center" bgcolor="#E3F1FC">
                    <%= HeJi == null ? string.Empty : HeJi.JiaoYiRenShu.ToString() %>
                </td>
                <td align="center" bgcolor="#E3F1FC">
                    共计<%= HeJi == null ? string.Empty : HeJi.JiaoYiCiShu.ToString() %>次
                </td>
                <td align="center" bgcolor="#E3F1FC">
                    <%= HeJi == null ? string.Empty : this.ToMoneyString(HeJi.JiaoYiJinE) %>
                </td>
                <td align="center" bgcolor="#E3F1FC">
                    共计<%= HeJi == null ? string.Empty : HeJi.BaiFangCiShu.ToString() %>次
                </td>
                <td align="center" bgcolor="#E3F1FC">
                    <%= HeJi == null ? string.Empty : this.ToMoneyString(HeJi.BaiFangJinE) %>
                </td>
            </tr>
        </table>
        <table width="100%" border="0" cellspacing="0" cellpadding="0">
            <tr>
                <td align="right">
                    <cc1:ExporPageInfoSelect runat="server" ID="page1" />
                </td>
            </tr>
        </table>
    </div>

    <script type="text/javascript">

        function changeURLPar(destiny, par, par_value) {
            var pattern = par + '=([^&]*)';
            var replaceText = par + '=' + par_value;

            if (destiny.match(pattern)) {
                var tmp = '/\\' + par + '=[^&]*/';
                tmp = destiny.replace(eval(tmp), replaceText);
                return (tmp);
            }
            else {
                if (destiny.match('[\?]')) {
                    return destiny + '&' + replaceText;
                }
                else {
                    return destiny + '?' + replaceText;
                }
            }

            return destiny + '\n' + par + '\n' + par_value;
        }

        $(document).ready(function() {
            utilsUri.initSearch();
            tableToolbar.init({ tableContainerSelector: "#liststyle" });

            pcToobar.init({
                pID: "#pid",
                cID: "#cid",
                comID: '<%=this.SiteUserInfo.CompanyId %>',
                gSelect: "1",
                pSelect: '<%= EyouSoft.Common.Utils.GetQueryStringValue("pid") %>',
                cSelect: '<%= EyouSoft.Common.Utils.GetQueryStringValue("cid") %>',
                isCy: "0"
            });

            $("#liststyle").find(".a_Orderby").each(function() {
                var url = window.location.href;
                url = changeURLPar(url, "oi", $.trim($(this).attr("data-orderindex")));
                $(this).click(function() {
                    window.location.href = url;
                });
            });
        });
    </script>

</asp:Content>
