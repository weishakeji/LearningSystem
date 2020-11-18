//构造菜单
(function () {
    //function menutree(){};
    var menutree = {
        ////菜单树的状态，为1是展开首个，为2时全部打开，其它值为关闭树形
        state: 2,
        //初始化菜单
        //mmbox:菜单区域
        //level:层级
        initial: function (mmbox, level) {
            if (mmbox.size() < 1) return;
            //菜单项的处理
            mmbox.find(">.mmitem").each(function () {
                //禁用超链接，改为上级元素事件
                $(this).filter("[mmtype!=link]").find("a").click(function () {
                    $(this).parent().click();
                    return false;
                });
                //下级菜单区
                var mmbox = $(this).next(".mmBox[mmtype!=node]");
                if (mmbox.size() > 0 && mmbox.find(">.mmitem").size() > 0) {
                    //下级菜单缩进
                    mmbox.find(">.mmitem").css("padding-left", (15 * level) + "px");
                    //树形展开的点击事件
                    $(this).click(function () {
                        mmbox.slideToggle(100, function () {
                            if ($(this).is(":hidden")) $(this).prev().removeClass("open");
                            if ($(this).is(":visible")) $(this).prev().addClass("open");
                        });

                    }).append("<div class=\"rightIco\">&nbsp;</div>");

                } else {
                    //如果没有下级菜单，或当前菜单为”node"，则跳转
                    $(this).filter("[mmtype!=link]").click(function () {
                        var type = $(this).attr("mmtype");
                        var tit = $.trim($(this).text());
                        var href = $(this).find("a").attr("href");
                        //弹出窗口
                        if (type == "open") {
                            new PageBox(tit, href, Number($(this).attr("wd")), Number($(this).attr("hg"))).Open();
                            return;
                        }
                        //js事件
                        if (type == "event") {
                            var share = "?sharekeyid";
                            if (href.indexOf(share) > -1) href = href.substring(0, href.indexOf(share));
                            try {
                                eval(href)();
                            } catch (e) { }
                        }
                        if (type == "item" || type == "node") {
                            var mmid = $(this).attr("mmid");
                            var url = $().setPara(window.location.href, "mmid", mmid);
                            try {
                                history.pushState({}, "", url); //更改地址栏信息
                            } catch (e) {
                                window.location.href = url;
                                return;
                            }
                            menutree.setAdminpage(mmid);
                        }
                    });
                }
                //递归
                menutree.initial(mmbox, level + 1);
            });
        },
        //初始样式，是展开还是折叠
        initstyle: function () {
            //树形菜单的初始展开状态
            var mmitem = $("body");
            if (menutree.state == 1) mmitem = $("#menuBox").find(">.mmitem:first");
            if (menutree.state == 2) mmitem = $("#menuBox").find(".mmitem");
            mmitem.each(function () {
                var mmbox = $(this).next(".mmBox[mmtype!=node]");
                if (mmbox.size() > 0 && mmbox.find(">.mmitem").size() > 0) {
                    $(this).click();
                }
            });
            //$("#menuBox").show();
        },
        //折叠按钮事件
        foldbutton: function () {
            var btn = $("#foldbtn");
            btn.click(function () {
                var cl = $(this).attr('class');
                $(this).attr('class', cl != 'close' ? 'close' : 'open');
                if ($(this).attr('class') == 'close') {
                    $("#leftArea").width('10px').find(">*[id!=foldbtn]").hide(500);
                    $("#rightArea").width('calc(calc(100% - 10px - 2px))');
                } else {
                    $("#leftArea").width('180px').find(">*[id!=foldbtn]").show(500);
                    $("#rightArea").width('calc(calc(100% - 180px - 2px))');
                }
            });
        },
        //设置右侧的内容区域
        setAdminpage: function (mmid) {
            var id = Number(mmid);
            if (id < 1) {
                //$(".lowest").hide();
                var lowest = $(".lowest");
                lowest.empty().show();
                lowest.append("<div class='item curr' mmid='0'>启始页</div>");
                var menutit = $(".menuTitle");
                menutree.setAdminurl(menutit.attr("href"), mmid, menutit.attr("title"));
                return;
            }
            //当前菜单项
            var mm = $("#menuBox .mmitem[mmid=" + id + "]");
            //三种情况，1、节点菜单有下级，2、没有下级；3、本身就是最低一级
            var mmbox = mm.next(".mmBox");  //当前菜单的下级菜单区域块
            var mmpat = mm.parent(".mmBox");  //当前菜单的同级菜单区域块
            //情况一：节点菜单有下级
            if (mm.attr("mmtype") == "node" && mmbox.size() > 0) {
                menutree.setLowest(mmbox, id, 0);
            }
            //情况二三
            if (mm.attr("mmtype") != "node" && mmbox.size() < 1) {
                //情况三、本身就是最低一级
                if (mmpat.size() > 0 && mmpat.attr("mmtype") == "node") {
                    mmbox = mm.parent(".mmBox");
                    menutree.setLowest(mmbox, id, id);
                } else {
                    //情况二：没有下级
                    //$(".lowest").hide();                 
                    menutree.setLowest(mm, id, id);
                    menutree.setAdminurl(mm, id);
                }
            }
        },
        //设置最低一级，即右侧菜单条
        setLowest: function (mmbox, mmid, currid) {
            //右侧菜单条，即最低一级菜单
            var lowest = $(".lowest");
            lowest.empty().show();
            var mmitem = mmbox.find(">.mmitem");
            if (mmitem.length == 0) mmitem = mmbox;

            mmitem.each(function () {
                lowest.append("<div class='item' mmid='" + $(this).attr("mmid") + "'>" + $.trim($(this).text()) + "</div>");
            });

            lowest.find(">.item").click(function () {
                var mmid = $(this).attr("mmid");
                var url = $().setPara(window.location.href, "mmid", mmid);
                try {
                    history.pushState({}, "", url); //更改地址栏信息
                } catch (e) {
                    window.location.href = url;
                    return;
                }
                window.menutree.setAdminpage(mmid);
            });
            //打开，右侧菜单项的第一个菜单
            currid = currid == 0 ? lowest.find(">.item:first").attr("mmid") : currid;
            var curr = $("#menuBox .mmitem[mmid=" + currid + "]");
            //最低一级的选中样式
            lowest.find(">.item").removeClass("curr");
            lowest.find(">.item[mmid=" + currid + "]").addClass("curr");
            //设置管理页面
            menutree.setAdminurl(curr, currid);
        },
        //设置右侧区域的页面
        //item:菜单项，如果是string，则为网址
        //mmid:参数id
        setAdminurl: function (item, mmid, menupath) {
            var url = "";
            if (typeof (item) == 'string') url = item;
            if (item instanceof jQuery) url = item.find("a").attr("href");
            var ifm = document.getElementById("adminPage");
            $("#adminPage").attr("src", url);
            //右侧管理界面上方的导航
            menupath = menupath == null ? menutree.getmenupath(mmid) : menupath;
            $("#menuPath").attr({ "href": url, "mmid": mmid }).html(menupath);
            menutree.loading.open();
        },
        //预载
        loading: {
            open: function () {
                var alpha = 50;     //预载框的透明度
                $("body").append("<div id=\"IframLoadMask\"/>");
                var mask = $("#IframLoadMask");
                var iframe = $("#adminPage");
                var off = iframe.offset();
                $("#IframLoadMask").css({
                    "position": "absolute", "z-index": "10000",
                    "width": iframe.width(), "height": iframe.height(), top: off.top, left: off.left,
                    "background-color": "#ffffff", "filter": "Alpha(Opacity=" + alpha + ")",
                    "display": "block", "-moz-opacity": alpha / 100, "opacity": alpha / 100
                }).fadeIn("slow");
            },
            close: function () {
                $("#IframLoadMask").remove();
            }
        },
        //获取菜单导航路径
        getmenupath: function (mmid) {
            var path = "";
            var mm = $("#menuBox .mmitem[mmid=" + mmid + "]");
            while (mm != null && mm.size() > 0) {
                path = $.trim(mm.text()) + " <i>&gt;</i> " + path;
                var box = mm.parent(".mmBox");
                mm = box.size() > 0 ? box.prev(".mmitem") : null;
                if (mm == null || mm.size() < 0) mm = null;
            }
            return path;
        }
    };
    //将内部变量赋给window，成为全局变量
    window.menutree = menutree;
    $(function () {
        window.menutree.initial($("#menuBox"), 1);
        window.menutree.initstyle();
        window.menutree.foldbutton();
        //主管理区加载完成事件
        $("#adminPage").load(function () {
            window.menutree.loading.close();
        });
    });
})();

window.onload = function () {
    /*//界面布局，自适应宽度
    var left = $("#leftArea");
    //alert(left.parent().width()-left.width()-8);
    $("#rightArea").hide()
        .width(left.parent().width() - left.width() - 8)
        .fadeIn(1000).find("iframe").width($("#rightArea").width() - 20);
    //$("#rightArea .bar").width($("#rightArea").width());
    */
}
//初始代码
$(function () {
    //初如化菜单项
    window.menutree.setAdminpage($().getPara("mmid"));
    //学员中心(左上方）的点击事件
    $(".menuTitle,#menuPath").click(function () {
        var mmid = $(this).attr("mmid");
        var url = $().setPara(window.location.href, "mmid", mmid);
        try {
            history.pushState({}, "", url); //更改地址栏信息
        } catch (e) {
            window.location.href = url;
            return;
        }
        window.menutree.setAdminpage(mmid);
        return false;
    });
});
//获取弹出窗的标题
function getAdminTitle() {
    return $(".menuNav").text();
}
