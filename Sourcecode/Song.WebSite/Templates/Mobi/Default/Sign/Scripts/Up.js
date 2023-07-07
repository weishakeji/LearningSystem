$ready(function () {

    window.vapp = new Vue({
        el: '#vapp',
        data: {
            platinfo: {},
            organ: {},
            config: {},      //当前机构配置项      
            referrer: {},            //推荐人
            //输入的信息
            form: {
                acc: '',
                pw: '',
                rec: '',
                recid: 0,
                vcode: '',
                vcodemd5: ''
            },
            password2: '',       //密码确认
            //账号的验证
            acc_rules: [
                { required: true, message: '*账号不得为空' },
                {
                    validator: function (v) {
                        return v.length >= 6;
                    }, message: '*账号长度需大于6个字符'
                },
                {
                    validator: function (v) {
                        return !/[\u4E00-\u9FA5]/i.test(v);
                    }, message: '*账号不允许用中文'
                },
                { pattern: /^[a-zA-z_]|[0-9]$/, message: '*仅限字母和数字' }

            ],
            pw_rules: [
                { required: true, message: '*密码不得为空' },
                {
                    validator: function (v) {
                        return v.length >= 6;
                    }, message: '*长度需大于等于6个字符'
                },
            ],
            pw2_rules: [
                {
                    validator: function (v) {
                        return v == vapp.form.pw;
                    }, message: '*密码不相同'
                }

            ],
            vcode_rules: [
                { required: true, message: '*校验码不得为空' },
                {
                    validator: function (v) {
                        return v.length == 4;
                    }, message: '*限4位数字'
                }
            ],
            //注册协议
            agreement: {
                text: '',        //协议内容
                show: false,     //是否显示协议
                checked: false       //是否选择“已经同意协议”
            },
            loading_submit: false,      //提交数据
            loading_init: true,
            loading_vcode: false     //校验码加载
        },
        mounted: function () {
            var th = this;
            $api.bat(
                $api.cache('Platform/PlatInfo:60'),
                $api.get('Organization/Current')
            ).then(axios.spread(function (platinfo, organ) {
                th.loading_init = false;
                //获取结果           
                th.platinfo = platinfo.data.result;
                th.organ = organ.data.result;
                //机构配置信息
                th.config = $api.organ(th.organ).config;
                //是否允许注册
                th.config['IsRegStudent'] = !!th.config['IsRegStudent'];

                $dom("#platname").text(th.organ.Org_PlatformName);
                //生成校证码
                th.getvcode();
            })).catch(function (err) {
                console.error(err);
            });
            this.getreferrer();
            this.getAgreement();
        },
        created: function () {

        },
        computed: {
            //是否有推荐人
            existref: function () {
                return JSON.stringify(this.referrer) != '{}' && this.referrer != null;
            }
        },
        watch: {
            'agreement.checked': {
                deep: true,
                handler: function (nv, ov) {
                    if (nv) this.agreement.show = false;
                    //console.log(nv.checked);
                }
            }
        },
        methods: {
            //查看注册协议
            showagreement: function () {
                if (this.agreement.text != '') {
                    this.agreement.show = true;
                    return;
                }
                this.getAgreement(this.agreement.show);
                //console.log('查看协言');
            },
            //获取推荐账号
            getreferrer: function () {
                if (window.sharekeyid == null || window.sharekeyid == '') return;
                var th = this;
                $api.get('Account/ForID', { 'id': window.sharekeyid }).then(function (req) {
                    if (req.data.success) {
                        th.referrer = req.data.result;
                        if (th.existref) {
                            th.form.rec = th.referrer.Ac_Name;
                            th.form.recid = th.referrer.Ac_ID;
                        }
                    } else {
                        console.error(req.data.exception);
                        throw req.config.way + ' ' + req.data.message;
                    }
                }).catch(err => console.error(err));
            },
            //获取校验吗
            getvcode: function () {
                var th = this;
                th.loading_vcode = true;
                //校验码生成
                $api.post('Helper/CheckCodeImg', { 'leng': 4, 'acc': th.organ.Org_PlatformName }).then(function (req) {
                    th.loading_vcode = false;
                    if (req.data.success) {
                        var result = req.data.result;
                        var vcodebase64 = result.base64;
                        th.form.vcodemd5 = result.value;
                        console.log(vapp.form.vcodemd5);
                        //
                        $dom("#vcode").attr('src', vcodebase64);
                    } else {
                        throw req.data.message;
                    }
                }).catch(function (err) {
                    alert(err);
                });
            },
            //获取注册协议的内容
            getAgreement: function (show) {
                var th = this;
                $api.get('Platform/RegisterAgreement').then(function (req) {
                    if (req.data.success) {
                        th.agreement.text = req.data.result;
                        if (show != null) show = true;
                    } else {
                        console.error(req.data.exception);
                        throw req.data.message;
                    }
                }).catch(function (err) {
                    alert(err);
                    console.error(err);
                });
            },
            //按钮提交事件
            btn_submit: function (values) {
                if (!this.agreement.checked) {
                    this.$dialog.alert({
                        message: '请阅读并同意《平台协议》',
                    });
                    return;
                }
                var th = this;
                th.loading_submit = true;
                $api.post('Account/Register', this.form).then(function (req) {
                    th.loading_submit = false;
                    if (req.data.success) {
                        var result = req.data.result;
                        if (result.Ac_IsPass) {
                            //直接注册通过，写入登录状态
                            $api.loginstatus('account', result.Ac_Pw, result.Ac_ID);
                            th.$dialog.alert({
                                message: '注册成功！',
                            }).then(() => {
                                th.goback(result);
                            });
                        } else {
                            $api.loginstatus('account', '');
                            //需要审核
                            th.$dialog.alert({
                                message: '注册成功，请等待审核！',
                            }).then(() => {
                                th.goback(result);
                            });
                        }
                        //...
                    } else {
                        throw req.data;
                    }
                }).catch(function (err) {
                    th.$dialog.alert({
                        message: err.message,
                    });
                    console.error(err);
                });
            },
            //注册成功后的跳转
            goback: function (account) {
                var referrer = $api.querystring('referrer');
                if (referrer == null || referrer == '')
                    referrer = $api.storage('singin_referrer');
                //注册成功后，是否需要填写详情
                if (this.config && this.config.IsRegDetail === true) {
                    window.location.href = $api.url.set('detail', {
                        'referrer': referrer,
                        'acid': account.Ac_ID,
                        'uid':account.Ac_CheckUID
                    });
                    return;
                }
                referrer = decodeURIComponent(referrer);
                window.location.href = referrer != '' && referrer != null ? referrer : '/';
            }
        }
    });

});
