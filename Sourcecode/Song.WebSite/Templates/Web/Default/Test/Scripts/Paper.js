document.oncontextmenu = function () {
    return false;
}
$ready(function () {

    window.vapp = new Vue({
        el: '#vapp',
        data: {
            id: $api.dot(),      //试卷id
            account: {},     //当前登录账号
            servertime: {},      //服务端时间

            platinfo: {},
            organ: {},
            config: {},      //当前机构配置项      

            paper: {},      //试卷对象
            course: {},          //课程对象

            studied: false,        //是否可以学习课程
            purchase: {},        //课程购买的记录
            purchase_query: { 'couid': -1, 'stid': -1 },
            //测试成绩
            results: [],
            result_query: {
                'stid': -1, 'tpid': $api.dot(), 'size': 12, 'index': 1
            },
            total: 1, //总记录数
            totalpages: 1, //总页数

            loading: false,
            loading_id: 0,           //单行的预载
            loading_result: true //加载成绩的预载
        },
        mounted: function () {
            var th = this;
            th.loading = true;
            $api.bat(
                $api.get('Platform/ServerTime'),
                $api.get('TestPaper/ForID', { 'id': th.id })
            ).then(axios.spread(function (time, paper) {
                th.loading = false;
                //判断结果是否正常
                for (var i = 0; i < arguments.length; i++) {
                    if (arguments[i].status != 200)
                        console.error(arguments[i]);
                    var data = arguments[i].data;
                    if (!data.success && data.exception != null) {
                        console.error(data.exception);
                        throw arguments[i].config.way + ' ' + data.message;
                    }
                }
                //服务器端时间
                th.servertime = time.data.result;
                //试卷
                th.paper = paper.data.result;
                document.title = th.paper.Tp_Name;
                th.getcourse(th.paper.Cou_ID);
                //购买记录查询参数中的课程id
                th.purchase_query.couid = th.paper.Cou_ID;
                //是否可以学习当前课程
                $api.get('Course/Studied', { 'couid': th.paper.Cou_ID }).then(function (req) {
                    if (req.data.success) {
                        th.studied = req.data.result;
                    } else {
                        console.error(req.data.exception);
                        throw req.config.way + ' ' + req.data.message;
                    }
                }).catch(function (err) {
                    //alert(err);
                    Vue.prototype.$alert(err);
                    console.error(err);
                });

            })).catch(function (err) {
                th.loading = false;
                Vue.prototype.$alert(err);
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
            //试卷是否存在
            isexist: function () {
                return JSON.stringify(this.paper) != '{}' && this.paper != null;
            },
            //是否过期，过期返回true
            isoverdue: function () {
                //if(this.course.Cou_IsFree)return false;
                var isbuy = JSON.stringify(this.purchase) != '{}' && this.purchase != null;
                if (!isbuy) return true;
                if (this.purchase.Stc_IsFree) return false;
                if (this.purchase.Stc_EndTime > this.servertime) return false;
                return true;
            },
            //是否存在结课考试
            final: function () {
                return JSON.stringify(this.paper) != '{}' && this.paper.Tp_IsFinal;
            },
            //可以学习
            canstudy: function () {
                return this.studied || this.course.Cou_IsFree || this.course.Cou_IsLimitFree;
            }
        },
        watch: {
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
                if (query.couid <= 0 || query.stid <= 0) return;
                th.loading = true;
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
                    //Vue.prototype.$alert(err);
                    console.error(err);
                });
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
                var url = "/Web/Test/doing";
                url = $api.url.set(url, {
                    'tpid': this.id,
                    'couid': this.course.Cou_ID
                });
                window.location.href = url;
            },
            btn_buy: function () {
                var url = "/web/course/buy.";
                url = $api.dot(this.course.Cou_ID,url);
                window.location.href = url;
            },
            //结课考试的按钮事件
            btn_final: function () {
                var disabled = this.final_disable();
                if (disabled) return;
                this.btn_test();
            },
            //结果考试的按钮是否通过,为true时表示不通过
            final_disable: function () {
                var final_condition = this.$refs["final_condition"];
                return final_condition.final_disable();
            },
            //成绩回顾的链接
            btnReview: function (item) {
                var url = '/student/test/Review?tr=121&tp=56&couid=132';
                url = $api.url.set(url, {
                    'stid': item.Ac_ID,
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
            },
            //删除成绩
            btnDelete: function (item) {
                var th = this;
                th.loading_id = item.Tr_ID;
                $api.delete('TestPaper/ResultDelete', { 'trid': item.Tr_ID }).then(function (req) {
                    th.loading_id = -1;
                    if (req.data.success) {
                        var result = req.data.result;
                        th.getresults();
                    } else {
                        console.error(req.data.exception);
                        throw req.config.way + ' ' + req.data.message;
                    }
                }).catch(function (err) {
                    th.loading_id = -1;
                    Vue.prototype.$alert(err);
                    console.error(err);
                });
            }
        }
    });

}, ["../Components/courses.js",
    '../scripts/pagebox.js',
    "Components/final_condition.js"]);
$dom.load.css([$dom.path() + 'styles/pagebox.css']);