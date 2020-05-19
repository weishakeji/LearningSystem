var couid = $api.querystring("couid");
var vdata = new Vue({
    data: {
        //数据实体
        account: {}, //当前账号信息
        course: {}, //当前课程
        outline: {}, //当前课程章节
        subject: {}, //当前专业
        outlines: [], //当前课程的章节列表（树形）
        access: [], //附件列表
        events: [], //视频事件
        messages: [], //咨询留言
        //当前章节的视频信息
        video: {
            url: '', //视频路径
            total: 0, //总时长      
            playTime: 0, //当前播放时间，单位：毫秒     
            playhistime: 0, //历史播放时间
            studytime: 0, //累计学习时间
            percent: 0 //完成度（百分比）
        },
        playtime: 0, //当前播放时间，单位：秒
        playpercent: 0, //当前播放完成度百分比
        //状态
        state: {}, //课程状态       
        couid: $api.querystring("couid"),
        olid: $api.querystring("olid"),
        median: false, //分隔线折叠状态
        titState: 'loading', //左侧选项卡的状态
        rightState: 'outline', //右侧选项卡状态，章节outline,交流chat
        outlineLoaded: false, //右侧章节列表加载中
        studylogUpdate: false, //学习记录是否在递交中
        studylogState: 0, //学习记录提交状态，1为成功，-1为失败
        switchSubtitle: $api.cookie("switchSubtitle_" + $api.querystring("couid")) == "true" ? true : false, //弹幕开关
        //控件
        player: null //播放器
    },
    watch: {
        //课程状态
        state: function(val) {
            if (vdata.state.isNull) vdata.titState = 'isNull';
            if (vdata.state.isAccess) vdata.titState = 'isAccess';
            if (vdata.state.isQues) vdata.titState = 'isQues';
            if (vdata.state.isContext) vdata.titState = 'isContext';
            if (vdata.state.isLive) vdata.titState = 'isLive';
            if (vdata.state.existVideo) vdata.titState = 'existVideo';
            //if (!vdata.state.canStudy) vdata.titState = 'isNull';
            //视频播放
            if (vdata.state.canStudy && (vdata.state.existVideo || vdata.state.isLive))
                vdata.videoPlay(vdata.state);
        },
        //左侧选项卡切换
        titState: function(val) {
            if (vdata.playready()) {
                vdata.titState == 'existVideo' ? vdata.player.play() : vdata.player.pause();
            }
        },
        //弹幕开关
        'switchSubtitle': function(val, old) {
            if (val != old)
                $api.cookie("switchSubtitle_" + vdata.couid, val);
        },
        //播放进度时间变化
        playtime: function(val) {
            vdata.video.studytime++;
            //当前视频播放进度百分比
            var per = Math.floor(vdata.video.studytime <= 0 ? 0 : vdata.video.studytime / vdata.video.total * 100);
            vdata.playpercent = per;
            //触发视频事件
            vdata.videoEvent(vdata.playtime);
        },
        //播放进度百分比变化，
        playpercent: function(val, oldval) {
            //console.log('当前播放进度百分比：'+val);
            //学习记录提交
            if (val <= 100) vdata.videoLog(val);
        }
    },
    methods: {
        //知识库的点击事件
        knlClick: function() {
            new top.PageBox('课程知识库', 'Knowledges.ashx?couid=' + vdata.couid, 100, 100, null, window.name).Open();
        },
        //附件的点击事件
        accessClick: function(file, tit, event) {
            var exist = file.substring(file.lastIndexOf(".") + 1).toLowerCase();
            if (exist == "pdf") {
                event.preventDefault();
                var box = new PageBox(tit, $api.pdfViewer(file), 100, 100, null, window.name);
                box.Open();
            }
            return false;
        },
        //章节列表的点击事件
        outlineClick: function(olid, event) {
            var url = $api.setpara("olid", olid);
            history.pushState({}, null, url);
            vdata.olid = olid;
            vdata.titState = 'loading';
            if (event != null) event.preventDefault();
            //获取当前章节状态，和专业信息
            $api.all(
                $api.get("Outline/ForID", {
                    id: olid
                }),
                $api.get("Outline/state", {
                    olid: olid
                })
            ).then(axios.spread(function(ol, state) {
                if (ol.data.success && state.data.success) {
                    vdata.outline = ol.data.result;
                    vdata.state = state.data.result;
                    if (!vdata.state.isLive && vdata.state.PlayTime / 1000 > 0) {
                        /*
                        vdata.$confirm('是否从上次进度播放？', '提示', {
                            confirmButtonText: '确定',
                            cancelButtonText: '取消',
                            type: 'warning'
                        }).then(function() {
                            vdata.videoSeek(vdata.state.PlayTime / 1000);
                            window.setTimeout(function() {
                                if (vdata.playready()) vdata.player.play();
                            }, 500);
                        }).catch(function() {

                        });*/
                    }
                    //视频播放记录
                    var result = state.data.result;
                    vdata.video.studytime = isNaN(result.StudyTime) ? 0 : result.StudyTime;
                    vdata.video.playhistime = isNaN(result.PlayTime) ? 0 : result.PlayTime / 1000;
                    window.setTimeout(function() {
                        vdata.outlineLoaded = true;
                    }, 100);
                    //获取附件
                    if (state.data.result.isAccess) {
                        $api.get("Outline/Accessory", {
                            uid: vdata.outline.Ol_UID
                        }).then(function(acc) {
                            if (acc.data.success) {
                                vdata.access = acc.data.result;
                            } else {
                                alert("附件信息加载异常！详情：\r" + acc.data.message);
                            }
                        });
                    }
                    //获取视频事件
                    if (state.data.result.existVideo) {
                        $api.get("Outline/VideoEvents", {
                            olid: vdata.outline.Ol_ID
                        }).then(function(req) {
                            if (req.data.success) {
                                vdata.events = req.data.result;
                            } else {
                                alert("视频事件加载失败！详情：\r" + req.data.message);
                            }
                        });
                    }
                } else {
                    if (!ol.data.success) alert("章节信息加载异常！详情：\r" + ol.data.message);
                    if (!state.data.success) alert("章节状态加载异常！详情：\r" + state.data.message);
                }
            }));
            //获取留言列表
            vdata.msgGet();
        },
        //播放器是否准备好
        playready: function() {
            if (vdata.player != null) {
                return vdata.player._isReady && vdata.player.engine;
            }
            return false;
        },
        //视频开始播放
        videoPlay: function(state) {
            if (vdata.player != null) {
                vdata.player.destroy();
                vdata.player = null;
            }
            if (!vdata.state.isLive) { //点播
                vdata.player = new QPlayer({
                    url: state.urlVideo,
                    container: document.getElementById("videoplayer"),
                    autoplay: true,
                });
            } else { //直播
                vdata.player = new QPlayer({
                    url: state.urlVideo,
                    container: document.getElementById("livebox"),
                    isLive: true,
                    autoplay: true
                });
            }
            if (vdata.player != null) {
                vdata.player.on("loading", vdata.videoready);
                vdata.player.on("ready", vdata.videoready);
                vdata.player.on("timeupdate", function(currentTime, totalTime) {
                    vdata.video.total = parseInt(totalTime);
                    vdata.video.playTime = currentTime; //详细时间，精确到毫秒
                    vdata.playtime = parseInt(currentTime);
                    //学习完成度，最大为百分百
                    vdata.video.percent = Math.floor(vdata.video.studytime <= 0 ? 0 : vdata.video.studytime / vdata.video.total * 1000) / 10;
                    vdata.video.percent = vdata.video.percent > 100 ? 100 : vdata.video.percent;
                });
                vdata.player.on("seeked", function() {
                    vdata.playtime = vdata.playready() ? vdata.player.currentTime : 0;
                    window.setTimeout(function() {
                        if (vdata.playready()) vdata.player.pause();
                    }, 50);

                });
            }
        },
        //播放器加载后的事件
        videoready: function() {
            //隐藏全屏按钮
            var fullbtn = document.getElementsByClassName("qplayer-fullscreen");
            for (var i = 0; i < fullbtn.length; i++) {
                fullbtn[i].style.display = "none";
            }
            //隐藏设置按钮(播放倍速也禁用了)
            var setbtn = document.getElementsByClassName("qplayer-settings-btn");
            for (var i = 0; i < setbtn.length; i++) {
                //setbtn[i].style.display = "none";
            }
            //给video对象增加属性
            var video = document.querySelector("video");
            video.setAttribute("x5-playsinline", "");
            video.setAttribute("playsinline", true);
            video.setAttribute("webkit-playsinline", true);
            video.setAttribute("x-webkit-airplay", true);
            video.setAttribute("x5-video-player-type", "h5");
            video.setAttribute("x5-video-player-fullscreen", "");
            video.setAttribute("x5-video-orientation", "portraint");
        },
        //视频播放跳转
        videoSeek: function(second) {
            if (vdata.playready()) {
                vdata.player.seek(second);
            }
        },
        //学习记录记录
        videoLog: function(per) {
            if (vdata.studylogUpdate) return;
            var interval = 1; //间隔百分比多少递交一次记录
            if (vdata.video.total <= 5 * 60) interval = 10; //5分钟内的视频
            else if (vdata.video.total <= 10 * 60) interval = 5; //10分钟的视频，5%递交一次      
            else if (vdata.video.total <= 30 * 60) interval = 2; //30分钟的视频，2%递交一次    
            if (per > 0 && per < (100 + interval) && per % interval == 0) {
                $api.post("Course/StudyLog", {
                    couid: vdata.course.Cou_ID,
                    olid: vdata.outline.Ol_ID,
                    playTime: vdata.playtime,
                    studyTime: vdata.video.studytime,
                    totalTime: vdata.video.total
                }, function() {
                    vdata.studylogUpdate = true;
                }, function() {
                    vdata.studylogUpdate = false;
                }).then(function(req) {
                    if (!req.data.success) {
                        if (vdata.playready()) {
                            vdata.player.pause();
                            vdata.player.destroy();
                            vdata.player = null;
                        }
                        alert(req.data.message);
                        return;
                    }
                    vdata.studylogState = 1;
                    window.setTimeout(function() {
                        vdata.studylogState = 0;
                    }, 2000);
                }).catch(function(err) {
                    vdata.studylogState = -1;
                    //alert(err);
                    window.setTimeout(function() {
                        vdata.studylogState = 0;
                    }, 2000);
                });
            }
        },
        //视频事件的触发
        videoEvent: function(playtime) {
            if (vdata.events.length < 1) return;
            var curr = null;
            for (var ev in vdata.events) {
                if (vdata.events[ev].Oe_TriggerPoint == playtime) {
                    curr = vdata.events[ev];
                    break;
                }
            }
            if (curr == null) return;
            //视频暂停
            if (vdata.playready()) vdata.player.pause();
            var box = new MsgBox();
            box.OverEvent = function() {
                if (vdata.playready()) vdata.player.play();
            }
            if (curr.Oe_EventType == 1)
                box.Init("提示：" + curr.Oe_Title, curr.Oe_Context, curr.Oe_Width, curr.Oe_Height, "alert").Open();
            if (curr.Oe_EventType == 2)
                box.Init("知识点：" + curr.Oe_Title, curr.Oe_Context, curr.Oe_Width, curr.Oe_Height, "alert").Open();
            if (curr.Oe_EventType == 3) {
                var items = eval(curr.Oe_Datatable);
                var context = curr.Oe_Context + "<div class='quesBox'>";
                for (var i in items) {
                    if (items[i].item == "") continue;
                    context += "<div onclick='vdata.videoEventClick(" + items[i].iscorrect + ",-1)'>" +
                        (Number(i) + 1) + "、" + items[i].item + "</div>";
                }
                context += "</div>";
                context += "<div id='event_error'>回答错误</div>";
                box.Init("提问" + curr.Oe_Title, context, curr.Oe_Width, curr.Oe_Height, "null").Open();
            }
            if (curr.Oe_EventType == 4) {
                var items = eval(curr.Oe_Datatable);
                var context = curr.Oe_Context + "<div class='quesBox'>";
                for (var i in items) {
                    if (items[i].item == "") continue;
                    context += "<div onclick='vdata.videoEventClick(null," + items[i].point + ")'>" +
                        (Number(i) + 1) + "、" + items[i].item + " - （跳转到：" + items[i].point + "秒）</div>";
                }
                context += "</div>";
                box.Init("反馈：" + curr.Oe_Title, context, curr.Oe_Width, curr.Oe_Height, "alert").Open();
            }
        },
        //视频事件的点击操作
        videoEventClick: function(iscorrect, seek) {
            //视频事件的问题
            if (iscorrect != null) {
                if (iscorrect) MsgBox.Close();
                if (!iscorrect) {
                    var err = document.getElementById("event_error");
                    err.style.height = 20 + 'px';
                    err.style.opacity = 1;
                    window.setTimeout(function() {
                        var err = document.getElementById("event_error");
                        err.style.height = 0 + 'px';
                        err.style.opacity = 0;
                    }, 1000);
                }
            }
            //视频跳转
            if (iscorrect == null && seek > 0) {
                if (!vdata.playready()) return;
                vdata.player.seek(seek);
                MsgBox.Close();
            }
        },
        //发送消息
        msgSend: function() {
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
            $api.post("message/add", {
                acc: '',
                msg: msg,
                playtime: vdata.playtime,
                couid: vdata.couid,
                olid: vdata.olid
            }).then(function(req) {
                var d = req.data;
                if (d.success) {
                    document.getElementById("messageinput").value = '';
                    vdata.msgGet();
                } else {
                    alert("信息添加发生异常！详情：\r" + d.message);
                }
            });
        },
        //获取当前章节的留言信息
        msgGet: function() {
            if (!vdata.olid || vdata.olid < 1) return;
            $api.post("message/All", {
                olid: vdata.olid,
                order: 'asc'
            }).then(function(req) {
                var d = req.data;
                if (d.success) {
                    vdata.messages = d.result;
                    window.setTimeout(function() {
                        var dl = document.getElementById("chatlistdl");
                        document.getElementById("chatlist").scrollTop = dl.offsetHeight;
                    }, 1000);
                } else {
                    alert("留言信息加载异常！详情：\r" + d.message);
                }
            }).catch(function(err) {
                //alert("msgGet方法存在错误："+err);
            });
        },
        init: function() {

        }
    },
    created: function() {
        var couid = $api.querystring("couid");
        $api.all(
            $api.get("Outline/tree", {
                couid: couid
            }),
            $api.get("Course/ForID", {
                id: couid
            })).then(axios.spread(function(ol, cur) {
            if (ol.data.success && cur.data.success) {
                vdata.outlines = ol.data.result;
                if (vdata.olid == '') vdata.olid = ol.data.result[0].Ol_ID;
                vdata.outlineClick(vdata.olid, null);
                vdata.course = cur.data.result;
                document.title = vdata.course.Cou_Name;
                $api.get("Subject/ForID", {
                    id: vdata.course.Sbj_ID
                }).then(function(subject) {
                    if (subject.data.success) {
                        vdata.subject = subject.data.result;
                    } else {
                        if (!subject.data.success) throw "课程所属专业加载异常！详情：\r" + subject.data.message;
                    }
                }).catch(function(err) {
                    alert(err);
                });
                vdata.msgGet();
            } else {
                if (!ol.data.success) throw "章节列表加载异常！详情：\r" + ol.data.message;
                if (!cur.data.success) throw "课程信息加载异常！详情：\r" + cur.data.message;
            }
        })).catch(function(err) {
            alert(err);
        });
        //当前登录学员
        $api.get("Account/Current", {}, null, null).then(function(req) {
            if (req.data.success) {
                vdata.account = req.data.result;
            }
        });
        //定时刷新（加载）咨询留言
        window.setInterval('vdata.msgGet()', 1000 * 20);
    },
    mounted: function() {
        //视频上面的漂浮信息（学员姓名和电话），防录屏
        window.setInterval(function() {
            var acc = document.getElementById("accinfo");
            if (acc == null) return;
            if (acc.parentNode.offsetHeight == 0 || acc.parentNode.offsetWidth == 0) return;
            //移动速度
            window.acctop = window.acctop ? window.acctop : Math.ceil(Math.random() * 100) / 10;
            window.accleft = window.accleft ? window.accleft : Math.ceil(Math.random() * 100) / 10;
            //获取当前坐标
            var top = Number(acc.style.top.replace('px', ''));
            var left = Number(acc.style.left.replace('px', ''));
            //转向            
            if (top < 0 || top > acc.parentNode.offsetHeight - acc.offsetHeight) window.acctop = -window.acctop;
            if (left < 0 || left > acc.parentNode.offsetWidth - acc.offsetWidth) window.accleft = -window.accleft;
            //移动 
            acc.style.top = (top < 0 ? 0 : (top > acc.parentNode.offsetHeight - acc.offsetHeight ? acc.parentNode.offsetHeight - acc.offsetHeight : top + window.acctop)) + "px";
            acc.style.left = (left < 0 ? 0 : (left > acc.parentNode.offsetWidth - acc.offsetWidth ? acc.parentNode.offsetWidth - acc.offsetWidth : left + window.accleft)) + "px";

        }, 200);

    },

});
vdata.$mount('#body');
window.onload = function() {
    this.vdata.init();
}

window.onblur = function() {
    if (vdata.playready()) {
        //if (vdata.state.isLive || vdata.state.existVideo)
        vdata.player.pause();
    }
}
window.onfocus = function() {
    if (vdata.playready()) {
        //vdata.titState == 'existVideo' && vdata.state.isLive ? vdata.player.play() : vdata.player.pause();
        //只有当处于视频状态时才播放
        if (vdata.titState == 'existVideo' || vdata.titState == 'isLive')
            vdata.player.play();
    }
}


//全局过滤器，日期格式化
Vue.filter('date', function(value, fmt) {
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