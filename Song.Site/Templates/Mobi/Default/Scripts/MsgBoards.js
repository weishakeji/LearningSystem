$(function () {
    //提交留言
    $("form").submit(function(){
		var href= document.location.href;	//要请求的网址
		var vcode=$("input[name=tbCode]").val();	//验证码
		var msg=$("textarea[name=msg]").val();	//留言内容
		$.post(href, { vcode: vcode, msg: msg, action: "submit" },function(data){
				 if(data=='-1'){
					 mui.alert('验证码不正确', '警告');
				 }
				 if(data=='1'){
					 mui.alert('信息提交成功，请等待审核', '成功');
				 }
			});	
		return false;
	});
});



var size = 3; //每页取多少条
var index = 1; //索引页
var sumcount = 0; //总记录数
/*
下拉刷新事件
说明：第一次加载会先执行上拉事件
*/
mui.init({
    pullRefresh: {
        container: '#pullrefresh',
        down: {
            height: 50, //可选,默认50.触发下拉刷新拖动距离,
            auto: true, //可选,默认false.自动下拉刷新一次
            contentdown: "下拉可以刷新", //可选，在下拉可刷新状态时，下拉刷新控件上显示的标题内容
            contentover: "释放立即刷新", //可选，在释放可刷新状态时，下拉刷新控件上显示的标题内容
            contentrefresh: "正在刷新...", //可选，正在刷新状态时，下拉刷新控件上显示的标题内容
            callback: pulldownRefresh //必选，刷新函数，根据具体业务来编写，比如通过ajax从服务器获取新数据；
        }
    }
});


/**
* 上拉加载具体业务，在尾部加载新内容
*/
function pulldownRefresh() {
    setTimeout("ajaxLoaddata()", 500);
}

//异步加载数据
function ajaxLoaddata() {
    index = size * index < sumcount ? ++index : index;
    var url = window.location.href;
    url = url.indexOf("?") > -1 ? url.substring(0, url.lastIndexOf("?")) : url;
    $.post(url, { size: size, index: index, action: "list" }, function (requestdata) {
        var data = eval("(" + requestdata + ")");
        sumcount = data.sumcount;
        var views = $('#detail-view');		
        for (var i = 0; i < data.items.length; i++) {
            if ($(".item[mbid=" + data.items[i].Mb_Id + "]").size() > 0) continue;
            if (data.items[i].Mb_IsAns) {
                var html = template('th_template', data.items[i]);
                views.prepend(html);
            }
            var html = template('st_template', data.items[i]);
            views.prepend(html);            
        }
        $("#detail-view .item:hidden").fadeIn(500, function () {
            var photo = $(this).find(".photo");
            photo.animate({ height: photo.outerWidth()});
        });
		if($("#detail-view .item").size()<1){
			views.html('<ul id="detail-view" class="mui-table-view"><li class="note-item mui-table-view-cell">没有信息</li></ul>');
		}
        mui('#pullrefresh').pullRefresh().endPulldownToRefresh();
        mui('#pullrefresh').pullRefresh().endPulldownToRefresh((size * index >= sumcount)); //参数为true代表没有更多数据了。  
    });
}
//学生消息输出
function _stMsg(obj) {
}