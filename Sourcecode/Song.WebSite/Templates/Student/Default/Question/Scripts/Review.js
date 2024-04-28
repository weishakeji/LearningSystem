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

            //一些数值         
            count: {
                answer: 0,      //答题数量
                correct: 0,     //正确数
                wrong: 0,           //错误数
                rate: 0         //正确率
            }
        },
        mounted: function () {
            var th = this;
            $api.bat(
                $api.get('Account/Current'),
                $api.cache('Question/Types:9999'),
                $api.cache('Course/ForID', { 'id': this.couid })
            ).then(axios.spread(function (account, types, course) {
                th.account = account.data.result;
                $state.setaccid(th.account.Ac_ID);
                th.types = types.data.result;
                th.course = course.data.result;
                th.getQuestion(false);
            })).catch(function (err) {
                th.error = err;
                console.error(err);
            }).finally(() => th.loading_init = false);

        },
        created: function () {
            window.addEventListener('resize', function () {
                $dom("section").hide();
                $dom("section").css('left', -($dom("#vapp").width() * vapp.swipeIndex) + 'px');
                window.setTimeout(function () {
                    $dom("section").show();
                }, 300);
            });
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
                    $state.last(ques.state);
                }
                $dom("section").css('left', -($dom("#vapp").width() * nv) + 'px');
            },
            //答题状态变更时，计算答题状态
            'questions': {
                deep: true,
                handler: function (nv, ov) {
                    if (!this.questions || this.questions.length < 1) return 0;
                    this.$nextTick(function () {
                        //记录答题状态，并返回答题数量    
                        this.count = $state.update(nv);
                    });
                }
            }
        },
        methods: {
            //加载试题,update：否更新，true为更新，强制从服务器读取数据；false则读本地缓存       
            getQuestion: function (update) {
                var th = this;
                th.loading = true;
                var query = $api.get('Question/ErrorQues', { 'acid': this.account.Ac_ID, 'couid': this.couid, 'type': '' });
                query.then(function (req) {
                    th.loading = false;
                    if (req.data.success) {
                        var result = req.data.result;
                        th.questions = $state.restore(result);
                        th.$nextTick(function () {
                            var last = $state.last();
                            if (last != null)
                                th.swipeIndex = last.index;
                        });
                        if (th.questions.length > 0)
                            th.$message({
                                message: '试题加载完成',
                                type: 'success',
                                showClose: true,
                                center: true,
                                duration: 2000,
                                offset: 100
                            });
                        console.log(result);
                    } else {
                        console.error(req.data.exception);
                        throw req.data.message;
                    }
                }).catch(function (err) {
                    th.error = err;
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
            //试题向右滑动 
            swiperight: function (e) {
                if (e) {
                    if (e && e.preventDefault) e.preventDefault();
                    var node = $dom(e.target ? e.target : e.srcElement);
                    if (node.hasClass("van-overlay") || node.hasClass("van-popup"))
                        return;
                }
                if (this.swipeIndex > 0) this.swipeIndex--;
            },
            //试题向左滑动
            swipeleft: function (e) {
                if (e) {
                    if (e && e.preventDefault) e.preventDefault();
                    var node = $dom(e.target ? e.target : e.srcElement);
                    if (node.hasClass("van-overlay") || node.hasClass("van-popup"))
                        return;
                }
                if (this.swipeIndex < this.questions.length - 1) this.swipeIndex++;
            },
            //切换背题模式
            switchmode: function () {
                this.learnmode++;
                this.learnmode = this.learnmode % 2;
                console.log(this.learnmode);
            },
            //删除错题
            deleteQues: function () {
                var ques = this.questions[this.swipeIndex];
                Vue.delete(this.questions, this.swipeIndex);
                var th = this;
                if (this.swipeIndex > 0)
                    this.swipeIndex--;
                $api.get('Question/ErrorDelete', { 'acid': th.account.Ac_ID, 'qid': ques.Qus_ID }).then(function (req) {
                    if (req.data.success) {
                        var result = req.data.result;
                        th.$message({
                            message: '删除成功',
                            type: 'success'
                        });
                    } else {
                        console.error(req.data.exception);
                        throw req.data.message;
                    }
                }).catch(function (err) {
                    alert(err);
                    console.error(err);
                });
            },
            //清除所有错题记录
            clearErrors: function (couid) {
                var acid = this.account.Ac_ID;
                couid = this.couid;
                var th = this;
                th.loading = true;
                $api.delete('Question/ErrorClear', { 'acid': acid, 'couid': couid }).then(function (req) {
                    if (req.data.success) {
                        th.$message({
                            message: '清除成功，关闭窗口',
                            type: 'success'
                        });
                        th.operateSuccess();
                    } else {
                        console.error(req.data.exception);
                        throw req.data.message;
                    }
                }).catch(err => console.error(err))
                    .finally(() => th.loading = false);
            },
            //操作成功
            operateSuccess: function () {
                window.top.vapp.shut(window.name, 'vapp.handleCurrentChange');
            }
        }
    });
}, ['Components/Question.js',
    'Components/Quesbuttons.js',
    'Components/AnswerCard.js',
    '/Utilities/Components/question/function.js',
    'Components/ExercisesState.js']);
