
$ready(function () {

    window.vapp = new Vue({
        el: '#vapp',
        data: {
            id: $api.querystring('id'),
            account: {}, //当前登录账号对象
            form: {
                pw1: '',
                pw2: ''
            },
            loading: false,
            rules: {
                pw1: [{ required: true, message: '请输入新密码', trigger: 'blur' }],
                pw2: [
                    { required: true, message: '请输入新密码', trigger: 'blur' },
                    {
                        validator: function (rule, value, callback) {
                            if (value !== vapp.form.pw1) {
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
            var th = this;
            th.loading = true;
            $api.get('Admin/ForID', { 'id': th.id }).then(function (req) {
                th.loading = false;
                if (req.data.success) {
                    var result = req.data.result;
                    th.account = result;
                } else {
                    console.error(req.data.exception);
                    throw req.data.message;
                }
            }).catch(function (err) {
                th.loading = false;
                th.$alert(err, '错误');
                console.error(err);
            });
        },
        methods: {
            btnEnter: function (formName) {
                var th = this;
                this.$refs[formName].validate((valid) => {
                    if (valid) {
                        th.loading = true;
                        $api.post('Admin/resetPw', {'accid':th.id,'pw':th.form.pw1}).then(function (req) {
                            th.loading = false;
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
                            th.$alert(err, '错误');
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
