$(function () {
    setMenuAccordion(null); 	//设置菜单展开与折叠
    setMenuStyle(); 	//设置菜单样式
    setMenuEvent(); 	//设置菜单事件
    _setAdminPage(); //设置管理菜单项的内容
    //主管理区加载完成事件
    $("#adminPage").load(function () {
        $("#IframLoadMask").remove();
        var ifm = document.getElementById("adminPage");
        var subWeb = document.frames ? document.frames["adminPage"].document : ifm.contentDocument;
        if (ifm != null && subWeb != null) {
            ifm.height = subWeb.body.scrollHeight;
            ifm.focus();
        }
    });
    //学员中心(左上方）的点击事件
    $(".menuTitle").click(function () {
        var mmid = $(this).attr("mmid");
        _setAdminPage(mmid);
        return false;
    });
});

//设置菜单展开与折叠
function setMenuAccordion(curr) {
    var first = $(".mmtop:first");
    $("#menuBox div[mmtype]").removeClass("currTwo");
    if (curr != null) {
        first = curr.parent().prev();
        curr.addClass("currTwo");
    }
    $(".mmtop").click(function () {
        $(".mmtop").removeClass("currTop");
        $(this).addClass("currTop");
        $(".twoBox:visible").hide();
        $(this).next().show();
    });
    first.click();
}
//设置菜单样式
function setMenuStyle() {
    $("#menuBox a").each(function () {
        if ($(this).attr("isBold") == "True") {
            $(this).css("font-weight", "bold");
        }
        if ($(this).attr("href") == "") {
            $(this).css("color", "#000000");
        }
    });
}
//设置菜单事件
function setMenuEvent() {
    //左侧菜单的点击事件
    $("#menuBox a").click(function () {
        var p = $(this).parent();
        if (p.attr("mmtype") == "item") {
            var mmid = $(this).attr("mmid"); //菜单项id		
            _setAdminPage(mmid);
        }
        if (p.attr("mmtype") == "link") {
            $(this).attr("target", "_blank");
            return true;
        }
        return false;
    });
    //上方的“当前操作”点击事件
    $("#menuPath").click(function () {
        _setAdminPage();
        return false;
    });
}
//设置管理页
function _setAdminPage(mmid) {
    var isinit = false; //是否初次加载，初次加载时没有mmid值
    if (mmid == null || mmid == undefined || mmid == '') {
        mmid = $().getPara("mmid");
        isinit = true;
    }
    if (mmid == '') mmid = 0;
    var url = $().setPara(window.location.href, "mmid", mmid);
    try {
        history.pushState({}, "", url); //更改地址栏信息
    } catch (e) {
        if (!isinit) {
            window.location.href = url;
            return;
        } 
    }
    var a = $("a[mmid=" + mmid + "]")
    var href = $.trim(a.attr("href")); //导航链接
    var title = $.trim(a.attr("title")); //导航标题
    var ptit = ""; //上级标题
    //上级菜单项
    var p = a.parent("[mmtype]");
    if (p.size() > 0) ptit = $.trim(p.parent().prev().text()) + ">>";
    setMenuAccordion(p);
    //开始设置管理内容
    OpenLoadMask();
    var ifm = document.getElementById("adminPage");
    $("#adminPage").attr("src", href);
    $("#menuPath").text(ptit + title);
    var subWeb = document.frames ? document.frames["adminPage"].document : ifm.contentDocument;
    if (ifm != null && subWeb != null) {
        ifm.height = 0;
        ifm.focus();
    }
}
//iframe加载完成的事件
function OpenLoadMask() {
    $("body").append("<div id=\"IframLoadMask\"/>");
    var mask = $("#IframLoadMask");
    var iframe = $("#adminPage");
    //屏幕的宽高
    var off = iframe.offset();
    mask.css({ "position": "absolute", "z-index": "10000",
        "width": iframe.width(), "height": iframe.height(), top: off.top, left: off.left
    });
    var alpha = 50;
    mask.css({ "background-color": "#ffffff", "filter": "Alpha(Opacity=" + alpha + ")",
        "display": "block", "-moz-opacity": alpha / 100, "opacity": alpha / 100
    });
    mask.fadeIn("slow");
}
//设置学生照片
function setPhoto(path) {
    var img = $(".stPhoto img");
    img.attr("src", path);
}
//获取管理菜单的标题
function getAdminTitle() {
    return $.trim($("#menuPath").text());
}