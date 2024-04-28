$ready(function () {

    window.vapp = new Vue({
        el: '#vapp',
        data: {
            account: {},     //当前登录账号
            organ: {},
            config: {},      //当前机构配置项  

            form: { 'acid': '', 'course': '', 'size': '', 'index': '' },
            courses: [],        //课程列表
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
                //获取结果
                th.account = account.data.result;
                th.organ = organ.data.result;
                th.form.acid = th.account.Ac_ID;
                th.handleCurrentChange(1);
                //机构配置信息
                th.config = $api.organ(th.organ).config;
            })).catch(err => console.error(err))
                .finally(() => th.loading_init = false);
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
                var area = document.documentElement.clientHeight - 100;
                th.form.size = Math.floor(area / 142);
                th.loading = true;
                $api.get("Question/ErrorCourse", th.form).then(function (d) {
                    if (d.data.success) {
                        th.courses = d.data.result;
                        th.totalpages = Number(d.data.totalpages);
                        th.total = d.data.total;
                        console.log(th.courses);
                    } else {
                        console.error(d.data.exception);
                        throw d.data.message;
                    }
                }).catch(err => console.error(err))
                    .finally(() => th.loading = false);
            },
            //清除所有错题记录
            clearErrors: function (couid) {
                var acid = this.account.Ac_ID;
                var th = this;
                th.loading = true;
                $api.delete('Question/ErrorClear', { 'acid': acid, 'couid': couid }).then(function (req) {
                    if (req.data.success) {
                        th.$message({
                            message: '清除成功，自动刷新数据',
                            type: 'success'
                        });
                        th.handleCurrentChange();
                    } else {
                        console.error(req.data.exception);
                        throw req.data.message;
                    }
                }).catch(err => console.error(err))
                    .finally(() => th.loading = false);
            },
            //打开错题详情的页面
            viewDetail: function (cour) {
                if (!window.top || !window.top.vapp) return;
                var url = $api.url.set("/web/Question/Error", {
                    "acid": this.account.Ac_ID,
                    "couid": cour.Cou_ID
                });
                var obj = {
                    'url': url,
                    'pid': window.name,
                    'ico': 'e75e', 'min': false, 'max': false, 'resize': false,
                    'title': '错题回顾 - ' + cour.Cou_Name,
                    'width': 500,
                    'height': '80%'
                }
                window.top.vapp.open(obj);
            }
        }
    });
    //成绩查看的考试项（用于成绩回顾）
    Vue.component('question_info', {
        props: ['acid', 'couid'],
        data: function () {
            return {
                questions: [],       //试题列表
                count: 0,             //试题数量
                loading: false
            }
        },
        watch: {
            'couid': {
                handler: function (nv, ov) {
                    this.getquestions();
                }, immediate: true, deep: true
            }
        },
        computed: {},
        mounted: function () {

        },
        methods: {
            //获取试题信息
            getquestions: function () {
                var th = this;
                th.loading = true;
                $api.cache('Question/ErrorQues', { 'acid': th.acid, 'couid': th.couid, 'type': -1 }).then(function (req) {

                    if (req.data.success) {
                        th.questions = req.data.result;
                        th.count = th.questions ? th.questions.length : 0;
                    } else {
                        console.error(req.data.exception);
                        throw req.config.way + ' ' + req.data.message;
                    }
                }).catch(err => console.error(err))
                    .finally(() => th.loading = false);
            }
        },
        template: "<slot :count='count' v-if='!loading'></slot>"
    });
});
