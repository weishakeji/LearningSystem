$(function () {
    selectedInit();
    subjectEvent(); 	//各种选择按钮的事件
    setButtonEvent();
    _quesSelectEvent();
	//折叠选择区域
	$("#btnShow").click(function(){
		$("#subject").toggle();
	});
});


//各种选择按钮的事件
function subjectEvent() {
    $(".selectItem a").click(function () {
        var attr = $(this).attr("attr");
        var val = $(this).attr(attr);
        //要清除的参数
        var clear = $(this).attr("clear");
        var url = buildUrl(attr, val, clear);
        window.document.location.href = url;
        return false;
    });
}
//初始化
function selectedInit() {
    //试题区域的高度
    //$("#quesArea").height(document.documentElement.clientHeight - 180);
    //参数数组
    var strs = GetRequest();
    $("#Selected").html("");
    for (var i = 0; i < strs.length; i++) {
        var item = $(".selectItem a[" + strs[i][0] + "=" + strs[i][1] + "]");
        if (item.size() < 1) continue;
        //主题，如专业、类型的文字信息
        var theme = item.parent().parent().prev().text();
        var txt = item.text();
        var clear = item.attr("clear");
        var tm = "<div class=\"selectedItem\" clear=\"" + clear + "\">";
        tm += "<div class='name'>" + theme + txt + "</div><div class='close' attr='" + strs[i][0] + "'>&nbsp;</div>";
        tm += "</div> ";
        $("#Selected").append(tm);
    }
    if ($("#Selected .selectedItem").size() < 1) {
        var tm = "<div class=\"selectedNoItem\" >";
        tm += "<div class='name'>全部</div>";
        tm += "</div> ";
        $("#Selected").append(tm);
    }
    //选中的专业在鼠标经过时
    $("#Selected .selectedItem").hover(function () {
        $(this).addClass("selectedItemOver");
        $(this).find(".close").addClass("closeOver");
    }, function () {
        $(this).removeClass("selectedItemOver");
        $(this).find(".close").removeClass("closeOver");
    });
    $("#Selected .close").click(function () {
        var clear = $(this).parent().attr("clear");
        var url = buildUrl($(this).attr("attr"), null, clear);
        window.document.location.href = url;
    });

}
//当前地址，根据参数生成地址
function buildUrl(attr, val, clear) {
    //当前页面的上级路径，因为子页面没有写路径，默认与本页面同路径
    var url = String(window.document.location.href);
    url = url.replace(/#/ig, "");
    if (url.indexOf("?") > -1) url = url.substring(0, url.lastIndexOf("?"));
    //参数数组
    var strs = GetRequest();
    var para = new Array();
    var isHave = false;
    for (var i = 0; i < strs.length; i++) {
        if (strs[i][0] == attr) {
            strs[i][1] = val;
            isHave = true;
        }
        if (!(strs[i][0] == attr && val == null)) {
            para.push(strs[i]);
        }
    }
    if (!isHave && val != null) para.push(new Array(attr, val));
    if (para.length < 1) return url;
    url += "?";
    //要清除的数据
    var clr = clear == null ? new Array() : clear.split(",");
    for (var i = 0; i < para.length; i++) {
        var tmIsHave = false;
        for (var j = 0; j < clr.length; j++) {
            if (para[i][0] == clr[j]) {
                tmIsHave = true;
                break;
            }
        }
        if (tmIsHave) continue;
        url += para[i][0] + "=" + para[i][1];
        if (i < para.length - 1) url += "&";
    }
    if (url.substring(url.length - 1) == "?") url = url.substring(0, url.length - 1);
    if (url.substring(url.length - 1) == "&") url = url.substring(0, url.length - 1);
    return url;
}
//获取参数
function GetRequest() {
    var url = location.search; //获取url中"?"符后的字串
    var theRequest = new Array();
    if (url.indexOf("?") != -1) {
        var str = url.substr(1);
        strs = str.split("&");
        for (var i = 0; i < strs.length; i++) {
            theRequest.push(new Array(strs[i].split("=")[0], strs[i].split("=")[1]));
        }
    }
    return theRequest;
}
/*

试题控制区域，例如上一题，下一题等

*/
//显示某道题
//index:当前试题的索引号
//action:向前，还是向后；正数向后，负数向前
function ShowQues(index, action) {
    var ques = $("#quesArea .quesItem");
    //新索引
    var newIndex = index + action;
    if (newIndex < 1) {
        alert("当前试题范围前一题不存在。");
        return;
    }
    if (newIndex > ques.size()) {
        alert("当前试题范围后一题不存在。");
        return;
    }
    //所有的试题隐藏   
    ques.hide();
    var dd = $("#quesArea .quesItem[index=" + newIndex + "]");
    if (dd.size() > 0) {
        dd.show();
        var numbox = $("#indexNum");
        numbox.attr("index", newIndex);
        numbox.html(Number(numbox.attr("initIndex")) + newIndex - 1);
    }
}
//设置控制区的按钮事件
function setButtonEvent() {
    $("#quesArea .quesItem:first").show();
    //上一题
    $("#btnPrev").click(function () {
        var index = Number($("#quesArea .quesItem:visible").attr("index"));
        ShowQues(index, -1);
    });
    //下一题
    $("#btnNext").click(function () {
        var index = Number($("#quesArea .quesItem:visible").attr("index"));
        ShowQues(index, 1);
    });
    //显示答案
    $("#btnAnswer").click(function () {
        var quesid = Number($("#quesArea .quesItem:visible").attr("qid"));
        var type = Number($("#quesArea .quesItem:visible").attr("type"));
        var file = "QuestionAnswer.ashx?id=" + quesid;
        if (type == 4)
            new PageBox("参考答案", file, 80, 80).Open();
        else
            new PageBox("参考答案", file, 400, 300).Open();
        return false;
    });
    //知识点讲解
    $("#btnExplan").click(function () {
        var quesid = Number($("#quesArea .quesItem:visible").attr("qid"));
        var file = "QuesExplain.ashx?id=" + quesid;
        new PageBox("试题解析", file, 80, 80).Open();
        return false;
    });
    //学习资料
    $("#btnKnl").click(function () {
        var quesid = Number($("#quesArea .quesItem:visible").attr("qid"));
        var file = "QuesKnl.ashx?id=" + quesid;
        new PageBox("学习资料", file, 80, 80).Open();
        return false;
    });
}
/*

试题的相关操作

*/
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
        var isSel = $(this).attr("isSel") == "true" ? true : false;
        if (isSel) {
            $(this).attr("isSel", false);
            $(this).removeClass("answerSel");
            $(this).find("span.type").html("&#xf00c6;");
        } else {
            //如要没有选中
            $(this).parent().find(".answer").attr("isSel", false);
            $(this).parent().find(".answer").removeClass("answerSel");
            $(this).parent().find("span.type").html("&#xf00c6;");
            $(this).attr("isSel", true);
            $(this).addClass("answerSel");
            $(this).find("span.type").html("&#xe667;");
        }
        //是否答对
        var ques = $(".quesItem[index=" + $("#indexNum").attr("index") + "]");
        _decide1(ques);
    });
}
//多选题的选择
function quesEventType2(ansItem) {
    ansItem.find(".answer").click(function () {
        var isSel = $(this).attr("isSel") == "true" ? true : false;
        if (isSel) {
            $(this).attr("isSel", false);
            $(this).removeClass("answerSel");
            $(this).find("span.type").html("&#xe603;");
        } else {
            $(this).attr("isSel", true);
            $(this).addClass("answerSel");
            $(this).find("span.type").html("&#xe62a;");
        }
        //是否答对
        var ques = $(".quesItem[index=" + $("#indexNum").attr("index") + "]");
        _decide2(ques);

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
        //是否答对
        var ques = $(".quesItem[index=" + $("#indexNum").attr("index") + "]");
        _decide3(ques);
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
        var iscorrect = true;
        //遍历当前试题的所有填空项
        $(this).parent().parent().find(".answer").each(function () {
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
                }else{
					tm= true;
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
    });
}
//显示答题的正确与否状态
function showResult(ques, isCorrect) {
    var box = ques;
    //var box=ques.find(".quesAnswerBox");
    if (isCorrect == null) {
        box.removeClass("error");
        box.removeClass("correct");
        return;
    }
    if (isCorrect) {
        box.addClass("correct");
        box.removeClass("error");
    } else {
        box.removeClass("correct");
        box.addClass("error");
        //增加错题
        //$.get("AddQues.ashx",{"qid":ques.attr("qid")},function(){
        //});
    }
    //正确的数量
    var succNum = $(".correct").size();
    $("#succCount").html(succNum);
    //正确率
    var per = Number(succNum) / $(".quesItem").size();
    per = Math.round(per * 10000) / 100;
    $("#perCount").html(per);
}
//单选题判断
function _decide1(ques) {
    var selitem = ques.find(".quesAnswerBox .answer[issel=true]");
    if (selitem.size() < 1) {
        showResult(ques, String(selitem.attr("correct")).toLowerCase() == "false");
    } else {
        showResult(ques, String(selitem.attr("correct")).toLowerCase() == "true");
    }
}
//多选题判断
function _decide2(ques) {
    var selitem = ques.find(".quesAnswerBox .answer[issel=true]");
    if (selitem.size() < 1) {
        return;
    }
    var isCorrect = true;
    var corrItem = ques.find(".quesAnswerBox .answer[correct=True]");
    corrItem.each(function () {
        if ($(this).attr("issel") != "true") isCorrect = false;
    });
    showResult(ques, isCorrect && corrItem.size() == selitem.size());
}
//判断题判断
function _decide3(ques) {
    var selitem = ques.find(".quesAnswerBox .answer[issel=true]");
    if (selitem.size() < 1) {
        return;
    }
    showResult(ques, String(selitem.attr("correct")).toLowerCase() == "true");
}



