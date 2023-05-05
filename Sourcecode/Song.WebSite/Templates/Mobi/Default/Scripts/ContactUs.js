$ready(function () {
    Vue.use(VueBaiduMap.default, {
        // ak 是在百度地图开发者平台申请的密钥 详见 http://lbsyun.baidu.com/apiconsole/key */
        ak: 'R6QvnyzsBlDiDpF8FNu5fYhS'
    });

    /*new Vue({
        el: '#map'
    });*/
    window.vapp = new Vue({
        el: '#vapp',
        data: {
            center: { lng: 0, lat: 0 },
            platinfo: {},
            organ: {},
            config: {},      //当前机构配置项      
            loading_init: true
        },
        mounted: function () {
            var th = this;
            $api.bat(
                $api.cache('Platform/PlatInfo:60'),
                $api.get('Organization/Current')
            ).then(axios.spread(function (platinfo, organ) {
                th.loading_init = false;
                //获取结果              
                th.platinfo = platinfo.data.result;
                th.organ = organ.data.result;

                //机构配置信息
                th.config = $api.organ(th.organ).config;
            })).catch(function (err) {
                console.error(err);
            });
        },
        created: function () {

        },
        computed: {
            //是否登录
            islogin: function () {
                return JSON.stringify(this.account) != '{}' && this.account != null;
            }
        },
        watch: {
        },
        methods: {
            handler({ BMap, map }) {
                console.log(BMap, map)
                //this.center.lng = 116.404
                //this.center.lat = 39.915
                this.center = {
                    lng: Number(this.organ.Org_Longitude),
                    lat: Number(this.organ.Org_Latitude)
                }
                this.zoom = 15
            }

        }
    });

}, ['Components/page_header.js',
    '/Utilities/baiduMap/vue-baidu-map.js'
]);
