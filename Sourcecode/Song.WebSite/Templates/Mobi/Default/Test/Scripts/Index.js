$ready(function () {

    window.vapp = new Vue({
        el: '#vapp',
        data: {
            account: {},     //当前登录账号
            platinfo: {},
            org: {},
            config: {},      //当前机构配置项 

            loading: false,
            loading_init: true,
            datas: [],           //信息列表
            finished: false,
            query: {
                'couid': $api.querystring("couid"),
                'search': $api.querystring("s"),
                'diff': -1,
                'size': 6, 'index': 0
            },
            total: 0
        },
        mounted: function () {

        },
        created: function () {

        },
        computed: {
            //是否登录
            islogin: (t) => { return !$api.isnull(t.account); }
        },
        watch: {
        },
        methods: {
            onload: function () {
                var th = this;
                th.query.index++;
                var query = $api.clone(this.query);
                //console.log(query);
                $api.get('TestPaper/ShowPager', query).then(function (req) {
                    th.loading = false;
                    if (req.data.success) {
                        th.total = req.data.total;
                        var result = req.data.result;
                        for (var i = 0; i < result.length; i++) {
                            if (!result[i].Tp_IsFinal)
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
            //页面跳转到试卷详情页
            gopaper: function (item) {
                var file = "Paper";
                var url = $api.dot(item.Tp_Id, file);
                url = $api.url.set(url, {
                    'couid': $api.querystring("couid")
                });
                //history.pushState({}, "", url);
                window.navigateTo(url);
            }
        }
    });

}, ['Components/TestHeader.js']);
