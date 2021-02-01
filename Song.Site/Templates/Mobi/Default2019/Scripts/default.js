window.vue = new Vue({
    el: '#app',
    data: {

    },
    methods: {}
});

$(window).load(function () {
   
});
$(function () {
 
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
                    var html = "<div class='cour-box' couid='{id}'>{rec}{free}{limitfree}<img src='{logo}' default='{defimg}'/><name>{live}{name}</name><price>{price}</price></div>";
                    html = html.replace("{logo}", unescape(cour.Cou_LogoSmall)).replace("{name}", unescape(cour.Cou_Name));
                    html = html.replace("{id}", cour.Cou_ID).replace("{defimg}", defimg);
                    html = html.replace("{rec}", (cour.Cou_IsRec ? "<rec></rec>" : ""));
                    html = html.replace("{free}", (cour.Cou_IsFree ? "<free></free>" : ""));
                    html = html.replace("{live}", (cour.Cou_ExistLive ? "<live></live>" : ""));
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
                    var mremove = $("body").attr("mremove") == "True" ? true : false;
                    if (mremove) {
                        list.find("price,free,limitfree").hide();
                    }
                    list.find("img:last").error(function () {
                        var errImg = $(this).attr("default");
                        if (errImg == null) return false;
                        $(this).attr("src", errImg);
                    });
                }
                //list.append("<a class='list-more' type='link' href='courses.ashx?sbjids=" + sbjid + "'>更多课程点这里...</a>");
                if (d.object.length <= 0) {
                    $(".cour-list[sbjid=" + sbjid + "]").html("<null>当前分类没有课程信息！</null>");
                }
                //计算课程行数（每行两个）
                var rownum = d.object.length % 2 == 0 ? d.object.length / 2 : d.object.length / 2 + 1;
                //课程图片宽高
                var imgHg = 13 + list.width() * 0.44 * 9 / 16;
                list.css('height', ((imgHg + 55 + 3) * rownum + 10) + 'px');
            }
            catch (err) {
                $(".cour-context .cour-list:visible span").html("加载错误<br/>详情：" + err);
            }
        });
    });
    //加载第一个课程选项卡
    mui.trigger(document.querySelector('.current'), 'tap');
}


