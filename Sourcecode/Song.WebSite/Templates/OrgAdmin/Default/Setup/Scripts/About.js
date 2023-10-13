$ready(function () {
    window.vapp = new Vue({
        el: '#vapp',
        data: {
            organ: {},
            config: {},
            loading: false,
        },
        mounted: function () {
            var th = this;
            th.loading = true;
            $api.bat($api.get('Organization/Current'))
                .then(axios.spread(function (organ) {
                    //获取结果             
                    th.organ = organ.data.result;
                    //机构配置信息
                    th.config = $api.organ(th.organ).config;
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
                        var apipath = 'Organization/ModifyIntro';
                        $api.post(apipath, { 'orgid': th.organ.Org_ID, 'text': th.organ.Org_Intro })
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

}, ['/Utilities/baiduMap/convertor.js']);
