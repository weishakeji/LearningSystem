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

                }
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
                //视频播放
                //.........               
            }
        }
        box.Open();
    });
});

var vdata = new Vue({
    data: {
        outline: {},    //当前课程章节
        messages: [],        //咨询留言
        state: {},           //课程状态  
        couid: $api.querystring("couid"),
        olid: $api.querystring("olid"),
        //当前章节的视频信息
        video: {
            url: '',         //视频路径
            total: 0,        //总时长      
            playTime: 0,         //当前播放时间，单位：毫秒     
            playhistime: 0,    //历史播放时间
            studytime: 0,    //累计学习时间
            percent: 0       //完成度（百分比）
        },
        playtime: 0,         //当前播放时间，单位：秒
        studylogUpdate: false,           //学习记录是否在递交中
        studylogState: 0,            //学习记录提交状态，1为成功，-1为失败
        switchSubtitle: $api.cookie("switchSubtitle_" + $api.querystring("couid")) == "true" ? true : false,               //弹幕开关
        //控件
        player: null             //播放器
    },
    watch: { //课程状态
        state: function (val) {
            //视频播放
            if (vdata.state.existVideo || vdata.state.isLive)
                vdata.videoPlay(vdata.state);
        },
        //播放进度变化
        playtime: function (val) {
            vdata.video.studytime++;
            //学习记录提交
            vdata.videoLog();
            //触发视频事件
            //vdata.videoEvent(vdata.playtime);
        }
    },
    methods: {
        //播放器是否准备好
        playready: function () {
            if (vdata.player != null) {
                return vdata.player._isReady && vdata.player.engine;
            }
            return false;
        },
        //视频播放跳转
        videoSeek: function (second) {
            if (vdata.playready()) vdata.player.seek(second);
        },
        //视频开始播放
        videoPlay: function (state) {
            if (vdata.player != null) {
                vdata.player.destroy();
                vdata.player = null;
            }
            if (!vdata.state.isLive) {  //点播
                vdata.player = new QPlayer({
                    url: state.urlVideo,
                    container: document.getElementById("videoplayer"),
                    autoplay: true,
                });
            } else { //直播
                var u = navigator.userAgent, app = navigator.appVersion;
                var isAndroid = u.indexOf('Android') > -1 || u.indexOf('Linux') > -1; //g
                var isIOS = !!u.match(/\(i[^;]+;( U;)? CPU.+Mac OS X/); //ios终端
                vdata.player = new QPlayer({
                    url: state.urlVideo,
                    container: document.getElementById("livebox"),
                    isLive: !isIOS,
                    autoplay: true
                });
            }
            if (vdata.player != null) {
                vdata.player.on("loading", vdata.videoready);
                vdata.player.on("ready", vdata.videoready);
                vdata.player.on("timeupdate", function (currentTime, totalTime) {
                    vdata.video.total = parseInt(totalTime);
                    vdata.video.playTime = currentTime;   //详细时间，精确到毫秒
                    vdata.playtime = parseInt(currentTime);
                    //学习完成度，最大为百分百
                    vdata.video.percent = Math.floor(vdata.video.studytime <= 0 ? 0 : vdata.video.studytime / vdata.video.total * 1000) / 10;
                    vdata.video.percent = vdata.video.percent > 100 ? 100 : vdata.video.percent;
                });
                vdata.player.on("seeked", function () {
                    vdata.playtime = vdata.playready() ? vdata.player.currentTime : 0;
                    window.setTimeout(function () {
                        if (vdata.playready()) vdata.player.pause();
                    }, 50);

                });
            }
        },
        //播放器加载后的事件
        videoready: function () {
            //隐藏全屏按钮
            var fullbtn = document.getElementsByClassName("qplayer-fullscreen");
            for (var i = 0; i < fullbtn.length; i++) {
                //fullbtn[i].style.display = "none";
            }
            //隐藏设置按钮(播放倍速也禁用了)
            var setbtn = document.getElementsByClassName("qplayer-settings-btn");
            for (var i = 0; i < setbtn.length; i++) {
                //setbtn[i].style.display = "none";
            }
        },
        //章节列表的点击事件
        outlineClick: function (olid, event) {
            var url = $api.setpara("olid", olid);
            history.pushState({}, null, url);
            vdata.olid = olid;
            if (event != null) event.preventDefault();
            //获取当前章节状态，和专业信息
            $api.all(
                $api.get("Outline/ForID", { id: olid }),
                $api.get("Outline/state", { olid: olid })
            ).then(axios.spread(function (ol, state) {
                if (ol.data.success && state.data.success) {
                    vdata.outline = ol.data.result;
                    vdata.state = state.data.result;
                    if (!vdata.state.isLive && vdata.state.PlayTime > 0) {
                        if (window.confirm("是否从上次进度播放？")) {
                            vdata.videoSeek(vdata.state.PlayTime / 1000);
                            window.setTimeout(function () {
                                if (vdata.playready()) vdata.player.play();
                            }, 500);
                        }
                    }
                    //视频播放记录
                    var result = state.data.result;
                    vdata.video.studytime = isNaN(result.StudyTime) ? 0 : result.StudyTime;
                    vdata.video.playhistime = isNaN(result.PlayTime) ? 0 : result.PlayTime / 1000;
                    window.setTimeout(function () {
                        vdata.outlineLoaded = true;
                    }, 100);
                } else {
                    if (!ol.data.success) alert("章节信息加载异常！详情：\r" + ol.data.message);
                    if (!state.data.success) alert("章节状态加载异常！详情：\r" + state.data.message);
                }
            }));
            //获取留言列表
            //vdata.msgGet();
        },
        //学习记录记录
        videoLog: function () {
            if (vdata.studylogUpdate) return;
            var interval = 1; 	//间隔百分比多少递交一次记录
            if (vdata.video.total <= 5 * 60) interval = 10; //5分钟内
            else if (vdata.video.total <= 10 * 60) interval = 5;
            var per = Math.floor(vdata.video.studytime <= 0 ? 0 : vdata.video.studytime / vdata.video.total * 1000) / 10;
            if (per > 0 && per < (100 + interval) && per % interval == 0) {
                $api.post("Course/StudyLog", {
                    couid: vdata.outline.Cou_ID, olid: vdata.outline.Ol_ID,
                    playTime: vdata.playtime, studyTime: vdata.video.studytime, totalTime: vdata.video.total
                }, function () {
                    vdata.studylogUpdate = true;
                }, function () {
                    vdata.studylogUpdate = false;
                }).then(function (req) {
                    if (!req.data.success) throw req.data.message;
                    vdata.studylogState = 1;
                    window.setTimeout(function () {
                        vdata.studylogState = 0;
                    }, 2000);
                }).catch(function (err) {
                    vdata.studylogState = -1;
                    alert(err);
                    window.setTimeout(function () {
                        vdata.studylogState = 0;
                    }, 2000);
                });
            }
        },
        //发送消息
        msgSend: function () {
            vdata.msgBlur();
            var msg = document.getElementById("messageinput").value;
            if ($api.trim(msg) == '') return;
            var span = Date.now() - Number($api.cookie("msgtime"));
            if (span / 1000 < 10) {
                vdata.$notify({
                    message: '不要频繁发消息！',
                    position: 'bottom-right'
                });
                return;
            }
            $api.cookie("msgtime", Date.now());
            $api.post("message/add", { msg: msg, playtime: vdata.playtime, couid: vdata.couid, olid: vdata.olid }).then(function (req) {
                var d = req.data;
                if (d.success) {
                    var input = document.getElementById("messageinput");
                    input.value = '';
                    vdata.msgGet();
                } else {
                    alert("信息添加发生异常！详情：\r" + d.message);
                }
            });
        },
        //留言输入框失去焦点
        msgBlur: function (e) {
            document.getElementById("footer").style.display = "table";
            document.getElementById("messageinput").blur();
        },
        //留言输入框获取焦点
        msgFocus: function (e) {
            document.getElementById("footer").style.display = "none";
            document.getElementById("messageinput").focus();
        },
        //获取当前章节的留言信息
        msgGet: function () {
            if (!vdata.olid || vdata.olid < 1) return;
            $api.post("message/All", { olid: vdata.olid, order: 'desc' }).then(function (req) {
                var d = req.data;
                if (d.success) {
                    vdata.messages = d.result;
                    window.setTimeout(function () {
                        var dl = document.getElementById("chatlistdl");
                        document.getElementById("chatlist").scrollTop = dl.offsetHeight;
                    }, 1000);
                } else {
                    throw "留言信息加载异常！详情：\r" + d.message;
                }
            }).catch(function (err) {
                //alert(err);
            });
        }
    },
    created: function () {
        var couid = $api.querystring("couid");
        $api.all(
            $api.get("Outline/tree", { couid: couid }),
            $api.get("Course/ForID", { id: couid })).then(axios.spread(function (ol, cur) {
                if (ol.data.success && cur.data.success) {
                    vdata.outlines = ol.data.result;
                    if (vdata.olid == '' || vdata.olid == null) vdata.olid = ol.data.result[0].Ol_ID;
                    vdata.outlineClick(vdata.olid, null);
                    vdata.course = cur.data.result;
                    vdata.msgGet();
                } else {
                    if (!ol.data.success) throw "章节列表加载异常！详情：\r" + ol.data.message;
                    if (!cur.data.success) throw "课程信息加载异常！详情：\r" + cur.data.message;
                }
            })).catch(function (err) {
                alert(err);
            });
    }
});
vdata.$mount('#context-box');
//窗体失去焦点的事件
window.onblur = function () {
    if (vdata.playready()) {
        if (!vdata.state.isLive)
            vdata.player.pause();
    }
}
window.onfocus = function () {
    if (vdata.playready()) {
        !vdata.state.isLive ? vdata.player.play() : vdata.player.pause();
    }
}
//全局过滤器，日期格式化
Vue.filter('date', function (value, fmt) {
    if ($api.getType(value) != 'Date') return value;
    var o = {
        "M+": value.getMonth() + 1,
        "d+": value.getDate(),
        "h+": value.getHours(),
        "m+": value.getMinutes(),
        "s+": value.getSeconds()
    };
    if (/(y+)/.test(fmt))
        fmt = fmt.replace(RegExp.$1, (value.getFullYear() + "").substr(4 - RegExp.$1.length));
    for (var k in o)
        if (new RegExp("(" + k + ")").test(fmt))
            fmt = fmt.replace(RegExp.$1, (RegExp.$1.length == 1) ? (o[k]) : (("00" + o[k]).substr(("" + o[k]).length)));
    return fmt;
});
/*
//点击留言按钮，进入留言输入状态
mui('body').on('tap', '#msginputBtn', function () {
    vdata.msgFocus();
});
*/
//
mui('body').on('tap', '#chatArea, .videobox', function () {
    vdata.msgBlur();
});