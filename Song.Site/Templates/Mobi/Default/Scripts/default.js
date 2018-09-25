$(function () {
    menuBox_Autoloyout();
    default_event();
    //热门课程点击事件
    mui('body').on('tap', '.cou-item', function () {
        var id = $(this).attr("couid");
        new PageBox("课程详情", "Course.ashx?id=" + id, 100, 100, "url").Open();
    });
    //    var isweixin = $().isWeixin();
    //    var ismini = $().isWeixinApp();
    //    var ua = window.navigator.userAgent.toLowerCase();
    //    $("#test").append(ua);
    //    $("#test").append("是否处于微信：" + isweixin);
    //    $("#test").append("是否处于微信小程序：" + ismini);
});

//自动登录
//WeixinLoginIsUse:微信登录是否启用
function AutoLogin_forAjax(WeixinLoginIsUse) {
    //取存储的学员id与密码（md5密文）
    var accid = $.storage("accid");
    var accpw = $.storage("accpw");
    accid = typeof (accid) == "undefined" ? "" : accid;
    accpw = typeof (accpw) == "undefined" ? "" : accpw;
    if (accid == "" || accpw == "") {
        goWinxilogin();
    } else {
        //异步登录
        $.get("login.ashx", { accid: accid, accpw: accpw }, function (data) {
            if (data == "1") {
                document.location.href = "default.ashx";
            } else {
                goWinxilogin();
            }
        });
    }
    function goWinxilogin() {
        if ($().isWeixin()) {
            if (WeixinLoginIsUse) document.location.href = "/mobile/weixin.ashx";
        }
    }
}
//首页的事件
function default_event() {
    //搜索框的提交事件
    $("#formSearch").submit(function () {
        var txt = $("#tbSearch").val();
        if ($.trim(txt) == "") return false;
    });
    mui('body').on('tap', '.btnSear', function () {
        var txt = $("#tbSearch").val();
        if ($.trim(txt) == "") return false;
        var href = $(this).parent("form").attr("action");
        window.location.href = $().setPara(href, "sear", txt);
    });
}
//自定义菜单的自动布局
function menuBox_Autoloyout() {
    //获取菜单项
    var mitem = $(".menuBox .mItem");
    //如果菜单项可以被4整除，则每行四个，默认是每行三个
    if (mitem.size() % 4 == 0) mitem.css("width", (100 / 4) + "%");
    //if (mitem.size() == 2) mitem.css("width", (100 / 2) + "%");
    //if (mitem.size() == 1) mitem.css({ "width": "20%", "margin-left": "auto", "margin-right": "auto", "float": "none", "border-right-style": "none" });
    //自动计算图片宽高
    mitem.find(".mitem-img").each(function (index, element) {
        var wd = $(this).width();
        $(this).height(Math.floor(wd));
    });
    //计动计算菜单项高度
    var maxheight = 0;
    $(".menuBox .mItem").each(function (index, element) {
        var imgHg = $(this).find(".mitem-img").outerHeight();
        var spanHg = $(this).find("span").outerHeight();
        var height = imgHg + spanHg + 10;
        maxheight = maxheight < height ? height : maxheight;
    }).height(maxheight);
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