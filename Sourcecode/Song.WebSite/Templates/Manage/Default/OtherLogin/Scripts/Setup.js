$ready(function () {
    window.vapp = new Vue({
        el: '#vapp',
        data: {
            tabs: [
                { name: 'QQ登录', tag: 'qq', icon: '&#xe82a', size: 16 },
                { name: '微信登录', tag: 'weixin', icon: '&#xe730', size: 18 },
                { name: '金碟.云之家', tag: 'yunzhijia', icon: '&#xe726', size: 18 }
            ],
            activeName: 'qq',      //选项卡

            qq: {},
            qq_rules: {
                APPID: [
                    { required: true, message: '不得为空', trigger: 'blur' }
                ],
                APPKey: [
                    { required: true, message: '不得为空', trigger: 'blur' }
                ],
                Returl: [
                    { required: true, message: '不得为空', trigger: 'blur' }
                ]
            },
            weixin: {},
            weixin_rules: {
                APPID: [
                    { required: true, message: '不得为空', trigger: 'blur' }
                ],
                Secret: [
                    { required: true, message: '不得为空', trigger: 'blur' }
                ],
                Returl: [
                    { required: true, message: '不得为空', trigger: 'blur' }
                ],
                pubAPPID: [
                    { required: true, message: '不得为空', trigger: 'blur' }
                ],
                pubSecret: [
                    { required: true, message: '不得为空', trigger: 'blur' }
                ],
                pubReturl: [
                    { required: true, message: '不得为空', trigger: 'blur' }
                ]
            },
            yunzhijia: {},
            yunzhijia_rules: {
                Appid: [
                    { required: true, message: '不得为空', trigger: 'blur' }
                ],
                AppSecret: [
                    { required: true, message: '不得为空', trigger: 'blur' }
                ],
                Domain: [
                    { required: true, message: '不得为空', trigger: 'blur' }
                ],
                Acc: [
                    { required: true, message: '不得为空', trigger: 'blur' }
                ]
            },
            loading: false,

        },
        watch: {
        },
        created: function () {
            var th = this;
            th.loading = true;
            $api.bat(
                $api.get('OtherLogin/QQ'),
                $api.get('OtherLogin/Weixin'),
                $api.get('OtherLogin/Yunzhijia')
            ).then(axios.spread(function (qq, weixin, yunzhijia) {
                th.loading = false;
                //判断结果是否正常
                for (var i = 0; i < arguments.length; i++) {
                    if (arguments[i].status != 200)
                        console.error(arguments[i]);
                    var data = arguments[i].data;
                    if (!data.success && data.exception != null) {
                        console.error(data.exception);
                        throw arguments[i].config.way + ' ' + data.message;
                    }
                }
                //获取结果
                th.qq = qq.data.result;
                th.weixin = weixin.data.result;
                th.yunzhijia = yunzhijia.data.result;
            })).catch(function (err) {
                th.loading = false;
                Vue.prototype.$alert(err);
                console.error(err);
            });
        },
        methods: {
            //保存信息
            btnEnter: function (formName, entity) {
                var th = this;
                this.$refs[formName].validate((valid) => {
                    if (valid) {
                        th.loading = true;
                        var apipath = "OtherLogin/" + formName + "Update"
                        $api.post(apipath, { 'data': entity }).then(function (req) {
                            th.loading = false;
                            if (req.data.success) {
                                var result = req.data.result;
                                th.$message({
                                    type: 'success',
                                    message: '操作成功!',
                                    center: true
                                });
                            } else {
                                throw req.data.message;
                            }
                        }).catch(function (err) {
                            th.loading = false;
                            Vue.prototype.$alert(err);
                        });
                    } else {
                        console.log('error submit!!');
                        return false;
                    }
                });
            },

        }
    });
    //window.vapp.$mount('#vapp');

});