$(function () {
    //打印详情的按钮事件
    $("a.print_details").click(function () {
        var href=$(this).attr("href");
        var iframe = $("#iframeExportDetails");
        iframe.attr("src", href);
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
//导入
function OnInput() {
    //$("input[name$=btnInput]").click(function () {
        OpenWin('Student_Input.aspx', '数据导入', 800, 600);
        return false;
    //});
}
//导出事件
function OnOutput() {
    var LocString = String(window.document.location.href);
    if (LocString.indexOf("?") > -1) {
        var para = LocString.substring(LocString.lastIndexOf("?"));
        OpenWin('Student_Export.aspx' + para, '学员导出', 800, 600);
    } else {
        OpenWin('Student_Export.aspx', '学员导出', 800, 600);
    }
    return false;
}