$ready(function () {

    window.vapp = new Vue({
        el: '#vapp',
        data: {
            account: {},     //当前登录账号
            platinfo: {},
            organ: {},
            config: {},      //当前机构配置项       
            query: {
                'acid': -1, 'search': '', 'enable': true, 'size': 10, 'index': 0
            },
            //左上角的按钮
            methods: [
                { 'name': '我的课程', 'label': 'purchased', 'icon': '&#xe813' },
                { 'name': '过期课程', 'label': 'overdue', 'icon': '&#xe671' },
                { 'name': '试学课程', 'label': 'ontrial', 'icon': '&#xe84d' }
            ],
            method_name: 'purchased',      //接口名称，来自选项卡的名称
            datas: [],           //课程列表
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
                $api.cache('Platform/PlatInfo'),
                $api.get('Organization/Current')
            ).then(([account, platinfo, organ]) => {
                //获取结果
                th.account = account.data.result;
                if (th.account && !!th.account.Ac_ID)
                    th.query.acid = th.account.Ac_ID;
                th.platinfo = platinfo.data.result;
                th.organ = organ.data.result;
                //机构配置信息
                th.config = $api.organ(th.organ).config;
                th.handleCurrentChange();

            }).catch(err => console.error(err))
                .finally(() => th.loading_init = false);
        },
        created: function () {

        },
        computed: {
            //是否登录
            islogin: t => !$api.isnull(t.account)
        },
        watch: {
            'method_name': {
                handler: function (nv, ov) {
                    this.handleCurrentChange(1);
                }, immediate: true
            }
        },
        methods: {
            handleCurrentChange: function (index) {
                var th = this;
                if (index != null) this.query.index = index;
                if (th.query.acid === undefined || th.query.acid == -1) return;
                th.loading = true;
                //var query = $api.clone(this.query);
                //每页多少条，通过界面高度自动计算
                var area = document.documentElement.clientHeight - 100;
                console.log(document.documentElement.clientHeight);
                th.query.size = Math.round(area / 200);
                var apiurl = "Course/" + this.method_name;
                $api.get(apiurl, th.query).then(function (req) {
                    if (req.data.success) {
                        th.total = req.data.total;
                        th.datas = [];
                        th.datas = req.data.result;
                    } else {
                        console.error(req.data.exception);
                        throw req.data.message;
                    }

                }).catch(function (err) {
                    th.error = err;
                    console.error(err);
                }).finally(() => th.loading = false);
            },
            //查看结课成绩的详情
            //参数：课程、学习记录
            viewScore: function (course, purchase) {
                if (!window.top || !window.top.vapp) return;
                var url = "/student/course/ScoreDetails";
                url = $api.url.dot(course.Cou_ID, url);
                url = $api.url.set(url, { 'stid': this.account.Ac_ID, 'stcid': purchase.Stc_ID });
                var obj = {
                    'url': url,
                    'ico': 'e6ef', 'min': false,
                    'title': '成绩详情 - ' + course.Cou_Name,
                    'width': '800px',
                    'height': '400px'
                }
                window.top.vapp.open(obj);
            }
        }
    });

}, ['Components/course_data.js',
    'Components/purchase_data.js',
    'Components/video_progress.js',
    'Components/ques_progress.js',
    'Components/exam_test.js']);
