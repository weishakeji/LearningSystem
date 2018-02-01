//异步加载
function ajax() { }
ajax.post = function (url, options, successFunc) {
    $.ajax({
        type: "POST",
        url: url,
        dataType: "text",
        data: options,
        //开始，进行预载
        beforeSend: function (XMLHttpRequest, textStatus) {
            Loading.Open();
        },
        //加载出错
        error: function (XMLHttpRequest, textStatus, errorThrown) {
            Loading.Close();
        },
        //加载成功！
        success: function (data) {
            Loading.Close();
            if (successFunc != null) successFunc(data);
        }
    });
}
//生成预加载
function Loading() { };
//生成默认遮罩
Loading.Open = function () {
    var mask = $(".LoadingScreenMask");
    if (mask.size() < 1) {
        $("body").append("<div class=\"LoadingScreenMask\" style=\"display:none;\"></div>");
        mask = $(".LoadingScreenMask");
    }
    //屏幕的宽高
    var hg = document.documentElement.clientHeight * 100;
    var wd = document.documentElement.clientWidth * 100;
    mask.css({ "position": "absolute", "z-index": "1000",
        "width": wd, "height": hg, top: $(window).scrollTop(), left: 0
    });
    var alpha = 60;
    mask.css({ "background-color": "#fff", "filter": "Alpha(Opacity=" + alpha + ")",
        "display": "block", "-moz-opacity": alpha / 100, "opacity": alpha / 100
    });
    mask.fadeIn("slow");
    Loading.ShowBox(40, 60, "", "", "load_027.gif");
}
//消息框
Loading.ShowBox = function (height, width, title, msg, img) {
    var showbox = $(".LoadingShowBox");
    if (showbox.size() < 1) {
        $("body").append("<div class=\"LoadingShowBox\" style=\"display:none;\"/>");
        showbox = $(".LoadingShowBox");
        showbox.append("<div class=\"ShowBoxTitle\">" + title + "</div>");

        if (img != null) {
            //var spt = $("script[path]");
            showbox.append("<img  class=\"ShowBoxImg\" src=\"/templates/mobi/_public/images/" + img + "\" />");
        }
        if (msg != null) {
            showbox.append("<div class=\"ShowBoxMsg\"> 正在提交…… </div>");
        }
    }
    //屏幕的宽高
    var hg = $(window).height();
    var wd = $(window).width();
    //如果为空，则为浏览器窗口一半宽高
    width = width != null && width != 0 && width != "" ? Number(width) : 400;
    height = height != null && height != 0 && height != "" ? Number(height) : 110;
    //如果宽高小于100，则默认为浏览器窗口的百分比	
    width = width > 100 ? Number(width) : $("body").width() * Number(width) / 100;
    height = height > 100 ? Number(height) : hg * Number(height) / 100;
    //样式
    showbox.css({ "position": "absolute", "z-index": "2000",
        "width": width, "height": height, "text-align": "center"
    });
    showbox.find(".ShowBoxTitle").css({ "line-height": "35px",
        "font-size": "18px", "margin-bottom": "0px", "margin-top": "10px"
    });
    showbox.find(".ShowBoxMsg").css({ "line-height": "20px",
        "font-size": "18px", "margin-bottom": "10px", "margin-top": "10px"
    });
    showbox.find(".ShowBoxImg").css({ "margin-top": "2px" });
    //位置
    showbox.css("top", (hg - showbox.height()) / 2 + $(window).scrollTop());
    showbox.css("left", (wd - showbox.width()) / 2);
    showbox.show();
}
Loading.Close = function () {
    if ($(".LoadingScreenMask").size() > 0) $(".LoadingScreenMask").remove();
    if ($(".LoadingShowBox").size() > 0) $(".LoadingShowBox").remove();
}
