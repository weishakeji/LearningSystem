$ready(function () {

    window.vapp = new Vue({
        el: '#vapp',
        data: {
            account: {},     //当前登录账号
            platinfo: {},
            organ: {},
            config: {},      //当前机构配置项       
            query: {
                'acid': -1, 'search': '', 'size': 10, 'index': 0
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
            ).then(axios.spread(function (account, platinfo, organ) {
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
                if (th.account && !!th.account.Ac_ID)
                    th.query.acid = th.account.Ac_ID;
                th.platinfo = platinfo.data.result;
                th.organ = organ.data.result;
                //机构配置信息
                th.config = $api.organ(th.organ).config;
                th.handleCurrentChange();

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
            'method_name': {
                handler: function (nv, ov) {
                    this.handleCurrentChange(1);
                }, immediate: true
            }
        },
        methods: {
            handleCurrentChange: function (index) {
                var th = this;
                th.loading = true;
                if (index != null) this.query.index = index;
                if (th.query.acid === undefined || th.query.acid == -1) return;
                //var query = $api.clone(this.query);
                //每页多少条，通过界面高度自动计算
                var area = document.documentElement.clientHeight - 100;
                console.log(document.documentElement.clientHeight);
                th.query.size = Math.round(area / 200);
                var apiurl = "Course/" + this.method_name;
                $api.get(apiurl, th.query).then(function (req) {
                    th.loading = false;
                    if (req.data.success) {
                        th.total = req.data.total;
                        th.datas = req.data.result;
                    } else {
                        console.error(req.data.exception);
                        throw req.data.message;
                    }

                }).catch(function (err) {
                    th.loading = false;
                    th.error = err;
                    console.error(err);
                });
            },
            //查看结课成绩的详情
            viewScore: function (item) {
                if (!window.top || !window.top.vapp) return;
                var obj = {
                    'url': '/Student/Course/ScoreDetails.'+item.Cou_ID,
                    'ico': 'e6ef', 'min': false,
                    'title': '成绩详情 - ' + item.Cou_Name,
                    'width': '80%',
                    'height': '80%'
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
