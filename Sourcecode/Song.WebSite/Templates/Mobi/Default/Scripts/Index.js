
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
            menus: [],        //主导航菜单
            subject: [],         //专业
            search: '',      //搜索框

            loading: true,
            loading_init: false
        },
        mounted: function () {
            alert('测试alert');
            var th = this;
            th.loading_init = true;
            $api.bat(
                $api.get('Account/Current'),
                $api.cache('Platform/PlatInfo:300'),
                $api.get('Organization/Current')
            ).then(axios.spread(function (account, platinfo, organ) {
                th.loading_init = false;
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
                th.organ = organ.data.result;
                //vapp.organ.Org_Logo = '';
                //机构配置信息
                th.config = $api.organ(th.organ).config;
                //轮换图片，通知公告,自定义菜单项，专业
                var orgid = th.organ.Org_ID;
                th.loading=true;
                $api.bat(
                    $api.cache('Showpic/Mobi:60', { 'orgid': orgid }),
                    $api.get('Notice/ShowItems', { 'orgid': orgid, 'type': 1, 'count': 10 }),
                    $api.cache('Navig/Mobi', { 'orgid': orgid, 'type': 'main' }),
                    $api.cache('Subject/ShowRoot:60', { 'orgid': orgid, 'count': 10 })
                ).then(axios.spread(function (showpic, notice, menus, subject) {
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
                    th.showpic = showpic.data.success ? showpic.data.result : [];
                    th.notice = notice.data.success ? notice.data.result : [];
                    th.menus = menus.data.success ? menus.data.result : [];
                    th.subject = subject.data.success ? subject.data.result : [];
                })).catch(function (err) {
                    th.loading = false;
                    console.error(err);
                });
            })).catch(function (err) {
                console.error(err);
            });
        },
        created: function () {
        },
        computed: {
        },
        watch: {
            'platinfo': function (nv, ov) {
                //document.title = nv.title;
            },
            'organ': function (nv, ov) {
                this.$nextTick(function () {
                    $dom("header img.logo").bind('load', function (event) {
                        var node = event.target ? event.target : event.srcElement;
                        var img = $dom(node);
                        var searWidth = $dom("header").width() - img.width();
                        img.next().width(searWidth);
                    });
                });
                document.title = nv.Org_PlatformName;
            },
            'menus': function (nv, ov) {
                var len = nv.length;
                this.$nextTick(function () {
                    if (len >= 12) return;
                    if (len % 5 == 0) return $dom(".custom-menu a").css('width', '20%');
                    if (len % 4 == 0) return $dom(".custom-menu a").css('width', '25%');
                    if (len % 3 == 0) return $dom(".custom-menu a").css('width', '33%');
                });
            }
        },
        methods: {
            onSearch: function () {
                if ($api.trim(this.search) == '') return;
                var search = encodeURIComponent(this.search);
                var url = $api.url.set('/mobi/course/index', {
                    'search': search
                });
                window.location.href = url;
            }
        }
    });

}, ['Components/courses.js',
    "/Utilities/Components/popup-notice.js"]);



