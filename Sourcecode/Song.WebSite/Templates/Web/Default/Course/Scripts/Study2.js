$ready(function () {
    //var couid = $api.querystring("couid");
    var vdata = new Vue({
        el: '#vapp',
        data: {
            //数据实体
            account: {}, //当前账号信息
            course: {}, //当前课程
            outline: {}, //当前课程章节       
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
            couid: $api.querystring("couid") == "" ? $api.dot() : $api.querystring("couid"),
            olid: $api.querystring("olid"),
            median: false, //分隔线折叠状态
            isMessage: false,         //是否启用留言咨询
            titState: 'loading', //左侧选项卡的状态
            rightState: 'outline', //右侧选项卡状态，章节outline,交流chat
            outlineLoaded: false, //右侧章节列表加载中
            studylogUpdate: false, //学习记录是否在递交中
            studylogState: 0, //学习记录提交状态，1为成功，-1为失败
            switchSubtitle: $api.cookie("switchSubtitle_" + $api.querystring("couid")) == "true" ? true : false, //弹幕开关
            //控件
            player: null //播放器
        },
        computed: {
            //是否登录
            islogin: function () {
                return JSON.stringify(this.account) != '{}' && this.account != null;
            }
        },
        watch: {
            //课程状态
            state: function (val) {
                if (this.state.isNull) this.titState = 'isNull';
                if (this.state.isAccess) this.titState = 'isAccess';
                if (this.state.isQues) this.titState = 'isQues';
                if (this.state.isContext) this.titState = 'isContext';
                if (this.state.isLive) this.titState = 'isLive';
                if (this.state.existVideo) this.titState = 'existVideo';
                //if (!this.state.canStudy) this.titState = 'isNull';
                if (this.state.DeskAllow) return;
                //视频播放
                if (this.state.canStudy && (this.state.existVideo || this.state.isLive))
                    this.videoPlay(this.state);
            },
            //左侧选项卡切换
            titState: function (val) {
                if (this.playready()) {
                    this.titState == 'existVideo' ? this.player.play() : this.player.pause();
                }
            },
            //弹幕开关
            'switchSubtitle': function (val, old) {
                if (val != old)
                    $api.cookie("switchSubtitle_" + this.couid, val);
            },
            //播放进度时间变化
            playtime: function (val) {
                this.video.studytime++;
                //当前视频播放进度百分比
                var per = Math.floor(this.video.studytime <= 0 ? 0 : this.video.studytime / this.video.total * 100);
                this.playpercent = per;
                //触发视频事件
                this.videoEvent(this.playtime);
            },
            //播放进度百分比变化，
            playpercent: function (val, oldval) {
                //console.log('当前播放进度百分比：'+val);
                //学习记录提交
                if (val <= 100) this.videoLog(val);
            }
        },
        methods: {
            //获取当前章节
            getOutline: function (id, outlines) {
                outlines = outlines ? outlines : this.outlines;
                var ol = null;
                for (var i = 0; i < outlines.length; i++) {
                    if (outlines[i].Ol_ID == Number(id)) {
                        ol = outlines[i];
                        break;
                    }
                    if (ol == null && (outlines[i].children && outlines[i].children.length > 0)) {
                        ol = this.getOutline(id, outlines[i].children);
                    }
                }
                return ol;
            },
            //知识库的点击事件
            knlClick: function () {
                $pagebox.create({
                    'url': $api.url.set('Knowledges', { 'couid': this.couid }),
                    'title': '课程知识库', 'ico': 'f0085',
                    'width': '80%', 'height': '80%',
                    'min': false, 'full': true, 'showmask': true
                }).open();
            },
            //章节列表的点击事件
            outlineClick: function (outline) {
                var th = this;
                var olid = outline.Ol_ID;
                var url = $api.setpara("olid", olid);
                history.pushState({}, null, url);
                this.olid = olid;
                //设置当前节点的样式
                this.$nextTick(function () {
                    var tree = this.$refs['outlines_tree'];
                    tree.setCurrentKey(outline.Ol_ID);
                });

                this.titState = 'loading';
                //获取当前章节状态，和专业信息
                $api.get('Outline/State', { 'olid': olid }).then(function (req) {
                    if (req.data.success) {
                        th.state = req.data.result;
                        if (!vdata.state.isLive && vdata.state.PlayTime / 1000 > 0) {
                            var vurl = vdata.state.urlVideo;  //视频播放地址
                            vurl = !vurl ? '' : vurl;
                            if (vurl.indexOf('.') > -1) {
                                var ext = vurl.substring(vurl.lastIndexOf('.') + 1).toLowerCase();
                                if (ext == 'm3u8') {
                                    vdata.$confirm('是否从上次进度播放？', '提示', {
                                        confirmButtonText: '确定',
                                        cancelButtonText: '取消',
                                        type: 'warning'
                                    }).then(function () {
                                        vdata.videoSeek(vdata.state.PlayTime / 1000);
                                        window.setTimeout(function () {
                                            if (vdata.playready()) vdata.player.play();
                                        }, 500);
                                    }).catch(function () {

                                    });
                                }
                            }
                        }
                        //视频播放记录
                        var result = req.data.result;
                        vdata.video.studytime = isNaN(result.StudyTime) ? 0 : result.StudyTime;
                        vdata.video.playhistime = isNaN(result.PlayTime) ? 0 : result.PlayTime / 1000;
                        window.setTimeout(function () {
                            vdata.outlineLoaded = true;
                        }, 100);
                        //获取视频事件
                        if (result.existVideo && vdata.outline) {
                            $api.get("Outline/VideoEvents", {
                                olid: vdata.outline.Ol_ID
                            }).then(function (req) {
                                if (req.data.success) {
                                    vdata.events = req.data.result;
                                } else {
                                    alert("视频事件加载失败！详情：\r" + req.data.message);
                                }
                            });
                        }
                    } else {
                        console.error(req.data.exception);
                        throw req.config.way + ' ' + req.data.message;
                    }
                }).catch(function (err) {
                    Vue.prototype.$alert(err);
                    console.error(err);
                });
                //获取留言列表
                if (vdata.isMessage) vdata.msgGet();
            },
            //播放器是否准备好
            playready: function () {
                if (vdata.player != null) {
                    return vdata.player._isReady && vdata.player.engine;
                }
                return false;
            },
            //视频开始播放
            videoPlay: function (state) {
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
                    vdata.player.on("timeupdate", function (currentTime, totalTime) {
                        vdata.video.total = parseInt(totalTime);
                        vdata.video.playTime = currentTime; //详细时间，精确到毫秒
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
                    vdata.player.on("play", function (e) {
                        if (window.videoFixed) vdata.player.pause();
                    });
                }
            },
            //播放器加载后的事件
            videoready: function () {
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
                if (video != null) {
                    video.setAttribute("x5-playsinline", "");
                    video.setAttribute("playsinline", true);
                    video.setAttribute("webkit-playsinline", true);
                    video.setAttribute("x-webkit-airplay", true);
                    video.setAttribute("x5-video-player-type", "h5");
                    video.setAttribute("x5-video-player-fullscreen", "");
                    video.setAttribute("x5-video-orientation", "portraint");
                }
            },
            //视频播放跳转
            videoSeek: function (second) {
                if (vdata.playready()) {
                    vdata.player.seek(second);
                }
            },
            //学习记录提交到服务器
            videoLog: function (per) {
                if (this.studylogUpdate) return;
                var interval = 1; //间隔百分比多少递交一次记录
                if (this.video.total <= 5 * 60) interval = 10; //5分钟内的视频
                else if (this.video.total <= 10 * 60) interval = 5; //10分钟的视频，5%递交一次      
                else if (this.video.total <= 30 * 60) interval = 2; //30分钟的视频，2%递交一次    
                var th = this;
                if (per > 0 && per < (100 + interval) && per % interval == 0) {
                    if (!this.outline) return;
                    $api.post("Course/StudyLog", {
                        couid: th.course.Cou_ID,
                        olid: th.outline.Ol_ID,
                        playTime: th.playtime,
                        studyTime: th.video.studytime,
                        totalTime: th.video.total
                    }, function () {
                        th.studylogUpdate = true;
                    }, function () {
                        th.studylogUpdate = false;
                    }).then(function (req) {
                        if (!req.data.success) {
                            if (vdata.playready()) {
                                th.player.pause();
                                th.player.destroy();
                                th.player = null;
                            }
                            alert(req.data.message);
                            return;
                        }
                        th.studylogState = 1;
                        window.setTimeout(function () {
                            vdata.studylogState = 0;
                        }, 2000);
                    }).catch(function (err) {
                        th.studylogState = -1;
                        var msg = "当前学员状态为“未登录”，请确认是否失效，还是存在多处登录的现像？";
                        msg += "<br/>提示：同一账号不可以同时登录多个设备或浏览器。"
                        th.$alert(msg, '登录状态失效', {
                            confirmButtonText: '确定',
                            dangerouslyUseHTMLString: true,
                            callback: action => {
                                th.account = {};
                                console.log(th.account);
                            }
                        });
                        window.setTimeout(function () {
                            th.studylogState = 0;
                        }, 2000);
                    });
                }
            },
            //视频事件的触发
            videoEvent: function (playtime) {
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
                box.OverEvent = function () {
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
            videoEventClick: function (iscorrect, seek) {
                //视频事件的问题
                if (iscorrect != null) {
                    if (iscorrect) MsgBox.Close();
                    if (!iscorrect) {
                        var err = document.getElementById("event_error");
                        err.style.height = 20 + 'px';
                        err.style.opacity = 1;
                        window.setTimeout(function () {
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
            msgSend: function () {
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
                }).then(function (req) {
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
            msgGet: function () {
                if (!vdata.olid || vdata.olid < 1) return;
                $api.post("message/count", {
                    olid: vdata.olid,
                    order: 'asc',
                    count: 100
                }).then(function (req) {
                    var d = req.data;
                    if (d.success) {
                        vdata.messages = d.result;
                        window.setTimeout(function () {
                            var dl = document.getElementById("chatlistdl");
                            document.getElementById("chatlist").scrollTop = dl.offsetHeight;
                        }, 1000);
                    } else {
                        alert("留言信息加载异常！详情：\r" + d.message);
                    }
                }).catch(function (err) {
                    //alert("msgGet方法存在错误："+err);
                });
            },
            init: function () {

            }
        },
        created: function () {
            $dom.load.css([$dom.path() + 'styles/pagebox.css']);
            var couid = this.couid;
            var th = this;
            $api.bat(
                $api.get("Outline/tree", { 'couid': couid, 'isuse': true }),
                $api.get("Course/ForID", { id: couid })).then(axios.spread(function (ol, cur) {
                    if (ol.data.success && cur.data.success) {
                        if (ol.data.result.length < 1) throw "没有课程章节";
                        th.outlines = ol.data.result;
                        th.course = cur.data.result;
                        document.title = th.course.Cou_Name;
                        //生成多级序号                   
                        (() => {
                            calcSerial(null, '');
                            function calcSerial(outline, lvl) {
                                var childarr = outline == null ? th.outlines : (outline.children ? outline.children : null);
                                if (childarr == null) return;
                                for (let i = 0; i < childarr.length; i++) {
                                    childarr[i].serial = lvl + (i + 1) + '.';
                                    calcSerial(childarr[i], childarr[i].serial);
                                }
                            }
                        })();
                        if (th.olid == '') th.olid = ol.data.result[0].Ol_ID;
                        th.outline = th.getOutline(th.olid, null);
                        if (th.outline == null) throw "当前章节不存在";
                        th.outlineClick(th.outline, null);
                    } else {
                        if (!ol.data.success) throw "章节列表加载异常！详情：\r" + ol.data.message;
                        if (!cur.data.success) throw "课程信息加载异常！详情：\r" + cur.data.message;
                    }
                })).catch(function (err) {
                    alert(err);
                    console.error(err);
                });

            //当前登录学员
            $api.login.account().then(function (acc) {
                th.account = acc;
                $api.login.account_fresh(() => {
                    var msg = "当前学员状态为“未登录”，请确认是否失效，还是存在多处登录的现像？";
                    msg += "<br/>提示：同一账号不可以同时登录多个设备或浏览器。"
                    th.$alert(msg, '登录状态失效', {
                        confirmButtonText: '确定',
                        dangerouslyUseHTMLString: true,
                        callback: action => {
                            th.account = {};
                            console.log(th.account);
                        }
                    });
                    th.account = {};
                });
            }).catch(() => { });

            //定时刷新（加载）咨询留言
            if (this.isMessage) {
                window.setInterval('vdata.msgGet()', 1000 * 10);
            }
        },
        mounted: function () {
            //视频上面的漂浮信息（学员姓名和电话），防录屏
            window.setInterval(function () {
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
    window.onload = function () {
        if (this.vdata)
            this.vdata.init();
    }

    window.onblur = function () {
        if (vdata.playready()) {
            //if (vdata.state.isLive || vdata.state.existVideo)
            if (!vdata.state.SwitchPlay)
                vdata.player.pause();
        }
    }
    window.onfocus = function () {
        //如果有视频事件弹出，则窗体获取焦点时，视频并不播放
        if ($("div[type=MsgBox]").size() > 0) return;
        if (vdata.playready()) {
            //vdata.titState == 'existVideo' && vdata.state.isLive ? vdata.player.play() : vdata.player.pause();
            //只有当处于视频状态时才播放
            if (vdata.titState == 'existVideo' || vdata.titState == 'isLive')
                vdata.player.play();
        }
    }

    window.onresize = function () {
        window.setTimeout(function () {
            var str = '';
            [22, 9, 4, 5, 15].forEach(x => str += String.fromCharCode(0x60 + x));
            var v = document.querySelector(str);
            if (!v) return;
            var styles = document.defaultView.getComputedStyle(v.parentNode, null);
            var posi = styles.getPropertyValue('position');
            window.videoFixed = posi == 'fixed';
            v.style.display = window.videoFixed ? 'none' : '';
            if (vdata.playready()) {
                !window.videoFixed && $("div[type=MsgBox]").size() < 1 ? v.play() : v.pause();
            }
        }, 100);
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
    //学习界面的选项卡
    Vue.component('study_tabs', {
        props: ['state', 'option'],
        data: function () {
            return {
                tabs: [
                    { 'name': '视频', 'tag': 'existVideo', 'icon': 'e856' },
                    { 'name': '直播', 'tag': 'isLive', 'icon': 'e856' },
                    { 'name': '内容', 'tag': 'isContext', 'icon': 'e856' },
                    { 'name': '附件', 'tag': 'isAccess', 'icon': 'e856' },
                    { 'name': '习题', 'tag': 'isQues', 'icon': 'e856' }],
            }
        },
        watch: {
            'state': function (val, old) {

            },

        },
        methods: {

        },
        template: '<div id="accessory">\
         <div  v-if="!isbuy" style="color:red;">课程未购买，资料不提供下载或预览</div>\
        <a  v-if="isbuy"  v-for="(item,index) in datas" target="_blank":href="item.As_FileName"\
            v-on:click="accessClick(item.As_FileName,item.As_Name,$event)"\
            :download="item.As_Name">{{index+1}}、{{item.As_Name}}\
        </a>\
        <div v-if="!isbuy"  v-for="(item,index) in datas" >\
        {{index+1}}、{{item.As_Name}}\
        </div>\
        <div class="noInfo">{{msg}}</div>\
        </div>'
    });
    //
    //******* 附件列表
    Vue.component('accessory', {
        props: ['uid', 'isbuy', 'account'],
        data: function () {
            return {
                datas: [],  //附件列表
                msg: ''      //提示信息
            }
        },
        watch: {
            'uid': function (val, old) {
                var th = this;
                th.msg = '';
                $api.get("Outline/Accessory", { uid: val, 'acid': th.account.Ac_ID }).then(function (acc) {
                    if (acc.data.success) {
                        th.datas = acc.data.result;
                    } else {
                        th.msg = "附件信息加载异常！详情：\r" + acc.data.message;
                    }
                }).catch(function (err) {
                    th.msg = err;
                });
            },
            'isbuy': function (val, old) {

            }
        },
        methods: {
            //附件的点击事件
            accessClick: function (file, tit, event) {
                var exist = file.substring(file.lastIndexOf(".") + 1).toLowerCase();
                if (exist == "pdf") {
                    event.preventDefault();
                    var box = new PageBox(tit, $api.pdfViewer(file), 100, 100, null, window.name);
                    box.Open();
                }
                return false;
            }
        },
        template: '<div id="accessory">\
         <div  v-if="!isbuy" style="color:red;">课程未购买，资料不提供下载或预览</div>\
        <a  v-if="isbuy"  v-for="(item,index) in datas" target="_blank":href="item.As_FileName"\
            v-on:click="accessClick(item.As_FileName,item.As_Name,$event)"\
            :download="item.As_Name">{{index+1}}、{{item.As_Name}}\
        </a>\
        <div v-if="!isbuy"  v-for="(item,index) in datas" >\
        {{index+1}}、{{item.As_Name}}\
        </div>\
        <div class="noInfo">{{msg}}</div>\
        </div>'
    });
}, ['/Utilities/Qiniuyun/qiniu-web-player-1.2.3.js',
    'Components/progress_video.js',
    '../scripts/pagebox.js',]);