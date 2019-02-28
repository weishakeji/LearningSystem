//窗体滚动时，需要固定的内容
$(document).ready(function () {
    var cardbox = $(".pagerCard");
    if (cardbox.size() > 0) {
        var off = cardbox.offset();
        window.cardbox_top = off.top; //
    }
    $(window).scroll();
});
$(window).scroll(function () {
    var cardbox = $(".pagerCard");
    if (cardbox.size() > 0) {
        var offsetTop = $(window).scrollTop(); //滚上去的高度值
        if (offsetTop > window.cardbox_top) {
            cardbox.css("margin-top", (offsetTop - window.cardbox_top + 15) + "px");
        } else {
            cardbox.css("margin-top", 10 + "px");
        }
    }
});
/*
考试现场的处理对象
*/
function Examing() {
    //相关属性
    this.attr = {
        theme: null,     //考试主题
        uid: null,        //全局唯一值
        testpager: 0,     //试卷id
        stid: 0,          //学员id
        isBlurSumit: false, //窗口失去焦点，是否自动提交
        isDisabledFresh: false, //是否禁止刷新页面，为true时禁止
        isDisabledCopy: true, 	//是否禁止复制，为true时禁止
        isShowConfirm: false          //是否显示答案的确认按钮
    };
    //时间，包括服务器端时间，客户端时间，当前时间
    this.time = {
        server: new Date(), //服务器时间
        client: new Date(), //客户端时间
        now: new Date(),    //当前时间，通过服务器时间校正过的
        span: 0,            //考试时长（单位分钟）
        wait: 0,           //离开始考试还有多久，单位秒
        begin: new Date(),  //考试开始时间
        over: new Date(),    //考试结束时间
        surplus: 0,        //考试剩余时间
        request: -1,        //加载试题的时间
        requestlimit: 10,    //离开考多久的时候，开始预加载试题，单位：分钟
        getmin: function (sec) { return parseInt(sec / 60); }, //通过总秒数，获取分钟
        getsec: function (sec) { return parseInt(sec % 60); }   //通过总秒数，获取秒
    };
    //考试中的状态
    this.state = {
        init: false,    //有没有初始化
        begin: false,   //考试开始了没有
        over: false,     //考试结束了没有
        loading: false, //是否正在加载试卷
        loaded: false,  //试卷是否加载完成
        layouting: false,    //是否正在布局
        layouted: false,    //是否布局完成
        uploading: false,   //是否正往服务器上提交数据
        submiting: false,    //是否正在交卷
        submit: false    //是否交卷
    };
    //涉及到的数据源
    this.data = {
        ques: null,  //从服务器端获取的试题数据
        answer: null     //答题信息
    };
    //涉及到的事件
    this.event = {
        timer: Examing.calculationTimer,    //时间循环
        begin: Examing.beginEventFunc,      //考试开始事件
        layout: Examing.setExamQuesLayout,   //将试题展现出来的事件
        over: null,      //考试结束事件
        submit: null     //交卷事件
    };
    //涉及的方法
    this.func = {
        getType: function (type) {		//获取试题类型的中文名称
            var arr = quesTypes.split(",");
            return arr[type - 1];
        }
    };
    Examing.Obj = this;
    //禁止刷新页面
    if (this.attr.isDisabledFresh) {
        document.body.onselectstart = document.body.oncontextmenu = function () { return false; };
        document.body.onpaste = document.body.oncopy = function () { return false; };
        document.body.onkeydown = function () {
            if (event.keyCode == 116) {
                event.keyCode = 0;
                event.cancelBubble = true;
                return false;
            }
        }
    }
    //禁止复制
    if (this.attr.isDisabledCopy) {
        document.oncontextmenu = new Function('event.returnValue=false;');
        document.onselectstart = new Function('event.returnValue=false;');
    }
}
//当前考试对象，用于记录全局的值
Examing.Obj = null;
Examing.prototype.Init=function(){	
	$(".startTime").text(this.time.begin.Format("yyyy-MM-dd hh:mm:ss"));
	$(".overTime").text(this.time.over.Format("yyyy-MM-dd hh:mm:ss"));
};
//计算时间，
Examing.prototype.TimeCalc = function () {
    //计算当前时间，经过服务器端时间的校正
    var curr = new Date(this.time.server.getTime() + (new Date().getTime() - this.time.client.getTime()));
    this.time.now = curr;
    //如果没有初始化
    if (!this.state.init) {
        //加载试题的最早时间，开考前10分钟
        var first = new Date(this.time.begin.getTime() - this.time.requestlimit * 60 * 1000);
        //最晚加载时间，开考前三分钟
        var last = new Date(this.time.begin.getTime() - 3 * 60 * 1000);
        //生成随机的试题加载时间（最早时间与早晚时间之间），防止并发请求造成服务器负荷过重
        this.time.request = this.time.now >= last ? last : new Date(first.getTime() + Math.random() * (last.getTime() - first.getTime()));
        //设置初始化过了
        this.state.init = true;
    }
    //随着时间进展，触发一系统列事件
    this.event.timer(this);
}
//
//时间循环执行，计算各项值
Examing.calculationTimer = function (examobj) {
    if (examobj.state.submit) return;
    Examing.calculationWaitTime(examobj);
    Examing.calculationOverTime(examobj);
    //***各种状态
    examobj.state.begin = examobj.time.wait < 0;      //考试是否开始，等待时间小于0，即为开始
    examobj.state.over = examobj.time.surplus < 0;      //考试是否结束，剩余时间小于0，即为考试结束
    //***事件执行
    if (examobj.state.begin) examobj.event.begin(examobj);  //考试开始事件
    //开始加载试题
    if (examobj.time.now > examobj.time.request && examobj.time.now < examobj.time.over) {
        Examing.getExamQues(examobj);
    }
}
//计算考试开始的等待时间（离开始剩余时间）
Examing.calculationWaitTime = function (examobj) {
    //当前时间，来自服务端
    $("#currTime").text(examobj.time.now.Format("yyyy-MM-dd hh:mm:ss"));
    //计算离考试开始时间，还有多久,单位：秒
    examobj.time.wait = Math.floor((examobj.time.begin.getTime() - examobj.time.now.getTime()) / 1000);
    //如果还没有开始
    if (examobj.time.wait >= 0) {
        var notTime = $("#distanceTime");
        notTime.show();
        notTime.find(".mm").text(examobj.time.getmin(examobj.time.wait));
        notTime.find(".ss").text(examobj.time.getsec(examobj.time.wait));
        //显示考试时限的剩余时间
        var resTime = $("#residueTime");
        resTime.find(".mm").text(examobj.time.span);
        resTime.find(".ss").text("00");
        //交卷按钮禁用
        $("#pagerSubmitBox").attr("isStart", "no").addClass("noStart");
    }
}
//计算考试的剩余的时间（离结束的剩余时间）
Examing.calculationOverTime = function (examobj) {
    //计算剩余的时间，还剩多久(单位为秒）
    examobj.time.surplus = Math.floor((examobj.time.over.getTime() - examobj.time.now.getTime()) / 1000);
    if (examobj.time.wait < 0) {
        //剩余时间大于0，说明还没有考完 
        if (examobj.time.surplus > 0) {
            var resTime = $("#residueTime");
            resTime.show();
            resTime.find(".mm").text(examobj.time.getmin(examobj.time.surplus));
            resTime.find(".ss").text(examobj.time.getsec(examobj.time.surplus));
        } else {
            //考试结束
            if (examobj.time.now > examobj.time.begin && examobj.time.now <= examobj.time.over) {
                if (window.submit != true) {
                    Mask.Submit();
                    window.submit = true;
                    submitResult(2);
                }
            }
        }
    }
}
//开始考试的事件方法
Examing.beginEventFunc = function (examobj) {
    //交卷按钮启用，显示等待时间的信息隐藏
    $("#pagerSubmitBox").attr("isStart", "yes").removeClass("noStart");
    $("#distanceTime").slideUp(100, function () {
        //设置试题
        if (!examobj.state.layouted && !examobj.state.layouting) {
            examobj.event.layout(examobj); //进行试题展现          
        }
    });

}
//获取试题
Examing.getExamQues = function (examobj) {
    if (examobj.state.loading == true) return;
    if (examobj.state.loaded == true) return;
    if (!(examobj.time.now > examobj.time.request && examobj.time.now < examobj.time.over)) return;
    if ($("#quesArea dd").size() > 0) return;
    var urlPath = "/ajax/ExamPager.ashx?timestamp=" + new Date().getTime();
    $.ajax({
        type: "POST", url: urlPath, dataType: "text",
        data: { examid: examID, tpid: testPagerID, stid: stid },
        //开始，进行预载
        beforeSend: function (XMLHttpRequest, textStatus) {
            examobj.state.loading = true;
            if (examobj.state.begin) Mask.Loading();
        },
        //加载出错
        error: function (XMLHttpRequest, textStatus, errorThrown) {
            //examobj.state.loading = false;
            alert("试题加载错误");
        },
        //加载成功！
        success: function (data) {
            //examobj.state.loading = false;
            examobj.state.loaded = true;
            if (data == "2") {
                examobj.state.submit = true;
                alert("您已经交过卷了！");
            } else {
                var ques = eval("(" + data + ")");
                examobj.data.ques = ques;
                $(".currTime").css("color", "#f00");
                //如果开始考试了，才进行布局
                if (examobj.state.begin) {
                    $("#distanceTime").hide(); //显示开始倒计时的一些信息隐藏掉
                    if (!examobj.state.layouted && !examobj.state.layouting) {
                        examobj.event.layout(examobj); //进行试题展现                       
                    }
                }
            }
        }
    });
}

//设置试题的布局
Examing.setExamQuesLayout = function (examobj) {
    var ques = examobj.data.ques;
    if (ques == null) {
        Examing.getExamQues(examobj);
        return;
    }
    examobj.state.layouting = true; //正在布局    
    //输出试题    
    var quesBox = $("#quesArea");
    //设置结束时间
    quesBox.attr("overTime", examobj.time.over.getTime());
    quesBox.attr("beginTime", examobj.time.begin.getTime());
    var hanzi = ["一", "二", "三", "四", "五", "六", "七", "八", "九"];
    for (var i = 0; i < ques.length; i++) {
        for (var j = 0; j < ques[i].ques.length; j++) {
            var q = ques[i].ques[j];
            if (q != null && q != undefined && q != '') {
                q.Qus_Title = q.Qus_Title.replace(/&quot;/ig, "\"");
            }
        }
        var func = eval("setQuestionLayout" + ques[i].type);
        if ("undefined" != typeof (func)) quesBox.append(func(ques[i], i, hanzi[i]));
    }
    fileupEvent();
    Examing.setExamQuesLayout_after(examobj);   //布局完成之后
    setCardQues(); //设置答题卡    
    quesEvent(); //答题时的事件    
    getResult(); //获取已经做好的试题
    //交卷事件
    $("#pagerSubmitBox").click(function () {
        var n = $("#cardBox dd[class]").size();
        var sum = $("#cardBox dd").size();
        if (n < sum) {
            if (confirm("您还有" + (sum - n) + "道题没有做，确定交卷吗？"))
                submitResult(2);
        } else {
            submitResult(2);
        }
    });
    /*当窗体失去焦点提示交卷 */
    if (Examing.Obj.attr.isBlurSumit) {
        $(window).blur(function () {
            var n = $("#cardBox dd[class]").size();
            var sum = $("#cardBox dd").size();
            var show = "离开考试界面将自动交卷；\n";
            if (n < sum) {
                show += "\n您还有 " + (sum - n) + "道题没有做；\n";
            }
            show += "确定交卷吗？";
            if (confirm(show)) submitResult(2);
            window.focus();
        });
    }
    $(window).focus(function () {

    });
    examobj.state.layouted = true; //完成布局
    //如果要显示确认按钮
    if (Examing.Obj.attr.isShowConfirm) {
        $("#quesArea dd").css("padding-bottom", "60px");
    }
}
//布局完成之后
Examing.setExamQuesLayout_after = function (examobj) {
    var quesBox = $("#quesArea");
    //给试题加上序号
    quesBox.find(".order").each(function (index) {
        $(this).html(index + 1);
    });
    quesBox.find(".sum").text(quesBox.find("dd").size());   //设置试题总数
    //设置高度
    quesBox.height(window.innerHeight - 40);
    quesBox.find("dl,dd").height(quesBox.height());
    //设置宽度
    quesBox.find("dd").width(window.innerWidth).css("top", "40px");
    quesBox.find("dl").each(function (index) {
        var wd = $(this).find("dd").size() * window.innerWidth;
        $(this).width(wd);
    });
    quesBox.width(quesBox.find("dd").size() * window.innerWidth);
    $("#pagerHeader").hide().show().width(window.innerWidth);
    $("#pagerFooter").hide().show().width(window.innerWidth);
	//字体控件
    $().setFontSize($(".quesTitle, .quesAnswerBox"));
    //左右滑动切换试题
    $("body").swipe({ fingers: 'all', swipeLeft: swipeFuc, swipeRight: swipeFuc,
				pinchIn: swipePinch, pinchOut: swipePinch });
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
        var tm = $("#quesArea dd:first").width();
        if (direction == "left") {
            if (Math.abs(fixLeft) < tm * ($("#quesArea dd").size() - 1)) {
                fixLeft = fixLeft - tm;
            }
        }
        if (direction == "right") {
            if (fixLeft < 0) fixLeft = fixLeft + tm;
        }
        fixLeft = Math.round(fixLeft / tm) * tm;
        quesAreaMove(fixLeft);
    }
}
//滑动试题
function quesAreaMove(fixLeft) {
    $("#quesArea").animate({ left: fixLeft }, function () {
        var index = Math.round(Math.abs(fixLeft) / $("#quesArea dd").width());
        $("#quesArea").css("left", -index * $("#quesArea dd").width());
    });
}
//设置答题卡
function setCardQues() {
    var tit = $("#cardBox");
    //设置结束时间
    tit.attr("overTime", $("#quesArea").attr("overTime"));
    tit.attr("beginTime", $("#quesArea").attr("beginTime"));
    $("#quesArea dl").each(function () {
        var name = $(this).find("dt b").text();
        var num = $(this).find("dt span").text();
        tit.append("<div class=\"quesType\" type='" + $(this).attr("type") + "'>"
        + "<div class='tit'>" + name + "</div><div class='num'>" + num + "</div></div>");
        var tm = "<dl type='" + $(this).attr("type") + "' count='" + $(this).attr("count") + "' number='" + $(this).attr("number") + "'>";
        $(this).find("dd").each(function () {
            var order = $(this).find(".order").text();
            tm += "<dd qid='" + $(this).attr("qid") + "' num='" + $(this).attr("number") + "'>" + order + "</dd>";
        });
        tit.append(tm + "</dl>");
    });
    //当点击试题分类时，定位试卷到指定位置
    tit.find("div.quesType").click(function () {
        var type = $(this).attr("type");
        var dl = $("#quesArea dl[type=" + type + "]");
        var off = dl.offset();
        $(window).scrollTop(off.top);
    });
    //当点击试题时，定位试卷到指定位置
    tit.find("dd").click(function () {
        var qid = $(this).attr("qid");
        var dd = $("#quesArea dd[qid=" + qid + "]");
        var index = Number($.trim(dd.find(".order").text())) - 1;
        //var off = dd.offset();
        //$(window).scrollTop(off.top);
        $("#quesArea").css({ left: -index * dd.width() });
        $(".quesCard").hide();
    });
    //$(".quesCard").show();
    //显示多少道题
    $(".tpcount").text($("#quesArea dd").size());
    $("#btnCard").click(function () {
        $(".quesCard").css({ "position": "absolute",
            top: 40, left: 0
        }).height(window.innerHeight - 80).fadeToggle();
    });
}

/*
试题展示
*/
//设置对试题的把握程序
function setQuesBtn(qusid) {
    var style = "";
    if (!Examing.Obj.attr.isShowConfirm) style = "display:none;";
    //按钮
    var html = "<div class='btnBox' qid=\"" + qusid + "\" style=\"" + style + "\">";
    html += "<div class='tit'>您是否对该题有把握：</div>";
    html += "<div class='btn sure1' level='1'>非常有把握</div>";
    html += "<div class='btn sure2' level='2'>没有把握</div>";
    html += "<div class='btn sure3' level='3'>完全靠瞎蒙</div>";
    html += "</div>";
    return html;
}
function quesInfo(qitem) {
    var num = qitem.count > 0 ? Math.floor(qitem.number / qitem.count * 10) / 10 : 0;
    var arr = quesTypes.split(",");
    var type = arr[qitem.type - 1];
    var html = "";
    /*<div class="quesInfo">
    <div class="info">[4/10] [<span class="type">多选题</span>]（<span class="num">10</span>分）</div>
    <div class="infoLeft"> 
    <!--收藏与报错 --> 
    <span class="examico btnFav IsCollect"></span> <span class="examico btnError"></span> </div>
    </div>*/
    html += "<div class='quesInfo'>";
    html += " <div class='info'>[<span class='order'></span>/<span class='sum'></span>] [<span class='type'>" + type + "题</span>]（<span class='num'>" + num + "</span>分）</div>";
    html += "</div>";
    return html;
}
//单选题
function setQuestionLayout1(qitem, typeIndex, indexHanzi) {
    //每题的分数
    var num = qitem.count > 0 ? Math.floor(qitem.number / qitem.count * 10) / 10 : 0;
    var html = "<dl type='" + qitem.type + "' count='" + qitem.count + "' number='" + qitem.number + "'><dt>"
    + indexHanzi + "、<b>单选题</b>（本大题共" + qitem.count + "道小题，<span>每题"
    + (qitem.count > 0 ? Math.floor(qitem.number / qitem.count * 10) / 10 : 0) + "分/共" + qitem.number + "分</span>）</dt>";
    for (var i = 0; i < qitem.ques.length; i++) {
        var q = qitem.ques[i];
        html += "<dd qid='" + q.Qus_ID + "' type='" + qitem.type + "' number='" + q.Qus_Number + "'>";
        html += "<div class='quesBox'>";
        html += quesInfo(qitem);
        html += "<div class='quesTitle'>" + unescape(q.Qus_Title) + "</div>";
        //选择项
        html += "<div class='itemBox type1'>";
        var answer = q.Answer;
        for (var j = 0; j < answer.length; j++) {
            html += "<div class=\"ansItem\" ansid=\"" + answer[j].Ans_ID + "\">";
            html += "<div class='char' >" + String.fromCharCode(65 + j) + "、</div>"
            html += "<div class=\"ansItemContext\">" + unescape(answer[j].Ans_Context) + "</div></div>";
        }
        html += "</div>";
        //选择按钮
        html += "<div class='itemBtnBox type1'>";
        for (var j = 0; j < answer.length; j++) {
            html += "<div class=\"ansBtn\" ansid=\"" + answer[j].Ans_ID + "\">" + String.fromCharCode(65 + j) + "</div>";
        }
        html += "</div>";
        html += "</div>";
        html += setQuesBtn(q.Qus_ID); //增加确认把握的选项
        html += "</dd>";
    }
    html + "</dl>";
    return html;
}
function setQuestionLayout2(qitem, typeIndex, indexHanzi) {
    var html = "<dl type='" + qitem.type + "' count='" + qitem.count + "' number='" + qitem.number + "'><dt>"
    + indexHanzi + "、<b>多选题</b>（本大题共"
    + qitem.count + "道小题，<span>每题" + (qitem.count > 0 ? Math.floor(qitem.number / qitem.count * 10) / 10 : 0) + "分/共" + qitem.number + "分</span>）</dt>";
    for (var i = 0; i < qitem.ques.length; i++) {
        var q = qitem.ques[i];
        html += "<dd qid='" + q.Qus_ID + "' type='" + qitem.type + "' number='" + q.Qus_Number + "'>";
        html += "<div class='quesBox'>";
        html += quesInfo(qitem);
        html += "<div class='quesTitle'>" + unescape(q.Qus_Title) + "</div>";
        //选项
        html += "<div class='itemBox type2'>";
        var answer = q.Answer;
        for (var j = 0; j < answer.length; j++) {
            html += "<div class=\"ansItem\" ansid=\"" + answer[j].Ans_ID + "\">";
            html += "<div class='char'>" + String.fromCharCode(65 + j) + "、</div>"
            html += "<div class=\"ansItemContext\">" + unescape(answer[j].Ans_Context) + "</div></div>";
        }
        html += "</div>";
        //选择按钮
        html += "<div class='itemBtnBox type2''>";
        for (var j = 0; j < answer.length; j++) {
            html += "<div class=\"ansBtn\" ansid=\"" + answer[j].Ans_ID + "\">" + String.fromCharCode(65 + j) + "</div>";
        }
        html += "</div>";
        html += "</div>";
        html += setQuesBtn(q.Qus_ID);
        html += "</dd>";
    }
    return html;
}
function setQuestionLayout3(qitem, typeIndex, indexHanzi) {
    var html = "<dl type='" + qitem.type + "' count='" + qitem.count + "' number='" + qitem.number + "'><dt>"
    + indexHanzi + "、<b>判断题</b>（本大题共"
    + qitem.count + "道小题，<span>每题" + (qitem.count > 0 ? Math.floor(qitem.number / qitem.count * 10) / 10 : 0) + "分/共" + qitem.number + "分</span>）</dt>";
    for (var i = 0; i < qitem.ques.length; i++) {
        var q = qitem.ques[i];
        html += "<dd qid='" + q.Qus_ID + "' type='" + qitem.type + "' number='" + q.Qus_Number + "'>";
        html += "<div class='quesBox'>";
        html += quesInfo(qitem);
        html += "<div class='quesTitle'>" + unescape(q.Qus_Title) + "</div>";
        //按钮
        html += "<div class='itemBtnBox type3'>";
        html += "<div class='ansBtn' ansid=\"0\">正确</div>";
        html += "<div class='ansBtn' ansid=\"1\">错误</div>";
        html += "</div>";
        html += "</div>";
        html += setQuesBtn(q.Qus_ID);
        html += "</dd>";
    }
    return html;
}
function setQuestionLayout4(qitem, typeIndex, indexHanzi) {
    var html = "<dl type='" + qitem.type + "' count='" + qitem.count + "' number='" + qitem.number + "'><dt>"
    + indexHanzi + "、<b>简答题</b>（本大题共"
    + qitem.count + "道小题，<span>每题" + (qitem.count > 0 ? Math.floor(qitem.number / qitem.count * 10) / 10 : 0) + "分/共" + qitem.number + "分</span>）</dt>";
    for (var i = 0; i < qitem.ques.length; i++) {
        var q = qitem.ques[i];
        html += "<dd qid='" + q.Qus_ID + "' type='" + qitem.type + "' number='" + q.Qus_Number + "'>";
        html += "<div class='quesBox'>";
        html += quesInfo(qitem);
        html += "<div class='quesTitle'>" + unescape(q.Qus_Title) + "</div>";
        //选项
        html += "<div class='itemBox type4'>";
        html += "<textarea name=''></textarea>";
        html += "<div class='fileupBox' qid='" + q.Qus_ID + "'><span class='upfileArea'> 上传附件：<input type='file' class='fileinput' id='fileup_" + q.Qus_ID + "' qid='" + q.Qus_ID + "' name='file' />";
        html += "<input type='button' name='button' class='btnUp' value='上传' /></span>";
        html += "<span class='filenameArea' style='display: none'> 附件：<span class='accfile'></span> <span class='delfile' title='重新上传'>重新上传</span> </span></div>";
        html += "</div>";
        html += "</div>";
        html += setQuesBtn(q.Qus_ID);
        html += "</dd>";
    }
    return html;
}
function setQuestionLayout5(qitem, typeIndex, indexHanzi) {
    var html = "<dl type='" + qitem.type + "' count='" + qitem.count + "' number='" + qitem.number + "'><dt>"
    + indexHanzi + "、<b>填空题</b>（本大题共"
    + qitem.count + "道小题，<span>每题" + (qitem.count > 0 ? Math.floor(qitem.number / qitem.count * 10) / 10 : 0) + "分/共" + qitem.number + "分</span>）</dt>";
    for (var i = 0; i < qitem.ques.length; i++) {
        var q = qitem.ques[i];
        html += "<dd qid='" + q.Qus_ID + "' type='" + qitem.type + "' number='" + q.Qus_Number + "'>";
        html += "<div class='quesBox'>";
        html += quesInfo(qitem);
        html += "<div class='quesTitle'>" + unescape(q.Qus_Title) + "</div>";
        //选项
        html += "<div class='itemBox type5'>";
        var answer = q.Answer;
        for (var j = 0; j < answer.length; j++) {
            html += "<div class=\"ansItem\" ansid=\"" + answer[j].Ans_ID + "\">";
            html += "<div class='char'>（" + (j + 1) + "）</div>"
            html += "<div class=\"ansItemContext\">";
            //html += "（" + (j + 1) + "）&nbsp;"
            html += "<input name='' type='text' class='textbox' />";
            html += "</div></div>";
        }
        html += "</div>";
        html += "</div>";
        html += setQuesBtn(q.Qus_ID);
        html += "</dd>";
    }
    return html;
}
/* 简答题的附件上传 */
function fileupEvent() {
    //附件上传
    $("input.btnUp").click(function () {
        var file = $(this).parent().find(".fileinput");
        if ($.trim(file.val()) == "") {
            alert("请选择要上传的文件！");
            file.focus();
            return;
        }
        var qid = file.attr("qid");
        var url = '/Utility/ExamFileUp.ashx?stid=' + stid + "&examid=" + examID + "&qid=" + qid;
        $.ajaxFileUpload({
            url: url, //用于文件上传的服务器端请求地址
            secureuri: false, //一般设置为false
            fileElementId: file.attr("id"), //文件上传空间的id属性  <input type="file" id="file" name="file" />
            dataType: 'json', //返回值类型 一般设置为json
            success: function (data, status) {
                $("#img1").attr("src", data.imgurl);
                if (typeof (data.error) != 'undefined') {
                    if (data.error != '') {
                        alert(data.error);
                    } else {
                        var box = $(".fileupBox[qid=" + data.qid + "]");
                        box.find(".upfileArea").hide();
                        box.find(".filenameArea").show();
                        box.find(".accfile").html(data.filename);
                        box.find(".delfile").click(function () {
                            var bx = $(this).parents(".fileupBox");
                            bx.find(".upfileArea").show();
                            bx.find(".filenameArea").hide();
                            var qid = $(this).parent().parent().attr("qid");
                            $.get('/Utility/ExamFileDel.ashx?stid=' + stid + "&examid=" + examID + "&qid=" + qid);
                        });
                    }
                }
            },
            error: function (data, status, e) {
                alert(e);
            }
        });
    });
    //附件加载
    $(".fileupBox").each(function () {
        var qid = $(this).attr("qid");
        var url = '/Utility/ExamFileLoad.ashx?stid=' + stid + "&examid=" + examID + "&qid=" + qid;
        $.get(url, function (data) {
            var d = eval("(" + data + ")");
            var box = $(".fileupBox[qid=" + d.qid + "]");
            if (d.filename != "") {
                box.find(".upfileArea").hide();
                box.find(".filenameArea").show();
                box.find(".accfile").html(d.filename);
            }
            box.find(".delfile").click(function () {
                var bx = $(this).parents(".fileupBox");
                bx.find(".upfileArea").show();
                bx.find(".filenameArea").hide();
                var qid = $(this).parent().parent().attr("qid");
                $.get('/Utility/ExamFileDel.ashx?stid=' + stid + "&examid=" + examID + "&qid=" + qid);
            });
        });
    });
}
/*答题时的事件处理*/
function quesEvent() {
    //单选题的事件
    $("#quesArea dd[type=1]").find(".ansItem, .ansBtn").click(function () {
        $(this).parents("dd[qid]").find(".ansItem, .ansBtn").removeClass("type1Over").removeClass("seleted");
        //答案id
        var ansid = $(this).attr("ansid");
        $(this).parents("dd[qid]").find(".ansItem[ansid=" + ansid + "]").addClass("type1Over");
        $(this).parents("dd[qid]").find(".ansBtn[ansid=" + ansid + "]").addClass("seleted");
        //试题id
        var dd = $(this).parents("dd[qid]");
        var qid = dd.attr("qid");
        //操作答题卡 
        var box = $("#cardBox dd[qid=" + qid + "]");
        box.attr("ans", $(this).attr("ansid"));
        dd.find(".btnBox .sure1").click();
        //计算完成的题数
        $(".CompleteNumber").text($("#cardBox dd[class]").size());
        submitResult(1);
    });
    //多选题的事件
    $("#quesArea dd[type=2]").find(".ansItem, .ansBtn").click(function () {
        var ansid = $(this).attr("ansid");  //答案id
        var dd = $(this).parents("dd[qid]");    //试题的html元素对象
        dd.find(".ansItem[ansid=" + ansid + "]").toggleClass("type1Over");
        dd.find(".ansBtn[ansid=" + ansid + "]").toggleClass("seleted");
        //确定的答题
        var ansid = "";
        dd.find(".type1Over").each(function () {
            ansid += $(this).attr("ansid") + ",";
        });
        //操作答题卡 
        var box = $("#cardBox dd[qid=" + dd.attr("qid") + "]");
        box.attr("ans", ansid);
        if (ansid.indexOf(",") > -1 && ansid.split(",").length > 2) {
            dd.find(".btnBox .sure1").click();
        } else {
            box.removeAttr("class");
        }
        //计算完成的题数
        $(".CompleteNumber").text($("#cardBox dd[class]").size());
        submitResult(1);
    });
    //判断
    $("#quesArea dd[type=3] .ansBtn").click(function () {
        $(this).parent().find(".seleted[ansid!=" + $(this).attr("ansid") + "]").removeClass("seleted");
        $(this).toggleClass("seleted");
        var dd = $(this).parents("dd[qid]");
        var qid = dd.attr("qid");
        //操作答题卡 
        var box = $("#cardBox dd[qid=" + qid + "]");
        if ($(this).parent().find(".seleted").size() > 0) {
            box.attr("ans", $(this).attr("ansid"));
            dd.find(".btnBox .sure1").click();
        } else {
            box.removeAttr("ans").removeAttr("class");
        }
        //计算完成的题数
        $(".CompleteNumber").text($("#cardBox dd[class]").size());
        submitResult(1);
    });
    //简答
    $("#quesArea dd[type=4] .itemBox textarea").focusout(function () {
        var dd = $(this).parents("dd[qid]");
        var qid = dd.attr("qid");
        var ansid = $.trim($(this).val());
        //操作答题卡 
        var box = $("#cardBox dd[qid=" + qid + "]");
        if (ansid.length > 0) {
            box.attr("ans", ansid);
            dd.find(".btnBox .sure1").click();
        } else {
            box.removeAttr("ans").removeAttr("class");
        }
        //计算完成的题数
        $(".CompleteNumber").text($("#cardBox dd[class]").size());
        submitResult(1);
    });
    //填空
    $("#quesArea dd[type=5] .itemBox .textbox").focusout(function () {
        var dd = $(this).parents("dd[qid]");
        var qid = dd.attr("qid");
        var ansid = "";
        dd.find(".textbox[value!='']").each(function () {
            ansid += $(this).val() + ",";
        });
        //操作答题卡 
        var box = $("#cardBox dd[qid=" + qid + "]");
        if (ansid.indexOf(",") > -1) {
            box.attr("ans", ansid);
            dd.find(".btnBox .sure1").click();
        } else {
            box.removeAttr("ans").removeAttr("class");
        }
        //计算完成的题数
        $(".CompleteNumber").text($("#cardBox dd[class]").size());
        submitResult(1);
    });
    //把握度的按钮
    $("#quesArea .btnBox .btn").click(function () {
        //试题id
        var qid = $(this).parent().attr("qid");
        var level = $(this).attr("level");
        //操作答题卡
        var dd = $("#cardBox dd[qid=" + qid + "]");
        if (dd.attr("ans") != null) {
            dd.attr("class", "level" + level);
            if (level == 1) dd.attr("title", "该题非常有把握");
            if (level == 2) dd.attr("title", "对该题没有把握，请回顾");
            if (level == 3) dd.attr("title", "该题完全靠蒙，祝你好运！");
        } else {
            alert("该题还没有做！");
        }
    });
}

/*
试卷的提交
*/
//提交答题信息
function submitResult(patter) {
    //return;
    //如果没有试题，则不提交
    if ($("#cardBox dl dd").size() < 1) return;
    //提交
    var xml = encodeURIComponent(getResultXml(patter));
    var urlPath = "/ajax/InResult.ashx?timestamp=" + new Date().getTime();
    $.ajax({
        type: "POST", url: urlPath, dataType: "text", data: { result: xml },
        //开始，进行预载
        beforeSend: function (XMLHttpRequest, textStatus) {
            if (patter == 1) Mask.InResult();
            if (patter == 2) Mask.Submit();
        },
        //加载出错
        error: function (XMLHttpRequest, textStatus, errorThrown) {
            //alert(textStatus);
        },
        //加载成功！
        success: function (data) {
            if (Number(data) == 0) alert("考试已经结束，不能提交试卷！");
            if (patter == 1) Mask.InResultClose();
            if (patter == 2) location.replace(location.href);
        }
    });
}
//获取答题信息
//patter:提交方式，1为自动提交，2为交卷
function getResultXml(patter) {
    var box = $("#cardBox");
    var res = "<?xml version=\"1.0\" encoding=\"utf-8\" ?>";
    res += "<results examid=\"" + examID + "\" tpid=\"" + testPagerID
     + "\" now=\"" + Examing.Obj.time.now.getTime() + "\"  begin=\"" + box.attr("beginTime") + "\"  overtime=\"" + box.attr("overTime")
    + "\" stid=\"" + stid + "\"  stname=\"" + ($.trim(stname) == "" ? stacc : $.trim(stname)) + "\"  stsid=\"" + stsid
    + "\" stsex=\"" + stsex + "\"  stcardid=\"" + stcardid
    + "\" uid=\"" + uid + "\"  theme=\"" + theme
    + "\" sbjid=\"" + subjectID + "\" sbjname=\"" + subjectName + "\" patter=\"" + patter + "\">";
    box.find("dl").each(function () {
        res += "<ques type='" + $(this).attr("type") + "' count='" + $(this).attr("count") + "' number='" + $(this).attr("number") + "'>";
        $(this).find("dd").each(function () {
            var type = Number($(this).parent().attr("type"));
            var ans = $(this).attr("ans") ? $(this).attr("ans") : "";
            if (type == 1 || type == 2 || type == 3) {
                res += "<q id=\"" + $(this).attr("qid") + "\" class=\"" + $(this).attr("class") + "\" num=\"" + $(this).attr("num") + "\" ans=\"" + ans + "\"/>";
            }
            if (type == 4 || type == 5) {
                res += "<q id=\"" + $(this).attr("qid") + "\" class=\"" + $(this).attr("class") + "\" num=\"" + $(this).attr("num") + "\">";
                res += "<![CDATA[" + ans + "]]>"
                res += "</q>";
            }
        });
        res += "</ques>";
    });
    res += "</results>";
    return res;
}


/*
加载已经做好的答题信息，用于考试中断电异常；
实现断电不丢答题信息
*/
function getResult() {
    var urlPath = "/ajax/GetResult.ashx?timestamp=" + new Date().getTime();
    $.ajax({
        type: "GET", url: urlPath, dataType: "text",
        data: { examid: examID, tpid: testPagerID, stid: stid },
        //开始，进行预载
        beforeSend: function (XMLHttpRequest, textStatus) {
            Mask.Loading();
        },
        //加载出错
        error: function (XMLHttpRequest, textStatus, errorThrown) {           
            alert("错误：由于网络原因，没有获取之前的答题信息");
        },
        //加载成功！
        success: function (data) {
            if (data != "0") {
                var result = eval("(" + data + ")");
                setResultState(result);
            }
            submitResult(1);
            Mask.LoadingClose();
        }
    });
}
//设置答题的状态
function setResultState(result) {
    var ques = result.ques;
    for (var tm in ques) {
        var q = ques[tm];
        var dd = $("#cardBox dd[qid=" + q.id + "]");
        if (q.cls != "undefined") dd.addClass(q.cls);
        if (q.ans != "") dd.attr("ans", q.ans);
        if (q.type == 1) {
            var dd = $("#quesArea dd[qid=" + q.id + "]");
            dd.find(".itemBox div[ansid=" + q.ans + "]").addClass("type1Over");
            dd.find(".itemBtnBox div[ansid=" + q.ans + "]").addClass("seleted");
        }
        if (q.type == 2) {
            var dd = $("#quesArea dd[qid=" + q.id + "]");
            var arr = q.ans.split(",");
            for (var tm in arr) {
                dd.find(".ansItem[ansid=" + arr[tm] + "]").addClass("type1Over");
                dd.find(".itemBtnBox div[ansid=" + arr[tm] + "]").addClass("seleted");
            }
        }
        if (q.type == 3) {
            $("#quesArea dd[qid=" + q.id + "] .ansBtn[ansid=" + q.ans + "]").addClass("seleted");
        }
        if (q.type == 4) {
            $("#quesArea dd[qid=" + q.id + "] textarea").val(q.ans);
        }
        if (q.type == 5) {
            var arr = q.ans.split(",");
            $("#quesArea dl dd[qid=" + q.id + "] .textbox").each(function (index) {
                $(this).val(arr[index]);
            });
        }
    }
    //计算完成的题数
    $(".CompleteNumber").text($("#cardBox dd[class]").size());
}