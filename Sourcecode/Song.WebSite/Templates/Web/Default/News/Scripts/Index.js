$ready(function () {
    window.vapp = new Vue({
        el: '#vapp',
        data: {
            account: {},
            platinfo: {},
            org: {},
            config: {},

            columns: [],        //新闻栏目++

            articles: [],         //新闻文章  
            notices: [],         //通知公告
            loading: false
        },
        mounted: function () {            
        },
        created: function () { },
        computed: {},
        watch: {
            'org': {
                handler: function (nv, ov) {
                    this.loadinit(nv.Org_ID);                   
                }, immediate: true
            }
        },
        methods: {
            //加载一些初始数据
            loadinit: function (orgid) {
                if (orgid == undefined) return;
                var th = this;
                th.loading = true;
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
                    th.notices = notice.data.result;
                    var regex = /(<([^>]+)>)/ig
                    for (let i = 0; i < th.notices.length; i++) {
                        th.notices[i].No_Context = th.notices[i].No_Context.replace(regex, "");
                    }
                    th.articles = articles.data.result;
                    th.columns = columns.data.result;
                })).catch(function (err) {
                    th.loading = false;
                    console.error(err);
                });
            },
        }
    });
}, ['Components/Articles.js',
    "../Components/subject_rec.js",
    'Components/SearchInput.js']);
