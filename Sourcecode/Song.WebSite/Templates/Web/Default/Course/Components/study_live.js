//直播
Vue.component('study_live', {
    props: ['account', 'state', 'outline'],
    data: function () {
        return {
            imageBg: $dom.path() + 'course/images/LiveBg.jpg'
        }
    },
    watch: {
        'state': function (nv, ov) {
            if (this.state.DeskAllow) return;
            //视频播放
            if (this.state.canStudy && this.state.isLive)
                this.startPlay(this.state);
        },
    },
    computed: {
        //是否登录
        islogin: function () {
            return JSON.stringify(this.account) != '{}' && this.account != null;
        }
    },
    mounted: function () {
        var css = $dom.path() + 'course/Components/Styles/study_live.css';
        $dom.load.css([css]);
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

                });
                player.on("play", function (e) {
                    if (window.videoFixed) window.live_player.pause();
                });
            }
        },
    },
    template: `<div id="study_live"  :video="state.urlVideo">
        <div id="livebox" v-show="state.isLive && state.isLiving" style="height: 100%;width:100%;"
            remark="直播">
        </div>
        <study_float :account="account" tag="study_live" remark="飘浮信息，防录屏"></study_float>
        <div id="liveStopbox" v-show="state.isLive && !state.isLiving" style="height: 100%;width:100%;"
            remark="直播未开始">
            <img :src="imageBg" class="bgPicture" style="height: 100%;width:100%;" />
            <div class="liveStop_Tit" v-show="!state.LiveStart">直播未开始！</div>
            <div class="liveStop_Tit" v-show="state.LiveOver">直播已经结束！</div>
        </div>
    </div>`
});