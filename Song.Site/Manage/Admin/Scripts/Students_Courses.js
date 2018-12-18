$(function () {

    //取消课程学习的按钮事件
    $(".item a.selected").click(function () {

        if (confirm("是否确定终止该课程的学习？")) {
            return true;
        }
        return false;
    });
    //初始样式
    $(".item").each(function () {
        var isFree = $(this).attr("isfree") == "True" ? true : false;
        var isTry = $(this).attr("istry") == "True" ? true : false;
        if (isFree) $(this).addClass("isFree");
        if (isTry) $(this).addClass("isTry");
    });
    //打印详情的按钮事件
    $("#btnPrint").click(function () {
        var href = $(this).attr("href");
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