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
            window.setTimeout(function () {
                th.loading_map = false;
                th.showMap(th.org);
            }, 500);
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
                map.centerAndZoom(new BMap.Point(lng, lat), 16);
                var marker = new BMap.Marker(new BMap.Point(lng, lat));  // 创建标注
                map.addOverlay(marker);
                //map.addEventListener("click", this.setMap);                
            },
        }
    });

}, ["../Components/courses.js",
"../Components/course.js"]);
