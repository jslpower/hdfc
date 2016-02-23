<%@ Page Language="C#" MasterPageFile="~/MasterPage/Boxy.Master" AutoEventWireup="true"
    CodeBehind="VisitEdit.aspx.cs" Inherits="Web.jidiaoCenter.VisitEdit" Title="回访" %>

<asp:Content ID="Content1" ContentPlaceHolderID="PageHead" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="PageBody" runat="server">
    <form runat="server" id="form1">
    <div style="width: 790px; margin: 10px auto;">
        <asp:PlaceHolder runat="server" ID="PhList"><span class="formtableT">回访记录</span>
            <table width="100%" cellspacing="1" cellpadding="0" border="0" align="center">
                <tbody>
                    <tr class="odd">
                        <th height="30">
                            <p>
                                全陪姓名</p>
                        </th>
                        <th>
                            电话
                        </th>
                        <th>
                            回访时间
                        </th>
                        <th align="center">
                            回访人
                        </th>
                        <th align="center">
                            评分
                        </th>
                        <th align="center">
                            客人意见
                        </th>
                        <th width="100">
                            操作
                        </th>
                    </tr>
                    <asp:Repeater runat="server" ID="rptList">
                        <ItemTemplate>
                            <tr class="<%# Container.ItemIndex % 2 == 0 ? "even" : "odd"%>" data-id='<%#Eval("VisitId") %>'>
                                <td height="30" bgcolor="#E3F1FC" align="center">
                                    <%#Eval("QuanPeiName")%>
                                </td>
                                <td bgcolor="#E3F1FC" align="center">
                                    <%#Eval("QuanPeiPhone")%>
                                </td>
                                <td bgcolor="#E3F1FC" align="center">
                                    <%#EyouSoft.Common.Utils.GetDateTime(Convert.ToString(Eval("VisitTime"))).ToString("yyyy-MM-dd")%>
                                </td>
                                <td bgcolor="#E3F1FC" align="center" class="pandl3">
                                    <%#Eval("Vistor")%>
                                </td>
                                <td bgcolor="#E3F1FC" align="center">
                                   <%#((EyouSoft.Model.EnumType.TourStructure.Score)Eval("Score")).ToString()%>
                                </td>
                                <td bgcolor="#E3F1FC" align="center">
                                    <%#Eval("CustomerOpinion")%>
                                </td>
                                <td bgcolor="#E3F1FC" align="center">
                                    <a href="javascript:;" class="update">修改</a> <a href="javascript:;" class="delete">
                                        删除</a>
                                </td>
                            </tr>
                        </ItemTemplate>
                    </asp:Repeater>
                </tbody>
            </table>
        </asp:PlaceHolder>
        <table width="100%" cellspacing="1" cellpadding="0" border="0" align="center" style="margin-top: 10px;">
            <tbody>
                <tr class="odd">
                    <th width="120" height="30" align="right">
                        <span class="fred">*</span>全陪姓名：
                    </th>
                    <td bgcolor="#E3F1FC">
                        <asp:TextBox ID="txtName" valid="required" errmsg="全陪姓名不能为空" class="formsize70 inputtext"
                            runat="server"></asp:TextBox>
                    </td>
                    <th width="120" align="right">
                        电话：
                    </th>
                    <td bgcolor="#E3F1FC">
                        <asp:TextBox ID="txtTel" class="formsize140 inputtext" runat="server"></asp:TextBox>
                    </td>
                </tr>
                <tr class="odd">
                    <th width="120" height="30" align="right">
                        <span class="fred">*</span>回访时间：
                    </th>
                    <td bgcolor="#E3F1FC">
                        <asp:TextBox ID="txtvisittime" valid="required" errmsg="回访时间不能为空" onfocus="WdatePicker()" class="formsize80 inputtext"
                            runat="server"></asp:TextBox>
                    </td>
                    <th align="right">
                        回访人：
                    </th>
                    <td bgcolor="#E3F1FC">
                        <asp:TextBox ID="txtvisiter" class="formsize70 inputtext" runat="server"></asp:TextBox>
                    </td>
                </tr>
                <tr class="odd">
                    <th height="30" align="right">
                        评分：
                    </th>
                    <td bgcolor="#E3F1FC" colspan="3">
                        <asp:DropDownList ID="dptscore" runat="server" CssClass="inputselect">
                        </asp:DropDownList>
                    </td>
                </tr>
                <tr class="odd">
                    <th align="right">
                        客人意见：
                    </th>
                    <td bgcolor="#E3F1FC" colspan="3">
                        <asp:TextBox ID="txtContent" TextMode="MultiLine" Height="70px" class="formsize450 inputtext"
                            runat="server"></asp:TextBox>
                    </td>
                </tr>
            </tbody>
        </table>
        <table width="100%" cellspacing="0" cellpadding="0" border="0" align="center" style="margin: 10px auto;">
            <tbody>
                <tr class="odd">
                    <td height="30" bgcolor="#E3F1FC" align="left" colspan="14">
                        <table cellspacing="0" cellpadding="0" border="0" align="center">
                            <tbody>
                                <tr>
                                    <td width="80" height="40" align="center" class="tjbtn02">
                                        <a href="javascript:;" id="btnsave" runat="server">保存</a>
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
            VisitPage.BindBtn();
        })
        var VisitPage={
            BindBtn:function(){
                $("#<%=btnsave.ClientID %>").click(function(){
                    var url="/jidiaoCenter/VisitEdit.aspx?dotype=save&tourid="+'<%=Request.QueryString["tourid"] %>'+"&visitid="+'<%=Request.QueryString["visitid"] %>';
                   if(!VisitPage.CheckForm()){
                        return ;
                    }
                    $(this).html("正在保存");
                    VisitPage.GoAjax(url);
                })
                $(".update").click(function(){
                    var visitid=$(this).closest("tr").attr("data-id");
                    var url="/jidiaoCenter/VisitEdit.aspx?dotype=show&visitid="+visitid+"&tourid="+'<%=Request.QueryString["tourid"] %>';
                    window.location.href=url;
                })
                $(".delete").click(function(){
                    var visitid=$(this).closest("tr").attr("data-id");
                    var url="/jidiaoCenter/VisitEdit.aspx?dotype=delete&visitid="+visitid;
                    parent.tableToolbar.ShowConfirmMsg("确定要删除吗？", function() {
                            VisitPage.GoAjax(url);
                    });
                    
                })
            },
            GoAjax: function(url) {
                $("#<%=btnsave.ClientID %>").unbind("click");
                $.newAjax({
                    type: "post",
                    cache: false,
                    url: url,
                    dataType: "json",
                    data: $("#<%=form1.ClientID %>").serialize(),
                    success: function(ret) {
                        if (ret.result == "1") {
                            parent.tableToolbar._showMsg(ret.msg, function() { location.href = "/jidiaoCenter/VisitEdit.aspx?dotype=add&tourid="+'<%=Request.QueryString["tourid"] %>'; });
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
