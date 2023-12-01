$ready(function () {
    window.vapp = new Vue({
        el: '#vapp',
        data: {
            //要操作的对象
            entity: { 'Sys_Key': 'BaiduLBS', 'Sys_Value': '', 'Sys_Id': 0 },
            loading: false,
            rules: {
                'Sys_Value': [
                    { required: true, message: '请输入百度地图的AppKey', trigger: 'blur' }
                ]
            }
        },
        watch: {
        },
        created: function () {
            this.getData();
        },
        methods: {
            //获取数据
            getData: function () {
                var th = this;
                th.loading = true;
                $api.get('Platform/ParamForKey', { 'key': th.entity.Sys_Key }).then(function (req) {
                    if (req.data.success) {
                        th.entity = req.data.result;
                    } else {
                        console.error(req.data.exception);
                        throw req.data.message;
                    }
                }).catch(err => console.error(err))
                    .finally(() => th.loading = false);
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
                            alert(err);
                        }).finally(() => th.loading = false);
                    }
                });
            },
        }
    });

}, ['/Utilities/baiduMap/map_show.js']);