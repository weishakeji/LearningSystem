$ready(function () {
    //var couid = $api.querystring("couid");
    window.vapp = new Vue({
        el: '#vapp',
        data: {
            couid: $api.dot() != "" ? $api.dot() : $api.querystring("couid"),
            olid: $api.querystring("olid"),

            organ: {},
            config: {},      //当前机构配置项       
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
            islogin: t => !$api.isnull(t.account),
        },
        watch: {
            //左侧选项卡切换
            titState: function (val) {
                //视频播放的暂停
                var videoplayer = this.$refs['videoplayer'];
                //console.log(videoplayer);
                if (videoplayer != null && videoplayer.playready()) {
                    this.titState == 'existVideo' ? videoplayer.play() : videoplayer.pause();
                }
            },

        },
        methods: {
            //页面跳转,路径上增加当前页地址作为来源页
            gourl: function (url) {
                return $api.url.set(url, {
                    'referrer': encodeURIComponent(location.href)
                });
            },
            //切换章节
            changeOutline: function (state, outline, evtype) {
                this.state = state;
                this.outline = outline;
                //如果当前章节没有内容，则跳转到下一章节，直接跳转到有内容的章节
                if (state.isNull && evtype != 'click') {
                    let refoutline = this.$refs['rightArea'];
                    if (refoutline != null) {
                        let next = refoutline.nextnode(this.outline);
                        if (next != null) refoutline.outlineClick(next);
                    }
                }
            },
            //视频播放完成
            videocompleted: function (outline, state) {
                //右侧章节列表控件
                let oltree = this.$refs['rightArea'];
                let nextvideo = oltree.nextVideo(outline);
                if (nextvideo != null) {
                    var msg = "当前视频播放完成，是否进入下一个视频章节?<br/>";
                    //msg += "<span style='color:red'>" + window.video_completed_countdown + "</span> 后自动跳转。";
                    this.$confirm(msg, '完成', {
                        confirmButtonText: '确定',
                        cancelButtonText: '取消',
                        dangerouslyUseHTMLString: true,
                        closeOnClickModal: false,
                        type: 'success'
                    }).then(() => {
                        oltree.outlineClick(nextvideo);
                    }).catch(() => { });
                }
            }
        },
        created: function () {
            $dom.load.css([$dom.path() + 'styles/pagebox.css']);
            var th = this;
            //当前登录学员
            $api.login.current('account', function (acc) {
                th.account = acc;
                /*
                $api.login.fresh('account', null, () => {
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
                });*/
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