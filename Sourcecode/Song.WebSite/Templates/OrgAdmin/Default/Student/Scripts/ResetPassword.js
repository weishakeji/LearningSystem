
$ready(function () {

    window.vapp = new Vue({
        el: '#vapp',
        data: {
            id: $api.querystring('id'),
            account: {}, //当前登录账号对象 
            organ: {},       //当前登录账号所在的机构
            //表单内容
            form: {
                pw1: '', pw2: ''
            },
            rules: {
                pw1: [
                    { required: true, message: '密码不得为空', trigger: 'blur' }
                ],
                pw2: [
                    {
                        validator: function (rule, value, callback) {
                            var pw1 = vapp.form.pw1;
                            if (pw1 != value) {
                                callback(new Error("两次输入的密码不相同"));
                            } else {
                                callback();
                            }
                        }, trigger: 'blur'
                    }
                ]
            },
            loading: false,
            fulloading: {}
        },
        created: function () {
            var th = this;
            th.loading = true;
            $api.get('Account/ForID', { 'id': th.id }).then(function (req) {
                if (req.data.success) {
                    var result = req.data.result;
                    th.account = result;
                    $api.get('Organization/ForID', { 'id': th.account.Org_ID }).then(function (req) {
                        if (req.data.success) {
                            th.organ = req.data.result;
                        } else {
                            console.error(req.data.exception);
                            throw req.data.message;
                        }
                    }).catch(function (err) {
                        alert(err);
                        console.error(err);
                    });
                } else {
                    console.error(req.data.exception);
                    throw req.data.message;
                }
            }).catch(function (err) {
                alert(err);
                console.error(err);
            }).finally(() => th.loading = false);

        },
        methods: {
            btnEnter: function (formName) {
                var th = this;
                this.$refs[formName].validate((valid) => {
                    if (valid) {
                        th.$confirm('是否继续?', '重置密码', {
                            confirmButtonText: '确定',
                            cancelButtonText: '取消',
                            type: 'warning'
                        }).then(() => {
                            if (th.loading) return;
                            th.loading = true;
                            th.fulloading = this.$fulloading();
                            var apipath = 'Account/ResetPassword';
                            $api.post(apipath, { 'acid': th.account.Ac_ID, 'pw': th.form.pw1 }).then(function (req) {
                                if (req.data.success) {
                                    var result = req.data.result;
                                    th.$message({
                                        type: 'success',
                                        message: '操作成功!',
                                        center: true
                                    });
                                    th.operateSuccess();
                                } else {
                                    throw req.data.message;
                                }
                            }).catch(function (err) {
                                alert(err, '错误');
                            }).finally(() => {
                                th.loading = false;
                                th.fulloading.close();
                            });
                        }).catch(() => { });
                    } else {
                        return false;
                    }
                });
            },
            //生成随机字符串
            btnRandom: function () {
                function getRandomString(length) {
                    const chars = 'ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789!@#$%^&*()_+';
                    let result = '';
                    for (let i = 0; i < length; i++) {
                        const randomIndex = Math.floor(Math.random() * chars.length);
                        result += chars[randomIndex];
                    }
                    return result;
                }
                let str = getRandomString(16);
                this.form.pw1 = this.form.pw2 = str;
            },
            //默认密码
            btnDefaultPw: function () {
                var th = this;
                $api.get('Account/defaultpw').then(req => {
                    if (req.data.success) {
                        th.form.pw1 = th.form.pw2 = req.data.result;
                    } else {
                        console.error(req.data.exception);
                        throw req.config.way + ' ' + req.data.message;
                    }
                }).catch(err => alert(err))
                    .finally(() => { });
            },
            //操作成功
            operateSuccess: function () {
                window.top.$pagebox.source.tab(window.name, 'vapp.handleCurrentChange', true);
            }
        },
    });

});
