﻿
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
            var th = this;
            $api.post('Admin/Super').then(function (req) {
                if (req.data.success) {
                    var result = req.data.result;
                    th.account = result;
                } else {
                    throw '未登录，或登录状态已失效';
                }
            }).catch(function (err) {
                // alert(err);
                th.account = null;
            });

        },
        methods: {
            btnEnter: function (formName) {
                this.$refs[formName].validate((valid) => {
                    if (valid) {
                        var th = this;
                        $api.post('Admin/ChangePw', { 'oldpw': th.form.oldpw, 'newpw': th.form.newpw }).then(function (req) {
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
                                }, 3000);
                            } else {
                                throw req.data.message;
                            }
                        }).catch(function (err) {
                            alert(err, '错误');
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
