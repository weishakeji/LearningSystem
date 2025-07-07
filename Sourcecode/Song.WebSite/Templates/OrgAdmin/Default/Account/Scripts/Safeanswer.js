
$ready(function () {

    window.vapp = new Vue({
        el: '#vapp',
        data: {
            account: {}, //当前登录账号对象
            loading: false,
            rules: {
                Acc_Qus: [
                    { required: true, message: '请输入问题', trigger: 'blur' }
                ],
                Acc_Ans: [{ required: true, message: '请输入回答', trigger: 'blur' }]
            }
        },
        created: function () {
            var th = this;
            $api.post('Admin/General').then(function (req) {
                if (req.data.success) {
                    var result = req.data.result;
                    th.account = result;
                } else {
                    throw '未登录，或登录状态已失效';
                }
            }).catch(function (err) {
                alert(err);
            }).finally(() => { });

        },
        methods: {
            btnEnter: function (formName) {
                var th = this;
                this.$refs[formName].validate((valid) => {
                    if (valid) {
                        if (th.loading) return;
                        th.loading = true;
                        $api.post('Admin/ChangeSafeQustion',
                            { 'ques': th.account.Acc_Qus, 'ans': th.account.Acc_Ans })
                            .then(function (req) {
                                if (req.data.success) {
                                    var result = req.data.result;
                                    th.$message({
                                        type: 'success',
                                        message: '修改成功!'
                                    });
                                    window.setTimeout(function () {
                                        var name = $dom.trim(window.name);
                                        if (window.top.$pagebox)
                                            window.top.$pagebox.shut(name);
                                    }, 1000);
                                } else {
                                    throw req.data.message;
                                }
                            }).catch(function (err) {
                                alert(err, '错误');
                            }).finally(() => setTimeout(() => th.loading = false, 1000));
                    } else {
                        console.log('error submit!!');
                        return false;
                    }
                });
            }
        },
    });

});
