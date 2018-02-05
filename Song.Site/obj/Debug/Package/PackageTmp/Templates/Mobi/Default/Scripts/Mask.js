//生成遮罩层
function Mask() { };
//生成默认遮罩
Mask.Open = function () {
    $("body").css("overflow", "hidden");
    var mask = $(".screenMask");
    if (mask.size() < 1) {
        $("body").append("<div class=\"screenMask\" style=\"display:none;\"></div>");
        mask = $(".screenMask");
    }
    //屏幕的宽高
    var hg = document.documentElement.clientHeight * 100;
    var wd = document.documentElement.clientWidth * 100;
    mask.css({ "position": "absolute", "z-index": "1000",
        "width": wd, "height": hg, top: $(window).scrollTop(), left: 0
    });
    var alpha = 60;
    mask.css({ "background-color": "#999", "filter": "Alpha(Opacity=" + alpha + ")",
        "display": "block", "-moz-opacity": alpha / 100, "opacity": alpha / 100
    });
    mask.fadeIn("slow");
}
//消息框
Mask.ShowBox = function (height, width, title, msg, img) {
    Mask.Open();
    var showbox = $(".ShowBox");
    if (showbox.size() < 1) {
        $("body").append("<div class=\"ShowBox\" style=\"display:none;\"/>");
        showbox = $(".ShowBox");
        showbox.append("<div class=\"ShowBoxTitle\">" + title + "</div>");
        if (msg != null) {
            showbox.append("<div class=\"ShowBoxMsg\"> 正在提交答题信息…… </div>");
        }
        if (img != null) {
            var spt = $("script[path]");
            showbox.append("<img  class=\"ShowBoxImg\" src=\"" + spt.attr("path") + "images/" + img + "\" />");
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
        "width": width, "height": height, "background-color": "#fff",
        "border": "2px solid #666", "text-align": "center"
    });
    showbox.find(".ShowBoxTitle").css({ "line-height": "35px",
        "font-size": "18px", "margin-bottom": "0px", "margin-top": "10px"
    });
    showbox.find(".ShowBoxMsg").css({ "line-height": "20px",
        "font-size": "14px", "margin-bottom": "10px", "margin-top": "10px"
    });
    showbox.find(".ShowBoxImg").css({ "margin-top": "2px" });
    //位置
    showbox.css("top", (hg - showbox.height()) / 2 + $(window).scrollTop());
    showbox.css("left", (wd - showbox.width()) / 2);
    showbox.show();
}
//生成加载的效果
Mask.Loading = function (height, width) {
    Mask.Open();
    Mask.ShowBox(height, width, "正在加载试题……", null, "load_024.gif");
}
Mask.LoadingClose = function () {
    if ($(".screenMask").size() > 0) $(".screenMask").remove();
    if ($(".ShowBox").size() > 0) $(".ShowBox").remove();
    $("body").css("overflow", "auto");
}
Mask.Submit = function (height, width) {
    Mask.Open();
    Mask.ShowBox(height, width, "考试结束", "正在提交答题信息……", "load_016.gif");
}
Mask.SubmitClose = function () {
    if ($(".screenMask").size() > 0) $(".screenMask").remove();
    if ($(".ShowBox").size() > 0) $(".ShowBox").remove();
    $("body").css("overflow", "auto");
    alert("交卷成功！");
}
Mask.InResult = function () {
    $("#inResultLoading").show();
}
Mask.InResultClose = function () {
    $("#inResultLoading").hide();
}