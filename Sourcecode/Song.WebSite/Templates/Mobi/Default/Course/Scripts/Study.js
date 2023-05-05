$ready(function () {

    window.vdata = new Vue({
        el: '#vapp',
        data: {
            couid: $api.querystring("couid") == "" ? $api.dot() : $api.querystring("couid"),
            account: {},		//当前登录学员
            course: {},		//当前课程
            outline: {},		//当前章节
            state: {},           //当前章节的状态
            videolog: [],        //课程章节的视频学习记录
            outlines: [], 	//当前课程所有章节
            studied: false,		//当前学员是否可以学习，免费试用也允许
            owned: false,        //当前学员是否购买该课程或学员组关联
            messages: [], //咨询留言
            menuShow: false,		//章节菜单是否显示
            isMessage: true,         //是否启用留言咨询
            contextShow: 'content',		//内容显示的判断
            //playtime: 0, //当前播放时间，单位：秒
            loading_init: true
        },
        watch: {
            //章节
            'outline': {
                deep: true, immediate: false,
                handler: function (n, o) {
                    if (JSON.stringify(n) != '{}' && n != null) {
                        this.getLogForOutlineVideo();
                        this.getstate(n.Ol_ID);
                    }
                }
            }
        },
        computed: {
            //是否登录
            islogin: function () {
                return JSON.stringify(this.account) != '{}' && this.account != null;
            }
        },
        created: function () {
            var th = this;
            //当前登录学员
            $api.login.account().then(function (acc) {
                th.account = acc;
                th.init();
                $api.login.account_fresh(() => {
                    alert('登录失效，同一账号不可以同时登录多个设备');
                });
            }).catch(() => {
                th.account = {};
            });
        },
        methods: {
            outline_change: function (node) {
                console.log(node);
                this.outline = node;
            },
            //初始化
            init: function () {
                var couid = this.couid;
                var th = this;
                $api.bat(
                    $api.cache("Course/ForID", { id: couid }),
                    $api.cache("Outline/TreeList", { couid: couid }),
                    $api.get('Course/Studied', { 'couid': couid }),
                    $api.get('Course/Owned', { 'couid': couid, 'acid': th.account.Ac_ID })
                ).then(axios.spread(function (cur, ol, studied, owned) {
                    th.loading_init = false;
                    th.course = cur.data.result;
                    document.title = th.course.Cou_Name;
                    th.outlines = ol.data.result;
                    //console.log(th.outlines);
                    th.studied = studied.data.result;
                    th.owned = owned.data.result;
                    //获取学习记录
                    if (th.islogin) th.getLogForOutlineVideo();
                })).catch(function (err) {
                    alert(err);
                });
            },
            //获取章节的状态
            getstate: function (olid) {
                var th = this;
                th.loading = true;
                //获取章节相关信息
                $api.bat(
                    $api.get('Outline/State', { 'olid': olid, 'acid': th.account.Ac_ID }),
                    $api.cache("Outline/Info", { 'olid': olid })
                ).then(axios.spread(function (state, info) {
                    th.loading = false;
                    //获取结果
                    var result = info.data.result;
                    for (let key in state.data.result) {
                        result[key] = state.data.result[key];
                    }
                    th.state = result;
                    //th.$emit('change', th.state, th.outline);
                    //console.log(th.state);
                })).catch(function (err) {
                    th.loading = false;
                    alert(err);
                    console.error(err);
                });
            },
            //记录或获取播放进度，包括播放时间与进度
            playinfo: function (olid, couid, time, percent) {
                var play = $api.storage('weisha_playinfo');
                if (play == null) play = [];
                var obj = {};
                var isexist = false;
                if (olid != '') {
                    for (var i = 0; i < play.length; i++) {
                        if (play[i].olid == olid) {
                            obj = play[i];
                            isexist = true;
                            break;
                        }
                    }
                }
                if (arguments.length <= 2) return obj;
                obj['olid'] = olid;
                obj['couid'] = couid;
                obj['time'] = time;
                obj['percent'] = percent;
                if (!isexist) play.push(obj);
                $api.storage('weisha_playinfo', play);
                return obj;
            },
            //获取当前课程的章节视频的学习记录
            getLogForOutlineVideo: function () {
                var th = this;
                $api.get('Course/LogForOutlineVideo',
                    { 'stid': th.account.Ac_ID, 'couid': th.couid })
                    .then(function (req) {
                        if (req.data.success) {
                            th.videolog = req.data.result;
                        } else {
                            throw req.data.message;
                        }
                    }).catch(function (err) {
                        console.error(err);
                    });
            }
        }
    });


    //底部按钮组件
    Vue.component('coursestudyfooter', {
        props: ['course'],
        data: function () {
            return {
                menus: [
                    { label: '章节列表', id: 'outline', icon: 'e841', show: true },
                    { label: '视频', id: 'video', icon: 'e761', show: true },
                    { label: '交流', id: 'message', icon: 'e817', show: false },
                    { label: '学习内容', id: 'content', icon: 'e813', show: true },
                    { label: '附件', id: 'accessory', icon: 'e853', show: true },
                    { label: '返回课程', id: 'goback', icon: 'f007c', show: true }
                ],
                loading: true //预载中             
            }
        },
        computed: {},
        created: function () {

        },
        methods: {
            //事件
            click: function (item) {
                switch (item.id) {
                    //显示章节树形菜单
                    case 'outline':
                        var tree = this.$parent.$refs['outline_tree'];
                        tree.show();
                        break;
                    case 'goback':
                        document.location.href = 'detail.' + this.course.Cou_ID;
                        break;
                    default:
                        vdata.contextShow = item.id;
                        break;
                }
                //console.log(this.course);
            }
        },
        template: `<footer id="nav_menu">
            <div v-for='item in menus' :id='item.id' v-if='item.show' v-on:click='click(item)'>
                <icon :style="'font-size:'+item.size+'px;'" 
                v-html="'&#x'+item.icon" v-if="!!item.icon && item.icon!=''"></icon>
                <icon v-else :style="'font-size:21px;'" v-html="'&#x'+deficon"></icon>   
                <span>{{item.label}}</span>
            </div>
            </footer>`
    });

    //留言咨询
    Vue.component('message', {
        props: ['outline', 'account'],
        data: function () {
            return {
                messages: [],		//信息列表	
                timer: null,			//	
                input_text: '',
                loading: true //预载中
            }
        },
        watch: {
            'outline': {
                deep: true,
                immediate: true,
                handler: function (newV, oldV) {
                    if (JSON.stringify(newV) == '{}' || newV == null) return;
                    this.messages = [];
                    this.msgGet();
                }
            }
        },
        created: function () {
            //定时刷新（加载）咨询留言
            this.timer = window.setInterval(this.msgGet, 1000 * 10);
        },
        methods: {
            //获取当前章节的留言信息
            msgGet: function () {
                if (!this.outline) return;
                var th = this;
                $api.get("message/count", {
                    olid: this.outline.Ol_ID, order: 'desc', count: 100
                }).then(function (req) {
                    if (req.data.success) {
                        th.messages = req.data.result;
                    } else {
                        console.error(req.data.exception);
                        throw req.data.message;
                    }
                }).catch(function (err) {
                    //alert("留言信息加载异常！详情：\r" + err);
                });
            },
            //发送消息
            msgSend: function () {

                this.msgBlur();
                var th = this;
                var msg = this.input_text;
                if ($api.trim(msg) == '') return;
                var span = Date.now() - Number($api.cookie("msgtime"));
                if (span / 1000 < 10) {
                    th.$dialog.alert({
                        message: '不要频繁发消息！',
                        position: 'bottom-right'
                    });
                    return;
                }
                $api.cookie("msgtime", Date.now());
                var play = vdata.playinfo(this.outline.Ol_ID);
                $api.post("message/add", {
                    acc: this.account.Ac_AccName, msg: msg, playtime: play.time,
                    couid: this.outline.Cou_ID, olid: this.outline.Ol_ID
                }).then(function (req) {
                    var d = req.data;
                    if (d.success) {
                        th.input_text = '';
                        th.msgGet();
                    } else {
                        var msg = "信息添加发生异常！详情：\r" + d.message;
                        th.$dialog.alert({
                            message: msg,
                            position: 'bottom-right'
                        });
                    }
                });
            },
            //留言输入框失去焦点
            msgBlur: function () {
                //document.getElementById("footer").style.display = "";
                //document.getElementById("messageinput").blur();
            },
            //留言输入框获取焦点
            msgFocus: function (e) {
                //document.getElementById("footer").style.display = "none";
                //document.getElementById("messageinput").focus();
            }
        },
        template: `<div id='chatarea'>
	<div class='outline-name'>
		<span v-show='!outline'>正在加载...</span>
		{{outline.Ol_Name}}
		<button id='msginputBtn' class='el-icon-edit' v-on:click='msgFocus'
			v-show='outline'> 留言
		</button>
	</div>
    <dl id='chatlist' v-if='messages.length>0'  v-on:click='msgBlur'>
            <dd v-for='(item,index) in messages'>
                <span :playtime='item.Msg_PlayTime'>
                    <acc><i class='el-icon-chat-dot-round'></i>{{item.Ac_Name}}：</acc>
                    <date>{{item.Msg_CrtTime | date('yyyy-M-d hh:mm:ss')}}</date>
                </span>
                <msg>{{item.Msg_Context}} </msg>
            </dd>
        </dl>
	<dl v-else id='chatlist'><dd  class='nomsg'>没有人留言！</dd></dl>
	<div id='chatbox' remark='留言录入区域'>
		<textarea rows='3' id='messageinput' v-model='input_text' name='messageinput' autofocus
			v-on:keyup.enter='msgSend'></textarea>
		<button id='btnMessage' v-on:click='msgSend'>发送</button>
    </div>
	</div>`
    });
    //视频播放
    Vue.component('videoplayer', {
        props: ['outline', 'account'],
        data: function () {
            return {
                state: {},		//章节状态
                player: null,	//播放器
                //当前章节的视频信息
                video: {
                    url: '', //视频路径
                    total: 0, //总时长      
                    playTime: 0, //当前播放时间，单位：毫秒     
                    playhistime: 0, //历史播放时间
                    studytime: 0, //累计学习时间
                    percent: 0, //完成度（百分比）
                    loading: false //是否处于加载状态
                },
                playtime: 0, //当前播放时间，单位：秒
                playpercent: 0, //当前播放完成度百分比
                studylogUpdate: false, //学习记录是否在递交中
                studylogState: 0, //学习记录提交状态，1为成功，-1为失败
                loading: true, 	//预载中
                state_loading: true,     //章节状态加载的预载
                error: ''        //错误信息
            }
        },
        watch: {
            'outline': {
                deep: true,
                immediate: true,
                handler: function (newV, oldV) {
                    if (JSON.stringify(newV) == '{}' || newV == null) return;
                    var th = this;
                    if (this.player != null) {
                        this.player.destroy();
                        this.player = null;
                    }
                    th.state_loading = true;
                    //获取章节相关信息
                    $api.bat(
                        $api.get('Outline/State', { 'olid': th.outline.Ol_ID, 'acid': th.account.Ac_ID }),
                        $api.cache("Outline/Info", { 'olid': th.outline.Ol_ID })
                    ).then(axios.spread(function (state, info) {
                        th.state_loading = false;
                        //获取结果
                        var result = info.data.result;
                        for (let key in state.data.result) {
                            result[key] = state.data.result[key];
                        }
                        th.state = result;
                        //视频播放记录                       
                        th.video.studytime = isNaN(result.StudyTime) ? 0 : result.StudyTime;
                        th.video.playhistime = isNaN(result.PlayTime) ? 0 : result.PlayTime / 1000;
                        window.setTimeout(function () {
                            th.outlineLoaded = true;
                        }, 100);
                        th.$emit('change', th.state, th.outline);
                        //console.log(th.state);
                    })).catch(function (err) {
                        th.state_loading = false;
                        th.error = err;
                        window.alert(err);
                        console.error(err);
                    });
                }
            },
            //课程状态
            state: function (val) {
                //视频播放
                if (val.existVideo || val.isLive) {
                    this.videoPlay(val);
                }
            },
            //播放进度变化
            playtime: function (val) {
                this.video.studytime++;
                //当前视频播放进度百分比
                var per = Math.floor(this.video.studytime <= 0 ? 0 : this.video.studytime / this.video.total * 100);
                this.playpercent = per;
                vdata.playinfo(this.outline.Ol_ID, this.outline.Cou_ID, val, per);
                //触发视频事件
                //vdata.videoEvent(vdata.playtime);
            },
            //播放进度百分比变化，
            playpercent: function (val, oldval) {
                //console.log('当前播放进度百分比：'+val);
                //学习记录提交
                if (val <= 100) this.videoLog(val);
            },
        },
        created: function () {

        },
        methods: {
            //视频开始播放
            videoPlay: function (state) {
                var th = this;
                if (!this.state.isLive) { //点播
                    var container = document.getElementById("videoplayer");
                    this.player = new QPlayer({
                        url: state.urlVideo,
                        container: container,
                        autoplay: true,
                        loop: "loop",
                        loggerLevel: 0
                    });
                } else { //直播
                    var u = navigator.userAgent,
                        app = navigator.appVersion;
                    var isAndroid = u.indexOf('Android') > -1 || u.indexOf('Linux') > -1; //g
                    var isIOS = !!u.match(/\(i[^;]+;( U;)? CPU.+Mac OS X/); //ios终端
                    this.player = new QPlayer({
                        url: state.urlVideo,
                        container: document.getElementById("livebox"),
                        isLive: !isIOS,
                        autoplay: true
                    });

                    this.player.on("error", function (e) {
                        //alert("播放发生错误："+e);
                    });
                    this.player.on("play", function (e) {
                        th.video.loading = false;
                    });
                    this.player.on("loading", function () {
                        th.video.loading = true;
                    });
                }
                if (this.player != null) {
                    this.player.on("ready", th.videoready);
                    this.player.on("timeupdate", function (currentTime, totalTime) {
                        th.video.total = parseInt(totalTime);
                        th.video.playTime = currentTime; //详细时间，精确到毫秒
                        th.playtime = parseInt(currentTime);
                        //学习完成度，最大为百分百
                        th.video.percent = Math.floor(th.video.studytime <= 0 ? 0 : th.video.studytime / th.video.total * 1000) / 10;
                        th.video.percent = th.video.percent > 100 ? 100 : th.video.percent;
                    });
                    this.player.on("seeked", function () {
                        th.playtime = th.playready() ? th.player.currentTime : 0;
                        window.setTimeout(function () {
                            if (th.playready()) th.player.pause();
                        }, 50);

                    });
                }
            },
            //页面刷新
            pagefresh: function () {
                //alert("页面刷新");
                window.location.reload();
            },
            //播放器是否准备好
            playready: function () {
                var th = this;
                if (this.player != null) {
                    return th.player._isReady && th.player.engine;
                }
                return false;
            },
            //视频播放跳转
            videoSeek: function (second) {
                if (this.playready()) this.player.seek(second);
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
                window.setInterval(function () {
                    var video = document.querySelector("video");
                    if (video == null) return;
                    video.setAttribute("type", "video/mp4");
                    if (!$api.isWeixin()) {
                        video.setAttribute("x5-playsinline", "true");
                        video.setAttribute("playsinline", "true");
                        video.setAttribute("webkit-playsinline", "true");
                        video.removeAttribute("controls");
                        //video.setAttribute("x5-video-player-fullscreen", "true");
                        video.setAttribute("x5-video-orientation", "portraint");
                        video.setAttribute("controlsList", "nodownload");
                    } else {
                        video.setAttribute("x-webkit-airplay", true);
                        video.setAttribute("x5-video-player-type", "h5");
                    }
                }, 3000);
                //给video对象增加属性
                var video = document.querySelector("video");
            },
            //学习记录记录
            videoLog: function (per) {
                if (this.studylogUpdate) return;
                var th = this;
                var interval = 1; //间隔百分比多少递交一次记录
                if (this.video.total <= 5 * 60) interval = 10; //5分钟内的视频
                else if (this.video.total <= 10 * 60) interval = 5; //10分钟的视频，5%递交一次      
                else if (this.video.total <= 30 * 60) interval = 2; //30分钟的视频，2%递交一次 
                if (per > 0 && per < (100 + interval) && per % interval == 0) {
                    $api.post("Course/StudyLog", {
                        couid: this.outline.Cou_ID,
                        olid: this.outline.Ol_ID,
                        playTime: this.playtime,
                        studyTime: this.video.studytime,
                        totalTime: this.video.total
                    }, function () {
                        th.studylogUpdate = true;
                    }, function () {
                        th.studylogUpdate = false;
                    }).then(function (req) {
                        if (!req.data.success) {
                            if (th.playready()) {
                                th.player.pause();
                                th.player.destroy();
                                th.player = null;
                            }
                            alert(req.data.message);
                            return;
                        }
                        th.studylogState = 1;
                        window.setTimeout(function () {
                            th.studylogState = 0;
                        }, 2000);
                    }).catch(function (err) {
                        th.studylogState = -1;
                        alert('登录失效，同一账号不可以同时登录多个设备');
                        window.setTimeout(function () {
                            th.studylogState = 0;
                        }, 2000);
                    });
                }
            }
        },
        template: `<div class='videobox'>
        <div class='loading' v-show='state_loading'>
            <van-loading size='24px' type='spinner'>加载中...</van-loading>
        </div>
     
            <div remark='视频'  :video='state.urlVideo' v-show='state.isLogin && state.existVideo && !state.isLive'>
                <div id='videoplayer' v-show='!outline.Ol_ID || (state.existVideo && !state.otherVideo && !state.isLive)'
                remark='点播'></div>
                <iframe remark='外部视频链接' id='vedioiframe' height='100%' width='100%'
                v-if='state.outerVideo && state.otherVideo && !state.isLive' :src='state.urlVideo'
                allowscriptaccess='always' allowfullscreen='true' wmode='opaque' allowtransparency='true'
                frameborder='0' type='application/x-shockwave-flash'></iframe>
                <div id='videoinfo' v-if='!state.otherVideo && !state.isLive' style='display: none;'>
                    <span style='display: none'>视频时长：{{video.total}}秒，播放进度：{{playtime}}秒，</span>
                    <span>累计学习{{video.studytime}}秒，完成{{video.percent}}%，</span>
                    <span style='cursor: pointer' v-on:click='videoSeek(video.playhistime)'>上次播放到{{video.playhistime}}秒</span>
                    <span class='videolog info' v-show='studylogState==1'> 学习进度提交成功!</span >
                    <span class='videolog error' v-show='studylogState==-1'>学习进度提交失败!</span>
                </div>
            </div>
            <div remark='直播' v-show='state.isLogin && state.isLive' :video='state.urlVideo'>
                <div id='livebox' v-show='state.isLive && state.isLiving'></div>
                <div id='liveStopbox' v-show='state.isLive && !state.isLiving' remark='直播未开始'>
                    <div class='liveStop_Tit' v-show='state.canStudy && !state.LiveStart'>直播未开始！</div>
                    <div class='liveStop_Tit' v-show='state.canStudy && state.LiveOver'>直播已经结束！</div>
                    <div class='liveStop_Tit' v-show='!state.canStudy'>无权阅览！</div>
                </div>
            </div>
            <div remark='没有视频' id='noVideo' v-if='!state.existVideo && !state.isLive'>
                <div v-if="error!=null || error!='' ">{{error}}</div>
                <template v-else>
                    <span v-if='!state.isLogin'><a :href="commonaddr('signin')">未登录，点击此处登录！</a></span>
                    <span v-else-if='!state.canStudy'>不允许学习相关内容</span> 
                </template>
            </div>
      
	</div>`
    });



}, ['/Utilities/Qiniuyun/qiniu-web-player-1.2.3.js',
    'Components/outline_tree.js',
    'Components/progress_video.js',
    'Components/accessory.js',
    'Components/study_video.js']);

