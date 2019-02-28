var size = Math.ceil((window.screen.height - 88) / 64); //每页取多少条
var index = 1; //索引页
var sumcount = 0; //总记录数
$(function () {
    //选项卡切换事件
    mui('body').on('tap', '.mui-control-item', function () {
        //下面三行，是为了防止重复点击选项卡
        var order = $(this).attr("order");
        if (window.order == order) return;
        window.order = order;
        //开始重新加载
        $("#context-area").html("");
        index = 1;
        sumcount = 0;
        mui('#pullrefresh').pullRefresh().refresh(true);
        mui('#pullrefresh').pullRefresh().pullupLoading();
        mui('#pullrefresh').pullRefresh().scrollTo(0, 0, 0);
    });
});
/*
下拉刷新事件
说明：第一次加载会先执行上拉事件
*/
mui.init({
    pullRefresh: {
        container: '#pullrefresh',
        up: {
            callback: pullupRefresh,
            contentinit: '上拉显示更多',
            contentdown: '上拉显示更多',
            contentrefresh: '正在加载...',
            contentnomore: '没有更多数据了'
        }
    },
    gestureConfig: {
        tap: true, //默认为true
        doubletap: true, //默认为false
        longtap: true, //默认为false
        swipe: true, //默认为true
        drag: true, //默认为true
        hold: false, //默认为false，不监听
        release: false//默认为false，不监听
    }
});

/**
* 上拉加载具体业务，在尾部加载新内容
*/
function pullupRefresh() {
    setTimeout("ajaxLoaddata()", 200);
}
if (mui.os.plus) {
    mui.plusReady(function () {
        setTimeout(function () {
            mui('#pullrefresh').pullRefresh().pullupLoading();
        }, 1000);

    });
} else {
    mui.ready(function () {
        mui('#pullrefresh').pullRefresh().pullupLoading();
    });
}
//异步加载数据
function ajaxLoaddata() {
    index = size * index < sumcount ? ++index : index;
    var order = $(".mui-active").attr("order");
    var sear = $().getPara("sear");
    var sbjids = $().getPara("sbjids");
    //限制输出字段，Only为只输出某些字段,wipe表示不输出某些字段
    var only = "Cou_ID,Cou_Name,Cou_PriceSpan,Cou_PriceUnit,Cou_Price,Cou_LogoSmall,Sbj_Name,Cou_IsFree,Cou_IsLimitFree,Cou_FreeEnd,Cou_IsRec,Cou_ViewNum";
    var wipe = "";
    var url = window.location.href;
    url = url.indexOf("?") > -1 ? url.substring(0, url.lastIndexOf("?")) : url;
    $.post(url, { size: size, index: index, order: order, sear: sear, sbjids: sbjids, only: only, wipe: wipe },
		function (requestdata) {
		    var data = eval("(" + requestdata + ")");
		    sumcount = data.sumcount;
		    mui('#pullrefresh').pullRefresh().endPullupToRefresh((size * index >= sumcount)); //参数为true代表没有更多数据了。
		    var table = $('#context-area');
		    for (var i = 0; i < data.items.length; i++) {
		        var d = data.items[i];
		        var li = document.createElement('li');
		        li.className = 'mui-table-view-cell mui-media cour-row';
		        li.setAttribute("couid", d.Cou_ID);
		        li.setAttribute("couname", unescape(d.Cou_Name));
		        var html = '';
		        //向左滑
		        html += '<div class="mui-slider-right mui-disabled"><a class="mui-btn mui-btn-yellow mui-icon mui-icon-chat" couid="' + d.Cou_ID + '"></a></div>';
		        //向右滑
		        html += '<div class="mui-slider-left mui-disabled">';
		        html += '<a class="mui-btn mui-btn-blue mui-icon muileft"><view>关注' + d.Cou_ViewNum + '次</view><outline>章节' + d.olcount + '个</outline><ques>试题' + d.quscount + '道</ques></a>';
		        //html += '<a class="mui-btn mui-btn-yellow mui-icon"></a>';
		        html += '</div>';
		        //----课程信息展示开始
		        html += '<div class="mui-slider-handle mui-table"><div class="mui-table-cell cour-box">';
		        //
		        html += buildCourse(d); ;
		        //----课程信息展示结束
		        html += '</div></div>';
		        li.innerHTML = html;
		        table.append(li);
				//是否隐藏收费信息
				var mremove=$("body").attr("mremove")=="True" ? true : false;
				if(mremove){
					$(li).find("price,free,limitfree").hide();
				}
		        $(li).find("img").error(function () {
		            var errImg = $(this).attr("default");
		            if (errImg == null) return false;
		            $(this).attr("src", errImg);
		        });
		    }

		    //长按弹出课程详情
		    mui('body').off('tap', '.cour-row');
		    mui('body').on('tap', '.cour-row', function () {
		        var id = $.trim($(this).attr("couid"));
		        var url = "Course.ashx?id=" + id;
		        var name = $(this).attr("couname");
		        history.pushState({}, "", $().setPara(window.location.href, "openurl", BASE64.encoder(url))); //更改地址栏信息
		        new PageBox(name, url, 100, 100, window.name, "url").Open();
		    });
		    mui('body').off('doubletap', '.cour-row');
		    mui('body').on('doubletap', '.cour-row', function () {
		        var id = $.trim($(this).attr("couid"));
		        var url = "Course.ashx?id=" + id;
		        var name = $(this).attr("couname");
		        history.pushState({}, "", $().setPara(window.location.href, "openurl", BASE64.encoder(url))); //更改地址栏信息
		        new PageBox(name, url, 100, 100, window.name, "url").Open();
		    });
		    //向左滑动，弹出咨询交流
		    mui('body').off('slideleft', '.mui-table-view-cell');
		    mui('body').on('slideleft', '.mui-table-view-cell', function (event) {
		        var id = $(this).attr("couid");
		        var name = $(this).attr("couname");
		        new PageBox("《" + name + "》", "MsgBoards.ashx?couid=" + id + "&state=nohead", 100, 100, window.name, "url").Open();
		        mui.swipeoutClose(this);
		    });
		});
}
//构建课程信息
function buildCourse(cour) {
    var defimg = $(".default-img").attr("default"); //默认图片
    var html = "<picture>{rec}{free}{limitfree}<img src='{logo}' default='{defimg}'/></picture><info>{name}{number}{sbjname}<price>{price}</price></info>";
    html = html.rep("{logo}", unescape(cour.Cou_LogoSmall));
    html = html.replace("{name}", "<name>" + unescape(cour.Cou_Name) + "</name>").replace("{sbjname}", "<sbjname>" + unescape(cour.Sbj_Name) + "</sbjname>");
    html = html.replace("{id}", cour.Cou_ID).replace("{defimg}", defimg);
    html = html.replace("{rec}", (cour.Cou_IsRec ? "<rec></rec>" : ""));
    html = html.replace("{free}", (cour.Cou_IsFree ? "<free></free>" : ""));
    html = html.replace("{limitfree}", (cour.Cou_IsLimitFree ? "<limitfree></limitfree>" : ""));
    //浏览数、章节数、试题数
    var numstr = '<view>' + cour.Cou_ViewNum + '</view><outline>' + cour.olcount + '</outline><ques>' + cour.quscount + '</ques>';
    html = html.replace("{number}", "<number>" + numstr + "</number>");
    //价格
    var price = "";
    if (cour.Cou_IsFree) {
        price = "<f>免费</f>";
    } else {
        if (cour.Cou_IsLimitFree) {
            var end = cour.Cou_FreeEnd.Format("yyyy年M月d日");
            price = "<l>免费至 <t>" + end + "</t></l>";
        } else {
            price = "<m>" + cour.Cou_PriceSpan + unescape(cour.Cou_PriceUnit) + cour.Cou_Price + "元</m>";
        }
    }
    html = html.replace("{price}", price);
    return html;
}

/*课程分类的弹出*/
$(function () {
    $(".sbj-panel").height($(window).height() - 40).click(function () {
        mui.trigger(document.querySelector('.foot-courses'), 'tap');
    });
    $(".sbj-left, .sbj-panel .sbj-area").height($(window).height() - 50);
    $(".sbj-panel sbj-tit:first").addClass("current");
    //课程专业分类面板的弹出与隐藏
    mui('body').on('tap', '.foot-courses', function () {
        var state = $(this).attr("state");
        var panel = $(".sbj-panel");
        if (panel.size() > 0 && state != "open") {
            panel.height($(window).height() - 40).width($(window).width());
            panel.css("left", $(window).width()).show();
            panel.animate({ left: 0 }, 200);
            $(this).attr("state", "open");
        }
        if (panel.size() > 0 && state == "open") {
            panel.animate({ left: $(window).width() }, 200);
            $(this).attr("state", "close");
        }
        return false;
    });
    //专业分类的切换展示
    mui('body').on('tap', '.sbj-panel sbj-tit', function () {
        var sbjid = $(this).attr("sbjid");
        $(".sbj-panel sbj-tit[sbjid=" + sbjid + "]").addClass("current")
			.siblings("[sbjid!=" + sbjid + "]").removeClass("current");
        $(".sbj-panel .sbj-area[sbjid=" + sbjid + "]").show()
			.siblings(".sbj-area[sbjid!=" + sbjid + "]").hide();
    });
    mui.trigger(document.querySelector('.sbj-panel sbj-tit.current'), 'tap');
    //专业分类的点击事件
    mui('body').on('tap', '.sbj-panel label', function () {
        var sbjid = $(this).attr("sbjid");
        if (sbjid == null) return;
        var href = window.location.href;
        if (href.indexOf("?") > -1) href = href.substring(0, href.indexOf("?"));
        window.location.href = href.replace("#", "") + "?sbjids=" + sbjid;
    });
    mui('body').on('tap', '.sbj-panel .sbj-two-name', function () {
        var sbjid = $(this).attr("sbjid");
        $(this).next().find("label").each(function (index, element) {
            sbjid += "," + $(this).attr("sbjid");
        });
        var href = window.location.href;
        if (href.indexOf("?") > -1) href = href.substring(0, href.indexOf("?"));
        window.location.href = href.replace("#", "") + "?sbjids=" + sbjid;
    });
});