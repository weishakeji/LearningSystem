$ready(function () {

    window.vapp = new Vue({
        el: '#vapp',
        data: {
            couid: $api.querystring("couid") == "" ? $api.dot() : $api.querystring("couid"),
            organ: {},
            config: {},      //当前机构配置项       

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
                        document.title = n.Ol_XPath + n.Ol_Name + " - 《" + this.course.Cou_Name + '》 ';
                    }
                }
            }
        },
        computed: {
            //是否登录
            islogin: (t) => !$api.isnull(t.account),
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
            //获取当前机构
            $api.get('Organization/Current').then(function (req) {
                if (req.data.success) {
                    th.organ = req.data.result;
                    //机构配置信息
                    th.config = $api.organ(th.organ).config;
                } else {
                    console.error(req.data.exception);
                    throw req.config.way + ' ' + req.data.message;
                }
            }).catch(function (err) {
                alert(err);
                console.error(err);
            });
        },
        methods: {
            //初始化
            init: function () {
                var couid = this.couid;
                var th = this;
                $api.bat(
                    $api.get("Course/ForID", { id: couid }),
                    $api.get("Outline/TreeList", { couid: couid }),
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
            //是否显示
            display: function (show) {
                let visible = show == null ? true : show;
                $dom('#study_video').css('visibility', visible ? 'visible' : 'hidden');
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
                    //alert(err);
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


    //视频播放
    Vue.component('videoplayer', {
        props: ['outline', 'account', 'config'],
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
                //随机暂停
                pausevalue: [],         //要暂停的时间值
                pausesetup: true,       //是否随机暂停

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
                        //window.alert(err);
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
            //机构配置项
            'config': {
                deep: true, immediate: true,
                handler: function (nv, ov) {
                    if ($api.isnull(nv)) return;
                    this.pausesetup = nv.random_pause_setup ? true : false;
                    let val = nv.random_pause_value ? 0 : nv.random_pause_value;
                    if (this.pausesetup) this.pausevalue = this.buildrandom(nv.random_pause_value, this.video.total);
                }
            },
            //视频总时长变化时
            'video.total': {
                handler: function (nv, ov) {
                    if (nv <= 0) return;
                    //计算需要暂停的时间点
                    if (this.config.random_pause_setup)
                        this.pausevalue = this.buildrandom(this.config.random_pause_value, nv);
                    console.log(this.pausevalue);
                }, immediate: true
            },
            //播放进度变化
            playtime: function (val) {
                this.video.studytime++;
                //当前视频播放进度百分比
                var per = Math.floor(this.video.studytime <= 0 ? 0 : this.video.studytime / this.video.total * 100);
                this.playpercent = per;
                vapp.playinfo(this.outline.Ol_ID, this.outline.Cou_ID, val, per);
                if (val >= this.video.total) return this.completed();
                //是否需要暂停
                if (this.config.random_pause_setup) {
                    let ispause = this.pausevalue.find(item => item == val);
                    if (ispause != null && ispause > 0) {
                        this.pause();
                        this.$dialog.alert({
                            title: '随机暂停',
                            message: '点击确定，继续播放...',
                        }).then(() => {
                            this.play();
                        });
                    }
                }
                //触发视频事件
                //vapp.videoEvent(vapp.playtime);
            },
            //播放进度百分比变化，
            playpercent: function (val, oldval) {
                //console.log('当前播放进度百分比：'+val);
                //学习记录提交
                if (val <= 100) this.videoLog(val);
            },
        },
        computed: {
            //是否登录
            islogin: t => { return !$api.isnull(t.account); }
        },
        created: function () {

        },
        methods: {
            //是否显示
            display: function (show) {
                let visible = show == null ? true : show;
                $dom('.videobox').css('visibility', visible ? 'visible' : 'hidden');
            },
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
                window.location.reload();
            },
            //播放器是否准备好
            playready: function () {
                let player = this.player;
                if (player != null && player.engine) return player._isReady;
                return false;
            },
            //视频播放跳转
            seek: function (second) {
                if (this.playready()) this.player.seek(second);
            },
            //视频播放
            play: function () {
                if (this.playready() && this.islogin) {
                    this.player.play();
                }
            },
            //视频暂停
            pause: function () {
                if (this.playready()) {
                    this.player.pause();
                }
            },
            //生成随机数，平均分布，且不重复
            buildrandom: function (count, length) {
                if (count == null || length <= 0) return [];
                let part = count * 2 + 1;       //分成几段
                let len = Math.floor(length / part);   //每段多长
                let arr = [];
                for (let i = 0; i < count; i++) {
                    let random = Math.floor(Math.random() * len);
                    arr.push(len * (i * 2 + 1) + random);
                }
                return arr;
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
                    <span style='cursor: pointer' v-on:click='seek(video.playhistime)'>上次播放到{{video.playhistime}}秒</span>
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
    'Components/study_footer.js',
    'Components/course_message.js',
    'Components/outline_tree.js',
    'Components/progress_video.js',
    'Components/accessory.js',
    'Components/study_video.js',
    'Components/study_live.js',
    'Components/study_float.js']);

