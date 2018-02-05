/*!
* 主  题：《消息窗口手机版》
* 说  明：用于子页面弹出时的窗口。
* 功能描述：
* 1、生成弹出消息，包括确认，提示，警告，预载；
* 2、窗口弹出时，生成背景遮罩；
* 3、窗口最小为宽高100，小于等于100时，宽高值默认为浏览器窗口的百分比；
*
*
* 作  者：宋雷鸣
* 开发时间: 2015年9月19日
*/
//窗体Jquery对象
MsgBox.prototype.WinBox = null;
//确认按钮的事件
MsgBox.prototype.EnterEvent = null;
//结束后的事件
MsgBox.OverEvent = null;
//是否显示右上角关闭按钮
MsgBox.prototype.ShowCloseBtn = true;
//弹出窗口的主类
//title:窗口标题
//info:要展示的信息，如果type为内嵌页页
//width:窗口宽度
//height:窗口高度
//type:窗口类型，confirm为确认,msg为消息，alert为警告,loading；默认为msg，如果为'null'则不显示关闭与按钮（白板窗体）
function MsgBox(title, info, width, height, type, event, overFunc) {
    this.EnterEvent = event;
    this.OverEvent = overFunc;
    this.Init(title, info, width, height, type);
}
//初始化参数
MsgBox.prototype.Init = function (title, info, width, height, type) {
    this.Title = title != null && title != "" ? title : "";
    this.Info = info;
    //如果为空，则为浏览器窗口一半宽高
    this.Width = !isNaN(Number(width)) ? Number(width) : $(window).width();
    this.Height = !isNaN(Number(height)) ? Number(height) : $(window).height();
    //如果宽高小于100，则默认为浏览器窗口的百分比
    this.Width = this.Width > 100 ? this.Width : $(window).width() * this.Width / 100;
    this.Height = this.Height > 100 ? this.Height : $(window).height() * this.Height / 100;
    this.WinId = new Date().getTime() + "_" + Math.floor(Math.random() * 1000 + 1);
    this.Type = type == null ? "null" : type;
}
//创建窗口，并打开
MsgBox.prototype.Open = function () {
    //判断是否已经存在窗口
    var WinBox = $("#MsgBox");
    if (WinBox.size() > 0) WinBox.remove();
    //生成窗口
    this.Mask();
    this.BuildFrame();
    this.BuildContext();
    this.BuildButton();
}
//生成窗体外框,包括标题
MsgBox.prototype.BuildFrame = function () {
    var hg = $(window).height();
    var wd = $(window).width();
    var closeFunc = String(this.OverEvent);
    closeFunc = closeFunc.replace(/^function(\s|\n)+(.+)\((.|\n)+$/, '$2');
    $("body").append("<div id=\"MsgBox\" type=\"MsgBox\" winId=\"" + this.WinId + "\" closeEvent='" + closeFunc + "'></div>");
    var MsgBox = $("#MsgBox");
    this.WinBox = MsgBox;
    //设置窗口的位置
    MsgBox.css("top", (hg - this.Height) / 2 + $(window).scrollTop());
    MsgBox.css("left", (wd - this.Width) / 2);
    MsgBox.css("position", "absolute").css("z-index", "10001");
    MsgBox.css("width", this.Width - 20);
    MsgBox.css("height", this.Height - 20);
    this.BuildTitle();
}
//生成标题栏
MsgBox.prototype.BuildTitle = function () {
    var box = this.WinBox;
    if (this.ShowCloseBtn && this.Type!="null") {
        box.append("<div id=\"MsgBoxTitle\">" + this.Title + "<div id=\"MsgBtnClose\">&#xe63d;</div></div>");
        $("#MsgBtnClose").click(function () {
            MsgBox.Close();
        });
    } else {
        box.append("<div id=\"MsgBoxTitle\">" + this.Title + "</div>");
    }
    box.append("<div id=\"MsgBoxContext\"></div>");
    var box = this.WinBox.find("#MsgBoxContext");
    box.width(this.WinBox.width() - 20);
    if (this.Type == "null")
        box.height(this.WinBox.height() - $("#MsgBoxTitle").height() - 20);
    else
        box.height(this.WinBox.height() - $("#MsgBoxTitle").height() -50 - 20);
}
//生成页面区域
MsgBox.prototype.BuildContext = function () {
    var box = this.WinBox.find("#MsgBoxContext");
    if (this.Type == "msg" || this.Type == "alert" || this.Type == "confirm" || this.Type=="null") {
        box.append(this.Info);
    }
    if (this.Type == "loading") {
        box.append(this.Info);
    }
}
//生成按钮区域
MsgBox.prototype.BuildButton = function () {
    if (this.Type == "null") return;
    this.WinBox.append("<div class='msgBtnBox' style='height:50px'></div>");
    var box = this.WinBox.find(".msgBtnBox");
    box.width(this.WinBox.width());
    if (this.Type == "msg" || this.Type == "alert" || this.Type == "confirm") {
        box.append("<div id='msgBtnClose' class='msgbtn'>关闭</div>");
        box.find("#msgBtnClose").click(function () {
            MsgBox.Close();
        });
    }
    if (this.Type == "confirm") {
        box.append("<div id='msgBtnEnter' class='msgbtn'>确定</div>");
        if (this.EnterEvent != null) {
            box.find("#msgBtnEnter").click(this.EnterEvent);
        }
    }
    if (this.Type == "loading") {
        box.addClass("msgLoading");
    }
}
//关闭窗口
//isquiet:是否安静，如果为true，则不执行事件
MsgBox.Close = function (isquiet) {
    $("#msgMask").fadeOut(100,function () {
        $("#msgMask").remove();
    });
    $("#MsgBox").fadeOut(100, function () {
        $("#MsgBox").remove();
    });
    //$("#MsgBox").remove();
    if (isquiet == null || isquiet != true) {
        if (MsgBox.OverEvent != null) MsgBox.OverEvent();
    }
}

//生成遮罩层
MsgBox.prototype.Mask = function () {
    var mask = $("#msgMask");
    if (mask.size() < 1) {
        $("body").append("<div id='msgMask'/>");
        mask = $("#msgMask");
    }
    var hg = $(window).height() > document.body.clientHeight ? $(window).height() : document.body.clientHeight;
    var wd = $(window).width() > document.body.clientWidth ? $(window).width() : document.body.clientWidth;
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
//弹窗是否存在
MsgBox.IsExist = function () {
    return $("#MsgBox").size() > 0;
}
//当窗口大小变化时
$(window).resize(function () {
    var winWd = $(window).width();
    var winHg = $(window).height();
    if (winWd == window.winWidth && winHg == window.winHeight) return;
    if ($("#msgMask").size()) {
        var hg = $(window).height() > document.body.clientHeight ? $(window).height() : document.body.clientHeight;
        var wd = $(window).width() > document.body.clientWidth ? $(window).width() : document.body.clientWidth;
        $("#msgMask").width(wd).height(hg);
    }
});