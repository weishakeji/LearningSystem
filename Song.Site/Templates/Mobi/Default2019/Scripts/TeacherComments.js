
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


var size = 10; //每页取多少条
var index = 1; //索引页
var sumcount = 0; //总记录数

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
    var url = window.location.href;
    $.post(url, { size: size, index: index }, function (requestdata) {
        var data = eval("(" + requestdata + ")");
        sumcount = data.sumcount;
        mui('#pullrefresh').pullRefresh().endPullupToRefresh((size * index >= sumcount)); //参数为true代表没有更多数据了。
        var table = document.body.querySelector('#detail-view');
        for (var i = 0; i < data.items.length; i++) {
            var d = data.items[i];
            var li = document.createElement('div');
            li.className = 'mui-card';
            li.setAttribute("id", 'muicard_' + d.Thc_ID);
            //标题（页 眉）
            var tit = document.createElement('div');
            tit.className = 'mui-card-header';
            tit.innerHTML = "<span class='star' score='" + d.Thc_Score + "' size='10'></span>";
            li.appendChild(tit);
            //内容
            var cont = document.createElement('div');
            cont.className = 'mui-card-content';
            cont.innerHTML = "<div class='content'>" + unescape(d.Thc_Comment) + "</div>";
			if(d.Thc_Reply!=null && d.Thc_Reply.length>0){
				cont.innerHTML += "<div class='reply'>回复：" + unescape(d.Thc_Reply) + "</div>";
			}
            li.appendChild(cont);
            //页脚
            var foot = document.createElement('div');
            foot.className = 'mui-card-footer';
            foot.innerHTML = "<span>" + new Date(d.Thc_CrtTime).Format("yyyy-MM-dd hh:mm:ss") + "</span>";
            foot.innerHTML += "<span>匿名</span>";
            li.appendChild(foot);
            table.appendChild(li);
            //设置星标
            Star.Init(li);
        }
    });
}

/*星标相关*/
function Star() { };
//初始化
Star.Init = function (element) {
    var id = element.getAttribute("id");
    var card = $("#" + id);
    //设置星标个数与宽度
    var star = card.find("span.star");
    var num = Math.floor((star.parent().width() - 40) / 28);
    star.attr("size",  num > 10 ? 10 : num);
    Star.Set(card);		//设置星星个数
}
//设置星标
Star.Set = function (card) {
    card.find("span.star").each(function (index, element) {
        //显示多少个星标
        var size = Number($(this).attr("size"));
        if (isNaN(size)) size = 5;
        //当前得分
        var score = Number($(this).attr("score"));
        if (isNaN(score)) return true;
        score = score / (10 / size);
        //生成空的星标
        while (size-- > 0) $(this).append("<i></i>");
        //生成实的星标
        var tm = Math.floor(score);
        $(this).find("i:lt(" + tm + ")").addClass("s1");
        //生成半实的星标
        if (score > tm) $(this).find("i:eq(" + tm + ")").addClass("s0");
        $(this).append("<b>" + $(this).attr("score") + "分</b>");
    });
}