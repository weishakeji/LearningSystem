/*!
* 主  题：《页面弹出窗口》
* 说  明：用于子页面弹出时的窗口。
* 功能描述：
* 1、生成弹出窗口，窗口内包括iframe控件，用于加载实际控制页面；
* 2、窗口弹出时，生成背景遮罩；
* 3、窗口最小为宽高100，小于等于100时，宽高值默认为浏览器窗口的百分比；
*
*
* 作  者：宋雷鸣 
* 开发时间: 2016年12月28日
*/
(function () {
    ////type:窗口类型，url为Iframe窗口（内嵌页面）,text为文本,obj为元素对象，默认为url
    function pagebox(title, page, width, height, id, patwin, type) {
        this.Init(title, page, width, height, id, patwin, type);
    }
    pagebox.version = "1.1";  //版本号
    pagebox.prototype.Parent = null;    //上级窗体
    pagebox.prototype.IsDrag = true;    //是否允许拖动
    pagebox.prototype.CloseEvent = null;    //窗口关闭时的事件
    pagebox.prototype.FullScreenEvent = function () {
        $("html").css("overflow", "hidden");
    };
    pagebox.prototype.WinScreenEvent = function (box) {
        if (Number(box.attr("wdper")) == 100 && Number(box.attr("hgper")) == 100)
            $("html").css("overflow", "auto");
    };
    //初始化参数
    pagebox.prototype.Init = function (title, page, width, height, winid, patwin, type) {
        if (width == 100 & height == 100) {
            if (this.FullScreenEvent != null) this.FullScreenEvent();
        }
        this.Info = page;
        //屏幕的宽高
        var hg = $(window).height();
        var wd = $(window).width();
        this.Title = title != null && title != "" ? title : "&nbsp;";
        //如果为空，则为浏览器窗口一半宽高
        width = width != null && width != 0 && width != "" ? Number(width) : wd / 2;
        height = height != null && height != 0 && height != "" ? Number(height) : hg / 2;
        //如果宽高小于100，则默认为浏览器窗口的百分比
        this.Wdper = width > 100 ? width / wd * 100 : width;    //宽度百分比
        this.Hgper = height > 100 ? height / hg * 100 : height;    //宽度百分比
        this.Width = width > 100 ? Number(width) : Math.floor(wd * Number(width) / 100);
        this.Height = height > 100 ? Number(height) : Math.floor(hg * Number(height) / 100);
        this.WinId = winid != null ? winid : new Date().getTime() + "_" + Math.floor(Math.random() * 1000 + 1);
        this.Type = type == null ? "url" : type;
        this.Page = pagebox.getCurrPath(patwin, page);
        //上级窗体
        if (patwin != null) this.Parent = pagebox.getParent(patwin);
    }
    //获取父窗口(iframe对象）
    pagebox.getParent = function (winname) {
        var winbox = $(".PageBox[winid=" + winname + "]", window.top.document);
        return winbox;
    }
    //获取当前窗体对象
    //winname:当前iframe名称
    pagebox.getPagebox = function (winname) {
        var box = $(".PageBox[winid=" + winname + "]");
        return box;
    }
    //获取父窗口的window对象
    //winname:当前窗体的名称
    pagebox.parentWindow = function (winname) {
        var box = pagebox.getParent(winname);
        if (box.size() > 0) box = pagebox.getParent(box.attr("parent"));
        var iframe = box.find("iframe");
        var win = null
        if (iframe.size() > 0) win = iframe.get(0).contentWindow;
        return win;
    }
    //获取父路径
    pagebox.getParentPath = function (winname) {
        var winbox = $(".PageBox[winid=" + winname + "]", window.top.document);
        if (winbox.size() < 1) winbox = $("iframe[name=" + winname + "]", window.top.document);
        var iframe = winbox.find("iframe");
        var path = "";
        if (iframe.size() > 0) {
            path = iframe.get(0).contentWindow.location.href;
        } else {
            path = window.top.location.href;
        }
        path = path.indexOf("?") ? path.substring(0, path.lastIndexOf("?") + 1) : "";
        path = path.indexOf("/") ? path.substring(0, path.lastIndexOf("/") + 1) : "";
        return path;
    }
    //获取当前要打开的路径
    pagebox.getCurrPath = function (winname, page) {
        var patpath = pagebox.getParentPath(winname);
        if ($.trim(patpath) == "") return page;
        var path = page;
        if (new RegExp("[a-zA-z]+://[^\s]*").exec(page)) return page;
        if (new RegExp("^[{\\\/\#]").exec(page)) return page;
        return patpath + path;
    }
    //创建窗口，并打开
    pagebox.prototype.Open = function (func) {
        //防止窗体滑动
        $("html").css("overflow", "hidden").attr("scrolltop", $("html").scrollTop()).scrollTop(0);
        $("body").find(">*[type!=PageBox]:visible").attr("visible-set", "true").hide();
        //生成窗口
        this.maskOpen();    //打开遮罩层
        this.buildFrame();  //创建窗体
        pagebox.coordinate(this.WinId);     //计算坐标相对窗体的比例
        //设置拖动
        if (this.IsDrag && !(this.Wdper == 100 && this.Hgper)) {
            var box = $(".PageBox[winid='" + this.WinId + "']");
            if (box.size() > 0) {
            }
        }
        if (arguments.length > 0) {
            func();
        }        
        //关闭事件，全屏事件
        if (this.CloseEvent != null) pagebox.events.add(this.WinId + "_CloseEvent", this.CloseEvent);
        if (this.FullScreenEvent != null) pagebox.events.add(this.WinId + "_FullScreenEvent", this.FullScreenEvent);
        if (this.WinScreenEvent != null) pagebox.events.add(this.WinId + "_WinScreenEvent", this.WinScreenEvent);

    }
    //生成窗体外框
    pagebox.prototype.buildFrame = function () {
        //屏幕的宽高
        var hg = $(window).height();
        var wd = $(window).width();
        $("body").append("<div class=\"PageBox\" type=\"PageBox\" winid=\"" + this.WinId + "\"></div>");
        var boxframe = $(".PageBox[winid=" + this.WinId + "]");
        boxframe.attr({ "wdper": this.Wdper, "hgper": this.Hgper });
        var border = parseInt(boxframe.css("border-width")); //窗体边线宽度
        border = !isNaN(border) ? border : 0;
        //设置窗口的位置
        boxframe.css({ top: (hg - this.Height) / 2 + $(window).scrollTop(),
            left: (wd - this.Width) / 2, position: "absolute",
            "width": (this.Width == wd ? wd : this.Width - border * 2),
            "height": (this.Height == hg ? hg : this.Height - border * 2)
        });
        //如果有父窗口
        if (this.Parent != null && this.Parent.size() > 0) {
            boxframe.attr({ "parent": this.Parent.attr("winid") });
            if (!(this.Parent.attr("wdper") == "100" && this.Parent.attr("hgper") == "100")) {
                var off = this.Parent.offset();
                boxframe.css({ top: off.top + 50, left: off.left + 50 });
            }
        }
        //设置层深
        var index = parseInt($(".screenMask[winid=" + this.WinId + "]").css("z-index"));
        boxframe.css({ "z-index": index + 1 });
        //设置标题
        boxframe.append("<div class='PageBoxTitle'></div>");
        var titbox = boxframe.find(".PageBoxTitle");
        titbox.append("<div class=\"PageBoxTitleTxt\">" + this.Title + "</div>");
        titbox.append("<div class=\"PageBoxTitleClose\" type=\"click\" href=\"#\">&#215; </div>");
        //标题栏上的关闭按钮，点击事件
        titbox.find(".PageBoxTitleClose").click(function () {
            var box = $(this).parents("div[type=PageBox]");
            PageBox.Close(box.attr("winid"));
            return false;
        });
        boxframe.append("<div class=\"PageBoxContext\"></div>");
        var box = boxframe.find(".PageBoxContext");
        box.width(boxframe.width());
        box.height(boxframe.height() - $(".PageBoxTitle").height());
        $(".PageBoxTitle span").width($(".PageBoxTitle").innerWidth() - $(".btnPageBoxClose").outerWidth() - 10);
        //生成窗体内容区，即iframe
        if (this.Type == "url") {
            box.append("<iframe name='" + this.WinId + "'></iframe>");
            var frame = boxframe.find("iframe");
            var titHg = boxframe.find(".PageBoxTitle").height();    //高度
            var height = boxframe.height() - titHg;
            frame.attr({ src: this.Page, width: boxframe.width(), height: height,
                marginwidth: 0, marginheight: 0, align: "top", scrolling: "auto", frameborder: 0
            });
        }
        var WinBox = $(".PageBox[winId='" + this.WinId + "']");
        var box = WinBox.find(".PageBoxContext");
        if (this.Type == "text") {
            box.append(this.Info);
        }
        if (this.Type == "obj") {
            if (this.Info.size() > 0)
                box.html(this.Info.html());
        }
        //生成iframe上面的覆盖
        boxframe.append("<div class='PageBoxIframeMask'></div>");
        var mask = boxframe.find(".PageBoxIframeMask");
        mask.css({ position: "absolute", "display": "block", "background-color": "#ffffff",
            left: "0px", top: boxframe.find(".PageBoxTitle").height(), opacity: .6,
            width: boxframe.width(), height: height
        }).fadeIn("slow");
        mask.hide();
        return boxframe;
    }
    //关闭窗口
    pagebox.prototype.Close = function (winid) {	//清除窗口
        PageBox.Close(winid);
    }
    pagebox.Close = function (winid) {
        if (winid == null) {
            $(".PageBox").remove();
            $(".screenMask").fadeOut(200);
        } else {
            //窗口关闭后的事件
            var funcClose = pagebox.events.get(winid + "_CloseEvent");
            var box = $(".PageBox[winid='" + winid + "']");
            var pbox = $(".PageBox[winid='" + box.attr("parent") + "']");
            if (funcClose != null) funcClose(box, pbox);   //关闭窗口的事件
            //去除全屏事件
            var funcWin = pagebox.events.get(winid + "_WinScreenEvent");
            if (funcWin != null) funcWin(box);
            //清除pagbox对象
            $(".PageBox[winid='" + winid + "']").remove();
            $(".screenMask[winid=" + winid + "]").fadeOut("slow", function () {
                $(this).remove();
            });
        }
        //去除禁止窗体滑动
        $("html").css("overflow", "auto");
        $("body").find(">*[type!=PageBox][visible-set=true]").show();
        $("html").scrollTop(Number($("html").attr("scrolltop")));
    }
    //刷新窗体
    pagebox.Refresh = function (winid) {
        var box = $(".PageBox[winid='" + winid + "']");
        var iframe = box.find("iframe");
        if (iframe.size() > 0) {
            iframe.attr("src", iframe.get(0).contentWindow.location.href);
        }
    }
    //隐藏关闭按钮
    pagebox.prototype.HideClose = function () {
        $(".PageBoxTitleClose").hide();
    }
    //关闭窗口并刷新当前打开的页面
    pagebox.CloseAndRefresh = function (winid) {
        var box = $(".PageBox[winid='" + winid + "']");
        pagebox.Refresh(box.attr("parent"));
        pagebox.Close(winid);
        //管理页刷新（如果是前端管理界面）
        var frame = $("#adminPage");
        if (frame.size() > 0) frame.attr("src", frame.get(0).contentWindow.location.href);
        //超管页刷新
        var frame = $(".consFramePanel:visible iframe");
        if (frame.size() > 0) frame.attr("src", frame.get(0).contentWindow.location.href);

    }
    //生成遮罩层
    pagebox.prototype.maskOpen = function () {
        $("body").append("<div class=\"screenMask\" winid='" + this.WinId + "'/>");
        var mask = $(".screenMask[winid=" + this.WinId + "]");
        //屏幕的宽高
        var hg = document.documentElement.clientHeight > document.body.scrollHeight ? document.documentElement.clientHeight : document.body.scrollHeight;
        var wd = document.documentElement.clientWidth > document.body.scrollWidth ? document.documentElement.clientWidth : document.body.scrollWidth;
        var alpha = 60;
        var initIndex = 1;
        if (this.Parent != null && this.Parent.size() > 0) {
            initIndex = parseInt(this.Parent.css("z-index"));
        }
        mask.css({ "position": "absolute", "z-index": initIndex + 10000, "display": "block",
            "width": wd, "height": hg, top: 0, left: 0, opacity: .3, "background-color": "#fff"
        }).fadeIn(1);
    }
    //当浏览器窗口变化时
    pagebox.OnReSize = function () {
        $(".screenMask").css({ width: $(window).width(), height: $(window).height() });
        //屏幕的宽高
        var winhg = $(window).height();
        var winwd = $(window).width();
        $(".PageBox").each(function () {
            var border = parseInt($(this).css("border-width")); //窗体边线宽度
            border = !isNaN(border) ? border : 0;
            var wd = Number($(this).attr("wdper")) * winwd / 100 - border * 2;
            var hg = Number($(this).attr("hgper")) * winhg / 100 - border * 2;
            $(this).css({ width: wd, height: hg }).find("iframe").css({ width: wd, height: hg - 30 });
            var left = Number($(this).attr("leftper")) * winwd;
            var top = Number($(this).attr("topper")) * winhg;
            $(this).css({ left: left, top: top });
        });
    }
    //计算坐标相对于窗体，并记录在pagebox上
    pagebox.coordinate = function (winid) {
        var box = $(".PageBox[winid='" + winid + "']");
        var winhg = $(window).height();
        var winwd = $(window).width();
        var off = box.offset();
        box.attr({ "leftper": off.left / winwd, "topper": off.top / winhg });
    }
    /*  储存事件方法的键值对，用于保存窗体事件   */
    pagebox.events = {
        //添加
        add: function (key, val) {
            var i = pagebox.events.indexOf(key);
            if (i > -1) {
                pagebox.events.values[i] = val;
            } else {
                pagebox.events.keys.push(key);
                pagebox.events.values.push(val);
            }
            return val;
        },
        //清除
        remove: function (key) {
            var i = pagebox.events.indexOf(key);
            if (i > -1) {
                pagebox.events.keys.splice(i, 1);
                pagebox.events.values.splice(i, 1);
            }
        },
        //通过key值，或取value
        get: function (key) {
            var i = pagebox.events.indexOf(key);
            if (i > -1) return pagebox.events.values[i];
            return null;
        },
        //是否为空
        isempty: function () { return pagebox.events.size < 1; },
        //长度
        size: function () { return pagebox.events.keys.length; },
        //通过key，获取索引值
        indexOf: function (key) {
            for (var i = 0; i < pagebox.events.keys.length; i++) {
                if (pagebox.events.keys[i] == key) return i;
            }
            return -1;
        },
        //清空
        clear: function () {
            pagebox.events.keys.splice(0, pagebox.events.keys.length);
            pagebox.events.values.splice(0, pagebox.events.values.length);
        },
        keys: new Array(),
        values: new Array()
    };
    window.PageBox = pagebox;
    $(window).resize(function () {
        window.PageBox.OnReSize();
    });
})();
