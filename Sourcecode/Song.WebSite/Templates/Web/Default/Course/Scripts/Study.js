$ready(function () {
    //var couid = $api.querystring("couid");
    window.vapp = new Vue({
        el: '#vapp',
        data: {
            couid: $api.dot() != "" ? $api.dot() : $api.querystring("couid"),
            olid: $api.querystring("olid"),
            //数据实体
            account: {}, //当前账号信息
            course: {}, //当前课程
            outline: {}, //当前课程章节    
            owned: false,       //是否购买或学员组关联课程

            //状态
            state: {}, //课程章节的状态       来自study_outline组件中的change事件传值

            fold: false, //分隔线折叠状态
            titState: 'loading', //左侧选项卡的状态  

        },
        computed: {
            //是否登录
            islogin: function () {
                return JSON.stringify(this.account) != '{}' && this.account != null;
            }
        },
        watch: {
            //左侧选项卡切换
            titState: function (val) {
                //视频播放的暂停
                var videoplayer = this.$refs['videoplayer'];
                console.log(videoplayer);
                if (videoplayer != null && videoplayer.playready()) {
                    this.titState == 'existVideo' ? videoplayer.play() : videoplayer.pause();
                }
            },

        },
        methods: {

        },
        created: function () {
            $dom.load.css([$dom.path() + 'styles/pagebox.css']);
            var th = this;
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
                    th.titState = "noLogin";
                    th.account = {};
                });
                $api.get('Course/Owned', { 'couid': th.couid, 'acid': acc.Ac_ID }).then(function (req) {
                    if (req.data.success) {
                        th.owned = req.data.result;
                    } else {
                        console.error(req.data.exception);
                        throw req.config.way + ' ' + req.data.message;
                    }
                }).catch(function (err) {
                    alert(err);
                    console.error(err);
                });
            }).catch(() => { });

        },
        mounted: function () {

        },

    });
    window.onload = function () {

    }
    //

}, ['/Utilities/Qiniuyun/qiniu-web-player-1.2.3.js',
    'Components/study_tabs.js',         //学习界面顶部选项卡
    'Components/study_float.js',         //飘浮信息
    'Components/study_outline.js',      //学习界面右侧的章节列表
    'Components/study_video.js',        //视频点播
    'Components/study_event.js',        //视频事件
    'Components/study_live.js',         //直播
    'Components/study_chat.js',         //交流咨询
    'Components/progress_video.js',     //章节视频的学习进度
    'Components/accessory.js',      //课程附件
    '../scripts/pagebox.js']);