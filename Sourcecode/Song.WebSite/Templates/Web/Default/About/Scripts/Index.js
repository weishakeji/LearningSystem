$ready(function () {

    window.vapp = new Vue({
        el: '#vapp',
        data: {
            account: {},     //当前登录账号
            platinfo: {},
            organ: {},
            config: {},      //当前机构配置项        
            datas: {},
            loading_init: true,
            loading_map: true
        },
        mounted: function () {
            $api.bat(
                $api.get('Account/Current'),
                $api.cache('Platform/PlatInfo:60'),
                $api.get('Organization/Current')
            ).then(axios.spread(function (account, platinfo, organ) {
                vapp.loading_init = false;
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
                vapp.account = account.data.result;
                vapp.platinfo = platinfo.data.result;
                vapp.organ = organ.data.result;
                //机构配置信息
                vapp.config = $api.organ(vapp.organ).config;
            })).catch(function (err) {
                console.error(err);
            });
        },
        created: function () {
            var th = this;
            window.setTimeout(function () {
                th.loading_map = false;
                th.showMap(th.organ);
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
            showMap: function (organ) {
                var lng = organ.Org_Longitude;
                var lat = organ.Org_Latitude;
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
