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
                        //仅保留当前页要修改的字段
                        let fields = th.$refs[formName].fields;
                        let props = [];
                        for (let i = 0; i < fields.length; i++)
                            props.push(fields[i].prop);
                        let exclude = '';
                        for (var key in th.organ) {
                            let index = props.indexOf(key);
                            if (index < 0) exclude += key + ',';
                        }
                        var para = { 'entity': th.organ, 'exclude': exclude };
                        $api.post('Organization/Modify', para)
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
