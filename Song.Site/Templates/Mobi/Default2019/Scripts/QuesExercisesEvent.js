$(function () {
    //试题事件的初始化
    quesEvent.init();
    //答题卡
    card.init();
    //字体大小的控制
    $().setFontSize($(".quesBox, .quesAnswerContent"));
    $(".fontoper").click(function () {
        if ($(this).attr("id") == "addFont") $().setFontSize($(".quesBox, .quesAnswerContent"), 2);
        if ($(this).attr("id") == "subFont") $().setFontSize($(".quesBox, .quesAnswerContent"), -2);
    });
});
$(function () {
    viewpatt.init();
});
/*
试题相关事件
*/
var quesEvent = {
    init: function () {
        $(".quesItem").each(function () {
            var type = Number($(this).attr("type"));
            var event_func = eval("quesEvent.itemsEvent.type" + type);
            if (event_func != null) event_func($(this));
        });
        this.btnEvent();
    },
    //答题项的点击事件，或填写事件（填空或简答题）
    itemsEvent: {
        type1: function (ansItem) {     //单选题
            ansItem.find(".quesItemsBox>div").click(function () {
                var pat = $(this).parent();
                var isSel = $.trim($(this).attr("isSel")) == "true" ? true : false;
                if (isSel) {
                    $(this).attr("isSel", false).removeClass("selected");
                } else {
                    //如要没有选中
                    pat.find(">div").attr("isSel", false).removeClass("selected");
                    $(this).attr("isSel", true).addClass("selected");
                }
                // 对选择项进行判断，并转到下一题
                if (pat.find(">div[issel=true]").size() > 0) {
                    //提交答题信息以验证对错
                    $(this).parents(".quesItem").find(".btnSubmit").click();
                }
            });
        },
        type2: function (ansItem) {     //多选题
            ansItem.find(".answer").click(function () {
                var isSel = $.trim($(this).attr("isSel")) == "true" ? true : false;
                $(this).attr("isSel", !isSel).toggleClass("selected");
            });
        },
        type3: function (ansItem) {     //判断题
            ansItem.find(".answer").click(function () {
                var isSel = $(this).attr("isSel") == "true" ? true : false;
                if (isSel) {
                    $(this).attr("isSel", false).removeClass("selected");
                } else {
                    //如要没有选中
                    $(this).parent().find(".answer").attr("isSel", false).removeClass("selected");
                    $(this).attr("isSel", true).addClass("selected");
                }
                // 对选择项进行判断，并转到下一题
                if ($(this).parent().find(".answer[issel=true]").size() > 0) {
                    //提交答题信息以验证对错
                    $(this).parents(".quesItem").find(".btnSubmit").click();
                }
            });
        },
        type4: function (ansItem) {     //简答题
            ansItem.find(".answer textarea").focusout(function () {
                var ansInput = $.trim($(this).val());
                if (ansInput != "") {
                    $(this).parents(".quesItem").find(".btnSubmit").click();
                }
            });
        },
        type5: function (ansItem) {     //填空题
            ansItem.find(".answer input[type=text]").focusout(function () {
                var quesItem = $(this).parents(".quesItem");
                var answer = $(this).parent().parent().find(".answer");
                var count = 0;
                answer.each(function () {
                    var ansInput = $.trim($(this).find("input[type=text]").val());
                    if (ansInput != "") count++;
                });
                //提交答题信息以验证对错
                if (count == answer.size()) {
                    $(this).parents(".quesItem").find(".btnSubmit").click();
                }
            });
        }
    },
    //答题是否正确的判断
    ansDecide: {
        type1: function (ques) {   //单选题
            var selitem = ques.find(".quesItemsBox>[issel=true]");
            if (selitem.size() < 1) {
                var msg = new MsgBox("提示", "您还没有答题！", 80, 40, "msg");
                msg.Open();
                return quesEvent.showResult(ques, null);
            }
            return quesEvent.showResult(ques, $.trim(selitem.attr("correct")).toLowerCase() == "true");
        },
        type2: function (ques) {    //多选题
            var selitem = ques.find(".quesItemsBox .answer[issel=true]");
            if (selitem.size() < 1) {
                var msg = new MsgBox("提示", "您还没有答题！<br/><br/><second>3</second>秒后关闭消息", 80, 200, "msg");
                msg.Open();
                return quesEvent.showResult(ques, null);
            }
            if (selitem.size() < 2) {
                var msg = new MsgBox("提示", "多选题至少要选择两个选项！<br/><br/><second>3</second>秒后关闭消息", 80, 200, "msg");
                msg.Open();
                return quesEvent.showResult(ques, null);
            }
            var isCorrect = true;
            var corrItem = ques.find(".quesItemsBox .answer[correct=True]");
            corrItem.each(function () {
                if ($(this).attr("issel") != "true") isCorrect = false;
            });
            return quesEvent.showResult(ques, isCorrect && corrItem.size() == selitem.size());
        },
        type3: function (ques) {    //判断题
            var selitem = ques.find(".quesItemsBox .answer[issel=true]");
            if (selitem.size() < 1) {
                var msg = new MsgBox("提示", "您还没有答题！", 80, 40, "msg");
                msg.Open();
                return quesEvent.showResult(ques, null);
            }
            return quesEvent.showResult(ques, selitem.attr("correct").toLowerCase() == "true");
        },
        type4: function (ques) {   //简答题
            //new MsgBox("提示", "简答题不自动判题！无须提交。", 90, 40, "msg").Open();
            return quesEvent.showResult(ques, false);
        },
        type5: function (ques) {    //填空题
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
            var ques = card.current();
            return quesEvent.showResult(ques, iscorrect);
        }
    },
    //按钮事件
    btnEvent: function () {
        //试题提交
        $(".btnSubmit").click(function () {
            //当前试题所在区域的html对象
            var ques = card.current();
            var type = $.trim(ques.attr("type"));
            var func = eval("quesEvent.ansDecide.type" + type);
            if (func != null) func(ques);
            //如果答题正确，则直接转到下一题
            if (ques.attr("ansstate") == "true") quesEvent.move(ques, 1);
            //计算正确率
            var sum = $("#cardBox").find("dd.error,dd.succ").size();
            var suss = $("#cardBox dd.succ").size();
            if (sum > 0) {
                var per = sum > 0 ? Math.floor(suss / sum * 10000) / 100 : 0;
                $(".correct-rate").text(per); //正确率
            }
            //正确的答题数，与错误的答题数
            $(".correct-num").text($("#cardBox").find("dd.succ").size());
            $(".error-num").text($("#cardBox").find("dd.error").size());
            //记录答题状态到本地
            if (typeof state != "undefined") state.update(ques);

        });
        //收藏
        $(".btnFav").click(function () {
            if (!isLogin) {
                //如果没有登录
                new MsgBox("提示", "未登录状态，不可以收藏试题。", 70, 40, "alert").Open();
            } else {
                var ques = card.current();
                var isCollect = ques.attr("IsCollect") == "True" ? true : false;
                $.get("AddCollect.ashx", { "qid": card.currid(), "couid": card.couid(), "isCollect": isCollect }, function () {
                    var ques = card.current();
                    ques.attr("IsCollect", isCollect ? "False" : "True");
                    ques.find(".btnFav").toggleClass("IsCollect");
                    var txt = isCollect ? "取消收藏成功" : "添加收藏成功";
                    var msg = new MsgBox("提示", txt + "！<br/><br/><second>2</second>秒后关闭消息", 70, 200, "msg");
                    msg.Open();
                });
            }
        });
        //笔记
        $(".btnNote").click(function () {
            if (!isLogin) {
                //如果没有登录
                new MsgBox("提示", "未登录状态，不可以编写笔记。", 70, 40, "alert").Open();
            } else {
                var qid = card.currid();
                new PageBox("添加笔记", "QuesNoteAdd.ashx?qid=" + qid, 70, 50, null).Open();
            }
        });
        //报错
        $(".btnError").click(function () {
            var qid = card.currid();
            new PageBox("错误试题提交", "QuesSubmitError.ashx?id=" + qid, 70, 80, null).Open();
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
    },
    //显示答题的正确与否状态
    //ques:试题区域
    //isCorrect:是否正确
    showResult: function (ques, isCorrect) {
        var qitem = ques.find(".quesBox");
        if (isCorrect == null) {
            qitem.removeClass("error").removeClass("correct");
            return false;
        }
        ques.attr("ansstate", isCorrect);
        var qid = ques.attr("qid");
        //如果正确
        if (isCorrect) {
            qitem.addClass("succ").removeClass("error");
            //设置答题卡状态
            card.set("succ", qid);
        } else {
            //如果错误
            qitem.removeClass("succ").addClass("error");
            //增加错题到个人记录
            $.get("AddQues.ashx", { "qid": qid, "couid": card.couid() }, function () { });
            //设置答题卡状态，并显示答案
            card.set("error", qid);
            $(".quesItem[qid=" + qid + "]").find(".quesAnswerBox").show();
        }
        return isCorrect;
    },
    //试题移动
    //quesitem:试题区域
    //dirt:方向，1为向右，-1为向左
    move: function (quesitem, dirt) {
        var direction = dirt == -1 ? "right" : "left";
        finger.slide(null, direction, null, null, 1);
    }
};

/*
生成答题卡
*/
var card = {
    init: function () {
        //试题类型
        var types = this.getQuesTypes();
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
        $("#cardBox").click(function () {
            $("#cardPanel").hide();
        });
        box.find("dl dd").click(function () {
            var index = $(this).attr("index");
            var fixLeft = (index - 1) * $(".quesItem").width();
            $("#quesArea").css({ left: -fixLeft });
            //
            var index = Math.round(Math.abs(fixLeft) / $(".quesItem").width());
            $("#indexNum").attr("index", index + 1);
            $("#indexNum").text(Number($("#indexNum").attr("initIndex")) + index);
            var qitem = $(".quesItem[index=" + (index + 1) + "]");
            $("#cardPanel").hide();
            card.set("curr");
        });
    },
    //获取试题类型
    getQuesTypes: function () {
        var quesTypes = $("body").attr("questype").split(",");
        var types = new Array();
        var qus = $(".quesItem");
        qus.each(function () {
            var type = Number($(this).attr("type"));
            var name = $.trim(quesTypes[type - 1]);
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
    },
    //获取试题html元素对象
    current: function (qid) {
        if (qid == null) {
            var ques = $(".quesItem[index=" + $("#indexNum").attr("index") + "]");
            return ques;
        }
        return $(".quesItem[qid=" + qid + "]");
    },
    //获取当前试题的ID
    currid: function () {
        var ques = this.current();
        if (ques.size() > 0) return Number(ques.attr("qid"));
        return 0;
    },
    //当前课程id
    couid: function () {
        return $().getPara("couid");
    },
    //设置答题卡状态
    //state:curr当前试题，succ正确，error错误,null默认状态
    //qid:试题的id
    set: function (state, qid) {
        var index = 0;
        //如果没有指明试题id，则取当前显示的试题
        if (qid == null) {
            var qitem = card.current();
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
            if (!box.hasClass("error")) {
                box.removeAttr("class").addClass("succ");
            }
        }
        if (state == "error") {
            box.removeAttr("class").addClass("error");
        }
        //记录答题状态到本地
        if (typeof window.state != "undefined") window.state.update($(".quesItem[qid=" + qid + "]"));
        //记录学习进度
        //        $.get("/ajax/LogForStudentQuestions.ashx",
        //            { acid: acid,
        //                couid: $().getPara("couid"),
        //                olid: $().getPara("olid"),
        //                qid: qid,
        //                index: index
        //            }, function () {
        //            });
    },
    //移除一个试题
    remove: function (qid) {
        card.current().fadeOut(2000, function () {
            card.current().remove();
            $(".quesItem").each(function (index, element) {
                $(this).attr("index", index + 1).find(".index").text(index + 1);
            });
            //移除答题卡中对应的信息
            $("#cardBoxInner dd[qid=" + qid + "]").remove();
            $("#cardBoxInner dd").each(function (index, element) {
                $(this).attr("index", index + 1).text(index + 1);
            });
            card.set("curr");
            //设置索引与总数
            var ques = card.current();
            var index = ques.attr("index");
            $("#indexNum").attr("index", index).text(index);
            $("#Total").text($(".quesItem").size());
        });


    },
    //清空所有试题
    clear: function () {
        $(".context").remove();
        $("#cardBoxInner dd").remove();
        $(".ctlBtn").remove();
    },
    //试题总个数
    size: function () {
        return $(".quesItem").size();
    }
}

/* 
手式滑动事件
*/
var finger = {
    //初始化手式事件，实现左右滑动切换试题
    init: function () {
        $(".context").swipe({ fingers: 'all', swipeLeft: finger.slide, swipeRight: finger.slide,
            pinchIn: finger.pinch, pinchOut: finger.pinch
        });
        //        //设置初始的题型
        //        var qid = $().getPara("qid");
        //        //设置初始显示的试题
        //        var firstQitem = qid != "" ? $(".quesItem[qid=" + qid + "]") : $(".quesItem:first");
        //        firstQitem = firstQitem.size() > 0 ? firstQitem : $(".quesItem:first");
        //        finger.qusmove((Number(firstQitem.attr("index")) - 1) * $(".quesItem").width());
        card.set("curr");
    },
    //放大与捏合事件
    pinch: function (event, direction, distance, duration, fingerCount) {
        if (direction == "in") $().setFontSize($(".quesBox, .quesAnswerContent"), 2);   //放大
        if (direction == "out") $().setFontSize($(".quesBox, .quesAnswerContent"), -2);    //捏合
    },
    //手式滑动
    slide: function (event, direction, distance, duration, fingerCount) {
        if (fingerCount > 1) return;
        var fixLeft = Number($("#quesArea").css("left").replace("px", ""));
        fixLeft = isNaN(fixLeft) ? 0 : fixLeft;
        var tm = $(".quesItem").width();
        var second = "<br/><br/><second>3</second>秒后关闭消息";
        if (direction == "left") {
            if (Math.abs(fixLeft) < tm * ($(".quesItem").size() - 1)) {
                fixLeft = fixLeft - tm;
            } else {
				var num=Number($(".correct-num").text())+Number($(".error-num").text());
				var per=$(".correct-rate").text();
				var sum=Number($("#Total").text());
				var txt="已经练习"+num+"道，正确率"+per+"%";
				if((sum-num)>0){
					txt+="，还有"+(sum-num)+"道没有练习";
				}
                var msg=new MsgBox("提示", "已经是最后一道试题，是否返回？</br/>"+txt+"。"
						+"<br/><br/><second>10</second>秒后关闭消息", 70, 250, "confirm");
				msg.EnterEvent=function(){
					var couid=$().getPara("couid");
					window.location.href="QuesOutlines.ashx?couid="+couid;
				}
				msg.Open();
            }
        }
        if (direction == "right") {
            if (fixLeft < 0) {
                fixLeft = fixLeft + tm;
            } else {
                new MsgBox("提示", "这是第一道试题，前面没有了！" + second, 70, 200, "msg").Open();
            }
        }
        finger.qusmove(fixLeft);
    },
    //移动试题
    qusmove: function (fixLeft) {
        $("#quesArea").animate({ left: fixLeft }, function () {
            var index = Math.round(Math.abs(fixLeft) / $(".quesItem").width());
            index = isNaN(index) ? -1 : index;
            $("#indexNum").attr("index", index + 1);
            $("#indexNum").text(Number($("#indexNum").attr("initIndex")) + index);
            var qitem = $(".quesItem[index=" + (index + 1) + "]");
            $("#quesArea").css("left", -index * $(".quesItem").width());
            //当前试题的答题卡选块
            card.set("curr");
        });
    }
};

/* 
视图模式
*/
var viewpatt = {
    init: function () {
        this.loyout();
        this.event();
        //初始化视图模式按钮
        var patter = viewpatt.storage.get();
        var first = $("#boxSetup dl.view-pattern dd[id=" + patter.patter + "]");
        first.addClass("view-selected");
        viewpatt.style(first.attr("id"));
    },
    loyout: function () {
        var html = "<div id='boxSetup' ><dl class='view-pattern'>";
        html += "<dd id='patt1'><b></b>护眼模式</dd><dd id='patt2' default='true'><b></b>日间模式</dd><dd id='patt3'><b></b>夜间模式</dd>";
        html += "</dl></div><div id='maskSetup'></div>";
        $("body").append(html);
    },
    event: function () {
        //打开设置项
        $(".btnSetup").click(function () {
            var mask = $("#maskSetup"); //遮罩层
            mask.css({ height: $(window).height() }).show();
            var box = $("#boxSetup");   //设置项的区域
            box.css({ left: $(window).width() }).show();
            box.animate({ left: 0 });
			$("body>*[id!=boxSetup]").addClass("blur");
        });
        //关闭设置项
        $("#maskSetup").click(function () {
            $("#boxSetup").animate({ left: $(window).width() }, function () {
                $(this).hide();
                $("#maskSetup").fadeOut(100);
            });
			$("body>*[class!=MsgBox]").removeClass("blur");
        });
        /*视图模式的点击事件*/
        $("#boxSetup dl.view-pattern dd").click(function () {
            $(this).parent().find("dd").removeClass("view-selected");
            $(this).addClass("view-selected");
            viewpatt.storage.set($(this).attr("id"));
            //设置颜色风格
            viewpatt.style($(this).attr("id"));
			$("body>*[class!=MsgBox]").removeClass("blur");
        });
    },
    //将设置存储到本地
    storage: {
        set: function (patter) {
            var obj = { patter: patter, time: new Date().getTime() };
            window.storage("view-pattern", obj);
        },
        get: function () {
            var first = $("#boxSetup dl.view-pattern dd[default]"); //默认值
            var obj = window.storage("view-pattern");
            //如果不存则，则取默认值
            if (obj == null || obj instanceof String)
                obj = { patter: first.attr("id"), time: new Date().getTime() };
            //如果设置记录在六个小时之前，则取默认值
            obj.time = new Date(obj.time);
            var span = parseInt(new Date().getTime() - obj.time.getTime()) / 1000 / 3600;
            if (span >= 6) obj = { patter: first.attr("id"), time: new Date().getTime() };
            return obj;
        }
    },
    //设置风格
    style: function (patt) {
        //背景色：1.主景背,2.块级元素,3.输入框,4.按钮色
        var color_patt1 = { bg: "#E8FFE8", box: "#C7EDCC", input: "#E8FFE8", font: "#000", btn: "#060", btntxt: "#E8FFE8" };
        var color_patt2 = { bg: "#efeff4", box: "#fff", input: "#fff", font: "#999", btn: "#1B9AF7", btntxt: "#fff" };
        var color_patt3 = { bg: "#333", box: "#999", input: "#ccc", font: "#000", btn: "#333", btntxt: "#ccc" };
        var color = eval("color_" + patt);
        //背景
        $("body").attr("class", patt).css("background-color", color.bg);
        $(".quesAnswerBox, .quesBox, .notebox").css("background-color", color.box);
        $(".ctlBtn, .topBpx, #noQues").css("background-color", color.box);
        $("#boxSetup, maskSetup").css("background-color", color.box);
        $("#cardBox").css("background-color", color.box);
        $(".quesItemsBox").find("input,textarea").css("background-color", color.input);
        $(".MsgBox").css("background-color", color.box);
        //字体
        $(".topBpx *, .item").css("color", color.font);
        //按钮
        $(".btnSubmit").css({ "background-color": color.btn,
            "border-color": color.btn,
            "color": color.btntxt
        });
    }
};