$ready(function () {

    window.vapp = new Vue({
        el: '#vapp',
        data: {
            account: {},     //当前登录账号
            platinfo: {},
            org: {},
            config: {},      //当前机构配置项      


            loading: true,

            datas: [],
            finished: false,
            query: {
                'orgid': -1, 'search': decodeURIComponent($api.querystring('search')),
                'size': 3, 'index': 0
            },
            total: 0

        },
        mounted: function () {
        },
        created: function () {

        },
        computed: {
            //是否为空，即通知公告不存在
            isempty: function () {
                return !(JSON.stringify(this.data) != '{}' && this.data != null);
            }
        },
        watch: {
            'query.search': function (nv, ov) {
                var url = $api.setpara('search', encodeURIComponent(this.query.search));
                history.pushState({}, "", url); //更改地址栏信息     
                this.query.index = 0;
                this.finished = false;
                this.total = 0;
                this.datas = [];
                //this.onSearch();
            },
            'org': {
                handler: function (nv, ov) {
                    if ($api.isnull(nv)) return;
                    this.query.orgid = nv.Org_ID;
                    this.loading = false;
                }, immediate: true
            },
        },
        methods: {
            onSearch: function () {
                var url = $api.setpara('search', encodeURIComponent(this.query.search));
                history.pushState({}, "", url); //更改地址栏信息              
                this.query.index = 0;
                this.finished = false;
                this.total = 0;
                this.datas = [];
                //this.onload();
            },
            onload: function () {
                var th = this;
                th.query.index++;
                console.log(th.query.index);
                if (th.query.orgid === undefined || th.query.orgid == -1) return;
                $api.cache('Notice/ShowPager', th.query).then(function (req) {
                    if (req.data.success) {
                        th.total = req.data.total;
                        let result = req.data.result;
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
                    th.error = err;
                    console.error(err);
                }).finally(() => th.loading = false);
            },
            //转向通知公告详情页
            gonotice: function (id) {
                window.navigateTo('Detail.' + id);
            }
        }
    });

});
