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
            results: [],
            query: {
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
            }
        },
        watch: {
            'organ': {
                handler: function (nv, ov) {
                    console.log(nv);
                },
                immediate: true
            },
            'account': {
                handler: function (nv, ov) {
                    if (nv) {
                        this.query.stid = nv.Ac_ID;
                        this.getresults(1);
                    }
                },
                immediate: true
            },
            'id': {
                handler: function (nv, ov) {
                    this.gettestpaper();
                },
                immediate: true
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
                if (index != null) this.query.index = index;
                var th = this;
                th.loading_result = true;
                $api.get('TestPaper/ResultsPager', th.query).then(function (req) {
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
            //成绩回顾
            btnReview: function (item) {
                var url = 'Review?tr=121&tp=56&couid=132';
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