$ready(function () {

    window.vapp = new Vue({
        el: '#vapp',
        data: {
            search: $api.querystring("s"),
            uid: $api.dot(),        //新闻栏目uid      
            articles: [],            //新闻文章           
            finished: false,
            query: {
                'uid': '', 'search': this.search,
                'order': 'rec', 'size': 6, 'index': 0
            },
            total: 0,
            loading: false
        },
        mounted: function () { },
        created: function () {
        },
        computed: {},
        watch: {
            'search': {
                handler(newName, oldName) {
                    document.title = '检索：' + newName;
                    this.onload();
                },
                immediate: true
            }
        },
        methods: {
            onload: function () {
                var th = this;
                th.query.index++;
                th.query.uid = this.uid;
                th.query.search = this.search;
                var query = this.query;
                $api.get('News/ArticlePagerShow', query).then(function (req) {
                    th.loading = false;
                    if (req.data.success) {
                        th.total = req.data.total;
                        var result = req.data.result;
                        for (var i = 0; i < result.length; i++) {
                            th.articles.push(result[i]);
                        }
                        var totalpages = req.data.totalpages;
                        // 数据全部加载完成
                        if (th.articles.length >= th.total || result.length == 0) {
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
            }
        }
    });

}, ['Components/Articles.js',
    'Components/SearchInput.js']);
