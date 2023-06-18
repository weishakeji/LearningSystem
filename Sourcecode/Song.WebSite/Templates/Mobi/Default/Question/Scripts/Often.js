
$ready(function () {

    window.vapp = new Vue({
        el: '#vapp',
        data: {
            couid: $api.querystring("couid", 0),
            olid: $api.querystring("olid", 0),

            learnmode: 0,            //练习模式，0为练习模式，1为背题模式        
            showCourse: false,           //显示课程  

            account: {},     //当前登录账号        
            types: [],          //试题类型
            course: {},         //当前课程
            outline: {},        //当前章节   
            error: '',           //错误信息       

            queslist: [],      //试题简要信息，只有题型与id,按题型分为多个数组
            loading: false,
            loading_init: false,         //初始信息加载

            fontsize: 0,         //字体增减值

            //答题的状态
            state: {},
            //一些数值         
            data: {
                num: 0,        //总数
                answer: 0,      //答题数量
                correct: 0,     //正确数
                wrong: 0,           //错误数
                rate: 0         //正确率
            },
            //
            starttime: new Date()    //起始时间，用于统计用时
        },
        mounted: function () {
            var th = this;
            th.loading_init = true;
            $api.bat(
                $api.get('Account/Current'),
                $api.cache('Question/Types:9999'),
                $api.cache('Course/ForID', { 'id': th.couid })
            ).then(axios.spread(function (acc, type, cou) {
                th.account = acc.data.result;
                th.types = type.data.result;
                th.course = cou.data.result;
                if (th.iscourse) document.title += th.course.Cou_Name;
                //如果登录状态，则加载试题
                if (th.islogin && th.iscourse) {
                    //创建试题练习状态的记录的操作对象
                    th.state = $state.create(th.account.Ac_ID, th.couid, 0);
                    //加载试题的id列表
                    th.getQuesSimplify(false);
                }
            })).catch(err => alert(err))
                .finally(() => th.loading_init = false);
        },
        created: function () {
            //当页面退出时，保存学习记录到服务器
            window.addEventListener('beforeunload', function (e) {
                window.vapp.state.toserver();
                e.preventDefault();
            });
            //加载当前课程各个章节的试题到缓存
            window.setTimeout(window.ques.get_cache_data(), 10 * 1000);
        },
        computed: {
            //是否有试题
            isques: (t) => { return !$api.isnull(t.queslist); },
            //是否登录
            islogin: (t) => { return !$api.isnull(t.account); },
            //课程是否加载正确
            iscourse: (t) => { return !$api.isnull(t.course); },
        },
        watch: {

        },
        methods: {
            //获取试题简要信息，只有试题类型与id
            //update:是否更新本地缓存数据
            getQuesSimplify: function (update) {
                var th = this;
                th.loading = true;
                //var query = $api.get('Question/ErrorQues', { 'acid': this.account.Ac_ID, 'couid': this.couid, 'type': '' });
                let form = { 'couid': th.couid, 'type': '', 'count': 1000 };
                let apiurl = 'Question/ErrorOftenQues:' + (query = update === false ? (60 * 24 * 30) : 'update');
                $api.get(apiurl, form).then(function (req) {
                    if (req.data.success) {
                        th.queslist = req.data.result;
                        if (!th.isques) throw ' 没有读取到数据';
                        //获取本地学习记录
                        th.state.gettolocal(req.data.result).then(function (d) {
                            th.data = d.count;
                            //初始显示第几条试题
                            th.$nextTick(function () {
                                let last = th.state.last();
                                let index = last != null ? last.index : 0;
                                th.$refs['quesarea'].setindex(null, index);
                                if (th.data.num > 0) {
                                    let span = new Date().getTime() - th.starttime.getTime();
                                    span = span / 1000;
                                    th.$toast.success({
                                        message: '试题加载完成\n 用时 ' + span.toFixed(2) + ' 秒',
                                        duration: 1500
                                    });
                                }
                            });
                            //获取服务器端的学习记录，如果本地最新则不再取值
                            th.state.restore(req.data.result).then(function (d) {
                                th.data = d.count;
                                th.$nextTick(function () {
                                    let last = th.state.last();
                                    let index = last != null ? last.index : 0;
                                    th.$refs['quesarea'].setindex(null, index);
                                });
                            }).catch(function (d) {
                                th.data = d.count;
                                //如果没有历史练习记录,显示操作指引的面板
                                th.$refs['prompt'].show();
                            });
                        });

                    } else {
                        console.error(req.data.exception);
                        throw req.config.way + ' ' + req.data.message;
                    }
                }).catch(err => console.error(err))
                    .finally(() => th.loading = false);
            },
            //更新试题
            updateQues: function () {
                if (window.temp_ques_list == null) {
                    const list = [];
                    for (let k in this.queslist) {
                        for (let i = 0; i < this.queslist[k].length; i++)
                            list.push(this.queslist[k][i]);
                    }
                    window.temp_ques_list = list;
                }
                var arr = window.temp_ques_list;
                if (arr.length < 1) return;
                //逐一更新试题
                var th = this;
                $api.cache('Question/ForID:update', { 'id': arr[0] }).then(function (req) {
                    //console.log(req);
                    //console.log('更新');
                }).catch(err => console.error(err))
                    .finally(() => {
                        if (arr.length > 0) arr.splice(0, 1);
                        if (arr.length > 0) window.setTimeout(th.updateQues(), 1000);
                    });
            },
            //手式捏合与缩放事件
            pinch: function (e) {
                if (e && e.preventDefault) e.preventDefault();
                //右上角的菜单组件，用来调用缩小与放大字符的方法
                let setupmenu = this.$refs['setupmenu'];
                if (!setupmenu) return;
                if (e.type == 'pinchin') setupmenu.setFont(-1);
                if (e.type == 'pinchout') setupmenu.setFont(1);
            },
            swipe: function (index) {
                let sheet = this.$refs['answersheet'];
                if (sheet) sheet.setindex(index + 1);
            }
        }
    });
}, ['/Utilities/Components/question/function.js',
    '/Utilities/Components/question/learnmode.js', //练习模式，答题或背题
    'Components/SetupMenu.js',          //右上角的设置项菜单 
    'Components/AnswerSheet.js',        //答题卡
    'Components/QuesArea.js',           //试题区域
    '/Utilities/Components/question/exercise.js',           //单个试题的展示
    'Components/PromptPanel.js',        //刚打开时的提示面板，手式操作的指引
    'Components/Quesbuttons.js',        //试题右上角的按钮，报错、笔记、收藏
    'Components/ExerciseState.js'       //记录学习状态
]);
