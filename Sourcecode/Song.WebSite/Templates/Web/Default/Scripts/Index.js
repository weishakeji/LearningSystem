
$ready(function () {
    window.vapp = new Vue({
        el: '#vapp',
        data: {
            platinfo: {},
            account: {},     //当前登录账号
            organ: {},
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
        mounted: function () {
            $api.bat(
                $api.cache('Platform/PlatInfo:30'),
                $api.get('Organization/Current')
            ).then(axios.spread(function (platinfo, organ) {
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
                vapp.organ = organ.data.result;
                if (!vapp.organ) return;
                //vapp.organ.Org_Logo = '';
                vapp.load();
                //机构配置信息
                vapp.config = $api.organ(vapp.organ).config;
                //轮换图片，通知公告,自定义菜单项，专业
                var orgid = vapp.organ.Org_ID;
                $api.bat(
                    $api.cache('Showpic/web:60', { 'orgid': orgid }),
                    $api.get('Notice/ShowItems', { 'orgid': orgid, 'count': 4 }),
                    $api.get('News/ArticlesShow', { 'orgid': orgid, 'uid': '', 'count': 12, 'order': 'img' }),
                    $api.cache('Subject/ShowRoot:60', { 'orgid': orgid, 'count': 10 })
                ).then(axios.spread(function (showpic, notice, articles, subject) {
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
                    vapp.showpic = showpic.data.result;
                    vapp.notice = notice.data.result;
                    if (vapp.notice) {
                        var regex = /(<([^>]+)>)/ig;
                        for (let i = 0; i < vapp.notice.length; i++) {
                            vapp.notice[i].No_Context = vapp.notice[i].No_Context.replace(regex, "");
                        }
                    }
                    vapp.articles = articles.data.result;

                    //vapp.subject = subject.data.result;
                })).catch(function (err) {
                    vapp.loading = false;
                    console.error(err);
                });
            })).catch(function (err) {
                console.error(err);
            });
        },
        created: function () {
        },
        computed: {},
        watch: {
            'platinfo': function (nv, ov) {
                if (nv) document.title = nv.title;
            },
            'showpic': {
                handler: function (nv, ov) {

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
            load: function () {
                var th = this;
                if (!th.organ.Org_ID) return;
                th.query.index++;
                th.query.orgid = th.organ.Org_ID;
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



