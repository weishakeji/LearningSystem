// 地理位置的显示，用的百度地图
Vue.component('map_show', {
    //lng:经度（Longitude）
    //lat:纬度（Latitude）
    //zoom:地图初始放大值，3-19
    //convert:经纬度坐标是否需要转换为百度专用值
    //boxid:地图区域的html标签id
    props: ['lng', 'lat', 'zoom', 'convert', 'boxid'],
    data: function () {
        return {
            appkey: '', //百度地图开放平台的ak
            error: ''
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
        },
        //地图区域的html标签id
        'mapid': function () {
            if (this.boxid == null || this.boxid == '') {
                let randomDecimal = Math.floor(Math.random() * 1000) + 1;
                return 'map_show_' + new Date().getTime() + '_' + randomDecimal;
            }
            return this.boxid;
        }
    },
    created: function () {
        var th = this;

        th.getappkey(); //加载百度地图浏览器ak
        //console.log(th.mapid);
        window['onBMapCallback_' + th.mapid] = function () {
            console.log('加载百度地图JS完成---');
            //如果坐标不需要转换，则直接显示
            if (th.convert == null || th.convert == false) return th.showmap(th.lng, th.lat);
            //如果坐标需要转换            
            BMap_Convertor.translate(th.lng, th.lat, 0, function (p) {
                th.showmap(p.lng, p.lat);
            });
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
            let url = window.location.protocol + '//api.map.baidu.com/api?v=3.0&ak=' + ak + '&callback=onBMapCallback_' + this.mapid;
            //$dom.load.js(url, null, 'map.baidu.com');
            //$dom('script[tag="map.baidu.com"]').remove();
            ///*
            let mapbaidu = $dom('script[tag="map.baidu.com"]');
            if (mapbaidu.length < 1) {
                $dom.load.js(url, null, 'map.baidu.com');
            } else {
                let f = window['onBMapCallback_' + this.mapid];
                if (!f) return;
                window.map_show_intervalId = setInterval(function () {
                    if (window.BMap && window.BMap.Map) {
                        f();
                        clearInterval(window.map_show_intervalId);
                    }
                }, 100);
            }
        },
        //显示地图
        showmap: function (lng, lat) {
            var el = $dom(this.$el);
            el.childs().remove();
            el.add("div").attr('id', this.mapid).css({
                width: '100%',
                height: '100%'
            });
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

//2011-7-25
(function () {        //闭包
    function load_script(xyUrl, callback) {
        var head = document.getElementsByTagName('head')[0];
        var script = document.createElement('script');
        script.type = 'text/javascript';
        script.src = xyUrl;
        //借鉴了jQuery的script跨域方法
        script.onload = script.onreadystatechange = function () {
            if ((!this.readyState || this.readyState === "loaded" || this.readyState === "complete")) {
                callback && callback();
                // Handle memory leak in IE
                script.onload = script.onreadystatechange = null;
                if (head && script.parentNode) {
                    head.removeChild(script);
                }
            }
        };
        // Use insertBefore instead of appendChild  to circumvent an IE6 bug.
        head.insertBefore(script, head.firstChild);
    }
    function translate(lng, lat, type, callback) {
        var callbackName = 'cbk_' + Math.round(Math.random() * 10000);    //随机函数名
        var xyUrl = window.location.protocol + '//api.map.baidu.com/ag/coord/convert?from=' + type + '&to=4&x=' + lng + '&y=' + lat + '&callback=BMap.Convertor.' + callbackName;
        //动态创建script标签
        load_script(xyUrl);
        BMap.Convertor[callbackName] = function (xyResult) {
            delete BMap.Convertor[callbackName];    //调用完需要删除改函数
            var point = new BMap.Point(xyResult.x, xyResult.y);
            callback && callback(point);
        }
    }

    window.BMap_Convertor = {};
    window.BMap_Convertor.translate = translate;
})();