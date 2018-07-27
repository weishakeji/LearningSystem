//全局变量
//是否已经交卷
window.isSubmited = false;
//是否正在卷
window.isSubmit = false;


$(function () {
    setExamingInitLoyout();
    if (isStudy) {
        setExamCurrDateTime();
        _quesAnswerEvent();
        setInterval("setExamCurrDateTime()", 1000);
        buildCard();
    }
});

//设置初始化的布局与事件
function setExamingInitLoyout() {
    //显示第一个
    $(".quesItem:first").show();
    //上一题
    $("#btnPrev").click(function () {
        if ($(this).attr("state") != "1") {
            ShowQues(null, -1);
        }
    });
    //下一题
    $("#btnNext").click(function () {
        if ($(this).attr("state") != "1") {
            ShowQues(null, 1);
        }
    });
    //确定，并进入下一题
    $("#btnEnterNext").click(function () {

    });
    //交卷
    $("#btnSubmint").click(function () {
        //完成数与试题总数
        var count = Number($("#CompleteNumber").html());
        var sum = Number($("#QuesCount").html());
        if (count < 1) {
            new MsgBox("提示", "你还没有做任何试题。", 400, 300, "msg").Open();
            return;
        } else {
            var show = "";
            show += "当前考试共 " + sum + " 道试题。<br/>";
            show += "您已经完成 " + count + " 道试题。<br/>";
            if (count < sum) show += "还有 <b>" + (sum - count) + "</b> 道试题没有做。<br/>";
            show += "<br/>您是否确认交卷？";
            var msg = new MsgBox("确认交卷", show, 400, 300, "confirm");
            msg.EnterEvent = submitResult;
            msg.Open();
        }
        return;
    });
}
//显示某道题
//index:当前试题的索引号，如果为null，则取quesArea的index属性
//action:向前，还是向后；正数向后，负数向前
function ShowQues(index, action) {
    if (index == null) index = Number($("#quesArea").attr("index"));
    //试题区域
    var contxt = $("#quesArea-context");
    var left = Number(contxt.css("left").replace("px", ""));
    if (action == 1) {
        if (index < $(".quesItem").size() - 1) {
            index++;
            left -= $(".quesItem").width();
        }
    }
    //向左
    if (action == -1) {
        if (Number($("#quesArea").attr("index")) > 0) {
            index--;
            left += $(".quesItem").width();
        }
    }
    if (action == 0) {
        left = $(".quesItem").width() * index;
        contxt.css("left", -left);
        $("#quesArea").attr("index", index);
    } else {
        $("#btnPrev,#btnNext").attr("state", 1);
        contxt.animate({ left: left }, 1000, function () {
            $("#quesArea").attr("index", index);
            $("#btnPrev,#btnNext").attr("state", 0);
        });
    }
}
//设置当前时间与剩余时间
function setExamCurrDateTime() {
    //计算客户端实时时间
    var tm = new Date();
    var span = tm.getTime() - ClientTime.getTime();
    var curr = new Date(ServerTime.getTime() + span);
    $("#currTime").text(curr.Format("hh:mm:ss"));
    //计算剩余的时间，还剩多久
    //考试结束时间
    var overSpan = Number($("#timeSpan").text());
    span = overSpan * 60 - parseInt(span / 1000);
    //剩余时间的分抄计算
    setExamSurplusDateTime(span);
}
//设置剩余时间
//second:剩余的秒数
function setExamSurplusDateTime(second) {
    var mm = parseInt(parseInt(second) / 60);
    var ss = parseInt(second) % 60;
    var resTime = $("#timeBox");
    if (second >= 0) {
        resTime.find(".mm").text(mm);
        resTime.find(".ss").text(ss);
    } else {
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
        var index = Number($(this).html()) - 1;
        ShowQues(index, 0);
    });
    //统计有多少道题
    $("#QuesCount").html(box.find("dl dd").size());
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

/*答题时的事件处理*/
function _quesAnswerEvent() {
    $(".quesItem").each(function () {
        var type = Number($(this).attr("type"));
        try {
            var func = eval("quesEventType" + type);
            if (func != null) func($(this));
        } catch (e) { }
    });
}
//单选题的选择
function quesEventType1(ansItem) {
    ansItem.find(".ansItem").click(function () {
        var isSel = String($(this).attr("isSel")).toLowerCase() == "true" ? true : false;
        if (isSel) {
            $(this).attr("isSel", false).removeClass("answerSel");
        } else {
            //如要是未选中状态，此操作将设置为选中，并取消其它选项
            $(this).parent().find(".ansItem").attr("isSel", false);
            $(this).parent().find(".ansItem").removeClass("answerSel");
            $(this).attr("isSel", true).addClass("answerSel");
        }
        var sel = $(this).parent().find(".ansItem[isSel=true]");
        if (sel.size() < 1) setCardStyle(ansItem.attr("qid"), null);
        if (sel.size() > 0) setCardStyle(ansItem.attr("qid"), sel.attr("ansid"));
        if ($(this).parent().find(".ansItem[issel=true]").size() > 0) {
            ShowQues(null, 1);
        }
    });
}
//多选题的选择
function quesEventType2(ansItem) {
    ansItem.find(".ansItem").click(function () {
        var isSel = String($(this).attr("isSel")).toLowerCase() == "true" ? true : false;
        if (isSel) {
            $(this).attr("isSel", false).removeClass("answerSel");
        } else {
            $(this).attr("isSel", true).addClass("answerSel");
        }
        var sel = $(this).parent().find(".ansItem[isSel=true]");
        if (sel.size() < 2) setCardStyle(ansItem.attr("qid"), null);
        if (sel.size() >= 2) {
            var answer = "";
            sel.each(function () {
                answer += $(this).attr("ansid") + ",";
            });
            setCardStyle(ansItem.attr("qid"), answer);
        }
    });
}
//判断题的选择
function quesEventType3(ansItem) {
    ansItem.find(".ansItem").click(function () {
        var isSel = $(this).attr("isSel") == "true" ? true : false;
        if (isSel) {
            $(this).attr("isSel", false).removeClass("answerSel");
        } else {
            //如果没有选中
            $(this).parent().find(".ansItem").attr("isSel", false);
            $(this).parent().find(".ansItem").removeClass("answerSel");
            //$(this).parent().find("span.type").html("&#xe621;");
            $(this).attr("isSel", true).addClass("answerSel");
        }
        var sel = $(this).parent().find(".ansItem[isSel=true]");
        if (sel.size() < 1) setCardStyle(ansItem.attr("qid"), null);
        if (sel.size() > 0) setCardStyle(ansItem.attr("qid"), sel.attr("ansid"));
        if ($(this).parent().find(".ansItem[issel=true]").size() > 0) {
            ShowQues(null, 1);
        }
    });
}
//简答题的作答
function quesEventType4(ansItem) {
    ansItem.find("textarea").focusout(function () {
        var answer = $.trim($(this).val());
        setCardStyle(ansItem.attr("qid"), answer);
    });
}
//填空题的作答
function quesEventType5(ansItem) {
    ansItem.find("input[type=text]").focusout(function () {
        var answer = "";
        ansItem.find("input.textbox5[value!='']").each(function () {
            answer += $.trim($(this).val()) + ",";
        });
        setCardStyle(ansItem.attr("qid"), answer);
    });
}

//操作答题卡
function setCardStyle(qid, answer) {
    var box = $("#cardBox dd[qid=" + qid + "]");
    if (answer != null && answer != "") {
        box.attr("ans", answer);
        box.addClass("answer");
    } else {
        box.removeAttr("ans");
        box.removeClass("answer");
    }
    //计算完成的题数
    $("#CompleteNumber").text($("#cardBox dd[ans]").size());
}

//提交答题信息
//patter:提交方式，1为自动提交，2为交卷
function submitResult(patter) {
    if (window.isSubmit) return;
    if (window.isSubmited) return;
    patter = !isNaN(Number(patter)) ? Number(patter) : 2;
    //提交考试成绩	
    var score = new Score().Clac(); //计算考试得分
    var xml = encodeURIComponent(getResultXml(patter));
    var urlPath = "/ajax/InTestResult.ashx?timestamp=" + new Date().getTime();
    $.ajax({
        type: "POST", url: urlPath, dataType: "text", data: { result: xml },
        //开始，进行预载
        beforeSend: function (XMLHttpRequest, textStatus) {
            window.isSubmit = true;
            if (patter == 1) Mask.Submit();
            if (patter == 2) {
                var msg = new MsgBox("交卷中", "当前考试得分：" + score + "分<br/>正在上传成绩，请稍等……", 400, 300, "loading");
                msg.ShowCloseBtn = false;
                msg.Open();
            }
        },
        //加载出错
        error: function (XMLHttpRequest, textStatus, errorThrown) {
            alert(errorThrown);
        },
        //加载成功！
        success: function (data) {
            try {
                MsgBox.Close();
                //返回三个参数：
                //score:得分;
                //trid:考试成绩记录的id
                //tpid:试卷Id
                var obj = eval("(" + data + ")");
                var url = "TestView.ashx?trid=" + obj.trid;
                var box = new top.PageBox("成绩回顾", url, 980, 80, null, window.name);
                box.CloseEvent = function () {
                    window.location.href = "Test.ashx";
                }
                box.Open();
                //MsgBox.Close(true);
                window.isSubmit = false;
                window.isSubmited = true;
            } catch (e) {
                alert("错误提示：" + e);
            }
        }
    });
}

//获取答题信息
//patter:提交方式，1为自动提交，2为交卷
function getResultXml(patter) {
    patter = !isNaN(Number(patter)) ? Number(patter) : 2;
    var score = $("#cardBox").attr("score");
    var res = "<?xml version=\"1.0\" encoding=\"utf-8\" ?>";
    res += "<results uid=\"" + uid + "\" examid=\"" + 0 + "\" stid=\"" + stID + "\"  stname=\"" + ($.trim(stName) == "" ? stAccName : $.trim(stName))
		+ "\" couid=\"" + courseid + "\" couname=\"" + courseName + "\" stsid=\"" + stsID + "\" stsname=\"" + stsName
        + "\" sbjid=\"" + sbjid + "\" sbjname=\"" + sbjName + "\" patter=\"" + patter + "\" score=\"" + score + "\" isclac=\"true"
		+ "\" tpid=\"" + testPagerID + "\" tpname=\"" + testPagerName + "\">";
    $("#cardBox dl").each(function () {
        res += "<ques type='" + $(this).attr("type") + "' count='" + $(this).attr("count") + "' number='" + $(this).attr("number") + "'>";
        $(this).find("dd").each(function () {
            var type = Number($(this).parent().attr("type"));
            var ans = $(this).attr("ans") ? $(this).attr("ans") : "";
            var sucess = $(this).attr("sucess");
            var score = $(this).attr("score");
            if (type == 1 || type == 2 || type == 3) {
                res += "<q id=\"" + $(this).attr("qid") + "\" class=\"" + $(this).attr("class") + "\" num=\"" + $(this).attr("num")
				+ "\" sucess=\"" + sucess + "\" score=\"" + score
				+ "\" ans=\"" + ans + "\"/>";
            }
            if (type == 4 || type == 5) {
                res += "<q id=\"" + $(this).attr("qid") + "\" class=\"" + $(this).attr("class") + "\" num=\"" + $(this).attr("num")
				+ "\" sucess=\"" + sucess + "\" score=\"" + score
				+ "\">";
                res += "<![CDATA[" + ans + "]]>"
                res += "</q>";
            }
        });
        res += "</ques>";
    });
    res += "</results>";
    return res;
}


/*计算成绩*/
function Score() {
    this.Cardbox = $("#cardBox");
}
//答题卡
Score.prototype.Cardbox = null;
//获取答题卡的一些参数
Score.getData = function (elem) {
    var obj = {};
    obj.qid = elem.attr("qid"); //试题id
    obj.ans = elem.attr("ans"); //答案
    obj.ans = !obj.ans ? "" : obj.ans;
    obj.num = Number(elem.attr("num")); //试题分数
    obj.num = isNaN(obj.num) ? 0 : obj.num;
    return obj;
}
Score.prototype.Clac = function () {
    var score = 0;
    score += this.clac_1();
    score += this.clac_2();
    score += this.clac_3();
    score += this.clac_5();
    this.Cardbox.attr("score", score);
    return score;
}
//单选题的计算
Score.prototype.clac_1 = function () {
    var score = 0;
    this.Cardbox.find("dl[type=1] dd").each(function () {
        var d = Score.getData($(this)); //获取各项值
        var ansid = $(".quesItem[qid=" + d.qid + "] .ansItem[correct=True]").attr("ansid");
        //回答正确
        if (d.ans == ansid) {
            score += d.num;
            $(this).attr("sucess", "true").attr("score", d.num);
        } else {
            $(this).attr("sucess", "false").attr("score", 0);
        }
    });
    return score;
}
//多选题的计算
Score.prototype.clac_2 = function () {
    var score = 0;
    this.Cardbox.find("dl[type=2] dd").each(function () {
        var d = Score.getData($(this)); //获取各项值
        //学员答题选中的选项id,数组从小到大排序
        var ans = d.ans.split(",");
        for (var i = 0; i < ans.length; i++) if (ans[i].length == 0) ans.splice(i, 1);
        for (var i = 0; i < ans.length; i++) ans[i] = Number(ans[i]);
        ans = ans.sort(function (a, b) { return a > b ? 1 : -1 });
        //该题的正确答案
        var ansitem = $(".quesItem[qid=" + d.qid + "] .ansItem[correct=True]");
        var ansid = new Array();
        ansitem.each(function () {
            ansid.push(Number($(this).attr("ansid")));
        });
        ansid = ansid.sort(function (a, b) { return a > b ? 1 : -1 });
        //判断对错
        var isSuccess = false;
        if (ans.length == ansid.length) {
            var istm = true;
            for (var i = 0; i < ans.length; i++) {
                if (ans[i] != ansid[i]) {
                    istm = false;
                    continue;
                }
            }
            isSuccess = istm;
        }
        //回答正确
        if (isSuccess) {
            score += d.num;
            $(this).attr("sucess", "true").attr("score", d.num);
        } else {
            $(this).attr("sucess", "false").attr("score", 0);
        }
    });
    return score;
}
//判断题的计算
Score.prototype.clac_3 = function () {
    var score = 0;
    this.Cardbox.find("dl[type=3] dd").each(function () {
        var d = Score.getData($(this)); //获取各项值
        var ansitem = $(".quesItem[qid=" + d.qid + "] .ansItem[correct=true]");
        var ansid = ansitem.attr("ansid"); //正确答案
        //回答正确
        if (d.ans == ansid) {
            score += d.num;
            $(this).attr("sucess", "true").attr("score", d.num);
        } else {
            $(this).attr("sucess", "false").attr("score", 0);
        }
    });
    return score;
}
//填空题的计算
Score.prototype.clac_5 = function () {
    var score = 0;
    this.Cardbox.find("dl[type=5] dd").each(function () {
        var d = Score.getData($(this)); //获取各项值
        //获取正确答案
        var ansid = new Array();
        $(".quesItem[qid=" + d.qid + "] .ansItem").each(function () {
            ansid.push({
                success: false,
                ans: $(this).attr("correct")
            });
        });
        //学员答题选中的选项id,数组从小到大排序
        var ans = d.ans.split(",");
        for (var i = 0; i < ans.length; i++) if (ans[i].length == 0) ans.splice(i, 1);
        //判断答题是否正确，逐一判断每个填空项
        for (var i = 0; i < ansid.length; i++) {
            var ansitem = ansid[i].ans.split(",");
            for (var j = 0; j < ansitem.length; j++) {
                if (ansitem[j].length == 0) continue;
                try {
                    if (ansitem[j] == ans[i]) {
                        ansid[i].success = true;
                        break;
                    }
                } catch (e) { }
            }
        }
        //所有填空项，都填对才能得分
        var isSuccess = true;
        for (var i = 0; i < ansid.length; i++) {
            if (!ansid[i].success) {
                isSuccess = false;
                break;
            }
        }
        //回答正确
        if (isSuccess) {
            score += d.num;
            $(this).attr("sucess", "true").attr("score", d.num);
        } else {
            $(this).attr("sucess", "false").attr("score", 0);
        }
    });
    return score;
}