

$(function () {
    verifyCode();
    //图片加载错误时，显示默认图片
    $("img").error(function () {
        var errImg = $(this).attr("default");
        if (errImg == null || errImg == '') return false;
        $(this).attr("src", errImg);
    });
});
$(window).load(function () {

});   //

//顶部右上方的下拉菜单
function RightMenuDrop() {
    $(".topright *[type][state!=drop]:visible").hover(function () {
        if ($(this)[0].tagName.toLowerCase() != "dl") {
            var type = $(this).attr("type"); //用于匹配下拉菜单的名称
            var off = $(this).offset();
            var drop = $(".topright *[type=" + type + "]:hidden"); //下拉菜单
            if (drop.size() < 1) drop = $(".topright *[type=" + type + "][state=drop]");
            drop.attr("state", "drop");
            drop.css({
                left: off.left + $(this).width() + 10 - drop.outerWidth(),
                top: off.top + $(this).height(), position: 'absolute'
            }).css("z-index", 999);
            $(".topright *[type]").removeClass("icon-hover");
            $(this).addClass("icon-hover");
            $(".topright *[state=drop]").hide();
            drop.show();
        }
    }, function () { });
    $(".topright *[type]").hover(function () {
        window.dropHover = true;
    }, function () {
        window.dropHover = false;
        window.setTimeout(function () {
            if (!window.dropHover) {
                $(".topright *[state=drop]").hide();
                $(".topright *[type]").removeClass("icon-hover");
            }
        }, 1000)
    });
}
//主菜单的事件
function MenuEvent() {
    $(".menubBar dl dd:first").addClass("overMenu");
    //加入收藏
    $("a.setfav").click(function () {
        var url = window.location.href;
        var title = $("title").text();
        if (document.all) {
            window.external.addFavorite(url, title);
        } else if (window.sidebar) {
            window.sidebar.addPanel(title, url, "");
        } else {
            alert("当前浏览器不支持该操作，请手工添加收藏。");
        }
        return false;
    });
    //设置为首页
    $("a.sethome").click(function () {
        var url = window.location;
        var obj = $(this);
        try {
            obj.style.behavior = 'url(#default#homepage)';
            obj.setHomePage(url);
        } catch (e) {
            if (window.netscape) {
                try {
                    netscape.security.PrivilegeManager.enablePrivilege("UniversalXPConnect");
                } catch (e) {
                    alert("抱歉，此操作被浏览器拒绝！\n\n请在浏览器地址栏输入“about:config”并回车然后将[signed.applets.codebase_principal_support]设置为'true'");
                }
            } else {
                alert("抱歉，您所使用的浏览器无法完成此操作。\n\n您需要手动将(" + url + ")设置为首页。");
            }
        }
        return false;
    });
    //课程检索
    $(".searbtn").click(function () {
        $(this).parents("form").submit();
    });
    $(".searbtn").parents("form").submit(function () {
        var txt = $("input.seacInput");
        if (txt.val() == txt.attr("def") || $.trim(txt.val()) == "") {
            Verify.ShowBox(txt, "请填写要检索的信息！");
            txt.focus();
            return false;
        }
    });
    $(".seacInput").val($(".seacInput").attr("def"));
    $(".seacInput").focus(function () {
        var txt = $("input.seacInput");
        if (txt.val() == txt.attr("def") || $.trim(txt.val()) == "") {
            txt.val("");
        }
        $(this).addClass("seacInputFocus");
    });
    $(".seacInput").blur(function () {
        var txt = $("input.seacInput");
        if ($.trim(txt.val()) == "") {
            txt.val(txt.attr("def"));
        }
        $(this).removeClass("seacInputFocus");
    });
}
//当点击验证码时，刷新验证码
function verifyCode() {
    $(".verifyCode").click(function () {
        var src = $(this).attr("src");
        if (src.indexOf("&") < 0) {
            src += "&timestamp=" + new Date().getTime();
        } else {
            src = src.substring(0, src.lastIndexOf("&"));
            src += "&timestamp=" + new Date().getTime();
        }
        $(this).attr("src", src);
    });
}
/*
学员在线浏览时间统计
注：当网页处于焦点时（即最前面）才会记录时间
*/
function StudentOnlineLog() { };
StudentOnlineLog.init = function () {
    $(window).blur(function () { window.cookieInterval = false; });
    $(window).focus(function () { window.cookieInterval = true; });
    $(window).load(function () { setInterval("StudentOnlineLog.event()", 1000); });
}
//记录在线时间的事件
StudentOnlineLog.event = function () {
    if (window.cookieInterval == null || window.cookieInterval) {
        var num = $().cookie('stOnlineNumx');
        if (num == null || num == 'null') num = 1;
        num = Number(num) + 1;
        num = num >= Number.MAX_VALUE ? 0 : num;
        $().cookie('stOnlineNumx', num, { expires: 7, path: '/' });
        //StudentOnlineLog.write(num);
        //ajax提交在线时间
        var interval = 60;
        if (Number(num) % interval == 0) {
            $.get("/Ajax/StudentOnline.ashx?interval=" + interval);
        }
    }
}
//在页面上显示计算信息，仅用于调试
StudentOnlineLog.write = function (num) {
    var box = $("#stOnlineNumBox");
    if (box.size() < 1) {
        $("body").children(":first").before("<div id='stOnlineNumBox'/>");
        box = $("#stOnlineNumBox");
    }
    box.html(num);
}
StudentOnlineLog.init();



/*
主菜单的下拉菜单
写于 2016-09-28
作者： 宋雷鸣 qq10522779
*/
//菜单项的初始化
function NavigationInit() {
    if ($(".menubBar").size() < 1) return;
    //菜单项宽度，按字数平均分配
    var sumcount = $(".menubBar dl").text().replace(/\s/g, "").length;
    //alert("菜单总字数：" + sumcount + " 宽度：" + parseInt($(".menubBar dl").css("width")));
    var per = parseInt($(".menubBar dl").css("width")) / sumcount;
    //$(".rootmenu").parent().css("visibility","hidden")
    $(".rootmenu").each(function () {
        var n = $(this).text().replace(/\s/g, "").length;
        $(this).css({ width: n * per + "px" }).show();
    });
    $(".rootmenu").parent().hide().fadeIn(500);
    //当鼠标滑过主菜单
    $(".rootmenu").hover(function () {
        var style = $(this).parent().attr("overClass");
        $(this).parent().children().removeClass(style);
        $(this).addClass(style);
        hidenaviBox(-1); //隐藏所有下拉菜单        	
        shownaviBox($(this)); //显示下拉菜单	
        window._menuHover = true;
    }, function () {
        setTimeout('hidenaviBox()', 3000);
        window._menuHover = false;
    });
    //当前菜单项是否有下拉菜单，如果有，则加个效果
    $(".naviItem").each(function () {
        var nid = $(this).attr("nid");
        var menu = $(".naviBox[pid=" + nid + "]");
        if (menu.size() > 0) $(this).append("<span>&gt;</span>");
    });
    //添加菜单项的事件
    $(".naviItem").hover(function (index) {
        //先隐藏所有菜单
        var nid = $(this).attr("nid"); //当前项id
        var pid = $(this).parent().attr("pid"); //上级id
        hidenaviBox(pid);
        //显示当前菜单的下级菜单
        var menu = $(".naviBox[pid=" + nid + "]");
        if (menu.size() > 0) {
            var off = $(this).offset();
            menu.css({
                left: off.left + $(this).width() - 5,
                top: off.top + $(this).height() / 3
            });
            if (menu.is(":hidden")) {
                menu.show(0, function () {
                    _menuAutowidth(nid);
                });
            }
        }
        $(this).addClass("naviHover");
        window._menuHover = true;
    }, function (index) {
        setTimeout('hidenaviBox()', 3000);
        window._menuHover = false;
    });
}
//显示下拉菜单
//root:根菜单的Html节点对象
function shownaviBox(root) {
    var pid = root.attr("nid"); //上级id
    //当前菜单面板
    var menuBox = $(".naviBox[pid=" + pid + "]");
    if (menuBox.size() < 1) return;
    menuBox.css("border-top-style", "none");
    var off = root.offset();
    menuBox.css({
        left: off.left,
        top: off.top + root.height()
    });
    menuBox.fadeIn(0, function () {
        _menuAutowidth(pid);
    });

}
//隐藏下级菜单，包括下级菜单的所有下级，递规查找
function hidenaviBox(pid) {
    if (pid == -1) $(".naviBox").hide();
    if ((pid == null || typeof (pid) == "undefined") && !window._menuHover) {
        $(".naviBox").hide();
        $(".rootmenu").removeClass("overMenu");
    }
    var menu = $(".naviBox[pid=" + pid + "]");
    menu.find(".naviItem").each(function () {
        var nid = $(this).attr("nid");
        $(".naviBox[pid=" + nid + "]").hide();
        $(this).removeClass("naviHover");
        hidenaviBox(nid);
    });
}
//自动宽度
function _menuAutowidth(pid) {
    var menuBox = $(".naviBox[pid=" + pid + "]");
    //上级对象
    var rootbox = $(".rootmenu[nid=" + pid + "]");
    if (rootbox.size() > 0) {
        menuBox.width(rootbox.width() > 150 ? rootbox.width() : 150);
        return;
    }
    var maxwd = 0;
    menuBox.find("a").each(function () {
        if ($(this).width() > maxwd) maxwd = $(this).width();
    });
    menuBox.find("a").width(maxwd);
    maxwd = maxwd + 15 + 30;
    maxwd = maxwd < $(".rootmenu").width() ? $(".rootmenu").width() : maxwd;
    menuBox.width(maxwd);
    menuBox.find(".naviItem").width(maxwd).css("background-position", (maxwd - 20) + "px 17px");
}

//给所有超链接增加当前登录账号的id
function ToAllLinkAddAccoutsID(acid) {
    //给所有超链接增加当前登录账号的id
    if (!(acid == "" || acid == undefined || acid == null || acid == 0)) {
        $("*[href]").each(function (index, element) {
            var href = $(this).attr("href");
            if (href == undefined || href == null || href == "" || href == "#") return true;
            if (href.length > 0 && href.substring(0, 1) == "#") return true;
            if (href.indexOf("javascript:") > -1) return true;
            if (href.length >= 7 && href.substr(0, 7).toLowerCase() == "http://") return true;
            if (href.length >= 8 && href.substr(0, 8).toLowerCase() == "https://") return true;
            href = $().setPara(href, "sharekeyid", acid);
            $(this).attr("href", href);
        });
    }
    //获取分享id
    var sid = $().getPara("sharekeyid");    //来自地址来的分享人id
    var cookie = $().cookie("sharekeyid");  //存储于本地的分享人id
    if (cookie == "" || cookie == null || typeof (cookie) == "undefined") cookie = $.storage("sharekeyid");
    if (cookie == "" || cookie == null || typeof (cookie) == "undefined") cookie = 0;
    if (sid != "" && cookie == "0" && sid != cookie) {
        //给分享的人增加积分
        $.get("/ajax/AddSharePoint.ashx", { sharekeyid: sid });
    }
    if (sid != "" && sid != cookie) {
        $().cookie("sharekeyid", sid);
        $.storage("sharekeyid", sid);
    }
}
