<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ExcelFileUploadControl.ascx.cs"
    Inherits="Web.UserControl.ExcelFileUploadControl" %>
<div style="margin: 3px 0px; float: left; width: 180px;">
    <div>
        <input type="hidden" id="<%=this.ClientHideID %>" />
        <span runat="server" id="spanButtonPlaceholder"></span><span id="errMsg_<%=this.ClientHideID %>"
            class="errmsg"></span>
    </div>
    <div id="divFileProgressContainer" runat="server">
    </div>
    <div id="thumbnails">
    </div>
</div>
<div>
    只能导入格式为.xls、xlsx<%=ContainsTxt?"、txt":""%>的文件</div>
<asp:Literal ID="litDown" runat="server"></asp:Literal>
<div style="clear: left">
    <input type="button" onclick="startExcelFileUpload();return false;" value="导入" id="btnLoadExcel"
        style="width: 80px; height: 30px" />
</div>

<script type="text/javascript">
	var <%=this.ClientID %>;
			    <%=this.ClientID %> = new SWFUpload({
		
				    upload_url: "/ashx/ExcelConvert.ashx",
				    file_post_name:"Filedata",
                    post_params : {
                        "ASPSESSID" : "<%=Session.SessionID %>",
                        "CompanyID":"<%=this.CompanyID %>"
                    },
				    // File Upload Settings
				    file_size_limit : "2 MB",
				    file_types : "<%=FileTypes %>",
				    file_types_description : "附件上传",
				    file_upload_limit : "1",    // Zero means unlimited

				    // Event Handler Settings - these functions as defined in Handlers.js
				    //  The handlers are not part of SWFUpload but are part of my website and control how
				    //  my website reacts to the SWFUpload events.
				    swfupload_loaded_handler :function(){},
				    file_dialog_start_handler : fileDialogStart,
				    file_queued_handler : fileQueued,
				    file_queue_error_handler : fileQueueError,
				    file_dialog_complete_handler :null,
				    upload_progress_handler : uploadProgress,
				    upload_error_handler : uploadError,
				    upload_success_handler : ExcelFileUploadSuccess,
				    upload_complete_handler : uploadComplete,
				    // Button settings
				    button_image_url : "/images/swfupload/XPButtonNoText_<%=Setimgwidth %>_<%=SetImgheight %>.gif",
				    button_placeholder_id : "<%=spanButtonPlaceholder.ClientID %>",
				    button_width: <%=Setimgwidth %>,
				    button_height: <%=SetImgheight %>,
				    button_text : '<span></span>',
				    button_text_style : '',
				    button_text_top_padding: 1,
				    button_text_left_padding: 3,
				    button_cursor: SWFUpload.CURSOR.HAND, 

				    // Flash Settings
				    flash_url : "/js/swfupload/swfupload.swf",	// Relative to this file

				    custom_settings : {
					    upload_target : "<%=divFileProgressContainer.ClientID %>",
				        HidFileNameId:"<%=this.ClientHideID %>",
				        HidFileName:"<%=this.ClientHideID %>",
				        ErrMsgId:"errMsg_<%=this.ClientHideID %>",
				        UploadSucessCallback:function(){}
				    },

				    // Debug Settings
				    debug: false,
    				
				    // SWFObject settings
		            minimum_flash_version : "9.0.28",
		            swfupload_pre_load_handler : swfUploadPreLoad,
		            swfupload_load_failed_handler : swfUploadLoadFailed
			    });
			    
			    try{
			        currentSwfuploadInstances.push( <%=this.ClientID %>);
			    }
			    catch(e){}
			    
	function startExcelFileUpload(){
        var swfFile = <%=this.ClientID %>;
        if (swfFile.getStats().files_queued > 0) {
            swfFile.startUpload();
        }
        else{
            alert("请选择文件后再点击导入！");
        }
    }    
    function ExcelFileUploadSuccess(file,serverData){
        try 
          {
           
            var obj = eval(serverData);
	        if(obj.error){
	            alert(obj.error);
	            resetSwfupload(<%=this.ClientID %>,file);
                return;
            }
            else
            {  
               var progress = new FileProgress(file,  this.customSettings.upload_target);
               progress.setStatus("上传成功.");
               try{
                if("<%= _uploadSuccessJavaScriptFunCallBack %>" != "")
                {
                    <%=_uploadSuccessJavaScriptFunCallBack+"(obj);" %>;
                }
               }catch(e){}
               
               resetSwfupload(<%=this.ClientID %>,file);
            }
          } 
          catch (ex){ }
	    }
			    
</script>

