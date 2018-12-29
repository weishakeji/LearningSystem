//全局变量
//是否已经交卷
window.isSubmited = false;
//是否正在卷
window.isSubmit = false;
window.onload=function(){
	_initTraning();
}
$(function () {
    
    setInterval("setExamCurrDateTime()", 1000);
	//字体控件
    $().setFontSize($(".quesTitle, .quesAnswerBox"));
    //左右滑动切换试题
    $(".context").swipe({ fingers: 'all', swipeLeft: swipeFuc, swipeRight: swipeFuc,
			pinchIn: swipePinch, pinchOut: swipePinch });
    buildCard();
	
});
//放大与捏合事件
function swipePinch(event, direction, distance, duration, fingerCount) {
    if (direction == "in") $().setFontSize($(".quesTitle, .quesAnswerBox"), 2);   //放大
    if (direction == "out") $().setFontSize($(".quesTitle, .quesAnswerBox"), -2);    //捏合

}
//滑动事件
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
	fixLeft = Math.round(fixLeft / tm) * tm;
    quesAreaMove(fixLeft);
}
//滑动试题
function quesAreaMove(fixLeft) {
    $("#quesArea").animate({ left: fixLeft }, function () {
        var index = Math.round(Math.abs(fixLeft) / $(".quesItem").width());
        $("#quesArea").css("left", -index * $(".quesItem").width());
        setCardState();
    });
}
//设置答题卡状态
function setCardState(){
	var fixLeft = Number($("#quesArea").css("left").replace("px", ""));
	//取索引
	var index = Math.round(Math.abs(fixLeft) / $(".quesItem").width());
	var qitem = $(".quesItem[index=" + (index + 1) + "]");
	//设置答题卡状态
	$("#cardBox dd[qid]").removeClass("curr");
	$("#cardBox dd[qid="+qitem.attr("qid")+"]").addClass("curr");
}
//初始化界面
function _initTraning() {
    //设置试题宽度
    var wd = $(window).width();
    var hg = $(".context").height();
	hg=document.querySelector(".context").clientHeight;
    var qitem = $(".quesItem");
    qitem.width(wd).height(hg);
    $("#quesArea").width(qitem.width() * qitem.size());
}

//设置当前时间与剩余时间
function setExamCurrDateTime() {
    //计算客户端实时时间
    var tm = new Date();
    var span = tm.getTime() - ClientTime.getTime();
    var curr = new Date(ServerTime.getTime() + span);
    $("#currTime").text(curr.Format("hh:mm:ss"));
    //计算剩余的时间，还剩多久
    var resTime = $("#residueTime");
    //考试结束时间
    var overSpan = Number(resTime.attr("span"));
    span = overSpan * 60 - parseInt(span / 1000);

    //剩余时间的分抄计算
    var mm = parseInt(span / 60);
    var ss = span % 60;
    if (span >= 0) {
        resTime.find(".mm").text(mm);
        resTime.find(".ss").text(ss);        
    }else {
        //考试时间结束，在这里触发交卷
        submitResult(2);
    }
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
            var num = $(this).find("span.num").html();
            html += "<dd qid='" + $(this).attr("qid") + "' num='" + num + "'>" + $(this).attr("index") + "</dd>";
        });
        html += "</dl>";
        box.append(html);
    }
    box.find("dl dd").click(function () {
        var index = $(this).html();
        var fixLeft = (index - 1) * $(".quesItem").width();
        $("#quesArea").css({ left: -fixLeft });
        $("#cardPanel").hide();
		setCardState();
    })
}
//获取试题类型
function getQuesType() {
    var types = new Array();
    var qus = $(".quesItem");
    qus.each(function () {
        var type = $(this).attr("type");
        var name = $.trim($(this).find("span.type").html());
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