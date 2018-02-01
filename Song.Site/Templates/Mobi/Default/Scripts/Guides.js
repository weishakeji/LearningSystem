var size = 10; //每页取多少条
var index = 1; //索引页
var sumcount = 0; //总记录数

$(function () {
    //筛选相关
    //筛选按钮事件，弹出筛选选择框
    mui('body').on('tap', '#btnSelect', function () {
        var txt = $("#select-box");
        var box = new PageBox("通知公告检索", txt, 100, 100, "obj");
        box.IsBackbtn = true;
        box.Open(function () {
            select_init();
            //设置内容区可以滚动
            $(".pagebox-context").height($("#PageBoxContext").height() - 85);
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
                var sorts = "";
                $("#PageBoxContext .checkbox").each(function () {
                    if ($(this).is(":checked")) sorts += $(this).attr("sortid") + ",";
                });
                var href = window.location.href;
                if (href.indexOf("?") > -1) href = href.substring(0, href.indexOf("?"));
                window.location.href = href.replace("#", "") + "?sear=" + encodeURI($("#PageBoxContext #tbSearch").val()) + "&sorts=" + sorts;
            });
            mui('body').off('tap', '.sbj-clear');
            mui('body').on('tap', '.sbj-clear', function (event) {
                $("#PageBoxContext .checkbox").attr("checked", false);
                checkbox_change();
            });
            //窗体打开后的事件，到此结束
        });
    });
    //
});
//当筛选框打开时，初始化之前选择的内容
function select_init() {
    //查询字符串
    $("#PageBoxContext #tbSearch").val(decodeURI($().getPara("sear")));
    var sorts = $().getPara("sorts").split(",");
    for (s in sorts) $("#PageBoxContext .checkbox[sortid=" + sorts[s] + "]").attr("checked", true);
    checkbox_change();
}
//当复选框变动时
function checkbox_change() {
    var n = $("#PageBoxContext .checkbox:checked").size();
    if (n < 1) $(".sbj-num").hide();
    if (n > 0) {
        $(".sbj-num").html("-选中" + n + "个");
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
    }
});


/**
* 上拉加载具体业务，在尾部加载新内容
*/
function pullupRefresh() {
    setTimeout("ajaxLoaddata()", 500);
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
	var sear = $().getPara("sear");
    var sorts = $().getPara("sorts");
    $.post(window.location.href, { size: size, index: index, sear: sear, sorts: sorts }, function (requestdata) {		
        var data = eval("(" + requestdata + ")");
        sumcount = data.sumcount;
        mui('#pullrefresh').pullRefresh().endPullupToRefresh((size * index >= sumcount)); //参数为true代表没有更多数据了。
        var table = document.body.querySelector('#detail-view');
        for (var i = 0; i < data.items.length; i++) {
            var d = data.items[i];
            var li = document.createElement('li');
            li.className = 'note-item mui-table-view-cell';
            li.setAttribute("gid", d.Gu_Id);
            //相关数据           
            var date = new Date(d.Gu_CrtTime).Format("MM-dd"); //创建日期
            var time = new Date(d.Gu_CrtTime).Format("hh:mm"); //的创建时时间			
            //
            var html = '';
            //html += '<div class="mui-slider-right mui-disabled" trid="' + d.Tr_ID + '" tpid="' + d.Tp_Id + '">';
            //html += '<a class="mui-btn mui-btn-red mui-icon mui-icon-trash" action="del"></a>';
            //html += '<a class="mui-btn mui-btn-blue mui-icon mui-icon mui-icon-compose"  action="edit"></a>';
			//html += "</div>";
            html += '<div class="mui-slider-handle mui-table"><div class="mui-table-cell">';
            html += '<a class="mui-navigate-right" href="#"><span class="iconfont ico">&#xe697;</span>' + d.Gu_Title + '</a>';
            html += '</div></div>';
            li.innerHTML = html;
            table.appendChild(li);
        }
        //弹出详情
        mui('body').off('tap', '.note-item');
        mui('body').on('tap', '.note-item', function () {
           var gid = $(this).attr("gid");
           new PageBox("通知公告","Guide.ashx?id="+gid,100,100,null).Open();
        });       
    });
}
