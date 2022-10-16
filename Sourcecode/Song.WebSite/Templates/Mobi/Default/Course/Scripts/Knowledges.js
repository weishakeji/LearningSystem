$ready(function () {

    window.vapp = new Vue({
        el: '#vapp',
        data: {
            sortid: $api.querystring("sortid"),
            sortuid: $api.querystring("sortuid"),
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
                'uid': $api.querystring("sortuid"),
                'isuse': true,
                'search': $api.querystring("s"),
                'size': 6, 'index': 0
            },
            total: 0
        },
        mounted: function () {
            $api.bat(
                $api.get('Account/Current'),
                $api.cache('Platform/PlatInfo:60'),
                $api.get('Organization/Current'),
                $api.cache('Course/ForID', { 'id': $api.querystring("couid", 0) })
            ).then(axios.spread(function (account, platinfo, organ, course) {
                vapp.loading_init = false;
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
                vapp.account = account.data.result;
                vapp.platinfo = platinfo.data.result;
                vapp.organ = organ.data.result;
                vapp.course = course.data.result;
                if (vapp.course)
                    document.title = vapp.course.Cou_Name + ' - ' + document.title;
                //机构配置信息
                vapp.config = $api.organ(vapp.organ).config;
                vapp.onload();
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
