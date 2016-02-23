<%@ Page Language="C#" MasterPageFile="~/MasterPage/Boxy.Master" AutoEventWireup="true" CodeBehind="AddYouke.aspx.cs" Inherits="Web.CustomerManage.AddYouke" Title="游客" %>
<asp:Content ID="Content1" ContentPlaceHolderID="PageHead" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="PageBody" runat="server">
 <form id="form1" runat="server">
    <div style="width: 500px; margin: 10px auto;">
        <asp:PlaceHolder runat="server" ID="PhList">
            <table width="100%" align="center" cellspacing="1" cellpadding="0" border="0">
                <tbody>
                    <tr class="odd">
                        <th width="100" height="30">
                            <p>
                                时间</p>
                        </th>
                        <th>
                            备注
                        </th>
                        <th width="100">
                            操作
                        </th>
                    </tr>
                    <asp:Repeater runat="server" ID="rptlist">
                        <ItemTemplate>
                            <tr class="<%# Container.ItemIndex % 2 == 0 ? "even" : "odd"%>" data-id='<%#Eval("Id") %>'>
                                <td height="30" align="center">
                                    <%#Convert.ToDateTime(Eval("SendGiftTime")).ToString("yyyy-MM-dd")%>
                                </td>
                                <td align="center">
                                    <%#Eval("Remark")%>
                                </td>
                                <td align="center">
                                    <a href="javascript:;" class="update">修改</a> <a class="delete" href="javascript:;">删除</a>
                                </td>
                            </tr>
                        </ItemTemplate>
                    </asp:Repeater>
                </tbody>
            </table>
        </asp:PlaceHolder>
        <table width="100%" align="center" cellspacing="1" cellpadding="0" border="0" style="margin: 10px auto;">
            <tbody>
                <tr class="odd">
                    <th width="100" height="30" align="right">
                        时间：
                    </th>
                    <td bgcolor="#E3F1FC">
                        <asp:TextBox ID="txtdate" name="txtdate" valid="required" errmsg="请填写时间" onfocus="WdatePicker()" CssClass="inputtext formsize80"
                            runat="server"></asp:TextBox>
                    </td>
                </tr>
                <tr class="odd">
                    <th align="right">
                        备注：
                    </th>
                    <td bgcolor="#E3F1FC">
                        <asp:TextBox ID="txtremark" name="txtremark" CssClass="inputtext formsize260" TextMode="MultiLine"
                            Height="100px" runat="server"></asp:TextBox>
                    </td>
                </tr>
            </tbody>
        </table>
        <table width="100%" align="center" cellspacing="0" cellpadding="0" border="0">
            <tbody>
                <tr class="odd">
                    <td height="30" align="left" bgcolor="#E3F1FC" colspan="14">
                        <table align="center" cellspacing="0" cellpadding="0" border="0">
                            <tbody>
                                <tr>
                                    <td width="80" height="40" align="center" class="tjbtn02">
                                        <a href="javascript:;" id="btn" runat="server">保存</a>
                                    </td>
                                </tr>
                            </tbody>
                        </table>
                    </td>
                </tr>
            </tbody>
        </table>
    </div>
    </form>

    <script type="text/javascript">
        $(function(){
            AddYoukePage.BindBtn();
        })
        var AddYoukePage={
            BindBtn:function(){
                $("#<%=btn.ClientID %>").click(function(){
                    var url="/CustomerManage/AddYouKe.aspx?dotype=save&travelid="+'<%=Request.QueryString["travelid"] %>'+"&giftid="+'<%=Request.QueryString["giftid"] %>';
                    if(!AddYoukePage.CheckForm()){
                        return ;
                    }
                    $(this).html("正在保存");
                    AddYoukePage.GoAjax(url);
                })
                $(".update").click(function(){
                    var giftid=$(this).closest("tr").attr("data-id");
                    var url="/CustomerManage/AddYouKe.aspx?dotype=show&travelid="+'<%=Request.QueryString["travelid"] %>'+"&giftid="+giftid;
                    window.location.href=url;
                })
                $(".delete").click(function(){
                        var giftid=$(this).closest("tr").attr("data-id");
                        var url="/CustomerManage/AddYouKe.aspx?dotype=delete&travelid="+'<%=Request.QueryString["travelid"] %>'+"&giftid="+giftid;
                        parent.tableToolbar.ShowConfirmMsg("确定要删除吗？", function() {
                            AddYoukePage.GoAjax(url);
                        });
                })
            },
            GoAjax: function(url) {
                $("#<%=btn.ClientID %>").unbind("click");
                $.newAjax({
                    type: "post",
                    cache: false,
                    url: url,
                    dataType: "json",
                    data: $("#<%=form1.ClientID %>").serialize(),
                    success: function(ret) {
                        if (ret.result == "1") {
                            parent.tableToolbar._showMsg(ret.msg, function() { location.href = "/CustomerManage/AddYouKe.aspx?dotype=add&travelid="+'<%=Request.QueryString["travelid"] %>'; });
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
            }
        }
    </script>
</asp:Content>
