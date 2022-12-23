$ready(function () {

    window.vapp = new Vue({
        el: '#vapp',
        data: {
            couid: $api.querystring("couid", 0),
            stid: $api.querystring("acid", 0),
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

            setup_show: false        //设置菜单是否显示
        },
        mounted: function () {
            var th = this;
            th.getAccount().then(function (d) {
                th.account = d;
                th.stid = d ? d.Ac_ID : 0;
                //创建试题练习状态的记录的操作对象
                th.state = $state.create(th.account.Ac_ID, th.couid, 0);
            }).catch(function (err) {
                Vue.prototype.$alert(err);
            });
            $api.bat(
                $api.cache('Question/Types:9999'),
                $api.cache('Course/ForID', { 'id': this.couid })
            ).then(axios.spread(function (types, course) {
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
                th.types = types.data.result;
                th.course = course.data.result;
                th.getQuestion(false);
            })).catch(function (err) {
                th.error = err;
                console.error(err);
            });

        },
        created: function () {

        },
        computed: {
            //是否登录
            islogin: function () {
                return JSON.stringify(this.account) != '{}' && this.account != null;
            },
        },
        watch: {
            //滑动试题，滑动到指定试题索引
            'swipeIndex': function (nv, ov) {
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
            }
        },
        methods: {
            //获取当前学员
            getAccount: async function () {
                var th = this;
                return new Promise(function (resolve, reject) {
                    var api = Number(th.stid) > 0 ? $api.get('Account/ForID', { 'id': th.stid }) : $api.get('Account/Current');
                    api.then(function (req) {
                        if (req.data.success) {
                            resolve(req.data.result);
                        } else {
                            console.error(req.data.exception);
                            throw req.config.way + ' ' + req.data.message;
                        }
                    }).catch(function (err) {
                        reject(err);
                    });
                });
            },
            //加载试题,update：否更新，true为更新，强制从服务器读取数据；false则读本地缓存       
            getQuestion: function (update) {
                var th = this;
                th.loading = true;
                var query = $api.get('Question/ErrorQues', { 'acid': th.stid, 'couid': th.couid, 'type': '' });
                query.then(function (req) {
                    //th.loading = false;
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
                    th.loading = false;
                    console.error(err);
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
            //试题答题状态变更时
            answer: function (state, ques) {
                var data = this.state.update(true);
                this.count = data.count;
            },

            //删除错题
            deleteQues: function () {
                var th = this;
                if (this.questions.length < 1) return;
                var ques = this.questions[this.swipeIndex];
                Vue.delete(this.questions, this.swipeIndex);
                if (this.swipeIndex > 0)
                    this.swipeIndex--;
                $api.get('Question/ErrorDelete', { 'acid': th.account.Ac_ID, 'qid': ques.Qus_ID }).then(function (req) {
                    if (req.data.success) {
                        var result = req.data.result;
                        th.$message.success('删除成功');
                    } else {
                        console.error(req.data.exception);
                        throw req.data.message;
                    }
                }).catch(function (err) {
                    alert(err);
                    console.error(err);
                });
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
