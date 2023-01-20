$ready(function () {

    window.vapp = new Vue({
        el: '#vapp',
        data: {
            couid: $api.querystring("couid", 0),
            olid: $api.querystring("olid", 0),
            back: Boolean($api.querystring("back", false)),  //是否显示返回图标

            account: {},     //当前登录账号        
            types: [],          //试题类型
            course: {},         //当前课程
            outline: {},        //当前章节   
            error: '',           //错误信息       

            questions: [],
            swipeIndex: 0,           //试题滑动时的索引，用于记录当前显示的试题索引号
            loading: true,
            loading_init: true,         //初始信息加载
            learnmode: 0,            //练习模式，0为练习模式，1为背题模式

            //答题的状态
            state: {},

            //一些数值         
            count: {
                answer: 0,      //答题数量
                correct: 0,     //正确数
                wrong: 0,           //错误数
                rate: 0         //正确率
            },

            showCourse: false,           //显示课程
            setup_show: false        //设置菜单是否显示
        },
        mounted: function () {
            var th = this;
            $api.bat(
                $api.get('Account/Current'),
                $api.cache('Question/Types:9999'),
                $api.cache('Course/ForID', { 'id': th.couid }),
                $api.cache('Outline/ForID', { 'id': th.olid })
            ).then(axios.spread(function (account, types, course, outline) {
                vapp.loading_init = false;
                //判断结果是否正常
                for (var i = 0; i < arguments.length; i++) {
                    if (arguments[i].status != 200)
                        console.error(arguments[i]);
                    var data = arguments[i].data;
                    if (!data.success && data.exception != null) {
                        console.error(data.message);
                    }
                }
                vapp.account = account.data.result;
                //创建试题练习状态的记录的操作对象
                if (th.islogin)
                    th.state = $state.create(th.account.Ac_ID, th.couid, th.olid);
                th.types = types.data.result;
                th.course = course.data.result;
                th.outline = outline.data.result;
                th.outline.Ol_XPath = $api.querystring('path');
                document.title = th.outline.Ol_Name;
                th.getQuestion(false);
            })).catch(function (err) {
                th.error = err;
                console.error(err);
            });

        },
        created: function () {
            if (window.ques) window.ques.get_cache_data();
            window.onresize = function () {
                $dom("section").hide();
                $dom("section").css('left', -($dom("#vapp").width() * vapp.swipeIndex) + 'px');
                window.setTimeout(function () {
                    $dom("section").show();
                }, 300);
            }
        },
        computed: {
            //是否登录
            islogin: function () {
                return JSON.stringify(this.account) != '{}' && this.account != null;
            },
        },
        watch: {
            //滑动试题，滑动到指定试题索引
            'swipeIndex': {
                handler: function (nv, ov) {
                    console.log(nv);
                    if (nv > this.questions.length - 1 || nv < 0) return;
                    //设置当前练习的试题
                    if (nv != null && this.questions.length > 0) {
                        var ques = this.questions[nv];
                        this.state.last(ques.Qus_ID, nv);
                    }
                    this.state.update(false);

                    window.setTimeout(function () {
                        $dom("section[remark]").css('left', -($dom("#vapp").width() * nv) + 'px');
                    }, 100);

                }, immediate: true
            }
        },
        methods: {
            //加载试题,update：否更新，true为更新，强制从服务器读取数据；false则读本地缓存       
            getQuestion: function (update) {
                if (this.couid == 0 || this.couid == 'undefined') return;
                var th = this;
                th.loading = true;

                var form = {
                    'couid': this.couid, 'olid': this.olid, 'type': -1, 'count': 0
                }
                var query = $api.cache('Question/ForCourse:' + (60 * 24 * 30), form);
                if (update === true) query = $api.cache('Question/ForCourse:update', form);
                query.then(function (req) {
                    if (req.data.success) {
                        //获取练习记录
                        th.state.restore().then(function (d) {
                            th.count = d.count;
                            //获取记录成功再赋值
                            th.questions = req.data.result;
                            //初始显示第几条试题
                            th.$nextTick(function () {
                                var last = th.state.last();
                                if (last != null) th.swipeIndex = last.index ? last.index : 0;
                            });
                        }).catch(function (d) {
                            th.count = d.count;
                            th.questions = req.data.result;
                        }).finally(function () {
                            th.loading = false;
                            if (th.questions.length > 0)
                                th.$message.success('试题加载成功');
                        });
                    } else {
                        console.error(req.data.exception);
                        throw req.data.message;
                    }
                }).catch(function (err) {
                    th.error = err;
                    //console.error(err);
                });
            },
            //清理本地缓存，但不刷新界面
            QuesCacheClear: function () {
                var form = {
                    'couid': this.couid, 'olid': this.olid, 'type': -1, 'count': 0
                }
                var query = $api.cache('Question/ForCourse:clear', form);
                query.then(function (req) {
                    console.log(req);
                });
            },
             //试题滑动 
             swipe: function (e) {
                if (e && e.preventDefault) {
                    e.preventDefault();
                    var node = $dom(e.target ? e.target : e.srcElement);
                    if (node.hasClass("van-overlay") || node.hasClass("van-popup"))
                        return;
                }
                //向左滑动
                if (e.direction == 2 && this.swipeIndex < this.questions.length - 1) this.swipeIndex++;
                //向右滑动
                if (e.direction == 4 && this.swipeIndex > 0) this.swipeIndex--;
            },
            //更新试题
            updateQues: function () {
                this.$confirm('将试题保持与服务器端同步', '更新试题', {
                    confirmButtonText: '确定',
                    cancelButtonText: '取消',
                    type: 'warning'
                }).then(function () {
                    vapp.setup_show = false;
                    this.getQuestion(true);
                }).catch(function () { });
            },
            //设置字体大小，默认16px，num为增减数字，例如-1
            setFont: function (num) {
                var size = 16;
                if (num == null) num == 0;
                ergodic($dom("section"), num);
                function ergodic(dom, num) {
                    var fontsize = parseInt(dom.css("font-size"));
                    fontsize = isNaN(fontsize) ? size : fontsize + num;
                    dom.css("font-size", fontsize + "px", true);
                    var child = dom.childs();
                    if (child.length < 1) return;
                    child.each(function (node) {
                        var n = $dom(this);
                        if (n.attr('no-font-size') != null) return true;
                        ergodic(n, num);
                    });
                }
            },
            //试题答题状态变更时
            answer: function (state, ques) {
                var data = this.state.update(true);
                this.count = data.count;
            }
        }
    });
}, ['/Utilities/Components/question/exercise.js',
    '/Utilities/Components/question/function.js',
    '/Utilities/Components/question/learnmode.js',
    'Components/Quesbuttons.js',
    'Components/AnswerCard.js',
    'Components/SetupMenu.js',
    'Components/ExerciseState.js']);
