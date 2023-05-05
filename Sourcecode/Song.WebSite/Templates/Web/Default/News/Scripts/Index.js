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
                    $api.get('News/ArticlesShow', { 'orgid': orgid, 'uid': '', 'count': 12, 'order': 'img' }),
                    $api.cache('News/ColumnsShow:60', { 'orgid': orgid, 'pid': '', 'count': 0 })
                ).then(axios.spread(function (articles, columns) {
                    vapp.loading = false;
                    //获取结果
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
