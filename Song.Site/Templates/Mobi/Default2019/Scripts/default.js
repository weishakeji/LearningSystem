$(window).load(function () {
    menuBox_Autoloyout();
});
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

//通知公告
$(function () {
    if ($(".notice-box li").size() > 1) {
        $(".notice-box li").hide();
        $(".notice-box li:first").show();
        window.setInterval("notice_alternate()", 3000);
    }   
});
function notice_alternate() {
    var curr = $(".notice-box li:visible");
    var next = curr.next();
    if (next.size() <= 0) {
        var first = $(".notice-box li:first");
		//alert(first.get(0).outerHTML);
        $(".notice-box").append(first.get(0).outerHTML);
        first.remove();
		next = curr.next();
    }
    curr.slideUp(500);
	next.slideDown(500);
}

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

    //课程选项卡方式展示
    $(".cour-bar .cour-tit").each(function (index) {
        if (index == 0) return true;
        var sbjid = $(this).attr("sbjid");  //专业id
        var html = $(".cour-context .cour-list:first").get(0).outerHTML;
        $(".cour-context").append(html);
        $(".cour-context .cour-list:last").attr("sbjid", sbjid).hide();
    });
    mui('body').on('tap', '.cour-bar .cour-tit', function () {
        var sbjid = $(this).attr("sbjid");
        var sbjname = $.trim($(this).text());   //专业名称
        //切换样式
        $(this).parent().find(".cour-tit").removeClass("current");
        $(this).addClass("current");
        $(".cour-context .cour-list").hide();
        //显示课程列表
        var list = $(".cour-context .cour-list[sbjid=" + sbjid + "]").show();
        list.html(list.html().replace("{0}", sbjname));
        //课程点击事件
        mui('body').off('tap', '.cour-box').on('tap', '.cour-box', function () {
            var id = $(this).attr("couid");
            var name = $(this).find("name").text();
            new PageBox(name, "Course.ashx?id=" + id, 100, 100, "url").Open();
        });
        if (list.find(".loading").size() <= 0) return;
        //加载课程
        $.get("/ajax/courses.ashx", { size: 10, sbjid: sbjid, order: "rec" }, function (data) {
            try {
                //throw "测试错误情况输出";
                var d = eval('(' + data + ')');
                list.html("");
                var defimg = $(".default-img").attr("default"); //默认图片
                for (var i = 0; i < d.object.length; i++) {
                    var cour = d.object[i];
                    var html = "<div class='cour-box' couid='{id}'>{rec}{free}{limitfree}<img src='{logo}' default='{defimg}'/><name>{name}</name><price>{price}</price></div>";
                    html = html.replace("{logo}", unescape(cour.Cou_LogoSmall)).replace("{name}", unescape(cour.Cou_Name));
                    html = html.replace("{id}", cour.Cou_ID).replace("{defimg}", defimg);
                    html = html.replace("{rec}", (cour.Cou_IsRec ? "<rec></rec>" : ""));
                    html = html.replace("{free}", (cour.Cou_IsFree ? "<free></free>" : ""));
                    html = html.replace("{limitfree}", (cour.Cou_IsLimitFree ? "<limitfree></limitfree>" : ""));
                    //价格
                    var price = "";
                    if (cour.Cou_IsFree) {
                        price = "<f>免费</f>";
                    } else {
                        if (cour.Cou_IsLimitFree) {
                            var end = cour.Cou_FreeEnd.Format("yyyy-M-d");
                            price = "<l>免费至 <t>" + end + "</t></l>";
                        } else {
                            price = "<m>" + unescape(cour.Cou_PriceSpan) + unescape(cour.Cou_PriceUnit) + cour.Cou_Price + "元</m>";
                        }
                    }
                    html = html.replace("{price}", price);
                    list.append(html);
					//是否隐藏收费信息
					var mremove=$("body").attr("mremove")=="True" ? true : false;
					if(mremove){
						list.find("price,free,limitfree").hide();
					}
                    list.find("img:last").error(function () {
                        var errImg = $(this).attr("default");
                        if (errImg == null) return false;
                        $(this).attr("src", errImg);
                    });
                    list.append("<a class='list-more' type='link' href='courses.ashx?sbjids=" + d.sbjid + "'>更多课程点这里...</a>");
                }
                if (d.object.length <= 0) {
                    $(".cour-list[sbjid=" + sbjid + "]").html("<null>当前分类没有课程信息！</null>");
                }
            }
            catch (err) {
                $(".cour-context .cour-list:visible span").html("加载错误<br/>详情：" + err);
            }
        });
    });
    //加载第一个课程选项卡
    mui.trigger(document.querySelector('.current'), 'tap');
}
//自定义菜单的自动布局
function menuBox_Autoloyout() {
    //获取菜单项
    var mitem = $(".custom-menu a");
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
    $(".custom-menu a").each(function (index, element) {
        var imgHg = $(this).find(".mitem").outerHeight();
        var spanHg = $(this).find("span").outerHeight();
        var height = imgHg + spanHg;
        maxheight = maxheight < height ? height : maxheight;
    }).height(maxheight);
}

