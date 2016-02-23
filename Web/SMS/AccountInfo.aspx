<%@ Page Title="账户信息_短信中心" Language="C#" MasterPageFile="~/MasterPage/Front.Master"
    AutoEventWireup="true" CodeBehind="AccountInfo.aspx.cs" Inherits="Web.SMS.AccountInfo" %>

<%@ Register Src="~/UserControl/SmsHeaderMenu.ascx" TagName="headerMenu" TagPrefix="uc1" %>
<%@ Register Assembly="ControlLibrary" Namespace="ControlLibrary" TagPrefix="asp" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style type="text/css">
        .hid
        {
            display: none;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="mainbody">
        <form runat="server" id="accountForm">
        <uc1:headerMenu id="cu_HeaderMenu" runat="server" TabIndex="5" />
        <table width="100%" border="0" cellspacing="0" cellpadding="0">
            <tr>
                <td align="left" class="yue">
                    当前您的余额为： <span>
                        <asp:Literal ID="litRemainMoney" runat="server"></asp:Literal></span>元 <span class="pandl10">
                            <a href="javascript:;" onclick="return AccountInfo.recharge();">
                                <img src="/images/dx-chongzhi.gif" alt="" width="151" height="44" /></a></span>
                </td>
            </tr>
        </table>
        <table width="99%" border="0" cellspacing="6" cellpadding="0">
            <tbody id="tableList">
            </tbody>
        </table>
        </form>
    </div>

    <script type="text/javascript">
                      var AccountInfo =
                    {
                        //打开弹窗
                        openDialog: function(p_url, p_title) {
                            Boxy.iframeDialog({ title: p_title, iframeUrl: p_url, width: "620px", height: "350px",data:"account"});
                        },
                        //充值
                        recharge: function() {
                            AccountInfo.openDialog("/SMS/ReCharge.aspx", "账户充值");
                            return false;
                        },//获取详细
                        getSD: function(p_method, p_id, p_title) {
                        Boxy.iframeDialog({ title: p_title, iframeUrl: "/SMS/SendDetailDialog.aspx?method=" + p_method + "&tid=" + p_id, width: "620px", height: "350px" });
                        },
                        //获取充值和消费明细
                        getList: function(tar, type) {
                            var page = tar = "" ? "1" : $(tar).attr("gotopage");
                            $.newAjax({
                                type: "GET",
                                dataType: "text",
                                url: "AjaxReChargeAndConsume.aspx",
                                data: { Page: page, type: type },
                                cache: false,
                                success: function(result) {
                                    if (type == "recharge") {
                                        
                                        $("#tr_recharge").replaceWith(result);
                                    }
                                    else if (type == "consume") {
                                        $("#tr_consume").replaceWith(result);
                                    } else {
                                        $("#tableList").html(result);
                                    }
                                }
                            });
                        }

                    }
                    //初次加载获取数据
                    $(document).ready(function() {
                        AccountInfo.getList("","");
                    });
    </script>

</asp:Content>
