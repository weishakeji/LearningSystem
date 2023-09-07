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
            //isMessage: true,         //是否启用留言咨询
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
            //是否启用留言咨询
            'isMessage': function () {
                return !(!!this.config.IsDisableChat ? this.config.IsDisableChat : false);
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
                    let result = info.data.result;
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

}, ['/Utilities/Qiniuyun/qiniu-web-player-1.2.3.js',
    'Components/study_footer.js',
    'Components/course_message.js',
    'Components/outline_tree.js',
    'Components/progress_video.js',
    'Components/accessory.js',
    'Components/study_video.js',
    'Components/study_live.js',
    'Components/study_float.js']);

