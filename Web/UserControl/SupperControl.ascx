<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="SupperControl.ascx.cs"
    Inherits="Web.UserControl.SupperControl" %>
<input type="text" id="<%=ClientText %>" readonly="readonly" style="background-color: #dadada"
    class="inputtext formsize120" name="<%=ClientText %>" value="<%=this.Name %>"
    <%if(IsMust){ %><%=NoticeHTML %><%} %> />
<input type="hidden" id="<%=ClientValue %>" name="<%=ClientValue %>" value="<%=this.HideID %>" />
<a id="<%=btnID %>" class="Offers xuanyong" href="javascript:void(0);" <%=IsEnable?"":"style='display:none'" %>></a>

<script type="text/javascript">
    $(function() {
        $("#<%=btnID %>,#<%=ClientText %>").click(function() {
            var url = "/CommonPage/UserSupper.aspx?";
            var hideObj = $("#<%=ClientValue %>");
            var showObj = $("#<%=ClientValue %>").attr("id");
            var type = "<%=SupplierType %>";
            url += $.param({ suppliertype: type, callBack: "<%=CallBack %>", hideID:$("#"+showObj).val(),pIframeID: "<%=IframeID %>", isall: "<%=AllType %>",isadd:"<%=IsAdd %>" })
            parent.Boxy.iframeDialog({
                iframeUrl: url,
                title: "选择供应商",
                modal: true,
                width: "820",
                height: "378"
            });
        })
        var isenable='<%=IsEnable %>';
        isenable= isenable.toLocaleLowerCase();
        if(isenable=="false"){
            $("#<%=btnID %>,#<%=ClientText %>").unbind("click");
        }

    });
    window["<%=ClientID %>"] = {
        _callBack: function(_data) {
            if (_data) {
                $("#<%=ClientText %>").val(_data.name);
                $("#<%=ClientValue %>").val(_data.id);
            }
        }
    };
</script>

