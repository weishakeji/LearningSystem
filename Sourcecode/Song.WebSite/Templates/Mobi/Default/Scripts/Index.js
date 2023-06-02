
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
            menus: [],        //主导航菜单
            subject: [],         //专业
            search: '',      //搜索框

            loading: true,
            loading_init: false
        },
        mounted: function () {

        },
        created: function () {
        },
        computed: {
        },
        watch: {
            'platinfo': function (nv, ov) {
                //document.title = nv.title;
            },
            'org': {
                handler: function (nv, ov) {
                    if ($api.isnull(nv)) return;
                    document.title = nv.Org_PlatformName;
                    var th = this;
                    th.loading_init = false;
                    //轮换图片，通知公告,自定义菜单项，专业
                    var orgid = nv.Org_ID;
                    th.loading = true;
                    $api.bat(
                        $api.cache('Showpic/Mobi:60', { 'orgid': orgid }),
                        $api.get('Notice/ShowItems', { 'orgid': orgid, 'type': 1, 'count': 10 }),
                        $api.cache('Navig/Mobi', { 'orgid': orgid, 'type': 'main' }),
                        $api.cache('Subject/ShowRoot:60', { 'orgid': orgid, 'count': 10 })
                    ).then(axios.spread(function (showpic, notice, menus, subject) {
                        //获取结果
                        th.showpic = showpic.data.result;
                        th.notice = notice.data.result;
                        th.menus = menus.data.result;
                        th.subject = subject.data.result;
                    })).catch(err => console.error(err))
                        .finally(() => th.loading = false);;
                }, immediate: true
            },
            'menus': function (nv, ov) {
                if (nv == null) return;
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



