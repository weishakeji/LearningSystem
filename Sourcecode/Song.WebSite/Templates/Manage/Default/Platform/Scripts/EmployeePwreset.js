
$ready(function () {

    var vue = new Vue({
        el: '#app',
        data: {
            id: $api.querystring('id'),
            form: {               
                newpw: '',
                newpw2: ''
            },
            account: {}, //当前登录账号对象
            rules: {               
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
            var th=this;
            $api.get('Admin/ForID', { 'id': th.id }).then(function (req) {
                if (req.data.success) {
                    var result = req.data.result;
                    th.account = result;                    

                } else {
                    throw '没有获取到信息';
                }
            }).catch(function (err) {
                alert(err);
            });
        },
        methods: {
            btnEnter: function (formName) {
                this.$refs[formName].validate((valid) => {
                    if (valid) {
                        $api.post('Admin/ResetPw', { 'accid': vue.id, 'pw': vue.form.newpw }).then(function (req) {
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
