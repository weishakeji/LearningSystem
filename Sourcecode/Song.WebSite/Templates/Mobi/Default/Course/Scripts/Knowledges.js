$ready(function () {

    window.vapp = new Vue({
        el: '#vapp',
        data: {
            sortid: $api.querystring("sortid"),       
            account: {},     //当前登录账号
            course: {},       //当前课程
            platinfo: {},
            organ: {},
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
                $api.get('Account/Current'),
                $api.cache('Platform/PlatInfo:60'),
                $api.get('Organization/Current'),
                $api.cache('Course/ForID', { 'id': $api.querystring("couid", 0) })
            ).then(axios.spread(function (account, platinfo, organ, course) {
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
                th.platinfo = platinfo.data.result;
                th.organ = organ.data.result;
                th.course = course.data.result;
                if (th.course)
                    document.title = th.course.Cou_Name + ' - ' + document.title;
                //机构配置信息
                th.config = $api.organ(th.organ).config;
                th.onload();
            })).catch(function (err) {
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
            onload: function () {
                var th = this;
                th.query.index++;
                var query = $api.clone(this.query);
                console.log(query);
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
                var url = $api.url.set('Knowledge', {
                    'id': id,
                    'couid': $api.querystring("couid")
                });
                window.location.href = url;
            }
        }
    });

}, ['Components/knlheader.js']);
