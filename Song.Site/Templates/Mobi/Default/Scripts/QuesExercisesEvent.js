$(function () {
    _quesSelectEvent();
    _btnEvent();
});

//试题的选择事件
function _quesSelectEvent() {
    $(".quesItem").each(function () {
        var type = Number($(this).attr("type"));
        //单选题
        if (type == 1) quesEventType1($(this));
        if (type == 2) quesEventType2($(this));
        if (type == 3) quesEventType3($(this));
        if (type == 4) quesEventType4($(this));
        if (type == 5) quesEventType5($(this));
    });
}
//单选题的选择
function quesEventType1(ansItem) {
    ansItem.find(".answer").click(function () {
        var pat = $(this).parent();
        var isSel = $.trim($(this).attr("isSel")) == "true" ? true : false;
        if (isSel) {
            $(this).attr("isSel", false);
            $(this).removeClass("answerSel");
            $(this).find("span.type").html("&#xf00c6;");
        } else {
            //如要没有选中			
            pat.find(".answer").attr("isSel", false).removeClass("answerSel");
            pat.find("span.type").html("&#xf00c6;");
            $(this).attr("isSel", true);
            $(this).addClass("answerSel");
            $(this).find("span.type").html("&#xe667;");
        }
        // 对选择项进行判断，并转到下一题
        if (pat.find(".answer[issel=true]").size() > 0) {
            //提交答题信息以验证对错
            $("#btnSubmit").click();
        }
    });
}
//多选题的选择
function quesEventType2(ansItem) {
    ansItem.find(".answer").click(function () {
        var isSel = $.trim($(this).attr("isSel")) == "true" ? true : false;
        if (isSel) {
            $(this).attr("isSel", false);
            $(this).removeClass("answerSel");
            $(this).find("span.type").html("&#xe603;");
        } else {
            $(this).attr("isSel", true);
            $(this).addClass("answerSel");
            $(this).find("span.type").html("&#xe62a;");
        }
    });
}
//判断题的选择
function quesEventType3(ansItem) {
    ansItem.find(".answer").click(function () {
        var isSel = $(this).attr("isSel") == "true" ? true : false;
        if (isSel) {
            $(this).attr("isSel", false);
            $(this).removeClass("answerSel");
            $(this).find("span.type").html("&#xe621;");
        } else {
            //如要没有选中
            $(this).parent().find(".answer").attr("isSel", false);
            $(this).parent().find(".answer").removeClass("answerSel");
            $(this).parent().find("span.type").html("&#xe621;");
            $(this).attr("isSel", true);
            $(this).addClass("answerSel");
            $(this).find("span.type").html("&#xe63b;");
        }
        // 对选择项进行判断，并转到下一题
        if ($(this).parent().find(".answer[issel=true]").size() > 0) {
            //提交答题信息以验证对错
            $("#btnSubmit").click();
        }
    });
}
//简答题的事件
function quesEventType4(ansItem) {
    ansItem.find(".answer textarea").focusout(function () {
        var ansInput = $.trim($(this).val());
        if (ansInput != "") {
            new MsgBox("提示", "简答题不自动判题！请自行查看答案。", 400, 200, "msg").Open();
        }
    });
}
//填空题的事件
function quesEventType5(ansItem) {
    ansItem.find(".answer input[type=text]").focusout(function () {
        var quesItem = $(this).parents(".quesItem");
        var answer = $(this).parent().parent().find(".answer");
        var count = 0;
        answer.each(function () {
            var ansInput = $.trim($(this).find("input[type=text]").val());
            if (ansInput != "") {
                count++;
            }
        });
        //提交答题信息以验证对错
        if (count == answer.size()) $("#btnSubmit").click();
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
    quesAreaMove(left);
}
/*
界面各各按钮的事件

*/
function _btnEvent() {
    //试题提交
    $("#btnSubmit").click(function () {
        //当前试题所在区域的html对象
        var ques = $(".quesItem[index=" + $("#indexNum").attr("index") + "]");
        var type = $.trim(ques.attr("type"));
        var func = eval("_decide" + type);
        if (func != null) func(ques);
        //如果答题正确，则直接转到下一题
        if (ques.attr("ansstate") == "true") _quesMove(ques, 1);

    });
    //查看答案
    $("#btnAnswer").click(function () {
        var qid = $(".quesItem[index=" + $("#indexNum").attr("index") + "]").attr("qid");
        new PageBox("参考答案", "QuesAnswer.ashx?id=" + qid, 80, 60, null).Open();
    });
    //试题解析
    $("#btnExplain").click(function () {
        var qid = $(".quesItem[index=" + $("#indexNum").attr("index") + "]").attr("qid");
        new PageBox("试题解析", "QuesExplain.ashx?id=" + qid, 90, 90, null).Open();
    });
    //收藏
    $("#btnFav").click(function () {
        if (!isLogin) {
            //如果没有登录
            new MsgBox("提示", "未登录状态，不可以收藏试题。", 90, 40, "alert").Open();
        } else {
            var ques = $(".quesItem[index=" + $("#indexNum").attr("index") + "]");
            var qid = ques.attr("qid");
            var isCollect = ques.attr("IsCollect") == "True" ? true : false;
            $.get("AddCollect.ashx", { "qid": qid, "isCollect": isCollect }, function () {
                var ques = $(".quesItem[index=" + $("#indexNum").attr("index") + "]");
                ques.attr("IsCollect", isCollect ? "False" : "True");
                $("#btnFav").toggleClass("IsCollect");
            });
        }
    });
    //笔记
    $("#btnNote").click(function () {
        if (!isLogin) {
            //如果没有登录
            new MsgBox("提示", "未登录状态，不可以编写笔记。", 90, 40, "alert").Open();
        } else {
            var qid = $(".quesItem[index=" + $("#indexNum").attr("index") + "]").attr("qid");
            new PageBox("添加笔记", "QuesNoteAdd.ashx?qid=" + qid, 90, 50, null).Open();
        }
    });
    //报错
    $("#btnError").click(function () {
        var qid = $(".quesItem[index=" + $("#indexNum").attr("index") + "]").attr("qid");
        new PageBox("错误试题提交", "QuesSubmitError.ashx?id=" + qid, 90, 80, null).Open();
    });
    //答题卡
    $("#btnCard").click(function () {
        var box = $("#cardBox");
        box.width($(window).width() - 10).height($(window).height() - 10);
        box.css({ left: 5, top: 5 });
        $("#cardBoxInner").width(box.width() - 20).height(box.height() - 60);
        $("#cardPanel").toggle();
    });
    //答题卡关闭按钮
    $("#cardBoxColse").click(function () {
        $("#cardPanel").hide();
    });
}
//显示答题的正确与否状态
//ques:试题区域
//isCorrect:是否正确
function showResult(ques, isCorrect) {
    var qitem = ques.find(".quesItemsBox");
    if (isCorrect == null) {
        qitem.removeClass("error").removeClass("correct");
        return;
    }
    ques.attr("ansstate", isCorrect);
    var qid = ques.attr("qid");
    //如果正确
    if (isCorrect) {
        qitem.addClass("correct").removeClass("error");
        //设置答题卡状态
        setCardState("succ", qid);
    } else {
        //如果错误
        qitem.removeClass("correct").addClass("error");
        //增加错题
        $.get("AddQues.ashx", { "qid": qid }, function () { });
        //设置答题卡状态，并显示答案
        setCardState("error", qid);
        $(".quesItem[qid=" + qid + "]").find(".quesAnswerBox").show();
    }
}
//单选题判断
//ques: 当前试题所在区域的html对象，即class='quesItem'
function _decide1(ques) {
    var selitem = ques.find(".quesItemsBox .answer[issel=true]");
    //alert(selitem.size());
    if (selitem.size() < 1) {
        var msg = new MsgBox("提示", "您还没有答题！", 90, 40, "msg");
        msg.Open();
        showResult(ques, null);
        return;
    }
    showResult(ques, $.trim(selitem.attr("correct")).toLowerCase() == "true");
}
//多选题判断
function _decide2(ques) {
    var selitem = ques.find(".quesItemsBox .answer[issel=true]");
    if (selitem.size() < 1) {
        var msg = new MsgBox("提示", "您还没有答题！", 90, 40, "msg");
        msg.Open();
        showResult(ques, null);
        return;
    }
    var isCorrect = true;
    var corrItem = ques.find(".quesItemsBox .answer[correct=True]");
    corrItem.each(function () {
        if ($(this).attr("issel") != "true") isCorrect = false;
    });
    showResult(ques, isCorrect && corrItem.size() == selitem.size());
}
//判断题判断
function _decide3(ques) {
    var selitem = ques.find(".quesItemsBox .answer[issel=true]");
    if (selitem.size() < 1) {
        var msg = new MsgBox("提示", "您还没有答题！", 90, 40, "msg");
        msg.Open();
        showResult(ques, null);
        return;
    }
    showResult(ques, selitem.attr("correct").toLowerCase() == "true");
}
//简答题判断
function _decide4(ques) {
    new MsgBox("提示", "简答题不自动判题！无须提交。", 90, 40, "msg").Open();
}
//填空题
//ques: 当前试题所在区域的html对象，即class='quesItem'
function _decide5(ques) {
    var iscorrect = true;
    var answer = ques.find(".answer");
    //遍历当前试题的所有填空项
    answer.each(function () {
        var ansInput = $.trim($(this).find("input[type=text]").val());
        if (ansInput == "") {
            iscorrect = false;
            return false;
        }
        var tm = false; 	//临时用判断当前试题对错的变量
        var correct = $(this).attr("correct");
        if (correct.indexOf(",") > -1) {
            var arr = correct.split(",");
            for (var s in arr) {
                if (ansInput == arr[s]) tm = true;
            }
        } else {
            if (correct != ansInput) {
                iscorrect = false;
                return false;
            } else {
                tm = true;
            }
        }
        if (!tm) {
            iscorrect = false;
            return false;
        }
    });
    //是否答对
    var ques = $(".quesItem[index=" + $("#indexNum").attr("index") + "]");
    showResult(ques, iscorrect);
}
