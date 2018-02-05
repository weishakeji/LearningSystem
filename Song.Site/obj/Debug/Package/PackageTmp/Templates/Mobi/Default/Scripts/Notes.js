
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
    $.post("Notes.ashx", { size: size, index: index, action: "list" }, function (requestdata) {
        var data = eval("(" + requestdata + ")");
        sumcount = data.sumcount;
        mui('#pullrefresh').pullRefresh().endPullupToRefresh((size * index >= sumcount)); //参数为true代表没有更多数据了。
        var table = document.body.querySelector('#detail-view');
        for (var i = 0; i < data.items.length; i++) {
            var d = data.items[i];
            var li = document.createElement('li');
            li.className = 'note-item mui-table-view-cell';
            li.setAttribute("nid", d.Stn_ID);
			li.setAttribute("qid", d.Qus_ID);
            //相关数据           
            var date = new Date(d.Stn_CrtTime).Format("MM-dd"); //资金流水的创建日期
            var time = new Date(d.Stn_CrtTime).Format("hh:mm"); //资金流水的创建时时间			
            //
            var html = '';
            html += '<div class="mui-slider-right mui-disabled" nid="' + d.Stn_ID + '" qid="' + d.Qus_ID + '">';
            html += '<a class="mui-btn mui-btn-red mui-icon mui-icon-trash" action="del"></a>';
            html += '<a class="mui-btn mui-btn-blue mui-icon mui-icon mui-icon-compose"  action="edit"></a>';
            html += '</div><div class="mui-slider-handle mui-table"><div class="mui-table-cell">';
            html += '<a class="mui-navigate-right" href="#"><span class="iconfont ico">&#xe657;</span>' + d.Qus_Title + '</a>';
            html += '</div></div>';
            li.innerHTML = html;
            table.appendChild(li);
        }
        //弹出详情
        mui('body').off('tap', '.note-item');
        mui('body').on('tap', '.note-item', function () {
           var qid = $(this).find(">div").attr("qid");
           new PageBox("查看试题","Questions.ashx?qid="+qid,100,100,null).Open();
        });
        //删除与编辑按钮的事件
        mui('body').off('tap', '.mui-btn');
        mui('body').on('tap', '.mui-btn', function (event) {
            var elem = this.parentNode.parentNode
            var nid = elem.getAttribute("nid");
            var action = $(this).attr("action");
            if (action == "del") {
                mui.confirm('确认删除该条记录？', '删除', ['取消', '确认'], function (e) {
                    if (e.index == 1) {
                        $.post("Notes.ashx", { action: "delete", id: nid }, function (data) {
                            if (data == "1") {
                                mui.toast('删除成功！', { duration: 1000, type: 'div' });
                            } else {
                                mui.toast('系统错误！', { duration: 1000, type: 'div' });
                            }
                            $(".note-item[nid=" + nid + "]").remove();
                        });

                    } else {
                        setTimeout(function () { mui.swipeoutClose(elem); }, 0);
                    }
                });
            }
            if (action == "edit") {
                 new PageBox("编辑笔记", "QuesNoteAdd.ashx?stnid=" + nid, 90, 50, "url").Open();
            }
            //
        });
    });
}
