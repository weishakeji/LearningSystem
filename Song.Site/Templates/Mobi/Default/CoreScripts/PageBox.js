/*!
* 主  题：《页面弹出窗口手机版》
* 说  明：用于子页面弹出时的窗口。
* 功能描述：
* 1、生成弹出窗口，窗口内包括iframe控件，用于加载实际控制页面；
* 2、窗口弹出时，生成背景遮罩；
* 3、窗口最小为宽高100，小于等于100时，宽高值默认为浏览器窗口的百分比；
*
*
* 作  者：宋雷鸣
* 开发时间: 2015年9月11日
*/
//窗体Jquery对象
PageBox.prototype.WinBox = null;
//是否允许拖动
PageBox.prototype.IsDrag = true;
//是否显示返回按钮，返回按钮在左侧，返回与关闭的区别是，返回不移除窗口对象，只是隐藏。
PageBox.prototype.IsBackbtn = false;
//窗口关闭时的事件
PageBox.OverEvent = null;
//弹出窗口的主类
//title:窗口标题
//info:要展示的信息，如果type为内嵌页页
//width:窗口宽度
//height:窗口高度
//type:窗口类型，url为Iframe窗口（内嵌页面）,text为文本,obj为元素对象，默认为url
function PageBox(title, info, width, height, type) {
    this.Init(title, info, width, height, type);
}
//初始化参数
PageBox.prototype.Init = function (title, info, width, height, type) {
    this.Title = title != null && title != "" ? title : "";
    this.Info = info;
    //如果为空，则为浏览器窗口一半宽高
    this.Width = !isNaN(Number(width)) ? Number(width) : $(window).width();
    this.Height = !isNaN(Number(height)) ? Number(height) : $(window).height();
    //如果宽高小于100，则默认为浏览器窗口的百分比
    this.Width = this.Width > 100 ? this.Width : document.documentElement.clientWidth * this.Width / 100;
    this.Height = this.Height > 100 ? this.Height : $(window).height() * this.Height / 100;
    this.WinId = new Date().getTime() + "_" + Math.floor(Math.random() * 1000 + 1);
    this.Type = type == null ? "url" : type;
}
//创建窗口，并打开
//func:打开窗体后要执行的方法
PageBox.prototype.Open = function (func) {
    //判断是否已经存在窗口
    var WinBox = $("#PageBox[winId='" + this.WinId + "']");
    if (WinBox.size() > 0) return;
    $("#screenMask").hide();
    $("#PageBox").remove();
    //生成窗口
    this.Mask();
    this.BuildFrame();
    this.BuildIFrame();
    if (arguments.length > 0) {
        func();
    }
    //主管理区加载完成事件
    $("#PageBox iframe").load(function () {
        PageBox.LoadMaskRemove();
    });
}
//iframe加载完成的事件
PageBox.LoadMaskOpen = function () {
    $("body").append("<div id=\"IframLoadMask\"><p class=\"mui-progressbar mui-progressbar-infinite\"></p><p class='loading'>正在加载...</p></div>");
    var mask = $("#IframLoadMask");
    var iframe = $("#PageBox iframe");
    //屏幕的宽高
    var off = iframe.offset();
    mask.css({ "position": "absolute", "z-index": "50000",
        "width": iframe.width(), "height": iframe.height(), top: off.top, left: off.left
    });
    var alpha = 50;
    mask.css({ "background-color": "#ffffff", "filter": "Alpha(Opacity=" + alpha + ")",
        "display": "block", "-moz-opacity": alpha / 100, "opacity": alpha / 100
    });
    mask.find(".loading").css({ "line-height": "200px", "text-align": "center" });
    mask.fadeIn("slow");
}
//移除预载界面
PageBox.LoadMaskRemove = function () {
    $("#IframLoadMask").remove();
}
//生成窗体外框,包括标题
PageBox.prototype.BuildFrame = function () {
    //屏幕的宽高
    var hg = $(window).height();
    var wd = $(window).width();
    $("body").append("<div id=\"PageBox\" type=\"PageBox\" winId=\"" + this.WinId + "\"></div>");
    var PageBox = $("#PageBox");
    this.WinBox = PageBox;
    var border = parseInt(PageBox.css("border-width")); //窗体边线宽度
    border = !isNaN(border) ? border : 0;
    //设置窗口的位置
    PageBox.css("top", ((hg - this.Height) / 2 - border) <= 0 ? 0 : (hg - this.Height) / 2 - border);
    PageBox.css("left", ((wd - this.Width) / 2 - border) <= 0 ? 0 : (wd - this.Width) / 2 - border);
    PageBox.css("position", "absolute").css("z-index", "10001");
    PageBox.css("width", this.Width);
    PageBox.css("height", this.Height);
    this.BuildTitle();
}
//生成标题栏
PageBox.prototype.BuildTitle = function () {
    var box = this.WinBox;
    if (!this.IsBackbtn) {
        box.append("<div id=\"PageBoxTitle\"><p>" + this.Title + "</p><div id=\"btnPageBoxClose\">&#xe63d;</div></div>");
    }
    else {
        box.append("<div id=\"PageBoxTitle\"><div id=\"btnPageBoxBack\">&#xe6c6;</div>" + this.Title + "</div>");
    }
    $("#btnPageBoxClose").click(function () { PageBox.Close(); });
    $("#btnPageBoxBack").click(function () { PageBox.Hide(); });
    box.append("<div id=\"PageBoxContext\"></div>");
    var box = this.WinBox.find("#PageBoxContext");
    box.width(this.WinBox.width());
    box.height(this.WinBox.height() - $("#PageBoxTitle").height());
    $("#PageBoxTitle span").width($("#PageBoxTitle").innerWidth() - $("#btnPageBoxClose").outerWidth() - 10);
}
//生成页面frame区域
PageBox.prototype.BuildIFrame = function () {
    var box = this.WinBox.find("#PageBoxContext");
    var width = this.WinBox.width();
    //标题栏的高度
    var titHg = $("#PageBoxTitle").height();
    var height = this.WinBox.height() - titHg;
    if (this.Type == "url") {
        var frame = "";
        frame += "<iframe src=\"" + this.Info + "\" name=\"PageBoxIframe\" id=\"PageBoxIframe\" ";
        frame += "width=\"" + width + "\"";
        frame += "height=\"" + height + "\"";
        frame += "marginwidth=\"0\"  marginheight=\"0\" align=\"top\" scrolling=\"auto\"";
        //frame+="frameborder=\"0\" allowtransparency=\"false\">";
        frame += "frameborder=\"0\" >";
        frame += "</iframe>";
        box.append(frame);
        PageBox.LoadMaskOpen();
    }
    if (this.Type == "text") {
        box.append(this.Info);
    }
    if (this.Type == "obj") {
        if (this.Info.size() > 0)
            box.html(this.Info.html());
    }
}
//关闭窗口
PageBox.Close = function (func) {
    $("#screenMask").hide();
    $("#PageBox").remove();
    if (arguments.length > 0) {
        func();
    }
    if (PageBox.OverEvent != null) PageBox.OverEvent();
    PageBox.LoadMaskRemove();
}
//隐藏窗口
PageBox.Hide = function (func) {
    $("#screenMask").hide();
    if (arguments.length > 0) {
        $("#PageBox").slideUp(200, func);
    } else {
        $("#PageBox").slideUp(200, function () {
            //$("#PageBox").remove();
        });
    }
}
//生成遮罩层
PageBox.prototype.Mask = function () {
    var mask = $("#screenMask");
    if (mask.size() < 1) {
        $("body").append("<div id='screenMask'/>");
        mask = $("#screenMask");
    }
    var hg = document.documentElement.clientHeight;
    var wd = document.documentElement.clientWidth;
    mask.width(wd).height(hg);
    mask.css({ "position": "absolute", "z-index": "10000" });
    mask.css({ left: 0, top: 0 });
    var alpha = 60;
    mask.css("background-color", "#ffffff");
    mask.css("filter", "Alpha(Opacity=" + alpha + ")");
    mask.css("display", "block");
    mask.css("-moz-opacity", alpha / 100);
    mask.css("opacity", alpha / 100);
    mask.show();
}

