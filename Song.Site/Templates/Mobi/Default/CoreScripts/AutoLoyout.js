/*!
* 主  题：《页面自适应全屏》
* 说  明：让页面自动适应浏览器全屏，程序自动计算页面各布局元素的宽高使之铺满全屏。
* 功能描述：
* 1、布局元素（例如div）可以设置为行loyout=row，或为列loyout=column；
* 2、如果为行，则可以设置高度是实际像素或auto；当设置实际像素时，窗口放大缩小，该div不受影响，如果为auto时，则根据窗口变化自动适应；
* 3、如果为列时，效果雷同
*
* 作者：宋雷鸣
* Q Q: 10522779
* 开发时间: 2013年7月9日
*/
$(function () {
   
});
$(window).load(function () {
    window.winWidth = typeof (window.winWidth) == "undefined" ? $(window).width() : window.winWidth;
    window.winHeight = typeof (window.winHeight) == "undefined" ? $(window).height() : window.winHeight;
	 mask_show();
    initLoyout();
    $("body").show();
    mask_hide();
    $("#bodyBox").css("visibility", "");
});
//当窗口大小变化时
$(window).resize(function () {
    var winWd = $(window).width();
    var winHg = $(window).height();
    if (winWd == window.winWidth && winHg == window.winHeight) return;
    window.winWidth = winWd;
    window.winHeight = winHg;

    initLoyout();

    /*window.windowResizeTemp=typeof(window.windowResizeTemp)=="undefined"  ?  0 : window.windowResizeTemp;
    //浏览器判断
    if($.browser.mozilla){ initLoyout();return;}
    if($.browser.msie){
    if(++window.windowResizeTemp%4==0)initLoyout();
    return;
    }
    window.windowResizeTemp=typeof(window.windowResizeTemp)=="undefined"  ?  0 : window.windowResizeTemp+1;
    //alert(window.windowResizeTemp);
    if(window.windowResizeTemp%2==0)
    {
    initLoyout();
    }*/
});
//初始化布局
function initLoyout() {
    //alert(window.windowResizeTemp);
    var start = new Date();
    //屏幕的宽高
    var hg = $(window).height();
    var wd = $(window).width();
    var tmbody = $("body");
    tmbody.width(wd).height(hg);
    tmbody.find("*[loyout='column']").height(1).css("overflow", "hidden");
    tmbody.find("*[loyout='column']").width(1).css("overflow", "hidden");
    tmbody.find("*[loyout='row']").height(1);
    setloyout(null, wd, hg);

    //计算布局耗时
    var finish = new Date();
    //alert(finish-start);
    //alert(window.windowResizeTemp);
}
function mask_show() {
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
    var alpha = 100;
    mask.css({ "background-color": "#fff", "filter": "Alpha(Opacity=" + alpha + ")",
        "display": "block", "-moz-opacity": alpha / 100, "opacity": alpha / 100
    });
    mask.show();
}
function mask_hide() {
    $(".screenMask").fadeOut(1000);
}
function setloyout(element, wd, hg) {

    hg = element != null ? element.innerHeight() : hg;
    wd = element != null ? element.innerWidth() : wd;
    //当元素下的布局元素
    var nodes = new LoyoutItem(element).Nodes;
    //行元素与列元素
    var row = new Array();
    var column = new Array();
    for (var i in nodes) {
        var tm = nodes[i];
        if (tm.loyout == "row") row.push(tm);
        if (tm.loyout == "column") column.push(tm);
    }
    setAutoWidth(column, wd, hg);
    setAutoHeight(row, wd, hg);
    for (var i in nodes) {
        var tm = nodes[i].self;
        setloyout(tm);
    }
}
//布局行进行自动高度
function setAutoHeight(arr, wd, hg) {
    if (arr.length < 1) return;
    var sum = 0;
    for (var i in arr) {
        arr[i].self.width(wd);
        if (arr[i].height != "auto") {
            arr[i].self.height(arr[i].height);
            hg -= arr[i].self.height();
        } else {
            sum += arr[i].self.height();
        }
    }
    if (hg <= 0) return;
    for (var i in arr) {
        if (arr[i].height == "auto") {
            var selfPer = arr[i].self.height() / sum;
            arr[i].self.height(Math.floor(selfPer * hg));
        }
    }
}
//布局行进行自动宽度
function setAutoWidth(arr, wd, hg) {
    if (arr.length < 1) return;
    var sum = 0;
    for (var i in arr) {
        arr[i].self.css("float", "left");
        arr[i].self.height(hg);
        if (arr[i].width != "auto") {
            arr[i].self.width(arr[i].width);
            wd -= arr[i].self.width();
        } else {
            sum += arr[i].self.width();
        }
    }
    if (wd <= 0) return;
    for (var i in arr) {
        if (arr[i].width == "auto") {
            var selfPer = arr[i].self.width() / sum;
            arr[i].self.width(Math.floor(selfPer * wd));
        }
    }
}

/*!
* 主  题：《页面中的布局对象》
* 说  明：页面中布局元素（例如div）的相关布局属性
* 功能描述：
* 1、在页面中可以设置div为行loyout=row，或为列loyout=column；
* 2、如果为行，则可以设置高度是实际像素或auto
* 3、如果为列，则可以设置宽高是实际像素或auto
*
* 作者：宋雷鸣
* Q Q: 10522779
* 开发时间: 2013年7月9日
*/
//布局元素的处理对象
function LoyoutItem(node) {
    this.Element = node != null ? node : $("body");
    //获取布局节点
    this.Nodes = LoyoutItem._init(this.Element);
}
//根节点，默认为整个网页
LoyoutItem.prototype.Element = $("body");
//布局节点
LoyoutItem.prototype.Nodes = null;
//初始化数据；
LoyoutItem._init = function (element) {
    var nodes = new Array();
    element.find(" > *[loyout]").each(
		function () {
		    var wd = $(this).attr("width") == null ? "auto" : $(this).attr("width");
		    var hg = $(this).attr("height") == null ? "auto" : $(this).attr("height");
		    var loyout = $(this).attr("loyout");
		    nodes.push({ width: typeof (wd) == "string" ? wd.replace("px", "") : wd,
		        height: typeof (hg) == "string" ? hg.replace("px", "") : hg,
		        loyout: loyout,
		        self: $(this)
		    });
		}
	);
    return nodes;
}
