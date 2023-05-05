$ready(function () {

    window.vapp = new Vue({
        el: '#vapp',
        data: {
            account: {},     //当前登录账号
            platinfo: {},
            organ: {},
            config: {},      //当前机构配置项          
            loading: true,
            sear_str: '',

            search: decodeURIComponent($api.querystring('search')),
            datas: [],
            finished: false,
            query: {
                'orgid': -1, 'search': '', 'size': 3, 'index': 0
            },
            total: 0

        },
        mounted: function () {
            $api.bat(
                $api.get('Account/Current'),
                $api.cache('Platform/PlatInfo'),
                $api.get('Organization/Current')
            ).then(axios.spread(function (account, platinfo, organ) {
                //获取结果
                vapp.account = account.data.result;
                vapp.platinfo = platinfo.data.result;
                vapp.organ = organ.data.result;
                vapp.query.orgid = vapp.organ.Org_ID;
                //机构配置信息
                vapp.config = $api.organ(vapp.organ).config;
                vapp.onSearch();
            })).catch(function (err) {
                console.error(err);
            });
        },
        created: function () {
            this.sear_str = this.search;
            this.onload();
        },
        computed: {
            //是否为空，即通知公告不存在
            isempty: function () {
                return !(JSON.stringify(this.data) != '{}' && this.data != null);
            }
        },
        watch: {
            'sear_str': function (nv, ov) {
                if (nv == '') {
                    vapp.query.search = nv;
                    this.onSearch();
                }
            }
        },
        methods: {
            onSearch: function () {
                var url = $api.setpara('search', encodeURIComponent(this.sear_str));
                history.pushState({}, "", url); //更改地址栏信息
                this.query.search = this.sear_str;
                this.query.index = 0;
                this.finished = false;
                this.total = 0;
                this.datas = [];
                this.onload();
            },
            onload: function () {
                var th = this;
                th.query.index++;
                if (th.query.orgid === undefined || th.query.orgid == -1) return;
                var query = this.query;
                $api.cache('Notice/ShowPager', query).then(function (req) {
                    th.loading = false;
                    if (req.data.success) {
                        th.total = req.data.total;
                        var result = req.data.result;
                        for (var i = 0; i < result.length; i++) {
                            th.datas.push(result[i]);
                        }
                        var totalpages = req.data.totalpages;
                        // 数据全部加载完成
                        if (th.datas.length >= th.total || result.length == 0) {
                            th.finished = true;
                        }
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
            //转向通知公告详情页
            gonotice: function (id) {
                var url = 'Detail.' + id;
                window.location.href = url;
            }
        }
    });

});
