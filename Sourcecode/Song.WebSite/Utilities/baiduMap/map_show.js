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

    },
    methods: {
        //加载
        loadmapjs: function (ak) {
            if (ak == null || ak == '') return;
            let url = 'https://api.map.baidu.com/api?v=2.0&ak=' + ak;
            $dom('script[tag="map.baidu.com"]').remove();
            var th=this;
            $dom.load.js(url, function(){
                this.ak
                console.error(th.ak);
            }, 'map.baidu.com');
        }
    },
    template: ` <div :id="mapid">
dd
    </div>`
});
