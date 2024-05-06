$ready(function () {

    window.vapp = new Vue({
        el: '#vapp',
        data: {
            account: {},
            platinfo: {},
            org: {},
            config: {},

            arid: $api.dot(),        //新闻id
            article: {},        //新闻对象
            isformat: $api.storage('article_isformat') == 'true',         //是否格式化

            accessory: [],       //新闻附件
            column: {},          //栏目信息

            courses: [],     //课程列表
            loading: false
        },
        mounted: function () {
            var th = this;
            th.loading = true;
            $api.cache('News/Article', { 'id': this.arid }).then(function (req) {
                if (req.data.success) {
                    th.article = req.data.result;
                    document.title = th.article.Art_Title;
                    $api.bat(
                        $api.cache('News/ColumnsForUID', { 'uid': th.article.Col_UID }),
                        $api.cache('News/VisitPlusOne:60', { 'id': th.article.Art_ID }),
                        $api.cache("News/Accessory", { 'uid': th.article.Art_Uid })
                    ).then(([column, visit, accessory]) => {
                        //栏目信息
                        th.column = column.data.result;
                        //访问量加一，并给当前新闻加上这个数
                        th.article.Art_Number = visit.data.result;
                        //新闻附件
                        th.accessory = accessory.data.result;
                    }).catch(function (err) {
                        console.error(err);
                    });
                } else {
                    console.error(req.data.exception);
                    throw req.data.message;
                }
            }).catch(err => console.error(err))
                .finally(() => th.loading = false);
            this.getcourses();
        },
        created: function () {
            //this.isformat = $api.storage('isformat') == 'true';
        },
        computed: {
            //是否为空，即不存在
            isempty: function () {
                return !(JSON.stringify(this.article) != '{}' && this.article != null);
            }
        },
        watch: {
            //是否格式化
            'isformat': {
                handler: function (nv, ov) {
                    if (nv != null)
                        $api.storage('article_isformat', nv);
                }, immediate: false,
            }
        },
        methods: {
            //获取课程
            getcourses: function () {
                var th = this;
                $api.get('Course/ShowCount', { 'sbjid': -1, 'orgid': th.org.Org_ID, 'search': '', 'order': 'rec', 'count': 6 }).then(function (req) {
                    if (req.data.success) {
                        th.courses = req.data.result;
                        console.log(th.courses);
                    } else {
                        console.error(req.data.exception);
                        throw req.data.message;
                    }
                }).catch(err => console.error(err));
            }
        }
    });

});
