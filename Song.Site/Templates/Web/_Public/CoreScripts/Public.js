$(function () {
    //图片加载错误时，显示默认图片
    $("img").error(function () {
        var errImg = $(this).attr("default");
        if (errImg == null) return false;
        $(this).attr("src", errImg);
    });
});
