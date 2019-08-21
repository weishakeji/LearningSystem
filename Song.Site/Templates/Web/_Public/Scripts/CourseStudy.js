var vdata = new Vue({
    data: {
        course: {},      //当前课程
        outline: {},     //当前课程章节
        subject: {},     //当前专业
        outlines: [],     //当前课程的章节列表（树形）
        access: {},          //附件列表
        state: {},           //课程状态
        titState: 'none',        //选项卡的状态
        couid: $api.querystring("couid"),
        olid: $api.querystring("olid"),
        median: false     //分隔线折叠状态
    },
    watch: {
        //课程状态
        state: function (val) {
            if (vdata.state.isNull) vdata.titState = 'isNull';
            if (vdata.state.isContext) vdata.titState = 'isContext';
            if (vdata.state.isAccess) vdata.titState = 'isAccess';
            if (vdata.state.isQues) vdata.titState = 'isQues';
            if (vdata.state.existVideo) vdata.titState = 'existVideo';
        }
    },
    methods: {
        //分隔线点击事件
        medianClick: function () {
            vdata.median = !vdata.median;
        },
        //知识库的点击事件
        knlClick:function(){
            new top.PageBox('课程知识库','Knowledges.ashx?couid='+vdata.couid,100,100,null,window.name).Open();
        },
        //附件的点击事件
        accessClick: function (file, tit, event) {
            var exist = file.substring(file.lastIndexOf(".") + 1).toLowerCase();
            if (exist == "pdf") {
                event.preventDefault();
                var box = new PageBox(tit, $().PdfViewer(file), 100, 100, null, window.name);
                box.Open();
            }
            return false;
        },
        //章节列表的点击事件
        outlineClick:function(){

        }
    },
    created: function () {
        var couid = $api.querystring("couid");
        $api.all(
            $api.get("Outline/tree", { couid: couid }),
            $api.get("Course/ForID", { id: couid })).then(axios.spread(function (ol, cur) {
                if (ol.data.success && cur.data.success) {
                    vdata.outlines = ol.data.result;
                    if (vdata.olid == '') vdata.olid = ol.data.result[0].Ol_ID;
                    vdata.course = cur.data.result;
                    //获取当前章节状态，和专业信息
                    $api.all(
                        $api.get("Outline/ForID", { id: vdata.olid }),
                        $api.get("Outline/state", { olid: vdata.olid }),
                        $api.get("Subject/ForID", { id: vdata.course.Sbj_ID })
                    ).then(axios.spread(function (ol, state, subject) {
                        if (ol.data.success && state.data.success && subject.data.success) {
                            vdata.outline = ol.data.result;
                            vdata.state = state.data.result;
                            vdata.subject = subject.data.result;
                            //获取附件
                            $api.get("Outline/Accessory", { uid: vdata.outline.Ol_UID }).then(function (acc) {
                                if (acc.data.success) {
                                    vdata.access = acc.data.result;
                                } else {
                                    if (!acc.data.success) alert("附件列表加载错误");
                                }
                            });
                        } else {
                            if (!ol.data.success) alert("章节信息加载错误");
                            if (!state.data.success) alert("章节状态加载错误");
                            if (!subject.data.success) alert("课程所属专业加载错误");
                        }
                    }));
                } else {
                    alert("章节列表加载错误");
                }
            }));
    },
    mounted: function () {
        //alert(3);
    },

});
vdata.$mount('#body');

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