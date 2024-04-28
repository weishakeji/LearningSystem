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
            courses: [],        //课程列表
            total: 1, //总记录数
            totalpages: 1, //总页数

            selects: [],     //被选中的课程

            loading_init: true,
            loading: false
        },
        mounted: function () {
            var th = this;
            $api.bat(
                $api.get('Account/Current'),
                $api.cache('Platform/PlatInfo:60'),
                $api.get('Organization/Current')
            ).then(axios.spread(function (account, platinfo, organ) {
                //获取结果
                th.account = account.data.result;
                if (th.account && !!th.account.Ac_ID) {
                    th.query.acid = th.account.Ac_ID;
                    th.handleCurrentChange(1);
                }
                th.platinfo = platinfo.data.result;
                th.organ = organ.data.result;
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
                var th = this;
                th.loading = true;
                if (index != null) this.query.index = index;
                //if (th.query.acid === undefined || th.query.acid == -1) return;
                var query = $api.clone(this.query);
                //每页多少条，通过界面高度自动计算
                var area = document.documentElement.clientHeight - 100;
                th.query.size = Math.floor(area / 213);
                $api.get("Course/ForStudent", query).then(function (req) {
                    if (req.data.success) {
                        th.total = req.data.total;
                        th.courses = req.data.result;
                    } else {
                        console.error(req.data.exception);
                        throw req.data.message;
                    }

                }).catch(function (err) {
                    th.error = err;
                    console.error(err);
                }).finally(() => th.loading = false);
            },
            //选择要打印的课程
            select: function (cour) {
                var exist = false;
                for (var i = 0; i < this.selects.length; i++) {
                    if (cour.Cou_ID == this.selects[i].Cou_ID) {
                        exist = true;
                        break;
                    }
                }
                if (!exist)
                    Vue.set(this.selects, this.selects.length, cour);
            },
            //取消选择
            cancel: function (cour) {
                if (cour == null) {
                    this.selects = [];
                } else {
                    for (var i = 0; i < this.selects.length; i++) {
                        if (cour.Cou_ID == this.selects[i].Cou_ID) {
                            this.selects.splice(i, 1);
                            break;
                        }
                    }
                }
            },
            //打印事件
            print: function () {
                if (!window.top || !window.top.vapp) return;
                var couids = '';
                for (var i = 0; i < this.selects.length; i++) {
                    couids += this.selects[i].Cou_ID;
                    if (i < this.selects.length - 1) {
                        couids += ',';
                    }
                }
                var url = $api.url.set("/student/Study/print." + this.account.Ac_ID,
                    { "courses": couids });
                var obj = {
                    'url': url,
                    'pid': window.name,
                    'ico': 'a046', 'max': true, 'resize': true, 'print': true,
                    'title': '学习证明打印',
                    'width': '1000px',
                    'height': '90%'
                }
                window.top.vapp.open(obj);
            }
        }
    });

}, ['Components/progress_value.js']);
