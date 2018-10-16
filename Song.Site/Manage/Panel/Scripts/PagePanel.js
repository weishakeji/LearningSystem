/*!
* 主  题：《内容页操作面板》
* 说  明：在管理界面右侧出现内容页操作面板。
* 功能描述：
* 1、生成仿傲游浏览器的标签栏，可以关闭标签；
* 2、面板内包含iframe控件，用于显示内容页；
* 3、面板可以全屏；
* 4、当关闭所有标签后，将打开默认页，即起始页
*
* 作  者：宋雷鸣 
* 开发时间: 2012年12月23日
* 修订时间：2014年3月12日 修正浏览适用，增加帮助编辑
* 修订时间：2014年10月11日 增加打印控件的控制
*/

//工作区域，页面（包括标题、页面ifame)的操作类
function PagePanel(name, path, page, id) {
    this.Init(name, path, page, id);
}
//初始化参数
PagePanel.prototype.Init = function (name, path, page, id) {
    this.Name = name != null && name != "" ? name : this.Default.name;
    this.Path = path != null && path != "" ? path : this.Default.path;
    this.Page = page != null && page != "" ? page : this.Default.page;
    this.Id = id != null && id != "" ? id : this.Default.id;
}
//最多打开几个页面
PagePanel.prototype.Max = 6;
//默认页面
PagePanel.prototype.Default = {
    name: "起始页",
    path: "起始页",
    page: "welcome.aspx",
    id: "def"
}
//打开默认的页面（起启页）
PagePanel.prototype.DefOpen = function () {
    this.add(this.Default.name, this.Default.path, this.Default.page, this.Default.id);
}
//增加页面操作面板
//name:标题名称
//path:页面所在的菜单路径
//page:要打开的页面
//id:当前菜单项id，系统菜单项，如桌面等均为字符表示
PagePanel.prototype.add = function (name, path, page, id) {
    this.Init(name, path, page, id);
    var bar = $("#consBoxTitleBar");
    //判断当前页面是否已经打开
    if (bar.find("#consBoxTitle_" + id).size() > 0) {
        this.focus(id);
        return;
    }
    //已经存在标题的总长
    var titWid = 0;
    bar.children().each(function () {
        titWid += $(this).outerWidth();
    });
    //alert(titWid);
    //if(titWid>this.width())alert("无法打开");
    /*if((titWid+this.size()*6)>(bar.width()-100))
    {
    alert("系统最多允许打开 "+this.size()+" 个窗口。");		
    return;
    }	*/
    //增加标签
    this.addTitle(name, path, id);
    //生成主区域
    var frame = "<div id=\"consFramePanel_" + id + "\" class=\"consFramePanel\" state=\"panel\" style=\"display:none\" type=\"panel\">";
    frame += "</div>";
    //bar.after(frame);
    $("#consContextPanel").append(frame);
    //页面内容区域的加载
    this.addBar(name, path, page, id);
    this.addPage(name, page, id);
    //将新打开的页面设为焦点
    this.focus(id);
    //当完成一切操作后，执行事件
    this.OnComplete();
}
//增加标签
PagePanel.prototype.addTitle = function (name, path, id) {
    var bar = $("#consBoxTitleBar");
    if (path == null) path = "/manage/ErrorPage/404.aspx";
    var pathArr = path.split(",");
    var pathText = "";
    for (var i = 0; i < pathArr.length - 1; i++) {
        pathText += pathArr[i] + " >> ";
    }
    pathText += pathArr[pathArr.length - 1];
    //生成标题框,并填充进页面
    var tit = "<dd id=\"consBoxTitle_" + id + "\" class=\"out\" title=\"" + pathText + "\">";
    tit += "<div class=\"txt\">" + name + "</div>";
    tit += "<img src=\"/manage/Images/close1.gif\" alt=\"关闭\"/>";
    tit += "</dd>";
    bar.append(tit);
    //增加标签事件，使某个页面处于焦点
    bar.find("#consBoxTitle_" + id + " div[class='txt']").click(function () {
        var thisId = $(this).parent().attr("id");
        thisId = thisId.substring(thisId.indexOf("_") + 1);
        var tm = new PagePanel();
        tm.focus(thisId);
    });
    //关闭当前页面的叉号
    var closeBtn = $("#consBoxTitle_" + id + " img");
    closeBtn.click(function () {
        var thisId = $(this).parent().attr("id");
        thisId = thisId.substring(thisId.indexOf("_") + 1);
        new PagePanel().del(thisId);
        return false;
    });
    closeBtn.hover(function () {
        $(this).attr("src", "/manage/Images/close2.gif");
    }, function () {
        $(this).attr("src", "/manage/Images/close1.gif");
    });
    var title = $("#consBoxTitle_" + id);
    title.dblclick(function () {
        var thisId = $(this).attr("id");
        thisId = thisId.substring(thisId.indexOf("_") + 1);
        new PagePanel().del(thisId);
        return false;
    });
}
//增加内容顶部条
//name:当前选择项卡的标题名称
//path:当前选项卡的标题路径，并不是文件路径，如系统,首选项,常规
//page:这才是页面的文件路径
PagePanel.prototype.addBar = function (name, path, page, id) {
    if (path == null) path = "/manage/ErrorPage/404.aspx";
    var pathArr = path.split(",");
    var pathText = "";
    for (var i = 0; i < pathArr.length - 1; i++) {
        pathText += pathArr[i] + " >> ";
    }
    pathText += pathArr[pathArr.length - 1];
    var top = "<table border=\"0\" cellspacing=\"0\" cellpadding=\"0\" class=\"topBar\" id=\"topBar_" + id + "\">";
    //var top="<div class=\"topBar\" id=\"topBar_"+id+"\">";
    top += "<tr>";
    top += "<td class=\"pagePanelPath\" >当前路径：<a href=\"#\" class=\"pagePanelInitHand\" id=\"pagePanelHelpHand_" + id + "\" >" + pathText + "</a></td>";
    //刷新
    top += "<td type=\"refresh\"  class=\"pagePanelRefreshHand\" id=\"pagePanelRefreshHand_" + id + "\">";
    top += "<img src=\"/manage/images/refresh.gif\"/>";
    top += "</td>";
    //帮助
    //如果当前打开页面的文名				
    var tag = "/manage/";
    page = page.toLowerCase();
    if (page.indexOf(tag) > -1) page = page.substring(page.indexOf(tag) + tag.length);
    if (page.lastIndexOf(".") > -1) page = page.substring(0, page.lastIndexOf("."));
    var helpFile = "/manage/help/" + page + ".html";
    top += "<td type=\"help\"  class=\"pagePanelHelpHand\"  id=\"pagePanelHelpHand_" + id + "\" name=\"" + name + "\"  path=\"" + helpFile + "\">";
    top += "<img src=\"/manage/images/help.gif\"/>";
    top += "</td>";
    //打印
    //top += "<td type=\"print\"  class=\"pagePanelPrintHand\" id=\"pagePanelPrintHand_" + id + "\">";
    //top += "<img src=\"/manage/images/print.gif\"/>";
    //top += "</td>";
    //全屏
    top += "<td type=\"window\"  class=\"pagePanelMaxHand\">";
    top += "<img src=\"/manage/images/fullscreen.gif\"/>";
    top += "</td>";
    //
    top += "</tr>";
    top += "</table>";
    var frameBox = $("#consFramePanel_" + id);
    frameBox.append(top);
    //全屏事件
    frameBox.find(".topBar .pagePanelMaxHand").click(function () {
        var thisId = $(this).parents(".consFramePanel").attr("id");
        thisId = thisId.substring(thisId.indexOf("_") + 1);
        var tm = new PagePanel();
        var type = $(this).attr("type");
        type == "window" ? tm.fullPage(id) : tm.revertPage(id);
        return false;
    });
    //初始化页面
    frameBox.find(".pagePanelInitHand").click(function () {
        var thisId = $(this).attr("id");
        thisId = thisId.substring(thisId.indexOf("_") + 1);
        var frame = $("#consFame_" + thisId);
        var src = frame.attr("initsrc");
        frame.attr("src", src);
        return false;
    });
    //刷新页面的事件
    frameBox.find(".pagePanelRefreshHand").click(function () {
        var thisId = $(this).attr("id");
        thisId = thisId.substring(thisId.indexOf("_") + 1);
        var frame = $("#consFame_" + thisId);
        //刷新界面
        var src = frame[0].contentWindow.location.href;
        frame.attr("src", src);
        return false;
    });
    //帮助文档
    frameBox.find(".topBar .pagePanelHelpHand").click(function () {
        //标题与帮助文件路径
        var name = $(this).attr("name");
        var helpFile = $(this).attr("path");
        if (appState == "Debug") {
            var helpFile = "/manage/panel/HelpEditer.aspx?helpfile=" + helpFile + "&name=" + name;
            new window.PageBox(name + "_帮助文档", helpFile, 80, 80).Open();
        } else {
            //如果不是调试状态，直接打开帮助窗口
            new window.PageBox(name + "_帮助文档", helpFile).Open();
        }
        return false;
    });
    //打印按钮
    frameBox.find(".topBar .pagePanelPrintHand").click(function () {
        var thisId = $(this).attr("id");
        thisId = thisId.substring(thisId.indexOf("_") + 1);
        var frame = $("#consFame_" + thisId);
        //打印页面的地址
        var src = frame.attr("src");
        //打印预览的标题
        var title = $("#consBoxTitleBar").find("dd[id$=_" + thisId + "]").find(".txt").text();
        //打印内容
        var frame = document.getElementById("consFame_" + thisId).contentWindow;
        var htm = $(frame.document.body).html();
        htm = "<div style=\"text-align:center;\">" + title + "</div>" + htm;
        htm = "<style>#header,.noprint{display: none;}table{border:1px solid #000;width:100%;}" +
            "table {border-collapse: collapse; border-style: double;border-width:3px;border-color: black;}" +
             "td,th{border-width: 1px;border-style: solid;} .center{text-align: center;} </style>" + htm;
        //打印
        var print = PagePanel.getPrintObject();
        if (print == null) return false;
        print.PRINT_INIT(title);
        print.ADD_PRINT_HTM(20, 40, 700, 900, htm);
        //print.ADD_PRINT_URL(30,20,746,"100%",src);
        print.SET_PRINT_STYLEA(0, "HOrient", 3);
        print.SET_PRINT_STYLEA(0, "VOrient", 3);
        print.PREVIEW();
        return false;
    });
}
//增加内容区
PagePanel.prototype.addPage = function (name, page, id) {
    var frameBox = $("#consFramePanel_" + id);
    //右侧管理内容页所处的区域，是td表格
    var context = $("#consContextPanel");
    var width = context.width();
    var height = context.height();
    //var height=context.height()-50;
    var frame = "";
    frame += "<iframe src=\"" + page + "\" initsrc=\""+page+"\" name=\"consFame_" + id + "\" id=\"consFame_" + id + "\" class=\"frame\"";
    //frame+="width=\""+width+"\"";
    frame += "width=\"100%\" ";
    frame += "height=\"" + (height - $(".topBar").height()) + "\"";
    //frame+="height=\"100%\" ";
    frame += "marginwidth=\"0\"  marginheight=\"0\" align=\"top\" scrolling=\"auto\"";
    frame += "frameborder=\"0\" allowtransparency=\"true\">";
    frame += "</iframe>";
    frameBox.append(frame);
}
//删除面板
PagePanel.prototype.del = function (id) {
    var bar = $("#consBoxTitleBar");
    //当前页面标题
    var curTit = bar.find("#consBoxTitle_" + id);
    //下一个要打开的页面
    var nextFocus = null;
    if (curTit.next().length == 1)
        nextFocus = curTit.next();
    else
        nextFocus = curTit.prev();
    //清除标题
    bar.find("#consBoxTitle_" + id).remove();
    //清除面板
    bar.parent().find("#consFramePanel_" + id).remove();
    //如果当前打开的页面小于1
    if (this.size() < 1) {
        this.add(this.Default.name, this.Default.path, this.Default.page, this.Default.id);
    } else {
        //关闭当前面板后，打开最后一个页面
        //var tit=bar.find("dd$:last-child");
        var thisId = nextFocus.attr("id");
        thisId = thisId.substring(thisId.indexOf("_") + 1);
        this.focus(thisId);
    }
}
//设置当前面板为焦点
PagePanel.prototype.focus = function (id) {
    var bar = $("#consBoxTitleBar");
    //如果id参数为空，则返回当前处在焦点的面板id
    if (id == null) {
        bar.find("dd").each(function () {
            var cl = $(this).attr("class");
            if (cl == "current") {
                var thisId = $(this).attr("id");
                thisId = thisId.substring(thisId.indexOf("_") + 1);
                id = thisId;
            }
        });
        return id;
    };
    //如果参数不为空，则设置当前焦点
    //取出所有的frame面板
    var panel = bar.parent().find("div[type='panel']");
    panel.each(function () {
        //当前的显示，其余隐藏
        var thisId = $(this).attr("id");
        thisId = thisId.substring(thisId.indexOf("_") + 1);
        thisId == id ? $(this).show() : $(this).hide();
    });
    //标题的操作
    bar.find("dd").each(function () {
        var thisId = $(this).attr("id");
        thisId = thisId.substring(thisId.indexOf("_") + 1);
        thisId == id ? $(this).attr("class", "current") : $(this).attr("class", "out");
        thisId == id ? $(this).find("img").show() : $(this).find("img").hide();
    });
    return id;
}
//标题个数
PagePanel.prototype.size = function () {
    var bar = $("#consBoxTitleBar");
    //判断是否已经存在
    return bar.find("dd").size();
}
//标题总宽度
PagePanel.prototype.width = function () {
    var bar = $("#consBoxTitleBar");
    return bar.width();
}
//根据id获取标题
PagePanel.prototype.name = function (id) {
    var titl = $("#consBoxTitle_" + id);
    if (titl.length > 0) return titl.text();
    return null;
}
//获取当前焦点页面的标题
PagePanel.prototype.focusName = function () {
    var id = this.focus();
    return this.name(id);
}
//将当前窗体最大化
PagePanel.prototype.fullPage = function (id) {
    //页面内容区div
    var frame = $("#consFramePanel_" + id);
    //按钮所处的盒子区域
    var span = frame.find(".topBar .pagePanelMaxHand");
    var offset = frame.offset();
    //改变大小之前，先记录原始信息
    if (span.attr("w") == null || span.attr("w") == "") {
        span.attr("w", frame.width());
        span.attr("h", frame.height());
        span.attr("t", offset.top);
        span.attr("l", offset.left);
    }
    span.attr("type", "fullscreen");
    //设置参数
    frame.css({ position: "absolute", top: offset.top, left: offset.left,
        height: frame.height(), width: frame.parent().width()
    });
    frame.css("z-index", "5001");
    frame.attr("class", "consFramePanel fullWindow");
    frame.attr("state", "fullscreen");
    //动态放大
    frame.animate({
        width: document.documentElement.clientWidth,
        height: document.documentElement.clientHeight,
        left: 0, top: 0
    }, 500, function () {
        var iframe = $(this).find("iframe");
        iframe.width($(this).width());
        iframe.height($(this).height() - iframe.prev().height());
    });
    span.html("<img src=\"/manage/images/window.gif\"/>");
}
//将窗体从最大化还原到普通
PagePanel.prototype.revertPage = function (id) {
    //页面内容区div
    var frame = $("#consFramePanel_" + id);
    //按钮所处的盒子区域
    var span = frame.find(".topBar .pagePanelMaxHand");
    //改变大小之前，取回原始信息
    var w = Number(span.attr("w"));
    var h = Number(span.attr("h"));
    var t = Number(span.attr("t"));
    var l = Number(span.attr("l"));
    span.attr("type", "window");
    frame.attr("class", "consFramePanel");
    //设置参数	
    frame.css({ position: "static", width: w, height: h });
    frame.find("iframe").height(h - $(".topBar").height());
    frame.find("iframe").width(w);
    //去高度，设置状态
    frame.attr("state", "panel");
    span.html("<img src=\"/manage/images/fullscreen.gif\"/>");
}
//当完成后，触发事件
PagePanel.prototype.OnComplete = function () {
    this.isHelp();
}
//是否有帮助文档
PagePanel.prototype.isHelp = function () {
    //判断当前页面是否有帮助，没有帮助则不显示帮助按钮
    var help = $("#pagePanelHelpHand_" + this.Id);
    //如果是调试状态，则一直显示帮助按钮
    if (appState == "Debug") help.show();
    $.ajax({ type: "POST", url: help.attr("path"), dataType: "html",
        //加载出错
        error: function () {
            if (appState == "Debug") {
                help.text("帮助");
            }
        }, success: function (o) {
            //加载成功，说明帮助文档存在，则显示帮助按钮	
            help.show();
        }
    });
}
//判断是否安装打印控件
PagePanel.isInstallPrint = function () {
    try {
        var LODOP = PagePanel.getPrintObject(document.getElementById('LODOP_OB'), document.getElementById('LODOP_EM'));
        if ((LODOP != null) && (typeof (LODOP.VERSION) != "undefined")) return true;
    } catch (err) {
        return false;
    }
}
//获取打印控件的对象
PagePanel.getPrintObject = function (oOBJECT, oEMBED) {
    oOBJECT = oOBJECT == null ? document.getElementById('LODOP_OB') : oOBJECT;
    oEMBED = oEMBED == null ? document.getElementById('LODOP_EM') : oEMBED;
    var LODOP = oEMBED;
    var wd = 400;
    var hg = 300;
    var path = "/manage/panel/PrintAlert.htm?para=";
    try {
        if (navigator.appVersion.indexOf("MSIE") >= 0) LODOP = oOBJECT;
        if ((LODOP == null) || (typeof (LODOP.VERSION) == "undefined")) {
            if (navigator.userAgent.indexOf('Firefox') >= 0)
                new top.PageBox("打印控件未安装", path + "strHtmFireFox", wd, hg).Open();
            if (navigator.userAgent.indexOf('WOW64') >= 0)
                new top.PageBox("打印控件未安装", path + "strHtm64_Install", wd, hg).Open();
            else
                new top.PageBox("打印控件未安装", path + "strHtmInstall", wd, hg).Open();
            return LODOP;
        } else if (LODOP.VERSION < "6.1.4.5") {
            if (navigator.userAgent.indexOf('WOW64') >= 0)
                new top.PageBox("打印控件需要升级", path + "strHtm64_Update", wd, hg).Open();
            else
                new top.PageBox("打印控件需要升级", path + "strHtmUpdate", wd, hg).Open();
            return LODOP;
        }
        return LODOP;
    } catch (err) {
        if (navigator.userAgent.indexOf('WOW64') >= 0)
            new top.PageBox("打印控件未安装", path + "strHtm64_Install", wd, hg).Open();
        else
            new top.PageBox("打印控件未安装", path + "strHtmInstall", wd, hg).Open();
        return LODOP;
    }
}