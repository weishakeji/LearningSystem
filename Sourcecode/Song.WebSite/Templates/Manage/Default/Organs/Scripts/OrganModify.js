
$ready(function () {

    window.vapp = new Vue({
        el: '#app',
        data: {
            id: $api.querystring('id'),
            entity: {}, //当前对象    
            levels: [], //机构等列表
            domain: '',  //主域
            lv_id: '',   //当前机构等级
            rules: {
                Org_PlatformName: [
                    { required: true, message: '平台名称不得为空', trigger: 'blur' }
                ],
                Org_Name: [
                    { required: true, message: '机构名称不得为空', trigger: 'blur' }
                ],
                Org_AbbrName: [
                    { required: true, message: '机构简称不得为空', trigger: 'blur' }
                ],
                Org_TwoDomain: [
                    { required: true, message: '平台域名不得为空', trigger: 'blur' }
                ]
            },
            mapshow: false,      //是否显示地图信息
            loading: false
        },
        watch: {
            'lv_id': function (nl, ol) {
                this.entity.Olv_ID = nl;
            },
            'mapshow': function (nl, ol) {
                if (nl) {
                    var th = this;
                    window.setTimeout(function () {
                        th.showMap(th.entity);
                    }, 500)

                }
            }
        },
        created: function () {
            var th = this;
            $api.bat(
                $api.get('Organization/LevelAll'),
                $api.get('Platform/Domain')
            ).then(axios.spread(function (level, domain) {
                //判断结果是否正常
                for (var i = 0; i < arguments.length; i++) {
                    if (arguments[i].status != 200)
                        console.error(arguments[i]);
                    var data = arguments[i].data;
                    if (!data.success && data.exception != null) {
                        console.error(data.exception);
                        throw data.message;
                    }
                }
                //获取结果
                vapp.levels = level.data.result;
                vapp.domain = domain.data.result;
                //默认机构
                var deflevel = vapp.levels.filter(item => item.Olv_IsDefault);
                if (deflevel.length > 0) vapp.lv_id = deflevel[0].Olv_ID;
                if (th.id != '') {
                    $api.get('Organization/ForID', { 'id': th.id }).then(function (req) {
                        if (req.data.success) {
                            th.entity = req.data.result;
                            vapp.lv_id = th.entity.Olv_ID;
                        } else {
                            console.error(req.data.exception);
                            throw req.data.message;
                        }
                    }).catch(function (err) {
                        alert(err);
                        console.error(err);
                    });
                }
            })).catch(function (err) {
                console.error(err);
            });
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
                this.entity.Org_Longitude = lng;
                this.entity.Org_Latitude = lat;
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
                        window.vapp.entity.Org_Longitude = lng;
                        window.vapp.entity.Org_Latitude = lat;
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
            //保存信息
            btnEnter: function (formName) {
                this.$refs[formName].validate((valid) => {
                    if (valid) {
                        var apipath = 'Organization/' + (this.id == '' ? api = 'add' : 'Modify');
                        $api.post(apipath, { 'entity': vapp.entity }).then(function (req) {
                            if (req.data.success) {
                                var result = req.data.result;
                                vapp.$message({
                                    type: 'success',
                                    message: '操作成功!',
                                    center: true
                                });
                                vapp.operateSuccess();
                            } else {
                                throw req.data.message;
                            }
                        }).catch(function (err) {
                            //window.top.ELEMENT.MessageBox(err, '错误');
                            vapp.$alert(err, '错误');
                        });
                    } else {
                        console.log('error submit!!');
                        return false;
                    }
                });
            },
            //操作成功
            operateSuccess: function () {
                window.top.$pagebox.source.tab(window.name, 'vue.handleCurrentChange', true);
            }
        },
    });

}, ['/Utilities/baiduMap/convertor.js']);
