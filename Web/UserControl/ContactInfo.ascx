<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ContactInfo.ascx.cs"
    Inherits="Web.UserControl.ContactInfo" %>
<table width="100%" cellspacing="0" cellpadding="0" border="0" class="table_white autoAdd"
    id="tb_Contact">
    <tbody>
        <tr>
            <th align="center">
                姓名
            </th>
            <th align="center">
                性别
            </th>
            <th align="center">
                生日
            </th>
            <th align="center">
                部门
            </th>
            <th align="center">
                职务
            </th>
            <th align="center">
                电话
            </th>
            <th align="center">
                手机
            </th>
            <th width="120" align="center">
                &nbsp;
            </th>
        </tr>
    </tbody>
    <asp:PlaceHolder runat="server" ID="PhDefault">
        <tbody class="TrContact">
            <tr>
                <td height="30" align="center">
                    <input type="text" class="formsize50 inputtext" name="contact_Name" value="" />
                    <input type="hidden" value="" name="contact_Id" />
                </td>
                <td align="center">
                    <select id="sltsex" name="sltsex" class="selecttext">
                        <option value="2">男</option>
                        <option value="1">女</option>
                    </select>
                </td>
                <td align="center">
                    <input type="text" onfocus="WdatePicker()" value="" class="formsize80 inputtext"
                        name="contact_birthday" />
                </td>
                <td align="center">
                    <input type="text" class="formsize80 inputtext" name="contact_depart" value="" />
                </td>
                <td align="center">
                    <input type="text" class="formsize80 inputtext" name="contact_duty" value="" />
                </td>
                <td align="center">
                    <input type="text" class="formsize80 inputtext" name="contact_tel" value="" valid="isPhone"
                        errmsg="电话格式不正确!" />
                </td>
                <td align="center">
                    <input type="text" class="formsize80 inputtext" name="contact_mobile" value="" valid="isMobile"
                        errmsg="手机格式不正确!" />
                </td>
                <td align="center" rowspan="3">
                    <a class="addcontact" href="javascript:void(0)">
                        <img width="48" height="20" src="/images/addimg.gif" alt="" /></a> <a class="delcontact"
                            href="javascript:void(0)">
                            <img width="48" height="20" src="/images/delimg.gif" alt="" /></a>
                </td>
            </tr>
            <tr>
                <th height="30" align="right">
                    QQ：
                </th>
                <td align="left" class="pandl4" colspan="2">
                    <input type="text" class="formsize80 inputtext" valid="isQQ" errmsg="QQ格式不正确!" name="contact_qq"
                        value="" />
                </td>
                <th align="right">
                    E-mail：
                </th>
                <td align="left" colspan="3">
                    <span class="pandl4">
                        <input type="text" class="formsize180 inputtext" valid="isEmail" errmsg="E-Mail格式不正确!"
                            name="contact_email" value="" />
                    </span>
                </td>
            </tr>
            <tr>
                <th align="right">
                    备注：
                </th>
                <td align="left" class="pandl4" colspan="6">
                    <textarea class="formsize450 inputtext" rows="3" name="contact_remark"></textarea>
                </td>
            </tr>
        </tbody>
    </asp:PlaceHolder>
    <asp:Repeater runat="server" ID="rptlist">
        <ItemTemplate>
            <tbody class="TrContact">
                <tr>
                    <td height="30" align="center">
                        <input type="text" class="formsize50 inputtext" name="contact_Name" value='<%#Eval("Name") %>' />
                        <input type="hidden" value='<%#Eval("CustomerId") %>' name="contact_Id" />
                    </td>
                    <td align="center">
                        <select id="sltsex" name="sltsex" class="selecttext">
                            <option value="2" <%# ((EyouSoft.Model.EnumType.CompanyStructure.Sex)Eval("Sex"))==EyouSoft.Model.EnumType.CompanyStructure.Sex.男 ?"selected='selected'":"" %>>男</option>
                            <option value="1" <%# ((EyouSoft.Model.EnumType.CompanyStructure.Sex)Eval("Sex"))==EyouSoft.Model.EnumType.CompanyStructure.Sex.女?"selected='selected'":"" %>>女</option>
                        </select>
                    </td>
                    <td align="center">
                        <input type="text" onfocus="WdatePicker()" value='<%#Convert.ToString(Eval("BirthDay"))==""?"":Convert.ToDateTime(Eval("BirthDay")).ToString("yyyy-MM-dd") %>'
                            class="formsize80 inputtext" name="contact_birthday" />
                    </td>
                    <td align="center">
                        <input type="text" class="formsize80 inputtext" name="contact_depart" value='<%#Eval("DepartmentName") %>' />
                    </td>
                    <td align="center">
                        <input type="text" class="formsize80 inputtext" name="contact_duty" value='<%#Eval("Job") %>' />
                    </td>
                    <td align="center">
                        <input type="text" class="formsize80 inputtext" valid="isPhone" errmsg="电话格式不正确"
                            name="contact_tel" value='<%#Eval("Tel") %>' />
                    </td>
                    <td align="center">
                        <input type="text" class="formsize80 inputtext" valid="isMobile" errmsg="手机格式不正确"
                            name="contact_mobile" value='<%#Eval("Mobile") %>' />
                    </td>
                    <td align="center" rowspan="3">
                        <a class="addcontact" href="javascript:void(0)">
                            <img width="48" height="20" src="/images/addimg.gif" alt="" /></a> <a class="delcontact"
                                href="javascript:void(0)">
                                <img width="48" height="20" src="/images/delimg.gif" alt="" /></a>
                    </td>
                </tr>
                <tr>
                    <th height="30" align="right">
                        QQ：
                    </th>
                    <td align="left" class="pandl4" colspan="2">
                        <input type="text" class="formsize80 inputtext" valid="isQQ" errmsg="QQ格式不正确" name="contact_qq"
                            value='<%#Eval("Qq") %>' />
                    </td>
                    <th align="right">
                        E-mail：
                    </th>
                    <td align="left" colspan="3">
                        <span class="pandl4">
                            <input type="text" class="formsize180 inputtext" valid="isEmail" errmsg="E-Mail格式不正确!"
                                name="contact_email" value='<%#Eval("Email") %>' />
                        </span>
                    </td>
                </tr>
                <tr>
                    <th align="right">
                        备注：
                    </th>
                    <td align="left" class="pandl4" colspan="6">
                        <textarea class="formsize450 inputtext" rows="3" name="contact_remark"><%#Eval("Remark") %></textarea>
                    </td>
                </tr>
            </tbody>
        </ItemTemplate>
    </asp:Repeater>
</table>

<script type="text/javascript">
    $(function(){
        $("#tb_Contact").autoAdd({tempRowClass:"TrContact",addButtonClass:"addcontact",delButtonClass:"delcontact"})
    })
</script>

