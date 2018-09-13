window.onload=function(){
	//总题数
	var count=Number($().getPara("count"));
    //设置试题宽度
    var wd = $(window).width();
    var hg = document.querySelector(".context").clientHeight;
	var area = $("#quesArea");
	area.width(wd * count);	
	var quesTypes=$("body").attr("questype").split(",");
    $(".quesItem").width(wd).height(hg).each(function(index, element) {
        var type=Number($(this).attr("type"));
		$(this).find(".ques-type").text("【"+$.trim(quesTypes[type-1])+"题】");
    });
	
    //$("#quesArea").width(wd.width() * count);
    //设置初始的题型
	var qid=$().getPara("qid");
	var q= $(".quesItem[qid="+qid+"]");
    var firstQitem =qid!="" ?  $(".quesItem[qid="+qid+"]"): $(".quesItem:first");
    $("#quesType span[type=" + firstQitem.attr("type") + "]").show();
    $("#quesType span[type!=" + firstQitem.attr("type") + "]").hide();
    //试题是否收藏的显示切换
    var isCollect = firstQitem.attr("IsCollect") == "True" ? true : false;
    if (isCollect) {
        $("#btnFav").addClass("IsCollect");
    } else {
        $("#btnFav").removeClass("IsCollect");
    }	
	//选项的序号，数字转字母
	$(".quesItemsBox").each(function() {
        $(this).find(">div").each(function(index, element) {
            var char=String.fromCharCode(0x41+index);
			$(this).find(">span").after(char);
        });
    });
	//答题卡
    buildCard();
    setCardState("curr",Number(firstQitem.attr("qid")));
	quesAreaMove((Number(firstQitem.attr("index"))-1)*$(".quesItem").width()) ;	
}
$(function () {
    //左右滑动切换试题
    $(".context").swipe({ fingers: 'all', swipeLeft: swipeFuc, swipeRight: swipeFuc,
        pinchIn: swipePinch, pinchOut: swipePinch
    });
    //字体控件
    $().setFontSize($(".quesBox"));
    $(".fontoper").click(function () {
        if ($(this).attr("id") == "addFont") $().setFontSize($(".quesBox"), 2);
        if ($(this).attr("id") == "subFont") $().setFontSize($(".quesBox"), -2);
    });
});
//放大与捏合事件
function swipePinch(event, direction, distance, duration, fingerCount) {
    if (direction == "in") $().setFontSize($(".quesBox"), 2);   //放大
    if (direction == "out") $().setFontSize($(".quesBox"), -2);    //捏合

}
//手式滑动事件
function swipeFuc(event, direction, distance, duration, fingerCount) {
	if(fingerCount>1)return;
    var fixLeft = Number($("#quesArea").css("left").replace("px", ""));
    fixLeft = isNaN(fixLeft) ? 0 : fixLeft;
    var tm = $(".quesItem").width();

    if (direction == "left") {
        if (Math.abs(fixLeft) < tm * ($(".quesItem").size() - 1)) {
            fixLeft = fixLeft - tm;
        }
    }
    if (direction == "right") {
        if (fixLeft < 0) fixLeft = fixLeft + tm;
    }
    quesAreaMove(fixLeft);
}
//滑动试题
function quesAreaMove(fixLeft) {
    $("#quesArea").animate({ left: fixLeft }, function () {
        var index = Math.round(Math.abs(fixLeft) / $(".quesItem").width());
        $("#indexNum").attr("index", index + 1);
        $("#indexNum").text(Number($("#indexNum").attr("initIndex")) + index);
        var qitem = $(".quesItem[index=" + (index + 1) + "]");
        //试题类型的显示切换
        $("#quesType span[type=" + qitem.attr("type") + "]").show();
        $("#quesType span[type!=" + qitem.attr("type") + "]").hide();
        //试题是否收藏的显示切换
        var isCollect = qitem.attr("IsCollect") == "True" ? true : false;
        if (isCollect) {
            $("#btnFav").addClass("IsCollect");
        } else {
            $("#btnFav").removeClass("IsCollect");
        }
        $("#quesArea").css("left", -index * $(".quesItem").width());
        //当前试题的答题卡选块
        setCardState("curr")
    });
}

//生成答题卡
function buildCard() {
    //试题类型
    var types = getQuesType();
    var box = $("#cardBoxInner");
    for (var i = 0; i < types.length; i++) {
        box.append("<div class='typeName'>" + types[i].name + "</div>");
        var ques = $(".quesItem[type=" + types[i].type + "]");
        var html = "<dl type='" + types[i].type + "' count='" + ques.size() + "' number='" + 0 + "'>"
        ques.each(function () {
            var num = $(this).find("span.index").html();
            var index = $(this).attr("index");
            html += "<dd qid='" + $(this).attr("qid") + "' index='" + index + "' num='" + num + "'>" + num + "</dd>";
        });
        html += "</dl>";
        box.append(html);
    }
    box.find("dl dd").click(function () {
        var index = $(this).attr("index");
        var fixLeft = (index - 1) * $(".quesItem").width();
        $("#quesArea").css({ left: -fixLeft });
        //
        var index = Math.round(Math.abs(fixLeft) / $(".quesItem").width());
        $("#indexNum").attr("index", index + 1);
        $("#indexNum").text(Number($("#indexNum").attr("initIndex")) + index);
        var qitem = $(".quesItem[index=" + (index + 1) + "]");
        //试题类型的显示切换
        $("#quesType span[type=" + qitem.attr("type") + "]").show();
        $("#quesType span[type!=" + qitem.attr("type") + "]").hide();
        //试题是否收藏的显示切换
        var isCollect = qitem.attr("IsCollect") == "True" ? true : false;
        if (isCollect) {
            $("#btnFav").addClass("IsCollect");
        } else {
            $("#btnFav").removeClass("IsCollect");
        }
        $("#cardPanel").hide();
        setCardState("curr");
    })
}
//设置答题卡状态
//state:curr当前试题，succ正确，error错误,null默认状态
//qid:试题的id
function setCardState(state, qid) {
	var index=0;
    //如果没有指明试题id，则取当前显示的试题
    if (qid == null) {
        index = $("#indexNum").attr("index");
        var qitem = $(".quesItem[index=" + index + "]");
        qid = qitem.attr("qid");
    }
    //获取当前试题的答题卡选块
    var box = $("#cardBoxInner dd[qid=" + qid + "]");
    if (box == null || box.size() < 1) return;	
    //设置当前试题的答题卡选块状态
    if (state == "curr") {
        $("#cardBoxInner dd").removeClass("curr");
        box.addClass("curr");
    }
    if (state == "succ") {
        box.addClass("succ");
    }
    if (state == "error") {
        box.addClass("error");
    }
	//记录学习进度
	$.get("/ajax/LogForStudentQuestions.ashx",
			{acid:acid,
			couid:$().getPara("couid"),
			olid:$().getPara("olid"),
			qid:qid,
			index:index
			},function(){
	});
}
//获取试题类型
function getQuesType() {
    var types = new Array();
    var qus = $(".quesItem");
    qus.each(function () {
        var type = $(this).attr("type");
        var name = $.trim($("#quesType").find("span[type=" + type + "]").html());
        var isExist = false;
        for (var i = 0; i < types.length; i++) {
            if (types[i].type == type) {
                isExist = true;
                break;
            }
        }
        if (!isExist) {
            types.push({ type: type, name: name });
        }
    });
    return types;
}
