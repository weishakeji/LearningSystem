$ready(function () {

    window.vapp = new Vue({
        el: '#vapp',
        data: {
            id: $api.dot(),      //试卷id
            account: {},     //当前登录账号

            platinfo: {},
            organ: {},
            config: {},      //当前机构配置项      

            paper: {},      //试卷对象
            course: {},          //课程对象

            purchase: {},        //课程购买的记录
            purchase_query: { 'couid': -1, 'stid': -1 },
            //测试成绩
            results: [],
            result_query: {
                'stid': -1, 'tpid': $api.dot(), 'size': 12, 'index': 1
            },
            total: 1, //总记录数
            totalpages: 1, //总页数

            loading: true,
            loading_result: true //加载成绩的预载
        },
        mounted: function () {
            console.log(this.organ);
        },
        created: function () {

        },
        computed: {
            //是否登录
            islogin: function () {
                return JSON.stringify(this.account) != '{}' && this.account != null;
            },
            //是否存在结课考试
            final: function () {
                return JSON.stringify(this.paper) != '{}' && this.paper.Tp_IsFinal;
            }
        },
        watch: {
            'id': {
                handler: function (nv, ov) {
                    this.gettestpaper();
                },
                immediate: true
            },
            'organ': {
                handler: function (nv, ov) {
                    console.log(nv);
                    console.log(this.config);
                },
                immediate: true
            },
            //当学员登录后
            'account': {
                handler: function (nv, ov) {
                    if (nv && this.islogin) {
                        this.result_query.stid = nv.Ac_ID;
                        this.getresults(1);
                        //购买记录查询参数中的学员id
                        this.purchase_query.stid = nv.Ac_ID;
                    }
                },
                immediate: true
            },
            //当购买课程的查询项变更时，获取课程购买记录
            'purchase_query': {
                handler: function (nv, ov) {
                    this.getpurchase(nv);
                },
                deep: true
            },
        },
        methods: {
            //获取试卷
            gettestpaper: function () {
                var th = this;
                th.loading = true;
                $api.get('TestPaper/ForID', { 'id': th.id }).then(function (req) {
                    th.loading = false;
                    if (req.data.success) {
                        th.paper = req.data.result;
                        document.title = th.paper.Tp_Name;
                        th.getcourse(th.paper.Cou_ID);
                        //购买记录查询参数中的课程id
                        th.purchase_query.couid = th.paper.Cou_ID;
                    } else {
                        console.error(req.data.exception);
                        throw req.config.way + ' ' + req.data.message;
                    }
                }).catch(function (err) {
                    th.loading = false;
                    Vue.prototype.$alert(err);
                    console.error(err);
                });
            },
            //获取课程
            getcourse: function (couid) {
                var th = this;
                $api.get('Course/ForID', { 'id': couid }).then(function (req) {
                    if (req.data.success) {
                        th.course = req.data.result;
                    } else {
                        console.error(req.data.exception);
                        throw req.config.way + ' ' + req.data.message;
                    }
                }).catch(function (err) {
                    //alert(err);
                    Vue.prototype.$alert(err);
                    console.error(err);
                });
            },
            //获取成绩
            getresults: function (index) {
                if (index != null) this.result_query.index = index;
                var th = this;
                th.loading_result = true;
                $api.get('TestPaper/ResultsPager', th.result_query).then(function (req) {
                    th.loading_result = false;
                    if (req.data.success) {
                        th.total = req.data.total;
                        th.results = req.data.result;
                    } else {
                        console.error(req.data.exception);
                        throw req.data.message;
                    }

                }).catch(function (err) {
                    th.loading_result = false;
                    th.error = err;
                    console.error(err);
                });
            },
            //获取购买课程的记录
            getpurchase: function (query) {
                var th = this;
                th.loading = true;
                if (query.couid <= 0 || query.stid <= 0) return;
                $api.get('Course/Purchaselog:5', query).then(function (req) {
                    th.loading = false;
                    if (req.data.success) {
                        th.purchase = req.data.result;
                        console.log(th.purchase);
                    } else {
                        console.error(req.data.exception);
                        throw req.data.message;
                    }
                }).catch(function (err) {
                    th.loading = false;
                    Vue.prototype.$alert(err);
                    console.error(err);
                });
            },
            //获取机构的配置参数
            orgconfig: function (para, def) {
                var val = this.config[para];
                if (!val) return def ? def : '';
                return val;
            },
            //最高得分
            score_highest: function () {
                if (this.results.length < 1) return;
                var highest = 0;
                for (let i = 0; i < this.results.length; i++) {
                    const el = this.results[i];
                    if (el.Tr_Score > highest) highest = el.Tr_Score;
                }
                return highest;
            },
            //得分样式
            scoreStyle: function (score) {
                //总分和及格分
                var total = this.paper.Tp_Total;
                var passscore = this.paper.Tp_PassScore;
                if (score == total) return "praise";
                if (score < passscore) return "nopass";
                if (score < total * 0.8) return "general";
                if (score >= total * 0.8) return "fine";
                return "";
            },
            //参加测试的按钮
            btn_test: function () {

            },
            //结课考试的按钮事件
            btn_final: function () {
                var disabled = this.final_disable();
                if (disabled) return;
                console.log(333);
            },
            //结果考试的按钮是否通过,为true时表示不通过
            final_disable: function () {
                //视频学习进度是否达成
                var condition_video = this.orgconfig('finaltest_condition_video', 100);
                if (condition_video > this.purchase.Stc_StudyScore) return true;
                //试题练习通过率是否达成
                var condition_ques = this.orgconfig('finaltest_condition_ques', 100);
                if (condition_ques > this.purchase.Stc_QuesScore) return true;
                //最多可以考几次
                var finaltest_count = this.orgconfig('finaltest_count', 1);
                if (finaltest_count <= this.results.length) return true;
                return false;
            },
            //成绩回顾的链接
            btnReview: function (item) {
                var url = '/student/test/Review?tr=121&tp=56&couid=132';
                url = $api.url.set(url, {
                    'tr': item.Tr_ID,
                    'tp': this.paper.Tp_Id,
                    'couid': this.course.Cou_ID
                });
                var obj =
                {
                    'url': url, 'ico': 'e6ef',
                    'pid': window.name,
                    'title': item.Tp_Name,
                    'width': '80%',
                    'height': '80%'
                }
                obj['showmask'] = true; //始终显示遮罩
                obj['min'] = false;
                var box = $pagebox.create(obj).open();
            }
        }
    });

}, ["../Components/courses.js",
    '../scripts/pagebox.js']);
$dom.load.css([$dom.path() + 'styles/pagebox.css']);