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
            $("#median").height($(window).height()).css("line-height", $(window).height() + "px");
            //章节区域高度设置
            $(".itemList").height($(window).height() - $(".boxBar").height() - 30);
            //习题的高度
            $("#Exercises").height($(window).height() - $(".mainTop").height());
            //
            if (loyout.fullScreen) {
                $("#MainBox,#videobox").width($(window).width() * 0.99);
                $("#outlineBox,.itemList").width($(window).width() * 0.1);
            } else {
                $("#MainBox,#videobox").width($(window).width() * 0.79);
                $("#outlineBox,.itemList").width($(window).width() * 0.19);
            }
            //内容区的大小
            $("#details").height($("#MainBox").height() - $(".mainTop").height() - 20);
            //当前章节高亮显示
            $(".outline .olitem").each(function () {
                var id = $().getPara("id");
                var olid = $(this).attr("olid");
                if (id == olid) $(this).addClass("current");
                if (id == "") $(".outline .olitem:eq(0)").addClass("current");
            });
        },
        //事件    
        event: function () {
            //分隔栏事件
            $("#median").click(function () {
                if (!window.loyout.fullScreen) {
                    $("#outlineBox").animate({ width: 100 }, 500, function () { $(this).hide(); });
                    $("#MainBox,#videobox").animate({ width: $(window).width() * 0.99 }, 500);
                    $("#btnClose, #btnOpen").toggle();
                } else {
                    $("#MainBox, #videobox").animate({ width: $(window).width() * 0.79 }, 500);
                    $("#outlineBox").animate({ width: $(window).width() * 0.2 }, 300, function () { $(this).slideDown(200); });
                    $("#btnClose, #btnOpen").toggle();
                }
                window.loyout.fullScreen = !window.loyout.fullScreen;
            });
            //设置标题栏的事件
            (function setInitTilte() {
                //取当前状态值
                var box = $("div[statebox]").first();
                var stateCurr = box.attr("statebox");
                //标题栏的项
                var tit = $("a.titBox");
                tit.removeClass("titCurr");
                tit.each(function () {
                    var href = $(this).attr("href");
                    var rs = new RegExp("(^|)state=([^\&]*)(\&|$)", "gi").exec(href), tmp;
                    var state = 0;
                    if (tmp = rs) state = tmp[2];
                    if (stateCurr == state) {
                        $(this).addClass("titCurr");
                        try {
                            var func = eval("setInit_" + state);
                            if ("undefined" != typeof (func) && func != null) func();
                        } catch (e) { }
                    }
                });
            })();
        }
    };
    window.loyout = loyout;
    $(window).resize(function () {
        window.loyout.init();
    });
})();
//习题界面的初始化
function setInit_4() {
    var frame = $("#Exercises");
    frame.height($(window).height() - $(".mainTop").height());
}

//如果你不需要某项设置，可以直接删除，注意var flashvars的最后一个值后面不能有逗号
function loadedHandler() {
    //上次播放进度
    var history = $().cookie("outlineVideo_" + $().getPara('id'));
    $("#historyTime").text(history);
    if (history == null) $(".historyInfo").hide();
    if (CKobject.getObjectById('ckplayer_videobox').getType()) {//说明使用html5播放器
        //CKobject.getObjectById('ckplayer_videobox').addListener('paused', pausedHandler);
        //alert(CKobject.getObjectById('ckplayer_videobox').innerHTML);
        //CKobject.getObjectById('ckplayer_videobox').addListener('time', timeHandler);
        CKobject.getObjectById('ckplayer_videobox').addListener('play', function (b) {
            //setT = window.setInterval(setIntervalFunction, 1000);
        });
        CKobject.getObjectById('ckplayer_videobox').addListener('pause', function (b) {
            //if (setT != null) window.clearInterval(setT);
        });
        CKobject.getObjectById('ckplayer_videobox').addListener('time', timeHandler);
    }
    else {
        CKobject.getObjectById('ckplayer_videobox').addListener('paused', 'pausedHandler');
        CKobject.getObjectById('ckplayer_videobox').addListener('time', 'timeHandler');
    }
}
/*当窗体失去焦点就停止播放，得到焦点继续播放，如果失去焦点前已经暂停，则保持原状态*/
$(window).blur(function () {
    try {
        CKobject.getObjectById('ckplayer_videobox').videoPause();
    } catch (e) { }
});
$(window).focus(function () {
    //CKobject.getObjectById('ckplayer_videobox').videoPlay();
    //如果视频事件的弹窗存在，则不做其它动作。
    if (MsgBox.IsExist) return;
    try {
        var id = $().getPara('id');
        CKobject.getObjectById('ckplayer_videobox').videoSeek(Number($().cookie("outlineVideo_" + id)));
    } catch (e) { }

});
/* 获取观看的累计时间，单位：秒 */
var watchTime = Number($("#studyTime").attr("num"));
watchTime = isNaN(watchTime) ? 0 : watchTime;
//历史递交记录
var p = Math.floor(watchTime / Number($("#totalTime").text()) * 10000) / 100;
var historyLog = isNaN(p) ? 0 : p;
//var setT = null;
function pausedHandler(b) {
    //if (setT) window.clearInterval(setT);
    //if (!b) setT = window.setInterval(setIntervalFunction, 1000);
}
//获取视频总时长
function getTotal() {
    //获取视频时长
    var dura = CKobject._K_('totalTime').innerHTML;
    if (Number(dura) < 1) {
        var a = CKobject.getObjectById('ckplayer_videobox').getStatus();
        var total = '';
        for (var k in a) {
            if (k == 'totalTime') total = a[k];
        }
        CKobject._K_('totalTime').innerHTML = total;
    }
    return Number(dura);
}
function setIntervalFunction() {
    //设置学习时间数值显示
    CKobject._K_('studyTime').innerHTML = Math.floor(watchTime);
    var total = getTotal(); //总时长
    //提交数据
    //完成度的百分比
    var percent = Math.floor(Number($("#per").text()));
    var interval = 2; 	//间隔百分比多少递交一次记录
    if (Number(total) <= 5 * 60) interval = 10; //5分钟内
    else if (Number(total) <= 10 * 60) interval = 5;
    if (percent % interval == 0 && (percent > 0 && percent <= 100) && percent > Math.floor(historyLog)) {
        
    
	historyLog += interval;
    $.ajax({
        url: "/Ajax/StudentStudy.ashx",
        data: { couid: couid, olid: olid, studyTime: watchTime,
            playTime: Number($("#playTime").html().trim()) * 1000,
            totalTime: Number($("#totalTime").html().trim()) * 1000
        },
        success: function (data) {            
            var d = Number(data);
            var show = $(".StudentStudyLog");
            if (d > 0) show.text("学习进度（" + d + "%）提交成功!").removeClass("error");
            if (d == -1) show.text("学员未登录，或登录失效").addClass("error");
            show.show(1000, function () {
                window.setTimeout(function () {
                    $(".StudentStudyLog").hide(500);
                }, 3000);
            });
        },
        error: function () {
            $(".StudentStudyLog").text("学习进度保存失败！ 稍后会再次提交..").addClass("error").show(1000, function () {
                window.setTimeout(function () {
                    $(".StudentStudyLog").hide(500);
                }, 5000);
            });
        }
    });
}
}
function timeHandler(t) {
    //播放进度
    if (t > -1) {
        var history = Number($.trim($("#playTime").text()));
        var time = Math.floor(t);
        if (history != time) {
            $("#playTime").text(time);
            activeEvent(time);  //触发播放事件
            watchTime++;    //观看时间加-
            //完成度的百分比
            var p = Math.floor(watchTime / Number($("#totalTime").text()) * 10000) / 100;
            $("#per").text(p);
            setIntervalFunction(); //向服务器提交学习记录
            //记录本地播放进度
            var id = $().getPara('id');
            $().cookie("outlineVideo_" + id, time);
        }
    }
}
//加载视频播放器
var flashvars = {
    f: videoPath, //视频地址
    a: '', //调用时的参数，只有当s>0的时候有效
    s: '0', //调用方式，0=普通方法（f=视频地址），1=网址形式,2=xml形式，3=swf形式(s>0时f=网址，配合a来完成对地址的组装)
    c: '0', //是否读取文本配置,0不是，1是
    x: '', //调用配置文件路径，只有在c=1时使用。默认为空调用的是ckplayer.xml
    //i:'http://www.ckplayer.com/images/loadimg3.jpg',//初始图片地址
    //d:'http://www.ckplayer.com/down/pause6.1_1.swf|http://www.ckplayer.com/down/pause6.1_2.swf',//暂停时播放的广告，swf/图片,多个用竖线隔开，图片要加链接地址，没有的时候留空就行
    u: '', //暂停时如果是图片的话，加个链接地址
    //l:'http://www.ckplayer.com/down/adv6.1_1.swf|http://www.ckplayer.com/down/adv6.1_2.swf',//前置广告，swf/图片/视频，多个用竖线隔开，图片和视频要加链接地址
    r: '', //前置广告的链接地址，多个用竖线隔开，没有的留空
    t: '10|10', //视频开始前播放swf/图片时的时间，多个用竖线隔开
    y: '', //这里是使用网址形式调用广告地址时使用，前提是要设置l的值为空
    z: 'down/buffer.swf', //缓冲广告，只能放一个，swf格式
    e: '0', //视频结束后的动作，0是调用js函数，1是循环播放，2是暂停播放并且不调用广告，3是调用视频推荐列表的插件，4是清除视频流并调用js功能和1差不多，5是暂停播放并且调用暂停广告
    v: '80', //默认音量，0-100之间
    p: '1', //视频默认0是暂停，1是播放，2是不加载视频
    h: '0', //播放http视频流时采用何种拖动方法，=0不使用任意拖动，=1是使用按关键帧，=2是按时间点，=3是自动判断按什么(如果视频格式是.mp4就按关键帧，.flv就按关键时间)，=4也是自动判断(只要包含字符mp4就按mp4来，只要包含字符flv就按flv来)
    q: '', //视频流拖动时参考函数，默认是start
    m: '', //让该参数为一个链接地址时，单击播放器将跳转到该地址
    o: '', //当p=2时，可以设置视频的时间，单位，秒
    w: '', //当p=2时，可以设置视频的总字节数
    g: '', //视频直接g秒开始播放
    j: '', //跳过片尾功能，j>0则从播放多少时间后跳到结束，<0则总总时间-该值的绝对值时跳到结束
    //k:'30|60',//提示点时间，如 30|60鼠标经过进度栏30秒，60秒会提示n指定的相应的文字
    //n:'这是提示点的功能，如果不需要删除k和n的值|提示点测试60秒',//提示点文字，跟k配合使用，如 提示点1|提示点2
    wh: '', //宽高比，可以自己定义视频的宽高或宽高比如：wh:'4:3',或wh:'1080:720'
    lv: '0', //是否是直播流，=1则锁定进度栏
    loaded: 'loadedHandler', //当播放器加载完成后发送该js函数loaded
    //调用播放器的所有参数列表结束
    //以下为自定义的播放器参数用来在插件里引用的
    my_url: encodeURIComponent(window.location.href)//本页面地址
    //调用自定义播放器参数结束
};
var params = { bgcolor: '#FFF', allowFullScreen: false, allowScriptAccess: 'always', wmode: 'transparent' }; //这里定义播放器的其它参数如背景色（跟flashvars中的b不同），是否支持全屏，是否支持交互
var video = [playMp4("undefined" != typeof videoPath ? videoPath : "") + '->video/mp4'];
if ($("#videobox").size() > 0) {
    //CKobject.embed('/Utility/Ckplayer/ckplayer/ckplayer.swf', 'videobox', 'ckplayer_videobox', '100%', '100%', false, flashvars, video, params);
    CKobject.embedHTML5('videobox', 'ckplayer_videobox', '100%', '100%', video, flashvars, ['all']);
}
//播放flv格式的同名mp4视频
function playMp4(vname) {
    var vfile = vname;
    if (!isOuter) {  //如果是外部链接
        //vname = window.BASE64.decoder(vname);
        if (vname.indexOf('.' > -1)) {
            var exist = vname.substring(vname.lastIndexOf('.') + 1);
            if (exist != "aspx") {
                vname = vname.substring(0, vname.lastIndexOf('.'));
                vfile = vname + ".mp4";
            }
        }
        $("#loadvideo").attr("href", vfile);
        $("#loadvideo").attr("download", vfile);
    } else {
        $("#loadvideo").attr("href", vname);
        $("#loadvideo").attr("download", vname);
    }
    return vfile;
}
/*
以上代码演示的兼容flash和html5环境的。如果只调用flash播放器或只调用html5请看其它示例
*/
//开关灯
var box = new LightBox();
function closelights() {//关灯
    box.Show();
    CKobject._K_('videobox').style.width = '440px';
    CKobject._K_('videobox').style.height = '550px';
    CKobject.getObjectById('ckplayer_videobox').width = 680;
    CKobject.getObjectById('ckplayer_videobox').height = 400;
}
function openlights() {//开灯
    box.Close();
    CKobject._K_('videobox').style.width = '680px';
    CKobject._K_('videobox').style.height = '400px';
    CKobject.getObjectById('ckplayer_videobox').width = 680;
    CKobject.getObjectById('ckplayer_videobox').height = 400;
}
function playerstop() {
    //只有当调用视频播放器时设置e=0或4时会有效果
    //播放完成后自动跳转到下一节
    var curr = $("div.itemList div.current");
    var next = curr.next().next();
    if (next.size() < 1) {
        next = curr.parent().next(".olitem");
    }
    if (next.size() > 0) {
        var a = next.find("a");
        if (a.size() > 0) {
            var href = a.attr("href");
            window.location.href = href;
        }
    }
}
//跳转到历史学习点
$(function () {
    $(".historyInfo").click(function () {
        var history = $.trim($("#historyTime").text());
        CKobject.getObjectById('ckplayer_videobox').videoSeek(history);
    });
});

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