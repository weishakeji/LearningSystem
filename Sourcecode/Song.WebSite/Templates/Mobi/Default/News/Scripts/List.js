$ready(function () {

    window.vapp = new Vue({
        el: '#vapp',
        data: {
            uid: $api.dot(),        //新闻栏目uid         
            column: {},          //栏目信息
            chilren: [],            //下级栏目
            articles: [],            //新闻文章           
            finished: false,
            query: {
                'uid': '', 'search': '',
                'order': 'top', 'size': 6, 'index': 0
            },
            total: 0,
            loading: false
        },
        mounted: function () {
            var th = this;
            $api.get('News/ColumnsChildren', { 'pid': this.uid, 'isuse': true }).then(function (req) {
                if (req.data.success) {
                    th.chilren = req.data.result;
                    console.log(th.chilren.length);
                } else {
                    console.error(req.data.exception);
                    throw req.data.message;
                }
            }).catch(function (err) {
                console.error(err);
            });
            $api.get('News/ColumnsForUID', { 'uid': this.uid }).then(function (req) {
                if (req.data.success) {
                    th.column = req.data.result;
                } else {
                    console.error(req.data.exception);
                    throw req.data.message;
                }
            }).catch(function (err) {
                alert(err);
                console.error(err);
            });
        },
        created: function () {

        },
        computed: {
        },
        watch: {},
        methods: {            
            onload: function () {
                var th = this;
                th.query.index++;
                th.query.uid = this.uid;              
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
