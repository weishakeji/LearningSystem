// 地理位置的显示，百度用地图
Vue.component('map_show', {
    //lng:经度（Longitude）
    //lat:纬度（Latitude）
    props: ['lng', 'lat', 'ak'],
    data: function () {
        return {
            mapid: 'map_show_' + new Date().getTime()
        }
    },
    watch: {
        //当appkey变更时
        'ak': {
            handler: function (val, old) {
                this.loadmapjs(val);
            }, immediate: true

        }
    },
    computed: {

    },
    created: function () {
        var th = this;
        window.onBMapCallback = function () {
            console.log(window.BMap);
            console.error('加载成功');
            //resolve(window.BMap);
            th.showmap();
        };
        window.alert = function (txt) {
            console.error(txt);
        }
    },
    methods: {
        //加载
        loadmapjs: function (ak) {
            if (ak == null || ak == '') return;
            let url = 'http://api.map.baidu.com/api?v=3.0&ak=' + ak + '&callback=onBMapCallback';
            $dom('script[tag="map.baidu.com"]').remove();
            var th = this;
            $dom.load.js(url, function () {
                window.setTimeout(function () {
                    //th.showmap();
                }, 2000);

            }, 'map.baidu.com');
            return;
            //
            return new Promise(function (resolve, reject) {
                if (typeof window.BMap !== 'undefined') {
                    resolve(window.BMap);
                    return true
                }
                window.onBMapCallback = function () {
                    console.log(window.BMap);
                    resolve(window.BMap);
                }
                let script = document.createElement('script');
                script.type = 'text/javascript'
                script.src = 'http://api.map.baidu.com/api?v=3.0&ak=' + ak + '&callback=onBMapCallback'
                script.onerror = reject;
                document.head.appendChild(script);
            })
        },
        //显示地图
        showmap: function () {
            console.error(this);
            var el = $dom(this.$el);
            el.childs().remove();
            el.add("div").attr('id', this.mapid).css({
                width: '500px',
                height: '200px'
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
        }
    },
    template: ` <div class="map_show">
dd
<d>333</d>
    </div>`
});
