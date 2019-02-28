
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
	//限制输出字段，Only为只输出某些字段,wipe表示不输出某些字段
    var only = "";
	var wipe = "Tr_Results";
	var url=window.location.href;
	//url=url.indexOf("?")>-1 ? url.substring(0,url.lastIndexOf("?")) : url;
	$.post(url, { size: size, index: index, action: "list", only: only, wipe: wipe }, function (requestdata) {
	    var data = eval("(" + requestdata + ")");
	    sumcount = data.sumcount;
	    mui('#pullrefresh').pullRefresh().endPullupToRefresh((size * index >= sumcount)); //参数为true代表没有更多数据了。
	    var table = document.body.querySelector('#detail-view');
	    for (var i = 0; i < data.items.length; i++) {
	        var d = data.items[i];
	        var li = document.createElement('li');
	        li.className = 'note-item mui-table-view-cell';
	        li.setAttribute("trid", d.Tr_ID);
	        li.setAttribute("tpid", d.Tp_Id);
	        //相关数据           
	        var date = new Date(d.Tr_CrtTime).Format("yyyy-MM-dd"); //创建日期
	        var time = new Date(d.Tr_CrtTime).Format("hh:mm"); //创建时时间	
	        var pass = Number(d.Tr_Score) < Number(d.Tp_PassScore) ? "<span class='nopass'>(不及格)</span>" : "";
	        var score = "<span class='score'>" + d.Tr_Score + " 分" + pass + "</span>";
	        var html = '';
	        //向左滑
	        html += '<div class="mui-slider-right mui-disabled"><a class="mui-btn mui-btn-red mui-icon mui-icon-trash" action="del"></a></div>';                       //
	        html += '<div class="mui-slider-handle mui-table"><div class="mui-table-cell">';
	        //
	        html += '<a href="javascript:;">';
			if(d.Tp_Logo!=""){
	        html += '<img class="mui-media-object mui-pull-left" src="' + d.Tp_Logo + '"/>';
			}else{
				html += '<div class="rowlogo"><span class="ico">&#xe73e;</span></div>';
			}
	        html += '<div class="mui-media-body mui-ellipsis">' + unescape(d.Tp_Name);
	        html += '<p class="mui-ellipsis">' + score + date + " " + time + '</p>';
	        html += '</div>';
	        html += '</a>';
	        //
	        html += '</div></div>';
	        li.innerHTML = html;
	        table.appendChild(li);
	    }
	    //弹出详情
	    mui('body').off('tap', '.note-item');
	    mui('body').on('tap', '.note-item', function () {
	        var trid = $(this).attr("trid");
	        new PageBox("试卷回顾", "TestView.ashx?trid=" + trid, 100, 100, null).Open();
	    });
	    //删除与编辑按钮的事件
	    mui('#detail-view').off('slideleft', '.mui-table-view-cell');
	    mui('#detail-view').on('slideleft', '.mui-table-view-cell', function (event) {
	        var elem = this;
	        var trid = elem.getAttribute("trid");
	        var action = $(this).attr("action");

	        mui.confirm('确认删除测试成绩？', '删除', ['取消', '确认'], function (e) {
	            if (e.index == 1) {
	                $.post("Statistics.ashx", { action: "delete", trid: trid }, function (data) {
	                    if (data == "1") {
	                        mui.toast('删除成功！', { duration: 1000, type: 'div' });
	                        $(".note-item[trid=" + trid + "]").remove();
	                    } else {
	                        mui.toast('系统错误！', { duration: 1000, type: 'div' });
	                    }
	                });

	            } else {
	                setTimeout(function () { mui.swipeoutClose(elem); }, 0);
	            }
	        });
	        //
	    });
	});
}
