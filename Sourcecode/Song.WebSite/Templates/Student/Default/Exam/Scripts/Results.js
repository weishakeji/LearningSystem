$ready(function () {

    window.vapp = new Vue({
        el: '#vapp',
        data: {
            account: {},     //当前登录账号          
            organ: {},
            config: {},      //当前机构配置项        
            //查询项
            form: { 'acid': '', 'orgid': '', 'sbjid': '', 'search': '', 'size': 10, 'index': 0 },
            results: [],        //成绩记录
            total: 1, //总记录数
            totalpages: 1, //总页数

            loading_init: true,
            loading: false
        },
        mounted: function () {
            var th = this;
            th.loading_init = true;
            $api.bat(
                $api.get('Account/Current'),
                $api.get('Organization/Current')
            ).then(axios.spread(function (account, organ) {
                th.loading_init = false;
                //判断结果是否正常
                for (var i = 0; i < arguments.length; i++) {
                    if (arguments[i].status != 200)
                        console.error(arguments[i]);
                    var data = arguments[i].data;
                    if (!data.success && data.exception != null) {
                        console.error(data.message);
                    }
                }
                //获取结果
                th.account = account.data.result;
                th.form.acid = th.account.Ac_ID;
                th.organ = organ.data.result;
                th.handleCurrentChange(1);
                //机构配置信息
                th.config = $api.organ(vapp.organ).config;
            })).catch(function (err) {
                th.loading_init = false;
                console.error(err);
            });
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
        },
        methods: {
            //加载数据页
            handleCurrentChange: function (index) {
                if (index != null) this.form.index = index;
                var th = this;
                //每页多少条，通过界面高度自动计算
                var area = document.documentElement.clientHeight - 84;
                th.form.size = Math.floor(area / 210);
                th.loading = true;
                var loading = this.showloading();
                $api.get("Exam/Result4Student", th.form).then(function (d) {
                    th.loading = false;
                    if (d.data.success) {
                        th.results = d.data.result;
                        th.totalpages = Number(d.data.totalpages);
                        th.total = d.data.total;
                        /* //修改得分，方便查看界面效果
                        for (var i = 0; i < th.results.length; i++) {
                            th.results[i].Exr_ScoreFinal = 100;
                        }*/
                        th.$nextTick(function () {
                            loading.close();
                        });
                    } else {
                        console.error(d.data.exception);
                        throw d.data.message;
                    }
                }).catch(function (err) {
                    console.error(err);
                });
            },
            //查看成绩
            review: function (result) {
                if (!window.top || !window.top.vapp) return;
                var url = $api.url.set("/student/exam/review", {
                    "examid": result.Exam_ID,
                    "exrid": result.Exr_ID
                });
                var obj = {
                    'url': url,
                    'ico': 'e6ef', 'min': false,
                    'title': '成绩查看 - ' + result.Exam_Title,
                    'width': 900,
                    'height': '80%'
                }
                window.top.vapp.open(obj);
            },
            //显示全屏Loading
            showloading: function () {
                return this.$loading({
                    lock: true,
                    text: 'Loading',
                    spinner: 'el-icon-loading',
                    background: 'rgba(255, 255, 255, 0.3)'
                });
            }
        }
    });
    //成绩查看的考试项（用于成绩回顾）
    Vue.component('score_item', {
        props: ['result', 'index', 'account'],
        data: function () {
            return {
                paper: {},       //试卷
                subject: {},     //专业
                exam: {},        //考试
                loading: false
            }
        },
        watch: {
            'result': {
                handler: function (nv, ov) {
                    this.getDatas();
                }, immediate: true
            }
        },
        computed: {},
        mounted: function () {

        },
        methods: {
            //获取详情
            getDatas: function () {
                var th = this;
                //获取“试卷详情”
                $api.bat(
                    $api.cache('Exam/ForID', { 'id': this.result.Exam_ID }),
                    $api.cache('TestPaper/ForID', { 'id': this.result.Tp_Id }),
                    $api.cache('Subject/ForID', { 'id': this.result.Sbj_ID })
                ).then(axios.spread(function (exam, paper, subject) {
                    //判断结果是否正常
                    for (var i = 0; i < arguments.length; i++) {
                        if (arguments[i].status != 200)
                            console.error(arguments[i]);
                        var data = arguments[i].data;
                        if (!data.success && data.exception != null) {
                            console.error(data.exception);
                            throw data.message;
                        }
                    }
                    //获取结果
                    th.exam = exam.data.result;
                    th.paper = paper.data.result;
                    th.subject = subject.data.result;
                })).catch(function (err) {
                    console.error(err);
                });
            },
            //得分样式
            scoreStyle: function (score) {
                //总分和及格分
                var total = this.exam ? this.exam.Exam_Total : -1;
                var passscore = this.paper ? this.paper.Tp_PassScore : -1;
                if (score == total) return "praise";
                if (score < passscore) return "nopass";
                if (score < total * 0.8) return "general";
                if (score >= total * 0.8) return "fine";
                return "";
            }
        },
        template: `<slot :paper="paper" :exam="exam" :subject="subject" :th="this"></slot>`
    });
});
