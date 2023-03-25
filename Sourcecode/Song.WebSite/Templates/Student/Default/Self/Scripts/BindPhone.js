$ready(function () {

    window.vapp = new Vue({
        el: '#vapp',
        data: {
            account: {}, //当前登录账号对象
            platinfo: {},
            organ: {},
            config: {},      //当前机构配置项

            form: {
                phone: '',
                sms: ''
            },
            sms_seconds: 0,
            sms_rules: {
                'phone': [
                    { required: true, message: '不得为空', trigger: 'blur' },
                    { pattern: /^[0-9_-]{11,11}$/, message: '请输入合法手机号', trigger: 'blur' }
                ],
                'sms': [
                    { required: true, message: '验证码不得为空', trigger: 'blur' },
                    { min: 6, max: 6, message: '仅限6位数字', trigger: 'blur' }
                ]
            },

            loading: true,  //
            loading_sms: false,
            uploading: false        //修改信息中
        },
        mounted: function () {
            var th = this;
            $api.bat(
                $api.get('Account/Current'),
                $api.cache('Platform/PlatInfo'),
                $api.get('Organization/Current')
            ).then(axios.spread(function (account, platinfo, organ) {
                th.loading = false;
                //判断结果是否正常
                for (var i = 0; i < arguments.length; i++) {
                    if (arguments[i].status != 200)
                        console.error(arguments[i]);
                    var data = arguments[i].data;
                    if (!data.success && data.exception != null) {
                        console.error(data.message);
                    }
                }
                //获取结果
                th.account = account.data.result;
                if (th.account) th.form.phone = th.account.Ac_MobiTel1;
                th.platinfo = platinfo.data.result;
                th.organ = organ.data.result;

            })).catch(function (err) {
                console.error(err);
            });
        },
        created: function () {

        },
        computed: {
            //是否登录
            islogin: function () {
                return JSON.stringify(this.account) != '{}' && this.account != null;
            },
            //是否绑定手机号了
            isbind: function () {
                return this.account.Ac_MobiTel1 != '' && this.account.Ac_MobiTel1 == this.account.Ac_MobiTel2;
            }
        },
        watch: {
        },
        methods: {
            //取消绑定
            phoneUnbind: function () {
                var th = this;
                this.$confirm('是否确定取消绑定？', '提示', {
                    confirmButtonText: '确定',
                    cancelButtonText: '取消',
                    type: 'warning'
                }).then(() => {
                    th.uploading = true;
                    $api.post('Account/PhoneUnbind', { 'acid': th.account.Ac_ID }).then(function (req) {
                        if (req.data.success) {
                            th.account = req.data.result;
                        } else {
                            console.error(req.data.exception);
                            throw req.config.way + ' ' + req.data.message;
                        }
                    }).catch(function (err) {
                        alert(err);
                        console.error(err);
                    }).finally(() => th.uploading = false);
                }).catch(() => { });

            },
            //获取短信
            getsms: function (formName) {
                var th = this;
                this.$refs[formName].validate((valid) => {
                    if (valid) {
                        var th = this;
                        th.loading_sms = true;
                        $api.post('Sms/SendBindVcode', { 'phone': th.form.phone, 'acid': th.account.Ac_ID, 'len': 6 }).then(function (req) {
                            if (req.data.success) {
                                var result = req.data.result;   //校验码
                                th.countdown(result);
                                window.login_sms_seconds = window.setInterval(function () {
                                    th.countdown();
                                }, 1000);

                            } else {
                                console.error(req.data.exception);
                                throw req.data.message;
                            }
                        }).catch(function (err) {
                            alert(err);
                            console.error(err);
                        }).finally(() => th.loading_sms = false);
                    } else {
                        console.log('error submit!!');
                        return false;
                    }
                });
            },
            //倒计时
            countdown: function (vcode) {
                let storage = 'bindphone_sms_seconds';
                var obj = $api.storage(storage);
                if (obj == undefined) obj = {};
                if (vcode != '' && vcode != undefined) {
                    obj = { 'second': 60, 'expire': new Date(new Date().getTime() + 1000 * 180), 'vcode': vcode };
                }
                else {
                    if (obj.second > 0) obj.second--;
                    else window.clearInterval(window.login_sms_seconds);
                }
                this.sms_seconds = obj.second;
                $api.storage(storage, obj);
                console.log(obj.second);
            },
            //绑定
            phonebind: function (formName) {
                var th = this;
                this.$refs[formName].validate((valid) => {
                    if (valid) {
                        th.uploading = true;
                        $api.post('Account/PhoneBind', { 'acid': th.account.Ac_ID, 'phone': this.form.phone }).then(function (req) {
                            if (req.data.success) {
                                th.account = req.data.result;
                            } else {
                                console.error(req.data.exception);
                                throw req.config.way + ' ' + req.data.message;
                            }
                        }).catch(function (err) {
                            alert(err);
                            console.error(err);
                        }).finally(() => th.uploading = false);
                    } else {
                        console.log('error submit!!');
                        return false;
                    }
                });
            },
        }
    });

}, ["/Utilities/Scripts/hanzi2pinyin.js",
    "/Utilities/Components/education.js"]);
