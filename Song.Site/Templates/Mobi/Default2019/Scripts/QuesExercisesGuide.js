$(function () {
    if (typeof guide != "undefined") guide.init();
});

var guide = {
    init: function () {
        this.builder();
    },
    html: function () {
        var html = "<div id='guide-box'>";
		html+="<div class='slide-top-box'><i>&#xe6d8;</i>设置视图模式</div>";
		html+="<div class='slide-chest-box'><i>&#xe63d;</i>滑动切换试题<i>&#xe6aa;</i></div>";
		html+="<div class='slide-waist-box'><row><i>&#xe631;</i>缩小字体</row><row>放大字体<i>&#xe636</i></row></div>";
		html+="<div class='slide-foot-box'><span>答题卡</span><i>&#xe786;</i></div>";
		html+="</div>";
        return html;
    },
    builder: function () {
        $("body").append(guide.html());
        var box = $("#guide-box");
        var hg = $(window).height() > document.body.clientHeight ? $(window).height() : document.body.clientHeight;
        var wd = $(window).width() > document.body.clientWidth ? $(window).width() : document.body.clientWidth;
        box.width(wd).height(hg);
        box.css({ "position": "absolute", "z-index": "100" });
        box.css({ left: 0, top: 0 });
        var alpha = 50;
        //box.css("background-color", "#000");
        //box.css("filter", "Alpha(Opacity=" + alpha + ")").css("display", "block");
        //box.css("-moz-opacity", alpha / 100).css("opacity", alpha / 100);
        box.show();
		this.events(box);
    },
    events: function (box) {
		box.click(function(){			
			$(this).fadeOut(500);
		});
		window.setTimeout(function(){
			//$("#guide-box").fadeOut(500);
		},5000);
    }
};