$ready(function () {

    window.vapp = new Vue({
        el: '#vapp',
        data: {
            account: {}, //当前登录账号对象
            rules_ques: {
                Ac_Qus: [
                    { required: true, message: '请输入问题', trigger: 'blur' }
                ],
                Ac_Ans: [{ required: true, message: '请输入回答', trigger: 'blur' }]
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

            //修改安全问题
            btnEnterQues: function (formName) {
                var th = this;
                this.$refs[formName].validate((valid) => {
                    if (valid) {
                        th.loading = true;
                        var obj = th.remove_redundance(th.account, formName);
                        $api.post('Account/ModifyJson', { 'acc': obj })
                            .then(function (req) {
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
                                alert(err, '错误');
                            }).finally(() => th.loading = false);
                    } else {
                        console.log('error submit!!');
                        return false;
                    }
                });
            },
            //清理冗余的属性，仅保持当前form表单的属性，未在表单中的不提交到服务器
            remove_redundance: function (obj, form) {
                //表单中的字段
                var props = ['Ac_ID'];
                var fields = this.$refs[form].fields;
                for (var i = 0; i < fields.length; i++)
                    props.push(fields[i].prop);
                //obj的属性字段,如果表单上没有，则删除               
                for (let att in obj) {
                    var exist = false;
                    for (var i = 0; i < props.length; i++) {
                        if (att == props[i]) {
                            exist = true;
                            break;
                        }
                    }
                    if (!exist) delete obj[att];
                }
                return obj;
            }
        }
    });

});
