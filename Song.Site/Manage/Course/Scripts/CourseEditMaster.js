//设置菜单链接
(function () {
    //获取当前网页
    var arrUrl = window.location.href.split("/");
    var strPage = arrUrl[arrUrl.length - 1].toLowerCase();
    if (strPage.indexOf("?") > -1) {
        strPage = strPage.substring(0, strPage.indexOf("?"));
    }
    //
    $("#editLeft a").removeClass("curr");
    $("#editLeft a").each(function () {
        var href = $(this).attr("href");
        if (href != null) {
            if (href.toLowerCase() == strPage) {
                $(this).addClass("curr");
            }
            //设置转换地址
            var couid = $().getPara("couid");
            if (Number(couid) > 0) {
                $(this).attr("href", href + "?couid=" + couid);
            }
        }
    });
})();
