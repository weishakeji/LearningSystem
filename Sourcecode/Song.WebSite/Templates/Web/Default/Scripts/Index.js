
$ready(function () {
    window.vapp = new Vue({
        el: '#vapp',
        data: {
            platinfo: {},
            account: {},     //当前登录账号
            org: {},
            config: {},      //当前机构配置项
            showpic: [],        //轮换图片
            notice: [],          //通知公告  
            articles: [],         //新闻文章           

            loading: true,

            subject: [],         //专业
            finished: false,
            query: {
                'orgid': '', 'pid': 0, 'index': 0, 'size': 1
            },
            total: 0,
            sbj_loading: false
        },
        mounted: function () { },
        created: function () { },
        computed: {},
        watch: {
            'org': {
                handler: function (nv, ov) {
                    this.loadinit(nv.Org_ID);
                    this.loadsbj();
                    document.title = nv.Org_PlatformName;
                }, immediate: true
            }
        },
        methods: {
            onSearch: function () {
                if ($api.trim(this.search) == '') return;
                var search = encodeURIComponent(this.search);
                var url = "/mobi/course/index?search=" + search;
                window.location.href = url;
            },
            //加载一些初始数据
            loadinit: function (orgid) {
                if (orgid == undefined) return;
                var th = this;
                th.loading = true;
                $api.bat(
                    $api.cache('Showpic/web:60', { 'orgid': orgid }),
                    $api.cache('Notice/ShowItems', { 'orgid': orgid, 'type': 1, 'count': 4 }),
                    $api.cache('News/ArticlesShow', { 'orgid': orgid, 'uid': '', 'count': 12, 'order': 'img' })
                ).then(axios.spread(function (showpic, notice, articles) {
                    th.loading = false;
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
                    th.showpic = showpic.data.result;
                    th.notice = notice.data.result;
                    if (th.notice) {
                        var regex = /(<([^>]+)>)/ig;
                        for (let i = 0; i < th.notice.length; i++) {
                            th.notice[i].No_Context = th.notice[i].No_Context.replace(regex, "");
                        }
                    }
                    th.articles = articles.data.result;
                })).catch(function (err) {
                    th.loading = false;
                    console.error(err);
                });
            },
            //加载专业
            loadsbj: function () {
                var th = this;
                if (!th.org.Org_ID) return;
                th.query.index++;
                th.query.orgid = th.org.Org_ID;
                var query = $api.clone(th.query);
                $api.cache('Subject/PagerFront', query).then(function (req) {
                    if (req.data.success) {
                        th.total = req.data.total;
                        var result = req.data.result;
                        for (var i = 0; i < result.length; i++) {
                            th.subject.push(result[i]);
                        }
                        var totalpages = req.data.totalpages;
                        // 数据全部加载完成
                        if (th.subject.length >= th.total || result.length == 0) {
                            th.finished = true;
                        }
                    } else {
                        console.error(req.data.exception);
                        throw req.data.message;
                    }
                }).catch(function (err) {
                    //alert(err);
                    console.error(err);
                });
            }
        }
    });

}, ["Components/subject_rec.js",
    "Components/subject_show.js",
    "Components/links.js",
    "/Utilities/Components/popup-notice.js"]);



