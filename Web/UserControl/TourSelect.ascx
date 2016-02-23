<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="TourSelect.ascx.cs"
    Inherits="Web.UserControl.TourSelect" %>
<input type="text" id="<%=ClientTourCode %>" readonly="readonly" style="background-color: #dadada"
    class="inputtext formsize120" name="<%=ClientTourCode %>" value="<%=this.TourCode %>"
    <%if(IsMust){ %><%=NoticeHTML %><%} %> />
<input type="hidden" id="<%=ClientTourID %>" name="<%=ClientTourID %>" value="<%=this.TourId %>" />
<a id="<%=btnID %>" class="Offers xuanyong" href="javascript:void(0);" <%=IsEnable?"":"style='display:none'" %> ></a>

<script type="text/javascript">
    $(function() {      
        $("#<%=btnID %>,#<%=ClientTourCode %>").click(function(){
            var url = "/jidiaoCenter/SelectTour.aspx?";
            var hideObj = $("#<%=ClientTourID %>");
            var showObj = $("#<%=ClientTourID %>").attr("id");
            url += $.param({ callBack: "<%=CallBack %>", hideID:$("#"+showObj).val(),pIframeID: "<%=IframeID %>",type:'<%=SupplierType %>' })
            parent.Boxy.iframeDialog({
                iframeUrl: url,
                title: "选择团号",
                modal: true,
                width: "820",
                height: "378"
            });
        })
        var isenable='<%=IsEnable %>';
        isenable= isenable.toLocaleLowerCase();
        if(isenable=="false"){
            $("#<%=btnID %>,#<%=ClientTourCode %>").unbind("click");
        }

    });
    window["<%=ClientID %>"] = {
        _callBack: function(_data) {
            if (_data) {
                $("#<%=ClientTourCode %>").val(_data.tourcode);
                $("#<%=ClientTourID %>").val(_data.tourid);
            }
        }
    };
</script>

