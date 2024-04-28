$ready(function () {

    window.vapp = new Vue({
        el: '#vapp',
        data: {
            account: {}, //当前登录账号对象
            loading: false,
            //修改密码
            formpw: {
                oldpw: '',
                newpw: '',
                newpw2: ''
            },
            rules_pw: {
                oldpw: [
                    { required: true, message: '请输入原密码', trigger: 'blur' }
                ],
                newpw: [{ required: true, message: '请输入新密码', trigger: 'blur' }],
                newpw2: [
                    { required: true, message: '请输入新密码', trigger: 'blur' },
                    {
                        validator: function (rule, value, callback) {
                            if (value !== vapp.formpw.newpw) {
                                callback(new Error('两次输入密码不一致!'));
                            } else {
                                callback();
                            }
                        }, trigger: 'blur'
                    }
                ]
            },

            loading: false,
            loading_init: true
        },
        mounted: function () {
            this.getAccount();
        },
        created: function () {

        },
        computed: {

        },
        watch: {
        },
        methods: {
            //获取当前登录账号
            getAccount: function () {
                var th = this;
                th.loading_init = true;
                $api.get('Account/Current').then(function (req) {
                    if (req.data.success) {
                        th.account = req.data.result;
                    } else {
                        console.error(req.data.exception);
                        throw req.data.message;
                    }
                }).catch(err => console.error(err))
                    .finally(() => th.loading_init = false);
            },
            btnEnter: function (formName) {
                var th = this;
                this.$refs[formName].validate((valid) => {
                    if (valid) {
                        th.loading = true;
                        $api.post('Account/ModifyPassword',
                            { 'oldpw': th.formpw.oldpw, 'newpw': th.formpw.newpw })
                            .then(function (req) {
                                if (req.data.success) {
                                    var result = req.data.result;
                                    th.getAccount();
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
                            }).finally(() => th.loading = false);
                    } else {
                        console.log('error submit!!');
                        return false;
                    }
                });
            }
        }
    });

}, ['/Utilities/Components/securitylevel.js']);
