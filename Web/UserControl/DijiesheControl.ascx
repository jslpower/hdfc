<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="DijiesheControl.ascx.cs"
    Inherits="Web.UserControl.DijiesheControl" %>
<table width="630" cellspacing="0" cellpadding="0" border="0" class="table_white autoAdd"
    id="TableDijieshe">
    <tbody>
        <tr>
            <th align="center">
                地接社
            </th>
            <th align="center">
                计调
            </th>
            <th align="center">
                电话
            </th>
            <th align="center">
                QQ
            </th>
            <th width="110" align="center">
                操作
            </th>
        </tr>
        <asp:PlaceHolder runat="server" ID="Ph_Dijie">
            <tr class="Trdijieshe">
                <td height="30" align="center">
                    <span class="indexdijie"></span>
                    <input type="text" class="formsize140 inputtext" name="txtDiejie_unit" style="background-color: #dadada"
                        readonly="readonly" value="" />
                    <input type="hidden" value="" name="hidDijie_id" />
                    <a href="javascript:;" class="xuanyong Offers"></a>
                </td>
                <td align="center">
                    <input type="text" class="formsize70 inputtext" name="txtDijie_planer" value="" />
                </td>
                <td align="center">
                    <input type="text" class="formsize100 inputtext" name="txtDijie_Tel" value="" />
                </td>
                <td align="center">
                    <input type="text" class="formsize80 inputtext" name="txtDijie_QQ" value="" />
                </td>
                <td align="center">
                    <a class="adddijieshe" href="javascript:void(0)">
                        <img width="48" height="20" src="/images/addimg.gif" alt="" /></a> <a class="deldijie"
                            href="javascript:void(0)">
                            <img width="48" height="20" src="/images/delimg.gif" alt="" /></a>
                </td>
            </tr>
        </asp:PlaceHolder>
        <asp:Repeater runat="server" ID="rptList">
            <ItemTemplate>
                <tr class="Trdijieshe">
                    <td height="30" align="center">
                        <input type="text" class="formsize140 inputtext" name="txtDiejie_unit" style="background-color: #dadada"
                            readonly="readonly" value="" />
                        <input type="hidden" value="" name="hidDijie_id" />
                        <a href="javascript:;" class="xuanyong Offers"></a>
                    </td>
                    <td align="center">
                        <input type="text" class="formsize70 inputtext" name="txtDijie_planer" value="" />
                    </td>
                    <td align="center">
                        <input type="text" class="formsize100 inputtext" name="txtDijie_Tel" value="" />
                    </td>
                    <td align="center">
                        <input type="text" class="formsize80 inputtext" name="txtDijie_QQ" value="" />
                    </td>
                    <td align="center">
                        <a class="adddijieshe" href="javascript:void(0)">
                            <img width="48" height="20" src="/images/addimg.gif" alt="" /></a> <a class="deldijie"
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
       DijieControl.PageInit();
       DijieControl.xuanyong();
    })
    var DijieControl={
        PageInit:function(){
             $("#TableDijieshe").autoAdd({tempRowClass:"Trdijieshe",addButtonClass:"adddijieshe",delButtonClass:"deldijie"});
        },
        xuanyong:function(){
            $(".Offers").live("click", function() {
            $(this).attr("id", "btn_" + parseInt(Math.random() * 100000));
            var url = "/CommonPage/UserSupper.aspx?aid=" + $(this).attr("id") + "&";
            var hideObj = $(this).parent().find("input[name='hidsourceid']");
            var showObj = $(this).parent().find("input[name='txtsourcename']");
            if (!hideObj.attr("id")) {
                hideObj.attr("id", "hideID_" + parseInt(Math.random() * 10000000));
            }
            if (!showObj.attr("id")) {
                showObj.attr("id", "ShowID_" + parseInt(Math.random() * 10000000));
            }
            url += $.param({ suppliertype: 1, hideID:$("#"+hideObj.attr("id")).val(),pIframeId:'<%= EyouSoft.Common.Utils.GetQueryStringValue("iframeId") %>',callback:"DijieControl.CallBackDijie" })
            top.Boxy.iframeDialog({
                iframeUrl: url,
                title: "选择地接社",
                modal: true,
                width: "880",
                height: "350"
            });
        });
        },
        CallBackDijie:function(obj){
            if (obj) {
                $("#" + obj.aid).closest("tr").find("input[name='txtDiejie_unit']").val(obj.name);
                $("#" + obj.aid).closest("tr").find("input[name='hidDijie_id']").val(obj.id);
                $("#" + obj.aid).closest("tr").find("input[name='txtDijie_planer']").val(obj.contactname);
                $("#" + obj.aid).closest("tr").find("input[name='txtDijie_Tel']").val(obj.contacttel);
                $("#" + obj.aid).closest("tr").find("input[name='txtDijie_QQ']").val(obj.contactqq);
            }
        }
    }
    
</script>

