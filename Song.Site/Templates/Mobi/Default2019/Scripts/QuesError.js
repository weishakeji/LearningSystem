window.onload = function () {
    //总题数
    var count = Number($("body").attr("quscount"));
    //设置试题宽度
    var wd = $(window).width();
    var hg = document.querySelector(".context").clientHeight;
    $("#quesArea").width(wd * (count==0 ? 1 : count + 10));
    //设置题型
    var quesTypes = $("body").attr("questype").split(",");
    //设置宽高，试题类型
    $(".quesItem").width(wd).height(hg).each(function (index, element) {
        var type = Number($(this).attr("type"));
        $(this).find(".ques-type").text("【" + $.trim(quesTypes[type - 1]) + "题】");
        if (type == 1 || type == 3) {
            $(this).find(".btnSubmit").hide();
        }
        //收藏图标的状态
        var isCollect = $(this).attr("IsCollect") == "True" ? true : false;
        if (isCollect) {
            $(this).find(".btnFav").addClass("IsCollect");
        }
    });
    //选项的序号，数字转字母
    $(".quesItemsBox").each(function () {
        $(this).find(">div").each(function (index, element) {
            var char = String.fromCharCode(0x41 + index);
            $(this).find("b").after(char + "、");
        });
    });
    //左右滑动切换试题
    finger.init();
}

$(function () {
    //删除试题
    $(".btnDel").click(function () {
		if(card.size()<1){
			 new MsgBox("提示", "没有试题供操作！", 90, 180, "msg").Open();
			 return;
		}
        var msg = new MsgBox("删除", "您是否确认删除当前错题？", 90, 180, "confirm");
        msg.EnterEvent = function () {
            var qid = card.currid();
            //记录学习进度
            $.post(window.location.href, { action: "delete", qid: qid }, function (data) {
                if (data == "1") {
                    var msg = new MsgBox("成功", "删除成功！！<br/><br/><second>2</second>秒关闭消息", 90, 180, "msg");
                    msg.Open();
                    //移动试题
                    card.remove(card.currid());                   

                } else {
                    var msg = new MsgBox("失败", data, 90, 180, "msg");
                    msg.Open();
                }
            });
            msg.Close(msg.WinId);
        }
        msg.Open();
    });
    //清空错题
    $(".btnClear").click(function () {
		if(card.size()<1){
			 new MsgBox("提示", "没有试题供操作！", 90, 180, "msg").Open();
			 return;
		}
        var msg = new MsgBox("清空", "您是否确认清空所有错题？", 90, 180, "confirm");
        msg.EnterEvent = function () {
            //记录学习进度
            $.post(window.location.href, { action: "clear",couid:$().getPara("couid")}, function (data) {
                if (data == "1") {
                    var msg = new MsgBox("成功", "所有错题被清空。<br/>请退出当前界面！", 90, 180, "msg");
					msg.OverEvent=function(){
						window.location.href="CoursePage.ashx";
					}
                    msg.Open(); 
					card.clear(); 
					           

                } else {
                    var msg = new MsgBox("失败", data, 90, 180, "msg");
                    msg.Open();
                }
            });
            msg.Close(msg.WinId);
        }
        msg.Open();
    });
});
