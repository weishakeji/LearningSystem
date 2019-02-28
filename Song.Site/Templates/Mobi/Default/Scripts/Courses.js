var size = Math.ceil((window.screen.height - 88) / 64); //每页取多少条
var index = 1; //索引页
var sumcount = 0; //总记录数

$(function () {	
	$("#search-top").val(decodeURIComponent($().getPara("sear")));
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
    //筛选相关
	//当在顶部填写搜索内容时
	$("#search-top").focus(function(){
		 cou_select();
		 $(this).blur();
		 $("#tbSearch").focus();	
	});
    //筛选按钮事件，弹出筛选选择框
    mui('body').on('tap', '#btnSelect', function () {
         cou_select();
    });
    //
});
function cou_select(){
	var txt = $("#select-box");
        var box = new PageBox("课程筛选", txt, 100, 100, null, window.name, "obj");
        box.IsBackbtn = true;
        box.Open(function () {
            select_init();
            //设置内容区可以滚动
            $(".pagebox-context").height($(".PageBoxContext").height() - 85);
            //复选框事件，并阻止事件冒泡
            mui('body').off('tap', 'label');
            mui('body').on('tap', 'label', function (event) {
                var cb = $(this).prev("input");
                cb.attr("checked", !cb.is(":checked"));
                cb.parent().next(".sbj-area").find(".checkbox").attr("checked", cb.is(":checked"));
                checkbox_change();
                event.stopPropagation();
            });
            //展开与折叠下级菜单
            $(".sbj-tit").click(function (event) {
                var area = $(this).next(".sbj-area");
                if (area.size() < 1) return;
                //alert($(this).html());
                //判断是否显示  显示：true 隐藏：false
                if (area.is(':visible')) {
                    $(this).next(".sbj-area").hide();
                    $(this).removeClass("open");
                } else {
                    $(this).parent().find(">.sbj-area").hide();
                    $(this).parent().find(">.sbj-tit").removeClass("open");
                    $(this).next(".sbj-area").show();
                    $(this).addClass("open");
                }
                event.stopPropagation();
            });
            //当复选框变更状态时
            $(".checkbox").change(function () {
                $(this).parent().next(".sbj-area").find(".checkbox").attr("checked", $(this).is(":checked"));
                checkbox_change();
            });
            //弹出后的查询按钮事件
            mui('body').off('tap', '#btnSearch');
            mui('body').on('tap', '#btnSearch', function (event) {
                var sbjids = "";
                $(".PageBoxContext .checkbox").each(function () {
                    if ($(this).is(":checked")) sbjids += $(this).attr("sbjid") + ",";
                });
                var href = window.location.href;
                if (href.indexOf("?") > -1) href = href.substring(0, href.indexOf("?"));
                window.location.href = href.replace("#", "") + "?sear=" + encodeURI($(".PageBoxContext #tbSearch").val()) + "&sbjids=" + sbjids;
            });
            mui('body').off('tap', '.sbj-clear');
            mui('body').on('tap', '.sbj-clear', function (event) {
                $(".PageBoxContext .checkbox").attr("checked", false);
                $("input[name=tbSearch]").val("");
                checkbox_change();
            });
            //窗体打开后的事件，到此结束
        });
}

//当筛选框打开时，初始化之前选择的内容
function select_init() {
    //查询字符串
    $(".PageBoxContext #tbSearch").val(decodeURI($().getPara("sear")));
    var sbjids = $().getPara("sbjids").split(",");
    for (s in sbjids) $(".PageBoxContext .checkbox[sbjid=" + sbjids[s] + "]").attr("checked", true);
    checkbox_change();
}
//当复选框变动时
function checkbox_change() {
    var n = $(".PageBoxContext .checkbox:checked").size();
    if (n < 1) $(".sbj-num").hide();
    if (n > 0) {
        $(".sbj-num").html("-选中" + n + "个专业");
        $(".sbj-num").show();
    }
}
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
    var only = "Cou_ID,Cou_Name,Cou_PriceSpan,Cou_PriceUnit,Cou_Price,Cou_LogoSmall,Sbj_Name,Cou_IsFree,Cou_ViewNum";
    var wipe = "";
    var url = window.location.href;
    url = url.indexOf("?") > -1 ? url.substring(0, url.lastIndexOf("?")) : url;
    $.post(url, { size: size, index: index, order: order, sear: sear, sbjids: sbjids, only: only, wipe: wipe },
		function (requestdata) {
		    var data = eval("(" + requestdata + ")");
		    sumcount = data.sumcount;
		    mui('#pullrefresh').pullRefresh().endPullupToRefresh((size * index >= sumcount)); //参数为true代表没有更多数据了。
		    var table = document.body.querySelector('#context-area');
		    for (var i = 0; i < data.items.length; i++) {
		        var d = data.items[i];
		        var li = document.createElement('li');
		        li.className = 'mui-table-view-cell mui-media news-item';
		        li.setAttribute("couid", d.Cou_ID);
		        li.setAttribute("couname", unescape(d.Cou_Name));
		        //相关数据
		        var price = d.Cou_IsFree ? "<span class='free'>免费</span>" : "<span class='price'>" + d.Cou_PriceSpan + unescape(d.Cou_PriceUnit) + d.Cou_Price + "元</span>";
		        var html = '';
		        //向左滑
		        html += '<div class="mui-slider-right mui-disabled"><a class="mui-btn mui-btn-yellow mui-icon mui-icon-chat" couid="' + d.Cou_ID + '"></a></div>';
		        //向右滑
		        html += '<div class="mui-slider-left mui-disabled">';
		        html += '<a class="mui-btn mui-btn-blue mui-icon icotxt">' + d.Cou_ViewNum + '次关注</a>';
		        html += '<a class="mui-btn mui-btn-yellow mui-icon icotxt">章节' + d.olcount + '个<br/>试题' + d.quscount + '道</a>';
		        html += '</div>';
		        //
		        html += '<div class="mui-slider-handle mui-table"><div class="mui-table-cell">';
		        //
		        html += '<a href="javascript:;">';
		        html += '<img class="mui-media-object mui-pull-left" src="' + d.Cou_LogoSmall + '"/>';
		        html += '<div class="mui-media-body mui-ellipsis">' + unescape(d.Cou_Name);
		        html += '<p class="mui-ellipsis">' + price + unescape(d.Sbj_Name) + '</p>';
		        html += '</div>';
		        html += '</a>';
		        //
		        html += '</div></div>';
		        li.innerHTML = html;
		        table.appendChild(li);
				$(li).find("img").error(function(){
					$(this).attr("src",tmpath+"Images/cou-nophoto.png");
				});
			}
			
		    //长按弹出课程详情
		    mui('body').off('tap', '.news-item');
		    mui('body').on('tap', '.news-item', function () {
		        var id = $.trim($(this).attr("couid"));
		        var url = "Course.ashx?id=" + id;
		        history.pushState({}, "", $().setPara(window.location.href, "openurl", BASE64.encoder(url))); //更改地址栏信息
		        new PageBox("课程详情", url, 100, 100,window.name, "url").Open();
		    });
		    mui('body').off('doubletap', '.news-item');
		    mui('body').on('doubletap', '.news-item', function () {
		        var id = $.trim($(this).attr("couid"));
		        var url = "Course.ashx?id=" + id;
		        history.pushState({}, "", $().setPara(window.location.href, "openurl", BASE64.encoder(url))); //更改地址栏信息
		        new PageBox("课程详情", url, 100, 100,window.name, "url").Open();
		    });
		    //向左滑动，弹出咨询交流
		    mui('body').off('slideleft', '.mui-table-view-cell');
		    mui('body').on('slideleft', '.mui-table-view-cell', function (event) {
		        var id = $(this).attr("couid");
		        var name = $(this).attr("couname");
		        new PageBox("《" + name + "》", "MsgBoards.ashx?couid=" + id + "&state=nohead", 100, 100,window.name, "url").Open();
		        mui.swipeoutClose(this);
		    });
		    /*
		    //向右滑动，弹出咨询交流
		    mui('body').off('slideright', '.mui-table-view-cell');
		    mui('body').on('slideright', '.mui-table-view-cell', function (event) {
		    var id = $(this).attr("couid");
		    var name = $(this).attr("couname");
		    new PageBox("《" + name + "》", "MsgBoards.ashx?couid=" + id + "&state=nohead", 100, 100, "url").Open();
		    mui.swipeoutClose(this);
		    });*/
		});
}