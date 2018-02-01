//全局变量
//是否已经交卷
window.isSubmited = false;
//是否正在卷
window.isSubmit = false;

$(function () {
    _initTraning();
    setInterval("setExamCurrDateTime()", 1000);
    var fixLeft = 0;
    //试题的手势滑动切换
    $(".context").swipe({
        swipeStatus: function (event, phase, direction, distance, duration, fingerCount) {
            var speed = Math.round((600 / duration < 1 ? 1 : 600 / duration) * distance);
            speed = speed > $(".quesItem").width() ? $(".quesItem").width() : speed;
            if (phase == "start") {
                fixLeft = Number($("#quesArea").css("left").replace("px", ""));
                fixLeft = isNaN(fixLeft) ? 0 : fixLeft;
            }
            if (phase == "move") {
                var area = $("#quesArea");
                if (direction == "left") $("#quesArea").css({ left: fixLeft - speed });
                if (direction == "right") $("#quesArea").css({ left: fixLeft + speed });
            }
            if ((phase == "end" || phase == "cancel")) {
                fixLeft = Number($("#quesArea").css("left").replace("px", ""));
                fixLeft = isNaN(fixLeft) ? 0 : fixLeft;
                if (fixLeft >= 0) fixLeft = 0;
                var maxWd = -$(window).width() * ($(".quesItem").size() - 1);
                if (fixLeft < maxWd) fixLeft = maxWd;
                if (fixLeft < 0 && fixLeft > maxWd) {
                    var index = Math.round(Math.abs(fixLeft) / $(".quesItem").width());
                    fixLeft = -index * $(".quesItem").width();
                }
                $("#quesArea").animate({ left: fixLeft }, function () {

                });
            }
        }
    });
    buildCard();
});

//初始化界面
function _initTraning() {
    //设置试题宽度
    var wd = $(window).width();
    var hg = $(".context").height();
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
    if (mm > 0 && ss > 0) {
        resTime.find(".mm").text(mm);
        resTime.find(".ss").text(ss);
        //getExamQues();
    }
    if (mm <= 0 && ss <= 0) {
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