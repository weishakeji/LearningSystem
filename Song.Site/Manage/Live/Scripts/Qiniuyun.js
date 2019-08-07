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
            this.$refs['form'].validate((valid) => {
                if (valid) {
                    $api.post("live/Setup", vm.form).then(function (req) {
                        var res = req.data;
                        if (res.success) {                           
                            vm.$message({message:'操作成功', type: 'success'});
                        } 
                    });
                }
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
    var res = response.data;
    if (!res.success) {
        vm.$alert('错误信息：' + res.message, '操作失败', { confirmButtonText: '确定' });
    }
});
