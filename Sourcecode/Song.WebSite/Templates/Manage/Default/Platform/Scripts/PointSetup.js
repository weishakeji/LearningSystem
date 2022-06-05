
$ready(function () {
    window.vapp = new Vue({
        el: '#vapp',
        data: {
            param: {},      //参数项
            rules: {
                LoginPoint: [
                    {
                        validator: function (rule, value, callback) {
                            var n = Number(value);
                            n = isNaN(n) ? 0 : n;
                            var max = Number(vapp.param.LoginPointMax);
                            if (n > max) {
                                callback(new Error("每次登录的积分不要大于每天最多积分"));
                            } else {
                                callback();
                            }
                        }, trigger: 'blur'
                    }
                ],
                SharePoint: [
                    {
                        validator: function (rule, value, callback) {
                            var n = Number(value);
                            n = isNaN(n) ? 0 : n;
                            var max = Number(vapp.param.SharePointMax);
                            if (n > max) {
                                callback(new Error("每次分享的积分不要大于每天最多积分"));
                            } else {
                                callback();
                            }
                        }, trigger: 'blur'
                    }
                ],
                RegPoint: [
                    {
                        validator: function (rule, value, callback) {
                            var n = Number(value);
                            n = isNaN(n) ? 0 : n;
                            var max = Number(vapp.param.RegPointMax);
                            if (n > max) {
                                callback(new Error("每次分享注册的积分不要大于每天最多积分"));
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
            th.loading = true;
            $api.get('Point/Param').then(function (req) {
                th.loading = false;
                if (req.data.success) {
                    th.param = req.data.result;
                    //...
                } else {
                    console.error(req.data.exception);
                    throw req.data.message;
                }
            }).catch(function (err) {
                alert(err);
                console.error(err);
            });
        },
        methods: {
            onSubmit: function (formName) {
                var th = this;
                this.$refs[formName].validate((valid) => {
                    if (valid) {
                        th.loading = true;
                        $api.get('Point/ParamSetup', th.param).then(function (req) {
                            th.loading = false;
                            if (req.data.success) {
                                var result = req.data.result;
                                //console.log(result);
                                th.$notify({
                                    title: '成功',
                                    message: '这是一条成功的提示消息',
                                    type: 'success'
                                });
                            } else {
                                console.error(req.data.exception);
                                throw req.data.message;
                            }
                        }).catch(function (err) {
                            //alert(err);
                            console.error(err);
                        });
                    } else {
                        console.log('error submit!!');
                        return false;
                    }
                });
            }
        }
    });
});
