<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="Uploader.ascx.cs" Inherits="Song.Site.Manage.Utility.Uploader" %>
<script type="text/javascript" src="<%=uploaderPath %>swfupload/swfupload.js"></script>
<script type="text/javascript" src="<%=uploaderPath %>js/swfupload.queue.js"></script>
<script type="text/javascript" src="<%=uploaderPath %>js/fileprogress.js"></script>
<script type="text/javascript" src="<%=uploaderPath %>js/filegroupprogress.js"></script>
<script type="text/javascript" src="<%=uploaderPath %>js/handlers.js"></script>

<div class="uploadbar" style="height: 30px;">
    <div class="swfPlace" style="height: 30px; width: 160px; line-height: 30px; float: left;">
        <span id="spanButtonPlaceHolder"></span>
    </div>
    <div class="selectFileBtn" style="height: 30px; width: 160px; line-height: 20px;
        float: left;">
        <a href="<%=uploaderPath %>FileExplorer.aspx?path=<%=UploadPath %>" hg="640" wd="480"
            target="_blank">选择服务器端文件</a></div>
    <div class="selectFileBtn" style="height: 30px; width: 160px; line-height: 20px;
        float: left;">
        <a href="<%=uploaderPath %>OuterLink.aspx?path=<%=UploadPath %>&uid=<%=UID %>" hg="600"
            wd="300" target="_blank">站外视频链接</a></div>
    <div class="selectFileBtn" style="height: 30px; width: 160px; line-height: 20px;
        float: left;">
        <a href="<%=uploaderPath %>OtherLink.aspx?path=<%=UploadPath %>&uid=<%=UID %>" hg="600"
            wd="300" target="_blank">视频平台链接</a></div>
</div>
<script type="text/javascript">
    var uploaderPath="<%=uploaderPath %>";
    var swfu;
    (function () {
        var settings = {
            flash_url: "<%=uploaderPath %>swfupload/swfupload.swf",
            upload_url: "<%=uploaderPath %>Uploading.ashx?path=<%=UploadPath %>&uid=<%=UID %>&count=<%= LimitCount %>",
            file_size_limit: "0",
            file_types: "*.mp4",
            file_types_description: "视频文件",
            file_upload_limit: <%= LimitCount %>,
            file_queue_limit: 0,
            custom_settings: {

                progressTarget: "divprogresscontainer",
                progressGroupTarget: "divprogressGroup",

                //progress object
                container_css: "progressobj",
                icoNormal_css: "IcoNormal",
                icoWaiting_css: "IcoWaiting",
                icoUpload_css: "IcoUpload",
                fname_css: "fle ftt",
                state_div_css: "statebarSmallDiv",
                state_bar_css: "statebar",
                percent_css: "ftt",
                href_delete_css: "ftt",

                //sum object
                /*
                页面中不应出现以"cnt_"开头声明的元素
                */
                s_cnt_progress: "cnt_progress",
                s_cnt_span_text: "fle",
                s_cnt_progress_statebar: "cnt_progress_statebar",
                s_cnt_progress_percent: "cnt_progress_percent",
                s_cnt_progress_uploaded: "cnt_progress_uploaded",
                s_cnt_progress_size: "cnt_progress_size"
            },
            debug: false,

            // Button settings
            //button_image_url: "<%=uploaderPath %>images/TestImageNoText_65x29.png",
            button_width: "200",
            button_height: "29",
            button_placeholder_id: "spanButtonPlaceHolder",
            button_text: '<span class="theFont">点击此处上传文件</span>',
            button_text_style: ".theFont { font-size: 12;color:#0068B7; }",
            button_text_left_padding: 12,
            button_text_top_padding: 3,
            button_cursor : SWFUpload.CURSOR.HAND,
            button_window_mode : SWFUpload.WINDOW_MODE.TRANSPARENT,
            // The event handler functions are defined in handlers.js
            file_queued_handler: fileQueued,
            file_queue_error_handler: fileQueueError,
            upload_start_handler: uploadStart,
            upload_progress_handler: uploadProgress,
            upload_error_handler: uploadError,
            upload_success_handler: uploadSuccess,
            upload_complete_handler: uploadComplete,
            file_dialog_complete_handler: fileDialogComplete
        };
        swfu = new SWFUpload(settings);
    })();
    function fileQueueError(fileobject, errorcode, message) {
        alert(errorcode);
    }
    function uploadSuccess(file, serverData) {
        var btn = $(".swfPlace");
        btn.remove();
        //上传后的文件
        var file = serverData;
        var width=500;
        var height=400;
        var str = "<object wmode='transparent' classid='clsid:D27CDB6E-AE6D-11cf-96B8-4445535411111'  codebase='http://download.macromedia.com/pub/shockwave/cabs/flash/swflash.cab#version=7,0,19,0'";
        str += "  width=" + width + " height=" + height + " >";
          str += "<param name='movie' value='<%=uploaderPath %>flvplayer.swf?vcastr_file=" + file + "' />";
          str += "<param name='quality' value='high' />";
          str += "<param name='wmode' value='transparent' />";
          str += "<param name='allowFullScreen' value='true' />";
          str += "<param name='FlashVars' value='vcastr_file=" + file + "&IsAutoPlay=1&IsContinue=1' />";
          str += "<embed src='<%=uploaderPath %>flvplayer.swf?vcastr_file="+file+"' allowfullscreen='true'";
          str += " flashvars='vcastr_file=" + file + "&IsAutoPlay=1&IsContinue=1' quality='high' pluginspage='http://www.macromedia.com/go/getflashplayer' ";
          str += " type='application/x-shockwave-flash'   wmode='transparent' width=" + width + " height=" + height + " />";
          str += "</object>";
          $("#divPlayer").html(str);
    }
</script>
<div id="divprogresscontainer">
</div>
<div id="divprogressGroup">
</div>
<div id="divPlayer">
</div>
<script language="javascript" type="text/javascript">
    //弹出文件选择的窗口
    $(".selectFileBtn a").click(function () {
        var src = $(this).attr("href");
        var title = $(this).text();
        var height = Number($(this).attr("hg"));
        var width = Number($(this).attr("wd"));
        var box = new top.PageBox(title, src, height, width, null, window.name);
        box.Open();
        return false;
    });
    //设置选择的文件
    function setSelectFile(file, path, winname) {
        new top.PageBox().Close(winname);
        //alert(path + file);
        uploadSuccess(file, path + file);
        //
        var select_url = "<%=uploaderPath %>SelectFile.ashx?path=<%=UploadPath %>&uid=<%=UID %>&count=<%= LimitCount %>&file=" + file;
        //alert(select_url);
        $.get(select_url, function () {
            alert("设置成功");
        });
    }
    //设置外部链接
    function setOuterLink(file, path, winname) {
        new top.PageBox().Close(winname);
        var video = $("a.video");
        if (video.size() > 1) {
            $("a.video").html(file + " <span style='color:red'>(外部链接)</span>");
            $("a.video").attr("href", path);
            alert("设置外部链接成功！");
        } else {
            window.location.reload();
        }
    }
</script>
