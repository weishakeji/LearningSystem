$(function () {
    menuBox_Autoloyout();
    default_event();
    //热门课程点击事件
    mui('body').on('tap', '.cou-item', function () {
        var id = $(this).attr("couid");
        new PageBox("课程详情", "Course.ashx?id=" + id, 100, 100, "url").Open();
    });
});

//自动登录
//WeixinLoginIsUse:微信登录是否启用
function AutoLogin_forAjax(WeixinLoginIsUse) {
    //判断是否处于微信中
    var ua = window.navigator.userAgent.toLowerCase();
    if (ua.match(/MicroMessenger/i) == 'micromessenger') {
		if(WeixinLoginIsUse)document.location.href = "/weixin.ashx";
    } else {
        //取存储的学员id与密码（md5密文）
        var accid = $.storage("accid");
        var accpw = $.storage("accpw");
        accid = typeof (accid) == "undefined" ? "" : accid;
        accpw = typeof (accpw) == "undefined" ? "" : accpw;
        if (accid == "" || accpw == "") return;
        //异步登录
        $.get("login.ashx", { accid: accid, accpw: accpw }, function (data) {
            if (data == "1") {
                document.location.href = "default.ashx";
            }
        });
    }
}
//首页的事件
function default_event() {
    //主菜单，即课程学习菜单的事件
    mui('body').on('tap', '.mm-item', function () {
        var couid = $(this).parent().attr("couid");
        if (couid == "") {
            var txt = "由此进入<a href='courses.ashx' type='link' target='_top'>课程中心</a>";
            txt += "或<a href='selfcourse.ashx' type='link' target='_top'>我的课程</a>";
            txt = "请选择当前要学习的课程！<br/>" + txt;
            new MsgBox("提示", txt, 90, 200, "alert").Open();
        } else {
            document.location.href = $(this).attr("href");
        }
        return false;
    });
}
//自定义菜单的自动布局
function menuBox_Autoloyout() {
    //获取菜单项
    var mitem = $(".menuBox .mItem");
    //如果菜单项可以被4整除，则每行四个，默认是每行三个
    if (mitem.size() % 4 == 0) mitem.css("width", (100 / 4) + "%");
    if (mitem.size() == 2) mitem.css("width", (100 / 2) + "%");
    if (mitem.size() == 1) mitem.css({ "width": "50%", "margin-left": "auto", "margin-right": "auto", "float": "none", "border-right-style": "none" });
    //自动计算图片宽高
    mitem.find(".mitem-img").each(function (index, element) {
        var wd = $(this).width();
        $(this).height(wd);
    });
    //计动计算菜单项高度
    $(".menuBox .mItem").each(function (index, element) {
        var imgHg = $(this).find(".mitem-img").outerHeight();
        var spanHg = $(this).find("span").outerHeight();
        $(this).height(imgHg + spanHg + 10);
    });
    /*
    //自定义菜单的事件
    mui('body').on('tap', '.mItem', function () { 
    var target = $.trim(this.getAttribute("target")); 
    if (type == "open" || target == "_open") { 
    document.location.href = $(this).attr("href");   
    }
    });
    */
}