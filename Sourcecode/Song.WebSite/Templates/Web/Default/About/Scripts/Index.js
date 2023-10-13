$ready(function () {

    window.vapp = new Vue({
        el: '#vapp',
        data: {
            account: {},     //当前登录账号
            platinfo: {},
            org: {},
            config: {},      //当前机构配置项        
            datas: {},
            loading_init: true,
            loading_map: true
        },
        mounted: function () {
        },
        created: function () {
            var th = this;
            th.$nextTick(function () {
                window.setTimeout(function () {
                    th.loading_map = false;
                    th.showMap(th.org);
                }, 300);
            });
        },
        computed: {},
        watch: {
        },
        methods: {
            //显示地图
            showMap: function (org) {
                var lng = org.Org_Longitude;
                var lat = org.Org_Latitude;
                lng = lng == 0 ? 116.404 : lng;
                lat = lat == 0 ? 39.915 : lat;
                //创建地图
                window.map = new BMap.Map("map");
                map.enableScrollWheelZoom();
                map.enableKeyboard();
                map.addControl(new BMap.NavigationControl());
                var point = new BMap.Point(lng, lat);
                map.centerAndZoom(point, 16);
                var marker = new BMap.Marker(point);  // 创建标注
                map.addOverlay(marker);
                marker.setAnimation("BMAP_ANIMATION_BOUNCE");
                map.centerAndZoom(point, 12);             
                //核心代码 （监听map加载完毕）
                var loadCount = 1;
                map.addEventListener("tilesloaded", function () {
                    if (loadCount == 1) {
                        map.setCenter(point);
                        map.centerAndZoom(point, 12);
                    }
                    loadCount = loadCount + 1;
                });
            },
        }
    });

}, ["../Components/courses.js",
    "../Components/course.js"]);
