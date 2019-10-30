$(function () {
    //加载css样式
    var css = $("script[href]").attr("href");
    $().loadcss(css);
});
