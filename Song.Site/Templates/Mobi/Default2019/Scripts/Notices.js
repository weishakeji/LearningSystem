$(function () {
	$("#tbSearch").val(decodeURIComponent($().getPara("sear")));
	//搜索框的提交事件
	 $("#formSearch").submit(function(){
	 	var txt=$("#tbSearch").val();
		var sear=$().getPara("sear");
		if($.trim(txt)=="" && sear=="")return false;
	 });
	 mui('body').on('tap', '.btnSearch', function () {
		 var txt=$("#tbSearch").val();
		 var sear=$().getPara("sear");
		if($.trim(txt)=="" && sear=="")return false;
		 var href=$(this).prev("form").attr("action");
		 window.location.href=$().setPara(href,"sear",txt);
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
		var table = $('.mui-table-view');       
        for (var i = 0; i < data.items.length; i++) {
			var d=data.items[i];
            var li = document.createElement('div');
            li.className = 'mui-table-view-cell mui-media news-item';
            li.setAttribute("artid", d.No_Id);			
			var html='<a href="notice.ashx?id='+d.No_Id+'" type="link">'+ unescape(d.No_Ttl);	
			//alert(d.No_StartTime);	
			html+="<span class='mui-badge'>"+d.No_StartTime.Format("MM-dd")+"</span>";
			html+='</a>';
            li.innerHTML = html;
            table.append(li.outerHTML);			
        }		
	});	
}