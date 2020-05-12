/*!
* 主  题：《消息窗口》
* 说  明：用于子页面弹出时的窗口。
* 功能描述：
* 1、生成弹出消息，包括确认，提示，警告，预载；
* 2、窗口弹出时，生成背景遮罩；
* 3、窗口最小为宽高100，小于等于100时，宽高值默认为浏览器窗口的百分比；
* 4、可以设定关闭与确定按钮事件
*
* 作  者：宋雷鸣
* 开发时间: 2015年9月19日
*/
(function () {
    //弹出窗口的主类
    //title:窗口标题
    //info:要展示的信息，如果type为内嵌页页
    //width:窗口宽度
    //height:窗口高度
    //type:窗口类型，confirm为确认,msg为消息，alert为警告,loading；默认为msg，如果为'null'则不显示关闭与按钮（白板窗体）
    function msgbox(title, info, width, height, type, event, overFunc) {
        this.EnterEvent = event;
        this.OverEvent = overFunc;
        this.Init(title, info, width, height, type);
    }
    msgbox.prototype.WinBox = null; //窗体Jquery对象
    msgbox.prototype.EnterEvent = null; //确认按钮的事件    
    msgbox.prototype.OverEvent = null;    //结束后的事件    
    msgbox.prototype.ShowCloseBtn = true;   //是否显示右上角关闭按钮
    //初始化参数
    msgbox.prototype.Init = function (title, info, width, height, type) {
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
        return this;
    }
    //创建窗口，并打开
    msgbox.prototype.Open = function (title, info, width, height, type) {
        //生成窗口
        this.Mask();
        this.BuildFrame();
        this.BuildContext();
        this.BuildButton();
        //如果需要数秒关闭
        var msgbox = $(".MsgBox[winid='" + this.WinId + "']");
        if (msgbox.find("second").size() > 0) {
            window.MsgBoxInterval = setInterval(function () {
                var tm = Number($("second").text());
                if (tm > 1) {
                    $("second").text(--tm)
                } else {
                    clearInterval(window.MsgBoxInterval);
                    MsgBox.CloseEvent($("second"));
                }
            }, 1000);
        }
    }
    //生成窗体外框,包括标题
    msgbox.prototype.BuildFrame = function () {
        var hg = $(window).height();
        var wd = $(window).width();
        $("body").append("<div class=\"MsgBox\" type=\"MsgBox\" winId=\"" + this.WinId + "\"></div>");
        var box = $(".MsgBox[winid='" + this.WinId + "']");
        this.WinBox = box;
        //关闭事件
        if (this.OverEvent != null) msgbox.events.add(this.WinId + "_OverEvent", this.OverEvent);
        //确定事件
        if (this.EnterEvent != null) msgbox.events.add(this.WinId + "_EnterEvent", this.EnterEvent);
        //设置窗口的位置
        box.css("top", (hg - this.Height) / 2);
        box.css("left", (wd - this.Width) / 2);
        box.css("position", "fixed").css("z-index", "10001");
        box.css({ "width": this.Width - 20, "height": this.Height - 20 });
        box.attr("width", box.width()).attr("height", box.height());
        this.BuildTitle();
    }
    //生成标题栏
    msgbox.prototype.BuildTitle = function () {
        var box = this.WinBox;
        if (this.ShowCloseBtn && this.Type != "null") {
            box.append("<div class=\"MsgBoxTitle\">" + this.Title + "<div class=\"MsgClose\">&#215;</div></div>");
            box.find(".MsgClose").click(function () {
                msgbox.CloseEvent($(this));
            });
        } else {
            box.append("<div class=\"MsgBoxTitle\">" + this.Title + "</div>");
        }
        box.append("<div class=\"MsgBoxContext\"></div>");
        var context = this.WinBox.find(".MsgBoxContext");
        context.width(this.WinBox.width() - 20);
        if (this.Type == "null") {
            context.height(box.height() - box.find(".MsgBoxTitle").height() - 20);
        } else {
            context.height(box.height() - box.find(".MsgBoxTitle").height() - 50 - 20);
        }
    }
    //生成页面区域
    msgbox.prototype.BuildContext = function () {
        var box = this.WinBox.find(".MsgBoxContext");
        if (this.Type == "msg" || this.Type == "alert" || this.Type == "confirm" || this.Type == "null") {
            box.append(this.Info);
        }
        if (this.Type == "loading") {
            box.append(this.Info);
        }
    }
    //生成按钮区域
    msgbox.prototype.BuildButton = function () {
        if (this.Type == "null") return;
        this.WinBox.append("<div class='msgBtnBox' style='height:50px'></div>");
        var box = this.WinBox.find(".msgBtnBox");
        box.width(this.WinBox.width());
        if (this.Type == "msg" || this.Type == "alert" || this.Type == "confirm") {
            box.append("<div class='msgBtnClose msgbtn'>关闭</div>");
            box.find(".msgBtnClose").click(function () {
                msgbox.CloseEvent($(this));
            });
        }
        if (this.Type == "confirm") {
            box.append("<div class='msgBtnEnter msgbtn'>确定</div>");
            if (this.EnterEvent != null) {
                box.find(".msgBtnEnter").click(function () {
                    var winid = $(this).parents(".MsgBox").attr("winid");
                    var func = msgbox.events.get(winid + "_EnterEvent");
                    if (func != null) func();
                });
            }
        }
        if (this.Type == "loading") {
            box.addClass("msgLoading");
        }
    }
    //关闭窗口
    //isquiet:是否安静，如果为true，则不执行事件
    msgbox.Close = function (isquiet) {
        //执行窗口关闭事件
        if (isquiet == null || isquiet != true) {
            //if (msgbox.OverEvent != null) msgbox.OverEvent();
            var mbox=$(".MsgBox");
            console.log('mbox:'+mbox.size());
            $(".MsgBox").each(function (index) {
                var winid = $(this).attr("winid");
                var func = msgbox.events.get(winid + "_OverEvent");
                if (func != null) func();
                //console.log('test');
            });
        }
        //$("#msgMask").fadeOut(100, function () {
            //$("#msgMask").remove();
        //});
        //$(".MsgBox").fadeOut(100, function () {
            $(".MsgBox").remove();
        //});

    }
    //关闭窗口的事件
    msgbox.CloseEvent = function (obj) {
        //关闭窗口的方法
        var winid = obj.parents(".MsgBox").attr("winid");
        var func = msgbox.events.get(winid + "_OverEvent");
        if (func != null) func();
        //关闭窗口
        //$("#msgMask").fadeOut(1, function () {
            $("#msgMask").remove();
        //});
        //obj.parents(".MsgBox").fadeOut(1, function () {
            $(".MsgBox").remove();
        //});
        if (MsgBox.OverEvent != null) MsgBox.OverEvent();
    }
    //生成遮罩层
    msgbox.prototype.Mask = function () {
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
        mask.css("filter", "Alpha(Opacity=" + alpha + ")").css("display", "block");
        mask.css("-moz-opacity", alpha / 100).css("opacity", alpha / 100);
        mask.show();
    }
    //弹窗是否存在
    msgbox.IsExist = function () {
        return $(".MsgBox").size() > 0;
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
        $(".MsgBox").each(function () {
            var height = Number($(this).attr("height"));
            var width = Number($(this).attr("width"));
            $(this).css({
                top: (winHg - height) / 2,
                left: (winWd - width) / 2
            });
        });
    });
    /*  储存事件方法的键值对，用于保存窗体事件   */
    msgbox.events = {
        //添加
        add: function (key, val) {
            var i = msgbox.events.indexOf(key);
            if (i > -1) {
                msgbox.events.values[i] = val;
            } else {
                msgbox.events.keys.push(key);
                msgbox.events.values.push(val);
            }
            return val;
        },
        //清除
        remove: function (key) {
            var i = msgbox.events.indexOf(key);
            if (i > -1) {
                msgbox.events.keys.splice(i, 1);
                msgbox.events.values.splice(i, 1);
            }
        },
        //通过key值，或取value
        get: function (key) {
            var i = msgbox.events.indexOf(key);
            if (i > -1) return msgbox.events.values[i];
            return null;
        },
        //是否为空
        isempty: function () { return msgbox.events.size < 1; },
        //长度       
        size: function () { return msgbox.events.keys.length; },
        //通过key，获取索引值
        indexOf: function (key) {
            for (var i = 0; i < msgbox.events.keys.length; i++) {
                if (msgbox.events.keys[i] == key) return i;
            }
            return -1;
        },
        //清空
        clear: function () {
            msgbox.events.keys.splice(0, msgbox.events.keys.length);
            msgbox.events.values.splice(0, msgbox.events.values.length);
        },
        keys: new Array(),
        values: new Array()
    };
    window.MsgBox = msgbox;
})();
