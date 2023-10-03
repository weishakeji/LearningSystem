$ready(function () {
    window.vapp = new Vue({
        el: '#vapp',
        data: {
            organ: {},
            config: {},      //当前机构配置项        
            datas: {},

            activeName: 'general',      //选项卡
            rules: {
                Org_PlatformName: [
                    { required: true, message: '平台名称不得为空', trigger: 'blur' }
                ]
            },
            //域名
            domain: {
                two: '',        //二级域名
                root: '',       //根域
                port: '80'        //端口
            },
            //图片文件
            upfile: null, //本地上传文件的对象   
            mapshow: false,     //是否显示地图

            loading: false,
            loading_init: true
        },
        mounted: function () {
            var th = this;
            $api.bat(
                $api.get('Organization/Current'),
                $api.get('Platform/Domain'),
                $api.get('Platform/ServerPort')
            ).then(axios.spread(function (organ, domain, port) {
                //获取结果             
                th.organ = organ.data.result;
                //机构配置信息
                th.config = $api.organ(th.organ).config;
                //域名
                th.domain.two = th.organ.Org_TwoDomain;
                th.domain.root = domain.data.result;
                th.domain.port = port.data.result;
            })).catch(err => console.error(err))
                .finally(() => th.loading_init = false);
        },
        created: function () {

        },
        computed: {
            //当前机构的二级域名
            twomain: function () {
                var domain = this.domain;
                if (domain.port != "80") return domain.two + '.' + domain.root + ':' + domain.port;
                else return domain.two + '.' + domain.root;
            }
        },
        watch: {
            //打开百度地图
            'mapshow': function (nl, ol) {
                if (nl) {
                    var th = this;
                    window.setTimeout(function () {
                        th.showMap(th.organ);
                    }, 500);
                }
            },
            //监听机构对象
            organ: {
                deep: true,
                handler: function (n, o) {
                    //console.log('watch中：', n)
                }
            }
        },
        methods: {
            //清除图片
            fileremove: function () {
                this.upfile = null;
                this.organ.Org_Logo = '';
            },
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
                map.addEventListener("click", this.setMap);
            },
            //设置地图坐标
            setMap: function (e) {
                var lng = e.point.lng;
                var lat = e.point.lat;
                var map = e.target;
                map.clearOverlays();
                map.closeInfoWindow();
                var zoom = map.getZoom();
                map.centerAndZoom(new BMap.Point(lng, lat), zoom);
                var point = new BMap.Point(lng, lat);
                var marker = new BMap.Marker(new BMap.Point(lng, lat));  // 创建标注
                map.addOverlay(marker);
                this.organ.Org_Longitude = lng;
                this.organ.Org_Latitude = lat;
            },
            //地址变化时
            addressChange: function (val) {
                var th = this;
                $api.get('Platform/PositionGPS', { 'address': val }).then(function (req) {
                    if (req.data.success) {
                        var result = req.data.result;
                        var zoom = window.map.getZoom();
                        var lng = result.lng;
                        var lat = result.lat;
                        window.map.centerAndZoom(new BMap.Point(lng, lat), zoom);
                        window.map.clearOverlays();
                        var point = new BMap.Point(lng, lat);
                        var marker = new BMap.Marker(point);  // 创建标注
                        window.map.addOverlay(marker);
                        window.vapp.organ.Org_Longitude = lng;
                        window.vapp.organ.Org_Latitude = lat;
                    } else {
                        console.error(req.data.exception);
                        throw req.data.message;
                    }
                }).catch(function (err) {
                    alert(err);
                    console.error(err);
                });
            },
            btnEnter: function (formName) {
                var th = this;
                this.$refs[formName].validate((valid, fields) => {
                    if (valid) {
                        th.loading = true;
                        var apipath = 'Organization/Modify';
                        //接口参数，如果有上传文件，则增加file
                        var para = {};
                        if (th.upfile == null) para = { 'entity': th.organ };
                        else
                            para = { 'file': th.upfile, 'entity': th.organ };
                        $api.post(apipath, para).then(function (req) {
                            if (req.data.success) {
                                var result = req.data.result;
                                th.$message({
                                    type: 'success', center: true,
                                    message: '操作成功!'
                                });
                            } else {
                                throw req.data.message;
                            }
                        }).catch(err => alert(err, '错误'))
                            .finally(() => th.loading = false);
                    } else {
                        //未通过验证的字段
                        let field = Object.keys(fields)[0];
                        let label = $dom('label[for="' + field + '"]');
                        while (label.attr('tab') == null)
                            label = label.parent();
                        th.activeName = label.attr('tab');
                        return false;
                    }
                });
            },
        }
    });

}, ['/Utilities/baiduMap/convertor.js']);
