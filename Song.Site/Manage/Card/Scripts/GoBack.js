$(function () {
    //关闭窗体
    $("input[name=btnClose]").click(function () {
        new top.PageBox().Close(window.name);
        return false;
    });
});

