// 地理位置的显示，百度用地图
Vue.component('map_show', {
    //lng:经度（Longitude）
    //lat:纬度（Latitude）
    //zoom:地图初始放大值，3-19
    props: ['lng', 'lat', 'zoom'],
    data: function () {
        return {
            appkey: '', //百度地图开放平台的ak
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
        window.alert = function (txt) {
            let keywords = ['LBS', 'lbsyun.baidu.com', 'AK'];
            let exist = false;
            for (let i = 0; i < keywords.length; i++) {
                if (txt.indexOf(keywords[i] > -1)) {
                    exist = true;
                    break;
                }
            }
            if (exist) th.error = txt;
            else th.$alert(txt);
        }
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
            if (ak == null || ak == '') return;
            this.error = '';
            let url = 'http://api.map.baidu.com/api?v=3.0&ak=' + ak + '&callback=onBMapCallback';
            $dom('script[tag="map.baidu.com"]').remove();
            var th = this;
            $dom.load.js(url, null, 'map.baidu.com');
        },
        //显示地图
        showmap: function () {
            console.error(this);
            var el = $dom(this.$el);
            el.childs().remove();
            el.add("div").attr('id', this.mapid).css({
                width: '100%',
                height: '100%'
            });

            //return;
            var lng = this.lng;
            var lat = this.lat;
            lng = lng == 0 ? 116.404 : lng;
            lat = lat == 0 ? 39.915 : lat;
            //创建地图
            window.map = new BMap.Map(this.mapid);
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
        }
    },
    template: ` <div class="map_show" v-show="error==''">
    </div>`
});
