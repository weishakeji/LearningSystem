
$ready(function () {

    window.vue = new Vue({
        el: '#app',
        data: {
            id: $api.querystring('id'),
            account: {}, //当前登录账号对象        
            //操作类型
            operates: [
                { 'label': '充值', 'type': 2 },
                { 'label': '扣除', 'type': 1 }],
            operated: '2',
            form: {
                acid: 0,
                coupon: '',
                remark: ''
            },
            rules: {
                coupon: [
                    { required: true, message: '不得为空', trigger: 'blur' },
                    {
                        validator: function (rule, value, callback) {
                            var n = Number(value);
                            n = isNaN(n) ? 0 : n;
                            if (n <= 0) {
                                callback(new Error("不得小于等于零"));
                            } else {
                                callback();
                            }
                        }, trigger: 'blur'
                    }
                ]
            },
            loading: true
        },
        watch: {
            operated: function (nv, ov) {
                console.log(nv);
            }
        },
        created: function () {
            var th = this;
            $api.get('Account/ForID', { 'id': th.id }).then(function (req) {
                if (req.data.success) {
                    var result = req.data.result;
                    th.account = result;
                    th.form.acid = result.Ac_ID;
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
                        th.$confirm('增减学员卡券, 是否继续?', '提示', {
                            confirmButtonText: '确定',
                            cancelButtonText: '取消',
                            type: 'warning'
                        }).then(() => {
                            th.loading = true;
                            var apipath = 'Coupon/';
                            if (Number(th.operated) == 2) apipath += 'Raise';
                            if (Number(th.operated) == 1) apipath += 'Subtract';
                            $api.post(apipath, th.form).then(function (req) {
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
                        console.log('error submit!!');
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
