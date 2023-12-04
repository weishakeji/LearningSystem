$ready(function () {
    window.vapp = new Vue({
        el: '#vapp',
        data: {
            //要操作的对象
            entity: { 'Sys_Key': 'BaiduLBS_Brw', 'Sys_Value': '', 'Sys_Id': 0 },
            rules: {
                'Sys_Value': [
                    { required: true, message: '请输入百度地图的AppKey', trigger: 'blur' }
                ]
            },
            error: '',       //错误提示
            //当前登录的管理员
            admin: null,
            //当前机构
            org: {},
            loading: false,
            loading_init: false
        },
        watch: {
            //当appkey变更时
            'entity.Sys_Value': {
                handler: function (val, old) {
                    if ($api.isnull(val)) return;
                    this.loadmapjs(val);
                }, immediate: true

            }
        },
        computed: {
            //是否登录
            islogin: (t) => { return !$api.isnull(t.admin); },
        },
        mounted: function () {
            var th = this;
            th.loading_init = true;
            $api.get('Admin/Super').then(function (req) {
                if (req.data.success) {
                    th.admin = req.data.result;
                    if (!th.islogin) return;
                    th.loading = true;
                    $api.bat(
                        $api.get('Platform/ParamForKey', { 'key': th.entity.Sys_Key }),
                        $api.get('Organization/ForID', { 'id': th.admin.Org_ID })
                    ).then(axios.spread(function (param, org) {
                        if (param.data.result)
                            th.entity = param.data.result;
                        th.org = org.data.result;
                    })).catch(err => alert(err))
                        .finally(() => th.loading = false);
                } else {
                    throw req.config.way + ' ' + req.data.message;
                }
            }).catch(err => console.error(err))
                .finally(() => th.loading_init = false);
        },
        created: function () {
            var th = this;
            window.onBMapCallback = function () {
                console.log('加载百度地图JS完成---');
                console.log(window.BMap);
                th.showmap();
            };
            window.alert = function (txt) {
                let keywords = ['LBS', 'APP', 'AK'];
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
            //加载
            loadmapjs: function (ak) {
                if (ak == null || ak == '') return;
                this.error = '';
                let url = 'http://api.map.baidu.com/api?v=3.0&ak=' + ak + '&callback=onBMapCallback';
                $dom('script[tag="map.baidu.com"]').remove();
                $dom.load.js(url, null, 'map.baidu.com');

            },
            //显示地图
            showmap: function () {
                //清除之前的地图区域
                var el = $dom('#mapbox');
                el.childs().remove();
                let mapid = 'map_show_' + new Date().getTime();  //地图区域的id
                el.add("div").attr('id', mapid);
                //
                var lng = this.org.Org_Longitude;
                var lat = this.org.Org_Latitude;
                lng = lng == 0 ? 116.404 : lng;
                lat = lat == 0 ? 39.915 : lat;
                //创建地图
                window.map = new BMap.Map(mapid);
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
            },
            //保存信息
            btnEnter: function () {
                var th = this;
                if (th.loading) return;
                this.$refs['entity'].validate(function (valid) {
                    if (valid) {
                        th.loading = true;
                        $api.post("Platform/ParamModify", { 'entity': th.entity }).then(function (req) {
                            if (req.data.success) {
                                th.entity = req.data.result;
                                th.$message({ message: '操作成功', type: 'success' });
                            } else {
                                console.error(req.data.exception);
                                throw req.config.way + ' ' + req.data.message;
                            }
                        }).catch(function (err) {
                            th.$alert(err);
                        }).finally(() => th.loading = false);
                    }
                });
            },
        }
    });

});