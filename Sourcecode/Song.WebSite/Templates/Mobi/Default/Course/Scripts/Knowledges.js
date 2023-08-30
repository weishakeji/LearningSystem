$ready(function () {

    window.vapp = new Vue({
        el: '#vapp',
        data: {
            sortid: $api.querystring("sortid"),       
            account: {},     //当前登录账号
            course: {},       //当前课程
            platinfo: {},
            org: {},
            config: {},      //当前机构配置项        

            loading: false,
            loading_init: true,
            datas: [],           //信息列表
            finished: false,
            query: {
                'couid': $api.querystring("couid"),
                'kns': $api.querystring("sortid"),
                'isuse': true,
                'search': $api.querystring("s"),
                'size': 2, 'index': 0
            },
            total: 0
        },
        mounted: function () {
            var th = this;
            $api.bat(               
                $api.cache('Course/ForID', { 'id': $api.querystring("couid", 0) })
            ).then(axios.spread(function (course) {
                th.course = course.data.result;
                if (th.iscourse)
                    document.title = th.course.Cou_Name + ' - ' + document.title;               
            })).catch(function (err) {
                console.error(err);
            });
        },
        created: function () {

        },
        computed: {
             //是否登录
             islogin: (t) => { return !$api.isnull(t.account); },
             //课程是否存在
             iscourse: (t) => { return !$api.isnull(t.course); }
        },
        watch: {
            'org': {
                handler: function (nv, ov) {
                    if ($api.isnull(nv)) return;
                    this.onload();
                    this.loading_init = false;
                }, immediate: true
            },
        },
        methods: {
            onload: function () {
                var th = this;
                th.query.index++;
                let query = $api.clone(this.query);             
                $api.get('Knowledge/Pager', query).then(function (req) {
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
            //进入详情页
            detail: function (id) {
                let url = $api.url.set('Knowledge', {
                    'id': id,
                    'couid': $api.querystring("couid")
                });
                window.navigateTo(url);
            }
        }
    });

}, ['Components/knlheader.js']);
