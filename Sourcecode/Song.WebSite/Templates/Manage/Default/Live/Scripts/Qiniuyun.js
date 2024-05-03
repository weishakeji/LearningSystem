$ready(function () {
    window.vapp = new Vue({
        el: '#vapp',
        data: {
            form: {},
            loading: false,
            rules: {
                'AccessKey': [
                    { required: true, message: '请输入密钥', trigger: 'blur' }
                ],
                'SecretKey': [
                    { required: true, message: '请输入密钥', trigger: 'blur' }
                ]
            }
        },
        watch: {
        },
        created: function () {
            var th = this;
            th.loading = true;
            $api.get('Live/GetSetup').then(function (req) {
                if (req.data.success) {
                    th.form = req.data.result;
                } else {
                    console.error(req.data.exception);
                    throw req.config.way + ' ' + req.data.message;
                }
            }).catch(err => { alert(err); console.error(err); })
                .finally(() => th.loading = false);
        },
        methods: {
            //保存信息
            btnEnter: function () {
                var th = this;
                if (th.loading) return;
                th.loading = true;
                this.$refs['form'].validate(function (valid) {
                    if (valid) {
                        $api.post("live/Setup", th.form).then(function (req) {
                            if (req.data.success) {
                                th.$message({ message: '操作成功', type: 'success' });
                            } else {
                                console.error(req.data.exception);
                                throw req.config.way + ' ' + req.data.message;
                            }
                        }).catch(function (err) {
                            alert('错误信息：' + err, '操作失败', { confirmButtonText: '确定' });
                        }).finally(() => th.loading = false);
                    }
                });
            },
            //测试链接
            btnTest: function () {
                var th = this;
                var ak = th.form.AccessKey;
                var sk = th.form.SecretKey;
                var hubname = th.form.LiveSpace;
                $api.get('Live/Test', { 'ak': ak, 'sk': sk, 'hubname': hubname }).then(function (req) {
                    if (req.data.success) {
                        var result = req.data.result;
                        th.$alert('测试通过', '成功',
                            {
                                dangerouslyUseHTMLString: true,
                                confirmButtonText: '确定'
                            });
                    } else {
                        throw req.data.message;
                    }
                }).catch(function (err) {
                    th.$alert('错误信息：' + err, '失败',
                        {
                            dangerouslyUseHTMLString: true,
                            confirmButtonText: '确定'
                        });
                });
            }
        }
    });

});