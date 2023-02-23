$ready(function () {

    window.vapp = new Vue({
        el: '#vapp',
        data: {
            platinfo: {},
            org: {},
            arid: $api.dot(),        //新闻id
            article: {},        //新闻对象
            accessory: [],       //新闻附件
            column: {},          //栏目信息

            courses: [],     //课程列表
            loading: false
        },
        mounted: function () {
            var th = this;
            this.loading = true;
            $api.bat(
                $api.get('Account/Current'),
                $api.cache('Platform/PlatInfo'),
                $api.get('Organization/Current')
            ).then(axios.spread(function (account, platinfo, org) {
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
                th.org = org.data.result;
                //机构配置信息
                th.config = $api.organ(th.org).config;

            })).catch(function (err) {
                console.error(err);
            });
            var th = this;
            th.loading = true;
            $api.cache('News/Article', { 'id': this.arid }).then(function (req) {
                th.loading = false;
                if (req.data.success) {
                    th.article = req.data.result;
                    document.title = th.article.Art_Title;
                    $api.bat(
                        $api.cache('News/ColumnsForUID', { 'uid': th.article.Col_UID }),
                        $api.cache('News/VisitPlusOne:60', { 'id': th.article.Art_ID }),
                        $api.cache("News/Accessory", { 'uid': th.article.Art_Uid })
                    ).then(axios.spread(function (column, visit, accessory) {
                        //判断结果是否正常
                        for (var i = 0; i < arguments.length; i++) {
                            if (arguments[i].status != 200)
                                console.error(arguments[i]);
                            var data = arguments[i].data;
                            if (!data.success && data.exception != null) {
                                console.error(data.exception);
                                throw data.message;
                            }
                        }
                        //栏目信息
                        th.column = column.data.result;
                        //访问量加一，并给当前新闻加上这个数
                        th.article.Art_Number = visit.data.result;
                        //新闻附件
                        th.accessory = accessory.data.result;
                    })).catch(function (err) {
                        console.error(err);
                    });
                } else {
                    console.error(req.data.exception);
                    throw req.data.message;
                }
            }).catch(function (err) {
                th.loading = false;
                console.error(err);
            });
            this.getcourses();
        },
        created: function () { },
        computed: {
            //是否为空，即不存在
            isempty: function () {
                return !(JSON.stringify(this.article) != '{}' && this.article != null);
            }
        },
        watch: {},
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
                }).catch(function (err) {

                    console.error(err);
                });
            }
        }
    });

});
