$ready(function () {

    window.vapp = new Vue({
        el: '#vapp',
        data: {
            couid: $api.querystring("couid", 0),

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

            showCard: false,         //答题卡是否显示
            showCourse: false,           //显示课程
            setup_show: false        //设置菜单是否显示
        },
        mounted: function () {
            var th = this;
            $api.bat(
                $api.get('Account/Current'),
                $api.cache('Question/Types:9999'),
                $api.cache('Course/ForID', { 'id': this.couid })
            ).then(axios.spread(function (account, types, course) {
                th.loading_init = false;
                th.account = account.data.result;
                //创建试题练习状态的记录的操作对象
                th.state = $state.create(th.account.Ac_ID, th.couid, 0);
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
                $dom("section").css('left', -($dom("#vapp").width() * nv) + 'px');
                this.showCard = false;
            }
        },
        methods: {
            //加载试题,update：否更新，true为更新，强制从服务器读取数据；false则读本地缓存       
            getQuestion: function (update) {
                var th = this;
                th.loading = true;
                if (update) {
                    th.questions=[];
                    th.swipeIndex = 0;
                }
                var query = $api.get('Question/CollecQues', { 'acid': this.account.Ac_ID, 'couid': this.couid, 'type': '' });
                query.then(function (req) {
                    th.loading = false;
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
                            //如果没有历史练习记录
                            th.questions = req.data.result;
                            //显示操作指引的面板
                            //th.$refs['prompt'].show();
                        }).finally(function () {
                            th.loading = false;
                            if (th.questions.length > 0)
                                th.$toast.success('试题加载成功');
                        });

                    } else {
                        console.error(req.data.exception);
                        throw req.data.message;
                    }
                }).catch(function (err) {
                    vapp.error = err;
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
            pinchin: function (e) {
                if (e && e.preventDefault) e.preventDefault();
                this.$refs['setupmenu'].setFont(-1);
            },
            pinchout: function (e) {
                if (e && e.preventDefault) e.preventDefault();
                this.$refs['setupmenu'].setFont(1);
            },
            //试题答题状态变更时
            answer: function (state, ques) {
                var data = this.state.update(true);
                this.count = data.count;
            },
            //删除错题
            deleteQues: function () {
                var ques = this.questions[this.swipeIndex];
                Vue.delete(this.questions, this.swipeIndex);
                if (this.swipeIndex > 0)
                    this.swipeIndex--;
            }
        }
    });
}, ['/Utilities/Components/question/exercise.js',
    '/Utilities/Components/question/function.js',
    '/Utilities/Components/question/learnmode.js',
    'Components/Quesbuttons.js',
    'Components/AnswerCard.js',
    'Components/SetupMenu.js',
    'Components/PromptPanel.js',
    'Components/ExerciseState.js']);
