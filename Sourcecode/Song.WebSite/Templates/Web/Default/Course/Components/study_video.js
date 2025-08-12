//视频点播
//事件:
//completed:播放完成，参数：当前章节
//playing:播放中，每播一秒触发一次，参数：当前进度（单位秒），累计学习计时（单位：秒），完成度的百分比，
Vue.component('study_video', {
    props: ['account', 'state', 'outline', 'config'],
    data: function () {
        return {
            //当前章节的视频信息
            video: {
                url: '', //视频路径
                total: 0, //总时长      
                playTime: 0, //当前播放时间，单位：毫秒     
                playhistime: 0, //历史播放时间
                studytime: 0, //累计学习时间
                percent: 0 //完成度（百分比）
            },
            //随机暂停
            pausevalue: [],         //要暂停的时间值
            pausesetup: true,       //是否随机暂停

            playtime: 0, //当前播放时间，单位：秒
            playpercent: 0, //当前播放完成度百分比

            studylogUpdate: false, //学习记录是否在递交中
            studylogState: 0, //学习记录提交状态，1为成功，-1为失败
        }
    },
    watch: {
        'state': {
            handler: function (nv, ov) {
                this.video.studytime = isNaN(nv.StudyTime) ? 0 : nv.StudyTime;
                this.video.playhistime = isNaN(nv.PlayTime) ? 0 : nv.PlayTime / 1000;
                if (this.state.DeskAllow) return;
                var th = this;
                this.$nextTick(function () {
                    //视频播放
                    if (th.state.canStudy && (th.state.existVideo || th.state.isLive))
                        th.startPlay(th.state);
                });

            }, deep: true, immediate: true,
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
        //播放进度时间变化
        playtime: function (val) {
            this.video.studytime++;
            //当前视频播放进度百分比
            var per = Math.floor(this.video.studytime <= 0 ? 0 : this.video.studytime / this.video.total * 100);
            this.playpercent = per;
            //播放前进的事件，三个参数：当前播放的时秒进度（单位：秒），累计学习计时（单位：秒），完成度的百分比，
            this.$emit('playing', val, this.video.studytime, per);
            if (val >= this.video.total) return this.completed();
            //是否需要暂停
            if (this.config.random_pause_setup) {
                let ispause = this.pausevalue.find(item => item == val);
                if (ispause != null && ispause > 0) {
                    this.$alert('点击确定，继续播放...', '随机暂停', {
                        confirmButtonText: '确定',
                        showClose: false,
                        callback: action => this.play()
                    });
                    this.pause();
                    console.log('是否需要暂停：' + ispause);
                }
            }
            //触发视频事件
            //this.videoEvent(this.playtime);
        },
        //播放进度百分比变化，
        playpercent: function (val, oldval) {
            //console.log('当前播放进度百分比：'+val);
            //学习记录提交
            if (val <= 100) this.videoRecord(val);
        }
    },
    computed: {
        //是否登录
        islogin: t => { return !$api.isnull(t.account); }
    },
    mounted: function () {
        var css = $dom.path() + 'course/Components/Styles/study_video.css';
        $dom.load.css([css]);
    },
    methods: {
        //播放器是否准备好
        playready: function () {
            var player = window.video_player;
            if (player != null && player.engine) return player._isReady;
            return false;
        },
        //创建播放器
        createplayer: function (state) {
            if (window.video_player != null) window.video_player.destroy();
            window.video_player = new QPlayer({
                url: state.urlVideo,
                container: document.getElementById("videoplayer"),
                autoplay: true,
            });
            return window.video_player;
        },
        //获取播放器
        getplayer: function () {
            if (window.video_player != null) return window.video_player;
            return this.createplayer(this.state);
        },
        //视频播放跳转
        seek: function (second) {
            if (this.playready()) {
                var player = window.video_player;
                player.seek(second);
                if (player.isPause) player.play();
            }
        },
        //视频播放
        play: function () {
            if (this.playready() && this.islogin) {
                window.video_player.play();
            }
        },
        //视频暂停
        pause: function () {
            if (this.playready()) {
                window.video_player.pause();
            }
        },
        //播放完成
        completed: function () {
            if (this.state.isLive) return;
            this.$emit('completed', this.outline, this.state);
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
        //视频开始播放
        startPlay: function (state) {
            var player = this.createplayer(state);
            var th = this;
            if (player != null) {
                player.on("loading", th.videoready);
                player.on("ready", th.videoready);
                player.on("timeupdate", function (currentTime, totalTime) {
                    th.video.total = parseInt(totalTime);
                    th.video.playTime = currentTime; //详细时间，精确到毫秒
                    th.playtime = parseInt(currentTime);
                    //学习完成度，最大为百分百
                    th.video.percent = Math.floor(th.video.studytime <= 0 ? 0 : th.video.studytime / th.video.total * 1000) / 10;
                    th.video.percent = th.video.percent > 100 ? 100 : th.video.percent;
                });
                player.on("seeked", function () {
                    th.playtime = th.playready() ? player.currentTime : 0;
                    window.setTimeout(function () {
                        //if (th.playready()) player.pause();
                    }, 50);
                });
                player.on("play", function (e) {
                    if (window.videoFixed) window.video_player.pause();
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

        //学习记录提交到服务器
        videoRecord: function (per) {
            if (this.studylogUpdate) return;
            var interval = 1; //间隔百分比多少递交一次记录
            if (this.video.total <= 5 * 60) interval = 10; //5分钟内的视频
            else if (this.video.total <= 10 * 60) interval = 5; //10分钟的视频，5%递交一次      
            else if (this.video.total <= 30 * 60) interval = 2; //30分钟的视频，2%递交一次    
            var th = this;
            if (per > 0 && per < (100 + interval) && per % interval == 0) {
                if (!this.outline) return;
                $api.post("Course/StudyLog", {
                    couid: th.outline.Cou_ID, olid: th.outline.Ol_ID,
                    playTime: th.playtime, studyTime: th.video.studytime,
                    totalTime: th.video.total
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
    },
    template: `<div id="study_video"  :video="state.urlVideo"> 
        <div id="videoplayer" v-show="!state.otherVideo && !state.isLive"></div>
        <study_float :account="account" tag="study_video" remark="飘浮信息，防录屏"></study_float>

        <div id="videoinfo" v-show="!state.otherVideo && !state.isLive">
            <span>
                <span class="info" v-show="studylogState==1">学习进度提交成功!</span>
                <span class="error" v-show="studylogState==-1">学习进度提交失败!</span>
            </span>
            <span>
                <span v-if="video.total>0">视频时长：{{video.total}}秒，播放进度：{{playtime}}秒，</span>
                <span>累计学习{{video.studytime}}秒，完成{{video.percent}}%，</span>
                <span style="cursor: pointer" v-on:click="seek(video.playhistime)">上次播放到{{video.playhistime}}秒</span>     
            </span>       
        </div>
        <div id="videolog" v-show="!state.otherVideo && !state.isLive">
            <template v-for="i in video.total">
                <i>1</i>
            </template>
        </div>
        <iframe id="vedioiframe" height="100%" width="100%"
            v-if="state.outerVideo && state.otherVideo  && !state.isLive" :src="state.urlVideo"
            allowscriptaccess="always" allowfullscreen="true" wmode="opaque" allowtransparency="true"
            frameborder="0" remark="外部视频链接" type="application/x-shockwave-flash"></iframe>
    </div>`
});


window.addEventListener('blur', function () {
    var player = window.video_player;
    var playready = false;
    if (player != null && player.engine) playready = player._isReady;
    if (playready) {
        var vapp = window.vapp;
        if (!window.vapp.state.SwitchPlay)
            window.video_player.pause();
    }
});
window.addEventListener('focus', function () {
    //如果有视频事件弹出，则窗体获取焦点时，视频并不播放
    //if ($("div[type=MsgBox]").length > 0) return;
    var player = window.video_player;
    var playready = false;
    if (player != null && player.engine) playready = player._isReady;
    if (playready) {
        if ($dom('.el-message-box').length > 0) return;
        if (window.vapp.titState == 'existVideo' || window.vapp.titState == 'isLive') {
            if (window.vapp && window.vapp.islogin)
                window.video_player.play();
        }
    }
});
window.addEventListener('resize', function () {
    window.setTimeout(function () {
        var str = '';
        [22, 9, 4, 5, 15].forEach(x => str += String.fromCharCode(0x60 + x));
        var v = document.querySelector(str);
        if (!v) return;
        var styles = document.defaultView.getComputedStyle(v.parentNode, null);
        var posi = styles.getPropertyValue('position');
        window.videoFixed = posi == 'fixed';
        v.style.display = window.videoFixed ? 'none' : '';

        var player = window.video_player;
        var playready = false;
        if (player != null && player.engine) playready = player._isReady;
        if (playready) {
            //!window.videoFixed ? v.play() : v.pause();
            if (window.videoFixed) {
                v.pause();
            } else {
                var vapp = window.vapp;
                if (!(vapp && vapp.islogin && vapp.titState == 'existVideo')) return;
                v.play();
            }
        }
    }, 100);
});
