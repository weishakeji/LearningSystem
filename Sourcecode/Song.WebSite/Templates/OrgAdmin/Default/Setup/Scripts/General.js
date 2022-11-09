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
            error: '',       //错误信息
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
                th.loading_init = false;
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
                th.organ = organ.data.result;               
                //机构配置信息
                th.config = $api.organ(th.organ).config;
                //域名
                th.domain.two = th.organ.Org_TwoDomain;
                th.domain.root = domain.data.result;
                th.domain.port = port.data.result;
            })).catch(function (err) {
                console.error(err);
            });
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
            //图片文件上传
            filechange: function (file) {
                var th = this;
                th.upfile = file;
            },
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
                console.log(val);
            },
            btnEnter: function (formName) {
                var th = this;
                th.organ.Org_Intro = th.$refs.editor.getContent();
                this.$refs[formName].validate((valid, obj) => {
                    if (valid) {
                        th.error = '';
                        th.loading = true;
                        var apipath = 'Organization/Modify';
                        //接口参数，如果有上传文件，则增加file
                        var para = {};
                        if (th.upfile == null) para = { 'entity': th.organ };
                        else
                            para = { 'file': th.upfile, 'entity': th.organ };
                        $api.post(apipath, para).then(function (req) {
                            th.loading = false;
                            if (req.data.success) {
                                var result = req.data.result;
                                th.$message({
                                    type: 'success',
                                    message: '操作成功!',
                                    center: true
                                });
                            } else {
                                throw req.data.message;
                            }
                        }).catch(function (err) {
                            //window.top.ELEMENT.MessageBox(err, '错误');
                            th.$alert(err, '错误');
                        });
                    } else {
                        console.log('error submit!!');
                        for (var t in obj) {
                            th.error = obj[t][0].message;
                            break;
                        }
                        window.setTimeout(function () {
                            th.error = '';
                        }, 5000);
                        return false;
                    }
                });
            },
        }
    });

}, ['/Utilities/baiduMap/convertor.js']);
