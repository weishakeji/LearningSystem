
$ready(function () {

    var vue = new Vue({
        el: '#vapp',
        data: {
            form: {
                oldpw: '',
                newpw: '',
                newpw2: ''
            },
            account: {}, //当前登录账号对象
            loading: false,
            rules: {
                oldpw: [
                    { required: true, message: '请输入原密码', trigger: 'blur' }
                ],
                newpw: [{ required: true, message: '请输入新密码', trigger: 'blur' }],
                newpw2: [
                    { required: true, message: '请输入新密码', trigger: 'blur' },
                    {
                        validator: function (rule, value, callback) {
                            if (value !== vue.form.newpw) {
                                callback(new Error('两次输入密码不一致!'));
                            } else {
                                callback();
                            }
                        }, trigger: 'blur'
                    }
                ]
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
                        $api.post('Admin/ChangePw', { 'oldpw': vue.form.oldpw, 'newpw': vue.form.newpw }).then(function (req) {
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
                                }, 3000);
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
