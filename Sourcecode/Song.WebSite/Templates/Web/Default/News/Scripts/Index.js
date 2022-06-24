$ready(function () {
    window.vapp = new Vue({
        el: '#vapp',
        data: {
            platinfo: {},
            org: {},
            config: {},      //当前机构配置项
            columns: [],        //新闻栏目++

            articles: [],         //新闻文章  
            notices: [],         //通知公告
            loading: false
        },
        mounted: function () {
            this.loading = true;
            $api.bat(
                $api.cache('Platform/PlatInfo'),
                $api.get('Organization/Current')
            ).then(axios.spread(function (platinfo, org) {
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

                vapp.platinfo = platinfo.data.result;
                vapp.org = org.data.result;
                //机构配置信息
                vapp.config = $api.organ(vapp.org).config;
                var orgid = vapp.org.Org_ID;
                $api.bat(
                    $api.get('Notice/ShowItems', { 'orgid': orgid, 'type': -1,'count': 4 }),
                    $api.get('News/ArticlesShow', { 'orgid': orgid, 'uid': '', 'count': 12, 'order': 'img' }),
                    $api.cache('News/ColumnsShow:60', { 'orgid': orgid, 'pid': '', 'count': 0 })
                ).then(axios.spread(function (notice, articles, columns) {
                    vapp.loading = false;
                    //判断结果是否正常
                    for (var i = 0; i < arguments.length; i++) {
                        if (arguments[i].status != 200)
                            console.error(arguments[i]);
                        var data = arguments[i].data;
                        if (!data.success && data.exception != null) {
                            console.error(data.exception);
                        }
                    }
                    //获取结果                
                    vapp.notices = notice.data.result;
                    var regex = /(<([^>]+)>)/ig
                    for (let i = 0; i < vapp.notices.length; i++) {
                        vapp.notices[i].No_Context = vapp.notices[i].No_Context.replace(regex, "");
                    }
                    vapp.articles = articles.data.result;
                    vapp.columns = columns.data.result;
                })).catch(function (err) {
                    vapp.loading = false;
                    console.error(err);
                });
            })).catch(function (err) {
                console.error(err);
            });
        },
        created: function () { },
        computed: {},
        watch: {},
        methods: {}
    });
}, ['Components/Articles.js',
    "../Components/subject_rec.js",
    'Components/SearchInput.js']);
