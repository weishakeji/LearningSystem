// 地理位置的设置，用的百度地图
//事件：
//change:当经纬度变化时，返回参数:lng,lat
Vue.component('map_setup', {
    //lng:经度（Longitude）
    //lat:纬度（Latitude）
    //zoom:地图初始放大值，3-19
    //address:地址，当地址变化时，会自动计算经纬度
    props: ['lng', 'lat', 'zoom', 'address'],
    data: function () {
        return {
            appkey: '', //百度地图开放平台的ak
            mapobject: null,     //地图对象
            error: '',
            mapid: 'map_show_' + new Date().getTime()
        }
    },
    watch: {
        //当appkey变更时
        'appkey': {
            handler: function (val, old) {
                if (!$api.isnull(val) && val != '')
                    this.loadmapjs(val);
            }, immediate: true

        },
        //当地址变更时
        'address': function (val, old) {
            //console.error(val);
            this.getLocation();
        }
    },
    computed: {
        //地图放大值
        'mapzoom': function () {
            let defnumber = 12;
            if ($api.isnull(this.zoom)) return defnumber;
            if ($api.getType(this.zoom) == 'Number') return this.zoom < 3 || this.zoom > 19 ? defnumber : this.zoom;
            else if (isNaN(Number(this.zoom))) return defnumber;
            else return Number(this.zoom);
        }
    },
    created: function () {
        var th = this;
        th.getappkey(); //加载百度地图浏览器ak
        window.onBMapCallback = function () {
            console.log('加载百度地图JS完成---');
            //console.log(window.BMap);
            th.showmap();
        };
    },
    methods: {
        //获取百度地图的Appkey
        getappkey: function () {
            var th = this;
            $api.get('Platform/Parameter', { 'key': 'BaiduLBS_Brw' }).then(function (req) {
                if (req.data.success) {
                    th.appkey = req.data.result;
                } else {
                    console.error(req.data.exception);
                    throw req.config.way + ' ' + req.data.message;
                }
            }).catch(err => console.error(err))
                .finally(() => { });
        },
        //加载
        loadmapjs: function (ak) {
            //if (ak == null || ak == '') return;
            this.error = '';
            let url = 'http://api.map.baidu.com/api?v=3.0&ak=' + ak + '&callback=onBMapCallback';
            $dom('script[tag="map.baidu.com"]').remove();
            var th = this;
            $dom.load.js(url, null, 'map.baidu.com');
        },
        //显示地图
        showmap: function () {
            var el = $dom(this.$el);
            el.childs().remove();
            el.add("div").attr('id', this.mapid).css({
                width: '100%',
                height: '100%'
            });
            var lng = this.lng;
            var lat = this.lat;
            lng = lng == 0 ? 116.404 : lng;
            lat = lat == 0 ? 39.915 : lat;
            //创建地图
            this.mapobject = new BMap.Map(this.mapid);
            var map = this.mapobject;
            map.enableScrollWheelZoom();
            map.enableKeyboard();
            map.addControl(new BMap.NavigationControl());
            var point = new BMap.Point(lng, lat);
            map.centerAndZoom(point, this.mapzoom);
            var marker = new BMap.Marker(point);  // 创建标注
            map.addOverlay(marker);
            marker.setAnimation("BMAP_ANIMATION_BOUNCE");
            //map.centerAndZoom(point, 18);
            //核心代码 （监听map加载完毕）
            var loadCount = 1;
            map.addEventListener("tilesloaded", function () {
                if (loadCount == 1) {
                    map.setCenter(point);
                    map.centerAndZoom(point, this.mapzoom);
                }
                loadCount = loadCount + 1;
            });
            map.addEventListener("click", this.setMap);
        },
        //设置地图坐标
        setMap: function (e) {
            let lng = e.point.lng;
            let lat = e.point.lat;
            let map = e.target;     //地图对象
            map.clearOverlays();
            map.closeInfoWindow();
            let zoom = map.getZoom();
            map.centerAndZoom(new BMap.Point(lng, lat), zoom);
            let point = new BMap.Point(lng, lat);
            let marker = new BMap.Marker(new BMap.Point(lng, lat));  // 创建标注
            map.addOverlay(marker);
            //触发事件,参数：经度、纬度
            this.$emit('change', lng, lat);
        },

        // 获取当前地址的经纬度坐标
        getLocation: async function () {
            try {
                let map = this.mapobject;
                const point = await this.getPointByAddress(this.address);
                this.point = point;
                //console.log('Js获取经度：', point.lng);
                //console.log('Js获取纬度：', point.lat);
                this.$emit('change', point.lng, point.lat);
                //
                var zoom = map.getZoom();
                var p = new BMap.Point(point.lng, point.lat);
                map.centerAndZoom(p, zoom);
                map.clearOverlays();
                //var point = new BMap.Point(lng, lat);
                var marker = new BMap.Marker(p);  // 创建标注
                map.addOverlay(marker);

            } catch (error) {
                console.error(error);
            }
        },
        // 根据地址名称获取经纬度坐标
        getPointByAddress: function (address) {
            // 创建地理编码实例
            const myGeo = new BMap.Geocoder();
            return new Promise((resolve, reject) => {
                // 对地址进行地理编码
                myGeo.getPoint(address, (point) => {
                    if (point) {
                        // 地理编码成功，返回经纬度坐标对象
                        resolve(point);
                    } else {
                        // 地理编码失败
                        reject('地理编码失败');
                    }
                }, '全国');
            });
        },
    },
    template: ` <div class="map_setup" v-show="error==''">
    </div>`
});
