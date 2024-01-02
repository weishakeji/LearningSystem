$ready(function () {

    window.vapp = new Vue({
        el: '#vapp',
        data: {
            account: {},     //当前登录账号
            platinfo: {},
            organ: {},
            config: {},      //当前机构配置项        

            posi: {},       //位置信息
            brw_posi: {},
            loading_init: true,
            loading: true
        },
        mounted: function () {
            var th = this;
            $api.bat(
                $api.get('Account/Current'),
                $api.cache('Platform/PlatInfo:60'),
                $api.get('Organization/Current')
            ).then(axios.spread(function (account, platinfo, organ) {
                //获取结果
                th.account = account.data.result;
                th.platinfo = platinfo.data.result;
                th.organ = organ.data.result;
                //机构配置信息
                th.config = $api.organ(th.organ).config;
            })).catch(err => console.error(err))
                .finally(() => th.loading_init = false);

            //
            window.setTimeout(function () {
                th.brw_posi = window.$posi.coords;
                th.getposi();
            }, 500);

        },
        created: function () {

        },
        computed: {
            //是否登录
            islogin: t => !$api.isnull(t.account)
        },
        watch: {
        },
        methods: {
            getposi: function () {
                var th = this;
                th.loading = true;
                $api.get('Snowflake/GetLBS').then(function (req) {
                    if (req.data.success) {
                        let result = req.data.result;
                        delete result['ip']['Enable'];
                        delete result['geo']['Enable'];
                        th.posi = result;
                    } else {
                        console.error(req.data.exception);
                        throw req.config.way + ' ' + req.data.message;
                    }
                }).catch(err => console.error(err))
                    .finally(() => th.loading = false);
            },
            calc: function (lng, lat) {
                var gpsPoint = new BMap.Point(lng, lat);
                BMap.Convertor.translate(gpsPoint, 0, translateCallback);
            }
        }
    });

}, [
    '/Utilities/baiduMap/map_show.js']);
