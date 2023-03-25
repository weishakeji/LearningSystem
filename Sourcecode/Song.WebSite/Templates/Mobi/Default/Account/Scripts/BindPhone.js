$ready(function () {

    window.vapp = new Vue({
        el: '#vapp',
        data: {
            account: {},     //当前登录账号
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
                    { regex: /^[0-9_-]{11,11}$/, message: '请输入合法手机号', trigger: 'blur' }
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
                return account.Ac_MobiTel1!='' && this.account.Ac_MobiTel1 == this.account.Ac_MobiTel2;
            }
        },
        watch: {
            //提交信息中的状态变更
            'uploading': function (nv, ov) {
                if (nv) {
                    this.$toast.loading({
                        message: '信息提交中...',
                        forbidClick: true,
                    });
                }
            }
        },
        methods: {
            //取消绑定
            phoneUnbind: function () {
                var th = this;
                this.$dialog.confirm({
                    message: '是否确定取消绑定？',
                }).then(function () {
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
                }).catch(function () { });
            },
            //绑定
            phonebind: function () {
                if (!this.verification(this.form, this.sms_rules)) return;
                //校验验证码
                let storage = 'bindphone_sms_seconds';
                var obj = $api.storage(storage);
                if (obj == undefined) return this.tips('sms', false, '请发送验证码');
                if (obj.expire < new Date()) return this.tips('sms', false, '验证码时效过期');
                if (obj.vcode != $api.md5(this.form.phone + this.form.sms)) return this.tips('sms', false, '验证码不正确');
                var th = this;
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
            },
            //显示提示信息
            tips: function (prop, success, msg) {
                if (prop == undefined || prop == null) {
                    return $dom('*[prop]').removeClass('login_error');
                }
                var dom = $dom('*[prop="' + prop + '"]');
                if (dom.length > 0) {
                    if (success) dom.removeClass('login_error');
                    if (!success) {
                        dom.addClass('login_error');
                        dom.find('input').focus();
                    }
                    dom.find('.tips').html(msg);
                }
                return success;
            },
            //校验数据，data数据项,rules校验规则（基本遵循ElemenuUI相关校验规划）
            verification: function (data, rules) {
                for (d in data) {   //遍历数据项
                    if (!check(d, data[d], rules[d], this.tips)) return false;
                }
                //校验方法
                function check(prop, val, rule, func) {
                    if (rule == undefined) return true;
                    for (let i = 0; i < rule.length; i++) {
                        const item = rule[i];
                        //为空判断
                        if (!!item['required'] && val == '') {
                            return func(prop, false, item['message']);
                        }
                        //判断长度
                        if ((!!item['min'] && val.length < item['min'])
                            || (!!item['max'] && val.length > item['max'])) {
                            return func(prop, false, item['message']);
                        }
                        //正则验证
                        if (!!item['regex']) {
                            var regex = new RegExp(item['regex']);
                            if (!regex.test(val))
                                return func(prop, false, item['message']);
                        }
                    }
                    return func(prop, true, '');
                }
                return true;
            },
            //获取短信
            getsms: function () {
                if (!this.verification({ 'phone': this.form.phone }, this.sms_rules)) return;
                var th = this;
                th.loading_sms = true;
                $api.post('Sms/SendBindVcode', { 'phone': th.form.phone, 'acid':th.account.Ac_ID,'len': 6 }).then(function (req) {
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
                    th.tips('phone', false, err);
                    console.error(err);
                }).finally(() => th.loading_sms = false);
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
        },
    });

}, ['/Utilities/Components/avatar.js']);
