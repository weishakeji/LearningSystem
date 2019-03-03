$(function () {
    //附件下载,如果是pdf则预览
    mui('body').on('tap', '#access a', function () {
        var href = $(this).attr("href");
        var exist = "";
        if (href.indexOf("?") > -1) href = href.substring(0, href.indexOf("?"));
        if (href.indexOf(".") > -1) exist = href.substring(href.lastIndexOf(".") + 1).toLowerCase();
        if (exist != "pdf") {
            document.location.href = this.href;
        } else {
            var tit = $.trim($(this).text());
            var pdfview = $().PdfViewer(href);
            var box = new PageBox(tit, pdfview, 100, 100);
            if ($(".video-box").height() > 10) $(".video-box").hide();
            $('video').trigger('pause');
            box.CloseEvent = function () {
                if ($(".video-box").height() > 10) {
                    $(".video-box").show();
                    if (CKobject) {	//视频播放
                        var videoObj = CKobject.getObjectById('ckplayer_videobox');
                        if (videoObj) videoObj.videoPlay();
                    }
                }
                //$('video').trigger('play');
            }
            box.Open();
        }
        return false;
    });
    //学习资料内容中的超链接打开
    mui('body').on('tap', '#details a', function () {
        var href = $(this).attr("href");
        var tit = $.trim($(this).text());
        var box = new PageBox(tit, href, 100, 100);
        if ($(".video-box").height() > 10) $(".video-box").hide();
        $('video').trigger('pause');
        box.CloseEvent = function () {
            if ($(".video-box").height() > 10) {
                $(".video-box").show();
                if (CKobject) {	//视频播放
                    var videoObj = CKobject.getObjectById('ckplayer_videobox');
                    if (videoObj) videoObj.videoPlay();
                }
            }
        }
        box.Open();
    });
});





/* 获取观看的累计时间，单位：秒 */
var watchTime = 0;
var setT = null;
//如果你不需要某项设置，可以直接删除，注意var flashvars的最后一个值后面不能有逗号
function loadedHandler() {
	//上次播放进度
    var history = $().cookie("outlineVideo_" + $().getPara('id'));
    $("#historyTime").text(history);
    if (history == null) $(".historyInfo").hide();
	window.setTimeout(function () {
                    $(".historyInfo").fadeOut(500);
                }, 3000);
    if (CKobject.getObjectById('ckplayer_videobox').getType()) {//说明使用html5播放器
        CKobject.getObjectById('ckplayer_videobox').addListener('play', function (b) {
            //setT = window.setInterval(setFunction, 1000);
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

function pausedHandler(b) {
    //if (setT != null) window.clearInterval(setT);
}
function playedHandler(b) {
    setT = window.setInterval(setFunction, 1000);
}
function pausedHandler(b) {
    //if (setT) window.clearInterval(setT);
    //if (!b) setT = window.setInterval(setFunction, 1000);
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
/* 获取观看的累计时间，单位：秒 */
var watchTime = Number($("#studyTime").attr("num"));
watchTime = isNaN(watchTime) ? 0 : watchTime;
//历史递交记录
var p = Math.floor(watchTime / Number($("#totalTime").text()) * 10000) / 100;
var historyLog = isNaN(p) ? 0 : p;
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
    if (!(percent % interval == 0 && (percent > 0 && percent <= 100) && percent > Math.floor(historyLog))) {
        return;
    }
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
                    $(".StudentStudyLog").fadeOut(1000);
                }, 3000);
            });
        },
        error: function () {
            $(".StudentStudyLog").text("学习进度保存失败！ 稍后会再次提交..").addClass("error").show(1000, function () {
                window.setTimeout(function () {
                    $(".StudentStudyLog").fadeOut(1000);
                }, 5000);
            });
        }
    });

}
function timeHandler(t) {
    //播放进度
    if (t > -1) {
        var history = Number($.trim($("#playTime").text()));
        var time = Math.floor(t);
        if (history != time) {
            $("#playTime").text(time);
            //activeEvent(time);  //触发播放事件
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
    //i:'http://www.ckplayer.com/images/loadimg3.jpg',//初始图片地址   
    e: '0', //视频结束后的动作，0是调用js函数，1是循环播放，2是暂停播放并且不调用广告，3是调用视频推荐列表的插件，4是清除视频流并调用js功能和1差不多，5是暂停播放并且调用暂停广告
    p: '1', //视频默认0是暂停，1是播放，2是不加载视频
    loaded: 'loadedHandler', //当播放器加载完成后发送该js函数loaded
    //调用播放器的所有参数列表结束
    //以下为自定义的播放器参数用来在插件里引用的
    my_url: encodeURIComponent(window.location.href)//本页面地址
    //调用自定义播放器参数结束
};
var params = { bgcolor: '#FFF', allowFullScreen: false, allowScriptAccess: 'always', wmode: 'transparent' }; //这里定义播放器的其它参数如背景色（跟flashvars中的b不同），是否支持全屏，是否支持交互
try {
    var video = [playMp4(videoFile) + '->video/mp4'];
    //alert(video);
    CKobject.embedHTML5('videobox', 'ckplayer_videobox', '100%', '260', video, flashvars, ['all']);
} catch (e) {
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
//跳转到历史学习点
$(function () {
    $(".historyInfo").click(function () {
        var history = $.trim($("#historyTime").text());
        CKobject.getObjectById('ckplayer_videobox').videoSeek(history);
    });
});

