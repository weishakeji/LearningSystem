$(function () {
    $("a").click(a_click);
    //默认打开的窗口，显示课程
    var openurl = $().getPara("openurl");
    if (openurl.length > 0) {
        openurl = BASE64.decoder(decodeURIComponent($.trim(openurl)));
        new PageBox("课程详情", openurl, 100, 100, "url").Open();
    }
    //当手机端处在iframe中，不跳转
    if (window.frames.length != parent.frames.length) {
        $("*[href]").each(function (index, element) {
            var href = $(this).attr("href");
            if (href == undefined || href == null || href == "" || href == "#") return true;
            if (href.length > 0 && href.substring(0, 1) == "#") return true;
            if (href.indexOf("javascript:") > -1) return true;
            href = $().setPara(href, "skip", "no");
            $(this).attr("href", href);
        });
    }
    //图片加载错误时，显示默认图片
    $("img").error(function () {
        var errImg = $(this).attr("default");
        if (errImg == null) return false;
        $(this).attr("src", errImg);
    });
});
//超链接的事件
function a_click(event) {
    var v = this.onclick;
    var type = $.trim(this.getAttribute("type"));
    var target = $.trim(this.getAttribute("target"));
    if (type == "link" || type == null) {
        if (target == null || target == "" || target == "_blank" || type == "_self")
            document.location.href = this.href;
        if (target == "_top" || type == "_parent") top.location.href = this.href;
        if (target == "_open") {
            var href = this.href;
            var txt = $.trim($(this).attr("title"));
            if (txt == "") txt = $(this).html();
            new PageBox(txt, href, 100, 100, "url").Open();
        }
    }
    if (type == "open" || target == "_open") {
        var href = this.href;
        var txt = $.trim($(this).attr("title"));
        if (txt == "") txt = $(this).html();
        new PageBox(txt, href, 100, 100, "url").Open();
    }
    if (type == "back") {
        try {
            mui.back();
        } catch (ex) {
            var ref = document.referrer;
            if (ref == null || ref == window.location.href) {
                window.location.href = "defaut.ashx";
            } else {
                window.location.href = document.referrer;
                //window.history.go(-1);
            }
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
        $("video").hide();
        PageBox.OverEvent = function () {
            $("video").show();
        }
        box.Open();
        return false;
    }
    //拨打电话
    if (type == "tel") {
        var btnArray = ['拨打', '取消'];
        var phone = $(this).attr("href");
        if (phone.indexOf(":") > -1) phone = phone.substring(phone.lastIndexOf(":") + 1);
        if (phone.indexOf("?") > -1) phone = phone.substring(0, phone.indexOf("?"));
        new PageBox("拨打电话", "CallPhone.ashx?phone=" + phone, 80, 150, "url").Open();
    }
    //点击事件
    if (type == "click") {
        var href = this.href;
        this.click();
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
            $.get("/Ajax/StudentOnline.ashx?plat=Mobi&interval=" + interval);
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

//给所有超链接增加当前登录账号的id
function ToAllLinkAddAccoutsID(acid) {
    //给所有超链接增加当前登录账号的id
    if (!(acid == "" || acid == undefined || acid == null || acid == 0)) {
        $("*[href]").each(function (index, element) {
            var name = $(this).get(0).tagName.toLowerCase();
            if (name == "link") return true;
            var href = $(this).attr("href");
            if (href == undefined || href == null || href == "" || href == "#") return true;
            if (href.length > 0 && href.substring(0, 1) == "#") return true;
            if (href.indexOf("javascript:") > -1) return true;
            href = $().setPara(href, "sharekeyid", acid);
            $(this).attr("href", href);
        });
    }
    //获取分享id
    var sid = $().getPara("sharekeyid");    //来自地址来的分享人id
    var cookie = $().cookie("sharekeyid");  //存储于本地的分享人id
    if (cookie == "" || cookie == null || typeof (cookie) == "undefined") cookie = $.storage("sharekeyid");
    if (cookie == "" || cookie == null || typeof (cookie) == "undefined") cookie = 0;
    if (sid != "" && cookie == "0" && sid != cookie) {
        //给分享的人增加积分
        $.get("/ajax/AddSharePoint.ashx", { sharekeyid: sid });
    }
    if (sid != "" && sid != cookie) {
        $().cookie("sharekeyid", sid);
        $.storage("sharekeyid", sid);
    }
}
