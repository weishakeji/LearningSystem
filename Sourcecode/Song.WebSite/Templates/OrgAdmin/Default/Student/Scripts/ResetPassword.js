
$ready(function () {

    window.vue = new Vue({
        el: '#app',
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
                            var pw1 = vue.form.pw1;
                            if (pw1 != value) {
                                callback(new Error("两次输入的密码不相同"));
                            } else {
                                callback();
                            }
                        }, trigger: 'blur'
                    }
                ]
            },
            loading: false
        },
        created: function () {
            var th = this;
            $api.get('Account/ForID', { 'id': th.id }).then(function (req) {
                if (req.data.success) {
                    var result = req.data.result;
                    th.account = result;
                    $api.get('Organization/ForID', { 'id': th.account.Org_ID }).then(function (req) {
                        if (req.data.success) {
                            vue.organ = req.data.result;
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
            }).finally(()=>{});

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
                            th.loading = true;
                            var apipath = 'Account/ResetPassword';
                            $api.post(apipath, { 'acid': th.account.Ac_ID, 'pw': th.form.pw1 }).then(function (req) {
                                th.loading = false;
                                if (req.data.success) {
                                    var result = req.data.result;
                                    vue.$message({
                                        type: 'success',
                                        message: '操作成功!',
                                        center: true
                                    });
                                    window.setTimeout(function () {
                                        vue.operateSuccess();
                                    }, 600);
                                } else {
                                    throw req.data.message;
                                }
                            }).catch(function (err) {
                                vue.$alert(err, '错误');
                            });
                        }).catch(() => { });
                    } else {
                        return false;
                    }
                });
            },
            //操作成功
            operateSuccess: function () {
                window.top.$pagebox.source.tab(window.name, 'vapp.handleCurrentChange', true);
            }
        },
    });

});
