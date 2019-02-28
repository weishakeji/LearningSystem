var size = 10; //每页取多少条
var index = 1; //索引页
var sumcount = 0; //总记录数


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
	var canStudy=$("body").attr("canStudy");
	if(canStudy!="True"){
		$(".mui-pull").html("请购买当前课程");
		return;
	}
    index = size * index < sumcount ? ++index : index;
	var sear = $().getPara("sear");
    var sorts = $().getPara("sorts");
    $.post(window.location.href, { size: size, index: index, sear: sear, sorts: sorts  }, function (requestdata) {
        var data = eval("(" + requestdata + ")");
        sumcount = data.sumcount;
        mui('#pullrefresh').pullRefresh().endPullupToRefresh((size * index >= sumcount)); //参数为true代表没有更多数据了。
        var table = document.body.querySelector('#detail-view');
        for (var i = 0; i < data.items.length; i++) {
            var d = data.items[i];
            var li = document.createElement('li');
            li.className = 'note-item mui-table-view-cell';
            li.setAttribute("kid", d.Kn_ID);
            //相关数据           
            var date = new Date(d.Kns_CrtTime).Format("MM-dd"); //创建日期
            var time = new Date(d.Kns_CrtTime).Format("hh:mm"); //创建时时间			
            //
            var html = '';           
            html += '<div class="mui-slider-handle mui-table"><div class="mui-table-cell">';
            html += '<a class="mui-navigate-right" href="#"><span class="ico"></span>' + unescape(d.Kn_Title) + '</a>';
            html += '</div></div>';
            li.innerHTML = html;
            table.appendChild(li);
        }
        //弹出详情
        mui('body').off('tap', '.note-item');
        mui('body').on('tap', '.note-item', function () {
           var kid = $(this).attr("kid");
           new PageBox("知识阅读","Knowledge.ashx?id="+kid,100,100,null).Open();
        });        
    });
}
