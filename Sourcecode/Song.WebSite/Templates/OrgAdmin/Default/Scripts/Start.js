$ready(function () {

    window.vapp = new Vue({
        el: '#vapp',
        data: {
            platinfo: {},
            org: {},
            config: {},      //当前机构配置项        
            stat: {},       //机构的各种统计数据
            loading_init: true,
            loading_stat: true
        },
        mounted: function () {
            var th = this;
            $api.bat(
                $api.cache('Platform/PlatInfo:60'),
                $api.get('Organization/Current')
            ).then(([platinfo, org]) => {
                //获取结果           
                th.platinfo = platinfo.data.result;
                th.org = org.data.result;
                //机构配置信息
                th.config = $api.organ(th.org).config;
                th.getStatistics(th.org);
            }).catch(err => console.error(err))
                .finally(() => th.loading_init = false);

            window.setTimeout(function () {

            }, 1000);
            this.$nextTick(function () {

            });
        },
        created: function () {

        },
        computed: {
        },
        watch: {
            'loading_stat': function (nv, ov) {
                if (nv) return;
            }
        },
        methods: {
            //获取统计数据
            getStatistics: function (org) {
                var th = this;
                th.loading_stat = true;
                $api.cache('Organization/Statistics', { 'orgid': org.Org_ID }).then(function (req) {
                    if (req.data.success) {
                        th.stat = req.data.result;
                    } else {
                        console.error(req.data.exception);
                        throw req.config.way + ' ' + req.data.message;
                    }
                }).catch(err => console.error(err))
                    .finally(() => th.loading_stat = false);
            }
        },
        filters: {
            //数字转三位带逗号
            'commas': function (number) {
                return number.toString().replace(/\B(?=(\d{3})+(?!\d))/g, ",");
            }
        }
    });
}, ['Viewport/Components/piece.js',
    'Viewport/Components/dataitem.js',
    'Viewport/Components/studentage.js',
    'Viewport/Components/studentlogin.js',
    '/Utilities/echarts/echarts.min.js']);
