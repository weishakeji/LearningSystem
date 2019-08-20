$(function () {
    //界面初始化
    window.loyout.load();
});
//布局
(function () {
    var loyout = {
        //是否视频全屏，即章节列表是否折叠
        fullScreen: false,
        load: function () {
            this.init();
            this.event();
        },
        //初始化页面布局
        init: function () {
            $("#MainBox").height($(window).height());
            //设置视频播放高度
            var hg = $(window).height() - $("#mainTop").height() - $("#playerInfo").height();
            $("#videobox").height(hg - 70);
            //中间分隔线的高度设置
            $("#median").height($(window).height()).find("span").width($("#median").width() + 2);
            //章节区域高度设置
            $(".itemList").height($(window).height() - $(".boxBar").height() - 30);
            //习题的高度
            $("#Exercises").height($(window).height() - $(".mainTop").height());
            //
            if (loyout.fullScreen) {
                $("#MainBox,#videobox").width($(window).width() * 0.99);
                $("#rightBox,.itemList").width($(window).width() * 0.1);
            } else {
                $("#MainBox,#videobox").width($(window).width() * 0.79);
                $("#rightBox,.itemList").width($(window).width() * 0.19);
            }
            //内容区的大小
            $("#details").height($("#MainBox").height() - $(".mainTop").height() - 20);
            //当前章节高亮显示
            $(".outline .olitem").each(function () {
                var id = $("body").attr("olid");
                var olid = $(this).attr("olid");
                if (id == olid) $(this).addClass("current");
                if (id == "") $(".outline .olitem:eq(0)").addClass("current");
            });
            //是否有视频
            $(".olitem").each(function () {
                var isvideo = $(this).attr("isvideo");
                if (isvideo == "True") $(this).addClass("li-video");
            });
        },
        //事件    
        event: function () {
            //分隔栏事件
            $("#median").click(function () {
                if (!window.loyout.fullScreen) {
                    $("#rightBox").animate({ width: 100 }, 500, function () { $(this).hide(); });
                    $("#MainBox,#videobox").animate({ width: $(window).width() * 0.99 }, 500);
                    $("#btnClose, #btnOpen").toggle();
                } else {
                    $("#MainBox, #videobox").animate({ width: $(window).width() * 0.79 }, 500);
                    $("#rightBox").animate({ width: $(window).width() * 0.2 }, 300, function () { $(this).slideDown(200); });
                    $("#btnClose, #btnOpen").toggle();
                }
                window.loyout.fullScreen = !window.loyout.fullScreen;
            });
            //知识库的按钮
            $("#btnKnowledge").click(function () {
                new top.PageBox('课程知识库', 'Knowledges.ashx?couid=' + $().getPara("couid"), 100, 100, null, window.name).Open();
                return false;
            });
            //设置标题栏的事件
            (function setInitTilte() {
                //取当前状态值               
                var stateCurr = $().getPara("state");
                //当前标题栏
                var currtit = $("a.titBox:first");
                if (stateCurr != "") {
                    $("a.titBox").each(function (index, element) {
                        var href = $(this).attr("href");
                        var rs = new RegExp("(^|)state=([^\&]*)(\&|$)", "gi").exec(href), tmp;
                        var state = 0;
                        if (tmp = rs) state = tmp[2];
                        if (stateCurr == state) currtit = $(this);
                    }).removeClass("titCurr");
                }
                currtit.addClass("titCurr");
                try {
                    var func = eval("setInit_" + state);
                    if ("undefined" != typeof (func) && func != null) func();
                } catch (e) { }

            })();
        }
    };
    window.loyout = loyout;
    $(window).resize(function () {
        window.loyout.init();
    });
})();

var vdata = new Vue({
    data: {
        course: {},      //当前课程
        outline: {},     //当前课程章节
        outlines: [],     //当前课程的章节列表（树形）
        state: {},           //课程状态
        couid: $api.querystring("couid"),
        olid: $api.querystring("olid")
    },
    created: function () {
        var couid = $api.querystring("couid");
        $api.all(
            $api.get("Outline/tree", { couid: couid }),
            $api.get("Course/ForID", { id: couid })).then(axios.spread(
                function (ol, cur) {
                    if (ol.data.success) {
                        vdata.outlines = ol.data.result;
                        if (vdata.olid == '')
                            vdata.olid = ol.data.result[0].Ol_ID;
                        $api.get("course/state", { couid: vdata.couid, olid: vdata.olid }).then(function (req) {
                            if (req.data.success) {
                                vdata.state = req.data.result;
                            }
                        });
                    }
                    if (cur.data.success) {
                        vdata.course = cur.data.result;
                    }
                })
            );
    },
    mounted: function () {
        //alert(3);
    },

});
vdata.$mount('#rightBox');

/*===========================================================================================

视频的播放事件

*/
MsgBox.OverEvent = function () {
    CKobject.getObjectById('ckplayer_videobox').videoPlay();
};
//通过播放时间，激活视频事件
function activeEvent(time) {
    //实际播放的时间值，单位秒
    var s = Math.floor(Number(time));
    //
    $("#events .eventItem").each(function () {
        var point = Number($(this).attr("point"));
        if (point == s) {
            //暂停播放
            CKobject.getObjectById('ckplayer_videobox').videoPause();
            //激出弹出窗口
            var tit = $(this).find(".eventTitle").html();
            var width = Number($(this).attr("winWidth"));
            var height = Number($(this).attr("winHeight"));
            var contx = $(this).find(".eventContext").html();
            var type = Number($(this).attr("type"));
            //如果是提醒或知识展示
            if (type == 1 || type == 2) {
                new MsgBox(tit, contx, width, height, "alert").Open();
            }
            //如果是试题
            if (type == 3) {
                new MsgBox(tit, $(this).html(), width, height, "null").Open();
                $(".MsgBoxContext .eventTitle").remove();
                $(".MsgBoxContext .quesBox .ansItem").click(function () {
                    if ($(this).attr("iscorrect") == "True") {
                        var quesAnd = $(".MsgBoxContext .quesAns");
                        quesAnd.hide();
                        quesAnd.html("&radic; 回答正确！");
                        quesAnd.css("color", "green");
                        quesAnd.show(100);
                        setTimeout("MsgBox.Close()", 1000);
                    } else {
                        var quesAnd = $(".MsgBoxContext .quesAns");
                        quesAnd.hide();
                        quesAnd.html("&times; 回答错误！");
                        quesAnd.css("color", "red");
                        quesAnd.show(100);
                    }
                });
            }
            //如果是实时反馈
            if (type == 4) {
                new MsgBox(tit, $(this).html(), width, height, "null").Open();
                $(".MsgBoxContext .eventTitle").remove();
                $(".MsgBoxContext .quesBox .ansItem").click(function () {
                    var playPoint = Number($(this).attr("point"));
                    CKobject.getObjectById('ckplayer_videobox').videoSeek(playPoint);
                    MsgBox.Close(true);
                });
            }
        }
    });
}