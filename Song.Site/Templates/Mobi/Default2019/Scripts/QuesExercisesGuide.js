$(function () {
    if (typeof guide != "undefined") guide.init();
});

var guide = {
    init: function () {
        this.builder();
    },
    html: function () {
        var html = "<div id='guide-box'>";
        //html+="<div class='slide-top-box'><i>&#xe6d8;</i>设置视图模式</div>";
        html += "<div><i>&#xe63d;</i>滑动切换试题<i>&#xe6aa;</i></div>";
        html += "<div><i>&#xe631;</i>捏合缩放字体<i>&#xe636</i></div>";
        //html+="<div class='slide-foot-box'><span>答题卡</span><i>&#xe786;</i></div>";
        html += "</div>";
        return html;
    },
    builder: function () {
        $("body").append(guide.html());
        var box = $("#guide-box");
        var hg = $(window).height() > document.body.clientHeight ? $(window).height() : document.body.clientHeight;
        var wd = $(window).width() > document.body.clientWidth ? $(window).width() : document.body.clientWidth;
        box.width(wd).height(hg);
        box.css({ "position": "absolute", "z-index": "100", "background-color": "rgba(0,0,0,0.6)", "color": "#fff" });
        box.css({ left: 0, top: 0 });
        box.show();
        //样式
        box.find(">div").each(function (index, element) {
            $(this).css({ "position": "absolute", "width": "100%", "text-align": "center", "text-shadow": "0px 0px 20px #fff", 
				"font-size": "20px", "top": (index + 1) * 160,"padding":"20px 0px"});
            $(this).find("i").css({ "font-family": "iconfont", "font-size": "60px", "margin": "0px 30px 0px 20px",
				"text-shadow": "0px 0px 20px #fff" });
        });
		box.find(">div:first").css({ "background-color": "rgba(0,0,0,0.2)" });
        box.find(">div:last").css({ "font-size": "15px" }).find("i").css({ "font-size": "40px" });
        this.events(box);
    },
    events: function (box) {
        box.click(function () {
            $(this).fadeOut(500);
        });
        window.setTimeout(function () {
            //$("#guide-box").fadeOut(500);
        }, 3000);
    }
};