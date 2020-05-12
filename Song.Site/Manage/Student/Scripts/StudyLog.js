$(function () {
    //	导出详情的按钮
    $("input[name$=btnOutput]").val("打印").click(function () {
        //$(this).attr("disabled", "disabled");     
        //当前选中的课程id 
        var cous = GetKeyValues('GridView1');
        var iframe = $("#iframeExportDetails");
        var link = iframe.attr("link")+"&cous="+ cous + "&t=" + new Date().getTime();
        //iframe.attr("src", iframe.attr("link") + "&t=" + new Date().getTime());
        
        //alert(cous);
        new top.PageBox("学习证明打印", link, 980, 90, null, window.name).Open();
        return false;
    });
    //主管理区加载完成事件
    $("#iframeExportDetails").load(function () {
        var iframe = $(this)[0];
        var bHeight = iframe.contentWindow.document.body.scrollHeight;
        var dHeight = iframe.contentWindow.document.documentElement.scrollHeight;
        var height = Math.max(bHeight, dHeight);
        iframe.height = height;
        //
        var frameWindow = $(this)[0].contentWindow;
        frameWindow.close();
        frameWindow.focus();
        frameWindow.print();
        $("input[name$=btnOutput]").removeAttr("disabled");
    });
});


//导出事件
function OnOutput() {
//    $(this).attr("disabled", "disabled");
//    var iframe = $("#iframeExportDetails");
//    iframe.attr("src", iframe.attr("link") + "?t=" + new Date().getTime());
//    return false;
}