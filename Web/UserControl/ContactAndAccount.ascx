<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ContactAndAccount.ascx.cs"
    Inherits="Web.UserControl.ContactAndAccount" %>
<table width="99%" cellspacing="0" cellpadding="0" border="0" style="height: auto;
    margin: 0 auto; zoom: 1; overflow: hidden;" id="contact_tbl" class="table_white">
    <tr style="height: 28px;" class="odd">
        <td align="center" class="alertboxTableT">
            姓名
        </td>
        <td align="center" class="alertboxTableT">
            职务
        </td>
        <td align="center" class="alertboxTableT">
            电话
        </td>
        <td align="center" class="alertboxTableT">
            手机
        </td>
        <td align="center" class="alertboxTableT">
            QQ
        </td>
        <td align="center" class="alertboxTableT">
            传真
        </td>
        <td align="center" class="alertboxTableT">
            生日
        </td>
        <td align="center" class="alertboxTableT">
            E-mail
        </td>
        <td align="center" class="alertboxTableT">
            操作
        </td>
    </tr>
    <asp:PlaceHolder runat="server" ID="PhDefault">
        <tbody class="Trcontact">
            <tr>
                <td height="28" align="center">
                    <input type="text" value="" errmsg="请输入联系人名称!" valid="required" class="inputtext formsize70"
                        name="contact_Name" /><font class="fred">*</font>
                    <input type="hidden" value="" name="contact_Id" />
                </td>
                <td align="center">
                    <input type="text" value="" class="inputtext formsize70" name="contact_duty" />
                </td>
                <td align="center">
                    <input type="text" value="" valid="isPhone" errmsg="电话格式错误!" class="inputtext formsize70"
                        name="contact_tel" />
                </td>
                <td align="center">
                    <input type="text" value="" valid="isMobile" errmsg="手机格式错误!" class="inputtext formsize70"
                        name="contact_mobile" />
                </td>
                <td align="center">
                    <input type="text" valid="isQQ" errmsg="QQ格式错误!" class="inputtext formsize70" value=""
                        digits="true" name="contact_qq" />
                </td>
                <td align="center">
                    <input type="text" valid="isPhone" errmsg="传真格式错误!" class="inputtext formsize70"
                        value="" digits="true" name="contact_fax" />
                </td>
                <td align="center">
                    <input type="text" class="inputtext formsize70" onfocus="WdatePicker()" value=""
                        name="contact_birthday" />
                </td>
                <td align="center">
                    <input type="text" value="" valid="isEmail" errmsg="邮箱格式错误!" class="inputtext formsize120"
                        name="contact_email" />
                </td>
                <td align="center">
                    <a class="addcontact" href="javascript:void(0)">
                        <img width="48" height="20" src="/images/addimg.gif" alt="" /></a> <a class="delcontact"
                            href="javascript:void(0)">
                            <img src="/images/delimg.gif" alt="" /></a>
                    <asp:PlaceHolder runat="server" ID="phaccount"><a href="javascript:;" data-class="account">
                        分配帐号</a></asp:PlaceHolder>
                        <input type="hidden" name="hidIsDel" value="0" />
                </td>
            </tr>
            <tr style="display: none" class="TrAccount">
                <td align="center">
                    帐号:
                </td>
                <td align="center">
                    <input type="text" value="" name="account" class="inputtext formsize70" />
                    <input type="hidden" value="" name="userid" />
                </td>
                <td align="center">
                    密码:
                </td>
                <td align="center" colspan="2">
                    <input type="password" value="" name="pwd" class="inputtext formsize120 pwd" />
                </td>
                <td align="center">
                    确认密码:
                </td>
                <td align="center" colspan="2">
                    <input type="password" value="" name="repwd" class="inputtext formsize120 repwd" />
                </td>
                <td></td>
            </tr>
        </tbody>
    </asp:PlaceHolder>
    <asp:Repeater ID="rptList" runat="server">
        <ItemTemplate>
            <tbody class="Trcontact">
                <tr>
                    <td height="28" align="center">
                        <input type="text" errmsg="请输入联系人名称!" valid="required" value="<%#Eval("ContactName")%>"
                            class="inputtext formsize70" name="contact_Name"><font class="fred">*</font>
                        <input type="hidden" value='<%#Eval("Id") %>' name="contact_Id" />
                    </td>
                    <td align="center">
                        <input type="text" value="<%#Eval("JobTitle")%>" class="inputtext formsize70" name="contact_duty" />
                    </td>
                    <td align="center">
                        <input type="text" valid="isPhone" errmsg="电话格式错误!" value="<%#Eval("ContactTel")%>"
                            class="inputtext formsize70" name="contact_tel" />
                    </td>
                    <td align="center">
                        <input type="text" valid="isMobile" errmsg="手机格式错误!" value="<%#Eval("ContactMobile")%>"
                            class="inputtext formsize70" name="contact_mobile" />
                    </td>
                    <td align="center">
                        <input type="text" valid="isQQ" errmsg="QQ格式错误!" class="inputtext formsize70" value="<%#Eval("QQ")%>"
                            digits="true" name="contact_qq" />
                    </td>
                    <td align="center">
                        <input type="text" valid="isPhone" errmsg="传真格式错误!" class="inputtext formsize70"
                            value="<%#Eval("ContactFax") %>" digits="true" name="contact_fax" />
                    </td>
                    <td align="center">
                        <input type="text" class="inputtext formsize70" onfocus="WdatePicker()" value='<%#Convert.ToString(Eval("BirthDay"))==""?"":Convert.ToDateTime(Eval("BirthDay")).ToString("yyyy-MM-dd") %>'
                            name="contact_birthday" />
                    </td>
                    <td align="center">
                        <input type="text" valid="isEmail" errmsg="邮箱格式错误!" value="<%#Eval("Email")%>" class="inputtext formsize120"
                            name="contact_email" />
                    </td>
                    <td align="center">
                        <a class="addcontact" href="javascript:void(0)">
                            <img width="48" height="20" src="/images/addimg.gif" alt="" /></a> <a class="delcontact"
                                href="javascript:void(0)">
                                <img src="/images/delimg.gif" alt="" /></a>
                        <%if (IsAccount)
                          { %>
                        <a href="javascript:;" <%#((EyouSoft.Model.SourceStructure.MSupplierContactLoginInfo)Eval("LoginInfo"))==null?"data-class='account'":"data-class='cencer'" %>>
                            <%#((EyouSoft.Model.SourceStructure.MSupplierContactLoginInfo)Eval("LoginInfo"))==null?"分配帐号":"删除帐号" %></a>
                        <%} %>
                         <input type="hidden" name="hidIsDel" value='<%#((EyouSoft.Model.SourceStructure.MSupplierContactLoginInfo)Eval("LoginInfo"))==null?"0":"1" %>' />
                    </td>
                </tr>
                <tr <%#((EyouSoft.Model.SourceStructure.MSupplierContactLoginInfo)Eval("LoginInfo"))==null?"style='display:none'":"" %>
                    class="TrAccount">
                    <td align="center">
                        帐号:
                    </td>
                    <td align="center">
                        <input type="text" <%#((EyouSoft.Model.SourceStructure.MSupplierContactLoginInfo)Eval("LoginInfo"))==null?"":"valid='required' errmsg='帐号不能为空'" %> value='<%#((EyouSoft.Model.SourceStructure.MSupplierContactLoginInfo)Eval("LoginInfo"))==null?"":((EyouSoft.Model.SourceStructure.MSupplierContactLoginInfo)Eval("LoginInfo")).UserName %>'
                            name="account" class="inputtext formsize70" <%#((EyouSoft.Model.SourceStructure.MSupplierContactLoginInfo)Eval("LoginInfo"))==null?"":"style='background-color:#dadada' readonly='readonly'" %> />
                        <input type="hidden" value='<%#((EyouSoft.Model.SourceStructure.MSupplierContactLoginInfo)Eval("LoginInfo"))==null?"":((EyouSoft.Model.SourceStructure.MSupplierContactLoginInfo)Eval("LoginInfo")).UserId %>'
                            name="userid" />
                    </td>
                    <td align="center">
                        密码:
                    </td>
                    <td align="center" colspan="2">
                        <input class="inputtext formsize120 pwd" <%#((EyouSoft.Model.SourceStructure.MSupplierContactLoginInfo)Eval("LoginInfo"))==null?"":"valid='required' errmsg='密码不能为空'" %> type="password" value='<%#((EyouSoft.Model.SourceStructure.MSupplierContactLoginInfo)Eval("LoginInfo"))==null? null :((EyouSoft.Model.CompanyStructure.PassWord)(((EyouSoft.Model.SourceStructure.MSupplierContactLoginInfo)Eval("LoginInfo")).PassWord)).NoEncryptPassword %>'
                            name="pwd" />
                    </td>
                    <td align="center">
                        确认密码:
                    </td>
                    <td align="center" colspan="2">
                        <input class="inputtext formsize120 repwd" type="password" <%#((EyouSoft.Model.SourceStructure.MSupplierContactLoginInfo)Eval("LoginInfo"))==null?"":"valid='required' errmsg='确认密码不能为空'" %> value='<%#((EyouSoft.Model.SourceStructure.MSupplierContactLoginInfo)Eval("LoginInfo"))==null?null:((EyouSoft.Model.CompanyStructure.PassWord)(((EyouSoft.Model.SourceStructure.MSupplierContactLoginInfo)Eval("LoginInfo")).PassWord)).NoEncryptPassword %>'
                            name="repwd" />
                    </td>
                    <td></td>
                </tr>
            </tbody>
        </ItemTemplate>
    </asp:Repeater>
</table>

<script type="text/javascript">
    $(function() {
        $("#contact_tbl").autoAdd({tempRowClass:"Trcontact",addButtonClass:"addcontact",delButtonClass:"delcontact",indexClass: "indexcontact"});
        $("#contact_tbl").find("a[data-class='account']").live("click",function(){
            $(this).attr("data-class","cencer");
            $(this).parent().find("input[name='hidIsDel']").val("1");
            $(this).html("删除帐号");
            var tr=$(this).closest("tbody").find("tr[class='TrAccount']");
            tr.show();
            //valid="equal" eqaulName="password"
            tr.find("input[type='text']").attr("valid","required").attr("errmsg","帐号不能为空");
            tr.find("input[name='repwd']").attr("valid","required").attr("errmsg","确认密码不能为空");
            tr.find("input[name='pwd']").attr("valid","required").attr("errmsg","密码不能为空");
        });
         $("#contact_tbl").find("a[data-class='cencer']").live("click",function(){
            $(this).attr("data-class","account");
            $(this).parent().find("input[name='hidIsDel']").val("0");
            $(this).html("分配帐号");
            var tr=$(this).closest("tbody").find("tr[class='TrAccount']");
            tr.hide();
            tr.find("input[type='text']").removeAttr("valid").removeAttr("errmsg");
            tr.find("input[type='password']").removeAttr("valid").removeAttr("errmsg");
        });
        $(".pwd").live("blur",function(){
            var self=$(this);
            var repwd=self.closest("tr").find("input[name='repwd']");
            if(repwd.val()!="" && self.val()!=repwd.val()){
                parent.tableToolbar._showMsg("两次密码输入不一致!");
                $(this).focus();
                return false;
            }
        })
        $(".repwd").live("blur",function(){
            var self=$(this);
            var pwd=self.closest("tr").find("input[name='pwd']");
            if(pwd.val()!="" && self.val()!=pwd.val()){
                parent.tableToolbar._showMsg("两次密码输入不一致!");
                $(this).focus();
                return false;
            }
        })
    })
    
</script>

