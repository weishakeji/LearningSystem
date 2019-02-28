
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

var count = 0;

var size=10;	//每页取多少条
var index=1;	//索引页
var sumcount=0;	//总记录数

/**
* 上拉加载具体业务，在尾部加载新内容
*/
function pullupRefresh() {
	setTimeout("ajaxLoaddata()",500);
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
function ajaxLoaddata(){
	index=size*index < sumcount ? ++index : index;
	$.post(window.location.href, { size: size, index: index },function(requestdata){		
		var data=eval("("+requestdata+")");
		sumcount=data.sumcount;
		mui('#pullrefresh').pullRefresh().endPullupToRefresh((size*index >= sumcount)); //参数为true代表没有更多数据了。
		var table = document.body.querySelector('.mui-table-view');
        var cells = document.body.querySelectorAll('.mui-table-view-cell');		
        for (var i = 0; i < data.items.length; i++) {
			var d=data.items[i];
            var li = document.createElement('div');
            li.className = 'mui-table-view-cell mui-media news-item';
			li.setAttribute("artid",d.Art_Id);
			//相关数据
			var defimg=document.getElementById("def-img");
			var img=d.Art_IsImg ? d.Art_Logo : defimg.getAttribute("src");			
			//
			var html='<a href="javascript:;">';
			html+='<img class="mui-media-object mui-pull-left" src="'+img+'"/>';
			html+='<div class="mui-media-body mui-ellipsis">'+unescape(d.Art_Title);
			html+='<p class="mui-ellipsis">'+d.Art_Intro+'</p>';
			html+='</div>';
			html+='</a>';
            li.innerHTML = html;
            table.appendChild(li);			
        }
		 mui('body').off('tap','.news-item');
		 mui('body').on('tap', '.news-item', function () {
				var id=$(this).attr("artid");
				/*$.post("MoneyDetail.ashx?id="+id, function (data) {
					new PageBox("账单详情",data, 100, 100, "text").Open();
				});*/
				new PageBox("新闻", "article.ashx?id="+id, 100, 100, "url").Open();
		});
	});	
}