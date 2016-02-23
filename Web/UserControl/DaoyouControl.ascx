<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="DaoyouControl.ascx.cs"
    Inherits="Web.UserControl.DaoyouControl" %>
<table width="450" cellspacing="0" cellpadding="0" border="0" class="table_white autoAdd" id="TableDaoyou">
    <tbody>
        <tr>
            <th align="center">
                姓名
            </th>
            <th align="center">
                电话
            </th>
            <th width="110" align="center">
                操作
            </th>
        </tr>
        <asp:PlaceHolder runat="server" ID="Ph_Daoyou">
            <tr class="TrDaoyou">
                <td height="30" align="center">
                    <input type="text" class="formsize100 inputtext Offers_daoyou" name="txtDaoyou_Name" style="background-color: #dadada" readonly="readonly" value="" />
                    <input type="hidden" value="" name="hidDaoyou_id" />
                    <a href="javascript:;" class="xuanyong Offers_daoyou"></a>
                </td>
                <td align="center">
                    <input type="text" class="formsize100 inputtext" name="txtDaoyou_Tel" value="" />
                </td>
                <td align="center">
                    <a class="adddaoyou" href="javascript:void(0)">
                        <img width="48" height="20" src="/images/addimg.gif" alt="" /></a> <a class="deldaoyou"
                            href="javascript:void(0)">
                            <img width="48" height="20" src="/images/delimg.gif" alt="" /></a>
                </td>
            </tr>
        </asp:PlaceHolder>
        <asp:Repeater runat="server" ID="rptlist">
            <ItemTemplate>
                <tr class="TrDaoyou">
                    <td height="30" align="center">
                    <span class="indexdaoyou"></span>
                        <input type="text" class="formsize100 inputtext Offers_daoyou" name="txtDaoyou_Name" style="background-color: #dadada" readonly="readonly" value='<%#Eval("GuideName") %>' />
                        <input type="hidden" value='<%#Eval("GuideId") %>' name="hidDaoyou_id" />
                        <a href="javascript:;" class="xuanyong Offers_daoyou"></a>
                    </td>
                    <td align="center">
                        <input type="text" class="formsize100 inputtext" name="txtDaoyou_Tel" value='<%#Eval("Phone") %>' />
                    </td>
                    <td align="center">
                        <a class="adddaoyou" href="javascript:void(0)">
                            <img width="48" height="20" src="/images/addimg.gif" alt="" /></a> <a class="deldaoyou"
                                href="javascript:void(0)">
                                <img width="48" height="20" src="/images/delimg.gif" alt="" /></a>
                    </td>
                </tr>
            </ItemTemplate>
        </asp:Repeater>
    </tbody>
</table>

<script type="text/javascript">
    $(function() {
       DaoyouControl.PageInit();
       DaoyouControl.xuanyong();
    })
    var DaoyouControl={
        PageInit:function(){
             $("#TableDaoyou").autoAdd({tempRowClass:"TrDaoyou",addButtonClass:"adddaoyou",delButtonClass:"deldaoyou",indexClass:"indexdaoyou"});
        },
        xuanyong:function(){
            $(".Offers_daoyou").live("click",function(){
                $(this).parent().find("a").attr("id", "btn_" + parseInt(Math.random() * 100000));
                var url = "/CommonPage/UserSupper.aspx?aid=" + $(this).parent().find("a").attr("id") + "&";
                var hideObj = $(this).parent().find("input[name='hidDaoyou_id']");
                var showObj = $(this).parent().find("input[name='txtDaoyou_Name']");
                if (!hideObj.attr("id")) {
                    hideObj.attr("id", "hideID_" + parseInt(Math.random() * 10000000));
                }
                if (!showObj.attr("id")) {
                    showObj.attr("id", "ShowID_" + parseInt(Math.random() * 10000000));
                }
                url += $.param({ suppliertype:'<%=EyouSoft.Model.EnumType.CompanyStructure.SupplierType.导游 %>' , hideID:$("#"+hideObj.attr("id")).val(),pIframeId:'<%= EyouSoft.Common.Utils.GetQueryStringValue("iframeId") %>',callback:"CallBackFun" })
                top.Boxy.iframeDialog({
                    iframeUrl: url,
                    title: "选择导游",
                    modal: true,
                    width: "880",
                    height: "350"
                });
            })
        }
    }
    //回调函数
    function CallBackFun(obj) {
        if (obj) {
            $("#" + obj.aid).closest("tr").find("input[name='txtDaoyou_Name']").val(obj.name);
            $("#" + obj.aid).closest("tr").find("input[name='hidDaoyou_id']").val(obj.id);
            $("#" + obj.aid).closest("tr").find("input[name='txtDaoyou_Tel']").val(obj.contacttel);
        }
    }
    
    
</script>

