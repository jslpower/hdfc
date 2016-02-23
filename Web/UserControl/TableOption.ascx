<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="TableOption.ascx.cs"
    Inherits="Web.UserControl.TableOption" %>
<table cellspacing="0" cellpadding="0" border="0" class="xtnav" id="Table_Option">
    <tbody>
        <tr>
            <td align="center">
                <a href="/CustomerManage/YuangongList.aspx" data-class="yuangong">员工</a>
            </td>
            <td align="center">
                <a href="/CustomerManage/DaoYouList.aspx" data-class="daoyou">导游</a>
            </td>
            <td align="center">
                <a href="/CustomerManage/ZutuansheList.aspx" data-class="zutuanshe">组团社</a>
            </td>
            <td align="center">
                <a href="/CustomerManage/YouKeList.aspx" data-class="youke">游客</a>
            </td>
            <td align="center">
                <a href="/CustomerManage/DijiesheList.aspx" data-class="dijie">地接社</a>
            </td>
            <td align="center">
                <a href="/CustomerManage/JingdianList.aspx" data-class="jingdian">景点</a>
            </td>
        </tr>
    </tbody>
</table>
<script type="text/javascript">
    $(function(){
        var type='<%=Type %>';
        $("#Table_Option").find("a").each(function(){
            if($(this).attr("data-class")==type){
                $(this).closest("td").addClass("xtnav-on")
            }
        })
    })
</script>
