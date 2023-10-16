$ready(function () {
    window.vapp = new Vue({
        el: '#vapp',
        data: {
            org: {},
            config: {},
            activeName: 'web',
            loading: false,
        },
        mounted: function () {
            var th = this;
            th.loading = true;
            $api.bat($api.get('Organization/Current'))
                .then(axios.spread(function (org) {
                    //获取结果             
                    th.org = org.data.result;
                    //机构配置信息
                    th.config = $api.organ(th.org).config;
                })).catch(err => console.error(err))
                .finally(() => th.loading = false);
        },
        created: function () {

        },
        computed: {

        },
        watch: {

        },
        methods: {

            btnEnter: function (formName) {
                var th = this;
                this.$refs[formName].validate((valid, fields) => {
                    if (valid) {
                        th.loading = true;
                        var para = { 'orgid': th.org.Org_ID, 'web': th.org.Org_ExtraWeb, 'mobi': th.org.Org_ExtraMobi };
                        $api.post('Organization/ModifyExtra', para)
                            .then(function (req) {
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
                        return false;
                    }
                });
            },
        }
    });
});
