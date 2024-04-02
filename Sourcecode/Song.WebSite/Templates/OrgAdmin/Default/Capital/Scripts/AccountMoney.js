
$ready(function () {

    window.vue = new Vue({
        el: '#app',
        data: {
            loading: false,  //         
            entity: {}, //当前数据对象
            rules: {
                money: [
                    { required: true, message: '不得为空', trigger: 'blur' },
                    {
                        validator: function (rule, value, callback) {
                            var num = Number(value);
                            if (isNaN(num)) {
                                callback(new Error("请输入数字"));
                            } else {
                                if (num <= 0) {
                                    callback(new Error("数字必须大于零"));
                                } else {
                                    callback();
                                }
                            }
                        }, trigger: 'blur'
                    }
                ]
            },
            money: 0,
            form: {
                acid: $api.querystring('id'),
                money: 0,         //操作的资金数
                remark: '',
                type: '2'    //操作方法，1为扣除，2为充值
            }
        },
        created: function () {
            var th = this;
            th.loading = true;
            $api.get('Account/ForID', { 'id': this.form.acid }).then(function (req) {
                if (req.data.success) {
                    th.entity = req.data.result;
                } else {
                    throw req.data.message;
                }
            }).catch(function (err) {
                alert(err);
            }).finally(() => th.loading = false);
        },
        methods: {
            btnEnter: function (formName) {
                var th = this;
                this.$refs[formName].validate((valid) => {
                    if (valid) {
                        th.loading = true;
                        $api.post('Money/AddOrSubtract', th.form).then(function (req) {
                            if (req.data.success) {
                                var result = req.data.result;
                                th.$message({
                                    type: 'success',
                                    message: '操作成功!',
                                    center: true
                                });
                                window.setTimeout(function () {
                                    th.operateSuccess();
                                }, 600);

                            } else {
                                console.error(req.data.exception);
                                throw req.data.message;
                            }
                        }).catch(function (err) {
                            alert(err);
                            console.error(err);
                        }).finally(() => th.loading = false);
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
