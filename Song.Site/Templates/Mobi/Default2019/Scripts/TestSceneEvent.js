$(function () {
    _quesAnswerEvent();
    _btnEvent();
});

/*
界面各按钮的事件
*/
function _btnEvent() {
    //交卷
    $("#SubmitExam").click(function () {
        //完成数与试题总数
        var count = Number($("#CompleteNumber").html());
        var sum = Number($("#QuesCount").html());
        if (count < 1) {
            new MsgBox("提示", "你还没有做任何试题。", 90, 40, "msg").Open();
            return;
        } else {
            var show = "";
            show += "当前考试共 " + sum + " 道试题。<br/>";
            show += "您已经完成 " + count + " 道试题。<br/>";
            if (count < sum) show += "还有 <b>" + (sum - count) + "</b> 道试题没有做。<br/>";
            show += "<br/>您是否确认交卷？";
            var msg = new MsgBox("确认交卷", show, 90, 40, "confirm");
            msg.EnterEvent = submitResult;
            msg.Open();
        }
        return;
    });
    //收藏
    $(".btnFav").click(function () {
        var ques = $(this).parents(".quesItem");
        var qid = ques.attr("qid");
        var isCollect = ques.attr("IsCollect") == "True" ? true : false;
        $.get("AddCollect.ashx", { "qid": qid, "isCollect": isCollect }, function () {
            var ques = $(".quesItem[qid=" + qid + "]");
            ques.attr("IsCollect", isCollect ? "False" : "True");
            $(".btnFav").toggleClass("IsCollect");
            $(".btnFav").html(!isCollect ? "&#xe662;" : "&#xe661;");
        });
    });
    //报错
    $(".btnError").click(function () {
        var qid = $(this).parents(".quesItem").attr("qid");
        new PageBox("错误试题提交", "QuesSubmitError.ashx?id=" + qid, 90, 80, null).Open();
    });
    //答题卡
    $("#btnCard").click(function () {
        var off = $(this).offset();
        var little = $("#cardLittle");
        little.height(12);
        little.css({ left: off.left, top: off.top - 10, width: $(this).width() });
        var box = $("#cardBox");
        box.width($(window).width()).height($(window).height() - $(this).height()-10);
        box.css({ left: off.left-10, top: 0 });
        $("#cardBoxInner").width(box.width() - 20).height(box.height() - 60);
        $("#cardPanel").toggle();
    });
    //答题卡关闭按钮
    $("#cardBoxColse").click(function () {
        $("#cardPanel").hide();
    });
}
//试题移动
//quesitem:试题区域
//dirt:方向，1为向右，-1为向左
function _quesMove(quesitem, dirt) {
    var left = Number($("#quesArea").css("left").replace("px", ""));
	left = isNaN(left) ? 0 : left;
    if (dirt == 1) {
        if (quesitem.attr("index") != $(".quesItem").size())
            left -= quesitem.width();
    }
    //向左
    if (dirt == -1) {
        if (quesitem.attr("index") != "1")
            left += quesitem.width();
    }
    $("#quesArea").animate({ left: left }, function () {

    });
}
/*
//答题时的事件

*/
function _quesAnswerEvent() {
    $(".quesItem").each(function () {
        var type = Number($(this).attr("type"));
        try {
            var func = eval("quesEventType" + type);
            if (func != null) func($(this));
        } catch (e) { }
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
            var quesItem = $(this).parents(".quesItem");
            _quesMove(quesItem, 1); //往右滑动一道题
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
            var quesItem = $(this).parents(".quesItem");
            _quesMove(quesItem, 1);
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

//提交答题信息
//patter:提交方式，1为自动提交，2为交卷
function submitResult(patter) {
    if (window.isSubmit) return;
    if (window.isSubmited) return;
    patter = !isNaN(Number(patter)) ? Number(patter) : 2;
    //提交考试成绩	
    var score = new Score().Clac();	//计算考试得分
    var xml = encodeURIComponent(getResultXml(patter));
    var urlPath = "/ajax/InTestResult.ashx?timestamp=" + new Date().getTime();
    $.ajax({
        type: "POST", url: urlPath, dataType: "text", data: { result: xml },
        //开始，进行预载
        beforeSend: function (XMLHttpRequest, textStatus) {
            window.isSubmit = true;            
            if (patter == 1) Mask.Submit();
            if (patter == 2) {
                var msg = new MsgBox("交卷中", "当前考试得分：" + score + "分<br/>正在上传成绩，请稍等……", 90, 40, "loading");
                msg.ShowCloseBtn = false;
                msg.Open();
            }
        },
        //加载出错
        error: function (XMLHttpRequest, textStatus, errorThrown) {
        },
        //加载成功！
        success: function (data) {
            try {
                //返回三个参数：
                //score:得分;
                //trid:考试成绩记录的id
                //tpid:试卷Id
                var obj = eval("(" + data + ")");
				//MsgBox.Close();	//关闭所有消息窗
                var msg = new MsgBox("成绩递交成功", "您的得分： " + obj.score+"分", 90, 40, "msg");
                window.trid = obj.trid;
                window.tpid = obj.tpid;
                msg.OverEvent = submitSuccess;
                msg.Open();
                window.isSubmit = false;
                window.isSubmited = true;
            } catch (e) {
                window.location.href = "TestPapers.ashx?couid=" + $().getPara("couid");
            }
        }
    });
}
//提交成功
function submitSuccess(trid, tpid) {
    var href = "TestView.ashx?trid=" + window.trid + "&tpid=" + window.tpid;
    var box = new PageBox("成绩查看", href, 100, 100, "url");
    box.CloseEvent = function () {
        window.location.href = "TestPapers.ashx?couid="+$().getPara("couid");
    }
    box.Open();
}
//获取答题信息
//patter:提交方式，1为自动提交，2为交卷
function getResultXml(patter) {
    patter = !isNaN(Number(patter)) ? Number(patter) : 2;
    var score = $("#cardBox").attr("score");
    var res = "<?xml version=\"1.0\" encoding=\"utf-8\" ?>";
    res += "<results uid=\"" + uid + "\" examid=\"" + 0 + "\" stid=\"" + stID + "\"  stname=\"" + ($.trim(stName) == "" ? stAccName : $.trim(stName))
		+ "\" couid=\"" + courseid + "\" couname=\"" + courseName + "\" stsid=\"" + stsID + "\" stsname=\"" + stsName
        + "\" sbjid=\"" + sbjid + "\" sbjname=\"" + sbjName + "\" patter=\"" + patter + "\" score=\"" + score+ "\" isclac=\"true" 
		+ "\" tpid=\"" + testPagerID + "\" tpname=\"" + testPagerName + "\">";
    $("#cardBox dl").each(function () {
        res += "<ques type='" + $(this).attr("type") + "' count='" + $(this).attr("count") + "' number='" + $(this).attr("number") + "'>";
        $(this).find("dd").each(function () {
            var type = Number($(this).parent().attr("type"));
            var ans = $(this).attr("ans") ? $(this).attr("ans") : "";
			var sucess=$(this).attr("sucess");
			var score=$(this).attr("score");
            if (type == 1 || type == 2 || type == 3) {
                res += "<q id=\"" + $(this).attr("qid") + "\" class=\"" + $(this).attr("class") + "\" num=\"" + $(this).attr("num") 
				+ "\" sucess=\"" + sucess+ "\" score=\"" + score
				+ "\" ans=\"" + ans + "\"/>";
            }
            if (type == 4 || type == 5) {
                res += "<q id=\"" + $(this).attr("qid") + "\" class=\"" + $(this).attr("class") + "\" num=\"" + $(this).attr("num") 
				+ "\" sucess=\"" + sucess+ "\" score=\"" + score
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