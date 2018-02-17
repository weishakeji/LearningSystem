$(function () {
    verifyCode();
    $("a").click(a_click);
});
//超链接的事件
function a_click() {
    var type = $.trim(this.getAttribute("type"));
    var target = $.trim(this.getAttribute("target"));
    if (type == "link" || type == null) {
        if (target == null || target == "" || target == "_blank" || type == "_self")
            document.location.href = this.href;
        if (target == "_top" || type == "_parent") top.location.href = this.href;
        if (target == "_open") {
            var href = this.href;
            var txt = $(this).text();
            var width = $(this).attr("width");
            var height = $(this).attr("height");
            try {
                new PageBox(txt, href, width, height, "url").Open();
            } catch (err) {
                new top.PageBox(txt, href, width, height, "url").Open();
            }
        }
    }
    if (type == "open" || target == "_open") {
        var href = this.href;
        var txt = $(this).text();
        var width = $(this).attr("width");
        var height = $(this).attr("height");
        try {
            new PageBox(txt, href, width, height, "url").Open();
        } catch (err) {
            new top.PageBox(txt, href, width, height, "url").Open();
        }
        return false;
    }
    if (type == "back") {
        try {
            mui.back();
        } catch (ex) {
            window.history.go(-1);
        }
    }
    if (type == "view") {
        var href = this.href;
        if (href.indexOf("?") > -1) href = href.substring(0, href.indexOf("?"));
        if (href.indexOf(".") > -1) {
            var exist = href.substring(href.lastIndexOf(".") + 1).toLowerCase();
            if (exist != "pdf") return false;
        }
        var tit = $.trim($(this).text());
        var pdfview = $().PdfViewer(href);
        var box = new PageBox(tit, pdfview, 100, 100);
        box.Open();
        return false;
    }
}
/*
学员在线浏览时间统计
注：当网页处于焦点时（即最前面）才会记录时间
*/
function StudentOnlineLog() { };
StudentOnlineLog.init = function () {
    $(window).blur(function () { window.cookieInterval = false; });
    $(window).focus(function () { window.cookieInterval = true; });
    $(window).load(function () { setInterval("StudentOnlineLog.event()", 1000); });
}
//记录在线时间的事件
StudentOnlineLog.event = function () {
    if (window.cookieInterval == null || window.cookieInterval) {
        var num = $().cookie('stOnlineNumx');
        if (num == null || num == 'null') num = 1;
        num = Number(num) + 1;
        num = num >= Number.MAX_VALUE ? 0 : num;
        $().cookie('stOnlineNumx', num, { expires: 7, path: '/' });
        //StudentOnlineLog.write(num);
        //ajax提交在线时间
        var interval = 60;
        if (Number(num) % interval == 0) {
            $.get("/Ajax/StudentOnline.ashx?interval=" + interval);
        }
    }
}
//在页面上显示计算信息，仅用于调试
StudentOnlineLog.write = function (num) {
    var box = $("#stOnlineNumBox");
    if (box.size() < 1) {
        $("body").children(":first").before("<div id='stOnlineNumBox'/>");
        box = $("#stOnlineNumBox");
    }
    box.html(num);
}
StudentOnlineLog.init();


//当点击验证码时，刷新验证码
function verifyCode() {
    $(".verifyCode").click(function () {
        var src = $(this).attr("src");
        if (src.indexOf("&") < 0) {
            src += "&timestamp=" + new Date().getTime();
        } else {
            src = src.substring(0, src.lastIndexOf("&"));
            src += "&timestamp=" + new Date().getTime();
        }
        $(this).attr("src", src);
    });
    $(".verifyCode").css("cursor", "pointer");
}