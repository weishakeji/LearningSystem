var vm = new Vue({
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
    methods: {
        //保存信息
        btnEnter: function () {
            if (vm.loading) return;
            this.$refs['form'].validate(function (valid) {
                if (valid) {
                    $api.post("live/Setup", vm.form).then(function (req) {
                        var res = req.data;
                        if (res.success) {
                            vm.$message({ message: '操作成功', type: 'success' });
                        } else {
                            throw res.data.message;
                        }
                    }).catch(function (err) {
                        vm.$alert('错误信息：' + err, '操作失败', { confirmButtonText: '确定' });
                    });
                }
            });
        },
        //测试链接
        btnTest: function () {
            $api.get('Live/Test').then(function (req) {
                if (req.data.success) {
                    var result = req.data.result;
                    //...
                } else {
                    throw req.data.message;
                }
            }).catch(function (err) {
                var msg = "可能的原因：<br/>1、信息填写错误；<br/>2、七牛云账号欠费。";
                vm.$alert('错误信息：' + err + '<br/>' + msg, '操作失败',
                    {
                        dangerouslyUseHTMLString: true,
                        confirmButtonText: '确定'
                    });
            });
        }
    },
    created: function () {
        $api.get("live/GetSetup").then(function (req) {
            vm.form = req.data.result;
        });
    }
});
vm.$mount('#app-form');

$api.effect(function () {
    vm.loading = true;
}, function (response) {
    vm.loading = false;
});
