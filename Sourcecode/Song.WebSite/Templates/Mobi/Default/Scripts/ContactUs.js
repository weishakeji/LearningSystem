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
            account: {},
            platinfo: {},
            org: {},
            config: {},      //当前机构配置项      

        },
        mounted: function () {

        },
        created: function () {

        },
        computed: {
          
        },
        watch: {
        },
        methods: {
            handler({ BMap, map }) {
                console.log(BMap, map)
                //this.center.lng = 116.404
                //this.center.lat = 39.915
                this.center = {
                    lng: Number(this.org.Org_Longitude),
                    lat: Number(this.org.Org_Latitude)
                }
                this.zoom = 15
            }

        }
    });

}, ['Components/page_header.js',
    '/Utilities/baiduMap/vue-baidu-map.js'
]);
