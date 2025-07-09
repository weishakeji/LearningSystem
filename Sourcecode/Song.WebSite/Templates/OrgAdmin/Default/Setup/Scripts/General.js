$ready(function () {
    window.vapp = new Vue({
        el: '#vapp',
        data: {
            org: {},
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
            ).then(([org, domain, port]) => {
                //获取结果             
                th.org = org.data.result;
                //机构配置信息
                th.config = $api.organ(th.org).config;
                //域名
                th.domain.two = th.org.Org_TwoDomain;
                th.domain.root = domain.data.result;
                th.domain.port = port.data.result;
            }).catch(err => console.error(err))
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
            //监听机构对象
            org: {
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
                this.org.Org_Logo = '';
            },
            //lng:经度（Longitude）
            //lat:纬度（Latitude）
            map_selected: function (lng, lat) {
                this.org.Org_Longitude = lng;
                this.org.Org_Latitude = lat;
            },
            btnEnter: function (formName) {
                var th = this;
                this.$refs[formName].validate((valid, fields) => {
                    if (valid) {
                        if (th.loading) return;
                        th.loading = true;
                        //仅保留当前页要修改的字段
                        let fields = th.$refs[formName].fields;
                        let props = [];
                        for (let i = 0; i < fields.length; i++)
                            props.push(fields[i].prop);
                        let exclude = '';
                        for (var key in th.org) {
                            let index = props.indexOf(key);
                            if (index < 0) exclude += key + ',';
                        }
                        //console.error(exclude);
                        //接口参数，如果有上传文件，则增加file
                        var para = { 'entity': th.org, 'exclude': exclude };
                        if (th.upfile != null) para['file'] = th.upfile;
                        $api.post('Organization/Modify', para).then(function (req) {
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

}, ['/Utilities/baiduMap/map_setup.js']);
