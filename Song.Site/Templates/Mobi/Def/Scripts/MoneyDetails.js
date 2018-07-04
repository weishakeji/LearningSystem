
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
    $.post("MoneyDetails.ashx", { size: size, index: index }, function (requestdata) {
        var data = eval("(" + requestdata + ")");
        sumcount = data.sumcount;
        mui('#pullrefresh').pullRefresh().endPullupToRefresh((size * index >= sumcount)); //参数为true代表没有更多数据了。
        var table = document.body.querySelector('#detail-view');
        //alert(table.innerHTML);
        //var cells = document.body.querySelectorAll('.mui-table-view-cell');		
        for (var i = 0; i < data.items.length; i++) {
            var d = data.items[i];
            var li = document.createElement('li');
            li.className = 'money-item mui-table-view-cell';
            li.setAttribute("maid", d.Ma_ID);
            //相关数据
            var type = d.Ma_Type == 1 ? "-" : "+";
            var ico = d.Ma_Type == 1 ? "&#xe62f;" : "&#xe62d;"; 	//支出图标，与收入图标
            var state = d.Ma_IsSuccess ? "成功" : "失败";
            var sourse = d.Ma_IsSuccess ? d.Ma_Source : "(失败)"; //资金来源			
            var date = new Date(d.Ma_CrtTime).Format("MM-dd"); //资金流水的创建日期
            var time = new Date(d.Ma_CrtTime).Format("hh:mm"); //资金流水的创建时时间
			var remark=$.trim(d.Ma_Remark)!="" ? "(有备注)" : "";
            //
            var html = '';
            html += '<div class="mui-slider-right mui-disabled"><a class="mui-btn mui-btn-red mui-icon mui-icon-trash" maid="' + d.Ma_ID + '"></a></div>';
            html += '<div class="mui-slider-handle mui-table"><div class="mui-table-cell">';
            //
            html += '<div class="dates">' + date + '<br/>' + time + '</div>';
            html += '<div class="ico"><div class="moneyico type' + Number(d.Ma_Type) + '">' + ico + "</div></div>";
            html += '<div class="info">';
            html += '<div class="row1"><div class="money">' + type + ' ' + Number(d.Ma_Money) + '</div> <div class="state state' + Number(d.Ma_IsSuccess) + '">' + state + '</div></div>';
            html += '<div class="row2">' + d.Ma_Source +remark+ '</div>';
            html += '</div>';
            //
            html += '</div></div>';
            li.innerHTML = html;
            table.appendChild(li);
        }
        //弹出详情
        mui('body').off('tap', '.money-item');
        mui('body').on('tap', '.money-item', function () {
            var id = $(this).attr("maid");
            new PageBox("账单详情", "MoneyDetail.ashx?id=" + id, 100, 100, "url").Open();
        });
        //删除记录
        mui('body').off('tap', '.mui-btn');
        mui('body').on('tap', '.mui-btn', function (event) {
            var maid = $(this).attr("maid");
            $(".money-item[maid=" + maid + "]").remove();
        });
        //向左滑动，直接弹出删除提示
        mui('#detail-view').off('slideleft', '.mui-table-view-cell');
        mui('#detail-view').on('slideleft', '.mui-table-view-cell', function (event) {
            var elem = this;
            mui.confirm('确认删除该条记录？', '删除', ['取消', '确认'], function (e) {
                if (e.index == 1) {
                    var maid = elem.getAttribute("maid");
                    $.post("MoneyDetail.ashx?id=" + maid, { action: "delete", id: maid }, function (data) {
                        if (data == "1") {
                            mui.toast('删除成功！', { duration: 1000, type: 'div' });
                        } else {
                            mui.toast('系统错误！', { duration: 1000, type: 'div' });
                        }
                        $(".money-item[maid=" + maid + "]").remove();
                    });

                } else {
                    setTimeout(function () {
                        mui.swipeoutClose(elem);
                    }, 0);
                }
            });
        });
    });
}
