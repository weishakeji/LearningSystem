
$ready(function () {

    var vue = new Vue({
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
            $api.post('Admin/General').then(function (req) {
                if (req.data.success) {
                    var result = req.data.result;
                    vue.account = result;
                } else {
                    throw '未登录，或登录状态已失效';
                }
            }).catch(function (err) {
                alert(err);
            });

        },
        methods: {
            btnEnter: function (formName) {
                var th = this;
                this.$refs[formName].validate((valid) => {
                    if (valid) {
                        th.loading = true;
                        $api.post('Admin/ChangeSafeQustion',
                            { 'ques': th.account.Acc_Qus, 'ans': th.account.Acc_Ans })
                            .then(function (req) {
                                th.loading = false;
                                if (req.data.success) {
                                    var result = req.data.result;
                                    vue.$message({
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
                                vue.$alert(err, '错误');
                            });
                    } else {
                        console.log('error submit!!');
                        return false;
                    }
                });
            }
        },
    });

});
