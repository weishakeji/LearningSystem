//直播
$dom.load.css([$dom.path() + 'course/Components/Styles/study_live.css']);
Vue.component('study_live', {
    props: ['account', 'state', 'outline'],
    data: function () {
        return {
            //imageBg: $dom.path() + 'course/images/LiveBg.jpg',
            playval: 0,
            abnormal: false,     //播放是否为不正常
            loading: false           //预载
        }
    },
    watch: {
        'state': {
            handler: function (nv, ov) {
                if (this.state.DeskAllow) return;
                //如果直播流不存在
                this.abnormal = !nv.isLiving;
                var th = this;
                this.$nextTick(function () {
                    //视频播放
                    if (th.state.canStudy && th.state.isLive)
                        th.startPlay(th.state);
                });
            }, immediate: true,
        },
        //是否播放异常
        'abnormal': function (nv, ov) {
            var th = this;
            if (nv) {
                window.abnormal_fresh = setInterval(function () {
                    th.fresh();
                }, 1000 * 10);
            } else {
                window.clearInterval(window.abnormal_fresh);
            }
        }
    },
    computed: {
        //是否登录
        islogin: function () {
            return JSON.stringify(this.account) != '{}' && this.account != null;
        }
    },
    mounted: function () {

    },
    methods: {
        //播放器是否准备好
        playready: function () {
            var player = window.live_player;
            if (player != null && player.engine) return player._isReady;
            return false;
        },
        //创建播放器
        createplayer: function (state) {
            if (window.live_player != null) window.live_player.destroy();
            //console.error(state.urlVideo);
            window.live_player = new QPlayer({
                url: state.urlVideo,
                container: document.getElementById("livebox"),
                isLive: true,
                autoplay: true
            });
            return window.live_player;
        },
        //获取播放器
        getplayer: function () {
            if (window.live_player != null) return window.live_player;
            return this.createplayer(this.state);
        },
        //视频播放
        play: function () {
            if (this.playready() && this.islogin) {
                window.live_player.play();
            }
        },
        //视频暂停
        pause: function () {
            if (this.playready()) {
                window.live_player.pause();
            }
        },
        startPlay: function (state) {
            var player = this.createplayer(state);
            var th = this;
            if (player != null) {
                player.on("loading", th.videoready);
                player.on("ready", th.videoready);
                player.on("timeupdate", function (currentTime, totalTime) {
                    th.playval = parseInt(currentTime);
                    //console.error(th.playval);
                });
                player.on("play", function (e) {
                    console.error(e);
                    if (window.videoFixed) window.live_player.pause();
                });
                player.on("error", function (e) {
                    console.error('error事件' + e);
                    //"levelLoadError"
                    if (e.code == 10002)
                        th.abnormal = true;
                });
                player.on("notification", function (e) {
                    console.error('notification事件' + e);
                });
            }
            return player;
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
        fresh: function () {
            if (this.loading) return;
            let liveid = this.state.LiveID;
            var th = this;
            th.loading = true;
            $api.get('Outline/Live', { 'liveid': liveid }).then(function (req) {
                if (req.data.success) {
                    var result = req.data.result;
                    if (result.isLiving) {
                        th.state.isLiving = result.isLiving;
                        th.startPlay(th.state);
                        th.play();
                        th.abnormal = false;
                    }
                } else {
                    console.error(req.data.exception);
                    throw req.config.way + ' ' + req.data.message;
                }
            }).catch(err => console.error(err))
                .finally(() => th.loading = false);
            // alert(liveid);
        }
    },
    template: `<div id="study_live"  :video="state.urlVideo">
        <div v-if="false"> {{state.isLiving}}<span>是否正常：{{!abnormal}}</span></div>
        <div id="livebox" v-show="state.isLive && state.isLiving" style="height: 100%;width:100%;"
            remark="直播">
        </div>
        <study_float :account="account" tag="study_live" remark="飘浮信息，防录屏"></study_float>
        <div id="liveStopbox" v-show="state.isLive && !state.isLiving" style="height: 100%;width:100%;"
            remark="直播未开始">
            <img class="bgPicture" style="height: 100%;width:100%;" />
                
        </div>
        <div id="abnormalbox" v-if="abnormal">  
            <div>
                <span @click="fresh">直播流异常，请点击此处刷新
                    <loading v-if="loading"></loading>
                </span>
                <div>
                    <span v-show="!state.LiveStart">直播可能还未开始！</span>
                    <span v-show="state.LiveOver">直播可能已经结束！</span>       
                </div>
            </div>
        </div>
    </div>`
});